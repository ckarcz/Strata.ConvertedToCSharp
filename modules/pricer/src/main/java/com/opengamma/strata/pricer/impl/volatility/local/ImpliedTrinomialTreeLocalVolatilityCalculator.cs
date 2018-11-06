using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.local
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.TIME_SQUARE;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using RecombiningTrinomialTreeData = com.opengamma.strata.pricer.fxopt.RecombiningTrinomialTreeData;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using BlackScholesFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackScholesFormulaRepository;

	/// <summary>
	/// Local volatility calculation based on trinomila tree model.
	/// <para>
	/// Emanuel Derman, Iraj Kani and Neil Chriss, "Implied Trinomial Trees of the Volatility Smile" (1996).
	/// </para>
	/// </summary>
	public class ImpliedTrinomialTreeLocalVolatilityCalculator : LocalVolatilityCalculator
	{

	  /// <summary>
	  /// The default interpolator.
	  /// </summary>
	  private static readonly GridSurfaceInterpolator DEFAULT_INTERPOLATOR = GridSurfaceInterpolator.of(TIME_SQUARE, LINEAR);

	  /// <summary>
	  /// The number of steps in trinomial tree.
	  /// </summary>
	  private readonly int nSteps;
	  /// <summary>
	  /// The maximum value of time in trinomial tree.
	  /// <para>
	  /// The time step in the tree is then given by {@code maxTime/nSteps}. 
	  /// </para>
	  /// </summary>
	  private readonly double maxTime;
	  /// <summary>
	  /// The interpolator for local volatilities.
	  /// <para>
	  /// The resulting local volatilities are interpolated by this interpolator along time and spot dimensions.
	  /// </para>
	  /// </summary>
	  private readonly SurfaceInterpolator interpolator;

	  /// <summary>
	  /// Creates an instance with default setups.
	  /// <para>
	  /// The number of time steps is 20, and the tree covers up to 3 years.
	  /// The time square linear interpolator is used for time direction, 
	  /// whereas the linear interpolator is used for spot dimension.
	  /// The extrapolation is flat for both the dimensions.
	  /// </para>
	  /// </summary>
	  public ImpliedTrinomialTreeLocalVolatilityCalculator() : this(20, 3d, DEFAULT_INTERPOLATOR)
	  {
	  }

	  /// <summary>
	  /// Creates an instance with the number of steps and maximum time fixed.
	  /// <para>
	  /// The default interpolators are used: the time square linear interpolator for time direction, 
	  /// the linear interpolator for spot dimension, and flat extrapolator for both the dimensions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nSteps">  the number of steps </param>
	  /// <param name="maxTime">  the maximum time </param>
	  public ImpliedTrinomialTreeLocalVolatilityCalculator(int nSteps, double maxTime) : this(nSteps, maxTime, DEFAULT_INTERPOLATOR)
	  {
	  }

	  /// <summary>
	  /// Creates an instance by specifying the number of steps, maximum time, and 2D interpolator.
	  /// </summary>
	  /// <param name="nSteps">  number of steps </param>
	  /// <param name="maxTime">  the maximum time </param>
	  /// <param name="interpolator">  the interpolator </param>
	  public ImpliedTrinomialTreeLocalVolatilityCalculator(int nSteps, double maxTime, SurfaceInterpolator interpolator)
	  {
		this.nSteps = nSteps;
		this.maxTime = maxTime;
		this.interpolator = interpolator;
	  }

	  //-------------------------------------------------------------------------
	  public virtual InterpolatedNodalSurface localVolatilityFromImpliedVolatility(Surface impliedVolatilitySurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate)
	  {

		System.Func<DoublesPair, double> surface = (DoublesPair tk) =>
		{
	return impliedVolatilitySurface.zValue(tk);
		};
		ImmutableList<double[]> localVolData = calibrate(surface, spot, interestRate, dividendRate).First;
		SurfaceMetadata metadata = DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.LOCAL_VOLATILITY).surfaceName(SurfaceName.of("localVol_" + impliedVolatilitySurface.Name)).build();
		return InterpolatedNodalSurface.ofUnsorted(metadata, DoubleArray.ofUnsafe(localVolData.get(0)), DoubleArray.ofUnsafe(localVolData.get(1)), DoubleArray.ofUnsafe(localVolData.get(2)), interpolator);
	  }

	  /// <summary>
	  /// Calibrate trinomial tree to implied volatility surface.
	  /// </summary>
	  /// <param name="impliedVolatilitySurface">  the implied volatility surface </param>
	  /// <param name="spot">  the spot </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="dividendRate">  the dividend rate </param>
	  /// <returns> the trinomial tree  </returns>
	  public virtual RecombiningTrinomialTreeData calibrateImpliedVolatility(System.Func<DoublesPair, double> impliedVolatilitySurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate)
	  {

		return calibrate(impliedVolatilitySurface, spot, interestRate, dividendRate).Second;
	  }

	  public virtual InterpolatedNodalSurface localVolatilityFromPrice(Surface callPriceSurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate)
	  {

		double[][] stateValue = new double[nSteps + 1][];
		double[] df = new double[nSteps];
		IList<DoubleMatrix> probability = new List<DoubleMatrix>(nSteps);
		int nTotal = (nSteps - 1) * (nSteps - 1) + 1;
		double[] timeRes = new double[nTotal];
		double[] spotRes = new double[nTotal];
		double[] volRes = new double[nTotal];
		// uniform grid based on TrigeorgisLatticeSpecification, using reference values
		double refPrice = callPriceSurface.zValue(maxTime, spot) * Math.Exp(interestRate(maxTime) * maxTime);
		double refForward = spot * Math.Exp((interestRate(maxTime) - dividendRate(maxTime)) * maxTime);
		double refVolatility = BlackFormulaRepository.impliedVolatility(refPrice, refForward, spot, maxTime, true);
		double dt = maxTime / nSteps;
		double dx = refVolatility * Math.Sqrt(3d * dt);
		double upFactor = Math.Exp(dx);
		double downFactor = Math.Exp(-dx);
		double[] adSec = new double[2 * nSteps + 1];
		double[] assetPrice = new double[2 * nSteps + 1];
		for (int i = nSteps; i > -1; --i)
		{
		  if (i == 0)
		  {
			resolveFirstLayer(interestRate, dividendRate, nTotal, dt, spot, adSec, assetPrice, timeRes, spotRes, volRes, df, stateValue, probability);
		  }
		  else
		  {
			double time = dt * i;
			double zeroRate = interestRate(time);
			double zeroDividendRate = dividendRate(time);
			int nNodes = 2 * i + 1;
			double[] assetPriceLocal = new double[nNodes];
			double[] callOptionPrice = new double[nNodes];
			double[] putOptionPrice = new double[nNodes];
			int position = i - 1;
			double assetTmp = spot * Math.Pow(upFactor, i);
			// call options for upper half nodes
			for (int j = nNodes - 1; j > position - 1; --j)
			{
			  assetPriceLocal[j] = assetTmp;
			  callOptionPrice[j] = callPriceSurface.zValue(time, assetPriceLocal[j]);
			  assetTmp *= downFactor;
			}
			// put options for lower half nodes
			assetTmp = spot * Math.Pow(downFactor, i);
			for (int j = 0; j < position + 2; ++j)
			{
			  assetPriceLocal[j] = assetTmp;
			  putOptionPrice[j] = callPriceSurface.zValue(time, assetPriceLocal[j]) - spot * Math.Exp(-zeroDividendRate * time) + Math.Exp(-zeroRate * time) * assetPriceLocal[j];
			  assetTmp *= upFactor;
			}
			resolveLayer(interestRate, dividendRate, i, nTotal, position, dt, zeroRate, zeroDividendRate, callOptionPrice, putOptionPrice, adSec, assetPrice, assetPriceLocal, timeRes, spotRes, volRes, df, stateValue, probability);
		  }
		}
		SurfaceMetadata metadata = DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.LOCAL_VOLATILITY).surfaceName(SurfaceName.of("localVol_" + callPriceSurface.Name)).build();
		return InterpolatedNodalSurface.ofUnsorted(metadata, DoubleArray.ofUnsafe(timeRes), DoubleArray.ofUnsafe(spotRes), DoubleArray.ofUnsafe(volRes), interpolator);
	  }

	  //-----------------------------------------------------------------------
	  private Pair<ImmutableList<double[]>, RecombiningTrinomialTreeData> calibrate(System.Func<DoublesPair, double> impliedVolatilitySurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate)
	  {

		double[][] stateValue = new double[nSteps + 1][];
		double[] df = new double[nSteps];
		double[] timePrim = new double[nSteps + 1];
		IList<DoubleMatrix> probability = new List<DoubleMatrix>(nSteps);
		int nTotal = (nSteps - 1) * (nSteps - 1) + 1;
		double[] timeRes = new double[nTotal];
		double[] spotRes = new double[nTotal];
		double[] volRes = new double[nTotal];
		// uniform grid based on TrigeorgisLatticeSpecification
		double volatility = impliedVolatilitySurface(DoublesPair.of(maxTime, spot));
		double dt = maxTime / nSteps;
		double dx = volatility * Math.Sqrt(3d * dt);
		double upFactor = Math.Exp(dx);
		double downFactor = Math.Exp(-dx);
		double[] adSec = new double[2 * nSteps + 1];
		double[] assetPrice = new double[2 * nSteps + 1];
		for (int i = nSteps; i > -1; --i)
		{
		  timePrim[i] = dt * i;
		  if (i == 0)
		  {
			resolveFirstLayer(interestRate, dividendRate, nTotal, dt, spot, adSec, assetPrice, timeRes, spotRes, volRes, df, stateValue, probability);
		  }
		  else
		  {
			double zeroRate = interestRate(timePrim[i]);
			double zeroDividendRate = dividendRate(timePrim[i]);
			double zeroCostRate = zeroRate - zeroDividendRate;
			int nNodes = 2 * i + 1;
			double[] assetPriceLocal = new double[nNodes];
			double[] callOptionPrice = new double[nNodes];
			double[] putOptionPrice = new double[nNodes];
			int position = i - 1;
			double assetTmp = spot * Math.Pow(upFactor, i);
			// call options for upper half nodes
			for (int j = nNodes - 1; j > position - 1; --j)
			{
			  assetPriceLocal[j] = assetTmp;
			  double impliedVol = impliedVolatilitySurface(DoublesPair.of(timePrim[i], assetPriceLocal[j]));
			  callOptionPrice[j] = BlackScholesFormulaRepository.price(spot, assetPriceLocal[j], timePrim[i], impliedVol, zeroRate, zeroCostRate, true);
			  assetTmp *= downFactor;
			}
			// put options for lower half nodes
			assetTmp = spot * Math.Pow(downFactor, i);
			for (int j = 0; j < position + 2; ++j)
			{
			  assetPriceLocal[j] = assetTmp;
			  double impliedVol = impliedVolatilitySurface(DoublesPair.of(timePrim[i], assetPriceLocal[j]));
			  putOptionPrice[j] = BlackScholesFormulaRepository.price(spot, assetPriceLocal[j], timePrim[i], impliedVol, zeroRate, zeroCostRate, false);
			  assetTmp *= upFactor;
			}
			resolveLayer(interestRate, dividendRate, i, nTotal, position, dt, zeroRate, zeroDividendRate, callOptionPrice, putOptionPrice, adSec, assetPrice, assetPriceLocal, timeRes, spotRes, volRes, df, stateValue, probability);
		  }
		}
		ImmutableList<double[]> localVolData = ImmutableList.of(timeRes, spotRes, volRes);
		RecombiningTrinomialTreeData treeData = RecombiningTrinomialTreeData.of(DoubleMatrix.ofUnsafe(stateValue), probability, DoubleArray.ofUnsafe(df), DoubleArray.ofUnsafe(timePrim));
		return Pair.of(localVolData, treeData);
	  }

	  // resolve the t=0 layer
	  private void resolveFirstLayer(System.Func<double, double> interestRate, System.Func<double, double> dividendRate, int nTotal, double dt, double spot, double[] adSec, double[] assetPrice, double[] timeRes, double[] spotRes, double[] volRes, double[] df, double[][] stateValue, IList<DoubleMatrix> probability)
	  {

		double discountFactor = Math.Exp(-interestRate(dt) * dt);
		double fwdFactor = Math.Exp((interestRate(dt) - dividendRate(dt)) * dt);
		double upProb = adSec[2] / discountFactor;
		double midProb = getMiddle(upProb, fwdFactor, spot, assetPrice[0], assetPrice[1], assetPrice[2]);
		double dwProb = 1d - upProb - midProb;
		double fwd = spot * fwdFactor;
		timeRes[nTotal - 1] = dt;
		spotRes[nTotal - 1] = spot;
		double var = (dwProb * Math.Pow(assetPrice[0] - fwd, 2) + midProb * Math.Pow(assetPrice[1] - fwd, 2) + upProb * Math.Pow(assetPrice[2] - fwd, 2)) / (fwd * fwd * dt);
		volRes[nTotal - 1] = Math.Sqrt(0.5 * (var + volRes[nTotal - 2] * volRes[nTotal - 2]));
		probability.Insert(0, DoubleMatrix.ofUnsafe(new double[][]
		{
			new double[] {dwProb, midProb, upProb}
		}));
		df[0] = discountFactor;
		stateValue[0] = new double[] {spot};
	  }

	  // resolve the i-th layer
	  private void resolveLayer(System.Func<double, double> interestRate, System.Func<double, double> dividendRate, int i, int nTotal, int position, double dt, double zeroRate, double zeroDividendRate, double[] callOptionPrice, double[] putOptionPrice, double[] adSec, double[] assetPrice, double[] assetPriceLocal, double[] timeRes, double[] spotRes, double[] volRes, double[] df, double[][] stateValue, IList<DoubleMatrix> probability)
	  {

		int positionLocal = position;
		int nNodes = callOptionPrice.Length;
		double[] adSecLocal = new double[nNodes];
		// AD security prices from call options
		for (int j = nNodes - 1; j > positionLocal; --j)
		{
		  adSecLocal[j] = callOptionPrice[j - 1];
		  for (int k = j + 1; k < nNodes; ++k)
		  {
			adSecLocal[j] -= (assetPriceLocal[k] - assetPriceLocal[j - 1]) * adSecLocal[k];
		  }
		  adSecLocal[j] /= (assetPriceLocal[j] - assetPriceLocal[j - 1]);
		}
		++positionLocal;
		// AD security prices from put options
		for (int j = 0; j < positionLocal; ++j)
		{
		  adSecLocal[j] = putOptionPrice[j + 1];
		  for (int k = 0; k < j; ++k)
		  {
			adSecLocal[j] -= (assetPriceLocal[j + 1] - assetPriceLocal[k]) * adSecLocal[k];
		  }
		  adSecLocal[j] /= (assetPriceLocal[j + 1] - assetPriceLocal[j]);
		}
		if (i != nSteps)
		{
		  double time = dt * i;
		  double timeNext = dt * (i - 1);
		  double rate = (zeroRate * time - interestRate(timeNext) * timeNext) / dt;
		  double dividend = (zeroDividendRate * time - dividendRate(timeNext) * timeNext) / dt;
		  double cost = rate - dividend;
		  double discountFactor = Math.Exp(-rate * dt);
		  double fwdFactor = Math.Exp(cost * dt);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] prob = new double[nNodes][3];
		  double[][] prob = RectangularArrays.ReturnRectangularDoubleArray(nNodes, 3);
		  // highest node
		  prob[nNodes - 1][2] = adSec[nNodes + 1] / adSecLocal[nNodes - 1] / discountFactor;
		  prob[nNodes - 1][1] = getMiddle(prob[nNodes - 1][2], fwdFactor, assetPriceLocal[nNodes - 1], assetPrice[nNodes - 1], assetPrice[nNodes], assetPrice[nNodes + 1]);
		  prob[nNodes - 1][0] = 1d - prob[nNodes - 1][2] - prob[nNodes - 1][1];
		  correctProbability(prob[nNodes - 1], fwdFactor, assetPriceLocal[nNodes - 1], assetPrice[nNodes - 1], assetPrice[nNodes], assetPrice[nNodes + 1]);
		  // second highest node
		  prob[nNodes - 2][2] = (adSec[nNodes] / discountFactor - prob[nNodes - 1][1] * adSecLocal[nNodes - 1]) / adSecLocal[nNodes - 2];
		  prob[nNodes - 2][1] = getMiddle(prob[nNodes - 2][2], fwdFactor, assetPriceLocal[nNodes - 2], assetPrice[nNodes - 2], assetPrice[nNodes - 1], assetPrice[nNodes]);
		  prob[nNodes - 2][0] = 1d - prob[nNodes - 2][2] - prob[nNodes - 2][1];
		  correctProbability(prob[nNodes - 2], fwdFactor, assetPriceLocal[nNodes - 2], assetPrice[nNodes - 2], assetPrice[nNodes - 1], assetPrice[nNodes]);
		  // subsequent nodes 
		  for (int j = nNodes - 3; j > -1; --j)
		  {
			prob[j][2] = (adSec[j + 2] / discountFactor - prob[j + 2][0] * adSecLocal[j + 2] - prob[j + 1][1] * adSecLocal[j + 1]) / adSecLocal[j];
			prob[j][1] = getMiddle(prob[j][2], fwdFactor, assetPriceLocal[j], assetPrice[j], assetPrice[j + 1], assetPrice[j + 2]);
			prob[j][0] = 1d - prob[j][1] - prob[j][2];
			correctProbability(prob[j], fwdFactor, assetPriceLocal[j], assetPrice[j], assetPrice[j + 1], assetPrice[j + 2]);
		  }
		  // local variance
		  int offset = nTotal - i * i - 1;
		  double[] varBare = new double[nNodes];
		  for (int k = 0; k < nNodes; ++k)
		  {
			double fwd = assetPriceLocal[k] * fwdFactor;
			varBare[k] = (prob[k][0] * Math.Pow(assetPrice[k] - fwd, 2) + prob[k][1] * Math.Pow(assetPrice[k + 1] - fwd, 2) + prob[k][2] * Math.Pow(assetPrice[k + 2] - fwd, 2)) / (fwd * fwd * dt);
			if (varBare[k] < 0d)
			{
			  throw new System.ArgumentException("Negative variance");
			}
		  }
		  // smoothing
		  for (int k = 0; k < nNodes - 2; ++k)
		  {
			double var = (k == 0 || k == nNodes - 3) ? (varBare[k] + varBare[k + 1] + varBare[k + 2]) / 3d : (varBare[k - 1] + varBare[k] + varBare[k + 1] + varBare[k + 2] + varBare[k + 3]) / 5d;
			volRes[offset + k] = i == nSteps - 1 ? Math.Sqrt(var) : Math.Sqrt(0.5 * (var + volRes[offset - (2 * i - k)] * volRes[offset - (2 * i - k)]));
			timeRes[offset + k] = dt * (i + 1d);
			spotRes[offset + k] = assetPriceLocal[k + 1];
		  }
		  probability.Insert(0, DoubleMatrix.ofUnsafe(prob));
		  df[i] = discountFactor;
		}
		stateValue[i] = Arrays.copyOf(assetPriceLocal, nNodes);
		Array.Copy(adSecLocal, 0, adSec, 0, nNodes);
		Array.Copy(assetPriceLocal, 0, assetPrice, 0, nNodes);
	  }

	  private void correctProbability(double[] probability, double factor, double assetBase, double assertPriceLow, double assertPriceMid, double assetPriceHigh)
	  {
		if (!(probability[2] > 0d && probability[1] > 0d && probability[0] > 0d))
		{
		  double fwd = assetBase * factor;
		  if (fwd <= assertPriceMid && fwd > assertPriceLow)
		  {
			probability[0] = 0.5 * (fwd - assertPriceLow) / (assetPriceHigh - assertPriceLow);
			probability[2] = 0.5 * ((assetPriceHigh - fwd) / (assetPriceHigh - assertPriceLow) + (assertPriceMid - fwd) / (assertPriceMid - assertPriceLow));
		  }
		  else if (fwd < assetPriceHigh && fwd > assertPriceMid)
		  {
			probability[0] = 0.5 * ((fwd - assertPriceMid) / (assetPriceHigh - assertPriceLow) + (fwd - assertPriceLow) / (assetPriceHigh - assertPriceLow));
			probability[2] = 0.5 * (assetPriceHigh - fwd) / assetPriceHigh;
		  }
		  probability[1] = 1d - probability[0] - probability[2];
		}
	  }

	  private double getMiddle(double upProbability, double factor, double assetBase, double assetPrevDw, double assetPrevMd, double assetPrevUp)
	  {
		return (factor * assetBase - assetPrevDw - upProbability * (assetPrevUp - assetPrevDw)) / (assetPrevMd - assetPrevDw);
	  }
	}

}
using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate.model
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using RidderSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.RidderSingleRootFinder;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;

	/// <summary>
	/// Methods related to the Hull-White one factor (extended Vasicek) model with piecewise constant volatility.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class HullWhiteOneFactorPiecewiseConstantInterestRateModel implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class HullWhiteOneFactorPiecewiseConstantInterestRateModel : ImmutableBean
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly HullWhiteOneFactorPiecewiseConstantInterestRateModel DEFAULT = new HullWhiteOneFactorPiecewiseConstantInterestRateModel();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future convexity factor used in future pricing.
	  /// <para>
	  /// The factor is called gamma in the reference: 
	  /// Henrard, M. "The Irony in the derivatives discounting Part II: the crisis", Wilmott Journal, 2010, 2, 301-316
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="data">  the Hull-White model parameters </param>
	  /// <param name="t0">  the first expiry time </param>
	  /// <param name="t1">  the first reference time </param>
	  /// <param name="t2">  the second reference time </param>
	  /// <returns> the factor </returns>
	  public double futuresConvexityFactor(HullWhiteOneFactorPiecewiseConstantParameters data, double t0, double t1, double t2)
	  {

		double factor1 = Math.Exp(-data.MeanReversion * t1) - Math.Exp(-data.MeanReversion * t2);
		double numerator = 2 * data.MeanReversion * data.MeanReversion * data.MeanReversion;
		int indexT0 = 1; // Period in which the time t0 is; volatilityTime[i-1] <= t0 < volatilityTime[i];
		while (t0 > data.VolatilityTime.get(indexT0))
		{
		  indexT0++;
		}
		double[] s = new double[indexT0 + 1];
		Array.Copy(data.VolatilityTime.toArray(), 0, s, 0, indexT0);
		s[indexT0] = t0;
		double factor2 = 0.0;
		for (int loopperiod = 0; loopperiod < indexT0; loopperiod++)
		{
		  factor2 += data.Volatility.get(loopperiod) * data.Volatility.get(loopperiod) * (Math.Exp(data.MeanReversion * s[loopperiod + 1]) - Math.Exp(data.MeanReversion * s[loopperiod])) * (2 - Math.Exp(-data.MeanReversion * (t2 - s[loopperiod + 1])) - Math.Exp(-data.MeanReversion * (t2 - s[loopperiod])));
		}
		return Math.Exp(factor1 / numerator * factor2);
	  }

	  /// <summary>
	  /// Calculates the future convexity factor and its derivatives with respect to the model volatilities.
	  /// <para>
	  /// The factor is called gamma in the reference: 
	  /// Henrard, M. "The Irony in the derivatives discounting Part II: the crisis", Wilmott Journal, 2010, 2, 301-316
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="data">  the Hull-White model parameters </param>
	  /// <param name="t0">  the expiry time </param>
	  /// <param name="t1">  the first reference time </param>
	  /// <param name="t2">  the second reference time </param>
	  /// <returns> the factor and drivatives </returns>
	  public ValueDerivatives futuresConvexityFactorAdjoint(HullWhiteOneFactorPiecewiseConstantParameters data, double t0, double t1, double t2)
	  {

		double factor1 = Math.Exp(-data.MeanReversion * t1) - Math.Exp(-data.MeanReversion * t2);
		double numerator = 2 * data.MeanReversion * data.MeanReversion * data.MeanReversion;
		int indexT0 = 1; // Period in which the time t0 is; volatilityTime[i-1] <= t0 < volatilityTime[i];
		while (t0 > data.VolatilityTime.get(indexT0))
		{
		  indexT0++;
		}
		double[] s = new double[indexT0 + 1];
		Array.Copy(data.VolatilityTime.toArray(), 0, s, 0, indexT0);
		s[indexT0] = t0;
		double factor2 = 0.0;
		double[] factorExp = new double[indexT0];
		for (int loopperiod = 0; loopperiod < indexT0; loopperiod++)
		{
		  factorExp[loopperiod] = (Math.Exp(data.MeanReversion * s[loopperiod + 1]) - Math.Exp(data.MeanReversion * s[loopperiod])) * (2 - Math.Exp(-data.MeanReversion * (t2 - s[loopperiod + 1])) - Math.Exp(-data.MeanReversion * (t2 - s[loopperiod])));
		  factor2 += data.Volatility.get(loopperiod) * data.Volatility.get(loopperiod) * factorExp[loopperiod];
		}
		double factor = Math.Exp(factor1 / numerator * factor2);
		// Backward sweep 
		double factorBar = 1.0;
		double factor2Bar = factor1 / numerator * factor * factorBar;
		double[] derivatives = new double[data.Volatility.size()];
		for (int loopperiod = 0; loopperiod < indexT0; loopperiod++)
		{
		  derivatives[loopperiod] = 2 * data.Volatility.get(loopperiod) * factorExp[loopperiod] * factor2Bar;
		}
		return ValueDerivatives.of(factor, DoubleArray.ofUnsafe(derivatives));
	  }

	  /// <summary>
	  /// Calculates the payment delay convexity factor used in coupons with mismatched dates pricing.
	  /// </summary>
	  /// <param name="parameters">  the Hull-White model parameters </param>
	  /// <param name="startExpiry">  the start expiry time </param>
	  /// <param name="endExpiry">  the end expiry time </param>
	  /// <param name="u">  the fixing period start time </param>
	  /// <param name="v">  the fixing period end time </param>
	  /// <param name="tp">  the payment time </param>
	  /// <returns> the factor </returns>
	  public double paymentDelayConvexityFactor(HullWhiteOneFactorPiecewiseConstantParameters parameters, double startExpiry, double endExpiry, double u, double v, double tp)
	  {

		double a = parameters.MeanReversion;
		double factor1 = (Math.Exp(-a * v) - Math.Exp(-a * tp)) * (Math.Exp(-a * v) - Math.Exp(-a * u));
		double numerator = 2 * a * a * a;
		int indexStart = Math.Abs(Arrays.binarySearch(parameters.VolatilityTime.toArray(), startExpiry) + 1);
		// Period in which the time startExpiry is; volatilityTime.get(i-1) <= startExpiry < volatilityTime.get(i);
		int indexEnd = Math.Abs(Arrays.binarySearch(parameters.VolatilityTime.toArray(), endExpiry) + 1);
		// Period in which the time endExpiry is; volatilityTime.get(i-1) <= endExpiry < volatilityTime.get(i);
		int sLen = indexEnd - indexStart + 1;
		double[] s = new double[sLen + 1];
		s[0] = startExpiry;
		Array.Copy(parameters.VolatilityTime.toArray(), indexStart, s, 1, sLen - 1);
		s[sLen] = endExpiry;
		double factor2 = 0.0;
		double[] exp2as = new double[sLen + 1];
		for (int loopperiod = 0; loopperiod < sLen + 1; loopperiod++)
		{
		  exp2as[loopperiod] = Math.Exp(2 * a * s[loopperiod]);
		}
		for (int loopperiod = 0; loopperiod < sLen; loopperiod++)
		{
		  factor2 += parameters.Volatility.get(loopperiod + indexStart - 1) * parameters.Volatility.get(loopperiod + indexStart - 1) * (exp2as[loopperiod + 1] - exp2as[loopperiod]);
		}
		return Math.Exp(factor1 * factor2 / numerator);
	  }

	  /// <summary>
	  /// Calculates the (zero-coupon) bond volatility divided by a bond numeraire, i.e., alpha, for a given period.
	  /// </summary>
	  /// <param name="data">  the Hull-White model data </param>
	  /// <param name="startExpiry"> the start time of the expiry period </param>
	  /// <param name="endExpiry">  the end time of the expiry period </param>
	  /// <param name="numeraireTime">  the time to maturity for the bond numeraire </param>
	  /// <param name="bondMaturity"> the time to maturity for the bond </param>
	  /// <returns> the re-based bond volatility </returns>
	  public double alpha(HullWhiteOneFactorPiecewiseConstantParameters data, double startExpiry, double endExpiry, double numeraireTime, double bondMaturity)
	  {

		double factor1 = Math.Exp(-data.MeanReversion * numeraireTime) - Math.Exp(-data.MeanReversion * bondMaturity);
		double numerator = 2 * data.MeanReversion * data.MeanReversion * data.MeanReversion;
		int indexStart = Math.Abs(Arrays.binarySearch(data.VolatilityTime.toArray(), startExpiry) + 1);
		// Period in which the time startExpiry is; volatilityTime.get(i-1) <= startExpiry < volatilityTime.get(i);
		int indexEnd = Math.Abs(Arrays.binarySearch(data.VolatilityTime.toArray(), endExpiry) + 1);
		// Period in which the time endExpiry is; volatilityTime.get(i-1) <= endExpiry < volatilityTime.get(i);
		int sLen = indexEnd - indexStart + 1;
		double[] s = new double[sLen + 1];
		s[0] = startExpiry;
		Array.Copy(data.VolatilityTime.toArray(), indexStart, s, 1, sLen - 1);
		s[sLen] = endExpiry;
		double factor2 = 0d;
		double[] exp2as = new double[sLen + 1];
		for (int loopperiod = 0; loopperiod < sLen + 1; loopperiod++)
		{
		  exp2as[loopperiod] = Math.Exp(2 * data.MeanReversion * s[loopperiod]);
		}
		for (int loopperiod = 0; loopperiod < sLen; loopperiod++)
		{
		  factor2 += data.Volatility.get(loopperiod + indexStart - 1) * data.Volatility.get(loopperiod + indexStart - 1) * (exp2as[loopperiod + 1] - exp2as[loopperiod]);
		}
		return factor1 * Math.Sqrt(factor2 / numerator);
	  }

	  /// <summary>
	  /// Calculates the (zero-coupon) bond volatility divided by a bond numeraire, i.e., alpha, for a given period and 
	  /// its derivatives.
	  /// <para>
	  /// The derivative values are the derivatives of the function alpha with respect to the piecewise constant volatilities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="data">  the Hull-White model data </param>
	  /// <param name="startExpiry">  the start time of the expiry period </param>
	  /// <param name="endExpiry">  the end time of the expiry period </param>
	  /// <param name="numeraireTime">  the time to maturity for the bond numeraire </param>
	  /// <param name="bondMaturity">  the time to maturity for the bond </param>
	  /// <returns> The re-based bond volatility </returns>
	  public ValueDerivatives alphaAdjoint(HullWhiteOneFactorPiecewiseConstantParameters data, double startExpiry, double endExpiry, double numeraireTime, double bondMaturity)
	  {

		// Forward sweep
		double factor1 = Math.Exp(-data.MeanReversion * numeraireTime) - Math.Exp(-data.MeanReversion * bondMaturity);
		double numerator = 2 * data.MeanReversion * data.MeanReversion * data.MeanReversion;
		int indexStart = Math.Abs(Arrays.binarySearch(data.VolatilityTime.toArray(), startExpiry) + 1);
		// Period in which the time startExpiry is; volatilityTime.get(i-1) <= startExpiry < volatilityTime.get(i);
		int indexEnd = Math.Abs(Arrays.binarySearch(data.VolatilityTime.toArray(), endExpiry) + 1);
		// Period in which the time endExpiry is; volatilityTime.get(i-1) <= endExpiry < volatilityTime.get(i);
		int sLen = indexEnd - indexStart + 1;
		double[] s = new double[sLen + 1];
		s[0] = startExpiry;
		Array.Copy(data.VolatilityTime.toArray(), indexStart, s, 1, sLen - 1);
		s[sLen] = endExpiry;
		double factor2 = 0.0;
		double[] exp2as = new double[sLen + 1];
		for (int loopperiod = 0; loopperiod < sLen + 1; loopperiod++)
		{
		  exp2as[loopperiod] = Math.Exp(2 * data.MeanReversion * s[loopperiod]);
		}
		for (int loopperiod = 0; loopperiod < sLen; loopperiod++)
		{
		  factor2 += data.Volatility.get(loopperiod + indexStart - 1) * data.Volatility.get(loopperiod + indexStart - 1) * (exp2as[loopperiod + 1] - exp2as[loopperiod]);
		}
		double sqrtFactor2Num = Math.Sqrt(factor2 / numerator);
		double alpha = factor1 * sqrtFactor2Num;
		// Backward sweep 
		double alphaBar = 1.0;
		double factor2Bar = factor1 / sqrtFactor2Num / 2.0 / numerator * alphaBar;
		double[] derivatives = new double[data.Volatility.size()];
		for (int loopperiod = 0; loopperiod < sLen; loopperiod++)
		{
		  derivatives[loopperiod + indexStart - 1] = 2 * data.Volatility.get(loopperiod + indexStart - 1) * (exp2as[loopperiod + 1] - exp2as[loopperiod]) * factor2Bar;
		}
		return ValueDerivatives.of(alpha, DoubleArray.ofUnsafe(derivatives));
	  }

	  /// <summary>
	  /// Calculates the exercise boundary for swaptions.
	  /// <para>
	  /// Reference: Henrard, M. (2003). "Explicit bond option and swaption formula in Heath-Jarrow-Morton one-factor model". 
	  /// International Journal of Theoretical and Applied Finance, 6(1):57--72.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountedCashFlow">  the cash flow equivalent discounted to today </param>
	  /// <param name="alpha">  the zero-coupon bond volatilities </param>
	  /// <returns> the exercise boundary </returns>
	  public double kappa(DoubleArray discountedCashFlow, DoubleArray alpha)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> swapValue = new java.util.function.Function<double, double>()
		System.Func<double, double> swapValue = (double? x) =>
		{
	double error = 0.0;
	for (int loopcf = 0; loopcf < alpha.size(); loopcf++)
	{
	  error += discountedCashFlow.get(loopcf) * Math.Exp(-0.5 * alpha.get(loopcf) * alpha.get(loopcf) - (alpha.get(loopcf) - alpha.get(0)) * x);
	}
	return error;
		};
		BracketRoot bracketer = new BracketRoot();
		double accuracy = 1.0E-8;
		RidderSingleRootFinder rootFinder = new RidderSingleRootFinder(accuracy);
		double[] range = bracketer.getBracketedPoints(swapValue, -2.0, 2.0);
		return rootFinder.getRoot(swapValue, range[0], range[1]).Value;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the beta parameter.
	  /// <para>
	  /// This is intended to be used in particular for Bermudan swaption first step of the pricing.
	  /// </para>
	  /// <para>
	  /// Reference: Henrard, "M. Bermudan Swaptions in Gaussian HJM One-Factor Model: Analytical and Numerical Approaches". 
	  /// SSRN, October 2008. Available at SSRN: http://ssrn.com/abstract=1287982
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="data">  the Hull-White model data </param>
	  /// <param name="startExpiry"> the start time of the expiry period </param>
	  /// <param name="endExpiry">  the end time of the expiry period </param>
	  /// <returns> the re-based bond volatility </returns>
	  public double beta(HullWhiteOneFactorPiecewiseConstantParameters data, double startExpiry, double endExpiry)
	  {
		double numerator = 2 * data.MeanReversion;
		int indexStart = 1; // Period in which the time startExpiry is; volatilityTime.get(i-1) <= startExpiry < volatilityTime.get(i);
		while (startExpiry > data.VolatilityTime.get(indexStart))
		{
		  indexStart++;
		}
		int indexEnd = indexStart; // Period in which the time endExpiry is; volatilityTime.get(i-1) <= endExpiry < volatilityTime.get(i);
		while (endExpiry > data.VolatilityTime.get(indexEnd))
		{
		  indexEnd++;
		}
		int sLen = indexEnd - indexStart + 1;
		double[] s = new double[sLen + 1];
		s[0] = startExpiry;
		Array.Copy(data.VolatilityTime.toArray(), indexStart, s, 1, sLen - 1);
		s[sLen] = endExpiry;
		double denominator = 0.0;
		for (int loopperiod = 0; loopperiod < sLen; loopperiod++)
		{
		  denominator += data.Volatility.get(loopperiod + indexStart - 1) * data.Volatility.get(loopperiod + indexStart - 1) * (Math.Exp(2 * data.MeanReversion * s[loopperiod + 1]) - Math.Exp(2 * data.MeanReversion * s[loopperiod]));
		}
		return Math.Sqrt(denominator / numerator);
	  }

	  /// <summary>
	  /// Calculates the common part of the exercise boundary of European swaptions forward.
	  /// <para>
	  /// This is intended to be used in particular for Bermudan swaption first step of the pricing.
	  /// </para>
	  /// <para>
	  /// Reference: Henrard, "M. Bermudan Swaptions in Gaussian HJM One-Factor Model: Analytical and Numerical Approaches". 
	  /// SSRN, October 2008. Available at SSRN: http://ssrn.com/abstract=1287982
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountedCashFlow">  the swap discounted cash flows </param>
	  /// <param name="alpha2">  square of the alpha parameter </param>
	  /// <param name="hwH">  the H factors </param>
	  /// <returns> the exercise boundary </returns>
	  public double lambda(DoubleArray discountedCashFlow, DoubleArray alpha2, DoubleArray hwH)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> swapValue = new java.util.function.Function<double, double>()
		System.Func<double, double> swapValue = (double? x) =>
		{
	double value = 0.0;
	for (int loopcf = 0; loopcf < alpha2.size(); loopcf++)
	{
	  value += discountedCashFlow.get(loopcf) * Math.Exp(-0.5 * alpha2.get(loopcf) - hwH.get(loopcf) * x);
	}
	return value;
		};
		BracketRoot bracketer = new BracketRoot();
		double accuracy = 1.0E-8;
		RidderSingleRootFinder rootFinder = new RidderSingleRootFinder(accuracy);
		double[] range = bracketer.getBracketedPoints(swapValue, -2.0, 2.0);
		return rootFinder.getRoot(swapValue, range[0], range[1]).Value;
	  }

	  /// <summary>
	  /// Calculates the maturity dependent part of the volatility (function called H in the implementation note).
	  /// </summary>
	  /// <param name="hwParameters">  the model parameters </param>
	  /// <param name="u">  the start time </param>
	  /// <param name="v">  the end time </param>
	  /// <returns> the volatility </returns>
	  public DoubleMatrix volatilityMaturityPart(HullWhiteOneFactorPiecewiseConstantParameters hwParameters, double u, DoubleMatrix v)
	  {

		double a = hwParameters.MeanReversion;
		double[][] result = new double[v.rowCount()][];
		double expau = Math.Exp(-a * u);
		for (int loopcf1 = 0; loopcf1 < v.rowCount(); loopcf1++)
		{
		  DoubleArray vRow = v.row(loopcf1);
		  result[loopcf1] = new double[vRow.size()];
		  for (int loopcf2 = 0; loopcf2 < vRow.size(); loopcf2++)
		  {
			result[loopcf1][loopcf2] = (expau - Math.Exp(-a * vRow.get(loopcf2))) / a;
		  }
		}
		return DoubleMatrix.copyOf(result);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the swap rate for a given value of the standard normal random variable
	  /// in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate </returns>
	  public double swapRate(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double numerator = 0.0;
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  numerator += discountedCashFlowIbor.get(loopcf) * Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		}
		double denominator = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  denominator += discountedCashFlowFixed.get(loopcf) * Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		}
		return -numerator / denominator;
	  }

	  /// <summary>
	  /// Calculates the first order derivative of the swap rate with respect to the value of the standard
	  /// normal random variable in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the first derivative of the swap rate </returns>
	  public double swapRateDx1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double f = 0.0;
		double df = 0.0;
		double term;
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  term = discountedCashFlowIbor.get(loopcf) * Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		  f += term;
		  df += -alphaIbor.get(loopcf) * term;
		}
		double g = 0.0;
		double dg = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  term = discountedCashFlowFixed.get(loopcf) * Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		  g += term;
		  dg += -alphaFixed.get(loopcf) * term;
		}
		return -(df * g - f * dg) / (g * g);
	  }

	  /// <summary>
	  /// Calculates the second order derivative of the swap rate with respect to the value
	  /// of the standard normal random variable in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the second derivative of the swap rate </returns>
	  public double swapRateDx2(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double f = 0.0;
		double df = 0.0;
		double df2 = 0.0;
		double term;
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  term = discountedCashFlowIbor.get(loopcf) * Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		  f += term;
		  df += -alphaIbor.get(loopcf) * term;
		  df2 += alphaIbor.get(loopcf) * alphaIbor.get(loopcf) * term;
		}
		double g = 0.0;
		double dg = 0.0;
		double dg2 = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  term = discountedCashFlowFixed.get(loopcf) * Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		  g += term;
		  dg += -alphaFixed.get(loopcf) * term;
		  dg2 += alphaFixed.get(loopcf) * alphaFixed.get(loopcf) * term;
		}
		double g2 = g * g;
		double g3 = g * g2;

		return -df2 / g + (2 * df * dg + f * dg2) / g2 - 2 * f * dg * dg / g3;
	  }

	  /// <summary>
	  /// Calculates the first order derivative of the swap rate with respect to
	  /// the {@code discountedCashFlowIbor} in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate and derivatives </returns>
	  public ValueDerivatives swapRateDdcfi1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double denominator = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  denominator += discountedCashFlowFixed.get(loopcf) * Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		}
		double numerator = 0.0;
		double[] swapRateDdcfi1 = new double[sizeIbor];
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  double exp = Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		  swapRateDdcfi1[loopcf] = -exp / denominator;
		  numerator += discountedCashFlowIbor.get(loopcf) * exp;
		}
		return ValueDerivatives.of(-numerator / denominator, DoubleArray.ofUnsafe(swapRateDdcfi1));
	  }

	  /// <summary>
	  /// Calculates the first order derivative of the swap rate with respect to the
	  /// {@code discountedCashFlowFixed} in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate and derivatives </returns>
	  public ValueDerivatives swapRateDdcff1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double[] expD = new double[sizeIbor];
		double numerator = 0.0;
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  numerator += discountedCashFlowIbor.get(loopcf) * Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		}
		double denominator = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  expD[loopcf] = Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		  denominator += discountedCashFlowFixed.get(loopcf) * expD[loopcf];
		}
		double ratio = numerator / (denominator * denominator);
		double[] swapRateDdcff1 = new double[sizeFixed];
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  swapRateDdcff1[loopcf] = ratio * expD[loopcf];
		}
		return ValueDerivatives.of(-numerator / denominator, DoubleArray.ofUnsafe(swapRateDdcff1));
	  }

	  /// <summary>
	  /// Calculates the first order derivative of the swap rate with respect to the {@code alphaIbor} 
	  /// in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate and derivatives </returns>
	  public ValueDerivatives swapRateDai1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double denominator = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  denominator += discountedCashFlowFixed.get(loopcf) * Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		}
		double numerator = 0.0;
		double[] swapRateDai1 = new double[sizeIbor];
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  double exp = Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		  swapRateDai1[loopcf] = discountedCashFlowIbor.get(loopcf) * exp * (x + alphaIbor.get(loopcf)) / denominator;
		  numerator += discountedCashFlowIbor.get(loopcf) * exp;
		}
		return ValueDerivatives.of(-numerator / denominator, DoubleArray.ofUnsafe(swapRateDai1));
	  }

	  /// <summary>
	  /// Calculates the first order derivative of the swap rate with respect to the {@code alphaFixed} 
	  /// in the {@code P(*,theta)} numeraire.
	  /// </summary>
	  /// <param name="x">  the random variable value. </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate and derivatives </returns>
	  public ValueDerivatives swapRateDaf1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double[] expD = new double[sizeIbor];
		double numerator = 0.0;
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  numerator += discountedCashFlowIbor.get(loopcf) * Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		}
		double denominator = 0.0;
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  expD[loopcf] = discountedCashFlowFixed.get(loopcf) * Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		  denominator += expD[loopcf];
		}
		double ratio = numerator / (denominator * denominator);
		double[] swapRateDaf1 = new double[sizeFixed];
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  swapRateDaf1[loopcf] = ratio * expD[loopcf] * (-x - alphaFixed.get(loopcf));
		}
		return ValueDerivatives.of(-numerator / denominator, DoubleArray.ofUnsafe(swapRateDaf1));
	  }

	  /// <summary>
	  /// Calculates the first order derivative with respect to the discountedCashFlowFixed and to the discountedCashFlowIbor 
	  /// of the of swap rate second derivative with respect to the random variable x in the {@code P(*,theta)} numeraire.
	  /// <para>
	  /// The result is made of a pair of arrays. The first one is the derivative with respect to {@code discountedCashFlowFixed} 
	  /// and the second one with respect to {@code discountedCashFlowIbor}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate derivatives </returns>
	  public Pair<DoubleArray, DoubleArray> swapRateDx2Ddcf1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double f = 0.0;
		double df = 0.0;
		double df2 = 0.0;
		double[] termIbor = new double[sizeIbor];
		double[] expIbor = new double[sizeIbor];
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  expIbor[loopcf] = Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		  termIbor[loopcf] = discountedCashFlowIbor.get(loopcf) * expIbor[loopcf];
		  f += termIbor[loopcf];
		  df += -alphaIbor.get(loopcf) * termIbor[loopcf];
		  df2 += alphaIbor.get(loopcf) * alphaIbor.get(loopcf) * termIbor[loopcf];
		}
		double g = 0.0;
		double dg = 0.0;
		double dg2 = 0.0;
		double[] termFixed = new double[sizeFixed];
		double[] expFixed = new double[sizeFixed];
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  expFixed[loopcf] = Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		  termFixed[loopcf] = discountedCashFlowFixed.get(loopcf) * expFixed[loopcf];
		  g += termFixed[loopcf];
		  dg += -alphaFixed.get(loopcf) * termFixed[loopcf];
		  dg2 += alphaFixed.get(loopcf) * alphaFixed.get(loopcf) * termFixed[loopcf];
		}
		double g2 = g * g;
		double g3 = g * g2;
		double g4 = g * g3;
		// Backward sweep
		double dx2Bar = 1d;
		double gBar = (df2 / g2 - 2d * f * dg2 / g3 - 4d * df * dg / g3 + 6d * dg * dg * f / g4) * dx2Bar;
		double dgBar = (2d * df / g2 - 4d * f * dg / g3) * dx2Bar;
		double dg2Bar = f / g2 * dx2Bar;
		double fBar = (dg2 / g2 - 2d * dg * dg / g3) * dx2Bar;
		double dfBar = 2d * dg / g2 * dx2Bar;
		double df2Bar = -dx2Bar / g;

		double[] discountedCashFlowFixedBar = new double[sizeFixed];
		double[] termFixedBar = new double[sizeFixed];
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  termFixedBar[loopcf] = gBar - alphaFixed.get(loopcf) * dgBar + alphaFixed.get(loopcf) * alphaFixed.get(loopcf) * dg2Bar;
		  discountedCashFlowFixedBar[loopcf] = expFixed[loopcf] * termFixedBar[loopcf];
		}
		double[] discountedCashFlowIborBar = new double[sizeIbor];
		double[] termIborBar = new double[sizeIbor];
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  termIborBar[loopcf] = fBar - alphaIbor.get(loopcf) * dfBar + alphaIbor.get(loopcf) * alphaIbor.get(loopcf) * df2Bar;
		  discountedCashFlowIborBar[loopcf] = expIbor[loopcf] * termIborBar[loopcf];
		}
		return Pair.of(DoubleArray.copyOf(discountedCashFlowFixedBar), DoubleArray.copyOf(discountedCashFlowIborBar));
	  }

	  /// <summary>
	  /// Calculates the first order derivative with respect to the alphaFixed and to the alphaIbor of
	  /// the of swap rate second derivative with respect to the random variable x in the
	  /// {@code P(*,theta)} numeraire.
	  /// <para>
	  /// The result is made of a pair of arrays. The first one is the derivative with respect to {@code alphaFixed} and 
	  /// the second one with respect to {@code alphaIbor}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the random variable value </param>
	  /// <param name="discountedCashFlowFixed">  the discounted cash flows equivalent of the swap fixed leg </param>
	  /// <param name="alphaFixed">  the zero-coupon bond volatilities for the swap fixed leg </param>
	  /// <param name="discountedCashFlowIbor">  the discounted cash flows equivalent of the swap Ibor leg </param>
	  /// <param name="alphaIbor">  the zero-coupon bond volatilities for the swap Ibor leg </param>
	  /// <returns> the swap rate derivatives </returns>
	  public Pair<DoubleArray, DoubleArray> swapRateDx2Da1(double x, DoubleArray discountedCashFlowFixed, DoubleArray alphaFixed, DoubleArray discountedCashFlowIbor, DoubleArray alphaIbor)
	  {

		int sizeIbor = discountedCashFlowIbor.size();
		int sizeFixed = discountedCashFlowFixed.size();
		ArgChecker.isTrue(sizeIbor == alphaIbor.size(), "Length should be equal");
		ArgChecker.isTrue(sizeFixed == alphaFixed.size(), "Length should be equal");
		double f = 0.0;
		double df = 0.0;
		double df2 = 0.0;
		double[] termIbor = new double[sizeIbor];
		double[] expIbor = new double[sizeIbor];
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  expIbor[loopcf] = Math.Exp(-alphaIbor.get(loopcf) * x - 0.5 * alphaIbor.get(loopcf) * alphaIbor.get(loopcf));
		  termIbor[loopcf] = discountedCashFlowIbor.get(loopcf) * expIbor[loopcf];
		  f += termIbor[loopcf];
		  df += -alphaIbor.get(loopcf) * termIbor[loopcf];
		  df2 += alphaIbor.get(loopcf) * alphaIbor.get(loopcf) * termIbor[loopcf];
		}
		double g = 0.0;
		double dg = 0.0;
		double dg2 = 0.0;
		double[] termFixed = new double[sizeFixed];
		double[] expFixed = new double[sizeFixed];
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  expFixed[loopcf] = Math.Exp(-alphaFixed.get(loopcf) * x - 0.5 * alphaFixed.get(loopcf) * alphaFixed.get(loopcf));
		  termFixed[loopcf] = discountedCashFlowFixed.get(loopcf) * expFixed[loopcf];
		  g += termFixed[loopcf];
		  dg += -alphaFixed.get(loopcf) * termFixed[loopcf];
		  dg2 += alphaFixed.get(loopcf) * alphaFixed.get(loopcf) * termFixed[loopcf];
		}
		double g2 = g * g;
		double g3 = g * g2;
		double g4 = g * g3;
		// Backward sweep
		double dx2Bar = 1d;
		double gBar = (df2 / g2 - 2d * f * dg2 / g3 - 4d * df * dg / g3 + 6d * dg * dg * f / g4) * dx2Bar;
		double dgBar = (2d * df / g2 - 4d * f * dg / g3) * dx2Bar;
		double dg2Bar = f / g2 * dx2Bar;
		double fBar = (dg2 / g2 - 2d * dg * dg / g3) * dx2Bar;
		double dfBar = 2d * dg / g2 * dx2Bar;
		double df2Bar = -dx2Bar / g;

		double[] alphaFixedBar = new double[sizeFixed];
		double[] termFixedBar = new double[sizeFixed];
		for (int loopcf = 0; loopcf < sizeFixed; loopcf++)
		{
		  termFixedBar[loopcf] = gBar - alphaFixed.get(loopcf) * dgBar + alphaFixed.get(loopcf) * alphaFixed.get(loopcf) * dg2Bar;
		  alphaFixedBar[loopcf] = termFixed[loopcf] * (-x - alphaFixed.get(loopcf)) * termFixedBar[loopcf] - termFixed[loopcf] * dgBar + 2d * alphaFixed.get(loopcf) * termFixed[loopcf] * dg2Bar;
		}
		double[] alphaIborBar = new double[sizeIbor];
		double[] termIborBar = new double[sizeIbor];
		for (int loopcf = 0; loopcf < sizeIbor; loopcf++)
		{
		  termIborBar[loopcf] = fBar - alphaIbor.get(loopcf) * dfBar + alphaIbor.get(loopcf) * alphaIbor.get(loopcf) * df2Bar;
		  alphaIborBar[loopcf] = termIbor[loopcf] * (-x - alphaIbor.get(loopcf)) * termIborBar[loopcf] - termIbor[loopcf] * dfBar + 2d * alphaIbor.get(loopcf) * termIbor[loopcf] * df2Bar;
		}
		return Pair.of(DoubleArray.copyOf(alphaFixedBar), DoubleArray.copyOf(alphaIborBar));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code HullWhiteOneFactorPiecewiseConstantInterestRateModel}.
	  /// </summary>
	  private static readonly TypedMetaBean<HullWhiteOneFactorPiecewiseConstantInterestRateModel> META_BEAN = LightMetaBean.of(typeof(HullWhiteOneFactorPiecewiseConstantInterestRateModel), MethodHandles.lookup());

	  /// <summary>
	  /// The meta-bean for {@code HullWhiteOneFactorPiecewiseConstantInterestRateModel}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<HullWhiteOneFactorPiecewiseConstantInterestRateModel> meta()
	  {
		return META_BEAN;
	  }

	  static HullWhiteOneFactorPiecewiseConstantInterestRateModel()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private HullWhiteOneFactorPiecewiseConstantInterestRateModel()
	  {
	  }

	  public override TypedMetaBean<HullWhiteOneFactorPiecewiseConstantInterestRateModel> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  return true;
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(32);
		buf.Append("HullWhiteOneFactorPiecewiseConstantInterestRateModel{");
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------

	}

}
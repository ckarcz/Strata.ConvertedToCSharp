using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Triple = com.opengamma.strata.collect.tuple.Triple;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using CholeskyDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.CholeskyDecompositionCommons;
	using PositiveOrZero = com.opengamma.strata.math.impl.minimization.PositiveOrZero;
	using LeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResults;
	using NonLinearLeastSquareWithPenalty = com.opengamma.strata.math.impl.statistics.leastsquare.NonLinearLeastSquareWithPenalty;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Caplet volatilities calibration to cap volatilities. 
	/// <para>
	/// The volatilities of the constituent caplets in the market caps are "model parameters"  
	/// and calibrated to the market data under the penalty constraint.
	/// The penalty is based on the second-order finite difference differentiation along the strike and expiry dimensions.
	/// </para>
	/// <para>
	/// If the shift curve is not present in {@code DirectIborCapletFloorletVolatilityDefinition}, 
	/// the resultant volatility type is the same as the input volatility type. e.g., 
	/// Black caplet volatilities are returned if Black cap volatilities are plugged in, 
	/// and normal caplet volatilities are returned otherwise. 
	/// On the other hand, if the shift curve is present in {@code DirectIborCapletFloorletVolatilityDefinition}, 
	/// Black caplet volatilities are returned for any input volatility type.  
	/// </para>
	/// <para>
	/// The calibration is conducted once the cap volatilities are converted to cap prices. 
	/// Thus the error values in {@code RawOptionData} are applied in the price space rather than the volatility space.
	/// </para>
	/// </summary>
	public class DirectIborCapletFloorletVolatilityCalibrator : IborCapletFloorletVolatilityCalibrator
	{

	  /// <summary>
	  /// Standard implementation. 
	  /// </summary>
	  private static readonly DirectIborCapletFloorletVolatilityCalibrator STANDARD = of(VolatilityIborCapFloorLegPricer.DEFAULT, 1.0e-8, ReferenceData.standard());

	  /// <summary>
	  /// The positive function.
	  /// <para>
	  /// The function returns true if the new trial position is positive or zero.
	  /// </para>
	  /// </summary>
	  private static readonly System.Func<DoubleArray, bool> POSITIVE = new PositiveOrZero();
	  /// <summary>
	  /// The conventional surface interpolator for the calibration.
	  /// <para>
	  /// Since node points are always hit in the calibration, the calibration does not rely on this interpolator generally.
	  /// </para>
	  /// </summary>
	  private static readonly GridSurfaceInterpolator INTERPOLATOR = GridSurfaceInterpolator.of(CurveInterpolators.LINEAR, CurveInterpolators.LINEAR);
	  /// <summary>
	  /// The non-linear least square with penalty. 
	  /// </summary>
	  private readonly NonLinearLeastSquareWithPenalty solver;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the standard instance. 
	  /// </summary>
	  /// <returns> the instance </returns>
	  public static DirectIborCapletFloorletVolatilityCalibrator standard()
	  {
		return DirectIborCapletFloorletVolatilityCalibrator.STANDARD;
	  }

	  /// <summary>
	  /// Obtains an instance. 
	  /// <para>
	  /// The epsilon is the parameter used in <seealso cref="NonLinearLeastSquareWithPenalty"/>,
	  /// where the iteration stops when certain quantities are smaller than this parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pricer">  the cap pricer </param>
	  /// <param name="epsilon">  the epsilon parameter </param>
	  /// <param name="referenceData">  the reference data </param>
	  /// <returns> the instance </returns>
	  public static DirectIborCapletFloorletVolatilityCalibrator of(VolatilityIborCapFloorLegPricer pricer, double epsilon, ReferenceData referenceData)
	  {

		return new DirectIborCapletFloorletVolatilityCalibrator(pricer, epsilon, referenceData);
	  }

	  // private constructor
	  private DirectIborCapletFloorletVolatilityCalibrator(VolatilityIborCapFloorLegPricer pricer, double epsilon, ReferenceData referenceData) : base(pricer, referenceData)
	  {

		this.solver = new NonLinearLeastSquareWithPenalty(new CholeskyDecompositionCommons(), epsilon);
	  }

	  //-------------------------------------------------------------------------
	  public override IborCapletFloorletVolatilityCalibrationResult calibrate(IborCapletFloorletVolatilityDefinition definition, ZonedDateTime calibrationDateTime, RawOptionData capFloorData, RatesProvider ratesProvider)
	  {

		ArgChecker.isTrue(ratesProvider.ValuationDate.Equals(calibrationDateTime.toLocalDate()), "valuationDate of ratesProvider should be coherent to calibrationDateTime");
		ArgChecker.isTrue(definition is DirectIborCapletFloorletVolatilityDefinition, "definition should be DirectIborCapletFloorletVolatilityDefinition");
		DirectIborCapletFloorletVolatilityDefinition directDefinition = (DirectIborCapletFloorletVolatilityDefinition) definition;
		// unpack cap data, create node caps
		IborIndex index = directDefinition.Index;
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate baseDate = index.EffectiveDateOffset.adjust(calibrationDate, ReferenceData);
		LocalDate startDate = baseDate.plus(index.Tenor);
		System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction = this.volatilitiesFunction(directDefinition, calibrationDateTime, capFloorData);
		SurfaceMetadata metadata = directDefinition.createMetadata(capFloorData);
		IList<Period> expiries = capFloorData.Expiries;
		DoubleArray strikes = capFloorData.Strikes;
		int nExpiries = expiries.Count;
		IList<double> timeList = new List<double>();
		IList<double> strikeList = new List<double>();
		IList<double> volList = new List<double>();
		IList<ResolvedIborCapFloorLeg> capList = new List<ResolvedIborCapFloorLeg>();
		IList<double> priceList = new List<double>();
		IList<double> errorList = new List<double>();
		DoubleMatrix errorMatrix = capFloorData.Error.orElse(DoubleMatrix.filled(nExpiries, strikes.size(), 1d));
		int[] startIndex = new int[nExpiries + 1];
		for (int i = 0; i < nExpiries; ++i)
		{
		  LocalDate endDate = baseDate.plus(expiries[i]);
		  DoubleArray volatilityForTime = capFloorData.Data.row(i);
		  DoubleArray errorForTime = errorMatrix.row(i);
		  reduceRawData(directDefinition, ratesProvider, capFloorData.Strikes, volatilityForTime, errorForTime, startDate, endDate, metadata, volatilitiesFunction, timeList, strikeList, volList, capList, priceList, errorList);
		  startIndex[i + 1] = volList.Count;
		  ArgChecker.isTrue(startIndex[i + 1] > startIndex[i], "no valid option data for {}", expiries[i]);
		}
		// create caplet nodes and initial caplet vol surface
		ResolvedIborCapFloorLeg cap = capList[capList.Count - 1];
		int nCaplets = cap.CapletFloorletPeriods.size();
		DoubleArray capletExpiries = DoubleArray.of(nCaplets, n => directDefinition.DayCount.relativeYearFraction(calibrationDate, cap.CapletFloorletPeriods.get(n).FixingDateTime.toLocalDate()));
		Triple<DoubleArray, DoubleArray, DoubleArray> capletNodes;
		DoubleArray initialVols = DoubleArray.copyOf(volList);
		if (directDefinition.ShiftCurve.Present)
		{
		  metadata = Surfaces.blackVolatilityByExpiryStrike(directDefinition.Name.Name, directDefinition.DayCount);
		  Curve shiftCurve = directDefinition.ShiftCurve.get();
		  if (capFloorData.DataType.Equals(NORMAL_VOLATILITY))
		  {
			initialVols = DoubleArray.of(capList.Count, n => volList[n] / (ratesProvider.iborIndexRates(index).rate(capList[n].FinalPeriod.IborRate.Observation) + shiftCurve.yValue(timeList[n])));
		  }
		  InterpolatedNodalSurface capVolSurface = InterpolatedNodalSurface.of(metadata, DoubleArray.copyOf(timeList), DoubleArray.copyOf(strikeList), initialVols, INTERPOLATOR);
		  capletNodes = createCapletNodes(capVolSurface, capletExpiries, strikes, directDefinition.ShiftCurve.get());
		  volatilitiesFunction = createShiftedBlackVolatilitiesFunction(index, calibrationDateTime, shiftCurve);
		}
		else
		{
		  InterpolatedNodalSurface capVolSurface = InterpolatedNodalSurface.of(metadata, DoubleArray.copyOf(timeList), DoubleArray.copyOf(strikeList), initialVols, INTERPOLATOR);
		  capletNodes = createCapletNodes(capVolSurface, capletExpiries, strikes);
		}
		InterpolatedNodalSurface baseSurface = InterpolatedNodalSurface.of(metadata, capletNodes.First, capletNodes.Second, capletNodes.Third, INTERPOLATOR);
		DoubleMatrix penaltyMatrix = directDefinition.computePenaltyMatrix(strikes, capletExpiries);
		// solve least square
		LeastSquareResults res = solver.solve(DoubleArray.copyOf(priceList), DoubleArray.copyOf(errorList), getPriceFunction(capList, ratesProvider, volatilitiesFunction, baseSurface), getJacobianFunction(capList, ratesProvider, volatilitiesFunction, baseSurface), capletNodes.Third, penaltyMatrix, POSITIVE);
		InterpolatedNodalSurface resSurface = InterpolatedNodalSurface.of(metadata, capletNodes.First, capletNodes.Second, res.FitParameters, directDefinition.Interpolator);
		return IborCapletFloorletVolatilityCalibrationResult.ofLeastSquare(volatilitiesFunction(resSurface), res.ChiSq);
	  }

	  private System.Func<Surface, IborCapletFloorletVolatilities> createShiftedBlackVolatilitiesFunction(IborIndex index, ZonedDateTime calibrationDateTime, Curve shiftCurve)
	  {

		System.Func<Surface, IborCapletFloorletVolatilities> func = (Surface s) =>
		{
	return ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(index, calibrationDateTime, s, shiftCurve);
		};
		return func;
	  }

	  //-------------------------------------------------------------------------
	  private Triple<DoubleArray, DoubleArray, DoubleArray> createCapletNodes(InterpolatedNodalSurface capVolSurface, DoubleArray capletExpiries, DoubleArray strikes)
	  {

		IList<double> timeCapletList = new List<double>();
		IList<double> strikeCapletList = new List<double>();
		IList<double> volCapletList = new List<double>();
		int nTimes = capletExpiries.size();
		int nStrikes = strikes.size();
		for (int i = 0; i < nTimes; ++i)
		{
		  double expiry = capletExpiries.get(i);
		  ((IList<double>)timeCapletList).AddRange(DoubleArray.filled(nStrikes, expiry).toList());
		  ((IList<double>)strikeCapletList).AddRange(strikes.toList());
		  ((IList<double>)volCapletList).AddRange(DoubleArray.of(nStrikes, n => capVolSurface.zValue(expiry, strikes.get(n))).toList()); // initial guess
		}
		return Triple.of(DoubleArray.copyOf(timeCapletList), DoubleArray.copyOf(strikeCapletList), DoubleArray.copyOf(volCapletList));
	  }

	  private Triple<DoubleArray, DoubleArray, DoubleArray> createCapletNodes(InterpolatedNodalSurface capVolSurface, DoubleArray capletExpiries, DoubleArray strikes, Curve shiftCurve)
	  {

		IList<double> timeCapletList = new List<double>();
		IList<double> strikeCapletList = new List<double>();
		IList<double> volCapletList = new List<double>();
		int nTimes = capletExpiries.size();
		int nStrikes = strikes.size();
		for (int i = 0; i < nTimes; ++i)
		{
		  double expiry = capletExpiries.get(i);
		  double shift = shiftCurve.yValue(expiry);
		  ((IList<double>)timeCapletList).AddRange(DoubleArray.filled(nStrikes, expiry).toList());
		  ((IList<double>)strikeCapletList).AddRange(strikes.plus(shift).toList());
		  ((IList<double>)volCapletList).AddRange(DoubleArray.of(nStrikes, n => capVolSurface.zValue(expiry, strikes.get(n) + shift)).toList()); // initial guess
		}
		return Triple.of(DoubleArray.copyOf(timeCapletList), DoubleArray.copyOf(strikeCapletList), DoubleArray.copyOf(volCapletList));
	  }

	  private System.Func<DoubleArray, DoubleArray> getPriceFunction(IList<ResolvedIborCapFloorLeg> capList, RatesProvider ratesProvider, System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction, InterpolatedNodalSurface baseSurface)
	  {

		int nCaps = capList.Count;
		System.Func<DoubleArray, DoubleArray> priceFunction = (DoubleArray capletVols) =>
		{
	IborCapletFloorletVolatilities newVols = volatilitiesFunction(baseSurface.withZValues(capletVols));
	return DoubleArray.of(nCaps, n => LegPricer.presentValue(capList[n], ratesProvider, newVols).Amount);
		};
		return priceFunction;
	  }

	  private System.Func<DoubleArray, DoubleMatrix> getJacobianFunction(IList<ResolvedIborCapFloorLeg> capList, RatesProvider ratesProvider, System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction, InterpolatedNodalSurface baseSurface)
	  {

		int nCaps = capList.Count;
		int nNodes = baseSurface.ParameterCount;
		System.Func<DoubleArray, DoubleMatrix> jacobianFunction = (DoubleArray capletVols) =>
		{
	IborCapletFloorletVolatilities newVols = volatilitiesFunction(baseSurface.withZValues(capletVols));
	return DoubleMatrix.ofArrayObjects(nCaps, nNodes, n => newVols.parameterSensitivity(LegPricer.presentValueSensitivityModelParamsVolatility(capList[n], ratesProvider, newVols).build()).Sensitivities.get(0).Sensitivity);
		};
		return jacobianFunction;
	  }

	}

}
using System.Collections;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory.SV_COMMONS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using DoubleRangeLimitTransform = com.opengamma.strata.math.impl.minimization.DoubleRangeLimitTransform;
	using NonLinearTransformFunction = com.opengamma.strata.math.impl.minimization.NonLinearTransformFunction;
	using ParameterLimitsTransform = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform;
	using ParameterLimitsTransform_LimitType = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform_LimitType;
	using SingleRangeLimitTransform = com.opengamma.strata.math.impl.minimization.SingleRangeLimitTransform;
	using UncoupledParameterTransforms = com.opengamma.strata.math.impl.minimization.UncoupledParameterTransforms;
	using LeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResults;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;
	using NonLinearLeastSquare = com.opengamma.strata.math.impl.statistics.leastsquare.NonLinearLeastSquare;
	using SabrParameters = com.opengamma.strata.pricer.model.SabrParameters;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Caplet volatilities calibration to cap volatilities based on SABR model.
	/// <para>
	/// The SABR parameters are represented by {@code NodalCurve}. 
	/// The node positions on the individual curves are flexible 
	/// and defined in  {@code SabrIborCapletFloorletVolatilityCalibrationDefinition}.
	/// The resulting volatilities object will be <seealso cref="SabrParametersIborCapletFloorletVolatilities"/>.
	/// </para>
	/// <para>
	/// The calibration to SABR is computed once the option volatility date is converted to prices. 
	/// Thus the error values in {@code RawOptionData} are applied in the price space rather than the volatility space.
	/// </para>
	/// </summary>
	public class SabrIborCapletFloorletVolatilityCalibrator : IborCapletFloorletVolatilityCalibrator
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SabrIborCapletFloorletVolatilityCalibrator DEFAULT = of(VolatilityIborCapFloorLegPricer.DEFAULT, SabrIborCapFloorLegPricer.DEFAULT, 1.0e-10, ReferenceData.standard());

	  /// <summary>
	  /// Transformation for SABR parameters.
	  /// </summary>
	  private static readonly ParameterLimitsTransform[] TRANSFORMS;
	  /// <summary>
	  /// SABR parameter range. 
	  /// </summary>
	  private const double RHO_LIMIT = 0.999;
	  static SabrIborCapletFloorletVolatilityCalibrator()
	  {
		TRANSFORMS = new ParameterLimitsTransform[4];
		TRANSFORMS[0] = new SingleRangeLimitTransform(0.0, ParameterLimitsTransform_LimitType.GREATER_THAN); // alpha > 0
		TRANSFORMS[1] = new DoubleRangeLimitTransform(0.0, 1.0); // 0 <= beta <= 1
		TRANSFORMS[2] = new DoubleRangeLimitTransform(-RHO_LIMIT, RHO_LIMIT); // -1 <= rho <= 1
		TRANSFORMS[3] = new DoubleRangeLimitTransform(0.001d, 2.50d);
		// nu > 0  and limit on Nu to avoid numerical instability in formula for large nu.
	  }

	  /// <summary>
	  /// The nonlinear least square solver.
	  /// </summary>
	  private readonly NonLinearLeastSquare solver;
	  /// <summary>
	  /// SABR pricer for cap/floor leg.
	  /// </summary>
	  private readonly SabrIborCapFloorLegPricer sabrPricer;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// The epsilon is the parameter used in <seealso cref="NonLinearLeastSquare"/>, where the iteration stops when certain 
	  /// quantities are smaller than this parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pricer">  the cap pricer </param>
	  /// <param name="sabrPricer">  the SABR cap pricer </param>
	  /// <param name="epsilon">  the epsilon parameter </param>
	  /// <param name="referenceData">  the reference data </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrator of(VolatilityIborCapFloorLegPricer pricer, SabrIborCapFloorLegPricer sabrPricer, double epsilon, ReferenceData referenceData)
	  {

		NonLinearLeastSquare solver = new NonLinearLeastSquare(SV_COMMONS, OG_ALGEBRA, epsilon);
		return new SabrIborCapletFloorletVolatilityCalibrator(pricer, sabrPricer, solver, referenceData);
	  }

	  // private constructor
	  private SabrIborCapletFloorletVolatilityCalibrator(VolatilityIborCapFloorLegPricer pricer, SabrIborCapFloorLegPricer sabrPricer, NonLinearLeastSquare solver, ReferenceData referenceData) : base(pricer, referenceData)
	  {

		this.sabrPricer = ArgChecker.notNull(sabrPricer, "sabrPricer");
		this.solver = ArgChecker.notNull(solver, "solver");
	  }

	  //-------------------------------------------------------------------------
	  public override IborCapletFloorletVolatilityCalibrationResult calibrate(IborCapletFloorletVolatilityDefinition definition, ZonedDateTime calibrationDateTime, RawOptionData capFloorData, RatesProvider ratesProvider)
	  {

		ArgChecker.isTrue(ratesProvider.ValuationDate.Equals(calibrationDateTime.toLocalDate()), "valuationDate of ratesProvider should be coherent to calibrationDateTime");
		ArgChecker.isTrue(definition is SabrIborCapletFloorletVolatilityCalibrationDefinition, "definition should be SabrIborCapletFloorletVolatilityCalibrationDefinition");
		SabrIborCapletFloorletVolatilityCalibrationDefinition sabrDefinition = (SabrIborCapletFloorletVolatilityCalibrationDefinition) definition;
		// unpack cap data, create node caps
		IborIndex index = sabrDefinition.Index;
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate baseDate = index.EffectiveDateOffset.adjust(calibrationDate, ReferenceData);
		LocalDate startDate = baseDate.plus(index.Tenor);
		System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction = this.volatilitiesFunction(sabrDefinition, calibrationDateTime, capFloorData);
		SurfaceMetadata metadata = sabrDefinition.createMetadata(capFloorData);
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
		  reduceRawData(sabrDefinition, ratesProvider, capFloorData.Strikes, volatilityForTime, errorForTime, startDate, endDate, metadata, volatilitiesFunction, timeList, strikeList, volList, capList, priceList, errorList);
		  startIndex[i + 1] = volList.Count;
		  ArgChecker.isTrue(startIndex[i + 1] > startIndex[i], "no valid option data for {}", expiries[i]);
		}
		// create initial caplet vol surface
		IList<CurveMetadata> metadataList = sabrDefinition.createSabrParameterMetadata();
		DoubleArray initialValues = sabrDefinition.createFullInitialValues();
		IList<Curve> curveList = sabrDefinition.createSabrParameterCurve(metadataList, initialValues);
		SabrParameters sabrParamsInitial = SabrParameters.of(curveList[0], curveList[1], curveList[2], curveList[3], sabrDefinition.ShiftCurve, sabrDefinition.SabrVolatilityFormula);
		SabrParametersIborCapletFloorletVolatilities vols = SabrParametersIborCapletFloorletVolatilities.of(sabrDefinition.Name, index, calibrationDateTime, sabrParamsInitial);
		// solve least square
		UncoupledParameterTransforms transform = new UncoupledParameterTransforms(initialValues, sabrDefinition.createFullTransform(TRANSFORMS), new BitArray());
		System.Func<DoubleArray, DoubleArray> valueFunction = createPriceFunction(sabrDefinition, ratesProvider, vols, capList, priceList);
		System.Func<DoubleArray, DoubleMatrix> jacobianFunction = createJacobianFunction(sabrDefinition, ratesProvider, vols, capList, priceList, index.Currency);
		NonLinearTransformFunction transFunc = new NonLinearTransformFunction(valueFunction, jacobianFunction, transform);
		LeastSquareResults res = solver.solve(DoubleArray.filled(priceList.Count, 1d), DoubleArray.copyOf(errorList), transFunc.FittingFunction, transFunc.FittingJacobian, transform.transform(initialValues));
		LeastSquareResultsWithTransform resTransform = new LeastSquareResultsWithTransform(res, transform);
		vols = updateParameters(sabrDefinition, vols, resTransform.ModelParameters);

		return IborCapletFloorletVolatilityCalibrationResult.ofLeastSquare(vols, res.ChiSq);
	  }

	  // price function
	  private System.Func<DoubleArray, DoubleArray> createPriceFunction(SabrIborCapletFloorletVolatilityCalibrationDefinition sabrDefinition, RatesProvider ratesProvider, SabrParametersIborCapletFloorletVolatilities volatilities, IList<ResolvedIborCapFloorLeg> capList, IList<double> priceList)
	  {

		System.Func<DoubleArray, DoubleArray> priceFunction = (DoubleArray x) =>
		{
	SabrParametersIborCapletFloorletVolatilities volsNew = updateParameters(sabrDefinition, volatilities, x);
	return DoubleArray.of(capList.Count, n => sabrPricer.presentValue(capList[n], ratesProvider, volsNew).Amount / priceList[n]);
		};
		return priceFunction;
	  }

	  // node sensitivity function
	  private System.Func<DoubleArray, DoubleMatrix> createJacobianFunction(SabrIborCapletFloorletVolatilityCalibrationDefinition sabrDefinition, RatesProvider ratesProvider, SabrParametersIborCapletFloorletVolatilities volatilities, IList<ResolvedIborCapFloorLeg> capList, IList<double> priceList, Currency currency)
	  {

		int nCaps = capList.Count;
		SabrParameters sabrParams = volatilities.Parameters;
		CurveName alphaName = sabrParams.AlphaCurve.Name;
		CurveName betaName = sabrParams.BetaCurve.Name;
		CurveName rhoName = sabrParams.RhoCurve.Name;
		CurveName nuName = sabrParams.NuCurve.Name;
		System.Func<DoubleArray, DoubleMatrix> jacobianFunction = (DoubleArray x) =>
		{
	SabrParametersIborCapletFloorletVolatilities volsNew = updateParameters(sabrDefinition, volatilities, x);
	double[][] jacobian = new double[nCaps][];
	for (int i = 0; i < nCaps; ++i)
	{
	  PointSensitivities point = sabrPricer.presentValueSensitivityModelParamsSabr(capList[i], ratesProvider, volsNew).build();
	  CurrencyParameterSensitivities sensi = volsNew.parameterSensitivity(point);
	  double targetPriceInv = 1d / priceList[i];
	  DoubleArray sensitivities = sensi.getSensitivity(alphaName, currency).Sensitivity;
	  if (sabrDefinition.BetaCurve.Present)
	  { // beta fixed
		sensitivities = sensitivities.concat(sensi.getSensitivity(rhoName, currency).Sensitivity);
	  }
	  else
	  { // rho fixed
		sensitivities = sensitivities.concat(sensi.getSensitivity(betaName, currency).Sensitivity);
	  }
	  jacobian[i] = sensitivities.concat(sensi.getSensitivity(nuName, currency).Sensitivity).multipliedBy(targetPriceInv).toArray();
	}
	return DoubleMatrix.ofUnsafe(jacobian);
		};
		return jacobianFunction;
	  }

	  // update vols
	  private SabrParametersIborCapletFloorletVolatilities updateParameters(SabrIborCapletFloorletVolatilityCalibrationDefinition sabrDefinition, SabrParametersIborCapletFloorletVolatilities volatilities, DoubleArray newValues)
	  {

		SabrParameters sabrParams = volatilities.Parameters;
		CurveMetadata alphaMetadata = sabrParams.AlphaCurve.Metadata;
		CurveMetadata betaMetadata = sabrParams.BetaCurve.Metadata;
		CurveMetadata rhoMetadata = sabrParams.RhoCurve.Metadata;
		CurveMetadata nuMetadata = sabrParams.NuCurve.Metadata;
		IList<Curve> newCurveList = sabrDefinition.createSabrParameterCurve(ImmutableList.of(alphaMetadata, betaMetadata, rhoMetadata, nuMetadata), newValues);
		SabrParameters newSabrParams = SabrParameters.of(newCurveList[0], newCurveList[1], newCurveList[2], newCurveList[3], sabrDefinition.ShiftCurve, sabrDefinition.SabrVolatilityFormula);
		SabrParametersIborCapletFloorletVolatilities newVols = SabrParametersIborCapletFloorletVolatilities.of(volatilities.Name, volatilities.Index, volatilities.ValuationDateTime, newSabrParams);
		return newVols;
	  }

	}

}
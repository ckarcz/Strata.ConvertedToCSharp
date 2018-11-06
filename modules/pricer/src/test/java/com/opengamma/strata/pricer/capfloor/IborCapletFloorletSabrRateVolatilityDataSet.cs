using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;


	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using SimpleCurveParameterMetadata = com.opengamma.strata.market.curve.SimpleCurveParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using SabrParameters = com.opengamma.strata.pricer.model.SabrParameters;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;

	/// <summary>
	/// Data sets for testing SABR model for Ibor caplet/floorlet.
	/// </summary>
	public class IborCapletFloorletSabrRateVolatilityDataSet
	{

	  private static readonly DoubleArray EUR_DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray EUR_DSC_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  private static readonly CurveName EUR_DSC_NAME = CurveName.of("EUR-DSCON");
	  private static readonly CurveMetadata EUR_DSC_META = Curves.zeroRates(EUR_DSC_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve EUR_DSC_CURVE = InterpolatedNodalCurve.of(EUR_DSC_META, EUR_DSC_TIME, EUR_DSC_RATE, LINEAR);
	  private static readonly DoubleArray EUR_FWD_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
	  private static readonly DoubleArray EUR_FWD_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0175, 0.0190, 0.0200, 0.0210);
	  private static readonly CurveName EUR_FWD_NAME = CurveName.of("EUR-FWD");
	  private static readonly CurveMetadata EUR_FWD_META = Curves.zeroRates(EUR_FWD_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve EUR_FWD_CURVE = InterpolatedNodalCurve.of(EUR_FWD_META, EUR_FWD_TIME, EUR_FWD_RATE, LINEAR);

	  private static readonly DoubleArray ALPHA_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray BETA_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0, 100.0);
	  private static readonly DoubleArray RHO_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0, 100.0);
	  private static readonly DoubleArray NU_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0, 100.0);
	  internal static readonly CurveMetadata META_ALPHA = Curves.sabrParameterByExpiry("Test-SABR-Alpha", ACT_ACT_ISDA, ValueType.SABR_ALPHA).withParameterMetadata(createParameterMetadata(ALPHA_TIME));
	  internal static readonly CurveMetadata META_BETA = Curves.sabrParameterByExpiry("Test-SABR-Beta", ACT_ACT_ISDA, ValueType.SABR_BETA).withParameterMetadata(createParameterMetadata(BETA_TIME));
	  internal static readonly CurveMetadata META_RHO = Curves.sabrParameterByExpiry("Test-SABR-Rho", ACT_ACT_ISDA, ValueType.SABR_RHO).withParameterMetadata(createParameterMetadata(RHO_TIME));
	  internal static readonly CurveMetadata META_NU = Curves.sabrParameterByExpiry("Test-SABR-Nu", ACT_ACT_ISDA, ValueType.SABR_NU).withParameterMetadata(createParameterMetadata(NU_TIME));

	  private static readonly DoubleArray ALPHA_VALUE_FLAT = DoubleArray.of(0.05, 0.05, 0.05, 0.05, 0.05, 0.05);
	  private static readonly DoubleArray BETA_VALUE_FLAT = DoubleArray.of(0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5);
	  private static readonly DoubleArray RHO_VALUE_FLAT = DoubleArray.of(-0.25, -0.25, -0.25, -0.25, -0.25, -0.25, -0.25);
	  private static readonly DoubleArray NU_VALUE_FLAT = DoubleArray.of(0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5);
	  private static readonly InterpolatedNodalCurve CURVE_ALPHA_FLAT = InterpolatedNodalCurve.of(META_ALPHA, ALPHA_TIME, ALPHA_VALUE_FLAT, LINEAR);
	  private static readonly InterpolatedNodalCurve CURVE_BETA_FLAT = InterpolatedNodalCurve.of(META_BETA, BETA_TIME, BETA_VALUE_FLAT, LINEAR);
	  private static readonly InterpolatedNodalCurve CURVE_RHO_FLAT = InterpolatedNodalCurve.of(META_RHO, RHO_TIME, RHO_VALUE_FLAT, LINEAR);
	  private static readonly InterpolatedNodalCurve CURVE_NU_FLAT = InterpolatedNodalCurve.of(META_NU, NU_TIME, NU_VALUE_FLAT, LINEAR);
	  internal static readonly SabrParameters SABR_PARAM_FLAT = SabrParameters.of(CURVE_ALPHA_FLAT, CURVE_BETA_FLAT, CURVE_RHO_FLAT, CURVE_NU_FLAT, SabrVolatilityFormula.hagan());

	  private static readonly DoubleArray ALPHA_VALUE = DoubleArray.of(0.035, 0.057, 0.044, 0.033, 0.032, 0.022);
	  private static readonly DoubleArray BETA_VALUE = DoubleArray.of(0.5, 0.6, 0.4, 0.1, 0.8, 0.2, 0.2);
	  private static readonly DoubleArray RHO_VALUE = DoubleArray.of(-0.75, -0.2, 0.05, 0.25, -0.25, 0.15, 0.22);
	  private static readonly DoubleArray NU_VALUE = DoubleArray.of(0.35, 0.45, 0.59, 0.52, 0.51, 0.32, 0.44);
	  private static readonly InterpolatedNodalCurve CURVE_ALPHA = InterpolatedNodalCurve.of(META_ALPHA, ALPHA_TIME, ALPHA_VALUE, LINEAR);
	  private static readonly InterpolatedNodalCurve CURVE_BETA = InterpolatedNodalCurve.of(META_BETA, BETA_TIME, BETA_VALUE, LINEAR);
	  private static readonly InterpolatedNodalCurve CURVE_RHO = InterpolatedNodalCurve.of(META_RHO, RHO_TIME, RHO_VALUE, LINEAR);
	  private static readonly InterpolatedNodalCurve CURVE_NU = InterpolatedNodalCurve.of(META_NU, NU_TIME, NU_VALUE, LINEAR);
	  private static readonly DoubleArray SHIFT_TIME = DoubleArray.of(0.0, 5.0, 10.0, 100.0);
	  private static readonly DoubleArray SHIFT_VALUE = DoubleArray.of(0.01, 0.02, 0.012, 0.01);
	  internal static readonly CurveMetadata META_SHIFT = DefaultCurveMetadata.builder().curveName(CurveName.of("Test-SABR-Shift")).xValueType(ValueType.YEAR_FRACTION).parameterMetadata(createParameterMetadata(SHIFT_TIME)).build();
	  private static readonly InterpolatedNodalCurve CURVE_SHIFT = InterpolatedNodalCurve.of(META_SHIFT, SHIFT_TIME, SHIFT_VALUE, LINEAR);
	  internal static readonly SabrParameters SABR_PARAM = SabrParameters.of(CURVE_ALPHA, CURVE_BETA, CURVE_RHO, CURVE_NU, CURVE_SHIFT, SabrVolatilityFormula.hagan());

	  internal const double CONST_SHIFT = 0.015;
	  private static readonly DefaultCurveMetadata META_CONST_SHIFT = DefaultCurveMetadata.of("Test-SABR-Shift");
	  internal static readonly ConstantCurve CURVE_CONST_SHIFT = ConstantCurve.of(META_CONST_SHIFT, CONST_SHIFT);
	  internal static readonly SabrParameters SABR_PARAM_CONST_SHIFT = SabrParameters.of(CURVE_ALPHA, CURVE_BETA, CURVE_RHO, CURVE_NU, CURVE_CONST_SHIFT, SabrVolatilityFormula.hagan());

	  internal static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("Test-SABR");

	  // create a list of SimpleCurveParameterMetadata
	  private static IList<ParameterMetadata> createParameterMetadata(DoubleArray time)
	  {
		int n = time.size();
		IList<ParameterMetadata> list = new List<ParameterMetadata>();
		for (int i = 0; i < n; ++i)
		{
		  list.Add(SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, time.get(i)));
		}
		return list;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains {@code ImmutableRatesProvider} for specified valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="index">  the index </param>
	  /// <param name="timeSeries">  the time series </param>
	  /// <returns> the rates provider </returns>
	  public static ImmutableRatesProvider getRatesProvider(LocalDate valuationDate, IborIndex index, LocalDateDoubleTimeSeries timeSeries)
	  {
		return ImmutableRatesProvider.builder(valuationDate).discountCurve(EUR, EUR_DSC_CURVE).iborIndexCurve(index, EUR_FWD_CURVE).timeSeries(index, timeSeries).build();
	  }

	  /// <summary>
	  /// Obtains {@code SabrParametersIborCapletFloorletVolatilities} with constant SABR parameters for specified valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="index">  the index </param>
	  /// <returns> the volatility provider </returns>
	  public static SabrParametersIborCapletFloorletVolatilities getVolatilitiesFlatParameters(LocalDate valuationDate, IborIndex index)
	  {

		ZonedDateTime dateTime = valuationDate.atStartOfDay(ZoneOffset.UTC);
		return SabrParametersIborCapletFloorletVolatilities.of(NAME, index, dateTime, SABR_PARAM_FLAT);
	  }

	  /// <summary>
	  /// Obtains {@code SabrParametersIborCapletFloorletVolatilities} with constant shift for specified valuation date.
	  /// </summary>
	  /// <param name="dateTime">  the valuation date time </param>
	  /// <param name="index">  the index </param>
	  /// <returns> the volatility provider </returns>
	  public static SabrParametersIborCapletFloorletVolatilities getVolatilities(ZonedDateTime dateTime, IborIndex index)
	  {
		return SabrParametersIborCapletFloorletVolatilities.of(NAME, index, dateTime, SABR_PARAM_CONST_SHIFT);
	  }

	}

}
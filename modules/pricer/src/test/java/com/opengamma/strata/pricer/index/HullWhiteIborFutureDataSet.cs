/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using IborFuture = com.opengamma.strata.product.index.IborFuture;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;

	/// <summary>
	/// Data set used for testing futures pricers under Hull-White one factor model.
	/// </summary>
	public class HullWhiteIborFutureDataSet
	{

	  // Hull-White model parameters
	  private const double MEAN_REVERSION = 0.01;
	  private static readonly DoubleArray VOLATILITY = DoubleArray.of(0.01, 0.011, 0.012, 0.013, 0.014);
	  private static readonly DoubleArray VOLATILITY_TIME = DoubleArray.of(0.5, 1.0, 2.0, 5.0);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParameters MODEL_PARAMETERS = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);

	  // Rates provider
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;
	  private static readonly DoubleArray DSC_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray DSC_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  /// <summary>
	  /// discounting curve name </summary>
	  public static readonly CurveName DSC_NAME = CurveName.of("EUR Dsc");
	  private static readonly CurveMetadata META_DSC = Curves.zeroRates(DSC_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve DSC_CURVE = InterpolatedNodalCurve.of(META_DSC, DSC_TIME, DSC_RATE, INTERPOLATOR);
	  private static readonly DoubleArray FWD3_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
	  private static readonly DoubleArray FWD3_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0175, 0.0190, 0.0200, 0.0210);
	  /// <summary>
	  /// Forward curve name </summary>
	  public static readonly CurveName FWD3_NAME = CurveName.of("EUR EURIBOR 3M");
	  private static readonly CurveMetadata META_FWD3 = Curves.zeroRates(FWD3_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve FWD3_CURVE = InterpolatedNodalCurve.of(META_FWD3, FWD3_TIME, FWD3_RATE, INTERPOLATOR);
	  private static readonly DoubleArray FWD6_TIME = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  private static readonly DoubleArray FWD6_RATE = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  /// <summary>
	  /// Forward curve name </summary>
	  public static readonly CurveName FWD6_NAME = CurveName.of("EUR EURIBOR 6M");
	  private static readonly CurveMetadata META_FWD6 = Curves.zeroRates(FWD6_NAME, ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve FWD6_CURVE = InterpolatedNodalCurve.of(META_FWD6, FWD6_TIME, FWD6_RATE, INTERPOLATOR);

	  /// <summary>
	  /// Creates Hull-White one factor model parameters with specified valuation date for swaption
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns>  the parameter provider </returns>
	  public static HullWhiteOneFactorPiecewiseConstantParametersProvider createHullWhiteProvider(LocalDate valuationDate)
	  {
		return HullWhiteOneFactorPiecewiseConstantParametersProvider.of(MODEL_PARAMETERS, ACT_ACT_ISDA, valuationDate.atTime(LocalTime.NOON).atZone(ZoneOffset.UTC));
	  }

	  /// <summary>
	  /// Creates rates provider with specified  valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns>  the rates provider </returns>
	  public static ImmutableRatesProvider createRatesProvider(LocalDate valuationDate)
	  {
		return ImmutableRatesProvider.builder(valuationDate).discountCurves(ImmutableMap.of(EUR, DSC_CURVE)).indexCurves(ImmutableMap.of(EUR_EURIBOR_3M, FWD3_CURVE, EUR_EURIBOR_6M, FWD6_CURVE)).fxRateProvider(FxMatrix.empty()).build();
	  }

	  // Instruments
	  /// <summary>
	  /// Notional of product </summary>
	  public const double NOTIONAL = 1000000.0;
	  private static readonly LocalDate LAST_TRADE_DATE = LocalDate.of(2012, 9, 17);
	  private const double FUTURE_FACTOR = 0.25;
	  /// <summary>
	  ///  Ibor future product </summary>
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Ticker", "FutSec");
	  public static readonly IborFuture IBOR_FUTURE = IborFuture.builder().securityId(SECURITY_ID).currency(EUR).notional(NOTIONAL).lastTradeDate(LAST_TRADE_DATE).index(EUR_EURIBOR_3M).accrualFactor(FUTURE_FACTOR).rounding(Rounding.none()).build();
	  /// <summary>
	  /// Quantity of trade </summary>
	  public const long QUANTITY = 400L;
	  private const double REFERENCE_PRICE = 0.99;
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2011, 5, 11);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE_DATE).build();
	  /// <summary>
	  /// Ibor future trade </summary>
	  public static readonly IborFutureTrade IBOR_FUTURE_TRADE = IborFutureTrade.builder().info(TRADE_INFO).product(IBOR_FUTURE).quantity(QUANTITY).price(REFERENCE_PRICE).build();
	  /// <summary>
	  /// Last margin price </summary>
	  public const double LAST_MARGIN_PRICE = 0.98;
	}

}
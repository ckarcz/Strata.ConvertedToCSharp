/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using IsdaCreditCurveDefinition = com.opengamma.strata.market.curve.IsdaCreditCurveDefinition;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CdsIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIsdaCreditCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using ResolvedTradeParameterMetadata = com.opengamma.strata.market.param.ResolvedTradeParameterMetadata;
	using ConstantRecoveryRates = com.opengamma.strata.pricer.credit.ConstantRecoveryRates;
	using FastCreditCurveCalibrator = com.opengamma.strata.pricer.credit.FastCreditCurveCalibrator;
	using ImmutableCreditRatesProvider = com.opengamma.strata.pricer.credit.ImmutableCreditRatesProvider;
	using IsdaCompliantCreditCurveCalibrator = com.opengamma.strata.pricer.credit.IsdaCompliantCreditCurveCalibrator;
	using IsdaCreditDiscountFactors = com.opengamma.strata.pricer.credit.IsdaCreditDiscountFactors;
	using LegalEntitySurvivalProbabilities = com.opengamma.strata.pricer.credit.LegalEntitySurvivalProbabilities;
	using RecoveryRates = com.opengamma.strata.pricer.credit.RecoveryRates;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using Cds = com.opengamma.strata.product.credit.Cds;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;
	using CdsConvention = com.opengamma.strata.product.credit.type.CdsConvention;
	using DatesCdsTemplate = com.opengamma.strata.product.credit.type.DatesCdsTemplate;
	using ImmutableCdsConvention = com.opengamma.strata.product.credit.type.ImmutableCdsConvention;

	/// <summary>
	/// Testing data.
	/// </summary>
	public class CreditDataSet
	{

	  private static readonly IsdaCompliantCreditCurveCalibrator BUILDER = FastCreditCurveCalibrator.standard();

	  internal static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double ONE_BP = 1.0e-4;
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2013, 1, 3);
	  private static readonly HolidayCalendarId CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "ABCXX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES;
	  static CreditDataSet()
	  {
		ImmutableList.Builder<StandardId> builder = ImmutableList.builder();
		for (int i = 0; i < 97; ++i)
		{
		  builder.add(StandardId.of("OG", i.ToString()));
		}
		LEGAL_ENTITIES = builder.build();
		double flatRate = 0.05;
		double t = 20.0;
		IsdaCreditDiscountFactors yieldCurve = IsdaCreditDiscountFactors.of(USD, VALUATION_DATE, CurveName.of("discount"), DoubleArray.of(t), DoubleArray.of(flatRate), ACT_365F);
		DISCOUNT_CURVE = yieldCurve.Curve;
		RecoveryRates recoveryRate = ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION_DATE, RECOVERY_RATE);
		// create the curve nodes and input market quotes
		ImmutableMarketDataBuilder marketQuoteBuilder = ImmutableMarketData.builder(VALUATION_DATE);
		ImmutableList.Builder<CdsIsdaCreditCurveNode> nodesBuilder = ImmutableList.builder();
		ImmutableList.Builder<ResolvedTradeParameterMetadata> cdsMetadataBuilder = ImmutableList.builder();
		ImmutableList.Builder<ResolvedTradeParameterMetadata> cdsIndexMetadataBuilder = ImmutableList.builder();
		for (int i = 0; i < NUM_MARKET_CDS; i++)
		{
		  QuoteId quoteId = QuoteId.of(StandardId.of("OG", PAR_SPD_DATES[i].ToString()));
		  CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofParSpread(DatesCdsTemplate.of(VALUATION_DATE, PAR_SPD_DATES[i], CDS_CONV), quoteId, LEGAL_ENTITY);
		  MARKET_CDS[i] = CdsTrade.builder().product(Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, VALUATION_DATE, PAR_SPD_DATES[i], P3M, SAT_SUN, PAR_SPREADS[i] * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
		  MARKET_CDS_INDEX[i] = CdsIndexTrade.builder().product(CdsIndex.of(BuySell.BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, VALUATION_DATE, PAR_SPD_DATES[i], P3M, SAT_SUN, PAR_SPREADS[i] * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
		  marketQuoteBuilder.addValue(quoteId, PAR_SPREADS[i] * ONE_BP);
		  nodesBuilder.add(node);
		  cdsMetadataBuilder.add(ResolvedTradeParameterMetadata.of(MARKET_CDS[i], MARKET_CDS[i].Product.ProtectionEndDate.ToString()));
		  cdsIndexMetadataBuilder.add(ResolvedTradeParameterMetadata.of(MARKET_CDS_INDEX[i], MARKET_CDS_INDEX[i].Product.ProtectionEndDate.ToString()));
		}
		ImmutableMarketData marketQuotes = marketQuoteBuilder.build();
		ImmutableList<CdsIsdaCreditCurveNode> nodes = nodesBuilder.build();
		CDS_METADATA = cdsMetadataBuilder.build();
		CDS_INDEX_METADATA = cdsIndexMetadataBuilder.build();
		ImmutableCreditRatesProvider rates = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY, recoveryRate)).discountCurves(ImmutableMap.of(USD, yieldCurve)).build();
		IsdaCreditCurveDefinition definition = IsdaCreditCurveDefinition.of(CREDIT_CURVE_NAME, USD, VALUATION_DATE, ACT_365F, nodes, true, true);
		// calibrate
		LegalEntitySurvivalProbabilities calibrated = BUILDER.calibrate(definition, marketQuotes, rates, REF_DATA);
		NodalCurve underlyingCurve = ((IsdaCreditDiscountFactors) calibrated.SurvivalProbabilities).Curve;
		CDS_CREDIT_CURVE = underlyingCurve;
		INDEX_CREDIT_CURVE = underlyingCurve.withMetadata(underlyingCurve.Metadata.withInfo(CurveInfoType.CDS_INDEX_FACTOR, INDEX_FACTOR).withParameterMetadata(CDS_INDEX_METADATA)); // replace parameter metadata
		CDS_RECOVERY_RATE = ConstantCurve.of(Curves.recoveryRates("CDS recovery rate", ACT_365F), RECOVERY_RATE);
		INDEX_RECOVERY_RATE = ConstantCurve.of(Curves.recoveryRates("Index recovery rate", ACT_365F), RECOVERY_RATE);
	  }
	  private const double NOTIONAL = 1.0e7;
	  // CDS trade
	  private static readonly Cds CDS_PRODUCT = Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, LocalDate.of(2012, 12, 20), LocalDate.of(2020, 10, 20), P3M, CALENDAR, 0.015);
	  private static readonly LocalDate SETTLEMENT_DATE = CDS_PRODUCT.SettlementDateOffset.adjust(VALUATION_DATE, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(VALUATION_DATE).settlementDate(SETTLEMENT_DATE).build();
	  private static readonly Payment CDS_UPFRONT = Payment.of(USD, -NOTIONAL * 0.2, SETTLEMENT_DATE);
	  internal static readonly CdsTrade CDS_TRADE = CdsTrade.builder().product(CDS_PRODUCT).info(TRADE_INFO).upfrontFee(AdjustablePayment.of(CDS_UPFRONT)).build();
	  internal static readonly ResolvedCdsTrade RESOLVED_CDS_TRADE = CDS_TRADE.resolve(REF_DATA);
	  // CDS index trade
	  private static readonly double INDEX_FACTOR = 93d / 97d;
	  private static readonly CdsIndex INDEX_PRODUCT = CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, LocalDate.of(2012, 12, 20), LocalDate.of(2020, 10, 20), P3M, CALENDAR, 0.015);
	  private static readonly Payment INDEX_UPFRONT = Payment.of(USD, -NOTIONAL * 0.15, SETTLEMENT_DATE);
	  internal static readonly CdsIndexTrade INDEX_TRADE = CdsIndexTrade.builder().product(INDEX_PRODUCT).info(TRADE_INFO).upfrontFee(AdjustablePayment.of(INDEX_UPFRONT)).build();
	  internal static readonly ResolvedCdsIndexTrade RESOLVED_INDEX_TRADE = INDEX_TRADE.resolve(REF_DATA);
	  // CDS lookup
	  internal static readonly CurveId CDS_CREDIT_CURVE_ID = CurveId.of("Default", "Credit-ABC");
	  internal static readonly CurveId USD_DSC_CURVE_ID = CurveId.of("Default", "Dsc-USD");
	  internal static readonly CurveId CDS_RECOVERY_CURVE_ID = CurveId.of("Default", "Recovery-ABC");
	  internal static readonly CreditRatesMarketDataLookup CDS_LOOKUP = CreditRatesMarketDataLookup.of(ImmutableMap.of(Pair.of(LEGAL_ENTITY, USD), CDS_CREDIT_CURVE_ID), ImmutableMap.of(USD, USD_DSC_CURVE_ID), ImmutableMap.of(LEGAL_ENTITY, CDS_RECOVERY_CURVE_ID));
	  internal static readonly CalculationParameters CDS_PARAMS = CalculationParameters.of(CDS_LOOKUP);
	  // CDS index lookup
	  internal static readonly CurveId INDEX_CREDIT_CURVE_ID = CurveId.of("Default", "Credit-ABCXX");
	  internal static readonly CurveId INDEX_RECOVERY_CURVE_ID = CurveId.of("Default", "Recovery-ABCXX");
	  internal static readonly CreditRatesMarketDataLookup INDEX_LOOKUP = CreditRatesMarketDataLookup.of(ImmutableMap.of(Pair.of(INDEX_ID, USD), INDEX_CREDIT_CURVE_ID), ImmutableMap.of(USD, USD_DSC_CURVE_ID), ImmutableMap.of(INDEX_ID, INDEX_RECOVERY_CURVE_ID));
	  internal static readonly CalculationParameters INDEX_PARAMS = CalculationParameters.of(INDEX_LOOKUP);

	  // curve
	  private static readonly LocalDate[] PAR_SPD_DATES = new LocalDate[] {LocalDate.of(2013, 6, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2014, 3, 20), LocalDate.of(2015, 3, 20), LocalDate.of(2016, 3, 20), LocalDate.of(2018, 3, 20), LocalDate.of(2023, 3, 20)};
	  private static readonly double[] PAR_SPREADS = new double[] {50, 70, 80, 95, 100, 95, 80};
	  private static readonly int NUM_MARKET_CDS = PAR_SPD_DATES.Length;
	  private static readonly ResolvedCdsTrade[] MARKET_CDS = new ResolvedCdsTrade[NUM_MARKET_CDS];
	  private static readonly ResolvedCdsIndexTrade[] MARKET_CDS_INDEX = new ResolvedCdsIndexTrade[NUM_MARKET_CDS];
	  private const double RECOVERY_RATE = 0.4;
	//  private static final RecoveryRates RECOVERY_CURVE = ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION_DATE, RECOVERY_RATE);
	//  private static final RecoveryRates RECOVERY_CURVE_INDEX = ConstantRecoveryRates.of(INDEX_ID, VALUATION_DATE, RECOVERY_RATE);
	//  private static final IsdaCompliantZeroRateDiscountFactors YIELD_CURVE;
	  private static readonly NodalCurve CDS_CREDIT_CURVE;
	  private static readonly NodalCurve INDEX_CREDIT_CURVE;
	  private static readonly ConstantCurve CDS_RECOVERY_RATE;
	  private static readonly ConstantCurve INDEX_RECOVERY_RATE;
	  private static readonly NodalCurve DISCOUNT_CURVE;
	  private static readonly CurveName CREDIT_CURVE_NAME = CurveName.of("credit");
	  private static readonly CdsConvention CDS_CONV = ImmutableCdsConvention.builder().businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, SAT_SUN)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).currency(USD).dayCount(ACT_360).name("sat_sun_conv").paymentFrequency(Frequency.P3M).settlementDateOffset(DaysAdjustment.ofBusinessDays(3, SAT_SUN)).build();
	  internal static readonly ImmutableList<ResolvedTradeParameterMetadata> CDS_METADATA;
	  private static readonly ImmutableList<ResolvedTradeParameterMetadata> CDS_INDEX_METADATA;
	  // the market data for credit pricing
	  internal static readonly ScenarioMarketData MARKET_DATA = ImmutableScenarioMarketData.of(1, VALUATION_DATE, ImmutableMap.of(CDS_CREDIT_CURVE_ID, MarketDataBox.ofSingleValue(CDS_CREDIT_CURVE), INDEX_CREDIT_CURVE_ID, MarketDataBox.ofSingleValue(INDEX_CREDIT_CURVE), USD_DSC_CURVE_ID, MarketDataBox.ofSingleValue(DISCOUNT_CURVE), CDS_RECOVERY_CURVE_ID, MarketDataBox.ofSingleValue(CDS_RECOVERY_RATE), INDEX_RECOVERY_CURVE_ID, MarketDataBox.ofSingleValue(INDEX_RECOVERY_RATE)), ImmutableMap.of());

	}

}
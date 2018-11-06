/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using IsdaCreditCurveDefinition = com.opengamma.strata.market.curve.IsdaCreditCurveDefinition;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CdsIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIsdaCreditCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ResolvedTradeParameterMetadata = com.opengamma.strata.market.param.ResolvedTradeParameterMetadata;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
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
	/// Test <seealso cref="AnalyticSpreadSensitivityCalculator"/> and <seealso cref="FiniteDifferenceSpreadSensitivityCalculator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SpreadSensitivityCalculatorTest
	public class SpreadSensitivityCalculatorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double ONE_BP = 1.0e-4;

	  private static readonly IsdaCompliantCreditCurveCalibrator BUILDER = FastCreditCurveCalibrator.standard();
	  private static readonly IsdaCdsTradePricer PRICER = IsdaCdsTradePricer.DEFAULT;
	  private static readonly IsdaHomogenousCdsIndexTradePricer PRICER_INDEX = IsdaHomogenousCdsIndexTradePricer.DEFAULT;
	  private static readonly FiniteDifferenceSpreadSensitivityCalculator CS01_FD = FiniteDifferenceSpreadSensitivityCalculator.DEFAULT;
	  private static readonly AnalyticSpreadSensitivityCalculator CS01_AN = AnalyticSpreadSensitivityCalculator.DEFAULT;
	  private static readonly MarketQuoteSensitivityCalculator QUOTE_CAL = MarketQuoteSensitivityCalculator.DEFAULT;
	  // valuation CDS
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2013, 4, 21);
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABCD");
	  private const double NOTIONAL = 1e7;
	  private static readonly LocalDate START = LocalDate.of(2013, 2, 3);
	  private static readonly LocalDate END1 = LocalDate.of(2018, 3, 20);
	  private static readonly LocalDate END2 = LocalDate.of(2020, 2, 20);
	  private const double DEAL_SPREAD = 101;
	  private static readonly ResolvedCdsTrade CDS1 = CdsTrade.builder().product(Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START, END1, P3M, SAT_SUN, DEAL_SPREAD * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
	  private static readonly ResolvedCdsTrade CDS2 = CdsTrade.builder().product(Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, START, END2, P3M, SAT_SUN, DEAL_SPREAD * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
	  // market CDSs
	  private static readonly LocalDate[] PAR_SPD_DATES = new LocalDate[] {LocalDate.of(2013, 6, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2014, 3, 20), LocalDate.of(2015, 3, 20), LocalDate.of(2016, 3, 20), LocalDate.of(2018, 3, 20), LocalDate.of(2023, 3, 20)};
	  private static readonly double[] PAR_SPREADS = new double[] {50, 70, 80, 95, 100, 95, 80};
	  private static readonly int NUM_MARKET_CDS = PAR_SPD_DATES.Length;
	  private static readonly ResolvedCdsTrade[] MARKET_CDS = new ResolvedCdsTrade[NUM_MARKET_CDS];
	  private static readonly ResolvedCdsIndexTrade[] MARKET_CDS_INDEX = new ResolvedCdsIndexTrade[NUM_MARKET_CDS];
	  // valuation CDS index
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "AAXX");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES = ImmutableList.of(StandardId.of("OG", "AA1"), StandardId.of("OG", "AA2"), StandardId.of("OG", "AA3"), StandardId.of("OG", "AA4"));
	  private const double INDEX_FACTOR = 0.75;
	  private static readonly ResolvedCdsIndexTrade CDS_INDEX = CdsIndexTrade.builder().product(CdsIndex.of(BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, START, END2, P3M, SAT_SUN, DEAL_SPREAD * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
	  // curve
	  private const double RECOVERY_RATE = 0.4;
	  private static readonly RecoveryRates RECOVERY_CURVE = ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION_DATE, RECOVERY_RATE);
	  private static readonly RecoveryRates RECOVERY_CURVE_INDEX = ConstantRecoveryRates.of(INDEX_ID, VALUATION_DATE, RECOVERY_RATE);
	  private static readonly IsdaCreditDiscountFactors YIELD_CURVE;
	  private static readonly LegalEntitySurvivalProbabilities CREDIT_CURVE;
	  private static readonly LegalEntitySurvivalProbabilities CREDIT_CURVE_INDEX;
	  private static readonly CurveName CREDIT_CURVE_NAME = CurveName.of("credit");
	  private static readonly CdsConvention CDS_CONV = ImmutableCdsConvention.builder().businessDayAdjustment(BusinessDayAdjustment.of(FOLLOWING, SAT_SUN)).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).currency(USD).dayCount(ACT_360).name("sat_sun_conv").paymentFrequency(Frequency.P3M).settlementDateOffset(DaysAdjustment.ofBusinessDays(3, SAT_SUN)).build();
	  private static readonly ImmutableList<ResolvedTradeParameterMetadata> CDS_METADATA;
	  private static readonly ImmutableList<ResolvedTradeParameterMetadata> CDS_INDEX_METADATA;
	  static SpreadSensitivityCalculatorTest()
	  {
		double flatRate = 0.05;
		double t = 20.0;
		YIELD_CURVE = IsdaCreditDiscountFactors.of(USD, VALUATION_DATE, CurveName.of("discount"), DoubleArray.of(t), DoubleArray.of(flatRate), ACT_365F);
		ImmutableMarketDataBuilder dataBuilder = ImmutableMarketData.builder(VALUATION_DATE);
		ImmutableList.Builder<CdsIsdaCreditCurveNode> nodesBuilder = ImmutableList.builder();
		ImmutableList.Builder<ResolvedTradeParameterMetadata> cdsMetadataBuilder = ImmutableList.builder();
		ImmutableList.Builder<ResolvedTradeParameterMetadata> cdsIndexMetadataBuilder = ImmutableList.builder();
		for (int i = 0; i < NUM_MARKET_CDS; i++)
		{
		  QuoteId quoteId = QuoteId.of(StandardId.of("OG", PAR_SPD_DATES[i].ToString()));
		  CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofParSpread(DatesCdsTemplate.of(VALUATION_DATE, PAR_SPD_DATES[i], CDS_CONV), quoteId, LEGAL_ENTITY);
		  MARKET_CDS[i] = CdsTrade.builder().product(Cds.of(BUY, LEGAL_ENTITY, USD, NOTIONAL, VALUATION_DATE, PAR_SPD_DATES[i], P3M, SAT_SUN, PAR_SPREADS[i] * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
		  MARKET_CDS_INDEX[i] = CdsIndexTrade.builder().product(CdsIndex.of(BuySell.BUY, INDEX_ID, LEGAL_ENTITIES, USD, NOTIONAL, VALUATION_DATE, PAR_SPD_DATES[i], P3M, SAT_SUN, PAR_SPREADS[i] * ONE_BP)).info(TradeInfo.of(VALUATION_DATE)).build().resolve(REF_DATA);
		  dataBuilder.addValue(quoteId, PAR_SPREADS[i] * ONE_BP);
		  nodesBuilder.add(node);
		  cdsMetadataBuilder.add(ResolvedTradeParameterMetadata.of(MARKET_CDS[i], MARKET_CDS[i].Product.ProtectionEndDate.ToString()));
		  cdsIndexMetadataBuilder.add(ResolvedTradeParameterMetadata.of(MARKET_CDS_INDEX[i], MARKET_CDS_INDEX[i].Product.ProtectionEndDate.ToString()));
		}
		ImmutableMarketData marketData = dataBuilder.build();
		ImmutableList<CdsIsdaCreditCurveNode> nodes = nodesBuilder.build();
		CDS_METADATA = cdsMetadataBuilder.build();
		CDS_INDEX_METADATA = cdsIndexMetadataBuilder.build();
		ImmutableCreditRatesProvider rates = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY, RECOVERY_CURVE)).discountCurves(ImmutableMap.of(USD, YIELD_CURVE)).build();
		IsdaCreditCurveDefinition definition = IsdaCreditCurveDefinition.of(CREDIT_CURVE_NAME, USD, VALUATION_DATE, ACT_365F, nodes, true, true);
		CREDIT_CURVE = BUILDER.calibrate(definition, marketData, rates, REF_DATA);
		NodalCurve underlyingCurve = ((IsdaCreditDiscountFactors) CREDIT_CURVE.SurvivalProbabilities).Curve;
		NodalCurve curveWithFactor = underlyingCurve.withMetadata(underlyingCurve.Metadata.withInfo(CurveInfoType.CDS_INDEX_FACTOR, INDEX_FACTOR).withParameterMetadata(CDS_INDEX_METADATA)); // replace parameter metadata
		CREDIT_CURVE_INDEX = LegalEntitySurvivalProbabilities.of(INDEX_ID, IsdaCreditDiscountFactors.of(USD, VALUATION_DATE, curveWithFactor));
	  }
	  private static readonly CreditRatesProvider RATES_PROVIDER = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY, RECOVERY_CURVE, INDEX_ID, RECOVERY_CURVE_INDEX)).discountCurves(ImmutableMap.of(USD, YIELD_CURVE)).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY, USD), CREDIT_CURVE, Pair.of(INDEX_ID, USD), CREDIT_CURVE_INDEX)).build();
	  private const double TOL = 1.0e-13;

	  public virtual void parellelCs01Test()
	  {
		double fromExcel = 4238.557409;
		CurrencyAmount fd = CS01_FD.parallelCs01(CDS1, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		CurrencyAmount analytic = CS01_AN.parallelCs01(CDS1, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.Amount * ONE_BP, fromExcel, TOL * NOTIONAL);
		assertEquals(analytic.Currency, USD);
		assertEquals(analytic.Amount * ONE_BP, fd.Amount * ONE_BP, ONE_BP * NOTIONAL);
		// equivalence to market quote sensitivity for par spread quote
		PointSensitivities point = PRICER.presentValueOnSettleSensitivity(CDS1, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, LEGAL_ENTITY, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		double cs01FromQuoteSensi = quoteSensi.Sensitivities.get(0).Sensitivity.sum();
		assertEquals(cs01FromQuoteSensi * ONE_BP, analytic.Amount * ONE_BP, TOL * NOTIONAL);
	  }

	  public virtual void parellelCs01FromNodesTest()
	  {
		double fromExcel = 4238.557409;
		CurrencyAmount fd = CS01_FD.parallelCs01(CDS1, RATES_PROVIDER, REF_DATA);
		CurrencyAmount analytic = CS01_AN.parallelCs01(CDS1, RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.Amount * ONE_BP, fromExcel, TOL * NOTIONAL);
		assertEquals(analytic.Currency, USD);
		assertEquals(analytic.Amount * ONE_BP, fd.Amount * ONE_BP, ONE_BP * NOTIONAL);
		// equivalence to market quote sensitivity for par spread quote
		PointSensitivities point = PRICER.presentValueOnSettleSensitivity(CDS1, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, LEGAL_ENTITY, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		double cs01FromQuoteSensi = quoteSensi.Sensitivities.get(0).Sensitivity.sum();
		assertEquals(cs01FromQuoteSensi * ONE_BP, analytic.Amount * ONE_BP, TOL * NOTIONAL);
	  }

	  public virtual void bucketedCs01Test()
	  {
		double[] expectedFd = new double[] {0.02446907003406107, 0.1166137422736746, 0.5196553952424576, 1.4989046391578054, 3.5860718603647483, 4233.77162264947, 0.0};
		CurrencyParameterSensitivity fd = CS01_FD.bucketedCs01(CDS1, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity analytic = CS01_AN.bucketedCs01(CDS1, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(fd.ParameterCount, NUM_MARKET_CDS);
		assertEquals(fd.ParameterMetadata, CDS_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(fd.Sensitivity.multipliedBy(ONE_BP).toArray(), expectedFd, NOTIONAL * TOL));
		assertEquals(analytic.Currency, USD);
		assertEquals(analytic.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(analytic.ParameterCount, NUM_MARKET_CDS);
		assertEquals(analytic.ParameterMetadata, CDS_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(analytic.Sensitivity.toArray(), fd.Sensitivity.toArray(), NOTIONAL * ONE_BP * 10d));
		PointSensitivities point = PRICER.presentValueOnSettleSensitivity(CDS1, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, LEGAL_ENTITY, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		assertTrue(DoubleArrayMath.fuzzyEquals(quoteSensi.Sensitivities.get(0).Sensitivity.toArray(), analytic.Sensitivity.toArray(), NOTIONAL * TOL));
	  }

	  public virtual void bucketedCs01SingleNodeCurveTest()
	  {
		ImmutableCreditRatesProvider ratesProviderNoCredit = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY, RECOVERY_CURVE)).discountCurves(ImmutableMap.of(USD, YIELD_CURVE)).build();
		QuoteId quoteId = QuoteId.of(StandardId.of("OG", END2.ToString()));
		CdsIsdaCreditCurveNode node = CdsIsdaCreditCurveNode.ofParSpread(DatesCdsTemplate.of(START, END2, CDS_CONV), quoteId, LEGAL_ENTITY);
		ImmutableMarketData marketData = ImmutableMarketData.builder(VALUATION_DATE).addValue(quoteId, DEAL_SPREAD * ONE_BP).build();
		IsdaCreditCurveDefinition definition = IsdaCreditCurveDefinition.of(CREDIT_CURVE_NAME, USD, VALUATION_DATE, ACT_365F, ImmutableList.of(node), true, false);
		LegalEntitySurvivalProbabilities creditCurve = BUILDER.calibrate(definition, marketData, ratesProviderNoCredit, REF_DATA);
		ImmutableCreditRatesProvider ratesProvider = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY, RECOVERY_CURVE)).discountCurves(ImmutableMap.of(USD, YIELD_CURVE)).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY, USD), creditCurve)).build();
		double[] expectedFd = new double[] {-6.876275937539589E-4, 1.1832215762730414E-4, 0.0012340982402658796, 0.002784985575488008, 0.005287295115619095, 2429.636217554099, 3101.303324461041};
		CurrencyParameterSensitivity analytic = CS01_AN.bucketedCs01(CDS2, ImmutableList.copyOf(MARKET_CDS), ratesProvider, REF_DATA);
		CurrencyParameterSensitivity fd = CS01_FD.bucketedCs01(CDS2, ImmutableList.copyOf(MARKET_CDS), ratesProvider, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(fd.ParameterCount, NUM_MARKET_CDS);
		assertEquals(fd.ParameterMetadata, CDS_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(fd.Sensitivity.multipliedBy(ONE_BP).toArray(), expectedFd, NOTIONAL * TOL));
		assertEquals(analytic.Currency, USD);
		assertEquals(analytic.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(analytic.ParameterCount, NUM_MARKET_CDS);
		assertEquals(analytic.ParameterMetadata, CDS_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(analytic.Sensitivity.toArray(), fd.Sensitivity.toArray(), NOTIONAL * ONE_BP * 10d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void parellelCs01IndexTest()
	  {
		CurrencyAmount fdSingle = CS01_FD.parallelCs01(CDS2, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		CurrencyAmount analyticSingle = CS01_AN.parallelCs01(CDS2, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		CurrencyAmount fd = CS01_FD.parallelCs01(CDS_INDEX, ImmutableList.copyOf(MARKET_CDS_INDEX), RATES_PROVIDER, REF_DATA);
		CurrencyAmount analytic = CS01_AN.parallelCs01(CDS_INDEX, ImmutableList.copyOf(MARKET_CDS_INDEX), RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.Amount, fdSingle.Amount * INDEX_FACTOR, TOL * NOTIONAL);
		assertEquals(analytic.Amount, analyticSingle.Amount * INDEX_FACTOR, TOL * NOTIONAL);
		assertEquals(analytic.Currency, USD);
		// equivalence to market quote sensitivity for par spread quote
		PointSensitivities point = PRICER_INDEX.presentValueOnSettleSensitivity(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, INDEX_ID, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		double cs01FromQuoteSensi = quoteSensi.Sensitivities.get(0).Sensitivity.sum();
		assertEquals(cs01FromQuoteSensi, analytic.Amount, TOL * NOTIONAL);
	  }

	  public virtual void parellelCs01WithNodesIndexTest()
	  {
		CurrencyAmount fdSingle = CS01_FD.parallelCs01(CDS2, RATES_PROVIDER, REF_DATA);
		CurrencyAmount analyticSingle = CS01_AN.parallelCs01(CDS2, RATES_PROVIDER, REF_DATA);
		CurrencyAmount fd = CS01_FD.parallelCs01(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		CurrencyAmount analytic = CS01_AN.parallelCs01(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.Amount, fdSingle.Amount * INDEX_FACTOR, TOL * NOTIONAL);
		assertEquals(analytic.Amount, analyticSingle.Amount * INDEX_FACTOR, TOL * NOTIONAL);
		assertEquals(analytic.Currency, USD);
		// equivalence to market quote sensitivity for par spread quote
		PointSensitivities point = PRICER_INDEX.presentValueOnSettleSensitivity(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, INDEX_ID, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		double cs01FromQuoteSensi = quoteSensi.Sensitivities.get(0).Sensitivity.sum();
		assertEquals(cs01FromQuoteSensi, analytic.Amount, TOL * NOTIONAL);
	  }

	  public virtual void bucketedCs01IndexTest()
	  {
		CurrencyParameterSensitivity fdSingle = CS01_FD.bucketedCs01(CDS2, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity analyticSingle = CS01_AN.bucketedCs01(CDS2, ImmutableList.copyOf(MARKET_CDS), RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity fd = CS01_FD.bucketedCs01(CDS_INDEX, ImmutableList.copyOf(MARKET_CDS_INDEX), RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity analytic = CS01_AN.bucketedCs01(CDS_INDEX, ImmutableList.copyOf(MARKET_CDS_INDEX), RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(fd.ParameterCount, NUM_MARKET_CDS);
		assertEquals(fd.ParameterMetadata, CDS_INDEX_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(fd.Sensitivity.toArray(), fdSingle.Sensitivity.multipliedBy(INDEX_FACTOR).toArray(), NOTIONAL * TOL));
		assertEquals(analytic.Currency, USD);
		assertEquals(analytic.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(analytic.ParameterCount, NUM_MARKET_CDS);
		assertEquals(analytic.ParameterMetadata, CDS_INDEX_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(analytic.Sensitivity.toArray(), analyticSingle.Sensitivity.multipliedBy(INDEX_FACTOR).toArray(), NOTIONAL * TOL));
		PointSensitivities point = PRICER_INDEX.presentValueOnSettleSensitivity(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, INDEX_ID, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		assertTrue(DoubleArrayMath.fuzzyEquals(quoteSensi.Sensitivities.get(0).Sensitivity.toArray(), analytic.Sensitivity.toArray(), NOTIONAL * TOL));
	  }

	  public virtual void bucketedCs01WithNodesIndexTest()
	  {
		CurrencyParameterSensitivity fdSingle = CS01_FD.bucketedCs01(CDS2, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity analyticSingle = CS01_AN.bucketedCs01(CDS2, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity fd = CS01_FD.bucketedCs01(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity analytic = CS01_AN.bucketedCs01(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		assertEquals(fd.Currency, USD);
		assertEquals(fd.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(fd.ParameterCount, NUM_MARKET_CDS);
		assertEquals(fd.ParameterMetadata, CDS_INDEX_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(fd.Sensitivity.toArray(), fdSingle.Sensitivity.multipliedBy(INDEX_FACTOR).toArray(), NOTIONAL * TOL));
		assertEquals(analytic.Currency, USD);
		assertEquals(analytic.MarketDataName, CurveName.of("impliedSpreads"));
		assertEquals(analytic.ParameterCount, NUM_MARKET_CDS);
		assertEquals(analytic.ParameterMetadata, CDS_INDEX_METADATA);
		assertTrue(DoubleArrayMath.fuzzyEquals(analytic.Sensitivity.toArray(), analyticSingle.Sensitivity.multipliedBy(INDEX_FACTOR).toArray(), NOTIONAL * TOL));
		PointSensitivities point = PRICER_INDEX.presentValueOnSettleSensitivity(CDS_INDEX, RATES_PROVIDER, REF_DATA);
		CurrencyParameterSensitivity paramSensi = RATES_PROVIDER.singleCreditCurveParameterSensitivity(point, INDEX_ID, USD);
		CurrencyParameterSensitivities quoteSensi = QUOTE_CAL.sensitivity(CurrencyParameterSensitivities.of(paramSensi), RATES_PROVIDER);
		assertTrue(DoubleArrayMath.fuzzyEquals(quoteSensi.Sensitivities.get(0).Sensitivity.toArray(), analytic.Sensitivity.toArray(), NOTIONAL * TOL));
	  }

	}

}
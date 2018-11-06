using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using IsdaCreditCurveDefinition = com.opengamma.strata.market.curve.IsdaCreditCurveDefinition;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CdsIndexIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIndexIsdaCreditCurveNode;
	using CdsIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIsdaCreditCurveNode;
	using LegalEntityInformation = com.opengamma.strata.market.observable.LegalEntityInformation;
	using LegalEntityInformationId = com.opengamma.strata.market.observable.LegalEntityInformationId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ResolvedTradeParameterMetadata = com.opengamma.strata.market.param.ResolvedTradeParameterMetadata;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;
	using CdsConvention = com.opengamma.strata.product.credit.type.CdsConvention;
	using CdsTemplate = com.opengamma.strata.product.credit.type.CdsTemplate;
	using ImmutableCdsConvention = com.opengamma.strata.product.credit.type.ImmutableCdsConvention;
	using TenorCdsTemplate = com.opengamma.strata.product.credit.type.TenorCdsTemplate;

	/// <summary>
	/// Test <seealso cref="IsdaCompliantIndexCurveCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IsdaCompliantIndexCurveCalibratorTest
	public class IsdaCompliantIndexCurveCalibratorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VALUATION_DATE = LocalDate.of(2014, 2, 13);
	  private static readonly DoubleArray TIME_YC = DoubleArray.ofUnsafe(new double[] {0.08767123287671233, 0.1726027397260274, 0.2602739726027397, 0.5095890410958904, 1.010958904109589, 2.010958904109589, 3.0136986301369864, 4.0191780821917815, 5.016438356164384, 6.013698630136987, 7.016438356164384, 8.016438356164384, 9.016438356164384, 10.021917808219179, 12.01917808219178, 15.027397260273974, 20.024657534246575, 25.027397260273972, 30.030136986301372});
	  private static readonly DoubleArray RATE_YC = DoubleArray.ofUnsafe(new double[] {0.0015967771993938666, 0.002000101499768777, 0.002363431670279865, 0.003338175293899776, 0.005634608399714134, 0.00440326902435394, 0.007809961130263494, 0.011941089607974827, 0.015908558015433557, 0.019426790989545677, 0.022365655212981644, 0.02480329609280203, 0.02681632723967965, 0.028566047406753222, 0.031343018999443514, 0.03409375145707815, 0.036451406286344155, 0.0374228389649933, 0.037841116301420584});
	  private static readonly DefaultCurveMetadata METADATA_YC = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("yield").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_YC = InterpolatedNodalCurve.of(METADATA_YC, TIME_YC, RATE_YC, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private static readonly IsdaCreditDiscountFactors CURVE_YC = IsdaCreditDiscountFactors.of(EUR, VALUATION_DATE, NODAL_YC);

	  private static readonly BusinessDayAdjustment BUS_ADJ = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
	  private static readonly DaysAdjustment CDS_SETTLE_STD = DaysAdjustment.ofBusinessDays(3, SAT_SUN);
	  private static readonly CdsConvention CONVENTION = ImmutableCdsConvention.of("conv", EUR, ACT_360, Frequency.P3M, BUS_ADJ, CDS_SETTLE_STD);
	  private static readonly StandardId INDEX_ID = StandardId.of("OG", "ABCXX-Series22-Version5");
	  private static readonly ImmutableList<StandardId> LEGAL_ENTITIES;
	  private static readonly ImmutableList<CdsIndexIsdaCreditCurveNode> CURVE_NODES;
	  private static readonly ImmutableList<CdsIndexIsdaCreditCurveNode> CURVE_NODES_PS;
	  private static readonly ImmutableMarketData MARKET_DATA;
	  private static readonly ImmutableMarketData MARKET_DATA_PS;
	  private const int INDEX_SIZE = 97;
	  private const int NUM_PILLARS = 4;
	  private static readonly ImmutableSet<int> DEFAULTED_NAMES = ImmutableSet.of(2, 15, 37, 51);
	  private const double COUPON = 0.05;
	  private const double RECOVERY_RATE_VALUE = 0.3;
	  private static readonly double[] PUF_QUOTES = new double[] {-0.0756, -0.0762, -0.0571, -0.0652};
	  private static readonly double[] PS_QUOTES = new double[] {0.0011, 0.0057, 0.0124, 0.0182};
	  private static readonly Tenor[] INDEX_TENORS = new Tenor[] {Tenor.TENOR_3Y, Tenor.TENOR_5Y, Tenor.TENOR_7Y, Tenor.TENOR_10Y};
	  static IsdaCompliantIndexCurveCalibratorTest()
	  {
		ImmutableList.Builder<StandardId> legalEntityIdsbuilder = ImmutableList.builder();
		ImmutableMarketDataBuilder marketDataBuilder = ImmutableMarketData.builder(VALUATION_DATE);
		ImmutableMarketDataBuilder marketDataPsBuilder = ImmutableMarketData.builder(VALUATION_DATE);
		for (int? i = 0; i.Value < INDEX_SIZE; ++i)
		{
		  StandardId legalEntityId = StandardId.of("OG", "ABC" + i.ToString());
		  LegalEntityInformation information = DEFAULTED_NAMES.contains(i) ? LegalEntityInformation.isDefaulted(legalEntityId) : LegalEntityInformation.isNotDefaulted(legalEntityId);
		  legalEntityIdsbuilder.add(legalEntityId);
		  marketDataBuilder.addValue(LegalEntityInformationId.of(legalEntityId), information);
		  marketDataPsBuilder.addValue(LegalEntityInformationId.of(legalEntityId), information);
		}
		LEGAL_ENTITIES = legalEntityIdsbuilder.build();
		ImmutableList.Builder<CdsIndexIsdaCreditCurveNode> curveNodesBuilder = ImmutableList.builder();
		ImmutableList.Builder<CdsIndexIsdaCreditCurveNode> curveNodesPsBuilder = ImmutableList.builder();
		for (int i = 0; i < NUM_PILLARS; ++i)
		{
		  QuoteId id = QuoteId.of(StandardId.of("OG", INDEX_TENORS[i].ToString()));
		  CdsTemplate temp = TenorCdsTemplate.of(INDEX_TENORS[i], CONVENTION);
		  curveNodesBuilder.add(CdsIndexIsdaCreditCurveNode.ofPointsUpfront(temp, id, INDEX_ID, LEGAL_ENTITIES, COUPON));
		  curveNodesPsBuilder.add(CdsIndexIsdaCreditCurveNode.ofParSpread(temp, id, INDEX_ID, LEGAL_ENTITIES));
		  marketDataBuilder.addValue(id, PUF_QUOTES[i]);
		  marketDataPsBuilder.addValue(id, PS_QUOTES[i]);
		}
		CURVE_NODES = curveNodesBuilder.build();
		MARKET_DATA = marketDataBuilder.build();
		CURVE_NODES_PS = curveNodesPsBuilder.build();
		MARKET_DATA_PS = marketDataPsBuilder.build();
	  }
	  private static readonly ImmutableCreditRatesProvider RATES_PROVIDER = ImmutableCreditRatesProvider.builder().valuationDate(VALUATION_DATE).discountCurves(ImmutableMap.of(EUR, CURVE_YC)).recoveryRateCurves(ImmutableMap.of(INDEX_ID, ConstantRecoveryRates.of(INDEX_ID, VALUATION_DATE, RECOVERY_RATE_VALUE))).build();
	  private static readonly CurveName CURVE_NAME = CurveName.of("test_credit");
	  private static readonly IsdaCompliantIndexCurveCalibrator CALIBRATOR = IsdaCompliantIndexCurveCalibrator.standard();
	  private const double TOL = 1.0e-14;
	  private const double EPS = 1.0e-4;

	  public virtual void test_regression()
	  {
		double[] expectedTimes = new double[] {2.852054794520548, 4.852054794520548, 6.854794520547945, 9.854794520547944};
		double[] expectedRates = new double[] {0.03240798261187516, 0.04858422754375164, 0.0616141083562273, 0.06235460926516589};
		IsdaCreditCurveDefinition curveDefinition = IsdaCreditCurveDefinition.of(CURVE_NAME, EUR, VALUATION_DATE, ACT_365F, CURVE_NODES, true, false);
		LegalEntitySurvivalProbabilities creditCurve = CALIBRATOR.calibrate(curveDefinition, MARKET_DATA, RATES_PROVIDER, REF_DATA);
		NodalCurve curve = (NodalCurve) creditCurve.SurvivalProbabilities.findData(CURVE_NAME).get();
		assertTrue(DoubleArrayMath.fuzzyEquals(curve.XValues.toArray(), expectedTimes, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(curve.YValues.toArray(), expectedRates, TOL));
		assertTrue(curve.getParameterMetadata(0) is DatedParameterMetadata);
		assertTrue(curve.getParameterMetadata(1) is DatedParameterMetadata);
		assertTrue(curve.getParameterMetadata(2) is DatedParameterMetadata);
		assertTrue(curve.getParameterMetadata(3) is DatedParameterMetadata);
		double computedIndex = curve.Metadata.getInfo(CurveInfoType.CDS_INDEX_FACTOR);
		assertEquals(computedIndex, 93.0 / 97.0, TOL);
		testJacobian(creditCurve, RATES_PROVIDER, CURVE_NODES, PUF_QUOTES);
	  }

	  public virtual void test_regression_single()
	  {
		double[] expectedTimes = new double[] {4.852054794520548};
		double[] expectedRates = new double[] {0.04666754810728295};
		ImmutableList<CdsIndexIsdaCreditCurveNode> singleNode = CURVE_NODES.subList(1, 2);
		IsdaCreditCurveDefinition curveDefinition = IsdaCreditCurveDefinition.of(CURVE_NAME, EUR, VALUATION_DATE, ACT_365F, singleNode, true, false);
		LegalEntitySurvivalProbabilities creditCurve = CALIBRATOR.calibrate(curveDefinition, MARKET_DATA, RATES_PROVIDER, REF_DATA);
		NodalCurve curve = (NodalCurve) creditCurve.SurvivalProbabilities.findData(CURVE_NAME).get();
		assertTrue(DoubleArrayMath.fuzzyEquals(curve.XValues.toArray(), expectedTimes, TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(curve.YValues.toArray(), expectedRates, TOL));
		assertTrue(curve.getParameterMetadata(0) is DatedParameterMetadata);
		double computedIndex = curve.Metadata.getInfo(CurveInfoType.CDS_INDEX_FACTOR);
		assertEquals(computedIndex, 93.0 / 97.0, TOL);
		testJacobian(creditCurve, RATES_PROVIDER, singleNode, PUF_QUOTES);
	  }

	  public virtual void test_consistency_singleName()
	  {
		IsdaCreditCurveDefinition curveDefinition = IsdaCreditCurveDefinition.of(CURVE_NAME, EUR, VALUATION_DATE, ACT_365F, CURVE_NODES_PS, true, true);
		LegalEntitySurvivalProbabilities creditCurveComputed = CALIBRATOR.calibrate(curveDefinition, MARKET_DATA_PS, RATES_PROVIDER, REF_DATA);
		NodalCurve curveComputed = (NodalCurve) creditCurveComputed.SurvivalProbabilities.findData(CURVE_NAME).get();
		double computedIndex = curveComputed.Metadata.getInfo(CurveInfoType.CDS_INDEX_FACTOR);
		assertEquals(computedIndex, 93.0 / 97.0, TOL);
		IsdaCompliantCreditCurveCalibrator cdsCalibrator = FastCreditCurveCalibrator.standard();
		IList<CdsIsdaCreditCurveNode> cdsNodes = new List<CdsIsdaCreditCurveNode>();
		for (int i = 0; i < CURVE_NODES_PS.size(); ++i)
		{
		  cdsNodes.Add(CdsIsdaCreditCurveNode.ofParSpread(CURVE_NODES_PS.get(i).Template, CURVE_NODES_PS.get(i).ObservableId, CURVE_NODES_PS.get(i).CdsIndexId));
		  ParameterMetadata metadata = curveComputed.getParameterMetadata(i);
		  assertTrue(metadata is ResolvedTradeParameterMetadata);
		  ResolvedTradeParameterMetadata tradeMetadata = (ResolvedTradeParameterMetadata) metadata;
		  assertTrue(tradeMetadata.Trade is ResolvedCdsIndexTrade);
		}
		IsdaCreditCurveDefinition cdsCurveDefinition = IsdaCreditCurveDefinition.of(CURVE_NAME, EUR, VALUATION_DATE, ACT_365F, cdsNodes, true, false);
		LegalEntitySurvivalProbabilities creditCurveExpected = cdsCalibrator.calibrate(cdsCurveDefinition, MARKET_DATA_PS, RATES_PROVIDER, REF_DATA);
		NodalCurve curveExpected = (NodalCurve) creditCurveExpected.SurvivalProbabilities.findData(CURVE_NAME).get();
		assertTrue(DoubleArrayMath.fuzzyEquals(curveComputed.XValues.toArray(), curveExpected.XValues.toArray(), TOL));
		assertTrue(DoubleArrayMath.fuzzyEquals(curveComputed.YValues.toArray(), curveExpected.YValues.toArray(), TOL));
		assertEquals(curveComputed.Metadata.getInfo(CurveInfoType.JACOBIAN), curveExpected.Metadata.getInfo(CurveInfoType.JACOBIAN));
	  }

	  //-------------------------------------------------------------------------
	  protected internal virtual void testJacobian(LegalEntitySurvivalProbabilities curve, ImmutableCreditRatesProvider ratesProvider, IList<CdsIndexIsdaCreditCurveNode> nodes, double[] quotes)
	  {

		int nNode = nodes.Count;
		IsdaCreditDiscountFactors df = (IsdaCreditDiscountFactors) curve.SurvivalProbabilities;
		int nCurveNode = df.ParameterCount;
		for (int i = 0; i < nCurveNode; ++i)
		{
		  double[] quotesUp = Arrays.copyOf(quotes, nNode);
		  double[] quotesDw = Arrays.copyOf(quotes, nNode);
		  quotesUp[i] += EPS;
		  quotesDw[i] -= EPS;
		  ImmutableMarketDataBuilder builderCreditUp = MARKET_DATA.toBuilder();
		  ImmutableMarketDataBuilder builderCreditDw = MARKET_DATA.toBuilder();
		  for (int j = 0; j < nNode; ++j)
		  {
			builderCreditUp.addValue(nodes[j].ObservableId, quotesUp[j]);
			builderCreditDw.addValue(nodes[j].ObservableId, quotesDw[j]);
		  }
		  ImmutableMarketData marketDataUp = builderCreditUp.build();
		  ImmutableMarketData marketDataDw = builderCreditDw.build();
		  IsdaCreditCurveDefinition definition = IsdaCreditCurveDefinition.of(df.Curve.Name, df.Currency, df.ValuationDate, df.DayCount, nodes, false, false);
		  IsdaCreditDiscountFactors ccUp = (IsdaCreditDiscountFactors) CALIBRATOR.calibrate(definition, marketDataUp, ratesProvider, REF_DATA).SurvivalProbabilities;
		  IsdaCreditDiscountFactors ccDw = (IsdaCreditDiscountFactors) CALIBRATOR.calibrate(definition, marketDataDw, ratesProvider, REF_DATA).SurvivalProbabilities;
		  for (int j = 0; j < nNode; ++j)
		  {
			double computed = df.Curve.Metadata.findInfo(CurveInfoType.JACOBIAN).get().JacobianMatrix.get(j, i);
			double expected = 0.5 * (ccUp.Curve.YValues.get(j) - ccDw.Curve.YValues.get(j)) / EPS;
			assertEquals(computed, expected, EPS * 10d);
		  }
		}
	  }

	}

}
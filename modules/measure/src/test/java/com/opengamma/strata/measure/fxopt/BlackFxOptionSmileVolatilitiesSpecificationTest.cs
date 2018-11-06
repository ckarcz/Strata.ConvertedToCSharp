/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.BLACK_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.RISK_REVERSAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.STRANGLE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PCHIP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ValueType = com.opengamma.strata.market.ValueType;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using BlackFxOptionSmileVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSmileVolatilities;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;
	using InterpolatedStrikeSmileDeltaTermStructure = com.opengamma.strata.pricer.fxopt.InterpolatedStrikeSmileDeltaTermStructure;
	using SmileDeltaTermStructure = com.opengamma.strata.pricer.fxopt.SmileDeltaTermStructure;

	/// <summary>
	/// Test <seealso cref="BlackFxOptionSmileVolatilitiesSpecification"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxOptionSmileVolatilitiesSpecificationTest
	public class BlackFxOptionSmileVolatilitiesSpecificationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxOptionVolatilitiesName VOL_NAME = FxOptionVolatilitiesName.of("test");
	  private static readonly CurrencyPair EUR_GBP = CurrencyPair.of(EUR, GBP);
	  private static readonly HolidayCalendarId TA_LO = HolidayCalendarIds.EUTA.combinedWith(HolidayCalendarIds.GBLO);
	  private static readonly DaysAdjustment SPOT_OFFSET = DaysAdjustment.ofBusinessDays(2, TA_LO);
	  private static readonly BusinessDayAdjustment BUS_ADJ = BusinessDayAdjustment.of(FOLLOWING, TA_LO);

	  private static readonly ImmutableList<Tenor> TENORS = ImmutableList.of(Tenor.TENOR_1Y, Tenor.TENOR_1Y, Tenor.TENOR_1Y, Tenor.TENOR_3M, Tenor.TENOR_3M, Tenor.TENOR_3M);
	  private static readonly ImmutableList<double> DELTAS = ImmutableList.of(0.1, 0.1, 0.5, 0.5, 0.1, 0.1);
	  private static readonly ImmutableList<ValueType> QUOTE_TYPE = ImmutableList.of(STRANGLE, RISK_REVERSAL, BLACK_VOLATILITY, BLACK_VOLATILITY, STRANGLE, RISK_REVERSAL);
	  private static readonly ImmutableList<FxOptionVolatilitiesNode> NODES;
	  private static readonly ImmutableList<QuoteId> QUOTE_IDS;
	  static BlackFxOptionSmileVolatilitiesSpecificationTest()
	  {
		ImmutableList.Builder<FxOptionVolatilitiesNode> builder = ImmutableList.builder();
		ImmutableList.Builder<QuoteId> quoteBuilder = ImmutableList.builder();
		for (int i = 0; i < TENORS.size(); ++i)
		{
		  QuoteId id = QuoteId.of(StandardId.of("OG", TENORS.get(i).ToString() + "_" + DELTAS.get(i).ToString() + "_" + QUOTE_TYPE.get(i).ToString()));
		  builder.add(FxOptionVolatilitiesNode.of(EUR_GBP, SPOT_OFFSET, BUS_ADJ, QUOTE_TYPE.get(i), id, TENORS.get(i), DeltaStrike.of(DELTAS.get(i))));
		  quoteBuilder.add(id);
		}
		NODES = builder.build();
		QUOTE_IDS = quoteBuilder.build();
	  }

	  public virtual void test_builder()
	  {
		BlackFxOptionSmileVolatilitiesSpecification test = BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(EUR_GBP).dayCount(ACT_360).nodes(NODES).timeInterpolator(PCHIP).timeExtrapolatorLeft(LINEAR).timeExtrapolatorRight(FLAT).strikeInterpolator(DOUBLE_QUADRATIC).strikeExtrapolatorLeft(FLAT).strikeExtrapolatorRight(LINEAR).build();
		assertEquals(test.CurrencyPair, EUR_GBP);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.Name, VOL_NAME);
		assertEquals(test.Nodes, NODES);
		assertEquals(test.ParameterCount, TENORS.size());
		assertEquals(test.StrikeInterpolator, DOUBLE_QUADRATIC);
		assertEquals(test.StrikeExtrapolatorLeft, FLAT);
		assertEquals(test.StrikeExtrapolatorRight, LINEAR);
		assertEquals(test.TimeInterpolator, PCHIP);
		assertEquals(test.TimeExtrapolatorLeft, LINEAR);
		assertEquals(test.TimeExtrapolatorRight, FLAT);
		assertEquals(test.volatilitiesInputs(), QUOTE_IDS);
	  }

	  public virtual void test_volatilities()
	  {
		BlackFxOptionSmileVolatilitiesSpecification @base = BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(EUR_GBP).dayCount(ACT_360).nodes(NODES).timeInterpolator(PCHIP).strikeInterpolator(PCHIP).build();
		LocalDate date = LocalDate.of(2017, 9, 25);
		ZonedDateTime dateTime = date.atStartOfDay().atZone(ZoneId.of("Europe/London"));
		DoubleArray parameters = DoubleArray.of(0.05, -0.05, 0.15, 0.25, 0.1, -0.1);
		BlackFxOptionSmileVolatilities computed = @base.volatilities(dateTime, parameters, REF_DATA);
		LocalDate spotDate = SPOT_OFFSET.adjust(dateTime.toLocalDate(), REF_DATA);
		DaysAdjustment expOffset = DaysAdjustment.ofBusinessDays(-2, TA_LO);
		DoubleArray expiries = DoubleArray.of(ACT_360.relativeYearFraction(date, expOffset.adjust(BUS_ADJ.adjust(spotDate.plus(Tenor.TENOR_3M), REF_DATA), REF_DATA)), ACT_360.relativeYearFraction(date, expOffset.adjust(BUS_ADJ.adjust(spotDate.plus(Tenor.TENOR_1Y), REF_DATA), REF_DATA)));
		SmileDeltaTermStructure smiles = InterpolatedStrikeSmileDeltaTermStructure.of(expiries, DoubleArray.of(0.1), DoubleArray.of(0.25, 0.15), DoubleMatrix.ofUnsafe(new double[][]
		{
			new double[] {-0.1},
			new double[] {-0.05}
		}), DoubleMatrix.ofUnsafe(new double[][]
		{
			new double[] {0.1},
			new double[] {0.05}
		}), ACT_360, PCHIP, FLAT, FLAT, PCHIP, FLAT, FLAT);
		BlackFxOptionSmileVolatilities expected = BlackFxOptionSmileVolatilities.of(VOL_NAME, EUR_GBP, dateTime, smiles);
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BlackFxOptionSmileVolatilitiesSpecification test1 = BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(EUR_GBP).dayCount(ACT_360).nodes(NODES).timeInterpolator(PCHIP).timeExtrapolatorLeft(LINEAR).timeExtrapolatorRight(LINEAR).strikeInterpolator(PCHIP).strikeExtrapolatorLeft(LINEAR).strikeExtrapolatorRight(LINEAR).build();
		coverImmutableBean(test1);
		CurrencyPair eurUsd = CurrencyPair.of(EUR, USD);
		ImmutableList.Builder<FxOptionVolatilitiesNode> builder = ImmutableList.builder();
		for (int i = 0; i < TENORS.size(); ++i)
		{
		  QuoteId id = QuoteId.of(StandardId.of("OG", TENORS.get(i).ToString() + "_" + DELTAS.get(i).ToString() + "_" + QUOTE_TYPE.get(i).ToString()));
		  builder.add(FxOptionVolatilitiesNode.of(eurUsd, DaysAdjustment.NONE, BusinessDayAdjustment.NONE, QUOTE_TYPE.get(i), id, TENORS.get(i), DeltaStrike.of(DELTAS.get(i))));
		}
		BlackFxOptionSmileVolatilitiesSpecification test2 = BlackFxOptionSmileVolatilitiesSpecification.builder().name(FxOptionVolatilitiesName.of("other")).currencyPair(eurUsd).dayCount(ACT_365F).nodes(builder.build()).timeInterpolator(DOUBLE_QUADRATIC).strikeInterpolator(DOUBLE_QUADRATIC).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void serialization()
	  {
		BlackFxOptionSmileVolatilitiesSpecification test = BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(EUR_GBP).dayCount(ACT_360).nodes(NODES).timeInterpolator(PCHIP).timeExtrapolatorLeft(LINEAR).timeExtrapolatorRight(FLAT).strikeInterpolator(DOUBLE_QUADRATIC).strikeExtrapolatorLeft(FLAT).strikeExtrapolatorRight(LINEAR).build();
		assertSerialization(test);
	  }

	}

}
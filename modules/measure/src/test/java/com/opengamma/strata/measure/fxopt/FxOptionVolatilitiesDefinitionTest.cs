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
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
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
	using ValueType = com.opengamma.strata.market.ValueType;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;

	/// <summary>
	/// Test <seealso cref="FxOptionVolatilitiesDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionVolatilitiesDefinitionTest
	public class FxOptionVolatilitiesDefinitionTest
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
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") private static final com.google.common.collect.ImmutableList<com.opengamma.strata.market.observable.QuoteId> QUOTE_IDS;
	  private static readonly ImmutableList<QuoteId> QUOTE_IDS;
	  static FxOptionVolatilitiesDefinitionTest()
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
	  private static readonly BlackFxOptionSmileVolatilitiesSpecification SPEC = BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(EUR_GBP).currencyPair(EUR_GBP).dayCount(ACT_365F).nodes(NODES).timeInterpolator(LINEAR).strikeInterpolator(LINEAR).build();

	  public virtual void test_of()
	  {
		FxOptionVolatilitiesDefinition test = FxOptionVolatilitiesDefinition.of(SPEC);
		assertEquals(test.Specification, SPEC);
		assertEquals(test.ParameterCount, SPEC.ParameterCount);
		assertEquals(test.volatilitiesInputs(), SPEC.volatilitiesInputs());
		ZonedDateTime dateTime = LocalDate.of(2017, 9, 25).atStartOfDay().atZone(ZoneId.of("Europe/London"));
		DoubleArray parameters = DoubleArray.of(0.05, -0.05, 0.15, 0.25, 0.1, -0.1);
		assertEquals(test.volatilities(dateTime, parameters, REF_DATA), SPEC.volatilities(dateTime, parameters, REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxOptionVolatilitiesDefinition test1 = FxOptionVolatilitiesDefinition.of(SPEC);
		coverImmutableBean(test1);
		BlackFxOptionSmileVolatilitiesSpecification spec2 = BlackFxOptionSmileVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(EUR_GBP).dayCount(ACT_360).nodes(NODES).timeInterpolator(LINEAR).strikeInterpolator(LINEAR).build();
		FxOptionVolatilitiesDefinition test2 = FxOptionVolatilitiesDefinition.of(spec2);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void serialization()
	  {
		FxOptionVolatilitiesDefinition test = FxOptionVolatilitiesDefinition.of(SPEC);
		assertSerialization(test);
	  }

	}

}
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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ValueType = com.opengamma.strata.market.ValueType;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using Strike = com.opengamma.strata.market.option.Strike;

	/// <summary>
	/// Test <seealso cref="FxOptionVolatilitiesNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionVolatilitiesNodeTest
	public class FxOptionVolatilitiesNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyPair EUR_GBP = CurrencyPair.of(EUR, GBP);
	  private static readonly CurrencyPair GBP_USD = CurrencyPair.of(GBP, USD);
	  private static readonly string LABEL = "LABEL";
	  private static readonly HolidayCalendarId LO_TA = GBLO.combinedWith(EUTA);
	  private static readonly HolidayCalendarId LO_NY = GBLO.combinedWith(USNY);
	  private static readonly DaysAdjustment SPOT_DATE_OFFSET = DaysAdjustment.ofBusinessDays(2, LO_TA);
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, LO_TA);
	  private static readonly DaysAdjustment EXPIRY_DATE_OFFSET = DaysAdjustment.ofBusinessDays(-3, LO_TA);
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(StandardId.of("OG", "TEST"));
	  private static readonly Strike STRIKE = SimpleStrike.of(0.95);

	  public virtual void test_builder()
	  {
		FxOptionVolatilitiesNode test = FxOptionVolatilitiesNode.builder().currencyPair(EUR_GBP).label(LABEL).spotDateOffset(SPOT_DATE_OFFSET).businessDayAdjustment(BDA).expiryDateOffset(EXPIRY_DATE_OFFSET).quoteValueType(ValueType.BLACK_VOLATILITY).quoteId(QUOTE_ID).tenor(Tenor.TENOR_3M).strike(STRIKE).build();
		assertEquals(test.BusinessDayAdjustment, BDA);
		assertEquals(test.CurrencyPair, EUR_GBP);
		assertEquals(test.Label, LABEL);
		assertEquals(test.QuoteValueType, ValueType.BLACK_VOLATILITY);
		assertEquals(test.SpotDateOffset, SPOT_DATE_OFFSET);
		assertEquals(test.ExpiryDateOffset, EXPIRY_DATE_OFFSET);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.Tenor, Tenor.TENOR_3M);
	  }

	  public virtual void test_builder_noExp()
	  {
		FxOptionVolatilitiesNode test = FxOptionVolatilitiesNode.builder().currencyPair(EUR_GBP).label(LABEL).spotDateOffset(SPOT_DATE_OFFSET).businessDayAdjustment(BDA).quoteValueType(ValueType.BLACK_VOLATILITY).quoteId(QUOTE_ID).tenor(Tenor.TENOR_3M).strike(STRIKE).build();
		assertEquals(test.BusinessDayAdjustment, BDA);
		assertEquals(test.CurrencyPair, EUR_GBP);
		assertEquals(test.Label, LABEL);
		assertEquals(test.QuoteValueType, ValueType.BLACK_VOLATILITY);
		assertEquals(test.SpotDateOffset, SPOT_DATE_OFFSET);
		assertEquals(test.ExpiryDateOffset, DaysAdjustment.ofBusinessDays(-2, LO_TA));
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.Tenor, Tenor.TENOR_3M);
	  }

	  public virtual void test_of()
	  {
		FxOptionVolatilitiesNode test = FxOptionVolatilitiesNode.of(EUR_GBP, SPOT_DATE_OFFSET, BDA, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_3M, STRIKE);
		assertEquals(test.BusinessDayAdjustment, BDA);
		assertEquals(test.CurrencyPair, EUR_GBP);
		assertEquals(test.Label, QUOTE_ID.ToString());
		assertEquals(test.QuoteValueType, ValueType.BLACK_VOLATILITY);
		assertEquals(test.SpotDateOffset, SPOT_DATE_OFFSET);
		assertEquals(test.ExpiryDateOffset, DaysAdjustment.ofBusinessDays(-2, LO_TA));
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.Tenor, Tenor.TENOR_3M);
	  }

	  public virtual void test_expiry()
	  {
		FxOptionVolatilitiesNode test = FxOptionVolatilitiesNode.of(EUR_GBP, SPOT_DATE_OFFSET, BDA, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_3M, STRIKE);
		ZonedDateTime dateTime = LocalDate.of(2016, 1, 23).atStartOfDay(ZoneId.of("Europe/London"));
		DaysAdjustment expAdj = DaysAdjustment.ofBusinessDays(-2, LO_TA);
		double computed = test.timeToExpiry(dateTime, ACT_365F, REF_DATA);
		double expected = ACT_365F.relativeYearFraction(dateTime.toLocalDate(), expAdj.adjust(BDA.adjust(SPOT_DATE_OFFSET.adjust(dateTime.toLocalDate(), REF_DATA).plus(Tenor.TENOR_3M), REF_DATA), REF_DATA));
		assertEquals(computed, expected);
	  }

	  public virtual void test_expiry_standard()
	  {
		DaysAdjustment spotLag = DaysAdjustment.ofBusinessDays(2, LO_NY);
		BusinessDayAdjustment bda = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, LO_NY);
		FxOptionVolatilitiesNode[] nodes = new FxOptionVolatilitiesNode[] {FxOptionVolatilitiesNode.of(GBP_USD, spotLag, bda, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_2M, STRIKE), FxOptionVolatilitiesNode.of(GBP_USD, spotLag, bda, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_10M, STRIKE), FxOptionVolatilitiesNode.of(GBP_USD, spotLag, bda, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_4M, STRIKE)};
		ZonedDateTime[] valDates = new ZonedDateTime[] {LocalDate.of(2017, 10, 25).atStartOfDay(ZoneId.of("Europe/London")), LocalDate.of(2017, 10, 25).atStartOfDay(ZoneId.of("Europe/London")), LocalDate.of(2017, 10, 27).atStartOfDay(ZoneId.of("Europe/London"))};
		LocalDate[] expDates = new LocalDate[] {LocalDate.of(2017, 12, 21), LocalDate.of(2018, 8, 23), LocalDate.of(2018, 2, 26)};
		for (int i = 0; i < expDates.Length; ++i)
		{
		  double computed = nodes[i].timeToExpiry(valDates[i], ACT_365F, REF_DATA);
		  double expected = ACT_365F.relativeYearFraction(valDates[i].toLocalDate(), expDates[i]);
		  assertEquals(computed, expected);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxOptionVolatilitiesNode test1 = FxOptionVolatilitiesNode.of(EUR_GBP, SPOT_DATE_OFFSET, BDA, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_3M, STRIKE);
		coverImmutableBean(test1);
		FxOptionVolatilitiesNode test2 = FxOptionVolatilitiesNode.of(CurrencyPair.of(GBP, USD), DaysAdjustment.NONE, BusinessDayAdjustment.NONE, ValueType.RISK_REVERSAL, QuoteId.of(StandardId.of("OG", "foo")), Tenor.TENOR_6M, DeltaStrike.of(0.1));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void serialization()
	  {
		FxOptionVolatilitiesNode test = FxOptionVolatilitiesNode.of(EUR_GBP, SPOT_DATE_OFFSET, BDA, ValueType.BLACK_VOLATILITY, QUOTE_ID, Tenor.TENOR_3M, STRIKE);
		assertSerialization(test);
	  }

	}

}
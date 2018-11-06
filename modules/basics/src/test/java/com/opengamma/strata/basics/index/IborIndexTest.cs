/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CZK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.DKK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.HKD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.HUF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.KRW;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.MXN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.NZD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.PLN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.SEK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.SGD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.ZAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING_BI_MONTHLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.AUSY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.CZPR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.DKCO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.HUBU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.MXMC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.PLWA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SEST;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.ZAJO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_13W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_4M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_4W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_5M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using PeriodAdditionConventions = com.opengamma.strata.basics.date.PeriodAdditionConventions;
	using TenorAdjustment = com.opengamma.strata.basics.date.TenorAdjustment;

	/// <summary>
	/// Test Ibor Index.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborIndexTest
	public class IborIndexTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly HolidayCalendarId NZBD = HolidayCalendarId.of("NZBD"); // no constant for this

	  public virtual void test_gbpLibor3m()
	  {
		IborIndex test = IborIndex.of("GBP-LIBOR-3M");
		assertEquals(test.Name, "GBP-LIBOR-3M");
		assertEquals(test.Currency, GBP);
		assertEquals(test.Active, true);
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, GBLO);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(PRECEDING, GBLO)));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(FOLLOWING, GBLO)));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.FloatingRateName, FloatingRateName.of("GBP-LIBOR"));
		assertEquals(test.ToString(), "GBP-LIBOR-3M");
	  }

	  public virtual void test_gbpLibor3m_dates()
	  {
		IborIndex test = IborIndex.of("GBP-LIBOR-3M");
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 13), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 13), REF_DATA), date(2015, 1, 13));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 13), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 13), REF_DATA), date(2015, 1, 13));
		// weekend
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2015, 1, 12));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 10), REF_DATA), date(2015, 1, 12));
		// input date is Sunday
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2015, 1, 13));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2015, 1, 13));
		// fixing time and zone
		assertEquals(test.calculateFixingDateTime(date(2014, 10, 13)), date(2014, 10, 13).atTime(LocalTime.of(11, 0)).atZone(ZoneId.of("Europe/London")));
		// resolve
		assertEquals(test.resolve(REF_DATA).apply(date(2014, 10, 13)), IborIndexObservation.of(test, date(2014, 10, 13), REF_DATA));
	  }

	  public virtual void test_getFloatingRateName()
	  {
		foreach (IborIndex index in IborIndex.extendedEnum().lookupAll().values())
		{
		  string name = index.Name.Substring(0, index.Name.LastIndexOf('-'));
		  assertEquals(index.FloatingRateName, FloatingRateName.of(name));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_usdLibor3m()
	  {
		IborIndex test = IborIndex.of("USD-LIBOR-3M");
		assertEquals(test.Currency, USD);
		assertEquals(test.Name, "USD-LIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, GBLO);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, GBLO));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, GBLO, BusinessDayAdjustment.of(FOLLOWING, GBLO.combinedWith(USNY))));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO.combinedWith(USNY))));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.FloatingRateName, FloatingRateName.of("USD-LIBOR"));
		assertEquals(test.ToString(), "USD-LIBOR-3M");
	  }

	  public virtual void test_usdLibor3m_dates()
	  {
		IborIndex test = IborIndex.of("USD-LIBOR-3M");
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 29));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 27), REF_DATA), date(2015, 1, 29));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 29), REF_DATA), date(2014, 10, 27));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 29), REF_DATA), date(2015, 1, 29));
		// weekend
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2015, 1, 14));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 14), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 14), REF_DATA), date(2015, 1, 14));
		// effective date is US holiday
		assertEquals(test.calculateEffectiveFromFixing(date(2015, 1, 16), REF_DATA), date(2015, 1, 20));
		assertEquals(test.calculateMaturityFromFixing(date(2015, 1, 16), REF_DATA), date(2015, 4, 20));
		assertEquals(test.calculateFixingFromEffective(date(2015, 1, 20), REF_DATA), date(2015, 1, 16));
		assertEquals(test.calculateMaturityFromEffective(date(2015, 1, 20), REF_DATA), date(2015, 4, 20));
		// input date is Sunday, 13th is US holiday, but not UK holiday (can fix, but not be effective)
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2015, 1, 15));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2015, 1, 14));
		// fixing time and zone
		assertEquals(test.calculateFixingDateTime(date(2014, 10, 13)), date(2014, 10, 13).atTime(LocalTime.of(11, 0)).atZone(ZoneId.of("Europe/London")));
		// resolve
		assertEquals(test.resolve(REF_DATA).apply(date(2014, 10, 27)), IborIndexObservation.of(test, date(2014, 10, 27), REF_DATA));
	  }

	  public virtual void test_euribor3m()
	  {
		IborIndex test = IborIndex.of("EUR-EURIBOR-3M");
		assertEquals(test.Currency, EUR);
		assertEquals(test.Name, "EUR-EURIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, EUTA);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, EUTA));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, EUTA));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, THIRTY_U_360);
		assertEquals(test.FloatingRateName, FloatingRateName.of("EUR-EURIBOR"));
		assertEquals(test.ToString(), "EUR-EURIBOR-3M");
	  }

	  public virtual void test_euribor3m_dates()
	  {
		IborIndex test = IborIndex.of("EUR-EURIBOR-3M");
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 29));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 27), REF_DATA), date(2015, 1, 29));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 29), REF_DATA), date(2014, 10, 27));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 29), REF_DATA), date(2015, 1, 29));
		// weekend
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2015, 1, 14));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 14), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 14), REF_DATA), date(2015, 1, 14));
		// input date is Sunday
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2015, 1, 15));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 9));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2015, 1, 13));
		// fixing time and zone
		assertEquals(test.calculateFixingDateTime(date(2014, 10, 13)), date(2014, 10, 13).atTime(LocalTime.of(11, 0)).atZone(ZoneId.of("Europe/Brussels")));
	  }

	  public virtual void test_tibor_japan3m()
	  {
		IborIndex test = IborIndex.of("JPY-TIBOR-JAPAN-3M");
		assertEquals(test.Currency, JPY);
		assertEquals(test.Name, "JPY-TIBOR-JAPAN-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, JPTO);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, JPTO));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, JPTO));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, JPTO)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.FloatingRateName, FloatingRateName.of("JPY-TIBOR-JAPAN"));
		assertEquals(test.ToString(), "JPY-TIBOR-JAPAN-3M");
	  }

	  public virtual void test_tibor_japan3m_dates()
	  {
		IborIndex test = IborIndex.of("JPY-TIBOR-JAPAN-3M");
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 29));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 27), REF_DATA), date(2015, 1, 29));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 29), REF_DATA), date(2014, 10, 27));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 29), REF_DATA), date(2015, 1, 29));
		// weekend
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2015, 1, 15));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 15), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 15), REF_DATA), date(2015, 1, 15));
		// input date is Sunday
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 16));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2015, 1, 16));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 9));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2015, 1, 14));
		// fixing time and zone
		assertEquals(test.calculateFixingDateTime(date(2014, 10, 13)), date(2014, 10, 13).atTime(LocalTime.of(11, 50)).atZone(ZoneId.of("Asia/Tokyo")));
	  }

	  public virtual void test_tibor_euroyen3m()
	  {
		IborIndex test = IborIndex.of("JPY-TIBOR-EUROYEN-3M");
		assertEquals(test.Currency, JPY);
		assertEquals(test.Name, "JPY-TIBOR-EUROYEN-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, JPTO);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, JPTO));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, JPTO));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, JPTO)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.FloatingRateName, FloatingRateName.of("JPY-TIBOR-EUROYEN"));
		assertEquals(test.ToString(), "JPY-TIBOR-EUROYEN-3M");
	  }

	  public virtual void test_tibor_euroyen3m_dates()
	  {
		IborIndex test = IborIndex.of("JPY-TIBOR-EUROYEN-3M");
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 29));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 27), REF_DATA), date(2015, 1, 29));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 29), REF_DATA), date(2014, 10, 27));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 29), REF_DATA), date(2015, 1, 29));
		// weekend
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2015, 1, 15));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 15), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 15), REF_DATA), date(2015, 1, 15));
		// input date is Sunday
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 16));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2015, 1, 16));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 9));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2015, 1, 14));
		// fixing time and zone
		assertEquals(test.calculateFixingDateTime(date(2014, 10, 13)), date(2014, 10, 13).atTime(LocalTime.of(11, 50)).atZone(ZoneId.of("Asia/Tokyo")));
	  }

	  public virtual void test_usdLibor_all()
	  {
		assertEquals(IborIndex.of("USD-LIBOR-1W").Name, "USD-LIBOR-1W");
		assertEquals(IborIndex.of("USD-LIBOR-1W"), IborIndices.USD_LIBOR_1W);
		assertEquals(IborIndex.of("USD-LIBOR-1M").Name, "USD-LIBOR-1M");
		assertEquals(IborIndex.of("USD-LIBOR-1M"), IborIndices.USD_LIBOR_1M);
		assertEquals(IborIndex.of("USD-LIBOR-2M").Name, "USD-LIBOR-2M");
		assertEquals(IborIndex.of("USD-LIBOR-2M"), IborIndices.USD_LIBOR_2M);
		assertEquals(IborIndex.of("USD-LIBOR-3M").Name, "USD-LIBOR-3M");
		assertEquals(IborIndex.of("USD-LIBOR-3M"), IborIndices.USD_LIBOR_3M);
		assertEquals(IborIndex.of("USD-LIBOR-6M").Name, "USD-LIBOR-6M");
		assertEquals(IborIndex.of("USD-LIBOR-6M"), IborIndices.USD_LIBOR_6M);
		assertEquals(IborIndex.of("USD-LIBOR-12M").Name, "USD-LIBOR-12M");
		assertEquals(IborIndex.of("USD-LIBOR-12M"), IborIndices.USD_LIBOR_12M);
	  }

	  public virtual void test_bbsw1m()
	  {
		IborIndex test = IborIndex.of("AUD-BBSW-1M");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Name, "AUD-BBSW-1M");
		assertEquals(test.Tenor, TENOR_1M);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, AUSY));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, AUSY));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_1M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING_BI_MONTHLY, AUSY)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-BBSW-1M");
	  }

	  public virtual void test_bbsw2m()
	  {
		IborIndex test = IborIndex.of("AUD-BBSW-2M");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Name, "AUD-BBSW-2M");
		assertEquals(test.Tenor, TENOR_2M);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, AUSY));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, AUSY));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_2M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING_BI_MONTHLY, AUSY)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-BBSW-2M");
	  }

	  public virtual void test_bbsw3m()
	  {
		IborIndex test = IborIndex.of("AUD-BBSW-3M");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Name, "AUD-BBSW-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, AUSY));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, AUSY));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING_BI_MONTHLY, AUSY)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-BBSW-3M");
	  }

	  public virtual void test_bbsw4m()
	  {
		IborIndex test = IborIndex.of("AUD-BBSW-4M");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Name, "AUD-BBSW-4M");
		assertEquals(test.Tenor, TENOR_4M);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, AUSY));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, AUSY));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_4M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING_BI_MONTHLY, AUSY)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-BBSW-4M");
	  }

	  public virtual void test_bbsw5m()
	  {
		IborIndex test = IborIndex.of("AUD-BBSW-5M");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Name, "AUD-BBSW-5M");
		assertEquals(test.Tenor, TENOR_5M);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, AUSY));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, AUSY));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_5M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING_BI_MONTHLY, AUSY)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-BBSW-5M");
	  }

	  public virtual void test_bbsw6m()
	  {
		IborIndex test = IborIndex.of("AUD-BBSW-6M");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Name, "AUD-BBSW-6M");
		assertEquals(test.Tenor, TENOR_6M);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, AUSY));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, AUSY));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_6M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING_BI_MONTHLY, AUSY)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-BBSW-6M");
	  }

	  public virtual void test_czk_pribor()
	  {
		IborIndex test = IborIndex.of("CZK-PRIBOR-3M");
		assertEquals(test.Currency, CZK);
		assertEquals(test.Name, "CZK-PRIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, CZPR);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, CZPR));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, CZPR));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CZPR)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.ToString(), "CZK-PRIBOR-3M");
	  }

	  public virtual void test_dkk_cibor()
	  {
		IborIndex test = IborIndex.of("DKK-CIBOR-3M");
		assertEquals(test.Currency, DKK);
		assertEquals(test.Name, "DKK-CIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, DKCO);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, DKCO));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, DKCO));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, DKCO)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, THIRTY_U_360);
		assertEquals(test.ToString(), "DKK-CIBOR-3M");
	  }

	  public virtual void test_hkd_hibor()
	  {
		HolidayCalendarId HKHK = HolidayCalendarId.of("HKHK");
		IborIndex test = IborIndex.of("HKD-HIBOR-3M");
		assertEquals(test.Currency, HKD);
		assertEquals(test.Name, "HKD-HIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, HKHK);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, HKHK));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, HKHK));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HKHK)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "HKD-HIBOR-3M");
	  }

	  public virtual void test_huf_bubor()
	  {
		IborIndex test = IborIndex.of("HUF-BUBOR-3M");
		assertEquals(test.Currency, HUF);
		assertEquals(test.Name, "HUF-BUBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, HUBU);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, HUBU));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, HUBU));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, HUBU)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "HUF-BUBOR-3M");
	  }

	  public virtual void test_krw_cd()
	  {
		HolidayCalendarId KRSE = HolidayCalendarId.of("KRSE");
		IborIndex test = IborIndex.of("KRW-CD-13W");
		assertEquals(test.Currency, KRW);
		assertEquals(test.Name, "KRW-CD-13W");
		assertEquals(test.Tenor, TENOR_13W);
		assertEquals(test.FixingCalendar, KRSE);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, KRSE));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, KRSE));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_13W, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(FOLLOWING, KRSE)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "KRW-CD-13W");

		IborIndex test2 = IborIndex.of("KRW-CD-3M");
		assertEquals(test2.Name, "KRW-CD-13W");
	  }

	  public virtual void test_mxn_tiie()
	  {
		IborIndex test = IborIndex.of("MXN-TIIE-4W");
		assertEquals(test.Currency, MXN);
		assertEquals(test.Name, "MXN-TIIE-4W");
		assertEquals(test.Tenor, TENOR_4W);
		assertEquals(test.FixingCalendar, MXMC);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-1, MXMC));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(1, MXMC));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_4W, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(FOLLOWING, MXMC)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.ToString(), "MXN-TIIE-4W");
	  }

	  public virtual void test_nzd_bkbm()
	  {
		IborIndex test = IborIndex.of("NZD-BKBM-3M");
		assertEquals(test.Currency, NZD);
		assertEquals(test.Name, "NZD-BKBM-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, NZBD);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(PRECEDING, NZBD)));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(FOLLOWING, NZBD)));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, NZBD)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "NZD-BKBM-3M");
	  }

	  public virtual void test_pln_wibor()
	  {
		IborIndex test = IborIndex.of("PLN-WIBOR-3M");
		assertEquals(test.Currency, PLN);
		assertEquals(test.Name, "PLN-WIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, PLWA);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, PLWA));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, PLWA));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, PLWA)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_ACT_ISDA);
		assertEquals(test.ToString(), "PLN-WIBOR-3M");
	  }

	  public virtual void test_sek_stibor()
	  {
		IborIndex test = IborIndex.of("SEK-STIBOR-3M");
		assertEquals(test.Currency, SEK);
		assertEquals(test.Name, "SEK-STIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, SEST);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, SEST));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, SEST));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SEST)));
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, THIRTY_U_360);
		assertEquals(test.ToString(), "SEK-STIBOR-3M");
	  }

	  public virtual void test_sgd_sibor()
	  {
		HolidayCalendarId SGSI = HolidayCalendarId.of("SGSI");
		IborIndex test = IborIndex.of("SGD-SIBOR-3M");
		assertEquals(test.Currency, SGD);
		assertEquals(test.Name, "SGD-SIBOR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, SGSI);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, SGSI));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofBusinessDays(2, SGSI));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SGSI)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "SGD-SIBOR-3M");
	  }

	  public virtual void test_zar_jibar()
	  {
		IborIndex test = IborIndex.of("ZAR-JIBAR-3M");
		assertEquals(test.Currency, ZAR);
		assertEquals(test.Name, "ZAR-JIBAR-3M");
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.FixingCalendar, ZAJO);
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(PRECEDING, ZAJO)));
		assertEquals(test.EffectiveDateOffset, DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(FOLLOWING, ZAJO)));
		assertEquals(test.MaturityDateOffset, TenorAdjustment.of(TENOR_3M, PeriodAdditionConventions.NONE, BusinessDayAdjustment.of(MODIFIED_FOLLOWING, ZAJO)));
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "ZAR-JIBAR-3M");
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {IborIndices.GBP_LIBOR_6M, "GBP-LIBOR-6M"},
			new object[] {IborIndices.CHF_LIBOR_6M, "CHF-LIBOR-6M"},
			new object[] {IborIndices.EUR_LIBOR_6M, "EUR-LIBOR-6M"},
			new object[] {IborIndices.JPY_LIBOR_6M, "JPY-LIBOR-6M"},
			new object[] {IborIndices.USD_LIBOR_6M, "USD-LIBOR-6M"},
			new object[] {IborIndices.EUR_EURIBOR_1M, "EUR-EURIBOR-1M"},
			new object[] {IborIndices.JPY_TIBOR_JAPAN_2M, "JPY-TIBOR-JAPAN-2M"},
			new object[] {IborIndices.JPY_TIBOR_EUROYEN_6M, "JPY-TIBOR-EUROYEN-6M"},
			new object[] {IborIndices.AUD_BBSW_1M, "AUD-BBSW-1M"},
			new object[] {IborIndices.AUD_BBSW_2M, "AUD-BBSW-2M"},
			new object[] {IborIndices.AUD_BBSW_3M, "AUD-BBSW-3M"},
			new object[] {IborIndices.AUD_BBSW_4M, "AUD-BBSW-4M"},
			new object[] {IborIndices.AUD_BBSW_5M, "AUD-BBSW-5M"},
			new object[] {IborIndices.AUD_BBSW_6M, "AUD-BBSW-6M"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(IborIndex convention, String name)
	  public virtual void test_name(IborIndex convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(IborIndex convention, String name)
	  public virtual void test_toString(IborIndex convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(IborIndex convention, String name)
	  public virtual void test_of_lookup(IborIndex convention, string name)
	  {
		assertEquals(IborIndex.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(IborIndex convention, String name)
	  public virtual void test_extendedEnum(IborIndex convention, string name)
	  {
		ImmutableMap<string, IborIndex> map = IborIndex.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => IborIndex.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => IborIndex.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		ImmutableIborIndex a = ImmutableIborIndex.builder().name("Test-3M").currency(Currency.GBP).fixingCalendar(GBLO).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).effectiveDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).maturityDateOffset(TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.NONE)).dayCount(ACT_360).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("Europe/London")).build();
		IborIndex b = a.toBuilder().name("Rubbish-3M").build();
		assertEquals(a.Equals(b), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableIborIndex index = ImmutableIborIndex.builder().name("Test-3M").currency(Currency.GBP).fixingCalendar(GBLO).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).effectiveDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).maturityDateOffset(TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.NONE)).dayCount(ACT_360).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("Europe/London")).build();
		coverImmutableBean(index);
		coverPrivateConstructor(typeof(IborIndices));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(IborIndex), IborIndices.GBP_LIBOR_12M);
	  }

	  public virtual void test_serialization()
	  {
		IborIndex index = ImmutableIborIndex.builder().name("Test-3M").currency(Currency.GBP).fixingCalendar(GBLO).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).effectiveDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).maturityDateOffset(TenorAdjustment.ofLastBusinessDay(TENOR_3M, BusinessDayAdjustment.NONE)).dayCount(ACT_360).fixingTime(LocalTime.NOON).fixingZone(ZoneId.of("Europe/London")).build();
		assertSerialization(index);
	  }

	}

}
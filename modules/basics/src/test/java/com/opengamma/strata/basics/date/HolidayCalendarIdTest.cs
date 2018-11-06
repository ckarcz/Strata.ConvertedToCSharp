/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.NO_HOLIDAYS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="HolidayCalendarId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HolidayCalendarIdTest
	public class HolidayCalendarIdTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of_single()
	  {
		HolidayCalendarId test = HolidayCalendarId.of("GB");
		assertEquals(test.Name, "GB");
		assertEquals(test.ReferenceDataType, typeof(HolidayCalendar));
		assertEquals(test.ToString(), "GB");
	  }

	  public virtual void test_of_combined()
	  {
		HolidayCalendarId test = HolidayCalendarId.of("GB+EU");
		assertEquals(test.Name, "EU+GB");
		assertEquals(test.ReferenceDataType, typeof(HolidayCalendar));
		assertEquals(test.ToString(), "EU+GB");

		HolidayCalendarId test2 = HolidayCalendarId.of("EU+GB");
		assertSame(test, test2);
	  }

	  public virtual void test_of_combined_NoHolidays()
	  {
		HolidayCalendarId test = HolidayCalendarId.of("GB+NoHolidays+EU");
		assertEquals(test.Name, "EU+GB");
		assertEquals(test.ReferenceDataType, typeof(HolidayCalendar));
		assertEquals(test.ToString(), "EU+GB");
	  }

	  public virtual void test_defaultByCurrency()
	  {
		assertEquals(HolidayCalendarId.defaultByCurrency(Currency.GBP), HolidayCalendarIds.GBLO);
		assertEquals(HolidayCalendarId.defaultByCurrency(Currency.CZK), HolidayCalendarIds.CZPR);
		assertEquals(HolidayCalendarId.defaultByCurrency(Currency.HKD), HolidayCalendarId.of("HKHK"));
		assertThrowsIllegalArg(() => HolidayCalendarId.defaultByCurrency(Currency.XAG));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve_single()
	  {
		HolidayCalendarId gb = HolidayCalendarId.of("GB");
		HolidayCalendarId eu = HolidayCalendarId.of("EU");
		HolidayCalendar gbCal = HolidayCalendars.SAT_SUN;
		ReferenceData refData = ImmutableReferenceData.of(gb, gbCal);
		assertEquals(gb.resolve(refData), gbCal);
		assertThrows(() => eu.resolve(refData), typeof(ReferenceDataNotFoundException));
		assertEquals(refData.getValue(gb), gbCal);
	  }

	  public virtual void test_resolve_combined_direct()
	  {
		HolidayCalendarId gb = HolidayCalendarId.of("GB");
		HolidayCalendar gbCal = HolidayCalendars.SAT_SUN;
		HolidayCalendarId eu = HolidayCalendarId.of("EU");
		HolidayCalendar euCal = HolidayCalendars.FRI_SAT;
		HolidayCalendarId combined = gb.combinedWith(eu);
		HolidayCalendar combinedCal = euCal.combinedWith(gbCal);
		ReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(combined, combinedCal));
		assertEquals(combined.resolve(refData), combinedCal);
		assertEquals(refData.getValue(combined), combinedCal);
	  }

	  public virtual void test_resolve_combined_indirect()
	  {
		HolidayCalendarId gb = HolidayCalendarId.of("GB");
		HolidayCalendar gbCal = HolidayCalendars.SAT_SUN;
		HolidayCalendarId eu = HolidayCalendarId.of("EU");
		HolidayCalendar euCal = HolidayCalendars.FRI_SAT;
		HolidayCalendarId combined = gb.combinedWith(eu);
		HolidayCalendar combinedCal = euCal.combinedWith(gbCal);
		ReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(gb, gbCal, eu, euCal));
		assertEquals(combined.resolve(refData), combinedCal);
		assertEquals(refData.getValue(combined), combinedCal);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testImmutableReferenceDataWithMergedHolidays()
	  public virtual void testImmutableReferenceDataWithMergedHolidays()
	  {
		HolidayCalendar hc = HolidayCalendars.FRI_SAT.combinedWith(HolidayCalendars.SAT_SUN);
		ImmutableReferenceData referenceData = ImmutableReferenceData.of(hc.Id, hc);
		LocalDate date = BusinessDayAdjustment.of(BusinessDayConventions.PRECEDING, hc.Id).adjust(LocalDate.of(2016, 8, 20), referenceData);
		assertEquals(LocalDate.of(2016, 8, 18), date);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		HolidayCalendarId gb = HolidayCalendarId.of("GB");
		HolidayCalendarId eu = HolidayCalendarId.of("EU");
		HolidayCalendarId us = HolidayCalendarId.of("US");
		HolidayCalendarId combined1 = eu.combinedWith(us).combinedWith(gb);
		HolidayCalendarId combined2 = us.combinedWith(eu).combinedWith(gb.combinedWith(us));
		assertEquals(combined1.Name, "EU+GB+US");
		assertEquals(combined1.ToString(), "EU+GB+US");
		assertEquals(combined2.Name, "EU+GB+US");
		assertEquals(combined2.ToString(), "EU+GB+US");
		assertEquals(combined1.Equals(combined2), true);
	  }

	  public virtual void test_combinedWithSelf()
	  {
		HolidayCalendarId gb = HolidayCalendarId.of("GB");
		assertEquals(gb.combinedWith(gb), gb);
		assertEquals(gb.combinedWith(NO_HOLIDAYS), gb);
		assertEquals(NO_HOLIDAYS.combinedWith(gb), gb);
		assertEquals(NO_HOLIDAYS.combinedWith(NO_HOLIDAYS), NO_HOLIDAYS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		HolidayCalendarId a = HolidayCalendarId.of("GB");
		HolidayCalendarId a2 = HolidayCalendarId.of("GB");
		HolidayCalendarId b = HolidayCalendarId.of("EU");
		assertEquals(a.GetHashCode(), a2.GetHashCode());
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(HolidayCalendarIds));
	  }

	  public virtual void test_serialization()
	  {
		HolidayCalendarId test = HolidayCalendarId.of("US");
		assertSerialization(test);
	  }

	}

}
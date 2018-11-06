using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.caputureLog;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="HolidayCalendar"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HolidayCalendarIniLookupTest
	public class HolidayCalendarIniLookupTest
	{

	  public virtual void test_valid1()
	  {
		ImmutableMap<string, HolidayCalendar> lookup = HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDataValid1.ini");
		assertEquals(lookup.size(), 1);

		HolidayCalendar test = lookup.get("TEST-VALID");
		assertTrue(test.isHoliday(date(2015, 1, 1)));
		assertTrue(test.isHoliday(date(2015, 1, 6)));
		assertTrue(test.isHoliday(date(2015, 4, 5)));
		assertTrue(test.isHoliday(date(2015, 12, 25)));
		assertTrue(test.isHoliday(date(2016, 1, 1)));
		assertEquals(test.Name, "TEST-VALID");
		assertEquals(test.ToString(), "HolidayCalendar[TEST-VALID]");
	  }

	  public virtual void test_valid2()
	  {
		ImmutableMap<string, HolidayCalendar> lookup = HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDataValid2.ini");
		assertEquals(lookup.size(), 1);

		HolidayCalendar test = lookup.get("TEST-VALID");
		assertTrue(test.isHoliday(date(2015, 1, 1)));
		assertTrue(test.isHoliday(date(2015, 1, 6)));
		assertTrue(test.isHoliday(date(2015, 4, 5)));
		assertTrue(test.isHoliday(date(2015, 12, 25)));
		assertTrue(test.isHoliday(date(2016, 1, 1)));
		assertEquals(test.Name, "TEST-VALID");
		assertEquals(test.ToString(), "HolidayCalendar[TEST-VALID]");
	  }

	  public virtual void test_valid1equals2()
	  {
		ImmutableMap<string, HolidayCalendar> lookup1 = HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDataValid1.ini");
		ImmutableMap<string, HolidayCalendar> lookup2 = HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDataValid2.ini");
		assertEquals(lookup1, lookup2);
	  }

	  public virtual void test_invalid1_invalidYear()
	  {
		  lock (this)
		  {
			IList<LogRecord> captured = caputureLog(typeof(HolidayCalendarIniLookup), () => HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDataInvalid1.ini"));
			assertEquals(captured.Count, 1);
			LogRecord record = captured[0];
			assertEquals(record.Level, Level.SEVERE);
			assertTrue(record.Thrown.Message.contains("Parsed date had incorrect year"));
		  }
	  }

	  public virtual void test_invalid1_invalidDayOfWeek()
	  {
		  lock (this)
		  {
			IList<LogRecord> captured = caputureLog(typeof(HolidayCalendarIniLookup), () => HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDataInvalid2.ini"));
			assertEquals(captured.Count, 1);
			LogRecord record = captured[0];
			assertEquals(record.Level, Level.SEVERE);
			assertTrue(record.Thrown is DateTimeParseException);
			assertTrue(record.Thrown.Message.contains("'Bob'"));
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultByCurrency_valid()
	  {
		  lock (this)
		  {
			ImmutableMap<Currency, HolidayCalendarId> test = HolidayCalendarIniLookup.loadDefaultsFromIni("HolidayCalendarDefaultDataValid.ini");
			assertEquals(test.size(), 2);
        
			assertEquals(test.get(Currency.GBP), HolidayCalendarIds.GBLO);
			assertEquals(test.get(Currency.USD), HolidayCalendarIds.NYSE);
		  }
	  }

	  public virtual void test_defaultByCurrency_invalid()
	  {
		  lock (this)
		  {
			IList<LogRecord> captured = caputureLog(typeof(HolidayCalendarIniLookup), () => HolidayCalendarIniLookup.loadFromIni("HolidayCalendarDefaultDataInvalid.ini"));
			assertEquals(captured.Count, 1);
			LogRecord record = captured[0];
			assertEquals(record.Level, Level.SEVERE);
			assertTrue(record.Message.contains("Error processing resource"));
		  }
	  }

	}

}
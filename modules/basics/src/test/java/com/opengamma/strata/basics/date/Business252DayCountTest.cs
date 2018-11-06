/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Business252DayCount"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class Business252DayCountTest
	public class Business252DayCountTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_factory_name()
	  {
		DayCount test = DayCount.of("Bus/252 EUTA");
		assertEquals(test.Name, "Bus/252 EUTA");
		assertEquals(test.ToString(), "Bus/252 EUTA");

		assertSame(DayCount.of("Bus/252 EUTA"), test);
		assertSame(DayCount.ofBus252(EUTA), test);
	  }

	  public virtual void test_factory_nameUpper()
	  {
		DayCount test = DayCount.of("BUS/252 EUTA");
		assertEquals(test.Name, "Bus/252 EUTA");
		assertEquals(test.ToString(), "Bus/252 EUTA");

		assertSame(DayCount.of("Bus/252 EUTA"), test);
		assertSame(DayCount.ofBus252(EUTA), test);
	  }

	  public virtual void test_factory_calendar()
	  {
		DayCount test = DayCount.ofBus252(GBLO);
		assertEquals(test.Name, "Bus/252 GBLO");
		assertEquals(test.ToString(), "Bus/252 GBLO");

		assertSame(DayCount.of("Bus/252 GBLO"), test);
		assertSame(DayCount.ofBus252(GBLO), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_yearFraction()
	  {
		DayCount test = DayCount.of("Bus/252 EUTA");
		LocalDate date1 = date(2014, 12, 1);
		LocalDate date2 = date(2014, 12, 1);
		for (int i = 0; i < 366; i++)
		{
		  assertEquals(test.yearFraction(date1, date2), EUTA.resolve(REF_DATA).daysBetween(date1, date2) / 252d);
		  date2 = date2.plusDays(1);
		}
	  }

	  public virtual void test_yearFraction_badOrder()
	  {
		DayCount test = DayCount.of("Bus/252 EUTA");
		LocalDate date1 = date(2014, 12, 2);
		LocalDate date2 = date(2014, 12, 1);
		assertThrowsIllegalArg(() => test.yearFraction(date1, date2));
	  }

	  public virtual void test_days()
	  {
		DayCount test = DayCount.of("Bus/252 EUTA");
		LocalDate date1 = date(2014, 12, 1);
		LocalDate date2 = date(2014, 12, 1);
		for (int i = 0; i < 366; i++)
		{
		  assertEquals(test.days(date1, date2), EUTA.resolve(REF_DATA).daysBetween(date1, date2));
		  date2 = date2.plusDays(1);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		DayCount a = DayCount.of("Bus/252 EUTA");
		DayCount b = DayCount.of("Bus/252 GBLO");
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.GetHashCode(), a.GetHashCode());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(Business252DayCount));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(DayCount.ofBus252(EUTA));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(DayCount), DayCount.ofBus252(EUTA));
	  }

	}

}
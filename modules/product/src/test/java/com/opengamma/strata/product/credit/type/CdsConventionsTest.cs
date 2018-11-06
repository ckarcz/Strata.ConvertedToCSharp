/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Test <seealso cref="CdsConventions"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsConventionsTest
	public class CdsConventionsTest
	{

	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_USNY_JPTO = JPTO.combinedWith(GBLO_USNY);
	  private static readonly HolidayCalendarId GBLO_EUTA = GBLO.combinedWith(EUTA);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "currency") public static Object[][] data_currency()
	  public static object[][] data_currency()
	  {
		return new object[][]
		{
			new object[] {CdsConventions.EUR_GB_STANDARD, EUR},
			new object[] {CdsConventions.EUR_STANDARD, EUR},
			new object[] {CdsConventions.GBP_STANDARD, GBP},
			new object[] {CdsConventions.GBP_US_STANDARD, GBP},
			new object[] {CdsConventions.JPY_STANDARD, JPY},
			new object[] {CdsConventions.JPY_US_GB_STANDARD, JPY},
			new object[] {CdsConventions.USD_STANDARD, USD}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "currency") public void test_spot_lag(ImmutableCdsConvention convention, com.opengamma.strata.basics.currency.Currency currency)
	  public virtual void test_spot_lag(ImmutableCdsConvention convention, Currency currency)
	  {
		assertEquals(convention.Currency, currency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "common") public static Object[][] data_common()
	  public static object[][] data_common()
	  {
		return new object[][]
		{
			new object[] {CdsConventions.EUR_GB_STANDARD},
			new object[] {CdsConventions.EUR_STANDARD},
			new object[] {CdsConventions.GBP_STANDARD},
			new object[] {CdsConventions.GBP_US_STANDARD},
			new object[] {CdsConventions.JPY_STANDARD},
			new object[] {CdsConventions.JPY_US_GB_STANDARD},
			new object[] {CdsConventions.USD_STANDARD}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "common") public void test_period(ImmutableCdsConvention convention)
	  public virtual void test_period(ImmutableCdsConvention convention)
	  {
		assertEquals(convention.PaymentFrequency, Frequency.P3M);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "common") public void test_day_count(ImmutableCdsConvention convention)
	  public virtual void test_day_count(ImmutableCdsConvention convention)
	  {
		assertEquals(convention.DayCount, ACT_360);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "businessDayAdjustment") public static Object[][] data_businessDayAdjustment()
	  public static object[][] data_businessDayAdjustment()
	  {
		return new object[][]
		{
			new object[] {CdsConventions.EUR_GB_STANDARD, BusinessDayAdjustment.of(FOLLOWING, GBLO_EUTA)},
			new object[] {CdsConventions.EUR_STANDARD, BusinessDayAdjustment.of(FOLLOWING, EUTA)},
			new object[] {CdsConventions.GBP_STANDARD, BusinessDayAdjustment.of(FOLLOWING, GBLO)},
			new object[] {CdsConventions.GBP_US_STANDARD, BusinessDayAdjustment.of(FOLLOWING, GBLO_USNY)},
			new object[] {CdsConventions.JPY_STANDARD, BusinessDayAdjustment.of(FOLLOWING, JPTO)},
			new object[] {CdsConventions.JPY_US_GB_STANDARD, BusinessDayAdjustment.of(FOLLOWING, GBLO_USNY_JPTO)},
			new object[] {CdsConventions.USD_STANDARD, BusinessDayAdjustment.of(FOLLOWING, USNY)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "businessDayAdjustment") public void test_businessDayAdjustment(ImmutableCdsConvention convention, com.opengamma.strata.basics.date.BusinessDayAdjustment adj)
	  public virtual void test_businessDayAdjustment(ImmutableCdsConvention convention, BusinessDayAdjustment adj)
	  {
		assertEquals(convention.BusinessDayAdjustment, adj);
		assertEquals(convention.StartDateBusinessDayAdjustment, adj);
		assertEquals(convention.EndDateBusinessDayAdjustment, BusinessDayAdjustment.NONE);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "string") public static Object[][] data_string()
	  public static object[][] data_string()
	  {
		return new object[][]
		{
			new object[] {CdsConventions.EUR_GB_STANDARD, "EUR-GB-STANDARD"},
			new object[] {CdsConventions.EUR_STANDARD, "EUR-STANDARD"},
			new object[] {CdsConventions.GBP_STANDARD, "GBP-STANDARD"},
			new object[] {CdsConventions.GBP_US_STANDARD, "GBP-US-STANDARD"},
			new object[] {CdsConventions.JPY_STANDARD, "JPY-STANDARD"},
			new object[] {CdsConventions.JPY_US_GB_STANDARD, "JPY-US-GB-STANDARD"},
			new object[] {CdsConventions.USD_STANDARD, "USD-STANDARD"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "string") public void test_string(ImmutableCdsConvention convention, String string)
	  public virtual void test_string(ImmutableCdsConvention convention, string @string)
	  {
		assertEquals(convention.ToString(), @string);
		assertEquals(convention.Name, @string);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(CdsConventions));
		coverPrivateConstructor(typeof(StandardCdsConventions));
	  }

	}

}
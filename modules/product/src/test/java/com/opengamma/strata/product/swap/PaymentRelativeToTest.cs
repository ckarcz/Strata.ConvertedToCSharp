/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PaymentRelativeToTest
	public class PaymentRelativeToTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {PaymentRelativeTo.PERIOD_START, "PeriodStart"},
			new object[] {PaymentRelativeTo.PERIOD_END, "PeriodEnd"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(PaymentRelativeTo convention, String name)
	  public virtual void test_toString(PaymentRelativeTo convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(PaymentRelativeTo convention, String name)
	  public virtual void test_of_lookup(PaymentRelativeTo convention, string name)
	  {
		assertEquals(PaymentRelativeTo.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => PaymentRelativeTo.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => PaymentRelativeTo.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void selectDate()
	  {
		LocalDate date1 = date(2014, 3, 27);
		LocalDate date2 = date(2014, 6, 27);
		SchedulePeriod period = SchedulePeriod.of(date1, date2);
		assertEquals(PaymentRelativeTo.PERIOD_START.selectBaseDate(period), date1);
		assertEquals(PaymentRelativeTo.PERIOD_END.selectBaseDate(period), date2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(PaymentRelativeTo));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PaymentRelativeTo.PERIOD_START);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(PaymentRelativeTo), PaymentRelativeTo.PERIOD_START);
	  }

	}

}
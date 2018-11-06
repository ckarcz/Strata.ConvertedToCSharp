/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.PeriodAdditionConventions.LAST_BUSINESS_DAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.PeriodAdditionConventions.LAST_DAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="TenorAdjustment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TenorAdjustmentTest
	public class TenorAdjustmentTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly PeriodAdditionConvention PAC_NONE = PeriodAdditionConventions.NONE;
	  private static readonly BusinessDayAdjustment BDA_NONE = BusinessDayAdjustment.NONE;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW_SAT_SUN = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, HolidayCalendarIds.SAT_SUN);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_additionConventionNone()
	  {
		TenorAdjustment test = TenorAdjustment.of(Tenor.of(Period.of(1, 2, 3)), PAC_NONE, BDA_NONE);
		assertEquals(test.Tenor, Tenor.of(Period.of(1, 2, 3)));
		assertEquals(test.AdditionConvention, PAC_NONE);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "1Y2M3D");
	  }

	  public virtual void test_of_additionConventionLastDay()
	  {
		TenorAdjustment test = TenorAdjustment.of(TENOR_3M, LAST_DAY, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.AdditionConvention, LAST_DAY);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.ToString(), "3M with LastDay then apply Following using calendar Sat/Sun");
	  }

	  public virtual void test_ofLastDay()
	  {
		TenorAdjustment test = TenorAdjustment.ofLastDay(TENOR_3M, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.AdditionConvention, LAST_DAY);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.ToString(), "3M with LastDay then apply Following using calendar Sat/Sun");
	  }

	  public virtual void test_ofLastBusinessDay()
	  {
		TenorAdjustment test = TenorAdjustment.ofLastBusinessDay(TENOR_3M, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.AdditionConvention, LAST_BUSINESS_DAY);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.ToString(), "3M with LastBusinessDay then apply Following using calendar Sat/Sun");
	  }

	  public virtual void test_of_invalid_conventionForPeriod()
	  {
		assertThrowsIllegalArg(() => TenorAdjustment.of(TENOR_1W, LAST_DAY, BDA_NONE));
		assertThrowsIllegalArg(() => TenorAdjustment.of(TENOR_1W, LAST_BUSINESS_DAY, BDA_NONE));
		assertThrowsIllegalArg(() => TenorAdjustment.ofLastDay(TENOR_1W, BDA_NONE));
		assertThrowsIllegalArg(() => TenorAdjustment.ofLastBusinessDay(TENOR_1W, BDA_NONE));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "adjust") public static Object[][] data_adjust()
	  public static object[][] data_adjust()
	  {
		return new object[][]
		{
			new object[] {1, date(2014, 8, 15), date(2014, 9, 15)},
			new object[] {2, date(2014, 8, 15), date(2014, 10, 15)},
			new object[] {3, date(2014, 8, 15), date(2014, 11, 17)},
			new object[] {1, date(2014, 2, 28), date(2014, 3, 31)},
			new object[] {1, date(2014, 6, 30), date(2014, 7, 31)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "adjust") public void test_adjust(int months, java.time.LocalDate date, java.time.LocalDate expected)
	  public virtual void test_adjust(int months, LocalDate date, LocalDate expected)
	  {
		TenorAdjustment test = TenorAdjustment.of(Tenor.ofMonths(months), LAST_DAY, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.adjust(date, REF_DATA), expected);
		assertEquals(test.resolve(REF_DATA).adjust(date), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void Equals()
	  {
		TenorAdjustment a = TenorAdjustment.of(TENOR_3M, LAST_DAY, BDA_FOLLOW_SAT_SUN);
		TenorAdjustment b = TenorAdjustment.of(TENOR_1M, LAST_DAY, BDA_FOLLOW_SAT_SUN);
		TenorAdjustment c = TenorAdjustment.of(TENOR_3M, PAC_NONE, BDA_FOLLOW_SAT_SUN);
		TenorAdjustment d = TenorAdjustment.of(TENOR_3M, LAST_DAY, BDA_NONE);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(c), false);
		assertEquals(a.Equals(d), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_beanBuilder()
	  {
		TenorAdjustment test = TenorAdjustment.builder().tenor(TENOR_3M).additionConvention(LAST_DAY).adjustment(BDA_FOLLOW_SAT_SUN).build();
		assertEquals(test.Tenor, TENOR_3M);
		assertEquals(test.AdditionConvention, LAST_DAY);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(TenorAdjustment.of(TENOR_3M, LAST_DAY, BDA_FOLLOW_SAT_SUN));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(TenorAdjustment.of(TENOR_3M, LAST_DAY, BDA_FOLLOW_SAT_SUN));
	  }

	}

}
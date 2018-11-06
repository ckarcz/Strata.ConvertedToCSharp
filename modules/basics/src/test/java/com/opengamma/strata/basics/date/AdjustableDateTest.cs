/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="AdjustableDate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AdjustableDateTest
	public class AdjustableDateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly BusinessDayAdjustment BDA_NONE = BusinessDayAdjustment.NONE;
	  private static readonly BusinessDayAdjustment BDA_FOLLOW_SAT_SUN = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, HolidayCalendarIds.SAT_SUN);

	  private static readonly LocalDate THU_2014_07_10 = LocalDate.of(2014, 7, 10);
	  private static readonly LocalDate FRI_2014_07_11 = LocalDate.of(2014, 7, 11);
	  private static readonly LocalDate SAT_2014_07_12 = LocalDate.of(2014, 7, 12);
	  private static readonly LocalDate SUN_2014_07_13 = LocalDate.of(2014, 7, 13);
	  private static readonly LocalDate MON_2014_07_14 = LocalDate.of(2014, 7, 14);
	  private static readonly LocalDate TUE_2014_07_15 = LocalDate.of(2014, 7, 15);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_1arg()
	  {
		AdjustableDate test = AdjustableDate.of(FRI_2014_07_11);
		assertEquals(test.Unadjusted, FRI_2014_07_11);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "2014-07-11");
		assertEquals(test.adjusted(REF_DATA), FRI_2014_07_11);
	  }

	  public virtual void test_of_2args_withAdjustment()
	  {
		AdjustableDate test = AdjustableDate.of(FRI_2014_07_11, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.Unadjusted, FRI_2014_07_11);
		assertEquals(test.Adjustment, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.ToString(), "2014-07-11 adjusted by Following using calendar Sat/Sun");
		assertEquals(test.adjusted(REF_DATA), FRI_2014_07_11);
	  }

	  public virtual void test_of_2args_withNoAdjustment()
	  {
		AdjustableDate test = AdjustableDate.of(FRI_2014_07_11, BDA_NONE);
		assertEquals(test.Unadjusted, FRI_2014_07_11);
		assertEquals(test.Adjustment, BDA_NONE);
		assertEquals(test.ToString(), "2014-07-11");
		assertEquals(test.adjusted(REF_DATA), FRI_2014_07_11);
	  }

	  public virtual void test_of_null()
	  {
		assertThrowsIllegalArg(() => AdjustableDate.of(null));
		assertThrowsIllegalArg(() => AdjustableDate.of(null, BDA_FOLLOW_SAT_SUN));
		assertThrowsIllegalArg(() => AdjustableDate.of(FRI_2014_07_11, null));
		assertThrowsIllegalArg(() => AdjustableDate.of(null, null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "adjusted") public static Object[][] data_adjusted()
	  public static object[][] data_adjusted()
	  {
		return new object[][]
		{
			new object[] {THU_2014_07_10, THU_2014_07_10},
			new object[] {FRI_2014_07_11, FRI_2014_07_11},
			new object[] {SAT_2014_07_12, MON_2014_07_14},
			new object[] {SUN_2014_07_13, MON_2014_07_14},
			new object[] {MON_2014_07_14, MON_2014_07_14},
			new object[] {TUE_2014_07_15, TUE_2014_07_15}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "adjusted") public void test_adjusted(java.time.LocalDate date, java.time.LocalDate expected)
	  public virtual void test_adjusted(LocalDate date, LocalDate expected)
	  {
		AdjustableDate test = AdjustableDate.of(date, BDA_FOLLOW_SAT_SUN);
		assertEquals(test.adjusted(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void Equals()
	  {
		AdjustableDate a1 = AdjustableDate.of(FRI_2014_07_11, BDA_FOLLOW_SAT_SUN);
		AdjustableDate a2 = AdjustableDate.of(FRI_2014_07_11, BDA_FOLLOW_SAT_SUN);
		AdjustableDate b = AdjustableDate.of(SAT_2014_07_12, BDA_FOLLOW_SAT_SUN);
		AdjustableDate c = AdjustableDate.of(FRI_2014_07_11, BDA_NONE);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(AdjustableDate.of(FRI_2014_07_11, BDA_FOLLOW_SAT_SUN));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(AdjustableDate.of(FRI_2014_07_11, BDA_FOLLOW_SAT_SUN));
	  }

	}

}
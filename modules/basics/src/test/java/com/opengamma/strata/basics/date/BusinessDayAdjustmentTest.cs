/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.NO_HOLIDAYS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="BusinessDayAdjustment"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BusinessDayAdjustmentTest
	public class BusinessDayAdjustmentTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test_basics()
	  {
		BusinessDayAdjustment test = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
		assertEquals(test.Convention, MODIFIED_FOLLOWING);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.ToString(), "ModifiedFollowing using calendar Sat/Sun");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "convention", dataProviderClass = BusinessDayConventionTest.class) public void test_adjustDate(BusinessDayConvention convention, java.time.LocalDate input, java.time.LocalDate expected)
	  public virtual void test_adjustDate(BusinessDayConvention convention, LocalDate input, LocalDate expected)
	  {
		BusinessDayAdjustment test = BusinessDayAdjustment.of(convention, SAT_SUN);
		assertEquals(test.adjust(input, REF_DATA), expected);
		assertEquals(test.resolve(REF_DATA).adjust(input), expected);
	  }

	  public virtual void test_noAdjust_constant()
	  {
		BusinessDayAdjustment test = BusinessDayAdjustment.NONE;
		assertEquals(test.Convention, BusinessDayConventions.NO_ADJUST);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.ToString(), "NoAdjust");
	  }

	  public virtual void test_noAdjust_factory()
	  {
		BusinessDayAdjustment test = BusinessDayAdjustment.of(BusinessDayConventions.NO_ADJUST, NO_HOLIDAYS);
		assertEquals(test.Convention, BusinessDayConventions.NO_ADJUST);
		assertEquals(test.Calendar, NO_HOLIDAYS);
		assertEquals(test.ToString(), "NoAdjust");
	  }

	  public virtual void test_noAdjust_normalized()
	  {
		BusinessDayAdjustment test = BusinessDayAdjustment.of(BusinessDayConventions.NO_ADJUST, SAT_SUN);
		assertEquals(test.Convention, BusinessDayConventions.NO_ADJUST);
		assertEquals(test.Calendar, SAT_SUN);
		assertEquals(test.ToString(), "NoAdjust using calendar Sat/Sun");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN));
	  }

	  public virtual void coverage_builder()
	  {
		BusinessDayAdjustment test = BusinessDayAdjustment.builder().convention(MODIFIED_FOLLOWING).calendar(SAT_SUN).build();
		assertEquals(test.Convention, MODIFIED_FOLLOWING);
		assertEquals(test.Calendar, SAT_SUN);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN));
	  }

	}

}
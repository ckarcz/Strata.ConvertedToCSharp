/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Test <seealso cref="ScheduleException"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScheduleExceptionTest
	public class ScheduleExceptionTest
	{

	  public virtual void test_withDefinition()
	  {
		PeriodicSchedule defn = PeriodicSchedule.of(date(2014, 6, 30), date(2014, 8, 30), Frequency.P1M, BusinessDayAdjustment.NONE, StubConvention.NONE, false);
		ScheduleException test = new ScheduleException(defn, "Hello {}", "World");
		assertEquals(test.Message, "Hello World");
		assertEquals(test.Definition, defn);
	  }

	  public virtual void test_withoutDefinition()
	  {
		ScheduleException test = new ScheduleException("Hello {}", "World");
		assertEquals(test.Message, "Hello World");
		assertEquals(test.Definition, null);
	  }

	}

}
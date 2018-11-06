/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Test <seealso cref="AdjustableDateValueFormatter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AdjustableDateValueFormatterTest
	public class AdjustableDateValueFormatterTest
	{

	  public virtual void formatForDisplay()
	  {
		AdjustableDate date = AdjustableDate.of(date(2016, 6, 30), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN));
		assertThat(AdjustableDateValueFormatter.INSTANCE.formatForDisplay(date)).isEqualTo("2016-06-30");
	  }

	  public virtual void formatForCsv()
	  {
		AdjustableDate date = AdjustableDate.of(date(2016, 6, 30), BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN));
		assertThat(AdjustableDateValueFormatter.INSTANCE.formatForCsv(date)).isEqualTo("2016-06-30");
	  }
	}

}
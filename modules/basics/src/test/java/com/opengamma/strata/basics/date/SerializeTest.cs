/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test serialization using Joda-Beans.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SerializeTest
	public class SerializeTest
	{

	  public virtual void test_jodaBeans_serialize()
	  {
		serialize(HolidayCalendars.NO_HOLIDAYS);
		serialize(HolidayCalendars.SAT_SUN);
		serialize(HolidayCalendars.of("GBLO"));
	  }

	  internal virtual void serialize(HolidayCalendar holCal)
	  {
		MockSerBean bean = new MockSerBean();
		bean.BdConvention = BusinessDayConventions.MODIFIED_FOLLOWING;
		bean.HolidayCalendar = holCal;
		bean.DayCount = DayCounts.ACT_360;
		bean.Objects = ImmutableList.of(BusinessDayConventions.MODIFIED_FOLLOWING, holCal, DayCounts.ACT_360);

		string xml = JodaBeanSer.PRETTY.xmlWriter().write(bean);
		MockSerBean test = JodaBeanSer.COMPACT.xmlReader().read(xml, typeof(MockSerBean));
		assertEquals(test, bean);
	  }

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.node
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;

	/// <summary>
	/// Test <seealso cref="SwapIsdaCreditCurveNode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapIsdaCreditCurveNodeTest
	public class SwapIsdaCreditCurveNodeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ObservableId OBS_ID = QuoteId.of(StandardId.of("OG", "USD_IRS_10Y"));
	  private static readonly Tenor TENOR = Tenor.TENOR_10Y;
	  private static readonly BusinessDayAdjustment BUS_ADJ = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, SAT_SUN);
	  private static readonly DaysAdjustment ADJ_3D = DaysAdjustment.ofBusinessDays(3, SAT_SUN);
	  private static readonly Frequency FREQUENCY = Frequency.P6M;
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2016, 9, 29);

	  public virtual void test_of()
	  {
		SwapIsdaCreditCurveNode test = SwapIsdaCreditCurveNode.of(OBS_ID, ADJ_3D, BUS_ADJ, TENOR, THIRTY_U_360, FREQUENCY);
		assertEquals(test.BusinessDayAdjustment, BUS_ADJ);
		assertEquals(test.DayCount, THIRTY_U_360);
		assertEquals(test.Label, TENOR.ToString());
		assertEquals(test.ObservableId, OBS_ID);
		assertEquals(test.PaymentFrequency, FREQUENCY);
		assertEquals(test.SpotDateOffset, ADJ_3D);
		assertEquals(test.Tenor, TENOR);
		assertEquals(test.date(TRADE_DATE, REF_DATA), LocalDate.of(2026, 10, 5));
		assertEquals(test.metadata(LocalDate.of(2026, 10, 5)), TenorDateParameterMetadata.of(LocalDate.of(2026, 10, 5), TENOR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwapIsdaCreditCurveNode test1 = SwapIsdaCreditCurveNode.of(OBS_ID, ADJ_3D, BUS_ADJ, TENOR, THIRTY_U_360, FREQUENCY);
		coverImmutableBean(test1);
		SwapIsdaCreditCurveNode test2 = SwapIsdaCreditCurveNode.builder().observableId(QuoteId.of(StandardId.of("OG", "foo"))).spotDateOffset(DaysAdjustment.NONE).businessDayAdjustment(BusinessDayAdjustment.NONE).tenor(Tenor.TENOR_15Y).dayCount(DayCounts.ACT_365F).paymentFrequency(Frequency.P3M).label("test2").build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SwapIsdaCreditCurveNode test = SwapIsdaCreditCurveNode.of(OBS_ID, ADJ_3D, BUS_ADJ, TENOR, THIRTY_U_360, FREQUENCY);
		assertSerialization(test);
	  }

	}

}
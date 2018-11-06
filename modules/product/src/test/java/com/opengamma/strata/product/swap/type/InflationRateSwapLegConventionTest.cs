/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_HICP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PriceIndexCalculationMethod.MONTHLY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;

	/// <summary>
	/// Test <seealso cref="InflationRateSwapLegConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationRateSwapLegConventionTest
	public class InflationRateSwapLegConventionTest
	{

	  private static readonly Period LAG_3M = Period.ofMonths(3);
	  private static readonly Period LAG_4M = Period.ofMonths(4);
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		InflationRateSwapLegConvention test = InflationRateSwapLegConvention.of(GB_HICP, LAG_3M, MONTHLY, BDA_MOD_FOLLOW);
		assertEquals(test.Index, GB_HICP);
		assertEquals(test.Lag, LAG_3M);
		assertEquals(test.IndexCalculationMethod, MONTHLY);
		assertEquals(test.NotionalExchange, false);
		assertEquals(test.Currency, GBP);
	  }

	  public virtual void test_builder()
	  {
		InflationRateSwapLegConvention test = InflationRateSwapLegConvention.builder().index(GB_HICP).lag(LAG_3M).build();
		assertEquals(test.Index, GB_HICP);
		assertEquals(test.Lag, LAG_3M);
		assertEquals(test.IndexCalculationMethod, MONTHLY);
		assertEquals(test.NotionalExchange, false);
		assertEquals(test.Currency, GBP);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_notEnoughData()
	  {
		assertThrowsIllegalArg(() => IborRateSwapLegConvention.builder().build());
	  }

	  public virtual void test_builderAllSpecified()
	  {
		InflationRateSwapLegConvention test = InflationRateSwapLegConvention.builder().index(GB_HICP).lag(LAG_3M).indexCalculationMethod(PriceIndexCalculationMethod.INTERPOLATED).notionalExchange(true).build();
		assertEquals(test.Index, GB_HICP);
		assertEquals(test.Lag, LAG_3M);
		assertEquals(test.IndexCalculationMethod, PriceIndexCalculationMethod.INTERPOLATED);
		assertEquals(test.NotionalExchange, true);
		assertEquals(test.Currency, GBP);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toLeg()
	  {
		InflationRateSwapLegConvention @base = InflationRateSwapLegConvention.of(GB_HICP, LAG_3M, MONTHLY, BDA_MOD_FOLLOW);
		LocalDate startDate = LocalDate.of(2015, 5, 5);
		LocalDate endDate = LocalDate.of(2020, 5, 5);
		RateCalculationSwapLeg test = @base.toLeg(startDate, endDate, PAY, NOTIONAL_2M);

		RateCalculationSwapLeg expected = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().frequency(Frequency.TERM).startDate(startDate).endDate(endDate).businessDayAdjustment(BDA_MOD_FOLLOW).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL_2M)).calculation(InflationRateCalculation.of(GB_HICP, 3, MONTHLY)).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationRateSwapLegConvention test = InflationRateSwapLegConvention.builder().index(GB_HICP).lag(LAG_3M).build();
		coverImmutableBean(test);
		InflationRateSwapLegConvention test2 = InflationRateSwapLegConvention.builder().index(GB_HICP).lag(LAG_4M).indexCalculationMethod(PriceIndexCalculationMethod.INTERPOLATED).notionalExchange(true).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationRateSwapLegConvention test = InflationRateSwapLegConvention.of(GB_HICP, LAG_3M, MONTHLY, BDA_MOD_FOLLOW);
		assertSerialization(test);
	  }

	}

}
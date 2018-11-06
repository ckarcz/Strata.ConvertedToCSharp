/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.NegativeRateMethod.ALLOW_NEGATIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.NegativeRateMethod.NOT_NEGATIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RateAccrualPeriodTest
	public class RateAccrualPeriodTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_03_28 = date(2014, 3, 28);
	  private static readonly LocalDate DATE_2014_03_30 = date(2014, 3, 30);
	  private static readonly LocalDate DATE_2014_03_31 = date(2014, 3, 31);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_07_01 = date(2014, 7, 1);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_03_27 = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 3, 27), REF_DATA);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_03_28 = IborRateComputation.of(GBP_LIBOR_3M, DATE_2014_03_28, REF_DATA);

	  public virtual void test_builder()
	  {
		RateAccrualPeriod test = RateAccrualPeriod.builder().startDate(DATE_2014_03_31).endDate(DATE_2014_07_01).unadjustedStartDate(DATE_2014_03_30).unadjustedEndDate(DATE_2014_06_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_03_28).build();
		assertEquals(test.StartDate, DATE_2014_03_31);
		assertEquals(test.EndDate, DATE_2014_07_01);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedEndDate, DATE_2014_06_30);
		assertEquals(test.YearFraction, 0.25d, 0d);
		assertEquals(test.RateComputation, GBP_LIBOR_3M_2014_03_28);
		assertEquals(test.Gearing, 1d, 0d);
		assertEquals(test.Spread, 0d, 0d);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
	  }

	  public virtual void test_builder_defaultDates()
	  {
		RateAccrualPeriod test = RateAccrualPeriod.builder().startDate(DATE_2014_03_31).endDate(DATE_2014_07_01).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_03_28).build();
		assertEquals(test.StartDate, DATE_2014_03_31);
		assertEquals(test.EndDate, DATE_2014_07_01);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_31);
		assertEquals(test.UnadjustedEndDate, DATE_2014_07_01);
		assertEquals(test.YearFraction, 0.25d, 0d);
		assertEquals(test.RateComputation, GBP_LIBOR_3M_2014_03_28);
		assertEquals(test.Gearing, 1d, 0d);
		assertEquals(test.Spread, 0d, 0d);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
	  }

	  public virtual void test_builder_schedulePeriod()
	  {
		SchedulePeriod schedulePeriod = SchedulePeriod.of(DATE_2014_03_31, DATE_2014_07_01, DATE_2014_03_30, DATE_2014_06_30);
		RateAccrualPeriod test = RateAccrualPeriod.builder(schedulePeriod).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_03_28).build();
		assertEquals(test.StartDate, DATE_2014_03_31);
		assertEquals(test.EndDate, DATE_2014_07_01);
		assertEquals(test.UnadjustedStartDate, DATE_2014_03_30);
		assertEquals(test.UnadjustedEndDate, DATE_2014_06_30);
		assertEquals(test.YearFraction, 0.25d, 0d);
		assertEquals(test.RateComputation, GBP_LIBOR_3M_2014_03_28);
		assertEquals(test.Gearing, 1d, 0d);
		assertEquals(test.Spread, 0d, 0d);
		assertEquals(test.NegativeRateMethod, ALLOW_NEGATIVE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RateAccrualPeriod test = RateAccrualPeriod.builder().startDate(DATE_2014_03_31).endDate(DATE_2014_07_01).unadjustedStartDate(DATE_2014_03_30).unadjustedEndDate(DATE_2014_06_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_03_28).build();
		coverImmutableBean(test);
		RateAccrualPeriod test2 = RateAccrualPeriod.builder().startDate(DATE_2014_03_30).endDate(DATE_2014_06_30).unadjustedStartDate(DATE_2014_03_31).unadjustedEndDate(DATE_2014_07_01).yearFraction(0.26d).rateComputation(GBP_LIBOR_3M_2014_03_27).gearing(1.1d).spread(0.25d).negativeRateMethod(NOT_NEGATIVE).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RateAccrualPeriod test = RateAccrualPeriod.builder().startDate(DATE_2014_03_31).endDate(DATE_2014_07_01).unadjustedStartDate(DATE_2014_03_30).unadjustedEndDate(DATE_2014_06_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_03_28).build();
		assertSerialization(test);
	  }

	}

}
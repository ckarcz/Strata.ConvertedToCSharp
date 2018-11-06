/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;

	/// <summary>
	/// Test <seealso cref="ResolvedFixedCouponBond"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFixedCouponBondTest
	public class ResolvedFixedCouponBondTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_getters()
	  {
		ResolvedFixedCouponBond test = sut();
		ImmutableList<FixedCouponBondPaymentPeriod> payments = test.PeriodicPayments;
		assertEquals(test.StartDate, payments.get(0).StartDate);
		assertEquals(test.EndDate, payments.get(payments.size() - 1).EndDate);
		assertEquals(test.UnadjustedStartDate, payments.get(0).UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, payments.get(payments.size() - 1).UnadjustedEndDate);
		assertEquals(test.hasExCouponPeriod(), true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_yearFraction()
	  {
		ResolvedFixedCouponBond test = sut();
		FixedCouponBondPaymentPeriod period = test.PeriodicPayments.get(0);
		assertEquals(test.yearFraction(period.UnadjustedStartDate, period.UnadjustedEndDate), period.YearFraction);
	  }

	  public virtual void test_yearFraction_scheduleInfo()
	  {
		ResolvedFixedCouponBond @base = sut();
		FixedCouponBondPaymentPeriod period = @base.PeriodicPayments.get(0);
		AtomicBoolean eom = new AtomicBoolean(false);
		DayCount dc = new DayCountAnonymousInnerClass(this, @base, period, eom);
		ResolvedFixedCouponBond test = @base.toBuilder().dayCount(dc).build();
		assertEquals(test.yearFraction(period.UnadjustedStartDate, period.UnadjustedEndDate), 0.5);
		// test with EOM=true
		ResolvedFixedCouponBond test2 = test.toBuilder().rollConvention(RollConventions.EOM).build();
		eom.set(true);
		assertEquals(test2.yearFraction(period.UnadjustedStartDate, period.UnadjustedEndDate), 0.5);
	  }

	  private class DayCountAnonymousInnerClass : DayCount
	  {
		  private readonly ResolvedFixedCouponBondTest outerInstance;

		  private com.opengamma.strata.product.bond.ResolvedFixedCouponBond @base;
		  private com.opengamma.strata.product.bond.FixedCouponBondPaymentPeriod period;
		  private AtomicBoolean eom;

		  public DayCountAnonymousInnerClass(ResolvedFixedCouponBondTest outerInstance, com.opengamma.strata.product.bond.ResolvedFixedCouponBond @base, com.opengamma.strata.product.bond.FixedCouponBondPaymentPeriod period, AtomicBoolean eom)
		  {
			  this.outerInstance = outerInstance;
			  this.@base = @base;
			  this.period = period;
			  this.eom = eom;
		  }

		  public double yearFraction(LocalDate firstDate, LocalDate secondDate, com.opengamma.strata.basics.date.DayCount_ScheduleInfo scheduleInfo)
		  {
			assertEquals(scheduleInfo.StartDate, @base.UnadjustedStartDate);
			assertEquals(scheduleInfo.EndDate, @base.UnadjustedEndDate);
			assertEquals(scheduleInfo.getPeriodEndDate(firstDate), period.UnadjustedEndDate);
			assertEquals(scheduleInfo.Frequency, @base.Frequency);
			assertEquals(scheduleInfo.EndOfMonthConvention, eom.get());
			return 0.5;
		  }

		  public int days(LocalDate firstDate, LocalDate secondDate)
		  {
			return 182;
		  }

		  public string Name
		  {
			  get
			  {
				return "";
			  }
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_findPeriod()
	  {
		ResolvedFixedCouponBond test = sut();
		ImmutableList<FixedCouponBondPaymentPeriod> payments = test.PeriodicPayments;
		assertEquals(test.findPeriod(test.UnadjustedStartDate), payments.get(0));
		assertEquals(test.findPeriod(test.UnadjustedEndDate.minusDays(1)), payments.get(payments.size() - 1));
		assertEquals(test.findPeriod(LocalDate.MIN), null);
		assertEquals(test.findPeriod(LocalDate.MAX), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void coverage_builder()
	  {
		ResolvedFixedCouponBond test = sut();
		test.toBuilder().periodicPayments(test.PeriodicPayments.toArray(new FixedCouponBondPaymentPeriod[0])).build();
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedFixedCouponBond sut()
	  {
		return FixedCouponBondTest.sut().resolve(REF_DATA);
	  }

	  internal static ResolvedFixedCouponBond sut2()
	  {
		return FixedCouponBondTest.sut2().resolve(REF_DATA);
	  }

	}

}
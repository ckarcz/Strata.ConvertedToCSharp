/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.NL_365;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.US_CPI_U;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.GB_IL_FLOAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.bond.CapitalIndexedBondYieldConvention.US_IL_REAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.PriceIndexCalculationMethod.INTERPOLATED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;
	using InflationRateCalculation = com.opengamma.strata.product.swap.InflationRateCalculation;

	/// <summary>
	/// Test <seealso cref="ResolvedCapitalIndexedBond"/>. 
	/// <para>
	/// The accrued interest method is test in the pricer test.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCapitalIndexedBondTest
	public class ResolvedCapitalIndexedBondTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LegalEntityId LEGAL_ENTITY = LegalEntityId.of("OG-Ticker", "US-Govt");
	  private const double COUPON = 0.01;
	  private static readonly InflationRateCalculation RATE_CALC = InflationRateCalculation.builder().gearing(ValueSchedule.of(COUPON)).index(US_CPI_U).lag(Period.ofMonths(3)).indexCalculationMethod(INTERPOLATED).firstIndexValue(198.475).build();
	  private const double NOTIONAL = 10_000_000d;
	  private static readonly BusinessDayAdjustment SCHEDULE_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY);
	  private static readonly DaysAdjustment SETTLE_OFFSET = DaysAdjustment.ofBusinessDays(2, USNY);

	  private static readonly CapitalIndexedBondPaymentPeriod[] PERIODIC = new CapitalIndexedBondPaymentPeriod[4];
	  static ResolvedCapitalIndexedBondTest()
	  {
		LocalDate[] unAdjDates = new LocalDate[] {LocalDate.of(2008, 1, 13), LocalDate.of(2008, 7, 13), LocalDate.of(2009, 1, 13), LocalDate.of(2009, 7, 13), LocalDate.of(2010, 1, 13)};
		for (int i = 0; i < 4; ++i)
		{
		  LocalDate start = SCHEDULE_ADJ.adjust(unAdjDates[i], REF_DATA);
		  LocalDate end = SCHEDULE_ADJ.adjust(unAdjDates[i + 1], REF_DATA);
		  RateComputation rateComputation = RATE_CALC.createRateComputation(end);
		  PERIODIC[i] = CapitalIndexedBondPaymentPeriod.builder().currency(USD).startDate(start).endDate(end).unadjustedStartDate(unAdjDates[i]).unadjustedEndDate(unAdjDates[i + 1]).detachmentDate(end).realCoupon(COUPON).rateComputation(rateComputation).notional(NOTIONAL).build();
		}
	  }
	  private static readonly CapitalIndexedBondPaymentPeriod NOMINAL = PERIODIC[3].withUnitCoupon(PERIODIC[0].StartDate, PERIODIC[0].UnadjustedStartDate);

	  public virtual void test_builder()
	  {
		ResolvedCapitalIndexedBond test = sut();
		assertEquals(test.Currency, USD);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.StartDate, PERIODIC[0].StartDate);
		assertEquals(test.EndDate, PERIODIC[3].EndDate);
		assertEquals(test.UnadjustedStartDate, PERIODIC[0].UnadjustedStartDate);
		assertEquals(test.UnadjustedEndDate, PERIODIC[3].UnadjustedEndDate);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.NominalPayment, NOMINAL);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.PeriodicPayments.toArray(), PERIODIC);
		assertEquals(test.SettlementDateOffset, SETTLE_OFFSET);
		assertEquals(test.YieldConvention, US_IL_REAL);
		assertEquals(test.hasExCouponPeriod(), false);
		assertEquals(test.FirstIndexValue, RATE_CALC.FirstIndexValue.Value);
		assertEquals(test.findPeriod(PERIODIC[0].UnadjustedStartDate), (test.PeriodicPayments.get(0)));
		assertEquals(test.findPeriod(LocalDate.MIN), null);
		assertEquals(test.findPeriodIndex(PERIODIC[0].UnadjustedStartDate), int?.of(0));
		assertEquals(test.findPeriodIndex(PERIODIC[1].UnadjustedStartDate), int?.of(1));
		assertEquals(test.findPeriodIndex(LocalDate.MIN), int?.empty());
		assertEquals(test.calculateSettlementDateFromValuation(date(2015, 6, 30), REF_DATA), SETTLE_OFFSET.adjust(date(2015, 6, 30), REF_DATA));
	  }

	  public virtual void test_builder_fail()
	  {
		CapitalIndexedBondPaymentPeriod period = CapitalIndexedBondPaymentPeriod.builder().startDate(PERIODIC[2].StartDate).endDate(PERIODIC[2].EndDate).currency(GBP).notional(NOTIONAL).rateComputation(PERIODIC[2].RateComputation).realCoupon(COUPON).build();
		assertThrowsIllegalArg(() => ResolvedCapitalIndexedBond.builder().dayCount(ACT_ACT_ISDA).legalEntityId(LEGAL_ENTITY).nominalPayment(NOMINAL).periodicPayments(PERIODIC[0], PERIODIC[1], period, PERIODIC[3]).settlementDateOffset(SETTLE_OFFSET).yieldConvention(US_IL_REAL).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_yearFraction_scheduleInfo()
	  {
		ResolvedCapitalIndexedBond @base = sut();
		CapitalIndexedBondPaymentPeriod period = @base.PeriodicPayments.get(0);
		AtomicBoolean eom = new AtomicBoolean(false);
		DayCount dc = new DayCountAnonymousInnerClass(this, @base, period, eom);
		ResolvedCapitalIndexedBond test = @base.toBuilder().dayCount(dc).build();
		assertEquals(test.yearFraction(period.UnadjustedStartDate, period.UnadjustedEndDate), 0.5);
		// test with EOM=true
		ResolvedCapitalIndexedBond test2 = test.toBuilder().rollConvention(RollConventions.EOM).build();
		eom.set(true);
		assertEquals(test2.yearFraction(period.UnadjustedStartDate, period.UnadjustedEndDate), 0.5);
	  }

	  private class DayCountAnonymousInnerClass : DayCount
	  {
		  private readonly ResolvedCapitalIndexedBondTest outerInstance;

		  private com.opengamma.strata.product.bond.ResolvedCapitalIndexedBond @base;
		  private com.opengamma.strata.product.bond.CapitalIndexedBondPaymentPeriod period;
		  private AtomicBoolean eom;

		  public DayCountAnonymousInnerClass(ResolvedCapitalIndexedBondTest outerInstance, com.opengamma.strata.product.bond.ResolvedCapitalIndexedBond @base, com.opengamma.strata.product.bond.CapitalIndexedBondPaymentPeriod period, AtomicBoolean eom)
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
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedCapitalIndexedBond sut()
	  {
		return ResolvedCapitalIndexedBond.builder().securityId(CapitalIndexedBondTest.sut().SecurityId).dayCount(ACT_ACT_ISDA).legalEntityId(LEGAL_ENTITY).nominalPayment(NOMINAL).periodicPayments(PERIODIC).frequency(CapitalIndexedBondTest.sut().AccrualSchedule.Frequency).rollConvention(CapitalIndexedBondTest.sut().AccrualSchedule.calculatedRollConvention()).settlementDateOffset(SETTLE_OFFSET).yieldConvention(US_IL_REAL).rateCalculation(RATE_CALC).build();
	  }

	  internal static ResolvedCapitalIndexedBond sut2()
	  {
		return ResolvedCapitalIndexedBond.builder().securityId(CapitalIndexedBondTest.sut2().SecurityId).dayCount(NL_365).legalEntityId(LegalEntityId.of("OG-Ticker", "US-Govt1")).nominalPayment(PERIODIC[1].withUnitCoupon(PERIODIC[0].StartDate, PERIODIC[0].UnadjustedStartDate)).periodicPayments(PERIODIC[0], PERIODIC[1]).frequency(CapitalIndexedBondTest.sut2().AccrualSchedule.Frequency).rollConvention(CapitalIndexedBondTest.sut2().AccrualSchedule.calculatedRollConvention()).settlementDateOffset(DaysAdjustment.ofBusinessDays(3, GBLO)).yieldConvention(GB_IL_FLOAT).rateCalculation(CapitalIndexedBondTest.sut2().RateCalculation).build();
	  }

	}

}
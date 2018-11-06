using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
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
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using FixingRelativeTo = com.opengamma.strata.product.swap.FixingRelativeTo;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// Test <seealso cref="IborCapFloorLeg"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapFloorLegTest
	public class IborCapFloorLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate START = LocalDate.of(2011, 3, 17);
	  private static readonly LocalDate END = LocalDate.of(2012, 3, 17);
	  private static readonly IborRateCalculation RATE_CALCULATION = IborRateCalculation.of(EUR_EURIBOR_3M);
	  private static readonly Frequency FREQUENCY = Frequency.P3M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE = PeriodicSchedule.builder().startDate(START).endDate(END).frequency(FREQUENCY).businessDayAdjustment(BUSS_ADJ).stubConvention(StubConvention.NONE).build();
	  private static readonly DaysAdjustment PAYMENT_OFFSET = DaysAdjustment.ofBusinessDays(2, EUTA);

	  private static readonly double[] NOTIONALS = new double[] {1.0e6, 1.2e6, 0.8e6, 1.0e6};
	  private static readonly double[] STRIKES = new double[] {0.03, 0.0275, 0.02, 0.0345};
	  private static readonly ValueSchedule CAP = ValueSchedule.of(0.0325);
	  private static readonly IList<ValueStep> FLOOR_STEPS = new List<ValueStep>();
	  private static readonly IList<ValueStep> NOTIONAL_STEPS = new List<ValueStep>();
	  static IborCapFloorLegTest()
	  {
		FLOOR_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(STRIKES[1])));
		FLOOR_STEPS.Add(ValueStep.of(2, ValueAdjustment.ofReplace(STRIKES[2])));
		FLOOR_STEPS.Add(ValueStep.of(3, ValueAdjustment.ofReplace(STRIKES[3])));
		NOTIONAL_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(NOTIONALS[1])));
		NOTIONAL_STEPS.Add(ValueStep.of(2, ValueAdjustment.ofReplace(NOTIONALS[2])));
		NOTIONAL_STEPS.Add(ValueStep.of(3, ValueAdjustment.ofReplace(NOTIONALS[3])));
	  }
	  private static readonly ValueSchedule FLOOR = ValueSchedule.of(STRIKES[0], FLOOR_STEPS);
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONALS[0], NOTIONAL_STEPS);

	  public virtual void test_builder_full()
	  {
		IborCapFloorLeg test = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).capSchedule(CAP).currency(GBP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(PAY).build();
		assertEquals(test.Calculation, RATE_CALCULATION);
		assertEquals(test.CapSchedule.get(), CAP);
		assertEquals(test.FloorSchedule.Present, false);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.PaymentDateOffset, PAYMENT_OFFSET);
		assertEquals(test.PaymentSchedule, SCHEDULE);
		assertEquals(test.PayReceive, PAY);
		assertEquals(test.StartDate, AdjustableDate.of(START, BUSS_ADJ));
		assertEquals(test.EndDate, AdjustableDate.of(END, BUSS_ADJ));
		assertEquals(test.Index, EUR_EURIBOR_3M);
	  }

	  public virtual void test_builder_min()
	  {
		IborCapFloorLeg test = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(FLOOR).notional(NOTIONAL).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
		assertEquals(test.Calculation, RATE_CALCULATION);
		assertEquals(test.CapSchedule.Present, false);
		assertEquals(test.FloorSchedule.get(), FLOOR);
		assertEquals(test.Currency, EUR);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
		assertEquals(test.PaymentSchedule, SCHEDULE);
		assertEquals(test.PayReceive, RECEIVE);
		assertEquals(test.StartDate, AdjustableDate.of(START, BUSS_ADJ));
		assertEquals(test.EndDate, AdjustableDate.of(END, BUSS_ADJ));
	  }

	  public virtual void test_builder_fail()
	  {
		// cap and floor present 
		assertThrowsIllegalArg(() => IborCapFloorLeg.builder().calculation(RATE_CALCULATION).capSchedule(CAP).floorSchedule(FLOOR).notional(NOTIONAL).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build());
		// cap and floor missing
		assertThrowsIllegalArg(() => IborCapFloorLeg.builder().calculation(RATE_CALCULATION).notional(NOTIONAL).paymentSchedule(PeriodicSchedule.builder().startDate(START).endDate(END).frequency(FREQUENCY).businessDayAdjustment(BUSS_ADJ).build()).payReceive(RECEIVE).build());
		// stub type
		assertThrowsIllegalArg(() => IborCapFloorLeg.builder().calculation(RATE_CALCULATION).capSchedule(CAP).currency(GBP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(PeriodicSchedule.builder().startDate(START).endDate(END).frequency(FREQUENCY).businessDayAdjustment(BUSS_ADJ).stubConvention(StubConvention.SHORT_FINAL).build()).payReceive(PAY).build());
	  }

	  public virtual void test_resolve_cap()
	  {
		IborRateCalculation rateCalc = IborRateCalculation.builder().index(EUR_EURIBOR_3M).fixingRelativeTo(FixingRelativeTo.PERIOD_END).fixingDateOffset(EUR_EURIBOR_3M.FixingDateOffset).build();
		IborCapFloorLeg @base = IborCapFloorLeg.builder().calculation(rateCalc).capSchedule(CAP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
		LocalDate[] unadjustedDates = new LocalDate[] {START, START.plusMonths(3), START.plusMonths(6), START.plusMonths(9), START.plusMonths(12)};
		IborCapletFloorletPeriod[] periods = new IborCapletFloorletPeriod[4];
		for (int i = 0; i < 4; ++i)
		{
		  LocalDate start = BUSS_ADJ.adjust(unadjustedDates[i], REF_DATA);
		  LocalDate end = BUSS_ADJ.adjust(unadjustedDates[i + 1], REF_DATA);
		  double yearFraction = EUR_EURIBOR_3M.DayCount.relativeYearFraction(start, end);
		  periods[i] = IborCapletFloorletPeriod.builder().caplet(CAP.InitialValue).currency(EUR).startDate(start).endDate(end).unadjustedStartDate(unadjustedDates[i]).unadjustedEndDate(unadjustedDates[i + 1]).paymentDate(PAYMENT_OFFSET.adjust(end, REF_DATA)).notional(NOTIONALS[i]).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, rateCalc.FixingDateOffset.adjust(end, REF_DATA), REF_DATA)).yearFraction(yearFraction).build();
		}
		ResolvedIborCapFloorLeg expected = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(periods).payReceive(RECEIVE).build();
		ResolvedIborCapFloorLeg computed = @base.resolve(REF_DATA);
		assertEquals(computed, expected);
	  }

	  public virtual void test_resolve_floor()
	  {
		IborCapFloorLeg @base = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(FLOOR).currency(GBP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(PAY).build();
		LocalDate[] unadjustedDates = new LocalDate[] {START, START.plusMonths(3), START.plusMonths(6), START.plusMonths(9), START.plusMonths(12)};
		IborCapletFloorletPeriod[] periods = new IborCapletFloorletPeriod[4];
		for (int i = 0; i < 4; ++i)
		{
		  LocalDate start = BUSS_ADJ.adjust(unadjustedDates[i], REF_DATA);
		  LocalDate end = BUSS_ADJ.adjust(unadjustedDates[i + 1], REF_DATA);
		  double yearFraction = EUR_EURIBOR_3M.DayCount.relativeYearFraction(start, end);
		  LocalDate fixingDate = RATE_CALCULATION.FixingDateOffset.adjust(start, REF_DATA);
		  periods[i] = IborCapletFloorletPeriod.builder().floorlet(STRIKES[i]).currency(GBP).startDate(start).endDate(end).unadjustedStartDate(unadjustedDates[i]).unadjustedEndDate(unadjustedDates[i + 1]).paymentDate(PAYMENT_OFFSET.adjust(end, REF_DATA)).notional(-NOTIONALS[i]).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, fixingDate, REF_DATA)).yearFraction(yearFraction).build();
		}
		ResolvedIborCapFloorLeg expected = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(periods).payReceive(PAY).build();
		ResolvedIborCapFloorLeg computed = @base.resolve(REF_DATA);
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborCapFloorLeg test1 = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(FLOOR).notional(NOTIONAL).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
		coverImmutableBean(test1);
		IborCapFloorLeg test2 = IborCapFloorLeg.builder().calculation(IborRateCalculation.of(GBP_LIBOR_6M)).capSchedule(CAP).notional(ValueSchedule.of(1000)).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(PeriodicSchedule.builder().startDate(START).endDate(END).frequency(Frequency.P6M).businessDayAdjustment(BUSS_ADJ).build()).payReceive(PAY).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborCapFloorLeg test = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(FLOOR).notional(NOTIONAL).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
		assertSerialization(test);
	  }

	}

}
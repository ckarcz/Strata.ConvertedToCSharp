using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365_ACTUAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FixingRelativeTo = com.opengamma.strata.product.swap.FixingRelativeTo;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Test <seealso cref="CmsLeg"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsLegTest
	public class CmsLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_10Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2017, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, SAT_SUN);
	  private static readonly PeriodicSchedule SCHEDULE = PeriodicSchedule.builder().startDate(START).endDate(END).frequency(FREQUENCY).businessDayAdjustment(BUSS_ADJ).build();
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private static readonly DaysAdjustment FIXING_OFFSET = DaysAdjustment.ofBusinessDays(-3, SAT_SUN);
	  private static readonly DaysAdjustment PAYMENT_OFFSET = DaysAdjustment.ofBusinessDays(2, SAT_SUN);
	  private static readonly ValueSchedule CAP = ValueSchedule.of(0.0125);
	  private static readonly IList<ValueStep> FLOOR_STEPS = new List<ValueStep>();
	  private static readonly IList<ValueStep> NOTIONAL_STEPS = new List<ValueStep>();
	  static CmsLegTest()
	  {
		FLOOR_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(0.02)));
		NOTIONAL_STEPS.Add(ValueStep.of(1, ValueAdjustment.ofReplace(1.2e6)));
	  }
	  private static readonly ValueSchedule FLOOR = ValueSchedule.of(0.011, FLOOR_STEPS);
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(1.0e6, NOTIONAL_STEPS);

	  public virtual void test_builder_full()
	  {
		CmsLeg test = CmsLeg.builder().currency(GBP).dayCount(ACT_365_ACTUAL).fixingRelativeTo(FixingRelativeTo.PERIOD_END).fixingDateOffset(FIXING_OFFSET).paymentDateOffset(PAYMENT_OFFSET).floorSchedule(FLOOR).index(INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE).build();
		assertEquals(test.PayReceive, PAY);
		assertFalse(test.CapSchedule.Present);
		assertEquals(test.FloorSchedule.get(), FLOOR);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.DayCount, ACT_365_ACTUAL);
		assertEquals(test.StartDate, AdjustableDate.of(START, SCHEDULE.BusinessDayAdjustment));
		assertEquals(test.EndDate, SCHEDULE.calculatedEndDate());
		assertEquals(test.Index, INDEX);
		assertEquals(test.PaymentSchedule, SCHEDULE);
		assertEquals(test.FixingRelativeTo, FixingRelativeTo.PERIOD_END);
		assertEquals(test.FixingDateOffset, FIXING_OFFSET);
		assertEquals(test.PaymentDateOffset, PAYMENT_OFFSET);
	  }

	  public virtual void test_builder_full_coupon()
	  {
		CmsLeg test = CmsLeg.builder().currency(GBP).dayCount(ACT_365_ACTUAL).fixingRelativeTo(FixingRelativeTo.PERIOD_END).fixingDateOffset(FIXING_OFFSET).paymentDateOffset(PAYMENT_OFFSET).index(INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE).build();
		assertEquals(test.PayReceive, PAY);
		assertFalse(test.CapSchedule.Present);
		assertFalse(test.FloorSchedule.Present);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.DayCount, ACT_365_ACTUAL);
		assertEquals(test.StartDate, AdjustableDate.of(START, SCHEDULE.BusinessDayAdjustment));
		assertEquals(test.EndDate, SCHEDULE.calculatedEndDate());
		assertEquals(test.Index, INDEX);
		assertEquals(test.PaymentSchedule, SCHEDULE);
		assertEquals(test.FixingRelativeTo, FixingRelativeTo.PERIOD_END);
		assertEquals(test.FixingDateOffset, FIXING_OFFSET);
		assertEquals(test.PaymentDateOffset, PAYMENT_OFFSET);
	  }

	  public virtual void test_builder_min()
	  {
		CmsLeg test = sutCap();
		assertEquals(test.PayReceive, RECEIVE);
		assertEquals(test.CapSchedule.get(), CAP);
		assertFalse(test.FloorSchedule.Present);
		assertEquals(test.Currency, EUR_EURIBOR_6M.Currency);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.DayCount, EUR_EURIBOR_6M.DayCount);
		assertEquals(test.StartDate, AdjustableDate.of(START, SCHEDULE_EUR.BusinessDayAdjustment));
		assertEquals(test.EndDate, SCHEDULE_EUR.calculatedEndDate());
		assertEquals(test.Index, INDEX);
		assertEquals(test.PaymentSchedule, SCHEDULE_EUR);
		assertEquals(test.FixingRelativeTo, FixingRelativeTo.PERIOD_START);
		assertEquals(test.FixingDateOffset, EUR_EURIBOR_6M.FixingDateOffset);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
	  }

	  public virtual void test_builder_min_coupon()
	  {
		CmsLeg test = CmsLeg.builder().index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build();
		assertEquals(test.PayReceive, RECEIVE);
		assertFalse(test.CapSchedule.Present);
		assertFalse(test.FloorSchedule.Present);
		assertEquals(test.Currency, EUR_EURIBOR_6M.Currency);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.DayCount, EUR_EURIBOR_6M.DayCount);
		assertEquals(test.StartDate, AdjustableDate.of(START, SCHEDULE_EUR.BusinessDayAdjustment));
		assertEquals(test.EndDate, SCHEDULE_EUR.calculatedEndDate());
		assertEquals(test.Index, INDEX);
		assertEquals(test.PaymentSchedule, SCHEDULE_EUR);
		assertEquals(test.FixingRelativeTo, FixingRelativeTo.PERIOD_START);
		assertEquals(test.FixingDateOffset, EUR_EURIBOR_6M.FixingDateOffset);
		assertEquals(test.PaymentDateOffset, DaysAdjustment.NONE);
	  }

	  public virtual void test_builder_fail()
	  {
		// index is null
		assertThrowsIllegalArg(() => CmsLeg.builder().capSchedule(CAP).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build());
		// floorSchedule and capSchedule are present
		assertThrowsIllegalArg(() => CmsLeg.builder().capSchedule(CAP).floorSchedule(FLOOR).index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build());
		// stub is on
		assertThrowsIllegalArg(() => CmsLeg.builder().index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.SHORT_INITIAL, RollConventions.NONE)).build());
	  }

	  public virtual void test_resolve()
	  {
		CmsLeg baseFloor = CmsLeg.builder().floorSchedule(FLOOR).index(INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE_EUR).build();
		ResolvedCmsLeg resolvedFloor = baseFloor.resolve(REF_DATA);
		LocalDate end1 = LocalDate.of(2016, 10, 21);
		LocalDate fixing1 = EUR_EURIBOR_6M.calculateFixingFromEffective(START, REF_DATA);
		LocalDate fixing2 = EUR_EURIBOR_6M.calculateFixingFromEffective(end1, REF_DATA);
		LocalDate fixing3 = EUR_EURIBOR_6M.calculateFixingFromEffective(END, REF_DATA);
		LocalDate endDate = SCHEDULE_EUR.calculatedEndDate().adjusted(REF_DATA);

		CmsPeriod period1 = CmsPeriod.builder().currency(EUR).floorlet(FLOOR.InitialValue).notional(-NOTIONAL.InitialValue).index(INDEX).startDate(START).endDate(end1).unadjustedStartDate(START).unadjustedEndDate(end1).fixingDate(fixing1).paymentDate(end1).yearFraction(EUR_EURIBOR_6M.DayCount.yearFraction(START, end1)).dayCount(EUR_EURIBOR_6M.DayCount).underlyingSwap(createUnderlyingSwap(fixing1)).build();
		CmsPeriod period2 = CmsPeriod.builder().currency(EUR).floorlet(FLOOR.Steps[0].Value.ModifyingValue).notional(-NOTIONAL.Steps[0].Value.ModifyingValue).index(INDEX).startDate(end1).endDate(endDate).unadjustedStartDate(end1).unadjustedEndDate(END).fixingDate(fixing2).paymentDate(endDate).yearFraction(EUR_EURIBOR_6M.DayCount.yearFraction(end1, endDate)).dayCount(EUR_EURIBOR_6M.DayCount).underlyingSwap(createUnderlyingSwap(fixing2)).build();
		assertEquals(resolvedFloor.Currency, EUR);
		assertEquals(resolvedFloor.StartDate, baseFloor.StartDate.adjusted(REF_DATA));
		assertEquals(resolvedFloor.EndDate, baseFloor.EndDate.adjusted(REF_DATA));
		assertEquals(resolvedFloor.Index, INDEX);
		assertEquals(resolvedFloor.PayReceive, PAY);
		assertEquals(resolvedFloor.CmsPeriods.size(), 2);
		assertEquals(resolvedFloor.CmsPeriods.get(0), period1);
		assertEquals(resolvedFloor.CmsPeriods.get(1), period2);

		CmsLeg baseFloorEnd = CmsLeg.builder().floorSchedule(FLOOR).fixingRelativeTo(FixingRelativeTo.PERIOD_END).index(INDEX).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE_EUR).build();
		ResolvedCmsLeg resolvedFloorEnd = baseFloorEnd.resolve(REF_DATA);
		CmsPeriod period1End = CmsPeriod.builder().currency(EUR).floorlet(FLOOR.InitialValue).notional(-NOTIONAL.InitialValue).index(INDEX).startDate(START).endDate(end1).unadjustedStartDate(START).unadjustedEndDate(end1).fixingDate(fixing2).paymentDate(end1).yearFraction(EUR_EURIBOR_6M.DayCount.yearFraction(START, end1)).dayCount(EUR_EURIBOR_6M.DayCount).underlyingSwap(createUnderlyingSwap(fixing2)).build();
		CmsPeriod period2End = CmsPeriod.builder().currency(EUR).floorlet(FLOOR.Steps[0].Value.ModifyingValue).notional(-NOTIONAL.Steps[0].Value.ModifyingValue).index(INDEX).startDate(end1).endDate(endDate).unadjustedStartDate(end1).unadjustedEndDate(END).fixingDate(fixing3).paymentDate(endDate).yearFraction(EUR_EURIBOR_6M.DayCount.yearFraction(end1, endDate)).dayCount(EUR_EURIBOR_6M.DayCount).underlyingSwap(createUnderlyingSwap(fixing3)).build();
		assertEquals(resolvedFloorEnd.Currency, EUR);
		assertEquals(resolvedFloorEnd.StartDate, baseFloor.StartDate.adjusted(REF_DATA));
		assertEquals(resolvedFloorEnd.EndDate, baseFloor.EndDate.adjusted(REF_DATA));
		assertEquals(resolvedFloorEnd.Index, INDEX);
		assertEquals(resolvedFloorEnd.PayReceive, PAY);
		assertEquals(resolvedFloorEnd.CmsPeriods.size(), 2);
		assertEquals(resolvedFloorEnd.CmsPeriods.get(0), period1End);
		assertEquals(resolvedFloorEnd.CmsPeriods.get(1), period2End);

		CmsLeg baseCap = CmsLeg.builder().index(INDEX).capSchedule(CAP).notional(NOTIONAL).payReceive(PAY).paymentSchedule(SCHEDULE_EUR).paymentDateOffset(PAYMENT_OFFSET).build();
		ResolvedCmsLeg resolvedCap = baseCap.resolve(REF_DATA);
		CmsPeriod periodCap1 = CmsPeriod.builder().currency(EUR).notional(-NOTIONAL.InitialValue).index(INDEX).caplet(CAP.InitialValue).startDate(START).endDate(end1).unadjustedStartDate(START).unadjustedEndDate(end1).fixingDate(fixing1).paymentDate(PAYMENT_OFFSET.adjust(end1, REF_DATA)).yearFraction(EUR_EURIBOR_6M.DayCount.yearFraction(START, end1)).dayCount(EUR_EURIBOR_6M.DayCount).underlyingSwap(createUnderlyingSwap(fixing1)).build();
		CmsPeriod periodCap2 = CmsPeriod.builder().currency(EUR).notional(-NOTIONAL.Steps[0].Value.ModifyingValue).index(INDEX).caplet(CAP.InitialValue).startDate(end1).endDate(endDate).unadjustedStartDate(end1).unadjustedEndDate(END).fixingDate(fixing2).paymentDate(PAYMENT_OFFSET.adjust(endDate, REF_DATA)).yearFraction(EUR_EURIBOR_6M.DayCount.yearFraction(end1, endDate)).dayCount(EUR_EURIBOR_6M.DayCount).underlyingSwap(createUnderlyingSwap(fixing2)).build();
		assertEquals(resolvedCap.Currency, EUR);
		assertEquals(resolvedCap.StartDate, baseCap.StartDate.adjusted(REF_DATA));
		assertEquals(resolvedCap.EndDate, baseCap.EndDate.adjusted(REF_DATA));
		assertEquals(resolvedCap.Index, INDEX);
		assertEquals(resolvedCap.PayReceive, PAY);
		assertEquals(resolvedCap.CmsPeriods.size(), 2);
		assertEquals(resolvedCap.CmsPeriods.get(0), periodCap1);
		assertEquals(resolvedCap.CmsPeriods.get(1), periodCap2);
	  }

	  private ResolvedSwap createUnderlyingSwap(LocalDate fixingDate)
	  {
		FixedIborSwapConvention conv = INDEX.Template.Convention;
		LocalDate effectiveDate = conv.calculateSpotDateFromTradeDate(fixingDate, REF_DATA);
		LocalDate maturityDate = effectiveDate.plus(INDEX.Template.Tenor);
		Swap swap = conv.toTrade(fixingDate, effectiveDate, maturityDate, BuySell.BUY, 1d, 1d).Product;
		return swap.resolve(REF_DATA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sutCap());
		coverBeanEquals(sutCap(), sutFloor());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sutCap());
	  }

	  //-------------------------------------------------------------------------
	  internal static CmsLeg sutCap()
	  {
		return CmsLeg.builder().capSchedule(CAP).index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build();
	  }

	  internal static CmsLeg sutFloor()
	  {
		return CmsLeg.builder().floorSchedule(FLOOR).index(SwapIndices.USD_LIBOR_1100_10Y).notional(ValueSchedule.of(1.e6)).payReceive(PAY).paymentSchedule(SCHEDULE).fixingRelativeTo(FixingRelativeTo.PERIOD_END).fixingDateOffset(FIXING_OFFSET).paymentDateOffset(FIXING_OFFSET).dayCount(ACT_365_ACTUAL).build();
	  }

	}

}
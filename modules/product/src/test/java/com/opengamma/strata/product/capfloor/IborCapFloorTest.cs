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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Test <seealso cref="IborCapFloor"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapFloorTest
	public class IborCapFloorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate START = LocalDate.of(2011, 3, 17);
	  private static readonly LocalDate END = LocalDate.of(2016, 3, 17);
	  private static readonly IborRateCalculation RATE_CALCULATION = IborRateCalculation.of(EUR_EURIBOR_3M);
	  private static readonly Frequency FREQUENCY = Frequency.P3M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE = PeriodicSchedule.builder().startDate(START).endDate(END).frequency(FREQUENCY).businessDayAdjustment(BUSS_ADJ).build();
	  private static readonly DaysAdjustment PAYMENT_OFFSET = DaysAdjustment.ofBusinessDays(2, EUTA);
	  private static readonly ValueSchedule CAP = ValueSchedule.of(0.0325);
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(1.0e6);
	  private static readonly IborCapFloorLeg CAPFLOOR_LEG = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).capSchedule(CAP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
	  private static readonly SwapLeg PAY_LEG = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(SCHEDULE).calculation(FixedRateCalculation.of(0.001, ACT_360)).paymentSchedule(PaymentSchedule.builder().paymentFrequency(FREQUENCY).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(EUR, NOTIONAL)).build();
	  private static readonly SwapLeg PAY_LEG_XCCY = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(SCHEDULE).calculation(IborRateCalculation.of(GBP_LIBOR_3M)).paymentSchedule(PaymentSchedule.builder().paymentFrequency(FREQUENCY).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(GBP, NOTIONAL)).build();

	  public virtual void test_of_oneLeg()
	  {
		IborCapFloor test = IborCapFloor.of(CAPFLOOR_LEG);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG);
		assertEquals(test.PayLeg.Present, false);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(EUR));
		assertEquals(test.allCurrencies(), ImmutableSet.of(EUR));
		assertEquals(test.allIndices(), ImmutableSet.of(EUR_EURIBOR_3M));
	  }

	  public virtual void test_of_twoLegs()
	  {
		IborCapFloor test = IborCapFloor.of(CAPFLOOR_LEG, PAY_LEG);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG);
		assertEquals(test.PayLeg.get(), PAY_LEG);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(EUR));
		assertEquals(test.allCurrencies(), ImmutableSet.of(EUR));
		assertEquals(test.allIndices(), ImmutableSet.of(EUR_EURIBOR_3M));
	  }

	  public virtual void test_of_twoLegs_xccy()
	  {
		IborCapFloor test = IborCapFloor.of(CAPFLOOR_LEG, PAY_LEG_XCCY);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG);
		assertEquals(test.PayLeg.get(), PAY_LEG_XCCY);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP, EUR));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP, EUR));
		assertEquals(test.allIndices(), ImmutableSet.of(GBP_LIBOR_3M, EUR_EURIBOR_3M));
	  }

	  public virtual void test_resolve_oneLeg()
	  {
		IborCapFloor @base = IborCapFloor.of(CAPFLOOR_LEG);
		ResolvedIborCapFloor test = @base.resolve(REF_DATA);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG.resolve(REF_DATA));
		assertEquals(test.PayLeg.Present, false);
	  }

	  public virtual void test_resolve_twoLegs()
	  {
		IborCapFloor @base = IborCapFloor.of(CAPFLOOR_LEG, PAY_LEG);
		ResolvedIborCapFloor test = @base.resolve(REF_DATA);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG.resolve(REF_DATA));
		assertEquals(test.PayLeg.get(), PAY_LEG.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborCapFloor test1 = IborCapFloor.of(CAPFLOOR_LEG);
		coverImmutableBean(test1);
		IborCapFloorLeg capFloor = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(CAP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
		IborCapFloor test2 = IborCapFloor.of(capFloor, PAY_LEG);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborCapFloor test = IborCapFloor.of(CAPFLOOR_LEG);
		assertSerialization(test);
	  }

	}

}
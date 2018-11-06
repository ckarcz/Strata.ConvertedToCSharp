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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Test <seealso cref="Cms"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsTest
	public class CmsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(1.0e6);
	  private static readonly SwapIndex INDEX = SwapIndices.EUR_EURIBOR_1100_10Y;
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 21);
	  private static readonly LocalDate END = LocalDate.of(2017, 10, 21);
	  private static readonly Frequency FREQUENCY = Frequency.P12M;
	  private static readonly BusinessDayAdjustment BUSS_ADJ_EUR = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, EUTA);
	  private static readonly PeriodicSchedule SCHEDULE_EUR = PeriodicSchedule.of(START, END, FREQUENCY, BUSS_ADJ_EUR, StubConvention.NONE, RollConventions.NONE);
	  private static readonly ValueSchedule STRIKE = ValueSchedule.of(0.0125);
	  private static readonly CmsLeg CMS_LEG = CmsLegTest.sutCap();
	  private static readonly SwapLeg PAY_LEG = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(SCHEDULE_EUR).calculation(FixedRateCalculation.of(0.01, ACT_360)).paymentSchedule(PaymentSchedule.builder().paymentFrequency(FREQUENCY).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(CurrencyAmount.of(EUR, 1.0e6))).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_twoLegs()
	  {
		Cms test = sutCap();
		assertEquals(test.CmsLeg, CMS_LEG);
		assertEquals(test.PayLeg.get(), PAY_LEG);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(CMS_LEG.Currency));
		assertEquals(test.allCurrencies(), ImmutableSet.of(CMS_LEG.Currency));
		assertEquals(test.allRateIndices(), ImmutableSet.of(CMS_LEG.UnderlyingIndex));
	  }

	  public virtual void test_of_oneLeg()
	  {
		Cms test = Cms.of(CMS_LEG);
		assertEquals(test.CmsLeg, CMS_LEG);
		assertFalse(test.PayLeg.Present);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(CMS_LEG.Currency));
		assertEquals(test.allCurrencies(), ImmutableSet.of(CMS_LEG.Currency));
	  }

	  public virtual void test_resolve_twoLegs()
	  {
		Cms @base = sutCap();
		ResolvedCms test = @base.resolve(REF_DATA);
		assertEquals(test.CmsLeg, CMS_LEG.resolve(REF_DATA));
		assertEquals(test.PayLeg.get(), PAY_LEG.resolve(REF_DATA));
	  }

	  public virtual void test_resolve_oneLeg()
	  {
		Cms @base = Cms.of(CMS_LEG);
		ResolvedCms test = @base.resolve(REF_DATA);
		assertEquals(test.CmsLeg, CMS_LEG.resolve(REF_DATA));
		assertFalse(test.PayLeg.Present);
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
	  internal static Cms sutCap()
	  {
		return Cms.of(CMS_LEG, PAY_LEG);
	  }

	  internal static Cms sutFloor()
	  {
		return Cms.of(CmsLeg.builder().floorSchedule(STRIKE).index(INDEX).notional(NOTIONAL).payReceive(RECEIVE).paymentSchedule(SCHEDULE_EUR).build());
	  }

	}

}
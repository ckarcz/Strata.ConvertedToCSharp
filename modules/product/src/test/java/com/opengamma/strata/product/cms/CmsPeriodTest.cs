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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapIndices = com.opengamma.strata.product.swap.SwapIndices;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Test <seealso cref="CmsPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsPeriodTest
	public class CmsPeriodTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly SwapIndex INDEX = SwapIndices.GBP_LIBOR_1100_15Y;
	  private static readonly LocalDate FIXING = LocalDate.of(2015, 10, 16);
	  private static readonly LocalDate START = LocalDate.of(2015, 10, 22);
	  private static readonly LocalDate END = LocalDate.of(2016, 10, 24);
	  private static readonly LocalDate START_UNADJUSTED = LocalDate.of(2015, 10, 22);
	  private static readonly LocalDate END_UNADJUSTED = LocalDate.of(2016, 10, 22); // SAT
	  private static readonly LocalDate PAYMENT = LocalDate.of(2016, 10, 26);
	  private const double STRIKE = 0.015;
	  private const double NOTIONAL = 1.0e6;
	  private const double YEAR_FRACTION = 1.005;

	  public virtual void test_builder_cap()
	  {
		CmsPeriod testCaplet = sutCap();
		assertEquals(testCaplet.Caplet.Value, STRIKE);
		assertFalse(testCaplet.Floorlet.HasValue);
		assertEquals(testCaplet.CmsPeriodType, CmsPeriodType.CAPLET);
		assertEquals(testCaplet.Currency, GBP);
		assertEquals(testCaplet.StartDate, START);
		assertEquals(testCaplet.EndDate, END);
		assertEquals(testCaplet.UnadjustedStartDate, START_UNADJUSTED);
		assertEquals(testCaplet.UnadjustedEndDate, END_UNADJUSTED);
		assertEquals(testCaplet.FixingDate, FIXING);
		assertEquals(testCaplet.PaymentDate, PAYMENT);
		assertEquals(testCaplet.Index, INDEX);
		assertEquals(testCaplet.Notional, NOTIONAL);
		assertEquals(testCaplet.YearFraction, YEAR_FRACTION);
		assertEquals(testCaplet.DayCount, ACT_360);
		assertEquals(testCaplet.Strike, STRIKE);
	  }

	  public virtual void test_builder_floor()
	  {
		CmsPeriod testFloorlet = sutFloor();
		assertFalse(testFloorlet.Caplet.HasValue);
		assertEquals(testFloorlet.Floorlet.Value, STRIKE);
		assertEquals(testFloorlet.CmsPeriodType, CmsPeriodType.FLOORLET);
		assertEquals(testFloorlet.Currency, GBP);
		assertEquals(testFloorlet.StartDate, START);
		assertEquals(testFloorlet.EndDate, END);
		assertEquals(testFloorlet.UnadjustedStartDate, START_UNADJUSTED);
		assertEquals(testFloorlet.UnadjustedEndDate, END_UNADJUSTED);
		assertEquals(testFloorlet.FixingDate, FIXING);
		assertEquals(testFloorlet.PaymentDate, PAYMENT);
		assertEquals(testFloorlet.Index, INDEX);
		assertEquals(testFloorlet.Notional, NOTIONAL);
		assertEquals(testFloorlet.YearFraction, YEAR_FRACTION);
		assertEquals(testFloorlet.DayCount, ACT_360);
		assertEquals(testFloorlet.Strike, STRIKE);
	  }

	  public virtual void test_builder_coupon()
	  {
		CmsPeriod testCoupon = sutCoupon();
		assertFalse(testCoupon.Caplet.HasValue);
		assertFalse(testCoupon.Floorlet.HasValue);
		assertEquals(testCoupon.CmsPeriodType, CmsPeriodType.COUPON);
		assertEquals(testCoupon.Currency, GBP);
		assertEquals(testCoupon.StartDate, START);
		assertEquals(testCoupon.EndDate, END);
		assertEquals(testCoupon.UnadjustedStartDate, START_UNADJUSTED);
		assertEquals(testCoupon.UnadjustedEndDate, END_UNADJUSTED);
		assertEquals(testCoupon.FixingDate, FIXING);
		assertEquals(testCoupon.PaymentDate, PAYMENT);
		assertEquals(testCoupon.Index, INDEX);
		assertEquals(testCoupon.Notional, NOTIONAL);
		assertEquals(testCoupon.YearFraction, YEAR_FRACTION);
		assertEquals(testCoupon.DayCount, ACT_360);
		assertEquals(testCoupon.Strike, 0d);
	  }

	  public virtual void test_builder_nonNullCapFloor()
	  {
		assertThrowsIllegalArg(() => CmsPeriod.builder().caplet(STRIKE).floorlet(STRIKE).startDate(START).endDate(END).index(INDEX).notional(NOTIONAL).yearFraction(YEAR_FRACTION).dayCount(ACT_360).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sutCap());
		coverBeanEquals(sutCap(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sutCap());
	  }

	  public virtual void test_toCouponEquivalent()
	  {
		CmsPeriod caplet = sutCap();
		CmsPeriod cpnEquivalent = caplet.toCouponEquivalent();

		assertEquals(cpnEquivalent.CmsPeriodType, CmsPeriodType.COUPON);
		assertEquals(caplet.Currency, cpnEquivalent.Currency);
		assertEquals(caplet.StartDate, cpnEquivalent.StartDate);
		assertEquals(caplet.EndDate, cpnEquivalent.EndDate);
		assertEquals(caplet.UnadjustedStartDate, cpnEquivalent.UnadjustedStartDate);
		assertEquals(caplet.UnadjustedEndDate, cpnEquivalent.UnadjustedEndDate);
		assertEquals(caplet.FixingDate, cpnEquivalent.FixingDate);
		assertEquals(caplet.PaymentDate, cpnEquivalent.PaymentDate);
		assertEquals(caplet.Index, cpnEquivalent.Index);
		assertEquals(caplet.Notional, cpnEquivalent.Notional);
		assertEquals(caplet.YearFraction, cpnEquivalent.YearFraction);
		assertEquals(caplet.DayCount, cpnEquivalent.DayCount);
	  }

	  //-------------------------------------------------------------------------
	  internal static CmsPeriod sutCap()
	  {
		FixedIborSwapConvention conv = INDEX.Template.Convention;
		ResolvedSwap swap = conv.toTrade(FIXING, START, END, BuySell.BUY, 1d, 0.01).Product.resolve(REF_DATA);
		return CmsPeriod.builder().currency(GBP).notional(NOTIONAL).startDate(START).endDate(END).unadjustedStartDate(START_UNADJUSTED).unadjustedEndDate(END_UNADJUSTED).yearFraction(YEAR_FRACTION).paymentDate(PAYMENT).fixingDate(FIXING).caplet(STRIKE).dayCount(ACT_360).index(INDEX).underlyingSwap(swap).build();
	  }

	  internal static CmsPeriod sutFloor()
	  {
		FixedIborSwapConvention conv = INDEX.Template.Convention;
		ResolvedSwap swap = conv.toTrade(FIXING, START, END, BuySell.BUY, 1d, 0.01).Product.resolve(REF_DATA);
		return CmsPeriod.builder().currency(GBP).notional(NOTIONAL).startDate(START).endDate(END).unadjustedStartDate(START_UNADJUSTED).unadjustedEndDate(END_UNADJUSTED).yearFraction(YEAR_FRACTION).paymentDate(PAYMENT).fixingDate(FIXING).floorlet(STRIKE).dayCount(ACT_360).index(INDEX).underlyingSwap(swap).build();
	  }

	  internal static CmsPeriod sutCoupon()
	  {
		FixedIborSwapConvention conv = INDEX.Template.Convention;
		ResolvedSwap swap = conv.toTrade(FIXING, START, END, BuySell.BUY, 1d, 0.01).Product.resolve(REF_DATA);
		return CmsPeriod.builder().currency(GBP).notional(NOTIONAL).startDate(START).endDate(END).unadjustedStartDate(START_UNADJUSTED).unadjustedEndDate(END_UNADJUSTED).yearFraction(YEAR_FRACTION).paymentDate(PAYMENT).fixingDate(FIXING).dayCount(ACT_360).index(INDEX).underlyingSwap(swap).build();
	  }

	  internal static CmsPeriod sut2()
	  {
		FixedIborSwapConvention conv = INDEX.Template.Convention;
		ResolvedSwap swap = conv.toTrade(FIXING.plusDays(1), START.plusDays(1), END.plusDays(1), BuySell.BUY, 1d, 1d).Product.resolve(REF_DATA);
		return CmsPeriod.builder().currency(EUR).notional(NOTIONAL + 1).startDate(START.plusDays(1)).endDate(END.plusDays(1)).unadjustedStartDate(START_UNADJUSTED.plusDays(1)).unadjustedEndDate(END_UNADJUSTED.plusDays(1)).yearFraction(YEAR_FRACTION + 0.01).paymentDate(PAYMENT.plusDays(1)).fixingDate(FIXING.plusDays(1)).floorlet(STRIKE).dayCount(ACT_365F).index(SwapIndices.EUR_EURIBOR_1100_5Y).underlyingSwap(swap).build();
	  }

	}

}
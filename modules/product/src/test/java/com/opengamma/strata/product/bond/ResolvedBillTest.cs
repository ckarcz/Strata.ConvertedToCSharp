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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Test <seealso cref="ResolvedBill"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedBillTest
	public class ResolvedBillTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private const double TOLERANCE_PRICE = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_getters()
	  {
		ResolvedBill test = sut();
		assertEquals(test.SecurityId, BillTest.US_BILL.SecurityId);
		assertEquals(test.Currency, BillTest.US_BILL.Currency);
		assertEquals(test.Notional, BillTest.US_BILL.Notional.resolve(REF_DATA));
		assertEquals(test.DayCount, BillTest.US_BILL.DayCount);
		assertEquals(test.YieldConvention, BillTest.US_BILL.YieldConvention);
		assertEquals(test.LegalEntityId, BillTest.US_BILL.LegalEntityId);
		assertEquals(test.SettlementDateOffset, BillTest.US_BILL.SettlementDateOffset);
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
	  public virtual void price_from_yield_discount()
	  {
		ResolvedBill bill = sut();
		double yield = 0.01;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = bill.DayCount.relativeYearFraction(settlementDate, bill.Notional.Date);
		double priceExpected = 1.0d - yield * af;
		double priceComputed = bill.priceFromYield(yield, settlementDate);
		assertEquals(priceExpected, priceComputed, TOLERANCE_PRICE);
	  }

	  public virtual void yield_from_price_discount()
	  {
		ResolvedBill bill = sut();
		double price = 0.99;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = bill.DayCount.relativeYearFraction(settlementDate, bill.Notional.Date);
		double yieldExpected = (1.0d - price) / af;
		double yieldComputed = bill.yieldFromPrice(price, settlementDate);
		assertEquals(yieldExpected, yieldComputed, TOLERANCE_PRICE);
	  }

	  public virtual void price_from_yield_intatmat()
	  {
		ResolvedBill bill = BillTest.US_BILL.toBuilder().yieldConvention(BillYieldConvention.INTEREST_AT_MATURITY).build().resolve(REF_DATA);
		double yield = 0.01;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = bill.DayCount.relativeYearFraction(settlementDate, bill.Notional.Date);
		double priceExpected = 1.0d / (1 + yield * af);
		double priceComputed = bill.priceFromYield(yield, settlementDate);
		assertEquals(priceExpected, priceComputed, TOLERANCE_PRICE);
	  }

	  public virtual void yield_from_price_intatmat()
	  {
		ResolvedBill bill = BillTest.US_BILL.toBuilder().yieldConvention(BillYieldConvention.INTEREST_AT_MATURITY).build().resolve(REF_DATA);
		double price = 0.99;
		LocalDate settlementDate = LocalDate.of(2018, 8, 17);
		double af = bill.DayCount.relativeYearFraction(settlementDate, bill.Notional.Date);
		double yieldExpected = (1.0d / price - 1.0d) / af;
		double yieldComputed = bill.yieldFromPrice(price, settlementDate);
		assertEquals(yieldExpected, yieldComputed, TOLERANCE_PRICE);
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedBill sut()
	  {
		return BillTest.US_BILL.resolve(REF_DATA);
	  }

	  internal static ResolvedBill sut2()
	  {
		return BillTest.BILL_2.resolve(REF_DATA);
	  }

	}

}
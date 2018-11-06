/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;

	/// <summary>
	/// Test <seealso cref="CapitalIndexedBondTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondTradeTest
	public class CapitalIndexedBondTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double QUANTITY = 10;
	  private const double QUANTITY2 = 20;
	  private const double PRICE = 0.995;
	  private const double PRICE2 = 0.9;
	  private static readonly BusinessDayAdjustment SCHEDULE_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USNY);
	  private static readonly CapitalIndexedBond PRODUCT = CapitalIndexedBondTest.sut();
	  private static readonly CapitalIndexedBond PRODUCT1 = CapitalIndexedBondTest.sut1();
	  private static readonly CapitalIndexedBond PRODUCT2 = CapitalIndexedBondTest.sut2();
	  private static readonly LocalDate START = PRODUCT.AccrualSchedule.StartDate;
	  private static readonly LocalDate TRADE = START.plusDays(7);
	  private static readonly LocalDate SETTLEMENT_DATE = SCHEDULE_ADJ.adjust(TRADE, REF_DATA);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(TRADE).settlementDate(SETTLEMENT_DATE).build();
	  private static readonly TradeInfo TRADE_INFO_EARLY = TradeInfo.builder().tradeDate(date(2008, 1, 1)).settlementDate(date(2008, 1, 1)).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		CapitalIndexedBondTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Price, PRICE);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withPrice(129).Price, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  private static readonly CapitalIndexedBondPaymentPeriod SETTLEMENT = CapitalIndexedBondPaymentPeriod.builder().startDate(SCHEDULE_ADJ.adjust(START, REF_DATA)).unadjustedStartDate(START).endDate(SETTLEMENT_DATE).currency(USD).rateComputation(PRODUCT.RateCalculation.createRateComputation(SETTLEMENT_DATE)).notional(-PRODUCT.Notional * QUANTITY * (PRICE + PRODUCT.resolve(REF_DATA).accruedInterest(SETTLEMENT_DATE) / PRODUCT.Notional)).realCoupon(1d).build();

	  private static readonly KnownAmountBondPaymentPeriod SETTLEMENT1 = KnownAmountBondPaymentPeriod.builder().startDate(SCHEDULE_ADJ.adjust(START, REF_DATA)).unadjustedStartDate(START).endDate(SETTLEMENT_DATE).payment(Payment.of(USD, -PRODUCT1.Notional * QUANTITY * (PRICE + PRODUCT1.resolve(REF_DATA).accruedInterest(SETTLEMENT_DATE) / PRODUCT1.Notional), SETTLEMENT_DATE)).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		CapitalIndexedBondTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.BOND).currencies(Currency.USD).description("Bond x 10").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedCapitalIndexedBondTrade test = sut().resolve(REF_DATA);
		ResolvedCapitalIndexedBondTrade expected = ResolvedCapitalIndexedBondTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).quantity(QUANTITY).settlement(ResolvedCapitalIndexedBondSettlement.of(SETTLEMENT_DATE, PRICE, SETTLEMENT)).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_resolve1()
	  {
		ResolvedCapitalIndexedBondTrade test = sut1().resolve(REF_DATA);
		ResolvedCapitalIndexedBondTrade expected = ResolvedCapitalIndexedBondTrade.builder().info(TRADE_INFO).product(PRODUCT1.resolve(REF_DATA)).quantity(QUANTITY).settlement(ResolvedCapitalIndexedBondSettlement.of(SETTLEMENT_DATE, PRICE, SETTLEMENT1)).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_resolve_invalid()
	  {
		CapitalIndexedBondTrade test = sut().toBuilder().info(TRADE_INFO_EARLY).build();
		assertThrowsIllegalArg(() => test.resolve(REF_DATA));
	  }

	  public virtual void test_resolve_noTradeOrSettlementDate()
	  {
		CapitalIndexedBondTrade test = CapitalIndexedBondTrade.builder().info(TradeInfo.empty()).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
		assertThrows(() => test.resolve(REF_DATA), typeof(System.InvalidOperationException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		CapitalIndexedBondTrade @base = sut();
		double quantity = 3456d;
		CapitalIndexedBondTrade computed = @base.withQuantity(quantity);
		CapitalIndexedBondTrade expected = CapitalIndexedBondTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(quantity).price(PRICE).build();
		assertEquals(computed, expected);
	  }

	  public virtual void test_withPrice()
	  {
		CapitalIndexedBondTrade @base = sut();
		double price = 0.95;
		CapitalIndexedBondTrade computed = @base.withPrice(price);
		CapitalIndexedBondTrade expected = CapitalIndexedBondTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(price).build();
		assertEquals(computed, expected);
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
	  internal static CapitalIndexedBondTrade sut()
	  {
		return CapitalIndexedBondTrade.builder().info(TRADE_INFO).product(PRODUCT).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static CapitalIndexedBondTrade sut1()
	  {
		return CapitalIndexedBondTrade.builder().info(TRADE_INFO).product(PRODUCT1).quantity(QUANTITY).price(PRICE).build();
	  }

	  internal static CapitalIndexedBondTrade sut2()
	  {
		return CapitalIndexedBondTrade.builder().info(TradeInfo.builder().tradeDate(START.plusDays(7)).build()).product(PRODUCT2).quantity(QUANTITY2).price(PRICE2).build();
	  }

	}

}
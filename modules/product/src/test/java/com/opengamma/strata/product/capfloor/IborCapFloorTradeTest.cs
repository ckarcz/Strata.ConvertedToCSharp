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
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// Test <seealso cref="IborCapFloorTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapFloorTradeTest
	public class IborCapFloorTradeTest
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
	  private const double NOTIONAL_VALUE = 1.0e6;
	  private static readonly ValueSchedule NOTIONAL = ValueSchedule.of(NOTIONAL_VALUE);
	  private static readonly IborCapFloorLeg CAP_LEG = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).capSchedule(CAP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
	  private static readonly IborCapFloorLeg FLOOR_LEG = IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(CAP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(RECEIVE).build();
	  private static readonly IborCapFloor PRODUCT = IborCapFloor.of(CAP_LEG);
	  private static readonly IborCapFloor PRODUCT_FLOOR = IborCapFloor.of(FLOOR_LEG);
	  private static readonly AdjustablePayment PREMIUM = AdjustablePayment.of(CurrencyAmount.of(EUR, NOTIONAL_VALUE), LocalDate.of(2011, 3, 18));
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.builder().tradeDate(LocalDate.of(2011, 3, 15)).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_full()
	  {
		IborCapFloorTrade test = sut();
		assertEquals(test.Premium.get(), PREMIUM);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.withInfo(TRADE_INFO).Info, TRADE_INFO);
	  }

	  public virtual void test_builder_min()
	  {
		IborCapFloorTrade test = IborCapFloorTrade.builder().product(PRODUCT).build();
		assertEquals(test.Premium.Present, false);
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TradeInfo.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		IborCapFloorTrade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.IBOR_CAP_FLOOR).currencies(Currency.EUR).description("5Y EUR 1mm Rec EUR-EURIBOR-3M Cap 3.25% / Pay Premium : 17Mar11-17Mar16").build();
		assertEquals(trade.summarize(), expected);
	  }

	  public virtual void test_summarize_floor()
	  {
		IborCapFloorTrade trade = IborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT_FLOOR).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(TRADE_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.IBOR_CAP_FLOOR).currencies(Currency.EUR).description("5Y EUR 1mm Rec EUR-EURIBOR-3M Floor 3.25% : 17Mar11-17Mar16").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		IborCapFloorTrade test = sut();
		ResolvedIborCapFloorTrade expected = ResolvedIborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).premium(PREMIUM.resolve(REF_DATA)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_noPremium()
	  {
		IborCapFloorTrade test = IborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT).build();
		ResolvedIborCapFloorTrade expected = ResolvedIborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT.resolve(REF_DATA)).build();
		assertEquals(test.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborCapFloorTrade test1 = sut();
		coverImmutableBean(test1);
		IborCapFloor product = IborCapFloor.of(IborCapFloorLeg.builder().calculation(RATE_CALCULATION).floorSchedule(CAP).notional(NOTIONAL).paymentDateOffset(PAYMENT_OFFSET).paymentSchedule(SCHEDULE).payReceive(PAY).build());
		IborCapFloorTrade test2 = IborCapFloorTrade.builder().product(product).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborCapFloorTrade test = sut();
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal virtual IborCapFloorTrade sut()
	  {
		return IborCapFloorTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
	  }

	}

}
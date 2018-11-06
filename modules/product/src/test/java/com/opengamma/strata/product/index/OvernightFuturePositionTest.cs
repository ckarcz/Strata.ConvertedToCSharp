/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="OvernightFuturePosition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightFuturePositionTest
	public class OvernightFuturePositionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly PositionInfo POSITION_INFO = PositionInfo.builder().id(StandardId.of("A", "B")).build();
	  private static readonly PositionInfo POSITION_INFO2 = PositionInfo.builder().id(StandardId.of("A", "C")).build();
	  private const double QUANTITY = 10;
	  private const double NOTIONAL = 5_000_000d;
	  private const double NOTIONAL2 = 10_000_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly double ACCRUAL_FACTOR2 = TENOR_3M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 9, 28);
	  private static readonly LocalDate START_DATE = date(2018, 9, 1);
	  private static readonly LocalDate END_DATE = date(2018, 9, 30);
	  private static readonly LocalDate LAST_TRADE_DATE2 = date(2018, 6, 15);
	  private static readonly LocalDate START_DATE2 = date(2018, 3, 15);
	  private static readonly LocalDate END_DATE2 = date(2018, 6, 15);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(5);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "OnFuture2");
	  private static readonly OvernightFuture PRODUCT = OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build();
	  private static readonly OvernightFuture PRODUCT2 = OvernightFuture.builder().securityId(SECURITY_ID2).currency(GBP).notional(NOTIONAL2).accrualFactor(ACCRUAL_FACTOR2).startDate(START_DATE2).endDate(END_DATE2).lastTradeDate(LAST_TRADE_DATE2).index(GBP_SONIA).accrualMethod(OvernightAccrualMethod.COMPOUNDED).rounding(Rounding.none()).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		OvernightFuturePosition test = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(QUANTITY).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.LongQuantity, QUANTITY, 0d);
		assertEquals(test.ShortQuantity, 0d, 0d);
		assertEquals(test.Quantity, QUANTITY, 0d);
		assertEquals(test.withInfo(POSITION_INFO).Info, POSITION_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		OvernightFuturePosition test = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(QUANTITY).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(POSITION_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.OVERNIGHT_FUTURE).currencies(Currency.USD).description("OnFuture x 10").build();
		assertEquals(test.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withQuantity()
	  {
		OvernightFuturePosition @base = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(QUANTITY).build();
		double quantity = 75343d;
		OvernightFuturePosition computed = @base.withQuantity(quantity);
		OvernightFuturePosition expected = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(quantity).build();
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		OvernightFuturePosition @base = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(QUANTITY).build();
		ResolvedOvernightFutureTrade expected = ResolvedOvernightFutureTrade.builder().info(POSITION_INFO).product(PRODUCT.resolve(REF_DATA)).quantity(QUANTITY).build();
		assertEquals(@base.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightFuturePosition test1 = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(QUANTITY).build();
		coverImmutableBean(test1);
		OvernightFuturePosition test2 = OvernightFuturePosition.builder().info(POSITION_INFO2).product(PRODUCT2).longQuantity(100).shortQuantity(50).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightFuturePosition test = OvernightFuturePosition.builder().info(POSITION_INFO).product(PRODUCT).longQuantity(QUANTITY).build();
		assertSerialization(test);
	  }

	}

}
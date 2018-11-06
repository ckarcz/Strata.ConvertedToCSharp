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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using TestHelper = com.opengamma.strata.collect.TestHelper;

	/// <summary>
	/// Test <seealso cref="OvernightFutureSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightFutureSecurityTest
	public class OvernightFutureSecurityTest
	{

	  private static readonly OvernightFuture PRODUCT = OvernightFutureTest.sut();
	  private static readonly OvernightFuture PRODUCT2 = OvernightFutureTest.sut2();
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(PRODUCT.SecurityId, PRICE_INFO);
	  private static readonly SecurityInfo INFO2 = SecurityInfo.of(PRODUCT2.SecurityId, PRICE_INFO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		OvernightFutureSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		OvernightFutureSecurity test = sut();
		assertEquals(test.createProduct(ReferenceData.empty()), PRODUCT);
		TradeInfo tradeInfo = TradeInfo.of(date(2016, 6, 30));
		OvernightFutureTrade expectedTrade = OvernightFutureTrade.builder().info(tradeInfo).product(PRODUCT).quantity(100).price(0.995).build();
		assertEquals(test.createTrade(tradeInfo, 100, 0.995, ReferenceData.empty()), expectedTrade);

		PositionInfo positionInfo = PositionInfo.empty();
		OvernightFuturePosition expectedPosition1 = OvernightFuturePosition.builder().info(positionInfo).product(PRODUCT).longQuantity(100).build();
		TestHelper.assertEqualsBean(test.createPosition(positionInfo, 100, ReferenceData.empty()), expectedPosition1);
		OvernightFuturePosition expectedPosition2 = OvernightFuturePosition.builder().info(positionInfo).product(PRODUCT).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, ReferenceData.empty()), expectedPosition2);
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
	  internal static OvernightFutureSecurity sut()
	  {
		return OvernightFutureSecurity.builder().info(INFO).notional(PRODUCT.Notional).accrualFactor(PRODUCT.AccrualFactor).index(PRODUCT.Index).accrualMethod(PRODUCT.AccrualMethod).lastTradeDate(PRODUCT.LastTradeDate).startDate(PRODUCT.StartDate).endDate(PRODUCT.EndDate).rounding(PRODUCT.Rounding).build();
	  }

	  internal static OvernightFutureSecurity sut2()
	  {
		return OvernightFutureSecurity.builder().info(INFO2).notional(PRODUCT2.Notional).accrualFactor(PRODUCT2.AccrualFactor).index(PRODUCT2.Index).accrualMethod(PRODUCT2.AccrualMethod).lastTradeDate(PRODUCT2.LastTradeDate).startDate(PRODUCT2.StartDate).endDate(PRODUCT2.EndDate).rounding(PRODUCT2.Rounding).build();
	  }

	}

}
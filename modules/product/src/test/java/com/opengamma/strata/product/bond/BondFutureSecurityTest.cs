/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using TestHelper = com.opengamma.strata.collect.TestHelper;

	/// <summary>
	/// Test <seealso cref="BondFutureSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureSecurityTest
	public class BondFutureSecurityTest
	{

	  private static readonly BondFuture PRODUCT = BondFutureTest.sut();
	  private static readonly BondFuture PRODUCT2 = BondFutureTest.sut2();
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(PRODUCT.SecurityId, PRICE_INFO);
	  private static readonly SecurityInfo INFO2 = SecurityInfo.of(PRODUCT2.SecurityId, PRICE_INFO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BondFutureSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.FirstDeliveryDate, PRODUCT.FirstDeliveryDate);
		assertEquals(test.LastDeliveryDate, PRODUCT.LastDeliveryDate);
		ImmutableList<FixedCouponBond> basket = PRODUCT.DeliveryBasket;
		assertEquals(test.UnderlyingIds, ImmutableSet.of(basket.get(0).SecurityId, basket.get(1).SecurityId));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		BondFutureSecurity test = sut();
		ImmutableList<FixedCouponBond> basket = PRODUCT.DeliveryBasket;
		FixedCouponBondSecurity bondSec0 = FixedCouponBondSecurityTest.createSecurity(PRODUCT.DeliveryBasket.get(0));
		FixedCouponBondSecurity bondSec1 = FixedCouponBondSecurityTest.createSecurity(PRODUCT.DeliveryBasket.get(1));
		ReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(basket.get(0).SecurityId, bondSec0, basket.get(1).SecurityId, bondSec1));
		BondFuture product = test.createProduct(refData);
		assertEquals(product.DeliveryBasket.get(0), PRODUCT.DeliveryBasket.get(0));
		assertEquals(product.DeliveryBasket.get(1), PRODUCT.DeliveryBasket.get(1));
		TradeInfo tradeInfo = TradeInfo.of(date(2016, 6, 30));
		BondFutureTrade expectedTrade = BondFutureTrade.builder().info(tradeInfo).product(product).quantity(100).price(123.50).build();
		assertEquals(test.createTrade(tradeInfo, 100, 123.50, refData), expectedTrade);

		PositionInfo positionInfo = PositionInfo.empty();
		BondFuturePosition expectedPosition1 = BondFuturePosition.builder().info(positionInfo).product(product).longQuantity(100).build();
		TestHelper.assertEqualsBean(test.createPosition(positionInfo, 100, refData), expectedPosition1);
		BondFuturePosition expectedPosition2 = BondFuturePosition.builder().info(positionInfo).product(product).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, refData), expectedPosition2);
	  }

	  public virtual void test_createProduct_wrongType()
	  {
		BondFutureSecurity test = sut();
		ImmutableList<FixedCouponBond> basket = PRODUCT.DeliveryBasket;
		SecurityId secId = basket.get(0).SecurityId;
		GenericSecurity sec = GenericSecurity.of(INFO);
		ReferenceData refData = ImmutableReferenceData.of(secId, sec);
		assertThrows(() => test.createProduct(refData), typeof(System.InvalidCastException));
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
	  internal static BondFutureSecurity sut()
	  {
		ImmutableList<FixedCouponBond> basket = PRODUCT.DeliveryBasket;
		return BondFutureSecurity.builder().info(INFO).currency(PRODUCT.Currency).deliveryBasketIds(basket.get(0).SecurityId, basket.get(1).SecurityId).conversionFactors(1d, 2d).firstNoticeDate(PRODUCT.FirstNoticeDate).firstDeliveryDate(PRODUCT.FirstDeliveryDate.get()).lastNoticeDate(PRODUCT.LastNoticeDate).lastDeliveryDate(PRODUCT.LastDeliveryDate.get()).lastTradeDate(PRODUCT.LastTradeDate).rounding(PRODUCT.Rounding).build();
	  }

	  internal static BondFutureSecurity sut2()
	  {
		ImmutableList<FixedCouponBond> basket = PRODUCT2.DeliveryBasket;
		return BondFutureSecurity.builder().info(INFO2).currency(PRODUCT2.Currency).deliveryBasketIds(basket.get(0).SecurityId).conversionFactors(3d).firstNoticeDate(PRODUCT2.FirstNoticeDate).lastNoticeDate(PRODUCT2.LastNoticeDate).lastTradeDate(PRODUCT2.LastTradeDate).rounding(PRODUCT2.Rounding).build();
	  }

	}

}
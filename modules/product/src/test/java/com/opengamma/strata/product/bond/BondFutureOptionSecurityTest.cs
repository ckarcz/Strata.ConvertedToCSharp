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
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.PUT;
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
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// Test <seealso cref="BondFutureOptionSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureOptionSecurityTest
	public class BondFutureOptionSecurityTest
	{

	  private static readonly BondFutureOption PRODUCT = BondFutureOptionTest.sut();
	  private static readonly BondFutureOption PRODUCT2 = BondFutureOptionTest.sut2();
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(PRODUCT.SecurityId, PRICE_INFO);
	  private static readonly SecurityInfo INFO2 = SecurityInfo.of(PRODUCT2.SecurityId, PRICE_INFO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BondFutureOptionSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.UnderlyingIds, ImmutableSet.of(PRODUCT.UnderlyingFuture.SecurityId));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		BondFutureOptionSecurity test = sut();
		BondFuture future = PRODUCT.UnderlyingFuture;
		BondFutureSecurity futureSec = BondFutureSecurityTest.sut();
		ImmutableList<FixedCouponBond> basket = future.DeliveryBasket;
		FixedCouponBondSecurity bondSec0 = FixedCouponBondSecurityTest.createSecurity(future.DeliveryBasket.get(0));
		FixedCouponBondSecurity bondSec1 = FixedCouponBondSecurityTest.createSecurity(future.DeliveryBasket.get(1));
		ReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of(test.UnderlyingFutureId, futureSec, basket.get(0).SecurityId, bondSec0, basket.get(1).SecurityId, bondSec1));
		BondFutureOption product = test.createProduct(refData);
		assertEquals(product.UnderlyingFuture.DeliveryBasket.get(0), future.DeliveryBasket.get(0));
		assertEquals(product.UnderlyingFuture.DeliveryBasket.get(1), future.DeliveryBasket.get(1));
		TradeInfo tradeInfo = TradeInfo.of(date(2016, 6, 30));
		BondFutureOptionTrade expectedTrade = BondFutureOptionTrade.builder().info(tradeInfo).product(product).quantity(100).price(123.50).build();
		assertEquals(test.createTrade(tradeInfo, 100, 123.50, refData), expectedTrade);

		PositionInfo positionInfo = PositionInfo.empty();
		BondFutureOptionPosition expectedPosition1 = BondFutureOptionPosition.builder().info(positionInfo).product(product).longQuantity(100).build();
		TestHelper.assertEqualsBean(test.createPosition(positionInfo, 100, refData), expectedPosition1);
		BondFutureOptionPosition expectedPosition2 = BondFutureOptionPosition.builder().info(positionInfo).product(product).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, refData), expectedPosition2);
	  }

	  public virtual void test_createProduct_wrongType()
	  {
		BondFutureOptionSecurity test = sut();
		BondFuture future = PRODUCT.UnderlyingFuture;
		SecurityId secId = future.SecurityId;
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
	  internal static BondFutureOptionSecurity sut()
	  {
		return BondFutureOptionSecurity.builder().info(INFO).currency(PRODUCT.Currency).putCall(CALL).strikePrice(PRODUCT.StrikePrice).expiryDate(PRODUCT.ExpiryDate).expiryTime(PRODUCT.ExpiryTime).expiryZone(PRODUCT.ExpiryZone).premiumStyle(FutureOptionPremiumStyle.DAILY_MARGIN).rounding(PRODUCT.Rounding).underlyingFutureId(PRODUCT.UnderlyingFuture.SecurityId).build();
	  }

	  internal static BondFutureOptionSecurity sut2()
	  {
		return BondFutureOptionSecurity.builder().info(INFO2).currency(PRODUCT2.Currency).putCall(PUT).strikePrice(PRODUCT2.StrikePrice).expiryDate(PRODUCT2.ExpiryDate).expiryTime(PRODUCT2.ExpiryTime).expiryZone(PRODUCT2.ExpiryZone).premiumStyle(FutureOptionPremiumStyle.UPFRONT_PREMIUM).rounding(PRODUCT2.Rounding).underlyingFutureId(PRODUCT2.UnderlyingFuture.SecurityId).build();
	  }

	}

}
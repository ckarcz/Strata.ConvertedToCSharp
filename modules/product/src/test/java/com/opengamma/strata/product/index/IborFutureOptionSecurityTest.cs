/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using TestHelper = com.opengamma.strata.collect.TestHelper;

	/// <summary>
	/// Test <seealso cref="IborFutureOptionSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureOptionSecurityTest
	public class IborFutureOptionSecurityTest
	{

	  private static readonly IborFutureOption OPTION = IborFutureOptionTest.sut();
	  private static readonly IborFutureOption OPTION2 = IborFutureOptionTest.sut2();
	  private static readonly IborFuture FUTURE = OPTION.UnderlyingFuture;
	  private static readonly IborFuture FUTURE2 = OPTION2.UnderlyingFuture;
	  private static readonly IborFutureSecurity FUTURE_SECURITY = IborFutureSecurityTest.sut();
	  private static readonly SecurityId FUTURE_ID = FUTURE.SecurityId;
	  private static readonly SecurityId FUTURE_ID2 = FUTURE2.SecurityId;
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(OPTION.SecurityId, PRICE_INFO);
	  private static readonly SecurityInfo INFO2 = SecurityInfo.of(OPTION2.SecurityId, PRICE_INFO);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		IborFutureOptionSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, OPTION.SecurityId);
		assertEquals(test.Currency, OPTION.Currency);
		assertEquals(test.PutCall, OPTION.PutCall);
		assertEquals(test.PremiumStyle, OPTION.PremiumStyle);
		assertEquals(test.UnderlyingFutureId, FUTURE_ID);
		assertEquals(test.UnderlyingIds, ImmutableSet.of(FUTURE_ID));
	  }

	  public virtual void test_builder_badPrice()
	  {
		assertThrowsIllegalArg(() => sut().toBuilder().strikePrice(2.1).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createProduct()
	  {
		IborFutureOptionSecurity test = sut();
		ReferenceData refData = ImmutableReferenceData.of(FUTURE_ID, FUTURE_SECURITY);
		assertEquals(test.createProduct(refData), OPTION);
		TradeInfo tradeInfo = TradeInfo.of(date(2016, 6, 30));
		IborFutureOptionTrade expectedTrade = IborFutureOptionTrade.builder().info(tradeInfo).product(OPTION).quantity(100).price(123.50).build();
		assertEquals(test.createTrade(tradeInfo, 100, 123.50, refData), expectedTrade);

		PositionInfo positionInfo = PositionInfo.empty();
		IborFutureOptionPosition expectedPosition1 = IborFutureOptionPosition.builder().info(positionInfo).product(OPTION).longQuantity(100).build();
		TestHelper.assertEqualsBean(test.createPosition(positionInfo, 100, refData), expectedPosition1);
		IborFutureOptionPosition expectedPosition2 = IborFutureOptionPosition.builder().info(positionInfo).product(OPTION).longQuantity(100).shortQuantity(50).build();
		assertEquals(test.createPosition(positionInfo, 100, 50, refData), expectedPosition2);
	  }

	  public virtual void test_createProduct_wrongType()
	  {
		IborFutureOptionSecurity test = sut();
		IborFuture future = OPTION.UnderlyingFuture;
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
	  internal static IborFutureOptionSecurity sut()
	  {
		return IborFutureOptionSecurity.builder().info(INFO).currency(OPTION.Currency).putCall(OPTION.PutCall).strikePrice(OPTION.StrikePrice).expiryDate(OPTION.ExpiryDate).expiryTime(OPTION.ExpiryTime).expiryZone(OPTION.ExpiryZone).premiumStyle(OPTION.PremiumStyle).rounding(OPTION.Rounding).underlyingFutureId(FUTURE_ID).build();
	  }

	  internal static IborFutureOptionSecurity sut2()
	  {
		return IborFutureOptionSecurity.builder().info(INFO2).currency(OPTION2.Currency).putCall(OPTION2.PutCall).strikePrice(OPTION2.StrikePrice).expiryDate(OPTION2.ExpiryDate).expiryTime(OPTION2.ExpiryTime).expiryZone(OPTION2.ExpiryZone).premiumStyle(OPTION2.PremiumStyle).rounding(OPTION2.Rounding).underlyingFutureId(FUTURE_ID2).build();
	  }

	}

}
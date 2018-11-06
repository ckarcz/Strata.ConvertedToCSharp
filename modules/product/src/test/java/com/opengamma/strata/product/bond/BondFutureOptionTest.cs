/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// Test <seealso cref="BondFutureOption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BondFutureOptionTest
	public class BondFutureOptionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // future
	  private static readonly BondFuture FUTURE = BondFutureTest.sut();
	  private static readonly BondFuture FUTURE2 = BondFutureTest.sut2();
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "BondFutureOption");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "BondFutureOption2");
	  // future option
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(3);
	  private static readonly LocalDate EXPIRY_DATE = date(2011, 9, 20);
	  private static readonly LocalTime EXPIRY_TIME = LocalTime.of(11, 0);
	  private static readonly ZoneId EXPIRY_ZONE = ZoneId.of("Z");
	  private const double STRIKE_PRICE = 1.15;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BondFutureOption test = sut();
		assertEquals(test.PutCall, CALL);
		assertEquals(test.StrikePrice, STRIKE_PRICE);
		assertEquals(test.ExpiryDate, EXPIRY_DATE);
		assertEquals(test.ExpiryTime, EXPIRY_TIME);
		assertEquals(test.ExpiryZone, EXPIRY_ZONE);
		assertEquals(test.Expiry, ZonedDateTime.of(EXPIRY_DATE, EXPIRY_TIME, EXPIRY_ZONE));
		assertEquals(test.Rounding, Rounding.none());
		assertEquals(test.UnderlyingFuture, FUTURE);
		assertEquals(test.Currency, FUTURE.Currency);
	  }

	  public virtual void test_builder_expiryNotAfterTradeDate()
	  {
		assertThrowsIllegalArg(() => BondFutureOption.builder().putCall(CALL).expiryDate(FUTURE.LastTradeDate).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).strikePrice(STRIKE_PRICE).underlyingFuture(FUTURE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		BondFutureOption test = sut();
		ResolvedBondFutureOption expected = ResolvedBondFutureOption.builder().securityId(SECURITY_ID).putCall(CALL).strikePrice(STRIKE_PRICE).expiry(EXPIRY_DATE.atTime(EXPIRY_TIME).atZone(EXPIRY_ZONE)).premiumStyle(FutureOptionPremiumStyle.DAILY_MARGIN).underlyingFuture(FUTURE.resolve(REF_DATA)).build();
		assertEquals(test.resolve(REF_DATA), expected);
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
	  internal static BondFutureOption sut()
	  {
		return BondFutureOption.builder().securityId(SECURITY_ID).putCall(CALL).strikePrice(STRIKE_PRICE).expiryDate(EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).premiumStyle(FutureOptionPremiumStyle.DAILY_MARGIN).underlyingFuture(FUTURE).build();
	  }

	  internal static BondFutureOption sut2()
	  {
		return BondFutureOption.builder().securityId(SECURITY_ID2).putCall(PUT).strikePrice(1.075).expiryDate(date(2011, 9, 21)).expiryTime(LocalTime.of(12, 0)).expiryZone(ZoneId.of("Europe/Paris")).premiumStyle(FutureOptionPremiumStyle.UPFRONT_PREMIUM).rounding(ROUNDING).underlyingFuture(FUTURE2).build();
	  }

	}

}
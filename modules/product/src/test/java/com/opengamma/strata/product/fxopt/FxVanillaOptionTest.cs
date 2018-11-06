/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
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


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;

	/// <summary>
	/// Test <seealso cref="FxVanillaOption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxVanillaOptionTest
	public class FxVanillaOptionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2015, 2, 14);
	  private static readonly LocalTime EXPIRY_TIME = LocalTime.of(12, 15);
	  private static readonly ZoneId EXPIRY_ZONE = ZoneId.of("Z");
	  private const LongShort LONG = LongShort.LONG;
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 2, 16);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * 1.35);
	  private static readonly FxSingle FX = FxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FxVanillaOption test = sut();
		assertEquals(test.ExpiryDate, EXPIRY_DATE);
		assertEquals(test.Expiry, ZonedDateTime.of(EXPIRY_DATE, EXPIRY_TIME, EXPIRY_ZONE));
		assertEquals(test.ExpiryZone, EXPIRY_ZONE);
		assertEquals(test.ExpiryTime, EXPIRY_TIME);
		assertEquals(test.LongShort, LONG);
		assertEquals(test.Underlying, FX);
		assertEquals(test.CurrencyPair, FX.CurrencyPair);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(EUR, USD));
		assertEquals(test.allCurrencies(), ImmutableSet.of(EUR, USD));
	  }

	  public virtual void test_builder_earlyPaymentDate()
	  {
		assertThrowsIllegalArg(() => FxVanillaOption.builder().longShort(LONG).expiryDate(LocalDate.of(2015, 2, 21)).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).underlying(FX).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxVanillaOption @base = sut();
		ResolvedFxVanillaOption expected = ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY_DATE.atTime(EXPIRY_TIME).atZone(EXPIRY_ZONE)).underlying(FX.resolve(REF_DATA)).build();
		assertEquals(@base.resolve(REF_DATA), expected);
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
	  internal static FxVanillaOption sut()
	  {
		return FxVanillaOption.builder().longShort(LONG).expiryDate(EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).underlying(FX).build();
	  }

	  internal static FxVanillaOption sut2()
	  {
		FxSingle fxProduct = FxSingle.of(CurrencyAmount.of(EUR, -NOTIONAL), CurrencyAmount.of(GBP, NOTIONAL * 0.9), PAYMENT_DATE);
		return FxVanillaOption.builder().longShort(LongShort.SHORT).expiryDate(LocalDate.of(2015, 2, 15)).expiryTime(LocalTime.of(12, 45)).expiryZone(ZoneId.of("GMT")).underlying(fxProduct).build();
	  }

	}

}
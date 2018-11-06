/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.PUT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;

	/// <summary>
	/// Test <seealso cref="ResolvedFxVanillaOption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxVanillaOptionTest
	public class ResolvedFxVanillaOptionTest
	{

	  private static readonly ZonedDateTime EXPIRY_DATE_TIME = ZonedDateTime.of(2015, 2, 14, 12, 15, 0, 0, ZoneOffset.UTC);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 2, 16);
	  private const double NOTIONAL = 1.0e6;
	  private const double STRIKE = 1.35;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * STRIKE);
	  private static readonly ResolvedFxSingle FX = ResolvedFxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private const double STRIKE_RE = 0.9;
	  private static readonly ResolvedFxSingle FX_RE = ResolvedFxSingle.of(CurrencyAmount.of(EUR, -NOTIONAL), CurrencyAmount.of(GBP, NOTIONAL * STRIKE_RE), PAYMENT_DATE);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedFxVanillaOption test = sut();
		assertEquals(test.Expiry, EXPIRY_DATE_TIME);
		assertEquals(test.ExpiryDate, EXPIRY_DATE_TIME.toLocalDate());
		assertEquals(test.LongShort, LONG);
		assertEquals(test.CounterCurrency, USD);
		assertEquals(test.PutCall, CALL);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.Underlying, FX);
		assertEquals(test.CurrencyPair, FX.CurrencyPair);
	  }

	  public virtual void test_builder_inverseFx()
	  {
		ResolvedFxVanillaOption test = sut2();
		assertEquals(test.Expiry, EXPIRY_DATE_TIME.plusSeconds(1));
		assertEquals(test.ExpiryDate, EXPIRY_DATE_TIME.toLocalDate());
		assertEquals(test.LongShort, SHORT);
		assertEquals(test.CounterCurrency, GBP);
		assertEquals(test.PutCall, PUT);
		assertEquals(test.Strike, STRIKE_RE);
		assertEquals(test.Underlying, FX_RE);
	  }

	  public virtual void test_builder_earlyPaymentDate()
	  {
		assertThrowsIllegalArg(() => ResolvedFxVanillaOption.builder().longShort(LONG).expiry(LocalDate.of(2015, 2, 21).atStartOfDay(ZoneOffset.UTC)).underlying(FX).build());
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
	  internal static ResolvedFxVanillaOption sut()
	  {
		return ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY_DATE_TIME).underlying(FX).build();
	  }

	  internal static ResolvedFxVanillaOption sut2()
	  {
		;
		return ResolvedFxVanillaOption.builder().longShort(SHORT).expiry(EXPIRY_DATE_TIME.plusSeconds(1)).underlying(FX_RE).build();
	  }

	}

}
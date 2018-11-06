/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
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
	using FxRate = com.opengamma.strata.basics.currency.FxRate;

	/// <summary>
	/// Test <seealso cref="FxNdf"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxNdfTest
	public class FxNdfTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxRate FX_RATE = FxRate.of(GBP, USD, 1.5d);
	  private const double NOTIONAL = 100_000_000;
	  private static readonly CurrencyAmount CURRENCY_NOTIONAL = CurrencyAmount.of(GBP, NOTIONAL);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 3, 19);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		FxNdf test = FxNdf.builder().agreedFxRate(FX_RATE).index(GBP_USD_WM).settlementCurrencyNotional(CURRENCY_NOTIONAL).paymentDate(PAYMENT_DATE).build();
		assertEquals(test.AgreedFxRate, FX_RATE);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.NonDeliverableCurrency, USD);
		assertEquals(test.SettlementCurrencyNotional, CURRENCY_NOTIONAL);
		assertEquals(test.PaymentDate, PAYMENT_DATE);
		assertEquals(test.SettlementCurrency, GBP);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP, USD));
	  }

	  public virtual void test_builder_inverse()
	  {
		FxRate fxRate = FxRate.of(USD, GBP, 0.7d);
		FxNdf test = FxNdf.builder().agreedFxRate(fxRate).settlementCurrencyNotional(CURRENCY_NOTIONAL).index(GBP_USD_WM).paymentDate(PAYMENT_DATE).build();
		assertEquals(test.AgreedFxRate, fxRate);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.NonDeliverableCurrency, USD);
		assertEquals(test.SettlementCurrencyNotional, CURRENCY_NOTIONAL);
		assertEquals(test.PaymentDate, PAYMENT_DATE);
		assertEquals(test.SettlementCurrency, GBP);
	  }

	  public virtual void test_builder_wrongRate()
	  {
		FxRate fxRate = FxRate.of(GBP, EUR, 1.1d);
		assertThrowsIllegalArg(() => FxNdf.builder().agreedFxRate(fxRate).settlementCurrencyNotional(CURRENCY_NOTIONAL).index(GBP_USD_WM).paymentDate(PAYMENT_DATE).build());
	  }

	  public virtual void test_builder_wrongCurrency()
	  {
		assertThrowsIllegalArg(() => FxNdf.builder().agreedFxRate(FX_RATE).settlementCurrencyNotional(CurrencyAmount.of(EUR, NOTIONAL)).index(GBP_USD_WM).paymentDate(PAYMENT_DATE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxNdf @base = sut();
		ResolvedFxNdf resolved = @base.resolve(REF_DATA);
		assertEquals(resolved.AgreedFxRate, FX_RATE);
		assertEquals(resolved.Index, GBP_USD_WM);
		assertEquals(resolved.NonDeliverableCurrency, USD);
		assertEquals(resolved.PaymentDate, PAYMENT_DATE);
		assertEquals(resolved.SettlementCurrency, GBP);
		assertEquals(resolved.SettlementCurrencyNotional, CURRENCY_NOTIONAL);
		assertEquals(resolved.SettlementNotional, NOTIONAL);
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
	  internal static FxNdf sut()
	  {
		return FxNdf.builder().agreedFxRate(FX_RATE).settlementCurrencyNotional(CURRENCY_NOTIONAL).index(GBP_USD_WM).paymentDate(PAYMENT_DATE).build();
	  }

	  internal static FxNdf sut2()
	  {
		return FxNdf.builder().agreedFxRate(FX_RATE).settlementCurrencyNotional(CurrencyAmount.of(USD, -NOTIONAL)).index(GBP_USD_WM).paymentDate(PAYMENT_DATE).build();
	  }

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_GBP_ECB;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;

	/// <summary>
	/// Test <seealso cref="ResolvedFxNdf"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxNdfTest
	public class ResolvedFxNdfTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxRate FX_RATE = FxRate.of(GBP, USD, 1.5d);
	  private const double NOTIONAL = 100_000_000;
	  private static readonly CurrencyAmount CURRENCY_NOTIONAL = CurrencyAmount.of(GBP, NOTIONAL);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 3, 19);
	  private static readonly LocalDate FIXING_DATE = LocalDate.of(2015, 3, 17);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedFxNdf test = sut();
		assertEquals(test.AgreedFxRate, FX_RATE);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.NonDeliverableCurrency, USD);
		assertEquals(test.PaymentDate, PAYMENT_DATE);
		assertEquals(test.SettlementCurrency, GBP);
		assertEquals(test.SettlementCurrencyNotional, CURRENCY_NOTIONAL);
		assertEquals(test.SettlementNotional, NOTIONAL);
	  }

	  public virtual void test_builder_inverse()
	  {
		CurrencyAmount currencyNotional = CurrencyAmount.of(USD, NOTIONAL);
		ResolvedFxNdf test = ResolvedFxNdf.builder().agreedFxRate(FX_RATE).observation(FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).settlementCurrencyNotional(currencyNotional).build();
		assertEquals(test.AgreedFxRate, FX_RATE);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.NonDeliverableCurrency, GBP);
		assertEquals(test.PaymentDate, PAYMENT_DATE);
		assertEquals(test.SettlementCurrency, USD);
		assertEquals(test.SettlementCurrencyNotional, currencyNotional);
		assertEquals(test.SettlementNotional, NOTIONAL);
	  }

	  public virtual void test_builder_wrongCurrency()
	  {
		CurrencyAmount currencyNotional = CurrencyAmount.of(EUR, NOTIONAL);
		assertThrowsIllegalArg(() => ResolvedFxNdf.builder().agreedFxRate(FX_RATE).observation(FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).settlementCurrencyNotional(currencyNotional).build());
	  }

	  public virtual void test_builder_wrongRate()
	  {
		FxRate fxRate = FxRate.of(GBP, EUR, 1.1d);
		assertThrowsIllegalArg(() => ResolvedFxNdf.builder().agreedFxRate(fxRate).observation(FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).settlementCurrencyNotional(CURRENCY_NOTIONAL).build());
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
	  internal static ResolvedFxNdf sut()
	  {
		return ResolvedFxNdf.builder().agreedFxRate(FX_RATE).observation(FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).settlementCurrencyNotional(CURRENCY_NOTIONAL).build();
	  }

	  internal static ResolvedFxNdf sut2()
	  {
		FxRate fxRate = FxRate.of(GBP, EUR, 1.1d);
		return ResolvedFxNdf.builder().agreedFxRate(fxRate).observation(FxIndexObservation.of(EUR_GBP_ECB, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).settlementCurrencyNotional(CURRENCY_NOTIONAL).build();
	  }

	}

}
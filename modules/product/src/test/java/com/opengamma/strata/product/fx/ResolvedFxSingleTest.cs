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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedFxSingle"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxSingleTest
	public class ResolvedFxSingleTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount USD_P1600 = CurrencyAmount.of(USD, 1_600);
	  private static readonly CurrencyAmount USD_M1600 = CurrencyAmount.of(USD, -1_600);
	  private static readonly CurrencyAmount EUR_P1600 = CurrencyAmount.of(EUR, 1_800);
	  private static readonly LocalDate DATE_2015_06_29 = date(2015, 6, 29);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);
	  private static readonly Payment PAYMENT_GBP_P1000 = Payment.of(GBP_P1000, DATE_2015_06_30);
	  private static readonly Payment PAYMENT_GBP_M1000 = Payment.of(GBP_M1000, DATE_2015_06_30);
	  private static readonly Payment PAYMENT_USD_P1600 = Payment.of(USD_P1600, DATE_2015_06_30);
	  private static readonly Payment PAYMENT_USD_M1600 = Payment.of(USD_M1600, DATE_2015_06_30);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_payments_rightOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(PAYMENT_GBP_P1000, PAYMENT_USD_M1600);
		assertEquals(test.BaseCurrencyPayment, PAYMENT_GBP_P1000);
		assertEquals(test.CounterCurrencyPayment, PAYMENT_USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_payments_switchOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(PAYMENT_USD_M1600, PAYMENT_GBP_P1000);
		assertEquals(test.BaseCurrencyPayment, PAYMENT_GBP_P1000);
		assertEquals(test.CounterCurrencyPayment, PAYMENT_USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_payments_sameCurrency()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSingle.of(PAYMENT_GBP_P1000, PAYMENT_GBP_M1000));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_amounts_rightOrder()
	  {
		ResolvedFxSingle test = sut();
		assertEquals(test.BaseCurrencyPayment, PAYMENT_GBP_P1000);
		assertEquals(test.CounterCurrencyPayment, PAYMENT_USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_amounts_switchOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(USD_M1600, GBP_P1000, DATE_2015_06_30);
		assertEquals(test.BaseCurrencyPayment, PAYMENT_GBP_P1000);
		assertEquals(test.CounterCurrencyPayment, PAYMENT_USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_amounts_bothZero()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(CurrencyAmount.zero(GBP), CurrencyAmount.zero(USD), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyPayment, Payment.of(CurrencyAmount.zero(GBP), DATE_2015_06_30));
		assertEquals(test.CounterCurrencyPayment, Payment.of(CurrencyAmount.zero(USD), DATE_2015_06_30));
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, CurrencyAmount.zero(USD));
	  }

	  public virtual void test_of_amounts_positiveNegative()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSingle.of(GBP_P1000, USD_P1600, DATE_2015_06_30));
		assertThrowsIllegalArg(() => ResolvedFxSingle.of(GBP_M1000, USD_M1600, DATE_2015_06_30));
		assertThrowsIllegalArg(() => ResolvedFxSingle.of(CurrencyAmount.zero(GBP), USD_M1600, DATE_2015_06_30));
		assertThrowsIllegalArg(() => ResolvedFxSingle.of(CurrencyAmount.zero(GBP), USD_P1600, DATE_2015_06_30));
	  }

	  public virtual void test_of_sameCurrency()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSingle.of(GBP_P1000, GBP_M1000, DATE_2015_06_30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_rate_rightOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(GBP_P1000, FxRate.of(GBP, USD, 1.6d), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyPayment, Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.CounterCurrencyPayment, Payment.of(USD_M1600, DATE_2015_06_30));
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_rate_switchOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(USD_M1600, FxRate.of(USD, GBP, 1d / 1.6d), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyPayment, Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.CounterCurrencyPayment, Payment.of(USD_M1600, DATE_2015_06_30));
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_rate_bothZero()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.of(CurrencyAmount.zero(GBP), FxRate.of(USD, GBP, 1.6d), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyPayment.Value, CurrencyAmount.zero(GBP));
		assertEquals(test.CounterCurrencyPayment.Value.Amount, CurrencyAmount.zero(USD).Amount, 1e-12);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, CurrencyAmount.of(USD, 0d));
	  }

	  public virtual void test_of_rate_wrongCurrency()
	  {
		assertThrowsIllegalArg(() => FxSingle.of(GBP_P1000, FxRate.of(USD, EUR, 1.45d), DATE_2015_06_30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_rightOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.meta().builder().set(ResolvedFxSingle.meta().baseCurrencyPayment(), PAYMENT_GBP_P1000).set(ResolvedFxSingle.meta().counterCurrencyPayment(), PAYMENT_USD_M1600).build();
		assertEquals(test.BaseCurrencyPayment, PAYMENT_GBP_P1000);
		assertEquals(test.CounterCurrencyPayment, PAYMENT_USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
	  }

	  public virtual void test_builder_switchOrder()
	  {
		ResolvedFxSingle test = ResolvedFxSingle.meta().builder().set(ResolvedFxSingle.meta().baseCurrencyPayment(), PAYMENT_USD_M1600).set(ResolvedFxSingle.meta().counterCurrencyPayment(), PAYMENT_GBP_P1000).build();
		assertEquals(test.BaseCurrencyPayment, PAYMENT_GBP_P1000);
		assertEquals(test.CounterCurrencyPayment, PAYMENT_USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
	  }

	  public virtual void test_builder_bothPositive()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSingle.meta().builder().set(ResolvedFxSingle.meta().baseCurrencyPayment(), PAYMENT_GBP_P1000).set(ResolvedFxSingle.meta().counterCurrencyPayment(), PAYMENT_USD_P1600).build());
	  }

	  public virtual void test_builder_bothNegative()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSingle.meta().builder().set(ResolvedFxSingle.meta().baseCurrencyPayment(), PAYMENT_GBP_M1000).set(ResolvedFxSingle.meta().counterCurrencyPayment(), PAYMENT_USD_M1600).build());
	  }

	  public virtual void test_builder_sameCurrency()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSingle.meta().builder().set(ResolvedFxSingle.meta().baseCurrencyPayment(), PAYMENT_GBP_P1000).set(ResolvedFxSingle.meta().counterCurrencyPayment(), PAYMENT_GBP_M1000).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_inverse()
	  {
		ResolvedFxSingle test = sut();
		assertEquals(test.inverse(), ResolvedFxSingle.of(GBP_M1000, USD_P1600, DATE_2015_06_30));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
		coverBeanEquals(sut(), sut3());
		coverBeanEquals(sut2(), sut3());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedFxSingle sut()
	  {
		return ResolvedFxSingle.of(GBP_P1000, USD_M1600, DATE_2015_06_30);
	  }

	  internal static ResolvedFxSingle sut2()
	  {
		return ResolvedFxSingle.of(GBP_M1000, EUR_P1600, DATE_2015_06_29);
	  }

	  internal static ResolvedFxSingle sut3()
	  {
		return ResolvedFxSingle.of(USD_M1600, EUR_P1600, DATE_2015_06_30);
	  }

	}

}
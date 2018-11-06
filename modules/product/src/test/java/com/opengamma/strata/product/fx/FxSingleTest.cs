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
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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


	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Test <seealso cref="FxSingle"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleTest
	public class FxSingleTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount USD_P1600 = CurrencyAmount.of(USD, 1_600);
	  private static readonly CurrencyAmount USD_M1600 = CurrencyAmount.of(USD, -1_600);
	  private static readonly CurrencyAmount EUR_P1600 = CurrencyAmount.of(EUR, 1_800);
	  private static readonly LocalDate DATE_2015_06_29 = date(2015, 6, 29);
	  private static readonly LocalDate DATE_2015_06_30 = date(2015, 6, 30);
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(FOLLOWING, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_rightOrderPayments()
	  {
		FxSingle test = FxSingle.of(Payment.of(GBP_P1000, DATE_2015_06_30), Payment.of(USD_M1600, DATE_2015_06_29), BDA);
		assertEquals(test.BaseCurrencyPayment, Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.CounterCurrencyPayment, Payment.of(USD_M1600, DATE_2015_06_29));
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.PaymentDateAdjustment, BDA);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.PayCurrencyAmount, USD_M1600);
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP, USD));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP, USD));
	  }

	  public virtual void test_of_switchOrderPayments()
	  {
		FxSingle test = FxSingle.of(Payment.of(USD_M1600, DATE_2015_06_30), Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.PayCurrencyAmount, USD_M1600);
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_rightOrder()
	  {
		FxSingle test = sut();
		assertEquals(test.BaseCurrencyPayment, Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.CounterCurrencyPayment, Payment.of(USD_M1600, DATE_2015_06_30));
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.PaymentDateAdjustment, null);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.PayCurrencyAmount, USD_M1600);
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP, USD));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP, USD));
	  }

	  public virtual void test_of_switchOrder()
	  {
		FxSingle test = FxSingle.of(USD_M1600, GBP_P1000, DATE_2015_06_30);
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_bothZero()
	  {
		FxSingle test = FxSingle.of(CurrencyAmount.zero(GBP), CurrencyAmount.zero(USD), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyAmount, CurrencyAmount.zero(GBP));
		assertEquals(test.CounterCurrencyAmount, CurrencyAmount.zero(USD));
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.PayCurrencyAmount, CurrencyAmount.zero(GBP));
		assertEquals(test.ReceiveCurrencyAmount, CurrencyAmount.zero(USD));
	  }

	  public virtual void test_of_positiveNegative()
	  {
		assertThrowsIllegalArg(() => FxSingle.of(GBP_P1000, USD_P1600, DATE_2015_06_30));
		assertThrowsIllegalArg(() => FxSingle.of(GBP_M1000, USD_M1600, DATE_2015_06_30));
		assertThrowsIllegalArg(() => FxSingle.of(CurrencyAmount.zero(GBP), USD_M1600, DATE_2015_06_30));
		assertThrowsIllegalArg(() => FxSingle.of(CurrencyAmount.zero(GBP), USD_P1600, DATE_2015_06_30));
	  }

	  public virtual void test_of_sameCurrency()
	  {
		assertThrowsIllegalArg(() => FxSingle.of(GBP_P1000, GBP_M1000, DATE_2015_06_30));
	  }

	  public virtual void test_of_withAdjustment()
	  {
		FxSingle test = FxSingle.of(GBP_P1000, USD_M1600, DATE_2015_06_30, BDA);
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.PaymentDateAdjustment, BDA);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_rate_rightOrder()
	  {
		FxSingle test = FxSingle.of(GBP_P1000, FxRate.of(GBP, USD, 1.6d), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.PaymentDateAdjustment, null);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_rate_switchOrder()
	  {
		FxSingle test = FxSingle.of(USD_M1600, FxRate.of(USD, GBP, 1d / 1.6d), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_of_rate_bothZero()
	  {
		FxSingle test = FxSingle.of(CurrencyAmount.zero(GBP), FxRate.of(USD, GBP, 1.6d), DATE_2015_06_30);
		assertEquals(test.BaseCurrencyAmount, CurrencyAmount.zero(GBP));
		assertEquals(test.CounterCurrencyAmount.Amount, CurrencyAmount.zero(USD).Amount, 1e-12);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, CurrencyAmount.of(USD, 0d));
	  }

	  public virtual void test_of_rate_wrongCurrency()
	  {
		assertThrowsIllegalArg(() => FxSingle.of(GBP_P1000, FxRate.of(USD, EUR, 1.45d), DATE_2015_06_30));
	  }

	  public virtual void test_of_rate_withAdjustment()
	  {
		FxSingle test = FxSingle.of(GBP_P1000, FxRate.of(GBP, USD, 1.6d), DATE_2015_06_30, BDA);
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.PaymentDateAdjustment, BDA);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_rightOrder()
	  {
		FxSingle test = FxSingle.meta().builder().set(FxSingle.meta().baseCurrencyPayment(), Payment.of(GBP_P1000, DATE_2015_06_30)).set(FxSingle.meta().counterCurrencyPayment(), Payment.of(USD_M1600, DATE_2015_06_30)).build();
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_builder_switchOrder()
	  {
		FxSingle test = FxSingle.meta().builder().set(FxSingle.meta().baseCurrencyPayment(), Payment.of(USD_M1600, DATE_2015_06_30)).set(FxSingle.meta().counterCurrencyPayment(), Payment.of(GBP_P1000, DATE_2015_06_30)).build();
		assertEquals(test.BaseCurrencyAmount, GBP_P1000);
		assertEquals(test.CounterCurrencyAmount, USD_M1600);
		assertEquals(test.PaymentDate, DATE_2015_06_30);
		assertEquals(test.CurrencyPair, CurrencyPair.of(GBP, USD));
		assertEquals(test.ReceiveCurrencyAmount, GBP_P1000);
	  }

	  public virtual void test_builder_bothPositive()
	  {
		assertThrowsIllegalArg(() => FxSingle.meta().builder().set(FxSingle.meta().baseCurrencyPayment(), Payment.of(GBP_P1000, DATE_2015_06_30)).set(FxSingle.meta().counterCurrencyPayment(), Payment.of(USD_P1600, DATE_2015_06_30)).build());
	  }

	  public virtual void test_builder_bothNegative()
	  {
		assertThrowsIllegalArg(() => FxSingle.meta().builder().set(FxSingle.meta().baseCurrencyPayment(), Payment.of(GBP_M1000, DATE_2015_06_30)).set(FxSingle.meta().counterCurrencyPayment(), Payment.of(USD_M1600, DATE_2015_06_30)).build());
	  }

	  public virtual void test_builder_sameCurrency()
	  {
		assertThrowsIllegalArg(() => FxSingle.meta().builder().set(FxSingle.meta().baseCurrencyPayment(), Payment.of(GBP_P1000, DATE_2015_06_30)).set(FxSingle.meta().counterCurrencyPayment(), Payment.of(GBP_M1000, DATE_2015_06_30)).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxSingle fwd = sut();
		ResolvedFxSingle test = fwd.resolve(REF_DATA);
		assertEquals(test.BaseCurrencyPayment, Payment.of(GBP_P1000, DATE_2015_06_30));
		assertEquals(test.CounterCurrencyPayment, Payment.of(USD_M1600, DATE_2015_06_30));
		assertEquals(test.PaymentDate, DATE_2015_06_30);
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
		string xml = JodaBeanSer.PRETTY.xmlWriter().write(sut());
		assertEquals(JodaBeanSer.PRETTY.xmlReader().read(xml), sut());

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		string newXml = "<bean type='" + typeof(FxSingle).FullName + "'>" +
			"<baseCurrencyPayment><value>GBP 1000</value><date>2015-06-30</date></baseCurrencyPayment>" +
			"<counterCurrencyPayment><value>USD -1600</value><date>2015-06-30</date></counterCurrencyPayment>" +
			"</bean>";
		assertEquals(JodaBeanSer.PRETTY.xmlReader().read(newXml), sut());

//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		string oldXml = "<bean type='" + typeof(FxSingle).FullName + "'>" +
			"<baseCurrencyAmount>GBP 1000</baseCurrencyAmount>" +
			"<counterCurrencyAmount>USD -1600</counterCurrencyAmount>" +
			"<paymentDate>2015-06-30</paymentDate>" +
			"</bean>";
		assertEquals(JodaBeanSer.PRETTY.xmlReader().read(oldXml), sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static FxSingle sut()
	  {
		return FxSingle.of(GBP_P1000, USD_M1600, DATE_2015_06_30);
	  }

	  internal static FxSingle sut2()
	  {
		return FxSingle.of(GBP_M1000, EUR_P1600, DATE_2015_06_29);
	  }

	}

}
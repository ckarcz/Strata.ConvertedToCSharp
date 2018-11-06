/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_USD_ECB;
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxResetNotionalExchangeTest
	public class FxResetNotionalExchangeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_03_28 = date(2014, 3, 28);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);

	  public virtual void test_of()
	  {
		FxResetNotionalExchange test = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 1000d), DATE_2014_06_30, FxIndexObservation.of(GBP_USD_WM, DATE_2014_03_28, REF_DATA));
		assertEquals(test.PaymentDate, DATE_2014_06_30);
		assertEquals(test.ReferenceCurrency, USD);
		assertEquals(test.NotionalAmount, CurrencyAmount.of(USD, 1000d));
		assertEquals(test.Notional, 1000d, 0d);
	  }

	  public virtual void test_invalidCurrency()
	  {
		assertThrowsIllegalArg(() => FxResetNotionalExchange.meta().builder().set(FxResetNotionalExchange.meta().paymentDate(), DATE_2014_06_30).set(FxResetNotionalExchange.meta().notionalAmount(), CurrencyAmount.of(GBP, 1000d)).set(FxResetNotionalExchange.meta().observation(), FxIndexObservation.of(EUR_USD_ECB, DATE_2014_03_28, REF_DATA)).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_adjustPaymentDate()
	  {
		FxResetNotionalExchange test = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 1000d), DATE_2014_06_30, FxIndexObservation.of(GBP_USD_WM, DATE_2014_03_28, REF_DATA));
		FxResetNotionalExchange expected = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 1000d), DATE_2014_06_30.plusDays(2), FxIndexObservation.of(GBP_USD_WM, DATE_2014_03_28, REF_DATA));
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(0))), test);
		assertEquals(test.adjustPaymentDate(TemporalAdjusters.ofDateAdjuster(d => d.plusDays(2))), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxResetNotionalExchange test = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 1000d), DATE_2014_03_28, FxIndexObservation.of(GBP_USD_WM, DATE_2014_03_28, REF_DATA));
		coverImmutableBean(test);
		FxResetNotionalExchange test2 = FxResetNotionalExchange.of(CurrencyAmount.of(EUR, 2000d), DATE_2014_06_30, FxIndexObservation.of(EUR_USD_ECB, DATE_2014_06_30, REF_DATA));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxResetNotionalExchange test = FxResetNotionalExchange.of(CurrencyAmount.of(USD, 1000d), DATE_2014_06_30, FxIndexObservation.of(GBP_USD_WM, DATE_2014_03_28, REF_DATA));
		assertSerialization(test);
	  }

	}

}
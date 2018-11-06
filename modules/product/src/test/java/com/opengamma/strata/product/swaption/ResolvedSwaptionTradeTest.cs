/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedSwaptionTrade"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedSwaptionTradeTest
	public class ResolvedSwaptionTradeTest
	{

	  private static readonly ResolvedSwaption SWAPTION = ResolvedSwaptionTest.sut();
	  private static readonly ResolvedSwaption SWAPTION2 = ResolvedSwaptionTest.sut2();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 6, 30));
	  private static readonly Payment PREMIUM = Payment.of(CurrencyAmount.of(Currency.USD, -3150000d), date(2014, 3, 17));
	  private static readonly Payment PREMIUM2 = Payment.of(CurrencyAmount.of(Currency.USD, -3160000d), date(2014, 3, 17));

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedSwaptionTrade test = ResolvedSwaptionTrade.of(TRADE_INFO, SWAPTION, PREMIUM);
		assertEquals(test.Product, SWAPTION);
		assertEquals(test.Info, TRADE_INFO);
	  }

	  public virtual void test_builder()
	  {
		ResolvedSwaptionTrade test = sut();
		assertEquals(test.Product, SWAPTION);
		assertEquals(test.Info, TRADE_INFO);
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
	  internal static ResolvedSwaptionTrade sut()
	  {
		return ResolvedSwaptionTrade.builder().product(SWAPTION).info(TRADE_INFO).premium(PREMIUM).build();
	  }

	  internal static ResolvedSwaptionTrade sut2()
	  {
		return ResolvedSwaptionTrade.builder().product(SWAPTION2).premium(PREMIUM2).build();
	  }

	}

}
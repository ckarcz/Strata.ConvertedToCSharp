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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedFxVanillaOptionTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxVanillaOptionTradeTest
	public class ResolvedFxVanillaOptionTradeTest
	{

	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 2, 16);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly ResolvedFxVanillaOption OPTION = ResolvedFxVanillaOptionTest.sut();
	  private static readonly ResolvedFxVanillaOption OPTION2 = ResolvedFxVanillaOptionTest.sut();
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2015, 1, 15));
	  private static readonly Payment PREMIUM = Payment.of(EUR_AMOUNT, PAYMENT_DATE);
	  private static readonly Payment PREMIUM2 = Payment.of(EUR_AMOUNT, PAYMENT_DATE.plusDays(1));

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedFxVanillaOptionTrade test = sut();
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Product, OPTION);
		assertEquals(test.Premium, PREMIUM);
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
	  internal static ResolvedFxVanillaOptionTrade sut()
	  {
		return ResolvedFxVanillaOptionTrade.builder().info(TRADE_INFO).product(OPTION).premium(PREMIUM).build();
	  }

	  internal static ResolvedFxVanillaOptionTrade sut2()
	  {
		return ResolvedFxVanillaOptionTrade.builder().product(OPTION2).premium(PREMIUM2).build();
	  }

	}

}
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
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="ResolvedFxSingleBarrierOptionTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxSingleBarrierOptionTradeTest
	public class ResolvedFxSingleBarrierOptionTradeTest
	{

	  private static readonly ZonedDateTime EXPIRY_DATE_TIME = ZonedDateTime.of(2015, 2, 14, 12, 15, 0, 0, ZoneOffset.UTC);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 2, 16);
	  private const double NOTIONAL = 1.0e6;
	  private const double STRIKE = 1.35;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * STRIKE);
	  private static readonly ResolvedFxSingle FX = ResolvedFxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private static readonly ResolvedFxVanillaOption VANILLA_OPTION = ResolvedFxVanillaOption.builder().longShort(LONG).expiry(EXPIRY_DATE_TIME).underlying(FX).build();
	  private static readonly SimpleConstantContinuousBarrier BARRIER = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 1.2);
	  private static readonly CurrencyAmount REBATE = CurrencyAmount.of(USD, 5.0e4);
	  private static readonly ResolvedFxSingleBarrierOption PRODUCT = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
	  private static readonly TradeInfo TRADE_INFO = TradeInfo.of(date(2014, 11, 12));
	  private static readonly Payment PREMIUM = Payment.of(CurrencyAmount.of(EUR, NOTIONAL * 0.05), date(2014, 11, 14));

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedFxSingleBarrierOptionTrade test = ResolvedFxSingleBarrierOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
		assertEquals(test.Product, PRODUCT);
		assertEquals(test.Info, TRADE_INFO);
		assertEquals(test.Premium, PREMIUM);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedFxSingleBarrierOptionTrade test1 = ResolvedFxSingleBarrierOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
		ResolvedFxSingleBarrierOptionTrade test2 = ResolvedFxSingleBarrierOptionTrade.builder().product(ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER)).premium(Payment.of(CurrencyAmount.of(EUR, NOTIONAL * 0.01), date(2014, 11, 13))).build();
		coverImmutableBean(test1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedFxSingleBarrierOptionTrade test = ResolvedFxSingleBarrierOptionTrade.builder().info(TRADE_INFO).product(PRODUCT).premium(PREMIUM).build();
		assertSerialization(test);
	  }

	}

}
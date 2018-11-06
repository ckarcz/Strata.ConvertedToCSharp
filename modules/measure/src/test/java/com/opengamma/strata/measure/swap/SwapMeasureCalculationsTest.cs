/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.KNOWN_AMOUNT_SWAP_LEG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.swap.SwapDummyData.SWAP_TRADE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using LegAmounts = com.opengamma.strata.market.amount.LegAmounts;
	using SwapLegAmount = com.opengamma.strata.market.amount.SwapLegAmount;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Test <seealso cref="SwapMeasureCalculations"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapMeasureCalculationsTest
	public class SwapMeasureCalculationsTest
	{

	  public virtual void test_legInitialNotional()
	  {
		ResolvedSwapLeg firstLeg = SWAP_TRADE.Product.Legs.get(0);
		ResolvedSwapLeg secondLeg = SWAP_TRADE.Product.Legs.get(1);
		Currency ccy = firstLeg.Currency;
		RatePaymentPeriod firstPaymentPeriod = (RatePaymentPeriod) firstLeg.PaymentPeriods.get(0);
		double notional = firstPaymentPeriod.Notional;

		LegAmounts expected = LegAmounts.of(SwapLegAmount.of(firstLeg, CurrencyAmount.of(ccy, notional)), SwapLegAmount.of(secondLeg, CurrencyAmount.of(ccy, notional)));

		assertEquals(SwapMeasureCalculations.DEFAULT.legInitialNotional(SWAP_TRADE), expected);
	  }

	  public virtual void test_legInitialNotionalWithoutNotional()
	  {
		ResolvedSwapTrade trade = ResolvedSwapTrade.builder().product(ResolvedSwap.of(KNOWN_AMOUNT_SWAP_LEG, KNOWN_AMOUNT_SWAP_LEG)).build();

		assertThrowsIllegalArg(() => SwapMeasureCalculations.DEFAULT.legInitialNotional(trade));
	  }

	}

}
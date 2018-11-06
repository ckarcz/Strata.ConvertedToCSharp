/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.amount
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;

	/// <summary>
	/// Test <seealso cref="SwapLegAmount"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapLegAmountTest
	public class SwapLegAmountTest
	{

	  private static readonly CurrencyAmount CURRENCY_AMOUNT = CurrencyAmount.of(Currency.USD, 123.45);

	  public virtual void test_of()
	  {
		SwapPaymentPeriod pp = mock(typeof(SwapPaymentPeriod));
		when(pp.Currency).thenReturn(Currency.GBP);
		ResolvedSwapLeg leg = ResolvedSwapLeg.builder().type(SwapLegType.FIXED).payReceive(PayReceive.PAY).paymentPeriods(pp).build();
		SwapLegAmount legAmount = SwapLegAmount.of(leg, CurrencyAmount.of(Currency.GBP, 10));
		SwapLegAmount test = legAmount.convertedTo(Currency.USD, FxRate.of(Currency.GBP, Currency.USD, 1.6));

		assertThat(test.Amount.Currency).isEqualTo(Currency.USD);
		assertThat(test.Amount.Amount).isEqualTo(16.0);
		assertThat(test.PayReceive).isEqualTo(legAmount.PayReceive);
		assertThat(test.Type).isEqualTo(legAmount.Type);
		assertThat(test.Currency).isEqualTo(legAmount.Currency);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void convertedTo()
	  {
		SwapLegAmount legAmount = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.GBP, 10)).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.GBP).build();
		SwapLegAmount convertedAmount = legAmount.convertedTo(Currency.USD, FxRate.of(Currency.GBP, Currency.USD, 1.6));

		assertThat(convertedAmount.Amount.Currency).isEqualTo(Currency.USD);
		assertThat(convertedAmount.Amount.Amount).isEqualTo(16.0);
		assertThat(convertedAmount.PayReceive).isEqualTo(legAmount.PayReceive);
		assertThat(convertedAmount.Type).isEqualTo(legAmount.Type);
		assertThat(convertedAmount.Currency).isEqualTo(legAmount.Currency);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwapLegAmount la1 = SwapLegAmount.builder().amount(CURRENCY_AMOUNT).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.EUR).build();
		coverImmutableBean(la1);
		SwapLegAmount la2 = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.GBP, 10000)).payReceive(PayReceive.RECEIVE).type(SwapLegType.IBOR).currency(Currency.GBP).build();
		coverBeanEquals(la1, la2);
	  }

	  public virtual void test_serialization()
	  {
		SwapLegAmount la = SwapLegAmount.builder().amount(CURRENCY_AMOUNT).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.EUR).build();
		assertSerialization(la);
	  }

	}

}
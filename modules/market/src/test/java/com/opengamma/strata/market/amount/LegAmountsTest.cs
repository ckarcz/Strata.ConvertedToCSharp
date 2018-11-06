using System.Collections.Generic;

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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	/// Test {@LegAmounts}.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LegAmountsTest
	public class LegAmountsTest
	{

	  private static readonly LegAmount LEG_AMOUNT_1 = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.USD, 500)).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.USD).build();
	  private static readonly LegAmount LEG_AMOUNT_2 = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.USD, 420)).payReceive(PayReceive.RECEIVE).type(SwapLegType.IBOR).currency(Currency.USD).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_arrayAmounts()
	  {
		LegAmounts test = LegAmounts.of(LEG_AMOUNT_1, LEG_AMOUNT_2);
		assertEquals(test.Amounts.size(), 2);
		assertEquals(test.Amounts.get(0), LEG_AMOUNT_1);
		assertEquals(test.Amounts.get(1), LEG_AMOUNT_2);
	  }

	  public virtual void test_of_list()
	  {
		IList<LegAmount> list = ImmutableList.of(LEG_AMOUNT_1, LEG_AMOUNT_2);
		LegAmounts test = LegAmounts.of(list);
		assertEquals(test.Amounts.size(), 2);
		assertEquals(test.Amounts.get(0), LEG_AMOUNT_1);
		assertEquals(test.Amounts.get(1), LEG_AMOUNT_2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void convertedTo()
	  {
		LegAmounts @base = LegAmounts.of(LEG_AMOUNT_1, LEG_AMOUNT_2);
		LegAmounts test = @base.convertedTo(Currency.GBP, FxRate.of(Currency.USD, Currency.GBP, 0.7));

		assertThat(test.Amounts.get(0).Amount.Currency).isEqualTo(Currency.GBP);
		assertThat(test.Amounts.get(0).Amount.Amount).isEqualTo(500d * 0.7d);
		assertThat(test.Amounts.get(1).Amount.Currency).isEqualTo(Currency.GBP);
		assertThat(test.Amounts.get(1).Amount.Amount).isEqualTo(420d * 0.7d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		LegAmounts test1 = LegAmounts.of(LEG_AMOUNT_1, LEG_AMOUNT_2);
		coverImmutableBean(test1);

		LegAmount swapLeg = SwapLegAmount.builder().amount(CurrencyAmount.of(Currency.GBP, 1557.445)).payReceive(PayReceive.PAY).type(SwapLegType.FIXED).currency(Currency.EUR).build();
		LegAmounts test2 = LegAmounts.of(swapLeg);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		LegAmounts test = LegAmounts.of(LEG_AMOUNT_1, LEG_AMOUNT_2);
		assertSerialization(test);
	  }

	}

}
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
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="ResolvedFxSingleBarrierOption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxSingleBarrierOptionTest
	public class ResolvedFxSingleBarrierOptionTest
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

	  public virtual void test_of()
	  {
		ResolvedFxSingleBarrierOption test = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		assertEquals(test.Barrier, BARRIER);
		assertEquals(test.Rebate.get(), REBATE);
		assertEquals(test.UnderlyingOption, VANILLA_OPTION);
		assertEquals(test.CurrencyPair, VANILLA_OPTION.CurrencyPair);
	  }

	  public virtual void test_of_noRebate()
	  {
		ResolvedFxSingleBarrierOption test = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER);
		assertEquals(test.Barrier, BARRIER);
		assertFalse(test.Rebate.Present);
		assertEquals(test.UnderlyingOption, VANILLA_OPTION);
	  }

	  public virtual void test_of_fail()
	  {
		CurrencyAmount negative = CurrencyAmount.of(USD, -5.0e4);
		assertThrowsIllegalArg(() => ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, negative));
		CurrencyAmount other = CurrencyAmount.of(GBP, 5.0e4);
		assertThrowsIllegalArg(() => ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, other));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedFxSingleBarrierOption test1 = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		ResolvedFxSingleBarrierOption test2 = ResolvedFxSingleBarrierOption.of(ResolvedFxVanillaOption.builder().longShort(SHORT).expiry(EXPIRY_DATE_TIME).underlying(FX).build(), SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, 1.5));
		coverImmutableBean(test1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedFxSingleBarrierOption test = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		assertSerialization(test);
	  }

	}

}
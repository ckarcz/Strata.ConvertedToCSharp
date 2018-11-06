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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;
	using KnockType = com.opengamma.strata.product.option.KnockType;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Test <seealso cref="FxSingleBarrierOption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSingleBarrierOptionTest
	public class FxSingleBarrierOptionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2015, 2, 14);
	  private static readonly LocalTime EXPIRY_TIME = LocalTime.of(12, 15);
	  private static readonly ZoneId EXPIRY_ZONE = ZoneId.of("Z");
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 2, 16);
	  private const double NOTIONAL = 1.0e6;
	  private static readonly CurrencyAmount EUR_AMOUNT = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT = CurrencyAmount.of(USD, -NOTIONAL * 1.35);
	  private static readonly FxSingle FX = FxSingle.of(EUR_AMOUNT, USD_AMOUNT, PAYMENT_DATE);
	  private static readonly FxVanillaOption VANILLA_OPTION = FxVanillaOption.builder().longShort(LONG).expiryDate(EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).underlying(FX).build();
	  private static readonly SimpleConstantContinuousBarrier BARRIER = SimpleConstantContinuousBarrier.of(BarrierType.DOWN, KnockType.KNOCK_IN, 1.2);
	  private static readonly CurrencyAmount REBATE = CurrencyAmount.of(USD, 5.0e4);

	  public virtual void test_of()
	  {
		FxSingleBarrierOption test = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		assertEquals(test.Barrier, BARRIER);
		assertEquals(test.Rebate.get(), REBATE);
		assertEquals(test.UnderlyingOption, VANILLA_OPTION);
		assertEquals(test.CurrencyPair, VANILLA_OPTION.CurrencyPair);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(EUR, USD));
		assertEquals(test.allCurrencies(), ImmutableSet.of(EUR, USD));
	  }

	  public virtual void test_builder()
	  {
		FxSingleBarrierOption test = FxSingleBarrierOption.builder().underlyingOption(VANILLA_OPTION).barrier(BARRIER).rebate(REBATE).build();
		assertEquals(test.Barrier, BARRIER);
		assertEquals(test.Rebate.get(), REBATE);
		assertEquals(test.UnderlyingOption, VANILLA_OPTION);
		assertEquals(test.CurrencyPair, VANILLA_OPTION.CurrencyPair);
	  }

	  public virtual void test_of_noRebate()
	  {
		FxSingleBarrierOption test = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER);
		assertEquals(test.Barrier, BARRIER);
		assertFalse(test.Rebate.Present);
		assertEquals(test.UnderlyingOption, VANILLA_OPTION);
	  }

	  public virtual void test_of_fail()
	  {
		CurrencyAmount negative = CurrencyAmount.of(USD, -5.0e4);
		assertThrowsIllegalArg(() => FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, negative));
		CurrencyAmount other = CurrencyAmount.of(GBP, 5.0e4);
		assertThrowsIllegalArg(() => FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, other));
	  }

	  public virtual void test_resolve()
	  {
		FxSingleBarrierOption @base = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		ResolvedFxSingleBarrierOption expected = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION.resolve(REF_DATA), BARRIER, REBATE);
		assertEquals(@base.resolve(REF_DATA), expected);
	  }

	  public virtual void test_resolve_noRebate()
	  {
		FxSingleBarrierOption @base = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER);
		ResolvedFxSingleBarrierOption expected = ResolvedFxSingleBarrierOption.of(VANILLA_OPTION.resolve(REF_DATA), BARRIER);
		assertEquals(@base.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxSingleBarrierOption test1 = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		FxSingleBarrierOption test2 = FxSingleBarrierOption.of(FxVanillaOption.builder().longShort(SHORT).expiryDate(EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(EXPIRY_ZONE).underlying(FX).build(), SimpleConstantContinuousBarrier.of(BarrierType.UP, KnockType.KNOCK_IN, 1.5));
		coverImmutableBean(test1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxSingleBarrierOption test = FxSingleBarrierOption.of(VANILLA_OPTION, BARRIER, REBATE);
		assertSerialization(test);
	  }

	}

}
/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ValueStep = com.opengamma.strata.basics.value.ValueStep;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NotionalScheduleTest
	public class NotionalScheduleTest
	{

	  private static readonly CurrencyAmount CA_GBP_1000 = CurrencyAmount.of(GBP, 1000d);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurrencyAmount()
	  {
		NotionalSchedule test = NotionalSchedule.of(CA_GBP_1000);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, ValueSchedule.of(1000d));
		assertEquals(test.FxReset, null);
		assertEquals(test.InitialExchange, false);
		assertEquals(test.IntermediateExchange, false);
		assertEquals(test.FinalExchange, false);
	  }

	  public virtual void test_of_CurrencyAndAmount()
	  {
		NotionalSchedule test = NotionalSchedule.of(GBP, 1000d);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, ValueSchedule.of(1000d));
		assertEquals(test.FxReset, null);
		assertEquals(test.InitialExchange, false);
		assertEquals(test.IntermediateExchange, false);
		assertEquals(test.FinalExchange, false);
	  }

	  public virtual void test_of_CurrencyAndValueSchedule()
	  {
		ValueSchedule valueSchedule = ValueSchedule.of(1000d, ValueStep.of(1, ValueAdjustment.ofReplace(2000d)));
		NotionalSchedule test = NotionalSchedule.of(GBP, valueSchedule);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Amount, valueSchedule);
		assertEquals(test.FxReset, null);
		assertEquals(test.InitialExchange, false);
		assertEquals(test.IntermediateExchange, false);
		assertEquals(test.FinalExchange, false);
	  }

	  public virtual void test_builder_FxResetSetsFlags()
	  {
		FxResetCalculation fxReset = FxResetCalculation.builder().referenceCurrency(GBP).index(GBP_USD_WM).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build();
		NotionalSchedule test = NotionalSchedule.builder().currency(USD).amount(ValueSchedule.of(2000d)).intermediateExchange(true).finalExchange(true).fxReset(fxReset).build();
		assertEquals(test.Currency, USD);
		assertEquals(test.Amount, ValueSchedule.of(2000d));
		assertEquals(test.FxReset, fxReset);
		assertEquals(test.InitialExchange, false);
		assertEquals(test.IntermediateExchange, true);
		assertEquals(test.FinalExchange, true);
	  }

	  public virtual void test_builder_invalidCurrencyFxReset()
	  {
		assertThrowsIllegalArg(() => NotionalSchedule.builder().currency(USD).amount(ValueSchedule.of(2000d)).fxReset(FxResetCalculation.builder().referenceCurrency(USD).index(GBP_USD_WM).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build());
		assertThrowsIllegalArg(() => NotionalSchedule.builder().currency(EUR).amount(ValueSchedule.of(2000d)).fxReset(FxResetCalculation.builder().referenceCurrency(USD).index(GBP_USD_WM).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).build());
	  }

	  public virtual void test_of_null()
	  {
		assertThrowsIllegalArg(() => NotionalSchedule.of(null));
		assertThrowsIllegalArg(() => NotionalSchedule.of(null, 1000d));
		assertThrowsIllegalArg(() => NotionalSchedule.of(GBP, null));
		assertThrowsIllegalArg(() => NotionalSchedule.of(null, ValueSchedule.of(1000d)));
		assertThrowsIllegalArg(() => NotionalSchedule.of(null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		NotionalSchedule test = NotionalSchedule.of(GBP, 1000d);
		coverImmutableBean(test);
		NotionalSchedule test2 = NotionalSchedule.builder().currency(USD).amount(ValueSchedule.of(2000d)).fxReset(FxResetCalculation.builder().referenceCurrency(GBP).index(GBP_USD_WM).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, GBLO)).build()).initialExchange(true).intermediateExchange(true).finalExchange(true).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		NotionalSchedule test = NotionalSchedule.of(GBP, 1000d);
		assertSerialization(test);
	  }

	}

}
/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_GBP_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_USD_ECB;
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxResetCalculationTest
	public class FxResetCalculationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly DaysAdjustment MINUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(-2, EUTA);
	  private static readonly DaysAdjustment MINUS_THREE_DAYS = DaysAdjustment.ofBusinessDays(-3, EUTA);
	  private static readonly LocalDate DATE_2014_03_31 = date(2014, 3, 31);
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);

	  public virtual void test_builder()
	  {
		FxResetCalculation test = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).fixingRelativeTo(FxResetFixingRelativeTo.PERIOD_START).build();
		assertEquals(test.Index, EUR_GBP_ECB);
		assertEquals(test.ReferenceCurrency, GBP);
		assertEquals(test.FixingDateOffset, MINUS_TWO_DAYS);
		assertEquals(test.FixingRelativeTo, FxResetFixingRelativeTo.PERIOD_START);
	  }

	  public virtual void test_builder_defaults()
	  {
		FxResetCalculation test = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).build();
		assertEquals(test.Index, EUR_GBP_ECB);
		assertEquals(test.ReferenceCurrency, GBP);
		assertEquals(test.FixingDateOffset, EUR_GBP_ECB.FixingDateOffset);
		assertEquals(test.FixingRelativeTo, FxResetFixingRelativeTo.PERIOD_START);
	  }

	  public virtual void test_invalidCurrency()
	  {
		assertThrowsIllegalArg(() => FxResetCalculation.builder().index(EUR_USD_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve_beforeStart_weekend()
	  {
		FxResetCalculation @base = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).build();
		Optional<FxReset> test = @base.resolve(REF_DATA).apply(0, SchedulePeriod.of(DATE_2014_03_31, DATE_2014_06_30));
		assertEquals(test, FxReset.of(FxIndexObservation.of(EUR_GBP_ECB, date(2014, 3, 27), REF_DATA), GBP));
	  }

	  public virtual void test_resolve_beforeEnd_weekend()
	  {
		FxResetCalculation @base = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).fixingRelativeTo(FxResetFixingRelativeTo.PERIOD_END).build();
		Optional<FxReset> test = @base.resolve(REF_DATA).apply(0, SchedulePeriod.of(DATE_2014_03_31, DATE_2014_06_30));
		assertEquals(test, FxReset.of(FxIndexObservation.of(EUR_GBP_ECB, date(2014, 6, 26), REF_DATA), GBP));
	  }

	  public virtual void test_resolve_beforeStart_threeDays()
	  {
		FxResetCalculation @base = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_THREE_DAYS).build();
		Optional<FxReset> test = @base.resolve(REF_DATA).apply(0, SchedulePeriod.of(DATE_2014_03_31, DATE_2014_06_30));
		assertEquals(test, FxReset.of(FxIndexObservation.of(EUR_GBP_ECB, date(2014, 3, 26), REF_DATA), GBP));
	  }

	  public virtual void test_resolve_initial_notional_override()
	  {
		FxResetCalculation @base = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).initialNotionalValue(100000d).build();
		Optional<FxReset> fxResetFirstPeriod = @base.resolve(REF_DATA).apply(0, SchedulePeriod.of(DATE_2014_03_31, DATE_2014_06_30));
		assertFalse(fxResetFirstPeriod.Present);

		Optional<FxReset> fxResetSecondPeriod = @base.resolve(REF_DATA).apply(1, SchedulePeriod.of(DATE_2014_03_31, DATE_2014_06_30));
		assertTrue(fxResetSecondPeriod.Present);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxResetCalculation test = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).build();
		coverImmutableBean(test);
		FxResetCalculation test2 = FxResetCalculation.builder().index(EUR_USD_ECB).referenceCurrency(Currency.EUR).fixingDateOffset(MINUS_THREE_DAYS).fixingRelativeTo(FxResetFixingRelativeTo.PERIOD_END).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxResetCalculation test = FxResetCalculation.builder().index(EUR_GBP_ECB).referenceCurrency(GBP).fixingDateOffset(MINUS_TWO_DAYS).build();
		assertSerialization(test);
	  }

	}

}
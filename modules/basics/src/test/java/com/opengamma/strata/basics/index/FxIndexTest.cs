/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.NO_HOLIDAYS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_CHF_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;

	/// <summary>
	/// Test <seealso cref="FxIndex"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxIndexTest
	public class FxIndexTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {FxIndices.EUR_CHF_ECB, "EUR/CHF-ECB"},
			new object[] {FxIndices.EUR_GBP_ECB, "EUR/GBP-ECB"},
			new object[] {FxIndices.EUR_JPY_ECB, "EUR/JPY-ECB"},
			new object[] {FxIndices.EUR_USD_ECB, "EUR/USD-ECB"},
			new object[] {FxIndices.USD_CHF_WM, "USD/CHF-WM"},
			new object[] {FxIndices.EUR_USD_WM, "EUR/USD-WM"},
			new object[] {FxIndices.GBP_USD_WM, "GBP/USD-WM"},
			new object[] {FxIndices.USD_JPY_WM, "USD/JPY-WM"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(FxIndex convention, String name)
	  public virtual void test_name(FxIndex convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FxIndex convention, String name)
	  public virtual void test_toString(FxIndex convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FxIndex convention, String name)
	  public virtual void test_of_lookup(FxIndex convention, string name)
	  {
		assertEquals(FxIndex.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(FxIndex convention, String name)
	  public virtual void test_extendedEnum(FxIndex convention, string name)
	  {
		ImmutableMap<string, FxIndex> map = FxIndex.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FxIndex.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FxIndex.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ecb_eur_gbp_dates()
	  {
		FxIndex test = FxIndices.EUR_GBP_ECB;
		assertEquals(test.FixingDateOffset, DaysAdjustment.ofBusinessDays(-2, EUTA.combinedWith(GBLO)));
		assertEquals(test.MaturityDateOffset, DaysAdjustment.ofBusinessDays(2, EUTA.combinedWith(GBLO)));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 13), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 15), REF_DATA), date(2014, 10, 13));
		// weekend
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 16), REF_DATA), date(2014, 10, 20));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 20), REF_DATA), date(2014, 10, 16));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 17), REF_DATA), date(2014, 10, 21));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 21), REF_DATA), date(2014, 10, 17));
		// input date is Sunday
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 19), REF_DATA), date(2014, 10, 22));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 19), REF_DATA), date(2014, 10, 16));
		// skip maturity over EUR (1st May) and GBP (5th May) holiday
		assertEquals(test.calculateMaturityFromFixing(date(2014, 4, 30), REF_DATA), date(2014, 5, 6));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 5, 6), REF_DATA), date(2014, 4, 30));
		// resolve
		assertEquals(test.resolve(REF_DATA).apply(date(2014, 5, 6)), FxIndexObservation.of(test, date(2014, 5, 6), REF_DATA));
	  }

	  public virtual void test_dates()
	  {
		FxIndex test = ImmutableFxIndex.builder().name("Test").currencyPair(CurrencyPair.of(EUR, GBP)).fixingCalendar(NO_HOLIDAYS).maturityDateOffset(DaysAdjustment.ofCalendarDays(2)).build();
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 13), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 15), REF_DATA), date(2014, 10, 13));
		// weekend
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 16), REF_DATA), date(2014, 10, 18));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 18), REF_DATA), date(2014, 10, 16));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 17), REF_DATA), date(2014, 10, 19));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 19), REF_DATA), date(2014, 10, 17));
		// input date is Sunday
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 19), REF_DATA), date(2014, 10, 21));
		assertEquals(test.calculateFixingFromMaturity(date(2014, 10, 19), REF_DATA), date(2014, 10, 17));
	  }

	  public virtual void test_cny()
	  {
		FxIndex test = FxIndex.of("USD/CNY-SAEC-CNY01");
		assertEquals(test.Name, "USD/CNY-SAEC-CNY01");
	  }

	  public virtual void test_inr()
	  {
		FxIndex test = FxIndex.of("USD/INR-FBIL-INR01");
		assertEquals(test.Name, "USD/INR-FBIL-INR01");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		ImmutableFxIndex a = ImmutableFxIndex.builder().name("GBP-EUR").currencyPair(CurrencyPair.of(GBP, EUR)).fixingCalendar(GBLO).maturityDateOffset(DaysAdjustment.ofBusinessDays(2, GBLO)).build();
		ImmutableFxIndex b = a.toBuilder().name("EUR-GBP").build();
		assertEquals(a.Equals(b), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FxIndices));
		coverImmutableBean((ImmutableBean) EUR_CHF_ECB);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FxIndex), EUR_CHF_ECB);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(EUR_CHF_ECB);
	  }

	}

}
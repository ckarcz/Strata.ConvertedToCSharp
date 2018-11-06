using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using FxRateId = com.opengamma.strata.data.FxRateId;

	/// <summary>
	/// Test <seealso cref="FxRatesCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRatesCsvLoaderTest
	public class FxRatesCsvLoaderTest
	{

	  private static readonly FxRateId EUR_USD_ID = FxRateId.of(Currency.EUR, Currency.USD);
	  private static readonly FxRateId GBP_USD_ID = FxRateId.of(Currency.GBP, Currency.USD);
	  private static readonly FxRateId EUR_CHF_ID = FxRateId.of(Currency.EUR, Currency.CHF);
	  private static readonly LocalDate DATE1 = date(2014, 1, 22);
	  private static readonly LocalDate DATE2 = date(2014, 1, 23);

	  private static readonly ResourceLocator RATES_1 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fx-rates-1.csv");
	  private static readonly ResourceLocator RATES_2 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fx-rates-2.csv");
	  private static readonly ResourceLocator RATES_INVALID_DATE = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fx-rates-invalid-date.csv");
	  private static readonly ResourceLocator RATES_INVALID_DUPLICATE = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fx-rates-invalid-duplicate.csv");

	  //-------------------------------------------------------------------------
	  public virtual void test_load_oneDate_file1_date1()
	  {
		IDictionary<FxRateId, FxRate> map = FxRatesCsvLoader.load(DATE1, RATES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1(map);
	  }

	  public virtual void test_load_oneDate_file1_date2()
	  {
		IDictionary<FxRateId, FxRate> map = FxRatesCsvLoader.load(DATE2, ImmutableList.of(RATES_1));
		assertEquals(map.Count, 2);
		assertFile1Date2(map);
	  }

	  public virtual void test_load_oneDate_file1file2_date1()
	  {
		IDictionary<FxRateId, FxRate> map = FxRatesCsvLoader.load(DATE1, ImmutableList.of(RATES_1, RATES_2));
		assertEquals(map.Count, 3);
		assertFile1Date1(map);
		assertFile2Date1(map);
	  }

	  public virtual void test_load_oneDate_invalidDate()
	  {
		assertThrows(() => FxRatesCsvLoader.load(date(2015, 10, 2), RATES_INVALID_DATE), typeof(System.ArgumentException), "Error processing resource as CSV file: .*");
	  }

	  public virtual void test_invalidDuplicate()
	  {
		assertThrowsIllegalArg(() => FxRatesCsvLoader.load(DATE1, RATES_INVALID_DUPLICATE));
	  }

	  public virtual void test_load_dateSet_file1_date1()
	  {
		IDictionary<LocalDate, ImmutableMap<FxRateId, FxRate>> map = FxRatesCsvLoader.load(ImmutableSet.of(DATE1, DATE2), RATES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1(map[DATE1]);
		assertFile1Date2(map[DATE2]);
	  }

	  public virtual void test_load_alLDates_file1_date1()
	  {
		IDictionary<LocalDate, ImmutableMap<FxRateId, FxRate>> map = FxRatesCsvLoader.loadAllDates(RATES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1(map[DATE1]);
		assertFile1Date2(map[DATE2]);
	  }

	  //-------------------------------------------------------------------------
	  private void assertFile1Date1(IDictionary<FxRateId, FxRate> map)
	  {
		assertTrue(map.ContainsKey(EUR_USD_ID));
		assertTrue(map.ContainsKey(GBP_USD_ID));
		assertEquals(map[EUR_USD_ID], FxRate.of(Currency.EUR, Currency.USD, 1.11));
		assertEquals(map[GBP_USD_ID], FxRate.of(Currency.GBP, Currency.USD, 1.51));
	  }

	  private void assertFile1Date2(IDictionary<FxRateId, FxRate> map)
	  {
		assertTrue(map.ContainsKey(EUR_USD_ID));
		assertTrue(map.ContainsKey(GBP_USD_ID));
		assertEquals(map[EUR_USD_ID], FxRate.of(Currency.EUR, Currency.USD, 1.12));
		assertEquals(map[GBP_USD_ID], FxRate.of(Currency.GBP, Currency.USD, 1.52));
	  }

	  private void assertFile2Date1(IDictionary<FxRateId, FxRate> map)
	  {
		assertTrue(map.ContainsKey(EUR_CHF_ID));
		assertEquals(map[EUR_CHF_ID], FxRate.of(Currency.EUR, Currency.CHF, 1.09));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FxRatesCsvLoader));
	  }

	}

}
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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;

	/// <summary>
	/// Test <seealso cref="FixingSeriesCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixingSeriesCsvLoaderTest
	public class FixingSeriesCsvLoaderTest
	{

	  private static readonly IndexQuoteId ID_USD_LIBOR_3M = IndexQuoteId.of(IborIndices.USD_LIBOR_3M);
	  private static readonly IndexQuoteId ID_USD_LIBOR_6M = IndexQuoteId.of(IborIndices.USD_LIBOR_6M);
	  private static readonly IndexQuoteId ID_GB_RPI = IndexQuoteId.of(PriceIndices.GB_RPI);

	  private static readonly ResourceLocator FIXING_SERIES_1 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-1.csv");
	  private static readonly ResourceLocator FIXING_SERIES_2 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-2.csv");
	  private static readonly ResourceLocator FIXING_SERIES_1_AND_2 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-1-and-2.csv");
	  private static readonly ResourceLocator FIXING_SERIES_INVALID_DATE = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-invalid-date.csv");
	  private static readonly ResourceLocator FIXING_SERIES_PRICE1 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-price1.csv");
	  private static readonly ResourceLocator FIXING_SERIES_PRICE2 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-price2.csv");
	  private static readonly ResourceLocator FIXING_SERIES_PRICE_INVALID = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/fixings-price-invalid.csv");

	  //-------------------------------------------------------------------------
	  public virtual void test_single_series_single_file()
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> ts = FixingSeriesCsvLoader.load(ImmutableList.of(FIXING_SERIES_1));

		assertEquals(ts.Count, 1);
		assertTrue(ts.ContainsKey(ID_USD_LIBOR_3M));
		assertLibor3mSeries(ts[ID_USD_LIBOR_3M]);
	  }

	  public virtual void test_multiple_series_single_file()
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> ts = FixingSeriesCsvLoader.load(ImmutableList.of(FIXING_SERIES_1_AND_2));
		assertLibor3m6mSeries(ts);
	  }

	  public virtual void test_multiple_series_multiple_files()
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> ts = FixingSeriesCsvLoader.load(FIXING_SERIES_1, FIXING_SERIES_2);
		assertLibor3m6mSeries(ts);
	  }

	  public virtual void test_priceIndex1()
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> ts = FixingSeriesCsvLoader.load(FIXING_SERIES_PRICE1);
		assertEquals(ts.Count, 1);
		assertTrue(ts.ContainsKey(ID_GB_RPI));
		assertPriceIndexSeries(ts[ID_GB_RPI]);
	  }

	  public virtual void test_priceIndex2()
	  {
		IDictionary<ObservableId, LocalDateDoubleTimeSeries> ts = FixingSeriesCsvLoader.load(FIXING_SERIES_PRICE2);
		assertEquals(ts.Count, 1);
		assertTrue(ts.ContainsKey(ID_GB_RPI));
		assertPriceIndexSeries(ts[ID_GB_RPI]);
	  }

	  public virtual void test_priceIndex_invalidDate()
	  {
		assertThrowsIllegalArg(() => FixingSeriesCsvLoader.load(FIXING_SERIES_PRICE_INVALID));
	  }

	  public virtual void test_single_series_multiple_files()
	  {
		assertThrows(() => FixingSeriesCsvLoader.load(FIXING_SERIES_1, FIXING_SERIES_1), typeof(System.ArgumentException), "Multiple entries with same key: .*");
	  }

	  public virtual void test_invalidDate()
	  {
		assertThrows(() => FixingSeriesCsvLoader.load(FIXING_SERIES_INVALID_DATE), typeof(System.ArgumentException), "Error processing resource as CSV file: .*");
	  }

	  //-------------------------------------------------------------------------
	  private void assertLibor3m6mSeries(IDictionary<ObservableId, LocalDateDoubleTimeSeries> ts)
	  {
		assertEquals(ts.Count, 2);
		assertTrue(ts.ContainsKey(ID_USD_LIBOR_3M));
		assertTrue(ts.ContainsKey(ID_USD_LIBOR_6M));
		assertLibor3mSeries(ts[ID_USD_LIBOR_3M]);
		assertLibor6mSeries(ts[ID_USD_LIBOR_6M]);
	  }

	  private void assertLibor3mSeries(LocalDateDoubleTimeSeries actualSeries)
	  {
		assertEquals(actualSeries.size(), 3);
		LocalDateDoubleTimeSeries expectedSeries = LocalDateDoubleTimeSeries.builder().put(LocalDate.of(1971, 1, 4), 0.065).put(LocalDate.of(1971, 1, 5), 0.0638).put(LocalDate.of(1971, 1, 6), 0.0638).build();
		assertEquals(actualSeries, expectedSeries);
	  }

	  private void assertLibor6mSeries(LocalDateDoubleTimeSeries actualSeries)
	  {
		assertEquals(actualSeries.size(), 3);
		LocalDateDoubleTimeSeries expectedSeries = LocalDateDoubleTimeSeries.builder().put(LocalDate.of(1971, 1, 4), 0.0681).put(LocalDate.of(1971, 1, 5), 0.0675).put(LocalDate.of(1971, 1, 6), 0.0669).build();
		assertEquals(actualSeries, expectedSeries);
	  }

	  private void assertPriceIndexSeries(LocalDateDoubleTimeSeries actualSeries)
	  {
		assertEquals(actualSeries.size(), 3);
		LocalDateDoubleTimeSeries expectedSeries = LocalDateDoubleTimeSeries.builder().put(LocalDate.of(2017, 1, 31), 200).put(LocalDate.of(2017, 2, 28), 300).put(LocalDate.of(2017, 3, 31), 390).build();
		assertEquals(actualSeries, expectedSeries);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FixingSeriesCsvLoader));
	  }

	}

}
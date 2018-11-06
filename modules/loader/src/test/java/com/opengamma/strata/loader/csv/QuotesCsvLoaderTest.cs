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
	using StandardId = com.opengamma.strata.basics.StandardId;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Test <seealso cref="QuotesCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuotesCsvLoaderTest
	public class QuotesCsvLoaderTest
	{

	  private static readonly QuoteId FGBL_MAR14 = QuoteId.of(StandardId.of("OG-Future", "Eurex-FGBL-Mar14"));
	  private static readonly QuoteId ED_MAR14 = QuoteId.of(StandardId.of("OG-Future", "CME-ED-Mar14"));
	  private static readonly QuoteId FGBL_JUN14 = QuoteId.of(StandardId.of("OG-Future", "Eurex-FGBL-Jun14"));

	  private static readonly LocalDate DATE1 = date(2014, 1, 22);
	  private static readonly LocalDate DATE2 = date(2014, 1, 23);

	  private static readonly ResourceLocator QUOTES_1 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/quotes-1.csv");
	  private static readonly ResourceLocator QUOTES_2 = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/quotes-2.csv");
	  private static readonly ResourceLocator QUOTES_INVALID_DATE = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/quotes-invalid-date.csv");
	  private static readonly ResourceLocator QUOTES_INVALID_DUPLICATE = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/quotes-invalid-duplicate.csv");

	  //-------------------------------------------------------------------------
	  public virtual void test_noFiles()
	  {
		IDictionary<QuoteId, double> map = QuotesCsvLoader.load(DATE1);
		assertEquals(map.Count, 0);
	  }

	  public virtual void test_load_oneDate_file1_date1()
	  {
		IDictionary<QuoteId, double> map = QuotesCsvLoader.load(DATE1, QUOTES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1(map);
	  }

	  public virtual void test_load_oneDate_file1_date1date2()
	  {
		IDictionary<LocalDate, ImmutableMap<QuoteId, double>> map = QuotesCsvLoader.load(ImmutableSet.of(DATE1, DATE2), QUOTES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1Date2(map);
	  }

	  public virtual void test_load_oneDate_file1_date2()
	  {
		IDictionary<QuoteId, double> map = QuotesCsvLoader.load(DATE2, ImmutableList.of(QUOTES_1));
		assertEquals(map.Count, 2);
		assertFile1Date2(map);
	  }

	  public virtual void test_load_oneDate_file1file2_date1()
	  {
		IDictionary<QuoteId, double> map = QuotesCsvLoader.load(DATE1, ImmutableList.of(QUOTES_1, QUOTES_2));
		assertEquals(map.Count, 3);
		assertFile1Date1(map);
		assertFile2Date1(map);
	  }

	  public virtual void test_load_oneDate_invalidDate()
	  {
		assertThrows(() => QuotesCsvLoader.load(date(2015, 10, 2), QUOTES_INVALID_DATE), typeof(System.ArgumentException), "Error processing resource as CSV file: .*");
	  }

	  public virtual void test_invalidDuplicate()
	  {
		assertThrowsIllegalArg(() => QuotesCsvLoader.load(DATE1, QUOTES_INVALID_DUPLICATE));
	  }

	  public virtual void test_load_dateSet_file1_date1()
	  {
		IDictionary<LocalDate, ImmutableMap<QuoteId, double>> map = QuotesCsvLoader.load(ImmutableSet.of(DATE1, DATE2), QUOTES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1(map[DATE1]);
		assertFile1Date2(map[DATE2]);
	  }

	  public virtual void test_load_alLDates_file1_date1()
	  {
		IDictionary<LocalDate, ImmutableMap<QuoteId, double>> map = QuotesCsvLoader.loadAllDates(QUOTES_1);
		assertEquals(map.Count, 2);
		assertFile1Date1(map[DATE1]);
		assertFile1Date2(map[DATE2]);
	  }

	  //-------------------------------------------------------------------------
	  private void assertFile1Date1(IDictionary<QuoteId, double> map)
	  {
		assertTrue(map.ContainsKey(FGBL_MAR14));
		assertTrue(map.ContainsKey(ED_MAR14));
		assertEquals(map[FGBL_MAR14], 150.43, 1e-6);
		assertEquals(map[ED_MAR14], 99.62, 1e-6);
	  }

	  private void assertFile1Date1Date2(IDictionary<LocalDate, ImmutableMap<QuoteId, double>> map)
	  {
		assertTrue(map.ContainsKey(DATE1));
		assertTrue(map.ContainsKey(DATE2));
		assertTrue(map[DATE1].containsKey(FGBL_MAR14));
		assertTrue(map[DATE2].containsKey(FGBL_MAR14));
		assertTrue(map[DATE1].containsKey(ED_MAR14));
		assertTrue(map[DATE2].containsKey(ED_MAR14));
		assertEquals(map[DATE1].get(FGBL_MAR14), 150.43, 1e-6);
		assertEquals(map[DATE1].get(ED_MAR14), 99.62, 1e-6);
		assertEquals(map[DATE2].get(FGBL_MAR14), 150.5, 1e-6);
		assertEquals(map[DATE2].get(ED_MAR14), 99.63, 1e-6);
	  }

	  private void assertFile1Date2(IDictionary<QuoteId, double> map)
	  {
		assertTrue(map.ContainsKey(FGBL_MAR14));
		assertTrue(map.ContainsKey(ED_MAR14));
		assertEquals(map[FGBL_MAR14], 150.50, 1e-6);
		assertEquals(map[ED_MAR14], 99.63, 1e-6);
	  }

	  private void assertFile2Date1(IDictionary<QuoteId, double> map)
	  {
		assertTrue(map.ContainsKey(FGBL_JUN14));
		assertEquals(map[FGBL_JUN14], 150.99, 1e-6);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(QuotesCsvLoader));
	  }

	}

}
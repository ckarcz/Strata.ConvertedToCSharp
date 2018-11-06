using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.timeseries.DenseLocalDateDoubleTimeSeries.DenseTimeSeriesCalculation.INCLUDE_WEEKENDS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.timeseries.DenseLocalDateDoubleTimeSeries.DenseTimeSeriesCalculation.SKIP_WEEKENDS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries.empty;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotEquals;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Doubles = com.google.common.primitives.Doubles;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DenseLocalDateDoubleTimeSeriesTest
	public class DenseLocalDateDoubleTimeSeriesTest
	{

	  private const double TOLERANCE = 1e-5;
	  private const object ANOTHER_TYPE = "";

	  private static readonly LocalDate DATE_2015_06_01 = date(2015, 6, 1);
	  private static readonly LocalDate DATE_2014_06_01 = date(2014, 6, 1);
	  private static readonly LocalDate DATE_2012_06_01 = date(2012, 6, 1);
	  private static readonly LocalDate DATE_2010_06_01 = date(2010, 6, 1);
	  private static readonly LocalDate DATE_2011_06_01 = date(2011, 6, 1);
	  private static readonly LocalDate DATE_2013_06_01 = date(2013, 6, 1);
	  private static readonly LocalDate DATE_2010_01_01 = date(2010, 1, 1);
	  private static readonly LocalDate DATE_2010_01_02 = date(2010, 1, 2);
	  private static readonly LocalDate DATE_2010_01_03 = date(2010, 1, 3);
	  // Avoid weekends for these days
	  private static readonly LocalDate DATE_2011_01_01 = date(2011, 1, 1);
	  private static readonly LocalDate DATE_2012_01_01 = date(2012, 1, 1);
	  private static readonly LocalDate DATE_2013_01_01 = date(2013, 1, 1);

	  private static readonly LocalDate DATE_2014_01_01 = date(2014, 1, 1);

	  private static readonly LocalDate DATE_2015_01_02 = date(2015, 1, 2);
	  private static readonly LocalDate DATE_2015_01_03 = date(2015, 1, 3);
	  private static readonly LocalDate DATE_2015_01_04 = date(2015, 1, 4);
	  private static readonly LocalDate DATE_2015_01_05 = date(2015, 1, 5);
	  private static readonly LocalDate DATE_2015_01_06 = date(2015, 1, 6);
	  private static readonly LocalDate DATE_2015_01_07 = date(2015, 1, 7);
	  private static readonly LocalDate DATE_2015_01_08 = date(2015, 1, 8);
	  private static readonly LocalDate DATE_2015_01_09 = date(2015, 1, 9);
	  private static readonly LocalDate DATE_2015_01_11 = date(2015, 1, 11);

	  private static readonly LocalDate DATE_2015_01_12 = date(2015, 1, 12);

	  private static readonly ImmutableList<LocalDate> DATES_2015_1_WEEK = dates(DATE_2015_01_05, DATE_2015_01_06, DATE_2015_01_07, DATE_2015_01_08, DATE_2015_01_09);

	  private static readonly ImmutableList<double> VALUES_1_WEEK = values(10, 11, 12, 13, 14);
	  private static readonly ImmutableList<LocalDate> DATES_2010_12 = dates(DATE_2010_01_01, DATE_2011_01_01, DATE_2012_01_01);
	  private static readonly ImmutableList<double> VALUES_10_12 = values(10, 11, 12);
	  private static readonly ImmutableList<double> VALUES_1_3 = values(1, 2, 3);
	  private static readonly ImmutableList<double> VALUES_4_7 = values(4, 5, 6, 7);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_singleton()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.of(DATE_2011_01_01, 2d);
		assertEquals(test.Empty, false);
		assertEquals(test.size(), 1);

		// Check start is not weekend

		assertEquals(test.containsDate(DATE_2010_01_01), false);
		assertEquals(test.containsDate(DATE_2011_01_01), true);
		assertEquals(test.containsDate(DATE_2012_01_01), false);
		assertEquals(test.get(DATE_2010_01_01), double?.empty());
		assertEquals(test.get(DATE_2011_01_01), double?.of(2d));
		assertEquals(test.get(DATE_2012_01_01), double?.empty());
		assertEquals(test.dates().toArray(), new object[] {DATE_2011_01_01});
		assertEquals(test.values().toArray(), new double[] {2d});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_singleton_nullDateDisallowed()
	  public virtual void test_of_singleton_nullDateDisallowed()
	  {
		LocalDateDoubleTimeSeries.of(null, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_collectionCollection()
	  {
		ICollection<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01, DATE_2012_01_01);
		ICollection<double> values = DenseLocalDateDoubleTimeSeriesTest.values(2d, 3d);

		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();
		assertEquals(test.Empty, false);
		assertEquals(test.size(), 2);
		assertEquals(test.containsDate(DATE_2010_01_01), false);
		assertEquals(test.containsDate(DATE_2011_01_01), true);
		assertEquals(test.containsDate(DATE_2012_01_01), true);
		assertEquals(test.get(DATE_2010_01_01), double?.empty());
		assertEquals(test.get(DATE_2011_01_01), double?.of(2d));
		assertEquals(test.get(DATE_2012_01_01), double?.of(3d));
		assertEquals(test.dates().toArray(), new object[] {DATE_2011_01_01, DATE_2012_01_01});
		assertEquals(test.values().toArray(), new double[] {2d, 3d});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collectionCollection_dateCollectionNull()
	  public virtual void test_of_collectionCollection_dateCollectionNull()
	  {
		ICollection<double> values = DenseLocalDateDoubleTimeSeriesTest.values(2d, 3d);

		LocalDateDoubleTimeSeries.builder().putAll(null, values).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collectionCollection_valueCollectionNull()
	  public virtual void test_of_collectionCollection_valueCollectionNull()
	  {
		ICollection<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01, DATE_2012_01_01);

		LocalDateDoubleTimeSeries.builder().putAll(dates, (double[]) null).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collectionCollection_dateCollectionWithNull()
	  public virtual void test_of_collectionCollection_dateCollectionWithNull()
	  {
		ICollection<LocalDate> dates = Arrays.asList(DATE_2011_01_01, null);
		ICollection<double> values = DenseLocalDateDoubleTimeSeriesTest.values(2d, 3d);

		LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collectionCollection_valueCollectionWithNull()
	  public virtual void test_of_collectionCollection_valueCollectionWithNull()
	  {
		ICollection<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01, DATE_2012_01_01);
		ICollection<double> values = Arrays.asList(2d, null);

		LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collectionCollection_collectionsOfDifferentSize()
	  public virtual void test_of_collectionCollection_collectionsOfDifferentSize()
	  {
		ICollection<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01);
		ICollection<double> values = DenseLocalDateDoubleTimeSeriesTest.values(2d, 3d);

		LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();
	  }

	  public virtual void test_of_collectionCollection_datesUnordered()
	  {
		ICollection<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2012_01_01, DATE_2011_01_01);
		ICollection<double> values = DenseLocalDateDoubleTimeSeriesTest.values(2d, 1d);

		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();
		assertEquals(series.get(DATE_2011_01_01), double?.of(1d));
		assertEquals(series.get(DATE_2012_01_01), double?.of(2d));
	  }

	  public virtual void test_NaN_is_not_allowed()
	  {
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.of(DATE_2015_01_02, Double.NaN));
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().put(DATE_2015_01_02, Double.NaN));
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(ImmutableMap.of(DATE_2015_01_02, Double.NaN)));
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().put(LocalDateDoublePoint.of(DATE_2015_01_02, Double.NaN)));
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(ImmutableList.of(DATE_2015_01_02), ImmutableList.of(Double.NaN)));
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(ImmutableList.of(LocalDateDoublePoint.of(DATE_2015_01_02, Double.NaN))));

		LocalDateDoubleTimeSeries s1 = LocalDateDoubleTimeSeries.of(DATE_2015_01_02, 1d);
		LocalDateDoubleTimeSeries s2 = LocalDateDoubleTimeSeries.of(DATE_2015_01_02, 2d);

		assertThrowsIllegalArg(() => s1.intersection(s2, (d1, d2) => Double.NaN));

		assertThrowsIllegalArg(() => s1.mapValues(d => Double.NaN));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_map()
	  {
		IDictionary<LocalDate, double> map = new Dictionary<LocalDate, double>();
		map[DATE_2011_01_01] = 2d;
		map[DATE_2012_01_01] = 3d;

		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(map).build();
		assertEquals(test.Empty, false);
		assertEquals(test.size(), 2);
		assertEquals(test.containsDate(DATE_2010_01_01), false);
		assertEquals(test.containsDate(DATE_2011_01_01), true);
		assertEquals(test.containsDate(DATE_2012_01_01), true);
		assertEquals(test.get(DATE_2010_01_01), double?.empty());
		assertEquals(test.get(DATE_2011_01_01), double?.of(2d));
		assertEquals(test.get(DATE_2012_01_01), double?.of(3d));
		assertEquals(test.dates().toArray(), new object[] {DATE_2011_01_01, DATE_2012_01_01});
		assertEquals(test.values().toArray(), new double[] {2d, 3d});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_map_null()
	  public virtual void test_of_map_null()
	  {

		LocalDateDoubleTimeSeries.builder().putAll((IDictionary<LocalDate, double>) null).build();
	  }

	  public virtual void test_of_map_empty()
	  {

		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().putAll(ImmutableMap.of()).build();
		assertEquals(series, empty());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_map_dateNull()
	  public virtual void test_of_map_dateNull()
	  {
		IDictionary<LocalDate, double> map = new Dictionary<LocalDate, double>();
		map[DATE_2011_01_01] = 2d;
		map[null] = 3d;

		LocalDateDoubleTimeSeries.builder().putAll(map).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_map_valueNull()
	  public virtual void test_of_map_valueNull()
	  {
		IDictionary<LocalDate, double> map = new Dictionary<LocalDate, double>();
		map[DATE_2011_01_01] = 2d;
		map[DATE_2012_01_01] = null;

		LocalDateDoubleTimeSeries.builder().putAll(map).build();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_collection()
	  {
		ICollection<LocalDateDoublePoint> points = Arrays.asList(LocalDateDoublePoint.of(DATE_2011_01_01, 2d), LocalDateDoublePoint.of(DATE_2012_01_01, 3d));

		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(points.stream()).build();
		assertEquals(test.Empty, false);
		assertEquals(test.size(), 2);
		assertEquals(test.containsDate(DATE_2010_01_01), false);
		assertEquals(test.containsDate(DATE_2011_01_01), true);
		assertEquals(test.containsDate(DATE_2012_01_01), true);
		assertEquals(test.get(DATE_2010_01_01), double?.empty());
		assertEquals(test.get(DATE_2011_01_01), double?.of(2d));
		assertEquals(test.get(DATE_2012_01_01), double?.of(3d));
		assertEquals(test.dates().toArray(), new object[] {DATE_2011_01_01, DATE_2012_01_01});
		assertEquals(test.values().toArray(), new double[] {2d, 3d});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collection_collectionNull()
	  public virtual void test_of_collection_collectionNull()
	  {

		LocalDateDoubleTimeSeries.builder().putAll((IList<LocalDateDoublePoint>) null).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_of_collection_collectionWithNull()
	  public virtual void test_of_collection_collectionWithNull()
	  {
		ICollection<LocalDateDoublePoint> points = Arrays.asList(LocalDateDoublePoint.of(DATE_2011_01_01, 2d), null);

		LocalDateDoubleTimeSeries.builder().putAll(points.stream()).build();
	  }

	  public virtual void test_of_collection_empty()
	  {
		ICollection<LocalDateDoublePoint> points = ImmutableList.of();

		assertEquals(LocalDateDoubleTimeSeries.builder().putAll(points.stream()).build(), empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_immutableViaBeanBuilder()
	  {
		LocalDate startDate = DATE_2010_01_01;
		double[] values = new double[] {6, 5, 4};
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends DenseLocalDateDoubleTimeSeries> builder = DenseLocalDateDoubleTimeSeries.meta().builder();
		BeanBuilder<DenseLocalDateDoubleTimeSeries> builder = DenseLocalDateDoubleTimeSeries.meta().builder();
		builder.set("startDate", startDate);
		builder.set("points", values);
		builder.set("dateCalculation", INCLUDE_WEEKENDS);
		DenseLocalDateDoubleTimeSeries test = builder.build();
		values[0] = -1;

//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		LocalDateDoublePoint[] points = test.ToArray(LocalDateDoublePoint[]::new);
		assertEquals(points[0], LocalDateDoublePoint.of(DATE_2010_01_01, 6d));
		assertEquals(points[1], LocalDateDoublePoint.of(DATE_2010_01_02, 5d));
		assertEquals(points[2], LocalDateDoublePoint.of(DATE_2010_01_03, 4d));
	  }

	  public virtual void test_immutableValuesViaBeanGet()
	  {

		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		double[] array = (double[])((Bean) test).property("points").get();
		array[0] = -1;
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		LocalDateDoublePoint[] points = test.ToArray(LocalDateDoublePoint[]::new);
		assertEquals(points[0], LocalDateDoublePoint.of(DATE_2015_01_05, 10d));
		assertEquals(points[1], LocalDateDoublePoint.of(DATE_2015_01_06, 11d));
		assertEquals(points[2], LocalDateDoublePoint.of(DATE_2015_01_07, 12d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_earliestLatest()
	  {

		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
		assertEquals(test.EarliestDate, DATE_2010_01_01);
		assertEquals(test.EarliestValue, 10d, TOLERANCE);
		assertEquals(test.LatestDate, DATE_2012_01_01);
		assertEquals(test.LatestValue, 12d, TOLERANCE);
	  }

	  public virtual void test_earliestLatest_whenEmpty()
	  {
		LocalDateDoubleTimeSeries test = empty();
		assertThrows(test.getEarliestDate, typeof(NoSuchElementException));
		assertThrows(test.getEarliestValue, typeof(NoSuchElementException));
		assertThrows(test.getLatestDate, typeof(NoSuchElementException));
		assertThrows(test.getLatestValue, typeof(NoSuchElementException));
	  }

	  public virtual void test_earliest_with_subseries()
	  {

		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().put(DATE_2015_01_03, 3d).put(DATE_2015_01_05, 5d).put(DATE_2015_01_06, 6d).put(DATE_2015_01_07, 7d).put(DATE_2015_01_08, 8d).put(DATE_2015_01_09, 9d).put(DATE_2015_01_11, 11d).build();

		LocalDateDoubleTimeSeries subSeries = series.subSeries(DATE_2015_01_04, DATE_2015_01_11);
		assertEquals(subSeries.EarliestDate, DATE_2015_01_05);
		assertEquals(subSeries.EarliestValue, 5d);
		assertEquals(subSeries.LatestDate, DATE_2015_01_09);
		assertEquals(subSeries.LatestValue, 9d);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "subSeries") public static Object[][] data_subSeries()
	  public static object[][] data_subSeries()
	  {
		return new object[][]
		{
			new object[] {DATE_2011_01_01, DATE_2011_01_01, new int[] {}},
			new object[] {date(2006, 1, 1), date(2009, 1, 1), new int[] {}},
			new object[]
			{
				DATE_2015_01_06, DATE_2015_01_07, new int[] {1}
			},
			new object[]
			{
				DATE_2015_01_06, DATE_2015_01_08, new int[] {1, 2}
			},
			new object[]
			{
				DATE_2015_01_05, DATE_2015_01_09, new int[] {0, 1, 2, 3}
			},
			new object[]
			{
				date(2014, 12, 31), date(2015, 2, 1), new int[] {0, 1, 2, 3, 4}
			}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "subSeries") public void test_subSeries(java.time.LocalDate start, java.time.LocalDate end, int[] expected)
	  public virtual void test_subSeries(LocalDate start, LocalDate end, int[] expected)
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();

		LocalDateDoubleTimeSeries test = @base.subSeries(start, end);

		assertEquals(test.size(), expected.Length);
		for (int i = 0; i < DATES_2015_1_WEEK.size(); i++)
		{
		  if (Arrays.binarySearch(expected, i) >= 0)
		  {
			assertEquals(test.get(DATES_2015_1_WEEK.get(i)), double?.of(VALUES_1_WEEK.get(i)));
		  }
		  else
		  {
			assertEquals(test.get(DATES_2015_1_WEEK.get(i)), double?.empty());
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "subSeries") public void test_subSeries_emptySeries(java.time.LocalDate start, java.time.LocalDate end, int[] expected)
	  public virtual void test_subSeries_emptySeries(LocalDate start, LocalDate end, int[] expected)
	  {
		LocalDateDoubleTimeSeries test = empty().subSeries(start, end);
		assertEquals(test.size(), 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_subSeries_startAfterEnd()
	  public virtual void test_subSeries_startAfterEnd()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		@base.subSeries(date(2011, 1, 2), DATE_2011_01_01);
	  }

	  public virtual void test_subSeries_picks_valid_dates()
	  {

		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().put(DATE_2015_01_02, 10).put(DATE_2015_01_05, 11).put(DATE_2015_01_06, 12).put(DATE_2015_01_07, 13).put(DATE_2015_01_08, 14).put(DATE_2015_01_09, 15).put(DATE_2015_01_12, 16).build();

		// Pick using weekend dates
		LocalDateDoubleTimeSeries subSeries = series.subSeries(DATE_2015_01_04, date(2015, 1, 10));

		assertEquals(subSeries.size(), 5);
		assertEquals(subSeries.get(DATE_2015_01_02), double?.empty());
		assertEquals(subSeries.get(DATE_2015_01_04), double?.empty());
		assertEquals(subSeries.get(DATE_2015_01_05), double?.of(11));
		assertEquals(subSeries.get(DATE_2015_01_06), double?.of(12));
		assertEquals(subSeries.get(DATE_2015_01_07), double?.of(13));
		assertEquals(subSeries.get(DATE_2015_01_08), double?.of(14));
		assertEquals(subSeries.get(DATE_2015_01_09), double?.of(15));
		assertEquals(subSeries.get(DATE_2015_01_12), double?.empty());
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "headSeries") public static Object[][] data_headSeries()
	  public static object[][] data_headSeries()
	  {
		return new object[][]
		{
			new object[] {0, new int[] {}},
			new object[]
			{
				1, new int[] {0}
			},
			new object[]
			{
				2, new int[] {0, 1}
			},
			new object[]
			{
				3, new int[] {0, 1, 2}
			},
			new object[]
			{
				4, new int[] {0, 1, 2, 3}
			},
			new object[]
			{
				5, new int[] {0, 1, 2, 3, 4}
			},
			new object[]
			{
				6, new int[] {0, 1, 2, 3, 4}
			}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "headSeries") public void test_headSeries(int count, int[] expected)
	  public virtual void test_headSeries(int count, int[] expected)
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		LocalDateDoubleTimeSeries test = @base.headSeries(count);
		assertEquals(test.size(), expected.Length);
		for (int i = 0; i < DATES_2015_1_WEEK.size(); i++)
		{
		  if (Arrays.binarySearch(expected, i) >= 0)
		  {
			assertEquals(test.get(DATES_2015_1_WEEK.get(i)), double?.of(VALUES_1_WEEK.get(i)));
		  }
		  else
		  {
			assertEquals(test.get(DATES_2015_1_WEEK.get(i)), double?.empty());
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "headSeries") public void test_headSeries_emptySeries(int count, int[] expected)
	  public virtual void test_headSeries_emptySeries(int count, int[] expected)
	  {
		LocalDateDoubleTimeSeries test = empty().headSeries(count);
		assertEquals(test.size(), 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_headSeries_negative()
	  public virtual void test_headSeries_negative()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		@base.headSeries(-1);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "tailSeries") public static Object[][] data_tailSeries()
	  public static object[][] data_tailSeries()
	  {
		return new object[][]
		{
			new object[] {0, new int[] {}},
			new object[]
			{
				1, new int[] {4}
			},
			new object[]
			{
				2, new int[] {3, 4}
			},
			new object[]
			{
				3, new int[] {2, 3, 4}
			},
			new object[]
			{
				4, new int[] {1, 2, 3, 4}
			},
			new object[]
			{
				5, new int[] {0, 1, 2, 3, 4}
			},
			new object[]
			{
				6, new int[] {0, 1, 2, 3, 4}
			}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "tailSeries") public void test_tailSeries(int count, int[] expected)
	  public virtual void test_tailSeries(int count, int[] expected)
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		LocalDateDoubleTimeSeries test = @base.tailSeries(count);
		assertEquals(test.size(), expected.Length);
		for (int i = 0; i < DATES_2015_1_WEEK.size(); i++)
		{
		  if (Arrays.binarySearch(expected, i) >= 0)
		  {
			assertEquals(test.get(DATES_2015_1_WEEK.get(i)), double?.of(VALUES_1_WEEK.get(i)));
		  }
		  else
		  {
			assertEquals(test.get(DATES_2015_1_WEEK.get(i)), double?.empty());
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "tailSeries") public void test_tailSeries_emptySeries(int count, int[] expected)
	  public virtual void test_tailSeries_emptySeries(int count, int[] expected)
	  {
		LocalDateDoubleTimeSeries test = empty().tailSeries(count);
		assertEquals(test.size(), 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_tailSeries_negative()
	  public virtual void test_tailSeries_negative()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		@base.tailSeries(-1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_stream()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
		object[] test = @base.ToArray();
		assertEquals(test[0], LocalDateDoublePoint.of(DATE_2010_01_01, 10));
		assertEquals(test[1], LocalDateDoublePoint.of(DATE_2011_01_01, 11));
		assertEquals(test[2], LocalDateDoublePoint.of(DATE_2012_01_01, 12));
	  }

	  public virtual void test_stream_withCollector()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		LocalDateDoubleTimeSeries test = @base.Select(point => point.withValue(1.5d)).collect(LocalDateDoubleTimeSeries.collector());
		assertEquals(test.size(), 3);
		assertEquals(test.get(DATE_2010_01_01), double?.of(1.5));
		assertEquals(test.get(DATE_2011_01_01), double?.of(1.5));
		assertEquals(test.get(DATE_2012_01_01), double?.of(1.5));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_dateStream()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		LocalDate[] test = @base.dates().toArray(LocalDate[]::new);
		assertEquals(test[0], DATE_2010_01_01);
		assertEquals(test[1], DATE_2011_01_01);
		assertEquals(test[2], DATE_2012_01_01);
	  }

	  public virtual void test_valueStream()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
		double[] test = @base.values().toArray();
		assertEquals(test[0], 10, TOLERANCE);
		assertEquals(test[1], 11, TOLERANCE);
		assertEquals(test[2], 12, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forEach()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		AtomicInteger counter = new AtomicInteger();
		@base.forEach((date, value) => counter.addAndGet((int) value));
		assertEquals(counter.get(), 10 + 11 + 12 + 13 + 14);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_intersection_withNoMatchingElements()
	  {

		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();

		IList<LocalDate> dates2 = dates(DATE_2010_06_01, DATE_2011_06_01, DATE_2012_06_01, DATE_2013_06_01, DATE_2014_06_01);

		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, VALUES_1_WEEK).build();

		LocalDateDoubleTimeSeries test = series1.intersection(series2, double?.sum);
		assertEquals(test, LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void test_intersection_withSomeMatchingElements()
	  {

		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();

		IDictionary<LocalDate, double> updates = ImmutableMap.of(DATE_2015_01_02, 1.0, DATE_2015_01_05, 1.1, DATE_2015_01_08, 1.2, DATE_2015_01_09, 1.3, DATE_2015_01_12, 1.4);

		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(updates).build();

		LocalDateDoubleTimeSeries test = series1.intersection(series2, double?.sum);
		assertEquals(test.size(), 3);
		assertEquals(test.get(DATE_2015_01_05), double?.of(11.1));
		assertEquals(test.get(DATE_2015_01_08), double?.of(14.2));
		assertEquals(test.get(DATE_2015_01_09), double?.of(15.3));
	  }

	  public virtual void test_intersection_withSomeMatchingElements2()
	  {
		IList<LocalDate> dates1 = dates(DATE_2010_01_01, DATE_2011_01_01, DATE_2012_01_01, DATE_2014_01_01, DATE_2015_06_01);
		IList<double> values1 = values(10, 11, 12, 13, 14);

		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(dates1, values1).build();

		IList<LocalDate> dates2 = dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_01_01, DATE_2014_01_01);
		IList<double> values2 = values(1.0, 1.1, 1.2, 1.3, 1.4);

		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, values2).build();

		LocalDateDoubleTimeSeries test = series1.intersection(series2, double?.sum);
		assertEquals(test.size(), 3);
		assertEquals(test.get(DATE_2010_01_01), double?.of(11.0));
		assertEquals(test.get(DATE_2012_01_01), double?.of(13.2));
		assertEquals(test.get(DATE_2014_01_01), double?.of(14.4));
	  }

	  public virtual void test_intersection_withAllMatchingElements()
	  {
		IList<LocalDate> dates1 = DATES_2015_1_WEEK;
		IList<double> values1 = values(10, 11, 12, 13, 14);

		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(dates1, values1).build();
		IList<LocalDate> dates2 = DATES_2015_1_WEEK;
		IList<double> values2 = values(1.0, 1.1, 1.2, 1.3, 1.4);

		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, values2).build();

		LocalDateDoubleTimeSeries combined = series1.intersection(series2, double?.sum);
		assertEquals(combined.size(), 5);
		assertEquals(combined.EarliestDate, DATE_2015_01_05);
		assertEquals(combined.LatestDate, DATE_2015_01_09);
		assertEquals(combined.get(DATE_2015_01_05), double?.of(11.0));
		assertEquals(combined.get(DATE_2015_01_06), double?.of(12.1));
		assertEquals(combined.get(DATE_2015_01_07), double?.of(13.2));
		assertEquals(combined.get(DATE_2015_01_08), double?.of(14.3));
		assertEquals(combined.get(DATE_2015_01_09), double?.of(15.4));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_union_withMatchingElements()
	  {

		IList<LocalDate> dates1 = dates(DATE_2015_01_03, DATE_2015_01_05, DATE_2015_01_06);
		IList<LocalDate> dates2 = dates(DATE_2015_01_02, DATE_2015_01_03, DATE_2015_01_05, DATE_2015_01_08);
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(dates1, VALUES_10_12).build();
		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, VALUES_4_7).build();

		LocalDateDoubleTimeSeries test = series1.union(series2, double?.sum);
		assertEquals(test.size(), 5);
		assertEquals(test.EarliestDate, DATE_2015_01_02);
		assertEquals(test.LatestDate, DATE_2015_01_08);
		assertEquals(test.get(DATE_2015_01_02), double?.of(4d));
		assertEquals(test.get(DATE_2015_01_03), double?.of(10d + 5d));
		assertEquals(test.get(DATE_2015_01_05), double?.of(11d + 6d));
		assertEquals(test.get(DATE_2015_01_06), double?.of(12d));
		assertEquals(test.get(DATE_2015_01_08), double?.of(7d));
	  }

	  public virtual void test_union_withNoMatchingElements()
	  {

		IList<LocalDate> dates1 = dates(DATE_2015_01_03, DATE_2015_01_05, DATE_2015_01_06);
		IList<LocalDate> dates2 = dates(DATE_2015_01_02, DATE_2015_01_04, DATE_2015_01_08);
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(dates1, VALUES_10_12).build();
		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, VALUES_1_3).build();

		LocalDateDoubleTimeSeries test = series1.union(series2, double?.sum);
		assertEquals(test.size(), 6);
		assertEquals(test.EarliestDate, DATE_2015_01_02);
		assertEquals(test.LatestDate, DATE_2015_01_08);
		assertEquals(test.get(DATE_2015_01_02), double?.of(1d));
		assertEquals(test.get(DATE_2015_01_03), double?.of(10d));
		assertEquals(test.get(DATE_2015_01_04), double?.of(2d));
		assertEquals(test.get(DATE_2015_01_05), double?.of(11d));
		assertEquals(test.get(DATE_2015_01_06), double?.of(12d));
		assertEquals(test.get(DATE_2015_01_08), double?.of(3d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapValues_addConstantToSeries()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		LocalDateDoubleTimeSeries test = @base.mapValues(d => d + 5);
		IList<double> expectedValues = values(15, 16, 17, 18, 19);

		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, expectedValues).build());
	  }

	  public virtual void test_mapValues_multiplySeries()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();

		LocalDateDoubleTimeSeries test = @base.mapValues(d => d * 5);
		IList<double> expectedValues = values(50, 55, 60, 65, 70);

		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, expectedValues).build());
	  }

	  public virtual void test_mapValues_invertSeries()
	  {
		IList<double> values = DenseLocalDateDoubleTimeSeriesTest.values(1, 2, 4, 5, 8);

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, values).build();
		LocalDateDoubleTimeSeries test = @base.mapValues(d => 1 / d);
		IList<double> expectedValues = DenseLocalDateDoubleTimeSeriesTest.values(1, 0.5, 0.25, 0.2, 0.125);

		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, expectedValues).build());
	  }

	  public virtual void test_mapDates()
	  {
		IList<double> values = DenseLocalDateDoubleTimeSeriesTest.values(1, 2, 4, 5, 8);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, values).build();
		LocalDateDoubleTimeSeries test = @base.mapDates(date => date.plusYears(1));
		ImmutableList<LocalDate> expectedDates = ImmutableList.of(date(2016, 1, 5), date(2016, 1, 6), date(2016, 1, 7), date(2016, 1, 8), date(2016, 1, 9));
		LocalDateDoubleTimeSeries expected = LocalDateDoubleTimeSeries.builder().putAll(expectedDates, values).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_mapDates_notAscending()
	  {
		IList<double> values = DenseLocalDateDoubleTimeSeriesTest.values(1, 2, 4, 5, 8);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, values).build();
		assertThrowsIllegalArg(() => @base.mapDates(date => date(2016, 1, 6)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filter_byDate()
	  {
		IList<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(dates, VALUES_1_WEEK).build();
		LocalDateDoubleTimeSeries test = @base.filter((ld, v) => ld.MonthValue != 6);
		assertEquals(test.size(), 3);
		assertEquals(test.get(DATE_2010_01_01), double?.of(10d));
		assertEquals(test.get(DATE_2012_01_01), double?.of(12d));
		assertEquals(test.get(DATE_2014_01_01), double?.of(14d));
	  }

	  public virtual void test_filter_byValue()
	  {

		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2015_1_WEEK, VALUES_1_WEEK).build();
		LocalDateDoubleTimeSeries test = @base.filter((ld, v) => v % 2 == 1);
		assertEquals(test.size(), 2);
		assertEquals(test.get(DATE_2015_01_06), double?.of(11d));
		assertEquals(test.get(DATE_2015_01_08), double?.of(13d));
	  }

	  public virtual void test_filter_byDateAndValue()
	  {
		IList<LocalDate> dates = DenseLocalDateDoubleTimeSeriesTest.dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);

		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().putAll(dates, VALUES_1_WEEK).build();

		LocalDateDoubleTimeSeries test = series.filter((ld, v) => ld.Year >= 2012 && v % 2 == 0);
		assertEquals(test.size(), 2);
		assertEquals(test.get(DATE_2012_01_01), double?.of(12d));
		assertEquals(test.get(DATE_2014_01_01), double?.of(14d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals_similarSeriesAreEqual()
	  {
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.of(DATE_2014_01_01, 1d);

		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates(DATE_2014_01_01), values(1d)).build();
		assertEquals(series1.size(), 1);
		assertEquals(series1, series2);
		assertEquals(series1, series1);
		assertEquals(series1.GetHashCode(), series1.GetHashCode());
	  }

	  public virtual void test_equals_notEqual()
	  {
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.of(DATE_2014_01_01, 1d);
		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.of(DATE_2013_06_01, 1d);
		LocalDateDoubleTimeSeries series3 = LocalDateDoubleTimeSeries.of(DATE_2014_01_01, 3d);
		assertNotEquals(series1, series2);
		assertNotEquals(series1, series3);
	  }

	  public virtual void test_equals_bad()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.of(DATE_2014_01_01, 1d);
		assertEquals(test.Equals(ANOTHER_TYPE), false);
		assertEquals(test.Equals(null), false);
	  }

	  public virtual void checkOffsetsIncludeWeekends()
	  {

		IDictionary<LocalDate, double> map = ImmutableMap.builder<LocalDate, double>().put(dt(2014, 12, 26), 14d).put(dt(2014, 12, 29), 13d).put(dt(2014, 12, 30), 12d).put(dt(2014, 12, 31), 11d).put(dt(2015, 1, 2), 11d).put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(map).build();
		assertThat(ts.get(dt(2014, 12, 26))).hasValue(14d);
		assertThat(ts.get(dt(2014, 12, 27))).Empty;
		assertThat(ts.get(dt(2014, 12, 28))).Empty;
		assertThat(ts.get(dt(2014, 12, 29))).hasValue(13d);
		assertThat(ts.get(dt(2014, 12, 30))).hasValue(12d);
		assertThat(ts.get(dt(2014, 12, 31))).hasValue(11d);
		assertThat(ts.get(dt(2015, 1, 1))).Empty;
		assertThat(ts.get(dt(2015, 1, 2))).hasValue(11d);
		assertThat(ts.get(dt(2015, 1, 3))).Empty;
		assertThat(ts.get(dt(2015, 1, 4))).Empty;
		assertThat(ts.get(dt(2015, 1, 5))).hasValue(12d);
		assertThat(ts.get(dt(2015, 1, 6))).hasValue(13d);
		assertThat(ts.get(dt(2015, 1, 7))).hasValue(14d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_coverage()
	  {
		TestHelper.coverImmutableBean((ImmutableBean) DenseLocalDateDoubleTimeSeries.of(DATE_2015_01_05, DATE_2015_01_05, Stream.of(LocalDateDoublePoint.of(DATE_2015_01_05, 1d)), SKIP_WEEKENDS));
	  }

	  //-------------------------------------------------------------------------
	  private static LocalDate date(int year, int month, int day)
	  {
		return LocalDate.of(year, month, day);
	  }

	  private static ImmutableList<LocalDate> dates(params LocalDate[] dates)
	  {
		return ImmutableList.copyOf(dates);
	  }

	  private static ImmutableList<double> values(params double[] values)
	  {
		return ImmutableList.copyOf(Doubles.asList(values));
	  }

	  public virtual void checkOffsetsSkipWeekends()
	  {

		IDictionary<LocalDate, double> map = ImmutableMap.builder<LocalDate, double>().put(dt(2014, 12, 26), 14d).put(dt(2014, 12, 29), 13d).put(dt(2014, 12, 30), 12d).put(dt(2014, 12, 31), 11d).put(dt(2015, 1, 2), 11d).put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(map).build();
		assertThat(ts.get(dt(2014, 12, 26))).hasValue(14d);
		assertThat(ts.get(dt(2014, 12, 29))).hasValue(13d);
		assertThat(ts.get(dt(2014, 12, 30))).hasValue(12d);
		assertThat(ts.get(dt(2014, 12, 31))).hasValue(11d);
		assertThat(ts.get(dt(2015, 1, 1))).Empty;
		assertThat(ts.get(dt(2015, 1, 2))).hasValue(11d);
		assertThat(ts.get(dt(2015, 1, 3))).Empty;
		assertThat(ts.get(dt(2015, 1, 4))).Empty;
		assertThat(ts.get(dt(2015, 1, 5))).hasValue(12d);
		assertThat(ts.get(dt(2015, 1, 6))).hasValue(13d);
		assertThat(ts.get(dt(2015, 1, 7))).hasValue(14d);
	  }

	  public virtual void underOneWeekNoWeekend()
	  {

		IDictionary<LocalDate, double> map = ImmutableMap.builder<LocalDate, double>().put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).put(dt(2015, 1, 8), 15d).put(dt(2015, 1, 9), 16d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(map).build();
		assertThat(ts.get(dt(2015, 1, 5))).hasValue(12d);
		assertThat(ts.get(dt(2015, 1, 9))).hasValue(16d);
	  }

	  public virtual void underOneWeekWithWeekend()
	  {

		IDictionary<LocalDate, double> map = ImmutableMap.builder<LocalDate, double>().put(dt(2015, 1, 1), 10d).put(dt(2015, 1, 2), 11d).put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).put(dt(2015, 1, 8), 15d).put(dt(2015, 1, 9), 16d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(map).build();
		assertThat(ts.get(dt(2015, 1, 1))).hasValue(10d);
		assertThat(ts.get(dt(2015, 1, 2))).hasValue(11d);
		assertThat(ts.get(dt(2015, 1, 5))).hasValue(12d);
		assertThat(ts.get(dt(2015, 1, 6))).hasValue(13d);
		assertThat(ts.get(dt(2015, 1, 7))).hasValue(14d);
		assertThat(ts.get(dt(2015, 1, 8))).hasValue(15d);
		assertThat(ts.get(dt(2015, 1, 9))).hasValue(16d);
	  }

	  public virtual void roundTrip()
	  {
		IDictionary<LocalDate, double> @in = ImmutableMap.builder<LocalDate, double>().put(dt(2015, 1, 1), 10d).put(dt(2015, 1, 2), 11d).put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).put(dt(2015, 1, 8), 15d).put(dt(2015, 1, 9), 16d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(@in).build();

//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IDictionary<LocalDate, double> @out = ts.collect(Guavate.toImmutableMap(LocalDateDoublePoint::getDate, LocalDateDoublePoint::getValue));
		assertThat(@out).isEqualTo(@in);
	  }

	  public virtual void partitionEmptySeries()
	  {

		Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partitioned = LocalDateDoubleTimeSeries.empty().partition((ld, d) => ld.Year == 2015);

		assertThat(partitioned.First).isEqualTo(LocalDateDoubleTimeSeries.empty());
		assertThat(partitioned.Second).isEqualTo(LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void partitionSeries()
	  {

		IDictionary<LocalDate, double> @in = ImmutableMap.builder<LocalDate, double>().put(dt(2015, 1, 1), 10d).put(dt(2015, 1, 2), 11d).put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).put(dt(2015, 1, 8), 15d).put(dt(2015, 1, 9), 16d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(@in).build();

		Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partitioned = ts.partition((ld, d) => ld.DayOfMonth % 2 == 0);

		LocalDateDoubleTimeSeries even = partitioned.First;
		LocalDateDoubleTimeSeries odd = partitioned.Second;

		assertThat(even.size()).isEqualTo(3);
		assertThat(even.get(dt(2015, 1, 2))).hasValue(11d);
		assertThat(even.get(dt(2015, 1, 6))).hasValue(13d);
		assertThat(even.get(dt(2015, 1, 8))).hasValue(15d);

		assertThat(odd.size()).isEqualTo(4);
		assertThat(odd.get(dt(2015, 1, 1))).hasValue(10d);
		assertThat(odd.get(dt(2015, 1, 5))).hasValue(12d);
		assertThat(odd.get(dt(2015, 1, 7))).hasValue(14d);
		assertThat(odd.get(dt(2015, 1, 9))).hasValue(16d);
	  }

	  public virtual void partitionByValueEmptySeries()
	  {

		Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partitioned = LocalDateDoubleTimeSeries.empty().partitionByValue(d => d > 10);

		assertThat(partitioned.First).isEqualTo(LocalDateDoubleTimeSeries.empty());
		assertThat(partitioned.Second).isEqualTo(LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void partitionByValueSeries()
	  {

		IDictionary<LocalDate, double> @in = ImmutableMap.builder<LocalDate, double>().put(dt(2015, 1, 1), 10d).put(dt(2015, 1, 2), 11d).put(dt(2015, 1, 5), 12d).put(dt(2015, 1, 6), 13d).put(dt(2015, 1, 7), 14d).put(dt(2015, 1, 8), 15d).put(dt(2015, 1, 9), 16d).build();

		LocalDateDoubleTimeSeries ts = LocalDateDoubleTimeSeries.builder().putAll(@in).build();

		Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partitioned = ts.partitionByValue(d => d < 12 || d > 15);

		LocalDateDoubleTimeSeries extreme = partitioned.First;
		LocalDateDoubleTimeSeries mid = partitioned.Second;

		assertThat(extreme.size()).isEqualTo(3);
		assertThat(extreme.get(dt(2015, 1, 1))).hasValue(10d);
		assertThat(extreme.get(dt(2015, 1, 2))).hasValue(11d);
		assertThat(extreme.get(dt(2015, 1, 9))).hasValue(16d);

		assertThat(mid.size()).isEqualTo(4);
		assertThat(mid.get(dt(2015, 1, 5))).hasValue(12d);
		assertThat(mid.get(dt(2015, 1, 6))).hasValue(13d);
		assertThat(mid.get(dt(2015, 1, 7))).hasValue(14d);
		assertThat(mid.get(dt(2015, 1, 8))).hasValue(15d);
	  }

	  private LocalDate dt(int yr, int mth, int day)
	  {
		return LocalDate.of(yr, mth, day);
	  }
	}

}
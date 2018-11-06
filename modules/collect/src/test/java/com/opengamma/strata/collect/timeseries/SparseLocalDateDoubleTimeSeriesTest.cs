using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotEquals;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Doubles = com.google.common.primitives.Doubles;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Test LocalDateDoubleTimeSeries.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SparseLocalDateDoubleTimeSeriesTest
	public class SparseLocalDateDoubleTimeSeriesTest
	{

	  private static readonly LocalDate DATE_2015_06_01 = date(2015, 6, 1);
	  private static readonly LocalDate DATE_2014_06_01 = date(2014, 6, 1);
	  private static readonly LocalDate DATE_2012_06_01 = date(2012, 6, 1);
	  private static readonly LocalDate DATE_2010_06_01 = date(2010, 6, 1);
	  private static readonly LocalDate DATE_2011_06_01 = date(2011, 6, 1);
	  private static readonly LocalDate DATE_2013_06_01 = date(2013, 6, 1);
	  private static readonly LocalDate DATE_2010_01_01 = date(2010, 1, 1);
	  private static readonly LocalDate DATE_2011_01_01 = date(2011, 1, 1);
	  private static readonly LocalDate DATE_2012_01_01 = date(2012, 1, 1);
	  private static readonly LocalDate DATE_2013_01_01 = date(2013, 1, 1);
	  private static readonly LocalDate DATE_2014_01_01 = date(2014, 1, 1);
	  private static readonly ImmutableList<LocalDate> DATES_2010_14 = dates(DATE_2010_01_01, DATE_2011_01_01, DATE_2012_01_01, DATE_2013_01_01, DATE_2014_01_01);
	  private static readonly ImmutableList<double> VALUES_10_14 = values(10, 11, 12, 13, 14);
	  private static readonly ImmutableList<LocalDate> DATES_2010_12 = dates(DATE_2010_01_01, DATE_2011_01_01, DATE_2012_01_01);
	  private static readonly ImmutableList<double> VALUES_10_12 = values(10, 11, 12);
	  private const double TOLERANCE = 0.00001d;
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_emptySeries()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.empty();
		assertEquals(test.Empty, true);
		assertEquals(test.size(), 0);
		assertEquals(test.containsDate(DATE_2010_01_01), false);
		assertEquals(test.containsDate(DATE_2011_01_01), false);
		assertEquals(test.containsDate(DATE_2012_01_01), false);
		assertEquals(test.get(DATE_2010_01_01), double?.empty());
		assertEquals(test.get(DATE_2011_01_01), double?.empty());
		assertEquals(test.get(DATE_2012_01_01), double?.empty());
		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(dates(), values()).build());
		assertEquals(test.dates().count(), 0);
		assertEquals(test.values().count(), 0);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_singleton()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.of(DATE_2011_01_01, 2d);
		assertEquals(test.Empty, false);
		assertEquals(test.size(), 1);
		assertEquals(test.containsDate(DATE_2010_01_01), false);
		assertEquals(test.containsDate(DATE_2011_01_01), true);
		assertEquals(test.containsDate(DATE_2012_01_01), false);
		assertEquals(test.get(DATE_2010_01_01), double?.empty());
		assertEquals(test.get(DATE_2011_01_01), double?.of(2d));
		assertEquals(test.get(DATE_2012_01_01), double?.empty());
		assertEquals(test.dates().toArray(), new object[] {DATE_2011_01_01});
		assertEquals(test.values().toArray(), new double[] {2d});
	  }

	  public virtual void test_of_singleton_nullDateDisallowed()
	  {
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.of(null, 1d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_collectionCollection()
	  {
		ICollection<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01, DATE_2012_01_01);
		ICollection<double> values = SparseLocalDateDoubleTimeSeriesTest.values(2d, 3d);
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

	  public virtual void test_of_collectionCollection_dateCollectionNull()
	  {
		ICollection<double> values = SparseLocalDateDoubleTimeSeriesTest.values(2d, 3d);
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll((ICollection<LocalDate>) null, values).build());
	  }

	  public virtual void test_of_collectionCollection_valueCollectionNull()
	  {
		ICollection<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01, DATE_2012_01_01);
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(dates, (ICollection<double>) null).build());
	  }

	  public virtual void test_of_collectionCollection_dateCollectionWithNull()
	  {
		ICollection<LocalDate> dates = Arrays.asList(DATE_2011_01_01, null);
		ICollection<double> values = SparseLocalDateDoubleTimeSeriesTest.values(2d, 3d);
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(dates, values).build());
	  }

	  public virtual void test_of_collectionCollection_valueCollectionWithNull()
	  {
		ICollection<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01, DATE_2012_01_01);
		ICollection<double> values = Arrays.asList(2d, null);
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(dates, values).build());
	  }

	  public virtual void test_of_collectionCollection_collectionsOfDifferentSize()
	  {
		ICollection<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2011_01_01);
		ICollection<double> values = SparseLocalDateDoubleTimeSeriesTest.values(2d, 3d);
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(dates, values).build());
	  }

	  public virtual void test_of_collectionCollection_sparse_differentSize()
	  {
		IList<LocalDate> dates = ImmutableList.of(DATE_2011_01_01, DATE_2011_06_01);
		IList<double> values = ImmutableList.of(1d);
		assertThrowsIllegalArg(() => SparseLocalDateDoubleTimeSeries.of(dates, values));
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

	  public virtual void test_of_map_null()
	  {
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll((IDictionary<LocalDate, double>) null).build());
	  }

	  public virtual void test_of_map_dateNull()
	  {
		IDictionary<LocalDate, double> map = new Dictionary<LocalDate, double>();
		map[DATE_2011_01_01] = 2d;
		map[null] = 3d;
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(map).build());
	  }

	  public virtual void test_of_map_valueNull()
	  {
		IDictionary<LocalDate, double> map = new Dictionary<LocalDate, double>();
		map[DATE_2011_01_01] = 2d;
		map[DATE_2012_01_01] = null;
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(map).build());
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

	  public virtual void test_of_collection_collectionNull()
	  {
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(((IList<LocalDateDoublePoint>) null)).build());
	  }

	  public virtual void test_of_collection_collectionWithNull()
	  {
		ICollection<LocalDateDoublePoint> points = Arrays.asList(LocalDateDoublePoint.of(DATE_2011_01_01, 2d), null);
		assertThrowsIllegalArg(() => LocalDateDoubleTimeSeries.builder().putAll(points.stream()).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_immutableViaBeanBuilder()
	  {
		LocalDate[] dates = new LocalDate[] {DATE_2010_01_01, DATE_2011_01_01, DATE_2012_01_01};
		double[] values = new double[] {6, 5, 4};
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends LocalDateDoubleTimeSeries> builder = SparseLocalDateDoubleTimeSeries.meta().builder();
		BeanBuilder<LocalDateDoubleTimeSeries> builder = SparseLocalDateDoubleTimeSeries.meta().builder();
		builder.set("dates", dates);
		builder.set("values", values);
		LocalDateDoubleTimeSeries test = builder.build();
		dates[0] = DATE_2012_01_01;
		values[0] = -1;
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		LocalDateDoublePoint[] points = test.ToArray(LocalDateDoublePoint[]::new);
		assertEquals(points[0], LocalDateDoublePoint.of(DATE_2010_01_01, 6d));
		assertEquals(points[1], LocalDateDoublePoint.of(DATE_2011_01_01, 5d));
		assertEquals(points[2], LocalDateDoublePoint.of(DATE_2012_01_01, 4d));
	  }

	  public virtual void test_immutableDatesViaBeanGet()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
		LocalDate[] array = (LocalDate[])((Bean) test).property("dates").get();
		array[0] = DATE_2012_01_01;
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		LocalDateDoublePoint[] points = test.ToArray(LocalDateDoublePoint[]::new);
		assertEquals(points[0], LocalDateDoublePoint.of(DATE_2010_01_01, 10d));
		assertEquals(points[1], LocalDateDoublePoint.of(DATE_2011_01_01, 11d));
		assertEquals(points[2], LocalDateDoublePoint.of(DATE_2012_01_01, 12d));
	  }

	  public virtual void test_immutableValuesViaBeanGet()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, VALUES_10_12).build();
		double[] array = (double[])((Bean) test).property("values").get();
		array[0] = -1;
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		LocalDateDoublePoint[] points = test.ToArray(LocalDateDoublePoint[]::new);
		assertEquals(points[0], LocalDateDoublePoint.of(DATE_2010_01_01, 10d));
		assertEquals(points[1], LocalDateDoublePoint.of(DATE_2011_01_01, 11d));
		assertEquals(points[2], LocalDateDoublePoint.of(DATE_2012_01_01, 12d));
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
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.empty();
		TestHelper.assertThrows(() => test.EarliestDate, typeof(NoSuchElementException));
		TestHelper.assertThrows(() => test.EarliestValue, typeof(NoSuchElementException));
		TestHelper.assertThrows(() => test.LatestDate, typeof(NoSuchElementException));
		TestHelper.assertThrows(() => test.LatestValue, typeof(NoSuchElementException));
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
				DATE_2011_01_01, date(2011, 1, 2), new int[] {1}
			},
			new object[]
			{
				DATE_2011_01_01, DATE_2013_01_01, new int[] {1, 2}
			},
			new object[]
			{
				DATE_2011_01_01, date(2013, 1, 2), new int[] {1, 2, 3}
			},
			new object[]
			{
				date(2010, 12, 31), date(2013, 1, 2), new int[] {1, 2, 3}
			},
			new object[]
			{
				date(2011, 1, 2), date(2013, 1, 2), new int[] {2, 3}
			}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "subSeries") public void test_subSeries(java.time.LocalDate start, java.time.LocalDate end, int[] expected)
	  public virtual void test_subSeries(LocalDate start, LocalDate end, int[] expected)
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		LocalDateDoubleTimeSeries test = @base.subSeries(start, end);
		assertEquals(test.size(), expected.Length);
		for (int i = 0; i < DATES_2010_14.size(); i++)
		{
		  if (Arrays.binarySearch(expected, i) >= 0)
		  {
			assertEquals(test.get(DATES_2010_14.get(i)), double?.of(VALUES_10_14.get(i)));
		  }
		  else
		  {
			assertEquals(test.get(DATES_2010_14.get(i)), double?.empty());
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "subSeries") public void test_subSeries_emptySeries(java.time.LocalDate start, java.time.LocalDate end, int[] expected)
	  public virtual void test_subSeries_emptySeries(LocalDate start, LocalDate end, int[] expected)
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.empty().subSeries(start, end);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_subSeries_startAfterEnd()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		assertThrowsIllegalArg(() => @base.subSeries(date(2011, 1, 2), DATE_2011_01_01));
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
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		LocalDateDoubleTimeSeries test = @base.headSeries(count);
		assertEquals(test.size(), expected.Length);
		for (int i = 0; i < DATES_2010_14.size(); i++)
		{
		  if (Arrays.binarySearch(expected, i) >= 0)
		  {
			assertEquals(test.get(DATES_2010_14.get(i)), double?.of(VALUES_10_14.get(i)));
		  }
		  else
		  {
			assertEquals(test.get(DATES_2010_14.get(i)), double?.empty());
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "headSeries") public void test_headSeries_emptySeries(int count, int[] expected)
	  public virtual void test_headSeries_emptySeries(int count, int[] expected)
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.empty().headSeries(count);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_headSeries_negative()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		assertThrowsIllegalArg(() => @base.headSeries(-1));
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
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		LocalDateDoubleTimeSeries test = @base.tailSeries(count);
		assertEquals(test.size(), expected.Length);
		for (int i = 0; i < DATES_2010_14.size(); i++)
		{
		  if (Arrays.binarySearch(expected, i) >= 0)
		  {
			assertEquals(test.get(DATES_2010_14.get(i)), double?.of(VALUES_10_14.get(i)));
		  }
		  else
		  {
			assertEquals(test.get(DATES_2010_14.get(i)), double?.empty());
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "tailSeries") public void test_tailSeries_emptySeries(int count, int[] expected)
	  public virtual void test_tailSeries_emptySeries(int count, int[] expected)
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.empty().tailSeries(count);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_tailSeries_negative()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		assertThrowsIllegalArg(() => @base.tailSeries(-1));
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
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		AtomicInteger counter = new AtomicInteger();
		@base.forEach((date, value) => counter.addAndGet((int) value));
		assertEquals(counter.get(), 10 + 11 + 12 + 13 + 14);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combineWith_intersectionWithNoMatchingElements()
	  {
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		IList<LocalDate> dates2 = dates(DATE_2010_06_01, DATE_2011_06_01, DATE_2012_06_01, DATE_2013_06_01, DATE_2014_06_01);
		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, VALUES_10_14).build();

		LocalDateDoubleTimeSeries test = series1.intersection(series2, double?.sum);
		assertEquals(test, LocalDateDoubleTimeSeries.empty());
	  }

	  public virtual void test_combineWith_intersectionWithSomeMatchingElements()
	  {
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		IList<LocalDate> dates2 = dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);
		IList<double> values2 = values(1.0, 1.1, 1.2, 1.3, 1.4);
		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, values2).build();

		LocalDateDoubleTimeSeries test = series1.intersection(series2, double?.sum);
		assertEquals(test.size(), 3);
		assertEquals(test.get(DATE_2010_01_01), double?.of(11.0));
		assertEquals(test.get(DATE_2012_01_01), double?.of(13.2));
		assertEquals(test.get(DATE_2014_01_01), double?.of(15.4));
	  }

	  public virtual void test_combineWith_intersectionWithSomeMatchingElements2()
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

	  public virtual void test_combineWith_intersectionWithAllMatchingElements()
	  {
		IList<LocalDate> dates1 = DATES_2010_14;
		IList<double> values1 = values(10, 11, 12, 13, 14);
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().putAll(dates1, values1).build();
		IList<LocalDate> dates2 = DATES_2010_14;
		IList<double> values2 = values(1.0, 1.1, 1.2, 1.3, 1.4);
		LocalDateDoubleTimeSeries series2 = LocalDateDoubleTimeSeries.builder().putAll(dates2, values2).build();

		LocalDateDoubleTimeSeries combined = series1.intersection(series2, double?.sum);
		assertEquals(combined.size(), 5);
		assertEquals(combined.EarliestDate, DATE_2010_01_01);
		assertEquals(combined.LatestDate, DATE_2014_01_01);
		assertEquals(combined.get(DATE_2010_01_01), double?.of(11.0));
		assertEquals(combined.get(DATE_2011_01_01), double?.of(12.1));
		assertEquals(combined.get(DATE_2012_01_01), double?.of(13.2));
		assertEquals(combined.get(DATE_2013_01_01), double?.of(14.3));
		assertEquals(combined.get(DATE_2014_01_01), double?.of(15.4));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapValues_addConstantToSeries()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		LocalDateDoubleTimeSeries test = @base.mapValues(d => d + 5);
		IList<double> expectedValues = values(15, 16, 17, 18, 19);
		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, expectedValues).build());
	  }

	  public virtual void test_mapValues_multiplySeries()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();

		LocalDateDoubleTimeSeries test = @base.mapValues(d => d * 5);
		IList<double> expectedValues = values(50, 55, 60, 65, 70);
		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, expectedValues).build());
	  }

	  public virtual void test_mapValues_invertSeries()
	  {
		IList<double> values = SparseLocalDateDoubleTimeSeriesTest.values(1, 2, 4, 5, 8);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, values).build();
		LocalDateDoubleTimeSeries test = @base.mapValues(d => 1 / d);
		IList<double> expectedValues = SparseLocalDateDoubleTimeSeriesTest.values(1, 0.5, 0.25, 0.2, 0.125);
		assertEquals(test, LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, expectedValues).build());
	  }

	  public virtual void test_mapDates()
	  {
		IList<double> values = SparseLocalDateDoubleTimeSeriesTest.values(1, 2, 4, 5, 8);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, values).build();
		LocalDateDoubleTimeSeries test = @base.mapDates(date => date.plusYears(1));
		ImmutableList<LocalDate> expectedDates = ImmutableList.of(DATE_2011_01_01, DATE_2012_01_01, DATE_2013_01_01, DATE_2014_01_01, LocalDate.of(2015, 1, 1));
		LocalDateDoubleTimeSeries expected = LocalDateDoubleTimeSeries.builder().putAll(expectedDates, @base.values().boxed().collect(toList())).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_mapDates_notAscending()
	  {
		IList<double> values = SparseLocalDateDoubleTimeSeriesTest.values(1, 2, 4);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_12, values).build();
		assertThrowsIllegalArg(() => @base.mapDates(date => date(2016, 1, 6)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filter_byDate()
	  {
		IList<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(dates, VALUES_10_14).build();
		LocalDateDoubleTimeSeries test = @base.filter((ld, v) => ld.MonthValue != 6);
		assertEquals(test.size(), 3);
		assertEquals(test.get(DATE_2010_01_01), double?.of(10d));
		assertEquals(test.get(DATE_2012_01_01), double?.of(12d));
		assertEquals(test.get(DATE_2014_01_01), double?.of(14d));
	  }

	  public virtual void test_filter_byValue()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(DATES_2010_14, VALUES_10_14).build();
		LocalDateDoubleTimeSeries test = @base.filter((ld, v) => v % 2 == 1);
		assertEquals(test.size(), 2);
		assertEquals(test.get(DATE_2011_01_01), double?.of(11d));
		assertEquals(test.get(DATE_2013_01_01), double?.of(13d));
	  }

	  public virtual void test_filter_byDateAndValue()
	  {
		IList<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);
		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().putAll(dates, VALUES_10_14).build();

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

	  public virtual void partition()
	  {
		IList<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);
		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().putAll(dates, VALUES_10_14).build();

		Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partition = series.partition((ld, d) => ld.Year % 2 == 0);

		LocalDateDoubleTimeSeries even = partition.First;
		LocalDateDoubleTimeSeries odd = partition.Second;

		assertThat(even.size()).isEqualTo(3);
		assertThat(odd.size()).isEqualTo(2);

		assertThat(even.get(DATE_2010_01_01)).hasValue(10);
		assertThat(even.get(DATE_2012_01_01)).hasValue(12);
		assertThat(even.get(DATE_2014_01_01)).hasValue(14);

		assertThat(odd.get(DATE_2011_06_01)).hasValue(11);
		assertThat(odd.get(DATE_2013_06_01)).hasValue(13);
	  }

	  public virtual void partitionByValue()
	  {
		IList<LocalDate> dates = SparseLocalDateDoubleTimeSeriesTest.dates(DATE_2010_01_01, DATE_2011_06_01, DATE_2012_01_01, DATE_2013_06_01, DATE_2014_01_01);
		LocalDateDoubleTimeSeries series = LocalDateDoubleTimeSeries.builder().putAll(dates, VALUES_10_14).build();

		Pair<LocalDateDoubleTimeSeries, LocalDateDoubleTimeSeries> partition = series.partitionByValue(d => d > 10 && d < 14);

		LocalDateDoubleTimeSeries mid = partition.First;
		LocalDateDoubleTimeSeries extreme = partition.Second;

		assertThat(mid.size()).isEqualTo(3);
		assertThat(extreme.size()).isEqualTo(2);

		assertThat(mid.get(DATE_2011_06_01)).hasValue(11);
		assertThat(mid.get(DATE_2012_01_01)).hasValue(12);
		assertThat(mid.get(DATE_2013_06_01)).hasValue(13);

		assertThat(extreme.get(DATE_2010_01_01)).hasValue(10);
		assertThat(extreme.get(DATE_2014_01_01)).hasValue(14);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IList<LocalDate> dates = ImmutableList.of(DATE_2011_01_01, DATE_2011_06_01);
		IList<double> values = ImmutableList.of(1d, 2d);
		SparseLocalDateDoubleTimeSeries test = SparseLocalDateDoubleTimeSeries.of(dates, values);
		coverImmutableBean(test);
		IList<LocalDate> dates2 = ImmutableList.of(DATE_2011_06_01, DATE_2012_01_01);
		IList<double> values2 = ImmutableList.of(2d, 3d);
		SparseLocalDateDoubleTimeSeries test2 = SparseLocalDateDoubleTimeSeries.of(dates2, values2);
		coverBeanEquals(test, test2);
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

	}

}
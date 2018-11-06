using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using Doubles = com.google.common.primitives.Doubles;

	/// <summary>
	/// Test LocalDateDoubleTimeSeriesBuilder.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LocalDateDoubleTimeSeriesBuilderTest
	public class LocalDateDoubleTimeSeriesBuilderTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test_buildEmptySeries()
		public virtual void test_buildEmptySeries()
		{
		assertEquals(LocalDateDoubleTimeSeries.builder().build(), LocalDateDoubleTimeSeries.empty());
		}

	  //-------------------------------------------------------------------------
	  public virtual void test_get()
	  {
		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder().put(date(2014, 1, 1), 14).put(date(2012, 1, 1), 12).put(date(2013, 1, 1), 13);

		assertEquals(test.get(date(2012, 1, 1)), double?.of(12d));
		assertEquals(test.get(date(2013, 1, 1)), double?.of(13d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(14d));
		assertEquals(test.get(date(2015, 1, 1)), double?.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_merge_dateValue()
	  {
		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.put(date(2013, 1, 1), 2d);
		test.merge(date(2013, 1, 1), 3d, double?.sum);

		assertEquals(test.get(date(2013, 1, 1)), double?.of(5d));
	  }

	  public virtual void test_merge_point()
	  {
		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.put(date(2013, 1, 1), 2d);
		test.merge(LocalDateDoublePoint.of(date(2013, 1, 1), 3d), double?.sum);

		assertEquals(test.get(date(2013, 1, 1)), double?.of(5d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_putAll_collections()
	  {
		ICollection<LocalDate> dates = Arrays.asList(date(2013, 1, 1), date(2014, 1, 1));
		ICollection<double> values = Doubles.asList(2d, 3d);
		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.putAll(dates, values);

		assertEquals(test.get(date(2013, 1, 1)), double?.of(2d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(3d));
	  }

	  public virtual void test_putAll_collection_array()
	  {
		ICollection<LocalDate> dates = Arrays.asList(date(2013, 1, 1), date(2014, 1, 1));
		double[] values = new double[] {2d, 3d};
		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.putAll(dates, values);

		assertEquals(test.get(date(2013, 1, 1)), double?.of(2d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(3d));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_putAll_collectionsMismatch()
	  public virtual void test_putAll_collectionsMismatch()
	  {
		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.putAll(Arrays.asList(date(2014, 1, 1)), Doubles.asList(2d, 3d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_putAll_stream()
	  {
		ICollection<LocalDate> dates = Arrays.asList(date(2013, 1, 1), date(2014, 1, 1));
		ICollection<double> values = Doubles.asList(2d, 3d);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();

		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.put(date(2012, 1, 1), 0d);
		test.put(date(2013, 1, 1), 1d);
		test.putAll(@base.stream());

		assertEquals(test.get(date(2012, 1, 1)), double?.of(0d));
		assertEquals(test.get(date(2013, 1, 1)), double?.of(2d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(3d));
	  }

	  public virtual void test_putAll_toBuilder()
	  {
		ICollection<LocalDate> dates = Arrays.asList(date(2013, 1, 1), date(2014, 1, 1));
		ICollection<double> values = Doubles.asList(2d, 3d);
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().putAll(dates, values).build();

		LocalDateDoubleTimeSeriesBuilder test = LocalDateDoubleTimeSeries.builder();
		test.put(date(2012, 1, 1), 0d);
		test.put(date(2013, 1, 1), 1d);
		test.putAll(@base.toBuilder());

		assertEquals(test.get(date(2012, 1, 1)), double?.of(0d));
		assertEquals(test.get(date(2013, 1, 1)), double?.of(2d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(3d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_seriesGetsSorted()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().put(date(2014, 1, 1), 14).put(date(2012, 1, 1), 12).put(date(2013, 1, 1), 13).build();

		assertEquals(test.size(), 3);
		assertEquals(test.EarliestDate, date(2012, 1, 1));
		assertEquals(test.LatestDate, date(2014, 1, 1));
		assertEquals(test.get(date(2012, 1, 1)), double?.of(12d));
		assertEquals(test.get(date(2013, 1, 1)), double?.of(13d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(14d));
	  }

	  public virtual void test_duplicatesGetOverwritten()
	  {
		LocalDateDoubleTimeSeries test = LocalDateDoubleTimeSeries.builder().put(date(2014, 1, 1), 12).put(date(2014, 1, 1), 14).build();

		assertEquals(test.size(), 1);
		assertEquals(test.get(date(2014, 1, 1)), double?.of(14d));
	  }

	  public virtual void test_useBuilderToAlterSeries()
	  {
		LocalDateDoubleTimeSeries @base = LocalDateDoubleTimeSeries.builder().put(date(2014, 1, 1), 14).put(date(2012, 1, 1), 12).put(date(2013, 1, 1), 13).build();
		LocalDateDoubleTimeSeries test = @base.toBuilder().put(date(2013, 1, 1), 23).put(date(2011, 1, 1), 21).build();

		assertEquals(test.size(), 4);
		assertEquals(test.EarliestDate, date(2011, 1, 1));
		assertEquals(test.LatestDate, date(2014, 1, 1));
		// new value
		assertEquals(test.get(date(2011, 1, 1)), double?.of(21d));
		assertEquals(test.get(date(2012, 1, 1)), double?.of(12d));
		// updated value
		assertEquals(test.get(date(2013, 1, 1)), double?.of(23d));
		assertEquals(test.get(date(2014, 1, 1)), double?.of(14d));
	  }

	  public virtual void densityChoosesImplementation()
	  {
		LocalDateDoubleTimeSeries series1 = LocalDateDoubleTimeSeries.builder().put(date(2015, 1, 5), 14).put(date(2015, 1, 12), 12).put(date(2015, 1, 19), 13).build();

		assertEquals(series1.GetType(), typeof(SparseLocalDateDoubleTimeSeries));

		// Now add in a week's worth of data
		LocalDateDoubleTimeSeries series2 = series1.toBuilder().put(date(2015, 1, 6), 14).put(date(2015, 1, 7), 13).put(date(2015, 1, 8), 12).put(date(2015, 1, 9), 13).build();

		// Not yet enough as we have 7/11 populated (i.e. below 70%)
		assertEquals(series2.GetType(), typeof(SparseLocalDateDoubleTimeSeries));

		// Add in 1 more days giving 8/11 populated
		LocalDateDoubleTimeSeries series3 = series2.toBuilder().put(date(2015, 1, 13), 11).build();

		assertEquals(series3.GetType(), typeof(DenseLocalDateDoubleTimeSeries));

		// Now add in a weekend date, which means we have 9/15
		LocalDateDoubleTimeSeries series4 = series3.toBuilder().put(date(2015, 1, 10), 12).build();

		assertEquals(series4.GetType(), typeof(SparseLocalDateDoubleTimeSeries));

		// Add in 2 new dates giving 11/15
		LocalDateDoubleTimeSeries series5 = series4.toBuilder().put(date(2015, 1, 14), 11).put(date(2015, 1, 15), 10).build();

		assertEquals(series5.GetType(), typeof(DenseLocalDateDoubleTimeSeries));
	  }

	  //-------------------------------------------------------------------------
	  private static LocalDate date(int year, int month, int day)
	  {
		return LocalDate.of(year, month, day);
	  }

	}

}
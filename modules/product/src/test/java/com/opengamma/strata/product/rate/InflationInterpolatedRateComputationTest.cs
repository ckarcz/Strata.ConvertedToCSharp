/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.CH_CPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_HICP;
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndexObservation = com.opengamma.strata.basics.index.PriceIndexObservation;

	/// <summary>
	/// Test <seealso cref="InflationInterpolatedRateComputation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationInterpolatedRateComputationTest
	public class InflationInterpolatedRateComputationTest
	{

	  private static readonly YearMonth START_MONTH_FIRST = YearMonth.of(2014, 1);
	  private static readonly YearMonth START_MONTH_SECOND = YearMonth.of(2014, 2);
	  private static readonly YearMonth END_MONTH_FIRST = YearMonth.of(2015, 1);
	  private static readonly YearMonth END_MONTH_SECOND = YearMonth.of(2015, 2);
	  private const double WEIGHT = 1.0 - 6.0 / 31.0;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		InflationInterpolatedRateComputation test = InflationInterpolatedRateComputation.of(GB_HICP, START_MONTH_FIRST, END_MONTH_FIRST, WEIGHT);
		assertEquals(test.Index, GB_HICP);
		assertEquals(test.StartObservation.FixingMonth, START_MONTH_FIRST);
		assertEquals(test.StartSecondObservation.FixingMonth, START_MONTH_SECOND);
		assertEquals(test.EndObservation.FixingMonth, END_MONTH_FIRST);
		assertEquals(test.EndSecondObservation.FixingMonth, END_MONTH_SECOND);
		assertEquals(test.Weight, WEIGHT, 1.0e-14);
	  }

	  public virtual void test_wrongMonthOrder()
	  {
		assertThrowsIllegalArg(() => InflationInterpolatedRateComputation.of(GB_HICP, END_MONTH_FIRST, START_MONTH_FIRST, WEIGHT));
		assertThrowsIllegalArg(() => InflationInterpolatedRateComputation.meta().builder().set(InflationInterpolatedRateComputation.meta().startObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 1))).set(InflationInterpolatedRateComputation.meta().startSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 1))).set(InflationInterpolatedRateComputation.meta().endObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 7))).set(InflationInterpolatedRateComputation.meta().endSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 8))).set(InflationInterpolatedRateComputation.meta().weight(), WEIGHT).build());
		assertThrowsIllegalArg(() => InflationInterpolatedRateComputation.meta().builder().set(InflationInterpolatedRateComputation.meta().startObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 1))).set(InflationInterpolatedRateComputation.meta().startSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 2))).set(InflationInterpolatedRateComputation.meta().endObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 7))).set(InflationInterpolatedRateComputation.meta().endSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 7))).set(InflationInterpolatedRateComputation.meta().weight(), WEIGHT).build());
		assertThrowsIllegalArg(() => InflationInterpolatedRateComputation.meta().builder().set(InflationInterpolatedRateComputation.meta().startObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 8))).set(InflationInterpolatedRateComputation.meta().startSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 9))).set(InflationInterpolatedRateComputation.meta().endObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 7))).set(InflationInterpolatedRateComputation.meta().endSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 8))).set(InflationInterpolatedRateComputation.meta().weight(), WEIGHT).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		InflationInterpolatedRateComputation test = InflationInterpolatedRateComputation.of(GB_HICP, START_MONTH_FIRST, END_MONTH_FIRST, WEIGHT);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GB_HICP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationInterpolatedRateComputation test1 = InflationInterpolatedRateComputation.of(GB_HICP, START_MONTH_FIRST, END_MONTH_FIRST, WEIGHT);
		coverImmutableBean(test1);
		InflationInterpolatedRateComputation test2 = InflationInterpolatedRateComputation.of(CH_CPI, YearMonth.of(2010, 1), YearMonth.of(2010, 7), WEIGHT + 0.1d);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationInterpolatedRateComputation test = InflationInterpolatedRateComputation.of(GB_HICP, START_MONTH_FIRST, END_MONTH_FIRST, WEIGHT);
		assertSerialization(test);
	  }

	}

}
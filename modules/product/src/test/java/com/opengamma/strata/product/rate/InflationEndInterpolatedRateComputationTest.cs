/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="InflationEndInterpolatedRateComputation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationEndInterpolatedRateComputationTest
	public class InflationEndInterpolatedRateComputationTest
	{

	  private const double START_INDEX = 135d;
	  private static readonly YearMonth END_MONTH_FIRST = YearMonth.of(2015, 1);
	  private static readonly YearMonth END_MONTH_SECOND = YearMonth.of(2015, 2);
	  private const double WEIGHT = 1.0 - 6.0 / 31.0;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		InflationEndInterpolatedRateComputation test = InflationEndInterpolatedRateComputation.of(GB_HICP, START_INDEX, END_MONTH_FIRST, WEIGHT);
		assertEquals(test.Index, GB_HICP);
		assertEquals(test.EndObservation.FixingMonth, END_MONTH_FIRST);
		assertEquals(test.EndSecondObservation.FixingMonth, END_MONTH_SECOND);
		assertEquals(test.StartIndexValue, START_INDEX);
		assertEquals(test.Weight, WEIGHT, 1.0e-14);
	  }

	  public virtual void test_wrongMonthOrder()
	  {
		assertThrowsIllegalArg(() => InflationEndInterpolatedRateComputation.meta().builder().set(InflationEndInterpolatedRateComputation.meta().startIndexValue(), START_INDEX).set(InflationEndInterpolatedRateComputation.meta().endObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 7))).set(InflationEndInterpolatedRateComputation.meta().endSecondObservation(), PriceIndexObservation.of(GB_HICP, YearMonth.of(2010, 7))).set(InflationEndInterpolatedRateComputation.meta().weight(), WEIGHT).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		InflationEndInterpolatedRateComputation test = InflationEndInterpolatedRateComputation.of(GB_HICP, START_INDEX, END_MONTH_FIRST, WEIGHT);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GB_HICP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationEndInterpolatedRateComputation test1 = InflationEndInterpolatedRateComputation.of(GB_HICP, START_INDEX, END_MONTH_FIRST, WEIGHT);
		coverImmutableBean(test1);
		InflationEndInterpolatedRateComputation test2 = InflationEndInterpolatedRateComputation.of(CH_CPI, 334d, YearMonth.of(2010, 7), WEIGHT + 1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationEndInterpolatedRateComputation test = InflationEndInterpolatedRateComputation.of(GB_HICP, START_INDEX, END_MONTH_FIRST, WEIGHT);
		assertSerialization(test);
	  }

	}

}
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
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test <seealso cref="InflationEndMonthRateComputation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationEndMonthRateComputationTest
	public class InflationEndMonthRateComputationTest
	{

	  private const double START_INDEX = 535d;
	  private static readonly YearMonth END_MONTH = YearMonth.of(2015, 1);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		InflationEndMonthRateComputation test = InflationEndMonthRateComputation.of(GB_HICP, START_INDEX, END_MONTH);
		assertEquals(test.Index, GB_HICP);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		InflationEndMonthRateComputation test = InflationEndMonthRateComputation.of(GB_HICP, START_INDEX, END_MONTH);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GB_HICP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationEndMonthRateComputation test1 = InflationEndMonthRateComputation.of(GB_HICP, START_INDEX, END_MONTH);
		coverImmutableBean(test1);
		InflationEndMonthRateComputation test2 = InflationEndMonthRateComputation.of(CH_CPI, 2324d, YearMonth.of(2015, 4));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationEndMonthRateComputation test = InflationEndMonthRateComputation.of(GB_HICP, START_INDEX, END_MONTH);
		assertSerialization(test);
	  }

	}

}
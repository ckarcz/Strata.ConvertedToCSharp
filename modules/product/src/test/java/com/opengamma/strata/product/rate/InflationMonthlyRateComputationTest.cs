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

	/// <summary>
	/// Test <seealso cref="InflationMonthlyRateComputation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InflationMonthlyRateComputationTest
	public class InflationMonthlyRateComputationTest
	{

	  private static readonly YearMonth START_MONTH = YearMonth.of(2014, 1);
	  private static readonly YearMonth END_MONTH = YearMonth.of(2015, 1);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		InflationMonthlyRateComputation test = InflationMonthlyRateComputation.of(GB_HICP, START_MONTH, END_MONTH);
		assertEquals(test.Index, GB_HICP);
	  }

	  public virtual void test_wrongMonthOrder()
	  {
		assertThrowsIllegalArg(() => InflationMonthlyRateComputation.of(GB_HICP, END_MONTH, START_MONTH));
		assertThrowsIllegalArg(() => InflationMonthlyRateComputation.of(GB_HICP, START_MONTH, START_MONTH));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		InflationMonthlyRateComputation test = InflationMonthlyRateComputation.of(GB_HICP, START_MONTH, END_MONTH);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GB_HICP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InflationMonthlyRateComputation test1 = InflationMonthlyRateComputation.of(GB_HICP, START_MONTH, END_MONTH);
		coverImmutableBean(test1);
		InflationMonthlyRateComputation test2 = InflationMonthlyRateComputation.of(CH_CPI, YearMonth.of(2014, 4), YearMonth.of(2015, 4));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		InflationMonthlyRateComputation test = InflationMonthlyRateComputation.of(GB_HICP, START_MONTH, END_MONTH);
		assertSerialization(test);
	  }

	}

}
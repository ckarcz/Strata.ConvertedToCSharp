/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedRateComputationTest
	public class FixedRateComputationTest
	{

	  public virtual void test_of()
	  {
		FixedRateComputation test = FixedRateComputation.of(0.05);
		assertEquals(test.Rate, 0.05);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		FixedRateComputation test = FixedRateComputation.of(0.05);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FixedRateComputation test = FixedRateComputation.of(0.05);
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		FixedRateComputation test = FixedRateComputation.of(0.05);
		assertSerialization(test);
	  }

	}

}
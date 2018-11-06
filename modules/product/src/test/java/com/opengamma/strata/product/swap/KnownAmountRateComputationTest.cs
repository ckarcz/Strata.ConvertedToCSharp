/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test <seealso cref="KnownAmountRateComputation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class KnownAmountRateComputationTest
	public class KnownAmountRateComputationTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1000);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		KnownAmountRateComputation test = KnownAmountRateComputation.of(GBP_P1000);
		assertEquals(test.Amount, GBP_P1000);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices_simple()
	  {
		KnownAmountRateComputation test = KnownAmountRateComputation.of(GBP_P1000);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		KnownAmountRateComputation test = KnownAmountRateComputation.of(GBP_P1000);
		coverImmutableBean(test);
		KnownAmountRateComputation test2 = KnownAmountRateComputation.of(GBP_P1000.plus(100));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		KnownAmountRateComputation test = KnownAmountRateComputation.of(GBP_P1000);
		assertSerialization(test);
	  }

	}

}
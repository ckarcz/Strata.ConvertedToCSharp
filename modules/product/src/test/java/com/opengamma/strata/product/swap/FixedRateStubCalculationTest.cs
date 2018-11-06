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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;

	/// <summary>
	/// Test <seealso cref="FixedRateStubCalculation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedRateStubCalculationTest
	public class FixedRateStubCalculationTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1000);

	  //-------------------------------------------------------------------------
	  public virtual void test_ofFixedRate()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.ofFixedRate(0.025d);
		assertEquals(test.FixedRate, double?.of(0.025d));
		assertEquals(test.KnownAmount, null);
		assertEquals(test.FixedRate, true);
		assertEquals(test.KnownAmount, false);
	  }

	  public virtual void test_ofKnownAmount()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.ofKnownAmount(GBP_P1000);
		assertEquals(test.FixedRate, double?.empty());
		assertEquals(test.KnownAmount, GBP_P1000);
		assertEquals(test.FixedRate, false);
		assertEquals(test.KnownAmount, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_invalid_fixedAndKnown()
	  {
		assertThrowsIllegalArg(() => FixedRateStubCalculation.meta().builder().set(FixedRateStubCalculation.meta().fixedRate(), 0.025d).set(FixedRateStubCalculation.meta().knownAmount(), GBP_P1000).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createRateComputation_NONE()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.NONE;
		assertEquals(test.createRateComputation(3d), FixedRateComputation.of(3d));
	  }

	  public virtual void test_createRateComputation_fixedRate()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.ofFixedRate(0.025d);
		assertEquals(test.createRateComputation(3d), FixedRateComputation.of(0.025d));
	  }

	  public virtual void test_createRateComputation_knownAmount()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.ofKnownAmount(GBP_P1000);
		assertEquals(test.createRateComputation(3d), KnownAmountRateComputation.of(GBP_P1000));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.ofFixedRate(0.025d);
		coverImmutableBean(test);
		FixedRateStubCalculation test2 = FixedRateStubCalculation.ofKnownAmount(GBP_P1000);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FixedRateStubCalculation test = FixedRateStubCalculation.ofFixedRate(0.025d);
		assertSerialization(test);
	  }

	}

}
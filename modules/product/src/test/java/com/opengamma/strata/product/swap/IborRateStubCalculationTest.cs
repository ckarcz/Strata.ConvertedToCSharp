/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborRateStubCalculationTest
	public class IborRateStubCalculationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1000);
	  private static readonly LocalDate DATE = date(2015, 6, 30);

	  //-------------------------------------------------------------------------
	  public virtual void test_ofFixedRate()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofFixedRate(0.025d);
		assertEquals(test.FixedRate, double?.of(0.025d));
		assertEquals(test.Index, null);
		assertEquals(test.IndexInterpolated, null);
		assertEquals(test.FixedRate, true);
		assertEquals(test.KnownAmount, false);
		assertEquals(test.FloatingRate, false);
		assertEquals(test.Interpolated, false);
	  }

	  public virtual void test_ofKnownAmount()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofKnownAmount(GBP_P1000);
		assertEquals(test.FixedRate, double?.empty());
		assertEquals(test.KnownAmount, GBP_P1000);
		assertEquals(test.Index, null);
		assertEquals(test.IndexInterpolated, null);
		assertEquals(test.FixedRate, false);
		assertEquals(test.KnownAmount, true);
		assertEquals(test.FloatingRate, false);
		assertEquals(test.Interpolated, false);
	  }

	  public virtual void test_ofIborRate()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofIborRate(GBP_LIBOR_3M);
		assertEquals(test.FixedRate, double?.empty());
		assertEquals(test.Index, GBP_LIBOR_3M);
		assertEquals(test.IndexInterpolated, null);
		assertEquals(test.FixedRate, false);
		assertEquals(test.KnownAmount, false);
		assertEquals(test.FloatingRate, true);
		assertEquals(test.Interpolated, false);
	  }

	  public virtual void test_ofIborInterpolatedRate()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_1M, GBP_LIBOR_3M);
		assertEquals(test.FixedRate, double?.empty());
		assertEquals(test.Index, GBP_LIBOR_1M);
		assertEquals(test.IndexInterpolated, GBP_LIBOR_3M);
		assertEquals(test.FixedRate, false);
		assertEquals(test.KnownAmount, false);
		assertEquals(test.FloatingRate, true);
		assertEquals(test.Interpolated, true);
	  }

	  public virtual void test_ofIborInterpolatedRate_invalid_interpolatedSameIndex()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_3M, GBP_LIBOR_3M));
	  }

	  public virtual void test_of_null()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.ofIborRate(null));
		assertThrowsIllegalArg(() => IborRateStubCalculation.ofIborInterpolatedRate(null, GBP_LIBOR_3M));
		assertThrowsIllegalArg(() => IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_3M, null));
		assertThrowsIllegalArg(() => IborRateStubCalculation.ofIborInterpolatedRate(null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_invalid_fixedAndIbor()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.builder().fixedRate(0.025d).index(GBP_LIBOR_3M).build());
	  }

	  public virtual void test_builder_invalid_fixedAndKnown()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.builder().fixedRate(0.025d).knownAmount(GBP_P1000).build());
	  }

	  public virtual void test_builder_invalid_knownAndIbor()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.builder().knownAmount(GBP_P1000).index(GBP_LIBOR_3M).build());
	  }

	  public virtual void test_builder_invalid_interpolatedWithoutBase()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.builder().indexInterpolated(GBP_LIBOR_3M).build());
	  }

	  public virtual void test_builder_invalid_interpolatedSameIndex()
	  {
		assertThrowsIllegalArg(() => IborRateStubCalculation.builder().index(GBP_LIBOR_3M).indexInterpolated(GBP_LIBOR_3M).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createRateComputation_NONE()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.NONE;
		assertEquals(test.createRateComputation(DATE, GBP_LIBOR_3M, REF_DATA), IborRateComputation.of(GBP_LIBOR_3M, DATE, REF_DATA));
	  }

	  public virtual void test_createRateComputation_fixedRate()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofFixedRate(0.025d);
		assertEquals(test.createRateComputation(DATE, GBP_LIBOR_3M, REF_DATA), FixedRateComputation.of(0.025d));
	  }

	  public virtual void test_createRateComputation_knownAmount()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofKnownAmount(GBP_P1000);
		assertEquals(test.createRateComputation(DATE, GBP_LIBOR_3M, REF_DATA), KnownAmountRateComputation.of(GBP_P1000));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofIborInterpolatedRate(GBP_LIBOR_1M, GBP_LIBOR_3M);
		coverImmutableBean(test);
		IborRateStubCalculation test2 = IborRateStubCalculation.ofFixedRate(0.028d);
		coverBeanEquals(test, test2);
		IborRateStubCalculation test3 = IborRateStubCalculation.ofKnownAmount(GBP_P1000);
		coverBeanEquals(test, test3);
	  }

	  public virtual void test_serialization()
	  {
		IborRateStubCalculation test = IborRateStubCalculation.ofIborRate(GBP_LIBOR_3M);
		assertSerialization(test);
	  }

	}

}
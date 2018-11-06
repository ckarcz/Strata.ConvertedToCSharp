/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborRateComputationTest
	public class IborRateComputationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		IborRateComputation test = IborRateComputation.of(USD_LIBOR_3M, date(2016, 2, 18), REF_DATA);
		IborIndexObservation obs = IborIndexObservation.of(USD_LIBOR_3M, date(2016, 2, 18), REF_DATA);
		IborRateComputation expected = IborRateComputation.of(obs);
		assertEquals(test, expected);
		assertEquals(test.Currency, USD);
		assertEquals(test.Index, obs.Index);
		assertEquals(test.FixingDate, obs.FixingDate);
		assertEquals(test.EffectiveDate, obs.EffectiveDate);
		assertEquals(test.MaturityDate, obs.MaturityDate);
		assertEquals(test.YearFraction, obs.YearFraction);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		IborRateComputation test = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborRateComputation test = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
		coverImmutableBean(test);
		IborRateComputation test2 = IborRateComputation.of(GBP_LIBOR_1M, date(2014, 7, 30), REF_DATA);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborRateComputation test = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
		assertSerialization(test);
	  }

	}

}
/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_2W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1W;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborInterpolatedRateComputationTest
	public class IborInterpolatedRateComputationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2014, 6, 30);
	  private static readonly IborIndexObservation GBP_LIBOR_1W_OBS = IborIndexObservation.of(GBP_LIBOR_1W, FIXING_DATE, REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_1M_OBS = IborIndexObservation.of(GBP_LIBOR_1M, FIXING_DATE, REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_3M_OBS = IborIndexObservation.of(GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
	  private static readonly IborIndexObservation EUR_EURIBOR_1W_OBS = IborIndexObservation.of(EUR_EURIBOR_1W, FIXING_DATE, REF_DATA);
	  private static readonly IborIndexObservation EUR_EURIBOR_2W_OBS = IborIndexObservation.of(EUR_EURIBOR_2W, FIXING_DATE, REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_3M_OBS2 = IborIndexObservation.of(GBP_LIBOR_3M, FIXING_DATE.plusDays(1), REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_monthly()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_1M, GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
		assertEquals(test.ShortObservation, GBP_LIBOR_1M_OBS);
		assertEquals(test.LongObservation, GBP_LIBOR_3M_OBS);
		assertEquals(test.FixingDate, FIXING_DATE);
	  }

	  public virtual void test_of_monthly_byObs()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_1M_OBS, GBP_LIBOR_3M_OBS);
		assertEquals(test.ShortObservation, GBP_LIBOR_1M_OBS);
		assertEquals(test.LongObservation, GBP_LIBOR_3M_OBS);
		assertEquals(test.FixingDate, FIXING_DATE);
	  }

	  public virtual void test_of_monthly_reverseOrder()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_3M, GBP_LIBOR_1M, FIXING_DATE, REF_DATA);
		assertEquals(test.ShortObservation, GBP_LIBOR_1M_OBS);
		assertEquals(test.LongObservation, GBP_LIBOR_3M_OBS);
		assertEquals(test.FixingDate, FIXING_DATE);
	  }

	  public virtual void test_of_weekly()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(EUR_EURIBOR_1W, EUR_EURIBOR_2W, FIXING_DATE, REF_DATA);
		assertEquals(test.ShortObservation, EUR_EURIBOR_1W_OBS);
		assertEquals(test.LongObservation, EUR_EURIBOR_2W_OBS);
		assertEquals(test.FixingDate, FIXING_DATE);
	  }

	  public virtual void test_of_weekly_reverseOrder()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(EUR_EURIBOR_2W, EUR_EURIBOR_1W, FIXING_DATE, REF_DATA);
		assertEquals(test.ShortObservation, EUR_EURIBOR_1W_OBS);
		assertEquals(test.LongObservation, EUR_EURIBOR_2W_OBS);
		assertEquals(test.FixingDate, FIXING_DATE);
	  }

	  public virtual void test_of_weekMonthCombination()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_1W, GBP_LIBOR_1M, FIXING_DATE, REF_DATA);
		assertEquals(test.ShortObservation, GBP_LIBOR_1W_OBS);
		assertEquals(test.LongObservation, GBP_LIBOR_1M_OBS);
		assertEquals(test.FixingDate, FIXING_DATE);
	  }

	  public virtual void test_of_sameIndex()
	  {
		assertThrowsIllegalArg(() => IborInterpolatedRateComputation.of(GBP_LIBOR_1M, GBP_LIBOR_1M, FIXING_DATE, REF_DATA));
	  }

	  public virtual void test_builder_indexOrder()
	  {
		assertThrowsIllegalArg(() => IborInterpolatedRateComputation.meta().builder().set(IborInterpolatedRateComputation.meta().shortObservation(), GBP_LIBOR_3M_OBS).set(IborInterpolatedRateComputation.meta().longObservation(), GBP_LIBOR_1M_OBS).build());
		assertThrowsIllegalArg(() => IborInterpolatedRateComputation.meta().builder().set(IborInterpolatedRateComputation.meta().shortObservation(), EUR_EURIBOR_2W_OBS).set(IborInterpolatedRateComputation.meta().longObservation(), EUR_EURIBOR_1W_OBS).build());
		assertThrowsIllegalArg(() => IborInterpolatedRateComputation.of(EUR_EURIBOR_2W_OBS, EUR_EURIBOR_1W_OBS));
	  }

	  public virtual void test_of_differentCurrencies()
	  {
		assertThrowsIllegalArg(() => IborInterpolatedRateComputation.of(EUR_EURIBOR_2W, GBP_LIBOR_1M, FIXING_DATE, REF_DATA));
	  }

	  public virtual void test_of_differentFixingDates()
	  {
		assertThrowsIllegalArg(() => IborInterpolatedRateComputation.meta().builder().set(IborInterpolatedRateComputation.meta().shortObservation(), GBP_LIBOR_1M_OBS).set(IborInterpolatedRateComputation.meta().longObservation(), GBP_LIBOR_3M_OBS2).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_1M, GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_1M, GBP_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_1M, GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
		coverImmutableBean(test);
		IborInterpolatedRateComputation test2 = IborInterpolatedRateComputation.of(USD_LIBOR_1M, USD_LIBOR_3M, date(2014, 7, 30), REF_DATA);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborInterpolatedRateComputation test = IborInterpolatedRateComputation.of(GBP_LIBOR_1M, GBP_LIBOR_3M, FIXING_DATE, REF_DATA);
		assertSerialization(test);
	  }

	}

}
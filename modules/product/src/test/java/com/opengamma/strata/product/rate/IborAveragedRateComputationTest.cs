/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborAveragedRateComputationTest
	public class IborAveragedRateComputationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborIndexObservation GBP_LIBOR_3M_OBS1 = IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
	  private static readonly IborIndexObservation GBP_LIBOR_3M_OBS2 = IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 7, 30), REF_DATA);

	  internal ImmutableList<IborAveragedFixing> FIXINGS = ImmutableList.of(IborAveragedFixing.of(GBP_LIBOR_3M_OBS1), IborAveragedFixing.of(GBP_LIBOR_3M_OBS2));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_List()
	  {
		IborAveragedRateComputation test = IborAveragedRateComputation.of(FIXINGS);
		assertEquals(test.Fixings, FIXINGS);
		assertEquals(test.TotalWeight, 2d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		IborAveragedRateComputation test = IborAveragedRateComputation.of(FIXINGS);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborAveragedRateComputation test = IborAveragedRateComputation.of(FIXINGS);
		coverImmutableBean(test);
		IborAveragedRateComputation test2 = IborAveragedRateComputation.of(FIXINGS.subList(0, 1));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborAveragedRateComputation test = IborAveragedRateComputation.of(FIXINGS);
		assertSerialization(test);
	  }

	}

}
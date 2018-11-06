/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
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
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;

	/// <summary>
	/// Test <seealso cref="RatesCurveGroupEntry"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveGroupEntryTest
	public class RatesCurveGroupEntryTest
	{

	  private static readonly CurveName CURVE_NAME1 = CurveName.of("Test");
	  private static readonly CurveName CURVE_NAME2 = CurveName.of("Test2");

	  public virtual void test_builder()
	  {
		RatesCurveGroupEntry test = RatesCurveGroupEntry.builder().curveName(CURVE_NAME1).discountCurrencies(GBP).indices(GBP_LIBOR_1M, GBP_LIBOR_3M, GBP_SONIA).build();
		assertEquals(test.CurveName, CURVE_NAME1);
		assertEquals(test.DiscountCurrencies, ImmutableSet.of(GBP));
		assertEquals(test.Indices, ImmutableSet.of(GBP_LIBOR_1M, GBP_LIBOR_3M, GBP_SONIA));
		assertEquals(test.getIndices(typeof(IborIndex)), ImmutableSet.of(GBP_LIBOR_1M, GBP_LIBOR_3M));
		assertEquals(test.getIndices(typeof(OvernightIndex)), ImmutableSet.of(GBP_SONIA));
		assertEquals(test.getIndices(typeof(PriceIndex)), ImmutableSet.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatesCurveGroupEntry test = RatesCurveGroupEntry.builder().curveName(CURVE_NAME1).discountCurrencies(GBP).build();
		coverImmutableBean(test);
		RatesCurveGroupEntry test2 = RatesCurveGroupEntry.builder().curveName(CURVE_NAME2).indices(GBP_LIBOR_1M, GBP_SONIA).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatesCurveGroupEntry test = RatesCurveGroupEntry.builder().curveName(CURVE_NAME1).discountCurrencies(GBP).build();
		assertSerialization(test);
	  }

	}

}
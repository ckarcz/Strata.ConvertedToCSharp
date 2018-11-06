/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
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
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="ResolvedFra"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFraTest
	public class ResolvedFraTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_1M = 1_000_000d;
	  private const double NOTIONAL_2M = 2_000_000d;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedFra test = sut();
		assertEquals(test.PaymentDate, date(2015, 6, 16));
		assertEquals(test.StartDate, date(2015, 6, 15));
		assertEquals(test.EndDate, date(2015, 9, 15));
		assertEquals(test.YearFraction, 0.25d, 0d);
		assertEquals(test.FixedRate, 0.25d, 0d);
		assertEquals(test.FloatingRate, IborRateComputation.of(GBP_LIBOR_3M, date(2015, 6, 12), REF_DATA));
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL_1M, 0d);
		assertEquals(test.Discounting, ISDA);
		assertEquals(test.allIndices(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  public virtual void test_builder_datesInOrder()
	  {
		assertThrowsIllegalArg(() => ResolvedFra.builder().notional(NOTIONAL_1M).paymentDate(date(2015, 6, 15)).startDate(date(2015, 6, 15)).endDate(date(2015, 6, 14)).fixedRate(0.25d).floatingRate(IborRateComputation.of(GBP_LIBOR_3M, date(2015, 6, 12), REF_DATA)).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		ResolvedFra test = sut();
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedFra sut()
	  {
		return ResolvedFra.builder().paymentDate(date(2015, 6, 16)).startDate(date(2015, 6, 15)).endDate(date(2015, 9, 15)).yearFraction(0.25d).fixedRate(0.25d).floatingRate(IborRateComputation.of(GBP_LIBOR_3M, date(2015, 6, 12), REF_DATA)).currency(GBP).notional(NOTIONAL_1M).discounting(ISDA).build();
	  }

	  internal static ResolvedFra sut2()
	  {
		return ResolvedFra.builder().paymentDate(date(2015, 6, 17)).startDate(date(2015, 6, 16)).endDate(date(2015, 9, 16)).yearFraction(0.26d).fixedRate(0.27d).floatingRate(IborRateComputation.of(GBP_LIBOR_2M, date(2015, 6, 12), REF_DATA)).currency(USD).notional(NOTIONAL_2M).discounting(FraDiscountingMethod.NONE).build();
	  }

	}

}
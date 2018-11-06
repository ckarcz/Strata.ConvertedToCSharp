/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
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

	/// <summary>
	/// Test <seealso cref="IborIndexObservation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborIndexObservationTest
	public class IborIndexObservationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		IborIndexObservation test = IborIndexObservation.of(USD_LIBOR_3M, date(2016, 2, 18), REF_DATA);
		double yearFraction = USD_LIBOR_3M.DayCount.yearFraction(date(2016, 2, 22), date(2016, 5, 23));
		IborIndexObservation expected = new IborIndexObservation(USD_LIBOR_3M, date(2016, 2, 18), date(2016, 2, 22), date(2016, 5, 23), yearFraction);
		assertEquals(test, expected);
		assertEquals(test.Currency, USD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborIndexObservation test = IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
		coverImmutableBean(test);
		IborIndexObservation test2 = IborIndexObservation.of(GBP_LIBOR_1M, date(2014, 7, 30), REF_DATA);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborIndexObservation test = IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);
		assertSerialization(test);
	  }

	}

}
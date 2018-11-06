/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_GBP_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
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
	/// Test <seealso cref="FxIndexObservation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxIndexObservationTest
	public class FxIndexObservationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2016, 2, 22);
	  private static readonly LocalDate MATURITY_DATE = GBP_USD_WM.calculateMaturityFromFixing(FIXING_DATE, REF_DATA);

	  public virtual void test_of()
	  {
		FxIndexObservation test = FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.FixingDate, FIXING_DATE);
		assertEquals(test.MaturityDate, MATURITY_DATE);
		assertEquals(test.CurrencyPair, GBP_USD_WM.CurrencyPair);
		assertEquals(test.ToString(), "FxIndexObservation[GBP/USD-WM on 2016-02-22]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxIndexObservation test = FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA);
		coverImmutableBean(test);
		FxIndexObservation test2 = FxIndexObservation.of(EUR_GBP_ECB, FIXING_DATE.plusDays(1), REF_DATA);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxIndexObservation test = FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA);
		assertSerialization(test);
	  }

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.CH_CPI;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.PriceIndices.GB_HICP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="PriceIndexObservation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PriceIndexObservationTest
	public class PriceIndexObservationTest
	{

	  private static readonly YearMonth FIXING_MONTH = YearMonth.of(2016, 2);

	  public virtual void test_of()
	  {
		PriceIndexObservation test = PriceIndexObservation.of(GB_HICP, FIXING_MONTH);
		assertEquals(test.Index, GB_HICP);
		assertEquals(test.FixingMonth, FIXING_MONTH);
		assertEquals(test.Currency, GB_HICP.Currency);
		assertEquals(test.ToString(), "PriceIndexObservation[GB-HICP on 2016-02]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		PriceIndexObservation test = PriceIndexObservation.of(GB_HICP, FIXING_MONTH);
		coverImmutableBean(test);
		PriceIndexObservation test2 = PriceIndexObservation.of(CH_CPI, FIXING_MONTH.plusMonths(1));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		PriceIndexObservation test = PriceIndexObservation.of(GB_HICP, FIXING_MONTH);
		assertSerialization(test);
	  }

	}

}
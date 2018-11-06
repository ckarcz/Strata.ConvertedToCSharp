/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="AdvancedMeasures"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AdvancedMeasuresTest
	public class AdvancedMeasuresTest
	{

	  public virtual void test_standard()
	  {
		assertEquals(AdvancedMeasures.PV01_SEMI_PARALLEL_GAMMA_BUCKETED.CurrencyConvertible, true);
	  }

	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(Measures));
	  }

	}

}
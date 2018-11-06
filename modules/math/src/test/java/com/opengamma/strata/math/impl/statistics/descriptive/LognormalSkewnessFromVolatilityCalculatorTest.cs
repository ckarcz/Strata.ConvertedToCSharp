/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LognormalSkewnessFromVolatilityCalculatorTest
	public class LognormalSkewnessFromVolatilityCalculatorTest
	{
	  private static readonly System.Func<double, double, double> F = new LognormalSkewnessFromVolatilityCalculator();
	  private const double SIGMA = 0.3;
	  private const double T = 0.25;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(F.applyAsDouble(SIGMA, T), 0.4560, 1e-4);
	  }
	}

}
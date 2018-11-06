using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;


	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParabolicMinimumBracketerTest extends MinimumBracketerTestCase
	public class ParabolicMinimumBracketerTest : MinimumBracketerTestCase
	{
	  private static readonly MinimumBracketer BRACKETER = new ParabolicMinimumBracketer();
	  private static readonly System.Func<double, double> LINEAR = (final double? x) =>
	  {

  return 2 * x - 4;

	  };
	  private static readonly System.Func<double, double> QUADRATIC = (final double? x) =>
	  {

  return x * x + 7 * x + 12;

	  };
	  private static readonly System.Func<double, double> MOD_QUADRATIC = (final double? x) =>
	  {

  return Math.Abs(x * x - 4);
	  };

	  private static readonly System.Func<double, double> STRETCHED_QUADRATIC = (final double? x) =>
	  {

  return FunctionUtils.square((x - 50) / 50.0);
	  };

	  protected internal override MinimumBracketer Bracketer
	  {
		  get
		  {
			return BRACKETER;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = com.opengamma.strata.math.MathException.class) public void test()
	  public virtual void test()
	  {
		BRACKETER.getBracketedPoints(LINEAR, 0.0, 1.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testQuadratic()
	  public virtual void testQuadratic()
	  {
		assertFunction(QUADRATIC, -100, 100);
		assertFunction(QUADRATIC, 100, -100);
		assertFunction(QUADRATIC, 100, 50);
		assertFunction(QUADRATIC, -100, -50);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testInitialGuessBracketsTwoMinima()
	  public virtual void testInitialGuessBracketsTwoMinima()
	  {
		assertFunction(MOD_QUADRATIC, -3, -1);
		assertFunction(MOD_QUADRATIC, -3, 3.5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testStretchedQuadratic()
	  public virtual void testStretchedQuadratic()
	  {
		assertFunction(STRETCHED_QUADRATIC, 0, 1);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void assertFunction(final java.util.function.Function<double, double> f, final double xLower, final double xUpper)
	  private void assertFunction(System.Func<double, double> f, double xLower, double xUpper)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] result = BRACKETER.getBracketedPoints(f, xLower, xUpper);
		double[] result = BRACKETER.getBracketedPoints(f, xLower, xUpper);
		if (result[0] < result[1])
		{
		  assertTrue(result[1] < result[2]);
		}
		else
		{
		  assertTrue(result[2] < result[1]);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double f2 = f.apply(result[1]);
		double f2 = f(result[1]);
		assertTrue(f(result[0]) > f2);
		assertTrue(f(result[2]) > f2);
	  }
	}

}
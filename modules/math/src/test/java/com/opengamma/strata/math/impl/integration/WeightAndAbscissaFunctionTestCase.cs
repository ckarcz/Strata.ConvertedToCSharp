/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class WeightAndAbscissaFunctionTestCase
	public abstract class WeightAndAbscissaFunctionTestCase
	{
	  private const double EPS = 1e-3;

	  protected internal abstract QuadratureWeightAndAbscissaFunction Function {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		Function.generate(-1);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertResults(final GaussianQuadratureData f, final double[] x, final double[] w)
	  protected internal virtual void assertResults(GaussianQuadratureData f, double[] x, double[] w)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] x1 = f.getAbscissas();
		double[] x1 = f.Abscissas;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] w1 = f.getWeights();
		double[] w1 = f.Weights;
		for (int i = 0; i < x.Length; i++)
		{
		  assertEquals(x1[i], x[i], EPS);
		  assertEquals(w1[i], w[i], EPS);
		}
	  }
	}

}
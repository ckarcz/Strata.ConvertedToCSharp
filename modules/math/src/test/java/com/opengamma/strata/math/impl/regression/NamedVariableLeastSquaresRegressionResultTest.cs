using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;


	using Well44497b = org.apache.commons.math3.random.Well44497b;
	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NamedVariableLeastSquaresRegressionResultTest
	public class NamedVariableLeastSquaresRegressionResultTest
	{

	  private static readonly Well44497b RANDOM = new Well44497b(0L);
	  private const double EPS = 1e-2;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullNames()
	  public virtual void testNullNames()
	  {
		new NamedVariableLeastSquaresRegressionResult(null, new LeastSquaresRegressionResult(null, null, 0, null, 0, 0, null, null, false));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullRegression()
	  public virtual void testNullRegression()
	  {
		new NamedVariableLeastSquaresRegressionResult(new List<string>(), null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNonMatchingInputs()
	  public virtual void testNonMatchingInputs()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<String> names = java.util.Arrays.asList("A", "B");
		IList<string> names = Arrays.asList("A", "B");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] array = new double[] {1.0 };
		double[] array = new double[] {1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegressionResult result = new LeastSquaresRegressionResult(array, array, 0.0, array, 0.0, 0.0, array, array, false);
		LeastSquaresRegressionResult result = new LeastSquaresRegressionResult(array, array, 0.0, array, 0.0, 0.0, array, array, false);
		new NamedVariableLeastSquaresRegressionResult(names, result);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		const int n = 100;
		const double beta0 = 0.3;
		const double beta1 = 2.5;
		const double beta2 = -0.3;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.DoubleBinaryOperator f1 = (x1, x2) -> beta1 * x1 + beta2 * x2;
		System.Func<double, double, double> f1 = (x1, x2) => beta1 * x1 + beta2 * x2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.DoubleBinaryOperator f2 = (x1, x2) -> beta0 + beta1 * x1 + beta2 * x2;
		System.Func<double, double, double> f2 = (x1, x2) => beta0 + beta1 * x1 + beta2 * x2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] x = new double[n][2];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] x = new double[n][2];
		double[][] x = RectangularArrays.ReturnRectangularDoubleArray(n, 2);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y1 = new double[n];
		double[] y1 = new double[n];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] y2 = new double[n];
		double[] y2 = new double[n];
		for (int i = 0; i < n; i++)
		{
		  x[i][0] = RANDOM.NextDouble();
		  x[i][1] = RANDOM.NextDouble();
		  y1[i] = f1(x[i][0], x[i][1]);
		  y2[i] = f2(x[i][0], x[i][1]);
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegression ols = new OrdinaryLeastSquaresRegression();
		LeastSquaresRegression ols = new OrdinaryLeastSquaresRegression();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<String> names = java.util.Arrays.asList("1", "2");
		IList<string> names = Arrays.asList("1", "2");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final NamedVariableLeastSquaresRegressionResult result1 = new NamedVariableLeastSquaresRegressionResult(names, ols.regress(x, null, y1, false));
		NamedVariableLeastSquaresRegressionResult result1 = new NamedVariableLeastSquaresRegressionResult(names, ols.regress(x, null, y1, false));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final NamedVariableLeastSquaresRegressionResult result2 = new NamedVariableLeastSquaresRegressionResult(names, ols.regress(x, null, y2, true));
		NamedVariableLeastSquaresRegressionResult result2 = new NamedVariableLeastSquaresRegressionResult(names, ols.regress(x, null, y2, true));
		try
		{
		  result1.getPredictedValue((IDictionary<string, double>) null);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		assertEquals(result1.getPredictedValue(System.Linq.Enumerable.Empty<string, double>()), 0.0, 1e-16);
		try
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Map<String, double> map = new java.util.HashMap<>();
		  IDictionary<string, double> map = new Dictionary<string, double>();
		  map["1"] = 0.0;
		  result1.getPredictedValue(map);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		double x1, x2, x3;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Map<String, double> var = new java.util.HashMap<>();
		IDictionary<string, double> var = new Dictionary<string, double>();
		for (int i = 0; i < 10; i++)
		{
		  x1 = RANDOM.NextDouble();
		  x2 = RANDOM.NextDouble();
		  x3 = RANDOM.NextDouble();
		  var["1"] = x1;
		  var["2"] = x2;
		  assertEquals(result1.getPredictedValue(var), f1(x1, x2), EPS);
		  assertEquals(result2.getPredictedValue(var), f2(x1, x2), EPS);
		  var["3"] = x3;
		  assertEquals(result1.getPredictedValue(var), f1(x1, x2), EPS);
		  assertEquals(result2.getPredictedValue(var), f2(x1, x2), EPS);
		}
	  }
	}

}
/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Assert = org.testng.Assert;

	/// <summary>
	/// Abstract test.
	/// </summary>
	public abstract class Minimizer1DTestCase
	{
	  private const double EPS = 1e-5;
	  private static readonly System.Func<double, double> QUADRATIC = (final double? x) =>
	  {

  return x * x + 7 * x + 12;

	  };
	  private static readonly System.Func<double, double> QUINTIC = (final double? x) =>
	  {
  return 1 + x * (-3 + x * (-9 + x * (-1 + x * (4 + x))));
	  };

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public void assertInputs(final ScalarMinimizer minimizer)
	  public virtual void assertInputs(ScalarMinimizer minimizer)
	  {
		try
		{
		  minimizer.minimize(null, 0.0, 2.0, 3.0);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public void assertMinimizer(final ScalarMinimizer minimizer)
	  public virtual void assertMinimizer(ScalarMinimizer minimizer)
	  {
		double result = minimizer.minimize(QUADRATIC, 0.0, -10.0, 10.0);
		assertEquals(-3.5, result, EPS);
		result = minimizer.minimize(QUINTIC, 0.0, 0.5, 2.0);
		assertEquals(1.06154, result, EPS);
	  }
	}

}
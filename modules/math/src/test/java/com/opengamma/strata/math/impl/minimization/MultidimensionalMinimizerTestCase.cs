/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.minimization.MinimizationTestFunctions.COUPLED_ROSENBROCK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.minimization.MinimizationTestFunctions.ROSENBROCK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.minimization.MinimizationTestFunctions.UNCOUPLED_ROSENBROCK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class MultidimensionalMinimizerTestCase
	public abstract class MultidimensionalMinimizerTestCase
	{

	  private static readonly System.Func<DoubleArray, double> F_2D = (final DoubleArray x) =>
	  {
  return (x.get(0) + 3.4) * (x.get(0) + 3.4) + (x.get(1) - 1) * (x.get(1) - 1);
	  };

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertInputs(final Minimizer<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, com.opengamma.strata.collect.array.DoubleArray> minimizer)
	  protected internal virtual void assertInputs(Minimizer<System.Func<DoubleArray, double>, DoubleArray> minimizer)
	  {
		try
		{
		  minimizer.minimize(null, DoubleArray.of(2d, 3d));
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		try
		{
		  minimizer.minimize(F_2D, null);
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
//ORIGINAL LINE: protected void assertMinimizer(final Minimizer<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, com.opengamma.strata.collect.array.DoubleArray> minimizer, final double tol)
	  protected internal virtual void assertMinimizer(Minimizer<System.Func<DoubleArray, double>, DoubleArray> minimizer, double tol)
	  {
		DoubleArray r = minimizer.minimize(F_2D, DoubleArray.of(10d, 10d));
		assertEquals(r.get(0), -3.4, tol);
		assertEquals(r.get(1), 1, tol);
		r = (minimizer.minimize(ROSENBROCK, DoubleArray.of(10d, -5d)));
		assertEquals(r.get(0), 1, tol);
		assertEquals(r.get(1), 1, tol);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingRosenbrock(final Minimizer<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, com.opengamma.strata.collect.array.DoubleArray> minimizer, final double tol)
	  protected internal virtual void assertSolvingRosenbrock(Minimizer<System.Func<DoubleArray, double>, DoubleArray> minimizer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1d, 1d);
		DoubleArray start = DoubleArray.of(-1d, 1d);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimizer.minimize(ROSENBROCK, start);
		DoubleArray solution = minimizer.minimize(ROSENBROCK, start);
		assertEquals(1.0, solution.get(0), tol);
		assertEquals(1.0, solution.get(1), tol);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingUncoupledRosenbrock(final Minimizer<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, com.opengamma.strata.collect.array.DoubleArray> minimizer, final double tol)
	  protected internal virtual void assertSolvingUncoupledRosenbrock(Minimizer<System.Func<DoubleArray, double>, DoubleArray> minimizer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
		DoubleArray start = DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimizer.minimize(UNCOUPLED_ROSENBROCK, start);
		DoubleArray solution = minimizer.minimize(UNCOUPLED_ROSENBROCK, start);
		for (int i = 0; i < solution.size(); i++)
		{
		  assertEquals(1.0, solution.get(i), tol);
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingCoupledRosenbrock(final Minimizer<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, com.opengamma.strata.collect.array.DoubleArray> minimizer, final double tol)
	  protected internal virtual void assertSolvingCoupledRosenbrock(Minimizer<System.Func<DoubleArray, double>, DoubleArray> minimizer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0, 1.0);
		DoubleArray start = DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0, 1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimizer.minimize(COUPLED_ROSENBROCK, start);
		DoubleArray solution = minimizer.minimize(COUPLED_ROSENBROCK, start);
		for (int i = 0; i < solution.size(); i++)
		{
		  assertEquals(1.0, solution.get(i), tol);
		}
	  }

	}

}
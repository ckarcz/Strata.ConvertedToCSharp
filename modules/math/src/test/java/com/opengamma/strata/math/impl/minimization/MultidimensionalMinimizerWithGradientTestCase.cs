/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class MultidimensionalMinimizerWithGradientTestCase
	public abstract class MultidimensionalMinimizerWithGradientTestCase
	{

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingRosenbrock(final MinimizerWithGradient<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray>, com.opengamma.strata.collect.array.DoubleArray> minimzer, final double tol)
	  protected internal virtual void assertSolvingRosenbrock(MinimizerWithGradient<System.Func<DoubleArray, double>, System.Func<DoubleArray, DoubleArray>, DoubleArray> minimzer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 1.0);
		DoubleArray start = DoubleArray.of(-1.0, 1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.ROSENBROCK, MinimizationTestFunctions.ROSENBROCK_GRAD, start);
		DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.ROSENBROCK, MinimizationTestFunctions.ROSENBROCK_GRAD, start);
		assertEquals(1.0, solution.get(0), tol);
		assertEquals(1.0, solution.get(1), tol);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingRosenbrockWithoutGradient(final MinimizerWithGradient<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray>, com.opengamma.strata.collect.array.DoubleArray> minimzer, final double tol)
	  protected internal virtual void assertSolvingRosenbrockWithoutGradient(MinimizerWithGradient<System.Func<DoubleArray, double>, System.Func<DoubleArray, DoubleArray>, DoubleArray> minimzer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 1.0);
		DoubleArray start = DoubleArray.of(-1.0, 1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.ROSENBROCK, start);
		DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.ROSENBROCK, start);
		assertEquals(1.0, solution.get(0), tol);
		assertEquals(1.0, solution.get(1), tol);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingCoupledRosenbrock(final MinimizerWithGradient<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray>, com.opengamma.strata.collect.array.DoubleArray> minimzer, final double tol)
	  protected internal virtual void assertSolvingCoupledRosenbrock(MinimizerWithGradient<System.Func<DoubleArray, double>, System.Func<DoubleArray, DoubleArray>, DoubleArray> minimzer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0, 1.0);
		DoubleArray start = DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0, 1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.COUPLED_ROSENBROCK, MinimizationTestFunctions.COUPLED_ROSENBROCK_GRAD, start);
		DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.COUPLED_ROSENBROCK, MinimizationTestFunctions.COUPLED_ROSENBROCK_GRAD, start);
		for (int i = 0; i < solution.size(); i++)
		{
		  assertEquals(1.0, solution.get(i), tol);
		}
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected void assertSolvingCoupledRosenbrockWithoutGradient(final MinimizerWithGradient<java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, double>, java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray>, com.opengamma.strata.collect.array.DoubleArray> minimzer, final double tol)
	  protected internal virtual void assertSolvingCoupledRosenbrockWithoutGradient(MinimizerWithGradient<System.Func<DoubleArray, double>, System.Func<DoubleArray, DoubleArray>, DoubleArray> minimzer, double tol)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray start = com.opengamma.strata.collect.array.DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0, 1.0);
		DoubleArray start = DoubleArray.of(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0, 1.0);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.COUPLED_ROSENBROCK, start);
		DoubleArray solution = minimzer.minimize(MinimizationTestFunctions.COUPLED_ROSENBROCK, start);
		for (int i = 0; i < solution.size(); i++)
		{
		  assertEquals(1.0, solution.get(i), tol);
		}
	  }
	}

}
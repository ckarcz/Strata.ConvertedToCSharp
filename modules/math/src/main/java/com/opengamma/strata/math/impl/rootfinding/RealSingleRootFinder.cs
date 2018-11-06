/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleFunction1D = com.opengamma.strata.math.impl.function.DoubleFunction1D;

	/// <summary>
	/// Parent class for root-finders that find a single real root $x$ for a function $f(x)$.  
	/// </summary>
	//CSOFF: JavadocMethod
	public abstract class RealSingleRootFinder : SingleRootFinder<double, double>
	{
		public abstract S getRoot(System.Func<S, T> function, params S[] roots);

	  public virtual double? getRoot(System.Func<double, double> function, params Double[] startingPoints)
	  {
		ArgChecker.notNull(startingPoints, "startingPoints");
		ArgChecker.isTrue(startingPoints.Length == 2);
		return getRoot(function, startingPoints[0], startingPoints[1]);
	  }

	  public abstract double? getRoot(System.Func<double, double> function, double? x1, double? x2);

	  /// <summary>
	  /// Tests that the inputs to the root-finder are not null, and that a root is bracketed by the bounding values.
	  /// </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="x1"> The first bound, not null </param>
	  /// <param name="x2"> The second bound, not null, must be greater than x1 </param>
	  /// <exception cref="IllegalArgumentException"> if x1 and x2 do not bracket a root </exception>
	  protected internal virtual void checkInputs(System.Func<double, double> function, double? x1, double? x2)
	  {
		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(x1, "x1");
		ArgChecker.notNull(x2, "x2");
		ArgChecker.isTrue(x1 <= x2, "x1 must be less or equal to  x2");
		ArgChecker.isTrue(function(x1) * function(x2) <= 0, "x1 and x2 do not bracket a root");
	  }

	  /// <summary>
	  /// Tests that the inputs to the root-finder are not null, and that a root is bracketed by the bounding values.
	  /// </summary>
	  /// <param name="function"> The function, not null </param>
	  /// <param name="x1"> The first bound, not null </param>
	  /// <param name="x2"> The second bound, not null, must be greater than x1 </param>
	  /// <exception cref="IllegalArgumentException"> if x1 and x2 do not bracket a root </exception>
	  protected internal virtual void checkInputs(DoubleFunction1D function, double? x1, double? x2)
	  {
		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(x1, "x1");
		ArgChecker.notNull(x2, "x2");
		ArgChecker.isTrue(x1 <= x2, "x1 must be less or equal to  x2");
		ArgChecker.isTrue(function.applyAsDouble(x1) * function.applyAsDouble(x2) <= 0, "x1 and x2 do not bracket a root");
	  }

	}

}
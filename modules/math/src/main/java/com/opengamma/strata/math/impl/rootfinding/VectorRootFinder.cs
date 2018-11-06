/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using Doubles = com.google.common.primitives.Doubles;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Parent class for root-finders that calculate a root for a vector function
	/// (i.e. $\mathbf{y} = f(\mathbf{x})$, where $\mathbf{x}$ and $\mathbf{y}$ are vectors).
	/// </summary>
	public abstract class VectorRootFinder : SingleRootFinder<DoubleArray, DoubleArray>
	{
		public abstract S getRoot(System.Func<S, T> function, params S[] roots);

	  /// <summary>
	  /// {@inheritDoc}
	  /// Vector root finders only need a single starting point; if more than one is provided, the first is used and any other points ignored.
	  /// </summary>
	  public virtual DoubleArray getRoot(System.Func<DoubleArray, DoubleArray> function, params DoubleArray[] startingPoint)
	  {
		ArgChecker.notNull(startingPoint, "starting point");
		return getRoot(function, startingPoint[0]);
	  }

	  /// <param name="function"> The (vector) function, not null </param>
	  /// <param name="x0"> The starting point, not null </param>
	  /// <returns> The vector root of this function </returns>
	  public abstract DoubleArray getRoot(System.Func<DoubleArray, DoubleArray> function, DoubleArray x0);

	  /// <param name="function"> The function, not null </param>
	  /// <param name="x0"> The starting point, not null </param>
	  protected internal virtual void checkInputs(System.Func<DoubleArray, DoubleArray> function, DoubleArray x0)
	  {
		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(x0, "x0");
		int n = x0.size();
		for (int i = 0; i < n; i++)
		{
		  if (!Doubles.isFinite(x0.get(i)))
		  {
			throw new System.ArgumentException("Invalid start position x0 = " + x0.ToString());
		  }
		}
		DoubleArray y = function(x0);
		int m = y.size();
		for (int i = 0; i < m; i++)
		{
		  if (!Doubles.isFinite(y.get(i)))
		  {
			throw new System.ArgumentException("Invalid start position f(x0) = " + y.ToString());
		  }
		}
	  }

	}

}
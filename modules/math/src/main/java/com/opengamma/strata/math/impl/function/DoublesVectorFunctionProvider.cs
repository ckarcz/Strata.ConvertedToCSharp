using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;

	/// <summary>
	/// An abstraction for anything that provides a <seealso cref="VectorFunction"/> for a set of data points (as Double).
	/// </summary>
	public abstract class DoublesVectorFunctionProvider : VectorFunctionProvider<double>
	{
		public abstract VectorFunction from(T[] samplePoints);

	  public virtual VectorFunction from(IList<double> x)
	  {
		ArgChecker.notNull(x, "x");
		return from(x.ToArray());
	  }

	  public virtual VectorFunction from(double?[] x)
	  {
		ArgChecker.notNull(x, "x");
		return from(DoubleArrayMath.toPrimitive(x));
	  }

	  /// <summary>
	  /// Produces a vector function that depends in some way on the given data points.
	  /// </summary>
	  /// <param name="x">  the array of data points </param>
	  /// <returns> a <seealso cref="VectorFunction"/> </returns>
	  public abstract VectorFunction from(double[] x);

	}

}
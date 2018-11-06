/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.util
{

	using UnivariateFunction = org.apache.commons.math3.analysis.UnivariateFunction;
	using Array2DRowRealMatrix = org.apache.commons.math3.linear.Array2DRowRealMatrix;
	using ArrayRealVector = org.apache.commons.math3.linear.ArrayRealVector;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using RealVector = org.apache.commons.math3.linear.RealVector;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Utility class for converting OpenGamma mathematical objects into
	/// <a href="http://commons.apache.org/math/api-2.1/index.html">Commons</a> objects and vice versa.
	/// </summary>
	public sealed class CommonsMathWrapper
	{

	  // restricted constructor
	  private CommonsMathWrapper()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Wraps a function.
	  /// </summary>
	  /// <param name="f">  an OG 1-D function mapping doubles onto doubles </param>
	  /// <returns> a Commons univariate real function </returns>
	  public static UnivariateFunction wrapUnivariate(System.Func<double, double> f)
	  {
		ArgChecker.notNull(f, "f");
		return f.apply;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Wraps a matrix.
	  /// </summary>
	  /// <param name="x">  an OG 2-D matrix of doubles </param>
	  /// <returns> a Commons matrix </returns>
	  public static RealMatrix wrap(DoubleMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		return new Array2DRowRealMatrix(x.toArrayUnsafe());
	  }

	  /// <summary>
	  /// Wraps a matrix.
	  /// </summary>
	  /// <param name="x">  an OG 1-D vector of doubles </param>
	  /// <returns> a Commons matrix  </returns>
	  public static RealMatrix wrapAsMatrix(DoubleArray x)
	  {
		ArgChecker.notNull(x, "x");
		int n = x.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] y = new double[n][1];
		double[][] y = RectangularArrays.ReturnRectangularDoubleArray(n, 1);
		for (int i = 0; i < n; i++)
		{
		  y[i][0] = x.get(i);
		}
		return new Array2DRowRealMatrix(x.toArrayUnsafe()); // cloned in Array2DRowRealMatrix constructor
	  }

	  /// <summary>
	  /// Unwraps a matrix.
	  /// </summary>
	  /// <param name="x">  a Commons matrix </param>
	  /// <returns> an OG 2-D matrix of doubles </returns>
	  public static DoubleMatrix unwrap(RealMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		return DoubleMatrix.ofUnsafe(x.Data);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Wraps a vector.
	  /// </summary>
	  /// <param name="x">  an OG vector of doubles </param>
	  /// <returns> a Commons vector </returns>
	  public static RealVector wrap(DoubleArray x)
	  {
		ArgChecker.notNull(x, "x");
		return new ArrayRealVector(x.toArrayUnsafe()); // cloned in ArrayRealVector constructor
	  }

	  /// <summary>
	  /// Unwraps a vector.
	  /// </summary>
	  /// <param name="x">  a Commons vector </param>
	  /// <returns> an OG 1-D matrix of doubles </returns>
	  public static DoubleArray unwrap(RealVector x)
	  {
		ArgChecker.notNull(x, "x");
		return DoubleArray.ofUnsafe(x.toArray());
	  }

	}

}
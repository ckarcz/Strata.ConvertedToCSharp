/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialResult = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult;

	/// <summary>
	/// Give a struct <seealso cref="PiecewisePolynomialResult"/>, Compute value, first derivative
	/// and integral of piecewise polynomial function.
	/// </summary>
	public class PiecewisePolynomialFunction1D
	{

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public PiecewisePolynomialFunction1D()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the values of piecewise polynomial functions at xKey 
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple splines, an element in the return values corresponds to each spline  </returns>
	  public virtual DoubleArray evaluate(PiecewisePolynomialResult pp, double xKey)
	  {
		ArgChecker.notNull(pp, "pp");

		ArgChecker.isFalse(double.IsNaN(xKey), "xKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(xKey), "xKey containing Infinity");

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();
		DoubleMatrix coefMatrix = pp.CoefMatrix;

		// check for 1 less interval that knots 
		int lowerBound = FunctionUtils.getLowerBoundIndex(knots, xKey);
		int indicator = lowerBound == nKnots - 1 ? lowerBound - 1 : lowerBound;

		return DoubleArray.of(pp.Dimensions, i =>
		{
		DoubleArray coefs = coefMatrix.row(pp.Dimensions * indicator + i);
		double res = getValue(coefs, xKey, knots.get(indicator));
		ArgChecker.isFalse(double.IsInfinity(res), "Too large input");
		ArgChecker.isFalse(double.IsNaN(res), "Too large input");
		return res;
		});
	  }

	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKeys">  the key </param>
	  /// <returns> the values of piecewise polynomial functions at xKeys 
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple piecewise polynomials, a row vector of return value corresponds to each piecewise polynomial </returns>
	  public virtual DoubleMatrix evaluate(PiecewisePolynomialResult pp, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.notNull(xKeys, "xKeys");

		int keyLength = xKeys.Length;
		for (int i = 0; i < keyLength; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xKeys[i]), "xKeys containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xKeys[i]), "xKeys containing Infinity");
		}

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();
		DoubleMatrix coefMatrix = pp.CoefMatrix;
		int dim = pp.Dimensions;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[dim][keyLength];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(dim, keyLength);

		for (int k = 0; k < dim; ++k)
		{
		  for (int j = 0; j < keyLength; ++j)
		  {
			int indicator = 0;
			if (xKeys[j] < knots.get(1))
			{
			  indicator = 0;
			}
			else
			{
			  for (int i = 1; i < nKnots - 1; ++i)
			  {
				if (knots.get(i) <= xKeys[j])
				{
				  indicator = i;
				}
			  }
			}
			DoubleArray coefs = coefMatrix.row(dim * indicator + k);
			res[k][j] = getValue(coefs, xKeys[j], knots.get(indicator));
			ArgChecker.isFalse(double.IsInfinity(res[k][j]), "Too large input");
			ArgChecker.isFalse(double.IsNaN(res[k][j]), "Too large input");
		  }
		}

		return DoubleMatrix.copyOf(res);
	  }

	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKeys">  the key </param>
	  /// <returns> the values of piecewise polynomial functions at xKeys
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple piecewise polynomials, one element of return vector of DoubleMatrix
	  ///   corresponds to each piecewise polynomial </returns>
	  public virtual DoubleMatrix[] evaluate(PiecewisePolynomialResult pp, double[][] xKeys)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.notNull(xKeys, "xKeys");

		int keyLength = xKeys[0].Length;
		int keyDim = xKeys.Length;
		for (int j = 0; j < keyDim; ++j)
		{
		  for (int i = 0; i < keyLength; ++i)
		  {
			ArgChecker.isFalse(double.IsNaN(xKeys[j][i]), "xKeys containing NaN");
			ArgChecker.isFalse(double.IsInfinity(xKeys[j][i]), "xKeys containing Infinity");
		  }
		}

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();
		DoubleMatrix coefMatrix = pp.CoefMatrix;
		int dim = pp.Dimensions;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] res = new double[dim][keyDim][keyLength];
		double[][][] res = RectangularArrays.ReturnRectangularDoubleArray(dim, keyDim, keyLength);

		for (int k = 0; k < dim; ++k)
		{
		  for (int l = 0; l < keyDim; ++l)
		  {
			for (int j = 0; j < keyLength; ++j)
			{
			  int indicator = 0;
			  if (xKeys[l][j] < knots.get(1))
			  {
				indicator = 0;
			  }
			  else
			  {
				for (int i = 1; i < nKnots - 1; ++i)
				{
				  if (knots.get(i) <= xKeys[l][j])
				  {
					indicator = i;
				  }
				}
			  }

			  DoubleArray coefs = coefMatrix.row(dim * indicator + k);
			  res[k][l][j] = getValue(coefs, xKeys[l][j], knots.get(indicator));
			  ArgChecker.isFalse(double.IsInfinity(res[k][l][j]), "Too large input");
			  ArgChecker.isFalse(double.IsNaN(res[k][l][j]), "Too large input");
			}
		  }
		}

		DoubleMatrix[] resMat = new DoubleMatrix[dim];
		for (int i = 0; i < dim; ++i)
		{
		  resMat[i] = DoubleMatrix.copyOf(res[i]);
		}
		return resMat;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the first derivatives.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the first derivatives of piecewise polynomial functions at xKey 
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple piecewise polynomials, an element in the return values corresponds to each piecewise polynomial  </returns>
	  public virtual DoubleArray differentiate(PiecewisePolynomialResult pp, double xKey)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.isFalse(pp.Order < 2, "polynomial degree < 1");

		DoubleArray knots = pp.Knots;
		int nCoefs = pp.Order;
		int rowCount = pp.Dimensions * pp.NumberOfIntervals;
		int colCount = nCoefs - 1;
		DoubleMatrix coef = DoubleMatrix.of(rowCount, colCount, (i, j) => pp.CoefMatrix.get(i, j) * (nCoefs - j - 1));
		PiecewisePolynomialResult ppDiff = new PiecewisePolynomialResult(knots, coef, colCount, pp.Dimensions);
		return evaluate(ppDiff, xKey);
	  }

	  /// <summary>
	  /// Finds the first derivatives.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKeys">  the key </param>
	  /// <returns> the first derivatives of piecewise polynomial functions at xKeys 
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple piecewise polynomials, a row vector of return value corresponds to each piecewise polynomial </returns>
	  public virtual DoubleMatrix differentiate(PiecewisePolynomialResult pp, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.isFalse(pp.Order < 2, "polynomial degree < 1");

		DoubleArray knots = pp.Knots;
		int nCoefs = pp.Order;
		int rowCount = pp.Dimensions * pp.NumberOfIntervals;
		int colCount = nCoefs - 1;
		DoubleMatrix coef = DoubleMatrix.of(rowCount, colCount, (i, j) => pp.CoefMatrix.get(i, j) * (nCoefs - j - 1));
		PiecewisePolynomialResult ppDiff = new PiecewisePolynomialResult(knots, coef, colCount, pp.Dimensions);
		return evaluate(ppDiff, xKeys);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the second derivatives.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the second derivatives of piecewise polynomial functions at xKey 
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple piecewise polynomials, an element in the return values corresponds to each piecewise polynomial  </returns>
	  public virtual DoubleArray differentiateTwice(PiecewisePolynomialResult pp, double xKey)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.isFalse(pp.Order < 3, "polynomial degree < 2");

		DoubleArray knots = pp.Knots;
		int nCoefs = pp.Order;
		int rowCount = pp.Dimensions * pp.NumberOfIntervals;
		int colCount = nCoefs - 2;
		DoubleMatrix coef = DoubleMatrix.of(rowCount, colCount, (i, j) => pp.CoefMatrix.get(i, j) * (nCoefs - j - 1) * (nCoefs - j - 2));
		PiecewisePolynomialResult ppDiff = new PiecewisePolynomialResult(knots, coef, nCoefs - 1, pp.Dimensions);
		return evaluate(ppDiff, xKey);
	  }

	  /// <summary>
	  /// Finds the second derivatives.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKeys">  the key </param>
	  /// <returns> the second derivatives of piecewise polynomial functions at xKeys 
	  ///   When _dim in PiecewisePolynomialResult is greater than 1, i.e., the struct contains
	  ///   multiple piecewise polynomials, a row vector of return value corresponds to each piecewise polynomial </returns>
	  public virtual DoubleMatrix differentiateTwice(PiecewisePolynomialResult pp, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.isFalse(pp.Order < 3, "polynomial degree < 2");

		DoubleArray knots = pp.Knots;
		int nCoefs = pp.Order;
		int rowCount = pp.Dimensions * pp.NumberOfIntervals;
		int colCount = nCoefs - 2;
		DoubleMatrix coef = DoubleMatrix.of(rowCount, colCount, (i, j) => pp.CoefMatrix.get(i, j) * (nCoefs - j - 1) * (nCoefs - j - 2));
		PiecewisePolynomialResult ppDiff = new PiecewisePolynomialResult(knots, coef, nCoefs - 1, pp.Dimensions);
		return evaluate(ppDiff, xKeys);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Integration.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="initialKey">  the initial key </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the integral of piecewise polynomial between initialKey and xKey  </returns>
	  public virtual double integrate(PiecewisePolynomialResult pp, double initialKey, double xKey)
	  {
		ArgChecker.notNull(pp, "pp");

		ArgChecker.isFalse(double.IsNaN(initialKey), "initialKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(initialKey), "initialKey containing Infinity");
		ArgChecker.isTrue(pp.Dimensions == 1, "Dimension should be 1");

		DoubleArray knots = pp.Knots;
		int nCoefs = pp.Order;
		int nKnots = pp.NumberOfIntervals + 1;

		int rowCount = nKnots - 1;
		int colCount = nCoefs + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[rowCount][colCount];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(rowCount, colCount);
		for (int i = 0; i < rowCount; ++i)
		{
		  for (int j = 0; j < nCoefs; ++j)
		  {
			res[i][j] = pp.CoefMatrix.get(i, j) / (nCoefs - j);
		  }
		}

		double[] constTerms = new double[rowCount];
		int indicator = 0;
		if (initialKey <= knots.get(1))
		{
		  indicator = 0;
		}
		else
		{
		  for (int i = 1; i < rowCount; ++i)
		  {
			if (knots.get(i) < initialKey)
			{
			  indicator = i;
			}
		  }
		}

		double sum = getValue(res[indicator], initialKey, knots.get(indicator));
		for (int i = indicator; i < nKnots - 2; ++i)
		{
		  constTerms[i + 1] = constTerms[i] + getValue(res[i], knots.get(i + 1), knots.get(i)) - sum;
		  sum = 0d;
		}
		constTerms[indicator] = -getValue(res[indicator], initialKey, knots.get(indicator));
		for (int i = indicator - 1; i > -1; --i)
		{
		  constTerms[i] = constTerms[i + 1] - getValue(res[i], knots.get(i + 1), knots.get(i));
		}
		for (int i = 0; i < rowCount; ++i)
		{
		  res[i][nCoefs] = constTerms[i];
		}
		PiecewisePolynomialResult ppInt = new PiecewisePolynomialResult(pp.Knots, DoubleMatrix.copyOf(res), colCount, 1);

		return evaluate(ppInt, xKey).get(0);
	  }

	  /// <summary>
	  /// Integration.
	  /// </summary>
	  /// <param name="pp"> the PiecewisePolynomialResult </param>
	  /// <param name="initialKey">  the initial key </param>
	  /// <param name="xKeys">  the keys </param>
	  /// <returns> the integral of piecewise polynomial between initialKey and xKeys  </returns>
	  public virtual DoubleArray integrate(PiecewisePolynomialResult pp, double initialKey, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.notNull(xKeys, "xKeys");

		ArgChecker.isFalse(double.IsNaN(initialKey), "initialKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(initialKey), "initialKey containing Infinity");
		ArgChecker.isTrue(pp.Dimensions == 1, "Dimension should be 1");

		DoubleArray knots = pp.Knots;
		int nCoefs = pp.Order;
		int nKnots = pp.NumberOfIntervals + 1;

		int rowCount = nKnots - 1;
		int colCount = nCoefs + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[rowCount][colCount];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(rowCount, colCount);
		for (int i = 0; i < rowCount; ++i)
		{
		  for (int j = 0; j < nCoefs; ++j)
		  {
			res[i][j] = pp.CoefMatrix.get(i, j) / (nCoefs - j);
		  }
		}

		double[] constTerms = new double[rowCount];
		int indicator = 0;
		if (initialKey <= knots.get(1))
		{
		  indicator = 0;
		}
		else
		{
		  for (int i = 1; i < rowCount; ++i)
		  {
			if (knots.get(i) < initialKey)
			{
			  indicator = i;
			}
		  }
		}

		double sum = getValue(res[indicator], initialKey, knots.get(indicator));
		for (int i = indicator; i < nKnots - 2; ++i)
		{
		  constTerms[i + 1] = constTerms[i] + getValue(res[i], knots.get(i + 1), knots.get(i)) - sum;
		  sum = 0.0;
		}

		constTerms[indicator] = -getValue(res[indicator], initialKey, knots.get(indicator));
		for (int i = indicator - 1; i > -1; --i)
		{
		  constTerms[i] = constTerms[i + 1] - getValue(res[i], knots.get(i + 1), knots.get(i));
		}
		for (int i = 0; i < rowCount; ++i)
		{
		  res[i][nCoefs] = constTerms[i];
		}

		PiecewisePolynomialResult ppInt = new PiecewisePolynomialResult(pp.Knots, DoubleMatrix.copyOf(res), colCount, 1);

		return evaluate(ppInt, xKeys).row(0);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Evaluates the function and its first derivative.
	  /// <para>
	  /// The dimension of {@code PiecewisePolynomialResult} must be 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the value and derivative </returns>
	  public virtual ValueDerivatives evaluateAndDifferentiate(PiecewisePolynomialResult pp, double xKey)
	  {
		ArgChecker.notNull(pp, "null pp");
		ArgChecker.isFalse(double.IsNaN(xKey), "xKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(xKey), "xKey containing Infinity");

		if (pp.Dimensions > 1)
		{
		  throw new System.NotSupportedException();
		}

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();
		int interval = FunctionUtils.getLowerBoundIndex(knots, xKey);
		if (interval == nKnots - 1)
		{
		  interval--; // there is 1 less interval that knots
		}

		double s = xKey - knots.get(interval);
		DoubleArray coefs = pp.CoefMatrix.row(interval);
		int nCoefs = coefs.size();

		double resValue = coefs.get(0);
		double resDeriv = coefs.get(0) * (nCoefs - 1);
		for (int i = 1; i < nCoefs - 1; i++)
		{
		  resValue *= s;
		  resValue += coefs.get(i);
		  resDeriv *= s;
		  resDeriv += coefs.get(i) * (nCoefs - i - 1);
		  ArgChecker.isFalse(double.IsInfinity(resValue), "Too large input");
		  ArgChecker.isFalse(double.IsNaN(resValue), "Too large input");
		}
		resValue *= s;
		resValue += coefs.get(nCoefs - 1);

		return ValueDerivatives.of(resValue, DoubleArray.of(resDeriv));
	  }

	  //-------------------------------------------------------------------------
	  /// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
	  /// <param name="x">  the x-value </param>
	  /// <param name="leftknot">  the knot specifying underlying interpolation function </param>
	  /// <returns> the value of the underlying interpolation function at the value of x </returns>
	  protected internal virtual double getValue(DoubleArray coefs, double x, double leftknot)
	  {
		// needs to delegate as method is protected
		return getValue(coefs.toArrayUnsafe(), x, leftknot);
	  }

	  /// <param name="coefs">  {a_n,a_{n-1},...} of f(x) = a_n x^{n} + a_{n-1} x^{n-1} + .... </param>
	  /// <param name="x">  the x-value </param>
	  /// <param name="leftknot">  the knot specifying underlying interpolation function </param>
	  /// <returns> the value of the underlying interpolation function at the value of x </returns>
	  protected internal virtual double getValue(double[] coefs, double x, double leftknot)
	  {
		int nCoefs = coefs.Length;
		double s = x - leftknot;
		double res = coefs[0];
		for (int i = 1; i < nCoefs; i++)
		{
		  res *= s;
		  res += coefs[i];
		}
		return res;
	  }

	}

}
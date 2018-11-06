/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialResultsWithSensitivity = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResultsWithSensitivity;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Give a class <seealso cref="PiecewisePolynomialResultsWithSensitivity"/>, compute node sensitivity of
	/// function value, first derivative value and second derivative value.
	/// </summary>
	public class PiecewisePolynomialWithSensitivityFunction1D : PiecewisePolynomialFunction1D
	{

	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();

	  /// <summary>
	  /// Finds the node sensitivity.
	  /// </summary>
	  /// <param name="pp">  the <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> Node sensitivity value at x=xKey </returns>
	  public virtual DoubleArray nodeSensitivity(PiecewisePolynomialResultsWithSensitivity pp, double xKey)
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
		DoubleMatrix a = pp.getCoefficientSensitivity(interval);
		int nCoefs = a.rowCount();

		DoubleArray res = a.row(0);
		for (int i = 1; i < nCoefs; i++)
		{
		  res = (DoubleArray) MA.scale(res, s);
		  res = (DoubleArray) MA.add(res, a.row(i));
		}

		return res;
	  }

	  /// <summary>
	  /// Finds the node sensitivity.
	  /// </summary>
	  /// <param name="pp">  the <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </param>
	  /// <param name="xKeys">  the keys </param>
	  /// <returns> the node sensitivity value at x=xKeys </returns>
	  public virtual DoubleArray[] nodeSensitivity(PiecewisePolynomialResultsWithSensitivity pp, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "null pp");
		ArgChecker.notNull(xKeys, "null xKeys");
		int nKeys = xKeys.Length;
		DoubleArray[] res = new DoubleArray[nKeys];

		for (int i = 0; i < nKeys; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xKeys[i]), "xKey containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xKeys[i]), "xKey containing Infinity");
		}
		if (pp.Dimensions > 1)
		{
		  throw new System.NotSupportedException();
		}

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();

		for (int j = 0; j < nKeys; ++j)
		{
		  double xKey = xKeys[j];
		  int interval = FunctionUtils.getLowerBoundIndex(knots, xKey);
		  if (interval == nKnots - 1)
		  {
			interval--; // there is 1 less interval that knots
		  }

		  double s = xKey - knots.get(interval);
		  DoubleMatrix a = pp.getCoefficientSensitivity(interval);
		  int nCoefs = a.rowCount();

		  res[j] = a.row(0);
		  for (int i = 1; i < nCoefs; i++)
		  {
			res[j] = (DoubleArray) MA.scale(res[j], s);
			res[j] = (DoubleArray) MA.add(res[j], a.row(i));
		  }
		}

		return res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Differentiates the node sensitivity.
	  /// </summary>
	  /// <param name="pp">  the <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the node sensitivity of derivative value at x=xKey </returns>
	  public virtual DoubleArray differentiateNodeSensitivity(PiecewisePolynomialResultsWithSensitivity pp, double xKey)
	  {
		ArgChecker.notNull(pp, "null pp");
		ArgChecker.isFalse(double.IsNaN(xKey), "xKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(xKey), "xKey containing Infinity");

		if (pp.Dimensions > 1)
		{
		  throw new System.NotSupportedException();
		}
		int nCoefs = pp.Order;
		ArgChecker.isFalse(nCoefs < 2, "Polynomial degree is too low");

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();
		int interval = FunctionUtils.getLowerBoundIndex(knots, xKey);
		if (interval == nKnots - 1)
		{
		  interval--; // there is 1 less interval that knots
		}

		double s = xKey - knots.get(interval);
		DoubleMatrix a = pp.getCoefficientSensitivity(interval);

		DoubleArray res = (DoubleArray) MA.scale(a.row(0), nCoefs - 1);
		for (int i = 1; i < nCoefs - 1; i++)
		{
		  res = (DoubleArray) MA.scale(res, s);
		  res = (DoubleArray) MA.add(res, MA.scale(a.row(i), nCoefs - 1 - i));
		}

		return res;
	  }

	  /// <summary>
	  /// Differentiates the node sensitivity.
	  /// </summary>
	  /// <param name="pp">  the <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </param>
	  /// <param name="xKeys">  the keys </param>
	  /// <returns> the node sensitivity of derivative value at x=xKeys </returns>
	  public virtual DoubleArray[] differentiateNodeSensitivity(PiecewisePolynomialResultsWithSensitivity pp, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "null pp");

		if (pp.Dimensions > 1)
		{
		  throw new System.NotSupportedException();
		}
		int nCoefs = pp.Order;
		ArgChecker.isFalse(nCoefs < 2, "Polynomial degree is too low");
		int nIntervals = pp.NumberOfIntervals;

		DoubleMatrix[] diffSense = new DoubleMatrix[nIntervals];
		DoubleMatrix[] senseMat = pp.CoefficientSensitivityAll;
		int nData = senseMat[0].columnCount();
		for (int i = 0; i < nIntervals; ++i)
		{
		  double[][] senseMatArray = senseMat[i].toArray();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] tmp = new double[nCoefs - 1][nData];
		  double[][] tmp = RectangularArrays.ReturnRectangularDoubleArray(nCoefs - 1, nData);
		  for (int j = 0; j < nCoefs - 1; ++j)
		  {
			for (int k = 0; k < nData; ++k)
			{
			  tmp[j][k] = (nCoefs - 1 - j) * senseMatArray[j][k];
			}
		  }
		  diffSense[i] = DoubleMatrix.copyOf(tmp);
		}

		PiecewisePolynomialResultsWithSensitivity ppDiff = new PiecewisePolynomialResultsWithSensitivity(pp.Knots, pp.CoefMatrix, nCoefs - 1, pp.Dimensions, diffSense);
		return nodeSensitivity(ppDiff, xKeys);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Differentiates the node sensitivity.
	  /// </summary>
	  /// <param name="pp">  the <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </param>
	  /// <param name="xKey">  the key </param>
	  /// <returns> the node sensitivity of second derivative value at x=xKey </returns>
	  public virtual DoubleArray differentiateTwiceNodeSensitivity(PiecewisePolynomialResultsWithSensitivity pp, double xKey)
	  {
		ArgChecker.notNull(pp, "null pp");
		ArgChecker.isFalse(double.IsNaN(xKey), "xKey containing NaN");
		ArgChecker.isFalse(double.IsInfinity(xKey), "xKey containing Infinity");

		if (pp.Dimensions > 1)
		{
		  throw new System.NotSupportedException();
		}
		int nCoefs = pp.Order;
		ArgChecker.isFalse(nCoefs < 3, "Polynomial degree is too low");

		DoubleArray knots = pp.Knots;
		int nKnots = knots.size();
		int interval = FunctionUtils.getLowerBoundIndex(knots, xKey);
		if (interval == nKnots - 1)
		{
		  interval--; // there is 1 less interval that knots
		}

		double s = xKey - knots.get(interval);
		DoubleMatrix a = pp.getCoefficientSensitivity(interval);

		DoubleArray res = (DoubleArray) MA.scale(a.row(0), (nCoefs - 1) * (nCoefs - 2));
		for (int i = 1; i < nCoefs - 2; i++)
		{
		  res = (DoubleArray) MA.scale(res, s);
		  res = (DoubleArray) MA.add(res, MA.scale(a.row(i), (nCoefs - 1 - i) * (nCoefs - 2 - i)));
		}

		return res;
	  }

	  /// <summary>
	  /// Differentiates the node sensitivity.
	  /// </summary>
	  /// <param name="pp">  the <seealso cref="PiecewisePolynomialResultsWithSensitivity"/> </param>
	  /// <param name="xKeys">  the keys </param>
	  /// <returns> the node sensitivity of second derivative value at x=xKeys </returns>
	  public virtual DoubleArray[] differentiateTwiceNodeSensitivity(PiecewisePolynomialResultsWithSensitivity pp, double[] xKeys)
	  {
		ArgChecker.notNull(pp, "null pp");

		if (pp.Dimensions > 1)
		{
		  throw new System.NotSupportedException();
		}
		int nCoefs = pp.Order;
		ArgChecker.isFalse(nCoefs < 3, "Polynomial degree is too low");
		int nIntervals = pp.NumberOfIntervals;

		DoubleMatrix[] diffSense = new DoubleMatrix[nIntervals];
		DoubleMatrix[] senseMat = pp.CoefficientSensitivityAll;
		int nData = senseMat[0].columnCount();
		for (int i = 0; i < nIntervals; ++i)
		{
		  double[][] senseMatArray = senseMat[i].toArray();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] tmp = new double[nCoefs - 2][nData];
		  double[][] tmp = RectangularArrays.ReturnRectangularDoubleArray(nCoefs - 2, nData);
		  for (int j = 0; j < nCoefs - 2; ++j)
		  {
			for (int k = 0; k < nData; ++k)
			{
			  tmp[j][k] = (nCoefs - 1 - j) * (nCoefs - 2 - j) * senseMatArray[j][k];
			}
		  }
		  diffSense[i] = DoubleMatrix.copyOf(tmp);
		}

		PiecewisePolynomialResultsWithSensitivity ppDiff = new PiecewisePolynomialResultsWithSensitivity(pp.Knots, pp.CoefMatrix, nCoefs - 2, pp.Dimensions, diffSense);
		return nodeSensitivity(ppDiff, xKeys);
	  }
	}

}
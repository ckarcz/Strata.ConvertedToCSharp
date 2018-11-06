using System;

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
	using PiecewisePolynomialResult2D = com.opengamma.strata.math.impl.interpolation.PiecewisePolynomialResult2D;

	/// <summary>
	/// Computes value, first derivative and integral of piecewise polynomial function.
	/// </summary>
	public class PiecewisePolynomialFunction2D
	{

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public PiecewisePolynomialFunction2D()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Key">  the first key </param>
	  /// <param name="x1Key">  the second key </param>
	  /// <returns> the value of piecewise polynomial function in 2D at (x0Key, x1Key) </returns>
	  public virtual double evaluate(PiecewisePolynomialResult2D pp, double x0Key, double x1Key)
	  {
		ArgChecker.notNull(pp, "pp");

		ArgChecker.isFalse(double.IsNaN(x0Key), "x0Key containing NaN");
		ArgChecker.isFalse(double.IsInfinity(x0Key), "x0Key containing Infinity");
		ArgChecker.isFalse(double.IsNaN(x1Key), "x1Key containing NaN");
		ArgChecker.isFalse(double.IsInfinity(x1Key), "x1Key containing Infinity");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();

		int ind0 = 0;
		int ind1 = 0;

		for (int k = 1; k < nKnots0 - 1; ++k)
		{
		  if (x0Key >= knots0.get(k))
		  {
			ind0 = k;
		  }
		}

		for (int i = 1; i < nKnots1 - 1; ++i)
		{
		  if (x1Key >= knots1.get(i))
		  {
			ind1 = i;
		  }
		}
		double res = getValue(pp.Coefs[ind0][ind1], x0Key, x1Key, knots0.get(ind0), knots1.get(ind1));

		ArgChecker.isFalse(double.IsInfinity(res), "Too large input");
		ArgChecker.isFalse(double.IsNaN(res), "Too large input");

		return res;
	  }

	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Keys">  the first keys </param>
	  /// <param name="x1Keys">  the first keys </param>
	  /// <returns> the values of piecewise polynomial function in 2D at (x0Keys_i, x1Keys_j) </returns>
	  public virtual DoubleMatrix evaluate(PiecewisePolynomialResult2D pp, double[] x0Keys, double[] x1Keys)
	  {
		ArgChecker.notNull(pp, "pp");
		ArgChecker.notNull(x0Keys, "x0Keys");
		ArgChecker.notNull(x1Keys, "x1Keys");

		int n0Keys = x0Keys.Length;
		int n1Keys = x1Keys.Length;

		for (int i = 0; i < n0Keys; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(x0Keys[i]), "x0Keys containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(x0Keys[i]), "x0Keys containing Infinity");
		}
		for (int i = 0; i < n1Keys; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(x1Keys[i]), "x1Keys containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(x1Keys[i]), "x1Keys containing Infinity");
		}

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[n0Keys][n1Keys];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(n0Keys, n1Keys);

		for (int i = 0; i < n0Keys; ++i)
		{
		  for (int j = 0; j < n1Keys; ++j)
		  {
			int ind0 = 0;
			int ind1 = 0;

			for (int k = 1; k < nKnots0 - 1; ++k)
			{
			  if (x0Keys[i] >= knots0.get(k))
			  {
				ind0 = k;
			  }
			}
			for (int k = 1; k < nKnots1 - 1; ++k)
			{
			  if (x1Keys[j] >= knots1.get(k))
			  {
				ind1 = k;
			  }
			}
			res[i][j] = getValue(pp.Coefs[ind0][ind1], x0Keys[i], x1Keys[j], knots0.get(ind0), knots1.get(ind1));
			ArgChecker.isFalse(double.IsInfinity(res[i][j]), "Too large input");
			ArgChecker.isFalse(double.IsNaN(res[i][j]), "Too large input");
		  }
		}

		return DoubleMatrix.copyOf(res);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the first derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Key">  the first key </param>
	  /// <param name="x1Key">  the second key </param>
	  /// <returns> the value of first derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x0 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual double differentiateX0(PiecewisePolynomialResult2D pp, double x0Key, double x1Key)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order0 < 2, "polynomial degree of x0 < 1");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0 - 1, order1, (k, l) => coef.get(k, l) * (order0 - k - 1));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0 - 1, order1});

		return evaluate(ppDiff, x0Key, x1Key);
	  }

	  /// <summary>
	  /// Finds the first derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Key">  the first key </param>
	  /// <param name="x1Key">  the second key </param>
	  /// <returns> the value of first derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x1 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual double differentiateX1(PiecewisePolynomialResult2D pp, double x0Key, double x1Key)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order1 < 2, "polynomial degree of x1 < 1");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0, order1 - 1, (k, l) => coef.get(k, l) * (order1 - l - 1));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0, order1 - 1});

		return evaluate(ppDiff, x0Key, x1Key);
	  }

	  /// <summary>
	  /// Finds the first derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Keys">  the first keys </param>
	  /// <param name="x1Keys">  the second keys </param>
	  /// <returns> Values of first derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x0 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual DoubleMatrix differentiateX0(PiecewisePolynomialResult2D pp, double[] x0Keys, double[] x1Keys)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order0 < 2, "polynomial degree of x0 < 1");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0 - 1, order1, (k, l) => coef.get(k, l) * (order0 - k - 1));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0 - 1, order1});

		return evaluate(ppDiff, x0Keys, x1Keys);
	  }

	  /// <summary>
	  /// Finds the first derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Keys">  the first keys </param>
	  /// <param name="x1Keys">  the second keys </param>
	  /// <returns> Values of first derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x1 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual DoubleMatrix differentiateX1(PiecewisePolynomialResult2D pp, double[] x0Keys, double[] x1Keys)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order1 < 2, "polynomial degree of x1 < 1");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0, order1 - 1, (k, l) => coef.get(k, l) * (order1 - l - 1));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0, order1 - 1});

		return evaluate(ppDiff, x0Keys, x1Keys);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the cross derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Key">  the first key </param>
	  /// <param name="x1Key">  the second key </param>
	  /// <returns> the value of cross derivative of two-dimensional piecewise polynomial function at (x0Keys_i, x1Keys_j) </returns>
	  public virtual double differentiateCross(PiecewisePolynomialResult2D pp, double x0Key, double x1Key)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order0 < 2, "polynomial degree of x0 < 1");
		ArgChecker.isFalse(order1 < 2, "polynomial degree of x1 < 1");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0 - 1, order1 - 1, (k, l) => coef.get(k, l) * (order1 - l - 1) * (order0 - k - 1));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0 - 1, order1 - 1});

		return evaluate(ppDiff, x0Key, x1Key);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the second derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Key">  the first key </param>
	  /// <param name="x1Key">  the second key </param>
	  /// <returns> the value of second derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x0 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual double differentiateTwiceX0(PiecewisePolynomialResult2D pp, double x0Key, double x1Key)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order0 < 3, "polynomial degree of x0 < 2");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0 - 2, order1, (k, l) => coef.get(k, l) * (order0 - k - 1) * (order0 - k - 2));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0 - 2, order1});

		return evaluate(ppDiff, x0Key, x1Key);
	  }

	  /// <summary>
	  /// Finds the second derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Key">  the first key </param>
	  /// <param name="x1Key">  the second key </param>
	  /// <returns> the value of second derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x1 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual double differentiateTwiceX1(PiecewisePolynomialResult2D pp, double x0Key, double x1Key)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order1 < 3, "polynomial degree of x1 < 2");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0, order1 - 2, (k, l) => coef.get(k, l) * (order1 - l - 1) * (order1 - l - 2));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0, order1 - 2});

		return evaluate(ppDiff, x0Key, x1Key);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the cross derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Keys">  the first keys </param>
	  /// <param name="x1Keys">  the second keys </param>
	  /// <returns> the values of cross derivative of two-dimensional piecewise polynomial function at (x0Keys_i, x1Keys_j) </returns>
	  public virtual DoubleMatrix differentiateCross(PiecewisePolynomialResult2D pp, double[] x0Keys, double[] x1Keys)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order0 < 2, "polynomial degree of x0 < 1");
		ArgChecker.isFalse(order1 < 2, "polynomial degree of x1 < 1");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);

		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0 - 1, order1 - 1, (k, l) => coef.get(k, l) * (order1 - l - 1) * (order0 - k - 1));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0 - 1, order1 - 1});

		return evaluate(ppDiff, x0Keys, x1Keys);
	  }

	  /// <summary>
	  /// Finds the second derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Keys">  the first keys </param>
	  /// <param name="x1Keys">  the second keys </param>
	  /// <returns> the values of second derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x0 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual DoubleMatrix differentiateTwiceX0(PiecewisePolynomialResult2D pp, double[] x0Keys, double[] x1Keys)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order0 < 3, "polynomial degree of x0 < 2");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);
		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0 - 2, order1, (k, l) => coef.get(k, l) * (order0 - k - 1) * (order0 - k - 2));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0 - 2, order1});

		return evaluate(ppDiff, x0Keys, x1Keys);
	  }

	  /// <summary>
	  /// Finds the second derivative.
	  /// </summary>
	  /// <param name="pp">  the PiecewisePolynomialResult2D </param>
	  /// <param name="x0Keys">  the first keys </param>
	  /// <param name="x1Keys">  the second keys </param>
	  /// <returns> the values of second derivative of two-dimensional piecewise polynomial function
	  ///   with respect to x1 at (x0Keys_i, x1Keys_j) </returns>
	  public virtual DoubleMatrix differentiateTwiceX1(PiecewisePolynomialResult2D pp, double[] x0Keys, double[] x1Keys)
	  {
		ArgChecker.notNull(pp, "pp");
		int order0 = pp.Order[0];
		int order1 = pp.Order[1];
		ArgChecker.isFalse(order1 < 3, "polynomial degree of x1 < 2");

		DoubleArray knots0 = pp.Knots0;
		DoubleArray knots1 = pp.Knots1;
		int nKnots0 = knots0.size();
		int nKnots1 = knots1.size();
		DoubleMatrix[][] coefs = pp.Coefs;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] res = new DoubleMatrix[nKnots0][nKnots1];
		DoubleMatrix[][] res = RectangularArrays.ReturnRectangularDoubleMatrixArray(nKnots0, nKnots1);
		for (int i = 0; i < nKnots0 - 1; ++i)
		{
		  for (int j = 0; j < nKnots1 - 1; ++j)
		  {
			DoubleMatrix coef = coefs[i][j];
			res[i][j] = DoubleMatrix.of(order0, order1 - 2, (k, l) => coef.get(k, l) * (order1 - l - 1) * (order1 - l - 2));
		  }
		}

		PiecewisePolynomialResult2D ppDiff = new PiecewisePolynomialResult2D(knots0, knots1, res, new int[] {order0, order1 - 2});

		return evaluate(ppDiff, x0Keys, x1Keys);
	  }

	  // sum_{i=0}^{order0-1} sum_{j=0}^{order1-1} coefMat_{ij} (x0-leftKnots0)^{order0-1-i} (x1-leftKnots1)^{order0-1-j}
	  private double getValue(DoubleMatrix coefMat, double x0, double x1, double leftKnot0, double leftKnot1)
	  {

		int order0 = coefMat.rowCount();
		int order1 = coefMat.columnCount();
		double x0Mod = x0 - leftKnot0;
		double x1Mod = x1 - leftKnot1;
		double res = 0.0;

		for (int i = 0; i < order0; ++i)
		{
		  for (int j = 0; j < order1; ++j)
		  {
			res += coefMat.get(order0 - i - 1, order1 - j - 1) * Math.Pow(x0Mod, i) * Math.Pow(x1Mod, j);
		  }
		}
		return res;
	  }

	}

}
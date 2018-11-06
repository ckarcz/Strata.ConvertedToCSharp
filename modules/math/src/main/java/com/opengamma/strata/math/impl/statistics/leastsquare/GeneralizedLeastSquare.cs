using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.apache.commons.math3.util.CombinatoricsUtils.binomialCoefficient;


	using Lists = com.google.common.collect.Lists;
	using Doubles = com.google.common.primitives.Doubles;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using SVDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.SVDecompositionCommons;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Generalized least square method.
	/// </summary>
	public class GeneralizedLeastSquare
	{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.math.linearalgebra.Decomposition<?> _decomposition;
	  private readonly Decomposition<object> _decomposition;
	  private readonly MatrixAlgebra _algebra;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public GeneralizedLeastSquare()
	  {
		_decomposition = new SVDecompositionCommons();
		_algebra = new CommonsMatrixAlgebra();
	  }

	  /// 
	  /// @param <T> The type of the independent variables (e.g. Double, double[], DoubleArray etc) </param>
	  /// <param name="x"> independent variables </param>
	  /// <param name="y"> dependent (scalar) variables </param>
	  /// <param name="sigma"> (Gaussian) measurement error on dependent variables </param>
	  /// <param name="basisFunctions"> set of basis functions - the fitting function is formed by these basis functions times a set of weights </param>
	  /// <returns> the results of the least square </returns>
	  public virtual GeneralizedLeastSquareResults<T> solve<T>(T[] x, double[] y, double[] sigma, IList<System.Func<T, double>> basisFunctions)
	  {
		return solve(x, y, sigma, basisFunctions, 0.0, 0);
	  }

	  /// <summary>
	  /// Generalised least square with penalty on (higher-order) finite differences of weights. </summary>
	  /// @param <T> The type of the independent variables (e.g. Double, double[], DoubleArray etc) </param>
	  /// <param name="x"> independent variables </param>
	  /// <param name="y"> dependent (scalar) variables </param>
	  /// <param name="sigma"> (Gaussian) measurement error on dependent variables </param>
	  /// <param name="basisFunctions"> set of basis functions - the fitting function is formed by these basis functions times a set of weights </param>
	  /// <param name="lambda"> strength of penalty function </param>
	  /// <param name="differenceOrder"> difference order between weights used in penalty function </param>
	  /// <returns> the results of the least square </returns>
	  public virtual GeneralizedLeastSquareResults<T> solve<T>(T[] x, double[] y, double[] sigma, IList<System.Func<T, double>> basisFunctions, double lambda, int differenceOrder)
	  {
		ArgChecker.notNull(x, "x null");
		ArgChecker.notNull(y, "y null");
		ArgChecker.notNull(sigma, "sigma null");
		ArgChecker.notEmpty(basisFunctions, "empty basisFunctions");
		int n = x.Length;
		ArgChecker.isTrue(n > 0, "no data");
		ArgChecker.isTrue(y.Length == n, "y wrong length");
		ArgChecker.isTrue(sigma.Length == n, "sigma wrong length");

		ArgChecker.isTrue(lambda >= 0.0, "negative lambda");
		ArgChecker.isTrue(differenceOrder >= 0, "difference order");

		IList<T> lx = Lists.newArrayList(x);
		IList<double> ly = Lists.newArrayList(Doubles.asList(y));
		IList<double> lsigma = Lists.newArrayList(Doubles.asList(sigma));

		return solveImp(lx, ly, lsigma, basisFunctions, lambda, differenceOrder);
	  }

	  internal virtual GeneralizedLeastSquareResults<double> solve(double[] x, double[] y, double[] sigma, IList<System.Func<double, double>> basisFunctions, double lambda, int differenceOrder)
	  {
		return solve(DoubleArrayMath.toObject(x), y, sigma, basisFunctions, lambda, differenceOrder);
	  }

	  /// 
	  /// @param <T> The type of the independent variables (e.g. Double, double[], DoubleArray etc) </param>
	  /// <param name="x"> independent variables </param>
	  /// <param name="y"> dependent (scalar) variables </param>
	  /// <param name="sigma"> (Gaussian) measurement error on dependent variables </param>
	  /// <param name="basisFunctions"> set of basis functions - the fitting function is formed by these basis functions times a set of weights </param>
	  /// <returns> the results of the least square </returns>
	  public virtual GeneralizedLeastSquareResults<T> solve<T>(IList<T> x, IList<double> y, IList<double> sigma, IList<System.Func<T, double>> basisFunctions)
	  {
		return solve(x, y, sigma, basisFunctions, 0.0, 0);
	  }

	  /// <summary>
	  /// Generalised least square with penalty on (higher-order) finite differences of weights. </summary>
	  /// @param <T> The type of the independent variables (e.g. Double, double[], DoubleArray etc) </param>
	  /// <param name="x"> independent variables </param>
	  /// <param name="y"> dependent (scalar) variables </param>
	  /// <param name="sigma"> (Gaussian) measurement error on dependent variables </param>
	  /// <param name="basisFunctions"> set of basis functions - the fitting function is formed by these basis functions times a set of weights </param>
	  /// <param name="lambda"> strength of penalty function </param>
	  /// <param name="differenceOrder"> difference order between weights used in penalty function </param>
	  /// <returns> the results of the least square </returns>
	  public virtual GeneralizedLeastSquareResults<T> solve<T>(IList<T> x, IList<double> y, IList<double> sigma, IList<System.Func<T, double>> basisFunctions, double lambda, int differenceOrder)
	  {
		ArgChecker.notEmpty(x, "empty measurement points");
		ArgChecker.notEmpty(y, "empty measurement values");
		ArgChecker.notEmpty(sigma, "empty measurement errors");
		ArgChecker.notEmpty(basisFunctions, "empty basisFunctions");
		int n = x.Count;
		ArgChecker.isTrue(n > 0, "no data");
		ArgChecker.isTrue(y.Count == n, "y wrong length");
		ArgChecker.isTrue(sigma.Count == n, "sigma wrong length");

		ArgChecker.isTrue(lambda >= 0.0, "negative lambda");
		ArgChecker.isTrue(differenceOrder >= 0, "difference order");

		return solveImp(x, y, sigma, basisFunctions, lambda, differenceOrder);
	  }

	  /// <summary>
	  /// Specialist method used mainly for solving multidimensional P-spline problems where the basis functions (B-splines) span a N-dimension space, and the weights sit on an N-dimension
	  ///  grid and are treated as a N-order tensor rather than a vector, so k-order differencing is done for each tensor index while varying the other indices. </summary>
	  /// @param <T> The type of the independent variables (e.g. Double, double[], DoubleArray etc) </param>
	  /// <param name="x"> independent variables </param>
	  /// <param name="y"> dependent (scalar) variables </param>
	  /// <param name="sigma"> (Gaussian) measurement error on dependent variables </param>
	  /// <param name="basisFunctions"> set of basis functions - the fitting function is formed by these basis functions times a set of weights </param>
	  /// <param name="sizes"> The size the weights tensor in each dimension (the product of this must equal the number of basis functions) </param>
	  /// <param name="lambda"> strength of penalty function in each dimension </param>
	  /// <param name="differenceOrder"> difference order between weights used in penalty function for each dimension </param>
	  /// <returns> the results of the least square </returns>
	  public virtual GeneralizedLeastSquareResults<T> solve<T>(IList<T> x, IList<double> y, IList<double> sigma, IList<System.Func<T, double>> basisFunctions, int[] sizes, double[] lambda, int[] differenceOrder)
	  {
		ArgChecker.notEmpty(x, "empty measurement points");
		ArgChecker.notEmpty(y, "empty measurement values");
		ArgChecker.notEmpty(sigma, "empty measurement errors");
		ArgChecker.notEmpty(basisFunctions, "empty basisFunctions");
		int n = x.Count;
		ArgChecker.isTrue(n > 0, "no data");
		ArgChecker.isTrue(y.Count == n, "y wrong length");
		ArgChecker.isTrue(sigma.Count == n, "sigma wrong length");

		int dim = sizes.Length;
		ArgChecker.isTrue(dim == lambda.Length, "number of penalty functions {} must be equal to number of directions {}", lambda.Length, dim);
		ArgChecker.isTrue(dim == differenceOrder.Length, "number of difference order {} must be equal to number of directions {}", differenceOrder.Length, dim);

		for (int i = 0; i < dim; i++)
		{
		  ArgChecker.isTrue(sizes[i] > 0, "sizes must be >= 1");
		  ArgChecker.isTrue(lambda[i] >= 0.0, "negative lambda");
		  ArgChecker.isTrue(differenceOrder[i] >= 0, "difference order");
		}
		return solveImp(x, y, sigma, basisFunctions, sizes, lambda, differenceOrder);
	  }

	  private GeneralizedLeastSquareResults<T> solveImp<T>(IList<T> x, IList<double> y, IList<double> sigma, IList<System.Func<T, double>> basisFunctions, double lambda, int differenceOrder)
	  {

		int n = x.Count;

		int m = basisFunctions.Count;

		double[] b = new double[m];

		double[] invSigmaSqr = new double[n];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] f = new double[m][n];
		double[][] f = RectangularArrays.ReturnRectangularDoubleArray(m, n);
		int i, j, k;

		for (i = 0; i < n; i++)
		{
		  double temp = sigma[i];
		  ArgChecker.isTrue(temp > 0, "sigma must be greater than zero");
		  invSigmaSqr[i] = 1.0 / temp / temp;
		}

		for (i = 0; i < m; i++)
		{
		  for (j = 0; j < n; j++)
		  {
			f[i][j] = basisFunctions[i](x[j]);
		  }
		}

		double sum;
		for (i = 0; i < m; i++)
		{
		  sum = 0;
		  for (k = 0; k < n; k++)
		  {
			sum += y[k] * f[i][k] * invSigmaSqr[k];
		  }
		  b[i] = sum;

		}

		DoubleArray mb = DoubleArray.copyOf(b);
		DoubleMatrix ma = getAMatrix(f, invSigmaSqr);

		if (lambda > 0.0)
		{
		  DoubleMatrix d = getDiffMatrix(m, differenceOrder);
		  ma = (DoubleMatrix) _algebra.add(ma, _algebra.scale(d, lambda));
		}

		DecompositionResult decmp = _decomposition.apply(ma);
		DoubleArray w = decmp.solve(mb);
		DoubleMatrix covar = decmp.solve(DoubleMatrix.identity(m));

		double chiSq = 0;
		for (i = 0; i < n; i++)
		{
		  double temp = 0;
		  for (k = 0; k < m; k++)
		  {
			temp += w.get(k) * f[k][i];
		  }
		  chiSq += FunctionUtils.square(y[i] - temp) * invSigmaSqr[i];
		}

		return new GeneralizedLeastSquareResults<T>(basisFunctions, chiSq, w, covar);
	  }

	  private GeneralizedLeastSquareResults<T> solveImp<T>(IList<T> x, IList<double> y, IList<double> sigma, IList<System.Func<T, double>> basisFunctions, int[] sizes, double[] lambda, int[] differenceOrder)
	  {

		int dim = sizes.Length;

		int n = x.Count;

		int m = basisFunctions.Count;

		double[] b = new double[m];

		double[] invSigmaSqr = new double[n];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] f = new double[m][n];
		double[][] f = RectangularArrays.ReturnRectangularDoubleArray(m, n);
		int i, j, k;

		for (i = 0; i < n; i++)
		{
		  double temp = sigma[i];
		  ArgChecker.isTrue(temp > 0, "sigma must be great than zero");
		  invSigmaSqr[i] = 1.0 / temp / temp;
		}

		for (i = 0; i < m; i++)
		{
		  for (j = 0; j < n; j++)
		  {
			f[i][j] = basisFunctions[i](x[j]);
		  }
		}

		double sum;
		for (i = 0; i < m; i++)
		{
		  sum = 0;
		  for (k = 0; k < n; k++)
		  {
			sum += y[k] * f[i][k] * invSigmaSqr[k];
		  }
		  b[i] = sum;

		}

		DoubleArray mb = DoubleArray.copyOf(b);
		DoubleMatrix ma = getAMatrix(f, invSigmaSqr);

		for (i = 0; i < dim; i++)
		{
		  if (lambda[i] > 0.0)
		  {
			DoubleMatrix d = getDiffMatrix(sizes, differenceOrder[i], i);
			ma = (DoubleMatrix) _algebra.add(ma, _algebra.scale(d, lambda[i]));
		  }
		}

		DecompositionResult decmp = _decomposition.apply(ma);
		DoubleArray w = decmp.solve(mb);
		DoubleMatrix covar = decmp.solve(DoubleMatrix.identity(m));

		double chiSq = 0;
		for (i = 0; i < n; i++)
		{
		  double temp = 0;
		  for (k = 0; k < m; k++)
		  {
			temp += w.get(k) * f[k][i];
		  }
		  chiSq += FunctionUtils.square(y[i] - temp) * invSigmaSqr[i];
		}

		return new GeneralizedLeastSquareResults<T>(basisFunctions, chiSq, w, covar);
	  }

	  private DoubleMatrix getAMatrix(double[][] funcMatrix, double[] invSigmaSqr)
	  {
		int m = funcMatrix.Length;
		int n = funcMatrix[0].Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] a = new double[m][m];
		double[][] a = RectangularArrays.ReturnRectangularDoubleArray(m, m);
		for (int i = 0; i < m; i++)
		{
		  double sum = 0;
		  for (int k = 0; k < n; k++)
		  {
			sum += FunctionUtils.square(funcMatrix[i][k]) * invSigmaSqr[k];
		  }
		  a[i][i] = sum;
		  for (int j = i + 1; j < m; j++)
		  {
			sum = 0;
			for (int k = 0; k < n; k++)
			{
			  sum += funcMatrix[i][k] * funcMatrix[j][k] * invSigmaSqr[k];
			}
			a[i][j] = sum;
			a[j][i] = sum;
		  }
		}

		return DoubleMatrix.copyOf(a);
	  }

	  private DoubleMatrix getDiffMatrix(int m, int k)
	  {
		ArgChecker.isTrue(k < m, "difference order too high");

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] data = new double[m][m];
		double[][] data = RectangularArrays.ReturnRectangularDoubleArray(m, m);
		if (m == 0)
		{
		  return DoubleMatrix.copyOf(data);
		}

		int[] coeff = new int[k + 1];

		int sign = 1;
		for (int i = k; i >= 0; i--)
		{
		  coeff[i] = (int)(sign * binomialCoefficient(k, i));
		  sign *= -1;
		}

		for (int i = k; i < m; i++)
		{
		  for (int j = 0; j < k + 1; j++)
		  {
			data[i][j + i - k] = coeff[j];
		  }
		}
		DoubleMatrix d = DoubleMatrix.copyOf(data);

		DoubleMatrix dt = _algebra.getTranspose(d);
		return (DoubleMatrix) _algebra.multiply(dt, d);
	  }

	  private DoubleMatrix getDiffMatrix(int[] size, int k, int indices)
	  {
		int dim = size.Length;

		DoubleMatrix d = getDiffMatrix(size[indices], k);

		int preProduct = 1;
		int postProduct = 1;
		for (int j = indices + 1; j < dim; j++)
		{
		  preProduct *= size[j];
		}
		for (int j = 0; j < indices; j++)
		{
		  postProduct *= size[j];
		}
		DoubleMatrix temp = d;
		if (preProduct != 1)
		{
		  temp = (DoubleMatrix) _algebra.kroneckerProduct(DoubleMatrix.identity(preProduct), temp);
		}
		if (postProduct != 1)
		{
		  temp = (DoubleMatrix) _algebra.kroneckerProduct(temp, DoubleMatrix.identity(postProduct));
		}

		return temp;
	  }

	}

}
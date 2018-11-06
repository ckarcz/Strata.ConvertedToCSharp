using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using DoubleFunction1D = com.opengamma.strata.math.impl.function.DoubleFunction1D;
	using OrthonormalHermitePolynomialFunction = com.opengamma.strata.math.impl.function.special.OrthonormalHermitePolynomialFunction;
	using NewtonRaphsonSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.NewtonRaphsonSingleRootFinder;

	/// <summary>
	/// Class that generates weights and abscissas for Gauss-Hermite quadrature.
	/// Orthonormal Hermite polynomials $H_N$ are used to generate the weights (see
	/// <seealso cref="OrthonormalHermitePolynomialFunction"/>)
	/// using the formula:
	/// $$
	/// \begin{align*}
	/// w_i = \frac{2}{(H_n'(x_i))^2}
	/// \end{align*}
	/// $$
	/// where $x_i$ is the $i^{th}$ root of the orthogonal polynomial and $H_i'$ is
	/// the first derivative of the $i^{th}$ polynomial.
	/// </summary>
	public class GaussHermiteWeightAndAbscissaFunction : QuadratureWeightAndAbscissaFunction
	{

	  /// <summary>
	  /// Weight generator </summary>
	  private static readonly OrthonormalHermitePolynomialFunction HERMITE = new OrthonormalHermitePolynomialFunction();
	  /// <summary>
	  /// The root-finder </summary>
	  private static readonly NewtonRaphsonSingleRootFinder ROOT_FINDER = new NewtonRaphsonSingleRootFinder(1e-12);

	  public virtual GaussianQuadratureData generate(int n)
	  {
		ArgChecker.isTrue(n > 0);
		double[] x = new double[n];
		double[] w = new double[n];
		bool odd = n % 2 != 0;
		int m = (n + 1) / 2 - (odd ? 1 : 0);
		Pair<DoubleFunction1D, DoubleFunction1D>[] polynomials = HERMITE.getPolynomialsAndFirstDerivative(n);
		Pair<DoubleFunction1D, DoubleFunction1D> pair = polynomials[n];
		DoubleFunction1D function = pair.First;
		DoubleFunction1D derivative = pair.Second;
		double root = 0;

		for (int i = 0; i < m; i++)
		{
		  root = getInitialRootGuess(root, i, n, x);
		  root = ROOT_FINDER.getRoot(function, derivative, root).Value;
		  double dp = derivative.applyAsDouble(root);
		  x[i] = -root;
		  x[n - 1 - i] = root;
		  w[i] = 2.0 / (dp * dp);
		  w[n - 1 - i] = w[i];
		}
		if (odd)
		{
		  double dp = derivative.applyAsDouble(0.0);
		  w[m] = 2.0 / dp / dp;
		}
		return new GaussianQuadratureData(x, w);
	  }

	  private double getInitialRootGuess(double previousRoot, int i, int n, double[] x)
	  {
		if (i == 0)
		{
		  return Math.Sqrt(2 * n + 1) - 1.85575 * Math.Pow(2 * n + 1, -1.0 / 6);
		}
		if (i == 1)
		{
		  return previousRoot - 1.14 * Math.Pow(n, 0.426) / previousRoot;
		}
		if (i == 2)
		{
		  return 1.86 * previousRoot + 0.86 * x[0];
		}
		if (i == 3)
		{
		  return 1.91 * previousRoot + 0.91 * x[1];
		}
		return 2 * previousRoot + x[i - 2];
	  }

	}

}
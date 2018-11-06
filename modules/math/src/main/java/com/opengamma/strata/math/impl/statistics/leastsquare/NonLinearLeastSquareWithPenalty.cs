using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using DecompositionFactory = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using MatrixAlgebraFactory = com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Modification to NonLinearLeastSquare to use a penalty function add to the normal chi^2 term of the form $a^TPa$ where
	/// $a$ is the vector of model parameters sort and P is some matrix. The idea is to extend the p-spline concept to
	/// non-linear models of the form $\hat{y}_j = H\left(\sum_{i=0}^{M-1} w_i b_i (x_j)\right)$ where $H(\cdot)$ is
	/// some non-linear function, $b_i(\cdot)$ are a set of basis functions and $w_i$ are the weights (to be found). As with
	/// (linear) p-splines, smoothness of the function is obtained by having a penalty on the nth order difference of the
	/// weights. The modified chi-squared is written as
	/// $\chi^2 = \sum_{i=0}^{N-1} \left(\frac{y_i-H\left(\sum_{k=0}^{M-1} w_k b_k (x_i)\right)}{\sigma_i} \right)^2 +
	/// \sum_{i,j=0}^{M-1}P_{i,j}x_ix_j$
	/// </summary>
	public class NonLinearLeastSquareWithPenalty
	{

	  private const int MAX_ATTEMPTS = 100000;

	  // Review should we use Cholesky as default
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.math.linearalgebra.Decomposition<?> DEFAULT_DECOMP = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory.SV_COMMONS;
	  private static readonly Decomposition<object> DEFAULT_DECOMP = DecompositionFactory.SV_COMMONS;
	  private static readonly OGMatrixAlgebra MA = new OGMatrixAlgebra();
	  private const double EPS = 1e-8; // Default convergence tolerance on the relative change in chi2

	  /// <summary>
	  /// Unconstrained allowed function - always returns true
	  /// </summary>
	  public static readonly System.Func<DoubleArray, bool> UNCONSTRAINED = (DoubleArray x) =>
	  {
  return true;
	  };

	  private readonly double _eps;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.math.linearalgebra.Decomposition<?> _decomposition;
	  private readonly Decomposition<object> _decomposition;
	  private readonly MatrixAlgebra _algebra;

	  /// <summary>
	  /// Default constructor. This uses SVD, <seealso cref="OGMatrixAlgebra"/> and a convergence tolerance of 1e-8
	  /// </summary>
	  public NonLinearLeastSquareWithPenalty() : this(DEFAULT_DECOMP, MA, EPS)
	  {
	  }

	  /// <summary>
	  /// Constructor allowing matrix decomposition to be set.
	  /// This uses <seealso cref="OGMatrixAlgebra"/> and a convergence tolerance of 1e-8.
	  /// </summary>
	  /// <param name="decomposition"> Matrix decomposition (see <seealso cref="DecompositionFactory"/> for list) </param>
	  public NonLinearLeastSquareWithPenalty<T1>(Decomposition<T1> decomposition) : this(decomposition, MA, EPS)
	  {
	  }

	  /// <summary>
	  /// Constructor allowing convergence tolerance to be set.
	  /// This uses SVD and <seealso cref="OGMatrixAlgebra"/>.
	  /// </summary>
	  /// <param name="eps"> Convergence tolerance </param>
	  public NonLinearLeastSquareWithPenalty(double eps) : this(DEFAULT_DECOMP, MA, eps)
	  {
	  }

	  /// <summary>
	  /// Constructor allowing matrix decomposition and convergence tolerance to be set.
	  /// This uses <seealso cref="OGMatrixAlgebra"/>.
	  /// </summary>
	  /// <param name="decomposition"> Matrix decomposition (see <seealso cref="DecompositionFactory"/> for list) </param>
	  /// <param name="eps"> Convergence tolerance </param>
	  public NonLinearLeastSquareWithPenalty<T1>(Decomposition<T1> decomposition, double eps) : this(decomposition, MA, eps)
	  {
	  }

	  /// <summary>
	  /// General constructor.
	  /// </summary>
	  /// <param name="decomposition"> Matrix decomposition (see <seealso cref="DecompositionFactory"/> for list) </param>
	  /// <param name="algebra">  The matrix algebra (see <seealso cref="MatrixAlgebraFactory"/> for list) </param>
	  /// <param name="eps"> Convergence tolerance </param>
	  public NonLinearLeastSquareWithPenalty<T1>(Decomposition<T1> decomposition, MatrixAlgebra algebra, double eps)
	  {
		ArgChecker.notNull(decomposition, "decomposition");
		ArgChecker.notNull(algebra, "algebra");
		ArgChecker.isTrue(eps > 0, "must have positive eps");
		_decomposition = decomposition;
		_algebra = algebra;
		_eps = eps;
	  }

	  /// <summary>
	  /// Use this when the model is given as a function of its parameters only (i.e. a function that takes a set of
	  /// parameters and return a set of model values,
	  /// so the measurement points are already known to the function), and analytic parameter sensitivity is not available.
	  /// </summary>
	  /// <param name="observedValues"> Set of measurement values </param>
	  /// <param name="func"> The model as a function of its parameters only </param>
	  /// <param name="startPos"> Initial value of the parameters </param>
	  /// <param name="penalty"> Penalty matrix </param>
	  /// <returns> value of the fitted parameters </returns>
	  public virtual LeastSquareWithPenaltyResults solve(DoubleArray observedValues, System.Func<DoubleArray, DoubleArray> func, DoubleArray startPos, DoubleMatrix penalty)
	  {

		int n = observedValues.size();
		VectorFieldFirstOrderDifferentiator jac = new VectorFieldFirstOrderDifferentiator();
		return solve(observedValues, DoubleArray.filled(n, 1.0), func, jac.differentiate(func), startPos, penalty);
	  }

	  /// <summary>
	  /// Use this when the model is given as a function of its parameters only (i.e. a function that takes a set of
	  /// parameters and return a set of model values,
	  /// so the measurement points are already known to the function), and analytic parameter sensitivity is not available </summary>
	  /// <param name="observedValues"> Set of measurement values </param>
	  /// <param name="sigma"> Set of measurement errors </param>
	  /// <param name="func"> The model as a function of its parameters only </param>
	  /// <param name="startPos"> Initial value of the parameters </param>
	  /// <param name="penalty"> Penalty matrix </param>
	  /// <returns> value of the fitted parameters </returns>
	  public virtual LeastSquareWithPenaltyResults solve(DoubleArray observedValues, DoubleArray sigma, System.Func<DoubleArray, DoubleArray> func, DoubleArray startPos, DoubleMatrix penalty)
	  {

		VectorFieldFirstOrderDifferentiator jac = new VectorFieldFirstOrderDifferentiator();
		return solve(observedValues, sigma, func, jac.differentiate(func), startPos, penalty);
	  }

	  /// <summary>
	  /// Use this when the model is given as a function of its parameters only (i.e. a function that takes a set of
	  /// parameters and return a set of model values,
	  /// so the measurement points are already known to the function), and analytic parameter sensitivity is not available </summary>
	  /// <param name="observedValues"> Set of measurement values </param>
	  /// <param name="sigma"> Set of measurement errors </param>
	  /// <param name="func"> The model as a function of its parameters only </param>
	  /// <param name="startPos"> Initial value of the parameters </param>
	  /// <param name="penalty"> Penalty matrix </param>
	  /// <param name="allowedValue"> a function which returned true if the new trial position is allowed by the model. An example
	  ///   would be to enforce positive parameters
	  ///   without resorting to a non-linear parameter transform. In some circumstances this approach will lead to slow
	  ///   convergence. </param>
	  /// <returns> value of the fitted parameters </returns>
	  public virtual LeastSquareWithPenaltyResults solve(DoubleArray observedValues, DoubleArray sigma, System.Func<DoubleArray, DoubleArray> func, DoubleArray startPos, DoubleMatrix penalty, System.Func<DoubleArray, bool> allowedValue)
	  {

		VectorFieldFirstOrderDifferentiator jac = new VectorFieldFirstOrderDifferentiator();
		return solve(observedValues, sigma, func, jac.differentiate(func), startPos, penalty, allowedValue);
	  }

	  /// <summary>
	  /// Use this when the model is given as a function of its parameters only (i.e. a function that takes a set of
	  /// parameters and return a set of model values,
	  /// so the measurement points are already known to the function), and analytic parameter sensitivity is available </summary>
	  /// <param name="observedValues"> Set of measurement values </param>
	  /// <param name="sigma"> Set of measurement errors </param>
	  /// <param name="func"> The model as a function of its parameters only </param>
	  /// <param name="jac"> The model sensitivity to its parameters (i.e. the Jacobian matrix) as a function of its parameters only </param>
	  /// <param name="startPos"> Initial value of the parameters </param>
	  /// <param name="penalty"> Penalty matrix </param>
	  /// <returns> the least-square results </returns>
	  public virtual LeastSquareWithPenaltyResults solve(DoubleArray observedValues, DoubleArray sigma, System.Func<DoubleArray, DoubleArray> func, System.Func<DoubleArray, DoubleMatrix> jac, DoubleArray startPos, DoubleMatrix penalty)
	  {

		return solve(observedValues, sigma, func, jac, startPos, penalty, UNCONSTRAINED);
	  }

	  /// <summary>
	  /// Use this when the model is given as a function of its parameters only (i.e. a function that takes a set of
	  /// parameters and return a set of model values,
	  /// so the measurement points are already known to the function), and analytic parameter sensitivity is available </summary>
	  /// <param name="observedValues"> Set of measurement values </param>
	  /// <param name="sigma"> Set of measurement errors </param>
	  /// <param name="func"> The model as a function of its parameters only </param>
	  /// <param name="jac"> The model sensitivity to its parameters (i.e. the Jacobian matrix) as a function of its parameters only </param>
	  /// <param name="startPos"> Initial value of the parameters </param>
	  /// <param name="penalty"> Penalty matrix (must be positive semi-definite) </param>
	  /// <param name="allowedValue"> a function which returned true if the new trial position is allowed by the model. An example
	  ///   would be to enforce positive parameters
	  ///   without resorting to a non-linear parameter transform. In some circumstances this approach will lead to slow
	  ///   convergence. </param>
	  /// <returns> the least-square results </returns>
	  public virtual LeastSquareWithPenaltyResults solve(DoubleArray observedValues, DoubleArray sigma, System.Func<DoubleArray, DoubleArray> func, System.Func<DoubleArray, DoubleMatrix> jac, DoubleArray startPos, DoubleMatrix penalty, System.Func<DoubleArray, bool> allowedValue)
	  {

		ArgChecker.notNull(observedValues, "observedValues");
		ArgChecker.notNull(sigma, " sigma");
		ArgChecker.notNull(func, " func");
		ArgChecker.notNull(jac, " jac");
		ArgChecker.notNull(startPos, "startPos");
		int nObs = observedValues.size();
		ArgChecker.isTrue(nObs == sigma.size(), "observedValues and sigma must be same length");
		ArgChecker.isTrue(allowedValue(startPos), "The start position {} is not valid for this model. Please choose a valid start position", startPos);

		DoubleMatrix alpha;
		DecompositionResult decmp;
		DoubleArray theta = startPos;

		double lambda = 0.0; // TODO debug if the model is linear, it will be solved in 1 step
		double newChiSqr, oldChiSqr;
		DoubleArray error = getError(func, observedValues, sigma, theta);

		DoubleArray newError;
		DoubleMatrix jacobian = getJacobian(jac, sigma, theta);

		oldChiSqr = getChiSqr(error);
		double p = getANorm(penalty, theta);
		oldChiSqr += p;

		DoubleArray beta = getChiSqrGrad(error, jacobian);
		DoubleArray temp = (DoubleArray) _algebra.multiply(penalty, theta);
		beta = (DoubleArray) _algebra.subtract(beta, temp);

		for (int count = 0; count < MAX_ATTEMPTS; count++)
		{

		  alpha = getModifiedCurvatureMatrix(jacobian, lambda, penalty);
		  DoubleArray deltaTheta;

		  try
		  {
			decmp = _decomposition.apply(alpha);
			deltaTheta = decmp.solve(beta);
		  }
		  catch (Exception e)
		  {
			throw new MathException(e);
		  }

		  DoubleArray trialTheta = (DoubleArray) _algebra.add(theta, deltaTheta);

		  if (!allowedValue(trialTheta))
		  {
			lambda = increaseLambda(lambda);
			continue;
		  }

		  newError = getError(func, observedValues, sigma, trialTheta);
		  p = getANorm(penalty, trialTheta);
		  newChiSqr = getChiSqr(newError);
		  newChiSqr += p;

		  // Check for convergence when no improvement in chiSqr occurs
		  if (Math.Abs(newChiSqr - oldChiSqr) / (1 + oldChiSqr) < _eps)
		  {

			DoubleMatrix alpha0 = lambda == 0.0 ? alpha : getModifiedCurvatureMatrix(jacobian, 0.0, penalty);

			if (lambda > 0.0)
			{
			  decmp = _decomposition.apply(alpha0);
			}
			return finish(alpha0, decmp, newChiSqr - p, p, jacobian, trialTheta, sigma);
		  }

		  if (newChiSqr < oldChiSqr)
		  {
			lambda = decreaseLambda(lambda);
			theta = trialTheta;
			error = newError;
			jacobian = getJacobian(jac, sigma, trialTheta);
			beta = getChiSqrGrad(error, jacobian);
			temp = (DoubleArray) _algebra.multiply(penalty, theta);
			beta = (DoubleArray) _algebra.subtract(beta, temp);

			oldChiSqr = newChiSqr;
		  }
		  else
		  {
			lambda = increaseLambda(lambda);
		  }
		}
		throw new MathException("Could not converge in " + MAX_ATTEMPTS + " attempts");
	  }

	  private double decreaseLambda(double lambda)
	  {
		return lambda / 10;
	  }

	  private double increaseLambda(double lambda)
	  {
		if (lambda == 0.0)
		{ // this will happen the first time a full quadratic step fails
		  return 0.1;
		}
		return lambda * 10;
	  }

	  private LeastSquareWithPenaltyResults finish(DoubleMatrix alpha, DecompositionResult decmp, double chiSqr, double penalty, DoubleMatrix jacobian, DoubleArray newTheta, DoubleArray sigma)
	  {

		DoubleMatrix covariance = decmp.solve(DoubleMatrix.identity(alpha.rowCount()));
		DoubleMatrix bT = getBTranspose(jacobian, sigma);
		DoubleMatrix inverseJacobian = decmp.solve(bT);
		return new LeastSquareWithPenaltyResults(chiSqr, penalty, newTheta, covariance, inverseJacobian);
	  }

	  private DoubleArray getError(System.Func<DoubleArray, DoubleArray> func, DoubleArray observedValues, DoubleArray sigma, DoubleArray theta)
	  {

		int n = observedValues.size();
		DoubleArray modelValues = func(theta);
		ArgChecker.isTrue(n == modelValues.size(), "Number of data points different between model (" + modelValues.size() + ") and observed (" + n + ")");
		return DoubleArray.of(n, i => (observedValues.get(i) - modelValues.get(i)) / sigma.get(i));
	  }

	  private DoubleMatrix getBTranspose(DoubleMatrix jacobian, DoubleArray sigma)
	  {
		int n = jacobian.rowCount();
		int m = jacobian.columnCount();

		DoubleMatrix res = DoubleMatrix.filled(m, n);
		double[][] data = res.toArray();
		for (int i = 0; i < n; i++)
		{
		  double sigmaInv = 1.0 / sigma.get(i);
		  for (int k = 0; k < m; k++)
		  {
			data[k][i] = jacobian.get(i, k) * sigmaInv;
		  }
		}
		return DoubleMatrix.ofUnsafe(data);
	  }

	  private DoubleMatrix getJacobian(System.Func<DoubleArray, DoubleMatrix> jac, DoubleArray sigma, DoubleArray theta)
	  {
		DoubleMatrix res = jac(theta);
		double[][] data = res.toArray();
		int n = res.rowCount();
		int m = res.columnCount();
		ArgChecker.isTrue(theta.size() == m, "Jacobian is wrong size");
		ArgChecker.isTrue(sigma.size() == n, "Jacobian is wrong size");

		for (int i = 0; i < n; i++)
		{
		  double sigmaInv = 1.0 / sigma.get(i);
		  for (int j = 0; j < m; j++)
		  {
			data[i][j] *= sigmaInv;
		  }
		}
		return DoubleMatrix.ofUnsafe(data);
	  }

	  private double getChiSqr(DoubleArray error)
	  {
		return _algebra.getInnerProduct(error, error);
	  }

	  private DoubleArray getChiSqrGrad(DoubleArray error, DoubleMatrix jacobian)
	  {
		return (DoubleArray) _algebra.multiply(error, jacobian);
	  }

	  private DoubleMatrix getModifiedCurvatureMatrix(DoubleMatrix jacobian, double lambda, DoubleMatrix penalty)
	  {
		double onePLambda = 1.0 + lambda;
		int m = jacobian.columnCount();
		DoubleMatrix alpha = (DoubleMatrix) MA.add(MA.matrixTransposeMultiplyMatrix(jacobian), penalty);
		// scale the diagonal
		double[][] data = alpha.toArray();
		for (int i = 0; i < m; i++)
		{
		  data[i][i] *= onePLambda;
		}
		return DoubleMatrix.ofUnsafe(data);
	  }

	  private double getANorm(DoubleMatrix a, DoubleArray x)
	  {
		int n = x.size();
		double sum = 0.0;
		for (int i = 0; i < n; i++)
		{
		  for (int j = 0; j < n; j++)
		  {
			sum += a.get(i, j) * x.get(i) * x.get(j);
		  }
		}
		return sum;
	  }

	}

}
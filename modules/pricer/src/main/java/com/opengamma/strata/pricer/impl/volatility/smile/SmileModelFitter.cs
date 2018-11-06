using System.Collections;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using DecompositionFactory = com.opengamma.strata.math.impl.linearalgebra.DecompositionFactory;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using NonLinearParameterTransforms = com.opengamma.strata.math.impl.minimization.NonLinearParameterTransforms;
	using NonLinearTransformFunction = com.opengamma.strata.math.impl.minimization.NonLinearTransformFunction;
	using LeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResults;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;
	using NonLinearLeastSquare = com.opengamma.strata.math.impl.statistics.leastsquare.NonLinearLeastSquare;

	/// <summary>
	/// Smile model fitter.
	/// <para>
	/// Attempts to calibrate a smile model to the implied volatilities of European vanilla options, by minimising the sum of 
	/// squares between the market and model implied volatilities.
	/// </para>
	/// <para>
	/// All the options must be for the same expiry and (implicitly) on the same underlying.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the data of smile model to be calibrated </param>
	public abstract class SmileModelFitter<T> where T : SmileModelData
	{
	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();
	  private static readonly NonLinearLeastSquare SOLVER = new NonLinearLeastSquare(DecompositionFactory.SV_COMMONS, MA, 1e-12);
	  private static readonly System.Func<DoubleArray, bool> UNCONSTRAINED = (DoubleArray x) =>
	  {
  return true;
	  };

	  private readonly VolatilityFunctionProvider<T> model;
	  private readonly System.Func<DoubleArray, DoubleArray> volFunc;
	  private readonly System.Func<DoubleArray, DoubleMatrix> volAdjointFunc;
	  private readonly DoubleArray marketValues;
	  private readonly DoubleArray errors;

	  /// <summary>
	  /// Constructs smile model fitter from forward, strikes, time to expiry, implied volatilities and error values.
	  /// <para>
	  /// {@code strikes}, {@code impliedVols} and {@code error} should be the same length and ordered coherently.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strikes">  the ordered values of strikes </param>
	  /// <param name="timeToExpiry">  the time-to-expiry </param>
	  /// <param name="impliedVols">  the market implied volatilities </param>
	  /// <param name="error">  the 'measurement' error to apply to the market volatility of a particular option TODO: Review should this be part of  EuropeanOptionMarketData? </param>
	  /// <param name="model">  the volatility function provider </param>
	  public SmileModelFitter(double forward, DoubleArray strikes, double timeToExpiry, DoubleArray impliedVols, DoubleArray error, VolatilityFunctionProvider<T> model)
	  {
		ArgChecker.notNull(strikes, "strikes");
		ArgChecker.notNull(impliedVols, "implied vols");
		ArgChecker.notNull(error, "errors");
		ArgChecker.notNull(model, "model");
		int n = strikes.size();
		ArgChecker.isTrue(n == impliedVols.size(), "vols not the same length as strikes");
		ArgChecker.isTrue(n == error.size(), "errors not the same length as strikes");

		this.marketValues = impliedVols;
		this.errors = error;
		this.model = model;
		this.volFunc = (DoubleArray x) =>
		{
	T data = toSmileModelData(x);
	double[] res = new double[n];
	for (int i = 0; i < n; ++i)
	{
	  res[i] = model.volatility(forward, strikes.get(i), timeToExpiry, data);
	}
	return DoubleArray.copyOf(res);
		};
		this.volAdjointFunc = (DoubleArray x) =>
		{
	T data = toSmileModelData(x);
	double[][] resAdj = new double[n][];
	for (int i = 0; i < n; ++i)
	{
	  DoubleArray deriv = model.volatilityAdjoint(forward, strikes.get(i), timeToExpiry, data).Derivatives;
	  resAdj[i] = deriv.subArray(2).toArrayUnsafe();
	}
	return DoubleMatrix.copyOf(resAdj);
		};
	  }

	  /// <summary>
	  /// Solves using the default NonLinearParameterTransforms for the concrete implementation.
	  /// <para>
	  /// This returns <seealso cref="LeastSquareResults"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="start">  the first guess at the parameter values </param>
	  /// <returns> the calibration results </returns>
	  public virtual LeastSquareResultsWithTransform solve(DoubleArray start)
	  {
		return solve(start, new BitArray());
	  }

	  /// <summary>
	  /// Solve using the default NonLinearParameterTransforms for the concrete implementation with some parameters fixed 
	  /// to their initial values (indicated by fixed). 
	  /// <para>
	  /// This returns <seealso cref="LeastSquareResults"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="start">  the first guess at the parameter values </param>
	  /// <param name="fixed">  the parameters are fixed </param>
	  /// <returns> the calibration results </returns>
	  public virtual LeastSquareResultsWithTransform solve(DoubleArray start, BitArray @fixed)
	  {
		NonLinearParameterTransforms transform = getTransform(start, @fixed);
		return solve(start, transform);
	  }

	  /// <summary>
	  /// Solve using a user supplied NonLinearParameterTransforms.
	  /// <para>
	  /// This returns <seealso cref="LeastSquareResults"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="start">  the first guess at the parameter values </param>
	  /// <param name="transform">  transform from model parameters to fitting parameters, and vice versa </param>
	  /// <returns> the calibration results </returns>
	  public virtual LeastSquareResultsWithTransform solve(DoubleArray start, NonLinearParameterTransforms transform)
	  {
		NonLinearTransformFunction transFunc = new NonLinearTransformFunction(volFunc, volAdjointFunc, transform);
		LeastSquareResults solRes = SOLVER.solve(marketValues, errors, transFunc.FittingFunction, transFunc.FittingJacobian, transform.transform(start), getConstraintFunction(transform), MaximumStep);
		return new LeastSquareResultsWithTransform(solRes, transform);
	  }

	  /// <summary>
	  /// Obtains volatility function of the smile model.
	  /// <para>
	  /// The function is defined in <seealso cref="VolatilityFunctionProvider"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the function </returns>
	  protected internal virtual System.Func<DoubleArray, DoubleArray> ModelValueFunction
	  {
		  get
		  {
			return volFunc;
		  }
	  }

	  /// <summary>
	  /// Obtains Jacobian function of the smile model.
	  /// <para>
	  /// The function is defined in <seealso cref="VolatilityFunctionProvider"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the function </returns>
	  protected internal virtual System.Func<DoubleArray, DoubleMatrix> ModelJacobianFunction
	  {
		  get
		  {
			return volAdjointFunc;
		  }
	  }

	  /// <summary>
	  /// Obtains the maximum number of iterations.
	  /// </summary>
	  /// <returns> the maximum number. </returns>
	  protected internal abstract DoubleArray MaximumStep {get;}

	  /// <summary>
	  /// Obtains the nonlinear transformation of parameters from the initial values.
	  /// </summary>
	  /// <param name="start">  the initial values </param>
	  /// <returns> the nonlinear transformation </returns>
	  protected internal abstract NonLinearParameterTransforms getTransform(DoubleArray start);

	  /// <summary>
	  /// Obtains the nonlinear transformation of parameters from the initial values with some parameters fixed.
	  /// </summary>
	  /// <param name="start">  the initial values </param>
	  /// <param name="fixed">  the parameters are fixed </param>
	  /// <returns> the nonlinear transformation </returns>
	  protected internal abstract NonLinearParameterTransforms getTransform(DoubleArray start, BitArray @fixed);

	  /// <summary>
	  /// Obtains {@code SmileModelData} instance from the model parameters.
	  /// </summary>
	  /// <param name="modelParameters">  the model parameters </param>
	  /// <returns> the smile model data </returns>
	  public abstract T toSmileModelData(DoubleArray modelParameters);

	  /// <summary>
	  /// Obtains the constraint function.
	  /// <para>
	  /// This is defaulted to be "unconstrained".
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="t">  the nonlinear transformation </param>
	  /// <returns> the constraint function </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: protected java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, bool> getConstraintFunction(@SuppressWarnings("unused") final com.opengamma.strata.math.impl.minimization.NonLinearParameterTransforms t)
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
	  protected internal virtual System.Func<DoubleArray, bool> getConstraintFunction(NonLinearParameterTransforms t)
	  {
		return UNCONSTRAINED;
	  }

	  /// <summary>
	  /// Obtains the volatility function provider.
	  /// </summary>
	  /// <returns> the volatility function provider </returns>
	  public virtual VolatilityFunctionProvider<T> Model
	  {
		  get
		  {
			return model;
		  }
	  }

	}

}
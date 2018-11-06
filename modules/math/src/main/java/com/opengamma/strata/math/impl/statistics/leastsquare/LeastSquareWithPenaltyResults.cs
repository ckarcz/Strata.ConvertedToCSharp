/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Hold for results of <seealso cref="NonLinearLeastSquareWithPenalty"/>.
	/// </summary>
	public class LeastSquareWithPenaltyResults : LeastSquareResults
	{

	  private readonly double _penalty;

	  /// <summary>
	  /// Holder for the results of minimising $\sum_{i=1}^N (y_i - f_i(\mathbf{x}))^2 + \mathbf{x}^T\mathbf{P}\mathbf{x}$
	  /// WRT $\mathbf{x}$  (the vector of model parameters). </summary>
	  /// <param name="chiSqr"> The value of the first term (the chi-squared)- the sum of squares between the 'observed' values $y_i$ and the model values 
	  ///   $f_i(\mathbf{x})$ </param>
	  /// <param name="penalty"> The value of the second term (the penalty) </param>
	  /// <param name="parameters"> The value of  $\mathbf{x}$ </param>
	  /// <param name="covariance"> The covariance matrix for  $\mathbf{x}$  </param>
	  public LeastSquareWithPenaltyResults(double chiSqr, double penalty, DoubleArray parameters, DoubleMatrix covariance) : base(chiSqr, parameters, covariance)
	  {
		_penalty = penalty;
	  }

	  /// <summary>
	  /// Holder for the results of minimising $\sum_{i=1}^N (y_i - f_i(\mathbf{x}))^2 + \mathbf{x}^T\mathbf{P}\mathbf{x}$
	  /// WRT $\mathbf{x}$  (the vector of model parameters). </summary>
	  /// <param name="chiSqr"> The value of the first term (the chi-squared)- the sum of squares between the 'observed' values $y_i$ and the model values 
	  ///   $f_i(\mathbf{x})$ </param>
	  /// <param name="penalty"> The value of the second term (the penalty) </param>
	  /// <param name="parameters"> The value of  $\mathbf{x}$ </param>
	  /// <param name="covariance"> The covariance matrix for  $\mathbf{x}$ </param>
	  /// <param name="inverseJacobian"> The inverse Jacobian - this is the sensitivities of the model parameters to the 'observed' values  </param>
	  public LeastSquareWithPenaltyResults(double chiSqr, double penalty, DoubleArray parameters, DoubleMatrix covariance, DoubleMatrix inverseJacobian) : base(chiSqr, parameters, covariance, inverseJacobian)
	  {
		_penalty = penalty;
	  }

	  /// <summary>
	  /// Gets the value of the penalty. </summary>
	  /// <returns> the penalty  </returns>
	  public virtual double Penalty
	  {
		  get
		  {
			return _penalty;
		  }
	  }

	}

}
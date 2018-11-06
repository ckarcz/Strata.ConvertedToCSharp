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

	/// <summary>
	/// Container for the results of a least square (minimum chi-square) fit, where some model (with a set of parameters), is calibrated
	/// to a data set.
	/// </summary>
	// CSOFF: JavadocMethod
	public class LeastSquareResults
	{

	  private readonly double _chiSq;
	  private readonly DoubleArray _parameters;
	  private readonly DoubleMatrix _covariance;
	  private readonly DoubleMatrix _inverseJacobian;

	  public LeastSquareResults(LeastSquareResults from) : this(from._chiSq, from._parameters, from._covariance, from._inverseJacobian)
	  {
	  }

	  public LeastSquareResults(double chiSq, DoubleArray parameters, DoubleMatrix covariance) : this(chiSq, parameters, covariance, null)
	  {
	  }

	  public LeastSquareResults(double chiSq, DoubleArray parameters, DoubleMatrix covariance, DoubleMatrix inverseJacobian)
	  {

		ArgChecker.isTrue(chiSq >= 0, "chi square < 0");
		ArgChecker.notNull(parameters, "parameters");
		ArgChecker.notNull(covariance, "covariance");
		int n = parameters.size();
		ArgChecker.isTrue(covariance.columnCount() == n, "covariance matrix not square");
		ArgChecker.isTrue(covariance.rowCount() == n, "covariance matrix wrong size");
		//TODO test size of inverse Jacobian
		_chiSq = chiSq;
		_parameters = parameters;
		_covariance = covariance;
		_inverseJacobian = inverseJacobian;
	  }

	  /// <summary>
	  /// Gets the Chi-square of the fit. </summary>
	  /// <returns> the chiSq </returns>
	  public virtual double ChiSq
	  {
		  get
		  {
			return _chiSq;
		  }
	  }

	  /// <summary>
	  /// Gets the value of the fitting parameters, when the chi-squared is minimised. </summary>
	  /// <returns> the parameters </returns>
	  public virtual DoubleArray FitParameters
	  {
		  get
		  {
			return _parameters;
		  }
	  }

	  /// <summary>
	  /// Gets the estimated covariance matrix of the standard errors in the fitting parameters.
	  /// <b>Note</b> only in the case of normally distributed errors, does this have any meaning
	  /// full mathematical interpretation (See NR third edition, p812-816) </summary>
	  /// <returns> the formal covariance matrix </returns>
	  public virtual DoubleMatrix Covariance
	  {
		  get
		  {
			return _covariance;
		  }
	  }

	  /// <summary>
	  /// This a matrix where the i,jth element is the (infinitesimal) sensitivity of the ith fitting
	  /// parameter to the jth data point (NOT the model point), when the fitting parameter are such
	  /// that the chi-squared is minimised. So it is a type of (inverse) Jacobian, but should not be
	  /// confused with the model jacobian (sensitivity of model data points, to parameters) or its inverse.
	  /// </summary>
	  /// <returns> a matrix </returns>
	  public virtual DoubleMatrix FittingParameterSensitivityToData
	  {
		  get
		  {
			if (_inverseJacobian == null)
			{
			  throw new System.NotSupportedException("The inverse Jacobian was not set");
			}
			return _inverseJacobian;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_chiSq);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		result = prime * result + _covariance.GetHashCode();
		result = prime * result + _parameters.GetHashCode();
		result = prime * result + (_inverseJacobian == null ? 0 : _inverseJacobian.GetHashCode());
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		LeastSquareResults other = (LeastSquareResults) obj;
		if (System.BitConverter.DoubleToInt64Bits(_chiSq) != Double.doubleToLongBits(other._chiSq))
		{
		  return false;
		}
		if (!Objects.Equals(_covariance, other._covariance))
		{
		  return false;
		}
		if (!Objects.Equals(_inverseJacobian, other._inverseJacobian))
		{
		  return false;
		}
		return Objects.Equals(_parameters, other._parameters);
	  }

	  public override string ToString()
	  {
		return "LeastSquareResults [chiSq=" + _chiSq + ", fit parameters=" + _parameters.ToString() +
			", covariance=" + _covariance.ToString() + "]";
	  }

	}

}
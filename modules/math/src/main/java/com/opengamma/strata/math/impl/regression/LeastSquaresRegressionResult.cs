/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Contains the result of a least squares regression.
	/// </summary>
	//CSOFF: JavadocMethod
	public class LeastSquaresRegressionResult
	{
	  //TODO the predicted value calculation should be separated out from this class.

	  private readonly double[] _residuals;
	  private readonly double[] _betas;
	  private readonly double _meanSquareError;
	  private readonly double[] _standardErrorOfBeta;
	  private readonly double _rSquared;
	  private readonly double _rSquaredAdjusted;
	  private readonly double[] _tStats;
	  private readonly double[] _pValues;
	  private readonly bool _hasIntercept;

	  public LeastSquaresRegressionResult(LeastSquaresRegressionResult result)
	  {
		ArgChecker.notNull(result, "regression result");
		_betas = result.Betas;
		_residuals = result.Residuals;
		_meanSquareError = result.MeanSquareError;
		_standardErrorOfBeta = result.StandardErrorOfBetas;
		_rSquared = result.RSquared;
		_rSquaredAdjusted = result.AdjustedRSquared;
		_tStats = result.TStatistics;
		_pValues = result.PValues;
		_hasIntercept = result.hasIntercept();
	  }

	  public LeastSquaresRegressionResult(double[] betas, double[] residuals, double meanSquareError, double[] standardErrorOfBeta, double rSquared, double rSquaredAdjusted, double[] tStats, double[] pValues, bool hasIntercept)
	  {

		_betas = betas;
		_residuals = residuals;
		_meanSquareError = meanSquareError;
		_standardErrorOfBeta = standardErrorOfBeta;
		_rSquared = rSquared;
		_rSquaredAdjusted = rSquaredAdjusted;
		_tStats = tStats;
		_pValues = pValues;
		_hasIntercept = hasIntercept;
	  }

	  public virtual double[] Betas
	  {
		  get
		  {
			return _betas;
		  }
	  }

	  public virtual double[] Residuals
	  {
		  get
		  {
			return _residuals;
		  }
	  }

	  public virtual double MeanSquareError
	  {
		  get
		  {
			return _meanSquareError;
		  }
	  }

	  public virtual double[] StandardErrorOfBetas
	  {
		  get
		  {
			return _standardErrorOfBeta;
		  }
	  }

	  public virtual double RSquared
	  {
		  get
		  {
			return _rSquared;
		  }
	  }

	  public virtual double AdjustedRSquared
	  {
		  get
		  {
			return _rSquaredAdjusted;
		  }
	  }

	  public virtual double[] TStatistics
	  {
		  get
		  {
			return _tStats;
		  }
	  }

	  public virtual double[] PValues
	  {
		  get
		  {
			return _pValues;
		  }
	  }

	  public virtual bool hasIntercept()
	  {
		return _hasIntercept;
	  }

	  public virtual double getPredictedValue(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		double[] betas = Betas;
		if (hasIntercept())
		{
		  if (x.Length != betas.Length - 1)
		  {
			throw new System.ArgumentException("Number of variables did not match number used in regression");
		  }
		}
		else
		{
		  if (x.Length != betas.Length)
		  {
			throw new System.ArgumentException("Number of variables did not match number used in regression");
		  }
		}
		double sum = 0;
		for (int i = 0; i < (hasIntercept() ? x.Length + 1 : x.Length); i++)
		{
		  if (hasIntercept())
		  {
			if (i == 0)
			{
			  sum += betas[0];
			}
			else
			{
			  sum += betas[i] * x[i - 1];
			}
		  }
		  else
		  {
			sum += x[i] * betas[i];
		  }
		}
		return sum;
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		result = prime * result + Arrays.GetHashCode(_betas);
		result = prime * result + (_hasIntercept ? 1231 : 1237);
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(_meanSquareError);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		result = prime * result + Arrays.GetHashCode(_pValues);
		temp = System.BitConverter.DoubleToInt64Bits(_rSquared);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		temp = System.BitConverter.DoubleToInt64Bits(_rSquaredAdjusted);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		result = prime * result + Arrays.GetHashCode(_residuals);
		result = prime * result + Arrays.GetHashCode(_standardErrorOfBeta);
		result = prime * result + Arrays.GetHashCode(_tStats);
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
		LeastSquaresRegressionResult other = (LeastSquaresRegressionResult) obj;
		if (!Arrays.Equals(_betas, other._betas))
		{
		  return false;
		}
		if (_hasIntercept != other._hasIntercept)
		{
		  return false;
		}
		if (System.BitConverter.DoubleToInt64Bits(_meanSquareError) != Double.doubleToLongBits(other._meanSquareError))
		{
		  return false;
		}
		if (!Arrays.Equals(_pValues, other._pValues))
		{
		  return false;
		}
		if (System.BitConverter.DoubleToInt64Bits(_rSquared) != Double.doubleToLongBits(other._rSquared))
		{
		  return false;
		}
		if (System.BitConverter.DoubleToInt64Bits(_rSquaredAdjusted) != Double.doubleToLongBits(other._rSquaredAdjusted))
		{
		  return false;
		}
		if (!Arrays.Equals(_residuals, other._residuals))
		{
		  return false;
		}
		if (!Arrays.Equals(_standardErrorOfBeta, other._standardErrorOfBeta))
		{
		  return false;
		}
		if (!Arrays.Equals(_tStats, other._tStats))
		{
		  return false;
		}
		return true;
	  }
	}

}
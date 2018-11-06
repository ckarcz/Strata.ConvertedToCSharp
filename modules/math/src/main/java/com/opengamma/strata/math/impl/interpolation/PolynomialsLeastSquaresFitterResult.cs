/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Contains the result of a least squares regression for polynomial.
	/// </summary>
	public class PolynomialsLeastSquaresFitterResult
	{

	  private double[] _coefficients;
	  private DoubleMatrix _rMatrix;
	  private int _dof;
	  private double _diffNorm;
	  private double[] _meanAndStd;

	  /// <param name="coefficients"> Coefficients of the polynomial </param>
	  /// <param name="rMatrix"> R-matrix of the QR decomposition used in PolynomialsLeastSquaresRegression </param>
	  /// <param name="dof"> Degrees of freedom = Number of data points - (degrees of Polynomial + 1) </param>
	  /// <param name="diffNorm"> Square norm of the vector, "residuals," whose components are yData_i - f(xData_i) </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public PolynomialsLeastSquaresFitterResult(final double[] coefficients, final com.opengamma.strata.collect.array.DoubleMatrix rMatrix, final int dof, final double diffNorm)
	  public PolynomialsLeastSquaresFitterResult(double[] coefficients, DoubleMatrix rMatrix, int dof, double diffNorm)
	  {

		_coefficients = coefficients;
		_rMatrix = rMatrix;
		_dof = dof;
		_diffNorm = diffNorm;
		_meanAndStd = null;

	  }

	  /// <param name="coefficients"> Coefficients {a_0, a_1, a_2 ...} of the polynomial a_0 + a_1 x^1 + a_2 x^2 + .... </param>
	  /// <param name="rMatrix"> R-matrix of the QR decomposition used in PolynomialsLeastSquaresRegression </param>
	  /// <param name="dof"> Degrees of freedom = Number of data points - (degrees of Polynomial + 1) </param>
	  /// <param name="diffNorm"> Norm of the vector, "residuals," whose components are yData_i - f(xData_i) </param>
	  /// <param name="meanAndStd"> Vector (mean , standard deviation) used in normalization  </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public PolynomialsLeastSquaresFitterResult(final double[] coefficients, final com.opengamma.strata.collect.array.DoubleMatrix rMatrix, final int dof, final double diffNorm, final double[] meanAndStd)
	  public PolynomialsLeastSquaresFitterResult(double[] coefficients, DoubleMatrix rMatrix, int dof, double diffNorm, double[] meanAndStd)
	  {

		_coefficients = coefficients;
		_rMatrix = rMatrix;
		_dof = dof;
		_diffNorm = diffNorm;
		_meanAndStd = meanAndStd;

	  }

	  /// <returns> Coefficients {a_0, a_1, a_2 ...} of polynomial a_0 + a_1 x^1 + a_2 x^2 + .... </returns>
	  public virtual double[] Coeff
	  {
		  get
		  {
			return _coefficients;
		  }
	  }

	  /// <returns> R Matrix of QR decomposition </returns>
	  public virtual DoubleMatrix RMat
	  {
		  get
		  {
			return _rMatrix;
		  }
	  }

	  /// <returns> Degrees of freedom = Number of data points - (degrees of Polynomial + 1)  </returns>
	  public virtual int Dof
	  {
		  get
		  {
			return _dof;
		  }
	  }

	  /// <returns> Norm of the vector, "residuals," whose components are yData_i - f(xData_i) </returns>
	  public virtual double DiffNorm
	  {
		  get
		  {
			return _diffNorm;
		  }
	  }

	  /// <returns> Vector (mean , standard deviation) used in normalization  </returns>
	  public virtual double[] MeanAndStd
	  {
		  get
		  {
			ArgChecker.notNull(_meanAndStd, "xData are not normalized");
			return _meanAndStd;
		  }
	  }

	}

}
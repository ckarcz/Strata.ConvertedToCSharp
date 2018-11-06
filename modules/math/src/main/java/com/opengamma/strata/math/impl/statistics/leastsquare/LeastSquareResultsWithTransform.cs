/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using NonLinearParameterTransforms = com.opengamma.strata.math.impl.minimization.NonLinearParameterTransforms;

	/// <summary>
	/// Container for the results of a least square (minimum chi-square) fit, where some model (with a set of parameters), is calibrated
	/// to a data set, but the model parameters are first transformed to some fitting parameters (usually to impose some constants).
	/// </summary>
	// CSOFF: JavadocMethod
	public class LeastSquareResultsWithTransform : LeastSquareResults
	{

	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();

	  private readonly NonLinearParameterTransforms _transform;
	  private readonly DoubleArray _modelParameters;
	  private DoubleMatrix _inverseJacobianModelPararms;

	  public LeastSquareResultsWithTransform(LeastSquareResults transformedFitResult) : base(transformedFitResult)
	  {
		_transform = null;
		_modelParameters = transformedFitResult.FitParameters;
		_inverseJacobianModelPararms = FittingParameterSensitivityToData;
	  }

	  public LeastSquareResultsWithTransform(LeastSquareResults transformedFitResult, NonLinearParameterTransforms transform) : base(transformedFitResult)
	  {
		ArgChecker.notNull(transform, "null transform");
		_transform = transform;
		_modelParameters = transform.inverseTransform(FitParameters);
	  }

	  public virtual DoubleArray ModelParameters
	  {
		  get
		  {
			return _modelParameters;
		  }
	  }

	  /// <summary>
	  /// This a matrix where the i,j-th element is the (infinitesimal) sensitivity of the i-th model parameter 
	  /// to the j-th data point, when the fitting parameter are such that the chi-squared is minimised. 
	  /// So it is a type of (inverse) Jacobian, but should not be confused with the model jacobian 
	  /// (sensitivity of model parameters to internal parameters used in calibration procedure) or its inverse. </summary>
	  /// <returns> a matrix </returns>
	  public virtual DoubleMatrix ModelParameterSensitivityToData
	  {
		  get
		  {
			if (_inverseJacobianModelPararms == null)
			{
			  setModelParameterSensitivityToData();
			}
			return _inverseJacobianModelPararms;
		  }
	  }

	  private void setModelParameterSensitivityToData()
	  {
		DoubleMatrix invJac = _transform.inverseJacobian(FitParameters);
		_inverseJacobianModelPararms = (DoubleMatrix) MA.multiply(invJac, FittingParameterSensitivityToData);
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = base.GetHashCode();
		result = prime * result + ((_inverseJacobianModelPararms == null) ? 0 : _inverseJacobianModelPararms.GetHashCode());
		result = prime * result + ((_modelParameters == null) ? 0 : _modelParameters.GetHashCode());
		result = prime * result + ((_transform == null) ? 0 : _transform.GetHashCode());
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (!base.Equals(obj))
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		LeastSquareResultsWithTransform other = (LeastSquareResultsWithTransform) obj;
		if (_inverseJacobianModelPararms == null)
		{
		  if (other._inverseJacobianModelPararms != null)
		  {
			return false;
		  }
		}
		else if (!_inverseJacobianModelPararms.Equals(other._inverseJacobianModelPararms))
		{
		  return false;
		}
		if (_modelParameters == null)
		{
		  if (other._modelParameters != null)
		  {
			return false;
		  }
		}
		else if (!_modelParameters.Equals(other._modelParameters))
		{
		  return false;
		}
		if (_transform == null)
		{
		  if (other._transform != null)
		  {
			return false;
		  }
		}
		else if (!_transform.Equals(other._transform))
		{
		  return false;
		}
		return true;
	  }

	  public override string ToString()
	  {
		return "LeastSquareResults [chiSq=" + ChiSq + ", fit parameters=" + FitParameters.ToString() +
			", model parameters= " + ModelParameters.ToString() + ", covariance="
			+ Covariance.ToString() + "]";
	  }

	}

}
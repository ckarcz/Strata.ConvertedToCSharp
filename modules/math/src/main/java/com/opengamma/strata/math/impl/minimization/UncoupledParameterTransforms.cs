using System.Collections;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// For a set of <i>n</i> function parameters, this takes <i>n</i> ParameterLimitsTransform (which can be the NullTransform which does NOT transform the parameter) which transform
	/// a constrained function parameter (e.g. must be between -1 and 1) to a unconstrained fit parameter. It also takes a BitSet (of length <i>n</i>) with an element set to <b>true</b> if
	/// that parameter is fixed - a set of <i>n</i> startValues must also be provided, with only those corresponding to fixed parameters being used (i.e. the parameter is fixed at the startValue).
	/// The purpose is to allow an optimiser to work with unconstrained parameters without modifying the function that one wishes to optimise.
	/// </summary>
	// TODO not tested
	public class UncoupledParameterTransforms : NonLinearParameterTransforms
	{

	  private readonly DoubleArray _startValues;
	  private readonly ParameterLimitsTransform[] _transforms;
	  private readonly bool[] _freeParameters;
	  private readonly int _nMP;
	  private readonly int _nFP;

	  /// 
	  /// <param name="startValues"> fixed parameter values (if no parameters are fixed this is completely ignored) </param>
	  /// <param name="transforms"> Array of ParameterLimitsTransform (which can be the NullTransform which does NOT transform the parameter) which transform
	  ///   a constrained function parameter (e.g. must be between -1 and 1) to a unconstrained fit parameter. </param>
	  /// <param name="fixed"> BitSet with an element set to <b>true</b> if that parameter is fixed </param>
	  public UncoupledParameterTransforms(DoubleArray startValues, ParameterLimitsTransform[] transforms, BitArray @fixed)
	  {
		ArgChecker.notNull(startValues, "null start values");
		ArgChecker.notEmpty(transforms, "must specify transforms");
		ArgChecker.notNull(@fixed, "must specify what is fixed (even if none)");
		_nMP = startValues.size();
		ArgChecker.isTrue(_nMP == transforms.Length, "Have {}-dimensional start value but {} transforms", _nMP, transforms.Length);
		_freeParameters = new bool[_nMP];
		for (int i = 0; i < _nMP; i++)
		{
		  if (i < @fixed.Count)
		  {
			_freeParameters[i] = !@fixed.Get(i);
		  }
		  else
		  {
			_freeParameters[i] = true;
		  }
		}
		int count = @fixed.cardinality();
		ArgChecker.isTrue(count < _nMP, "all parameters are fixed");
		_nFP = _nMP - count;
		_startValues = startValues;
		_transforms = transforms;
	  }

	  /// 
	  /// <returns> The number of function parameters </returns>
	  public virtual int NumberOfModelParameters
	  {
		  get
		  {
			return _nMP;
		  }
	  }

	  /// 
	  /// <returns> The number of fitting parameters (equals the number of model parameters minus the number of fixed parameters) </returns>
	  public virtual int NumberOfFittingParameters
	  {
		  get
		  {
			return _nFP;
		  }
	  }

	  /// <summary>
	  /// Transforms from a set of function parameters (some of which may have constrained range and/or be fixed)
	  /// to a (possibly smaller) set of unconstrained fitting parameters.
	  /// <b>Note:</b> If a parameter is fixed, it is its value as provided by <i>startValues</i> not the value
	  /// given here that will be returned by inverseTransform (and thus used in the function).
	  /// </summary>
	  /// <param name="functionParameters"> The function parameters </param>
	  /// <returns> The fitting parameters </returns>
	  public virtual DoubleArray transform(DoubleArray functionParameters)
	  {
		ArgChecker.notNull(functionParameters, "function parameters");
		ArgChecker.isTrue(functionParameters.size() == _nMP, "functionParameters wrong dimension");
		double[] fittingParameter = new double[_nFP];
		for (int i = 0, j = 0; i < _nMP; i++)
		{
		  if (_freeParameters[i])
		  {
			fittingParameter[j] = _transforms[i].transform(functionParameters.get(i));
			j++;
		  }
		}
		return DoubleArray.copyOf(fittingParameter);
	  }

	  /// <summary>
	  /// Transforms from a set of unconstrained fitting parameters to a (possibly larger) set of function parameters (some of which may have constrained range and/or be fixed). </summary>
	  /// <param name="fittingParameters"> The fitting parameters </param>
	  /// <returns> The function parameters </returns>
	  public virtual DoubleArray inverseTransform(DoubleArray fittingParameters)
	  {
		ArgChecker.notNull(fittingParameters, "fitting parameters");
		ArgChecker.isTrue(fittingParameters.size() == _nFP, "fittingParameter wrong dimension");
		double[] modelParameter = new double[_nMP];
		for (int i = 0, j = 0; i < _nMP; i++)
		{
		  if (_freeParameters[i])
		  {
			modelParameter[i] = _transforms[i].inverseTransform(fittingParameters.get(j));
			j++;
		  }
		  else
		  {
			modelParameter[i] = _startValues.get(i);
		  }
		}
		return DoubleArray.copyOf(modelParameter);
	  }

	  /// <summary>
	  /// Calculates the Jacobian of the transform from function parameters to fitting parameters -
	  /// the i,j element will be the partial derivative of i^th fitting parameter with respect.
	  /// to the j^th function parameter </summary>
	  /// <param name="functionParameters"> The function parameters </param>
	  /// <returns> matrix of partial derivative of fitting parameter with respect to function parameters </returns>
	  // TODO not tested
	  public virtual DoubleMatrix jacobian(DoubleArray functionParameters)
	  {
		ArgChecker.notNull(functionParameters, "function parameters");
		ArgChecker.isTrue(functionParameters.size() == _nMP, "functionParameters wrong dimension");
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] jac = new double[_nFP][_nMP];
		double[][] jac = RectangularArrays.ReturnRectangularDoubleArray(_nFP, _nMP);
		for (int i = 0, j = 0; i < _nMP; i++)
		{
		  if (_freeParameters[i])
		  {
			jac[j][i] = _transforms[i].transformGradient(functionParameters.get(i));
			j++;
		  }
		}
		return DoubleMatrix.copyOf(jac);
	  }

	  /// <summary>
	  /// Calculates the Jacobian of the transform from fitting parameters to function parameters -
	  /// the i,j element will be the partial derivative of i^th function parameter with respect.
	  /// to the j^th  fitting parameter </summary>
	  /// <param name="fittingParameters">  The fitting parameters </param>
	  /// <returns>  matrix of partial derivative of function parameter with respect to fitting parameters </returns>
	  // TODO not tested

	  public virtual DoubleMatrix inverseJacobian(DoubleArray fittingParameters)
	  {
		ArgChecker.notNull(fittingParameters, "fitting parameters");
		ArgChecker.isTrue(fittingParameters.size() == _nFP, "fitting parameters wrong dimension");
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] jac = new double[_nMP][_nFP];
		double[][] jac = RectangularArrays.ReturnRectangularDoubleArray(_nMP, _nFP);
		int[] p = new int[_nMP];
		int[] q = new int[_nMP];
		int t = 0;
		for (int i = 0; i < _nMP; i++)
		{
		  if (_freeParameters[i])
		  {
			p[t] = i;
			q[t] = t;
			t++;
		  }
		}
		int pderef, qderef;
		for (int i = 0; i < t; i++)
		{
		  pderef = p[i];
		  qderef = q[i];
		  jac[pderef][qderef] = _transforms[pderef].inverseTransformGradient(fittingParameters.get(qderef));
		}
		return DoubleMatrix.copyOf(jac);
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		result = prime * result + Arrays.GetHashCode(_freeParameters);
		result = prime * result + _startValues.GetHashCode();
		result = prime * result + Arrays.GetHashCode(_transforms);
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
		UncoupledParameterTransforms other = (UncoupledParameterTransforms) obj;
		if (!Arrays.Equals(_freeParameters, other._freeParameters))
		{
		  return false;
		}
		if (!Objects.Equals(_startValues, other._startValues))
		{
		  return false;
		}
		return Arrays.Equals(_transforms, other._transforms);
	  }

	}

}
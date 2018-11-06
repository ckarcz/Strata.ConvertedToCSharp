/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Describes the transformation (and its inverse) from  a set of n variables (e.g. model parameters) to a set of m variables
	/// (e.g. fitting parameters), where m <= n.The principle use is in constrained optimisation, where the valid values of the parameters
	/// of a model live in some hyper-volume in R^n, but we wish to work with unconstrained variables in R^m. <para>
	/// The model parameters are denoted as <b>y</b> and the unconstrained variables as <b>y*</b>, which are related by the vector function
	/// <b>y*</b> = f(<b>y</b>), and its inverse  <b>y</b> = f<sup>-1</sup>(<b>y*</b>). The i,j element of the Jacobian is the rate of change of 
	/// the i<sup>th</sup> element of <b>y*</b> with respect to the  j <sup>th</sup> element of <b>y</b>, which is a (matrix) function of <b>y</b>,
	/// i.e. <b>J</b>(<b>y</b>). The inverse Jacobian is the rate of change of <b>y</b> with respect to  <b>y*</b>, i.e. <b>J</b><sup>-1</sup>(<b>y*</b>).
	/// These four functions must be provided by implementations of this interface. 
	/// </para>
	/// </summary>
	//CSOFF: JavadocMethod
	public interface NonLinearParameterTransforms
	{

	  int NumberOfModelParameters {get;}

	  int NumberOfFittingParameters {get;}

	  /// <summary>
	  /// Transforms from a set of model parameters to a (possibly smaller) set of unconstrained fitting parameters. </summary>
	  /// <param name="modelParameters">   the model parameters </param>
	  /// <returns> The fitting parameters </returns>
	  DoubleArray transform(DoubleArray modelParameters);

	  /// <summary>
	  /// Transforms from a set of unconstrained fitting parameters to a (possibly larger) set of function parameters. </summary>
	  /// <param name="fittingParameters"> The fitting parameters </param>
	  /// <returns> The model parameters </returns>
	  DoubleArray inverseTransform(DoubleArray fittingParameters);

	  /// <summary>
	  /// Calculates the Jacobian - the rate of change of the fitting parameters WRT the model parameters. </summary>
	  /// <param name="modelParameters"> The model parameters </param>
	  /// <returns> The Jacobian  </returns>
	  DoubleMatrix jacobian(DoubleArray modelParameters);

	  /// <summary>
	  /// Calculates the inverse Jacobian  - the rate of change of the model parameters WRT the fitting parameters. </summary>
	  /// <param name="fittingParameters"> The fitting parameters </param>
	  /// <returns> the inverse Jacobian  </returns>
	  DoubleMatrix inverseJacobian(DoubleArray fittingParameters);

	}

}
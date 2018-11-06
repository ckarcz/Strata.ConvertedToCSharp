/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
	/// <summary>
	/// Interface for objects containing functions that can transform constrained model parameters into unconstrained fitting parameters and vice versa. It also
	/// provides functions that will provide the gradient of the functions that perform these transformations. Let y be the model parameter and
	/// yStar the transformed (fitting) parameter, then we write y* = f(y)
	/// </summary>
	public interface ParameterLimitsTransform
	{

	  /// <summary>
	  /// Types of the limits. </summary>

	  /// <summary>
	  /// A function to transform a constrained model parameter (y) to an unconstrained fitting parameter (y*) - i.e. y* = f(y) </summary>
	  /// <param name="x"> Model parameter </param>
	  /// <returns> Fitting parameter </returns>
	  double transform(double x);

	  //  /**
	  //   * A function to transform a set of constrained model parameters to a set of unconstrained fitting parameters
	  //   * @param x Model parameter
	  //   * @return Fitting parameter
	  //   */
	  //  double[] transform(double[] x);

	  /// <summary>
	  /// A function to transform an unconstrained fitting parameter (y*) to a constrained model parameter (y) - i.e. y = f^-1(y*) </summary>
	  /// <param name="y"> Fitting parameter </param>
	  /// <returns> Model parameter  </returns>
	  double inverseTransform(double y);

	  /// <summary>
	  /// The gradient of the function used to transform from a model parameter that is only allows
	  /// to take certain values, to a fitting parameter that can take any value. </summary>
	  /// <param name="x"> Model parameter </param>
	  /// <returns> the gradient </returns>
	  double transformGradient(double x);

	  /// <summary>
	  /// The gradient of the function used to transform from a fitting parameter that can take any value,
	  /// to a model parameter that is only allows to take certain values. </summary>
	  /// <param name="y"> fitting parameter </param>
	  /// <returns> the gradient </returns>
	  double inverseTransformGradient(double y);

	}

	  public enum ParameterLimitsTransform_LimitType
	  {
	/// <summary>
	/// Greater than limit. </summary>
	GREATER_THAN,
	/// <summary>
	/// Less than limit. </summary>
	LESS_THAN
	  }

}
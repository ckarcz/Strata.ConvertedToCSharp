/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ScalarFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFieldFirstOrderDifferentiator;

	/// <summary>
	/// A parameterised curve that gives the both the curve (the function y=f(x) where x and y are scalars) and the
	/// curve sensitivity (dy/dp where p is one of the parameters) for given parameters.
	/// </summary>
	public abstract class ParameterizedCurve : ParameterizedFunction<double, DoubleArray, double>
	{

	  private static readonly ScalarFieldFirstOrderDifferentiator FIRST_ORDER_DIFF = new ScalarFieldFirstOrderDifferentiator();

	  /// <summary>
	  /// For a scalar function (curve) that can be written as $y=f(x;\boldsymbol{\theta})$ where x & y are scalars and
	  /// $\boldsymbol{\theta})$ is a vector of parameters (i.e. $x,y \in \mathbb{R}$ and $\boldsymbol{\theta} \in \mathbb{R}^n$)
	  /// this returns the function $g : \mathbb{R} \to \mathbb{R}^n; x \mapsto g(x)$, which is the function's (curve's) sensitivity 
	  /// to its parameters, i.e. $g(x) = \frac{\partial f(x;\boldsymbol{\theta})}{\partial \boldsymbol{\theta}}$<para>
	  /// The default calculation is performed using finite difference (via <seealso cref="ScalarFieldFirstOrderDifferentiator"/>) but
	  /// it is expected that this will be overridden by concrete subclasses.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="params">  the value of the parameters ($\boldsymbol{\theta}$) at which the sensitivity is calculated </param>
	  /// <returns> the sensitivity as a function with a Double (x) as its single argument and a vector as its return value </returns>
	  public virtual System.Func<double, DoubleArray> getYParameterSensitivity(DoubleArray @params)
	  {

		return (double? x) =>
		{

	Function<DoubleArray, double> f = asFunctionOfParameters(x);
	Function<DoubleArray, DoubleArray> g = FIRST_ORDER_DIFF.differentiate(f);
	return g.apply(@params);
		};
	  }

	}

}
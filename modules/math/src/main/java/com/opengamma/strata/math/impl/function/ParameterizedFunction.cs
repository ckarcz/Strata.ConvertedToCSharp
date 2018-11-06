/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// This class defines a 1-D function that takes both its argument and parameters inputs
	/// into the <seealso cref="#evaluate"/> method. The function can also be converted into a 1-D function
	/// of the arguments or a 1-D function of the parameters.
	/// 
	/// For example, assume that there is a function $f(x, \overline{a})$ defined as:
	/// $$
	/// \begin{align*}
	/// f(x, \overline{a}) = a_0 + a_1 x + a_2 x^2 + a_3 x^6
	/// \end{align*}
	/// $$
	/// The <seealso cref="#evaluate"/> method takes the value $x$ and the parameters
	/// $\overline{a}$ and returns the result. If the function is converted into a
	/// function of the arguments, the resulting function $g(x)$ is:
	/// $$
	/// \begin{align*}
	/// g(x) = a_0 + a_1 x + a_2 x^2 + a_3 x^6
	/// \end{align*}
	/// $$
	/// with $\overline{a}$ constant. If the function is converted into a function
	/// of the parameters, the resulting function $h(\overline{a})$ is:
	/// $$
	/// \begin{align*}
	/// h(\overline{a}) = a_0 + a_1 x + a_2 x^2 + a_3 x^6
	/// \end{align*}
	/// $$
	/// with $x$ constant.
	/// 
	/// This class is particularly useful when trying to fit the parameters of a model,
	/// such as in a Nelson Siegel Svennson bond curve model.
	/// </summary>
	/// @param <S> the type of arguments </param>
	/// @param <T> the type of parameters </param>
	/// @param <U> the type of result </param>
	public abstract class ParameterizedFunction<S, T, U>
	{

	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="x">  the value at which the function is to be evaluated </param>
	  /// <param name="parameters">  the parameters of the function </param>
	  /// <returns> The value of the function at <i>x</i> with the parameters as input </returns>
	  public abstract U evaluate(S x, T parameters);

	  /// <summary>
	  /// Uses the parameters to create a function.
	  /// </summary>
	  /// <param name="x">  the value at which the function is to be evaluated, not null </param>
	  /// <returns> a function that is always evaluated at <i>x</i> for different values of the parameters </returns>
	  public virtual System.Func<T, U> asFunctionOfParameters(S x)
	  {
		ArgChecker.notNull(x, "x");
		return (T @params) =>
		{
	return ParameterizedFunction.this.evaluate(x, @params);
		};
	  }

	  /// <summary>
	  /// Uses the parameters to create a function.
	  /// </summary>
	  /// <param name="params">  the parameters for which the function is to be evaluated, not null </param>
	  /// <returns> a function that can be evaluated at different <i>x</i> with the input parameters </returns>
	  public virtual System.Func<S, U> asFunctionOfArguments(T @params)
	  {
		ArgChecker.notNull(@params, "params");
		return (S x) =>
		{
	return ParameterizedFunction.this.evaluate(x, @params);
		};
	  }

	  /// <summary>
	  /// Gets the number of parameters.
	  /// </summary>
	  /// <returns> the number of parameters  </returns>
	  public abstract int NumberOfParameters {get;}

	}

}
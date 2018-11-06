/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;

	/// <summary>
	/// The base interface for calculation parameters.
	/// <para>
	/// Parameters are used to control the calculation.
	/// </para>
	/// <para>
	/// For example, <seealso cref="ReportingCurrency"/> is a parameter that controls currency conversion.
	/// If specified, on a <seealso cref="Column"/>, or in <seealso cref="CalculationRules"/>, then the output will
	/// be converted to the specified currency.
	/// </para>
	/// <para>
	/// Applications may implement this interface to add new parameters to the system.
	/// In order to be used, new implementations of <seealso cref="CalculationFunction"/> must be written
	/// that receive the parameters and perform appropriate behavior.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface CalculationParameter
	{

	  /// <summary>
	  /// Gets the type that the parameter will be queried by.
	  /// <para>
	  /// Parameters can be queried using <seealso cref="CalculationParameters#findParameter(Class)"/>.
	  /// This type is the key that callers must use in that method.
	  /// </para>
	  /// <para>
	  /// By default, this is just <seealso cref="Object#getClass()"/>.
	  /// It will only differ if the query type is an interface rather than the concrete class.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return getClass();
	//  }

	  /// <summary>
	  /// Filters this parameter to the specified target and measure.
	  /// <para>
	  /// Parameters may apply to all targets and measures or just a subset.
	  /// The <seealso cref="CalculationParameters#filter(CalculationTarget, Measure)"/> method
	  /// uses this method to filter a complete set of parameters.
	  /// </para>
	  /// <para>
	  /// By default, this returns {@code Optional.of(this)}.
	  /// If the parameter does not apply to either the target or measure, then optional empty must be returned.
	  /// If desired, the result can be a different parameter, allowing one parameter to delegate
	  /// to another when filtered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the calculation target, such as a trade </param>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <returns> the parameter appropriate to the target and measure, empty if this parameter does not apply </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.Optional<CalculationParameter> filter(com.opengamma.strata.basics.CalculationTarget target, com.opengamma.strata.calc.Measure measure)
	//  {
	//	return Optional.of(this);
	//  }

	}

}
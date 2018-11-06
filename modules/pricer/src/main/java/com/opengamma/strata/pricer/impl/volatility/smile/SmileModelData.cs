/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{
	/// <summary>
	/// A data bundle of a volatility model.
	/// <para>
	/// An implementation contains the data required for a volatility model.
	/// This is used with <seealso cref="VolatilityFunctionProvider"/>. 
	/// </para>
	/// </summary>
	public interface SmileModelData
	{

	  /// <summary>
	  /// Obtains the number of model parameters.
	  /// </summary>
	  /// <returns> the number of model parameters </returns>
	  int NumberOfParameters {get;}

	  /// <summary>
	  /// Obtains a model parameter specified by the index.
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <returns> the model parameter </returns>
	  double getParameter(int index);

	  /// <summary>
	  /// Checks the value satisfies the constraint for a model parameter.
	  /// <para>
	  /// The parameter is specified by {@code index}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="value">  the value </param>
	  /// <returns> true if allowed, false otherwise </returns>
	  bool isAllowed(int index, double value);

	  /// <summary>
	  /// Creates a new smile model data bundle with a model parameter replaced.
	  /// <para>
	  /// The parameter is specified by {@code index} and replaced by {@code value}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="value">  the value </param>
	  /// <returns> the new bundle </returns>
	  SmileModelData with(int index, double value);

	}

}
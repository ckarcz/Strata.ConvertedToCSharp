/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;

	/// <summary>
	/// An abstraction of market data in terms of a number of arbitrary {@code double} parameters.
	/// <para>
	/// This interface provides an abstraction over many different kinds of market data,
	/// including curves, surfaces and cubes. This abstraction allows an API to be structured
	/// in such a way that it does not directly expose the underlying data.
	/// For example, swaption volatilities might be based on a surface or a cube.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface ParameterizedData
	{

	  /// <summary>
	  /// Gets the number of parameters.
	  /// <para>
	  /// This returns the number of parameters, which can be used to create a loop
	  /// to access the other methods on this interface.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of parameters </returns>
	  int ParameterCount {get;}

	  /// <summary>
	  /// Gets the value of the parameter at the specified index.
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the value of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  double getParameter(int parameterIndex);

	  /// <summary>
	  /// Gets the metadata of the parameter at the specified index.
	  /// <para>
	  /// If there is no specific parameter metadata, an empty instance will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the metadata of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  ParameterMetadata getParameterMetadata(int parameterIndex);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of the data with the value at the specified index altered.
	  /// <para>
	  /// This instance is immutable and unaffected by this method call.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <param name="newValue">  the new value for the specified parameter </param>
	  /// <returns> a parameterized data instance based on this with the specified parameter altered </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  ParameterizedData withParameter(int parameterIndex, double newValue);

	  /// <summary>
	  /// Returns a perturbed copy of the data.
	  /// <para>
	  /// The perturbation instance will be invoked once for each parameter in this instance,
	  /// returning the perturbed value for that parameter. The result of this method is a
	  /// new instance that is based on those perturbed values.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method call.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="perturbation">  the perturbation to apply </param>
	  /// <returns> a parameterized data instance based on this with the specified perturbation applied </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ParameterizedData withPerturbation(ParameterPerturbation perturbation)
	//  {
	//	ParameterizedData result = this;
	//	for (int i = 0; i < getParameterCount(); i++)
	//	{
	//	  double currentValue = getParameter(i);
	//	  double perturbedValue = perturbation.perturbParameter(i, currentValue, getParameterMetadata(i));
	//	  // compare using Double.doubleToLongBits()
	//	  result = JodaBeanUtils.equal(currentValue, perturbedValue) ? result : result.withParameter(i, perturbedValue);
	//	}
	//	return result;
	//  }

	}

}
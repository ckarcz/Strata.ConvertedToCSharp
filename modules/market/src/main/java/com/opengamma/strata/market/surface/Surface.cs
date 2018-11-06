/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A surface that maps a {@code double} x-value and y-value to a {@code double} z-value.
	/// <para>
	/// Implementations of this interface provide the ability to find a z-value on the surface
	/// from the x-value and y-value.
	/// </para>
	/// <para>
	/// Each implementation will be backed by a number of <i>parameters</i>.
	/// The meaning of the parameters is implementation dependent.
	/// The sensitivity of the result to each of the parameters can also be obtained.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= InterpolatedNodalSurface </seealso>
	public interface Surface : ParameterizedData
	{

	  /// <summary>
	  /// Gets the surface metadata.
	  /// <para>
	  /// This method returns metadata about the surface and the surface parameters.
	  /// </para>
	  /// <para>
	  /// The metadata includes an optional list of parameter metadata.
	  /// If parameter metadata is present, the size of the list will match the number of parameters of this surface.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the metadata </returns>
	  SurfaceMetadata Metadata {get;}

	  /// <summary>
	  /// Returns a new surface with the specified metadata.
	  /// <para>
	  /// This allows the metadata of the surface to be changed while retaining all other information.
	  /// If parameter metadata is present, the size of the list must match the number of parameters of this surface.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the new metadata for the surface </param>
	  /// <returns> the new surface </returns>
	  Surface withMetadata(SurfaceMetadata metadata);

	  /// <summary>
	  /// Gets the surface name.
	  /// </summary>
	  /// <returns> the surface name </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default SurfaceName getName()
	//  {
	//	return getMetadata().getSurfaceName();
	//  }

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.ParameterMetadata getParameterMetadata(int parameterIndex)
	//  {
	//	return getMetadata().getParameterMetadata(parameterIndex);
	//  }

	  Surface withParameter(int parameterIndex, double newValue);

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Surface withPerturbation(com.opengamma.strata.market.param.ParameterPerturbation perturbation)
	//  {
	//	return (Surface) ParameterizedData.this.withPerturbation(perturbation);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the z-value for the specified x-value and y-value.
	  /// </summary>
	  /// <param name="x">  the x-value to find the z-value for </param>
	  /// <param name="y">  the y-value to find the z-value for </param>
	  /// <returns> the value at the x/y point </returns>
	  double zValue(double x, double y);

	  /// <summary>
	  /// Computes the z-value for the specified pair of x-value and y-value.
	  /// </summary>
	  /// <param name="xyPair">  the pair of x-value and y-value to find the z-value for </param>
	  /// <returns> the value at the x/y point </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double zValue(com.opengamma.strata.collect.tuple.DoublesPair xyPair)
	//  {
	//	return zValue(xyPair.getFirst(), xyPair.getSecond());
	//  }

	  /// <summary>
	  /// Computes the sensitivity of the z-value with respect to the surface parameters.
	  /// <para>
	  /// This returns an array with one element for each x-y parameter of the surface.
	  /// The array contains one a sensitivity value for each parameter used to create the surface.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x-value at which the parameter sensitivity is computed </param>
	  /// <param name="y">  the y-value at which the parameter sensitivity is computed </param>
	  /// <returns> the sensitivity at the x/y/ point </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
	  UnitParameterSensitivity zValueParameterSensitivity(double x, double y);

	  /// <summary>
	  /// Computes the sensitivity of the z-value with respect to the surface parameters.
	  /// <para>
	  /// This returns an array with one element for each x-y parameter of the surface.
	  /// The array contains one sensitivity value for each parameter used to create the surface.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xyPair">  the pair of x-value and y-value at which the parameter sensitivity is computed </param>
	  /// <returns> the sensitivity at the x/y/ point </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.UnitParameterSensitivity zValueParameterSensitivity(com.opengamma.strata.collect.tuple.DoublesPair xyPair)
	//  {
	//	return zValueParameterSensitivity(xyPair.getFirst(), xyPair.getSecond());
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a parameter sensitivity instance for this surface when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#zValueParameterSensitivity(double, double)"/> should be used and manipulated.
	  /// However, it can be useful to create a <seealso cref="UnitParameterSensitivity"/> from pre-computed sensitivity values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivity values, which must match the parameter count of the surface </param>
	  /// <returns> the sensitivity </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.UnitParameterSensitivity createParameterSensitivity(com.opengamma.strata.collect.array.DoubleArray sensitivities)
	//  {
	//	List<ParameterMetadata> paramMeta = IntStream.range(0, getParameterCount()).mapToObj(i -> getParameterMetadata(i)).collect(toImmutableList());
	//	return UnitParameterSensitivity.of(getName(), paramMeta, sensitivities);
	//  }

	  /// <summary>
	  /// Creates a parameter sensitivity instance for this surface when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#zValueParameterSensitivity(double, double)"/> should be used and manipulated.
	  /// However, it can be useful to create a <seealso cref="CurrencyParameterSensitivity"/> from pre-computed sensitivity values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="sensitivities">  the sensitivity values, which must match the parameter count of the surface </param>
	  /// <returns> the sensitivity </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.CurrencyParameterSensitivity createParameterSensitivity(com.opengamma.strata.basics.currency.Currency currency, com.opengamma.strata.collect.array.DoubleArray sensitivities)
	//  {
	//	List<ParameterMetadata> paramMeta = IntStream.range(0, getParameterCount()).mapToObj(i -> getParameterMetadata(i)).collect(toImmutableList());
	//	return CurrencyParameterSensitivity.of(getName(), paramMeta, currency, sensitivities);
	//  }

	}

}
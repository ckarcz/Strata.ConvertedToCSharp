/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// A surface based on {@code double} nodal points.
	/// <para>
	/// This provides access to a surface mapping a {@code double} x-value and
	/// {@code double} y-value to a {@code double} z-value.
	/// </para>
	/// <para>
	/// The parameters of an x-y surface are the x-y values.
	/// The values themselves are returned by <seealso cref="#getXValues()"/> and <seealso cref="#getYValues()"/>.
	/// The metadata is returned by <seealso cref="#getMetadata()"/>.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= InterpolatedNodalSurface </seealso>
	public interface NodalSurface : Surface
	{

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
	  NodalSurface withMetadata(SurfaceMetadata metadata);

	  /// <summary>
	  /// Gets the metadata of the parameter at the specified index.
	  /// <para>
	  /// If there is no specific parameter metadata, <seealso cref="SimpleSurfaceParameterMetadata"/> will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the metadata of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.ParameterMetadata getParameterMetadata(int parameterIndex)
	//  {
	//	return getMetadata().getParameterMetadata().map(pm -> pm.get(parameterIndex)).orElse(SimpleSurfaceParameterMetadata.of(getMetadata().getXValueType(), getXValues().get(parameterIndex), getMetadata().getYValueType(), getYValues().get(parameterIndex)));
	//  }

	  /// <summary>
	  /// Gets the known x-values of the surface.
	  /// <para>
	  /// This method returns the fixed x-values used to define the surface.
	  /// This will be of the same size as the y-values and z-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the x-values </returns>
	  DoubleArray XValues {get;}

	  /// <summary>
	  /// Gets the known y-values of the surface.
	  /// <para>
	  /// This method returns the fixed y-values used to define the surface.
	  /// This will be of the same size as the x-values and z-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the y-values </returns>
	  DoubleArray YValues {get;}

	  /// <summary>
	  /// Gets the known z-values of the surface.
	  /// <para>
	  /// This method returns the fixed z-values used to define the surface.
	  /// This will be of the same size as the x-values and y-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the z-values </returns>
	  DoubleArray ZValues {get;}

	  /// <summary>
	  /// Returns a new surface with the specified values.
	  /// <para>
	  /// This allows the z-values of the surface to be changed while retaining the
	  /// same x-values and y-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the new y-values for the surface </param>
	  /// <returns> the new surface </returns>
	  NodalSurface withZValues(DoubleArray values);

	  //-------------------------------------------------------------------------
	  NodalSurface withParameter(int parameterIndex, double newValue);

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default NodalSurface withPerturbation(com.opengamma.strata.market.param.ParameterPerturbation perturbation)
	//  {
	//	return (NodalSurface) Surface.this.withPerturbation(perturbation);
	//  }

	}

}
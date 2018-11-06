/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// A curve based on {@code double} nodal points.
	/// <para>
	/// This provides access to a curve mapping a {@code double} x-value to a {@code double} y-value.
	/// </para>
	/// <para>
	/// The parameters of an x-y curve are the x-y values.
	/// The values themselves are returned by <seealso cref="#getXValues()"/> and <seealso cref="#getYValues()"/>.
	/// The metadata is returned by <seealso cref="#getMetadata()"/>.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= InterpolatedNodalCurve </seealso>
	public interface NodalCurve : Curve
	{

	  /// <summary>
	  /// Returns a new curve with the specified metadata.
	  /// <para>
	  /// This allows the metadata of the curve to be changed while retaining all other information.
	  /// If parameter metadata is present, the size of the list must match the number of parameters of this curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the new metadata for the curve </param>
	  /// <returns> the new curve </returns>
	  NodalCurve withMetadata(CurveMetadata metadata);

	  /// <summary>
	  /// Gets the metadata of the parameter at the specified index.
	  /// <para>
	  /// If there is no specific parameter metadata, <seealso cref="SimpleCurveParameterMetadata"/> will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterIndex">  the zero-based index of the parameter to get </param>
	  /// <returns> the metadata of the parameter </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.ParameterMetadata getParameterMetadata(int parameterIndex)
	//  {
	//	return getMetadata().getParameterMetadata().map(pm -> pm.get(parameterIndex)).orElse(SimpleCurveParameterMetadata.of(getMetadata().getXValueType(), getXValues().get(parameterIndex)));
	//  }

	  /// <summary>
	  /// Gets the known x-values of the curve.
	  /// <para>
	  /// This method returns the fixed x-values used to define the curve.
	  /// This will be of the same size as the y-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the x-values </returns>
	  DoubleArray XValues {get;}

	  /// <summary>
	  /// Gets the known y-values of the curve.
	  /// <para>
	  /// This method returns the fixed y-values used to define the curve.
	  /// This will be of the same size as the x-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the y-values </returns>
	  DoubleArray YValues {get;}

	  /// <summary>
	  /// Returns a new curve with the specified values.
	  /// <para>
	  /// This allows the y-values of the curve to be changed while retaining the same x-values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the new y-values for the curve </param>
	  /// <returns> the new curve </returns>
	  NodalCurve withYValues(DoubleArray values);

	  /// <summary>
	  /// Returns a new curve with the specified x-values and y-values.
	  /// <para>
	  /// This allows the x values and y-values of the curve to be changed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValues">  the new x-values for the curve </param>
	  /// <param name="yValues">  the new y-values for the curve </param>
	  /// <returns> the new curve </returns>
	  NodalCurve withValues(DoubleArray xValues, DoubleArray yValues);

	  //-------------------------------------------------------------------------
	  NodalCurve withParameter(int parameterIndex, double newValue);

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default NodalCurve withPerturbation(com.opengamma.strata.market.param.ParameterPerturbation perturbation)
	//  {
	//	return (NodalCurve) Curve.this.withPerturbation(perturbation);
	//  }

	  /// <summary>
	  /// Returns a new curve with an additional node, specifying the parameter metadata.
	  /// <para>
	  /// The result will contain the specified node.
	  /// If the x-value equals an existing x-value, the y-value will be changed.
	  /// If the x-value does not equal an existing x-value, the node will be added.
	  /// </para>
	  /// <para>
	  /// The result will only contain the specified parameter metadata if this curve also has parameter meta-data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the new x-value </param>
	  /// <param name="y">  the new y-value </param>
	  /// <param name="paramMetadata">  the new parameter metadata </param>
	  /// <returns> the updated curve </returns>
	  NodalCurve withNode(double x, double y, ParameterMetadata paramMetadata);

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// A curve that maps a {@code double} x-value to a {@code double} y-value.
	/// <para>
	/// Implementations of this interface provide the ability to find a y-value on the curve from the x-value.
	/// </para>
	/// <para>
	/// Each implementation will be backed by a number of <i>parameters</i>.
	/// The meaning of the parameters is implementation dependent.
	/// The sensitivity of the result to each of the parameters can also be obtained.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= InterpolatedNodalCurve </seealso>
	public interface Curve : ParameterizedData
	{

	  /// <summary>
	  /// Gets the curve metadata.
	  /// <para>
	  /// This method returns metadata about the curve and the curve parameters.
	  /// </para>
	  /// <para>
	  /// For example, a curve may be defined based on financial instruments.
	  /// The parameters might represent 1 day, 1 week, 1 month, 3 months, 6 months and 12 months.
	  /// The metadata could be used to describe each parameter in terms of a <seealso cref="Period"/>.
	  /// </para>
	  /// <para>
	  /// The metadata includes an optional list of parameter metadata.
	  /// If parameter metadata is present, the size of the list will match the number of parameters of this curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the metadata </returns>
	  CurveMetadata Metadata {get;}

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
	  Curve withMetadata(CurveMetadata metadata);

	  /// <summary>
	  /// Gets the curve name.
	  /// </summary>
	  /// <returns> the curve name </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default CurveName getName()
	//  {
	//	return getMetadata().getCurveName();
	//  }

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.ParameterMetadata getParameterMetadata(int parameterIndex)
	//  {
	//	return getMetadata().getParameterMetadata(parameterIndex);
	//  }

	  Curve withParameter(int parameterIndex, double newValue);

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Curve withPerturbation(com.opengamma.strata.market.param.ParameterPerturbation perturbation)
	//  {
	//	return (Curve) ParameterizedData.this.withPerturbation(perturbation);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the y-value for the specified x-value.
	  /// </summary>
	  /// <param name="x">  the x-value to find the y-value for </param>
	  /// <returns> the value at the x-value </returns>
	  double yValue(double x);

	  /// <summary>
	  /// Computes the sensitivity of the y-value with respect to the curve parameters.
	  /// <para>
	  /// This returns an array with one element for each parameter of the curve.
	  /// The array contains the sensitivity of the y-value at the specified x-value to each parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x-value at which the parameter sensitivity is computed </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the sensitivity cannot be calculated </exception>
	  UnitParameterSensitivity yValueParameterSensitivity(double x);

	  /// <summary>
	  /// Computes the first derivative of the curve.
	  /// <para>
	  /// The first derivative is {@code dy/dx}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x-value at which the derivative is taken </param>
	  /// <returns> the first derivative </returns>
	  /// <exception cref="RuntimeException"> if the derivative cannot be calculated </exception>
	  double firstDerivative(double x);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a parameter sensitivity instance for this curve when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#yValueParameterSensitivity(double)"/> should be used and manipulated.
	  /// However, it can be useful to create a <seealso cref="UnitParameterSensitivity"/> from pre-computed sensitivity values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivity values, which must match the parameter count of the curve </param>
	  /// <returns> the sensitivity </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.UnitParameterSensitivity createParameterSensitivity(com.opengamma.strata.collect.array.DoubleArray sensitivities)
	//  {
	//	List<ParameterMetadata> paramMeta = IntStream.range(0, getParameterCount()).mapToObj(i -> getParameterMetadata(i)).collect(toImmutableList());
	//	return UnitParameterSensitivity.of(getName(), paramMeta, sensitivities);
	//  }

	  /// <summary>
	  /// Creates a parameter sensitivity instance for this curve when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#yValueParameterSensitivity(double)"/> should be used and manipulated.
	  /// However, it can be useful to create a <seealso cref="CurrencyParameterSensitivity"/> from pre-computed sensitivity values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="sensitivities">  the sensitivity values, which must match the parameter count of the curve </param>
	  /// <returns> the sensitivity </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.CurrencyParameterSensitivity createParameterSensitivity(com.opengamma.strata.basics.currency.Currency currency, com.opengamma.strata.collect.array.DoubleArray sensitivities)
	//  {
	//	List<ParameterMetadata> paramMeta = IntStream.range(0, getParameterCount()).mapToObj(i -> getParameterMetadata(i)).collect(toImmutableList());
	//	return CurrencyParameterSensitivity.of(getName(), paramMeta, currency, sensitivities);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a list of underlying curves. 
	  /// <para>
	  /// In most cases, the number of underlying curves is 1, thus a list of this curve is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the underlying curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableList<Curve> split()
	//  {
	//	return ImmutableList.of(this);
	//  }

	  /// <summary>
	  /// Replaces an underlying curve by a new curve.
	  /// <para>
	  /// {@code curveIndex} must be coherent to the index of the list in {@code split()}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveIndex">  the curve index </param>
	  /// <param name="curve">  the new split curve </param>
	  /// <returns> the new curve </returns>
	  /// <exception cref="IllegalArgumentException"> if {@code curveIndex} is outside the range </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Curve withUnderlyingCurve(int curveIndex, Curve curve)
	//  {
	//	if (curveIndex == 0)
	//	{
	//	  return curve;
	//	}
	//	throw new IllegalArgumentException("curveIndex is outside the range");
	//  }

	}

}
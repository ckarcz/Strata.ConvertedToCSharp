/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketData = com.opengamma.strata.data.MarketData;

	/// <summary>
	/// Provides the definition of how to calibrate a curve.
	/// <para>
	/// A curve is built from a number of parameters and described by metadata.
	/// Calibration is based on a list of <seealso cref="CurveNode"/> instances that specify the underlying instruments.
	/// </para>
	/// </summary>
	public interface CurveDefinition
	{

	  /// <summary>
	  /// Gets the curve name.
	  /// </summary>
	  /// <returns> the curve name </returns>
	  CurveName Name {get;}

	  /// <summary>
	  /// Gets the number of parameters in the curve.
	  /// <para>
	  /// This returns the number of parameters in the curve, which is not necessarily equal the size of {@code getNodes()}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of parameters </returns>
	  int ParameterCount {get;}

	  /// <summary>
	  /// Gets the y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the y-value type </returns>
	  ValueType YValueType {get;}

	  /// <summary>
	  /// Gets the nodes that define the curve.
	  /// <para>
	  /// The nodes are used to calibrate the curve.
	  /// If the objective curve is a nodal curve, each node is used to produce a parameter in the final curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the nodes </returns>
	  ImmutableList<CurveNode> Nodes {get;}

	  /// <summary>
	  /// Returns a filtered version of this definition with no invalid nodes.
	  /// <para>
	  /// A curve is formed of a number of nodes, each of which has an associated date.
	  /// To be valid, the curve node dates must be in order from earliest to latest.
	  /// Each node has certain rules, <seealso cref="CurveNodeDateOrder"/>, that are used to determine
	  /// what happens if the date of one curve node is equal or earlier than the date of the previous node.
	  /// </para>
	  /// <para>
	  /// Filtering occurs in two stages. The first stage looks at each node in turn. The previous and next
	  /// nodes are checked for clash. If clash occurs, then one of the two nodes is dropped according to
	  /// the <seealso cref="CurveNodeClashAction clash action"/> "drop" values. The second stage then looks
	  /// again at the nodes, and if there are still any invalid nodes, an exception is thrown.
	  /// </para>
	  /// <para>
	  /// This approach means that in most cases, only those nodes that have fixed dates,
	  /// such as futures, need to be annotated with {@code CurveNodeDateOrder}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the resolved definition, that should be used in preference to this one </returns>
	  /// <exception cref="IllegalArgumentException"> if the curve nodes are invalid </exception>
	  CurveDefinition filtered(LocalDate valuationDate, ReferenceData refData);

	  /// <summary>
	  /// Creates the curve metadata.
	  /// <para>
	  /// This method returns metadata about the curve and the curve parameters.
	  /// </para>
	  /// <para>
	  /// For example, a curve may be defined based on financial instruments.
	  /// The parameters might represent 1 day, 1 week, 1 month, 3 months, 6 months and 12 months.
	  /// The metadata could be used to describe each parameter in terms of a <seealso cref="Period"/>.
	  /// </para>
	  /// <para>
	  /// The optional parameter-level metadata will be populated on the resulting metadata.
	  /// The size of the parameter-level metadata will match the number of parameters of this curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the metadata </returns>
	  CurveMetadata metadata(LocalDate valuationDate, ReferenceData refData);

	  /// <summary>
	  /// Creates the curve from an array of parameter values.
	  /// <para>
	  /// The meaning of the parameters is determined by the implementation.
	  /// The size of the array must match the <seealso cref="#getParameterCount() count of parameters"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="metadata">  the curve metadata </param>
	  /// <param name="parameters">  the array of parameters </param>
	  /// <returns> the curve </returns>
	  Curve curve(LocalDate valuationDate, CurveMetadata metadata, DoubleArray parameters);

	  /// <summary>
	  /// Converts this definition to the summary form.
	  /// <para>
	  /// The <seealso cref="CurveParameterSize"/> class provides a summary of this definition
	  /// consisting of the name and parameter size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the summary form </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default CurveParameterSize toCurveParameterSize()
	//  {
	//	return CurveParameterSize.of(getName(), getParameterCount());
	//  }

	  /// <summary>
	  /// Gets the list of all initial guesses.
	  /// <para>
	  /// This returns initial guess for the curve parameters.
	  /// The valuation date is defined by the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <returns> the initial guess </returns>
	  ImmutableList<double> initialGuess(MarketData marketData);

	}

}
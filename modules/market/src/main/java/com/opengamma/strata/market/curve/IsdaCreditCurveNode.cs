/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;

	/// <summary>
	/// A node specifying how to calibrate an ISDA compliant curve.
	/// <para>
	/// A curve node is associated with an instrument and provide the information of the instrument for pricing.
	/// </para>
	/// </summary>
	public interface IsdaCreditCurveNode
	{

	  /// <summary>
	  /// Gets the label to use for the node.
	  /// </summary>
	  /// <returns> the label, not empty </returns>
	  string Label {get;}

	  /// <summary>
	  /// Get the observable ID. 
	  /// <para>
	  /// The observable ID is the identifier of the market data value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the observable ID </returns>
	  ObservableId ObservableId {get;}

	  /// <summary>
	  /// Calculates the date associated with the node.
	  /// <para>
	  /// Each curve node has an associated date which defines the x-value in the curve. 
	  /// This is typically the adjusted end date of the instrument.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the node date </returns>
	  LocalDate date(LocalDate tradeDate, ReferenceData refData);

	  /// <summary>
	  /// Returns metadata for the node from the node date. 
	  /// <para>
	  /// The node date must be computed by <seealso cref="#date(LocalDate, ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="nodeDate">  the node date used when calibrating the curve </param>
	  /// <returns> metadata for the node </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.DatedParameterMetadata metadata(java.time.LocalDate nodeDate)
	//  {
	//	return LabelDateParameterMetadata.of(nodeDate, getLabel());
	//  }

	}

}
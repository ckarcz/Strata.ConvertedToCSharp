using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using Trade = com.opengamma.strata.product.Trade;

	/// <summary>
	/// A node in the configuration specifying how to calibrate a curve.
	/// <para>
	/// A curve node is associated with an instrument and provides a method to create a trade representing the instrument.
	/// </para>
	/// </summary>
	public interface CurveNode
	{

	  /// <summary>
	  /// Gets the label to use for the node.
	  /// </summary>
	  /// <returns> the label, not empty </returns>
	  string Label {get;}

	  /// <summary>
	  /// Gets the date order rules that apply to this node within the curve.
	  /// <para>
	  /// Each curve node has an associated date which defines the x-value in the curve,
	  /// available via <seealso cref="#date(LocalDate, ReferenceData)"/>. Restrictions may be placed
	  /// on the node to prevent it from being too close, before or after another node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the date order rules </returns>
	  CurveNodeDateOrder DateOrder {get;}

	  /// <summary>
	  /// Determines the market data that is required by the node.
	  /// <para>
	  /// This returns the market data needed to build the trade that the node represents.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> requirements for the market data needed to build a trade representing the instrument at the node </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> requirements();
	  ISet<MarketDataId<object>> requirements();

	  /// <summary>
	  /// Calculates the date associated with the node.
	  /// <para>
	  /// Each curve node has an associated date which defines the x-value in the curve.
	  /// This date is visible in the <seealso cref="DatedParameterMetadata parameter metadata"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date used when calibrating the curve </param>
	  /// <param name="refData">  the reference data to use to resolve the trade </param>
	  /// <returns> the date associated with the node </returns>
	  LocalDate date(LocalDate valuationDate, ReferenceData refData);

	  /// <summary>
	  /// Returns metadata for the node.
	  /// <para>
	  /// This provides curve metadata for the node at the specified valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date used when calibrating the curve </param>
	  /// <param name="refData">  the reference data to use to resolve the trade </param>
	  /// <returns> metadata for the node </returns>
	  DatedParameterMetadata metadata(LocalDate valuationDate, ReferenceData refData);

	  /// <summary>
	  /// Creates a trade representing the instrument at the node.
	  /// <para>
	  /// This uses the observed market data to build the trade that the node represents.
	  /// The reference data is typically used to find the start date of the trade from the valuation date.
	  /// The resulting trade is not resolved.
	  /// The notional of the trade is taken from the 'quantity' variable.
	  /// The quantity is signed and will affect whether the trade is Buy or Sell.
	  /// The valuation date is defined by the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="quantity">  the quantity or notional of the trade </param>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> a trade representing the instrument at the node </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  Trade trade(double quantity, MarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// Creates a resolved trade representing the instrument at the node.
	  /// <para>
	  /// This uses the observed market data to build the trade that the node represents.
	  /// The trade is then resolved using the specified reference data if necessary.
	  /// The valuation date is defined by the market data.
	  /// </para>
	  /// <para>
	  /// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	  /// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="quantity">  the quantity or notional of the trade </param>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <param name="refData">  the reference data, used to resolve the trade </param>
	  /// <returns> a trade representing the instrument at the node </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  ResolvedTrade resolvedTrade(double quantity, MarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// Gets the initial guess used for calibrating the node.
	  /// <para>
	  /// This uses the observed market data to select a suitable initial guess.
	  /// For example, a Fixed-Ibor swap would return the market quote, which is the fixed rate,
	  /// providing that the value type is 'ZeroRate'.
	  /// The valuation date is defined by the market data.
	  /// </para>
	  /// <para>
	  /// This is primarily used as a performance hint. Since the guess is refined by
	  /// calibration, in most cases any suitable number can be returned, such as zero.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <param name="valueType">  the type of y-value that the curve will contain </param>
	  /// <returns> the initial guess of the calibrated value </returns>
	  double initialGuess(MarketData marketData, ValueType valueType);

	}

}
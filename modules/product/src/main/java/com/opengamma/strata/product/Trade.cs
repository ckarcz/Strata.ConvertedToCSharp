/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A trade with additional structured information.
	/// <para>
	/// A trade is a transaction that occurred on a specific date between two counterparties.
	/// For example, an interest rate swap trade agreed on a particular date for
	/// cash-flows in the future.
	/// </para>
	/// <para>
	/// The reference to <seealso cref="TradeInfo"/> captures structured information common to different types of trade.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface Trade : PortfolioItem
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default PortfolioItemSummary summarize()
	//  {
	//	return SummarizerUtils.summary(this, ProductType.OTHER, "Unknown: " + getClass().getSimpleName());
	//  }

	  /// <summary>
	  /// Gets the standard trade information.
	  /// <para>
	  /// All trades contain this standard set of information.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the trade information </returns>
	  TradeInfo Info {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  Trade withInfo(TradeInfo info);

	}

}
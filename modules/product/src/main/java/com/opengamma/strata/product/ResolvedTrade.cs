/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	/// <summary>
	/// A trade that has been resolved for pricing.
	/// <para>
	/// Resolved trades are the primary input to pricers.
	/// </para>
	/// <para>
	/// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
	public interface ResolvedTrade
	{

	  /// <summary>
	  /// Gets the standard information.
	  /// <para>
	  /// All resolved trades contain this standard set of information.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the information </returns>
	  PortfolioItemInfo Info {get;}

	  /// <summary>
	  /// Gets the underlying product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the product </returns>
	  ResolvedProduct Product {get;}

	}

}
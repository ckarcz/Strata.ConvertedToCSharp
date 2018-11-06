/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	/// <summary>
	/// A trade that is directly based on a product.
	/// <para>
	/// A product trade is a <seealso cref="Trade"/> that directly contains a reference to a <seealso cref="Product"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface ProductTrade : Trade
	{

	  /// <summary>
	  /// Gets the underlying product that was agreed when the trade occurred.
	  /// <para>
	  /// The product captures the contracted financial details of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the product </returns>
	  Product Product {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  ProductTrade withInfo(TradeInfo info);

	}

}
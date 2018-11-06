/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	/// <summary>
	/// A trade that is based on security, quantity and price.
	/// <para>
	/// If the trade is directly based on a securitized product, the trade type is <seealso cref="SecuritizedProductTrade"/>.
	/// If not, the financial instrument involved in the trade is represented in alternative form, e.g., <seealso cref="Security"/>.
	/// See individual implementations for more details.
	/// </para>
	/// </summary>
	public interface SecurityQuantityTrade : Trade, SecurityQuantity
	{

	  /// <summary>
	  /// Gets the price that was traded.
	  /// <para>
	  /// This is the unit price agreed when the trade occurred.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the price </returns>
	  double Price {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  SecurityQuantityTrade withInfo(TradeInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  SecurityQuantityTrade withQuantity(double quantity);

	  /// <summary>
	  /// Returns an instance with the specified price.
	  /// </summary>
	  /// <param name="price">  the new price </param>
	  /// <returns> the instance with the specified price </returns>
	  SecurityQuantityTrade withPrice(double price);

	}

}
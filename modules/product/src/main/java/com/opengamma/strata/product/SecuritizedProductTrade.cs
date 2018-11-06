/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// A trade that is directly based on a securitized product.
	/// <para>
	/// This defines a trade in a securitized product.
	/// A securitized product contains the structure of a financial instrument that is traded as a <seealso cref="Security"/>.
	/// See <seealso cref="SecuritizedProduct"/> to understand the difference between a security and a securitized product.
	/// </para>
	/// <para>
	/// When trading securities, the standard trade type is <seealso cref="SecurityTrade"/>.
	/// That trade type relies on securities being looked up in <seealso cref="ReferenceData"/>.
	/// One use for trade types that implement {@code SecuritizedProductTrade} is to price
	/// and hold trades without needing to populate reference data, because the securitized
	/// product representation completely models the trade.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// 
	/// </para>
	/// </summary>
	/// @param <P> the type of securitized product </param>
	public interface SecuritizedProductTrade<P> : ProductTrade, SecurityQuantityTrade, SecuritizedProductPortfolioItem<P> where P : SecuritizedProduct
	{

	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  SecuritizedProductTrade<P> withInfo(TradeInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  SecuritizedProductTrade<P> withQuantity(double quantity);

	  /// <summary>
	  /// Returns an instance with the specified price.
	  /// </summary>
	  /// <param name="price">  the new price </param>
	  /// <returns> the instance with the specified price </returns>
	  SecuritizedProductTrade<P> withPrice(double price);

	}

}
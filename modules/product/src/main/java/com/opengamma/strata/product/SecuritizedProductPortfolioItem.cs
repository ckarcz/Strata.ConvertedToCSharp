/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// A trade that is directly based on a securitized product.
	/// <para>
	/// This defines a trade or position in a securitized product.
	/// A securitized product contains the structure of a financial instrument that is traded as a <seealso cref="Security"/>.
	/// See <seealso cref="SecuritizedProduct"/> to understand the difference between a security and a securitized product.
	/// </para>
	/// <para>
	/// When trading securities, the standard trade type is <seealso cref="SecurityTrade"/> and the
	/// standard position type is <seealso cref="SecurityPosition"/>.
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
	public interface SecuritizedProductPortfolioItem<P> : PortfolioItem, SecurityQuantity where P : SecuritizedProduct
	{

	  /// <summary>
	  /// Gets the product of the security that was traded.
	  /// </summary>
	  /// <returns> the product </returns>
	  P Product {get;}

	  /// <summary>
	  /// Gets the currency of the position.
	  /// <para>
	  /// This is typically the same as the currency of the product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the trading currency </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.Currency getCurrency()
	//  {
	//	return getProduct().getCurrency();
	//  }

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default SecurityId getSecurityId()
	//  {
	//	return getProduct().getSecurityId();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  SecuritizedProductPortfolioItem<P> withQuantity(double quantity);

	}

}
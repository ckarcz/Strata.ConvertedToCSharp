/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// A position that is directly based on a securitized product.
	/// <para>
	/// This defines a position in a securitized product.
	/// A securitized product contains the structure of a financial instrument that is traded as a <seealso cref="Security"/>.
	/// See <seealso cref="SecuritizedProduct"/> to understand the difference between a security and a securitized product.
	/// </para>
	/// <para>
	/// When storing positions, the standard trade type is <seealso cref="SecurityPosition"/>.
	/// That type relies on securities being looked up in <seealso cref="ReferenceData"/>.
	/// One use for positions that implement {@code SecuritizedProductPosition} is to price
	/// and hold positions without needing to populate reference data, because the securitized
	/// product representation completely models the trade.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// 
	/// </para>
	/// </summary>
	/// @param <P> the type of securitized product </param>
	public interface SecuritizedProductPosition<P> : Position, SecuritizedProductPortfolioItem<P> where P : SecuritizedProduct
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default SecurityId getSecurityId()
	//  {
	//	return getProduct().getSecurityId();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  SecuritizedProductPosition<P> withInfo(PositionInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  SecuritizedProductPosition<P> withQuantity(double quantity);

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	/// <summary>
	/// A quantity of a security.
	/// <para>
	/// This is used to represent the total quantity of a <seealso cref="Security"/>.
	/// This is the base interface for <seealso cref="Position"/> and trades in securities, such as <seealso cref="SecurityTrade"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface SecurityQuantity
	{

	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the security identifier </returns>
	  SecurityId SecurityId {get;}

	  /// <summary>
	  /// Gets the quantity of the security.
	  /// <para>
	  /// This returns the <i>net</i> quantity of the underlying security.
	  /// It can be negative if the security is effectively being sold.
	  /// </para>
	  /// <para>
	  /// For a trade, the quantity is positive if the security is being bought and
	  /// negative if being sold. For a position, the quantity is positive if the net
	  /// position is <i>long</i> and negative if the net position is <i>short</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the net quantity of the security that is held </returns>
	  double Quantity {get;}

	}

}
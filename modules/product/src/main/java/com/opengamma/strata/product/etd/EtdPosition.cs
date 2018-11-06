/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// A position in an ETD, where the security is embedded ready for mark-to-market pricing.
	/// <para>
	/// This represents a position in an ETD, defined by long and short quantity.
	/// The ETD security is embedded directly.
	/// </para>
	/// <para>
	/// The net quantity of the position is stored using two fields - {@code longQuantity} and {@code shortQuantity}.
	/// These two fields must not be negative.
	/// In many cases, only a long quantity or short quantity will be present with the other set to zero.
	/// However it is also possible for both to be non-zero, allowing long and short positions to be treated separately.
	/// The net quantity is available via <seealso cref="#getQuantity()"/>.
	/// </para>
	/// </summary>
	public interface EtdPosition : Position
	{

	  /// <summary>
	  /// Gets the currency of the position.
	  /// <para>
	  /// This is the currency of the security.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the trading currency </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.Currency getCurrency()
	//  {
	//	return getSecurity().getCurrency();
	//  }

	  /// <summary>
	  /// Gets the underlying ETD security.
	  /// </summary>
	  /// <returns> the ETD security </returns>
	  EtdSecurity Security {get;}

	  /// <summary>
	  /// Gets the type of the contract - future or option.
	  /// </summary>
	  /// <returns> the type, future or option </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default EtdType getType()
	//  {
	//	return getSecurity().getType();
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the security identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.SecurityId getSecurityId()
	//  {
	//	return getSecurity().getSecurityId();
	//  }

	  /// <summary>
	  /// Gets the net quantity of the security.
	  /// <para>
	  /// This returns the <i>net</i> quantity of the underlying security.
	  /// The result is positive if the net position is <i>long</i> and negative
	  /// if the net position is <i>short</i>.
	  /// </para>
	  /// <para>
	  /// This is calculated by subtracting the short quantity from the long quantity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the net quantity of the underlying security </returns>
	  double Quantity {get;}

	  /// <summary>
	  /// Gets the long quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that is held.
	  /// The quantity cannot be negative, as that would imply short selling.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the long quantity </returns>
	  double LongQuantity {get;}

	  /// <summary>
	  /// Gets the short quantity of the security.
	  /// <para>
	  /// This is the quantity of the underlying security that has been short sold.
	  /// The quantity cannot be negative, as that would imply the position is long.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the short quantity </returns>
	  double ShortQuantity {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  EtdPosition withInfo(PositionInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  EtdPosition withQuantity(double quantity);

	}

}
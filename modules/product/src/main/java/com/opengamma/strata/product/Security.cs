/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// A security that can be traded.
	/// <para>
	/// A security is one of the building blocks of finance, representing a fungible instrument that can be traded.
	/// This is intended to cover instruments such as listed equities, bonds and exchange traded derivatives (ETD).
	/// Within Strata, a financial instrument is a security if it is a standardized
	/// exchange-based contract that can be referenced by a <seealso cref="SecurityId"/>.
	/// The security can be looked up in <seealso cref="ReferenceData"/> using the identifier.
	/// </para>
	/// <para>
	/// It is intended that Over-The-Counter (OTC) instruments, such as an interest rate swap,
	/// are embedded directly within the trade, rather than handled as one-off securities.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface Security
	{

	  /// <summary>
	  /// Gets the standard security information.
	  /// <para>
	  /// All securities contain this standard set of information.
	  /// It includes the identifier, information about the price and an extensible data map.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the security information </returns>
	  SecurityInfo Info {get;}

	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the security identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default SecurityId getSecurityId()
	//  {
	//	return getInfo().getId();
	//  }

	  /// <summary>
	  /// Gets the currency that the security is traded in.
	  /// </summary>
	  /// <returns> the trading currency </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.Currency getCurrency()
	//  {
	//	return getInfo().getPriceInfo().getCurrency();
	//  }

	  /// <summary>
	  /// Gets the set of underlying security identifiers.
	  /// <para>
	  /// The set must contain all the security identifiers that this security directly refers to.
	  /// For example, a bond future will return the identifiers of the underlying basket of bonds,
	  /// but a bond future option will only return the identifier of the underlying future, not the basket.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the underlying security identifiers </returns>
	  ImmutableSet<SecurityId> UnderlyingIds {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  Security withInfo(SecurityInfo info);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the product associated with this security.
	  /// <para>
	  /// The product of a security is distinct from the security.
	  /// The product includes the financial details from this security,
	  /// but excludes the additional information.
	  /// The product also includes the products of any underlying securities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data used to find underlying securities </param>
	  /// <returns> the product </returns>
	  /// <exception cref="UnsupportedOperationException"> if the security does not contain information about the product model </exception>
	  SecuritizedProduct createProduct(ReferenceData refData);

	  /// <summary>
	  /// Creates a trade based on this security.
	  /// <para>
	  /// This creates a trade of a suitable type for this security.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  the trade information </param>
	  /// <param name="quantity">  the number of contracts in the trade </param>
	  /// <param name="tradePrice">  the price agreed when the trade occurred </param>
	  /// <param name="refData">  the reference data used to find underlying securities </param>
	  /// <returns> the trade </returns>
	  SecurityQuantityTrade createTrade(TradeInfo tradeInfo, double quantity, double tradePrice, ReferenceData refData);

	  /// <summary>
	  /// Creates a position based on this security from a net quantity.
	  /// <para>
	  /// This creates a position of a suitable type for this security.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="quantity">  the number of contracts in the position </param>
	  /// <param name="refData">  the reference data used to find underlying securities </param>
	  /// <returns> the position </returns>
	  Position createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData);

	  /// <summary>
	  /// Creates a position based on this security from a long and short quantity.
	  /// <para>
	  /// This creates a position of a suitable type for this security.
	  /// </para>
	  /// <para>
	  /// The long quantity and short quantity must be zero or positive, not negative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="positionInfo">  the position information </param>
	  /// <param name="longQuantity">  the long quantity in the position </param>
	  /// <param name="shortQuantity">  the short quantity in the position </param>
	  /// <param name="refData">  the reference data used to find underlying securities </param>
	  /// <returns> the position </returns>
	  Position createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData);

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// The product details of a financial instrument.
	/// <para>
	/// A product is a high level abstraction applicable to many different types.
	/// For example, an Interest Rate Swap is a product, as is a Forward Rate Agreement (FRA).
	/// </para>
	/// <para>
	/// A product exists independently from a <seealso cref="Trade"/>. It represents the economics of the
	/// financial instrument regardless of the trade date or counterparties.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface Product
	{

	  /// <summary>
	  /// Checks if this product is cross-currency.
	  /// <para>
	  /// A cross currency product is defined as one that refers to two or more currencies.
	  /// Any product with direct or indirect FX exposure will be cross-currency.
	  /// </para>
	  /// <para>
	  /// For example, a fixed/Ibor swap in USD observing USD-LIBOR is not cross currency,
	  /// but one that observes EURIBOR is cross currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if cross currency </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isCrossCurrency()
	//  {
	//	return allCurrencies().size() > 1;
	//  }

	  /// <summary>
	  /// Returns the set of currencies that the product pays in.
	  /// <para>
	  /// This returns the complete set of payment currencies.
	  /// This will typically return one or two currencies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of payment currencies </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> allPaymentCurrencies()
	//  {
	//	return allCurrencies();
	//  }

	  /// <summary>
	  /// Returns the set of currencies the product refers to.
	  /// <para>
	  /// This returns the complete set of currencies, not just the payment currencies.
	  /// For example, the sets will differ when one of the currencies is non-deliverable.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of currencies the product refers to </returns>
	  ImmutableSet<Currency> allCurrencies();

	}

}
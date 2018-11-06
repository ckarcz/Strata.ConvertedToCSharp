/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// The product details of a financial instrument that is traded as a security.
	/// <para>
	/// A securitized product contains the structure of a financial instrument that is traded as a <seealso cref="Security"/>.
	/// The product of a security is distinct from the security itself.
	/// A <seealso cref="Security"/> contains details about itself, with any underlying securities
	/// referred to by <seealso cref="SecurityId identifier"/>.
	/// By contrast, the product contains the full model for pricing, including underlying products.
	/// </para>
	/// <para>
	/// For example, the securitized product of a bond future option directly contains all
	/// the details of the future and the basket of bonds. Whereas, a bond future option security
	/// only contains details of the option and an identifier referring to the future.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface SecuritizedProduct : Product
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
	  /// Gets the currency that the security is traded in.
	  /// </summary>
	  /// <returns> the trading currency </returns>
	  Currency Currency {get;}

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> allCurrencies()
	//  {
	//	return ImmutableSet.of(getCurrency());
	//  }

	}

}
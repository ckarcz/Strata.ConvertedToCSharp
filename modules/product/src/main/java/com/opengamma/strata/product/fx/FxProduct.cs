/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;

	/// <summary>
	/// A foreign exchange product, such as an FX forward, FX spot or FX option.
	/// <para>
	/// FX products operate on two different currencies.
	/// For example, it might represent the payment of USD 1,000 and the receipt of EUR 932.
	/// </para>
	/// </summary>
	public interface FxProduct : Product
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isCrossCurrency()
	//  {
	//	return true;
	//  }

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableSet<com.opengamma.strata.basics.currency.Currency> allCurrencies()
	//  {
	//	return getCurrencyPair().toSet();
	//  }

	  /// <summary>
	  /// Gets the currency pair that the FX trade is based on, in conventional order.
	  /// <para>
	  /// This represents the main currency pair of the FX. If the trade settles in a
	  /// third currency, that is not recorded here.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  CurrencyPair CurrencyPair {get;}

	}

}
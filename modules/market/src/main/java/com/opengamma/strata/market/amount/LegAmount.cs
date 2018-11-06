/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.amount
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;

	/// <summary>
	/// Represents an amount of a currency associated with one leg of an instrument.
	/// </summary>
	public interface LegAmount : FxConvertible<LegAmount>
	{

	  /// <summary>
	  /// Gets the amount associated with the leg.
	  /// </summary>
	  /// <returns>  the amount </returns>
	  CurrencyAmount Amount {get;}

	}

}
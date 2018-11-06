/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating credit default swap trades.
	/// </summary>
	public interface CdsTemplate : TradeTemplate
	{

	  /// <summary>
	  /// Gets the market convention of the credit default swap.
	  /// </summary>
	  /// <returns> the convention </returns>
	  CdsConvention Convention {get;}

	  /// <summary>
	  /// Creates a trade based on this template.
	  /// <para>
	  /// This returns a trade based on the specified trade date.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the CDS, the protection is received from the counterparty on default, with the fixed coupon being paid.
	  /// If selling the CDS, the protection is paid to the counterparty on default, with the fixed coupon being received.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  CdsTrade createTrade(StandardId legalEntityId, LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, ReferenceData refData);

	  /// <summary>
	  /// Creates a trade based on this template.
	  /// <para>
	  /// This returns a trade based on the specified trade date and upfront fee.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the CDS, the protection is received from the counterparty on default, with the fixed coupon being paid.
	  /// If selling the CDS, the protection is paid to the counterparty on default, with the fixed coupon being received.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <param name="upFrontFee">  the reference data </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  CdsTrade createTrade(StandardId legalEntityId, LocalDate tradeDate, BuySell buySell, double notional, double fixedRate, AdjustablePayment upFrontFee, ReferenceData refData);

	}

}
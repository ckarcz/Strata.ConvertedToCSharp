/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.impl.fpml
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using FpmlDocument = com.opengamma.strata.loader.fpml.FpmlDocument;
	using FpmlParserPlugin = com.opengamma.strata.loader.fpml.FpmlParserPlugin;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;

	/// <summary>
	/// FpML parser for Term Deposits.
	/// <para>
	/// This parser handles the subset of FpML necessary to populate the trade model.
	/// </para>
	/// </summary>
	internal sealed class TermDepositFpmlParserPlugin : FpmlParserPlugin
	{
	  // this class is loaded by ExtendedEnum reflection

	  /// <summary>
	  /// The singleton instance of the parser.
	  /// </summary>
	  public static readonly TermDepositFpmlParserPlugin INSTANCE = new TermDepositFpmlParserPlugin();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private TermDepositFpmlParserPlugin()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// supported elements:
		// 'payerPartyReference'
		// 'receiverPartyReference'
		// 'startDate'
		// 'maturityDate'
		// 'principal'
		// 'fixedRate'
		// 'dayCountFraction'
		// ignored elements:
		// 'payerAccountReference?'
		// 'receiverAccountReference?'
		// 'interest?'
		// rejected elements:
		// 'features?'
		// 'payment*'
		TradeInfoBuilder tradeInfoBuilder = document.parseTradeInfo(tradeEl);
		XmlElement termEl = tradeEl.getChild("termDeposit");
		document.validateNotPresent(termEl, "features");
		document.validateNotPresent(termEl, "payment");
		TermDeposit.Builder termBuilder = TermDeposit.builder();
		// pay/receive and counterparty
		PayReceive payReceive = document.parsePayerReceiver(termEl, tradeInfoBuilder);
		termBuilder.buySell(BuySell.ofBuy(payReceive.Pay));
		// start date
		termBuilder.startDate(document.parseDate(termEl.getChild("startDate")));
		// maturity date
		termBuilder.endDate(document.parseDate(termEl.getChild("maturityDate")));
		// principal
		CurrencyAmount principal = document.parseCurrencyAmount(termEl.getChild("principal"));
		termBuilder.currency(principal.Currency);
		termBuilder.notional(principal.Amount);
		// fixed rate
		termBuilder.rate(document.parseDecimal(termEl.getChild("fixedRate")));
		// day count
		termBuilder.dayCount(document.parseDayCountFraction(termEl.getChild("dayCountFraction")));

		return TermDepositTrade.builder().info(tradeInfoBuilder.build()).product(termBuilder.build()).build();
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return "termDeposit";
		  }
	  }

	}

}
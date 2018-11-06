/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.impl.fpml
{

	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using FpmlDocument = com.opengamma.strata.loader.fpml.FpmlDocument;
	using FpmlParseException = com.opengamma.strata.loader.fpml.FpmlParseException;
	using FpmlParserPlugin = com.opengamma.strata.loader.fpml.FpmlParserPlugin;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using CashSwaptionSettlement = com.opengamma.strata.product.swaption.CashSwaptionSettlement;
	using CashSwaptionSettlementMethod = com.opengamma.strata.product.swaption.CashSwaptionSettlementMethod;
	using PhysicalSwaptionSettlement = com.opengamma.strata.product.swaption.PhysicalSwaptionSettlement;
	using Swaption = com.opengamma.strata.product.swaption.Swaption;
	using SwaptionSettlement = com.opengamma.strata.product.swaption.SwaptionSettlement;
	using SwaptionTrade = com.opengamma.strata.product.swaption.SwaptionTrade;

	/// <summary>
	/// FpML parser for Swaptions.
	/// <para>
	/// This parser handles the subset of FpML necessary to populate the swaption trade model.
	/// </para>
	/// </summary>
	internal sealed class SwaptionFpmlParserPlugin : FpmlParserPlugin
	{

	  /// <summary>
	  /// The singleton instance of the parser.
	  /// </summary>
	  public static readonly SwaptionFpmlParserPlugin INSTANCE = new SwaptionFpmlParserPlugin();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private SwaptionFpmlParserPlugin()
	  {
	  }

	//-------------------------------------------------------------------------
	  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// supported elements:
		//  'swaption'
		//  'swaption/buyerPartyReference'
		//  'swaption/sellerPartyReference'
		//  'swaption/premium/payerPartyReference'
		//  'swaption/premium/receiverPartyReference'
		//  'swaption/premium/paymentAmount'
		//  'swaption/premium/paymentDate'
		//  'swaption/europeanExercise'
		//  'swaption/europeanExercise/expirationDate'
		//  'swaption/europeanExercise/expirationDate/adjustableDate'
		//  'swaption/europeanExercise/expirationDate/adjustableDate/unadjustedDate'
		//  'swaption/europeanExercise/expirationDate/adjustableDate/dateAdjustments'
		//  'swaption/europeanExercise/expirationTime
		//  'swaption/swap'
		// ignored elements:
		//  'Product.model?'
		//  'swaption/calculationAgent'
		//  'swaption/assetClass'
		//  'swaption/primaryAssestClass'
		//  'swaption/productId'
		//  'swaption/productType'
		//  'swaption/secondaryAssetClass'
		//  'swaption/sellerAccountReference'
		//  'swaption/sellerPartyReference'
		//  'swaption/swaptionAdjustedDates'
		//  'swaption/swaptionStraddle'
		TradeInfoBuilder tradeInfoBuilder = document.parseTradeInfo(tradeEl);

		XmlElement swaptionEl = tradeEl.getChild("swaption");
		XmlElement europeanExerciseEl = swaptionEl.getChild("europeanExercise");
		XmlElement expirationTimeEl = europeanExerciseEl.getChild("expirationTime");

		// Parse the premium, expiry date, expiry time and expiry zone, longShort and swaption settlement.
		AdjustablePayment premium = parsePremium(swaptionEl, document, tradeInfoBuilder);
		AdjustableDate expiryDate = parseExpiryDate(europeanExerciseEl, document);
		LocalTime expiryTime = parseExpiryTime(expirationTimeEl, document);
		ZoneId expiryZone = parseExpiryZone(expirationTimeEl, document);
		LongShort longShort = parseLongShort(swaptionEl, document, tradeInfoBuilder);
		SwaptionSettlement swaptionSettlement = parseSettlement(swaptionEl, document);

		//Re use the Swap FpML parser to parse the underlying swap on this swaption.
		SwapFpmlParserPlugin swapParser = SwapFpmlParserPlugin.INSTANCE;
		Swap swap = swapParser.parseSwap(document, swaptionEl, tradeInfoBuilder);

		Swaption swaption = Swaption.builder().expiryDate(expiryDate).expiryZone(expiryZone).expiryTime(expiryTime).longShort(longShort).swaptionSettlement(swaptionSettlement).underlying(swap).build();

		return SwaptionTrade.builder().info(tradeInfoBuilder.build()).product(swaption).premium(premium).build();
	  }

	  private AdjustableDate parseExpiryDate(XmlElement europeanExerciseEl, FpmlDocument document)
	  {
		XmlElement expirationDate = europeanExerciseEl.getChild("expirationDate");
		return expirationDate.findChild("adjustableDate").map(el => document.parseAdjustableDate(el)).get();
	  }

	  private LocalTime parseExpiryTime(XmlElement expirationTimeEl, FpmlDocument document)
	  {
		return document.parseTime(expirationTimeEl.getChild("hourMinuteTime"));
	  }

	  private ZoneId parseExpiryZone(XmlElement expirationTimeEl, FpmlDocument document)
	  {
		string businessCenter = expirationTimeEl.getChild("businessCenter").Content;
		Optional<ZoneId> optionalZoneId = document.getZoneId(businessCenter);
		if (!optionalZoneId.Present)
		{
		  throw new FpmlParseException("Unknown businessCenter" + " attribute value: " + businessCenter);
		}
		return optionalZoneId.get();
	  }

	  private AdjustablePayment parsePremium(XmlElement swaptionEl, FpmlDocument document, TradeInfoBuilder tradeInfoBuilder)
	  {
		XmlElement premiumEl = swaptionEl.getChild("premium");
		PayReceive payReceive = document.parsePayerReceiver(premiumEl, tradeInfoBuilder);
		XmlElement paymentAmountEl = premiumEl.getChild("paymentAmount");
		CurrencyAmount ccyAmount = document.parseCurrencyAmount(paymentAmountEl);
		ccyAmount = payReceive.Pay ? ccyAmount.negated() : ccyAmount;
		AdjustableDate paymentDate = premiumEl.findChild("paymentDate").map(el => document.parseAdjustableDate(el)).get();
		return AdjustablePayment.of(ccyAmount, paymentDate);
	  }

	  private LongShort parseLongShort(XmlElement swaptionEl, FpmlDocument document, TradeInfoBuilder tradeInfoBuilder)
	  {
		BuySell buySell = document.parseBuyerSeller(swaptionEl, tradeInfoBuilder);
		return buySell.Buy ? LongShort.LONG : LongShort.SHORT;
	  }

	  private SwaptionSettlement parseSettlement(XmlElement swaptionEl, FpmlDocument document)
	  {
		Optional<string> optionalCashSettlement = swaptionEl.findAttribute("cashSettlement");
		if (optionalCashSettlement.Present)
		{
		  XmlElement cashSettlementEl = swaptionEl.getChild("cashSettlement");
		  CashSwaptionSettlementMethod method = parseCashSettlementMethod(cashSettlementEl);
		  LocalDate settlementDate = document.parseAdjustedRelativeDateOffset(cashSettlementEl).Unadjusted;
		  return CashSwaptionSettlement.of(settlementDate, method);
		}
		else
		{
		  // treat physical as the default to match FpML examples
		  return PhysicalSwaptionSettlement.DEFAULT;
		}
	  }

	  private CashSwaptionSettlementMethod parseCashSettlementMethod(XmlElement cashSettlementEl)
	  {
		if (cashSettlementEl.findChild("cashPriceAlternateMethod").Present)
		{
		  return CashSwaptionSettlementMethod.CASH_PRICE;

		}
		else if (cashSettlementEl.findChild("parYieldCurveUnadjustedMethod").Present || cashSettlementEl.findChild("parYieldCurveAadjustedMethod").Present)
		{
		  return CashSwaptionSettlementMethod.PAR_YIELD;

		}
		else if (cashSettlementEl.findChild("zeroCouponYieldAdjustedMethod").Present)
		{
		  return CashSwaptionSettlementMethod.ZERO_COUPON_YIELD;

		}
		else
		{
		  throw new FpmlParseException("Invalid swaption cash settlement method: " + cashSettlementEl);
		}
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return "swaption";
		  }
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.impl.fpml
{


	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using ImmutableFxIndex = com.opengamma.strata.basics.index.ImmutableFxIndex;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using FpmlDocument = com.opengamma.strata.loader.fpml.FpmlDocument;
	using FpmlParseException = com.opengamma.strata.loader.fpml.FpmlParseException;
	using FpmlParserPlugin = com.opengamma.strata.loader.fpml.FpmlParserPlugin;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FxNdf = com.opengamma.strata.product.fx.FxNdf;
	using FxNdfTrade = com.opengamma.strata.product.fx.FxNdfTrade;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;

	/// <summary>
	/// FpML parser for single leg FX.
	/// <para>
	/// This parser handles the subset of FpML necessary to populate the trade model.
	/// </para>
	/// </summary>
	internal sealed class FxSingleLegFpmlParserPlugin : FpmlParserPlugin
	{
	  // this class is loaded by ExtendedEnum reflection

	  /// <summary>
	  /// The singleton instance of the parser.
	  /// </summary>
	  public static readonly FxSingleLegFpmlParserPlugin INSTANCE = new FxSingleLegFpmlParserPlugin();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FxSingleLegFpmlParserPlugin()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// supported elements:
		// 'exchangedCurrency1/paymentAmount'
		// 'exchangedCurrency2/paymentAmount'
		// 'valueDate'
		// 'currency1ValueDate'
		// 'currency2ValueDate'
		// 'nonDeliverableSettlement?'
		// ignored elements:
		// 'dealtCurrency?'
		// 'exchangeRate'
		XmlElement fxEl = tradeEl.getChild("fxSingleLeg");
		// amounts
		TradeInfoBuilder tradeInfoBuilder = document.parseTradeInfo(tradeEl);
		XmlElement curr1El = fxEl.getChild("exchangedCurrency1");
		XmlElement curr2El = fxEl.getChild("exchangedCurrency2");
		// pay/receive and counterparty
		PayReceive curr1PayReceive = document.parsePayerReceiver(curr1El, tradeInfoBuilder);
		PayReceive curr2PayReceive = document.parsePayerReceiver(curr2El, tradeInfoBuilder);
		if (curr1PayReceive == curr2PayReceive)
		{
		  throw new FpmlParseException("FX single leg currencies must not have same Pay/Receive direction");
		}
		// amount
		CurrencyAmount curr1Amount = document.parseCurrencyAmount(curr1El.getChild("paymentAmount"));
		CurrencyAmount curr2Amount = document.parseCurrencyAmount(curr2El.getChild("paymentAmount"));
		if (curr1PayReceive == PayReceive.PAY)
		{
		  curr1Amount = curr1Amount.negative();
		  curr2Amount = curr2Amount.positive();
		}
		else
		{
		  curr1Amount = curr1Amount.positive();
		  curr2Amount = curr2Amount.negative();
		}
		// payment date
		LocalDate currency1Date = document.parseDate(fxEl.findChild("currency1ValueDate").orElseGet(() => fxEl.getChild("valueDate")));
		LocalDate currency2Date = document.parseDate(fxEl.findChild("currency2ValueDate").orElseGet(() => fxEl.getChild("valueDate")));
		// FxSingle or NDF
		Optional<XmlElement> ndfEl = fxEl.findChild("nonDeliverableSettlement");
		if (!ndfEl.Present)
		{
		  return FxSingleTrade.builder().info(tradeInfoBuilder.build()).product(FxSingle.of(Payment.of(curr1Amount, currency1Date), Payment.of(curr2Amount, currency2Date))).build();
		}
		if (!currency1Date.Equals(currency2Date))
		{
		  throw new FpmlParseException("FxNdf only supports a single payment date");
		}
		return parseNdf(document, fxEl, ndfEl.get(), curr1Amount, curr2Amount, currency1Date, tradeInfoBuilder);
	  }

	  private Trade parseNdf(FpmlDocument document, XmlElement fxEl, XmlElement ndfEl, CurrencyAmount curr1Amount, CurrencyAmount curr2Amount, LocalDate valueDate, TradeInfoBuilder tradeInfoBuilder)
	  {

		// rate
		XmlElement rateEl = fxEl.getChild("exchangeRate");
		double rate = document.parseDecimal(rateEl.getChild("rate"));
		XmlElement pairEl = rateEl.getChild("quotedCurrencyPair");
		Currency curr1 = document.parseCurrency(pairEl.getChild("currency1"));
		Currency curr2 = document.parseCurrency(pairEl.getChild("currency2"));
		string basis = pairEl.getChild("quoteBasis").Content;
		FxRate fxRate;
		if ("Currency2PerCurrency1".Equals(basis))
		{
		  fxRate = FxRate.of(curr1, curr2, rate);
		}
		else if ("Currency1PerCurrency2".Equals(basis))
		{
		  fxRate = FxRate.of(curr2, curr1, rate);
		}
		else
		{
		  throw new FpmlParseException("Unknown quote basis: " + basis);
		}
		// settlement currency
		Currency settleCurr = document.parseCurrency(ndfEl.getChild("settlementCurrency"));
		CurrencyAmount settleCurrAmount = curr1Amount.Currency.Equals(settleCurr) ? curr1Amount : curr2Amount;
		// index
		XmlElement fixingEl = ndfEl.getChild("fixing"); // only support one of these in pricing model
		LocalDate fixingDate = document.parseDate(fixingEl.getChild("fixingDate"));
		DaysAdjustment offset = DaysAdjustment.ofCalendarDays(Math.toIntExact(valueDate.until(fixingDate, DAYS)));
		XmlElement sourceEl = fixingEl.getChild("fxSpotRateSource"); // required for our model
		XmlElement primarySourceEl = sourceEl.getChild("primaryRateSource");
		string primarySource = primarySourceEl.getChild("rateSource").Content;
		string primaryPage = primarySourceEl.findChild("rateSourcePage").map(e => e.Content).orElse("");
		LocalTime time = document.parseTime(sourceEl.getChild("fixingTime").getChild("hourMinuteTime")); // required for our model
		HolidayCalendarId calendar = document.parseBusinessCenter(sourceEl.getChild("fixingTime").getChild("businessCenter"));
		FxIndex index = ImmutableFxIndex.builder().name(primarySource + "/" + primaryPage + "/" + time).currencyPair(CurrencyPair.of(curr1, curr2)).fixingCalendar(calendar).maturityDateOffset(offset).build();
		return FxNdfTrade.builder().info(tradeInfoBuilder.build()).product(FxNdf.builder().settlementCurrencyNotional(settleCurrAmount).agreedFxRate(fxRate).index(index).paymentDate(valueDate).build()).build();
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return "fxSingleLeg";
		  }
	  }

	}

}
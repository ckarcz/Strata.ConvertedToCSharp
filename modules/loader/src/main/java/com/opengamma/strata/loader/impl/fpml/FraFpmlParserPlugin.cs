using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.impl.fpml
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using FpmlDocument = com.opengamma.strata.loader.fpml.FpmlDocument;
	using FpmlParseException = com.opengamma.strata.loader.fpml.FpmlParseException;
	using FpmlParserPlugin = com.opengamma.strata.loader.fpml.FpmlParserPlugin;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraDiscountingMethod = com.opengamma.strata.product.fra.FraDiscountingMethod;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;

	/// <summary>
	/// FpML parser for FRAs.
	/// <para>
	/// This parser handles the subset of FpML necessary to populate the trade model.
	/// </para>
	/// </summary>
	internal sealed class FraFpmlParserPlugin : FpmlParserPlugin
	{
	  // this class is loaded by ExtendedEnum reflection

	  /// <summary>
	  /// The singleton instance of the parser.
	  /// </summary>
	  public static readonly FraFpmlParserPlugin INSTANCE = new FraFpmlParserPlugin();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FraFpmlParserPlugin()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// supported elements:
		//  'buyerPartyReference'
		//  'sellerPartyReference'
		//  'adjustedTerminationDate'
		//  'paymentDate'
		//  'fixingDateOffset'
		//  'dayCountFraction'
		//  'notional'
		//  'fixedRate'
		//  'floatingRateIndex'
		//  'indexTenor+'
		//  'fraDiscounting'
		// ignored elements:
		//  'Product.model?'
		//  'buyerAccountReference?'
		//  'sellerAccountReference?'
		//  'calculationPeriodNumberOfDays'
		//  'additionalPayment*'
		TradeInfoBuilder tradeInfoBuilder = document.parseTradeInfo(tradeEl);
		XmlElement fraEl = tradeEl.getChild("fra");
		Fra.Builder fraBuilder = Fra.builder();
		// buy/sell and counterparty
		fraBuilder.buySell(document.parseBuyerSeller(fraEl, tradeInfoBuilder));
		// start date
		fraBuilder.startDate(document.parseDate(fraEl.getChild("adjustedEffectiveDate")));
		// end date
		fraBuilder.endDate(document.parseDate(fraEl.getChild("adjustedTerminationDate")));
		// payment date
		fraBuilder.paymentDate(document.parseAdjustableDate(fraEl.getChild("paymentDate")));
		// fixing offset
		fraBuilder.fixingDateOffset(document.parseRelativeDateOffsetDays(fraEl.getChild("fixingDateOffset")));
		// dateRelativeTo required to refer to adjustedEffectiveDate, so ignored here
		// day count
		fraBuilder.dayCount(document.parseDayCountFraction(fraEl.getChild("dayCountFraction")));
		// notional
		CurrencyAmount notional = document.parseCurrencyAmount(fraEl.getChild("notional"));
		fraBuilder.currency(notional.Currency);
		fraBuilder.notional(notional.Amount);
		// fixed rate
		fraBuilder.fixedRate(document.parseDecimal(fraEl.getChild("fixedRate")));
		// index
		IList<Index> indexes = document.parseIndexes(fraEl);
		switch (indexes.Count)
		{
		  case 1:
			fraBuilder.index((IborIndex) indexes[0]);
			break;
		  case 2:
			fraBuilder.index((IborIndex) indexes[0]);
			fraBuilder.indexInterpolated((IborIndex) indexes[1]);
			break;
		  default:
			throw new FpmlParseException("Expected one or two indexes, but found " + indexes.Count);
		}
		// discounting
		fraBuilder.discounting(FraDiscountingMethod.of(fraEl.getChild("fraDiscounting").Content));

		return FraTrade.builder().info(tradeInfoBuilder.build()).product(fraBuilder.build()).build();
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return "fra";
		  }
	  }

	}

}
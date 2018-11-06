/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.impl.fpml
{
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using FpmlDocument = com.opengamma.strata.loader.fpml.FpmlDocument;
	using FpmlParserPlugin = com.opengamma.strata.loader.fpml.FpmlParserPlugin;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using BulletPayment = com.opengamma.strata.product.payment.BulletPayment;
	using BulletPaymentTrade = com.opengamma.strata.product.payment.BulletPaymentTrade;

	/// <summary>
	/// FpML parser for Bullet Payments.
	/// <para>
	/// This parser handles the subset of FpML necessary to populate the trade model.
	/// </para>
	/// </summary>
	internal sealed class BulletPaymentFpmlParserPlugin : FpmlParserPlugin
	{
	  // this class is loaded by ExtendedEnum reflection

	  /// <summary>
	  /// The singleton instance of the parser.
	  /// </summary>
	  public static readonly BulletPaymentFpmlParserPlugin INSTANCE = new BulletPaymentFpmlParserPlugin();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private BulletPaymentFpmlParserPlugin()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// supported elements:
		// 'payment/payerPartyReference'
		// 'payment/receiverPartyReference'
		// 'payment/paymentAmount'
		// 'payment/paymentDate?'
		// ignored elements:
		// 'payment/payerAccountReference?'
		// 'payment/receiverAccountReference?'
		// 'payment/paymentType?'
		// 'payment/settlementInformation?'
		// 'payment/discountFactor?'
		// 'payment/presentValueAmount?'
		TradeInfoBuilder tradeInfoBuilder = document.parseTradeInfo(tradeEl);
		XmlElement bulletEl = tradeEl.getChild("bulletPayment");
		XmlElement paymentEl = bulletEl.getChild("payment");
		BulletPayment.Builder bulletBuilder = BulletPayment.builder();
		// pay/receive and counterparty
		bulletBuilder.payReceive(document.parsePayerReceiver(paymentEl, tradeInfoBuilder));
		// payment date
		bulletBuilder.date(document.parseAdjustableDate(paymentEl.getChild("paymentDate")));
		// amount
		bulletBuilder.value(document.parseCurrencyAmount(paymentEl.getChild("paymentAmount")));

		return BulletPaymentTrade.builder().info(tradeInfoBuilder.build()).product(bulletBuilder.build()).build();
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return "bulletPayment";
		  }
	  }

	}

}
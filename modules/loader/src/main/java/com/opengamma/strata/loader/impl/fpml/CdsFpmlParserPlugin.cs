/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.impl.fpml
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using FpmlDocument = com.opengamma.strata.loader.fpml.FpmlDocument;
	using FpmlParseException = com.opengamma.strata.loader.fpml.FpmlParseException;
	using FpmlParserPlugin = com.opengamma.strata.loader.fpml.FpmlParserPlugin;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using Cds = com.opengamma.strata.product.credit.Cds;
	using CdsIndex = com.opengamma.strata.product.credit.CdsIndex;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;

	/// <summary>
	/// FpML parser for CDS.
	/// <para>
	/// This parser handles the subset of FpML necessary to populate the trade model.
	/// </para>
	/// </summary>
	internal sealed class CdsFpmlParserPlugin : FpmlParserPlugin
	{
	  // this class is loaded by ExtendedEnum reflection

	  /// <summary>
	  /// The singleton instance of the parser.
	  /// </summary>
	  public static readonly CdsFpmlParserPlugin INSTANCE = new CdsFpmlParserPlugin();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private CdsFpmlParserPlugin()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// supported elements:
		//  'generalTerms/effectiveDate'
		//  'generalTerms/scheduledTerminationDate'
		//  'generalTerms/buyerSellerModel'
		//  'generalTerms/dateAdjustments'
		//  'generalTerms/referenceInformation'
		//  'generalTerms/indexReferenceInformation'
		//  'feeLeg/initialPayment'
		//  'feeLeg/periodicPayment'
		//  'protectionTerms/calculationAmount'
		// ignored elements:
		//  'generalTerms/additionalTerm'
		//  'generalTerms/substitution'
		//  'generalTerms/modifiedEquityDelivery'
		//  'feeLeg/periodicPayment/adjustedPaymentDates'
		//  'feeLeg/marketFixedRate'
		//  'feeLeg/paymentDelay'
		//  'feeLeg/initialPoints'
		//  'feeLeg/marketPrice'
		//  'feeLeg/quotationStyle'
		//  'protectionTerms/*'
		//  'cashSettlementTerms'
		//  'physicalSettlementTerms'
		// rejected elements:
		//  'generalTerms/basketReferenceInformation'
		//  'feeLeg/singlePayment'
		TradeInfoBuilder tradeInfoBuilder = document.parseTradeInfo(tradeEl);
		return parseCds(document, tradeEl, tradeInfoBuilder);
	  }

	  // parses the CDS
	  internal Trade parseCds(FpmlDocument document, XmlElement tradeEl, TradeInfoBuilder tradeInfoBuilder)
	  {
		XmlElement cdsEl = tradeEl.getChild("creditDefaultSwap");
		XmlElement generalTermsEl = cdsEl.getChild("generalTerms");
		XmlElement feeLegEl = cdsEl.getChild("feeLeg");
		document.validateNotPresent(generalTermsEl, "basketReferenceInformation");
		document.validateNotPresent(feeLegEl, "singlePayment");
		BuySell buySell = document.parseBuyerSeller(generalTermsEl, tradeInfoBuilder);

		// effective and termination date are optional in FpML but mandatory for Strata
		AdjustableDate effectiveDate = document.parseAdjustableDate(generalTermsEl.getChild("effectiveDate"));
		AdjustableDate terminationDate = document.parseAdjustableDate(generalTermsEl.getChild("scheduledTerminationDate"));
		BusinessDayAdjustment bda = generalTermsEl.findChild("dateAdjustments").map(el => document.parseBusinessDayAdjustments(el)).orElse(BusinessDayAdjustment.NONE);
		PeriodicSchedule.Builder scheduleBuilder = PeriodicSchedule.builder().startDate(effectiveDate.Unadjusted).startDateBusinessDayAdjustment(effectiveDate.Adjustment).endDate(terminationDate.Unadjusted).endDateBusinessDayAdjustment(terminationDate.Adjustment).businessDayAdjustment(bda);

		// an upfront fee
		Optional<XmlElement> initialPaymentOptEl = feeLegEl.findChild("initialPayment");
		AdjustablePayment upfrontFee = null;
		if (initialPaymentOptEl.Present)
		{
		  XmlElement initialPaymentEl = initialPaymentOptEl.get();
		  PayReceive payRec = document.parsePayerReceiver(initialPaymentEl, tradeInfoBuilder);
		  CurrencyAmount amount = document.parseCurrencyAmount(initialPaymentEl.getChild("paymentAmount"));
		  LocalDate date = initialPaymentEl.findChild("adjustablePaymentDate").map(el => document.parseDate(el)).orElse(effectiveDate.Unadjusted);
		  AdjustableDate adjDate = AdjustableDate.of(date, bda);
		  upfrontFee = payRec.Pay ? AdjustablePayment.ofPay(amount, adjDate) : AdjustablePayment.ofReceive(amount, adjDate);
		}

		// we require a periodicPayment and fixedAmountCalculation
		XmlElement periodicPaymentEl = feeLegEl.getChild("periodicPayment");
		scheduleBuilder.frequency(periodicPaymentEl.findChild("paymentFrequency").map(el => document.parseFrequency(el)).orElse(Frequency.P3M));
		periodicPaymentEl.findChild("firstPaymentDate").ifPresent(el => scheduleBuilder.firstRegularStartDate(document.parseDate(el)));
		periodicPaymentEl.findChild("firstPeriodStartDate").ifPresent(el => scheduleBuilder.overrideStartDate(AdjustableDate.of(document.parseDate(el))));
		periodicPaymentEl.findChild("lastRegularPaymentDate").ifPresent(el => scheduleBuilder.lastRegularEndDate(document.parseDate(el)));
		scheduleBuilder.rollConvention(periodicPaymentEl.findChild("rollConvention").map(el => document.convertRollConvention(el.Content)).orElse(null));
		XmlElement fixedAmountCalcEl = periodicPaymentEl.getChild("fixedAmountCalculation");
		double fixedRate = document.parseDecimal(fixedAmountCalcEl.getChild("fixedRate"));
		DayCount dayCount = fixedAmountCalcEl.findChild("dayCountFraction").map(el => document.parseDayCountFraction(el)).orElse(DayCounts.ACT_360);

		// handle a single protectionTerms element
		XmlElement protectionTermEl = cdsEl.getChild("protectionTerms");
		CurrencyAmount notional = document.parseCurrencyAmount(protectionTermEl.getChild("calculationAmount"));

		// single name CDS
		Optional<XmlElement> singleOptEl = generalTermsEl.findChild("referenceInformation");
		if (singleOptEl.Present)
		{
		  // we require a single entityId
		  XmlElement referenceEntityEl = singleOptEl.get().getChild("referenceEntity");
		  XmlElement entityIdEl = referenceEntityEl.getChild("entityId");
		  string scheme = entityIdEl.findAttribute("entityIdScheme").orElse("http://www.fpml.org/coding-scheme/external/entity-id-RED-1-0");
		  string value = entityIdEl.Content;
		  StandardId entityId = StandardId.of(scheme, value);
		  Cds cds = Cds.builder().buySell(buySell).legalEntityId(entityId).currency(notional.Currency).notional(notional.Amount).paymentSchedule(scheduleBuilder.build()).fixedRate(fixedRate).dayCount(dayCount).build();
		  return CdsTrade.builder().info(tradeInfoBuilder.build()).product(cds).upfrontFee(upfrontFee).build();
		}

		// CDS index
		Optional<XmlElement> indexOptEl = generalTermsEl.findChild("indexReferenceInformation");
		if (indexOptEl.Present)
		{
		  string indexName = indexOptEl.get().getChild("indexName").Content;
		  CdsIndex cdsIndex = CdsIndex.builder().buySell(buySell).cdsIndexId(StandardId.of("CDX-Name", indexName)).currency(notional.Currency).notional(notional.Amount).paymentSchedule(scheduleBuilder.build()).fixedRate(fixedRate).dayCount(dayCount).build();
		  return CdsIndexTrade.builder().info(tradeInfoBuilder.build()).product(cdsIndex).upfrontFee(upfrontFee).build();
		}

		// unknown type
		throw new FpmlParseException("FpML CDS must be single name or index");
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return "creditDefaultSwap";
		  }
	  }

	}

}
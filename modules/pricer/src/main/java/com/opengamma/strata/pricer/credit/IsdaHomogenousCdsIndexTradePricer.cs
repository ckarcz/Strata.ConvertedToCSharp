/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;

	/// <summary>
	/// Pricer for CDS portfolio index trade based on ISDA standard model. 
	/// <para>
	/// The underlying CDS index product is priced as a single name CDS using a single credit curve rather than 
	/// credit curves of constituent single names. 
	/// </para>
	/// </summary>
	public class IsdaHomogenousCdsIndexTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IsdaHomogenousCdsIndexTradePricer DEFAULT = new IsdaHomogenousCdsIndexTradePricer();

	  /// <summary>
	  /// The product pricer.
	  /// </summary>
	  private readonly IsdaHomogenousCdsIndexProductPricer productPricer;
	  /// <summary>
	  /// The upfront fee pricer.
	  /// </summary>
	  private readonly DiscountingPaymentPricer upfrontPricer;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The default constructor.
	  /// <para>
	  /// The default pricers are used.
	  /// </para>
	  /// </summary>
	  public IsdaHomogenousCdsIndexTradePricer()
	  {
		this.productPricer = IsdaHomogenousCdsIndexProductPricer.DEFAULT;
		this.upfrontPricer = DiscountingPaymentPricer.DEFAULT;
	  }

	  /// <summary>
	  /// The constructor with the accrual-on-default formula specified.
	  /// </summary>
	  /// <param name="formula">  the accrual-on-default formula </param>
	  public IsdaHomogenousCdsIndexTradePricer(AccrualOnDefaultFormula formula)
	  {
		this.productPricer = new IsdaHomogenousCdsIndexProductPricer(formula);
		this.upfrontPricer = DiscountingPaymentPricer.DEFAULT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual-on-default formula used in this pricer. 
	  /// </summary>
	  /// <returns> the formula </returns>
	  public virtual AccrualOnDefaultFormula AccrualOnDefaultFormula
	  {
		  get
		  {
			return productPricer.AccrualOnDefaultFormula;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the underlying product, which is the present value per unit notional. 
	  /// <para>
	  /// This method can calculate the clean or dirty price, see <seealso cref="PriceType"/>. 
	  /// If calculating the clean price, the accrued interest is calculated based on the step-in date.
	  /// </para>
	  /// <para>
	  /// This is coherent to <seealso cref="#presentValueOnSettle(ResolvedCdsIndexTrade, CreditRatesProvider, PriceType, ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the price </returns>
	  public virtual double price(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, PriceType priceType, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.price(trade.Product, ratesProvider, settlementDate, priceType, refData);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the underlying product. 
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities priceSensitivity(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.priceSensitivity(trade.Product, ratesProvider, settlementDate, refData).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread of the underlying product.
	  /// <para>
	  /// The par spread is a coupon rate such that the clean price is 0. 
	  /// The result is represented in decimal form. 
	  /// </para>
	  /// <para>
	  /// This is coherent to <seealso cref="#price(ResolvedCdsIndexTrade, CreditRatesProvider, PriceType, ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.parSpread(trade.Product, ratesProvider, settlementDate, refData);
	  }

	  /// <summary>
	  /// Calculates the par spread sensitivity of the underling product.
	  /// <para>
	  /// The par spread sensitivity of the product is the sensitivity of par spread to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.parSpreadSensitivity(trade.Product, ratesProvider, settlementDate, refData).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the trade.
	  /// <para>
	  /// The present value of the product is based on the valuation date.
	  /// </para>
	  /// <para>
	  /// This method can calculate the clean or dirty present value, see <seealso cref="PriceType"/>. 
	  /// If calculating the clean value, the accrued interest is calculated based on the step-in date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the price </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, PriceType priceType, ReferenceData refData)
	  {

		CurrencyAmount pvProduct = productPricer.presentValue(trade.Product, ratesProvider, ratesProvider.ValuationDate, priceType, refData);
		if (!trade.UpfrontFee.Present)
		{
		  return pvProduct;
		}
		Payment upfront = trade.UpfrontFee.get();
		CurrencyAmount pvUpfront = upfrontPricer.presentValue(upfront, ratesProvider.discountFactors(upfront.Currency).toDiscountFactors());
		return pvProduct.plus(pvUpfront);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the trade. 
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		PointSensitivityBuilder pvSensiProduct = productPricer.presentValueSensitivity(trade.Product, ratesProvider, ratesProvider.ValuationDate, refData);
		if (!trade.UpfrontFee.Present)
		{
		  return pvSensiProduct.build();
		}
		Payment upfront = trade.UpfrontFee.get();
		PointSensitivityBuilder pvUpfront = upfrontPricer.presentValueSensitivity(upfront, ratesProvider.discountFactors(upfront.Currency).toDiscountFactors());
		return pvSensiProduct.combinedWith(pvUpfront).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the underlying product. 
	  /// <para>
	  /// The present value is computed based on the settlement date rather than the valuation date.
	  /// </para>
	  /// <para>
	  /// This method can calculate the clean or dirty present value, see <seealso cref="PriceType"/>. 
	  /// If calculating the clean value, the accrued interest is calculated based on the step-in date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the price </returns>
	  public virtual CurrencyAmount presentValueOnSettle(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, PriceType priceType, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.presentValue(trade.Product, ratesProvider, settlementDate, priceType, refData);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the underlying product. 
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of present value to the underlying curves.
	  /// The present value sensitivity is computed based on the settlement date rather than the valuation date.
	  /// </para>
	  /// <para>
	  /// This is coherent to <seealso cref="#presentValueOnSettle(ResolvedCdsIndexTrade, CreditRatesProvider, PriceType, ReferenceData)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueOnSettleSensitivity(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.presentValueSensitivity(trade.Product, ratesProvider, settlementDate, refData).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the risky PV01 of the underlying product. 
	  /// <para>
	  /// RPV01 is defined as minus of the present value sensitivity to coupon rate.
	  /// </para>
	  /// <para>
	  /// This is computed based on the settlement date rather than the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference date </param>
	  /// <returns> the RPV01 </returns>
	  public virtual CurrencyAmount rpv01OnSettle(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, PriceType priceType, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.rpv01(trade.Product, ratesProvider, settlementDate, priceType, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the recovery01 of the underlying product.
	  /// <para>
	  /// The recovery01 is defined as the present value sensitivity to the recovery rate.
	  /// Since the ISDA standard model requires the recovery rate to be constant throughout the lifetime of the CDS,  
	  /// one currency amount is returned by this method.
	  /// </para>
	  /// <para>
	  /// This is computed based on the settlement date rather than the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the recovery01 </returns>
	  public virtual CurrencyAmount recovery01OnSettle(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.recovery01(trade.Product, ratesProvider, settlementDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the jump-to-default of the underlying product.
	  /// <para>
	  /// The jump-to-default is the value of the product in case of immediate default of a constituent single name.
	  /// </para>
	  /// <para>
	  /// Under the homogeneous pool assumption, the jump-to-default values are the same for all of the undefaulted names, 
	  /// and zero for defaulted names. Thus the resulting object contains a single number.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the recovery01 </returns>
	  public virtual JumpToDefault jumpToDefault(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		LocalDate settlementDate = calculateSettlementDate(trade, ratesProvider, refData);
		return productPricer.jumpToDefault(trade.Product, ratesProvider, settlementDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the expected loss of the underlying product.
	  /// <para>
	  /// The expected loss is the (undiscounted) expected default settlement value paid by the protection seller. 
	  /// The resulting value is always positive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the recovery01 </returns>
	  public virtual CurrencyAmount expectedLoss(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider)
	  {

		return productPricer.expectedLoss(trade.Product, ratesProvider);
	  }

	  //-------------------------------------------------------------------------
	  private LocalDate calculateSettlementDate(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		return trade.Info.SettlementDate.orElse(trade.Product.calculateSettlementDateFromValuation(ratesProvider.ValuationDate, refData));
	  }

	}

}
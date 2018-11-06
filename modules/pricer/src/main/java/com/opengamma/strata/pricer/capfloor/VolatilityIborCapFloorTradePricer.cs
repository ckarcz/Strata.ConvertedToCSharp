/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborCapFloorTrade = com.opengamma.strata.product.capfloor.IborCapFloorTrade;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;

	/// <summary>
	/// Pricer for cap/floor trades based on volatilities.
	/// <para>
	/// This function provides the ability to price <seealso cref="IborCapFloorTrade"/>. 
	/// The pricing methodologies are defined in individual implementations of the
	/// volatilities, <seealso cref="IborCapletFloorletVolatilities"/>.
	/// </para>
	/// <para>
	/// Greeks of the underlying product are computed in the product pricer, <seealso cref="VolatilityIborCapFloorProductPricer"/>.
	/// </para>
	/// </summary>
	public class VolatilityIborCapFloorTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilityIborCapFloorTradePricer DEFAULT = new VolatilityIborCapFloorTradePricer(VolatilityIborCapFloorProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);
	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedIborCapFloor"/>.
	  /// </summary>
	  private readonly VolatilityIborCapFloorProductPricer productPricer;
	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedIborCapFloor"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public VolatilityIborCapFloorTradePricer(VolatilityIborCapFloorProductPricer productPricer, DiscountingPaymentPricer paymentPricer)
	  {

		this.productPricer = ArgChecker.notNull(productPricer, "productPricer");
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  /// <summary>
	  /// Gets the payment pricer.
	  /// </summary>
	  /// <returns> the payment pricer </returns>
	  protected internal virtual DiscountingPaymentPricer PaymentPricer
	  {
		  get
		  {
			return paymentPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor cap/floor trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The cap/floor leg and pay leg are typically in the same currency, thus the
	  /// present value gamma is expressed as a single currency amount in most cases.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		MultiCurrencyAmount pvProduct = productPricer.presentValue(trade.Product, ratesProvider, volatilities);
		if (!trade.Premium.Present)
		{
		  return pvProduct;
		}
		CurrencyAmount pvPremium = paymentPricer.presentValue(trade.Premium.get(), ratesProvider);
		return pvProduct.plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value rates sensitivity of the Ibor cap/floor trade.
	  /// <para>
	  /// The present value rates sensitivity of the trade is the sensitivity
	  /// of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivityRates(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivityBuilder pvSensiProduct = productPricer.presentValueSensitivityRates(trade.Product, ratesProvider, volatilities);
		if (!trade.Premium.Present)
		{
		  return pvSensiProduct.build();
		}
		PointSensitivityBuilder pvSensiPremium = paymentPricer.presentValueSensitivity(trade.Premium.get(), ratesProvider);
		return pvSensiProduct.combinedWith(pvSensiPremium).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value volatility sensitivity of the Ibor cap/floor product.
	  /// <para>
	  /// The present value volatility sensitivity of the product is the sensitivity
	  /// of the present value to the volatility values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return productPricer.presentValueSensitivityModelParamsVolatility(trade.Product, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the Ibor cap/floor trade.
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		MultiCurrencyAmount ceProduct = productPricer.currencyExposure(trade.Product, ratesProvider, volatilities);
		if (!trade.Premium.Present)
		{
		  return ceProduct;
		}
		CurrencyAmount pvPremium = paymentPricer.presentValue(trade.Premium.get(), ratesProvider);
		return ceProduct.plus(pvPremium);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current cash of the Ibor cap/floor trade.
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		MultiCurrencyAmount ccProduct = productPricer.currentCash(trade.Product, ratesProvider, volatilities);
		if (!trade.Premium.Present)
		{
		  return ccProduct;
		}
		Payment premium = trade.Premium.get();
		if (premium.Date.Equals(ratesProvider.ValuationDate))
		{
		  ccProduct = ccProduct.plus(premium.Currency, premium.Amount);
		}
		return ccProduct;
	  }

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;
	using ResolvedIborCapFloorTrade = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorTrade;

	/// <summary>
	/// Pricer for cap/floor trades in SABR model.
	/// </summary>
	public class SabrIborCapFloorTradePricer : VolatilityIborCapFloorTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly SabrIborCapFloorTradePricer DEFAULT = new SabrIborCapFloorTradePricer(SabrIborCapFloorProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// The pricer for <seealso cref="ResolvedIborCapFloor"/>.
	  /// </summary>
	  private readonly SabrIborCapFloorProductPricer productPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedIborCapFloor"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public SabrIborCapFloorTradePricer(SabrIborCapFloorProductPricer productPricer, DiscountingPaymentPricer paymentPricer) : base(productPricer, paymentPricer)
	  {
		this.productPricer = productPricer;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value rates sensitivity of the Ibor cap/floor trade.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky model parameter" style, i.e. the sensitivity to the 
	  /// curve nodes with the SABR model parameters unchanged. This sensitivity does not include a potential 
	  /// re-calibration of the model parameters to the raw market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivities presentValueSensitivityRatesStickyModel(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivityBuilder pvSensiProduct = productPricer.presentValueSensitivityRatesStickyModel(trade.Product, ratesProvider, volatilities);
		if (!trade.Premium.Present)
		{
		  return pvSensiProduct.build();
		}
		PointSensitivityBuilder pvSensiPremium = PaymentPricer.presentValueSensitivity(trade.Premium.get(), ratesProvider);
		return pvSensiProduct.combinedWith(pvSensiPremium).build();
	  }

	  /// <summary>
	  /// Calculates the present value volatility sensitivity of the Ibor cap/floor trade.
	  /// <para>
	  /// The sensitivity of the present value to the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the Ibor cap/floor trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(ResolvedIborCapFloorTrade trade, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		return productPricer.presentValueSensitivityModelParamsSabr(trade.Product, ratesProvider, volatilities);
	  }

	}

}
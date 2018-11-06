/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Pricer for cap/floor products in SABR model.
	/// </summary>
	public class SabrIborCapFloorProductPricer : VolatilityIborCapFloorProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly SabrIborCapFloorProductPricer DEFAULT = new SabrIborCapFloorProductPricer(SabrIborCapFloorLegPricer.DEFAULT, DiscountingSwapLegPricer.DEFAULT);

	  /// <summary>
	  /// The leg pricer.
	  /// </summary>
	  private readonly SabrIborCapFloorLegPricer capFloorLegPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="capFloorLegPricer">  the pricer for <seealso cref="IborCapFloorLeg"/> </param>
	  /// <param name="payLegPricer">  the pricer for <seealso cref="SwapLeg"/> </param>
	  public SabrIborCapFloorProductPricer(SabrIborCapFloorLegPricer capFloorLegPricer, DiscountingSwapLegPricer payLegPricer) : base(capFloorLegPricer, payLegPricer)
	  {
		this.capFloorLegPricer = capFloorLegPricer;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value rates sensitivity of the Ibor cap/floor product.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky model parameter" style, i.e. the sensitivity to the 
	  /// curve nodes with the SABR model parameters unchanged. This sensitivity does not include a potential 
	  /// re-calibration of the model parameters to the raw market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyModel(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivityBuilder pvSensiCapFloorLeg = capFloorLegPricer.presentValueSensitivityRatesStickyModel(capFloor.CapFloorLeg, ratesProvider, volatilities);
		if (!capFloor.PayLeg.Present)
		{
		  return pvSensiCapFloorLeg;
		}
		PointSensitivityBuilder pvSensiPayLeg = PayLegPricer.presentValueSensitivity(capFloor.PayLeg.get(), ratesProvider);
		return pvSensiCapFloorLeg.combinedWith(pvSensiPayLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value volatility sensitivity of the Ibor cap/floor product.
	  /// <para>
	  /// The sensitivity of the present value to the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		return capFloorLegPricer.presentValueSensitivityModelParamsSabr(capFloor.CapFloorLeg, ratesProvider, volatilities);
	  }

	}

}
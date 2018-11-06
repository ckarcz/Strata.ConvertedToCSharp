/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Pricer for cap/floor legs in SABR model.
	/// </summary>
	public class SabrIborCapFloorLegPricer : VolatilityIborCapFloorLegPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly SabrIborCapFloorLegPricer DEFAULT = new SabrIborCapFloorLegPricer(SabrIborCapletFloorletPeriodPricer.DEFAULT);

	  /// <summary>
	  /// The period pricer.
	  /// </summary>
	  private readonly SabrIborCapletFloorletPeriodPricer periodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="periodPricer">  the pricer for <seealso cref="IborCapletFloorletPeriod"/>. </param>
	  public SabrIborCapFloorLegPricer(SabrIborCapletFloorletPeriodPricer periodPricer) : base(periodPricer)
	  {
		this.periodPricer = periodPricer;
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the Ibor cap/floor leg to the rate curves.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky model parameter" style, i.e. the sensitivity to the 
	  /// curve nodes with the SABR model parameters unchanged. This sensitivity does not include a potential 
	  /// re-calibration of the model parameters to the raw market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyModel(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueSensitivityRatesStickyModel(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.combinedWith(c2)).get();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity to the SABR model parameters of the Ibor cap/floor.
	  /// <para>
	  /// The sensitivity of the present value to the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities"> the volatilities </param>
	  /// <returns> the point sensitivity to the SABR model parameters </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueSensitivityModelParamsSabr(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.combinedWith(c2)).get();
	  }

	}

}
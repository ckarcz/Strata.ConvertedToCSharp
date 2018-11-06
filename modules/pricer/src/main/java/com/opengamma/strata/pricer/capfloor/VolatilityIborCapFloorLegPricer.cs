/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Pricer for cap/floor legs based on volatilities.
	/// <para>
	/// This function provides the ability to price <seealso cref="ResolvedIborCapFloorLeg"/>. 
	/// One must apply {@code expand()} in order to price <seealso cref="IborCapFloorLeg"/>. 
	/// </para>
	/// <para>
	/// The pricing methodologies are defined in individual implementations of the
	/// volatilities, <seealso cref="IborCapletFloorletVolatilities"/>. 
	/// </para>
	/// </summary>
	public class VolatilityIborCapFloorLegPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilityIborCapFloorLegPricer DEFAULT = new VolatilityIborCapFloorLegPricer(VolatilityIborCapletFloorletPeriodPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="IborCapletFloorletPeriod"/>.
	  /// </summary>
	  private readonly VolatilityIborCapletFloorletPeriodPricer periodPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="periodPricer">  the pricer for <seealso cref="IborCapletFloorletPeriod"/>. </param>
	  public VolatilityIborCapFloorLegPricer(VolatilityIborCapletFloorletPeriodPricer periodPricer)
	  {
		this.periodPricer = ArgChecker.notNull(periodPricer, "periodPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the underlying period pricer. 
	  /// </summary>
	  /// <returns> the period pricer </returns>
	  public virtual VolatilityIborCapletFloorletPeriodPricer PeriodPricer
	  {
		  get
		  {
			return periodPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor cap/floor leg.
	  /// <para>
	  /// The present value of the leg is the value on the valuation date.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValue(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.plus(c2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value delta of the Ibor cap/floor leg.
	  /// <para>
	  /// The present value delta of the leg is the sensitivity value on the valuation date.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value delta </returns>
	  public virtual CurrencyAmount presentValueDelta(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueDelta(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.plus(c2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value gamma of the Ibor cap/floor leg.
	  /// <para>
	  /// The present value gamma of the leg is the sensitivity value on the valuation date.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value gamma </returns>
	  public virtual CurrencyAmount presentValueGamma(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueGamma(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.plus(c2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value theta of the Ibor cap/floor leg.
	  /// <para>
	  /// The present value theta of the leg is the sensitivity value on the valuation date.
	  /// The result is returned using the payment currency of the leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value theta </returns>
	  public virtual CurrencyAmount presentValueTheta(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueTheta(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.plus(c2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value rates sensitivity of the Ibor cap/floor leg.
	  /// <para>
	  /// The present value rates sensitivity of the leg is the sensitivity
	  /// of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value curve sensitivity  </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRates(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueSensitivityRates(period, ratesProvider, volatilities)).Aggregate((p1, p2) => p1.combinedWith(p2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value volatility sensitivity of the Ibor cap/floor leg.
	  /// <para>
	  /// The present value volatility sensitivity of the leg is the sensitivity
	  /// of the present value to the volatility values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value volatility sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Select(period => periodPricer.presentValueSensitivityModelParamsVolatility(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.combinedWith(c2)).get();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current cash of the Ibor cap/floor leg.
	  /// </summary>
	  /// <param name="capFloorLeg">  the Ibor cap/floor leg </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedIborCapFloorLeg capFloorLeg, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		return capFloorLeg.CapletFloorletPeriods.Where(period => period.PaymentDate.Equals(ratesProvider.ValuationDate)).Select(period => periodPricer.presentValue(period, ratesProvider, volatilities)).Aggregate((c1, c2) => c1.plus(c2)).orElse(CurrencyAmount.zero(capFloorLeg.Currency));
	  }

	  //-------------------------------------------------------------------------
	  protected internal virtual void validate(RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {
		ArgChecker.isTrue(volatilities.ValuationDate.Equals(ratesProvider.ValuationDate), "volatility and rate data must be for the same date");
	  }

	}

}
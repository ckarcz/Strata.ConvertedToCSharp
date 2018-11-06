/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Pricer for cap/floor products based on volatilities.
	/// <para>
	/// This function provides the ability to price <seealso cref="ResolvedIborCapFloor"/>. 
	/// </para>
	/// <para>
	/// The pricing methodologies are defined in individual implementations of the
	/// volatilities, <seealso cref="IborCapletFloorletVolatilities"/>. 
	/// </para>
	/// </summary>
	public class VolatilityIborCapFloorProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilityIborCapFloorProductPricer DEFAULT = new VolatilityIborCapFloorProductPricer(VolatilityIborCapFloorLegPricer.DEFAULT, DiscountingSwapLegPricer.DEFAULT);
	  /// <summary>
	  /// The pricer for <seealso cref="IborCapFloorLeg"/>.
	  /// </summary>
	  private readonly VolatilityIborCapFloorLegPricer capFloorLegPricer;
	  /// <summary>
	  /// The pricer for <seealso cref="SwapLeg"/>.
	  /// </summary>
	  private readonly DiscountingSwapLegPricer payLegPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="capFloorLegPricer">  the pricer for <seealso cref="IborCapFloorLeg"/> </param>
	  /// <param name="payLegPricer">  the pricer for <seealso cref="SwapLeg"/> </param>
	  public VolatilityIborCapFloorProductPricer(VolatilityIborCapFloorLegPricer capFloorLegPricer, DiscountingSwapLegPricer payLegPricer)
	  {

		this.capFloorLegPricer = ArgChecker.notNull(capFloorLegPricer, "capFloorLegPricer");
		this.payLegPricer = ArgChecker.notNull(payLegPricer, "payLegPricer");
	  }

	  /// <summary>
	  /// Gets the pay leg pricer.
	  /// </summary>
	  /// <returns> the pay leg pricer </returns>
	  protected internal virtual DiscountingSwapLegPricer PayLegPricer
	  {
		  get
		  {
			return payLegPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor cap/floor product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The cap/floor leg and pay leg are typically in the same currency, thus the
	  /// present value gamma is expressed as a single currency amount in most cases.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual MultiCurrencyAmount presentValue(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		CurrencyAmount pvCapFloorLeg = capFloorLegPricer.presentValue(capFloor.CapFloorLeg, ratesProvider, volatilities);
		if (!capFloor.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(pvCapFloorLeg);
		}
		CurrencyAmount pvPayLeg = payLegPricer.presentValue(capFloor.PayLeg.get(), ratesProvider);
		return MultiCurrencyAmount.of(pvCapFloorLeg).plus(pvPayLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value delta of the Ibor cap/floor product.
	  /// <para>
	  /// The present value of the product is the sensitivity value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The cap/floor leg and pay leg are typically in the same currency, thus the
	  /// present value gamma is expressed as a single currency amount in most cases.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value delta </returns>
	  public virtual MultiCurrencyAmount presentValueDelta(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		CurrencyAmount pvCapFloorLeg = capFloorLegPricer.presentValueDelta(capFloor.CapFloorLeg, ratesProvider, volatilities);
		return MultiCurrencyAmount.of(pvCapFloorLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value gamma of the Ibor cap/floor product.
	  /// <para>
	  /// The present value of the product is the sensitivity value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The cap/floor leg and pay leg are typically in the same currency, thus the
	  /// present value gamma is expressed as a single currency amount in most cases.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value gamma </returns>
	  public virtual MultiCurrencyAmount presentValueGamma(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		CurrencyAmount pvCapFloorLeg = capFloorLegPricer.presentValueGamma(capFloor.CapFloorLeg, ratesProvider, volatilities);
		return MultiCurrencyAmount.of(pvCapFloorLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value theta of the Ibor cap/floor product.
	  /// <para>
	  /// The present value of the product is the sensitivity value on the valuation date.
	  /// </para>
	  /// <para>
	  /// The cap/floor leg and pay leg are typically in the same currency, thus the
	  /// present value gamma is expressed as a single currency amount in most cases.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value theta </returns>
	  public virtual MultiCurrencyAmount presentValueTheta(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		CurrencyAmount pvCapFloorLeg = capFloorLegPricer.presentValueTheta(capFloor.CapFloorLeg, ratesProvider, volatilities);
		return MultiCurrencyAmount.of(pvCapFloorLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value rates sensitivity of the Ibor cap/floor product.
	  /// <para>
	  /// The present value rates sensitivity of the product is the sensitivity
	  /// of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRates(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		PointSensitivityBuilder pvSensiCapFloorLeg = capFloorLegPricer.presentValueSensitivityRates(capFloor.CapFloorLeg, ratesProvider, volatilities);
		if (!capFloor.PayLeg.Present)
		{
		  return pvSensiCapFloorLeg;
		}
		PointSensitivityBuilder pvSensiPayLeg = payLegPricer.presentValueSensitivity(capFloor.PayLeg.get(), ratesProvider);
		return pvSensiCapFloorLeg.combinedWith(pvSensiPayLeg);
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
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		return capFloorLegPricer.presentValueSensitivityModelParamsVolatility(capFloor.CapFloorLeg, ratesProvider, volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the Ibor cap/floor product.
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		CurrencyAmount ceCapFloorLeg = capFloorLegPricer.presentValue(capFloor.CapFloorLeg, ratesProvider, volatilities);
		if (!capFloor.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(ceCapFloorLeg);
		}
		MultiCurrencyAmount cePayLeg = payLegPricer.currencyExposure(capFloor.PayLeg.get(), ratesProvider);
		return cePayLeg.plus(ceCapFloorLeg);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the current cash of the Ibor cap/floor product.
	  /// </summary>
	  /// <param name="capFloor">  the Ibor cap/floor product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the current cash </returns>
	  public virtual MultiCurrencyAmount currentCash(ResolvedIborCapFloor capFloor, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		CurrencyAmount ccCapFloorLeg = capFloorLegPricer.currentCash(capFloor.CapFloorLeg, ratesProvider, volatilities);
		if (!capFloor.PayLeg.Present)
		{
		  return MultiCurrencyAmount.of(ccCapFloorLeg);
		}
		CurrencyAmount ccPayLeg = payLegPricer.currentCash(capFloor.PayLeg.get(), ratesProvider);
		return MultiCurrencyAmount.of(ccPayLeg).plus(ccCapFloorLeg);
	  }

	}

}
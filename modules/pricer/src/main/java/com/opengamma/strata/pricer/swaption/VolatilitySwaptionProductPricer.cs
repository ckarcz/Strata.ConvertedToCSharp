/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using SettlementType = com.opengamma.strata.product.common.SettlementType;
	using ResolvedSwaption = com.opengamma.strata.product.swaption.ResolvedSwaption;

	/// <summary>
	/// Pricer for swaptions handling physical and cash par yield settlement based on volatilities.
	/// <para>
	/// The swap underlying the swaption must have a fixed leg on which the forward rate is computed.
	/// The underlying swap must be single currency.
	/// </para>
	/// <para>
	/// The volatility parameters are not adjusted for the underlying swap convention.
	/// </para>
	/// <para>
	/// The value of the swaption after expiry is 0.
	/// For a swaption which has already expired, a negative number is returned by
	/// <seealso cref="SwaptionVolatilities#relativeTime(ZonedDateTime)"/>.
	/// </para>
	/// </summary>
	public class VolatilitySwaptionProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilitySwaptionProductPricer DEFAULT = new VolatilitySwaptionProductPricer(VolatilitySwaptionCashParYieldProductPricer.DEFAULT, VolatilitySwaptionPhysicalProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for cash par yield.
	  /// </summary>
	  private readonly VolatilitySwaptionCashParYieldProductPricer cashParYieldPricer;
	  /// <summary>
	  /// Pricer for physical.
	  /// </summary>
	  private readonly VolatilitySwaptionPhysicalProductPricer physicalPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="cashParYieldPricer">  the pricer for cash par yield </param>
	  /// <param name="physicalPricer">  the pricer for physical </param>
	  public VolatilitySwaptionProductPricer(VolatilitySwaptionCashParYieldProductPricer cashParYieldPricer, VolatilitySwaptionPhysicalProductPricer physicalPricer)
	  {

		this.cashParYieldPricer = ArgChecker.notNull(cashParYieldPricer, "cashParYieldPricer");
		this.physicalPricer = ArgChecker.notNull(physicalPricer, "physicalPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swaption.
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.presentValue(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.presentValue(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the currency exposure of the swaption.
	  /// <para>
	  /// This is equivalent to the present value of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.currencyExposure(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.currencyExposure(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied volatility of the swaption.
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the implied volatility </returns>
	  public virtual double impliedVolatility(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.impliedVolatility(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.impliedVolatility(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value delta of the swaption.
	  /// <para>
	  /// The present value delta is given by {@code pvbp * priceDelta} where {@code priceDelta}
	  /// is the first derivative of the price with respect to forward.
	  /// </para>
	  /// <para>
	  /// The derivative is computed in the formula underlying the volatility (Black or Normal).
	  /// It does not take into account the potential change of implied volatility induced by
	  /// the change of forward. The number computed by this method is closely related to the
	  /// <seealso cref="VolatilitySwaptionProductPricer#presentValueSensitivityRatesStickyStrike"/> method.
	  /// </para>
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value delta </returns>
	  public virtual CurrencyAmount presentValueDelta(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.presentValueDelta(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.presentValueDelta(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value gamma of the swaption.
	  /// <para>
	  /// The present value gamma is given by {@code pvbp * priceGamma} where {@code priceGamma}
	  /// is the second derivative of the price with respect to forward.
	  /// </para>
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value gamma </returns>
	  public virtual CurrencyAmount presentValueGamma(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.presentValueGamma(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.presentValueGamma(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the swaption.
	  /// <para>
	  /// The present value theta is given by {@code pvbp * priceTheta} where {@code priceTheta}
	  /// is the minus of the price sensitivity to {@code timeToExpiry}.
	  /// </para>
	  /// <para>
	  /// The result is expressed using the currency of the swaption.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the present value theta </returns>
	  public virtual CurrencyAmount presentValueTheta(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.presentValueTheta(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.presentValueTheta(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the swaption to the rate curves.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky strike" style, i.e. the sensitivity to the 
	  /// curve nodes with the volatility at the swaption strike unchanged. This sensitivity does not include a potential 
	  /// change of volatility due to the implicit change of forward rate or moneyness.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyStrike(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.presentValueSensitivityRatesStickyStrike(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.presentValueSensitivityRatesStickyStrike(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the implied volatility of the swaption.
	  /// <para>
	  /// The sensitivity to the implied volatility is also called vega.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="swaption">  the swaption </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="swaptionVolatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the implied volatility </returns>
	  public virtual SwaptionSensitivity presentValueSensitivityModelParamsVolatility(ResolvedSwaption swaption, RatesProvider ratesProvider, SwaptionVolatilities swaptionVolatilities)
	  {

		if (isCash(swaption))
		{
		  return cashParYieldPricer.presentValueSensitivityModelParamsVolatility(swaption, ratesProvider, swaptionVolatilities);
		}
		else
		{
		  return physicalPricer.presentValueSensitivityModelParamsVolatility(swaption, ratesProvider, swaptionVolatilities);
		}
	  }

	  //-------------------------------------------------------------------------
	  // is this a cash swaption
	  private bool isCash(ResolvedSwaption product)
	  {
		return product.SwaptionSettlement.SettlementType.Equals(SettlementType.CASH);
	  }

	}

}
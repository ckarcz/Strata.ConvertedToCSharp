using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Pricer for caplet/floorlet based on volatilities.
	/// <para>
	/// The pricing methodologies are defined in individual implementations of the volatilities, <seealso cref="IborCapletFloorletVolatilities"/>. 
	/// </para>
	/// <para>
	/// The value of the caplet/floorlet after expiry is a fixed payoff amount. The value is zero if valuation date is 
	/// after payment date of the caplet/floorlet.
	/// </para>
	/// <para>
	/// The consistency between {@code RatesProvider} and {@code IborCapletFloorletVolatilities} is not checked in this 
	/// class, but validated only once in <seealso cref="VolatilityIborCapFloorLegPricer"/>.
	/// </para>
	/// </summary>
	public class VolatilityIborCapletFloorletPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VolatilityIborCapletFloorletPeriodPricer DEFAULT = new VolatilityIborCapletFloorletPeriodPricer();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the Ibor caplet/floorlet period.
	  /// <para>
	  /// The result is expressed using the currency of the period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		Currency currency = period.Currency;
		if (ratesProvider.ValuationDate.isAfter(period.PaymentDate))
		{
		  return CurrencyAmount.of(currency, 0d);
		}
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		PutCall putCall = period.PutCall;
		double strike = period.Strike;
		double indexRate = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		if (expiry < 0d)
		{ // Option has expired already
		  double sign = putCall.Call ? 1d : -1d;
		  double payoff = Math.Max(sign * (indexRate - strike), 0d);
		  return CurrencyAmount.of(currency, df * payoff * period.YearFraction * period.Notional);
		}
		double volatility = volatilities.volatility(expiry, strike, indexRate);
		double price = df * period.YearFraction * volatilities.price(expiry, putCall, strike, indexRate, volatility);
		return CurrencyAmount.of(currency, price * period.Notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied volatility of the Ibor caplet/floorlet.
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the implied volatility </returns>
	  public virtual double impliedVolatility(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		ArgChecker.isTrue(expiry >= 0d, "Option must be before expiry to compute an implied volatility");
		double forward = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		double strike = period.Strike;
		return volatilities.volatility(expiry, strike, forward);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value delta of the Ibor caplet/floorlet period.
	  /// <para>
	  /// The present value delta is given by the first derivative of the present value with respect to forward.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value delta </returns>
	  public virtual CurrencyAmount presentValueDelta(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		Currency currency = period.Currency;
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(currency, 0d);
		}
		double forward = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		double strike = period.Strike;
		double volatility = volatilities.volatility(expiry, strike, forward);
		PutCall putCall = period.PutCall;
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double priceDelta = df * period.YearFraction * volatilities.priceDelta(expiry, putCall, strike, forward, volatility);
		return CurrencyAmount.of(currency, priceDelta * period.Notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value gamma of the Ibor caplet/floorlet period.
	  /// <para>
	  /// The present value gamma is given by the second derivative of the present value with respect to forward.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value gamma </returns>
	  public virtual CurrencyAmount presentValueGamma(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		Currency currency = period.Currency;
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(currency, 0d);
		}
		double forward = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		double strike = period.Strike;
		double volatility = volatilities.volatility(expiry, strike, forward);
		PutCall putCall = period.PutCall;
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double priceGamma = df * period.YearFraction * volatilities.priceGamma(expiry, putCall, strike, forward, volatility);
		return CurrencyAmount.of(currency, priceGamma * period.Notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value theta of the Ibor caplet/floorlet period.
	  /// <para>
	  /// The present value theta is given by the minus of the present value sensitivity to the {@code timeToExpiry} 
	  /// parameter of the model.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value theta </returns>
	  public virtual CurrencyAmount presentValueTheta(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		Currency currency = period.Currency;
		if (expiry < 0d)
		{ // Option has expired already
		  return CurrencyAmount.of(currency, 0d);
		}
		double forward = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		double strike = period.Strike;
		double volatility = volatilities.volatility(expiry, strike, forward);
		PutCall putCall = period.PutCall;
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double priceTheta = df * period.YearFraction * volatilities.priceTheta(expiry, putCall, strike, forward, volatility);
		return CurrencyAmount.of(currency, priceTheta * period.Notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value rates sensitivity of the Ibor caplet/floorlet.
	  /// <para>
	  /// The present value rates sensitivity of the caplet/floorlet is the sensitivity
	  /// of the present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the present value curve sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRates(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		Currency currency = period.Currency;
		if (ratesProvider.ValuationDate.isAfter(period.PaymentDate))
		{
		  return PointSensitivityBuilder.none();
		}
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		PutCall putCall = period.PutCall;
		double strike = period.Strike;
		double indexRate = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		PointSensitivityBuilder dfSensi = ratesProvider.discountFactors(currency).zeroRatePointSensitivity(period.PaymentDate);
		if (expiry < 0d)
		{ // Option has expired already
		  double sign = putCall.Call ? 1d : -1d;
		  double payoff = Math.Max(sign * (indexRate - strike), 0d);
		  return dfSensi.multipliedBy(payoff * period.YearFraction * period.Notional);
		}
		PointSensitivityBuilder indexRateSensiSensi = ratesProvider.iborIndexRates(period.Index).ratePointSensitivity(period.IborRate.Observation);
		double volatility = volatilities.volatility(expiry, strike, indexRate);
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double factor = period.Notional * period.YearFraction;
		double fwdPv = factor * volatilities.price(expiry, putCall, strike, indexRate, volatility);
		double fwdDelta = factor * volatilities.priceDelta(expiry, putCall, strike, indexRate, volatility);
		return dfSensi.multipliedBy(fwdPv).combinedWith(indexRateSensiSensi.multipliedBy(fwdDelta * df));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value volatility sensitivity of the Ibor caplet/floorlet.
	  /// <para>
	  /// The present value volatility sensitivity of the caplet/floorlet is the sensitivity
	  /// of the present value to the implied volatility.
	  /// </para>
	  /// <para>
	  /// The sensitivity to the implied volatility is also called vega.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the volatility </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(IborCapletFloorletPeriod period, RatesProvider ratesProvider, IborCapletFloorletVolatilities volatilities)
	  {

		validate(volatilities);
		double expiry = volatilities.relativeTime(period.FixingDateTime);
		double strike = period.Strike;
		Currency currency = period.Currency;
		if (expiry <= 0d)
		{ // Option has expired already or at expiry
		  return PointSensitivityBuilder.none();
		}
		double forward = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		double volatility = volatilities.volatility(expiry, strike, forward);
		PutCall putCall = period.PutCall;
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double vega = df * period.YearFraction * volatilities.priceVega(expiry, putCall, strike, forward, volatility);
		return IborCapletFloorletSensitivity.of(volatilities.Name, expiry, strike, forward, currency, vega * period.Notional);
	  }

	  /// <summary>
	  /// Validate the volatilities provider.
	  /// <para>
	  /// This validate method should be overridden such that a correct implementation of
	  /// {@code IborCapletFloorletVolatilities} is used for pricing.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilities">  the volatilities </param>
	  protected internal virtual void validate(IborCapletFloorletVolatilities volatilities)
	  {
	  }

	}

}
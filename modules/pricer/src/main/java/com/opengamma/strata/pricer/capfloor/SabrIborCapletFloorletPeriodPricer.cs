using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.ALPHA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.BETA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.NU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.model.SabrParameterType.RHO;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Pricer for caplet/floorlet in SABR model.
	/// <para>
	/// The value of the caplet/floorlet after expiry is a fixed payoff amount. The value is zero if valuation date is 
	/// after payment date of the caplet/floorlet.
	/// </para>
	/// </summary>
	public class SabrIborCapletFloorletPeriodPricer : VolatilityIborCapletFloorletPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly SabrIborCapletFloorletPeriodPricer DEFAULT = new SabrIborCapletFloorletPeriodPricer();

	  protected internal override void validate(IborCapletFloorletVolatilities volatilities)
	  {
		ArgChecker.isTrue(volatilities is SabrIborCapletFloorletVolatilities, "volatilities must be SABR volatilities");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the Ibor caplet/floorlet to the rate curves.
	  /// <para>
	  /// The present value sensitivity is computed in a "sticky model parameter" style, i.e. the sensitivity to the 
	  /// curve nodes with the SABR model parameters unchanged. This sensitivity does not include a potential 
	  /// re-calibration of the model parameters to the raw market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the rate curves </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyModel(IborCapletFloorletPeriod period, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

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
		double factor = period.Notional * period.YearFraction;
		if (expiry < 0d)
		{ // option expired already, but not yet paid
		  double sign = putCall.Call ? 1d : -1d;
		  double payoff = Math.Max(sign * (indexRate - strike), 0d);
		  return dfSensi.multipliedBy(payoff * factor);
		}
		ValueDerivatives volatilityAdj = volatilities.volatilityAdjoint(expiry, strike, indexRate);
		PointSensitivityBuilder indexRateSensiSensi = ratesProvider.iborIndexRates(period.Index).ratePointSensitivity(period.IborRate.Observation);
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double fwdPv = factor * volatilities.price(expiry, putCall, strike, indexRate, volatilityAdj.Value);
		double fwdDelta = factor * volatilities.priceDelta(expiry, putCall, strike, indexRate, volatilityAdj.Value);
		double fwdVega = factor * volatilities.priceVega(expiry, putCall, strike, indexRate, volatilityAdj.Value);

		return dfSensi.multipliedBy(fwdPv).combinedWith(indexRateSensiSensi.multipliedBy(fwdDelta * df + fwdVega * volatilityAdj.getDerivative(0) * df));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity to the SABR model parameters of the Ibor caplet/floorlet.
	  /// <para>
	  /// The sensitivity of the present value to the SABR model parameters, alpha, beta, rho and nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the Ibor caplet/floorlet period </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the point sensitivity to the SABR model parameters </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsSabr(IborCapletFloorletPeriod period, RatesProvider ratesProvider, SabrIborCapletFloorletVolatilities volatilities)
	  {

		double expiry = volatilities.relativeTime(period.FixingDateTime);
		if (expiry < 0d)
		{ // option expired already
		  return PointSensitivityBuilder.none();
		}
		Currency currency = period.Currency;
		PutCall putCall = period.PutCall;
		double strike = period.Strike;
		double indexRate = ratesProvider.iborIndexRates(period.Index).rate(period.IborRate.Observation);
		double factor = period.Notional * period.YearFraction;
		ValueDerivatives volatilityAdj = volatilities.volatilityAdjoint(expiry, strike, indexRate);
		DoubleArray derivative = volatilityAdj.Derivatives;
		double df = ratesProvider.discountFactor(currency, period.PaymentDate);
		double vega = df * factor * volatilities.priceVega(expiry, putCall, strike, indexRate, volatilityAdj.Value);
		IborCapletFloorletVolatilitiesName name = volatilities.Name;

		return PointSensitivityBuilder.of(IborCapletFloorletSabrSensitivity.of(name, expiry, ALPHA, currency, vega * derivative.get(2)), IborCapletFloorletSabrSensitivity.of(name, expiry, BETA, currency, vega * derivative.get(3)), IborCapletFloorletSabrSensitivity.of(name, expiry, RHO, currency, vega * derivative.get(4)), IborCapletFloorletSabrSensitivity.of(name, expiry, NU, currency, vega * derivative.get(5)));
	  }

	}

}
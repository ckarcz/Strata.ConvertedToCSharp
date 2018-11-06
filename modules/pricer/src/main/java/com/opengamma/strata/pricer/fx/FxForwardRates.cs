/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides access to rates for a currency pair.
	/// <para>
	/// This provides forward rates for a single <seealso cref="Currency pair"/>, such as 'EUR/GBP'.
	/// The forward rate is the conversion rate between two currencies on a fixing date in the future.
	/// </para>
	/// </summary>
	public interface FxForwardRates : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Gets the currency pair.
	  /// <para>
	  /// The the currency pair that the forward rates are for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  CurrencyPair CurrencyPair {get;}

	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// The raw data in this provider is calibrated for this date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  FxForwardRates withParameter(int parameterIndex, double newValue);

	  FxForwardRates withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the forward rate at the specified payment date.
	  /// <para>
	  /// The exchange rate of the currency pair varies over time.
	  /// This method obtains the estimated rate for the payment date.
	  /// </para>
	  /// <para>
	  /// This method specifies which of the two currencies in the currency pair is to be treated
	  /// as the base currency for the purposes of the returned rate.
	  /// If the specified base currency equals the base currency of the currency pair, then
	  /// the rate is simply returned. If the specified base currency equals the counter currency
	  /// of the currency pair, then the inverse rate is returned.
	  /// As such, an amount in the specified base currency can be directly multiplied by the
	  /// returned FX rate to perform FX conversion.
	  /// </para>
	  /// <para>
	  /// To convert an amount in the specified base currency to the other currency,
	  /// multiply it by the returned FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency that the rate should be expressed against </param>
	  /// <param name="referenceDate">  the date to query the rate for </param>
	  /// <returns> the forward rate of the currency pair </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double rate(Currency baseCurrency, LocalDate referenceDate);

	  /// <summary>
	  /// Calculates the point sensitivity of the forward rate at the specified payment date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the points that were queried in the market data.
	  /// The sensitivity refers to the result of <seealso cref="#rate(Currency, LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency that the rate should be expressed against </param>
	  /// <param name="referenceDate">  the date to find the sensitivity for </param>
	  /// <returns> the point sensitivity of the rate </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  PointSensitivityBuilder ratePointSensitivity(Currency baseCurrency, LocalDate referenceDate);

	  /// <summary>
	  /// Calculates the sensitivity of the forward rate to the current FX rate.
	  /// <para>
	  /// This returns the sensitivity to the current FX rate that was used to determine the FX forward rate.
	  /// The sensitivity refers to the result of <seealso cref="#rate(Currency, LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency that the rate should be expressed against </param>
	  /// <param name="referenceDate">  the date to find the sensitivity for </param>
	  /// <returns> the sensitivity of the FX forward rate to the current FX rate </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double rateFxSpotSensitivity(Currency baseCurrency, LocalDate referenceDate);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the parameter sensitivity from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to parameter sensitivity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  CurrencyParameterSensitivities parameterSensitivity(FxForwardSensitivity pointSensitivity);

	  /// <summary>
	  /// Calculates the currency exposure from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to currency exposure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the currency exposure </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  MultiCurrencyAmount currencyExposure(FxForwardSensitivity pointSensitivity);

	}

}
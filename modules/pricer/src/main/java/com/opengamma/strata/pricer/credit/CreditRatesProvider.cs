/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// The rates provider, used to calculate analytic measures.
	/// <para>
	/// The primary usage of this provider is to price credit default swaps on a legal entity.
	/// This includes credit curves, discounting curves and recovery rate curves.
	/// </para>
	/// </summary>
	public interface CreditRatesProvider
	{

	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// The raw data in this provider is calibrated for this date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the survival probabilities for a standard ID and a currency.
	  /// <para>
	  /// If both the standard ID and currency are matched, the relevant {@code LegalEntitySurvivalProbabilities} is returned. 
	  /// </para>
	  /// <para>
	  /// If the valuation date is on the specified date, the survival probability is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the standard ID of legal entity to get the discount factors for </param>
	  /// <param name="currency">  the currency to get the discount factors for </param>
	  /// <returns> the survival probabilities </returns>
	  /// <exception cref="IllegalArgumentException"> if the survival probabilities are not available </exception>
	  LegalEntitySurvivalProbabilities survivalProbabilities(StandardId legalEntityId, Currency currency);

	  /// <summary>
	  /// Gets the discount factors for a currency. 
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency 
	  /// when comparing the valuation date to the specified date. 
	  /// </para>
	  /// <para>
	  /// If the valuation date is on the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to get the discount factors for </param>
	  /// <returns> the discount factors for the specified currency </returns>
	  CreditDiscountFactors discountFactors(Currency currency);

	  /// <summary>
	  /// Gets the recovery rates for a standard ID.
	  /// <para>
	  /// If both the standard ID and currency are matched, the relevant {@code RecoveryRates} is returned. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the standard ID of legal entity to get the discount factors for </param>
	  /// <returns> the recovery rates </returns>
	  /// <exception cref="IllegalArgumentException"> if the recovery rates are not available </exception>
	  RecoveryRates recoveryRates(StandardId legalEntityId);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the parameter sensitivity.
	  /// <para>
	  /// This computes the <seealso cref="CurrencyParameterSensitivities"/> associated with the <seealso cref="PointSensitivities"/>.
	  /// This corresponds to the projection of the point sensitivity to the curve internal parameters representation.
	  /// </para>
	  /// <para>
	  /// The sensitivities handled here are <seealso cref="CreditCurveZeroRateSensitivity"/>, <seealso cref="ZeroRateSensitivity"/>. 
	  /// For the other sensitivity objects, use <seealso cref="RatesProvider"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivity </param>
	  /// <returns> the sensitivity to the curve parameters </returns>
	  CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities);

	  /// <summary>
	  /// Computes the parameter sensitivity for a specific credit curve.
	  /// <para>
	  /// The credit curve is specified by {@code legalEntityId} and {@code currency}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivity </param>
	  /// <param name="legalEntityId">  the legal entity </param>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the sensitivity to the curve parameters </returns>
	  CurrencyParameterSensitivity singleCreditCurveParameterSensitivity(PointSensitivities pointSensitivities, StandardId legalEntityId, Currency currency);

	  /// <summary>
	  /// Computes the parameter sensitivity for a specific discount curve.
	  /// <para>
	  /// The discount curve is specified by {@code currency}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivity </param>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the sensitivity to the curve parameters </returns>
	  CurrencyParameterSensitivity singleDiscountCurveParameterSensitivity(PointSensitivities pointSensitivities, Currency currency);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the market data with the specified name.
	  /// <para>
	  /// This is most commonly used to find a <seealso cref="Curve"/> using a <seealso cref="CurveName"/>.
	  /// If the market data cannot be found, empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="name">  the name to find </param>
	  /// <returns> the market data value, empty if not found </returns>
	  Optional<T> findData<T>(MarketDataName<T> name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this provider to an equivalent {@code ImmutableCreditRatesProvider}.
	  /// </summary>
	  /// <returns> the equivalent immutable legal entity provider </returns>
	  ImmutableCreditRatesProvider toImmutableCreditRatesProvider();

	}

}
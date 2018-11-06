/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// A provider of data for bond pricing, based on repo and issuer discounting.
	/// <para>
	/// This provides the environmental information against which bond pricing occurs,
	/// which is the repo and issuer curves. If the bond is inflation linked, the
	/// price index data is obtained from <seealso cref="RatesProvider"/>.
	/// </para>
	/// <para>
	/// The standard independent implementation is <seealso cref="ImmutableLegalEntityDiscountingProvider"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface LegalEntityDiscountingProvider
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
	  /// Gets the discount factors from a repo curve based on the security ID, issuer ID and currency.
	  /// <para>
	  /// This searches first for a curve associated with the security iD and currency,
	  /// and then for a curve associated with the issuer ID and currency.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="securityId">  the standard ID of security to get the discount factors for </param>
	  /// <param name="issuerId">  the standard ID of legal entity to get the discount factors for </param>
	  /// <param name="currency">  the currency to get the discount factors for </param>
	  /// <returns> the discount factors </returns>
	  /// <exception cref="IllegalArgumentException"> if the discount factors are not available </exception>
	  RepoCurveDiscountFactors repoCurveDiscountFactors(SecurityId securityId, LegalEntityId issuerId, Currency currency);

	  /// <summary>
	  /// Gets the discount factors from a repo curve based on the issuer ID and currency.
	  /// <para>
	  /// This searches for a curve associated with the issuer ID and currency.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="issuerId">  the standard ID of legal entity to get the discount factors for </param>
	  /// <param name="currency">  the currency to get the discount factors for </param>
	  /// <returns> the discount factors </returns>
	  /// <exception cref="IllegalArgumentException"> if the discount factors are not available </exception>
	  RepoCurveDiscountFactors repoCurveDiscountFactors(LegalEntityId issuerId, Currency currency);

	  /// <summary>
	  /// Gets the discount factors from an issuer based on the issuer ID and currency.
	  /// <para>
	  /// This searches for a curve associated with the issuer ID and currency.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="issuerId">  the standard ID to get the discount factors for </param>
	  /// <param name="currency">  the currency to get the discount factors for </param>
	  /// <returns> the discount factors </returns>
	  /// <exception cref="IllegalArgumentException"> if the discount factors are not available </exception>
	  IssuerCurveDiscountFactors issuerCurveDiscountFactors(LegalEntityId issuerId, Currency currency);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the parameter sensitivity.
	  /// <para>
	  /// This computes the <seealso cref="CurrencyParameterSensitivities"/> associated with the <seealso cref="PointSensitivities"/>.
	  /// This corresponds to the projection of the point sensitivity to the curve internal parameters representation.
	  /// </para>
	  /// <para>
	  /// This method handles <seealso cref="RepoCurveZeroRateSensitivity"/> and <seealso cref="IssuerCurveZeroRateSensitivity"/>. 
	  /// For other sensitivity objects, see <seealso cref="RatesProvider#parameterSensitivity(PointSensitivities)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivity </param>
	  /// <returns> the sensitivity to the curve parameters </returns>
	  CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets market data of a specific type.
	  /// <para>
	  /// This is a general purpose mechanism to obtain market data.
	  /// In general, it is desirable to pass the specific market data needed for pricing into
	  /// the pricing method. However, in some cases, notably swaps, this is not feasible.
	  /// It is strongly recommended to clearly state on pricing methods what data is required.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the data associated with the key </returns>
	  /// <exception cref="IllegalArgumentException"> if the data is not available </exception>
	  T data<T>(MarketDataId<T> id);

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
	  /// Converts this provider to an equivalent {@code ImmutableLegalEntityDiscountingProvider}.
	  /// </summary>
	  /// <returns> the equivalent immutable legal entity provider </returns>
	  ImmutableLegalEntityDiscountingProvider toImmutableLegalEntityDiscountingProvider();

	}

}
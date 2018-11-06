using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Triple = com.opengamma.strata.collect.tuple.Triple;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsIndex = com.opengamma.strata.product.credit.ResolvedCdsIndex;

	/// <summary>
	/// Pricer for CDS portfolio index based on ISDA standard model. 
	/// <para>
	/// The CDS index is priced as a single name CDS using a single credit curve rather than 
	/// credit curves of constituent single names. 
	/// </para>
	/// <para>
	/// {@code CreditRatesProvider} must contain the index credit curve as well as 
	/// the information on the relevant recovery rate and index factor. 
	/// </para>
	/// <para>
	/// This pricer invokes the implementation in <seealso cref="IsdaCdsProductPricer"/>. 
	/// </para>
	/// </summary>
	public class IsdaHomogenousCdsIndexProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IsdaHomogenousCdsIndexProductPricer DEFAULT = new IsdaHomogenousCdsIndexProductPricer(AccrualOnDefaultFormula.ORIGINAL_ISDA);

	  /// <summary>
	  /// The pricer for single name CDS.
	  /// </summary>
	  private readonly IsdaCdsProductPricer underlyingPricer;

	  /// <summary>
	  /// Constructor specifying the formula to use for the accrued on default calculation.  
	  /// </summary>
	  /// <param name="formula">  the formula </param>
	  public IsdaHomogenousCdsIndexProductPricer(AccrualOnDefaultFormula formula)
	  {
		this.underlyingPricer = new IsdaCdsProductPricer(formula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual-on-default formula used in this pricer. 
	  /// </summary>
	  /// <returns> the formula </returns>
	  public virtual AccrualOnDefaultFormula AccrualOnDefaultFormula
	  {
		  get
		  {
			return underlyingPricer.AccrualOnDefaultFormula;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the CDS index product, which is the minus of the present value per unit notional. 
	  /// <para>
	  /// This method can calculate the clean or dirty price, see <seealso cref="PriceType"/>. 
	  /// If calculating the clean price, the accrued interest is calculated based on the step-in date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the price </returns>
	  public virtual double price(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, PriceType priceType, ReferenceData refData)
	  {

		ResolvedCds cds = cdsIndex.toSingleNameCds();
		return underlyingPricer.price(cds, ratesProvider, referenceDate, priceType, refData);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the product. 
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder priceSensitivity(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, ReferenceData refData)
	  {

		ResolvedCds cds = cdsIndex.toSingleNameCds();
		return underlyingPricer.priceSensitivity(cds, ratesProvider, referenceDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the CDS index product.
	  /// <para>
	  /// The present value of the product is based on {@code referenceDate}.
	  /// This is typically the valuation date, or cash settlement date if the product is associated with a {@code Trade}. 
	  /// </para>
	  /// <para>
	  /// This method can calculate the clean or dirty present value, see <seealso cref="PriceType"/>. 
	  /// If calculating the clean value, the accrued interest is calculated based on the step-in date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value </returns>
	  public virtual CurrencyAmount presentValue(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, PriceType priceType, ReferenceData refData)
	  {

		if (isExpired(cdsIndex, ratesProvider))
		{
		  return CurrencyAmount.of(cdsIndex.Currency, 0d);
		}
		ResolvedCds cds = cdsIndex.toSingleNameCds();
		LocalDate stepinDate = cds.StepinDateOffset.adjust(ratesProvider.ValuationDate, refData);
		LocalDate effectiveStartDate = cds.calculateEffectiveStartDate(stepinDate);
		double recoveryRate = underlyingPricer.recoveryRate(cds, ratesProvider);
		Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> rates = reduceDiscountFactors(cds, ratesProvider);
		double protectionLeg = (1d - recoveryRate) * underlyingPricer.protectionFull(cds, rates.First, rates.Second, referenceDate, effectiveStartDate);
		double rpv01 = underlyingPricer.riskyAnnuity(cds, rates.First, rates.Second, referenceDate, stepinDate, effectiveStartDate, priceType);
		double amount = cds.BuySell.normalize(cds.Notional) * rates.Third * (protectionLeg - rpv01 * cds.FixedRate);
		return CurrencyAmount.of(cds.Currency, amount);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the product. 
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of present value to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivity(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, ReferenceData refData)
	  {

		if (isExpired(cdsIndex, ratesProvider))
		{
		  return PointSensitivityBuilder.none();
		}
		ResolvedCds cds = cdsIndex.toSingleNameCds();
		LocalDate stepinDate = cds.StepinDateOffset.adjust(ratesProvider.ValuationDate, refData);
		LocalDate effectiveStartDate = cds.calculateEffectiveStartDate(stepinDate);
		double recoveryRate = underlyingPricer.recoveryRate(cds, ratesProvider);
		Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> rates = reduceDiscountFactors(cds, ratesProvider);

		double signedNotional = cds.BuySell.normalize(cds.Notional);
		PointSensitivityBuilder protectionLegSensi = underlyingPricer.protectionLegSensitivity(cds, rates.First, rates.Second, referenceDate, effectiveStartDate, recoveryRate);
		protectionLegSensi = protectionLegSensi.multipliedBy(signedNotional * rates.Third);
		PointSensitivityBuilder riskyAnnuitySensi = underlyingPricer.riskyAnnuitySensitivity(cds, rates.First, rates.Second, referenceDate, stepinDate, effectiveStartDate);
		riskyAnnuitySensi = riskyAnnuitySensi.multipliedBy(-cds.FixedRate * signedNotional * rates.Third);

		return protectionLegSensi.combinedWith(riskyAnnuitySensi);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par spread of the CDS index product.
	  /// <para>
	  /// The par spread is a coupon rate such that the clean PV is 0. 
	  /// The result is represented in decimal form. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, ReferenceData refData)
	  {

		ResolvedCds cds = cdsIndex.toSingleNameCds();
		return underlyingPricer.parSpread(cds, ratesProvider, referenceDate, refData);
	  }

	  /// <summary>
	  /// Calculates the par spread sensitivity of the product.
	  /// <para>
	  /// The par spread sensitivity of the product is the sensitivity of par spread to the underlying curves.
	  /// The resulting sensitivity is based on the currency of the CDS index product.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the par spread </returns>
	  public virtual PointSensitivityBuilder parSpreadSensitivity(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, ReferenceData refData)
	  {

		ResolvedCds cds = cdsIndex.toSingleNameCds();
		return underlyingPricer.parSpreadSensitivity(cds, ratesProvider, referenceDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the risky PV01 of the CDS index product. 
	  /// <para>
	  /// RPV01 is defined as minus of the present value sensitivity to coupon rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="priceType">  the price type </param>
	  /// <param name="refData">  the reference date </param>
	  /// <returns> the RPV01 </returns>
	  public virtual CurrencyAmount rpv01(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, PriceType priceType, ReferenceData refData)
	  {

		if (isExpired(cdsIndex, ratesProvider))
		{
		  return CurrencyAmount.of(cdsIndex.Currency, 0d);
		}
		ResolvedCds cds = cdsIndex.toSingleNameCds();
		LocalDate stepinDate = cds.StepinDateOffset.adjust(ratesProvider.ValuationDate, refData);
		LocalDate effectiveStartDate = cds.calculateEffectiveStartDate(stepinDate);
		Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> rates = reduceDiscountFactors(cds, ratesProvider);
		double riskyAnnuity = underlyingPricer.riskyAnnuity(cds, rates.First, rates.Second, referenceDate, stepinDate, effectiveStartDate, priceType);
		double amount = cds.BuySell.normalize(cds.Notional) * riskyAnnuity * rates.Third;
		return CurrencyAmount.of(cds.Currency, amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the recovery01 of the CDS index product.
	  /// <para>
	  /// The recovery01 is defined as the present value sensitivity to the recovery rate.
	  /// Since the ISDA standard model requires the recovery rate to be constant throughout the lifetime of the CDS index,  
	  /// one currency amount is returned by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the recovery01 </returns>
	  public virtual CurrencyAmount recovery01(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, ReferenceData refData)
	  {

		if (isExpired(cdsIndex, ratesProvider))
		{
		  return CurrencyAmount.of(cdsIndex.Currency, 0d);
		}
		ResolvedCds cds = cdsIndex.toSingleNameCds();
		LocalDate stepinDate = cds.StepinDateOffset.adjust(ratesProvider.ValuationDate, refData);
		LocalDate effectiveStartDate = cds.calculateEffectiveStartDate(stepinDate);
		underlyingPricer.validateRecoveryRates(cds, ratesProvider);
		Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> rates = reduceDiscountFactors(cds, ratesProvider);
		double protectionFull = underlyingPricer.protectionFull(cds, rates.First, rates.Second, referenceDate, effectiveStartDate);
		double amount = -cds.BuySell.normalize(cds.Notional) * protectionFull * rates.Third;
		return CurrencyAmount.of(cds.Currency, amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the jump-to-default of the CDS index product.
	  /// <para>
	  /// The jump-to-default is the value of the product in case of immediate default of a constituent single name.
	  /// </para>
	  /// <para>
	  /// Under the homogeneous pool assumption, the jump-to-default values are the same for all of the undefaulted names, 
	  /// and zero for defaulted names. Thus the resulting object contains a single number.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the recovery01 </returns>
	  public virtual JumpToDefault jumpToDefault(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider, LocalDate referenceDate, ReferenceData refData)
	  {

		StandardId indexId = cdsIndex.CdsIndexId;
		Currency currency = cdsIndex.Currency;
		if (isExpired(cdsIndex, ratesProvider))
		{
		  return JumpToDefault.of(currency, ImmutableMap.of(indexId, 0d));
		}
		ResolvedCds cds = cdsIndex.toSingleNameCds();
		LocalDate stepinDate = cds.StepinDateOffset.adjust(ratesProvider.ValuationDate, refData);
		LocalDate effectiveStartDate = cds.calculateEffectiveStartDate(stepinDate);
		double recoveryRate = underlyingPricer.recoveryRate(cds, ratesProvider);
		Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> rates = reduceDiscountFactors(cds, ratesProvider);
		double protectionFull = underlyingPricer.protectionFull(cds, rates.First, rates.Second, referenceDate, effectiveStartDate);
		double rpv01 = underlyingPricer.riskyAnnuity(cds, rates.First, rates.Second, referenceDate, stepinDate, effectiveStartDate, PriceType.CLEAN);
		double lgd = 1d - recoveryRate;
		double numTotal = cdsIndex.LegalEntityIds.size();
		double jtd = (lgd - (lgd * protectionFull - cds.FixedRate * rpv01)) / numTotal;
		return JumpToDefault.of(currency, ImmutableMap.of(indexId, cds.BuySell.normalize(cds.Notional) * jtd));
	  }

	  /// <summary>
	  /// Calculates the expected loss of the CDS index product.
	  /// <para>
	  /// The expected loss is the (undiscounted) expected default settlement value paid by the protection seller. 
	  /// The resulting value is always positive.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cdsIndex">  the product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the expected loss </returns>
	  public virtual CurrencyAmount expectedLoss(ResolvedCdsIndex cdsIndex, CreditRatesProvider ratesProvider)
	  {

		if (isExpired(cdsIndex, ratesProvider))
		{
		  return CurrencyAmount.of(cdsIndex.Currency, 0d);
		}
		ResolvedCds cds = cdsIndex.toSingleNameCds();
		double recoveryRate = underlyingPricer.recoveryRate(cds, ratesProvider);
		Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> rates = reduceDiscountFactors(cds, ratesProvider);
		double survivalProbability = rates.Second.survivalProbability(cds.ProtectionEndDate);
		double el = (1d - recoveryRate) * (1d - survivalProbability) * rates.Third;
		return CurrencyAmount.of(cds.Currency, Math.Abs(cds.Notional) * el);
	  }

	  //-------------------------------------------------------------------------
	  internal virtual bool isExpired(ResolvedCdsIndex index, CreditRatesProvider ratesProvider)
	  {
		return !index.ProtectionEndDate.isAfter(ratesProvider.ValuationDate);
	  }

	  internal virtual Triple<CreditDiscountFactors, LegalEntitySurvivalProbabilities, double> reduceDiscountFactors(ResolvedCds cds, CreditRatesProvider ratesProvider)
	  {

		Currency currency = cds.Currency;
		CreditDiscountFactors discountFactors = ratesProvider.discountFactors(currency);
		ArgChecker.isTrue(discountFactors.IsdaCompliant, "discount factors must be IsdaCompliantZeroRateDiscountFactors");
		LegalEntitySurvivalProbabilities survivalProbabilities = ratesProvider.survivalProbabilities(cds.LegalEntityId, currency);
		ArgChecker.isTrue(survivalProbabilities.SurvivalProbabilities.IsdaCompliant, "survival probabilities must be IsdaCompliantZeroRateDiscountFactors");
		ArgChecker.isTrue(discountFactors.DayCount.Equals(survivalProbabilities.SurvivalProbabilities.DayCount), "day count conventions of discounting curve and credit curve must be the same");
		double indexFactor = ((IsdaCreditDiscountFactors) survivalProbabilities.SurvivalProbabilities).Curve.Metadata.getInfo(CurveInfoType.CDS_INDEX_FACTOR);
		return Triple.of(discountFactors, survivalProbabilities, indexFactor);
	  }

	}

}
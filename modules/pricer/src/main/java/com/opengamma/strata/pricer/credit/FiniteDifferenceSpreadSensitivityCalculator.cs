using System.Collections.Generic;

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
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;

	/// <summary>
	/// Finite difference spread sensitivity calculator. 
	/// <para>
	/// This computes the present value sensitivity to par spreads of bucketed CDSs by bump-and-reprice, i.e., 
	/// finite difference method. 
	/// </para>
	/// </summary>
	public class FiniteDifferenceSpreadSensitivityCalculator : SpreadSensitivityCalculator
	{

	  /// <summary>
	  /// Default implementation.
	  /// <para>
	  /// The bump amount is one basis point.
	  /// </para>
	  /// </summary>
	  public static readonly FiniteDifferenceSpreadSensitivityCalculator DEFAULT = new FiniteDifferenceSpreadSensitivityCalculator(AccrualOnDefaultFormula.ORIGINAL_ISDA, 1.0e-4);
	  /// <summary>
	  /// The bump amount for the finite difference method.
	  /// <para>
	  /// The magnitude of the bump amount must be greater than 1e-10. 
	  /// However, this bound does not guarantee that the finite difference calculation produces reliable numbers.
	  /// </para>
	  /// </summary>
	  private readonly double bumpAmount;

	  /// <summary>
	  /// Constructor with accrual-on-default formula and bump amount specified.
	  /// </summary>
	  /// <param name="formula">  the formula </param>
	  /// <param name="bumpAmount">  the bump amount </param>
	  public FiniteDifferenceSpreadSensitivityCalculator(AccrualOnDefaultFormula formula, double bumpAmount) : base(formula)
	  {
		this.bumpAmount = ArgChecker.notZero(bumpAmount, 1.0e-10, "bumpAmount");
	  }

	  //-------------------------------------------------------------------------
	  public override CurrencyAmount parallelCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		checkCdsBucket(trade, bucketCds);
		ResolvedCds product = trade.Product;
		Currency currency = product.Currency;
		StandardId legalEntityId = product.LegalEntityId;
		LocalDate valuationDate = ratesProvider.ValuationDate;
		ImmutableCreditRatesProvider immutableRatesProvider = ratesProvider.toImmutableCreditRatesProvider();

		int nBucket = bucketCds.Count;
		DoubleArray impSp = impliedSpread(bucketCds, ratesProvider, refData);
		NodalCurve creditCurveBase = Calibrator.calibrate(bucketCds, impSp, DoubleArray.filled(nBucket), CurveName.of("baseImpliedCreditCurve"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		Pair<StandardId, Currency> lePair = Pair.of(legalEntityId, currency);

		IsdaCreditDiscountFactors df = IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurveBase);
		CreditRatesProvider ratesProviderBase = immutableRatesProvider.toBuilder().creditCurves(ImmutableMap.of(lePair, LegalEntitySurvivalProbabilities.of(legalEntityId, df))).build();
		CurrencyAmount pvBase = Pricer.presentValueOnSettle(trade, ratesProviderBase, PriceType.DIRTY, refData);

		DoubleArray bumpedSp = DoubleArray.of(nBucket, i => impSp.get(i) + bumpAmount);
		NodalCurve creditCurveBump = Calibrator.calibrate(bucketCds, bumpedSp, DoubleArray.filled(nBucket), CurveName.of("bumpedImpliedCreditCurve"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		IsdaCreditDiscountFactors dfBump = IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurveBump);
		CreditRatesProvider ratesProviderBump = immutableRatesProvider.toBuilder().creditCurves(ImmutableMap.of(lePair, LegalEntitySurvivalProbabilities.of(legalEntityId, dfBump))).build();
		CurrencyAmount pvBumped = Pricer.presentValueOnSettle(trade, ratesProviderBump, PriceType.DIRTY, refData);

		return CurrencyAmount.of(currency, (pvBumped.Amount - pvBase.Amount) / bumpAmount);
	  }

	  internal override DoubleArray computedBucketedCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		checkCdsBucket(trade, bucketCds);
		ResolvedCds product = trade.Product;
		Currency currency = product.Currency;
		StandardId legalEntityId = product.LegalEntityId;
		LocalDate valuationDate = ratesProvider.ValuationDate;
		ImmutableCreditRatesProvider immutableRatesProvider = ratesProvider.toImmutableCreditRatesProvider();

		int nBucket = bucketCds.Count;
		double[] res = new double[nBucket];
		DoubleArray impSp = impliedSpread(bucketCds, ratesProvider, refData);
		NodalCurve creditCurveBase = Calibrator.calibrate(bucketCds, impSp, DoubleArray.filled(nBucket), CurveName.of("baseImpliedCreditCurve"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		Pair<StandardId, Currency> lePair = Pair.of(legalEntityId, currency);

		IsdaCreditDiscountFactors df = IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurveBase);
		CreditRatesProvider ratesProviderBase = immutableRatesProvider.toBuilder().creditCurves(ImmutableMap.of(lePair, LegalEntitySurvivalProbabilities.of(legalEntityId, df))).build();
		double pvBase = Pricer.presentValueOnSettle(trade, ratesProviderBase, PriceType.DIRTY, refData).Amount;
		for (int i = 0; i < nBucket; ++i)
		{
		  double[] bumpedSp = impSp.toArray();
		  bumpedSp[i] += bumpAmount;
		  NodalCurve creditCurveBump = Calibrator.calibrate(bucketCds, DoubleArray.ofUnsafe(bumpedSp), DoubleArray.filled(nBucket), CurveName.of("bumpedImpliedCreditCurve"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		  IsdaCreditDiscountFactors dfBump = IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurveBump);
		  CreditRatesProvider ratesProviderBump = immutableRatesProvider.toBuilder().creditCurves(ImmutableMap.of(lePair, LegalEntitySurvivalProbabilities.of(legalEntityId, dfBump))).build();
		  double pvBumped = Pricer.presentValueOnSettle(trade, ratesProviderBump, PriceType.DIRTY, refData).Amount;
		  res[i] = (pvBumped - pvBase) / bumpAmount;
		}
		return DoubleArray.ofUnsafe(res);
	  }

	}

}
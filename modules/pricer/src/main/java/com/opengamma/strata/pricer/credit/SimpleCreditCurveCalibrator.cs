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
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using BrentSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.BrentSingleRootFinder;
	using RealSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.RealSingleRootFinder;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;

	/// <summary>
	/// Simple credit curve calibrator.
	/// <para>
	/// This is a bootstrapper for the credit curve that is consistent with ISDA 
	/// in that it will produce the same curve from the same inputs (up to numerical round-off). 
	/// </para>
	/// <para>
	/// The external pricer, <seealso cref="IsdaCdsTradePricer"/>, is used in the calibration.
	/// </para>
	/// </summary>
	public sealed class SimpleCreditCurveCalibrator : IsdaCompliantCreditCurveCalibrator
	{

	  /// <summary>
	  /// The standard implementation.
	  /// </summary>
	  private static readonly SimpleCreditCurveCalibrator STANDARD = new SimpleCreditCurveCalibrator();

	  /// <summary>
	  /// The root bracket finder.
	  /// </summary>
	  private static readonly BracketRoot BRACKER = new BracketRoot();
	  /// <summary>
	  /// The root finder.
	  /// </summary>
	  private static readonly RealSingleRootFinder ROOTFINDER = new BrentSingleRootFinder();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the standard calibrator.
	  /// <para>
	  /// The original ISDA accrual-on-default formula (version 1.8.2 and lower) is used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard calibrator </returns>
	  public static SimpleCreditCurveCalibrator standard()
	  {
		return SimpleCreditCurveCalibrator.STANDARD;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Constructs a default credit curve builder. 
	  /// <para>
	  /// The original ISDA accrual-on-default formula (version 1.8.2 and lower) is used.
	  /// </para>
	  /// </summary>
	  private SimpleCreditCurveCalibrator() : base()
	  {
	  }

	  /// <summary>
	  /// Constructors a credit curve calibrator with the accrual-on-default formula specified.
	  /// </summary>
	  /// <param name="formula">  the accrual-on-default formula </param>
	  public SimpleCreditCurveCalibrator(AccrualOnDefaultFormula formula) : base(formula)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override NodalCurve calibrate(IList<ResolvedCdsTrade> calibrationCDSs, DoubleArray premiums, DoubleArray pointsUpfront, CurveName name, LocalDate valuationDate, CreditDiscountFactors discountFactors, RecoveryRates recoveryRates, ReferenceData refData)
	  {

		int n = calibrationCDSs.Count;
		double[] guess = new double[n];
		double[] t = new double[n];
		double[] lgd = new double[n];
		for (int i = 0; i < n; i++)
		{
		  LocalDate endDate = calibrationCDSs[i].Product.ProtectionEndDate;
		  t[i] = discountFactors.relativeYearFraction(endDate);
		  lgd[i] = 1d - recoveryRates.recoveryRate(endDate);
		  guess[i] = (premiums.get(i) + pointsUpfront.get(i) / t[i]) / lgd[i];
		}
		DoubleArray times = DoubleArray.ofUnsafe(t);
		CurveMetadata baseMetadata = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName(name).dayCount(discountFactors.DayCount).build();
		NodalCurve creditCurve = n == 1 ? ConstantNodalCurve.of(baseMetadata, t[0], guess[0]) : InterpolatedNodalCurve.of(baseMetadata, times, DoubleArray.ofUnsafe(guess), CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);

		for (int i = 0; i < n; i++)
		{
		  System.Func<double, double> func = getPriceFunction(i, calibrationCDSs[i], premiums.get(i), pointsUpfront.get(i), valuationDate, creditCurve, discountFactors, recoveryRates, refData);
		  double[] bracket = BRACKER.getBracketedPoints(func, 0.8 * guess[i], 1.25 * guess[i], 0.0, double.PositiveInfinity);
		  double zeroRate = bracket[0] > bracket[1] ? ROOTFINDER.getRoot(func, bracket[1], bracket[0]) : ROOTFINDER.getRoot(func, bracket[0], bracket[1]); //Negative guess handled
		  creditCurve = creditCurve.withParameter(i, zeroRate);
		}

		return creditCurve;
	  }

	  private System.Func<double, double> getPriceFunction(int index, ResolvedCdsTrade cds, double flactionalSpread, double pointsUpfront, LocalDate valuationDate, NodalCurve creditCurve, CreditDiscountFactors discountFactors, RecoveryRates recoveryRates, ReferenceData refData)
	  {

		ResolvedCds cdsProduct = cds.Product;
		Currency currency = cdsProduct.Currency;
		StandardId legalEntityId = cdsProduct.LegalEntityId;
		Pair<StandardId, Currency> pair = Pair.of(legalEntityId, currency);
		ImmutableCreditRatesProvider ratesbase = ImmutableCreditRatesProvider.builder().valuationDate(valuationDate).discountCurves(ImmutableMap.of(currency, discountFactors)).recoveryRateCurves(ImmutableMap.of(legalEntityId, recoveryRates)).build();
		System.Func<double, double> func = (double? x) =>
		{
	NodalCurve tempCreditCurve = creditCurve.withParameter(index, x.Value);
	ImmutableCreditRatesProvider rates = ratesbase.toBuilder().creditCurves(ImmutableMap.of(pair, LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, tempCreditCurve)))).build();
	double price = TradePricer.price(cds, rates, flactionalSpread, PriceType.CLEAN, refData);
	return price - pointsUpfront;
		};
		return func;
	  }

	}

}
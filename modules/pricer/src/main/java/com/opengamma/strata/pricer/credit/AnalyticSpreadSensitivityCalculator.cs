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
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using LUDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons;
	using LUDecompositionResult = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionResult;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;

	/// <summary>
	/// Analytic spread sensitivity calculator.
	/// <para>
	/// This analytically computes the present value sensitivity to par spreads of bucketed CDSs. 
	/// </para>
	/// </summary>
	public class AnalyticSpreadSensitivityCalculator : SpreadSensitivityCalculator
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly AnalyticSpreadSensitivityCalculator DEFAULT = new AnalyticSpreadSensitivityCalculator(AccrualOnDefaultFormula.ORIGINAL_ISDA);

	  /// <summary>
	  /// The matrix algebra used for matrix inversion.
	  /// </summary>
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new CommonsMatrixAlgebra();
	  /// <summary>
	  /// LU decomposition.
	  /// </summary>
	  private static readonly LUDecompositionCommons DECOMPOSITION = new LUDecompositionCommons();

	  /// <summary>
	  /// Constructor with the accrual-on-default formula specified.
	  /// </summary>
	  /// <param name="formula">  the formula </param>
	  public AnalyticSpreadSensitivityCalculator(AccrualOnDefaultFormula formula) : base(formula)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override CurrencyAmount parallelCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		DoubleArray temp = computedBucketedCs01(trade, bucketCds, ratesProvider, refData);
		return CurrencyAmount.of(trade.Product.Currency, temp.sum());
	  }

	  internal override DoubleArray computedBucketedCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		checkCdsBucket(trade, bucketCds);
		ResolvedCds product = trade.Product;
		Currency currency = product.Currency;
		StandardId legalEntityId = product.LegalEntityId;
		LocalDate valuationDate = ratesProvider.ValuationDate;

		int nBucket = bucketCds.Count;
		DoubleArray impSp = impliedSpread(bucketCds, ratesProvider, refData);
		NodalCurve creditCurveBase = Calibrator.calibrate(bucketCds, impSp, DoubleArray.filled(nBucket), CurveName.of("baseImpliedCreditCurve"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		IsdaCreditDiscountFactors df = IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurveBase);
		CreditRatesProvider ratesProviderBase = ratesProvider.toImmutableCreditRatesProvider().toBuilder().creditCurves(ImmutableMap.of(Pair.of(legalEntityId, currency), LegalEntitySurvivalProbabilities.of(legalEntityId, df))).build();

		double[][] res = new double[nBucket][];
		PointSensitivities pointPv = Pricer.presentValueOnSettleSensitivity(trade, ratesProviderBase, refData);
		DoubleArray vLambda = ratesProviderBase.singleCreditCurveParameterSensitivity(pointPv, legalEntityId, currency).Sensitivity;
		for (int i = 0; i < nBucket; i++)
		{
		  PointSensitivities pointSp = Pricer.parSpreadSensitivity(bucketCds[i], ratesProviderBase, refData);
		  res[i] = ratesProviderBase.singleCreditCurveParameterSensitivity(pointSp, legalEntityId, currency).Sensitivity.toArray();
		}
		DoubleMatrix jacT = MATRIX_ALGEBRA.getTranspose(DoubleMatrix.ofUnsafe(res));
		LUDecompositionResult luRes = DECOMPOSITION.apply(jacT);
		DoubleArray vS = luRes.solve(vLambda);
		return vS;
	  }

	}

}
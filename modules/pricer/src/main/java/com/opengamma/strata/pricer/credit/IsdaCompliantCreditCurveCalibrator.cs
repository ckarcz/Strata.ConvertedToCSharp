using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using IsdaCreditCurveDefinition = com.opengamma.strata.market.curve.IsdaCreditCurveDefinition;
	using IsdaCreditCurveNode = com.opengamma.strata.market.curve.IsdaCreditCurveNode;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CdsIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIsdaCreditCurveNode;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ResolvedTradeParameterMetadata = com.opengamma.strata.market.param.ResolvedTradeParameterMetadata;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using CdsCalibrationTrade = com.opengamma.strata.product.credit.CdsCalibrationTrade;
	using CdsQuote = com.opengamma.strata.product.credit.CdsQuote;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;

	/// <summary>
	/// ISDA compliant credit curve calibrator.
	/// <para>
	/// A single credit curve is calibrated for credit default swaps on a legal entity.
	/// </para>
	/// <para>
	/// The curve is defined using one or more <seealso cref="IsdaCreditCurveNode nodes"/>.
	/// Each node primarily defines enough information to produce a reference CDS trade.
	/// All of the curve nodes must be based on a common legal entity and currency.
	/// </para>
	/// <para>
	/// Calibration involves pricing, and re-pricing, these trades to find the best fit using a root finder.
	/// Relevant discount curve and recovery rate curve are required to complete the calibration.
	/// </para>
	/// </summary>
	public abstract class IsdaCompliantCreditCurveCalibrator
	{

	  /// <summary>
	  /// Default arbitrage handling.
	  /// </summary>
	  private const ArbitrageHandling DEFAULT_ARBITRAGE_HANDLING = ArbitrageHandling.IGNORE;
	  /// <summary>
	  /// Default pricing formula.
	  /// </summary>
	  private const AccrualOnDefaultFormula DEFAULT_FORMULA = AccrualOnDefaultFormula.ORIGINAL_ISDA;
	  /// <summary>
	  /// The matrix algebra used for matrix inversion.
	  /// </summary>
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new CommonsMatrixAlgebra();

	  /// <summary>
	  /// The arbitrage handling.
	  /// </summary>
	  private readonly ArbitrageHandling arbHandling;
	  /// <summary>
	  /// The accrual-on-default formula.
	  /// </summary>
	  private readonly AccrualOnDefaultFormula formula;
	  /// <summary>
	  /// The trade pricer.
	  /// </summary>
	  private readonly IsdaCdsTradePricer tradePricer;

	  //-------------------------------------------------------------------------
	  protected internal IsdaCompliantCreditCurveCalibrator() : this(DEFAULT_FORMULA, DEFAULT_ARBITRAGE_HANDLING)
	  {
	  }

	  protected internal IsdaCompliantCreditCurveCalibrator(AccrualOnDefaultFormula formula) : this(formula, DEFAULT_ARBITRAGE_HANDLING)
	  {
	  }

	  protected internal IsdaCompliantCreditCurveCalibrator(AccrualOnDefaultFormula formula, ArbitrageHandling arbHandling)
	  {
		this.arbHandling = ArgChecker.notNull(arbHandling, "arbHandling");
		this.formula = ArgChecker.notNull(formula, "formula");
		this.tradePricer = new IsdaCdsTradePricer(formula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the arbitrage handling.
	  /// <para>
	  /// See <seealso cref="ArbitrageHandling"/> for detail.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the arbitrage handling </returns>
	  protected internal virtual ArbitrageHandling ArbitrageHandling
	  {
		  get
		  {
			return arbHandling;
		  }
	  }

	  /// <summary>
	  /// Obtains the accrual-on-default formula.
	  /// <para>
	  /// See <seealso cref="AccrualOnDefaultFormula"/> for detail.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the formula </returns>
	  protected internal virtual AccrualOnDefaultFormula AccrualOnDefaultFormula
	  {
		  get
		  {
			return formula;
		  }
	  }

	  /// <summary>
	  /// Obtains the trade pricer used in this calibration.
	  /// </summary>
	  /// <returns> the trade pricer </returns>
	  protected internal virtual IsdaCdsTradePricer TradePricer
	  {
		  get
		  {
			return tradePricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrates the ISDA compliant credit curve to the market data.
	  /// <para>
	  /// This creates the single credit curve for a legal entity.
	  /// The curve nodes in {@code IsdaCreditCurveDefinition} should be single-name credit default swaps on this legal entity.
	  /// </para>
	  /// <para>
	  /// The relevant discount curve and recovery rate curve must be stored in {@code ratesProvider}. 
	  /// The day count convention for the resulting credit curve is the same as that of the discount curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveDefinition">  the curve definition </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the ISDA compliant credit curve </returns>
	  public virtual LegalEntitySurvivalProbabilities calibrate(IsdaCreditCurveDefinition curveDefinition, MarketData marketData, ImmutableCreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		ArgChecker.isTrue(curveDefinition.CurveValuationDate.Equals(ratesProvider.ValuationDate), "ratesProvider and curveDefinition must be based on the same valuation date");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<CdsIsdaCreditCurveNode> curveNodes = curveDefinition.CurveNodes.Where(n => n is CdsIsdaCreditCurveNode).Select(n => (CdsIsdaCreditCurveNode) n).collect(Guavate.toImmutableList());
		return calibrate(curveNodes, curveDefinition.Name, marketData, ratesProvider, curveDefinition.DayCount, curveDefinition.Currency, curveDefinition.ComputeJacobian, curveDefinition.StoreNodeTrade, refData);
	  }

	  internal virtual LegalEntitySurvivalProbabilities calibrate(IList<CdsIsdaCreditCurveNode> curveNodes, CurveName name, MarketData marketData, ImmutableCreditRatesProvider ratesProvider, DayCount definitionDayCount, Currency definitionCurrency, bool computeJacobian, bool storeTrade, ReferenceData refData)
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<StandardId> legalEntities = curveNodes.Select(CdsIsdaCreditCurveNode::getLegalEntityId).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		StandardId legalEntityId = legalEntities.next();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(legalEntities.hasNext(), "legal entity must be common to curve nodes");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<Currency> currencies = curveNodes.Select(n => n.Template.Convention.Currency).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		Currency currency = currencies.next();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(currencies.hasNext(), "currency must be common to curve nodes");
		ArgChecker.isTrue(definitionCurrency.Equals(currency), "curve definition currency must be the same as the currency of CDS");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<CdsQuoteConvention> quoteConventions = curveNodes.Select(n => n.QuoteConvention).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		CdsQuoteConvention quoteConvention = quoteConventions.next();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(quoteConventions.hasNext(), "quote convention must be common to curve nodes");
		LocalDate valuationDate = marketData.ValuationDate;
		ArgChecker.isTrue(valuationDate.Equals(marketData.ValuationDate), "ratesProvider and marketDate must be based on the same valuation date");
		CreditDiscountFactors discountFactors = ratesProvider.discountFactors(currency);
		ArgChecker.isTrue(definitionDayCount.Equals(discountFactors.DayCount), "credit curve and discount curve must be based on the same day count convention");
		RecoveryRates recoveryRates = ratesProvider.recoveryRates(legalEntityId);

		int nNodes = curveNodes.Count;
		double[] coupons = new double[nNodes];
		double[] pufs = new double[nNodes];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] diag = new double[nNodes][nNodes];
		double[][] diag = RectangularArrays.ReturnRectangularDoubleArray(nNodes, nNodes);
		ImmutableList.Builder<ResolvedCdsTrade> tradesBuilder = ImmutableList.builder();
		for (int i = 0; i < nNodes; i++)
		{
		  CdsCalibrationTrade tradeCalibration = curveNodes[i].trade(1d, marketData, refData);
		  ResolvedCdsTrade trade = tradeCalibration.UnderlyingTrade.resolve(refData);
		  tradesBuilder.add(trade);
		  double[] temp = getStandardQuoteForm(trade, tradeCalibration.Quote, valuationDate, discountFactors, recoveryRates, computeJacobian, refData);
		  coupons[i] = temp[0];
		  pufs[i] = temp[1];
		  diag[i][i] = temp[2];
		}
		ImmutableList<ResolvedCdsTrade> trades = tradesBuilder.build();
		NodalCurve nodalCurve = calibrate(trades, DoubleArray.ofUnsafe(coupons), DoubleArray.ofUnsafe(pufs), name, valuationDate, discountFactors, recoveryRates, refData);

		if (computeJacobian)
		{
		  LegalEntitySurvivalProbabilities creditCurve = LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, nodalCurve));
		  ImmutableCreditRatesProvider ratesProviderNew = ratesProvider.toBuilder().creditCurves(ImmutableMap.of(Pair.of(legalEntityId, currency), creditCurve)).build();
		  System.Func<ResolvedCdsTrade, DoubleArray> sensiFunc = quoteConvention.Equals(CdsQuoteConvention.PAR_SPREAD) ? getParSpreadSensitivityFunction(ratesProviderNew, name, currency, refData) : getPointsUpfrontSensitivityFunction(ratesProviderNew, name, currency, refData);
		  DoubleMatrix sensi = DoubleMatrix.ofArrayObjects(nNodes, nNodes, i => sensiFunc(trades.get(i)));
		  sensi = (DoubleMatrix) MATRIX_ALGEBRA.multiply(DoubleMatrix.ofUnsafe(diag), sensi);
		  JacobianCalibrationMatrix jacobian = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(name, nNodes)), MATRIX_ALGEBRA.getInverse(sensi));
		  nodalCurve = nodalCurve.withMetadata(nodalCurve.Metadata.withInfo(CurveInfoType.JACOBIAN, jacobian));
		}

		ImmutableList<ParameterMetadata> parameterMetadata;
		if (storeTrade)
		{
		  parameterMetadata = IntStream.range(0, nNodes).mapToObj(n => ResolvedTradeParameterMetadata.of(trades.get(n), curveNodes[n].Label)).collect(Guavate.toImmutableList());
		}
		else
		{
		  parameterMetadata = IntStream.range(0, nNodes).mapToObj(n => curveNodes[n].metadata(trades.get(n).Product.ProtectionEndDate)).collect(Guavate.toImmutableList());
		}
		nodalCurve = nodalCurve.withMetadata(nodalCurve.Metadata.withParameterMetadata(parameterMetadata));

		return LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, nodalCurve));
	  }

	  private System.Func<ResolvedCdsTrade, DoubleArray> getPointsUpfrontSensitivityFunction(CreditRatesProvider ratesProvider, CurveName curveName, Currency currency, ReferenceData refData)
	  {

		System.Func<ResolvedCdsTrade, DoubleArray> func = (ResolvedCdsTrade trade) =>
		{
	PointSensitivities point = tradePricer.priceSensitivity(trade, ratesProvider, refData);
	return ratesProvider.parameterSensitivity(point).getSensitivity(curveName, currency).Sensitivity;
		};
		return func;
	  }

	  private System.Func<ResolvedCdsTrade, DoubleArray> getParSpreadSensitivityFunction(CreditRatesProvider ratesProvider, CurveName curveName, Currency currency, ReferenceData refData)
	  {

		System.Func<ResolvedCdsTrade, DoubleArray> func = (ResolvedCdsTrade trade) =>
		{
	PointSensitivities point = tradePricer.parSpreadSensitivity(trade, ratesProvider, refData);
	return ratesProvider.parameterSensitivity(point).getSensitivity(curveName, currency).Sensitivity;
		};
		return func;
	  }

	  /// <summary>
	  /// Calibrate the ISDA compliant credit curve to points upfront and fractional spread.
	  /// </summary>
	  /// <param name="calibrationCDSs">  the calibration CDS </param>
	  /// <param name="flactionalSpreads">  the fractional spreads </param>
	  /// <param name="pointsUpfront">  the points upfront values </param>
	  /// <param name="name">  the curve name </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="discountFactors">  the discount factors </param>
	  /// <param name="recoveryRates">  the recovery rates </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the ISDA compliant credit curve </returns>
	  public abstract NodalCurve calibrate(IList<ResolvedCdsTrade> calibrationCDSs, DoubleArray flactionalSpreads, DoubleArray pointsUpfront, CurveName name, LocalDate valuationDate, CreditDiscountFactors discountFactors, RecoveryRates recoveryRates, ReferenceData refData);

	  private double[] getStandardQuoteForm(ResolvedCdsTrade calibrationCds, CdsQuote marketQuote, LocalDate valuationDate, CreditDiscountFactors discountFactors, RecoveryRates recoveryRates, bool computeJacobian, ReferenceData refData)
	  {

		double[] res = new double[3];
		res[2] = 1d;
		if (marketQuote.QuoteConvention.Equals(CdsQuoteConvention.PAR_SPREAD))
		{
		  res[0] = marketQuote.QuotedValue;
		}
		else if (marketQuote.QuoteConvention.Equals(CdsQuoteConvention.QUOTED_SPREAD))
		{
		  double qSpread = marketQuote.QuotedValue;
		  CurveName curveName = CurveName.of("quoteConvertCurve");
		  NodalCurve tempCreditCurve = calibrate(ImmutableList.of(calibrationCds), DoubleArray.of(qSpread), DoubleArray.of(0d), curveName, valuationDate, discountFactors, recoveryRates, refData);
		  Currency currency = calibrationCds.Product.Currency;
		  StandardId legalEntityId = calibrationCds.Product.LegalEntityId;
		  ImmutableCreditRatesProvider rates = ImmutableCreditRatesProvider.builder().valuationDate(valuationDate).discountCurves(ImmutableMap.of(currency, discountFactors)).recoveryRateCurves(ImmutableMap.of(legalEntityId, recoveryRates)).creditCurves(ImmutableMap.of(Pair.of(legalEntityId, currency), LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, tempCreditCurve)))).build();
		  res[0] = calibrationCds.Product.FixedRate;
		  res[1] = tradePricer.price(calibrationCds, rates, PriceType.CLEAN, refData);
		  if (computeJacobian)
		  {
			CurrencyParameterSensitivities pufSensi = rates.parameterSensitivity(tradePricer.priceSensitivity(calibrationCds, rates, refData));
			CurrencyParameterSensitivities spSensi = rates.parameterSensitivity(tradePricer.parSpreadSensitivity(calibrationCds, rates, refData));
			res[2] = spSensi.getSensitivity(curveName, currency).Sensitivity.get(0) / pufSensi.getSensitivity(curveName, currency).Sensitivity.get(0);
		  }
		}
		else if (marketQuote.QuoteConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT))
		{
		  res[0] = calibrationCds.Product.FixedRate;
		  res[1] = marketQuote.QuotedValue;
		}
		else
		{
		  throw new System.ArgumentException("Unknown CDSQuoteConvention type " + marketQuote.GetType());
		}
		return res;
	  }

	}

}
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
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ResolvedTradeParameterMetadata = com.opengamma.strata.market.param.ResolvedTradeParameterMetadata;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsIndex = com.opengamma.strata.product.credit.ResolvedCdsIndex;
	using ResolvedCdsIndexTrade = com.opengamma.strata.product.credit.ResolvedCdsIndexTrade;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;

	/// <summary>
	/// The spread sensitivity calculator. 
	/// <para>
	/// The spread sensitivity, also called CS01, is the sensitivity of the CDS product present value to par spreads of the bucket CDSs. 
	/// The bucket CDSs do not necessarily correspond to the node point of the input credit curve.
	/// </para>
	/// </summary>
	public abstract class SpreadSensitivityCalculator
	{

	  /// <summary>
	  /// The trade pricer.
	  /// </summary>
	  private readonly IsdaCdsTradePricer pricer;
	  /// <summary>
	  /// The credit curve calibrator.
	  /// </summary>
	  private readonly IsdaCompliantCreditCurveCalibrator calibrator;

	  /// <summary>
	  /// Constructor with accrual-on-default formula.
	  /// </summary>
	  /// <param name="formula">  the accrual-on-default formula </param>
	  public SpreadSensitivityCalculator(AccrualOnDefaultFormula formula)
	  {
		this.pricer = new IsdaCdsTradePricer(formula);
		this.calibrator = new FastCreditCurveCalibrator(formula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the pricer.
	  /// </summary>
	  /// <returns> the pricer </returns>
	  protected internal virtual IsdaCdsTradePricer Pricer
	  {
		  get
		  {
			return pricer;
		  }
	  }

	  /// <summary>
	  /// Gets the calibrator.
	  /// </summary>
	  /// <returns> the calibrator </returns>
	  protected internal virtual IsdaCompliantCreditCurveCalibrator Calibrator
	  {
		  get
		  {
			return calibrator;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes parallel CS01 for CDS. 
	  /// <para>
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// </para>
	  /// <para>
	  /// The CDS trades used in the curve calibration are reused as bucket CDS by this method.
	  /// Thus the credit curve must store <seealso cref="ResolvedTradeParameterMetadata"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the parallel CS01 </returns>
	  public virtual CurrencyAmount parallelCs01(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		IList<ResolvedCdsTrade> bucketCds = getBucketCds(trade.Product, ratesProvider);
		return parallelCs01(trade, bucketCds, ratesProvider, refData);
	  }

	  /// <summary>
	  /// Computes parallel CS01 for CDS. 
	  /// <para>
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="bucketCds">  the CDS bucket </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the parallel CS01 </returns>
	  public abstract CurrencyAmount parallelCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes bucketed CS01 for CDS.
	  /// <para>
	  /// The relevant credit curve must be stored in {@code RatesProvider}. 
	  /// </para>
	  /// <para>
	  /// The CDS trades used in the curve calibration are reused as bucket CDS by this method.
	  /// Thus the credit curve must store <seealso cref="ResolvedTradeParameterMetadata"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the bucketed CS01 </returns>
	  public virtual CurrencyParameterSensitivity bucketedCs01(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		IList<ResolvedCdsTrade> bucketCds = getBucketCds(trade.Product, ratesProvider);
		return bucketedCs01(trade, bucketCds, ratesProvider, refData);
	  }

	  /// <summary>
	  /// Computes bucketed CS01 for CDS.
	  /// <para>
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="bucketCds">  the CDS bucket </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the bucketed CS01 </returns>
	  public virtual CurrencyParameterSensitivity bucketedCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<ResolvedTradeParameterMetadata> metadata = bucketCds.Select(t => ResolvedTradeParameterMetadata.of(t, t.Product.ProtectionEndDate.ToString())).collect(Guavate.toImmutableList());
		return bucketedCs01(trade, bucketCds, metadata, ratesProvider, refData);
	  }

	  private CurrencyParameterSensitivity bucketedCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, IList<ResolvedTradeParameterMetadata> metadata, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		DoubleArray sensiValue = computedBucketedCs01(trade, bucketCds, ratesProvider, refData);
		return CurrencyParameterSensitivity.of(CurveName.of("impliedSpreads"), metadata, trade.Product.Currency, sensiValue);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes parallel CS01 for CDS index using a single credit curve. 
	  /// <para>
	  /// This is coherent to the pricer <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>.
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// </para>
	  /// <para>
	  /// The CDS index trades used in the curve calibration are reused as bucket CDS index by this method.
	  /// Thus the credit curve must store <seealso cref="ResolvedTradeParameterMetadata"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the parallel CS01 </returns>
	  public virtual CurrencyAmount parallelCs01(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		IList<ResolvedCdsIndexTrade> bucketCdsIndex = getBucketCdsIndex(trade.Product, ratesProvider);
		return parallelCs01(trade, bucketCdsIndex, ratesProvider, refData);
	  }

	  /// <summary>
	  /// Computes parallel CS01 for CDS index using a single credit curve. 
	  /// <para>
	  /// This is coherent to the pricer <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>.
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="bucketCdsIndex">  the CDS index bucket </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the parallel CS01 </returns>
	  public virtual CurrencyAmount parallelCs01(ResolvedCdsIndexTrade trade, IList<ResolvedCdsIndexTrade> bucketCdsIndex, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		ResolvedCdsTrade cdsTrade = trade.toSingleNameCds();
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		IList<ResolvedCdsTrade> bucketCds = bucketCdsIndex.Select(ResolvedCdsIndexTrade::toSingleNameCds).ToList();
		CurrencyAmount cs01Cds = parallelCs01(cdsTrade, bucketCds, ratesProvider, refData);
		double indexFactor = getIndexFactor(cdsTrade.Product, ratesProvider);
		return cs01Cds.multipliedBy(indexFactor);
	  }

	  /// <summary>
	  /// Computes bucketed CS01 for CDS index using a single credit curve.
	  /// <para>
	  /// This is coherent to the pricer <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>.
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// </para>
	  /// <para>
	  /// The CDS index trades used in the curve calibration are reused as bucket CDS index by this method.
	  /// Thus the credit curve must store <seealso cref="ResolvedTradeParameterMetadata"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the bucketed CS01 </returns>
	  public virtual CurrencyParameterSensitivity bucketedCs01(ResolvedCdsIndexTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		IList<ResolvedCdsIndexTrade> bucketCdsIndex = getBucketCdsIndex(trade.Product, ratesProvider);
		return bucketedCs01(trade, bucketCdsIndex, ratesProvider, refData);
	  }

	  /// <summary>
	  /// Computes bucketed CS01 for CDS index using a single credit curve.
	  /// <para>
	  /// This is coherent to the pricer <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>.
	  /// The relevant credit curve must be stored in {@code RatesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="bucketCdsIndex">  the CDS index bucket </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the bucketed CS01 </returns>
	  public virtual CurrencyParameterSensitivity bucketedCs01(ResolvedCdsIndexTrade trade, IList<ResolvedCdsIndexTrade> bucketCdsIndex, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		ResolvedCdsTrade cdsTrade = trade.toSingleNameCds();
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		IList<ResolvedCdsTrade> bucketCds = bucketCdsIndex.Select(ResolvedCdsIndexTrade::toSingleNameCds).ToList();
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<ResolvedTradeParameterMetadata> metadata = bucketCdsIndex.Select(t => ResolvedTradeParameterMetadata.of(t, t.Product.ProtectionEndDate.ToString())).collect(Guavate.toImmutableList());
		CurrencyParameterSensitivity bucketedCs01 = this.bucketedCs01(cdsTrade, bucketCds, metadata, ratesProvider, refData);
		double indexFactor = getIndexFactor(cdsTrade.Product, ratesProvider);
		return bucketedCs01.multipliedBy(indexFactor);
	  }

	  //-------------------------------------------------------------------------
	  // extract CDS trades from credit curve
	  private ImmutableList<ResolvedCdsTrade> getBucketCds(ResolvedCds product, CreditRatesProvider ratesProvider)
	  {
		CreditDiscountFactors creditCurve = ratesProvider.survivalProbabilities(product.LegalEntityId, product.Currency).SurvivalProbabilities;
		int nNodes = creditCurve.ParameterCount;
		ImmutableList.Builder<ResolvedCdsTrade> builder = ImmutableList.builder();
		for (int i = 0; i < nNodes; ++i)
		{
		  ParameterMetadata metadata = creditCurve.getParameterMetadata(i);
		  ArgChecker.isTrue(metadata is ResolvedTradeParameterMetadata, "ParameterMetadata of credit curve must be ResolvedTradeParameterMetadata");
		  ResolvedTradeParameterMetadata tradeMetadata = (ResolvedTradeParameterMetadata) metadata;
		  ResolvedTrade trade = tradeMetadata.Trade;
		  ArgChecker.isTrue(trade is ResolvedCdsTrade, "ResolvedTrade must be ResolvedCdsTrade");
		  builder.add((ResolvedCdsTrade) trade);
		}
		return builder.build();
	  }

	  // extract CDS index trades from credit curve
	  private ImmutableList<ResolvedCdsIndexTrade> getBucketCdsIndex(ResolvedCdsIndex product, CreditRatesProvider ratesProvider)
	  {
		CreditDiscountFactors creditCurve = ratesProvider.survivalProbabilities(product.CdsIndexId, product.Currency).SurvivalProbabilities;
		int nNodes = creditCurve.ParameterCount;
		ImmutableList.Builder<ResolvedCdsIndexTrade> builder = ImmutableList.builder();
		for (int i = 0; i < nNodes; ++i)
		{
		  ParameterMetadata metadata = creditCurve.getParameterMetadata(i);
		  ArgChecker.isTrue(metadata is ResolvedTradeParameterMetadata, "ParameterMetadata of credit curve must be ResolvedTradeParameterMetadata");
		  ResolvedTradeParameterMetadata tradeMetadata = (ResolvedTradeParameterMetadata) metadata;
		  ResolvedTrade trade = tradeMetadata.Trade;
		  ArgChecker.isTrue(trade is ResolvedCdsIndexTrade, "ResolvedTrade must be ResolvedCdsIndexTrade");
		  builder.add((ResolvedCdsIndexTrade) trade);
		}
		return builder.build();
	  }

	  // internal bucketed CS01 computation
	  internal abstract DoubleArray computedBucketedCs01(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData);

	  // check legal entity and currency are common for all of the CDSs
	  protected internal virtual void checkCdsBucket(ResolvedCdsTrade trade, IList<ResolvedCdsTrade> bucketCds)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<StandardId> legalEntities = bucketCds.Select(t => t.Product.LegalEntityId).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isTrue(legalEntities.next().Equals(trade.Product.LegalEntityId), "legal entity must be common");
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(legalEntities.hasNext(), "legal entity must be common");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<Currency> currencies = bucketCds.Select(t => t.Product.Currency).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isTrue(currencies.next().Equals(trade.Product.Currency), "currency must be common");
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(currencies.hasNext(), "currency must be common");
	  }

	  protected internal virtual DoubleArray impliedSpread(IList<ResolvedCdsTrade> bucketCds, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		int size = bucketCds.Count;
		return DoubleArray.of(size, n => pricer.parSpread(bucketCds[n], ratesProvider, refData));
	  }

	  private double getIndexFactor(ResolvedCds cds, CreditRatesProvider ratesProvider)
	  {
		LegalEntitySurvivalProbabilities survivalProbabilities = ratesProvider.survivalProbabilities(cds.LegalEntityId, cds.Currency);
		// instance is checked in pricer
		double indexFactor = ((IsdaCreditDiscountFactors) survivalProbabilities.SurvivalProbabilities).Curve.Metadata.getInfo(CurveInfoType.CDS_INDEX_FACTOR);
		return indexFactor;
	  }

	}

}
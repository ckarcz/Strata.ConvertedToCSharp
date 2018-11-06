using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using CdsQuote = com.opengamma.strata.product.credit.CdsQuote;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;

	/// <summary>
	/// The market quote converter for credit default swaps.
	/// </summary>
	public class CdsMarketQuoteConverter
	{

	  /// <summary>
	  /// The default implementation.
	  /// </summary>
	  public static readonly CdsMarketQuoteConverter DEFAULT = new CdsMarketQuoteConverter();

	  /// <summary>
	  /// The credit curve calibrator.
	  /// </summary>
	  private readonly IsdaCompliantCreditCurveCalibrator calibrator;
	  /// <summary>
	  /// The trade pricer.
	  /// </summary>
	  private readonly IsdaCdsTradePricer pricer;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The default constructor.
	  /// <para>
	  /// The original ISDA accrual-on-default formula (version 1.8.2 and lower) is used.
	  /// </para>
	  /// </summary>
	  public CdsMarketQuoteConverter()
	  {
		this.calibrator = FastCreditCurveCalibrator.standard();
		this.pricer = IsdaCdsTradePricer.DEFAULT;
	  }

	  /// <summary>
	  /// The constructor with the accrual-on-default formula specified.
	  /// </summary>
	  /// <param name="formula">  the accrual-on-default formula </param>
	  public CdsMarketQuoteConverter(AccrualOnDefaultFormula formula)
	  {
		this.calibrator = new FastCreditCurveCalibrator(formula);
		this.pricer = new IsdaCdsTradePricer(formula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes market clean price from points upfront.
	  /// <para>
	  /// The points upfront and resultant price are represented as a fraction. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointsUpfront">  the points upfront </param>
	  /// <returns> the clean price </returns>
	  public virtual double cleanPriceFromPointsUpfront(double pointsUpfront)
	  {
		return 1d - pointsUpfront;
	  }

	  /// <summary>
	  /// Computes the market clean price. 
	  /// <para>
	  /// The market clean price is usually expressed in percentage. 
	  /// Here a fraction of notional is returned, e.g., 0.98 is 98(%) clean price.
	  /// </para>
	  /// <para>
	  /// A relevant credit curve must be pre-calibrated and stored in {@code ratesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the clean price </returns>
	  public virtual double cleanPrice(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {
		double puf = pointsUpfront(trade, ratesProvider, refData);
		return 1d - puf;
	  }

	  /// <summary>
	  /// Computes the points upfront. 
	  /// <para>
	  /// The points upfront quote is usually expressed in percentage. 
	  /// Here a fraction of notional is returned, e.g., 0.01 is 1(%) points up-front
	  /// </para>
	  /// <para>
	  /// The relevant credit curve must be pre-calibrated and stored in {@code ratesProvider}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the points upfront </returns>
	  public virtual double pointsUpfront(ResolvedCdsTrade trade, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {
		return pricer.price(trade, ratesProvider, PriceType.CLEAN, refData);
	  }

	  /// <summary>
	  /// Converts quoted spread to points upfront.
	  /// <para>
	  /// Thus {@code quote} must be {@code CdsQuoteConvention.QUOTED_SPREAD}.
	  /// </para>
	  /// <para>
	  /// The relevant discount curve and recovery rate curve must be stored in {@code ratesProvider}.
	  /// The credit curve is internally calibrated to convert one quote type to the other quote type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="quote">  the quote </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the quote </returns>
	  public virtual CdsQuote pointsUpFrontFromQuotedSpread(ResolvedCdsTrade trade, CdsQuote quote, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		ArgChecker.notNull(trade, "trade");
		ArgChecker.notNull(quote, "quote");
		ArgChecker.notNull(ratesProvider, "ratesProvider");
		ArgChecker.notNull(refData, "refData");
		ArgChecker.isTrue(quote.QuoteConvention.Equals(CdsQuoteConvention.QUOTED_SPREAD), "quote must be quoted spread");

		ResolvedCds product = trade.Product;
		Currency currency = product.Currency;
		StandardId legalEntityId = product.LegalEntityId;
		LocalDate valuationDate = ratesProvider.ValuationDate;
		NodalCurve creditCurve = calibrator.calibrate(ImmutableList.of(trade), DoubleArray.of(quote.QuotedValue), DoubleArray.of(0d), CurveName.of("temp"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		CreditRatesProvider ratesProviderNew = ratesProvider.toImmutableCreditRatesProvider().toBuilder().creditCurves(ImmutableMap.of(Pair.of(legalEntityId, currency), LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurve)))).build();
		double puf = pointsUpfront(trade, ratesProviderNew, refData);
		return CdsQuote.of(CdsQuoteConvention.POINTS_UPFRONT, puf);
	  }

	  /// <summary>
	  /// Converts points upfront to quoted spread.
	  /// <para>
	  /// Thus {@code quote} must be {@code CdsQuoteConvention.POINTS_UPFRONT}.
	  /// </para>
	  /// <para>
	  /// The relevant discount curve and recovery rate curve must be stored in {@code ratesProvider}.
	  /// The credit curve is internally calibrated to convert one quote type to the other quote type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="quote">  the quote </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the quote </returns>
	  public virtual CdsQuote quotedSpreadFromPointsUpfront(ResolvedCdsTrade trade, CdsQuote quote, CreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		ArgChecker.notNull(trade, "trade");
		ArgChecker.notNull(quote, "quote");
		ArgChecker.notNull(ratesProvider, "ratesProvider");
		ArgChecker.notNull(refData, "refData");
		ArgChecker.isTrue(quote.QuoteConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT), "quote must be points upfront");

		ResolvedCds product = trade.Product;
		Currency currency = product.Currency;
		StandardId legalEntityId = product.LegalEntityId;
		LocalDate valuationDate = ratesProvider.ValuationDate;
		NodalCurve creditCurve = calibrator.calibrate(ImmutableList.of(trade), DoubleArray.of(product.FixedRate), DoubleArray.of(quote.QuotedValue), CurveName.of("temp"), valuationDate, ratesProvider.discountFactors(currency), ratesProvider.recoveryRates(legalEntityId), refData);
		CreditRatesProvider ratesProviderNew = ratesProvider.toImmutableCreditRatesProvider().toBuilder().creditCurves(ImmutableMap.of(Pair.of(legalEntityId, currency), LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurve)))).build();
		double sp = pricer.parSpread(trade, ratesProviderNew, refData);
		return CdsQuote.of(CdsQuoteConvention.QUOTED_SPREAD, sp);
	  }

	  /// <summary>
	  /// The par spread quotes are converted to points upfronts or quoted spreads. 
	  /// <para>
	  /// The relevant discount curve and recovery rate curve must be stored in {@code ratesProvider}.
	  /// The credit curve is internally calibrated to par spread values.
	  /// </para>
	  /// <para>
	  /// {@code trades} must be sorted in ascending order in maturity and coherent to {@code quotes}. 
	  /// </para>
	  /// <para> 
	  /// The resultant quote is specified by {@code targetConvention}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trades">  the trades </param>
	  /// <param name="quotes">  the quotes </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="targetConvention">  the target convention </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the quotes </returns>
	  public virtual IList<CdsQuote> quotesFromParSpread(IList<ResolvedCdsTrade> trades, IList<CdsQuote> quotes, CreditRatesProvider ratesProvider, CdsQuoteConvention targetConvention, ReferenceData refData)
	  {

		ArgChecker.noNulls(trades, "trades");
		ArgChecker.noNulls(quotes, "quotes");
		ArgChecker.notNull(ratesProvider, "ratesProvider");
		ArgChecker.notNull(targetConvention, "targetConvention");
		ArgChecker.notNull(refData, "refData");

		int nNodes = trades.Count;
		ArgChecker.isTrue(quotes.Count == nNodes, "trades and quotes must be the same size");
		quotes.ForEach(q => ArgChecker.isTrue(q.QuoteConvention.Equals(CdsQuoteConvention.PAR_SPREAD), "quote must be par spread"));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<StandardId> legalEntities = trades.Select(t => t.Product.LegalEntityId).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		StandardId legalEntityId = legalEntities.next();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(legalEntities.hasNext(), "legal entity must be common to trades");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IEnumerator<Currency> currencies = trades.Select(t => t.Product.Currency).collect(Collectors.toSet()).GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		Currency currency = currencies.next();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		ArgChecker.isFalse(currencies.hasNext(), "currency must be common to trades");

		LocalDate valuationDate = ratesProvider.ValuationDate;
		CreditDiscountFactors discountFactors = ratesProvider.discountFactors(currency);
		RecoveryRates recoveryRates = ratesProvider.recoveryRates(legalEntityId);
		NodalCurve creditCurve = calibrator.calibrate(trades, DoubleArray.of(nNodes, q => quotes[q].QuotedValue), DoubleArray.filled(nNodes), CurveName.of("temp"), valuationDate, discountFactors, recoveryRates, refData);
		CreditRatesProvider ratesProviderNew = ratesProvider.toImmutableCreditRatesProvider().toBuilder().creditCurves(ImmutableMap.of(Pair.of(legalEntityId, currency), LegalEntitySurvivalProbabilities.of(legalEntityId, IsdaCreditDiscountFactors.of(currency, valuationDate, creditCurve)))).build();

		System.Func<ResolvedCdsTrade, CdsQuote> quoteValueFunction = createQuoteValueFunction(ratesProviderNew, targetConvention, refData);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<CdsQuote> result = trades.Select(c => quoteValueFunction(c)).collect(Collectors.collectingAndThen(Collectors.toList(), ImmutableList.copyOf));
		return result;
	  }

	  //-------------------------------------------------------------------------
	  private System.Func<ResolvedCdsTrade, CdsQuote> createQuoteValueFunction(CreditRatesProvider ratesProviderNew, CdsQuoteConvention targetConvention, ReferenceData refData)
	  {

		System.Func<ResolvedCdsTrade, CdsQuote> quoteValueFunction;
		if (targetConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT))
		{
		  quoteValueFunction = (ResolvedCdsTrade x) =>
		  {
	  double puf = pointsUpfront(x, ratesProviderNew, refData);
	  return CdsQuote.of(targetConvention, puf);
		  };
		}
		else if (targetConvention.Equals(CdsQuoteConvention.QUOTED_SPREAD))
		{
		  quoteValueFunction = (ResolvedCdsTrade x) =>
		  {
	  double puf = pointsUpfront(x, ratesProviderNew, refData);
	  return quotedSpreadFromPointsUpfront(x, CdsQuote.of(CdsQuoteConvention.POINTS_UPFRONT, puf), ratesProviderNew, refData);
		  };
		}
		else
		{
		  throw new System.ArgumentException("unsuported CDS quote convention");
		}
		return quoteValueFunction;
	  }

	}

}
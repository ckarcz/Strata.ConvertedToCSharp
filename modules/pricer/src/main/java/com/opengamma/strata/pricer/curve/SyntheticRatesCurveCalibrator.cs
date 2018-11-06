using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Sets = com.google.common.collect.Sets;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Index = com.opengamma.strata.basics.index.Index;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Synthetic curve calibrator.
	/// <para>
	/// A synthetic curve is a curve calibrated on synthetic instruments.
	/// A synthetic instrument is an instrument for which a theoretical or synthetic quote
	/// can be computed from a <seealso cref="RatesProvider"/>.
	/// </para>
	/// <para>
	/// This curve transformation is often used to have a different risk view or to standardize
	/// all risk to a common set of instruments, even if they are not the most liquid in a market.
	/// </para>
	/// </summary>
	public sealed class SyntheticRatesCurveCalibrator
	{

	  /// <summary>
	  /// The standard synthetic curve calibrator.
	  /// <para>
	  /// This uses the standard <seealso cref="RatesCurveCalibrator"/> and <seealso cref="CalibrationMeasures"/>.
	  /// </para>
	  /// </summary>
	  private static readonly SyntheticRatesCurveCalibrator STANDARD = SyntheticRatesCurveCalibrator.of(RatesCurveCalibrator.standard(), CalibrationMeasures.MARKET_QUOTE);

	  /// <summary>
	  /// The curve calibrator.
	  /// </summary>
	  private readonly RatesCurveCalibrator calibrator;
	  /// <summary>
	  /// The market-quotes measures used to produce the synthetic quotes.
	  /// </summary>
	  private readonly CalibrationMeasures measures;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The standard synthetic curve calibrator.
	  /// <para>
	  /// The <seealso cref="CalibrationMeasures#MARKET_QUOTE"/> measures are used for calibration.
	  /// The underlying calibrator is <seealso cref="RatesCurveCalibrator#standard()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard synthetic curve calibrator </returns>
	  public static SyntheticRatesCurveCalibrator standard()
	  {
		return STANDARD;
	  }

	  /// <summary>
	  /// Obtains an instance, specifying market quotes measures to use and calibrator.
	  /// </summary>
	  /// <param name="calibrator">  the mechanism used to calibrate curves once the synthetic market quotes are known </param>
	  /// <param name="marketQuotesMeasures">  the measures used to compute the market quotes </param>
	  /// <returns> the synthetic curve calibrator </returns>
	  public static SyntheticRatesCurveCalibrator of(RatesCurveCalibrator calibrator, CalibrationMeasures marketQuotesMeasures)
	  {
		return new SyntheticRatesCurveCalibrator(calibrator, marketQuotesMeasures);
	  }

	  // restricted constructor
	  private SyntheticRatesCurveCalibrator(RatesCurveCalibrator calibrator, CalibrationMeasures marketQuotesMeasures)
	  {
		this.measures = marketQuotesMeasures;
		this.calibrator = calibrator;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market quote measures.
	  /// </summary>
	  /// <returns> the measures </returns>
	  public CalibrationMeasures Measures
	  {
		  get
		  {
			return measures;
		  }
	  }

	  /// <summary>
	  /// Gets the curve calibrator.
	  /// </summary>
	  /// <returns> the calibrator </returns>
	  public RatesCurveCalibrator Calibrator
	  {
		  get
		  {
			return calibrator;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrates synthetic curves from the configuration of the new curves and an existing rates provider.
	  /// </summary>
	  /// <param name="group">  the curve group definition for the synthetic curves and instruments </param>
	  /// <param name="inputProvider">  the input rates provider </param>
	  /// <param name="refData">  the reference data, used to resolve the trades </param>
	  /// <returns> the rates provider </returns>
	  public ImmutableRatesProvider calibrate(RatesCurveGroupDefinition group, RatesProvider inputProvider, ReferenceData refData)
	  {

		// Computes the synthetic market quotes
		MarketData marketQuotesSy = marketData(group, inputProvider, refData);
		// Calibrate to the synthetic instrument with the synthetic quotes
		return calibrator.calibrate(group, marketQuotesSy, refData);
	  }

	  /// <summary>
	  /// Constructs the synthetic market data from an existing rates provider and the configuration of the new curves.
	  /// </summary>
	  /// <param name="group">  the curve group definition for the synthetic curves and instruments </param>
	  /// <param name="inputProvider">  the input rates provider </param>
	  /// <param name="refData">  the reference data, used to resolve the trades </param>
	  /// <returns> the market data </returns>
	  public ImmutableMarketData marketData(RatesCurveGroupDefinition group, RatesProvider inputProvider, ReferenceData refData)
	  {

		// Retrieve the set of required indices and the list of required currencies
		ISet<Index> indicesRequired = new HashSet<Index>();
		IList<Currency> ccyRequired = new List<Currency>();
		foreach (RatesCurveGroupEntry entry in group.Entries)
		{
		  indicesRequired.addAll(entry.Indices);
		  ((IList<Currency>)ccyRequired).AddRange(entry.DiscountCurrencies);
		}
		// Retrieve the required time series if present in the original provider
		IDictionary<IndexQuoteId, LocalDateDoubleTimeSeries> ts = new Dictionary<IndexQuoteId, LocalDateDoubleTimeSeries>();
		foreach (Index idx in Sets.intersection(inputProvider.TimeSeriesIndices, indicesRequired))
		{
		  ts[IndexQuoteId.of(idx)] = inputProvider.timeSeries(idx);
		}

		LocalDate valuationDate = inputProvider.ValuationDate;
		ImmutableList<CurveDefinition> curveGroups = group.CurveDefinitions;
		// Create fake market quotes of 0, only to be able to generate trades
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, double> mapId0 = new java.util.HashMap<>();
		IDictionary<MarketDataId<object>, double> mapId0 = new Dictionary<MarketDataId<object>, double>();
		foreach (CurveDefinition entry in curveGroups)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  for (int i = 0; i < nodes.size(); i++)
		  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (com.opengamma.strata.data.MarketDataId<?> key : nodes.get(i).requirements())
			foreach (MarketDataId<object> key in nodes.get(i).requirements())
			{
			  mapId0[key] = 0.0d;
			}
		  }
		}
		ImmutableMarketData marketQuotes0 = ImmutableMarketData.of(valuationDate, mapId0);
		// Generate market quotes from the trades
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> mapIdSy = new java.util.HashMap<>();
		IDictionary<MarketDataId<object>, object> mapIdSy = new Dictionary<MarketDataId<object>, object>();
		foreach (CurveDefinition entry in curveGroups)
		{
		  ImmutableList<CurveNode> nodes = entry.Nodes;
		  foreach (CurveNode node in nodes)
		  {
			ResolvedTrade trade = node.resolvedTrade(1d, marketQuotes0, refData);
			double mq = measures.value(trade, inputProvider);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> k = node.requirements().iterator().next();
			MarketDataId<object> k = node.requirements().GetEnumerator().next();
			mapIdSy[k] = mq;
		  }
		}
		// Generate quotes for FX pairs. The first currency is arbitrarily selected as starting point. 
		// The crosses are automatically generated by the MarketDataFxRateProvider used in calibration.
		for (int loopccy = 1; loopccy < ccyRequired.Count; loopccy++)
		{
		  CurrencyPair ccyPair = CurrencyPair.of(ccyRequired[0], ccyRequired[loopccy]);
		  FxRateId fxId = FxRateId.of(ccyPair);
		  mapIdSy[fxId] = FxRate.of(ccyPair, inputProvider.fxRate(ccyPair));
		}
		return ImmutableMarketData.builder(valuationDate).addValueMap(mapIdSy).addTimeSeriesMap(ts).build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Messages.format("SyntheticCurveCalibrator[{}, {}]", calibrator, measures);
	  }

	}

}
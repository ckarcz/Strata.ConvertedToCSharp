using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFunction = com.opengamma.strata.calc.marketdata.MarketDataFunction;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Function which builds <seealso cref="FxRate"/> instances from observable market data.
	/// <para>
	/// <seealso cref="FxRateConfig"/> defines which market data is used to build the FX rates.
	/// </para>
	/// </summary>
	public class FxRateMarketDataFunction : MarketDataFunction<FxRate, FxRateId>
	{

	  public virtual MarketDataRequirements requirements(FxRateId id, MarketDataConfig marketDataConfig)
	  {
		FxRateConfig fxRateConfig = marketDataConfig.get(typeof(FxRateConfig), id.ObservableSource);
		Optional<QuoteId> optional = fxRateConfig.getObservableRateKey(id.Pair);
		return optional.map(key => MarketDataRequirements.of(key)).orElse(MarketDataRequirements.empty());
	  }

	  public virtual MarketDataBox<FxRate> build(FxRateId id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
	  {

		FxRateConfig fxRateConfig = marketDataConfig.get(typeof(FxRateConfig), id.ObservableSource);
		Optional<QuoteId> optional = fxRateConfig.getObservableRateKey(id.Pair);
		return optional.map(key => buildFxRate(id, key, marketData)).orElseThrow(() => new System.ArgumentException("No FX rate configuration available for " + id.Pair));
	  }

	  private MarketDataBox<FxRate> buildFxRate(FxRateId id, QuoteId key, ScenarioMarketData marketData)
	  {

		MarketDataBox<double> quote = marketData.getValue(key);
		return quote.map(rate => FxRate.of(id.Pair, rate));
	  }

	  public virtual Type<FxRateId> MarketDataIdType
	  {
		  get
		  {
			return typeof(FxRateId);
		  }
	  }

	}

}
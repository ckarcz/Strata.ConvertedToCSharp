using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// A provider of FX rates which takes its data from one scenario in a set of data for multiple scenarios.
	/// </summary>
	[Serializable]
	internal class DefaultScenarioFxRateProvider : ScenarioFxRateProvider
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The market data for a set of scenarios.
	  /// </summary>
	  private readonly ScenarioMarketData marketData;

	  /// <summary>
	  /// The source of the FX rates.
	  /// </summary>
	  private readonly ObservableSource source;

	  // creates an instance
	  internal DefaultScenarioFxRateProvider(ScenarioMarketData marketData, ObservableSource source)
	  {
		this.marketData = marketData;
		this.source = source;
	  }

	  public virtual int ScenarioCount
	  {
		  get
		  {
			return marketData.ScenarioCount;
		  }
	  }

	  public virtual FxRateProvider fxRateProvider(int scenarioIndex)
	  {
		return MarketDataFxRateProvider.of(marketData.scenario(scenarioIndex), source);
	  }

	}

}
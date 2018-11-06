using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ScenarioFxRateProvider = com.opengamma.strata.data.scenario.ScenarioFxRateProvider;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// A provider of scenario FX rates that uses FX rate lookup.
	/// The use of <seealso cref="FxRateLookup"/> allows triangulation currency and observable source to be controlled.
	/// </summary>
	[Serializable]
	internal class LookupScenarioFxRateProvider : ScenarioFxRateProvider
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The market data for a set of scenarios.
	  /// </summary>
	  private readonly ScenarioMarketData marketData;
	  /// <summary>
	  /// The FX rate lookup.
	  /// </summary>
	  private readonly FxRateLookup lookup;

	  // obtains an instance, returning the interface type to make type system happy at call site
	  internal static ScenarioFxRateProvider of(ScenarioMarketData marketData, FxRateLookup lookup)
	  {
		return new LookupScenarioFxRateProvider(marketData, lookup);
	  }

	  private LookupScenarioFxRateProvider(ScenarioMarketData marketData, FxRateLookup lookup)
	  {
		this.marketData = ArgChecker.notNull(marketData, "marketData");
		this.lookup = ArgChecker.notNull(lookup, "lookup");
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
		return lookup.fxRateProvider(marketData.scenario(scenarioIndex));
	  }

	}

}
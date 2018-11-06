/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// A provider of FX rates for scenarios.
	/// <para>
	/// This provides the ability to obtain a set of FX rates, one for each scenario.
	/// The interface does not mandate when the rate applies, however it typically represents the current rate.
	/// </para>
	/// <para>
	/// This is the multi-scenario version of <seealso cref="FxRateProvider"/>.
	/// </para>
	/// <para>
	/// Implementations do not have to be immutable, but calls to the methods must be thread-safe.
	/// </para>
	/// </summary>
	public interface ScenarioFxRateProvider
	{

	  /// <summary>
	  /// Returns a scenario FX rate provider which takes its data from the provided market data.
	  /// </summary>
	  /// <param name="marketData">  market data containing FX rates </param>
	  /// <returns> a scenario FX rate provider which takes its data from the provided market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ScenarioFxRateProvider of(ScenarioMarketData marketData)
	//  {
	//	return new DefaultScenarioFxRateProvider(marketData, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Returns a scenario FX rate provider which takes its data from the provided market data.
	  /// </summary>
	  /// <param name="marketData">  market data containing FX rates </param>
	  /// <param name="source">  the source of the FX rates </param>
	  /// <returns> a scenario FX rate provider which takes its data from the provided market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ScenarioFxRateProvider of(ScenarioMarketData marketData, com.opengamma.strata.data.ObservableSource source)
	//  {
	//	return new DefaultScenarioFxRateProvider(marketData, source);
	//  }

	  /// <summary>
	  /// Gets the number of scenarios.
	  /// </summary>
	  /// <returns> the number of scenarios </returns>
	  int ScenarioCount {get;}

	  /// <summary>
	  /// Converts an amount in a currency to an amount in a different currency using a rate from this provider.
	  /// </summary>
	  /// <param name="amount">  an amount in {@code fromCurrency} </param>
	  /// <param name="fromCurrency">  the currency of the amount </param>
	  /// <param name="toCurrency">  the currency into which the amount should be converted </param>
	  /// <param name="scenarioIndex">  the scenario index </param>
	  /// <returns> the amount converted into {@code toCurrency} </returns>
	  /// <exception cref="IllegalArgumentException"> if either of the currencies aren't included in the currency pair of this rate </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double convert(double amount, com.opengamma.strata.basics.currency.Currency fromCurrency, com.opengamma.strata.basics.currency.Currency toCurrency, int scenarioIndex)
	//  {
	//	return amount * fxRate(fromCurrency, toCurrency, scenarioIndex);
	//  }

	  /// <summary>
	  /// Gets the FX rate for the specified currency pair and scenario index.
	  /// <para>
	  /// The rate returned is the rate from the base currency to the counter currency
	  /// as defined by this formula: {@code (1 * baseCurrency = fxRate * counterCurrency)}.
	  /// This will return 1 if the two input currencies are the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseCurrency">  the base currency, to convert from </param>
	  /// <param name="counterCurrency">  the counter currency, to convert to </param>
	  /// <param name="scenarioIndex">  the scenario index </param>
	  /// <returns> the FX rate for the currency pair </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double fxRate(com.opengamma.strata.basics.currency.Currency baseCurrency, com.opengamma.strata.basics.currency.Currency counterCurrency, int scenarioIndex)
	//  {
	//	return fxRateProvider(scenarioIndex).fxRate(baseCurrency, counterCurrency);
	//  }

	  /// <summary>
	  /// Gets the FX rate provider for the specified scenario index.
	  /// </summary>
	  /// <param name="scenarioIndex">  the scenario index </param>
	  /// <returns> the FX rate for the currency pair </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  FxRateProvider fxRateProvider(int scenarioIndex);

	}

}
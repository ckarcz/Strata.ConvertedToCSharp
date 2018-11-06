using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateMarketDataFunctionTest
	public class FxRateMarketDataFunctionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly QuoteId QUOTE_ID = QuoteId.of(StandardId.of("test", "EUR/USD"));
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(Currency.EUR, Currency.USD);
	  private static readonly FxRateId RATE_ID = FxRateId.of(CURRENCY_PAIR);

	  public virtual void requirements()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		MarketDataRequirements requirements = function.requirements(RATE_ID, config());
		assertThat(requirements).isEqualTo(MarketDataRequirements.of(QUOTE_ID));
	  }

	  public virtual void requirementsInverse()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		MarketDataRequirements requirements = function.requirements(FxRateId.of(CURRENCY_PAIR.inverse()), config());
		assertThat(requirements).isEqualTo(MarketDataRequirements.of(QUOTE_ID));
	  }

	  public virtual void requirementsMissingConfig()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		string regex = "No configuration found .*FxRateConfig";
		assertThrowsIllegalArg(() => function.requirements(RATE_ID, MarketDataConfig.empty()), regex);
	  }

	  public virtual void requirementsNoConfigForPair()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		CurrencyPair gbpUsd = CurrencyPair.of(Currency.GBP, Currency.USD);
		assertThat(function.requirements(FxRateId.of(gbpUsd), config())).isEqualTo(MarketDataRequirements.empty());
	  }

	  public virtual void build()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		MarketDataBox<double> quoteBox = MarketDataBox.ofSingleValue(1.1d);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(QUOTE_ID, quoteBox).build();
		MarketDataBox<FxRate> rateBox = function.build(RATE_ID, config(), marketData, REF_DATA);
		assertThat(rateBox.SingleValue).True;
		assertThat(rateBox.SingleValue).isEqualTo(FxRate.of(CURRENCY_PAIR, 1.1d));
	  }

	  public virtual void buildInverse()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		MarketDataBox<double> quoteBox = MarketDataBox.ofSingleValue(1.1d);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(QUOTE_ID, quoteBox).build();
		MarketDataBox<FxRate> rateBox = function.build(FxRateId.of(CURRENCY_PAIR.inverse()), config(), marketData, REF_DATA);
		assertThat(rateBox.SingleValue).True;
		assertThat(rateBox.SingleValue).isEqualTo(FxRate.of(CURRENCY_PAIR, 1.1d));
	  }

	  public virtual void buildScenario()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		MarketDataBox<double> quoteBox = MarketDataBox.ofScenarioValues(1.1d, 1.2d, 1.3d);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(QUOTE_ID, quoteBox).build();
		MarketDataBox<FxRate> rateBox = function.build(RATE_ID, config(), marketData, REF_DATA);
		assertThat(rateBox.SingleValue).False;
		assertThat(rateBox.ScenarioCount).isEqualTo(3);
		assertThat(rateBox.getValue(0)).isEqualTo(FxRate.of(CURRENCY_PAIR, 1.1d));
		assertThat(rateBox.getValue(1)).isEqualTo(FxRate.of(CURRENCY_PAIR, 1.2d));
		assertThat(rateBox.getValue(2)).isEqualTo(FxRate.of(CURRENCY_PAIR, 1.3d));
	  }

	  public virtual void buildMissingConfig()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		string regex = "No configuration found .*FxRateConfig";
		assertThrowsIllegalArg(() => function.build(RATE_ID, MarketDataConfig.empty(), ScenarioMarketData.empty(), REF_DATA), regex);
	  }

	  public virtual void buildNoConfigForPair()
	  {
		FxRateMarketDataFunction function = new FxRateMarketDataFunction();
		string regex = "No FX rate configuration available for GBP/USD";
		CurrencyPair gbpUsd = CurrencyPair.of(Currency.GBP, Currency.USD);
		assertThrowsIllegalArg(() => function.build(FxRateId.of(gbpUsd), config(), ScenarioMarketData.empty(), REF_DATA), regex);
	  }

	  private static MarketDataConfig config()
	  {
		IDictionary<CurrencyPair, QuoteId> ratesMap = ImmutableMap.of(CURRENCY_PAIR, QUOTE_ID);
		FxRateConfig fxRateConfig = FxRateConfig.builder().observableRates(ratesMap).build();
		return MarketDataConfig.builder().add(ObservableSource.NONE, fxRateConfig).build();
	  }

	}

}
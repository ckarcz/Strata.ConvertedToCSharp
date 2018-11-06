/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using BeforeClass = org.testng.annotations.BeforeClass;
	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScenarioFxRateProviderTest
	public class ScenarioFxRateProviderTest
	{

	  private ScenarioFxRateProvider fxRateProvider;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeforeClass public void setUp() throws Exception
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void setUp()
	  {
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addValue(FxRateId.of(Currency.GBP, Currency.USD), FxRate.of(Currency.GBP, Currency.USD, 1.4d)).build();

		fxRateProvider = ScenarioFxRateProvider.of(marketData);
	  }

	  public virtual void convert()
	  {
		assertThat(fxRateProvider.convert(10, Currency.GBP, Currency.USD, 0)).isEqualTo(14d);
	  }

	  public virtual void fxRate()
	  {
		assertThat(fxRateProvider.fxRate(Currency.GBP, Currency.USD, 0)).isEqualTo(1.4d);
	  }

	  public virtual void specifySource()
	  {
		ObservableSource testSource = ObservableSource.of("test");
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addValue(FxRateId.of(Currency.GBP, Currency.USD), FxRate.of(Currency.GBP, Currency.USD, 1.4d)).addValue(FxRateId.of(Currency.GBP, Currency.USD, testSource), FxRate.of(Currency.GBP, Currency.USD, 1.41d)).build();

		ScenarioFxRateProvider defaultRateProvider = ScenarioFxRateProvider.of(marketData);
		ScenarioFxRateProvider sourceRateProvider = ScenarioFxRateProvider.of(marketData, testSource);
		assertThat(defaultRateProvider.fxRate(Currency.GBP, Currency.USD, 0)).isEqualTo(1.4d);
		assertThat(sourceRateProvider.fxRate(Currency.GBP, Currency.USD, 0)).isEqualTo(1.41d);
	  }
	}

}
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// Test <seealso cref="MarketDataFxRateProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MarketDataFxRateProviderTest
	public class MarketDataFxRateProviderTest
	{

	  private static readonly LocalDate VAL_DATE = date(2015, 6, 30);
	  private const double EUR_USD = 1.10;
	  private const double GBP_USD = 1.50;
	  private const double EUR_CHF = 1.05;
	  private const double GBP_CHF = 1.41;
	  private static readonly Currency BEF = Currency.of("BEF");
	  private const double EUR_BEF = 40.3399;
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");

	  //-------------------------------------------------------------------------
	  public virtual void fxRate()
	  {
		double eurUsdRate = provider().fxRate(EUR, USD);
		assertThat(eurUsdRate).isEqualTo(1.1d);
	  }

	  public virtual void sameCurrencies()
	  {
		double eurRate = provider().fxRate(EUR, EUR);
		assertThat(eurRate).isEqualTo(1d);
	  }

	  public virtual void missingCurrencies()
	  {
		assertThrows(() => provider().fxRate(EUR, GBP), typeof(MarketDataNotFoundException), "No FX rate market data for EUR/GBP using source 'Vendor'");
		assertThrows(() => provider2().fxRate(JPY, GBP), typeof(MarketDataNotFoundException), "No FX rate market data for JPY/GBP");
	  }

	  public virtual void cross_specified()
	  {
		IDictionary<FxRateId, FxRate> marketDataMap = ImmutableMap.of(FxRateId.of(EUR, CHF), FxRate.of(EUR, CHF, EUR_CHF), FxRateId.of(GBP, CHF), FxRate.of(GBP, CHF, GBP_CHF));
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, marketDataMap);
		FxRateProvider fx = MarketDataFxRateProvider.of(marketData, ObservableSource.NONE, CHF);
		assertEquals(fx.fxRate(GBP, EUR), GBP_CHF / EUR_CHF, 1.0E-10);
		assertEquals(fx.fxRate(EUR, GBP), EUR_CHF / GBP_CHF, 1.0E-10);
		assertThrows(() => fx.fxRate(EUR, USD), typeof(MarketDataNotFoundException));
	  }

	  public virtual void cross_base()
	  {
		IDictionary<FxRateId, FxRate> marketDataMap = ImmutableMap.of(FxRateId.of(EUR, USD), FxRate.of(EUR, USD, EUR_USD), FxRateId.of(GBP, USD), FxRate.of(GBP, USD, GBP_USD));
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, marketDataMap);
		FxRateProvider fx = MarketDataFxRateProvider.of(marketData);
		assertEquals(fx.fxRate(GBP, EUR), GBP_USD / EUR_USD, 1.0E-10);
		assertEquals(fx.fxRate(EUR, GBP), EUR_USD / GBP_USD, 1.0E-10);
	  }

	  public virtual void cross_counter()
	  {
		IDictionary<FxRateId, FxRate> marketDataMap = ImmutableMap.of(FxRateId.of(EUR, USD), FxRate.of(EUR, USD, EUR_USD), FxRateId.of(EUR, BEF), FxRate.of(EUR, BEF, EUR_BEF));
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, marketDataMap);
		FxRateProvider fx = MarketDataFxRateProvider.of(marketData);
		assertEquals(fx.fxRate(USD, BEF), EUR_BEF / EUR_USD, 1.0E-10);
		assertEquals(fx.fxRate(BEF, USD), EUR_USD / EUR_BEF, 1.0E-10);
	  }

	  public virtual void cross_double_triangle()
	  {
		IDictionary<FxRateId, FxRate> marketDataMap = ImmutableMap.of(FxRateId.of(EUR, USD), FxRate.of(EUR, USD, EUR_USD), FxRateId.of(EUR, BEF), FxRate.of(EUR, BEF, EUR_BEF), FxRateId.of(GBP, USD), FxRate.of(GBP, USD, GBP_USD));
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, marketDataMap);
		FxRateProvider fx = MarketDataFxRateProvider.of(marketData);
		assertEquals(fx.fxRate(GBP, BEF), GBP_USD * EUR_BEF / EUR_USD, 1.0E-10);
		assertEquals(fx.fxRate(BEF, GBP), EUR_USD / EUR_BEF / GBP_USD, 1.0E-10);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		MarketDataFxRateProvider test = provider();
		coverImmutableBean(test);
		MarketDataFxRateProvider test2 = provider2();
		coverBeanEquals(test, test2);
	  }

	  //-------------------------------------------------------------------------
	  private static MarketDataFxRateProvider provider()
	  {
		IDictionary<FxRateId, FxRate> marketDataMap = ImmutableMap.of(FxRateId.of(EUR, USD, OBS_SOURCE), FxRate.of(EUR, USD, EUR_USD));
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, marketDataMap);
		return MarketDataFxRateProvider.of(marketData, OBS_SOURCE, GBP);
	  }

	  private static MarketDataFxRateProvider provider2()
	  {
		IDictionary<FxRateId, FxRate> marketDataMap = ImmutableMap.of(FxRateId.of(EUR, USD), FxRate.of(EUR, USD, EUR_USD), FxRateId.of(EUR, BEF), FxRate.of(EUR, BEF, EUR_BEF), FxRateId.of(GBP, USD), FxRate.of(GBP, USD, GBP_USD));
		MarketData marketData = ImmutableMarketData.of(VAL_DATE, marketDataMap);
		return MarketDataFxRateProvider.of(marketData, ObservableSource.NONE, GBP);
	  }

	}

}
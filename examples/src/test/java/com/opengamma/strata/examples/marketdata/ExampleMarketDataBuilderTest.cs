using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using Messages = com.opengamma.strata.collect.Messages;
	using FieldName = com.opengamma.strata.data.FieldName;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Test <seealso cref="ExampleMarketDataBuilder"/>, <seealso cref="DirectoryMarketDataBuilder"/> and <seealso cref="JarMarketDataBuilder"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExampleMarketDataBuilderTest
	public class ExampleMarketDataBuilderTest
	{

	  private const string EXAMPLE_MARKET_DATA_CLASSPATH_ROOT = "example-marketdata";
	  private const string EXAMPLE_MARKET_DATA_DIRECTORY_ROOT = "src/main/resources/example-marketdata";

	  private const string TEST_SPACES_DIRECTORY_ROOT = "src/test/resources/test-marketdata with spaces";
	  private const string TEST_SPACES_CLASSPATH_ROOT = "test-marketdata with spaces";

	  private static readonly CurveGroupName DEFAULT_CURVE_GROUP = CurveGroupName.of("Default");

	  private static readonly LocalDate MARKET_DATA_DATE = LocalDate.of(2014, 1, 22);

	  private static readonly ISet<ObservableId> TIME_SERIES = ImmutableSet.of(IndexQuoteId.of(IborIndices.USD_LIBOR_3M), IndexQuoteId.of(IborIndices.USD_LIBOR_6M), IndexQuoteId.of(OvernightIndices.USD_FED_FUND), IndexQuoteId.of(IborIndices.GBP_LIBOR_3M));

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Set<com.opengamma.strata.data.MarketDataId<?>> VALUES = com.google.common.collect.ImmutableSet.of(com.opengamma.strata.market.curve.CurveId.of(DEFAULT_CURVE_GROUP, com.opengamma.strata.market.curve.CurveName.of("USD-Disc")), com.opengamma.strata.market.curve.CurveId.of(DEFAULT_CURVE_GROUP, com.opengamma.strata.market.curve.CurveName.of("GBP-Disc")), com.opengamma.strata.market.curve.CurveId.of(DEFAULT_CURVE_GROUP, com.opengamma.strata.market.curve.CurveName.of("USD-3ML")), com.opengamma.strata.market.curve.CurveId.of(DEFAULT_CURVE_GROUP, com.opengamma.strata.market.curve.CurveName.of("USD-6ML")), com.opengamma.strata.market.curve.CurveId.of(DEFAULT_CURVE_GROUP, com.opengamma.strata.market.curve.CurveName.of("GBP-3ML")), com.opengamma.strata.data.FxRateId.of(com.opengamma.strata.basics.currency.Currency.USD, com.opengamma.strata.basics.currency.Currency.GBP), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "Eurex-FGBL-Mar14")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "Eurex-FGBL-Mar14"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-FutOpt", "Eurex-OGBL-Mar14-C150")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-FutOpt", "Eurex-OGBL-Mar14-C150"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "CME-ED-Mar14")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "CME-ED-Mar14"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Mar15")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Mar15"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Jun15")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Jun15"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "CME-F1U-Mar15")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "CME-F1U-Mar15"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "CME-F1U-Jun15")), com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG-Future", "CME-F1U-Jun15"), com.opengamma.strata.data.FieldName.SETTLEMENT_PRICE));
	  private static readonly ISet<MarketDataId<object>> VALUES = ImmutableSet.of(CurveId.of(DEFAULT_CURVE_GROUP, CurveName.of("USD-Disc")), CurveId.of(DEFAULT_CURVE_GROUP, CurveName.of("GBP-Disc")), CurveId.of(DEFAULT_CURVE_GROUP, CurveName.of("USD-3ML")), CurveId.of(DEFAULT_CURVE_GROUP, CurveName.of("USD-6ML")), CurveId.of(DEFAULT_CURVE_GROUP, CurveName.of("GBP-3ML")), FxRateId.of(Currency.USD, Currency.GBP), QuoteId.of(StandardId.of("OG-Future", "Eurex-FGBL-Mar14")), QuoteId.of(StandardId.of("OG-Future", "Eurex-FGBL-Mar14"), FieldName.SETTLEMENT_PRICE), QuoteId.of(StandardId.of("OG-FutOpt", "Eurex-OGBL-Mar14-C150")), QuoteId.of(StandardId.of("OG-FutOpt", "Eurex-OGBL-Mar14-C150"), FieldName.SETTLEMENT_PRICE), QuoteId.of(StandardId.of("OG-Future", "CME-ED-Mar14")), QuoteId.of(StandardId.of("OG-Future", "CME-ED-Mar14"), FieldName.SETTLEMENT_PRICE), QuoteId.of(StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Mar15")), QuoteId.of(StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Mar15"), FieldName.SETTLEMENT_PRICE), QuoteId.of(StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Jun15")), QuoteId.of(StandardId.of("OG-Future", "Ibor-USD-LIBOR-3M-Jun15"), FieldName.SETTLEMENT_PRICE), QuoteId.of(StandardId.of("OG-Future", "CME-F1U-Mar15")), QuoteId.of(StandardId.of("OG-Future", "CME-F1U-Mar15"), FieldName.SETTLEMENT_PRICE), QuoteId.of(StandardId.of("OG-Future", "CME-F1U-Jun15")), QuoteId.of(StandardId.of("OG-Future", "CME-F1U-Jun15"), FieldName.SETTLEMENT_PRICE));

	  public virtual void test_directory()
	  {
		Path rootPath = (new File(EXAMPLE_MARKET_DATA_DIRECTORY_ROOT)).toPath();
		DirectoryMarketDataBuilder builder = new DirectoryMarketDataBuilder(rootPath);
		assertBuilder(builder);
	  }

	  public virtual void test_ofPath()
	  {
		Path rootPath = (new File(EXAMPLE_MARKET_DATA_DIRECTORY_ROOT)).toPath();
		ExampleMarketDataBuilder builder = ExampleMarketDataBuilder.ofPath(rootPath);
		assertBuilder(builder);
	  }

	  public virtual void test_ofPath_with_spaces()
	  {
		Path rootPath = (new File(TEST_SPACES_DIRECTORY_ROOT)).toPath();
		ExampleMarketDataBuilder builder = ExampleMarketDataBuilder.ofPath(rootPath);

		ImmutableMarketData snapshot = builder.buildSnapshot(LocalDate.of(2015, 1, 1));
		assertEquals(snapshot.TimeSeries.size(), 1);
	  }

	  public virtual void test_ofResource_directory()
	  {
		ExampleMarketDataBuilder builder = ExampleMarketDataBuilder.ofResource(EXAMPLE_MARKET_DATA_CLASSPATH_ROOT);
		assertBuilder(builder);
	  }

	  public virtual void test_ofResource_directory_extraSlashes()
	  {
		ExampleMarketDataBuilder builder = ExampleMarketDataBuilder.ofResource("/" + EXAMPLE_MARKET_DATA_CLASSPATH_ROOT + "/");
		assertBuilder(builder);
	  }

	  public virtual void test_ofResource_directory_notFound()
	  {
		assertThrowsIllegalArg(() => ExampleMarketDataBuilder.ofResource("bad-dir"));
	  }

	  public virtual void test_ofResource_directory_with_spaces()
	  {
		ExampleMarketDataBuilder builder = ExampleMarketDataBuilder.ofResource(TEST_SPACES_CLASSPATH_ROOT);

		ImmutableMarketData snapshot = builder.buildSnapshot(MARKET_DATA_DATE);
		assertEquals(snapshot.TimeSeries.size(), 1);
	  }

	  //-------------------------------------------------------------------------
	  private void assertBuilder(ExampleMarketDataBuilder builder)
	  {
		ImmutableMarketData snapshot = builder.buildSnapshot(MARKET_DATA_DATE);

		assertEquals(MARKET_DATA_DATE, snapshot.ValuationDate);

		foreach (ObservableId id in TIME_SERIES)
		{
		  assertFalse(snapshot.getTimeSeries(id).Empty, "Time-series not found: " + id);
		}
		assertEquals(snapshot.TimeSeries.size(), TIME_SERIES.Count, Messages.format("Snapshot contained unexpected time-series: {}", Sets.difference(snapshot.TimeSeries.Keys, TIME_SERIES)));

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (com.opengamma.strata.data.MarketDataId<?> id : VALUES)
		foreach (MarketDataId<object> id in VALUES)
		{
		  assertTrue(snapshot.containsValue(id), "Id not found: " + id);
		}

		assertEquals(snapshot.Values.size(), VALUES.Count, Messages.format("Snapshot contained unexpected market data: {}", Sets.difference(snapshot.Values.Keys, VALUES)));
	  }

	}

}
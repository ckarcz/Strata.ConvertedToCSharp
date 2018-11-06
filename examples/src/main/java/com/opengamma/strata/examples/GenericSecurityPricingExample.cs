using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataId = com.opengamma.strata.basics.ReferenceDataId;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ExampleData = com.opengamma.strata.examples.marketdata.ExampleData;
	using ExampleMarketData = com.opengamma.strata.examples.marketdata.ExampleMarketData;
	using ExampleMarketDataBuilder = com.opengamma.strata.examples.marketdata.ExampleMarketDataBuilder;
	using Measures = com.opengamma.strata.measure.Measures;
	using StandardComponents = com.opengamma.strata.measure.StandardComponents;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using SecurityTrade = com.opengamma.strata.product.SecurityTrade;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using ReportCalculationResults = com.opengamma.strata.report.ReportCalculationResults;
	using TradeReport = com.opengamma.strata.report.trade.TradeReport;
	using TradeReportTemplate = com.opengamma.strata.report.trade.TradeReportTemplate;

	/// <summary>
	/// Example to illustrate using the engine to price generic securities.
	/// <para>
	/// This makes use of the example engine and the example market data environment.
	/// </para>
	/// </summary>
	public class GenericSecurityPricingExample
	{

	  private static readonly AttributeType<string> EXCHANGE_TYPE = AttributeType.of("exchange");
	  private static readonly AttributeType<string> PRODUCT_FAMILY_TYPE = AttributeType.of("productFamily");
	  private static readonly AttributeType<LocalDate> EXPIRY_TYPE = AttributeType.of("expiryDate");
	  private static readonly SecurityId FGBL_MAR14_ID = SecurityId.of("OG-Future", "Eurex-FGBL-Mar14");
	  private static readonly SecurityId OGBL_MAR14_C150_ID = SecurityId.of("OG-FutOpt", "Eurex-OGBL-Mar14-C150");
	  private static readonly SecurityId ED_MAR14_ID = SecurityId.of("OG-Future", "CME-ED-Mar14");
	  private static readonly GenericSecurity FGBL_MAR14 = GenericSecurity.of(SecurityInfo.of(FGBL_MAR14_ID, 0.01, CurrencyAmount.of(EUR, 10)).withAttribute(EXCHANGE_TYPE, "Eurex").withAttribute(PRODUCT_FAMILY_TYPE, "FGBL").withAttribute(EXPIRY_TYPE, LocalDate.of(2014, 3, 13)));
	  private static readonly GenericSecurity OGBL_MAR14_C150 = GenericSecurity.of(SecurityInfo.of(OGBL_MAR14_C150_ID, 0.01, CurrencyAmount.of(EUR, 10)).withAttribute(EXCHANGE_TYPE, "Eurex").withAttribute(PRODUCT_FAMILY_TYPE, "OGBL").withAttribute(EXPIRY_TYPE, LocalDate.of(2014, 3, 10)));
	  private static readonly GenericSecurity ED_MAR14 = GenericSecurity.of(SecurityInfo.of(ED_MAR14_ID, 0.005, CurrencyAmount.of(USD, 12.5)).withAttribute(EXCHANGE_TYPE, "CME").withAttribute(PRODUCT_FAMILY_TYPE, "ED").withAttribute(EXPIRY_TYPE, LocalDate.of(2014, 3, 10)));

	  /// <summary>
	  /// Runs the example, pricing the instruments, producing the output as an ASCII table.
	  /// </summary>
	  /// <param name="args">  ignored </param>
	  public static void Main(string[] args)
	  {
		// setup calculation runner component, which needs life-cycle management
		// a typical application might use dependency injection to obtain the instance
		using (CalculationRunner runner = CalculationRunner.ofMultiThreaded())
		{
		  calculate(runner);
		}
	  }

	  // obtains the data and calculates the grid of results
	  private static void calculate(CalculationRunner runner)
	  {
		// the trades that will have measures calculated
		IList<Trade> trades = ImmutableList.of(createFutureTrade1(), createFutureTrade2(), createOptionTrade1(), createOptionTrade2());

		// the columns, specifying the measures to be calculated
		IList<Column> columns = ImmutableList.of(Column.of(Measures.PRESENT_VALUE));

		// use the built-in example market data
		LocalDate valuationDate = LocalDate.of(2014, 1, 22);
		ExampleMarketDataBuilder marketDataBuilder = ExampleMarketData.builder();
		MarketData marketData = marketDataBuilder.buildSnapshot(valuationDate);

		// the complete set of rules for calculating measures
		CalculationFunctions functions = StandardComponents.calculationFunctions();
		CalculationRules rules = CalculationRules.of(functions);

		// the reference data, such as holidays and securities
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.basics.ReferenceData refData = com.opengamma.strata.basics.ImmutableReferenceData.of(com.google.common.collect.ImmutableMap.of<com.opengamma.strata.basics.ReferenceDataId<?>, Object>(FGBL_MAR14_ID, FGBL_MAR14, OGBL_MAR14_C150_ID, OGBL_MAR14_C150, ED_MAR14_ID, ED_MAR14));
		ReferenceData refData = ImmutableReferenceData.of(ImmutableMap.of<ReferenceDataId<object>, object>(FGBL_MAR14_ID, FGBL_MAR14, OGBL_MAR14_C150_ID, OGBL_MAR14_C150, ED_MAR14_ID, ED_MAR14));

		// calculate the results
		Results results = runner.calculate(rules, trades, columns, marketData, refData);

		// use the report runner to transform the engine results into a trade report
		ReportCalculationResults calculationResults = ReportCalculationResults.of(valuationDate, trades, columns, results, functions, refData);

		TradeReportTemplate reportTemplate = ExampleData.loadTradeReportTemplate("security-report-template");
		TradeReport tradeReport = TradeReport.of(calculationResults, reportTemplate);
		tradeReport.writeAsciiTable(System.out);
	  }

	  //-----------------------------------------------------------------------  
	  // create a futures trade where the security is looked up in reference data
	  private static Trade createFutureTrade1()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().addAttribute(AttributeType.DESCRIPTION, "20 x Euro-Bund Mar14").counterparty(StandardId.of("mn", "Dealer G")).settlementDate(LocalDate.of(2013, 12, 15)).build();
		return SecurityTrade.of(tradeInfo, FGBL_MAR14_ID, 20, 99.550);
	  }

	  // create a futures trade that embeds details of the security
	  private static Trade createFutureTrade2()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().addAttribute(AttributeType.DESCRIPTION, "8 x EuroDollar Mar14").counterparty(StandardId.of("mn", "Dealer G")).settlementDate(LocalDate.of(2013, 12, 18)).build();
		return GenericSecurityTrade.of(tradeInfo, ED_MAR14, 8, 99.550);
	  }

	  // create an options trade where the security is looked up in reference data
	  private static Trade createOptionTrade1()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().addAttribute(AttributeType.DESCRIPTION, "20 x Call on Euro-Bund Mar14").counterparty(StandardId.of("mn", "Dealer G")).settlementDate(LocalDate.of(2013, 1, 15)).build();
		return SecurityTrade.of(tradeInfo, OGBL_MAR14_C150_ID, 20, 1.6);
	  }

	  // create an options trade that embeds details of the security
	  private static Trade createOptionTrade2()
	  {
		TradeInfo tradeInfo = TradeInfo.builder().addAttribute(AttributeType.DESCRIPTION, "15 x Call on Euro-Bund Mar14").counterparty(StandardId.of("mn", "Dealer G")).settlementDate(LocalDate.of(2013, 1, 15)).build();
		return GenericSecurityTrade.of(tradeInfo, OGBL_MAR14_C150, 15, 1.62);
	  }

	}

}
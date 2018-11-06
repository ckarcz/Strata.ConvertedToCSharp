using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using TestId = com.opengamma.strata.calc.marketdata.TestId;
	using TestObservableId = com.opengamma.strata.calc.marketdata.TestObservableId;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using ImmutableScenarioMarketData = com.opengamma.strata.data.scenario.ImmutableScenarioMarketData;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Test <seealso cref="CalculationTask"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationTaskTest
	public class CalculationTaskTest
	{

	  internal static readonly ObservableSource OBS_SOURCE = ObservableSource.of("MarketDataVendor");

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ReportingCurrency NATURAL = ReportingCurrency.NATURAL;
	  private static readonly ReportingCurrency REPORTING_CURRENCY_USD = ReportingCurrency.of(Currency.USD);
	  private static readonly TestTarget TARGET = new TestTarget();
	  private static readonly ISet<Measure> MEASURES = ImmutableSet.of(TestingMeasures.PRESENT_VALUE, TestingMeasures.PRESENT_VALUE_MULTI_CCY);

	  public virtual void requirements()
	  {
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, new TestFunction(), cell);
		MarketDataRequirements requirements = task.requirements(REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> nonObservables = requirements.getNonObservables();
		ISet<MarketDataId<object>> nonObservables = requirements.NonObservables;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet<? extends com.opengamma.strata.data.ObservableId> observables = requirements.getObservables();
		ImmutableSet<ObservableId> observables = requirements.Observables;
		ImmutableSet<ObservableId> timeSeries = requirements.TimeSeries;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> timeSeriesId = com.opengamma.strata.calc.marketdata.TestObservableId.of("3", OBS_SOURCE);
		MarketDataId<object> timeSeriesId = TestObservableId.of("3", OBS_SOURCE);
		assertThat(timeSeries).hasSize(1);
		assertThat(timeSeries.GetEnumerator().next()).isEqualTo(timeSeriesId);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> nonObservableId = new com.opengamma.strata.calc.marketdata.TestId("1");
		MarketDataId<object> nonObservableId = new TestId("1");
		assertThat(nonObservables).hasSize(1);
		assertThat(nonObservables.GetEnumerator().next()).isEqualTo(nonObservableId);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> observableId = com.opengamma.strata.calc.marketdata.TestObservableId.of("2", OBS_SOURCE);
		MarketDataId<object> observableId = TestObservableId.of("2", OBS_SOURCE);
		assertThat(observables).hasSize(1);
		assertThat(observables.GetEnumerator().next()).isEqualTo(observableId);
	  }

	  /// <summary>
	  /// Test that the result is converted to the reporting currency if it implements ScenarioFxConvertible and
	  /// the FX rates are available in the market data.
	  /// </summary>
	  public virtual void convertResultCurrencyUsingReportingCurrency()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<FxRate> rates = ImmutableList.of(1.61, 1.62, 1.63).Select(rate => FxRate.of(GBP, USD, rate)).collect(toImmutableList());
		CurrencyScenarioArray list = CurrencyScenarioArray.of(GBP, values);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).addScenarioValue(FxRateId.of(GBP, USD), rates).build();
		ConvertibleFunction fn = ConvertibleFunction.of(() => list, GBP);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);

		DoubleArray expectedValues = DoubleArray.of(1 * 1.61, 2 * 1.62, 3 * 1.63);
		CurrencyScenarioArray expectedArray = CurrencyScenarioArray.of(USD, expectedValues);

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(expectedArray);
	  }

	  /// <summary>
	  /// Test that the result is not converted if the isCurrencyConvertible flag on the measure is false.
	  /// </summary>
	  public virtual void currencyConversionHonoursConvertibleFlagOnMeasure()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<FxRate> rates = ImmutableList.of(1.61, 1.62, 1.63).Select(rate => FxRate.of(GBP, USD, rate)).collect(toImmutableList());
		CurrencyScenarioArray list = CurrencyScenarioArray.of(GBP, values);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).addScenarioValue(FxRateId.of(GBP, USD), rates).build();
		ConvertibleFunction fn = ConvertibleFunction.of(() => list, GBP);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE_MULTI_CCY, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);

		CurrencyScenarioArray expectedArray = CurrencyScenarioArray.of(GBP, values);

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(expectedArray);
	  }

	  /// <summary>
	  /// Test that the result is converted to the reporting currency if it implements ScenarioFxConvertible and
	  /// the FX rates are available in the market data. The "natural" currency is taken from the function.
	  /// </summary>
	  public virtual void convertResultCurrencyUsingDefaultReportingCurrency()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<FxRate> rates = ImmutableList.of(1.61, 1.62, 1.63).Select(rate => FxRate.of(GBP, USD, rate)).collect(toImmutableList());
		CurrencyScenarioArray list = CurrencyScenarioArray.of(GBP, values);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).addScenarioValue(FxRateId.of(GBP, USD), rates).build();
		ConvertibleFunction fn = ConvertibleFunction.of(() => list, USD);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);

		DoubleArray expectedValues = DoubleArray.of(1 * 1.61, 2 * 1.62, 3 * 1.63);
		CurrencyScenarioArray expectedArray = CurrencyScenarioArray.of(USD, expectedValues);

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(expectedArray);
	  }

	  /// <summary>
	  /// Test that the result is returned unchanged if it is a failure.
	  /// </summary>
	  public virtual void convertResultCurrencyFailure()
	  {
		ConvertibleFunction fn = ConvertibleFunction.of(() =>
		{
		throw new Exception("This is a failure");
		}, GBP);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.CALCULATION_FAILED).hasFailureMessageMatching("Error when invoking function 'ConvertibleFunction' for ID '123': This is a failure");
	  }

	  /// <summary>
	  /// Test the result is returned unchanged if using ReportingCurrency.NONE.
	  /// </summary>
	  public virtual void convertResultCurrencyNoConversionRequested()
	  {
		SupplierFunction<CurrencyAmount> fn = SupplierFunction.of(() => CurrencyAmount.of(EUR, 1d));
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, ReportingCurrency.NONE);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).build();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(ScenarioArray.of(CurrencyAmount.of(EUR, 1d)));
	  }

	  /// <summary>
	  /// Test the result is returned unchanged if it is not ScenarioFxConvertible.
	  /// </summary>
	  public virtual void convertResultCurrencyNotConvertible()
	  {
		TestFunction fn = new TestFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).build();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(ScenarioArray.of("bar"));
	  }

	  /// <summary>
	  /// Test a non-convertible result is returned even if there is no reporting currency.
	  /// </summary>
	  public virtual void nonConvertibleResultReturnedWhenNoReportingCurrency()
	  {
		TestFunction fn = new TestFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).build();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(ScenarioArray.of("bar"));
	  }

	  /// <summary>
	  /// Test that a failure is returned if currency conversion fails.
	  /// </summary>
	  public virtual void convertResultCurrencyConversionFails()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyScenarioArray list = CurrencyScenarioArray.of(GBP, values);
		// Market data doesn't include FX rates, conversion to USD will fail
		ScenarioMarketData marketData = ScenarioMarketData.empty();
		ConvertibleFunction fn = ConvertibleFunction.of(() => list, GBP);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.CURRENCY_CONVERSION).hasFailureMessageMatching("Failed to convert value '.*' to currency 'USD'");
	  }

	  /// <summary>
	  /// Tests that executing a function wraps its return value in a success result.
	  /// </summary>
	  public virtual void execute()
	  {
		SupplierFunction<string> fn = SupplierFunction.of(() => "foo");
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).build();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(ScenarioArray.of("foo"));
	  }

	  /// <summary>
	  /// Test executing a bad function that fails to return expected measure.
	  /// </summary>
	  public virtual void executeMissingMeasure()
	  {
		// function claims it supports 'PresentValueMultiCurrency' but fails to return it when asked
		MeasureCheckFunction fn = new MeasureCheckFunction(ImmutableSet.of(TestingMeasures.PRESENT_VALUE), ("123"));
		CalculationTaskCell cell0 = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTaskCell cell1 = CalculationTaskCell.of(0, 1, TestingMeasures.PRESENT_VALUE_MULTI_CCY, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell0, cell1);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result0 = calculationResults.getCells().get(0).getResult();
		Result<object> result0 = calculationResults.Cells.get(0).Result;
		assertThat(result0).Success.hasValue(ImmutableSet.of(TestingMeasures.PRESENT_VALUE, TestingMeasures.PRESENT_VALUE_MULTI_CCY));
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result1 = calculationResults.getCells().get(1).getResult();
		Result<object> result1 = calculationResults.Cells.get(1).Result;
		assertThat(result1).isFailure(FailureReason.CALCULATION_FAILED).hasFailureMessageMatching("Function 'MeasureCheckFunction' did not return requested measure 'PresentValueMultiCurrency' for ID '123'");
	  }

	  /// <summary>
	  /// Tests that executing a function filters the set of measures sent to function.
	  /// </summary>
	  public virtual void executeFilterMeasures()
	  {
		// function does not support 'ParRate', so it should not be asked for it
		MeasureCheckFunction fn = new MeasureCheckFunction(ImmutableSet.of(TestingMeasures.PRESENT_VALUE), ("123"));
		CalculationTaskCell cell0 = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTaskCell cell1 = CalculationTaskCell.of(0, 1, TestingMeasures.PAR_RATE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell0, cell1);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result0 = calculationResults.getCells().get(0).getResult();
		Result<object> result0 = calculationResults.Cells.get(0).Result;
		assertThat(result0).Success.hasValue(ImmutableSet.of(TestingMeasures.PRESENT_VALUE)); // ParRate not requested
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result1 = calculationResults.getCells().get(1).getResult();
		Result<object> result1 = calculationResults.Cells.get(1).Result;
		assertThat(result1).isFailure(FailureReason.UNSUPPORTED).hasFailureMessageMatching("Measure 'ParRate' is not supported by function 'MeasureCheckFunction'");
	  }

	  /// <summary>
	  /// Tests that executing a function that throws an exception wraps the exception in a failure result.
	  /// </summary>
	  public virtual void executeException()
	  {
		SupplierFunction<string> fn = SupplierFunction.of(() =>
		{
		throw new System.ArgumentException("foo");
		});
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.CALCULATION_FAILED).hasFailureMessageMatching("Error when invoking function 'SupplierFunction' for ID '123': foo");
	  }

	  /// <summary>
	  /// Tests that executing a function that throws a market data exception wraps the exception in a failure result.
	  /// </summary>
	  public virtual void executeException_marketData()
	  {
		SupplierFunction<string> fn = SupplierFunction.of(() =>
		{
		throw new MarketDataNotFoundException("foo");
		});
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.MISSING_DATA).hasFailureMessageMatching("Missing market data when invoking function 'SupplierFunction' for ID '123': foo");
	  }

	  /// <summary>
	  /// Tests that executing a function that throws a market data exception wraps the exception in a failure result.
	  /// Target has no identifier.
	  /// </summary>
	  public virtual void executeException_marketData_noIdentifier()
	  {
		SupplierFunction<string> fn = SupplierFunction.of(() =>
		{
		throw new MarketDataNotFoundException("foo");
		}, null);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.MISSING_DATA).hasFailureMessageMatching("Missing market data when invoking function 'SupplierFunction': foo: for target '.*'");
	  }

	  /// <summary>
	  /// Tests that executing a function that throws a reference data exception wraps the exception in a failure result.
	  /// </summary>
	  public virtual void executeException_referenceData()
	  {
		SupplierFunction<string> fn = SupplierFunction.of(() =>
		{
		throw new ReferenceDataNotFoundException("foo");
		});
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.MISSING_DATA).hasFailureMessageMatching("Missing reference data when invoking function 'SupplierFunction' for ID '123': foo");
	  }

	  /// <summary>
	  /// Tests that executing a function that throws an unsupported exception wraps the exception in a failure result.
	  /// </summary>
	  public virtual void executeException_unsupported()
	  {
		SupplierFunction<string> fn = SupplierFunction.of(() =>
		{
		throw new System.NotSupportedException("foo");
		});
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.UNSUPPORTED).hasFailureMessageMatching("Unsupported operation when invoking function 'SupplierFunction' for ID '123': foo");
	  }

	  /// <summary>
	  /// Tests that executing a function that returns a success result returns the underlying result without wrapping it.
	  /// </summary>
	  public virtual void executeSuccessResultValue()
	  {
		SupplierFunction<Result<ScenarioArray<string>>> fn = SupplierFunction.of(() => Result.success(ScenarioArray.of("foo")));
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ImmutableScenarioMarketData.builder(date(2011, 3, 8)).build();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).hasValue(ScenarioArray.of("foo"));
	  }

	  /// <summary>
	  /// Tests that executing a function that returns a failure result returns the underlying result without wrapping it.
	  /// </summary>
	  public virtual void executeFailureResultValue()
	  {
		SupplierFunction<Result<string>> fn = SupplierFunction.of(() => Result.failure(FailureReason.NOT_APPLICABLE, "bar"));
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		ScenarioMarketData marketData = ScenarioMarketData.empty();

		CalculationResults calculationResults = task.execute(marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCells().get(0).getResult();
		Result<object> result = calculationResults.Cells.get(0).Result;
		assertThat(result).isFailure(FailureReason.NOT_APPLICABLE).hasFailureMessageMatching("bar");
	  }

	  /// <summary>
	  /// Tests that requirements are added for the FX rates needed to convert the results into the reporting currency.
	  /// </summary>
	  public virtual void fxConversionRequirements()
	  {
		OutputCurrenciesFunction fn = new OutputCurrenciesFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		MarketDataRequirements requirements = task.requirements(REF_DATA);

		assertThat(requirements.NonObservables).containsOnly(FxRateId.of(GBP, USD, OBS_SOURCE), FxRateId.of(EUR, USD, OBS_SOURCE));
	  }

	  /// <summary>
	  /// Tests that no requirements are added when not performing currency conversion.
	  /// </summary>
	  public virtual void fxConversionRequirements_noConversion()
	  {
		OutputCurrenciesFunction fn = new OutputCurrenciesFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, ReportingCurrency.NONE);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		MarketDataRequirements requirements = task.requirements(REF_DATA);

		assertThat(requirements.NonObservables).Empty;
	  }

	  public virtual void testToString()
	  {
		OutputCurrenciesFunction fn = new OutputCurrenciesFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(1, 2, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		assertThat(task.ToString()).isEqualTo("CalculationTask[CalculationTaskCell[(1, 2), measure=PresentValue, currency=Specific:USD]]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OutputCurrenciesFunction fn = new OutputCurrenciesFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(1, 2, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask test = CalculationTask.of(TARGET, fn, cell);
		coverImmutableBean(test);

		OutputCurrenciesFunction fn2 = new OutputCurrenciesFunction();
		CalculationTaskCell cell2 = CalculationTaskCell.of(1, 3, TestingMeasures.PRESENT_VALUE, REPORTING_CURRENCY_USD);
		CalculationTask test2 = CalculationTask.of(new TestTarget(), fn2, cell2);
		coverBeanEquals(test, test2);
		assertNotNull(CalculationTask.meta());
	  }

	  //-------------------------------------------------------------------------
	  internal class TestTarget : CalculationTarget
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Function that returns a value that is not currency convertible.
	  /// </summary>
	  public sealed class TestFunction : CalculationFunction<TestTarget>
	  {

		public Type<TestTarget> targetType()
		{
		  return typeof(TestTarget);
		}

		public ISet<Measure> supportedMeasures()
		{
		  return MEASURES;
		}

		public Currency naturalCurrency(TestTarget trade, ReferenceData refData)
		{
		  return USD;
		}

		public FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  return FunctionRequirements.builder().valueRequirements(ImmutableSet.of(TestId.of("1"), TestObservableId.of("2"))).timeSeriesRequirements(TestObservableId.of("3")).observableSource(OBS_SOURCE).build();
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  ScenarioArray<string> array = ScenarioArray.of("bar");
		  return ImmutableMap.of(TestingMeasures.PRESENT_VALUE, Result.success(array));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Function that returns a value that is currency convertible.
	  /// </summary>
	  private sealed class ConvertibleFunction : CalculationFunction<TestTarget>
	  {

		internal readonly System.Func<CurrencyScenarioArray> supplier;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal readonly Currency naturalCurrency_Renamed;

		internal static ConvertibleFunction of(System.Func<CurrencyScenarioArray> supplier, Currency naturalCurrency)
		{
		  return new ConvertibleFunction(supplier, naturalCurrency);
		}

		internal ConvertibleFunction(System.Func<CurrencyScenarioArray> supplier, Currency naturalCurrency)
		{
		  this.supplier = supplier;
		  this.naturalCurrency_Renamed = naturalCurrency;
		}

		public Type<TestTarget> targetType()
		{
		  return typeof(TestTarget);
		}

		public ISet<Measure> supportedMeasures()
		{
		  return MEASURES;
		}

		public override Optional<string> identifier(TestTarget target)
		{
		  return ("123");
		}

		public Currency naturalCurrency(TestTarget trade, ReferenceData refData)
		{
		  return naturalCurrency_Renamed;
		}

		public FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  return FunctionRequirements.empty();
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  Result<CurrencyScenarioArray> result = Result.success(supplier.get());
		  return ImmutableMap.of(TestingMeasures.PRESENT_VALUE, result, TestingMeasures.PRESENT_VALUE_MULTI_CCY, result);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Function that returns a value from a Supplier.
	  /// </summary>
	  private sealed class SupplierFunction<T> : CalculationFunction<TestTarget>
	  {

		internal readonly System.Func<T> supplier;
		internal readonly Optional<string> id;

		public static SupplierFunction<T> of<T>(System.Func<T> supplier)
		{
		  return of(supplier, ("123"));
		}

		public static SupplierFunction<T> of<T>(System.Func<T> supplier, Optional<string> id)
		{
		  return new SupplierFunction<T>(supplier, id);
		}

		internal SupplierFunction(System.Func<T> supplier, Optional<string> id)
		{
		  this.supplier = supplier;
		  this.id = id;
		}

		public Type<TestTarget> targetType()
		{
		  return typeof(TestTarget);
		}

		public ISet<Measure> supportedMeasures()
		{
		  return MEASURES;
		}

		public override Optional<string> identifier(TestTarget target)
		{
		  return id;
		}

		public Currency naturalCurrency(TestTarget trade, ReferenceData refData)
		{
		  return USD;
		}

		public FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  return FunctionRequirements.empty();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  T obj = supplier.get();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: if (obj instanceof com.opengamma.strata.collect.result.Result<?>)
		  if (obj is Result<object>)
		  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return com.google.common.collect.ImmutableMap.of(com.opengamma.strata.calc.TestingMeasures.PRESENT_VALUE, (com.opengamma.strata.collect.result.Result<?>) obj);
			return ImmutableMap.of(TestingMeasures.PRESENT_VALUE, (Result<object>) obj);
		  }
		  ScenarioArray<object> array = ScenarioArray.of(obj);
		  return ImmutableMap.of(TestingMeasures.PRESENT_VALUE, Result.success(array));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Function that returns a value from a Supplier.
	  /// </summary>
	  private sealed class MeasureCheckFunction : CalculationFunction<TestTarget>
	  {

		internal readonly ISet<Measure> resultMeasures;
		internal readonly Optional<string> id;

		internal MeasureCheckFunction(ISet<Measure> resultMeasures, Optional<string> id)
		{
		  this.resultMeasures = resultMeasures;
		  this.id = id;
		}

		public Type<TestTarget> targetType()
		{
		  return typeof(TestTarget);
		}

		public ISet<Measure> supportedMeasures()
		{
		  return MEASURES;
		}

		public override Optional<string> identifier(TestTarget target)
		{
		  return id;
		}

		public Currency naturalCurrency(TestTarget trade, ReferenceData refData)
		{
		  return USD;
		}

		public FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  return FunctionRequirements.empty();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> map = new java.util.HashMap<>();
		  IDictionary<Measure, Result<object>> map = new Dictionary<Measure, Result<object>>();
		  foreach (Measure measure in resultMeasures)
		  {
			map[measure] = Result.success(measures);
		  }
		  return map;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Function that returns requirements containing output currencies.
	  /// </summary>
	  private sealed class OutputCurrenciesFunction : CalculationFunction<TestTarget>
	  {

		public Type<TestTarget> targetType()
		{
		  return typeof(TestTarget);
		}

		public ISet<Measure> supportedMeasures()
		{
		  return MEASURES;
		}

		public Currency naturalCurrency(TestTarget trade, ReferenceData refData)
		{
		  return USD;
		}

		public FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  return FunctionRequirements.builder().outputCurrencies(GBP, EUR, USD).observableSource(OBS_SOURCE).build();
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  throw new System.NotSupportedException("calculate not implemented");
		}
	  }

	}

}
using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.BUCKETED_PV01;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.CASH_FLOWS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.PAR_RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.PRESENT_VALUE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.PRESENT_VALUE_MULTI_CCY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using TestObservableId = com.opengamma.strata.calc.marketdata.TestObservableId;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DerivedCalculationFunctionTest
	public class DerivedCalculationFunctionTest
	{

	  /// <summary>
	  /// Tests all measures are calculated by the derived function and the underlying function.
	  /// </summary>
	  public virtual void calculateMeasure()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5), PRESENT_VALUE, Result.success(7));
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), new DelegateFn(delegateResults));

		ISet<Measure> measures = ImmutableSet.of(BUCKETED_PV01, CASH_FLOWS, PAR_RATE, PRESENT_VALUE);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		assertThat(wrapper.supportedMeasures()).isEqualTo(measures);
		assertThat(wrapper.targetType()).isEqualTo(typeof(TestTarget));
		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[BUCKETED_PV01]).hasValue(35);
		assertThat(results[CASH_FLOWS]).hasValue(3);
		assertThat(results[PAR_RATE]).hasValue(5);
		assertThat(results[PRESENT_VALUE]).hasValue(7);
	  }

	  /// <summary>
	  /// Test two derived function composed together
	  /// </summary>
	  public virtual void calculateMeasuresNestedDerivedClasses()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5), PRESENT_VALUE, Result.success(7));
		DerivedFn derivedFn1 = new DerivedFn(BUCKETED_PV01);
		DerivedFn derivedFn2 = new DerivedFn(PRESENT_VALUE_MULTI_CCY);
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(derivedFn1, new DelegateFn(delegateResults));
		wrapper = new DerivedCalculationFunctionWrapper<>(derivedFn2, wrapper);

		ISet<Measure> measures = ImmutableSet.of(BUCKETED_PV01, PRESENT_VALUE_MULTI_CCY, CASH_FLOWS, PAR_RATE, PRESENT_VALUE);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		assertThat(wrapper.supportedMeasures()).isEqualTo(measures);
		assertThat(wrapper.targetType()).isEqualTo(typeof(TestTarget));
		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[BUCKETED_PV01]).hasValue(35);
		assertThat(results[PRESENT_VALUE_MULTI_CCY]).hasValue(35);
		assertThat(results[CASH_FLOWS]).hasValue(3);
		assertThat(results[PAR_RATE]).hasValue(5);
		assertThat(results[PRESENT_VALUE]).hasValue(7);
	  }

	  /// <summary>
	  /// Test that the derived measure isn't calculated unless it is requested.
	  /// </summary>
	  public virtual void derivedMeasureNotRequested()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PRESENT_VALUE, Result.success(7));
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), new DelegateFn(delegateResults));

		ISet<Measure> measures = ImmutableSet.of(CASH_FLOWS, PRESENT_VALUE);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[CASH_FLOWS]).hasValue(3);
		assertThat(results[PRESENT_VALUE]).hasValue(7);
	  }

	  /// <summary>
	  /// Test that measures aren't returned if they are needed to calculate the derived measure but aren't
	  /// requested by the user.
	  /// </summary>
	  public virtual void requiredMeasureNotReturned()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5), PRESENT_VALUE, Result.success(7));
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), new DelegateFn(delegateResults));

		ISet<Measure> measures = ImmutableSet.of(BUCKETED_PV01);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[BUCKETED_PV01]).hasValue(35);
	  }

	  /// <summary>
	  /// Test the behaviour when the underlying function doesn't support the measures required by the derived function.
	  /// </summary>
	  public virtual void requiredMeasuresNotSupported()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(PAR_RATE, com.opengamma.strata.collect.result.Result.success(5), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(PAR_RATE, Result.success(5), PRESENT_VALUE, Result.success(7));
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), new DelegateFn(delegateResults));

		ISet<Measure> measures = ImmutableSet.of(BUCKETED_PV01, PAR_RATE);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		// The derived measure isn't supported because its required measure isn't available
		assertThat(wrapper.supportedMeasures()).isEqualTo(ImmutableSet.of(PAR_RATE, PRESENT_VALUE));
		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[BUCKETED_PV01]).hasFailureMessageMatching(".*cannot calculate the required measures.*");
		assertThat(results[PAR_RATE]).hasValue(5);
	  }

	  /// <summary>
	  /// Test the derived measure result is a failure if any of the required measures are failures
	  /// </summary>
	  public virtual void requiredMeasureFails()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.failure(com.opengamma.strata.collect.result.FailureReason.ERROR, "Failed to calculate bar"), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.failure(FailureReason.ERROR, "Failed to calculate bar"), PAR_RATE, Result.success(5), PRESENT_VALUE, Result.success(7));
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), new DelegateFn(delegateResults));

		ISet<Measure> measures = ImmutableSet.of(BUCKETED_PV01);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[BUCKETED_PV01]).hasFailureMessageMatching("Failed to calculate bar");
	  }

	  /// <summary>
	  /// Test the behaviour when the delegate function returns no value for a measure it claims to support.
	  /// This is a bug in the function, it should always return a result for all measures that are supported and
	  /// were requested.
	  /// </summary>
	  public virtual void supportedMeasureNotReturned()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5), PRESENT_VALUE, com.opengamma.strata.collect.result.Result.success(7));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5), PRESENT_VALUE, Result.success(7));

		DelegateFn delegateFn = new DelegateFnAnonymousInnerClass(this, delegateResults, target);
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), delegateFn);

		ISet<Measure> measures = ImmutableSet.of(BUCKETED_PV01);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), com.opengamma.strata.data.scenario.ScenarioMarketData.empty(), com.opengamma.strata.basics.ReferenceData.standard());
		IDictionary<Measure, Result<object>> results = wrapper.calculate(target, measures, CalculationParameters.empty(), ScenarioMarketData.empty(), ReferenceData.standard());

		assertThat(results.Keys).isEqualTo(measures);
		assertThat(results[BUCKETED_PV01]).hasFailureMessageMatching(".*did not return the expected measures.*");
	  }

	  private class DelegateFnAnonymousInnerClass : DelegateFn
	  {
		  private readonly DerivedCalculationFunctionTest outerInstance;

		  private com.opengamma.strata.calc.runner.TestTarget target;

		  public DelegateFnAnonymousInnerClass<T1>(DerivedCalculationFunctionTest outerInstance, IDictionary<T1> delegateResults, com.opengamma.strata.calc.runner.TestTarget target) : base(delegateResults)
		  {
			  this.outerInstance = outerInstance;
			  this.target = target;
		  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		  public override IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		  {

			// Don't return TestingMeasures.CASH_FLOWS even though it should be supported
			return ImmutableMap.of(PAR_RATE, Result.success(5));
		  }
	  }

	  public virtual void requirements()
	  {
		TestTarget target = new TestTarget(10);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of();
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of();
		DerivedCalculationFunctionWrapper<TestTarget, int> wrapper = new DerivedCalculationFunctionWrapper<TestTarget, int>(new DerivedFn(), new DelegateFn(delegateResults));

		FunctionRequirements requirements = wrapper.requirements(target, ImmutableSet.of(), CalculationParameters.empty(), ReferenceData.empty());

		FunctionRequirements expected = FunctionRequirements.builder().valueRequirements(TestObservableId.of("a"), TestObservableId.of("b"), TestObservableId.of("d")).timeSeriesRequirements(TestObservableId.of("c"), TestObservableId.of("e")).outputCurrencies(Currency.GBP, Currency.EUR, Currency.USD).build();

		assertThat(requirements).isEqualTo(expected);
		assertThat(wrapper.naturalCurrency(target, ReferenceData.empty())).isEqualTo(Currency.AUD);
	  }
	}

	//--------------------------------------------------------------------------------------------------

	internal sealed class TestTarget : CalculationTarget
	{

	  private readonly int value;

	  internal TestTarget(int value)
	  {
		this.value = value;
	  }

	  public int Value
	  {
		  get
		  {
			return value;
		  }
	  }
	}

	internal sealed class DerivedFn : AbstractDerivedCalculationFunction<TestTarget, int>
	{

	  internal DerivedFn(Measure measure, ISet<Measure> requiredMeasures) : base(typeof(TestTarget), measure, requiredMeasures)
	  {
	  }

	  internal DerivedFn(Measure measure) : this(measure, ImmutableSet.of(CASH_FLOWS, PAR_RATE))
	  {
	  }

	  internal DerivedFn() : this(BUCKETED_PV01)
	  {
	  }

	  public int? calculate(TestTarget target, IDictionary<Measure, object> requiredMeasures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
	  {

		int? bar = (int?) requiredMeasures[CASH_FLOWS];
		int? baz = (int?) requiredMeasures[PAR_RATE];

		return target.Value * bar + baz;
	  }

	  public FunctionRequirements requirements(TestTarget target, CalculationParameters parameters, ReferenceData refData)
	  {
		return FunctionRequirements.builder().valueRequirements(TestObservableId.of("a"), TestObservableId.of("b")).timeSeriesRequirements(TestObservableId.of("c")).outputCurrencies(Currency.GBP).build();
	  }
	}

	//--------------------------------------------------------------------------------------------------

	internal class DelegateFn : CalculationFunction<TestTarget>
	{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results;
	  private readonly IDictionary<Measure, Result<object>> results;

	  internal DelegateFn<T1>(IDictionary<T1> results)
	  {
		this.results = results;
	  }

	  public virtual Type<TestTarget> targetType()
	  {
		return typeof(TestTarget);
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return results.Keys;
	  }

	  public virtual Currency naturalCurrency(TestTarget target, ReferenceData refData)
	  {
		return AUD;
	  }

	  public virtual FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		return FunctionRequirements.builder().valueRequirements(TestObservableId.of("d")).timeSeriesRequirements(TestObservableId.of("e")).outputCurrencies(Currency.EUR, Currency.USD).build();
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
	  {

		ISet<Measure> missingMeasures = Sets.difference(measures, results.Keys);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = com.opengamma.strata.collect.MapStream.of(this.results).filterKeys(measures::contains).toMap();
		IDictionary<Measure, Result<object>> results = MapStream.of(this.results).filterKeys(measures.contains).toMap();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> missingResults = missingMeasures.stream().collect(toMap(m -> m, this::missingResult));
		IDictionary<Measure, Result<object>> missingResults = missingMeasures.ToDictionary(m => m, this.missingResult);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> allResults = new java.util.HashMap<>(results);
		IDictionary<Measure, Result<object>> allResults = new Dictionary<Measure, Result<object>>(results);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		allResults.putAll(missingResults);
		return allResults;
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> missingResult(com.opengamma.strata.calc.Measure measure)
	  private Result<object> missingResult(Measure measure)
	  {
		return Result.failure(FailureReason.CALCULATION_FAILED, "{} not supported", measure);
	  }
	}

}
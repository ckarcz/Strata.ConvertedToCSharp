using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Sets = com.google.common.collect.Sets;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// A <seealso cref="CalculationFunction"/> implementation which wraps a <seealso cref="DerivedCalculationFunction"/>.
	/// <para>
	/// A derived calculation function calculates a measure using the measures calculated by another function.
	/// This functions takes care of calling the delegate function and passing the results to the derived function.
	/// </para>
	/// <para>
	/// Most of the logic is concerned with bookkeeping - packing and unpacking maps of measures and results before
	/// passing them on or returning them.
	/// </para>
	/// </summary>
	internal class DerivedCalculationFunctionWrapper<T, R> : CalculationFunction<T> where T : com.opengamma.strata.basics.CalculationTarget
	{

	  /// <summary>
	  /// The derived calculation function which calculates one measure.
	  /// <para>
	  /// The inputs to the measure can include measures calculated by the delegate calculation function.
	  /// </para>
	  /// </summary>
	  private readonly DerivedCalculationFunction<T, R> derivedFunction;

	  /// <summary>
	  /// A calculation function whose results can be used by the derived calculation function.
	  /// </summary>
	  private readonly CalculationFunction<T> @delegate;

	  /// <summary>
	  /// The measures supported by this function; the union of the measures supported by the delegate function and
	  /// the derived function.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ISet<Measure> supportedMeasures_Renamed;

	  /// <summary>
	  /// True if the delegate function supports all measures required by the calculation function.
	  /// If this is true the calculation function can be invoked.
	  /// If it is false the only measures which can be calculated are the measures supported by the delegate.
	  /// </summary>
	  private readonly bool requiredMeasuresSupported;

	  /// <summary>
	  /// Creates a new function which invokes the delegate function, passes the result to the derived function
	  /// and returns the combined results.
	  /// </summary>
	  /// <param name="derivedFunction">  a function which calculates one measure using the measure values calculated by the other function </param>
	  /// <param name="delegate">  a function which calculates multiple measures </param>
	  internal DerivedCalculationFunctionWrapper(DerivedCalculationFunction<T, R> derivedFunction, CalculationFunction<T> @delegate)
	  {

		this.derivedFunction = derivedFunction;
		this.@delegate = @delegate;

		ISet<Measure> delegateMeasures = @delegate.supportedMeasures();
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'containsAll' method:
		this.requiredMeasuresSupported = delegateMeasures.containsAll(derivedFunction.requiredMeasures());
		this.supportedMeasures_Renamed = requiredMeasuresSupported ? ImmutableSet.builder<Measure>().addAll(delegateMeasures).add(derivedFunction.measure()).build() : delegateMeasures;
	  }

	  public virtual Type<T> targetType()
	  {
		return derivedFunction.targetType();
	  }

	  public virtual ISet<Measure> supportedMeasures()
	  {
		return supportedMeasures_Renamed;
	  }

	  public override Optional<string> identifier(T target)
	  {
		return @delegate.identifier(target);
	  }

	  public virtual Currency naturalCurrency(T target, ReferenceData refData)
	  {
		return @delegate.naturalCurrency(target, refData);
	  }

	  public virtual FunctionRequirements requirements(T target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		FunctionRequirements delegateRequirements = @delegate.requirements(target, measures, parameters, refData);
		FunctionRequirements functionRequirements = derivedFunction.requirements(target, parameters, refData);
		return delegateRequirements.combinedWith(functionRequirements);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public virtual IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
	  {

		// The caller didn't ask for the derived measure so just return the measures calculated by the delegate
		Measure derivedMeasure = derivedFunction.measure();
		if (!measures.Contains(derivedMeasure))
		{
		  return @delegate.calculate(target, measures, parameters, marketData, refData);
		}
		// Add the measures required to calculate the derived measure to the measures requested by the caller
		ISet<Measure> allRequiredMeasures = Sets.union(measures, derivedFunction.requiredMeasures());
		ISet<Measure> requiredMeasures = Sets.difference(allRequiredMeasures, ImmutableSet.of(derivedMeasure));
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = delegate.calculate(target, requiredMeasures, parameters, marketData, refData);
		IDictionary<Measure, Result<object>> delegateResults = @delegate.calculate(target, requiredMeasures, parameters, marketData, refData);

		// Calculate the derived measure
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculateMeasure(target, delegateResults, parameters, marketData, refData);
		Result<object> result = calculateMeasure(target, delegateResults, parameters, marketData, refData);

		// The results containing only the requested measures and not including extra measures that were inserted above
		// Also filter out any results for calculationFunction.measure(). There will be failures from functions below
		// that don't support that measure.
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> requestedResults = com.opengamma.strata.collect.MapStream.of(delegateResults).filterKeys(measures::contains).filterKeys(measure -> !measure.equals(derivedMeasure)).toMap();
		IDictionary<Measure, Result<object>> requestedResults = MapStream.of(delegateResults).filterKeys(measures.contains).filterKeys(measure => !measure.Equals(derivedMeasure)).toMap();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return com.google.common.collect.ImmutableMap.builder<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>>().put(derivedMeasure, result).putAll(requestedResults).build();
		return ImmutableMap.builder<Measure, Result<object>>().put(derivedMeasure, result).putAll(requestedResults).build();
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> calculateMeasure(T target, java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  private Result<object> calculateMeasure<T1>(T target, IDictionary<T1> delegateResults, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
	  {

		if (!requiredMeasuresSupported)
		{
		  // Can't calculate the measure if the delegate can't calculate its inputs
		  return Result.failure(FailureReason.NOT_APPLICABLE, "The delegate function cannot calculate the required measures. Required measures: {}, " + "supported measures: {}, delegate {}", derivedFunction.requiredMeasures(), @delegate.supportedMeasures(), @delegate);
		}
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'containsAll' method:
		if (!delegateResults.Keys.containsAll(derivedFunction.requiredMeasures()))
		{
		  // There's a bug in the delegate function - it claims to support the required measures but didn't return
		  // a result for all of them.
		  return Result.failure(FailureReason.CALCULATION_FAILED, "Delegate did not return the expected measures. Required {}, actual {}, delegate {}", derivedFunction.requiredMeasures(), delegateResults.Keys, @delegate);
		}
		// Check whether all the required measures were successfully calculated
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> failures = com.opengamma.strata.collect.MapStream.of(delegateResults).filterKeys(derivedFunction.requiredMeasures()::contains).map(entry -> entry.getValue()).filter(result -> result.isFailure()).collect(toList());
		IList<Result<object>> failures = MapStream.of(delegateResults).filterKeys(derivedFunction.requiredMeasures().contains).map(entry => entry.Value).filter(result => result.Failure).collect(toList());

		if (failures.Count > 0)
		{
		  return Result.failure(failures);
		}
		// Unwrap the results before passing them to the function
		IDictionary<Measure, object> resultValues = MapStream.of(delegateResults).filterKeys(derivedFunction.requiredMeasures().contains).mapValues(result => (object) result.Value).toMap();
		return Result.of(() => derivedFunction.calculate(target, resultValues, parameters, marketData, refData));
	  }
	}

}
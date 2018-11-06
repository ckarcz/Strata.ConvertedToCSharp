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
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Sets = com.google.common.collect.Sets;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using MarketDataRequirementsBuilder = com.opengamma.strata.calc.marketdata.MarketDataRequirementsBuilder;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioFxRateProvider = com.opengamma.strata.data.scenario.ScenarioFxRateProvider;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// A single task that will be used to perform a calculation.
	/// <para>
	/// This is a single unit of execution in the calculation runner.
	/// It consists of a <seealso cref="CalculationFunction"/> and the appropriate inputs,
	/// including a single <seealso cref="CalculationTarget"/>. When invoked, it will
	/// calculate a result for one or more columns in the grid of results.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class CalculationTask implements org.joda.beans.ImmutableBean
	public sealed class CalculationTask : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.CalculationTarget target;
		private readonly CalculationTarget target;
	  /// <summary>
	  /// The function that will calculate the value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CalculationFunction<com.opengamma.strata.basics.CalculationTarget> function;
	  private readonly CalculationFunction<CalculationTarget> function;
	  /// <summary>
	  /// The additional parameters.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CalculationParameters parameters;
	  private readonly CalculationParameters parameters;
	  /// <summary>
	  /// The cells to be calculated.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final java.util.List<CalculationTaskCell> cells;
	  private readonly IList<CalculationTaskCell> cells;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that will calculate the specified cells.
	  /// <para>
	  /// The cells must all be for the same row index and none of the column indices must overlap.
	  /// The result will contain no calculation parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target for which the value will be calculated </param>
	  /// <param name="function">  the function that performs the calculation </param>
	  /// <param name="cells">  the cells to be calculated by this task </param>
	  /// <returns> the task </returns>
	  public static CalculationTask of<T1>(CalculationTarget target, CalculationFunction<T1> function, params CalculationTaskCell[] cells) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		return of(target, function, CalculationParameters.empty(), ImmutableList.copyOf(cells));
	  }

	  /// <summary>
	  /// Obtains an instance that will calculate the specified cells.
	  /// <para>
	  /// The cells must all be for the same row index and none of the column indices must overlap.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target for which the value will be calculated </param>
	  /// <param name="function">  the function that performs the calculation </param>
	  /// <param name="parameters">  the additional parameters </param>
	  /// <param name="cells">  the cells to be calculated by this task </param>
	  /// <returns> the task </returns>
	  public static CalculationTask of<T1>(CalculationTarget target, CalculationFunction<T1> function, CalculationParameters parameters, IList<CalculationTaskCell> cells) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") CalculationFunction<com.opengamma.strata.basics.CalculationTarget> functionCast = (CalculationFunction<com.opengamma.strata.basics.CalculationTarget>) function;
		CalculationFunction<CalculationTarget> functionCast = (CalculationFunction<CalculationTarget>) function;
		return new CalculationTask(target, functionCast, parameters, cells);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index of the row in the grid of results.
	  /// </summary>
	  /// <returns> the row index </returns>
	  public int RowIndex
	  {
		  get
		  {
			return cells[0].RowIndex;
		  }
	  }

	  /// <summary>
	  /// Gets the set of measures that will be calculated by this task.
	  /// </summary>
	  /// <returns> the measures </returns>
	  public ISet<Measure> Measures
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return cells.Select(c => c.Measure).collect(toImmutableSet());
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns requirements specifying the market data the function needs to perform its calculations.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> requirements specifying the market data the function needs to perform its calculations </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public com.opengamma.strata.calc.marketdata.MarketDataRequirements requirements(com.opengamma.strata.basics.ReferenceData refData)
	  public MarketDataRequirements requirements(ReferenceData refData)
	  {
		// determine market data requirements of the function
		FunctionRequirements functionRequirements = function.requirements(target, Measures, parameters, refData);
		ObservableSource obsSource = functionRequirements.ObservableSource;

		// convert function requirements to market data requirements
		MarketDataRequirementsBuilder requirementsBuilder = MarketDataRequirements.builder();
		foreach (ObservableId id in functionRequirements.TimeSeriesRequirements)
		{
		  requirementsBuilder.addTimeSeries(id.withObservableSource(obsSource));
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (com.opengamma.strata.data.MarketDataId<?> id : functionRequirements.getValueRequirements())
		foreach (MarketDataId<object> id in functionRequirements.ValueRequirements)
		{
		  if (id is ObservableId)
		  {
			requirementsBuilder.addValues(((ObservableId) id).withObservableSource(obsSource));
		  }
		  else
		  {
			requirementsBuilder.addValues(id);
		  }
		}

		// add requirements for the FX rates needed to convert the output values into the reporting currency
		foreach (CalculationTaskCell cell in cells)
		{
		  if (cell.Measure.CurrencyConvertible && !cell.ReportingCurrency.None)
		  {
			Currency reportingCurrency = cell.reportingCurrency(this, refData);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			IList<MarketDataId<FxRate>> fxRateIds = functionRequirements.OutputCurrencies.Where(outputCurrency => !outputCurrency.Equals(reportingCurrency)).Select(outputCurrency => CurrencyPair.of(outputCurrency, reportingCurrency)).Select(pair => FxRateId.of(pair, obsSource)).collect(toImmutableList());
			requirementsBuilder.addValues(fxRateIds);
		  }
		}
		return requirementsBuilder.build();
	  }

	  /// <summary>
	  /// Determines the natural currency of the target.
	  /// <para>
	  /// This is only called for measures that are currency convertible.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the natural currency </returns>
	  public Currency naturalCurrency(ReferenceData refData)
	  {
		return function.naturalCurrency(target, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Executes the task, performing calculations for the target using multiple sets of market data.
	  /// <para>
	  /// This invokes the function with the correct set of market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data used in the calculation </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> results of the calculation, one for every scenario in the market data </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public CalculationResults execute(com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public CalculationResults execute(ScenarioMarketData marketData, ReferenceData refData)
	  {
		// calculate the results
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> results = calculate(marketData, refData);
		IDictionary<Measure, Result<object>> results = calculate(marketData, refData);

		// get a suitable FX provider
		ScenarioFxRateProvider fxProvider = parameters.findParameter(typeof(FxRateLookup)).map(lookup => LookupScenarioFxRateProvider.of(marketData, lookup)).orElse(ScenarioFxRateProvider.of(marketData));

		// convert the results, using a normal loop for better stack traces
		ImmutableList.Builder<CalculationResult> resultBuilder = ImmutableList.builder();
		foreach (CalculationTaskCell cell in cells)
		{
		  resultBuilder.add(cell.createResult(this, target, results, fxProvider, refData));
		}

		// return the result
		return CalculationResults.of(target, resultBuilder.build());
	  }

	  // calculates the result
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  private IDictionary<Measure, Result<object>> calculate(ScenarioMarketData marketData, ReferenceData refData)
	  {
		try
		{
		  ISet<Measure> requestedMeasures = Measures;
		  ISet<Measure> supportedMeasures = function.supportedMeasures();
		  ISet<Measure> measures = Sets.intersection(requestedMeasures, supportedMeasures);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> map = com.google.common.collect.ImmutableMap.of();
		  IDictionary<Measure, Result<object>> map = ImmutableMap.of();
		  if (measures.Count > 0)
		  {
			map = function.calculate(target, measures, parameters, marketData, refData);
		  }
		  // check if result does not contain all requested measures
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the java.util.Collection 'containsAll' method:
		  if (!map.Keys.containsAll(requestedMeasures))
		  {
			return handleMissing(requestedMeasures, supportedMeasures, map);
		  }
		  return map;

		}
		catch (Exception ex)
		{
		  return handleFailure(ex);
		}
	  }

	  // populate the result with failures
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> handleMissing(java.util.Set<com.opengamma.strata.calc.Measure> requestedMeasures, java.util.Set<com.opengamma.strata.calc.Measure> supportedMeasures, java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculatedResults)
	  private IDictionary<Measure, Result<object>> handleMissing<T1>(ISet<Measure> requestedMeasures, ISet<Measure> supportedMeasures, IDictionary<T1> calculatedResults)
	  {

		// need to add missing measures
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> updated = new java.util.HashMap<>(calculatedResults);
		IDictionary<Measure, Result<object>> updated = new Dictionary<Measure, Result<object>>(calculatedResults);
		string fnName = function.GetType().Name;
		foreach (Measure requestedMeasure in requestedMeasures)
		{
		  if (!calculatedResults.ContainsKey(requestedMeasure))
		  {
			if (supportedMeasures.Contains(requestedMeasure))
			{
			  string msg = function.identifier(target).map(v => "for ID '" + v + "'").orElse("for target '" + target.ToString() + "'");
			  updated[requestedMeasure] = Result.failure(FailureReason.CALCULATION_FAILED, "Function '{}' did not return requested measure '{}' {}", fnName, requestedMeasure, msg);
			}
			else
			{
			  updated[requestedMeasure] = Result.failure(FailureReason.UNSUPPORTED, "Measure '{}' is not supported by function '{}'", requestedMeasure, fnName);
			}
		  }
		}
		return updated;
	  }

	  // handle the failure, extracted to aid inlining
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> handleFailure(RuntimeException ex)
	  private IDictionary<Measure, Result<object>> handleFailure(Exception ex)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> failure;
		Result<object> failure;
		string fnName = function.GetType().Name;
		string exMsg = ex.Message;
		Optional<string> id = function.identifier(target);
		string msg = id.map(v => " for ID '" + v + "': " + exMsg).orElse(": " + exMsg + ": for target '" + target.ToString() + "'");
		if (ex is MarketDataNotFoundException)
		{
		  failure = Result.failure(FailureReason.MISSING_DATA, ex, "Missing market data when invoking function '{}'{}", fnName, msg);

		}
		else if (ex is ReferenceDataNotFoundException)
		{
		  failure = Result.failure(FailureReason.MISSING_DATA, ex, "Missing reference data when invoking function '{}'{}", fnName, msg);

		}
		else if (ex is System.NotSupportedException)
		{
		  failure = Result.failure(FailureReason.UNSUPPORTED, ex, "Unsupported operation when invoking function '{}'{}", fnName, msg);

		}
		else
		{
		  failure = Result.failure(FailureReason.CALCULATION_FAILED, ex, "Error when invoking function '{}'{}", fnName, msg);
		}
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return Measures.collect(toImmutableMap(m => m, m => failure));
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return "CalculationTask" + cells;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationTask}.
	  /// </summary>
	  private static readonly TypedMetaBean<CalculationTask> META_BEAN = LightMetaBean.of(typeof(CalculationTask), MethodHandles.lookup(), new string[] {"target", "function", "parameters", "cells"}, null, null, null, ImmutableList.of());

	  /// <summary>
	  /// The meta-bean for {@code CalculationTask}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<CalculationTask> meta()
	  {
		return META_BEAN;
	  }

	  static CalculationTask()
	  {
		MetaBean.register(META_BEAN);
	  }

	  private CalculationTask(CalculationTarget target, CalculationFunction<CalculationTarget> function, CalculationParameters parameters, IList<CalculationTaskCell> cells)
	  {
		JodaBeanUtils.notNull(target, "target");
		JodaBeanUtils.notNull(function, "function");
		JodaBeanUtils.notNull(parameters, "parameters");
		JodaBeanUtils.notEmpty(cells, "cells");
		this.target = target;
		this.function = function;
		this.parameters = parameters;
		this.cells = ImmutableList.copyOf(cells);
	  }

	  public override TypedMetaBean<CalculationTask> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the target for which the value will be calculated.
	  /// This is typically a trade. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationTarget Target
	  {
		  get
		  {
			return target;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the function that will calculate the value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationFunction<CalculationTarget> Function
	  {
		  get
		  {
			return function;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional parameters. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationParameters Parameters
	  {
		  get
		  {
			return parameters;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the cells to be calculated. </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public IList<CalculationTaskCell> Cells
	  {
		  get
		  {
			return cells;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  CalculationTask other = (CalculationTask) obj;
		  return JodaBeanUtils.equal(target, other.target) && JodaBeanUtils.equal(function, other.function) && JodaBeanUtils.equal(parameters, other.parameters) && JodaBeanUtils.equal(cells, other.cells);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(target);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(function);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cells);
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
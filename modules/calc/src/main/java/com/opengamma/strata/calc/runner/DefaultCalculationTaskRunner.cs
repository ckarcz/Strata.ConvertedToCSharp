using System.Collections.Generic;
using System.Threading;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// The default calculation task runner.
	/// <para>
	/// This uses a single instance of <seealso cref="ExecutorService"/>.
	/// </para>
	/// </summary>
	internal sealed class DefaultCalculationTaskRunner : CalculationTaskRunner
	{

	  /// <summary>
	  /// Executes the tasks that perform the individual calculations.
	  /// This will typically be multi-threaded, but single or direct executors also work.
	  /// </summary>
	  private readonly ExecutorService executor;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a standard multi-threaded calculation task runner capable of performing calculations.
	  /// <para>
	  /// This factory creates an executor basing the number of threads on the number of available processors.
	  /// It is recommended to use try-with-resources to manage the runner:
	  /// <pre>
	  ///  try (DefaultCalculationTaskRunner runner = DefaultCalculationTaskRunner.ofMultiThreaded()) {
	  ///    // use the runner
	  ///  }
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calculation task runner </returns>
	  internal static DefaultCalculationTaskRunner ofMultiThreaded()
	  {
		return new DefaultCalculationTaskRunner(createExecutor(Runtime.Runtime.availableProcessors()));
	  }

	  /// <summary>
	  /// Creates a calculation task runner capable of performing calculations, specifying the executor.
	  /// <para>
	  /// It is the callers responsibility to manage the life-cycle of the executor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="executor">  the executor to use </param>
	  /// <returns> the calculation task runner </returns>
	  internal static DefaultCalculationTaskRunner of(ExecutorService executor)
	  {
		return new DefaultCalculationTaskRunner(executor);
	  }

	  // create an executor with daemon threads
	  private static ExecutorService createExecutor(int threads)
	  {
		int effectiveThreads = (threads <= 0 ? Runtime.Runtime.availableProcessors() : threads);
		ThreadFactory threadFactory = r =>
		{
	  Thread t = Executors.defaultThreadFactory().newThread(r);
	  t.Name = "CalculationTaskRunner-" + t.Name;
	  t.Daemon = true;
	  return t;
		};
		return Executors.newFixedThreadPool(effectiveThreads, threadFactory);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance specifying the executor to use.
	  /// </summary>
	  /// <param name="executor">  the executor that is used to perform the calculations </param>
	  private DefaultCalculationTaskRunner(ExecutorService executor)
	  {
		this.executor = ArgChecker.notNull(executor, "executor");
	  }

	  //-------------------------------------------------------------------------
	  public Results calculate(CalculationTasks tasks, MarketData marketData, ReferenceData refData)
	  {

		// perform the calculations
		ScenarioMarketData md = ScenarioMarketData.of(1, marketData);
		Results results = calculateMultiScenario(tasks, md, refData);

		// unwrap the results
		// since there is only one scenario it is not desirable to return scenario result containers
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> mappedResults = results.getCells().stream().map(r -> unwrapScenarioResult(r)).collect(toImmutableList());
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Result<object>> mappedResults = results.Cells.Select(r => unwrapScenarioResult(r)).collect(toImmutableList());
		return Results.of(results.Columns, mappedResults);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Unwraps the result from an instance of <seealso cref="ScenarioArray"/> containing a single result.
	  /// <para>
	  /// When the user executes a single scenario the functions are invoked with a set of scenario market data
	  /// of size 1. This means the functions are simpler and always deal with scenarios. But if the user has
	  /// asked for a single set of results they don't want to see a collection of size 1 so the scenario results
	  /// need to be unwrapped.
	  /// </para>
	  /// <para>
	  /// If {@code result} is a failure or doesn't contain a {@code ScenarioArray} it is returned.
	  /// </para>
	  /// <para>
	  /// If this method is called with a {@code ScenarioArray} containing more than one value it throws an exception.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static com.opengamma.strata.collect.result.Result<?> unwrapScenarioResult(com.opengamma.strata.collect.result.Result<?> result)
	  private static Result<object> unwrapScenarioResult<T1>(Result<T1> result)
	  {
		if (result.Failure)
		{
		  return result;
		}
		object value = result.Value;
		if (!(value is ScenarioArray))
		{
		  return result;
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.ScenarioArray<?> scenarioResult = (com.opengamma.strata.data.scenario.ScenarioArray<?>) value;
		ScenarioArray<object> scenarioResult = (ScenarioArray<object>) value;

		if (scenarioResult.ScenarioCount != 1)
		{
		  throw new System.ArgumentException(Messages.format("Expected one result but found {} in {}", scenarioResult.ScenarioCount, scenarioResult));
		}
		return Result.success(scenarioResult.get(0));
	  }

	  public void calculateAsync(CalculationTasks tasks, MarketData marketData, ReferenceData refData, CalculationListener listener)
	  {

		// the listener is decorated to unwrap ScenarioArrays containing a single result
		ScenarioMarketData md = ScenarioMarketData.of(1, marketData);
		UnwrappingListener unwrappingListener = new UnwrappingListener(listener);
		calculateMultiScenarioAsync(tasks, md, refData, unwrappingListener);
	  }

	  //-------------------------------------------------------------------------
	  public Results calculateMultiScenario(CalculationTasks tasks, ScenarioMarketData marketData, ReferenceData refData)
	  {

		ResultsListener listener = new ResultsListener();
		calculateMultiScenarioAsync(tasks, marketData, refData, listener);
		return listener.result();
	  }

	  public void calculateMultiScenarioAsync(CalculationTasks tasks, ScenarioMarketData marketData, ReferenceData refData, CalculationListener listener)
	  {

		IList<CalculationTask> taskList = tasks.Tasks;
		// the listener is invoked via this wrapper
		// the wrapper ensures thread-safety for the listener
		// it also calls the listener with single CalculationResult cells, not CalculationResults
		System.Action<CalculationResults> consumer = new ListenerWrapper(listener, taskList.Count, tasks.Targets, tasks.Columns);

		// run each task using the executor
		taskList.ForEach(task => runTask(task, marketData, refData, consumer));
	  }

	  // submits a task to the executor to be run
	  private void runTask(CalculationTask task, ScenarioMarketData marketData, ReferenceData refData, System.Action<CalculationResults> consumer)
	  {

		// the task is executed, with the result passed to the consumer
		// the consumer wraps the listener to ensure thread-safety
		System.Func<CalculationResults> taskExecutor = () => task.execute(marketData, refData);
		CompletableFuture.supplyAsync(taskExecutor, executor).thenAccept(consumer);
	  }

	  //-------------------------------------------------------------------------
	  public void close()
	  {
		executor.shutdown();
	  }

	  //-------------------------------------------------------------------------

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Listener that decorates another listener and unwraps <seealso cref="ScenarioArray"/> instances
	  /// containing a single value before passing the value to the delegate listener.
	  /// This is used by the single scenario async method.
	  /// </summary>
	  private sealed class UnwrappingListener : CalculationListener
	  {

		internal readonly CalculationListener @delegate;

		internal UnwrappingListener(CalculationListener @delegate)
		{
		  this.@delegate = @delegate;
		}

		public override void calculationsStarted(IList<CalculationTarget> targets, IList<Column> columns)
		{
		  @delegate.calculationsStarted(targets, columns);
		}

		public void resultReceived(CalculationTarget target, CalculationResult calculationResult)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResult.getResult();
		  Result<object> result = calculationResult.Result;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> unwrappedResult = unwrapScenarioResult(result);
		  Result<object> unwrappedResult = unwrapScenarioResult(result);
		  CalculationResult unwrappedCalculationResult = calculationResult.withResult(unwrappedResult);
		  @delegate.resultReceived(target, unwrappedCalculationResult);
		}

		public void calculationsComplete()
		{
		  @delegate.calculationsComplete();
		}
	  }

	}

}
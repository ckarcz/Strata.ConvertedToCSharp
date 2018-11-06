using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CalculationListener = com.opengamma.strata.calc.runner.CalculationListener;
	using CalculationTaskRunner = com.opengamma.strata.calc.runner.CalculationTaskRunner;
	using CalculationTasks = com.opengamma.strata.calc.runner.CalculationTasks;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// The default calculation runner.
	/// <para>
	/// This delegates to an instance of <seealso cref="CalculationTaskRunner"/>.
	/// </para>
	/// </summary>
	internal class DefaultCalculationRunner : CalculationRunner
	{

	  /// <summary>
	  /// The underlying task runner.
	  /// </summary>
	  private readonly CalculationTaskRunner taskRunner;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a standard multi-threaded calculation runner capable of performing calculations.
	  /// <para>
	  /// This factory creates an executor basing the number of threads on the number of available processors.
	  /// It is recommended to use try-with-resources to manage the runner:
	  /// <pre>
	  ///  try (DefaultCalculationRunner runner = DefaultCalculationRunner.ofMultiThreaded()) {
	  ///    // use the runner
	  ///  }
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calculation runner </returns>
	  internal static DefaultCalculationRunner ofMultiThreaded()
	  {
		return new DefaultCalculationRunner(CalculationTaskRunner.ofMultiThreaded());
	  }

	  /// <summary>
	  /// Creates a calculation runner capable of performing calculations, specifying the executor.
	  /// <para>
	  /// It is the callers responsibility to manage the life-cycle of the executor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="executor">  the executor to use </param>
	  /// <returns> the calculation runner </returns>
	  internal static DefaultCalculationRunner of(ExecutorService executor)
	  {
		return new DefaultCalculationRunner(CalculationTaskRunner.of(executor));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance specifying the underlying task runner to use.
	  /// </summary>
	  /// <param name="taskRunner">  the underlying task runner </param>
	  internal DefaultCalculationRunner(CalculationTaskRunner taskRunner)
	  {
		this.taskRunner = ArgChecker.notNull(taskRunner, "taskRunner");
	  }

	  //-------------------------------------------------------------------------
	  public virtual Results calculate<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, MarketData marketData, ReferenceData refData) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		CalculationTasks tasks = CalculationTasks.of(calculationRules, targets, columns, refData);
		return taskRunner.calculate(tasks, marketData, refData);
	  }

	  public virtual void calculateAsync<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, MarketData marketData, ReferenceData refData, CalculationListener listener) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		CalculationTasks tasks = CalculationTasks.of(calculationRules, targets, columns, refData);
		taskRunner.calculateAsync(tasks, marketData, refData, listener);
	  }

	  //-------------------------------------------------------------------------
	  public virtual Results calculateMultiScenario<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, ScenarioMarketData marketData, ReferenceData refData) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		CalculationTasks tasks = CalculationTasks.of(calculationRules, targets, columns, refData);
		return taskRunner.calculateMultiScenario(tasks, marketData, refData);
	  }

	  public virtual void calculateMultiScenarioAsync<T1>(CalculationRules calculationRules, IList<T1> targets, IList<Column> columns, ScenarioMarketData marketData, ReferenceData refData, CalculationListener listener) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		CalculationTasks tasks = CalculationTasks.of(calculationRules, targets, columns, refData);
		taskRunner.calculateMultiScenarioAsync(tasks, marketData, refData, listener);
	  }

	  //-------------------------------------------------------------------------
	  public virtual CalculationTaskRunner TaskRunner
	  {
		  get
		  {
			return taskRunner;
		  }
	  }

	  public virtual void close()
	  {
		taskRunner.close();
	  }

	}

}
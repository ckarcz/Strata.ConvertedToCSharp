using System;
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
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.ReportingCurrency.NATURAL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.CollectProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using MoreExecutors = com.google.common.util.concurrent.MoreExecutors;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using TestTarget = com.opengamma.strata.calc.runner.CalculationTaskTest.TestTarget;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Test <seealso cref="CalculationTaskRunner"/> and <seealso cref="DefaultCalculationTaskRunner"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(singleThreaded = true) public class DefaultCalculationTaskRunnerTest
	public class DefaultCalculationTaskRunnerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly TestTarget TARGET = new TestTarget();
	  private static readonly LocalDate VAL_DATE = date(2011, 3, 8);
	  private static readonly ISet<Measure> MEASURES = ImmutableSet.of(TestingMeasures.PRESENT_VALUE);

	  //-------------------------------------------------------------------------
	  // Test that ScenarioArrays containing a single value are unwrapped.
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void unwrapScenarioResults() throws Exception
	  public virtual void unwrapScenarioResults()
	  {
		ScenarioArray<string> scenarioResult = ScenarioArray.of("foo");
		ScenarioResultFunction fn = new ScenarioResultFunction(TestingMeasures.PRESENT_VALUE, scenarioResult);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		Column column = Column.of(TestingMeasures.PRESENT_VALUE);
		CalculationTasks tasks = CalculationTasks.of(ImmutableList.of(task), ImmutableList.of(column));

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTaskRunner test = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());

		MarketData marketData = MarketData.empty(VAL_DATE);
		Results results1 = test.calculate(tasks, marketData, REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result1 = results1.get(0, 0);
		Result<object> result1 = results1.get(0, 0);
		// Check the result contains the string directly, not the result wrapping the string
		assertThat(result1).hasValue("foo");

		Results results2 = test.calculateMultiScenario(tasks, ScenarioMarketData.of(1, marketData), REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result2 = results2.get(0, 0);
		Result<object> result2 = results2.get(0, 0);
		// Check the result contains the scenario result wrapping the string
		assertThat(result2).hasValue(scenarioResult);

		ResultsListener resultsListener = new ResultsListener();
		test.calculateAsync(tasks, marketData, REF_DATA, resultsListener);
		CompletableFuture<Results> future = resultsListener.Future;
		// The future is guaranteed to be done because everything is running on a single thread
		assertThat(future.Done).True;
		Results results3 = future.get();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result3 = results3.get(0, 0);
		Result<object> result3 = results3.get(0, 0);
		// Check the result contains the string directly, not the result wrapping the string
		assertThat(result3).hasValue("foo");
	  }

	  /// <summary>
	  /// Test that ScenarioArrays containing multiple values are an error.
	  /// </summary>
	  public virtual void unwrapMultipleScenarioResults()
	  {
		ScenarioArray<string> scenarioResult = ScenarioArray.of("foo", "bar");
		ScenarioResultFunction fn = new ScenarioResultFunction(TestingMeasures.PAR_RATE, scenarioResult);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PAR_RATE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		Column column = Column.of(TestingMeasures.PAR_RATE);
		CalculationTasks tasks = CalculationTasks.of(ImmutableList.of(task), ImmutableList.of(column));

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTaskRunner test = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());

		MarketData marketData = MarketData.empty(VAL_DATE);
		assertThrowsIllegalArg(() => test.calculate(tasks, marketData, REF_DATA));
	  }

	  /// <summary>
	  /// Test that ScenarioArrays containing a single value are unwrapped when calling calculateAsync().
	  /// </summary>
	  public virtual void unwrapScenarioResultsAsync()
	  {
		ScenarioArray<string> scenarioResult = ScenarioArray.of("foo");
		ScenarioResultFunction fn = new ScenarioResultFunction(TestingMeasures.PRESENT_VALUE, scenarioResult);
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		Column column = Column.of(TestingMeasures.PRESENT_VALUE);
		CalculationTasks tasks = CalculationTasks.of(ImmutableList.of(task), ImmutableList.of(column));

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTaskRunner test = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());
		Listener listener = new Listener();

		MarketData marketData = MarketData.empty(VAL_DATE);
		test.calculateAsync(tasks, marketData, REF_DATA, listener);
		CalculationResult calculationResult1 = listener.result;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result1 = calculationResult1.getResult();
		Result<object> result1 = calculationResult1.Result;
		// Check the result contains the string directly, not the result wrapping the string
		assertThat(result1).hasValue("foo");

		test.calculateMultiScenarioAsync(tasks, ScenarioMarketData.of(1, marketData), REF_DATA, listener);
		CalculationResult calculationResult2 = listener.result;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result2 = calculationResult2.getResult();
		Result<object> result2 = calculationResult2.Result;
		// Check the result contains the scenario result wrapping the string
		assertThat(result2).hasValue(scenarioResult);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Tests that running an empty list of tasks completes and returns a set of results with zero rows.
	  /// </summary>
	  public virtual void runWithNoTasks()
	  {
		Column column = Column.of(TestingMeasures.PRESENT_VALUE);
		CalculationTasks tasks = CalculationTasks.of(ImmutableList.of(), ImmutableList.of(column));

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTaskRunner test = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());

		MarketData marketData = MarketData.empty(VAL_DATE);
		Results results = test.calculate(tasks, marketData, REF_DATA);
		assertThat(results.RowCount).isEqualTo(0);
		assertThat(results.ColumnCount).isEqualTo(1);
		assertThat(results.Columns.get(0).Measure).isEqualTo(TestingMeasures.PRESENT_VALUE);
	  }

	  //-------------------------------------------------------------------------
	  private sealed class ScenarioResultFunction : CalculationFunction<TestTarget>
	  {

		internal readonly Measure measure;
		internal readonly ScenarioArray<string> result;

		internal ScenarioResultFunction(Measure measure, ScenarioArray<string> result)
		{
		  this.measure = measure;
		  this.result = result;
		}

		public Type<TestTarget> targetType()
		{
		  return typeof(TestTarget);
		}

		public ISet<Measure> supportedMeasures()
		{
		  return ImmutableSet.of(measure);
		}

		public Currency naturalCurrency(TestTarget trade, ReferenceData refData)
		{
		  return USD;
		}

		public FunctionRequirements requirements(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  return FunctionRequirements.empty();
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.calc.runner.CalculationTaskTest.TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  return ImmutableMap.of(measure, Result.success(result));
		}
	  }

	  //-------------------------------------------------------------------------
	  private sealed class Listener : CalculationListener
	  {

		internal CalculationResult result;

		public void resultReceived(CalculationTarget target, CalculationResult result)
		{
		  this.result = result;
		}

		public void calculationsComplete()
		{
		  // Do nothing
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(timeOut = 5000) public void interruptHangingCalculate() throws InterruptedException
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void interruptHangingCalculate()
	  {
		HangingFunction fn = new HangingFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		Column column = Column.of(TestingMeasures.PRESENT_VALUE);
		CalculationTasks tasks = CalculationTasks.of(ImmutableList.of(task), ImmutableList.of(column));

		// using the direct executor means there is no need to close/shutdown the runner
		CalculationTaskRunner test = CalculationTaskRunner.of(MoreExecutors.newDirectExecutorService());
		MarketData marketData = MarketData.empty(VAL_DATE);

		AtomicBoolean shouldNeverThrow = new AtomicBoolean();
		AtomicBoolean interrupted = new AtomicBoolean();
		AtomicReference<Results> results = new AtomicReference<Results>();
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		Thread thread = new Thread(() =>
		{
		try
		{
			Results result = test.calculate(tasks, marketData, REF_DATA);
			interrupted.set(Thread.CurrentThread.Interrupted);
			results.set(result);
		}
		catch (Exception)
		{
			shouldNeverThrow.set(true);
		}
		latch.Signal();
		});
		// run the thread, wait until properly started, then interrupt, wait until properly handled
		thread.Start();
		while (!fn.started)
		{
		}
		thread.Interrupt();
		latch.await();
		// asserts
		assertEquals(interrupted.get(), true);
		assertEquals(shouldNeverThrow.get(), false);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result00 = results.get().get(0, 0);
		Result<object> result00 = results.get().get(0, 0);
		assertEquals(result00.Failure, true);
		assertEquals(result00.Failure.Reason, FailureReason.CALCULATION_FAILED);
		assertEquals(result00.Failure.Message.Contains("Runtime interrupted"), true);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(timeOut = 5000) public void interruptHangingResultsListener() throws InterruptedException
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
	  public virtual void interruptHangingResultsListener()
	  {
		HangingFunction fn = new HangingFunction();
		CalculationTaskCell cell = CalculationTaskCell.of(0, 0, TestingMeasures.PRESENT_VALUE, NATURAL);
		CalculationTask task = CalculationTask.of(TARGET, fn, cell);
		Column column = Column.of(TestingMeasures.PRESENT_VALUE);
		CalculationTasks tasks = CalculationTasks.of(ImmutableList.of(task), ImmutableList.of(column));

		ExecutorService executor = Executors.newSingleThreadExecutor();
		try
		{
		  CalculationTaskRunner test = CalculationTaskRunner.of(executor);
		  MarketData marketData = MarketData.empty(VAL_DATE);

		  AtomicBoolean shouldNeverComplete = new AtomicBoolean();
		  AtomicBoolean interrupted = new AtomicBoolean();
		  AtomicReference<Exception> thrown = new AtomicReference<Exception>();
		  ResultsListener listener = new ResultsListener();
		  test.calculateAsync(tasks, marketData, REF_DATA, listener);
		  System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		  Thread thread = new Thread(() =>
		  {
		  try
		  {
			  listener.result();
			  shouldNeverComplete.set(true);
		  }
		  catch (Exception ex)
		  {
			  interrupted.set(Thread.CurrentThread.Interrupted);
			  thrown.set(ex);
		  }
		  latch.Signal();
		  });
		  // run the thread, wait until properly started, then interrupt, wait until properly handled
		  thread.Start();
		  while (!fn.started)
		  {
		  }
		  thread.Interrupt();
		  latch.await();
		  // asserts
		  assertEquals(interrupted.get(), true);
		  assertEquals(shouldNeverComplete.get(), false);
		  assertEquals(thrown.get() is Exception, true);
		  assertEquals(thrown.get().Cause is InterruptedException, true);
		}
		finally
		{
		  executor.shutdownNow();
		}
	  }

	  //-------------------------------------------------------------------------
	  public sealed class HangingFunction : CalculationFunction<TestTarget>
	  {

		internal volatile bool started;

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

		  return FunctionRequirements.empty();
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.calc.runner.CalculationTaskTest.TestTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(TestTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  while (true)
		  {
			if (Thread.CurrentThread.Interrupted)
			{
			  throw new Exception("Runtime interrupted");
			}
			started = true;
		  }
		}
	  }

	}

}
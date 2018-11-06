/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using MoreExecutors = com.google.common.util.concurrent.MoreExecutors;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Test <seealso cref="CalculationRunner"/> and <seealso cref="DefaultCalculationRunner"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DefaultCalculationRunnerTest
	public class DefaultCalculationRunnerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly TestTarget TARGET = new TestTarget();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		using (CalculationRunner test = CalculationRunner.ofMultiThreaded())
		{
		  assertThat(test.TaskRunner).NotNull;
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void calculate()
	  {
		ImmutableList<CalculationTarget> targets = ImmutableList.of(TARGET);
		Column column1 = Column.of(TestingMeasures.PRESENT_VALUE);
		Column column2 = Column.of(TestingMeasures.BUCKETED_PV01);
		ImmutableList<Column> columns = ImmutableList.of(column1, column2);
		CalculationRules rules = CalculationRules.of(CalculationFunctions.empty());
		MarketData md = MarketData.empty(date(2016, 6, 30));
		ScenarioMarketData smd = ScenarioMarketData.empty();

		// use of try-with-resources checks class is AutoCloseable
		using (CalculationRunner test = CalculationRunner.of(MoreExecutors.newDirectExecutorService()))
		{
		  assertThat(test.calculate(rules, targets, columns, md, REF_DATA).get(0, 0).Failure).True;
		  assertThat(test.calculateMultiScenario(rules, targets, columns, smd, REF_DATA).get(0, 0).Failure).True;
		}
	  }

	  //-------------------------------------------------------------------------
	  private class TestTarget : CalculationTarget
	  {
	  }

	}

}
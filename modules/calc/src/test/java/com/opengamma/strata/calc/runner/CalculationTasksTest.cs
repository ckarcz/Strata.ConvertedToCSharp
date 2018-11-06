using System.Collections.Generic;

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
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using TestId = com.opengamma.strata.calc.marketdata.TestId;
	using TestObservableId = com.opengamma.strata.calc.marketdata.TestObservableId;
	using TestFunction = com.opengamma.strata.calc.runner.CalculationTaskTest.TestFunction;
	using TestTarget = com.opengamma.strata.calc.runner.CalculationTaskTest.TestTarget;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// Test <seealso cref="CalculationTasks"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationTasksTest
	public class CalculationTasksTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly TestTarget TARGET1 = new TestTarget();
	  private static readonly TestTarget TARGET2 = new TestTarget();
	  private static readonly CalculationFunctions CALC_FUNCTIONS = CalculationFunctions.empty();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		CalculationFunctions functions = CalculationFunctions.of(ImmutableMap.of(typeof(TestTarget), new TestFunction()));
		IList<TestTarget> targets = ImmutableList.of(TARGET1, TARGET2);
		IList<Column> columns = ImmutableList.of(Column.of(TestingMeasures.PRESENT_VALUE), Column.of(TestingMeasures.PAR_RATE));
		CalculationRules calculationRules = CalculationRules.of(functions, USD);

		CalculationTasks test = CalculationTasks.of(calculationRules, targets, columns);
		assertThat(test.Targets).hasSize(2);
		assertThat(test.Targets).containsExactly(TARGET1, TARGET2);
		assertThat(test.Columns).hasSize(2);
		assertThat(test.Columns).containsExactly(Column.of(TestingMeasures.PRESENT_VALUE), Column.of(TestingMeasures.PAR_RATE));
		assertThat(test.Tasks).hasSize(2);
		assertThat(test.Tasks[0].Target).isEqualTo(TARGET1);
		assertThat(test.Tasks[0].Cells.size()).isEqualTo(2);
		assertThat(test.Tasks[0].Cells.get(0).RowIndex).isEqualTo(0);
		assertThat(test.Tasks[0].Cells.get(0).ColumnIndex).isEqualTo(0);
		assertThat(test.Tasks[0].Cells.get(0).Measure).isEqualTo(TestingMeasures.PRESENT_VALUE);
		assertThat(test.Tasks[0].Cells.get(1).RowIndex).isEqualTo(0);
		assertThat(test.Tasks[0].Cells.get(1).ColumnIndex).isEqualTo(1);
		assertThat(test.Tasks[0].Cells.get(1).Measure).isEqualTo(TestingMeasures.PAR_RATE);

		assertThat(test.Tasks[1].Target).isEqualTo(TARGET2);
		assertThat(test.Tasks[1].Cells.size()).isEqualTo(2);
		assertThat(test.Tasks[1].Cells.get(0).RowIndex).isEqualTo(1);
		assertThat(test.Tasks[1].Cells.get(0).ColumnIndex).isEqualTo(0);
		assertThat(test.Tasks[1].Cells.get(0).Measure).isEqualTo(TestingMeasures.PRESENT_VALUE);
		assertThat(test.Tasks[1].Cells.get(1).RowIndex).isEqualTo(1);
		assertThat(test.Tasks[1].Cells.get(1).ColumnIndex).isEqualTo(1);
		assertThat(test.Tasks[1].Cells.get(1).Measure).isEqualTo(TestingMeasures.PAR_RATE);

		coverImmutableBean(test);
		assertNotNull(CalculationTasks.meta());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_requirements()
	  {
		CalculationFunctions functions = CalculationFunctions.of(ImmutableMap.of(typeof(TestTarget), new TestFunction()));
		CalculationRules calculationRules = CalculationRules.of(functions, USD);
		IList<TestTarget> targets = ImmutableList.of(TARGET1);
		IList<Column> columns = ImmutableList.of(Column.of(TestingMeasures.PRESENT_VALUE));

		CalculationTasks test = CalculationTasks.of(calculationRules, targets, columns);

		MarketDataRequirements requirements = test.requirements(REF_DATA);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> nonObservables = requirements.getNonObservables();
		ISet<MarketDataId<object>> nonObservables = requirements.NonObservables;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableSet<? extends com.opengamma.strata.data.ObservableId> observables = requirements.getObservables();
		ImmutableSet<ObservableId> observables = requirements.Observables;
		ImmutableSet<ObservableId> timeSeries = requirements.TimeSeries;

		assertThat(nonObservables).hasSize(1);
		assertThat(nonObservables.GetEnumerator().next()).isEqualTo(TestId.of("1"));

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> observableId = com.opengamma.strata.calc.marketdata.TestObservableId.of("2", CalculationTaskTest.OBS_SOURCE);
		MarketDataId<object> observableId = TestObservableId.of("2", CalculationTaskTest.OBS_SOURCE);
		assertThat(observables).hasSize(1);
		assertThat(observables.GetEnumerator().next()).isEqualTo(observableId);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<?> timeSeriesId = com.opengamma.strata.calc.marketdata.TestObservableId.of("3", CalculationTaskTest.OBS_SOURCE);
		MarketDataId<object> timeSeriesId = TestObservableId.of("3", CalculationTaskTest.OBS_SOURCE);
		assertThat(timeSeries).hasSize(1);
		assertThat(timeSeries.GetEnumerator().next()).isEqualTo(timeSeriesId);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void testToString()
	  {
		IList<TestTarget> targets = ImmutableList.of(TARGET1, TARGET1);
		IList<Column> columns = ImmutableList.of(Column.of(TestingMeasures.PRESENT_VALUE), Column.of(TestingMeasures.PRESENT_VALUE), Column.of(TestingMeasures.PRESENT_VALUE));
		CalculationRules rules = CalculationRules.of(CALC_FUNCTIONS, USD);
		CalculationTasks task = CalculationTasks.of(rules, targets, columns);
		assertThat(task.ToString()).isEqualTo("CalculationTasks[grid=2x3]");
	  }

	}

}
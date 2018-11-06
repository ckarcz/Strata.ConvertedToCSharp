using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScenarioMarketDataBoxTest
	public class ScenarioMarketDataBoxTest
	{

	  public virtual void isSingleOrScenarioValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		assertThat(box.SingleValue).False;
		assertThat(box.ScenarioValue).True;
	  }

	  public virtual void getSingleValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		assertThrows(box.getSingleValue, typeof(System.InvalidOperationException), "This box does not contain a single value");
	  }

	  public virtual void getValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		assertThat(box.getValue(0)).isEqualTo(27);
		assertThat(box.getValue(1)).isEqualTo(28);
		assertThat(box.getValue(2)).isEqualTo(29);
		assertThrows(() => box.getValue(-1), typeof(System.ArgumentException), "Expected 0 <= 'scenarioIndex' < 3, but found -1");
		assertThrows(() => box.getValue(3), typeof(System.ArgumentException), "Expected 0 <= 'scenarioIndex' < 3, but found 3");
	  }

	  public virtual void getScenarioValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		ScenarioArray<int> scenarioValue = box.ScenarioValue;
		assertThat(scenarioValue.ScenarioCount).isEqualTo(3);
		assertThat(scenarioValue.get(0)).isEqualTo(27);
		assertThat(scenarioValue.get(1)).isEqualTo(28);
		assertThat(scenarioValue.get(2)).isEqualTo(29);
	  }

	  public virtual void getScenarioCount()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		assertThat(box.ScenarioCount).isEqualTo(3);
	  }

	  public virtual void map()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		MarketDataBox<int> result = box.map(v => v * 2);
		assertThat(result).isEqualTo(MarketDataBox.ofScenarioValues(54, 56, 58));
	  }

	  /// <summary>
	  /// Tests that applying a function multiple times to the value creates a box of scenario values.
	  /// </summary>
	  public virtual void mapWithIndex()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		MarketDataBox<int> scenarioBox = box.mapWithIndex(3, (v, idx) => v + idx);
		assertThat(scenarioBox.ScenarioValue).True;
		assertThat(scenarioBox.ScenarioCount).isEqualTo(3);
		assertThat(scenarioBox.getValue(0)).isEqualTo(27);
		assertThat(scenarioBox.getValue(1)).isEqualTo(29);
		assertThat(scenarioBox.getValue(2)).isEqualTo(31);
	  }

	  /// <summary>
	  /// Tests that an exception is thrown when trying to apply a function multiple times with a scenario count
	  /// that doesn't match the scenario count of the box.
	  /// </summary>
	  public virtual void mapWithIndexWrongNumberOfScenarios()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		assertThrows(() => box.mapWithIndex(4, (v, idx) => v + idx), typeof(System.ArgumentException));
	  }

	  public virtual void combineWithSingleBox()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		MarketDataBox<int> otherBox = MarketDataBox.ofSingleValue(15);
		MarketDataBox<int> resultBox = box.combineWith(otherBox, (v1, v2) => v1 + v2);
		assertThat(resultBox.ScenarioValue).True;
		assertThat(resultBox.ScenarioCount).isEqualTo(3);
		assertThat(resultBox.getValue(0)).isEqualTo(42);
		assertThat(resultBox.getValue(1)).isEqualTo(43);
		assertThat(resultBox.getValue(2)).isEqualTo(44);
	  }

	  public virtual void combineWithScenarioBox()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		MarketDataBox<int> otherBox = MarketDataBox.ofScenarioValues(15, 16, 17);
		MarketDataBox<int> resultBox = box.combineWith(otherBox, (v1, v2) => v1 + v2);
		assertThat(resultBox.ScenarioValue).True;
		assertThat(resultBox.ScenarioCount).isEqualTo(3);
		assertThat(resultBox.getValue(0)).isEqualTo(42);
		assertThat(resultBox.getValue(1)).isEqualTo(44);
		assertThat(resultBox.getValue(2)).isEqualTo(46);
	  }

	  public virtual void combineWithScenarioBoxWithWrongNumberOfScenarios()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		MarketDataBox<int> otherBox = MarketDataBox.ofScenarioValues(15, 16, 17, 18);
		assertThrows(() => box.combineWith(otherBox, (v1, v2) => v1 + v2), typeof(System.ArgumentException), "Scenario values must have the same number of scenarios.*");
	  }

	  public virtual void getMarketDataType()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		assertThat(box.MarketDataType).isEqualTo(typeof(Integer));
	  }

	  public virtual void stream()
	  {
		MarketDataBox<int> box = MarketDataBox.ofScenarioValues(27, 28, 29);
		IList<int> list = box.ToList();
		assertThat(list).isEqualTo(ImmutableList.of(27, 28, 29));
	  }
	}

}
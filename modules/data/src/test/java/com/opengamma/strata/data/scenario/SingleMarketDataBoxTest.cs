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
//ORIGINAL LINE: @Test public class SingleMarketDataBoxTest
	public class SingleMarketDataBoxTest
	{

	  public virtual void isSingleOrScenarioValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		assertThat(box.SingleValue).True;
		assertThat(box.ScenarioValue).False;
	  }

	  public virtual void getSingleValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		assertThat(box.SingleValue).isEqualTo(27);
	  }

	  /// <summary>
	  /// Test that the box always returns the same value for any non-negative scenario index.
	  /// </summary>
	  public virtual void getValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		assertThat(box.getValue(0)).isEqualTo(27);
		assertThat(box.getValue(int.MaxValue)).isEqualTo(27);
		assertThrows(() => box.getValue(-1), typeof(System.ArgumentException));
	  }

	  public virtual void getScenarioValue()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		assertThrows(box.getScenarioValue, typeof(System.InvalidOperationException), "This box does not contain a scenario value");
	  }

	  public virtual void getScenarioCount()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		assertThat(box.ScenarioCount).isEqualTo(-1);
	  }

	  public virtual void map()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		MarketDataBox<int> result = box.map(v => v * 2);
		assertThat(result).isEqualTo(MarketDataBox.ofSingleValue(54));
	  }

	  /// <summary>
	  /// Tests that applying a function multiple times to the value creates a box of scenario values.
	  /// </summary>
	  public virtual void mapWithIndex()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		MarketDataBox<int> scenarioBox = box.mapWithIndex(3, (v, idx) => v + idx);
		assertThat(scenarioBox.ScenarioValue).True;
		assertThat(scenarioBox.ScenarioCount).isEqualTo(3);
		assertThat(scenarioBox.getValue(0)).isEqualTo(27);
		assertThat(scenarioBox.getValue(1)).isEqualTo(28);
		assertThat(scenarioBox.getValue(2)).isEqualTo(29);
	  }

	  public virtual void combineWithSingleBox()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		MarketDataBox<int> otherBox = MarketDataBox.ofSingleValue(15);
		MarketDataBox<int> resultBox = box.combineWith(otherBox, (v1, v2) => v1 + v2);
		assertThat(resultBox.SingleValue).True;
		assertThat(resultBox.getValue(0)).isEqualTo(42);
	  }

	  public virtual void combineWithScenarioBox()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		MarketDataBox<int> otherBox = MarketDataBox.ofScenarioValues(15, 16, 17);
		MarketDataBox<int> resultBox = box.combineWith(otherBox, (v1, v2) => v1 + v2);
		assertThat(resultBox.ScenarioValue).True;
		assertThat(resultBox.ScenarioCount).isEqualTo(3);
		assertThat(resultBox.getValue(0)).isEqualTo(42);
		assertThat(resultBox.getValue(1)).isEqualTo(43);
		assertThat(resultBox.getValue(2)).isEqualTo(44);
	  }

	  public virtual void getMarketDataType()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		assertThat(box.MarketDataType).isEqualTo(typeof(Integer));
	  }

	  public virtual void stream()
	  {
		MarketDataBox<int> box = MarketDataBox.ofSingleValue(27);
		IList<int> list = box.ToList();
		assertThat(list).isEqualTo(ImmutableList.of(27));
	  }
	}

}
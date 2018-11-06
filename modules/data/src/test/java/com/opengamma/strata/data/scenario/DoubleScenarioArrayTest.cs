using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="DoubleScenarioArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoubleScenarioArrayTest
	public class DoubleScenarioArrayTest
	{

	  public virtual void create()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		DoubleScenarioArray test = DoubleScenarioArray.of(values);
		assertThat(test.Values).isEqualTo(values);
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(1d);
		assertThat(test.get(1)).isEqualTo(2d);
		assertThat(test.get(2)).isEqualTo(3);
		assertThat(test.ToList()).containsExactly(1d, 2d, 3d);
	  }

	  public virtual void create_fromList()
	  {
		IList<double> values = ImmutableList.of(1d, 2d, 3d);
		DoubleScenarioArray test = DoubleScenarioArray.of(values);
		assertThat(test.Values).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(1d);
		assertThat(test.get(1)).isEqualTo(2d);
		assertThat(test.get(2)).isEqualTo(3);
		assertThat(test.ToList()).containsExactly(1d, 2d, 3d);
	  }

	  public virtual void create_fromFunction()
	  {
		IList<double> values = ImmutableList.of(1d, 2d, 3d);
		DoubleScenarioArray test = DoubleScenarioArray.of(3, i => values[i]);
		assertThat(test.Values).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(1d);
		assertThat(test.get(1)).isEqualTo(2d);
		assertThat(test.get(2)).isEqualTo(3);
		assertThat(test.ToList()).containsExactly(1d, 2d, 3d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		DoubleScenarioArray test = DoubleScenarioArray.of(values);
		coverImmutableBean(test);
		DoubleArray values2 = DoubleArray.of(1, 2, 3);
		DoubleScenarioArray test2 = DoubleScenarioArray.of(values2);
		coverBeanEquals(test, test2);
	  }

	}

}
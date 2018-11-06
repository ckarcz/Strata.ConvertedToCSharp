using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="ScenarioArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScenarioArrayTest
	public class ScenarioArrayTest
	{

	  public virtual void test_of_array()
	  {
		ScenarioArray<string> test = ScenarioArray.of("1", "2", "3");
		DefaultScenarioArray<string> expected = DefaultScenarioArray.of("1", "2", "3");
		assertEquals(test, expected);
	  }

	  public virtual void test_of_list()
	  {
		ScenarioArray<string> test = ScenarioArray.of(ImmutableList.of("1", "2", "3"));
		DefaultScenarioArray<string> expected = DefaultScenarioArray.of("1", "2", "3");
		assertEquals(test, expected);
	  }

	  public virtual void test_of_function()
	  {
		ScenarioArray<string> test = ScenarioArray.of(3, i => Convert.ToString(i + 1));
		DefaultScenarioArray<string> expected = DefaultScenarioArray.of("1", "2", "3");
		assertEquals(test, expected);
	  }

	  public virtual void test_ofSingleValue()
	  {
		ScenarioArray<string> test = ScenarioArray.ofSingleValue(3, "aaa");
		SingleScenarioArray<string> expected = SingleScenarioArray.of(3, "aaa");
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_stream()
	  {
		ScenarioArray<string> test = new ScenarioArrayAnonymousInnerClass(this);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<string> output = test.collect(toImmutableList());
		assertEquals(output, ImmutableList.of("0", "1", "2"));
	  }

	  private class ScenarioArrayAnonymousInnerClass : ScenarioArray<string>
	  {
		  private readonly ScenarioArrayTest outerInstance;

		  public ScenarioArrayAnonymousInnerClass(ScenarioArrayTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


		  public int ScenarioCount
		  {
			  get
			  {
				return 3;
			  }
		  }

		  public string get(int scenarioIndex)
		  {
			return "" + scenarioIndex;
		  }

	  }

	}

}
using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioPerturbation = com.opengamma.strata.data.scenario.ScenarioPerturbation;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScenarioDefinitionTest
	public class ScenarioDefinitionTest
	{

	  private static readonly TestFilter FILTER_A = new TestFilter("a");
	  private static readonly TestFilter FILTER_B = new TestFilter("b");
	  private static readonly TestFilter FILTER_C = new TestFilter("c");
	  private static readonly TestPerturbation PERTURBATION_A1 = new TestPerturbation(1, 2);
	  private static readonly TestPerturbation PERTURBATION_B1 = new TestPerturbation(3, 4);
	  private static readonly TestPerturbation PERTURBATION_C1 = new TestPerturbation(5, 6);

	  private static readonly PerturbationMapping<object> MAPPING_A = PerturbationMapping.of(FILTER_A, PERTURBATION_A1);

	  private static readonly PerturbationMapping<object> MAPPING_B = PerturbationMapping.of(FILTER_B, PERTURBATION_B1);

	  private static readonly PerturbationMapping<object> MAPPING_C = PerturbationMapping.of(FILTER_C, PERTURBATION_C1);

	  public virtual void ofMappings()
	  {
		IList<PerturbationMapping<object>> mappings = ImmutableList.of(MAPPING_A, MAPPING_B, MAPPING_C);
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(mappings);
		IList<string> scenarioNames = ImmutableList.of("Scenario 1", "Scenario 2");
		assertThat(scenarioDefinition.Mappings).isEqualTo(mappings);
		assertThat(scenarioDefinition.ScenarioNames).isEqualTo(scenarioNames);
	  }

	  public virtual void ofMappingsWithNames()
	  {
		IList<PerturbationMapping<object>> mappings = ImmutableList.of(MAPPING_A, MAPPING_B, MAPPING_C);
		IList<string> scenarioNames = ImmutableList.of("foo", "bar");
		ScenarioDefinition scenarioDefinition = ScenarioDefinition.ofMappings(mappings, scenarioNames);
		assertThat(scenarioDefinition.Mappings).isEqualTo(mappings);
		assertThat(scenarioDefinition.ScenarioNames).isEqualTo(scenarioNames);
	  }

	  /// <summary>
	  /// Tests that a scenario definition won't be built if the scenarios names are specified and there
	  /// are the wrong number. The mappings all have 2 perturbations which should mean 2 scenarios, but
	  /// there are 3 scenario names.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void ofMappingsWrongNumberOfScenarioNames()
	  public virtual void ofMappingsWrongNumberOfScenarioNames()
	  {
		IList<PerturbationMapping<object>> mappings = ImmutableList.of(MAPPING_A, MAPPING_B, MAPPING_C);
		IList<string> scenarioNames = ImmutableList.of("foo", "bar", "baz");
		ScenarioDefinition.ofMappings(mappings, scenarioNames);
	  }

	  /// <summary>
	  /// Tests that a scenario definition won't be built if the mappings don't have the same number of scenarios
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void ofMappingsDifferentNumberOfScenarios()
	  public virtual void ofMappingsDifferentNumberOfScenarios()
	  {
		PerturbationMapping<object> mappingC = PerturbationMapping.of(FILTER_C, new TestPerturbation(27));
		IList<PerturbationMapping<object>> mappings = ImmutableList.of(MAPPING_A, MAPPING_B, mappingC);
		ScenarioDefinition.ofMappings(mappings);
	  }

	  /// <summary>
	  /// Tests that a scenario definition won't be built if the mappings don't have the same number of scenarios
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void ofMappingsWithNamesDifferentNumberOfScenarios()
	  public virtual void ofMappingsWithNamesDifferentNumberOfScenarios()
	  {
		PerturbationMapping<object> mappingC = PerturbationMapping.of(FILTER_C, new TestPerturbation(27));
		IList<PerturbationMapping<object>> mappings = ImmutableList.of(MAPPING_A, MAPPING_B, mappingC);
		IList<string> scenarioNames = ImmutableList.of("foo", "bar");
		ScenarioDefinition.ofMappings(mappings, scenarioNames);
	  }

	  public virtual void repeatItems()
	  {
		IList<int> inputs = ImmutableList.of(1, 2, 3, 4);

		IList<int> expected1 = ImmutableList.of(1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4);
		assertThat(ScenarioDefinition.repeatItems(inputs, 12, 1)).isEqualTo(expected1);

		IList<int> expected2 = ImmutableList.of(1, 1, 2, 2, 3, 3, 4, 4);
		assertThat(ScenarioDefinition.repeatItems(inputs, 8, 2)).isEqualTo(expected2);

		IList<int> expected3 = ImmutableList.of(1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4);
		assertThat(ScenarioDefinition.repeatItems(inputs, 12, 3)).isEqualTo(expected3);

		IList<int> expected4 = ImmutableList.of(1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4);
		assertThat(ScenarioDefinition.repeatItems(inputs, 24, 3)).isEqualTo(expected4);
	  }

	  /// <summary>
	  /// Tests that exceptions are thrown when the scenario names contain duplicate values.
	  /// </summary>
	  public virtual void nonUniqueNames()
	  {
		IList<PerturbationMapping<object>> mappings2 = ImmutableList.of(MAPPING_A, MAPPING_B, MAPPING_C);
		IList<string> names2 = ImmutableList.of("foo", "foo");
		string msg2 = "Scenario names must be unique but duplicates were found: foo";
		assertThrows(() => ScenarioDefinition.ofMappings(mappings2, names2), typeof(System.ArgumentException), msg2);
	  }

	  //-------------------------------------------------------------------------
	  private sealed class TestPerturbation : ScenarioPerturbation<object>
	  {

		internal readonly int[] values;

		internal TestPerturbation(params int[] values)
		{
		  this.values = values;
		}

		public MarketDataBox<object> applyTo(MarketDataBox<object> marketData, ReferenceData refData)
		{
		  return marketData;
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestPerturbation that = (TestPerturbation) o;
		  return Arrays.Equals(values, that.values);
		}

		public int ScenarioCount
		{
			get
			{
			  return values.Length;
			}
		}

		public Type<object> MarketDataType
		{
			get
			{
			  return typeof(object);
			}
		}

		public override int GetHashCode()
		{
		  return Objects.hash(new object[] {values});
		}

		public override string ToString()
		{
		  return "TestPerturbation [id=" + Arrays.ToString(values) + "]";
		}
	  }

	  private sealed class TestFilter : MarketDataFilter<object, MarketDataId<object>>
	  {

		internal readonly string name;

		internal TestFilter(string name)
		{
		  this.name = name;
		}

		public bool matches(MarketDataId<object> marketDataId, MarketDataBox<object> marketData, ReferenceData refData)
		{
		  return false;
		}

		public Type MarketDataIdType
		{
			get
			{
			  return typeof(MarketDataId);
			}
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestFilter that = (TestFilter) o;
		  return Objects.Equals(name, that.name);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(name);
		}

		public override string ToString()
		{
		  return "TestFilter [name='" + name + "']";
		}
	  }
	}

}
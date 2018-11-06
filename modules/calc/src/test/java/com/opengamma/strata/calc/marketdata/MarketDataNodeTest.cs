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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using FieldName = com.opengamma.strata.data.FieldName;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MarketDataNodeTest
	public class MarketDataNodeTest
	{

	  /// <summary>
	  /// Tests removing the leaf dependencies from the tree to decide which market data can be built.
	  /// </summary>
	  public virtual void withLeavesRemoved()
	  {
		MarketDataNode root = rootNode(observableNode(new TestIdA(this, "1")), valueNode(new TestIdB(this, "2"), valueNode(new TestIdB(this, "3")), observableNode(new TestIdA(this, "4")), valueNode(new TestIdB(this, "5"), timeSeriesNode(new TestIdA(this, "6")))), valueNode(new TestIdB(this, "7")));

		Pair<MarketDataNode, MarketDataRequirements> pair1 = root.withLeavesRemoved();

		MarketDataRequirements expectedReqs1 = MarketDataRequirements.builder().addValues(new TestIdA(this, "1")).addValues(new TestIdB(this, "3")).addValues(new TestIdA(this, "4")).addTimeSeries(new TestIdA(this, "6")).addValues(new TestIdB(this, "7")).build();

		MarketDataNode expectedTree1 = rootNode(valueNode(new TestIdB(this, "2"), valueNode(new TestIdB(this, "5"))));

		MarketDataNode tree1 = pair1.First;
		MarketDataRequirements reqs1 = pair1.Second;

		assertThat(tree1).isEqualTo(expectedTree1);
		assertThat(expectedReqs1).isEqualTo(reqs1);

		Pair<MarketDataNode, MarketDataRequirements> pair2 = tree1.withLeavesRemoved();

		MarketDataRequirements expectedReqs2 = MarketDataRequirements.builder().addValues(new TestIdB(this, "5")).build();

		MarketDataNode expectedTree2 = rootNode(valueNode(new TestIdB(this, "2")));

		MarketDataNode tree2 = pair2.First;
		MarketDataRequirements reqs2 = pair2.Second;

		assertThat(tree2).isEqualTo(expectedTree2);
		assertThat(expectedReqs2).isEqualTo(reqs2);

		Pair<MarketDataNode, MarketDataRequirements> pair3 = tree2.withLeavesRemoved();

		MarketDataRequirements expectedReqs3 = MarketDataRequirements.builder().addValues(new TestIdB(this, "2")).build();

		MarketDataNode tree3 = pair3.First;
		MarketDataRequirements reqs3 = pair3.Second;

		assertThat(tree3.Leaf).True;
		assertThat(expectedReqs3).isEqualTo(reqs3);
	  }

	  /// <summary>
	  /// Tests building a tree of requirements using market data functions.
	  /// </summary>
	  public virtual void buildDependencyTree()
	  {
		MarketDataNode expected = rootNode(observableNode(new TestIdA(this, "1")), valueNode(new TestIdB(this, "2"), valueNode(new TestIdB(this, "4"), observableNode(new TestIdA(this, "5"))), timeSeriesNode(new TestIdA(this, "3"))), timeSeriesNode(new TestIdA(this, "6")));

		// The requirements for the data directly used by the calculations
		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(new TestIdA(this, "1"), new TestIdB(this, "2")).addTimeSeries(new TestIdA(this, "6")).build();

		// Requirements for each item in the tree - used to initialize the functions
		MarketDataRequirements id2Reqs = MarketDataRequirements.builder().addTimeSeries(new TestIdA(this, "3")).addValues(new TestIdB(this, "4")).build();

		MarketDataRequirements id4Reqs = MarketDataRequirements.builder().addValues(new TestIdA(this, "5")).build();

		ImmutableMap<TestIdB, MarketDataRequirements> reqsMap = ImmutableMap.of(new TestIdB(this, "2"), id2Reqs, new TestIdB(this, "4"), id4Reqs);

		TestMarketDataFunctionA builderA = new TestMarketDataFunctionA();
		TestMarketDataFunctionB builderB = new TestMarketDataFunctionB(reqsMap);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableMap<Class, MarketDataFunction<?, ?>> functions = com.google.common.collect.ImmutableMap.of(TestIdA.class, builderA, TestIdB.class, builderB);
		ImmutableMap<Type, MarketDataFunction<object, ?>> functions = ImmutableMap.of(typeof(TestIdA), builderA, typeof(TestIdB), builderB);

		MarketDataNode root = MarketDataNode.buildDependencyTree(requirements, BuiltScenarioMarketData.empty(), MarketDataConfig.empty(), functions);

		assertThat(root).isEqualTo(expected);
	  }

	  /// <summary>
	  /// Tests that supplied data is in a leaf node and the functions aren't asked for dependencies for supplied data.
	  /// </summary>
	  public virtual void noDependenciesForSuppliedData()
	  {
		MarketDataNode expected1 = rootNode(valueNode(new TestIdB(this, "1"), observableNode(new TestIdA(this, "2"))), valueNode(new TestIdB(this, "3"), valueNode(new TestIdB(this, "4"))));

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(new TestIdB(this, "1"), new TestIdB(this, "3")).build();

		MarketDataRequirements id1Reqs = MarketDataRequirements.builder().addValues(new TestIdA(this, "2")).build();

		MarketDataRequirements id3Reqs = MarketDataRequirements.builder().addValues(new TestIdB(this, "4")).build();

		ImmutableMap<TestIdB, MarketDataRequirements> reqsMap = ImmutableMap.of(new TestIdB(this, "1"), id1Reqs, new TestIdB(this, "3"), id3Reqs);

		TestMarketDataFunctionB builder = new TestMarketDataFunctionB(reqsMap);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableMap<Class, MarketDataFunction<?, ?>> functions = com.google.common.collect.ImmutableMap.of(TestIdB.class, builder);
		ImmutableMap<Type, MarketDataFunction<object, ?>> functions = ImmutableMap.of(typeof(TestIdB), builder);

		MarketDataNode root1 = MarketDataNode.buildDependencyTree(requirements, BuiltScenarioMarketData.empty(), MarketDataConfig.empty(), functions);

		assertThat(root1).isEqualTo(expected1);

		BuiltScenarioMarketData suppliedData = BuiltScenarioMarketData.builder(date(2011, 3, 8)).addValue(new TestIdB(this, "1"), new TestMarketDataB()).addValue(new TestIdB(this, "3"), new TestMarketDataB()).build();

		MarketDataNode root2 = MarketDataNode.buildDependencyTree(requirements, suppliedData, MarketDataConfig.empty(), functions);

		MarketDataNode expected2 = rootNode(valueNode(new TestIdB(this, "1")), valueNode(new TestIdB(this, "3")));

		assertThat(root2).isEqualTo(expected2);
	  }

	  /// <summary>
	  /// Test a node with no children is added when there is no market data function for an ID.
	  /// </summary>
	  public virtual void noMarketDataBuilder()
	  {
		MarketDataNode expected = rootNode(valueNode(new TestIdC("1")), valueNode(new TestIdB(this, "2"), valueNode(new TestIdC("3"))));

		MarketDataRequirements requirements = MarketDataRequirements.builder().addValues(new TestIdC("1"), new TestIdB(this, "2")).build();

		MarketDataRequirements id2Reqs = MarketDataRequirements.builder().addValues(new TestIdC("3")).build();

		TestMarketDataFunctionB builder = new TestMarketDataFunctionB(ImmutableMap.of(new TestIdB(this, "2"), id2Reqs));
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableMap<Class, MarketDataFunction<?, ?>> functions = com.google.common.collect.ImmutableMap.of(TestIdB.class, builder);
		ImmutableMap<Type, MarketDataFunction<object, ?>> functions = ImmutableMap.of(typeof(TestIdB), builder);
		// Build the tree without providing a market data function to handle TestId3
		MarketDataNode root = MarketDataNode.buildDependencyTree(requirements, BuiltScenarioMarketData.empty(), MarketDataConfig.empty(), functions);

		assertThat(root).isEqualTo(expected);
	  }

	  //-------------------------------------------------------------------------
	  private static MarketDataNode rootNode(params MarketDataNode[] children)
	  {
		return MarketDataNode.root(Arrays.asList(children));
	  }

	  private static MarketDataNode valueNode<T1>(MarketDataId<T1> id, params MarketDataNode[] children)
	  {
		return MarketDataNode.child(id, MarketDataNode.DataType.SINGLE_VALUE, Arrays.asList(children));
	  }

	  private static MarketDataNode observableNode(ObservableId id)
	  {
		return MarketDataNode.leaf(id, MarketDataNode.DataType.SINGLE_VALUE);
	  }

	  private static MarketDataNode timeSeriesNode(ObservableId id)
	  {
		return MarketDataNode.leaf(id, MarketDataNode.DataType.TIME_SERIES);
	  }

	  internal class TestIdA : ObservableId
	  {
		  private readonly MarketDataNodeTest outerInstance;


		internal readonly StandardId id;

		internal TestIdA(MarketDataNodeTest outerInstance, string id)
		{
			this.outerInstance = outerInstance;
		  this.id = StandardId.of("test", id);
		}

		public virtual StandardId StandardId
		{
			get
			{
			  return id;
			}
		}

		public virtual FieldName FieldName
		{
			get
			{
			  return FieldName.MARKET_VALUE;
			}
		}

		public virtual ObservableSource ObservableSource
		{
			get
			{
			  return ObservableSource.NONE;
			}
		}

		public virtual ObservableId withObservableSource(ObservableSource obsSource)
		{
		  return this;
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
		  TestIdA idA = (TestIdA) o;
		  return Objects.Equals(id, idA.id);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(id);
		}

		public override string ToString()
		{
		  return "TestId1 [id=" + id + "]";
		}
	  }

	  internal class TestIdB : MarketDataId<TestMarketDataB>
	  {
		  private readonly MarketDataNodeTest outerInstance;


		internal readonly string str;

		internal TestIdB(MarketDataNodeTest outerInstance, string str)
		{
			this.outerInstance = outerInstance;
		  this.str = str;
		}

		public virtual Type<TestMarketDataB> MarketDataType
		{
			get
			{
			  return typeof(TestMarketDataB);
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
		  TestIdB idB = (TestIdB) o;
		  return Objects.Equals(str, idB.str);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(str);
		}

		public override string ToString()
		{
		  return "TestId2 [str='" + str + "']";
		}
	  }

	  private sealed class TestIdC : MarketDataId<string>
	  {

		internal readonly string id;

		internal TestIdC(string id)
		{
		  this.id = id;
		}

		public Type<string> MarketDataType
		{
			get
			{
			  return typeof(string);
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
		  TestIdC idC = (TestIdC) o;
		  return Objects.Equals(id, idC.id);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(id);
		}

		public override string ToString()
		{
		  return "BazId [id='" + id + "']";
		}
	  }

	  private sealed class TestMarketDataFunctionA : MarketDataFunction<double, TestIdA>
	  {

		public MarketDataRequirements requirements(TestIdA id, MarketDataConfig marketDataConfig)
		{
		  // The ID represents observable data which has no dependencies by definition
		  return MarketDataRequirements.empty();
		}

		public MarketDataBox<double> build(TestIdA id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
		{

		  throw new System.NotSupportedException("build not implemented");
		}

		public Type<TestIdA> MarketDataIdType
		{
			get
			{
			  return typeof(TestIdA);
			}
		}
	  }

	  private sealed class TestMarketDataB
	  {
	  }

	  private sealed class TestMarketDataFunctionB : MarketDataFunction<TestMarketDataB, TestIdB>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal readonly IDictionary<TestIdB, MarketDataRequirements> requirements_Renamed;

		internal TestMarketDataFunctionB(IDictionary<TestIdB, MarketDataRequirements> requirements)
		{
		  this.requirements_Renamed = requirements;
		}

		public MarketDataRequirements requirements(TestIdB id, MarketDataConfig marketDataConfig)
		{
		  return requirements_Renamed.getOrDefault(id, MarketDataRequirements.empty());
		}

		public MarketDataBox<TestMarketDataB> build(TestIdB id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
		{

		  throw new System.NotSupportedException("build not implemented");
		}

		public Type<TestIdB> MarketDataIdType
		{
			get
			{
			  return typeof(TestIdB);
			}
		}
	  }
	}

}
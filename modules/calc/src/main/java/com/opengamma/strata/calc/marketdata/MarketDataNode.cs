using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// A node in a tree of dependencies of market data required by a set of calculations.
	/// <para>
	/// The immediate children of the root node are the market data values required by the calculations. Their child
	/// nodes are the market data they depend on, and so on.
	/// </para>
	/// <para>
	/// This tree is used to determine the order in which market data is built. The leaves of the tree
	/// represent market data with no dependencies. This includes:
	/// <ul>
	///   <li>Market data that is already available</li>
	///   <li>Observable data whose value can be obtained from a market data provider</li>
	///   <li>Market data that can be built from data that is already available</li>
	/// </ul>
	/// For example, if a function requests a curve, there will be a node below the root representing the curve.
	/// The curve's node will have a child node representing the curve group containing the curve. The curve group node
	/// has child nodes representing the market data values at each of the curve points. It might also have a child node
	/// representing another curve, or possibly an FX rate, and the curve and FX rate nodes would themselves
	/// depend on market data values.
	/// </para>
	/// </summary>
	internal sealed class MarketDataNode
	{

	  /// <summary>
	  /// The type of market data represented by the node, either a single value or a time series of values. </summary>
	  internal enum DataType
	  {

		/// <summary>
		/// The node represents a single market data value. </summary>
		SINGLE_VALUE,

		/// <summary>
		/// The node represents a time series of market data values. </summary>
		TIME_SERIES
	  }

	  /// <summary>
	  /// The ID of the required market data. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.data.MarketDataId<?> id;
	  private readonly MarketDataId<object> id;

	  /// <summary>
	  /// The type of the required market data. </summary>
	  private readonly DataType dataType;

	  /// <summary>
	  /// Child nodes identifying the market data required to build the market data in this node. </summary>
	  private readonly IList<MarketDataNode> dependencies;

	  /// <summary>
	  /// Builds a tree representing the dependencies between items of market data and returns the root node.
	  /// </summary>
	  /// <param name="requirements">  IDs of the market data that must be provided </param>
	  /// <param name="suppliedData">  data supplied by the user </param>
	  /// <param name="marketDataConfig">  configuration specifying how market data values should be built </param>
	  /// <param name="functions">  functions for market data, keyed by the type of market data ID they handle </param>
	  /// <returns> the root node of the market data dependency tree </returns>
	  internal static MarketDataNode buildDependencyTree<T1>(MarketDataRequirements requirements, ScenarioMarketData suppliedData, MarketDataConfig marketDataConfig, IDictionary<T1> functions)
	  {

		DependencyTreeBuilder treeBuilder = DependencyTreeBuilder.of(suppliedData, requirements, marketDataConfig, functions);
		return MarketDataNode.root(treeBuilder.dependencyNodes());
	  }

	  /// <summary>
	  /// Returns a root node which doesn't have a market data ID or data type.
	  /// </summary>
	  /// <param name="children">  the child nodes representing the market data dependencies of the root node </param>
	  /// <returns> a root node which doesn't have a market data ID or data type </returns>
	  internal static MarketDataNode root(IList<MarketDataNode> children)
	  {
		ArgChecker.notNull(children, "children");
		return new MarketDataNode(null, null, children);
	  }

	  /// <summary>
	  /// Returns a child node representing an item of market data.
	  /// </summary>
	  /// <param name="id">  an ID identifying the market data represented by the node </param>
	  /// <param name="dataType">  the type of market data represented by the node, either a single value or a time series of values </param>
	  /// <param name="children">  the child nodes representing the market data dependencies of the node </param>
	  /// <returns> a child node representing an item of market data </returns>
	  internal static MarketDataNode child<T1>(MarketDataId<T1> id, DataType dataType, IList<MarketDataNode> children)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(dataType, "dataType");
		ArgChecker.notNull(children, "children");
		return new MarketDataNode(id, dataType, children);
	  }

	  /// <summary>
	  /// Returns a leaf node representing an item of market data with no dependencies on other market data.
	  /// </summary>
	  /// <param name="id">  an ID identifying the market data represented by the node </param>
	  /// <param name="dataType">  the type of market data represented by the node, either a single value or a time series of values </param>
	  /// <returns> a leaf node representing an item of market data with no dependencies on other market data </returns>
	  internal static MarketDataNode leaf<T1>(MarketDataId<T1> id, DataType dataType)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(dataType, "dataType");
		return new MarketDataNode(id, dataType, ImmutableList.of());
	  }

	  private MarketDataNode<T1>(MarketDataId<T1> id, DataType dataType, IList<MarketDataNode> dependencies)
	  {
		this.dataType = dataType;
		this.id = id;
		this.dependencies = ImmutableList.copyOf(dependencies);
	  }

	  /// <summary>
	  /// Returns a copy of the dependency tree without the leaf nodes. It also returns the market data requirements
	  /// represented by the leaf nodes.
	  /// <para>
	  /// The leaf nodes represent market data with no missing requirements for market data. This includes:
	  /// <ul>
	  ///   <li>Market data that is already available</li>
	  ///   <li>Observable data whose value can be obtained from a market data provider</li>
	  ///   <li>Market data that can be built from data that is already available</li>
	  /// </ul>
	  /// Therefore the market data represented by the leaf nodes can be built immediately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a copy of the dependency tree without the leaf nodes and the market data requirements
	  ///   represented by the leaf nodes </returns>
	  internal Pair<MarketDataNode, MarketDataRequirements> withLeavesRemoved()
	  {
		ImmutableList.Builder<MarketDataNode> childNodesBuilder = ImmutableList.builder();
		MarketDataRequirementsBuilder requirementsBuilder = MarketDataRequirements.builder();

		foreach (MarketDataNode child in dependencies)
		{
		  if (child.Leaf)
		  {
			switch (child.dataType)
			{
			  case com.opengamma.strata.calc.marketdata.MarketDataNode.DataType.SINGLE_VALUE:
				requirementsBuilder.addValues(child.id);
				break;
			  case com.opengamma.strata.calc.marketdata.MarketDataNode.DataType.TIME_SERIES:
				requirementsBuilder.addTimeSeries(((ObservableId) child.id));
				break;
			}
		  }
		  else
		  {
			Pair<MarketDataNode, MarketDataRequirements> childResult = child.withLeavesRemoved();
			childNodesBuilder.add(childResult.First);
			requirementsBuilder.addRequirements(childResult.Second);
		  }
		}
		MarketDataNode node = new MarketDataNode(id, dataType, childNodesBuilder.build());
		MarketDataRequirements requirements = requirementsBuilder.build();

		return Pair.of(node, requirements);
	  }

	  /// <summary>
	  /// Returns true if this node has no children.
	  /// </summary>
	  /// <returns> true if this node has no children </returns>
	  internal bool Leaf
	  {
		  get
		  {
			return dependencies.Count == 0;
		  }
	  }

	  /// <summary>
	  /// Returns the ID of the market data value represented by this node.
	  /// </summary>
	  /// <returns> the ID of the market data value represented by this node </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.opengamma.strata.data.MarketDataId<?> getId()
	  public MarketDataId<object> Id
	  {
		  get
		  {
			return id;
		  }
	  }

	  /// <summary>
	  /// Prints this node and its tree of dependencies to an ASCII tree.
	  /// </summary>
	  /// <param name="builder">  a string builder into which the result will be written </param>
	  /// <param name="indent">  the indent printed at the start of the line before the node </param>
	  /// <param name="childIndent">  the indent printed at the start of the line before the node's children </param>
	  /// <returns> the string builder containing the pretty-printed tree </returns>
	  private StringBuilder prettyPrint(StringBuilder builder, string indent, string childIndent)
	  {
		string nodeDescription = (id == null) ? "Root" : (id + " " + dataType);
		builder.Append('\n').Append(indent).Append(nodeDescription);

		for (IEnumerator<MarketDataNode> it = dependencies.GetEnumerator(); it.MoveNext();)
		{
		  MarketDataNode child = it.Current;
		  string newIndent;
		  string newChildIndent;
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		  bool isFinalChild = !it.hasNext();

		  if (!isFinalChild)
		  {
			newIndent = childIndent + " |--"; // Unicode boxes: \u251c\u2500\u2500
			newChildIndent = childIndent + " |  "; // Unicode boxes: \u2502
		  }
		  else
		  {
			newIndent = childIndent + " `--"; // Unicode boxes: \u2514\u2500\u2500
			newChildIndent = childIndent + "    ";
		  }
		  child.prettyPrint(builder, newIndent, newChildIndent);
		}
		return builder;
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
		MarketDataNode that = (MarketDataNode) o;
		return Objects.Equals(id, that.id) && Objects.Equals(dataType, that.dataType) && Objects.Equals(dependencies, that.dependencies);
	  }

	  public override int GetHashCode()
	  {
		return Objects.hash(id, dataType, dependencies);
	  }

	  public override string ToString()
	  {
		return prettyPrint(new StringBuilder(), "", "").ToString();
	  }
	}

}
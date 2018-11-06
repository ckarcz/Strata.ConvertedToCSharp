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
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Builds a dependency tree for the items of market used in a set of calculations.
	/// <para>
	/// The root of the tree represents the calculations and the child nodes represent items of market data on which
	/// the calculations depend. Market data can depend on other market data, creating a tree structure of unlimited
	/// depth.
	/// </para>
	/// <para>
	/// Edges between nodes represent dependencies on items of market data. Leaf nodes represent market data
	/// with no unsatisfied dependencies which can be built immediately.
	/// </para>
	/// <para>
	/// See <seealso cref="MarketDataNode"/> for more detailed documentation.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= MarketDataNode </seealso>
	internal sealed class DependencyTreeBuilder
	{

	  /// <summary>
	  /// The market data supplied by the user. </summary>
	  private readonly ScenarioMarketData suppliedData;

	  /// <summary>
	  /// The functions that create items of market data. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<Class, MarketDataFunction<?, ?>> functions;
	  private readonly IDictionary<Type, MarketDataFunction<object, ?>> functions;

	  /// <summary>
	  /// The requirements for market data used in a set of calculations. </summary>
	  private readonly MarketDataRequirements requirements;

	  /// <summary>
	  /// Configuration specifying how market data values should be built. </summary>
	  private readonly MarketDataConfig marketDataConfig;

	  /// <summary>
	  /// Returns a tree builder that builds the dependency tree for the market data required by a set of calculations.
	  /// </summary>
	  /// <param name="suppliedData">  market data supplied by the user </param>
	  /// <param name="requirements">  specifies the market data required for the calculations </param>
	  /// <param name="marketDataConfig">  configuration specifying how market data values should be built </param>
	  /// <param name="functions">  functions that create items of market data </param>
	  /// <returns> a tree builder that builds the dependency tree for the market data required by a set of calculations </returns>
	  internal static DependencyTreeBuilder of<T1>(ScenarioMarketData suppliedData, MarketDataRequirements requirements, MarketDataConfig marketDataConfig, IDictionary<T1> functions)
	  {

		return new DependencyTreeBuilder(suppliedData, requirements, marketDataConfig, functions);
	  }

	  private DependencyTreeBuilder<T1>(ScenarioMarketData suppliedData, MarketDataRequirements requirements, MarketDataConfig marketDataConfig, IDictionary<T1> functions)
	  {

		this.suppliedData = suppliedData;
		this.requirements = requirements;
		this.marketDataConfig = marketDataConfig;
		this.functions = functions;
	  }

	  /// <summary>
	  /// Returns nodes representing the dependencies of the market data required for a set of calculations.
	  /// </summary>
	  /// <returns> nodes representing the dependencies of the market data required for a set of calculations </returns>
	  internal IList<MarketDataNode> dependencyNodes()
	  {
		return dependencyNodes(requirements);
	  }

	  /// <summary>
	  /// Returns nodes representing the dependencies of a set of market data.
	  /// </summary>
	  /// <param name="requirements">  requirements for market data needed for a set of calculations </param>
	  /// <returns> nodes representing the dependencies of a set of market data </returns>
	  private IList<MarketDataNode> dependencyNodes(MarketDataRequirements requirements)
	  {

		IList<MarketDataNode> observableNodes = buildNodes(requirements.Observables, MarketDataNode.DataType.SINGLE_VALUE);

		IList<MarketDataNode> nonObservableNodes = buildNodes(requirements.NonObservables, MarketDataNode.DataType.SINGLE_VALUE);

		IList<MarketDataNode> timeSeriesNodes = buildNodes(requirements.TimeSeries, MarketDataNode.DataType.TIME_SERIES);

		return ImmutableList.builder<MarketDataNode>().addAll(observableNodes).addAll(nonObservableNodes).addAll(timeSeriesNodes).build();
	  }

	  /// <summary>
	  /// Builds nodes for a set of market data IDs.
	  /// </summary>
	  /// <param name="ids">  the IDs </param>
	  /// <param name="dataType">  the type of data represented by the IDs, either single values or time series of values </param>
	  /// <returns> market data nodes for the IDs </returns>
	  private IList<MarketDataNode> buildNodes<T1>(ISet<T1> ids, MarketDataNode.DataType dataType) where T1 : com.opengamma.strata.data.MarketDataId<T1>
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return ids.Select(id => buildNode(id, dataType)).collect(toImmutableList());
	  }

	  /// <summary>
	  /// Builds a node for a market data ID.
	  /// </summary>
	  /// <param name="id">  the ID </param>
	  /// <param name="dataType">  the type of data represented by the ID, either a single value or a time series of values </param>
	  /// <returns> a market data node for the ID </returns>
	  private MarketDataNode buildNode<T1>(MarketDataId<T1> id, MarketDataNode.DataType dataType)
	  {

		// Observable data has special handling and is guaranteed to have a function.
		// Supplied data definitely has no dependencies because it already exists and doesn't need to be built.
		if (id is ObservableId || isSupplied(id, dataType, suppliedData))
		{
		  return MarketDataNode.leaf(id, dataType);
		}
		// Find the function that can build the data identified by the ID
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") MarketDataFunction function = functions.get(id.getClass());
		MarketDataFunction function = functions[id.GetType()];

		if (function != null)
		{
		  try
		  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") MarketDataRequirements requirements = function.requirements(id, marketDataConfig);
			MarketDataRequirements requirements = function.requirements(id, marketDataConfig);
			return MarketDataNode.child(id, dataType, dependencyNodes(requirements));
		  }
		  catch (Exception)
		  {
			return MarketDataNode.child(id, dataType, ImmutableList.of());
		  }
		}
		else
		{
		  // If there is no function insert a leaf node. It will be flagged as an error when the data is built
		  return MarketDataNode.leaf(id, dataType);
		}
	  }

	  /// <summary>
	  /// Returns true if the market data identified by the ID and data type is present in the supplied data.
	  /// </summary>
	  /// <param name="id">  an ID identifying market data </param>
	  /// <param name="dataType">  the data type of the market data, either a single value or a time series of values </param>
	  /// <returns> true if the market data identified by the ID and data type is present in the supplied data </returns>
	  private static bool isSupplied<T1>(MarketDataId<T1> id, MarketDataNode.DataType dataType, ScenarioMarketData suppliedData)
	  {

		switch (dataType)
		{
		  case com.opengamma.strata.calc.marketdata.MarketDataNode.DataType.TIME_SERIES:
			return (id is ObservableId) && !suppliedData.getTimeSeries((ObservableId) id).Empty;
		  case com.opengamma.strata.calc.marketdata.MarketDataNode.DataType.SINGLE_VALUE:
			return suppliedData.containsValue(id);
		  default:
			throw new System.ArgumentException("Unexpected data type " + dataType);
		}
	  }
	}

}
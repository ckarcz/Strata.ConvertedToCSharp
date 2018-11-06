using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using Result = com.opengamma.strata.collect.result.Result;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using DoubleScenarioArray = com.opengamma.strata.data.scenario.DoubleScenarioArray;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;

	/// <summary>
	/// Static utility methods useful when writing calculation functions.
	/// </summary>
	public sealed class FunctionUtils
	{

	  // Private constructor because this only contains static helper methods.
	  private FunctionUtils()
	  {
	  }

	  /// <summary>
	  /// Returns a collector which can be used at the end of a stream of results to build a <seealso cref="ScenarioArray"/>.
	  /// </summary>
	  /// @param <T> the type of the results in the stream </param>
	  /// <returns> a collector used to create a {@code CurrencyAmountList} from a stream of {@code CurrencyAmount} </returns>
	  public static Collector<T, IList<T>, ScenarioArray<T>> toScenarioArray<T>()
	  {
		// edited to compile in Eclipse
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(List<T>::new, (a, b) => a.add(b), (l, r) =>
		{
		l.addAll(r);
		return l;
		}, list => buildResult(list));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private static <T, R> com.opengamma.strata.data.scenario.ScenarioArray<T> buildResult(java.util.List<T> results)
	  private static ScenarioArray<T> buildResult<T, R>(IList<T> results)
	  {
		return ScenarioArray.of(results);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a collector that builds a multi-currency scenerio result.
	  /// <para>
	  /// This is used at the end of a stream to collect per-scenario instances of <seealso cref="MultiCurrencyAmount"/>
	  /// into a single instance of <seealso cref="MultiCurrencyScenarioArray"/>, which is designed to be space-efficient.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a collector used at the end of a stream of <seealso cref="MultiCurrencyAmount"/>
	  ///   to build a <seealso cref="MultiCurrencyScenarioArray"/> </returns>
	  public static Collector<MultiCurrencyAmount, IList<MultiCurrencyAmount>, MultiCurrencyScenarioArray> toMultiCurrencyValuesArray()
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(List<MultiCurrencyAmount>::new, (a, b) => a.add(b), (l, r) =>
		{
		l.addAll(r);
		return l;
		}, list => MultiCurrencyScenarioArray.of(list));
	  }

	  /// <summary>
	  /// Returns a collector that builds a single-currency scenerio result.
	  /// <para>
	  /// This is used at the end of a stream to collect per-scenario instances of <seealso cref="CurrencyAmount"/>
	  /// into a single instance of <seealso cref="CurrencyScenarioArray"/>, which is designed to be space-efficient.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a collector used at the end of a stream of <seealso cref="CurrencyAmount"/> to build a <seealso cref="CurrencyScenarioArray"/> </returns>
	  public static Collector<CurrencyAmount, IList<CurrencyAmount>, CurrencyScenarioArray> toCurrencyValuesArray()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(List<CurrencyAmount>::new, (a, b) => a.add(b), (l, r) =>
		{
		l.addAll(r);
		return l;
		}, list => CurrencyScenarioArray.of(list));
	  }

	  /// <summary>
	  /// Returns a collector that builds a scenario result based on {@code Double}.
	  /// <para>
	  /// This is used at the end of a stream to collect per-scenario instances of {@code Double}
	  /// into a single instance of <seealso cref="DoubleScenarioArray"/>, which is designed to be space-efficient.
	  /// </para>
	  /// <para>
	  /// Note that <seealso cref="DoubleStream"/> does not support collectors, which makes this less efficient
	  /// than it should be.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a collector used at the end of a stream of <seealso cref="Double"/> to build a <seealso cref="DoubleScenarioArray"/> </returns>
	  public static Collector<double, IList<double>, DoubleScenarioArray> toValuesArray()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(List<double>::new, (a, b) => a.add(b), (l, r) =>
		{
		l.addAll(r);
		return l;
		}, list => DoubleScenarioArray.of(list));
	  }

	  /// <summary>
	  /// Checks if a map of results contains a value for a key, and if it does inserts it into the map for a different key.
	  /// </summary>
	  /// <param name="existingKey">  a key for which the map possibly contains a value </param>
	  /// <param name="newKey">  the key which is inserted into the map </param>
	  /// <param name="mutableMeasureMap">  a mutable map of values, keyed by measure </param>
	  public static void duplicateResult<T1>(Measure existingKey, Measure newKey, IDictionary<T1> mutableMeasureMap)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = mutableMeasureMap.get(existingKey);
		Result<object> result = mutableMeasureMap[existingKey];

		if (result == null)
		{
		  return;
		}
		mutableMeasureMap[newKey] = result;
	  }

	}

}
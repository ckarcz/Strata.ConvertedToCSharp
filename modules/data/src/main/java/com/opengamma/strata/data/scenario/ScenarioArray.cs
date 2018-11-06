/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	/// <summary>
	/// An array of values, one for each scenario.
	/// <para>
	/// Implementations of this interface provide scenario-specific values.
	/// </para>
	/// <para>
	/// In the simplest case, this might be a list of values, one for each scenario.
	/// This is handled by this factory methods on this interface.
	/// </para>
	/// <para>
	/// There are two obvious reasons for creating implementations of this interface with
	/// special handling for certain types of value:
	/// <ul>
	///   <li>Reducing memory usage</li>
	///   <li>Improving performance</li>
	/// </ul>
	/// For example, if the system stores multiple copies of a curve as a list it must store the x-values with
	/// each copy of the curve. This data is mostly redundant as the x-values are the same in every scenario.
	/// A custom data type for storing scenario data for a curve can store one set of x-values shared between
	/// all scenarios, reducing memory footprint.
	/// </para>
	/// <para>
	/// When dealing with primitive data it is likely be more efficient to store the scenario values in a primitive
	/// array instead of using a list. This removes the need for boxing and reduces memory footprint.
	/// Also, if a function calculates values for all scenarios at the same time, it is likely to be more efficient
	/// if the data is stored in arrays as the values will be stored in a contiguous block of memory.
	/// </para>
	/// <para>
	/// The generic type parameter is the type of the single value associated with each scenario.
	/// For example, in the case of optimized curve storage, the single value is a curve.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of each individual value </param>
	public interface ScenarioArray<T>
	{

	  /// <summary>
	  /// Obtains an instance from the specified array of values.
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="values">  the values, one value for each scenario </param>
	  /// <returns> an instance with the specified values </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static <T> ScenarioArray<T> of(T... values)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> ScenarioArray<T> of(T... values)
	//  {
	//	return DefaultScenarioArray.of(values);
	//  }

	  /// <summary>
	  /// Obtains an instance from the specified list of values.
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="values">  the values, one value for each scenario </param>
	  /// <returns> an instance with the specified values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> ScenarioArray<T> of(java.util.List<T> values)
	//  {
	//	return DefaultScenarioArray.of(values);
	//  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the scenario index and returns the value for that index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="valueFunction">  the function used to obtain each value </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> ScenarioArray<T> of(int scenarioCount, System.Func<int, T> valueFunction)
	//  {
	//	return DefaultScenarioArray.of(scenarioCount, valueFunction);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a single value where the value applies to all scenarios.
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="scenarioCount">  the nnumber of scenarios </param>
	  /// <param name="value">  the single value, used for all scenarios </param>
	  /// <returns> an instance with the specified values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> ScenarioArray<T> ofSingleValue(int scenarioCount, T value)
	//  {
	//	return SingleScenarioArray.of(scenarioCount, value);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of scenarios.
	  /// </summary>
	  /// <returns> the number of scenarios </returns>
	  int ScenarioCount {get;}

	  /// <summary>
	  /// Gets the value at the specified scenario index.
	  /// </summary>
	  /// <param name="scenarioIndex">  the zero-based index of the scenario </param>
	  /// <returns> the value at the specified index </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  T get(int scenarioIndex);

	  /// <summary>
	  /// Returns a stream of the values.
	  /// <para>
	  /// The stream will return the value for each scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream of the values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.stream.Stream<T> stream()
	//  {
	//	return IntStream.range(0, getScenarioCount()).mapToObj(i -> get(i));
	//  }

	}

}
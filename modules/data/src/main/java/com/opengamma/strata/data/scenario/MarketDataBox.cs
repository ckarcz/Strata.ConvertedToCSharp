using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using ObjIntFunction = com.opengamma.strata.collect.function.ObjIntFunction;

	/// <summary>
	/// A box which can provide values for an item of market data used in scenarios.
	/// <para>
	/// A box can contain a single value for the data or it can contain multiple values, one for each scenario.
	/// If it contains a single value then the same value is used in every scenario.
	/// </para>
	/// <para>
	/// Wrapping the data in a box allows a simple interface for looking up market data that hides whether there
	/// is one value or multiple values. Without the box every function that uses market data would have to
	/// handle the two cases separately.
	/// </para>
	/// <para>
	/// The box also takes care of transforming the market data when using it to build other market data values
	/// (see the {@code apply} methods). This means that market data functions and perturbations don't need
	/// different logic to deal with single and multiple values.
	/// </para>
	/// <para>
	/// Using a box allows scenario data to be stored more efficiently in some cases. For example, curve data for
	/// multiple scenarios can include one copy of the x-axis data which is used in all scenarios. If a separate
	/// curve were stored for each scenario that data would be unnecessarily stored multiple times.
	/// </para>
	/// <para>
	/// In some cases a function might need to access the data for all scenarios at the same time. For example, if
	/// part of the calculation is the same for all scenarios it can be done once and reused instead of recalculated
	/// for each scenario. In this case a <seealso cref="ScenarioMarketDataId"/> should be used to retrieve the scenario
	/// value from the market data container.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of data held in the box </param>
	public interface MarketDataBox<T>
	{

	  /// <summary>
	  /// Obtains an instance containing a single market data value that is used in all scenarios.
	  /// </summary>
	  /// @param <T> the type of the market data value used in each scenario </param>
	  /// <param name="singleValue">  the market data value containing data for a single scenario </param>
	  /// <returns> a box containing a single market data value that is used in all scenarios </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataBox<T> ofSingleValue(T singleValue)
	//  {
	//	return SingleMarketDataBox.of(singleValue);
	//  }

	  /// <summary>
	  /// Obtains an instance containing a scenario market data value with data for multiple scenarios.
	  /// <para>
	  /// The market data is made up of multiple values, one for each scenario.
	  /// The <seealso cref="ScenarioArray"/> instance may provide optimized internal storage of these values.
	  /// </para>
	  /// <para>
	  /// A box may be created that contains a value for one scenario. Such a box is distinct from
	  /// a box created using <seealso cref="#ofSingleValue(Object)"/>, which is valid for any number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the market data value used in each scenario </param>
	  /// <param name="scenarioValue">  the market data value containing data for multiple scenarios </param>
	  /// <returns> a box containing a scenario market data value with data for multiple scenarios </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataBox<T> ofScenarioValue(ScenarioArray<T> scenarioValue)
	//  {
	//	return ScenarioMarketDataBox.of(scenarioValue);
	//  }

	  /// <summary>
	  /// Obtains an instance containing a scenario market data value with data for multiple scenarios.
	  /// <para>
	  /// The market data is made up of multiple values, one for each scenario.
	  /// </para>
	  /// <para>
	  /// A box may be created that contains a value for one scenario. Such a box is distinct from
	  /// a box created using <seealso cref="#ofSingleValue(Object)"/>, which is valid for any number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the market data value used in each scenario </param>
	  /// <param name="scenarioValues">  the market data values for each scenario </param>
	  /// <returns> a box containing a scenario market data value with data for multiple scenarios </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static <T> MarketDataBox<T> ofScenarioValues(T... scenarioValues)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataBox<T> ofScenarioValues(T... scenarioValues)
	//  {
	//	return ScenarioMarketDataBox.of(scenarioValues);
	//  }

	  /// <summary>
	  /// Obtains an instance containing a scenario market data value with data for multiple scenarios.
	  /// <para>
	  /// The market data is made up of multiple values, one for each scenario.
	  /// </para>
	  /// <para>
	  /// A box may be created that contains a value for one scenario. Such a box is distinct from
	  /// a box created using <seealso cref="#ofSingleValue(Object)"/>, which is valid for any number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the market data value used in each scenario </param>
	  /// <param name="scenarioValues">  the market data values for each scenario </param>
	  /// <returns> a box containing a scenario market data value with data for multiple scenarios </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataBox<T> ofScenarioValues(java.util.List<T> scenarioValues)
	//  {
	//	return ScenarioMarketDataBox.of(scenarioValues);
	//  }

	  /// <summary>
	  /// Obtains an instance containing no market data.
	  /// </summary>
	  /// @param <T> the type of the market data value used in each scenario </param>
	  /// <returns> a box containing no market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> MarketDataBox<T> empty()
	//  {
	//	return EmptyMarketDataBox.empty();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the single market data value used for all scenarios if available.
	  /// <para>
	  /// If this box contains data for multiple scenarios an exception is thrown.
	  /// </para>
	  /// <para>
	  /// This method should only be called if <seealso cref="#isSingleValue()"/> returns {@code true}
	  /// or <seealso cref="#isScenarioValue()"/> return {@code false}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the single market data value used for all scenarios if available </returns>
	  /// <exception cref="UnsupportedOperationException"> if this box contains data for multiple scenarios </exception>
	  T SingleValue {get;}

	  /// <summary>
	  /// Gets the market data value containing data for multiple scenarios.
	  /// <para>
	  /// If this box contains data for a single scenario an exception is thrown.
	  /// </para>
	  /// <para>
	  /// This method should only be called if <seealso cref="#isSingleValue()"/> returns {@code false}
	  /// or <seealso cref="#isScenarioValue()"/> return {@code true}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the market data value containing data for multiple scenarios </returns>
	  /// <exception cref="UnsupportedOperationException"> if this box contains data for a single scenario </exception>
	  ScenarioArray<T> ScenarioValue {get;}

	  /// <summary>
	  /// Gets the market data value associated with the specified scenario.
	  /// </summary>
	  /// <param name="scenarioIndex">  the index of the scenario </param>
	  /// <returns> the market data value associated with the scenario </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  T getValue(int scenarioIndex);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this box contains a single market data value that is used for all scenarios.
	  /// </summary>
	  /// <returns> true if this box contains a single market data value that is used for all scenarios </returns>
	  bool SingleValue {get;}

	  /// <summary>
	  /// Checks if this box contains market data for multiple scenarios.
	  /// </summary>
	  /// <returns> true if this box contains market data for multiple scenarios </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isScenarioValue()
	//  {
	//	return !isSingleValue();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of scenarios for which this box contains data.
	  /// <para>
	  /// A "single value" box can be used with any number of scenarios.
	  /// To indicate this, the method will return -1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of scenarios for which this box contains data, -1 if the number is not specified </returns>
	  int ScenarioCount {get;}

	  /// <summary>
	  /// Gets the type of the market data value used in each scenario.
	  /// </summary>
	  /// <returns> the type of the market data value used in each scenario </returns>
	  Type MarketDataType {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Applies a function to the contents of the box and returns another box.
	  /// <para>
	  /// The box implementation takes care of checking whether it contains a single value or a scenario value,
	  /// applying the function to the value for each scenario and packing the return value into a box.
	  /// </para>
	  /// <para>
	  /// This is primarily intended for use by market data factories which might receive single values or
	  /// scenario values from upstream market data factories.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the return type of the function </param>
	  /// <param name="fn">  a function to apply to the market data in the box </param>
	  /// <returns> a result wrapping a box containing the return value of the function </returns>
	  MarketDataBox<R> map<R>(System.Func<T, R> fn);

	  /// <summary>
	  /// Applies a function to the contents of the box once for each scenario and returns a box containing
	  /// the values returned from the function.
	  /// <para>
	  /// The <seealso cref="#getScenarioCount() scenario count"/> of the box must be one or it must be equal to the
	  /// {@code scenarioCount} argument. The <seealso cref="#getScenarioCount() scenario count"/> of the return value
	  /// will be equal to {@code scenarioCount}.
	  /// </para>
	  /// <para>
	  /// The box implementation takes care of checking whether it contains a single value or a scenario value,
	  /// applying the function to the value for each scenario and packing the return values into a box.
	  /// </para>
	  /// <para>
	  /// This is primarily intended to be used by perturbations which generate separate market data values for
	  /// each scenario data by applying a function to the existing value for the scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the total number of scenarios </param>
	  /// <param name="fn">  the function that is invoked with a scenario index and the market data value for that scenario.
	  ///   The return value is used as the scenario data in the returned box </param>
	  /// @param <R>  the type of the returned market data </param>
	  /// <returns> a box containing market data created by applying the function to the contents of this box </returns>
	  MarketDataBox<R> mapWithIndex<R>(int scenarioCount, ObjIntFunction<T, R> fn);

	  /// <summary>
	  /// Applies a function to the market data in this box and another box and returns a box containing the result.
	  /// <para>
	  /// The box implementation takes care of checking whether the input boxes contain single values or a scenario values,
	  /// applying the function to the value for each scenario and packing the return value into a box.
	  /// </para>
	  /// <para>
	  /// This is primarily intended for use by market data factories which might receive single values or
	  /// scenario values from upstream market data factories.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <U>  the type of market data in the other box </param>
	  /// @param <R>  the type of the market data returned in the result of the function </param>
	  /// <param name="other">  another market data box </param>
	  /// <param name="fn">  the function invoked with the market data from each box. The return value is used to build the data
	  ///   in the returned box </param>
	  /// <returns> a box containing market data created by applying the function to the data in this box and another box </returns>
	  MarketDataBox<R> combineWith<U, R>(MarketDataBox<U> other, System.Func<T, U, R> fn);

	  /// <summary>
	  /// Returns a stream over the contents of the box.
	  /// </summary>
	  /// <returns> a stream over the contents of the box </returns>
	  Stream<T> stream();

	}

}
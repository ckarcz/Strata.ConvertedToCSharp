using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

	/// <summary>
	/// Provides access to market data across one or more scenarios.
	/// <para>
	/// Market data is looked up using subclasses of <seealso cref="MarketDataId"/>.
	/// All data is valid for a single date, defined by <seealso cref="#getValuationDate()"/>.
	/// </para>
	/// <para>
	/// There are two ways to access the available market data.
	/// </para>
	/// <para>
	/// The first way is to use the access methods on this interface that return the data
	/// associated with a single identifier for all scenarios. The two key methods are
	/// <seealso cref="#getValue(MarketDataId)"/> and <seealso cref="#getScenarioValue(ScenarioMarketDataId)"/>.
	/// </para>
	/// <para>
	/// The second way is to use the method <seealso cref="#scenarios()"/> or <seealso cref="#scenario(int)"/>.
	/// These return all the data associated with a single scenario.
	/// This approach is convenient for single scenario pricers.
	/// </para>
	/// <para>
	/// The standard implementation is <seealso cref="ImmutableScenarioMarketData"/>.
	/// </para>
	/// </summary>
	public interface ScenarioMarketData
	{

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a valuation date, map of values and time-series.
	  /// <para>
	  /// The valuation date and map of values must have the same number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="valuationDate">  the valuation dates associated with all scenarios </param>
	  /// <param name="values">  the market data values, one for each scenario </param>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> a set of market data containing the values in the map </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ScenarioMarketData of(int scenarioCount, java.time.LocalDate valuationDate, java.util.Map<JavaToDotNetGenericWildcard extends com.opengamma.strata.data.MarketDataId<JavaToDotNetGenericWildcard>, MarketDataBox<JavaToDotNetGenericWildcard>> values, java.util.Map<JavaToDotNetGenericWildcard extends com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries)
	//  {
	//
	//	return of(scenarioCount, MarketDataBox.ofSingleValue(valuationDate), values, timeSeries);
	//  }

	  /// <summary>
	  /// Obtains an instance from a valuation date, map of values and time-series.
	  /// <para>
	  /// The valuation date and map of values must have the same number of scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="valuationDate">  the valuation dates associated with the market data, one for each scenario </param>
	  /// <param name="values">  the market data values, one for each scenario </param>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> a set of market data containing the values in the map </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ScenarioMarketData of(int scenarioCount, MarketDataBox<java.time.LocalDate> valuationDate, java.util.Map<JavaToDotNetGenericWildcard extends com.opengamma.strata.data.MarketDataId<JavaToDotNetGenericWildcard>, MarketDataBox<JavaToDotNetGenericWildcard>> values, java.util.Map<JavaToDotNetGenericWildcard extends com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries> timeSeries)
	//  {
	//
	//	return ImmutableScenarioMarketData.of(scenarioCount, valuationDate, values, timeSeries);
	//  }

	  /// <summary>
	  /// Obtains an instance by wrapping a single set of market data.
	  /// <para>
	  /// The result will consist of a {@code ScenarioMarketData} that returns the specified
	  /// market data for each scenario.
	  /// </para>
	  /// <para>
	  /// This can be used in association with the
	  /// <seealso cref="#withPerturbation(MarketDataId, ScenarioPerturbation, ReferenceData) withPerturbation"/>
	  /// method to take a base set of market data and create a complete set of perturbations.
	  /// See {@code MarketDataFactory} for the ability to apply multiple perturbations, including
	  /// perturbations to calibration inputs, such as quotes.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioCount">  the number of scenarios, one or more </param>
	  /// <param name="marketData">  the single set of market data </param>
	  /// <returns> a set of market data containing the values in the map </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ScenarioMarketData of(int scenarioCount, com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return RepeatedScenarioMarketData.of(scenarioCount, marketData);
	//  }

	  /// <summary>
	  /// Obtains a market data instance that contains no data and has no scenarios.
	  /// </summary>
	  /// <returns> an empty instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ScenarioMarketData empty()
	//  {
	//	return ImmutableScenarioMarketData.empty();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a box that can provide the valuation date of each scenario.
	  /// </summary>
	  /// <returns> the valuation dates of the scenarios </returns>
	  MarketDataBox<LocalDate> ValuationDate {get;}

	  /// <summary>
	  /// Gets the number of scenarios.
	  /// </summary>
	  /// <returns> the number of scenarios </returns>
	  int ScenarioCount {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a stream of market data, one for each scenario.
	  /// <para>
	  /// The stream will return instances of <seealso cref="MarketData"/>, where each represents
	  /// a single scenario view of the complete set of data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the stream of market data, one for the each scenario </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.stream.Stream<com.opengamma.strata.data.MarketData> scenarios()
	//  {
	//	return IntStream.range(0, getScenarioCount()).mapToObj(scenarioIndex -> SingleScenarioMarketData.of(this, scenarioIndex));
	//  }

	  /// <summary>
	  /// Returns market data for a single scenario.
	  /// <para>
	  /// This returns a view of the market data for the single specified scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scenarioIndex">  the scenario index </param>
	  /// <returns> the market data for the specified scenario </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.data.MarketData scenario(int scenarioIndex)
	//  {
	//	return SingleScenarioMarketData.of(this, scenarioIndex);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this market data contains a value for the specified identifier.
	  /// </summary>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> true if the market data contains a value for the identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean containsValue(com.opengamma.strata.data.MarketDataId<JavaToDotNetGenericWildcard> id)
	//  {
	//	return findValue(id).isPresent();
	//  }

	  /// <summary>
	  /// Gets the market data value associated with the specified identifier.
	  /// <para>
	  /// The result is a box that provides data for all scenarios.
	  /// If this market data instance contains the identifier, the value will be returned.
	  /// Otherwise, an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the market data value box providing data for all scenarios </returns>
	  /// <exception cref="MarketDataNotFoundException"> if the identifier is not found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> MarketDataBox<T> getValue(com.opengamma.strata.data.MarketDataId<T> id)
	//  {
	//	return findValue(id).orElseThrow(() -> new MarketDataNotFoundException(Messages.format("Market data not found for '{}' of type '{}'", id, id.getClass().getSimpleName())));
	//  }

	  /// <summary>
	  /// Finds the market data value associated with the specified identifier.
	  /// <para>
	  /// The result is a box that provides data for all scenarios.
	  /// If this market data instance contains the identifier, the value will be returned.
	  /// Otherwise, an empty optional will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the market data value box providing data for all scenarios, empty if not found </returns>
	  Optional<MarketDataBox<T>> findValue<T>(MarketDataId<T> id);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data identifiers.
	  /// </summary>
	  /// <returns> the set of market data identifiers </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract java.util.Set<com.opengamma.strata.data.MarketDataId<?>> getIds();
	  ISet<MarketDataId<object>> Ids {get;}

	  /// <summary>
	  /// Finds the market data identifiers associated with the specified name.
	  /// <para>
	  /// This returns the unique identifiers that refer to the specified name.
	  /// There may be more than one identifier associated with a name as the name is not unique.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="name">  the name to find </param>
	  /// <returns> the set of market data identifiers, empty if name not found </returns>
	  ISet<MarketDataId<T>> findIds<T>(MarketDataName<T> name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets an object containing market data for multiple scenarios.
	  /// <para>
	  /// There are many possible ways to store scenario market data for a data type. For example, if the single
	  /// values are doubles, the scenario value might simply be a {@code List<Double>} or it might be a wrapper
	  /// class that stores the values more efficiently in a {@code double[]}.
	  /// </para>
	  /// <para>
	  /// If the market data contains a single value for the identifier or a scenario value of the wrong type,
	  /// a value of the required type is created by invoking <seealso cref="ScenarioMarketDataId#createScenarioValue"/>.
	  /// </para>
	  /// <para>
	  /// Normally this should not be necessary. It is assumed the required scenario values will be created by the
	  /// perturbations that create scenario data. However there is no mechanism in the market data system to guarantee
	  /// that scenario values of a particular type are available. If they are not they are created on demand.
	  /// </para>
	  /// <para>
	  /// Values returned from this method might be cached for efficiency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier to find </param>
	  /// @param <T>  the type of the individual market data values used when performing calculations for one scenario </param>
	  /// @param <U>  the type of the object containing the market data for all scenarios </param>
	  /// <returns> an object containing market data for multiple scenarios </returns>
	  /// <exception cref="IllegalArgumentException"> if no value is found </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public default <T, U extends ScenarioArray<T>> U getScenarioValue(ScenarioMarketDataId<T, U> id)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T, U> U getScenarioValue(ScenarioMarketDataId<T, U> id)
	//  {
	//	MarketDataBox<T> box = getValue(id.getMarketDataId());
	//
	//	if (box.isSingleValue())
	//	{
	//	  return id.createScenarioValue(box, getScenarioCount());
	//	}
	//	ScenarioArray<T> scenarioValue = box.getScenarioValue();
	//	if (id.getScenarioMarketDataType().isInstance(scenarioValue))
	//	{
	//	  return (U) scenarioValue;
	//	}
	//	return id.createScenarioValue(box, getScenarioCount());
	//  }

	  /// <summary>
	  /// Returns set of market data which combines the data from this set of data with another set.
	  /// <para>
	  /// If the same item of data is available in both sets, it will be taken from this set.
	  /// </para>
	  /// <para>
	  /// Both sets of data must contain the same number of scenarios, or one of them must have one scenario.
	  /// If one of the sets of data has one scenario, the combined set will have the scenario count
	  /// of the other set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another set of market data </param>
	  /// <returns> a set of market data combining the data in this set with the data in the other </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ScenarioMarketData combinedWith(ScenarioMarketData other)
	//  {
	//	return new CombinedScenarioMarketData(this, other);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-series identifiers.
	  /// <para>
	  /// Time series are not affected by scenarios, therefore there is a single time-series
	  /// for each identifier which is shared between all scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of observable identifiers </returns>
	  ISet<ObservableId> TimeSeriesIds {get;}

	  /// <summary>
	  /// Gets the time-series associated with the specified identifier, empty if not found.
	  /// <para>
	  /// Time series are not affected by scenarios, therefore there is a single time-series
	  /// for each identifier which is shared between all scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the time-series, empty if no time-series found </returns>
	  LocalDateDoubleTimeSeries getTimeSeries(ObservableId id);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this market data with the specified value.
	  /// <para>
	  /// When the result is queried for the specified identifier, the specified value will be returned.
	  /// </para>
	  /// <para>
	  /// The number of scenarios in the box must match this market data.
	  /// </para>
	  /// <para>
	  /// For example, this method could be used to replace a curve with a bumped curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier </param>
	  /// <param name="value">  the value to associate with the identifier </param>
	  /// <returns> the derived market data with the specified identifier and value </returns>
	  /// <exception cref="IllegalArgumentException"> if the scenario count does not match </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> ScenarioMarketData withValue(com.opengamma.strata.data.MarketDataId<T> id, MarketDataBox<T> value)
	//  {
	//	return ExtendedScenarioMarketData.of(id, value, this);
	//  }

	  /// <summary>
	  /// Returns a copy of this market data with the specified value perturbed.
	  /// <para>
	  /// This finds the market data value using the identifier, throwing an exception if not found.
	  /// It then perturbs the value and returns a new instance containing the value.
	  /// </para>
	  /// <para>
	  /// The number of scenarios of the perturbation must match this market data.
	  /// </para>
	  /// <para>
	  /// This method is intended for one off perturbations of calibrated market data, such as curves.
	  /// See {@code MarketDataFactory} for the ability to apply multiple perturbations, including
	  /// perturbations to calibration inputs, such as quotes.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method call.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="id">  the identifier to perturb </param>
	  /// <param name="perturbation">  the perturbation to apply </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> a parameterized data instance based on this with the specified perturbation applied </returns>
	  /// <exception cref="IllegalArgumentException"> if the scenario count does not match </exception>
	  /// <exception cref="MarketDataNotFoundException"> if the identifier is not found </exception>
	  /// <exception cref="RuntimeException"> if unable to perform the perturbation </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> ScenarioMarketData withPerturbation(com.opengamma.strata.data.MarketDataId<T> id, ScenarioPerturbation<T> perturbation, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	if (perturbation.getScenarioCount() != 1 && perturbation.getScenarioCount() != getScenarioCount())
	//	{
	//	  throw new IllegalArgumentException(Messages.format("Scenario count mismatch: perturbation has {} scenarios but this market data has {}", perturbation.getScenarioCount(), getScenarioCount()));
	//	}
	//	return withValue(id, perturbation.applyTo(getValue(id), refData));
	//  }

	}

}
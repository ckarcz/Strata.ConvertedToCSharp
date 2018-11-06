using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// Mutable builder for creating instances of <seealso cref="MarketDataRequirements"/>.
	/// </summary>
	public sealed class MarketDataRequirementsBuilder
	{

	  /// <summary>
	  /// IDs identifying the observable market data values required for the calculations. </summary>
	  private readonly ISet<ObservableId> observables = new HashSet<ObservableId>();

	  /// <summary>
	  /// IDs identifying the non-observable market data values required for the calculations. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Set<com.opengamma.strata.data.MarketDataId<?>> nonObservables = new java.util.HashSet<>();
	  private readonly ISet<MarketDataId<object>> nonObservables = new HashSet<MarketDataId<object>>();

	  /// <summary>
	  /// IDs identifying the time series of market data values required for the calculations. </summary>
	  private readonly ISet<ObservableId> timeSeries = new HashSet<ObservableId>();

	  /// <summary>
	  /// The currencies used in the outputs of the calculations. </summary>
	  private readonly ISet<Currency> outputCurrencies = new HashSet<Currency>();

	  /// <summary>
	  /// Adds requirements for time series of observable market data.
	  /// </summary>
	  /// <param name="ids">  IDs of the data </param>
	  /// <returns> this builder </returns>
	  public MarketDataRequirementsBuilder addTimeSeries<T1>(ICollection<T1> ids) where T1 : com.opengamma.strata.data.ObservableId
	  {
		ArgChecker.notNull(ids, "ids");
		timeSeries.addAll(ids);
		return this;
	  }

	  /// <summary>
	  /// Adds requirements for time series of observable market data.
	  /// </summary>
	  /// <param name="ids">  IDs of the data </param>
	  /// <returns> this builder </returns>
	  public MarketDataRequirementsBuilder addTimeSeries(params ObservableId[] ids)
	  {
		return addTimeSeries(Arrays.asList(ids));
	  }

	  /// <summary>
	  /// Adds requirements for single values of market data.
	  /// </summary>
	  /// <param name="ids">  IDs of the data </param>
	  /// <returns> this builder </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public MarketDataRequirementsBuilder addValues(java.util.Collection<? extends com.opengamma.strata.data.MarketDataId<?>> ids)
	  public MarketDataRequirementsBuilder addValues<T1>(ICollection<T1> ids)
	  {
		ArgChecker.notNull(ids, "ids");

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (com.opengamma.strata.data.MarketDataId<?> id : ids)
		foreach (MarketDataId<object> id in ids)
		{
		  if (id is ObservableId)
		  {
			observables.Add((ObservableId) id);
		  }
		  else
		  {
			nonObservables.Add(id);
		  }
		}
		return this;
	  }

	  /// <summary>
	  /// Adds requirements for single values of market data.
	  /// </summary>
	  /// <param name="ids">  IDs of the data </param>
	  /// <returns> this builder </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public MarketDataRequirementsBuilder addValues(com.opengamma.strata.data.MarketDataId<?>... ids)
	  public MarketDataRequirementsBuilder addValues(params MarketDataId<object>[] ids)
	  {
		return addValues(Arrays.asList(ids));
	  }

	  /// <summary>
	  /// Adds the output currencies.
	  /// <para>
	  /// These are used to ensure that FX rate market data is available for currency conversion.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencies">  the output currencies </param>
	  /// <returns> this builder </returns>
	  public MarketDataRequirementsBuilder addOutputCurrencies(params Currency[] currencies)
	  {
		outputCurrencies.addAll(Arrays.asList(currencies));
		return this;
	  }

	  /// <summary>
	  /// Adds all requirements from an instance of {@code MarketDataRequirements} to this builder.
	  /// </summary>
	  /// <param name="requirements">  a set of requirements </param>
	  /// <returns> this builder </returns>
	  public MarketDataRequirementsBuilder addRequirements(MarketDataRequirements requirements)
	  {
		ArgChecker.notNull(requirements, "requirements");
		observables.addAll(requirements.Observables);
		outputCurrencies.addAll(requirements.OutputCurrencies);
		nonObservables.addAll(requirements.NonObservables);
		timeSeries.addAll(requirements.TimeSeries);
		return this;
	  }

	  /// <summary>
	  /// Returns a set of market data requirements built from the data in this builder.
	  /// </summary>
	  /// <returns> a set of market data requirements built from the data in this builder </returns>
	  public MarketDataRequirements build()
	  {
		return new MarketDataRequirements(observables, nonObservables, timeSeries, outputCurrencies);
	  }
	}

}
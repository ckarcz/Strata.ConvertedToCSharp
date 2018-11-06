using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// Builder for {@code CurrencyParameterSensitivities}.
	/// </summary>
	public sealed class CurrencyParameterSensitivitiesBuilder
	{

	  /// <summary>
	  /// The map of sensitivity data.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.SortedMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.data.MarketDataName<?>, com.opengamma.strata.basics.currency.Currency>, CurrencyParameterSensitivityBuilder> data = new java.util.TreeMap<>();
	  private readonly SortedDictionary<Pair<MarketDataName<object>, Currency>, CurrencyParameterSensitivityBuilder> data = new SortedDictionary<Pair<MarketDataName<object>, Currency>, CurrencyParameterSensitivityBuilder>();

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  internal CurrencyParameterSensitivitiesBuilder()
	  {
	  }

	  // restricted constructor
	  internal CurrencyParameterSensitivitiesBuilder(IList<CurrencyParameterSensitivity> sensitivities)
	  {
		sensitivities.ForEach(this.add);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds sensitivities to the builder.
	  /// <para>
	  /// Values with the same market data name and currency will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivities to add </param>
	  /// <returns> this, for chaining </returns>
	  public CurrencyParameterSensitivitiesBuilder add(CurrencyParameterSensitivities sensitivities)
	  {
		return add(sensitivities.Sensitivities);
	  }

	  /// <summary>
	  /// Adds sensitivities to the builder.
	  /// <para>
	  /// Values with the same market data name and currency will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivities to add </param>
	  /// <returns> this, for chaining </returns>
	  public CurrencyParameterSensitivitiesBuilder add(IList<CurrencyParameterSensitivity> sensitivities)
	  {
		sensitivities.ForEach(this.add);
		return this;
	  }

	  /// <summary>
	  /// Adds a sensitivity to the builder.
	  /// <para>
	  /// Values with the same market data name and currency will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensToAdd">  the sensitivity to add </param>
	  /// <returns> this, for chaining </returns>
	  public CurrencyParameterSensitivitiesBuilder add(CurrencyParameterSensitivity sensToAdd)
	  {
		sensToAdd.sensitivities().forEach((md, value) => add(sensToAdd.MarketDataName, sensToAdd.Currency, md, value));
		return this;
	  }

	  /// <summary>
	  /// Adds a single sensitivity to the builder.
	  /// <para>
	  /// Values with the same market data name and currency will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketDataName">  the curve name </param>
	  /// <param name="currency">  the currency of the sensitivity </param>
	  /// <param name="metadata">  the sensitivity metadata, not empty </param>
	  /// <param name="sensitivityValue">  the sensitivity value </param>
	  /// <returns> this, for chaining </returns>
	  public CurrencyParameterSensitivitiesBuilder add<T1>(MarketDataName<T1> marketDataName, Currency currency, ParameterMetadata metadata, double sensitivityValue)
	  {

		data.computeIfAbsent(Pair.of(marketDataName, currency), t => new CurrencyParameterSensitivityBuilder(marketDataName, currency)).add(metadata, sensitivityValue);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Maps the sensitivity metadata.
	  /// <para>
	  /// If the function returns the same metadata for two different inputs, the sensitivity value will be summed.
	  /// For example, this could be used to normalize tenors.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadataFn">  the function to adjust the metadata </param>
	  /// <returns> this, for chaining </returns>
	  public CurrencyParameterSensitivitiesBuilder mapMetadata(System.Func<ParameterMetadata, ParameterMetadata> metadataFn)
	  {
		foreach (CurrencyParameterSensitivityBuilder builder in data.Values)
		{
		  builder.mapMetadata(metadataFn);
		}
		return this;
	  }

	  /// <summary>
	  /// Filters the sensitivity values.
	  /// <para>
	  /// For example, this could be used to remove sensitivities near to zero.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  the predicate to test the value, return true to retain the value </param>
	  /// <returns> this, for chaining </returns>
	  public CurrencyParameterSensitivitiesBuilder filterSensitivity(System.Func<double, bool> predicate)
	  {
		foreach (CurrencyParameterSensitivityBuilder builder in data.Values)
		{
		  builder.filterSensitivity(predicate);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the sensitivity from the provided data.
	  /// <para>
	  /// If all the values added are tenor-based, or all are date-based, then the resulting
	  /// sensitivity will have the tenors sorted.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the sensitivities instance </returns>
	  public CurrencyParameterSensitivities build()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return CurrencyParameterSensitivities.of(data.Values.Select(CurrencyParameterSensitivityBuilder::build).OrderBy(CurrencyParameterSensitivity::compareKey).collect(toImmutableList()));
	  }

	}

}
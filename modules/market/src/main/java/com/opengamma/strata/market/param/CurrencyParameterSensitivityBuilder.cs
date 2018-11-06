using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// Builder for {@code CurrencyParameterSensitivity}
	/// </summary>
	internal sealed class CurrencyParameterSensitivityBuilder
	{

	  /// <summary>
	  /// The market data name.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.data.MarketDataName<?> marketDataName;
	  private readonly MarketDataName<object> marketDataName;
	  /// <summary>
	  /// The currency.
	  /// </summary>
	  private readonly Currency currency;
	  /// <summary>
	  /// The map of sensitivity data.
	  /// </summary>
	  private readonly IDictionary<ParameterMetadata, double> sensitivity = new LinkedHashMap<ParameterMetadata, double>();

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  internal CurrencyParameterSensitivityBuilder<T1>(MarketDataName<T1> marketDataName, Currency currency)
	  {
		this.marketDataName = ArgChecker.notNull(marketDataName, "marketDataName");
		this.currency = ArgChecker.notNull(currency, "currency");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a single sensitivity to the builder.
	  /// <para>
	  /// If the key already exists, the sensitivity value will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the sensitivity metadata </param>
	  /// <param name="sensitivityValue">  the sensitivity value </param>
	  /// <returns> this, for chaining </returns>
	  internal CurrencyParameterSensitivityBuilder add(ParameterMetadata metadata, double sensitivityValue)
	  {
		if (metadata.Equals(ParameterMetadata.empty()))
		{
		  throw new System.ArgumentException("Builder does not allow empty parameter metadata");
		}
		this.sensitivity.merge(metadata, sensitivityValue, double?.sum);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Maps the sensitivity metadata.
	  /// </summary>
	  /// <returns> this, for chaining </returns>
	  internal CurrencyParameterSensitivityBuilder mapMetadata(System.Func<ParameterMetadata, ParameterMetadata> metadataFn)
	  {
		ImmutableMap<ParameterMetadata, double> @base = ImmutableMap.copyOf(sensitivity);
		sensitivity.Clear();
		MapStream.of(@base).mapKeys(metadataFn).forEach((md, v) => add(md, v));
		return this;
	  }

	  /// <summary>
	  /// Filters the sensitivity values.
	  /// </summary>
	  /// <returns> this, for chaining </returns>
	  internal CurrencyParameterSensitivityBuilder filterSensitivity(System.Func<double, bool> predicate)
	  {
		for (IEnumerator<double> it = sensitivity.Values.GetEnumerator(); it.MoveNext();)
		{
		  double? value = it.Current;
		  if (!predicate(value))
		  {
//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
			it.remove();
		  }
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the sensitivity from the provided data.
	  /// </summary>
	  /// <returns> the sensitivities instance </returns>
	  internal CurrencyParameterSensitivity build()
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSet<Type> metadataTypes = sensitivity.Keys.Select(object.getClass).collect(toImmutableSet());
		if (metadataTypes.size() == 1)
		{
		  if (metadataTypes.GetEnumerator().next().IsAssignableFrom(typeof(TenoredParameterMetadata)))
		  {
			IDictionary<ParameterMetadata, double> sorted = MapStream.of(sensitivity).sortedKeys(System.Collections.IComparer.comparing(k => ((TenoredParameterMetadata) k).Tenor)).toMap();
			return CurrencyParameterSensitivity.of(marketDataName, currency, sorted);
		  }
		  if (metadataTypes.GetEnumerator().next().IsAssignableFrom(typeof(DatedParameterMetadata)))
		  {
			IDictionary<ParameterMetadata, double> sorted = MapStream.of(sensitivity).sortedKeys(System.Collections.IComparer.comparing(k => ((DatedParameterMetadata) k).Date)).toMap();
			return CurrencyParameterSensitivity.of(marketDataName, currency, sorted);
		  }
		}
		return CurrencyParameterSensitivity.of(marketDataName, currency, sensitivity);
	  }

	}

}
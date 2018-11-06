using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// A mutable builder for building an instance of <seealso cref="MarketDataConfig"/>.
	/// </summary>
	public sealed class MarketDataConfigBuilder
	{

	  /// <summary>
	  /// The configuration objects, keyed by their type and name. </summary>
	  private readonly IDictionary<Type, SingleTypeMarketDataConfig> values = new Dictionary<Type, SingleTypeMarketDataConfig>();

	  /// <summary>
	  /// The configuration objects where there is only one instance per type. </summary>
	  private readonly IDictionary<Type, object> defaultValues = new Dictionary<Type, object>();

	  /// <summary>
	  /// Package-private constructor used by <seealso cref="MarketDataConfig#builder()"/>.
	  /// </summary>
	  internal MarketDataConfigBuilder()
	  {
	  }

	  /// <summary>
	  /// Adds an item of configuration under the specified name.
	  /// </summary>
	  /// <param name="name">  the name of the configuration item </param>
	  /// <param name="value">  the configuration item </param>
	  /// <returns> this builder </returns>
	  public MarketDataConfigBuilder add(string name, object value)
	  {
		ArgChecker.notEmpty(name, "name");
		ArgChecker.notNull(value, "value");

		Type configType = value.GetType();
		SingleTypeMarketDataConfig configs = configsForType(configType);
		values[configType] = configs.withConfig(name, value);
		return this;
	  }

	  /// <summary>
	  /// Adds an item of configuration under the specified name.
	  /// </summary>
	  /// <param name="name">  the name of the configuration item </param>
	  /// <param name="value">  the configuration item </param>
	  /// <returns> this builder </returns>
	  public MarketDataConfigBuilder add<T1>(TypedString<T1> name, object value)
	  {
		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(value, "value");

		Type configType = value.GetType();
		SingleTypeMarketDataConfig configs = configsForType(configType);
		values[configType] = configs.withConfig(name.Name, value);
		return this;
	  }

	  /// <summary>
	  /// Adds an item of configuration that is the default of its type.
	  /// <para>
	  /// There can only be one default item for each type.
	  /// </para>
	  /// <para>
	  /// There is a class of configuration where there is always a one value shared between all calculations.
	  /// An example is the configuration which specifies which market quote to use when building FX rates for
	  /// a currency pair. All calculations use the same set of FX rates obtained from the same underlying
	  /// market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the configuration value </param>
	  /// @param <T>  the type used when looking up the configuration </param>
	  /// <returns> this builder </returns>
	  public MarketDataConfigBuilder addDefault<T>(T value)
	  {
		ArgChecker.notNull(value, "value");
		defaultValues[value.GetType()] = value;
		return this;
	  }

	  /// <summary>
	  /// Returns a <seealso cref="MarketDataConfig"/> instance built from the data in this builder.
	  /// </summary>
	  /// <returns> a <seealso cref="MarketDataConfig"/> instance built from the data in this builder </returns>
	  public MarketDataConfig build()
	  {
		return new MarketDataConfig(values, defaultValues);
	  }

	  /// <summary>
	  /// Returns a set of configuration object for the specified type, creating one and adding it to
	  /// the map if not found.
	  /// </summary>
	  private SingleTypeMarketDataConfig configsForType(Type configType)
	  {
		SingleTypeMarketDataConfig configs = values[configType];
		if (configs != null)
		{
		  return configs;
		}
		SingleTypeMarketDataConfig newConfigs = SingleTypeMarketDataConfig.builder().configType(configType).build();
		values[configType] = newConfigs;
		return newConfigs;
	  }
	}

}
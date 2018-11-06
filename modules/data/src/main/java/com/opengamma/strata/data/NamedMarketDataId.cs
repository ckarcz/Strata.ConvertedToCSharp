/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	/// <summary>
	/// An identifier for a unique item of market data that can has a non-unique name.
	/// <para>
	/// A <seealso cref="MarketDataId"/> is used to uniquely identify market data within a system.
	/// By contrast, a <seealso cref="MarketDataName"/> is only unique within a single coherent data set.
	/// </para>
	/// <para>
	/// For example, a curve group contains a set of curves, and within the group the name is unique.
	/// But the market data system may contain many curve groups where the same name appears in each group.
	/// The {@code MarketDataId} includes both the group name and curve name in order to ensure uniqueness.
	/// But within a specific context, the <seealso cref="MarketDataName"/> is also sufficient to find the same data.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data this identifier refers to </param>
	public interface NamedMarketDataId<T> : MarketDataId<T>
	{

	  /// <summary>
	  /// Gets the market data name.
	  /// <para>
	  /// This name can be used to obtain the market data within a single coherent data set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the name of the market data this identifier refers to </returns>
	  MarketDataName<T> MarketDataName {get;}

	}

}
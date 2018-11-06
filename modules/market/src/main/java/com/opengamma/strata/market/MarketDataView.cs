/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{

	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// A high-level view of a single item of market data.
	/// <para>
	/// Implementations provide a high-level view of a single piece of market data.
	/// The market data has typically been calibrated, such as a curve or surface.
	/// The data is valid on a single valuation date.
	/// </para>
	/// </summary>
	public interface MarketDataView
	{

	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// The raw data in this provider is calibrated for this date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  /// <summary>
	  /// Finds the market data with the specified name.
	  /// <para>
	  /// This is most commonly used to find an underlying curve or surface by name.
	  /// If the market data cannot be found, empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="name">  the name to find </param>
	  /// <returns> the market data value, empty if not found </returns>
	  Optional<T> findData<T>(MarketDataName<T> name);

	}

}
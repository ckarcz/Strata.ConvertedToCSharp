using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using FxForwardRates = com.opengamma.strata.pricer.fx.FxForwardRates;
	using FxForwardSensitivity = com.opengamma.strata.pricer.fx.FxForwardSensitivity;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using FxIndexSensitivity = com.opengamma.strata.pricer.fx.FxIndexSensitivity;

	/// <summary>
	/// A provider of rates, such as Ibor and Overnight, used for pricing financial instruments.
	/// <para>
	/// This provides the environmental information against which pricing occurs.
	/// The valuation date, FX rates, discount factors, time-series and forward curves are included.
	/// </para>
	/// <para>
	/// The standard independent implementation is <seealso cref="ImmutableRatesProvider"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface RatesProvider : BaseProvider
	{

	  /// <summary>
	  /// Gets the set of Ibor indices that are available.
	  /// <para>
	  /// If an index is present in the result of this method, then
	  /// <seealso cref="#iborIndexRates(IborIndex)"/> should not throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of Ibor indices </returns>
	  ISet<IborIndex> IborIndices {get;}

	  /// <summary>
	  /// Gets the set of Overnight indices that are available.
	  /// <para>
	  /// If an index is present in the result of this method, then
	  /// <seealso cref="#overnightIndexRates(OvernightIndex)"/> should not throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of Overnight indices </returns>
	  ISet<OvernightIndex> OvernightIndices {get;}

	  /// <summary>
	  /// Gets the set of Price indices that are available.
	  /// <para>
	  /// If an index is present in the result of this method, then
	  /// <seealso cref="#priceIndexValues(PriceIndex)"/> should not throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of Price indices </returns>
	  ISet<PriceIndex> PriceIndices {get;}

	  /// <summary>
	  /// Gets the set of indices that have time-series available.
	  /// <para>
	  /// Note that the method <seealso cref="#timeSeries(Index)"/> returns an empty time-series
	  /// when the index is not known, thus this method is useful to determine if there
	  /// actually is a time-series in the underlying data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices with time-series </returns>
	  ISet<Index> TimeSeriesIndices {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rates for an FX index.
	  /// <para>
	  /// This returns an object that can provide historic and forward rates for the specified index.
	  /// </para>
	  /// <para>
	  /// An FX rate is the conversion rate between two currencies. An FX index is the rate
	  /// as published by a specific organization, typically at a well-known time-of-day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index to find rates for </param>
	  /// <returns> the rates for the specified index </returns>
	  /// <exception cref="IllegalArgumentException"> if the rates are not available </exception>
	  FxIndexRates fxIndexRates(FxIndex index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the forward FX rates for a currency pair.
	  /// <para>
	  /// This returns an object that can provide forward rates for the specified currency pair.
	  /// See <seealso cref="#fxIndexRates(FxIndex)"/> for forward rates with daily fixings.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair to find forward rates for </param>
	  /// <returns> the forward rates for the specified currency pair </returns>
	  /// <exception cref="IllegalArgumentException"> if the rates are not available </exception>
	  FxForwardRates fxForwardRates(CurrencyPair currencyPair);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rates for an Ibor index.
	  /// <para>
	  /// The rate of the Ibor index, such as 'GBP-LIBOR-3M', varies over time.
	  /// This returns an object that can provide historic and forward rates for the specified index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index to find rates for </param>
	  /// <returns> the rates for the specified index </returns>
	  /// <exception cref="IllegalArgumentException"> if the rates are not available </exception>
	  IborIndexRates iborIndexRates(IborIndex index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rates for an Overnight index.
	  /// <para>
	  /// The rate of the Overnight index, such as 'EUR-EONIA', varies over time.
	  /// This returns an object that can provide historic and forward rates for the specified index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index to find rates for </param>
	  /// <returns> the rates for the specified index </returns>
	  /// <exception cref="IllegalArgumentException"> if the rates are not available </exception>
	  OvernightIndexRates overnightIndexRates(OvernightIndex index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the values for an Price index.
	  /// <para>
	  /// The value of the Price index, such as 'US-CPI-U', varies over time.
	  /// This returns an object that can provide historic and forward values for the specified index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index to find values for </param>
	  /// <returns> the values for the specified index </returns>
	  /// <exception cref="IllegalArgumentException"> if the values are not available </exception>
	  PriceIndexValues priceIndexValues(PriceIndex index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the parameter sensitivity.
	  /// <para>
	  /// This computes the <seealso cref="CurrencyParameterSensitivities"/> associated with the <seealso cref="PointSensitivities"/>.
	  /// This corresponds to the projection of the point sensitivity to the internal parameters representation.
	  /// </para>
	  /// <para>
	  /// For example, the point sensitivities could represent the sensitivity to a date on the first
	  /// of each month in a year relative to a specific forward curve. This method converts to the point
	  /// sensitivities to be relative to each parameter on the underlying curve, such as the 1 day, 1 week,
	  /// 1 month, 3 month, 12 month and 5 year nodal points.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivities </param>
	  /// <returns> the sensitivity to the curve parameters </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.param.CurrencyParameterSensitivities parameterSensitivity(com.opengamma.strata.market.sensitivity.PointSensitivities pointSensitivities)
	//  {
	//	CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
	//	for (PointSensitivity point : pointSensitivities.getSensitivities())
	//	{
	//	  if (point instanceof ZeroRateSensitivity)
	//	  {
	//		ZeroRateSensitivity pt = (ZeroRateSensitivity) point;
	//		DiscountFactors factors = discountFactors(pt.getCurveCurrency());
	//		sens = sens.combinedWith(factors.parameterSensitivity(pt));
	//
	//	  }
	//	  else if (point instanceof IborRateSensitivity)
	//	  {
	//		IborRateSensitivity pt = (IborRateSensitivity) point;
	//		IborIndexRates rates = iborIndexRates(pt.getIndex());
	//		sens = sens.combinedWith(rates.parameterSensitivity(pt));
	//
	//	  }
	//	  else if (point instanceof OvernightRateSensitivity)
	//	  {
	//		OvernightRateSensitivity pt = (OvernightRateSensitivity) point;
	//		OvernightIndexRates rates = overnightIndexRates(pt.getIndex());
	//		sens = sens.combinedWith(rates.parameterSensitivity(pt));
	//
	//	  }
	//	  else if (point instanceof FxIndexSensitivity)
	//	  {
	//		FxIndexSensitivity pt = (FxIndexSensitivity) point;
	//		FxIndexRates rates = fxIndexRates(pt.getIndex());
	//		sens = sens.combinedWith(rates.parameterSensitivity(pt));
	//
	//	  }
	//	  else if (point instanceof InflationRateSensitivity)
	//	  {
	//		InflationRateSensitivity pt = (InflationRateSensitivity) point;
	//		PriceIndexValues rates = priceIndexValues(pt.getIndex());
	//		sens = sens.combinedWith(rates.parameterSensitivity(pt));
	//
	//	  }
	//	  else if (point instanceof FxForwardSensitivity)
	//	  {
	//		FxForwardSensitivity pt = (FxForwardSensitivity) point;
	//		FxForwardRates rates = fxForwardRates(pt.getCurrencyPair());
	//		sens = sens.combinedWith(rates.parameterSensitivity(pt));
	//	  }
	//	}
	//	return sens;
	//  }

	  /// <summary>
	  /// Computes the currency exposure.
	  /// <para>
	  /// This computes the currency exposure in the form of a <seealso cref="MultiCurrencyAmount"/> associated with the 
	  /// <seealso cref="PointSensitivities"/>. This corresponds to the projection of the point sensitivity to the
	  /// currency exposure associated to an <seealso cref="FxIndexSensitivity"/>.
	  /// </para>
	  /// <para>
	  /// For example, the point sensitivities could represent the sensitivity to a FX Index.
	  /// This method produces the implicit currency exposure embedded in the FX index sensitivity.
	  /// </para>
	  /// <para>
	  /// Reference: Currency Exposure and FX index, OpenGamma Documentation 32, July 2015.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivities">  the point sensitivities </param>
	  /// <returns> the currency exposure </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.MultiCurrencyAmount currencyExposure(com.opengamma.strata.market.sensitivity.PointSensitivities pointSensitivities)
	//  {
	//	MultiCurrencyAmount ce = MultiCurrencyAmount.empty();
	//	for (PointSensitivity point : pointSensitivities.getSensitivities())
	//	{
	//	  if (point instanceof FxIndexSensitivity)
	//	  {
	//		FxIndexSensitivity pt = (FxIndexSensitivity) point;
	//		FxIndexRates rates = fxIndexRates(pt.getIndex());
	//		ce = ce.plus(rates.currencyExposure(pt));
	//	  }
	//	  if (point instanceof FxForwardSensitivity)
	//	  {
	//		FxForwardSensitivity pt = (FxForwardSensitivity) point;
	//		pt = (FxForwardSensitivity) pt.convertedTo(pt.getReferenceCurrency(), this);
	//		FxForwardRates rates = fxForwardRates(pt.getCurrencyPair());
	//		ce = ce.plus(rates.currencyExposure(pt));
	//	  }
	//	}
	//	return ce;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the market data with the specified name.
	  /// <para>
	  /// This is most commonly used to find a <seealso cref="Curve"/> using a <seealso cref="CurveName"/>.
	  /// If the market data cannot be found, empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data value </param>
	  /// <param name="name">  the name to find </param>
	  /// <returns> the market data value, empty if not found </returns>
	  Optional<T> findData<T>(MarketDataName<T> name);

	  /// <summary>
	  /// Gets the time series.
	  /// <para>
	  /// This returns time series for the index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <returns> the time series, empty if time-series not found </returns>
	  LocalDateDoubleTimeSeries timeSeries(Index index);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this provider to an equivalent {@code ImmutableRatesProvider}.
	  /// </summary>
	  /// <returns> the equivalent immutable rates provider </returns>
	  ImmutableRatesProvider toImmutableRatesProvider();

	}

}
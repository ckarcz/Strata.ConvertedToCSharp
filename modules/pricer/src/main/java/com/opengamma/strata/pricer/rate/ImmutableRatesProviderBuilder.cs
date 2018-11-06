using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using Curve = com.opengamma.strata.market.curve.Curve;

	/// <summary>
	/// Builder for the immutable rates provider.
	/// </summary>
	/// <seealso cref= ImmutableRatesProvider </seealso>
	public sealed class ImmutableRatesProviderBuilder
	{

	  /// <summary>
	  /// The valuation date.
	  /// All curves and other data items in this provider are calibrated for this date.
	  /// </summary>
	  private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The provider of foreign exchange rates.
	  /// Conversions where both currencies are the same always succeed.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private FxRateProvider fxRateProvider_Renamed = FxMatrix.empty();
	  /// <summary>
	  /// The discount curves, defaulted to an empty map.
	  /// The curve data, predicting the future, associated with each currency.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly IDictionary<Currency, Curve> discountCurves_Renamed = new Dictionary<Currency, Curve>();
	  /// <summary>
	  /// The forward curves, defaulted to an empty map.
	  /// The curve data, predicting the future, associated with each index.
	  /// This is used for Ibor, Overnight and Price indices.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly IDictionary<Index, Curve> indexCurves_Renamed = new Dictionary<Index, Curve>();
	  /// <summary>
	  /// The time-series, defaulted to an empty map.
	  /// The historic data associated with each index.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly IDictionary<Index, LocalDateDoubleTimeSeries> timeSeries_Renamed = new Dictionary<Index, LocalDateDoubleTimeSeries>();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance specifying the valuation date.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  internal ImmutableRatesProviderBuilder(LocalDate valuationDate)
	  {
		this.valuationDate = ArgChecker.notNull(valuationDate, "valuationDate");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the FX rate provider.
	  /// </summary>
	  /// <param name="fxRateProvider">  the rate provider </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder fxRateProvider(FxRateProvider fxRateProvider)
	  {
		this.fxRateProvider_Renamed = ArgChecker.notNull(fxRateProvider, "fxRateProvider");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a discount curve to the provider.
	  /// <para>
	  /// This adds the specified discount curve to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the currency as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the curve </param>
	  /// <param name="discountCurve">  the discount curve </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder discountCurve(Currency currency, Curve discountCurve)
	  {
		ArgChecker.notNull(currency, "currency");
		ArgChecker.notNull(discountCurve, "discountCurve");
		this.discountCurves_Renamed[currency] = discountCurve;
		return this;
	  }

	  /// <summary>
	  /// Adds discount curves to the provider.
	  /// <para>
	  /// This adds the specified discount curves to the provider.
	  /// This operates using <seealso cref="Map#putAll(Map)"/> semantics using the currency as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountCurves">  the discount curves </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder discountCurves<T1>(IDictionary<T1> discountCurves) where T1 : com.opengamma.strata.market.curve.Curve
	  {
		ArgChecker.notNull(discountCurves, "discountCurves");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<com.opengamma.strata.basics.currency.Currency, ? extends com.opengamma.strata.market.curve.Curve> entry : discountCurves.entrySet())
		foreach (KeyValuePair<Currency, ? extends Curve> entry in discountCurves.SetOfKeyValuePairs())
		{
		  discountCurve(entry.Key, entry.Value);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds an Ibor index forward curve to the provider.
	  /// <para>
	  /// This adds the specified forward curve to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the Ibor index forward curve </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder iborIndexCurve(IborIndex index, Curve forwardCurve)
	  {
		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		this.indexCurves_Renamed[index] = forwardCurve;
		return this;
	  }

	  /// <summary>
	  /// Adds an Ibor index forward curve to the provider with associated time-series.
	  /// <para>
	  /// This adds the specified forward curve and time-series to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the index forward curve </param>
	  /// <param name="timeSeries">  the associated time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder iborIndexCurve(IborIndex index, Curve forwardCurve, LocalDateDoubleTimeSeries timeSeries)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.indexCurves_Renamed[index] = forwardCurve;
		this.timeSeries_Renamed[index] = timeSeries;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds an Overnight index forward curve to the provider.
	  /// <para>
	  /// This adds the specified forward curve to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the Overnight index forward curve </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder overnightIndexCurve(OvernightIndex index, Curve forwardCurve)
	  {
		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		this.indexCurves_Renamed[index] = forwardCurve;
		return this;
	  }

	  /// <summary>
	  /// Adds an Overnight index forward curve to the provider with associated time-series.
	  /// <para>
	  /// This adds the specified forward curve and time-series to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the index forward curve </param>
	  /// <param name="timeSeries">  the associated time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder overnightIndexCurve(OvernightIndex index, Curve forwardCurve, LocalDateDoubleTimeSeries timeSeries)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.indexCurves_Renamed[index] = forwardCurve;
		this.timeSeries_Renamed[index] = timeSeries;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a Price index forward curve to the provider.
	  /// <para>
	  /// This adds the specified forward curve to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the Price index forward curve </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder priceIndexCurve(PriceIndex index, Curve forwardCurve)
	  {
		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		this.indexCurves_Renamed[index] = forwardCurve;
		return this;
	  }

	  /// <summary>
	  /// Adds an index forward curve to the provider with associated time-series.
	  /// <para>
	  /// This adds the specified forward curve and time-series to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the index forward curve </param>
	  /// <param name="timeSeries">  the associated time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder priceIndexCurve(PriceIndex index, Curve forwardCurve, LocalDateDoubleTimeSeries timeSeries)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.indexCurves_Renamed[index] = forwardCurve;
		this.timeSeries_Renamed[index] = timeSeries;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds an index forward curve to the provider.
	  /// <para>
	  /// This adds the specified forward curve to the provider.
	  /// This is used for Ibor, Overnight and Price indices.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the Ibor index forward curve </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder indexCurve(Index index, Curve forwardCurve)
	  {
		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		if (index is IborIndex || index is OvernightIndex || index is PriceIndex)
		{
		  this.indexCurves_Renamed[index] = forwardCurve;
		}
		else
		{
		  throw new System.ArgumentException("Unsupported index: " + index);
		}
		return this;
	  }

	  /// <summary>
	  /// Adds an index forward curve to the provider with associated time-series.
	  /// <para>
	  /// This adds the specified forward curve to the provider.
	  /// This is used for Ibor, Overnight and Price indices.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index of the curve </param>
	  /// <param name="forwardCurve">  the Ibor index forward curve </param>
	  /// <param name="timeSeries">  the time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder indexCurve(Index index, Curve forwardCurve, LocalDateDoubleTimeSeries timeSeries)
	  {
		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(forwardCurve, "forwardCurve");
		if (index is IborIndex || index is OvernightIndex || index is PriceIndex)
		{
		  this.indexCurves_Renamed[index] = forwardCurve;
		  this.timeSeries_Renamed[index] = timeSeries;
		}
		else
		{
		  throw new System.ArgumentException("Unsupported index: " + index);
		}
		return this;
	  }

	  /// <summary>
	  /// Adds index forward curves to the provider with associated time-series.
	  /// <para>
	  /// This adds the specified index forward curves to the provider.
	  /// This is used for Ibor, Overnight and Price indices.
	  /// This operates using <seealso cref="Map#putAll(Map)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexCurves">  the index forward curves </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder indexCurves<T1>(IDictionary<T1> indexCurves) where T1 : com.opengamma.strata.basics.index.Index
	  {
		ArgChecker.noNulls(indexCurves, "indexCurves");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.basics.index.Index, ? extends com.opengamma.strata.market.curve.Curve> entry : indexCurves.entrySet())
		foreach (KeyValuePair<Index, ? extends Curve> entry in indexCurves.SetOfKeyValuePairs())
		{
		  indexCurve(entry.Key, entry.Value);
		}
		return this;
	  }

	  /// <summary>
	  /// Adds index forward curves to the provider with associated time-series.
	  /// <para>
	  /// This adds the specified index forward curves to the provider.
	  /// This is used for Ibor, Overnight and Price indices.
	  /// This operates using <seealso cref="Map#putAll(Map)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexCurves">  the index forward curves </param>
	  /// <param name="timeSeries">  the associated time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder indexCurves<T1, T2>(IDictionary<T1> indexCurves, IDictionary<T2> timeSeries) where T1 : com.opengamma.strata.basics.index.Index where T2 : com.opengamma.strata.market.curve.Curve
	  {

		ArgChecker.noNulls(indexCurves, "indexCurves");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<? extends com.opengamma.strata.basics.index.Index, ? extends com.opengamma.strata.market.curve.Curve> entry : indexCurves.entrySet())
		foreach (KeyValuePair<Index, ? extends Curve> entry in indexCurves.SetOfKeyValuePairs())
		{
		  Index index = entry.Key;
		  LocalDateDoubleTimeSeries ts = timeSeries[index];
		  ts = (ts != null ? ts : LocalDateDoubleTimeSeries.empty());
		  indexCurve(entry.Key, entry.Value, ts);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a time-series to the provider.
	  /// <para>
	  /// This adds the specified time-series to the provider.
	  /// This operates using <seealso cref="Map#put(Object, Object)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the FX index </param>
	  /// <param name="timeSeries">  the FX index time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder timeSeries(Index index, LocalDateDoubleTimeSeries timeSeries)
	  {
		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(timeSeries, "timeSeries");
		this.timeSeries_Renamed[index] = timeSeries;
		return this;
	  }

	  /// <summary>
	  /// Adds time-series to the provider.
	  /// <para>
	  /// This adds the specified time-series to the provider.
	  /// This operates using <seealso cref="Map#putAll(Map)"/> semantics using the index as the key.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="timeSeries">  the FX index time-series </param>
	  /// <returns> this, for chaining </returns>
	  public ImmutableRatesProviderBuilder timeSeries<T1>(IDictionary<T1> timeSeries) where T1 : com.opengamma.strata.basics.index.Index
	  {
		ArgChecker.noNulls(timeSeries, "timeSeries");
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.timeSeries_Renamed.putAll(timeSeries);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Completes the builder, returning the provider.
	  /// </summary>
	  /// <returns> the provider </returns>
	  public ImmutableRatesProvider build()
	  {
		return new ImmutableRatesProvider(valuationDate, fxRateProvider_Renamed, discountCurves_Renamed, indexCurves_Renamed, timeSeries_Renamed);
	  }

	}

}
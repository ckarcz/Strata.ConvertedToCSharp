/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using FxForwardRates = com.opengamma.strata.pricer.fx.FxForwardRates;
	using FxIndexRates = com.opengamma.strata.pricer.fx.FxIndexRates;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using OvernightIndexRates = com.opengamma.strata.pricer.rate.OvernightIndexRates;
	using PriceIndexValues = com.opengamma.strata.pricer.rate.PriceIndexValues;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Mock implementation of rate provider.
	/// Throws exceptions for most methods.
	/// </summary>
	public class MockRatesProvider : RatesProvider
	{

	  /// <summary>
	  /// The FX rate.
	  /// </summary>
	  public const double RATE = 1.6d;

	  /// <summary>
	  /// The valuation date.
	  /// </summary>
	  private readonly LocalDate valuationDate;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public MockRatesProvider()
	  {
		this.valuationDate = null;
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  public MockRatesProvider(LocalDate valuationDate)
	  {
		this.valuationDate = valuationDate;
	  }

	  //-------------------------------------------------------------------------
	  public virtual ImmutableSet<Currency> DiscountCurrencies
	  {
		  get
		  {
			throw new System.NotSupportedException();
		  }
	  }

	  public virtual ImmutableSet<IborIndex> IborIndices
	  {
		  get
		  {
			throw new System.NotSupportedException();
		  }
	  }

	  public virtual ImmutableSet<OvernightIndex> OvernightIndices
	  {
		  get
		  {
			throw new System.NotSupportedException();
		  }
	  }

	  public virtual ImmutableSet<PriceIndex> PriceIndices
	  {
		  get
		  {
			throw new System.NotSupportedException();
		  }
	  }

	  public virtual ImmutableSet<Index> TimeSeriesIndices
	  {
		  get
		  {
			throw new System.NotSupportedException();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual T data<T>(MarketDataId<T> key)
	  {
		throw new System.NotSupportedException();
	  }

	  //-------------------------------------------------------------------------
	  public virtual double fxRate(Currency baseCurrency, Currency counterCurrency)
	  {
		return baseCurrency.Equals(counterCurrency) ? 1 : RATE;
	  }

	  //-------------------------------------------------------------------------
	  public virtual DiscountFactors discountFactors(Currency currency)
	  {
		throw new System.NotSupportedException();
	  }

	  //-------------------------------------------------------------------------
	  public virtual FxIndexRates fxIndexRates(FxIndex index)
	  {
		throw new System.NotSupportedException();
	  }

	  //-------------------------------------------------------------------------
	  public virtual FxForwardRates fxForwardRates(CurrencyPair currencyPair)
	  {
		throw new System.NotSupportedException();
	  }

	  //-------------------------------------------------------------------------
	  public virtual IborIndexRates iborIndexRates(IborIndex index)
	  {
		throw new System.NotSupportedException();
	  }

	  //-------------------------------------------------------------------------
	  public virtual OvernightIndexRates overnightIndexRates(OvernightIndex index)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual PriceIndexValues priceIndexValues(PriceIndex index)
	  {
		throw new System.NotSupportedException();
	  }

	  //-------------------------------------------------------------------------
	  public virtual LocalDate ValuationDate
	  {
		  get
		  {
			if (valuationDate == null)
			{
			  throw new System.NotSupportedException();
			}
			return valuationDate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return null;
	  }

	  public virtual LocalDateDoubleTimeSeries timeSeries(Index index)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual ImmutableRatesProvider toImmutableRatesProvider()
	  {
		throw new System.NotSupportedException();
	  }

	}

}
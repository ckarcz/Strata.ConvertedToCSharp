/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;

	/// <summary>
	/// Test implementation of <seealso cref="IborIndexRates"/>.
	/// </summary>
	internal class TestingIborIndexRates : IborIndexRates
	{

	  private readonly IborIndex index;
	  private readonly LocalDate valuationDate;
	  private readonly LocalDateDoubleTimeSeries rates;
	  private readonly LocalDateDoubleTimeSeries fixings;
	  private readonly IborRateSensitivity sens;

	  public TestingIborIndexRates(IborIndex index, LocalDate valuationDate, LocalDateDoubleTimeSeries rates, LocalDateDoubleTimeSeries fixings)
	  {

		this.index = index;
		this.valuationDate = valuationDate;
		this.rates = rates;
		this.fixings = fixings;
		this.sens = null;
	  }

	  public TestingIborIndexRates(IborIndex index, LocalDate valuationDate, LocalDateDoubleTimeSeries rates, LocalDateDoubleTimeSeries fixings, IborRateSensitivity sens)
	  {

		this.index = index;
		this.valuationDate = valuationDate;
		this.rates = rates;
		this.fixings = fixings;
		this.sens = sens;
	  }

	  //-------------------------------------------------------------------------
	  public virtual LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  public virtual IborIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  public virtual LocalDateDoubleTimeSeries Fixings
	  {
		  get
		  {
			return fixings;
		  }
	  }

	  public virtual double rate(IborIndexObservation observation)
	  {
		LocalDate fixingDate = observation.FixingDate;
		if (fixingDate.Equals(valuationDate) && fixings.containsDate(fixingDate))
		{
		  return fixings.get(fixingDate).Value;
		}
		return rates.get(fixingDate).Value;
	  }

	  public virtual PointSensitivityBuilder ratePointSensitivity(IborIndexObservation observation)
	  {
		return sens;
	  }

	  //-------------------------------------------------------------------------
	  public virtual Optional<T> findData<T>(MarketDataName<T> name)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual int ParameterCount
	  {
		  get
		  {
			throw new System.NotSupportedException();
		  }
	  }

	  public virtual double getParameter(int parameterIndex)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual IborIndexRates withParameter(int parameterIndex, double newValue)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual IborIndexRates withPerturbation(ParameterPerturbation perturbation)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual double rateIgnoringFixings(IborIndexObservation observation)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual PointSensitivityBuilder rateIgnoringFixingsPointSensitivity(IborIndexObservation observation)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual CurrencyParameterSensitivities parameterSensitivity(IborRateSensitivity pointSensitivity)
	  {
		throw new System.NotSupportedException();
	  }

	  public virtual CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		throw new System.NotSupportedException();
	  }

	}

}
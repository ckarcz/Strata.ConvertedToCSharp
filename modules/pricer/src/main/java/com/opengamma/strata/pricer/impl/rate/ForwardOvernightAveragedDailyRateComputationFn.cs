/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using OvernightIndexRates = com.opengamma.strata.pricer.rate.OvernightIndexRates;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using OvernightAveragedDailyRateComputation = com.opengamma.strata.product.rate.OvernightAveragedDailyRateComputation;

	/// <summary>
	/// Rate computation implementation for an averaged daily rate for a single Overnight index.
	/// <para>
	/// The rate computation retrieves the rate at each fixing date in the period 
	/// from the <seealso cref="RatesProvider"/> and average them.
	/// </para>
	/// </summary>
	public class ForwardOvernightAveragedDailyRateComputationFn : RateComputationFn<OvernightAveragedDailyRateComputation>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ForwardOvernightAveragedDailyRateComputationFn DEFAULT = new ForwardOvernightAveragedDailyRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardOvernightAveragedDailyRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(OvernightAveragedDailyRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndex index = computation.Index;
		OvernightIndexRates rates = provider.overnightIndexRates(index);
		LocalDate lastFixingDate = computation.EndDate;
		double interestSum = 0d;
		int numberOfDays = 0;
		LocalDate currentFixingDate = computation.StartDate;
		while (!currentFixingDate.isAfter(lastFixingDate))
		{
		  LocalDate referenceFixingDate = computation.FixingCalendar.previousOrSame(currentFixingDate);
		  OvernightIndexObservation indexObs = computation.observeOn(referenceFixingDate);
		  double forwardRate = rates.rate(indexObs);
		  interestSum += forwardRate;
		  numberOfDays++;
		  currentFixingDate = currentFixingDate.plusDays(1);
		}

		return interestSum / numberOfDays;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(OvernightAveragedDailyRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndex index = computation.Index;
		OvernightIndexRates rates = provider.overnightIndexRates(index);
		LocalDate lastFixingDate = computation.EndDate;
		PointSensitivityBuilder pointSensitivityBuilder = PointSensitivityBuilder.none();
		int numberOfDays = 0;
		LocalDate currentFixingDate = computation.StartDate;
		while (!currentFixingDate.isAfter(lastFixingDate))
		{
		  LocalDate referenceFixingDate = computation.FixingCalendar.previousOrSame(currentFixingDate);
		  OvernightIndexObservation indexObs = computation.observeOn(referenceFixingDate);
		  PointSensitivityBuilder forwardRateSensitivity = rates.ratePointSensitivity(indexObs);
		  pointSensitivityBuilder = pointSensitivityBuilder.combinedWith(forwardRateSensitivity);
		  numberOfDays++;
		  currentFixingDate = currentFixingDate.plusDays(1);
		}

		return pointSensitivityBuilder.multipliedBy(1d / numberOfDays);
	  }

	  public virtual double explainRate(OvernightAveragedDailyRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
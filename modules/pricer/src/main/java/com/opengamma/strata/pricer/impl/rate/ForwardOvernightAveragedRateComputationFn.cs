/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using OvernightAveragedRateComputation = com.opengamma.strata.product.rate.OvernightAveragedRateComputation;

	/// <summary>
	/// Rate computation implementation for a rate based on a single overnight index that is arithmetically averaged.
	/// <para>
	/// The rate computation retrieves the rate at each fixing date in the period 
	/// from the <seealso cref="RatesProvider"/> and average them.
	/// </para>
	/// </summary>
	public class ForwardOvernightAveragedRateComputationFn : RateComputationFn<OvernightAveragedRateComputation>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ForwardOvernightAveragedRateComputationFn DEFAULT = new ForwardOvernightAveragedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardOvernightAveragedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(OvernightAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndex index = computation.Index;
		OvernightIndexRates rates = provider.overnightIndexRates(index);
		LocalDate lastNonCutoffFixing = computation.EndDate;
		int cutoffOffset = computation.RateCutOffDays > 1 ? computation.RateCutOffDays : 1;
		double accumulatedInterest = 0.0d;
		double accrualFactorTotal = 0.0d;
		// Cut-off period. Starting from the end as the cutoff period is defined as a lag from the end.
		// When the fixing period end-date is not a good business day in the index calendar, 
		// the last fixing end date will be after the fixing end-date.
		double cutoffAccrualFactor = 0.0;
		OvernightIndexObservation lastIndexObs = null;
		// cutoffOffset >= 1, so loop always runs at least once
		for (int i = 0; i < cutoffOffset; i++)
		{
		  lastNonCutoffFixing = computation.FixingCalendar.previous(lastNonCutoffFixing);
		  lastIndexObs = computation.observeOn(lastNonCutoffFixing);
		  accrualFactorTotal += lastIndexObs.YearFraction;
		  cutoffAccrualFactor += lastIndexObs.YearFraction;
		}
		double forwardRateCutOff = rates.rate(lastIndexObs);
		accumulatedInterest += cutoffAccrualFactor * forwardRateCutOff;
		LocalDate currentFixingNonCutoff = computation.StartDate;
		while (currentFixingNonCutoff.isBefore(lastNonCutoffFixing))
		{
		  // All dates involved in the period are computed. Potentially slow.
		  // The fixing periods are added as long as their start date is (strictly) before the no cutoff period end-date.
		  OvernightIndexObservation indexObs = computation.observeOn(currentFixingNonCutoff);
		  double forwardRate = rates.rate(indexObs);
		  accrualFactorTotal += indexObs.YearFraction;
		  accumulatedInterest += indexObs.YearFraction * forwardRate;
		  currentFixingNonCutoff = computation.FixingCalendar.next(currentFixingNonCutoff);
		}
		// final rate
		return accumulatedInterest / accrualFactorTotal;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(OvernightAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndex index = computation.Index;
		OvernightIndexRates rates = provider.overnightIndexRates(index);
		LocalDate lastNonCutoffFixing = computation.EndDate;
		int cutoffOffset = computation.RateCutOffDays > 1 ? computation.RateCutOffDays : 1;
		double accrualFactorTotal = 0.0d;
		// Cut-off period. Starting from the end as the cutoff period is defined as a lag from the end.
		// When the fixing period end-date is not a good business day in the index calendar, 
		// the last fixing end date will be after the fixing end-date.
		double cutoffAccrualFactor = 0.0;
		OvernightIndexObservation lastIndexObs = null;
		// cutoffOffset >= 1, so loop always runs at least once
		for (int i = 0; i < cutoffOffset; i++)
		{
		  lastNonCutoffFixing = computation.FixingCalendar.previous(lastNonCutoffFixing);
		  lastIndexObs = computation.observeOn(lastNonCutoffFixing);
		  accrualFactorTotal += lastIndexObs.YearFraction;
		  cutoffAccrualFactor += lastIndexObs.YearFraction;
		}
		PointSensitivityBuilder combinedPointSensitivityBuilder = rates.ratePointSensitivity(lastIndexObs).multipliedBy(cutoffAccrualFactor);

		LocalDate currentFixingNonCutoff = computation.StartDate;
		while (currentFixingNonCutoff.isBefore(lastNonCutoffFixing))
		{
		  // All dates involved in the period are computed. Potentially slow.
		  // The fixing periods are added as long as their start date is (strictly) before the no cutoff period end-date.
		  OvernightIndexObservation indexObs = computation.observeOn(currentFixingNonCutoff);
		  PointSensitivityBuilder forwardRateSensitivity = rates.ratePointSensitivity(indexObs).multipliedBy(indexObs.YearFraction);
		  combinedPointSensitivityBuilder = combinedPointSensitivityBuilder.combinedWith(forwardRateSensitivity);
		  accrualFactorTotal += indexObs.YearFraction;
		  currentFixingNonCutoff = computation.FixingCalendar.next(currentFixingNonCutoff);
		}
		return combinedPointSensitivityBuilder.multipliedBy(1.0 / accrualFactorTotal);
	  }

	  public virtual double explainRate(OvernightAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
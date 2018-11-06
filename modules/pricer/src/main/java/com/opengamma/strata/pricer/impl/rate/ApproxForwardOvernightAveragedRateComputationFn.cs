using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
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
	/// The rate already fixed are retrieved from the time series of the <seealso cref="RatesProvider"/>.
	/// The rate in the future and not in the cut-off period are computed by approximation.
	/// The rate in the cut-off period (already fixed or forward) are added.
	/// </para>
	/// <para>
	/// Reference: Overnight Indexes related products, OpenGamma documentation 29, version 1.1, March 2013.
	/// </para>
	/// </summary>
	public class ApproxForwardOvernightAveragedRateComputationFn : RateComputationFn<OvernightAveragedRateComputation>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ApproxForwardOvernightAveragedRateComputationFn DEFAULT = new ApproxForwardOvernightAveragedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ApproxForwardOvernightAveragedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(OvernightAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndexRates rates = provider.overnightIndexRates(computation.Index);
		LocalDate valuationDate = rates.ValuationDate;
		LocalDate startFixingDate = computation.StartDate;
		LocalDate startPublicationDate = computation.calculatePublicationFromFixing(startFixingDate);
		// No fixing to analyze. Go directly to approximation and cut-off.
		if (valuationDate.isBefore(startPublicationDate))
		{
		  return rateForward(computation, rates);
		}
		ObservationDetails details = new ObservationDetails(computation, rates);
		return details.calculateRate();
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(OvernightAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndexRates rates = provider.overnightIndexRates(computation.Index);
		LocalDate valuationDate = rates.ValuationDate;
		LocalDate startFixingDate = computation.StartDate;
		LocalDate startPublicationDate = computation.calculatePublicationFromFixing(startFixingDate);
		// No fixing to analyze. Go directly to approximation and cut-off.
		if (valuationDate.isBefore(startPublicationDate))
		{
		  return rateForwardSensitivity(computation, rates);
		}
		ObservationDetails details = new ObservationDetails(computation, rates);
		return details.calculateRateSensitivity();
	  }

	  public virtual double explainRate(OvernightAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	  //-------------------------------------------------------------------------
	  // Compute the approximated rate in the case where the whole period is forward.
	  // There is no need to compute overnight periods, except for the cut-off period.
	  private double rateForward(OvernightAveragedRateComputation computation, OvernightIndexRates rates)
	  {
		OvernightIndex index = computation.Index;
		HolidayCalendar calendar = computation.FixingCalendar;
		LocalDate startFixingDate = computation.StartDate;
		LocalDate endFixingDateP1 = computation.EndDate;
		LocalDate endFixingDate = calendar.previous(endFixingDateP1);
		LocalDate onRateEndDate = computation.calculateMaturityFromFixing(endFixingDate);
		LocalDate onRateStartDate = computation.calculateEffectiveFromFixing(startFixingDate);
		LocalDate onRateNoCutOffEndDate = onRateEndDate;
		int cutoffOffset = computation.RateCutOffDays > 1 ? computation.RateCutOffDays : 1;
		double accumulatedInterest = 0.0d;
		double accrualFactorTotal = index.DayCount.yearFraction(onRateStartDate, onRateEndDate);
		if (cutoffOffset > 1)
		{ // Cut-off period
		  LocalDate currentFixingDate = endFixingDate;
		  OvernightIndexObservation lastIndexObs = null;
		  double cutOffAccrualFactorTotal = 0d;
		  for (int i = 1; i < cutoffOffset; i++)
		  {
			currentFixingDate = calendar.previous(currentFixingDate);
			lastIndexObs = computation.observeOn(currentFixingDate);
			onRateNoCutOffEndDate = lastIndexObs.MaturityDate;
			cutOffAccrualFactorTotal += lastIndexObs.YearFraction;
		  }
		  double forwardRateCutOff = rates.rate(lastIndexObs);
		  accumulatedInterest += cutOffAccrualFactorTotal * forwardRateCutOff;
		}
		// Approximated part
		accumulatedInterest += approximatedInterest(computation.observeOn(onRateStartDate), onRateNoCutOffEndDate, rates);
		// final rate
		return accumulatedInterest / accrualFactorTotal;
	  }

	  private PointSensitivityBuilder rateForwardSensitivity(OvernightAveragedRateComputation computation, OvernightIndexRates rates)
	  {

		OvernightIndex index = computation.Index;
		HolidayCalendar calendar = computation.FixingCalendar;
		LocalDate startFixingDate = computation.StartDate;
		LocalDate endFixingDateP1 = computation.EndDate;
		LocalDate endFixingDate = calendar.previous(endFixingDateP1);
		LocalDate onRateEndDate = computation.calculateMaturityFromFixing(endFixingDate);
		LocalDate onRateStartDate = computation.calculateEffectiveFromFixing(startFixingDate);
		LocalDate lastNonCutOffMatDate = onRateEndDate;
		int cutoffOffset = computation.RateCutOffDays > 1 ? computation.RateCutOffDays : 1;
		PointSensitivityBuilder combinedPointSensitivityBuilder = PointSensitivityBuilder.none();
		double accrualFactorTotal = index.DayCount.yearFraction(onRateStartDate, onRateEndDate);
		if (cutoffOffset > 1)
		{ // Cut-off period
		  IList<double> noCutOffAccrualFactorList = new List<double>();
		  LocalDate currentFixingDate = endFixingDateP1;
		  LocalDate cutOffEffectiveDate;
		  for (int i = 0; i < cutoffOffset; i++)
		  {
			currentFixingDate = calendar.previous(currentFixingDate);
			cutOffEffectiveDate = computation.calculateEffectiveFromFixing(currentFixingDate);
			lastNonCutOffMatDate = computation.calculateMaturityFromEffective(cutOffEffectiveDate);
			double accrualFactor = index.DayCount.yearFraction(cutOffEffectiveDate, lastNonCutOffMatDate);
			noCutOffAccrualFactorList.Add(accrualFactor);
		  }
		  OvernightIndexObservation lastIndexObs = computation.observeOn(currentFixingDate);
		  PointSensitivityBuilder forwardRateCutOffSensitivity = rates.ratePointSensitivity(lastIndexObs);
		  double totalAccrualFactor = 0.0;
		  for (int i = 0; i < cutoffOffset - 1; i++)
		  {
			totalAccrualFactor += noCutOffAccrualFactorList[i];
		  }
		  forwardRateCutOffSensitivity = forwardRateCutOffSensitivity.multipliedBy(totalAccrualFactor);
		  combinedPointSensitivityBuilder = combinedPointSensitivityBuilder.combinedWith(forwardRateCutOffSensitivity);
		}
		// Approximated part
		OvernightIndexObservation indexObs = computation.observeOn(onRateStartDate);
		PointSensitivityBuilder approximatedInterestAndSensitivity = approximatedInterestSensitivity(indexObs, lastNonCutOffMatDate, rates);
		combinedPointSensitivityBuilder = combinedPointSensitivityBuilder.combinedWith(approximatedInterestAndSensitivity);
		combinedPointSensitivityBuilder = combinedPointSensitivityBuilder.multipliedBy(1.0 / accrualFactorTotal);
		// final rate
		return combinedPointSensitivityBuilder;
	  }

	  // Compute the accrued interest on a given period by approximation
	  private static double approximatedInterest(OvernightIndexObservation observation, LocalDate endDate, OvernightIndexRates rates)
	  {

		DayCount dayCount = observation.Index.DayCount;
		double remainingFixingAccrualFactor = dayCount.yearFraction(observation.EffectiveDate, endDate);
		double forwardRate = rates.periodRate(observation, endDate);
		return Math.Log(1.0 + forwardRate * remainingFixingAccrualFactor);
	  }

	  // Compute the accrued interest sensitivity on a given period by approximation
	  private static PointSensitivityBuilder approximatedInterestSensitivity(OvernightIndexObservation observation, LocalDate endDate, OvernightIndexRates rates)
	  {

		DayCount dayCount = observation.Index.DayCount;
		double remainingFixingAccrualFactor = dayCount.yearFraction(observation.EffectiveDate, endDate);
		double forwardRate = rates.periodRate(observation, endDate);
		PointSensitivityBuilder forwardRateSensitivity = rates.periodRatePointSensitivity(observation, endDate);
		double rateExp = 1.0 + forwardRate * remainingFixingAccrualFactor;
		forwardRateSensitivity = forwardRateSensitivity.multipliedBy(remainingFixingAccrualFactor / rateExp);
		return forwardRateSensitivity;
	  }

	  //-------------------------------------------------------------------------
	  // Internal class representing all the details related to the computation
	  private sealed class ObservationDetails
	  {
		// The list below are created in the constructor and never modified after.
		internal readonly OvernightIndexRates rates;
		internal readonly IList<OvernightIndexObservation> observations; // one observation per fixing date
		internal int fixedPeriod; // Note this is mutable
		internal readonly double accrualFactorTotal;
		internal readonly int nbPeriods;
		internal readonly OvernightIndex index;
		internal readonly int cutoffOffset;

		// Construct all the details related to the observation: fixing dates, publication dates, start and end dates, 
		// accrual factors, number of already fixed ON rates.
		internal ObservationDetails(OvernightAveragedRateComputation computation, OvernightIndexRates rates)
		{
		  this.index = computation.Index;
		  this.rates = rates;
		  LocalDate startFixingDate = computation.StartDate;
		  LocalDate endFixingDateP1 = computation.EndDate;
		  this.cutoffOffset = computation.RateCutOffDays > 1 ? computation.RateCutOffDays : 1;
		  double accrualFactorAccumulated = 0d;
		  // find all observations in the period
		  LocalDate currentFixing = startFixingDate;
		  IList<OvernightIndexObservation> indexObsList = new List<OvernightIndexObservation>();
		  while (currentFixing.isBefore(endFixingDateP1))
		  {
			OvernightIndexObservation indexObs = computation.observeOn(currentFixing);
			indexObsList.Add(indexObs);
			currentFixing = computation.FixingCalendar.next(currentFixing);
			accrualFactorAccumulated += indexObs.YearFraction;
		  }
		  this.accrualFactorTotal = accrualFactorAccumulated;
		  this.nbPeriods = indexObsList.Count;
		  // dealing with cut-off by replacing observations with ones where fixing/publication locked
		  // within cut-off, the effective/maturity dates of each observation have to stay the same
		  for (int i = 0; i < cutoffOffset - 1; i++)
		  {
			OvernightIndexObservation fixingIndexObs = indexObsList[nbPeriods - cutoffOffset];
			OvernightIndexObservation cutoffIndexObs = indexObsList[nbPeriods - 1 - i];
			OvernightIndexObservation updatedIndexObs = cutoffIndexObs.toBuilder().fixingDate(fixingIndexObs.FixingDate).publicationDate(fixingIndexObs.PublicationDate).build();
			indexObsList[nbPeriods - 1 - i] = updatedIndexObs;
		  }
		  this.observations = Collections.unmodifiableList(indexObsList);
		}

		// Accumulated rate - publication strictly before valuation date: try accessing fixing time-series.
		// fixedPeriod is altered by this method.
		internal double pastAccumulation()
		{
		  double accumulatedInterest = 0.0d;
		  LocalDateDoubleTimeSeries indexFixingDateSeries = rates.Fixings;
		  while ((fixedPeriod < nbPeriods) && rates.ValuationDate.isAfter(observations[fixedPeriod].PublicationDate))
		  {
			OvernightIndexObservation obs = observations[fixedPeriod];
			accumulatedInterest += obs.YearFraction * checkedFixing(obs.FixingDate, indexFixingDateSeries, index);
			fixedPeriod++;
		  }
		  return accumulatedInterest;
		}

		// Accumulated rate - publication on valuation: Check if a fixing is available on current date.
		// fixedPeriod is altered by this method.
		internal double valuationDateAccumulation()
		{
		  double accumulatedInterest = 0.0d;
		  LocalDateDoubleTimeSeries indexFixingDateSeries = rates.Fixings;
		  bool ratePresent = true;
		  while (ratePresent && fixedPeriod < nbPeriods && rates.ValuationDate.isEqual(observations[fixedPeriod].PublicationDate))
		  {
			OvernightIndexObservation obs = observations[fixedPeriod];
			double? fixedRate = indexFixingDateSeries.get(obs.FixingDate);
			if (fixedRate.HasValue)
			{
			  accumulatedInterest += obs.YearFraction * fixedRate.Value;
			  fixedPeriod++;
			}
			else
			{
			  ratePresent = false;
			}
		  }
		  return accumulatedInterest;
		}

		//  Accumulated rate - approximated forward rates if not all fixed and not part of cutoff
		internal double approximatedForwardAccumulation()
		{
		  int nbPeriodNotCutOff = nbPeriods - cutoffOffset + 1;
		  if (fixedPeriod < nbPeriodNotCutOff)
		  {
			LocalDate endDateApprox = observations[nbPeriodNotCutOff - 1].MaturityDate;
			return approximatedInterest(observations[fixedPeriod], endDateApprox, rates);
		  }
		  return 0.0d;
		}

		//  Accumulated rate sensitivity - approximated forward rates if not all fixed and not part of cutoff
		internal PointSensitivityBuilder approximatedForwardAccumulationSensitivity()
		{
		  int nbPeriodNotCutOff = nbPeriods - cutoffOffset + 1;
		  if (fixedPeriod < nbPeriodNotCutOff)
		  {
			LocalDate endDateApprox = observations[nbPeriodNotCutOff - 1].MaturityDate;
			return approximatedInterestSensitivity(observations[fixedPeriod], endDateApprox, rates);
		  }
		  return PointSensitivityBuilder.none();
		}

		// Accumulated rate - cutoff part if not fixed
		internal double cutOffAccumulation()
		{
		  double accumulatedInterest = 0.0d;
		  int nbPeriodNotCutOff = nbPeriods - cutoffOffset + 1;
		  for (int i = Math.Max(fixedPeriod, nbPeriodNotCutOff); i < nbPeriods; i++)
		  {
			OvernightIndexObservation obs = observations[i];
			double forwardRate = rates.rate(obs);
			accumulatedInterest += obs.YearFraction * forwardRate;
		  }
		  return accumulatedInterest;
		}

		// Accumulated rate sensitivity - cutoff part if not fixed
		internal PointSensitivityBuilder cutOffAccumulationSensitivity()
		{
		  PointSensitivityBuilder combinedPointSensitivityBuilder = PointSensitivityBuilder.none();
		  int nbPeriodNotCutOff = nbPeriods - cutoffOffset + 1;
		  for (int i = Math.Max(fixedPeriod, nbPeriodNotCutOff); i < nbPeriods; i++)
		  {
			OvernightIndexObservation obs = observations[i];
			PointSensitivityBuilder forwardRateSensitivity = rates.ratePointSensitivity(obs).multipliedBy(obs.YearFraction);
			combinedPointSensitivityBuilder = combinedPointSensitivityBuilder.combinedWith(forwardRateSensitivity);
		  }
		  return combinedPointSensitivityBuilder;
		}

		// Calculate the total rate.
		internal double calculateRate()
		{
		  return (pastAccumulation() + valuationDateAccumulation() + approximatedForwardAccumulation() + cutOffAccumulation()) / accrualFactorTotal;
		}

		// Calculate the total rate sensitivity.
		internal PointSensitivityBuilder calculateRateSensitivity()
		{
		  // call these methods to ensure mutable fixedPeriod variable is updated
		  pastAccumulation();
		  valuationDateAccumulation();
		  // calculate sensitivity
		  PointSensitivityBuilder combinedPointSensitivity = approximatedForwardAccumulationSensitivity();
		  PointSensitivityBuilder cutOffAccumulationSensitivity = this.cutOffAccumulationSensitivity();
		  combinedPointSensitivity = combinedPointSensitivity.combinedWith(cutOffAccumulationSensitivity);
		  combinedPointSensitivity = combinedPointSensitivity.multipliedBy(1.0d / accrualFactorTotal);
		  return combinedPointSensitivity;
		}

		// Check that the fixing is present. Throws an exception if not and return the rate as double.
		internal static double checkedFixing(LocalDate currentFixingTs, LocalDateDoubleTimeSeries indexFixingDateSeries, OvernightIndex index)
		{

		  double? fixedRate = indexFixingDateSeries.get(currentFixingTs);
		  return fixedRate.orElseThrow(() => new PricingException("Could not get fixing value of index " + index.Name + " for date " + currentFixingTs));
		}
	  }

	}

}
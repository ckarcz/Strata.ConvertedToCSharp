using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using ObjDoublePair = com.opengamma.strata.collect.tuple.ObjDoublePair;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using OvernightIndexRates = com.opengamma.strata.pricer.rate.OvernightIndexRates;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using OvernightCompoundedRateComputation = com.opengamma.strata.product.rate.OvernightCompoundedRateComputation;

	/// <summary>
	/// Rate computation implementation for a rate based on a single overnight index that is compounded.
	/// <para>
	/// Rates that are already fixed are retrieved from the time series of the <seealso cref="RatesProvider"/>.
	/// Rates that are in the future and not in the cut-off period are computed as unique forward rate in the full future period.
	/// Rates that are in the cut-off period (already fixed or forward) are compounded.
	/// </para>
	/// </summary>
	public class ForwardOvernightCompoundedRateComputationFn : RateComputationFn<OvernightCompoundedRateComputation>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ForwardOvernightCompoundedRateComputationFn DEFAULT = new ForwardOvernightCompoundedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardOvernightCompoundedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(OvernightCompoundedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndexRates rates = provider.overnightIndexRates(computation.Index);
		ObservationDetails details = new ObservationDetails(computation, rates);
		return details.calculateRate();
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(OvernightCompoundedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		OvernightIndexRates rates = provider.overnightIndexRates(computation.Index);
		ObservationDetails details = new ObservationDetails(computation, rates);
		return details.calculateRateSensitivity();
	  }

	  public virtual double explainRate(OvernightCompoundedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	  //-------------------------------------------------------------------------
	  // Internal class. Observation details stored in a separate class to clarify the construction.
	  private sealed class ObservationDetails
	  {

		internal readonly OvernightCompoundedRateComputation computation;
		internal readonly OvernightIndexRates rates;
		internal readonly LocalDateDoubleTimeSeries indexFixingDateSeries;
		internal readonly DayCount dayCount;
		internal readonly int cutoffOffset;
		internal readonly LocalDate firstFixing; // The date of the first fixing
		internal readonly LocalDate lastFixingP1; // The date after the last fixing
		internal readonly LocalDate lastFixing; // The date of the last fixing
		internal readonly LocalDate lastFixingNonCutoff; // The last fixing not in the cutoff period.
		internal readonly double accrualFactorTotal; // Total accrual factor
		internal readonly double[] accrualFactorCutoff; // Accrual factors for the sub-periods using the cutoff rate.
		internal LocalDate nextFixing; // Running variable through the different methods: next fixing date to be analyzed

		internal ObservationDetails(OvernightCompoundedRateComputation computation, OvernightIndexRates rates)
		{
		  this.computation = computation;
		  this.rates = rates;
		  this.indexFixingDateSeries = rates.Fixings;
		  this.dayCount = computation.Index.DayCount;
		  // Details of the cutoff period
		  this.firstFixing = computation.StartDate;
		  this.lastFixingP1 = computation.EndDate;
		  this.lastFixing = computation.FixingCalendar.previous(lastFixingP1);
		  this.cutoffOffset = Math.Max(computation.RateCutOffDays, 1);
		  this.accrualFactorCutoff = new double[cutoffOffset - 1];
		  LocalDate currentFixing = lastFixing;
		  for (int i = 0; i < cutoffOffset - 1; i++)
		  {
			currentFixing = computation.FixingCalendar.previous(currentFixing);
			LocalDate effectiveDate = computation.calculateEffectiveFromFixing(currentFixing);
			LocalDate maturityDate = computation.calculateMaturityFromEffective(effectiveDate);
			accrualFactorCutoff[i] = dayCount.yearFraction(effectiveDate, maturityDate);
		  }
		  this.lastFixingNonCutoff = currentFixing;
		  LocalDate startUnderlyingPeriod = computation.calculateEffectiveFromFixing(firstFixing);
		  LocalDate endUnderlyingPeriod = computation.calculateMaturityFromFixing(lastFixing);
		  this.accrualFactorTotal = dayCount.yearFraction(startUnderlyingPeriod, endUnderlyingPeriod);
		}

		// Composition - publication strictly before valuation date: try accessing fixing time-series
		internal double pastCompositionFactor()
		{
		  double compositionFactor = 1.0d;
		  LocalDate currentFixing = firstFixing;
		  LocalDate currentPublication = computation.calculatePublicationFromFixing(currentFixing);
		  while ((currentFixing.isBefore(lastFixingNonCutoff)) && rates.ValuationDate.isAfter(currentPublication))
		  { // publication before valuation
			LocalDate effectiveDate = computation.calculateEffectiveFromFixing(currentFixing);
			LocalDate maturityDate = computation.calculateMaturityFromEffective(effectiveDate);
			double accrualFactor = dayCount.yearFraction(effectiveDate, maturityDate);
			compositionFactor *= 1.0d + accrualFactor * checkedFixing(currentFixing, indexFixingDateSeries, computation.Index);
			currentFixing = computation.FixingCalendar.next(currentFixing);
			currentPublication = computation.calculatePublicationFromFixing(currentFixing);
		  }
		  if (currentFixing.Equals(lastFixingNonCutoff) && rates.ValuationDate.isAfter(currentPublication))
		  { // publication before valuation
			double rate = checkedFixing(currentFixing, indexFixingDateSeries, computation.Index);
			LocalDate effectiveDate = computation.calculateEffectiveFromFixing(currentFixing);
			LocalDate maturityDate = computation.calculateMaturityFromEffective(effectiveDate);
			double accrualFactor = dayCount.yearFraction(effectiveDate, maturityDate);
			compositionFactor *= 1.0d + accrualFactor * rate;
			for (int i = 0; i < cutoffOffset - 1; i++)
			{
			  compositionFactor *= 1.0d + accrualFactorCutoff[i] * rate;
			}
			currentFixing = computation.FixingCalendar.next(currentFixing);
		  }
		  nextFixing = currentFixing;
		  return compositionFactor;
		}

		// Composition - publication on valuation date: Check if a fixing is available on current date
		internal double valuationCompositionFactor()
		{
		  LocalDate currentFixing = nextFixing;
		  LocalDate currentPublication = computation.calculatePublicationFromFixing(currentFixing);
		  if (rates.ValuationDate.Equals(currentPublication) && !(currentFixing.isAfter(lastFixingNonCutoff)))
		  { // If currentFixing > lastFixingNonCutoff, everything fixed
			double? fixedRate = indexFixingDateSeries.get(currentFixing);
			if (fixedRate.HasValue)
			{
			  nextFixing = computation.FixingCalendar.next(nextFixing);
			  LocalDate effectiveDate = computation.calculateEffectiveFromFixing(currentFixing);
			  LocalDate maturityDate = computation.calculateMaturityFromEffective(effectiveDate);
			  double accrualFactor = dayCount.yearFraction(effectiveDate, maturityDate);
			  if (currentFixing.isBefore(lastFixingNonCutoff))
			  {
				return 1.0d + accrualFactor * fixedRate.Value;
			  }
			  double compositionFactor = 1.0d + accrualFactor * fixedRate.Value;
			  for (int i = 0; i < cutoffOffset - 1; i++)
			  {
				compositionFactor *= 1.0d + accrualFactorCutoff[i] * fixedRate.Value;
			  }
			  return compositionFactor;
			}
		  }
		  return 1.0d;
		}

		// Composition - forward part in non-cutoff period; past/valuation date case dealt with in previous methods
		internal double compositionFactorNonCutoff()
		{
		  if (!nextFixing.isAfter(lastFixingNonCutoff))
		  {
			OvernightIndexObservation obs = computation.observeOn(nextFixing);
			LocalDate startDate = obs.EffectiveDate;
			LocalDate endDate = computation.calculateMaturityFromFixing(lastFixingNonCutoff);
			double accrualFactor = dayCount.yearFraction(startDate, endDate);
			double rate = rates.periodRate(obs, endDate);
			return 1.0d + accrualFactor * rate;
		  }
		  return 1.0d;
		}

		// Composition - forward part in non-cutoff period; past/valuation date case dealt with in previous methods
		internal ObjDoublePair<PointSensitivityBuilder> compositionFactorAndSensitivityNonCutoff()
		{
		  if (!nextFixing.isAfter(lastFixingNonCutoff))
		  {
			OvernightIndexObservation obs = computation.observeOn(nextFixing);
			LocalDate startDate = obs.EffectiveDate;
			LocalDate endDate = computation.calculateMaturityFromFixing(lastFixingNonCutoff);
			double accrualFactor = dayCount.yearFraction(startDate, endDate);
			double rate = rates.periodRate(obs, endDate);
			PointSensitivityBuilder rateSensitivity = rates.periodRatePointSensitivity(obs, endDate);
			rateSensitivity = rateSensitivity.multipliedBy(accrualFactor);
			return ObjDoublePair.of(rateSensitivity, 1.0d + accrualFactor * rate);
		  }
		  return ObjDoublePair.of(PointSensitivityBuilder.none(), 1.0d);
		}

		// Composition - forward part in the cutoff period; past/valuation date case dealt with in previous methods
		internal double compositionFactorCutoff()
		{
		  if (!nextFixing.isAfter(lastFixingNonCutoff))
		  {
			OvernightIndexObservation obs = computation.observeOn(lastFixingNonCutoff);
			double rate = rates.rate(obs);
			double compositionFactor = 1.0d;
			for (int i = 0; i < cutoffOffset - 1; i++)
			{
			  compositionFactor *= 1.0d + accrualFactorCutoff[i] * rate;
			}
			return compositionFactor;
		  }
		  return 1.0d;
		}

		// Composition - forward part in the cutoff period; past/valuation date case dealt with in previous methods
		internal ObjDoublePair<PointSensitivityBuilder> compositionFactorAndSensitivityCutoff()
		{
		  OvernightIndexObservation obs = computation.observeOn(lastFixingNonCutoff);
		  if (!nextFixing.isAfter(lastFixingNonCutoff))
		  {
			double rate = rates.rate(obs);
			double compositionFactor = 1.0d;
			double compositionFactorDerivative = 0.0;
			for (int i = 0; i < cutoffOffset - 1; i++)
			{
			  compositionFactor *= 1.0d + accrualFactorCutoff[i] * rate;
			  compositionFactorDerivative += accrualFactorCutoff[i] / (1.0d + accrualFactorCutoff[i] * rate);
			}
			compositionFactorDerivative *= compositionFactor;
			PointSensitivityBuilder rateSensitivity = cutoffOffset <= 1 ? PointSensitivityBuilder.none() : rates.ratePointSensitivity(obs);
			rateSensitivity = rateSensitivity.multipliedBy(compositionFactorDerivative);
			return ObjDoublePair.of(rateSensitivity, compositionFactor);
		  }
		  return ObjDoublePair.of(PointSensitivityBuilder.none(), 1.0d);
		}

		// Calculate the total rate
		internal double calculateRate()
		{
		  return (pastCompositionFactor() * valuationCompositionFactor() * compositionFactorNonCutoff() * compositionFactorCutoff() - 1.0d) / accrualFactorTotal;
		}

		// Calculate the total rate sensitivity
		internal PointSensitivityBuilder calculateRateSensitivity()
		{
		  double factor = pastCompositionFactor() * valuationCompositionFactor() / accrualFactorTotal;
		  ObjDoublePair<PointSensitivityBuilder> compositionFactorAndSensitivityNonCutoff = this.compositionFactorAndSensitivityNonCutoff();
		  ObjDoublePair<PointSensitivityBuilder> compositionFactorAndSensitivityCutoff = this.compositionFactorAndSensitivityCutoff();

		  PointSensitivityBuilder combinedPointSensitivity = compositionFactorAndSensitivityNonCutoff.First.multipliedBy(compositionFactorAndSensitivityCutoff.Second * factor);
		  combinedPointSensitivity = combinedPointSensitivity.combinedWith(compositionFactorAndSensitivityCutoff.First.multipliedBy(compositionFactorAndSensitivityNonCutoff.Second * factor));

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
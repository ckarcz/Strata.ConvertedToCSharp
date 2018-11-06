/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using PriceIndexValues = com.opengamma.strata.pricer.rate.PriceIndexValues;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using InflationInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationInterpolatedRateComputation;

	/// <summary>
	/// Rate computation implementation for rate based on the weighted average of fixings 
	/// of a single price index.
	/// <para>
	/// The rate computed by this instance is based on four observations of the index,
	/// two relative to the accrual start date and two relative to the accrual end date.
	/// The start index is the weighted average of the index values associated with the first two reference dates, 
	/// and the end index is derived from the index values on the last two reference dates.
	/// Then the pay-off for a unit notional is {@code (IndexEnd / IndexStart - 1)}. 
	/// </para>
	/// </summary>
	public class ForwardInflationInterpolatedRateComputationFn : RateComputationFn<InflationInterpolatedRateComputation>
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly ForwardInflationInterpolatedRateComputationFn DEFAULT = new ForwardInflationInterpolatedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardInflationInterpolatedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(InflationInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double indexStart = interpolateStart(computation, values);
		double indexEnd = interpolateEnd(computation, values);
		return indexEnd / indexStart - 1d;
	  }

	  // interpolate the computations at the start
	  private double interpolateStart(InflationInterpolatedRateComputation computation, PriceIndexValues values)
	  {
		double weight = computation.Weight;
		double indexValue1 = values.value(computation.StartObservation);
		double indexValue2 = values.value(computation.StartSecondObservation);
		return weight * indexValue1 + (1d - weight) * indexValue2;
	  }

	  // interpolate the observations at the end
	  private double interpolateEnd(InflationInterpolatedRateComputation computation, PriceIndexValues values)
	  {
		double weight = computation.Weight;
		double indexValue1 = values.value(computation.EndObservation);
		double indexValue2 = values.value(computation.EndSecondObservation);
		return weight * indexValue1 + (1d - weight) * indexValue2;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(InflationInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double indexStart = interpolateStart(computation, values);
		double indexEnd = interpolateEnd(computation, values);
		double indexStartInv = 1d / indexStart;
		PointSensitivityBuilder sensi1 = startSensitivity(computation, values).multipliedBy(-indexEnd * indexStartInv * indexStartInv);
		PointSensitivityBuilder sensi2 = endSensitivity(computation, values).multipliedBy(indexStartInv);
		return sensi1.combinedWith(sensi2);
	  }

	  // interpolate the observations at the start
	  private PointSensitivityBuilder startSensitivity(InflationInterpolatedRateComputation computation, PriceIndexValues values)
	  {
		double weight = computation.Weight;
		PointSensitivityBuilder sensi1 = values.valuePointSensitivity(computation.StartObservation).multipliedBy(weight);
		PointSensitivityBuilder sensi2 = values.valuePointSensitivity(computation.StartSecondObservation).multipliedBy(1d - weight);
		return sensi1.combinedWith(sensi2);
	  }

	  // interpolate the observations at the end
	  private PointSensitivityBuilder endSensitivity(InflationInterpolatedRateComputation computation, PriceIndexValues values)
	  {
		double weight = computation.Weight;
		PointSensitivityBuilder sensi1 = values.valuePointSensitivity(computation.EndObservation).multipliedBy(weight);
		PointSensitivityBuilder sensi2 = values.valuePointSensitivity(computation.EndSecondObservation).multipliedBy(1d - weight);
		return sensi1.combinedWith(sensi2);
	  }

	  public virtual double explainRate(InflationInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double w1 = computation.Weight;
		double w2 = 1d - w1;
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.StartObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, values.value(computation.StartObservation)).put(ExplainKey.WEIGHT, w1));
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.StartSecondObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, values.value(computation.StartSecondObservation)).put(ExplainKey.WEIGHT, w2));
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.EndObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, values.value(computation.EndObservation)).put(ExplainKey.WEIGHT, w1));
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.EndSecondObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, values.value(computation.EndSecondObservation)).put(ExplainKey.WEIGHT, w2));
		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
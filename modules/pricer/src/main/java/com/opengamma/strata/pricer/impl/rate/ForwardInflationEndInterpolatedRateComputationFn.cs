/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;

	/// <summary>
	/// Rate computation implementation for rate based on the weighted average of fixings 
	/// of a single price index.
	/// <para>
	/// The rate computed by this instance is based on fixed start index value
	/// and two observations relative to the end date  of the period.
	/// The start index is given by {@code InflationEndInterpolatedRateComputation}.
	/// The end index is the weighted average of the index values associated with the two reference dates.
	/// Then the pay-off for a unit notional is {@code IndexEnd / IndexStart}. 
	/// </para>
	/// </summary>
	public class ForwardInflationEndInterpolatedRateComputationFn : RateComputationFn<InflationEndInterpolatedRateComputation>
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly ForwardInflationEndInterpolatedRateComputationFn DEFAULT = new ForwardInflationEndInterpolatedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardInflationEndInterpolatedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(InflationEndInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double indexStart = computation.StartIndexValue;
		double indexEnd = interpolateEnd(computation, values);
		return indexEnd / indexStart - 1d;
	  }

	  // interpolate the computations at the end
	  private double interpolateEnd(InflationEndInterpolatedRateComputation computation, PriceIndexValues values)
	  {
		double weight = computation.Weight;
		double indexValue1 = values.value(computation.EndObservation);
		double indexValue2 = values.value(computation.EndSecondObservation);
		return weight * indexValue1 + (1d - weight) * indexValue2;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(InflationEndInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		PointSensitivityBuilder sensi = endSensitivity(computation, values);
		return sensi.multipliedBy(1d / computation.StartIndexValue);
	  }

	  // interpolate the observations at the end
	  private PointSensitivityBuilder endSensitivity(InflationEndInterpolatedRateComputation computation, PriceIndexValues values)
	  {
		double weight = computation.Weight;
		PointSensitivityBuilder sensi1 = values.valuePointSensitivity(computation.EndObservation).multipliedBy(weight);
		PointSensitivityBuilder sensi2 = values.valuePointSensitivity(computation.EndSecondObservation).multipliedBy(1d - weight);
		return sensi1.combinedWith(sensi2);
	  }

	  public virtual double explainRate(InflationEndInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double w1 = computation.Weight;
		double w2 = 1d - w1;
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.EndObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, values.value(computation.EndObservation)).put(ExplainKey.WEIGHT, w1));
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.EndSecondObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, values.value(computation.EndSecondObservation)).put(ExplainKey.WEIGHT, w2));
		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using PriceIndexValues = com.opengamma.strata.pricer.rate.PriceIndexValues;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using InflationMonthlyRateComputation = com.opengamma.strata.product.rate.InflationMonthlyRateComputation;

	/// <summary>
	/// Rate computation implementation for a price index.
	/// <para>
	/// The pay-off for a unit notional is {@code (IndexEnd / IndexStart - 1)}, where
	/// start index value and end index value are simply returned by {@code RatesProvider}.
	/// </para>
	/// </summary>
	public class ForwardInflationMonthlyRateComputationFn : RateComputationFn<InflationMonthlyRateComputation>
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly ForwardInflationMonthlyRateComputationFn DEFAULT = new ForwardInflationMonthlyRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardInflationMonthlyRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(InflationMonthlyRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndex index = computation.Index;
		PriceIndexValues values = provider.priceIndexValues(index);
		double indexStart = values.value(computation.StartObservation);
		double indexEnd = values.value(computation.EndObservation);
		return indexEnd / indexStart - 1d;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(InflationMonthlyRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndex index = computation.Index;
		PriceIndexValues values = provider.priceIndexValues(index);
		double indexStart = values.value(computation.StartObservation);
		double indexEnd = values.value(computation.EndObservation);
		double indexStartInv = 1d / indexStart;
		PointSensitivityBuilder sensi1 = values.valuePointSensitivity(computation.StartObservation).multipliedBy(-indexEnd * indexStartInv * indexStartInv);
		PointSensitivityBuilder sensi2 = values.valuePointSensitivity(computation.EndObservation).multipliedBy(indexStartInv);
		return sensi1.combinedWith(sensi2);
	  }

	  public virtual double explainRate(InflationMonthlyRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		PriceIndex index = computation.Index;
		PriceIndexValues values = provider.priceIndexValues(index);
		double indexStart = values.value(computation.StartObservation);
		double indexEnd = values.value(computation.EndObservation);

		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.StartObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, index).put(ExplainKey.INDEX_VALUE, indexStart));
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.EndObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, index).put(ExplainKey.INDEX_VALUE, indexEnd));
		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
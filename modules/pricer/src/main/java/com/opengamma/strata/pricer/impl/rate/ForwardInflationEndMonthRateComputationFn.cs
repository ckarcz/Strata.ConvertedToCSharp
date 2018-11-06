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
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;

	/// <summary>
	/// Rate computation implementation for a price index.
	/// <para>
	/// The pay-off for a unit notional is {@code (IndexEnd / IndexStart)}, where
	/// the start index value is given by  {@code InflationEndMonthRateComputation}
	/// and the end index value is returned by {@code RatesProvider}.
	/// </para>
	/// </summary>
	public class ForwardInflationEndMonthRateComputationFn : RateComputationFn<InflationEndMonthRateComputation>
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly ForwardInflationEndMonthRateComputationFn DEFAULT = new ForwardInflationEndMonthRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardInflationEndMonthRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(InflationEndMonthRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double indexStart = computation.StartIndexValue;
		double indexEnd = values.value(computation.EndObservation);
		return indexEnd / indexStart - 1;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(InflationEndMonthRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		return values.valuePointSensitivity(computation.EndObservation).multipliedBy(1d / computation.StartIndexValue);
	  }

	  public virtual double explainRate(InflationEndMonthRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		PriceIndexValues values = provider.priceIndexValues(computation.Index);
		double indexEnd = values.value(computation.EndObservation);
		builder.addListEntry(ExplainKey.OBSERVATIONS, child => child.put(ExplainKey.ENTRY_TYPE, "InflationObservation").put(ExplainKey.FIXING_DATE, computation.EndObservation.FixingMonth.atEndOfMonth()).put(ExplainKey.INDEX, computation.Index).put(ExplainKey.INDEX_VALUE, indexEnd));
		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
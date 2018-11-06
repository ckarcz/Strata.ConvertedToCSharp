/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Rate computation implementation for an Ibor index.
	/// <para>
	/// The implementation simply returns the rate from the {@code RatesProvider}.
	/// </para>
	/// </summary>
	public class ForwardIborRateComputationFn : RateComputationFn<IborRateComputation>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ForwardIborRateComputationFn DEFAULT = new ForwardIborRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardIborRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(IborRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		IborIndexRates rates = provider.iborIndexRates(computation.Index);
		return rates.rate(computation.Observation);
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(IborRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		IborIndexRates rates = provider.iborIndexRates(computation.Index);
		return rates.ratePointSensitivity(computation.Observation);
	  }

	  public virtual double explainRate(IborRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		IborIndexRates rates = provider.iborIndexRates(computation.Index);
		double rate = rates.explainRate(computation.Observation, builder, child =>
		{
		});
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
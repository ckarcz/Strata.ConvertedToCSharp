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
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborAveragedFixing = com.opengamma.strata.product.rate.IborAveragedFixing;
	using IborAveragedRateComputation = com.opengamma.strata.product.rate.IborAveragedRateComputation;

	/// <summary>
	/// Rate computation implementation for a rate based on the average of multiple fixings of a
	/// single Ibor floating rate index.
	/// <para>
	/// The rate computation queries the rates from the {@code RatesProvider} and weighted-average them.
	/// There is no convexity adjustment computed in this implementation.
	/// </para>
	/// </summary>
	public class ForwardIborAveragedRateComputationFn : RateComputationFn<IborAveragedRateComputation>
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly ForwardIborAveragedRateComputationFn DEFAULT = new ForwardIborAveragedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardIborAveragedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(IborAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		IborIndexRates rates = provider.iborIndexRates(computation.Index);

		// take (rate * weight) for each fixing and divide by total weight
		double weightedRate = computation.Fixings.Select(fixing => this.weightedRate(fixing, rates)).Sum();
		return weightedRate / computation.TotalWeight;
	  }

	  // Compute the rate adjusted by the weight for one IborAverageFixing.
	  private double weightedRate(IborAveragedFixing fixing, IborIndexRates rates)
	  {
		double rate = fixing.FixedRate.GetValueOrDefault(rates.rate(fixing.Observation));
		return rate * fixing.Weight;
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(IborAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		IborIndexRates rates = provider.iborIndexRates(computation.Index);

		// combine the weighted sensitivity to each fixing
		// omit fixed rates as they have no sensitivity to a curve
		return computation.Fixings.Where(fixing => !fixing.FixedRate.Present).Select(fixing => weightedSensitivity(fixing, computation.TotalWeight, rates)).Aggregate(PointSensitivityBuilder.none(), PointSensitivityBuilder.combinedWith);
	  }

	  // Compute the weighted sensitivity for one IborAverageFixing.
	  private PointSensitivityBuilder weightedSensitivity(IborAveragedFixing fixing, double totalWeight, IborIndexRates rates)
	  {

		return rates.ratePointSensitivity(fixing.Observation).multipliedBy(fixing.Weight / totalWeight);
	  }

	  public virtual double explainRate(IborAveragedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		IborIndexRates rates = provider.iborIndexRates(computation.Index);
		foreach (IborAveragedFixing fixing in computation.Fixings)
		{
		  rates.explainRate(fixing.Observation, builder, child => child.put(ExplainKey.WEIGHT, fixing.Weight));
		}
		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	}

}
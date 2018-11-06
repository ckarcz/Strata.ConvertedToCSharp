/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;

	/// <summary>
	/// Rate computation implementation for rate based on the weighted average of the fixing
	/// on a single date of two Ibor indices.
	/// <para>
	/// The rate computation queries the rates from the {@code RatesProvider} and interpolates them.
	/// There is no convexity adjustment computed in this implementation.
	/// </para>
	/// </summary>
	public class ForwardIborInterpolatedRateComputationFn : RateComputationFn<IborInterpolatedRateComputation>
	{

	  /// <summary>
	  /// Default instance.
	  /// </summary>
	  public static readonly ForwardIborInterpolatedRateComputationFn DEFAULT = new ForwardIborInterpolatedRateComputationFn();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public ForwardIborInterpolatedRateComputationFn()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(IborInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		IborIndexObservation obs1 = computation.ShortObservation;
		IborIndexObservation obs2 = computation.LongObservation;
		IborIndexRates rates1 = provider.iborIndexRates(obs1.Index);
		IborIndexRates rates2 = provider.iborIndexRates(obs2.Index);

		double rate1 = rates1.rate(obs1);
		double rate2 = rates2.rate(obs2);
		DoublesPair weights = this.weights(obs1, obs2, endDate);
		return ((rate1 * weights.First) + (rate2 * weights.Second)) / (weights.First + weights.Second);
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(IborInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		// computes the dates related to the underlying deposits associated to the indices
		IborIndexObservation obs1 = computation.ShortObservation;
		IborIndexObservation obs2 = computation.LongObservation;
		DoublesPair weights = this.weights(obs1, obs2, endDate);
		double totalWeight = weights.First + weights.Second;

		IborIndexRates ratesIndex1 = provider.iborIndexRates(obs1.Index);
		PointSensitivityBuilder sens1 = ratesIndex1.ratePointSensitivity(obs1).multipliedBy(weights.First / totalWeight);
		IborIndexRates ratesIndex2 = provider.iborIndexRates(obs2.Index);
		PointSensitivityBuilder sens2 = ratesIndex2.ratePointSensitivity(obs2).multipliedBy(weights.Second / totalWeight);
		return sens1.combinedWith(sens2);
	  }

	  public virtual double explainRate(IborInterpolatedRateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		IborIndexObservation obs1 = computation.ShortObservation;
		IborIndexObservation obs2 = computation.LongObservation;
		DoublesPair weights = this.weights(obs1, obs2, endDate);
		IborIndexRates rates1 = provider.iborIndexRates(obs1.Index);
		IborIndexRates rates2 = provider.iborIndexRates(obs2.Index);
		rates1.explainRate(obs1, builder, child => child.put(ExplainKey.WEIGHT, weights.First));
		rates2.explainRate(obs2, builder, child => child.put(ExplainKey.WEIGHT, weights.Second));
		double rate = this.rate(computation, startDate, endDate, provider);
		builder.put(ExplainKey.COMBINED_RATE, rate);
		return rate;
	  }

	  // computes the weights related to the two indices
	  private DoublesPair weights(IborIndexObservation obs1, IborIndexObservation obs2, LocalDate endDate)
	  {
		// weights: linear interpolation on the number of days between the fixing date and the maturity dates of the 
		//   actual coupons on one side and the maturity dates of the underlying deposit on the other side.
		long fixingEpochDay = obs1.FixingDate.toEpochDay();
		double days1 = obs1.MaturityDate.toEpochDay() - fixingEpochDay;
		double days2 = obs2.MaturityDate.toEpochDay() - fixingEpochDay;
		double daysN = endDate.toEpochDay() - fixingEpochDay;
		double weight1 = (days2 - daysN) / (days2 - days1);
		double weight2 = (daysN - days1) / (days2 - days1);
		return DoublesPair.of(weight1, weight2);
	  }

	}

}
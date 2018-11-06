/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.rate
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborAveragedRateComputation = com.opengamma.strata.product.rate.IborAveragedRateComputation;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;
	using InflationInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationInterpolatedRateComputation;
	using InflationMonthlyRateComputation = com.opengamma.strata.product.rate.InflationMonthlyRateComputation;
	using OvernightAveragedDailyRateComputation = com.opengamma.strata.product.rate.OvernightAveragedDailyRateComputation;
	using OvernightAveragedRateComputation = com.opengamma.strata.product.rate.OvernightAveragedRateComputation;
	using OvernightCompoundedRateComputation = com.opengamma.strata.product.rate.OvernightCompoundedRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Rate computation implementation using multiple dispatch.
	/// <para>
	/// Dispatches the request to the correct implementation.
	/// </para>
	/// </summary>
	public class DispatchingRateComputationFn : RateComputationFn<RateComputation>
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DispatchingRateComputationFn DEFAULT = new DispatchingRateComputationFn(ForwardIborRateComputationFn.DEFAULT, ForwardIborInterpolatedRateComputationFn.DEFAULT, ForwardIborAveragedRateComputationFn.DEFAULT, ForwardOvernightCompoundedRateComputationFn.DEFAULT, ApproxForwardOvernightAveragedRateComputationFn.DEFAULT, ForwardOvernightAveragedDailyRateComputationFn.DEFAULT, ForwardInflationMonthlyRateComputationFn.DEFAULT, ForwardInflationInterpolatedRateComputationFn.DEFAULT, ForwardInflationEndMonthRateComputationFn.DEFAULT, ForwardInflationEndInterpolatedRateComputationFn.DEFAULT);

	  /// <summary>
	  /// Rate provider for <seealso cref="IborRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<IborRateComputation> iborRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="IborInterpolatedRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<IborInterpolatedRateComputation> iborInterpolatedRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="IborAveragedRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<IborAveragedRateComputation> iborAveragedRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="OvernightCompoundedRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<OvernightCompoundedRateComputation> overnightCompoundedRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="OvernightAveragedRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<OvernightAveragedRateComputation> overnightAveragedRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="OvernightAveragedDailyRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<OvernightAveragedDailyRateComputation> overnightAveragedDailyRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="InflationMonthlyRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<InflationMonthlyRateComputation> inflationMonthlyRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="InflationInterpolatedRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<InflationInterpolatedRateComputation> inflationInterpolatedRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="InflationEndMonthRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<InflationEndMonthRateComputation> inflationEndMonthRateComputationFn;
	  /// <summary>
	  /// Rate provider for <seealso cref="InflationEndInterpolatedRateComputation"/>.
	  /// </summary>
	  private readonly RateComputationFn<InflationEndInterpolatedRateComputation> inflationEndInterpolatedRateComputationFn;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="iborRateComputationFn">  the rate provider for <seealso cref="IborRateComputation"/> </param>
	  /// <param name="iborInterpolatedRateComputationFn">  the rate computation for <seealso cref="IborInterpolatedRateComputation"/> </param>
	  /// <param name="iborAveragedRateComputationFn">  the rate computation for <seealso cref="IborAveragedRateComputation"/> </param>
	  /// <param name="overnightCompoundedRateComputationFn">  the rate computation for <seealso cref="OvernightCompoundedRateComputation"/> </param>
	  /// <param name="overnightAveragedRateComputationFn">  the rate computation for <seealso cref="OvernightAveragedRateComputation"/> </param>
	  /// <param name="overnightAveragedDailyRateComputationFn">  the rate computation for <seealso cref="OvernightAveragedDailyRateComputation"/> </param>
	  /// <param name="inflationMonthlyRateComputationFn">  the rate computation for <seealso cref="InflationMonthlyRateComputation"/> </param>
	  /// <param name="inflationInterpolatedRateComputationFn">  the rate computation for <seealso cref="InflationInterpolatedRateComputation"/> </param>
	  /// <param name="inflationEndMonthRateComputationFn">  the rate computation for <seealso cref="InflationEndMonthRateComputation"/> </param>
	  /// <param name="inflationEndInterpolatedRateComputationFn">  the rate computation for <seealso cref="InflationEndInterpolatedRateComputation"/> </param>
	  public DispatchingRateComputationFn(RateComputationFn<IborRateComputation> iborRateComputationFn, RateComputationFn<IborInterpolatedRateComputation> iborInterpolatedRateComputationFn, RateComputationFn<IborAveragedRateComputation> iborAveragedRateComputationFn, RateComputationFn<OvernightCompoundedRateComputation> overnightCompoundedRateComputationFn, RateComputationFn<OvernightAveragedRateComputation> overnightAveragedRateComputationFn, RateComputationFn<OvernightAveragedDailyRateComputation> overnightAveragedDailyRateComputationFn, RateComputationFn<InflationMonthlyRateComputation> inflationMonthlyRateComputationFn, RateComputationFn<InflationInterpolatedRateComputation> inflationInterpolatedRateComputationFn, RateComputationFn<InflationEndMonthRateComputation> inflationEndMonthRateComputationFn, RateComputationFn<InflationEndInterpolatedRateComputation> inflationEndInterpolatedRateComputationFn)
	  {

		this.iborRateComputationFn = ArgChecker.notNull(iborRateComputationFn, "iborRateComputationFn");
		this.iborInterpolatedRateComputationFn = ArgChecker.notNull(iborInterpolatedRateComputationFn, "iborInterpolatedRateComputationFn");
		this.iborAveragedRateComputationFn = ArgChecker.notNull(iborAveragedRateComputationFn, "iborAverageRateComputationFn");
		this.overnightCompoundedRateComputationFn = ArgChecker.notNull(overnightCompoundedRateComputationFn, "overnightCompoundedRateComputationFn");
		this.overnightAveragedRateComputationFn = ArgChecker.notNull(overnightAveragedRateComputationFn, "overnightAveragedRateComputationFn");
		this.overnightAveragedDailyRateComputationFn = ArgChecker.notNull(overnightAveragedDailyRateComputationFn, "overnightAveragedDailyRateComputationFn");
		this.inflationMonthlyRateComputationFn = ArgChecker.notNull(inflationMonthlyRateComputationFn, "inflationMonthlyRateComputationFn");
		this.inflationInterpolatedRateComputationFn = ArgChecker.notNull(inflationInterpolatedRateComputationFn, "inflationInterpolatedRateComputationFn");
		this.inflationEndMonthRateComputationFn = ArgChecker.notNull(inflationEndMonthRateComputationFn, "inflationEndMonthRateComputationFn");
		this.inflationEndInterpolatedRateComputationFn = ArgChecker.notNull(inflationEndInterpolatedRateComputationFn, "inflationEndInterpolatedRateComputationFn");
	  }

	  //-------------------------------------------------------------------------
	  public virtual double rate(RateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		// dispatch by runtime type
		if (computation is FixedRateComputation)
		{
		  // inline code (performance) avoiding need for FixedRateComputationFn implementation
		  return ((FixedRateComputation) computation).Rate;
		}
		else if (computation is IborRateComputation)
		{
		  return iborRateComputationFn.rate((IborRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is IborInterpolatedRateComputation)
		{
		  return iborInterpolatedRateComputationFn.rate((IborInterpolatedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is IborAveragedRateComputation)
		{
		  return iborAveragedRateComputationFn.rate((IborAveragedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is OvernightAveragedRateComputation)
		{
		  return overnightAveragedRateComputationFn.rate((OvernightAveragedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is OvernightCompoundedRateComputation)
		{
		  return overnightCompoundedRateComputationFn.rate((OvernightCompoundedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is OvernightAveragedDailyRateComputation)
		{
		  return overnightAveragedDailyRateComputationFn.rate((OvernightAveragedDailyRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationMonthlyRateComputation)
		{
		  return inflationMonthlyRateComputationFn.rate((InflationMonthlyRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationInterpolatedRateComputation)
		{
		  return inflationInterpolatedRateComputationFn.rate((InflationInterpolatedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationEndMonthRateComputation)
		{
		  return inflationEndMonthRateComputationFn.rate((InflationEndMonthRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationEndInterpolatedRateComputation)
		{
		  return inflationEndInterpolatedRateComputationFn.rate((InflationEndInterpolatedRateComputation) computation, startDate, endDate, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown Rate type: " + computation.GetType().Name);
		}
	  }

	  public virtual PointSensitivityBuilder rateSensitivity(RateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider)
	  {

		// dispatch by runtime type
		if (computation is FixedRateComputation)
		{
		  // inline code (performance) avoiding need for FixedRateComputationFn implementation
		  return PointSensitivityBuilder.none();
		}
		else if (computation is IborRateComputation)
		{
		  return iborRateComputationFn.rateSensitivity((IborRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is IborInterpolatedRateComputation)
		{
		  return iborInterpolatedRateComputationFn.rateSensitivity((IborInterpolatedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is IborAveragedRateComputation)
		{
		  return iborAveragedRateComputationFn.rateSensitivity((IborAveragedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is OvernightAveragedRateComputation)
		{
		  return overnightAveragedRateComputationFn.rateSensitivity((OvernightAveragedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is OvernightCompoundedRateComputation)
		{
		  return overnightCompoundedRateComputationFn.rateSensitivity((OvernightCompoundedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is OvernightAveragedDailyRateComputation)
		{
		  return overnightAveragedDailyRateComputationFn.rateSensitivity((OvernightAveragedDailyRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationMonthlyRateComputation)
		{
		  return inflationMonthlyRateComputationFn.rateSensitivity((InflationMonthlyRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationInterpolatedRateComputation)
		{
		  return inflationInterpolatedRateComputationFn.rateSensitivity((InflationInterpolatedRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationEndMonthRateComputation)
		{
		  return inflationEndMonthRateComputationFn.rateSensitivity((InflationEndMonthRateComputation) computation, startDate, endDate, provider);
		}
		else if (computation is InflationEndInterpolatedRateComputation)
		{
		  return inflationEndInterpolatedRateComputationFn.rateSensitivity((InflationEndInterpolatedRateComputation) computation, startDate, endDate, provider);
		}
		else
		{
		  throw new System.ArgumentException("Unknown Rate type: " + computation.GetType().Name);
		}
	  }

	  public virtual double explainRate(RateComputation computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder)
	  {

		// dispatch by runtime type
		if (computation is FixedRateComputation)
		{
		  // inline code (performance) avoiding need for FixedRateComputationFn implementation
		  double rate = ((FixedRateComputation) computation).Rate;
		  builder.put(ExplainKey.FIXED_RATE, rate);
		  builder.put(ExplainKey.COMBINED_RATE, rate);
		  return rate;
		}
		else if (computation is IborRateComputation)
		{
		  return iborRateComputationFn.explainRate((IborRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is IborInterpolatedRateComputation)
		{
		  return iborInterpolatedRateComputationFn.explainRate((IborInterpolatedRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is IborAveragedRateComputation)
		{
		  return iborAveragedRateComputationFn.explainRate((IborAveragedRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is OvernightAveragedRateComputation)
		{
		  return overnightAveragedRateComputationFn.explainRate((OvernightAveragedRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is OvernightCompoundedRateComputation)
		{
		  return overnightCompoundedRateComputationFn.explainRate((OvernightCompoundedRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is OvernightAveragedDailyRateComputation)
		{
		  return overnightAveragedDailyRateComputationFn.explainRate((OvernightAveragedDailyRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is InflationMonthlyRateComputation)
		{
		  return inflationMonthlyRateComputationFn.explainRate((InflationMonthlyRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is InflationInterpolatedRateComputation)
		{
		  return inflationInterpolatedRateComputationFn.explainRate((InflationInterpolatedRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is InflationEndMonthRateComputation)
		{
		  return inflationEndMonthRateComputationFn.explainRate((InflationEndMonthRateComputation) computation, startDate, endDate, provider, builder);
		}
		else if (computation is InflationEndInterpolatedRateComputation)
		{
		  return inflationEndInterpolatedRateComputationFn.explainRate((InflationEndInterpolatedRateComputation) computation, startDate, endDate, provider, builder);
		}
		else
		{
		  throw new System.ArgumentException("Unknown Rate type: " + computation.GetType().Name);
		}
	  }

	}

}
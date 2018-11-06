/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedOvernightFuture = com.opengamma.strata.product.index.ResolvedOvernightFuture;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Pricer for for Overnight rate future products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedOvernightFuture"/>.
	/// 
	/// <h4>Price</h4>
	/// The price of an Overnight rate future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Overnight rate futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class DiscountingOvernightFutureProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingOvernightFutureProductPricer DEFAULT = new DiscountingOvernightFutureProductPricer(RateComputationFn.standard());

	  /// <summary>
	  /// Rate computation.
	  /// </summary>
	  private readonly RateComputationFn<RateComputation> rateComputationFn;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="rateComputationFn">  the rate computation function </param>
	  public DiscountingOvernightFutureProductPricer(RateComputationFn<RateComputation> rateComputationFn)
	  {
		this.rateComputationFn = ArgChecker.notNull(rateComputationFn, "rateComputationFn");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number related to Overnight rate futures product on which the daily margin is computed.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code (marginIndex(future, C2) - marginIndex(future, C1))}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="price">  the price of the product, in decimal form </param>
	  /// <returns> the index </returns>
	  internal virtual double marginIndex(ResolvedOvernightFuture future, double price)
	  {
		return price * future.Notional * future.AccrualFactor;
	  }

	  /// <summary>
	  /// Calculates the margin index sensitivity of the Overnight rate future product.
	  /// <para>
	  /// The margin index sensitivity is the sensitivity of the margin index to the underlying curves.
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code (marginIndex(future, C2) - marginIndex(future, C1))}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="priceSensitivity">  the price sensitivity of the product </param>
	  /// <returns> the index sensitivity </returns>
	  internal virtual PointSensitivities marginIndexSensitivity(ResolvedOvernightFuture future, PointSensitivities priceSensitivity)
	  {
		return priceSensitivity.multipliedBy(future.Notional * future.AccrualFactor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the Overnight rate future product.
	  /// <para>
	  /// The price of the product is the price on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public virtual double price(ResolvedOvernightFuture future, RatesProvider ratesProvider)
	  {
		double forwardRate = rateComputationFn.rate(future.OvernightRate, future.OvernightRate.StartDate, future.OvernightRate.EndDate, ratesProvider);
		return 1d - forwardRate;
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the Overnight rate future product.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual PointSensitivities priceSensitivity(ResolvedOvernightFuture future, RatesProvider ratesProvider)
	  {

		PointSensitivityBuilder forwardRateSensitivity = rateComputationFn.rateSensitivity(future.OvernightRate, future.OvernightRate.StartDate, future.OvernightRate.EndDate, ratesProvider);
		// The sensitivity should be to no currency or currency XXX. To avoid useless conversion, the dimension-less 
		// price sensitivity is reported in the future currency.
		return forwardRateSensitivity.build().multipliedBy(-1d);
	  }

	}

}
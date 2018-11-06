/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{

	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using HullWhiteOneFactorPiecewiseConstantParametersProvider = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParametersProvider;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFuture = com.opengamma.strata.product.index.IborFuture;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;

	/// <summary>
	/// Pricer for for Ibor future products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="IborFuture"/> based on
	/// Hull-White one-factor model with piecewise constant volatility.
	/// </para>
	/// <para> 
	/// Reference: Henrard M., Eurodollar Futures and Options: Convexity Adjustment in HJM One-Factor Model. March 2005.
	/// Available at <a href="http://ssrn.com/abstract=682343">http://ssrn.com/abstract=682343</a>
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
	public class HullWhiteIborFutureProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly HullWhiteIborFutureProductPricer DEFAULT = new HullWhiteIborFutureProductPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public HullWhiteIborFutureProductPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number related to Ibor futures product on which the daily margin is computed.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code (marginIndex(future, C2) - marginIndex(future, C1))}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="price">  the price of the product, in decimal form </param>
	  /// <returns> the index </returns>
	  internal virtual double marginIndex(ResolvedIborFuture future, double price)
	  {
		return price * future.Notional * future.AccrualFactor;
	  }

	  /// <summary>
	  /// Calculates the margin index sensitivity of the Ibor future product.
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
	  internal virtual PointSensitivities marginIndexSensitivity(ResolvedIborFuture future, PointSensitivities priceSensitivity)
	  {
		return priceSensitivity.multipliedBy(future.Notional * future.AccrualFactor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the Ibor future product.
	  /// <para>
	  /// The price of the product is the price on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public virtual double price(ResolvedIborFuture future, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		double parRate = this.parRate(future, ratesProvider, hwProvider);
		return 1d - parRate;
	  }

	  /// <summary>
	  /// Calculates the convexity adjustment (to the price) of the Ibor future product.
	  /// <para>
	  /// The convexity adjustment of the product is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the convexity adjustment, in decimal form </returns>
	  public virtual double convexityAdjustment(ResolvedIborFuture future, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		IborIndexObservation obs = future.IborRate.Observation;
		double forward = ratesProvider.iborIndexRates(future.Index).rate(obs);
		double parRate = this.parRate(future, ratesProvider, hwProvider);
		return forward - parRate;
	  }

	  /// <summary>
	  /// Calculates the par rate of the Ibor future product.
	  /// <para>
	  /// The par rate is given by ({@code 1 - price}).
	  /// The par rate of the product is the value on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the par rate of the product, in decimal form </returns>
	  public virtual double parRate(ResolvedIborFuture future, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		IborIndexObservation obs = future.IborRate.Observation;
		double forward = ratesProvider.iborIndexRates(future.Index).rate(obs);
		LocalDate fixingStartDate = obs.EffectiveDate;
		LocalDate fixingEndDate = obs.MaturityDate;
		double fixingYearFraction = obs.YearFraction;
		double convexity = hwProvider.futuresConvexityFactor(future.LastTradeDate, fixingStartDate, fixingEndDate);
		return convexity * forward - (1d - convexity) / fixingYearFraction;
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the Ibor future product.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual PointSensitivities priceSensitivityRates(ResolvedIborFuture future, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		IborIndexObservation obs = future.IborRate.Observation;
		LocalDate fixingStartDate = obs.EffectiveDate;
		LocalDate fixingEndDate = obs.MaturityDate;
		double convexity = hwProvider.futuresConvexityFactor(future.LastTradeDate, fixingStartDate, fixingEndDate);
		IborRateSensitivity sensi = IborRateSensitivity.of(obs, -convexity);
		// The sensitivity should be to no currency or currency XXX. To avoid useless conversion, the dimension-less 
		// price sensitivity is reported in the future currency.
		return PointSensitivities.of(sensi);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity to piecewise constant volatility parameters of the Hull-White model.
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="hwProvider">  the Hull-White model parameter provider </param>
	  /// <returns> the price parameter sensitivity of the product </returns>
	  public virtual DoubleArray priceSensitivityModelParamsHullWhite(ResolvedIborFuture future, RatesProvider ratesProvider, HullWhiteOneFactorPiecewiseConstantParametersProvider hwProvider)
	  {

		IborIndexObservation obs = future.IborRate.Observation;
		double forward = ratesProvider.iborIndexRates(future.Index).rate(obs);
		LocalDate fixingStartDate = obs.EffectiveDate;
		LocalDate fixingEndDate = obs.MaturityDate;
		double fixingYearFraction = obs.YearFraction;
		DoubleArray convexityDeriv = hwProvider.futuresConvexityFactorAdjoint(future.LastTradeDate, fixingStartDate, fixingEndDate).Derivatives;
		convexityDeriv = convexityDeriv.multipliedBy(-forward - 1d / fixingYearFraction);
		return convexityDeriv;
	  }
	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;

	/// <summary>
	/// Pricer for for Ibor future products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedIborFuture"/>.
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
	public class DiscountingIborFutureProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingIborFutureProductPricer DEFAULT = new DiscountingIborFutureProductPricer();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public DiscountingIborFutureProductPricer()
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
	  /// <returns> the price of the product, in decimal form </returns>
	  public virtual double price(ResolvedIborFuture future, RatesProvider ratesProvider)
	  {
		IborIndexRates rates = ratesProvider.iborIndexRates(future.Index);
		double forward = rates.rate(future.IborRate.Observation);
		return 1.0 - forward;
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
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual PointSensitivities priceSensitivity(ResolvedIborFuture future, RatesProvider ratesProvider)
	  {
		IborRateSensitivity sensi = IborRateSensitivity.of(future.IborRate.Observation, -1d);
		// The sensitivity should be to no currency or currency XXX. To avoid useless conversion, the dimension-less 
		// price sensitivity is reported in the future currency.
		return PointSensitivities.of(sensi);
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.dsf
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedDsf = com.opengamma.strata.product.dsf.ResolvedDsf;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;

	/// <summary>
	/// Pricer for for Deliverable Swap Futures (DSFs).
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedDsf"/>.
	/// 
	/// <h4>Price</h4>
	/// The price of a DSF is based on the present value (NPV) of the underlying swap on the delivery date.
	/// For example, a price of 100.182 represents a present value of $100,182.00, if the notional is $100,000.
	/// This price can also be viewed as a percentage present value - {@code (100 + percentPv)}, or 0.182% in this example.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for DSFs in the trade model, pricers and market data.
	/// The decimal price is based on the decimal multiplier equivalent to the implied percentage.
	/// Thus the market price of 100.182 is represented in Strata by 1.00182.
	/// </para>
	/// </summary>
	public sealed class DiscountingDsfProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingDsfProductPricer DEFAULT = new DiscountingDsfProductPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="ResolvedSwap"/>.
	  /// </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="ResolvedSwap"/>. </param>
	  public DiscountingDsfProductPricer(DiscountingSwapProductPricer swapPricer)
	  {
		this.swapPricer = ArgChecker.notNull(swapPricer, "swapPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the pricer used to price the underlying swap.
	  /// </summary>
	  /// <returns> the pricer </returns>
	  internal DiscountingSwapProductPricer SwapPricer
	  {
		  get
		  {
			return swapPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number related to deliverable swap futures product on which the daily margin is computed.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code (marginIndex(future, C2) - marginIndex(future, C1))}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="price">  the price of the product, in decimal form </param>
	  /// <returns> the index </returns>
	  internal double marginIndex(ResolvedDsf future, double price)
	  {
		return price * future.Notional;
	  }

	  /// <summary>
	  /// Calculates the margin index sensitivity of the deliverable swap futures product.
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
	  internal PointSensitivities marginIndexSensitivity(ResolvedDsf future, PointSensitivities priceSensitivity)
	  {

		return priceSensitivity.multipliedBy(future.Notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the deliverable swap futures product.
	  /// <para>
	  /// The price of the product is the price on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double price(ResolvedDsf future, RatesProvider ratesProvider)
	  {
		ResolvedSwap swap = future.UnderlyingSwap;
		Currency currency = future.Currency;
		CurrencyAmount pvSwap = swapPricer.presentValue(swap, currency, ratesProvider);
		double df = ratesProvider.discountFactor(currency, future.DeliveryDate);
		return 1d + pvSwap.Amount / df;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity of the deliverable swap futures product.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public PointSensitivities priceSensitivity(ResolvedDsf future, RatesProvider ratesProvider)
	  {
		ResolvedSwap swap = future.UnderlyingSwap;
		Currency currency = future.Currency;
		double pvSwap = swapPricer.presentValue(swap, currency, ratesProvider).Amount;
		double dfInv = 1d / ratesProvider.discountFactor(currency, future.DeliveryDate);
		PointSensitivityBuilder sensiSwapPv = swapPricer.presentValueSensitivity(swap, ratesProvider).multipliedBy(dfInv);
		PointSensitivityBuilder sensiDf = ratesProvider.discountFactors(currency).zeroRatePointSensitivity(future.DeliveryDate).multipliedBy(-pvSwap * dfInv * dfInv);
		return sensiSwapPv.combinedWith(sensiDf).build();
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using NormalFormulaRepository = com.opengamma.strata.pricer.impl.option.NormalFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using IborFutureOption = com.opengamma.strata.product.index.IborFutureOption;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;
	using ResolvedIborFutureOption = com.opengamma.strata.product.index.ResolvedIborFutureOption;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// Pricer of options on Ibor future with a normal model on the underlying future price.
	/// <para>
	/// This provides the ability to price an Ibor future option.
	/// The option must be based on <seealso cref="FutureOptionPremiumStyle#DAILY_MARGIN daily margin"/>.
	/// 
	/// <h4>Price</h4>
	/// The price of an Ibor future option is based on the price of the underlying future, the volatility
	/// and the time to expiry. The price of the at-the-money option tends to zero as expiry approaches.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Ibor future options in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, an option price of 0.2 is related to a futures price of 99.32 that implies an
	/// interest rate of 0.68%. Strata represents the price of the future as 0.9932 and thus
	/// represents the price of the option as 0.002.
	/// </para>
	/// </summary>
	public class NormalIborFutureOptionMarginedProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly NormalIborFutureOptionMarginedProductPricer DEFAULT = new NormalIborFutureOptionMarginedProductPricer(DiscountingIborFutureProductPricer.DEFAULT);

	  /// <summary>
	  /// The underlying future pricer.
	  /// The pricer take only the curves as inputs, no model parameters.
	  /// </summary>
	  private readonly DiscountingIborFutureProductPricer futurePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="futurePricer">  the pricer for <seealso cref="IborFutureOption"/> </param>
	  public NormalIborFutureOptionMarginedProductPricer(DiscountingIborFutureProductPricer futurePricer)
	  {
		this.futurePricer = ArgChecker.notNull(futurePricer, "futurePricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the underlying future pricer function.
	  /// </summary>
	  /// <returns> the future pricer </returns>
	  internal virtual DiscountingIborFutureProductPricer FuturePricer
	  {
		  get
		  {
			return futurePricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number related to Ibor futures product on which the daily margin is computed.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code marginIndex(future, C2) - marginIndex(future, C1)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="price">  the price of the product, in decimal form </param>
	  /// <returns> the index </returns>
	  internal virtual double marginIndex(ResolvedIborFutureOption option, double price)
	  {
		double notional = option.UnderlyingFuture.Notional;
		double accrualFactor = option.UnderlyingFuture.AccrualFactor;
		return price * notional * accrualFactor;
	  }

	  /// <summary>
	  /// Calculates the margin index sensitivity of the Ibor future product.
	  /// <para>
	  /// The margin index sensitivity if the sensitivity of the margin index to the underlying curves.
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code marginIndex(future, C2) - marginIndex(future, C1)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="priceSensitivity">  the price sensitivity of the product </param>
	  /// <returns> the index sensitivity </returns>
	  internal virtual PointSensitivities marginIndexSensitivity(ResolvedIborFutureOption option, PointSensitivities priceSensitivity)
	  {

		double notional = option.UnderlyingFuture.Notional;
		double accrualFactor = option.UnderlyingFuture.AccrualFactor;
		return priceSensitivity.multipliedBy(notional * accrualFactor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the Ibor future option product.
	  /// <para>
	  /// The price of the option is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public virtual double price(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, ratesProvider);
		return price(futureOption, ratesProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the price of the Ibor future option product
	  /// based on the price of the underlying future.
	  /// <para>
	  /// The price of the option is the price on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future, in decimal form </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public virtual double price(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		ArgChecker.isTrue(futureOption.UnderlyingFuture.Index.Equals(volatilities.Index), "Future index should be the same as data index");

		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double strike = futureOption.StrikePrice;
		ResolvedIborFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(timeToExpiry, future.LastTradeDate, strike, futurePrice);

		return NormalFormulaRepository.price(futurePrice, strike, timeToExpiry, volatility, futureOption.PutCall);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the delta of the Ibor future option product.
	  /// <para>
	  /// The delta of the product is the sensitivity of the option price to the future price.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual double deltaStickyStrike(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, ratesProvider);
		return deltaStickyStrike(futureOption, ratesProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the delta of the Ibor future option product
	  /// based on the price of the underlying future.
	  /// <para>
	  /// The delta of the product is the sensitivity of the option price to the future price.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future, in decimal form </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual double deltaStickyStrike(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");

		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double strike = futureOption.StrikePrice;
		ResolvedIborFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(timeToExpiry, future.LastTradeDate, strike, futurePrice);

		return NormalFormulaRepository.delta(futurePrice, strike, timeToExpiry, volatility, futureOption.PutCall);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity of the Ibor future option product based on curves.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual PointSensitivities priceSensitivityRatesStickyStrike(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");

		double futurePrice = this.futurePrice(futureOption, ratesProvider);
		return priceSensitivityRatesStickyStrike(futureOption, ratesProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the Ibor future option product
	  /// based on the price of the underlying future.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future, in decimal form </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public virtual PointSensitivities priceSensitivityRatesStickyStrike(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double futurePrice)
	  {

		double delta = deltaStickyStrike(futureOption, ratesProvider, volatilities, futurePrice);
		PointSensitivities futurePriceSensitivity = futurePricer.priceSensitivity(futureOption.UnderlyingFuture, ratesProvider);
		return futurePriceSensitivity.multipliedBy(delta);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity to the normal volatility used for the pricing of the Ibor future option.
	  /// <para>
	  /// This sensitivity is also called the <i>price normal vega</i>.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the sensitivity </returns>
	  public virtual IborFutureOptionSensitivity priceSensitivityModelParamsVolatility(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, ratesProvider);
		return priceSensitivityModelParamsVolatility(futureOption, ratesProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity to the normal volatility used for the pricing of the Ibor future option
	  /// based on the price of the underlying future.
	  /// <para>
	  /// This sensitivity is also called the <i>price normal vega</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the underlying future price, in decimal form </param>
	  /// <returns> the sensitivity </returns>
	  public virtual IborFutureOptionSensitivity priceSensitivityModelParamsVolatility(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider, NormalIborFutureOptionVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");

		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double strike = futureOption.StrikePrice;
		ResolvedIborFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(timeToExpiry, future.LastTradeDate, strike, futurePrice);

		double vega = NormalFormulaRepository.vega(futurePrice, strike, timeToExpiry, volatility, futureOption.PutCall);
		return IborFutureOptionSensitivity.of(volatilities.Name, timeToExpiry, future.LastTradeDate, strike, futurePrice, future.Currency, vega);
	  }

	  //-------------------------------------------------------------------------
	  // calculate the price of the underlying future
	  private double futurePrice(ResolvedIborFutureOption futureOption, RatesProvider ratesProvider)
	  {
		ResolvedIborFuture future = futureOption.UnderlyingFuture;
		return futurePricer.price(future, ratesProvider);
	  }

	}

}
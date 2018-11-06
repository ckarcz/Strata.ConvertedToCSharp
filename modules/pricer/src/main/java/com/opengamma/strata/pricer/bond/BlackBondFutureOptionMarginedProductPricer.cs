/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using BondFuture = com.opengamma.strata.product.bond.BondFuture;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedBondFutureOption = com.opengamma.strata.product.bond.ResolvedBondFutureOption;
	using FutureOptionPremiumStyle = com.opengamma.strata.product.option.FutureOptionPremiumStyle;

	/// <summary>
	/// Pricer of options on bond future with a log-normal model on the underlying future price.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures options in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="BondFuture"/>.
	/// </summary>
	public sealed class BlackBondFutureOptionMarginedProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BlackBondFutureOptionMarginedProductPricer DEFAULT = new BlackBondFutureOptionMarginedProductPricer(DiscountingBondFutureProductPricer.DEFAULT);

	  /// <summary>
	  /// The underlying future pricer.
	  /// The pricer take only the curves as inputs, no model parameters.
	  /// </summary>
	  private readonly DiscountingBondFutureProductPricer futurePricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="futurePricer">  the pricer for <seealso cref="ResolvedBondFutureOption"/> </param>
	  public BlackBondFutureOptionMarginedProductPricer(DiscountingBondFutureProductPricer futurePricer)
	  {
		this.futurePricer = ArgChecker.notNull(futurePricer, "futurePricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the underlying future pricer function.
	  /// </summary>
	  /// <returns> the future pricer </returns>
	  internal DiscountingBondFutureProductPricer FuturePricer
	  {
		  get
		  {
			return futurePricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number related to bond futures product on which the daily margin is computed.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code marginIndex(future, C2) - marginIndex(future, C1)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="price">  the price of the product, in decimal form </param>
	  /// <returns> the index </returns>
	  internal double marginIndex(ResolvedBondFutureOption option, double price)
	  {
		double notional = option.UnderlyingFuture.Notional;
		return price * notional;
	  }

	  /// <summary>
	  /// Calculates the margin index sensitivity of the bond future product.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code marginIndex(future, C2) - marginIndex(future, C1)}.
	  /// The margin index sensitivity if the sensitivity of the margin index to the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="priceSensitivity">  the price sensitivity of the product </param>
	  /// <returns> the index sensitivity </returns>
	  internal PointSensitivities marginIndexSensitivity(ResolvedBondFutureOption option, PointSensitivities priceSensitivity)
	  {
		double notional = option.UnderlyingFuture.Notional;
		return priceSensitivity.multipliedBy(notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the bond future option product.
	  /// <para>
	  /// The price of the option is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bond futures. This is coherent with the pricing of <seealso cref="BondFuture"/>.
	  /// For example, a price of 1.32% is represented in Strata by 0.0132.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double price(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, discountingProvider);
		return price(futureOption, discountingProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the price of the bond future option product
	  /// based on the price of the underlying future.
	  /// <para>
	  /// The price of the option is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bond futures. This is coherent with the pricing of <seealso cref="BondFuture"/>.
	  /// For example, a price of 1.32% is represented in Strata by 0.0132.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double price(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		double strike = futureOption.StrikePrice;
		ResolvedBondFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(futureOption.Expiry, future.LastTradeDate, strike, futurePrice);
		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double price = BlackFormulaRepository.price(futurePrice, strike, timeToExpiry, volatility, futureOption.PutCall.Call);
		return price;
	  }

	  internal double price(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		ArgChecker.isTrue(volatilities is BlackBondFutureVolatilities, "Provider must be of type BlackVolatilityBondFutureProvider");
		return price(futureOption, discountingProvider, (BlackBondFutureVolatilities) volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the delta of the bond future option product.
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
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public double deltaStickyStrike(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, discountingProvider);
		return deltaStickyStrike(futureOption, discountingProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the delta of the bond future option product based on the price of the underlying future.
	  /// <para>
	  /// The delta of the product is the sensitivity of the option price to the future price.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public double deltaStickyStrike(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		double strike = futureOption.StrikePrice;
		ResolvedBondFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(futureOption.Expiry, future.LastTradeDate, strike, futurePrice);
		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double delta = BlackFormulaRepository.delta(futurePrice, strike, timeToExpiry, volatility, futureOption.PutCall.Call);
		return delta;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the gamma of the bond future option product.
	  /// <para>
	  /// The gamma of the product is the sensitivity of the option delta to the future price.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public double gammaStickyStrike(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, discountingProvider);
		return gammaStickyStrike(futureOption, discountingProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the gamma of the bond future option product based on the price of the underlying future.
	  /// <para>
	  /// The gamma of the product is the sensitivity of the option delta to the future price.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public double gammaStickyStrike(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		double strike = futureOption.StrikePrice;
		ResolvedBondFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(futureOption.Expiry, future.LastTradeDate, strike, futurePrice);
		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double gamma = BlackFormulaRepository.gamma(futurePrice, strike, timeToExpiry, volatility);
		return gamma;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the theta of the bond future option product.
	  /// <para>
	  /// The theta of the product is the minus of the option price sensitivity to the time to expiry.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// </para>
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public double theta(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, discountingProvider);
		return theta(futureOption, discountingProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the theta of the bond future option product based on the price of the underlying future.
	  /// <para>
	  /// The theta of the product is minus of the option price sensitivity to the time to expiry.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public double theta(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		double strike = futureOption.StrikePrice;
		ResolvedBondFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(futureOption.Expiry, future.LastTradeDate, strike, futurePrice);
		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double theta = BlackFormulaRepository.driftlessTheta(futurePrice, strike, timeToExpiry, volatility);
		return theta;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity of the bond future option product based on curves.
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
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public PointSensitivities priceSensitivityRatesStickyStrike(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		double futurePrice = this.futurePrice(futureOption, discountingProvider);
		return priceSensitivityRatesStickyStrike(futureOption, discountingProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the bond future option product based on the price of the underlying future.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// The volatility is unchanged for a fixed strike in the sensitivity computation, hence the "StickyStrike" name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the price of the underlying future </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public PointSensitivities priceSensitivityRatesStickyStrike(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		double delta = deltaStickyStrike(futureOption, discountingProvider, volatilities, futurePrice);
		PointSensitivities futurePriceSensitivity = futurePricer.priceSensitivity(futureOption.UnderlyingFuture, discountingProvider);
		return futurePriceSensitivity.multipliedBy(delta);
	  }

	  internal PointSensitivities priceSensitivity(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BondFutureVolatilities volatilities)
	  {

		ArgChecker.isTrue(volatilities is BlackBondFutureVolatilities, "Provider must be of type BlackVolatilityBondFutureProvider");
		return priceSensitivityRatesStickyStrike(futureOption, discountingProvider, (BlackBondFutureVolatilities) volatilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity to the Black volatility used for the pricing of the bond future option.
	  /// <para>
	  /// This calculates the underlying future price using the future pricer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <returns> the sensitivity </returns>
	  public BondFutureOptionSensitivity priceSensitivityModelParamsVolatility(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities)
	  {

		double futurePrice = this.futurePrice(futureOption, discountingProvider);
		return priceSensitivityModelParamsVolatility(futureOption, discountingProvider, volatilities, futurePrice);
	  }

	  /// <summary>
	  /// Calculates the price sensitivity to the Black volatility used for the pricing of the bond future option
	  /// based on the price of the underlying future.
	  /// </summary>
	  /// <param name="futureOption">  the option product </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="volatilities">  the volatilities </param>
	  /// <param name="futurePrice">  the underlying future price </param>
	  /// <returns> the sensitivity </returns>
	  public BondFutureOptionSensitivity priceSensitivityModelParamsVolatility(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider, BlackBondFutureVolatilities volatilities, double futurePrice)
	  {

		ArgChecker.isTrue(futureOption.PremiumStyle.Equals(FutureOptionPremiumStyle.DAILY_MARGIN), "Premium style should be DAILY_MARGIN");
		double strike = futureOption.StrikePrice;
		ResolvedBondFuture future = futureOption.UnderlyingFuture;
		double volatility = volatilities.volatility(futureOption.Expiry, future.LastTradeDate, strike, futurePrice);
		double timeToExpiry = volatilities.relativeTime(futureOption.Expiry);
		double vega = BlackFormulaRepository.vega(futurePrice, strike, timeToExpiry, volatility);
		return BondFutureOptionSensitivity.of(volatilities.Name, timeToExpiry, future.LastTradeDate, strike, futurePrice, future.Currency, vega);
	  }

	  //-------------------------------------------------------------------------
	  // calculate the price of the underlying future
	  private double futurePrice(ResolvedBondFutureOption futureOption, LegalEntityDiscountingProvider discountingProvider)
	  {
		ResolvedBondFuture future = futureOption.UnderlyingFuture;
		return futurePricer.price(future, discountingProvider);
	  }

	}

}
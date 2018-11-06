using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingFxSingleProductPricer = com.opengamma.strata.pricer.fx.DiscountingFxSingleProductPricer;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;

	/// <summary>
	/// Pricer for foreign exchange vanilla option transaction products with a lognormal model.
	/// <para>
	/// This function provides the ability to price an <seealso cref="ResolvedFxVanillaOption"/>.
	/// </para>
	/// <para>
	/// All of the computation is be based on the counter currency of the underlying FX transaction.
	/// For example, price, PV and risk measures of the product will be expressed in USD for an option on EUR/USD.
	/// </para>
	/// </summary>
	public class BlackFxVanillaOptionProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BlackFxVanillaOptionProductPricer DEFAULT = new BlackFxVanillaOptionProductPricer(DiscountingFxSingleProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying FX pricer.
	  /// </summary>
	  private readonly DiscountingFxSingleProductPricer fxPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="fxPricer">  the pricer for <seealso cref="ResolvedFxSingle"/> </param>
	  public BlackFxVanillaOptionProductPricer(DiscountingFxSingleProductPricer fxPricer)
	  {
		this.fxPricer = ArgChecker.notNull(fxPricer, "fxPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the pricer used to price the underlying FX product.
	  /// </summary>
	  /// <returns> the pricer </returns>
	  internal virtual DiscountingFxSingleProductPricer DiscountingFxSingleProductPricer
	  {
		  get
		  {
			return fxPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the foreign exchange vanilla option product.
	  /// <para>
	  /// The price of the product is the value on the valuation date for one unit of the base currency 
	  /// and is expressed in the counter currency. The price does not take into account the long/short flag.
	  /// See <seealso cref="#presentValue"/> for scaling and currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the price of the product </returns>
	  public virtual double price(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingle underlying = option.Underlying;
		double forwardPrice = undiscountedPrice(option, ratesProvider, volatilities);
		double discountFactor = ratesProvider.discountFactor(option.CounterCurrency, underlying.PaymentDate);
		return discountFactor * forwardPrice;
	  }

	  /// <summary>
	  /// Calculates the present value of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// It is expressed in the counter currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double price = this.price(option, ratesProvider, volatilities);
		return CurrencyAmount.of(option.CounterCurrency, signedNotional(option) * price);
	  }

	  // the price without discounting
	  private double undiscountedPrice(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry < 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlying = option.Underlying;
		FxRate forward = fxPricer.forwardFxRate(underlying, ratesProvider);
		CurrencyPair strikePair = underlying.CurrencyPair;
		double forwardRate = forward.fxRate(strikePair);
		double strikeRate = option.Strike;
		bool isCall = option.PutCall.Call;
		if (timeToExpiry == 0d)
		{
		  return isCall ? Math.Max(forwardRate - strikeRate, 0d) : Math.Max(0d, strikeRate - forwardRate);
		}
		double volatility = volatilities.volatility(strikePair, option.Expiry, strikeRate, forwardRate);
		return BlackFormulaRepository.price(forwardRate, strikeRate, timeToExpiry, volatility, isCall);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the delta of the foreign exchange vanilla option product.
	  /// <para>
	  /// The delta is the first derivative of <seealso cref="#price"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the delta of the product </returns>
	  public virtual double delta(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxSingle underlying = option.Underlying;
		double fwdDelta = undiscountedDelta(option, ratesProvider, volatilities);
		double discountFactor = ratesProvider.discountFactor(option.CounterCurrency, underlying.PaymentDate);
		double fwdRateSpotSensitivity = fxPricer.forwardFxRateSpotSensitivity(option.PutCall.Call ? underlying : underlying.inverse(), ratesProvider);
		return fwdDelta * discountFactor * fwdRateSpotSensitivity;
	  }

	  /// <summary>
	  /// Calculates the present value delta of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value delta is the first derivative of <seealso cref="#presentValue"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value delta of the product </returns>
	  public virtual CurrencyAmount presentValueDelta(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double delta = this.delta(option, ratesProvider, volatilities);
		return CurrencyAmount.of(option.CounterCurrency, signedNotional(option) * delta);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of <seealso cref="#presentValue"/> to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The volatility is fixed in this sensitivity computation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivities presentValueSensitivityRatesStickyStrike(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		if (volatilities.relativeTime(option.Expiry) < 0d)
		{
		  return PointSensitivities.empty();
		}
		ResolvedFxSingle underlying = option.Underlying;
		double fwdDelta = undiscountedDelta(option, ratesProvider, volatilities);
		double discountFactor = ratesProvider.discountFactor(option.CounterCurrency, underlying.PaymentDate);
		double notional = signedNotional(option);
		PointSensitivityBuilder fwdSensi = fxPricer.forwardFxRatePointSensitivity(option.PutCall.Call ? underlying : underlying.inverse(), ratesProvider).multipliedBy(notional * discountFactor * fwdDelta);
		double fwdPrice = undiscountedPrice(option, ratesProvider, volatilities);
		PointSensitivityBuilder dscSensi = ratesProvider.discountFactors(option.CounterCurrency).zeroRatePointSensitivity(underlying.PaymentDate).multipliedBy(notional * fwdPrice);
		return fwdSensi.combinedWith(dscSensi).build().convertedTo(option.CounterCurrency, ratesProvider);
	  }

	  // the delta without discounting
	  private double undiscountedDelta(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry < 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlying = option.Underlying;
		FxRate forward = fxPricer.forwardFxRate(underlying, ratesProvider);
		CurrencyPair strikePair = underlying.CurrencyPair;
		double forwardRate = forward.fxRate(strikePair);
		double strikeRate = option.Strike;
		bool isCall = option.PutCall.Call;
		if (timeToExpiry == 0d)
		{
		  return isCall ? (forwardRate > strikeRate ? 1d : 0d) : (strikeRate > forwardRate ? -1d : 0d);
		}
		double volatility = volatilities.volatility(strikePair, option.Expiry, strikeRate, forwardRate);
		return BlackFormulaRepository.delta(forwardRate, strikeRate, timeToExpiry, volatility, isCall);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the gamma of the foreign exchange vanilla option product.
	  /// <para>
	  /// The gamma is the second derivative of <seealso cref="#price"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the gamma of the product </returns>
	  public virtual double gamma(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlying = option.Underlying;
		FxRate forward = fxPricer.forwardFxRate(underlying, ratesProvider);
		CurrencyPair strikePair = underlying.CurrencyPair;
		double forwardRate = forward.fxRate(strikePair);
		double strikeRate = option.Strike;
		double volatility = volatilities.volatility(strikePair, option.Expiry, strikeRate, forwardRate);
		double forwardGamma = BlackFormulaRepository.gamma(forwardRate, strikeRate, timeToExpiry, volatility);
		double discountFactor = ratesProvider.discountFactor(option.CounterCurrency, underlying.PaymentDate);
		double fwdRateSpotSensitivity = fxPricer.forwardFxRateSpotSensitivity(option.PutCall.Call ? underlying : underlying.inverse(), ratesProvider);
		return forwardGamma * discountFactor * fwdRateSpotSensitivity * fwdRateSpotSensitivity;
	  }

	  /// <summary>
	  /// Calculates the present value delta of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value gamma is the second derivative of the <seealso cref="#presentValue"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value gamma of the product </returns>
	  public virtual CurrencyAmount presentValueGamma(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double gamma = this.gamma(option, ratesProvider, volatilities);
		return CurrencyAmount.of(option.CounterCurrency, signedNotional(option) * gamma);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the vega of the foreign exchange vanilla option product.
	  /// <para>
	  /// The vega is the first derivative of <seealso cref="#price"/> with respect to volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the vega of the product </returns>
	  public virtual double vega(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlying = option.Underlying;
		FxRate forward = fxPricer.forwardFxRate(underlying, ratesProvider);
		CurrencyPair strikePair = underlying.CurrencyPair;
		double forwardRate = forward.fxRate(strikePair);
		double strikeRate = option.Strike;
		double volatility = volatilities.volatility(strikePair, option.Expiry, strikeRate, forwardRate);
		double fwdVega = BlackFormulaRepository.vega(forwardRate, strikeRate, timeToExpiry, volatility);
		double discountFactor = ratesProvider.discountFactor(option.CounterCurrency, underlying.PaymentDate);
		return discountFactor * fwdVega;
	  }

	  /// <summary>
	  /// Calculates the present value vega of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value vega is the first derivative of the <seealso cref="#presentValue"/> with respect to volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value vega of the product </returns>
	  public virtual CurrencyAmount presentValueVega(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double vega = this.vega(option, ratesProvider, volatilities);
		return CurrencyAmount.of(option.CounterCurrency, signedNotional(option) * vega);
	  }

	  /// <summary>
	  /// Computes the present value sensitivity to the black volatility used in the pricing.
	  /// <para>
	  /// The result is a single sensitivity to the volatility used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		if (volatilities.relativeTime(option.Expiry) <= 0d)
		{
		  return PointSensitivityBuilder.none();
		}
		ResolvedFxSingle underlying = option.Underlying;
		FxRate forward = fxPricer.forwardFxRate(underlying, ratesProvider);
		CurrencyPair strikePair = underlying.CurrencyPair;
		CurrencyAmount valueVega = presentValueVega(option, ratesProvider, volatilities);
		return FxOptionSensitivity.of(volatilities.Name, strikePair, volatilities.relativeTime(option.Expiry), option.Strike, forward.fxRate(strikePair), valueVega.Currency, valueVega.Amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the Black theta of the foreign exchange vanilla option product.
	  /// <para>
	  /// The theta is the negative of the first derivative of <seealso cref="#price"/> with respect to time parameter
	  /// in Black formula (the discounted driftless theta).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the theta of the product </returns>
	  public virtual double theta(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlying = option.Underlying;
		FxRate forward = fxPricer.forwardFxRate(underlying, ratesProvider);
		CurrencyPair strikePair = underlying.CurrencyPair;
		double forwardRate = forward.fxRate(strikePair);
		double strikeRate = option.Strike;
		double volatility = volatilities.volatility(strikePair, option.Expiry, strikeRate, forwardRate);
		double fwdTheta = BlackFormulaRepository.driftlessTheta(forwardRate, strikeRate, timeToExpiry, volatility);
		double discountFactor = ratesProvider.discountFactor(option.CounterCurrency, underlying.PaymentDate);
		return discountFactor * fwdTheta;
	  }

	  /// <summary>
	  /// Calculates the present value theta of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value theta is the negative of the first derivative of <seealso cref="#presentValue"/> with time parameter
	  /// in Black formula, i.e., the driftless theta of the present value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value vega of the product </returns>
	  public virtual CurrencyAmount presentValueTheta(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double theta = this.theta(option, ratesProvider, volatilities);
		return CurrencyAmount.of(option.CounterCurrency, signedNotional(option) * theta);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the implied Black volatility of the foreign exchange vanilla option product.
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the implied volatility of the product </returns>
	  /// <exception cref="IllegalArgumentException"> if the option has expired </exception>
	  public virtual double impliedVolatility(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  throw new System.ArgumentException("valuation is after option's expiry.");
		}
		FxRate forward = fxPricer.forwardFxRate(option.Underlying, ratesProvider);
		CurrencyPair strikePair = option.Underlying.CurrencyPair;
		return volatilities.volatility(strikePair, option.Expiry, option.Strike, forward.fxRate(strikePair));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the foreign exchange vanilla option product.
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		CurrencyPair strikePair = option.Underlying.CurrencyPair;
		double price = this.price(option, ratesProvider, volatilities);
		double delta = this.delta(option, ratesProvider, volatilities);
		double spot = ratesProvider.fxRate(strikePair);
		double signedNotional = this.signedNotional(option);
		CurrencyAmount domestic = CurrencyAmount.of(strikePair.Counter, (price - delta * spot) * signedNotional);
		CurrencyAmount foreign = CurrencyAmount.of(strikePair.Base, delta * signedNotional);
		return MultiCurrencyAmount.of(domestic, foreign);
	  }

	  //-------------------------------------------------------------------------
	  // signed notional amount to computed present value and value Greeks
	  private double signedNotional(ResolvedFxVanillaOption option)
	  {
		return (option.LongShort.Long ? 1d : -1d) * Math.Abs(option.Underlying.BaseCurrencyPayment.Amount);
	  }

	}

}
using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using BlackBarrierPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackBarrierPriceFormulaRepository;
	using BlackOneTouchAssetPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackOneTouchAssetPriceFormulaRepository;
	using BlackOneTouchCashPriceFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackOneTouchCashPriceFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Pricer for FX barrier option products in Black-Scholes world.
	/// <para>
	/// This function provides the ability to price an <seealso cref="ResolvedFxSingleBarrierOption"/>.
	/// </para>
	/// <para>
	/// All of the computation is be based on the counter currency of the underlying FX transaction.
	/// For example, price, PV and risk measures of the product will be expressed in USD for an option on EUR/USD.
	/// </para>
	/// </summary>
	public class BlackFxSingleBarrierOptionProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly BlackFxSingleBarrierOptionProductPricer DEFAULT = new BlackFxSingleBarrierOptionProductPricer();

	  /// <summary>
	  /// Pricer for barrier option without rebate.
	  /// </summary>
	  private static readonly BlackBarrierPriceFormulaRepository BARRIER_PRICER = new BlackBarrierPriceFormulaRepository();
	  /// <summary>
	  /// Pricer for rebate.
	  /// </summary>
	  private static readonly BlackOneTouchAssetPriceFormulaRepository ASSET_REBATE_PRICER = new BlackOneTouchAssetPriceFormulaRepository();
	  /// <summary>
	  /// Pricer for rebate.
	  /// </summary>
	  private static readonly BlackOneTouchCashPriceFormulaRepository CASH_REBATE_PRICER = new BlackOneTouchCashPriceFormulaRepository();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public BlackFxSingleBarrierOptionProductPricer()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX barrier option product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// It is expressed in the counter currency.
	  /// </para>
	  /// <para>
	  /// The volatility used in this computation is the Black implied volatility at expiry time and strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double price = this.price(option, ratesProvider, volatilities);
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		return CurrencyAmount.of(underlyingOption.CounterCurrency, signedNotional(underlyingOption) * price);
	  }

	  /// <summary>
	  /// Calculates the price of the FX barrier option product.
	  /// <para>
	  /// The price of the product is the value on the valuation date for one unit of the base currency 
	  /// and is expressed in the counter currency. The price does not take into account the long/short flag.
	  /// See <seealso cref="#presentValue"/> for scaling and currency.
	  /// </para>
	  /// <para>
	  /// The volatility used in this computation is the Black implied volatility at expiry time and strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the price of the product </returns>
	  public virtual double price(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		validate(option, ratesProvider, volatilities);
		SimpleConstantContinuousBarrier barrier = (SimpleConstantContinuousBarrier) option.Barrier;
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		if (volatilities.relativeTime(underlyingOption.Expiry) < 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlyingFx = underlyingOption.Underlying;
		Currency ccyBase = underlyingFx.BaseCurrencyPayment.Currency;
		Currency ccyCounter = underlyingFx.CounterCurrencyPayment.Currency;
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		DiscountFactors baseDiscountFactors = ratesProvider.discountFactors(ccyBase);
		DiscountFactors counterDiscountFactors = ratesProvider.discountFactors(ccyCounter);

		double rateBase = baseDiscountFactors.zeroRate(underlyingFx.PaymentDate);
		double rateCounter = counterDiscountFactors.zeroRate(underlyingFx.PaymentDate);
		double costOfCarry = rateCounter - rateBase;
		double dfBase = baseDiscountFactors.discountFactor(underlyingFx.PaymentDate);
		double dfCounter = counterDiscountFactors.discountFactor(underlyingFx.PaymentDate);
		double todayFx = ratesProvider.fxRate(currencyPair);
		double strike = underlyingOption.Strike;
		double forward = todayFx * dfBase / dfCounter;
		double volatility = volatilities.volatility(currencyPair, underlyingOption.Expiry, strike, forward);
		double timeToExpiry = volatilities.relativeTime(underlyingOption.Expiry);
		double price = BARRIER_PRICER.price(todayFx, strike, timeToExpiry, costOfCarry, rateCounter, volatility, underlyingOption.PutCall.Call, barrier);
		if (option.Rebate.Present)
		{
		  CurrencyAmount rebate = option.Rebate.get();
		  double priceRebate = rebate.Currency.Equals(ccyCounter) ? CASH_REBATE_PRICER.price(todayFx, timeToExpiry, costOfCarry, rateCounter, volatility, barrier.inverseKnockType()) : ASSET_REBATE_PRICER.price(todayFx, timeToExpiry, costOfCarry, rateCounter, volatility, barrier.inverseKnockType());
		  price += priceRebate * rebate.Amount / Math.Abs(underlyingFx.BaseCurrencyPayment.Amount);
		}
		return price;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the FX barrier option product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of <seealso cref="#presentValue"/> to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The volatility is fixed in this sensitivity computation, i.e., sticky-strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyStrike(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		if (volatilities.relativeTime(underlyingOption.Expiry) <= 0d)
		{
		  return PointSensitivityBuilder.none();
		}
		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		ResolvedFxSingle underlyingFx = underlyingOption.Underlying;
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		double signedNotional = this.signedNotional(underlyingOption);
		double counterYearFraction = ratesProvider.discountFactors(currencyPair.Counter).relativeYearFraction(underlyingFx.PaymentDate);
		ZeroRateSensitivity counterSensi = ZeroRateSensitivity.of(currencyPair.Counter, counterYearFraction, signedNotional * (priceDerivatives.getDerivative(2) + priceDerivatives.getDerivative(3)));
		double baseYearFraction = ratesProvider.discountFactors(currencyPair.Base).relativeYearFraction(underlyingFx.PaymentDate);
		ZeroRateSensitivity baseSensi = ZeroRateSensitivity.of(currencyPair.Base, baseYearFraction, currencyPair.Counter, -priceDerivatives.getDerivative(3) * signedNotional);
		return counterSensi.combinedWith(baseSensi);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value delta of the FX barrier option product.
	  /// <para>
	  /// The present value delta is the first derivative of <seealso cref="#presentValue"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value delta of the product </returns>
	  public virtual CurrencyAmount presentValueDelta(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double delta = this.delta(option, ratesProvider, volatilities);
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		return CurrencyAmount.of(underlyingOption.CounterCurrency, signedNotional(underlyingOption) * delta);
	  }

	  /// <summary>
	  /// Calculates the delta of the FX barrier option product.
	  /// <para>
	  /// The delta is the first derivative of <seealso cref="#price"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the delta of the product </returns>
	  public virtual double delta(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		if (volatilities.relativeTime(option.UnderlyingOption.Expiry) < 0d)
		{
		  return 0d;
		}
		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		return priceDerivatives.getDerivative(0);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value gamma of the FX barrier option product.
	  /// <para>
	  /// The present value gamma is the second derivative of <seealso cref="#presentValue"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value gamma of the product </returns>
	  public virtual CurrencyAmount presentValueGamma(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double gamma = this.gamma(option, ratesProvider, volatilities);
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		return CurrencyAmount.of(underlyingOption.CounterCurrency, signedNotional(underlyingOption) * gamma);
	  }

	  /// <summary>
	  /// Calculates the gamma of the FX barrier option product.
	  /// <para>
	  /// The delta is the second derivative of <seealso cref="#price"/> with respect to spot.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the gamma of the product </returns>
	  public virtual double gamma(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		return priceDerivatives.getDerivative(6);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the present value sensitivity to the black volatility used in the pricing.
	  /// <para>
	  /// The result is a single sensitivity to the volatility used. This is also called Black vega.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		if (volatilities.relativeTime(underlyingOption.Expiry) <= 0d)
		{
		  return PointSensitivityBuilder.none();
		}
		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		ResolvedFxSingle underlyingFx = underlyingOption.Underlying;
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		Currency ccyBase = currencyPair.Base;
		Currency ccyCounter = currencyPair.Counter;
		double dfBase = ratesProvider.discountFactor(ccyBase, underlyingFx.PaymentDate);
		double dfCounter = ratesProvider.discountFactor(ccyCounter, underlyingFx.PaymentDate);
		double todayFx = ratesProvider.fxRate(currencyPair);
		double forward = todayFx * dfBase / dfCounter;
		return FxOptionSensitivity.of(volatilities.Name, currencyPair, volatilities.relativeTime(underlyingOption.Expiry), underlyingOption.Strike, forward, ccyCounter, priceDerivatives.getDerivative(4) * signedNotional(underlyingOption));
	  }

	  /// <summary>
	  /// Calculates the vega of the FX barrier option product.
	  /// <para>
	  /// The delta is the first derivative of <seealso cref="#price"/> with respect to Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the vega of the product </returns>
	  public virtual double vega(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		return priceDerivatives.getDerivative(4);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value theta of the FX barrier option product.
	  /// <para>
	  /// The present value theta is the negative of the first derivative of <seealso cref="#presentValue"/> with time parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value theta of the product </returns>
	  public virtual CurrencyAmount presentValueTheta(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double theta = this.theta(option, ratesProvider, volatilities);
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		return CurrencyAmount.of(underlyingOption.CounterCurrency, signedNotional(underlyingOption) * theta);
	  }

	  /// <summary>
	  /// Calculates the theta of the FX barrier option product.
	  /// <para>
	  /// The theta is the negative of the first derivative of <seealso cref="#price"/> with respect to time parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the theta of the product </returns>
	  public virtual double theta(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		return -priceDerivatives.getDerivative(5);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the FX barrier option product.
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		if (volatilities.relativeTime(underlyingOption.Expiry) < 0d)
		{
		  return MultiCurrencyAmount.empty();
		}
		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities);
		double price = priceDerivatives.Value;
		double delta = priceDerivatives.getDerivative(0);
		CurrencyPair currencyPair = underlyingOption.Underlying.CurrencyPair;
		double todayFx = ratesProvider.fxRate(currencyPair);
		double signedNotional = this.signedNotional(underlyingOption);
		CurrencyAmount domestic = CurrencyAmount.of(currencyPair.Counter, (price - delta * todayFx) * signedNotional);
		CurrencyAmount foreign = CurrencyAmount.of(currencyPair.Base, delta * signedNotional);
		return MultiCurrencyAmount.of(domestic, foreign);
	  }

	  //-------------------------------------------------------------------------
	  //  The derivatives are [0] spot, [1] strike, [2] rate, [3] cost-of-carry, [4] volatility, [5] timeToExpiry, [6] spot twice
	  private ValueDerivatives priceDerivatives(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		validate(option, ratesProvider, volatilities);
		SimpleConstantContinuousBarrier barrier = (SimpleConstantContinuousBarrier) option.Barrier;
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		double[] derivatives = new double[7];
		if (volatilities.relativeTime(underlyingOption.Expiry) < 0d)
		{
		  return ValueDerivatives.of(0d, DoubleArray.ofUnsafe(derivatives));
		}
		ResolvedFxSingle underlyingFx = underlyingOption.Underlying;
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		Currency ccyBase = currencyPair.Base;
		Currency ccyCounter = currencyPair.Counter;
		DiscountFactors baseDiscountFactors = ratesProvider.discountFactors(ccyBase);
		DiscountFactors counterDiscountFactors = ratesProvider.discountFactors(ccyCounter);

		double rateBase = baseDiscountFactors.zeroRate(underlyingFx.PaymentDate);
		double rateCounter = counterDiscountFactors.zeroRate(underlyingFx.PaymentDate);
		double costOfCarry = rateCounter - rateBase;
		double dfBase = baseDiscountFactors.discountFactor(underlyingFx.PaymentDate);
		double dfCounter = counterDiscountFactors.discountFactor(underlyingFx.PaymentDate);
		double todayFx = ratesProvider.fxRate(currencyPair);
		double strike = underlyingOption.Strike;
		double forward = todayFx * dfBase / dfCounter;
		double volatility = volatilities.volatility(currencyPair, underlyingOption.Expiry, strike, forward);
		double timeToExpiry = volatilities.relativeTime(underlyingOption.Expiry);
		ValueDerivatives valueDerivatives = BARRIER_PRICER.priceAdjoint(todayFx, strike, timeToExpiry, costOfCarry, rateCounter, volatility, underlyingOption.PutCall.Call, barrier);
		if (!option.Rebate.Present)
		{
		  return valueDerivatives;
		}
		CurrencyAmount rebate = option.Rebate.get();
		ValueDerivatives valueDerivativesRebate = rebate.Currency.Equals(ccyCounter) ? CASH_REBATE_PRICER.priceAdjoint(todayFx, timeToExpiry, costOfCarry, rateCounter, volatility, barrier.inverseKnockType()) : ASSET_REBATE_PRICER.priceAdjoint(todayFx, timeToExpiry, costOfCarry, rateCounter, volatility, barrier.inverseKnockType());
		double rebateRate = rebate.Amount / Math.Abs(underlyingFx.BaseCurrencyPayment.Amount);
		double price = valueDerivatives.Value + rebateRate * valueDerivativesRebate.Value;
		derivatives[0] = valueDerivatives.getDerivative(0) + rebateRate * valueDerivativesRebate.getDerivative(0);
		derivatives[1] = valueDerivatives.getDerivative(1);
		for (int i = 2; i < 7; ++i)
		{
		  derivatives[i] = valueDerivatives.getDerivative(i) + rebateRate * valueDerivativesRebate.getDerivative(i - 1);
		}
		return ValueDerivatives.of(price, DoubleArray.ofUnsafe(derivatives));
	  }

	  //-------------------------------------------------------------------------
	  private void validate(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ArgChecker.isTrue(option.Barrier is SimpleConstantContinuousBarrier, "Barrier should be SimpleConstantContinuousBarrier");
		ArgChecker.isTrue(ratesProvider.ValuationDate.isEqual(volatilities.ValuationDateTime.toLocalDate()), "Volatility and rate data must be for the same date");
	  }

	  // signed notional amount to computed present value and value Greeks
	  private double signedNotional(ResolvedFxVanillaOption option)
	  {
		return (option.LongShort.Long ? 1d : -1d) * Math.Abs(option.Underlying.BaseCurrencyPayment.Amount);
	  }

	}

}
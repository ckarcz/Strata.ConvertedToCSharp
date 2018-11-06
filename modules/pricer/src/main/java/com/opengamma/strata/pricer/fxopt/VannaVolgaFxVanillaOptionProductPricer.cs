using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DiscountingFxSingleProductPricer = com.opengamma.strata.pricer.fx.DiscountingFxSingleProductPricer;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;

	/// <summary>
	/// Pricing method for vanilla Forex option transactions with Vanna-Volga method.
	/// <para>
	/// The volatilities are expressed using {@code BlackFxOptionSmileVolatilities}. 
	/// Each smile of the term structure consists of 3 data points, where the middle point corresponds to ATM volatility.
	/// </para>
	/// <para>
	/// Reference: The vanna-volga method for implied volatilities (2007), A. Castagna and F. Mercurio, Risk, 106-111, January 2007.
	/// OG implementation: Vanna-volga method for Forex options, version 1.0, June 2012.
	/// </para>
	/// </summary>
	public class VannaVolgaFxVanillaOptionProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly VannaVolgaFxVanillaOptionProductPricer DEFAULT = new VannaVolgaFxVanillaOptionProductPricer(DiscountingFxSingleProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying FX pricer.
	  /// </summary>
	  private readonly DiscountingFxSingleProductPricer fxPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="fxPricer">  the pricer for <seealso cref="ResolvedFxSingle"/> </param>
	  public VannaVolgaFxVanillaOptionProductPricer(DiscountingFxSingleProductPricer fxPricer)
	  {
		this.fxPricer = ArgChecker.notNull(fxPricer, "fxPricer");
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
	  public virtual double price(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return 0d;
		}
		ResolvedFxSingle underlyingFx = option.Underlying;
		Currency ccyCounter = option.CounterCurrency;
		double df = ratesProvider.discountFactor(ccyCounter, underlyingFx.PaymentDate);
		FxRate forward = fxPricer.forwardFxRate(underlyingFx, ratesProvider);
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		double forwardRate = forward.fxRate(currencyPair);
		double strikeRate = option.Strike;
		bool isCall = option.PutCall.Call;
		SmileDeltaParameters smileAtTime = volatilities.Smile.smileForExpiry(timeToExpiry);
		double[] strikes = smileAtTime.strike(forwardRate).toArray();
		double[] vols = smileAtTime.Volatility.toArray();
		double volAtm = vols[1];
		double[] x = vannaVolgaWeights(forwardRate, strikeRate, timeToExpiry, volAtm, strikes);
		double priceFwd = BlackFormulaRepository.price(forwardRate, strikeRate, timeToExpiry, volAtm, isCall);
		for (int i = 0; i < 3; i += 2)
		{
		  double priceFwdAtm = BlackFormulaRepository.price(forwardRate, strikes[i], timeToExpiry, volAtm, isCall);
		  double priceFwdSmile = BlackFormulaRepository.price(forwardRate, strikes[i], timeToExpiry, vols[i], isCall);
		  priceFwd += x[i] * (priceFwdSmile - priceFwdAtm);
		}
		return df * priceFwd;
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
	  public virtual CurrencyAmount presentValue(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		double price = this.price(option, ratesProvider, volatilities);
		return CurrencyAmount.of(option.CounterCurrency, signedNotional(option) * price);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the foreign exchange vanilla option product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of <seealso cref="#presentValue"/> to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The implied strikes and weights are fixed in this sensitivity computation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityRatesStickyStrike(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return PointSensitivityBuilder.none();
		}
		ResolvedFxSingle underlyingFx = option.Underlying;
		Currency ccyCounter = option.CounterCurrency;
		double df = ratesProvider.discountFactor(ccyCounter, underlyingFx.PaymentDate);
		FxRate forward = fxPricer.forwardFxRate(underlyingFx, ratesProvider);
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		double forwardRate = forward.fxRate(currencyPair);
		double strikeRate = option.Strike;
		bool isCall = option.PutCall.Call;
		SmileDeltaParameters smileAtTime = volatilities.Smile.smileForExpiry(timeToExpiry);
		double[] strikes = smileAtTime.strike(forwardRate).toArray();
		double[] vols = smileAtTime.Volatility.toArray();
		double volAtm = vols[1];
		double[] x = vannaVolgaWeights(forwardRate, strikeRate, timeToExpiry, volAtm, strikes);
		double priceFwd = BlackFormulaRepository.price(forwardRate, strikeRate, timeToExpiry, volAtm, isCall);
		double deltaFwd = BlackFormulaRepository.delta(forwardRate, strikeRate, timeToExpiry, volAtm, isCall);
		for (int i = 0; i < 3; i += 2)
		{
		  double priceFwdAtm = BlackFormulaRepository.price(forwardRate, strikes[i], timeToExpiry, volAtm, isCall);
		  double priceFwdSmile = BlackFormulaRepository.price(forwardRate, strikes[i], timeToExpiry, vols[i], isCall);
		  priceFwd += x[i] * (priceFwdSmile - priceFwdAtm);
		  double deltaFwdAtm = BlackFormulaRepository.delta(forwardRate, strikes[i], timeToExpiry, volAtm, isCall);
		  double deltaFwdSmile = BlackFormulaRepository.delta(forwardRate, strikes[i], timeToExpiry, vols[i], isCall);
		  deltaFwd += x[i] * (deltaFwdSmile - deltaFwdAtm);
		}
		double signedNotional = this.signedNotional(option);
		PointSensitivityBuilder dfSensi = ratesProvider.discountFactors(ccyCounter).zeroRatePointSensitivity(underlyingFx.PaymentDate).multipliedBy(priceFwd * signedNotional);
		PointSensitivityBuilder fwdSensi = fxPricer.forwardFxRatePointSensitivity(option.PutCall.Call ? underlyingFx : underlyingFx.inverse(), ratesProvider).multipliedBy(df * deltaFwd * signedNotional);
		return dfSensi.combinedWith(fwdSensi);
	  }

	  /// <summary>
	  /// Computes the present value sensitivity to the black volatilities used in the pricing.
	  /// <para>
	  /// The implied strikes and weights are fixed in this sensitivity computation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value sensitivity </returns>
	  public virtual PointSensitivityBuilder presentValueSensitivityModelParamsVolatility(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return PointSensitivityBuilder.none();
		}
		ResolvedFxSingle underlyingFx = option.Underlying;
		Currency ccyCounter = option.CounterCurrency;
		double df = ratesProvider.discountFactor(ccyCounter, underlyingFx.PaymentDate);
		FxRate forward = fxPricer.forwardFxRate(underlyingFx, ratesProvider);
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		double forwardRate = forward.fxRate(currencyPair);
		double strikeRate = option.Strike;
		SmileDeltaParameters smileAtTime = volatilities.Smile.smileForExpiry(timeToExpiry);
		double[] strikes = smileAtTime.strike(forwardRate).toArray();
		double[] vols = smileAtTime.Volatility.toArray();
		double volAtm = vols[1];
		double[] x = vannaVolgaWeights(forwardRate, strikeRate, timeToExpiry, volAtm, strikes);
		double vegaAtm = BlackFormulaRepository.vega(forwardRate, strikeRate, timeToExpiry, volAtm);
		double signedNotional = this.signedNotional(option);
		PointSensitivityBuilder sensiSmile = PointSensitivityBuilder.none();
		for (int i = 0; i < 3; i += 2)
		{
		  double vegaFwdAtm = BlackFormulaRepository.vega(forwardRate, strikes[i], timeToExpiry, volAtm);
		  vegaAtm -= x[i] * vegaFwdAtm;
		  double vegaFwdSmile = BlackFormulaRepository.vega(forwardRate, strikes[i], timeToExpiry, vols[i]);
		  sensiSmile = sensiSmile.combinedWith(FxOptionSensitivity.of(volatilities.Name, currencyPair, timeToExpiry, strikes[i], forwardRate, ccyCounter, df * signedNotional * x[i] * vegaFwdSmile));
		}
		FxOptionSensitivity sensiAtm = FxOptionSensitivity.of(volatilities.Name, currencyPair, timeToExpiry, strikes[1], forwardRate, ccyCounter, df * signedNotional * vegaAtm);
		return sensiAtm.combinedWith(sensiSmile);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the foreign exchange vanilla option product.
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		if (timeToExpiry <= 0d)
		{
		  return MultiCurrencyAmount.empty();
		}
		ResolvedFxSingle underlyingFx = option.Underlying;
		Currency ccyCounter = option.CounterCurrency;
		double df = ratesProvider.discountFactor(ccyCounter, underlyingFx.PaymentDate);
		FxRate forward = fxPricer.forwardFxRate(underlyingFx, ratesProvider);
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		double spot = ratesProvider.fxRate(currencyPair);
		double forwardRate = forward.fxRate(currencyPair);
		double fwdRateSpotSensitivity = fxPricer.forwardFxRateSpotSensitivity(option.PutCall.Call ? underlyingFx : underlyingFx.inverse(), ratesProvider);
		double strikeRate = option.Strike;
		bool isCall = option.PutCall.Call;
		SmileDeltaParameters smileAtTime = volatilities.Smile.smileForExpiry(timeToExpiry);
		double[] strikes = smileAtTime.strike(forwardRate).toArray();
		double[] vols = smileAtTime.Volatility.toArray();
		double volAtm = vols[1];
		double[] x = vannaVolgaWeights(forwardRate, strikeRate, timeToExpiry, volAtm, strikes);
		double priceFwd = BlackFormulaRepository.price(forwardRate, strikeRate, timeToExpiry, volAtm, isCall);
		double deltaFwd = BlackFormulaRepository.delta(forwardRate, strikeRate, timeToExpiry, volAtm, isCall);
		for (int i = 0; i < 3; i += 2)
		{
		  double priceFwdAtm = BlackFormulaRepository.price(forwardRate, strikes[i], timeToExpiry, volAtm, isCall);
		  double priceFwdSmile = BlackFormulaRepository.price(forwardRate, strikes[i], timeToExpiry, vols[i], isCall);
		  priceFwd += x[i] * (priceFwdSmile - priceFwdAtm);
		  double deltaFwdAtm = BlackFormulaRepository.delta(forwardRate, strikes[i], timeToExpiry, volAtm, isCall);
		  double deltaFwdSmile = BlackFormulaRepository.delta(forwardRate, strikes[i], timeToExpiry, vols[i], isCall);
		  deltaFwd += x[i] * (deltaFwdSmile - deltaFwdAtm);
		}
		double price = df * priceFwd;
		double delta = df * deltaFwd * fwdRateSpotSensitivity;
		double signedNotional = this.signedNotional(option);
		CurrencyAmount domestic = CurrencyAmount.of(currencyPair.Counter, (price - delta * spot) * signedNotional);
		CurrencyAmount foreign = CurrencyAmount.of(currencyPair.Base, delta * signedNotional);
		return MultiCurrencyAmount.of(domestic, foreign);
	  }

	  //-------------------------------------------------------------------------
	  // signed notional amount to computed present value and value Greeks
	  private double signedNotional(ResolvedFxVanillaOption option)
	  {
		return (option.LongShort.Long ? 1d : -1d) * Math.Abs(option.Underlying.BaseCurrencyPayment.Amount);
	  }

	  private double[] vannaVolgaWeights(double forward, double strike, double timeToExpiry, double volATM, double[] strikesReference)
	  {

		double lnk21 = Math.Log(strikesReference[1] / strikesReference[0]);
		double lnk31 = Math.Log(strikesReference[2] / strikesReference[0]);
		double lnk32 = Math.Log(strikesReference[2] / strikesReference[1]);
		double[] lnk = new double[3];
		for (int loopvv = 0; loopvv < 3; loopvv++)
		{
		  lnk[loopvv] = Math.Log(strikesReference[loopvv] / strike);
		}
		double[] x = new double[3];
		double vega0 = BlackFormulaRepository.vega(forward, strikesReference[0], timeToExpiry, volATM);
		double vegaFlat = BlackFormulaRepository.vega(forward, strike, timeToExpiry, volATM);
		double vega2 = BlackFormulaRepository.vega(forward, strikesReference[2], timeToExpiry, volATM);
		x[0] = vegaFlat * lnk[1] * lnk[2] / (vega0 * lnk21 * lnk31);
		x[2] = vegaFlat * lnk[0] * lnk[1] / (vega2 * lnk31 * lnk32);
		return x;
	  }

	  private void validate(RatesProvider ratesProvider, BlackFxOptionSmileVolatilities volatilities)
	  {
		ArgChecker.isTrue(volatilities.ValuationDateTime.toLocalDate().Equals(ratesProvider.ValuationDate), "volatility and rate data must be for the same date");
		ArgChecker.isTrue(volatilities.Smile.StrikeCount == 3, "the number of data points must be 3");
	  }
	}

}
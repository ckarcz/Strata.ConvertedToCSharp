using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleMath = com.google.common.math.DoubleMath;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ConstantContinuousSingleBarrierKnockoutFunction = com.opengamma.strata.pricer.impl.tree.ConstantContinuousSingleBarrierKnockoutFunction;
	using EuropeanVanillaOptionFunction = com.opengamma.strata.pricer.impl.tree.EuropeanVanillaOptionFunction;
	using TrinomialTree = com.opengamma.strata.pricer.impl.tree.TrinomialTree;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxSingleBarrierOption = com.opengamma.strata.product.fxopt.ResolvedFxSingleBarrierOption;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;
	using SimpleConstantContinuousBarrier = com.opengamma.strata.product.option.SimpleConstantContinuousBarrier;

	/// <summary>
	/// Pricer for FX barrier option products under implied trinomial tree.
	/// <para>
	/// This function provides the ability to price an <seealso cref="ResolvedFxSingleBarrierOption"/>.
	/// </para>
	/// <para>
	/// All of the computation is be based on the counter currency of the underlying FX transaction.
	/// For example, price, PV and risk measures of the product will be expressed in USD for an option on EUR/USD.
	/// </para>
	/// </summary>
	public class ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer
	{

	  /// <summary>
	  /// The trinomial tree.
	  /// </summary>
	  private static readonly TrinomialTree TREE = new TrinomialTree();
	  /// <summary>
	  /// Small parameter.
	  /// </summary>
	  private const double SMALL = 1.0e-12;
	  /// <summary>
	  /// Default number of time steps.
	  /// </summary>
	  private const int NUM_STEPS_DEFAULT = 51;

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer DEFAULT = new ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer(NUM_STEPS_DEFAULT);

	  /// <summary>
	  /// Number of time steps.
	  /// </summary>
	  private readonly ImpliedTrinomialTreeFxOptionCalibrator calibrator;

	  /// <summary>
	  /// Pricer with the default number of time steps.
	  /// </summary>
	  public ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer() : this(NUM_STEPS_DEFAULT)
	  {
	  }

	  /// <summary>
	  /// Pricer with the specified number of time steps.
	  /// </summary>
	  /// <param name="nSteps">  number of time steps </param>
	  public ImpliedTrinomialTreeFxSingleBarrierOptionProductPricer(int nSteps)
	  {
		this.calibrator = new ImpliedTrinomialTreeFxOptionCalibrator(nSteps);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the calibrator.
	  /// </summary>
	  /// <returns> the calibrator </returns>
	  public virtual ImpliedTrinomialTreeFxOptionCalibrator Calibrator
	  {
		  get
		  {
			return calibrator;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the FX barrier option product.
	  /// <para>
	  /// The price of the product is the value on the valuation date for one unit of the base currency 
	  /// and is expressed in the counter currency. The price does not take into account the long/short flag.
	  /// See <seealso cref="#presentValue(ResolvedFxSingleBarrierOption, RatesProvider, BlackFxOptionVolatilities) presentValue"/> 
	  /// for scaling and currency.
	  /// </para>
	  /// <para>
	  /// The trinomial tree is first calibrated to Black volatilities, 
	  /// then the price is computed based on the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the price of the product </returns>
	  public virtual double price(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		RecombiningTrinomialTreeData treeData = calibrator.calibrateTrinomialTree(option.UnderlyingOption, ratesProvider, volatilities);
		return price(option, ratesProvider, volatilities, treeData);
	  }

	  /// <summary>
	  /// Calculates the price of the FX barrier option product.
	  /// <para>
	  /// The price of the product is the value on the valuation date for one unit of the base currency 
	  /// and is expressed in the counter currency. The price does not take into account the long/short flag.
	  /// See <seealso cref="#presentValue(ResolvedFxSingleBarrierOption, RatesProvider, BlackFxOptionVolatilities, RecombiningTrinomialTreeData) presnetValue"/> 
	  /// for scaling and currency.
	  /// </para>
	  /// <para>
	  /// This assumes the tree is already calibrated and the tree data is stored as {@code RecombiningTrinomialTreeData}.
	  /// The tree data should be consistent with the pricer and other inputs, see <seealso cref="#validateData"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <param name="treeData">  the trinomial tree data </param>
	  /// <returns> the price of the product </returns>
	  public virtual double price(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities, RecombiningTrinomialTreeData treeData)
	  {

		return priceDerivatives(option, ratesProvider, volatilities, treeData).Value;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FX barrier option product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// It is expressed in the counter currency.
	  /// </para>
	  /// <para>
	  /// The trinomial tree is first calibrated to Black volatilities, 
	  /// then the price is computed based on the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		RecombiningTrinomialTreeData treeData = calibrator.calibrateTrinomialTree(option.UnderlyingOption, ratesProvider, volatilities);
		return presentValue(option, ratesProvider, volatilities, treeData);
	  }

	  /// <summary>
	  /// Calculates the present value of the FX barrier option product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// It is expressed in the counter currency.
	  /// </para>
	  /// <para>
	  /// This assumes the tree is already calibrated and the tree data is stored as {@code RecombiningTrinomialTreeData}.
	  /// The tree data should be consistent with the pricer and other inputs, see <seealso cref="#validateData"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <param name="treeData">  the trinomial tree data </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities, RecombiningTrinomialTreeData treeData)
	  {

		double price = this.price(option, ratesProvider, volatilities, treeData);
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		return CurrencyAmount.of(underlyingOption.CounterCurrency, signedNotional(underlyingOption) * price);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the FX barrier option product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of <seealso cref="#presentValue"/> to
	  /// the underlying curve parameters.
	  /// </para>
	  /// <para>
	  /// The sensitivity is computed by bump and re-price.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyParameterSensitivities presentValueSensitivityRates(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		RecombiningTrinomialTreeData baseTreeData = calibrator.calibrateTrinomialTree(option.UnderlyingOption, ratesProvider, volatilities);
		return presentValueSensitivityRates(option, ratesProvider, volatilities, baseTreeData);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the FX barrier option product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of <seealso cref="#presentValue"/> to
	  /// the underlying curve parameters.
	  /// </para>
	  /// <para>
	  /// The sensitivity is computed by bump and re-price.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <param name="baseTreeData">  the trinomial tree data </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyParameterSensitivities presentValueSensitivityRates(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities, RecombiningTrinomialTreeData baseTreeData)
	  {

		ArgChecker.isTrue(baseTreeData.NumberOfSteps == calibrator.NumberOfSteps, "the number of steps mismatch between pricer and trinomial tree data");
		double shift = 1.0e-5;
		CurrencyAmount pvBase = presentValue(option, ratesProvider, volatilities, baseTreeData);
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		ResolvedFxSingle underlyingFx = underlyingOption.Underlying;
		CurrencyPair currencyPair = underlyingFx.CurrencyPair;
		ImmutableRatesProvider immRatesProvider = ratesProvider.toImmutableRatesProvider();
		ImmutableMap<Currency, Curve> baseCurves = immRatesProvider.DiscountCurves;
		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();

		foreach (KeyValuePair<Currency, Curve> entry in baseCurves.entrySet())
		{
		  if (currencyPair.contains(entry.Key))
		  {
			Curve curve = entry.Value;
			int nParams = curve.ParameterCount;
			DoubleArray sensitivity = DoubleArray.of(nParams, i =>
			{
			Curve dscBumped = curve.withParameter(i, curve.getParameter(i) + shift);
			IDictionary<Currency, Curve> mapBumped = new Dictionary<Currency, Curve>(baseCurves);
			mapBumped[entry.Key] = dscBumped;
			ImmutableRatesProvider providerDscBumped = immRatesProvider.toBuilder().discountCurves(mapBumped).build();
			double pvBumped = presentValue(option, providerDscBumped, volatilities).Amount;
			return (pvBumped - pvBase.Amount) / shift;
			});
			result = result.combinedWith(curve.createParameterSensitivity(pvBase.Currency, sensitivity));
		  }
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the currency exposure of the FX barrier option product.
	  /// <para>
	  /// The trinomial tree is first calibrated to Black volatilities, 
	  /// then the price is computed based on the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		RecombiningTrinomialTreeData treeData = calibrator.calibrateTrinomialTree(option.UnderlyingOption, ratesProvider, volatilities);
		return currencyExposure(option, ratesProvider, volatilities, treeData);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the FX barrier option product.
	  /// <para>
	  /// This assumes the tree is already calibrated and the tree data is stored as {@code RecombiningTrinomialTreeData}.
	  /// The tree data should be consistent with the pricer and other inputs, see <seealso cref="#validateData"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the option product </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <param name="treeData">  the trinomial tree data </param>
	  /// <returns> the currency exposure </returns>
	  public virtual MultiCurrencyAmount currencyExposure(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities, RecombiningTrinomialTreeData treeData)
	  {

		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		ValueDerivatives priceDerivatives = this.priceDerivatives(option, ratesProvider, volatilities, treeData);
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
	  private ValueDerivatives priceDerivatives(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities, RecombiningTrinomialTreeData data)
	  {

		validate(option, ratesProvider, volatilities);
		validateData(option, ratesProvider, volatilities, data);
		int nSteps = data.NumberOfSteps;
		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		double timeToExpiry = data.getTime(nSteps);
		ResolvedFxSingle underlyingFx = underlyingOption.Underlying;
		Currency ccyBase = underlyingFx.CounterCurrencyPayment.Currency;
		Currency ccyCounter = underlyingFx.CounterCurrencyPayment.Currency;
		DiscountFactors baseDiscountFactors = ratesProvider.discountFactors(ccyBase);
		DiscountFactors counterDiscountFactors = ratesProvider.discountFactors(ccyCounter);
		double rebateAtExpiry = 0d; // used to price knock-in option
		double rebateAtExpiryDerivative = 0d; // used to price knock-in option
		double notional = Math.Abs(underlyingFx.BaseCurrencyPayment.Amount);
		double[] rebateArray = new double[nSteps + 1];
		SimpleConstantContinuousBarrier barrier = (SimpleConstantContinuousBarrier) option.Barrier;
		if (option.Rebate.Present)
		{
		  CurrencyAmount rebateCurrencyAmount = option.Rebate.get();
		  double rebatePerUnit = rebateCurrencyAmount.Amount / notional;
		  bool isCounter = rebateCurrencyAmount.Currency.Equals(ccyCounter);
		  double rebate = isCounter ? rebatePerUnit : rebatePerUnit * barrier.BarrierLevel;
		  if (barrier.KnockType.KnockIn)
		  { // use in-out parity
			double dfCounterAtExpiry = counterDiscountFactors.discountFactor(timeToExpiry);
			double dfBaseAtExpiry = baseDiscountFactors.discountFactor(timeToExpiry);
			for (int i = 0; i < nSteps + 1; ++i)
			{
			  rebateArray[i] = isCounter ? rebate * dfCounterAtExpiry / counterDiscountFactors.discountFactor(data.getTime(i)) : rebate * dfBaseAtExpiry / baseDiscountFactors.discountFactor(data.getTime(i));
			}
			if (isCounter)
			{
			  rebateAtExpiry = rebatePerUnit * dfCounterAtExpiry;
			}
			else
			{
			  rebateAtExpiry = rebatePerUnit * data.Spot * dfBaseAtExpiry;
			  rebateAtExpiryDerivative = rebatePerUnit * dfBaseAtExpiry;
			}
		  }
		  else
		  {
			Arrays.fill(rebateArray, rebate);
		  }
		}
		ConstantContinuousSingleBarrierKnockoutFunction barrierFunction = ConstantContinuousSingleBarrierKnockoutFunction.of(underlyingOption.Strike, timeToExpiry, underlyingOption.PutCall, nSteps, barrier.BarrierType, barrier.BarrierLevel, DoubleArray.ofUnsafe(rebateArray));
		ValueDerivatives barrierPrice = TREE.optionPriceAdjoint(barrierFunction, data);
		if (barrier.KnockType.KnockIn)
		{ // use in-out parity
		  EuropeanVanillaOptionFunction vanillaFunction = EuropeanVanillaOptionFunction.of(underlyingOption.Strike, timeToExpiry, underlyingOption.PutCall, nSteps);
		  ValueDerivatives vanillaPrice = TREE.optionPriceAdjoint(vanillaFunction, data);
		  return ValueDerivatives.of(vanillaPrice.Value + rebateAtExpiry - barrierPrice.Value, DoubleArray.of(vanillaPrice.getDerivative(0) + rebateAtExpiryDerivative - barrierPrice.getDerivative(0)));
		}
		return barrierPrice;
	  }

	  //-------------------------------------------------------------------------
	  private void validateData(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities, RecombiningTrinomialTreeData data)
	  {

		ResolvedFxVanillaOption underlyingOption = option.UnderlyingOption;
		ArgChecker.isTrue(DoubleMath.fuzzyEquals(data.getTime(data.NumberOfSteps), volatilities.relativeTime(underlyingOption.Expiry), SMALL), "time to expiry mismatch between pricing option and trinomial tree data");
		ArgChecker.isTrue(DoubleMath.fuzzyEquals(data.Spot, ratesProvider.fxRate(underlyingOption.Underlying.CurrencyPair), SMALL), "today's FX rate mismatch between rates provider and trinomial tree data");
	  }

	  private void validate(ResolvedFxSingleBarrierOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ArgChecker.isTrue(option.Barrier is SimpleConstantContinuousBarrier, "barrier should be SimpleConstantContinuousBarrier");
		ArgChecker.isTrue(ratesProvider.ValuationDate.isEqual(volatilities.ValuationDateTime.toLocalDate()), "Volatility and rate data must be for the same date");
	  }

	  // signed notional amount to computed present value and value Greeks
	  private double signedNotional(ResolvedFxVanillaOption option)
	  {
		return (option.LongShort.Long ? 1d : -1d) * Math.Abs(option.Underlying.BaseCurrencyPayment.Amount);
	  }

	}

}
using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using DiscountingIborFixingDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingIborFixingDepositProductPricer;
	using DiscountingTermDepositProductPricer = com.opengamma.strata.pricer.deposit.DiscountingTermDepositProductPricer;
	using DiscountingFraProductPricer = com.opengamma.strata.pricer.fra.DiscountingFraProductPricer;
	using DiscountingFxSwapProductPricer = com.opengamma.strata.pricer.fx.DiscountingFxSwapProductPricer;
	using DiscountingIborFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingIborFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Provides calibration measures for a single type of trade based on functions.
	/// <para>
	/// This is initialized using functions that typically refer to pricers.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade type </param>
	public sealed class TradeCalibrationMeasure<T> : CalibrationMeasure<T> where T : com.opengamma.strata.product.ResolvedTrade
	{

	  /// <summary>
	  /// The calibrator for <seealso cref="ResolvedFraTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly TradeCalibrationMeasure<ResolvedFraTrade> FRA_PAR_SPREAD = TradeCalibrationMeasure.of("FraParSpreadDiscounting", typeof(ResolvedFraTrade), (trade, p) => DiscountingFraProductPricer.DEFAULT.parSpread(trade.Product, p), (trade, p) => DiscountingFraProductPricer.DEFAULT.parSpreadSensitivity(trade.Product, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="ResolvedIborFutureTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly TradeCalibrationMeasure<ResolvedIborFutureTrade> IBOR_FUTURE_PAR_SPREAD = TradeCalibrationMeasure.of("IborFutureParSpreadDiscounting", typeof(ResolvedIborFutureTrade), (trade, p) => DiscountingIborFutureTradePricer.DEFAULT.parSpread(trade, p, 0.0), (trade, p) => DiscountingIborFutureTradePricer.DEFAULT.parSpreadSensitivity(trade, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="ResolvedSwapTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly TradeCalibrationMeasure<ResolvedSwapTrade> SWAP_PAR_SPREAD = TradeCalibrationMeasure.of("SwapParSpreadDiscounting", typeof(ResolvedSwapTrade), (trade, p) => DiscountingSwapProductPricer.DEFAULT.parSpread(trade.Product, p), (trade, p) => DiscountingSwapProductPricer.DEFAULT.parSpreadSensitivity(trade.Product, p).build());

	  /// <summary>
	  /// The calibrator for <seealso cref="ResolvedIborFixingDepositTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly TradeCalibrationMeasure<ResolvedIborFixingDepositTrade> IBOR_FIXING_DEPOSIT_PAR_SPREAD = TradeCalibrationMeasure.of("IborFixingDepositParSpreadDiscounting", typeof(ResolvedIborFixingDepositTrade), (trade, p) => DiscountingIborFixingDepositProductPricer.DEFAULT.parSpread(trade.Product, p), (trade, p) => DiscountingIborFixingDepositProductPricer.DEFAULT.parSpreadSensitivity(trade.Product, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="ResolvedTermDepositTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly TradeCalibrationMeasure<ResolvedTermDepositTrade> TERM_DEPOSIT_PAR_SPREAD = TradeCalibrationMeasure.of("TermDepositParSpreadDiscounting", typeof(ResolvedTermDepositTrade), (trade, p) => DiscountingTermDepositProductPricer.DEFAULT.parSpread(trade.Product, p), (trade, p) => DiscountingTermDepositProductPricer.DEFAULT.parSpreadSensitivity(trade.Product, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="ResolvedFxSwapTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly TradeCalibrationMeasure<ResolvedFxSwapTrade> FX_SWAP_PAR_SPREAD = TradeCalibrationMeasure.of("FxSwapParSpreadDiscounting", typeof(ResolvedFxSwapTrade), (trade, p) => DiscountingFxSwapProductPricer.DEFAULT.parSpread(trade.Product, p), (trade, p) => DiscountingFxSwapProductPricer.DEFAULT.parSpreadSensitivity(trade.Product, p));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The name.
	  /// </summary>
	  private readonly string name;
	  /// <summary>
	  /// The trade type.
	  /// </summary>
	  private readonly Type<T> tradeType;
	  /// <summary>
	  /// The value measure.
	  /// </summary>
	  private readonly System.Func<T, RatesProvider, double> valueFn;
	  /// <summary>
	  /// The sensitivity measure.
	  /// </summary>
	  private readonly System.Func<T, RatesProvider, PointSensitivities> sensitivityFn;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a calibrator for a specific type of trade.
	  /// <para>
	  /// The functions typically refer to pricers.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the trade type </param>
	  /// <param name="name">  the name </param>
	  /// <param name="tradeType">  the trade type </param>
	  /// <param name="valueFn">  the function for calculating the value </param>
	  /// <param name="sensitivityFn">  the function for calculating the sensitivity </param>
	  /// <returns> the calibrator </returns>
	  public static TradeCalibrationMeasure<R> of<R>(string name, Type<R> tradeType, System.Func<R, RatesProvider, double> valueFn, System.Func<R, RatesProvider, PointSensitivities> sensitivityFn) where R : com.opengamma.strata.product.ResolvedTrade
	  {

		return new TradeCalibrationMeasure<R>(name, tradeType, valueFn, sensitivityFn);
	  }

	  // restricted constructor
	  private TradeCalibrationMeasure(string name, Type<T> tradeType, System.Func<T, RatesProvider, double> valueFn, System.Func<T, RatesProvider, PointSensitivities> sensitivityFn)
	  {

		this.name = name;
		this.tradeType = tradeType;
		this.valueFn = ArgChecker.notNull(valueFn, "valueFn");
		this.sensitivityFn = ArgChecker.notNull(sensitivityFn, "sensitivityFn");
	  }

	  //-------------------------------------------------------------------------
	  public Type<T> TradeType
	  {
		  get
		  {
			return tradeType;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public double value(T trade, RatesProvider provider)
	  {
		return valueFn.applyAsDouble(trade, provider);
	  }

	  public CurrencyParameterSensitivities sensitivities(T trade, RatesProvider provider)
	  {
		PointSensitivities pts = sensitivityFn.apply(trade, provider);
		return provider.parameterSensitivity(pts);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return name;
	  }

	}

}
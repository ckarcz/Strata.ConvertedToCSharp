using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using DiscountingIborFutureProductPricer = com.opengamma.strata.pricer.index.DiscountingIborFutureProductPricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Provides market quote measures for a single type of trade based on functions.
	/// <para>
	/// This is initialized using functions that typically refer to pricers.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade type </param>
	public sealed class MarketQuoteMeasure<T> : CalibrationMeasure<T> where T : com.opengamma.strata.product.ResolvedTrade
	{

	  /// <summary>
	  /// The measure for <seealso cref="ResolvedFraTrade"/> using par rate discounting.
	  /// </summary>
	  public static readonly MarketQuoteMeasure<ResolvedFraTrade> FRA_MQ = MarketQuoteMeasure.of("FraParRateDiscounting", typeof(ResolvedFraTrade), (trade, p) => DiscountingFraProductPricer.DEFAULT.parRate(trade.Product, p), (trade, p) => DiscountingFraProductPricer.DEFAULT.parRateSensitivity(trade.Product, p));

	  /// <summary>
	  /// The measure for <seealso cref="ResolvedIborFutureTrade"/> using price discounting.
	  /// </summary>
	  public static readonly MarketQuoteMeasure<ResolvedIborFutureTrade> IBOR_FUTURE_MQ = MarketQuoteMeasure.of("IborFuturePriceDiscounting", typeof(ResolvedIborFutureTrade), (trade, p) => DiscountingIborFutureProductPricer.DEFAULT.price(trade.Product, p), (trade, p) => DiscountingIborFutureProductPricer.DEFAULT.priceSensitivity(trade.Product, p));

	  /// <summary>
	  /// The measure for <seealso cref="ResolvedSwapTrade"/> using par rate discounting. Apply only to swap with a fixed leg.
	  /// </summary>
	  public static readonly MarketQuoteMeasure<ResolvedSwapTrade> SWAP_MQ = MarketQuoteMeasure.of("SwapParRateDiscounting", typeof(ResolvedSwapTrade), (trade, p) => DiscountingSwapProductPricer.DEFAULT.parRate(trade.Product, p), (trade, p) => DiscountingSwapProductPricer.DEFAULT.parRateSensitivity(trade.Product, p).build());

	  /// <summary>
	  /// The measure for <seealso cref="ResolvedIborFixingDepositTrade"/> using par rate discounting.
	  /// </summary>
	  public static readonly MarketQuoteMeasure<ResolvedIborFixingDepositTrade> IBOR_FIXING_DEPOSIT_MQ = MarketQuoteMeasure.of("IborFixingDepositParRateDiscounting", typeof(ResolvedIborFixingDepositTrade), (trade, p) => DiscountingIborFixingDepositProductPricer.DEFAULT.parRate(trade.Product, p), (trade, p) => DiscountingIborFixingDepositProductPricer.DEFAULT.parRateSensitivity(trade.Product, p));

	  /// <summary>
	  /// The measure for <seealso cref="ResolvedTermDepositTrade"/> using par rate discounting.
	  /// </summary>
	  public static readonly MarketQuoteMeasure<ResolvedTermDepositTrade> TERM_DEPOSIT_MQ = MarketQuoteMeasure.of("TermDepositParRateDiscounting", typeof(ResolvedTermDepositTrade), (trade, p) => DiscountingTermDepositProductPricer.DEFAULT.parRate(trade.Product, p), (trade, p) => DiscountingTermDepositProductPricer.DEFAULT.parRateSensitivity(trade.Product, p));

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
	  public static MarketQuoteMeasure<R> of<R>(string name, Type<R> tradeType, System.Func<R, RatesProvider, double> valueFn, System.Func<R, RatesProvider, PointSensitivities> sensitivityFn) where R : com.opengamma.strata.product.ResolvedTrade
	  {

		return new MarketQuoteMeasure<R>(name, tradeType, valueFn, sensitivityFn);
	  }

	  // restricted constructor
	  private MarketQuoteMeasure(string name, Type<T> tradeType, System.Func<T, RatesProvider, double> valueFn, System.Func<T, RatesProvider, PointSensitivities> sensitivityFn)
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
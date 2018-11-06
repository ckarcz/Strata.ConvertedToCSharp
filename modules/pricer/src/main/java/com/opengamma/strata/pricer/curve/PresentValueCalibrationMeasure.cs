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
	using DiscountingIborFutureTradePricer = com.opengamma.strata.pricer.index.DiscountingIborFutureTradePricer;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using MarketQuoteSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.MarketQuoteSensitivityCalculator;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using IborFixingDepositTrade = com.opengamma.strata.product.deposit.IborFixingDepositTrade;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Provides calibration measures for a single type of trade based on functions.
	/// <para>
	/// This set of measures return the present value of the product. For multi-currency instruments, the present value
	/// is converted into the currency of the first leg.
	/// The sensitivities are with respect to the market quote sensitivities and are also converted in the currency of the 
	/// first leg when necessary.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the trade type </param>
	public sealed class PresentValueCalibrationMeasure<T> : CalibrationMeasure<T> where T : com.opengamma.strata.product.ResolvedTrade
	{

	  private static readonly MarketQuoteSensitivityCalculator MQC = MarketQuoteSensitivityCalculator.DEFAULT;

	  /// <summary>
	  /// The measure for <seealso cref="FraTrade"/> using present value discounting.
	  /// </summary>
	  public static readonly PresentValueCalibrationMeasure<ResolvedFraTrade> FRA_PV = PresentValueCalibrationMeasure.of("FraPresentValueDiscounting", typeof(ResolvedFraTrade), (trade, p) => DiscountingFraProductPricer.DEFAULT.presentValue(trade.Product, p).Amount, (trade, p) => DiscountingFraProductPricer.DEFAULT.presentValueSensitivity(trade.Product, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="IborFutureTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly PresentValueCalibrationMeasure<ResolvedIborFutureTrade> IBOR_FUTURE_PV = PresentValueCalibrationMeasure.of("IborFutureParSpreadDiscounting", typeof(ResolvedIborFutureTrade), (trade, p) => DiscountingIborFutureTradePricer.DEFAULT.presentValue(trade, p, 0.0).Amount, (trade, p) => DiscountingIborFutureTradePricer.DEFAULT.presentValueSensitivity(trade, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="SwapTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly PresentValueCalibrationMeasure<ResolvedSwapTrade> SWAP_PV = PresentValueCalibrationMeasure.of("SwapParSpreadDiscounting", typeof(ResolvedSwapTrade), (trade, p) => DiscountingSwapProductPricer.DEFAULT.presentValue(trade.Product, p).convertedTo(trade.Product.Legs.get(0).Currency, p).Amount, (trade, p) => DiscountingSwapProductPricer.DEFAULT.presentValueSensitivity(trade.Product, p).build().convertedTo(trade.Product.Legs.get(0).Currency, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="IborFixingDepositTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly PresentValueCalibrationMeasure<ResolvedIborFixingDepositTrade> IBOR_FIXING_DEPOSIT_PV = PresentValueCalibrationMeasure.of("IborFixingDepositParSpreadDiscounting", typeof(ResolvedIborFixingDepositTrade), (trade, p) => DiscountingIborFixingDepositProductPricer.DEFAULT.presentValue(trade.Product, p).Amount, (trade, p) => DiscountingIborFixingDepositProductPricer.DEFAULT.presentValueSensitivity(trade.Product, p));

	  /// <summary>
	  /// The calibrator for <seealso cref="TermDepositTrade"/> using par spread discounting.
	  /// </summary>
	  public static readonly PresentValueCalibrationMeasure<ResolvedTermDepositTrade> TERM_DEPOSIT_PV = PresentValueCalibrationMeasure.of("TermDepositParSpreadDiscounting", typeof(ResolvedTermDepositTrade), (trade, p) => DiscountingTermDepositProductPricer.DEFAULT.presentValue(trade.Product, p).Amount, (trade, p) => DiscountingTermDepositProductPricer.DEFAULT.presentValueSensitivity(trade.Product, p));

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
	  public static PresentValueCalibrationMeasure<R> of<R>(string name, Type<R> tradeType, System.Func<R, RatesProvider, double> valueFn, System.Func<R, RatesProvider, PointSensitivities> sensitivityFn) where R : com.opengamma.strata.product.ResolvedTrade
	  {

		return new PresentValueCalibrationMeasure<R>(name, tradeType, valueFn, sensitivityFn);
	  }

	  // restricted constructor
	  private PresentValueCalibrationMeasure(string name, Type<T> tradeType, System.Func<T, RatesProvider, double> valueFn, System.Func<T, RatesProvider, PointSensitivities> sensitivityFn)
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
		CurrencyParameterSensitivities ps = provider.parameterSensitivity(pts);
		return MQC.sensitivity(ps, provider);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return name;
	  }

	}

}
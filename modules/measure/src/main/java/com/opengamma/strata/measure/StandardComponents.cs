using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CalculationRunner = com.opengamma.strata.calc.CalculationRunner;
	using MarketDataFactory = com.opengamma.strata.calc.marketdata.MarketDataFactory;
	using MarketDataFunction = com.opengamma.strata.calc.marketdata.MarketDataFunction;
	using ObservableDataProvider = com.opengamma.strata.calc.marketdata.ObservableDataProvider;
	using TimeSeriesProvider = com.opengamma.strata.calc.marketdata.TimeSeriesProvider;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using BillTradeCalculationFunction = com.opengamma.strata.measure.bond.BillTradeCalculationFunction;
	using BondFutureOptionTradeCalculationFunction = com.opengamma.strata.measure.bond.BondFutureOptionTradeCalculationFunction;
	using BondFutureTradeCalculationFunction = com.opengamma.strata.measure.bond.BondFutureTradeCalculationFunction;
	using CapitalIndexedBondTradeCalculationFunction = com.opengamma.strata.measure.bond.CapitalIndexedBondTradeCalculationFunction;
	using FixedCouponBondTradeCalculationFunction = com.opengamma.strata.measure.bond.FixedCouponBondTradeCalculationFunction;
	using IborCapFloorTradeCalculationFunction = com.opengamma.strata.measure.capfloor.IborCapFloorTradeCalculationFunction;
	using CdsIndexTradeCalculationFunction = com.opengamma.strata.measure.credit.CdsIndexTradeCalculationFunction;
	using CdsTradeCalculationFunction = com.opengamma.strata.measure.credit.CdsTradeCalculationFunction;
	using CurveMarketDataFunction = com.opengamma.strata.measure.curve.CurveMarketDataFunction;
	using TermDepositTradeCalculationFunction = com.opengamma.strata.measure.deposit.TermDepositTradeCalculationFunction;
	using DsfTradeCalculationFunction = com.opengamma.strata.measure.dsf.DsfTradeCalculationFunction;
	using FraTradeCalculationFunction = com.opengamma.strata.measure.fra.FraTradeCalculationFunction;
	using FxNdfTradeCalculationFunction = com.opengamma.strata.measure.fx.FxNdfTradeCalculationFunction;
	using FxRateMarketDataFunction = com.opengamma.strata.measure.fx.FxRateMarketDataFunction;
	using FxSingleTradeCalculationFunction = com.opengamma.strata.measure.fx.FxSingleTradeCalculationFunction;
	using FxSwapTradeCalculationFunction = com.opengamma.strata.measure.fx.FxSwapTradeCalculationFunction;
	using FxOptionVolatilitiesMarketDataFunction = com.opengamma.strata.measure.fxopt.FxOptionVolatilitiesMarketDataFunction;
	using FxSingleBarrierOptionTradeCalculationFunction = com.opengamma.strata.measure.fxopt.FxSingleBarrierOptionTradeCalculationFunction;
	using FxVanillaOptionTradeCalculationFunction = com.opengamma.strata.measure.fxopt.FxVanillaOptionTradeCalculationFunction;
	using IborFutureOptionTradeCalculationFunction = com.opengamma.strata.measure.index.IborFutureOptionTradeCalculationFunction;
	using IborFutureTradeCalculationFunction = com.opengamma.strata.measure.index.IborFutureTradeCalculationFunction;
	using BulletPaymentTradeCalculationFunction = com.opengamma.strata.measure.payment.BulletPaymentTradeCalculationFunction;
	using RatesCurveGroupMarketDataFunction = com.opengamma.strata.measure.rate.RatesCurveGroupMarketDataFunction;
	using RatesCurveInputsMarketDataFunction = com.opengamma.strata.measure.rate.RatesCurveInputsMarketDataFunction;
	using GenericSecurityPositionCalculationFunction = com.opengamma.strata.measure.security.GenericSecurityPositionCalculationFunction;
	using GenericSecurityTradeCalculationFunction = com.opengamma.strata.measure.security.GenericSecurityTradeCalculationFunction;
	using SecurityPositionCalculationFunction = com.opengamma.strata.measure.security.SecurityPositionCalculationFunction;
	using SecurityTradeCalculationFunction = com.opengamma.strata.measure.security.SecurityTradeCalculationFunction;
	using SwapTradeCalculationFunction = com.opengamma.strata.measure.swap.SwapTradeCalculationFunction;
	using SwaptionTradeCalculationFunction = com.opengamma.strata.measure.swaption.SwaptionTradeCalculationFunction;
	using GenericSecurityPosition = com.opengamma.strata.product.GenericSecurityPosition;
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using SecurityPosition = com.opengamma.strata.product.SecurityPosition;
	using SecurityTrade = com.opengamma.strata.product.SecurityTrade;
	using BondFutureOptionPosition = com.opengamma.strata.product.bond.BondFutureOptionPosition;
	using BondFutureOptionTrade = com.opengamma.strata.product.bond.BondFutureOptionTrade;
	using BondFuturePosition = com.opengamma.strata.product.bond.BondFuturePosition;
	using BondFutureTrade = com.opengamma.strata.product.bond.BondFutureTrade;
	using CapitalIndexedBondPosition = com.opengamma.strata.product.bond.CapitalIndexedBondPosition;
	using CapitalIndexedBondTrade = com.opengamma.strata.product.bond.CapitalIndexedBondTrade;
	using FixedCouponBondPosition = com.opengamma.strata.product.bond.FixedCouponBondPosition;
	using FixedCouponBondTrade = com.opengamma.strata.product.bond.FixedCouponBondTrade;
	using IborCapFloorTrade = com.opengamma.strata.product.capfloor.IborCapFloorTrade;
	using CdsIndexTrade = com.opengamma.strata.product.credit.CdsIndexTrade;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using DsfPosition = com.opengamma.strata.product.dsf.DsfPosition;
	using DsfTrade = com.opengamma.strata.product.dsf.DsfTrade;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using FxNdfTrade = com.opengamma.strata.product.fx.FxNdfTrade;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;
	using FxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.FxSingleBarrierOptionTrade;
	using FxVanillaOptionTrade = com.opengamma.strata.product.fxopt.FxVanillaOptionTrade;
	using IborFutureOptionPosition = com.opengamma.strata.product.index.IborFutureOptionPosition;
	using IborFutureOptionTrade = com.opengamma.strata.product.index.IborFutureOptionTrade;
	using IborFuturePosition = com.opengamma.strata.product.index.IborFuturePosition;
	using IborFutureTrade = com.opengamma.strata.product.index.IborFutureTrade;
	using BulletPaymentTrade = com.opengamma.strata.product.payment.BulletPaymentTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using SwaptionTrade = com.opengamma.strata.product.swaption.SwaptionTrade;

	/// <summary>
	/// Factory methods for creating standard Strata components.
	/// <para>
	/// These components are suitable for performing calculations using the built-in asset classes,
	/// market data types and pricers.
	/// </para>
	/// <para>
	/// The market data factory can create market data values derived from other values.
	/// For example it can create calibrated curves given market quotes.
	/// However it cannot request market data from an external provider, such as Bloomberg,
	/// or look up data from a data store, for example a time series database.
	/// Instances of <seealso cref="CalculationRunner"/> are created directly using the static methods on the interface.
	/// </para>
	/// </summary>
	public sealed class StandardComponents
	{

	  /// <summary>
	  /// The standard calculation functions.
	  /// </summary>
	  private static readonly CalculationFunctions STANDARD = CalculationFunctions.of(new BulletPaymentTradeCalculationFunction(), new CdsTradeCalculationFunction(), new CdsIndexTradeCalculationFunction(), new FraTradeCalculationFunction(), new FxNdfTradeCalculationFunction(), new FxSingleBarrierOptionTradeCalculationFunction(), new FxSingleTradeCalculationFunction(), new FxSwapTradeCalculationFunction(), new FxVanillaOptionTradeCalculationFunction(), new IborCapFloorTradeCalculationFunction(), new SecurityPositionCalculationFunction(), new SecurityTradeCalculationFunction(), new SwapTradeCalculationFunction(), new SwaptionTradeCalculationFunction(), new TermDepositTradeCalculationFunction(), new GenericSecurityPositionCalculationFunction(), new GenericSecurityTradeCalculationFunction(), BondFutureTradeCalculationFunction.TRADE, BondFutureTradeCalculationFunction.POSITION, BondFutureOptionTradeCalculationFunction.TRADE, BondFutureOptionTradeCalculationFunction.POSITION, CapitalIndexedBondTradeCalculationFunction.TRADE, CapitalIndexedBondTradeCalculationFunction.POSITION, DsfTradeCalculationFunction.TRADE, DsfTradeCalculationFunction.POSITION, FixedCouponBondTradeCalculationFunction.TRADE, FixedCouponBondTradeCalculationFunction.POSITION, BillTradeCalculationFunction.TRADE, BillTradeCalculationFunction.POSITION, IborFutureTradeCalculationFunction.TRADE, IborFutureTradeCalculationFunction.POSITION, IborFutureOptionTradeCalculationFunction.TRADE, IborFutureOptionTradeCalculationFunction.POSITION);

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardComponents()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a market data factory containing the standard set of market data functions.
	  /// <para>
	  /// This factory can create market data values from other market data. For example it
	  /// can create calibrated curves given a set of market quotes for the points on the curve.
	  /// </para>
	  /// <para>
	  /// The set of functions are the ones provided by <seealso cref="#marketDataFunctions()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a market data factory containing the standard set of market data functions </returns>
	  public static MarketDataFactory marketDataFactory()
	  {
		return marketDataFactory(ObservableDataProvider.none());
	  }

	  /// <summary>
	  /// Returns a market data factory containing the standard set of market data functions.
	  /// <para>
	  /// This factory can create market data values from other market data. For example it
	  /// can create calibrated curves given a set of market quotes for the points on the curve.
	  /// </para>
	  /// <para>
	  /// The set of functions are the ones provided by <seealso cref="#marketDataFunctions()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observableDataProvider">  the provider of observable data </param>
	  /// <returns> a market data factory containing the standard set of market data functions </returns>
	  public static MarketDataFactory marketDataFactory(ObservableDataProvider observableDataProvider)
	  {
		return MarketDataFactory.of(observableDataProvider, TimeSeriesProvider.none(), marketDataFunctions());
	  }

	  /// <summary>
	  /// Returns the standard market data functions used to build market data values from other market data.
	  /// <para>
	  /// These include functions to build:
	  /// <ul>
	  ///  <li>Par rates from quotes
	  ///  <li>Curve groups from par rates
	  ///  <li>Curves from curve groups
	  ///  <li>Discount factors and index rates from curves
	  ///  <li>FX rates from quotes
	  ///  <li>FX option volatilities from quotes
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard market data functions </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static java.util.List<com.opengamma.strata.calc.marketdata.MarketDataFunction<?, ?>> marketDataFunctions()
	  public static IList<MarketDataFunction<object, ?>> marketDataFunctions()
	  {
		return ImmutableList.of(new CurveMarketDataFunction(), new RatesCurveGroupMarketDataFunction(), new RatesCurveInputsMarketDataFunction(), new FxRateMarketDataFunction(), new FxOptionVolatilitiesMarketDataFunction());
	  }

	  /// <summary>
	  /// Returns the standard calculation functions.
	  /// <para>
	  /// These define how to calculate the standard measures for the standard asset classes.
	  /// </para>
	  /// <para>
	  /// The standard calculation functions require no further configuration and are designed to allow
	  /// easy access to all built-in asset class coverage.
	  /// The supported asset classes are:
	  /// <ul>
	  ///  <li>Bond future - <seealso cref="BondFutureTrade"/> and <seealso cref="BondFuturePosition"/>
	  ///  <li>Bond future option - <seealso cref="BondFutureOptionTrade"/> and <seealso cref="BondFutureOptionPosition"/>
	  ///  <li>Bullet Payment - <seealso cref="BulletPaymentTrade"/>
	  ///  <li>Cap/floor (Ibor) - <seealso cref="IborCapFloorTrade"/>
	  ///  <li>Capital Indexed bond - <seealso cref="CapitalIndexedBondTrade"/> and <seealso cref="CapitalIndexedBondPosition"/>
	  ///  <li>Credit Default Swap - <seealso cref="CdsTrade"/>
	  ///  <li>CDS Index - <seealso cref="CdsIndexTrade"/>
	  ///  <li>Deliverable Swap Future - <seealso cref="DsfTrade"/> and <seealso cref="DsfPosition"/>
	  ///  <li>Forward Rate Agreement - <seealso cref="FraTrade"/>
	  ///  <li>Fixed coupon bond - <seealso cref="FixedCouponBondTrade"/> and <seealso cref="FixedCouponBondPosition"/>
	  ///  <li>FX spot and FX forward - <seealso cref="FxSingleTrade"/>
	  ///  <li>FX NDF - <seealso cref="FxNdfTrade"/>
	  ///  <li>FX swap - <seealso cref="FxSwapTrade"/>
	  ///  <li>FX vanilla option - <seealso cref="FxVanillaOptionTrade"/>
	  ///  <li>FX single barrier option - <seealso cref="FxSingleBarrierOptionTrade"/>
	  ///  <li>Generic Security - <seealso cref="GenericSecurityTrade"/> and <seealso cref="GenericSecurityPosition"/>
	  ///  <li>Rate Swap - <seealso cref="SwapTrade"/>
	  ///  <li>Swaption - <seealso cref="SwaptionTrade"/>
	  ///  <li>Security - <seealso cref="SecurityTrade"/> and <seealso cref="SecurityPosition"/>
	  ///  <li>STIR Future (Ibor) - <seealso cref="IborFutureTrade"/> and <seealso cref="IborFuturePosition"/>
	  ///  <li>STIR Future Option (Ibor) - <seealso cref="IborFutureOptionTrade"/> and <seealso cref="IborFutureOptionPosition"/>
	  ///  <li>Term Deposit - <seealso cref="TermDepositTrade"/>
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> calculation functions used to perform calculations </returns>
	  public static CalculationFunctions calculationFunctions()
	  {
		return STANDARD;
	  }

	}

}
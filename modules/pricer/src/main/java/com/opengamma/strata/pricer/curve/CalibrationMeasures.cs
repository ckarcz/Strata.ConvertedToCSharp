using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using UnitParameterSensitivities = com.opengamma.strata.market.param.UnitParameterSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Provides access to the measures needed to perform curve calibration.
	/// <para>
	/// The most commonly used measures are par spread and converted present value.
	/// </para>
	/// </summary>
	public sealed class CalibrationMeasures
	{

	  /// <summary>
	  /// The par spread instance, which is the default used in curve calibration.
	  /// <para>
	  /// This computes par spread for Term Deposits, IborFixingDeposit, FRA, Ibor Futures
	  /// Swap and FX Swap by discounting.
	  /// </para>
	  /// </summary>
	  public static readonly CalibrationMeasures PAR_SPREAD = CalibrationMeasures.of("ParSpread", TradeCalibrationMeasure.FRA_PAR_SPREAD, TradeCalibrationMeasure.FX_SWAP_PAR_SPREAD, TradeCalibrationMeasure.IBOR_FIXING_DEPOSIT_PAR_SPREAD, TradeCalibrationMeasure.IBOR_FUTURE_PAR_SPREAD, TradeCalibrationMeasure.SWAP_PAR_SPREAD, TradeCalibrationMeasure.TERM_DEPOSIT_PAR_SPREAD);
	  /// <summary>
	  /// The market quote instance, which is the default used in synthetic curve calibration.
	  /// <para>
	  /// This computes par rate for Term Deposits, IborFixingDeposit, FRA and Swap by discounting,
	  /// and price Ibor Futures by discounting.
	  /// </para>
	  /// </summary>
	  public static readonly CalibrationMeasures MARKET_QUOTE = CalibrationMeasures.of("MarketQuote", MarketQuoteMeasure.FRA_MQ, MarketQuoteMeasure.IBOR_FIXING_DEPOSIT_MQ, MarketQuoteMeasure.IBOR_FUTURE_MQ, MarketQuoteMeasure.SWAP_MQ, MarketQuoteMeasure.TERM_DEPOSIT_MQ);
	  /// <summary>
	  /// The present value instance, which is the default used in present value sensitivity to market quote stored during 
	  /// curve calibration.
	  /// <para>
	  /// This computes present value for Term Deposits, IborFixingDeposit, FRA and Swap by discounting,
	  /// and price Ibor Futures by discounting; the derivative is the derivative with respect to the market quotes.
	  /// </para>
	  /// </summary>
	  public static readonly CalibrationMeasures PRESENT_VALUE = CalibrationMeasures.of("PresentValue", PresentValueCalibrationMeasure.FRA_PV, PresentValueCalibrationMeasure.IBOR_FIXING_DEPOSIT_PV, PresentValueCalibrationMeasure.IBOR_FUTURE_PV, PresentValueCalibrationMeasure.SWAP_PV, PresentValueCalibrationMeasure.TERM_DEPOSIT_PV);

	  /// <summary>
	  /// The name of the set of measures.
	  /// </summary>
	  private readonly string name;
	  /// <summary>
	  /// The calibration measure providers keyed by type.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.google.common.collect.ImmutableMap<Class, CalibrationMeasure<? extends com.opengamma.strata.product.ResolvedTrade>> measuresByTrade;
	  private readonly ImmutableMap<Type, CalibrationMeasure<ResolvedTrade>> measuresByTrade;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a list of individual trade-specific measures.
	  /// <para>
	  /// Each measure must be for a different trade type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the set of measures </param>
	  /// <param name="measures">  the list of measures </param>
	  /// <returns> the calibration measures </returns>
	  /// <exception cref="IllegalArgumentException"> if a trade type is specified more than once </exception>
	  public static CalibrationMeasures of<T1>(string name, IList<T1> measures) where T1 : CalibrationMeasure<T1 extends com.opengamma.strata.product.ResolvedTrade>
	  {
		return new CalibrationMeasures(name, measures);
	  }

	  /// <summary>
	  /// Obtains an instance from a list of individual trade-specific measures.
	  /// <para>
	  /// Each measure must be for a different trade type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the set of measures </param>
	  /// <param name="measures">  the list of measures </param>
	  /// <returns> the calibration measures </returns>
	  /// <exception cref="IllegalArgumentException"> if a trade type is specified more than once </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static CalibrationMeasures of(String name, CalibrationMeasure<? extends com.opengamma.strata.product.ResolvedTrade>... measures)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static CalibrationMeasures of(string name, params CalibrationMeasure<ResolvedTrade>[] measures)
	  {
		return new CalibrationMeasures(name, ImmutableList.copyOf(measures));
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  private CalibrationMeasures<T1>(string name, IList<T1> measures) where T1 : CalibrationMeasure<T1 extends com.opengamma.strata.product.ResolvedTrade>
	  {
		this.name = ArgChecker.notEmpty(name, "name");
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		this.measuresByTrade = measures.collect(toImmutableMap(CalibrationMeasure::getTradeType, m => m));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the set of measures.
	  /// </summary>
	  /// <returns> the name </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  /// <summary>
	  /// Gets the supported trade types.
	  /// </summary>
	  /// <returns> the supported trade types </returns>
	  public ImmutableSet<Type> TradeTypes
	  {
		  get
		  {
			return measuresByTrade.Keys;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the value, such as par spread.
	  /// <para>
	  /// The value must be calculated using the specified rates provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the sensitivity </returns>
	  /// <exception cref="IllegalArgumentException"> if the trade cannot be valued </exception>
	  public double value(ResolvedTrade trade, RatesProvider provider)
	  {
		CalibrationMeasure<ResolvedTrade> measure = getMeasure(trade);
		return measure.value(trade, provider);
	  }

	  /// <summary>
	  /// Calculates the sensitivity with respect to the rates provider.
	  /// <para>
	  /// The result array is composed of the concatenated curve sensitivities from
	  /// all curves currently being processed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="curveOrder">  the order of the curves </param>
	  /// <returns> the sensitivity derivative </returns>
	  public DoubleArray derivative(ResolvedTrade trade, RatesProvider provider, IList<CurveParameterSize> curveOrder)
	  {
		UnitParameterSensitivities unitSens = extractSensitivities(trade, provider);

		// expand to a concatenated array
		DoubleArray result = DoubleArray.EMPTY;
		foreach (CurveParameterSize curveParams in curveOrder)
		{
		  DoubleArray sens = unitSens.findSensitivity(curveParams.Name).map(s => s.Sensitivity).orElseGet(() => DoubleArray.filled(curveParams.ParameterCount));
		  result = result.concat(sens);
		}
		return result;
	  }

	  // determine the curve parameter sensitivities, removing the curency
	  private UnitParameterSensitivities extractSensitivities(ResolvedTrade trade, RatesProvider provider)
	  {
		CalibrationMeasure<ResolvedTrade> measure = getMeasure(trade);
		CurrencyParameterSensitivities paramSens = measure.sensitivities(trade, provider);
		UnitParameterSensitivities unitSens = UnitParameterSensitivities.empty();
		foreach (CurrencyParameterSensitivity ccySens in paramSens.Sensitivities)
		{
		  unitSens = unitSens.combinedWith(ccySens.toUnitParameterSensitivity());
		}
		return unitSens;
	  }

	  //-------------------------------------------------------------------------
	  // finds the correct measure implementation
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private <T extends com.opengamma.strata.product.ResolvedTrade> CalibrationMeasure<com.opengamma.strata.product.ResolvedTrade> getMeasure(com.opengamma.strata.product.ResolvedTrade trade)
	  private CalibrationMeasure<ResolvedTrade> getMeasure<T>(ResolvedTrade trade) where T : com.opengamma.strata.product.ResolvedTrade
	  {
		Type tradeType = trade.GetType();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: CalibrationMeasure<? extends com.opengamma.strata.product.ResolvedTrade> measure = measuresByTrade.get(tradeType);
		CalibrationMeasure<ResolvedTrade> measure = measuresByTrade.get(tradeType);
		if (measure == null)
		{
		  throw new System.ArgumentException(Messages.format("Trade type '{}' is not supported for calibration", tradeType.Name));
		}
		// cast makes life easier for the code using this method
		return (CalibrationMeasure<ResolvedTrade>) measure;
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return name;
	  }

	}

}
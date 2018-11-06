/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using SecurityTrade = com.opengamma.strata.product.SecurityTrade;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using FxSingleTrade = com.opengamma.strata.product.fx.FxSingleTrade;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Resolves additional information when parsing trade CSV files.
	/// <para>
	/// Data loaded from a CSV may contain additional information that needs to be captured.
	/// This plugin point allows the additional CSV columns to be parsed and captured.
	/// </para>
	/// </summary>
	public interface TradeCsvInfoResolver
	{

	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static TradeCsvInfoResolver standard()
	//  {
	//	return StandardCsvInfoImpl.INSTANCE;
	//  }

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static TradeCsvInfoResolver of(com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//	return StandardCsvInfoImpl.of(refData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reference data being used.
	  /// </summary>
	  /// <returns> the reference data </returns>
	  ReferenceData ReferenceData {get;}

	  /// <summary>
	  /// Parses attributes into {@code TradeInfo}.
	  /// <para>
	  /// If they are available, the trade ID, date, time and zone will have been set
	  /// before this method is called. They may be altered if necessary, although
	  /// this is not recommended.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="builder">  the builder to update </param>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default void parseTradeInfo(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.TradeInfoBuilder builder)
	//  {
	//	// do nothing
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Completes the trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called by each of the {@code completeTrade} methods.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the trade type </param>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade </param>
	  /// <returns> the updated trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> T completeTradeCommon(com.opengamma.strata.collect.io.CsvRow row, T trade)
	//  {
	//	//do nothing
	//	return trade;
	//  }

	  /// <summary>
	  /// Completes the FRA trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the trade has been parsed and after
	  /// <seealso cref="#parseTradeInfo(CsvRow, TradeInfoBuilder)"/>.
	  /// </para>
	  /// <para>
	  /// By default this calls <seealso cref="#completeTradeCommon(CsvRow, Trade)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade </param>
	  /// <returns> the updated trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fra.FraTrade completeTrade(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.fra.FraTrade trade)
	//  {
	//	// do nothing
	//	return completeTradeCommon(row, trade);
	//  }

	  /// <summary>
	  /// Completes the trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the trade has been parsed and after
	  /// <seealso cref="#parseTradeInfo(CsvRow, TradeInfoBuilder)"/>.
	  /// </para>
	  /// <para>
	  /// By default this calls <seealso cref="#completeTradeCommon(CsvRow, Trade)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade </param>
	  /// <returns> the updated trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.SecurityTrade completeTrade(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.SecurityTrade trade)
	//  {
	//	// do nothing
	//	return completeTradeCommon(row, trade);
	//  }

	  /// <summary>
	  /// Completes the FRA trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the trade has been parsed and after
	  /// <seealso cref="#parseTradeInfo(CsvRow, TradeInfoBuilder)"/>.
	  /// </para>
	  /// <para>
	  /// By default this calls <seealso cref="#completeTradeCommon(CsvRow, Trade)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade </param>
	  /// <returns> the updated trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade completeTrade(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.swap.SwapTrade trade)
	//  {
	//	// do nothing
	//	return completeTradeCommon(row, trade);
	//  }

	  /// <summary>
	  /// Completes the trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the trade has been parsed and after
	  /// <seealso cref="#parseTradeInfo(CsvRow, TradeInfoBuilder)"/>.
	  /// </para>
	  /// <para>
	  /// By default this calls <seealso cref="#completeTradeCommon(CsvRow, Trade)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade </param>
	  /// <returns> the updated trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.deposit.TermDepositTrade completeTrade(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.deposit.TermDepositTrade trade)
	//  {
	//	// do nothing
	//	return completeTradeCommon(row, trade);
	//  }

	  /// <summary>
	  /// Completes the FX Forward trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the trade has been parsed and after
	  /// <seealso cref="#parseTradeInfo(CsvRow, TradeInfoBuilder)"/>.
	  /// </para>
	  /// <para>
	  /// By default this calls <seealso cref="#completeTradeCommon(CsvRow, Trade)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade, as an instance of <seealso cref="FxSingleTrade"/> </param>
	  /// <returns> the updated trade, as an instance of <seealso cref="FxSingleTrade"/> </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fx.FxSingleTrade completeTrade(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.fx.FxSingleTrade trade)
	//  {
	//	//do nothing
	//	return completeTradeCommon(row, trade);
	//  }

	  /// <summary>
	  /// Completes the FX Swap trade, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the trade has been parsed and after
	  /// <seealso cref="#parseTradeInfo(CsvRow, TradeInfoBuilder)"/>.
	  /// </para>
	  /// <para>
	  /// By default this calls <seealso cref="#completeTradeCommon(CsvRow, Trade)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="trade">  the parsed trade, as an instance of <seealso cref="FxSwapTrade"/> </param>
	  /// <returns> the updated trade, as an instance of <seealso cref="FxSwapTrade"/> </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fx.FxSwapTrade completeTrade(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.fx.FxSwapTrade trade)
	//  {
	//	//do nothing
	//	return completeTradeCommon(row, trade);
	//  }

	}

}
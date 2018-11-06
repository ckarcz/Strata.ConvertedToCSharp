/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for swap trades.
	/// <para>
	/// This defines the market convention for a a swap.
	/// Each different type of swap has its own convention - this interface provides an abstraction.
	/// </para>
	/// </summary>
	public interface SingleCurrencySwapConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static SingleCurrencySwapConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SingleCurrencySwapConvention of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return Guavate.firstNonEmpty(() -> FixedIborSwapConvention.extendedEnum().find(uniqueName), () -> IborIborSwapConvention.extendedEnum().find(uniqueName), () -> FixedOvernightSwapConvention.extendedEnum().find(uniqueName), () -> OvernightIborSwapConvention.extendedEnum().find(uniqueName), () -> FixedInflationSwapConvention.extendedEnum().find(uniqueName), () -> ThreeLegBasisSwapConvention.extendedEnum().find(uniqueName)).orElseThrow(() -> new IllegalArgumentException("SingleCurrencySwapConvention not found: " + uniqueName));
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the spot value date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date to find the start date.
	  /// A typical value is "plus 2 business days".
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the spot date offset, not null </returns>
	  DaysAdjustment SpotDateOffset {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a spot-starting trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified tenor. For example, a tenor
	  /// of 5 years creates a swap starting on the spot date and maturing 5 years later.
	  /// </para>
	  /// <para>
	  /// See the instrument-level documentation to understand how the fixed rate or spread is applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="fixedRateOrSpread">  the fixed rate or spread in decimal form, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade createTrade(java.time.LocalDate tradeDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRateOrSpread, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	return createTrade(tradeDate, Period.ZERO, tenor, buySell, notional, fixedRateOrSpread, refData);
	//  }

	  /// <summary>
	  /// Creates a forward-starting trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified period and tenor. For example, a period of
	  /// 3 months and a tenor of 5 years creates a swap starting three months after the spot date
	  /// and maturing 5 years later.
	  /// </para>
	  /// <para>
	  /// See the instrument-level documentation to understand how the fixed rate or spread is applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="fixedRateOrSpread">  the fixed rate or spread in decimal form, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade createTrade(java.time.LocalDate tradeDate, java.time.Period periodToStart, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRateOrSpread, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate spotValue = calculateSpotDateFromTradeDate(tradeDate, refData);
	//	LocalDate startDate = spotValue.plus(periodToStart);
	//	LocalDate endDate = startDate.plus(tenor.getPeriod());
	//	return toTrade(tradeDate, startDate, endDate, buySell, notional, fixedRateOrSpread);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// </para>
	  /// <para>
	  /// See the instrument-level documentation to understand how the fixed rate or spread is applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="fixedRateOrSpread">  the fixed rate or spread in decimal form, typically derived from the market </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRateOrSpread)
	//  {
	//
	//	TradeInfo tradeInfo = TradeInfo.of(tradeDate);
	//	return toTrade(tradeInfo, startDate, endDate, buySell, notional, fixedRateOrSpread);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// </para>
	  /// <para>
	  /// See the instrument-level documentation to understand how the fixed rate or spread is applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="fixedRateOrSpread">  the fixed rate or spread in decimal form, typically derived from the market </param>
	  /// <returns> the trade </returns>
	  SwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRateOrSpread);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the spot date from the trade date.
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="refData">  the reference data, used to resolve the date </param>
	  /// <returns> the spot date </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate calculateSpotDateFromTradeDate(java.time.LocalDate tradeDate, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//	return getSpotDateOffset().adjust(tradeDate, refData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this convention.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	}

}
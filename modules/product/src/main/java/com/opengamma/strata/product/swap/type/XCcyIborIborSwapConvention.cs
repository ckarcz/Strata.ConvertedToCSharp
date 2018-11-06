/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for cross-currency Ibor-Ibor swap trades without FX reset.
	/// <para>
	/// This defines the market convention for a cross-currency Ibor-Ibor swap.
	/// The convention is formed by combining two swap leg conventions in different currencies.
	/// </para>
	/// <para>
	/// The market price is for the difference (spread) between the values of the two legs.
	/// This convention has two legs, the "spread leg" and the "flat leg". The spread will be
	/// added to the "spread leg".
	/// </para>
	/// <para>
	/// For example, a 'EUR/USD' basis swap has 'EUR-EURIBOR-3M' as the spread leg and 'USD-LIBOR-3M' as the flat leg.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableXCcyIborIborSwapConvention"/>.
	/// To register a specific convention, see {@code XCcyIborIborSwapConvention.ini}.
	/// </para>
	/// </summary>
	public interface XCcyIborIborSwapConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static XCcyIborIborSwapConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static XCcyIborIborSwapConvention of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the convention to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<XCcyIborIborSwapConvention> extendedEnum()
	//  {
	//	return XCcyIborIborSwapConventions.ENUM_LOOKUP;
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg that has the spread applied.
	  /// <para>
	  /// The spread is the market price of the instrument.
	  /// It is added to the observed interest rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the spread leg convention </returns>
	  IborRateSwapLegConvention SpreadLeg {get;}

	  /// <summary>
	  /// Gets the market convention of the floating leg that does not have the spread applied.
	  /// </summary>
	  /// <returns> the flat leg convention </returns>
	  IborRateSwapLegConvention FlatLeg {get;}

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

	  /// <summary>
	  /// Gets the currency pair of the convention.
	  /// </summary>
	  /// <returns> the currency pair </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.CurrencyPair getCurrencyPair()
	//  {
	//	return CurrencyPair.of(getSpreadLeg().getCurrency(), getFlatLeg().getCurrency());
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a spot-starting trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified tenor. For example, a tenor
	  /// of 5 years creates a swap starting on the spot date and maturing 5 years later.
	  /// </para>
	  /// <para>
	  /// The notionals are unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the flat leg is received, with the rate of the spread leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notionalSpreadLeg">  the notional amount for the spread leg </param>
	  /// <param name="notionalFlatLeg">  the notional amount for the flat leg </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade createTrade(java.time.LocalDate tradeDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notionalSpreadLeg, double notionalFlatLeg, double spread, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	return createTrade(tradeDate, Period.ZERO, tenor, buySell, notionalSpreadLeg, notionalFlatLeg, spread, refData);
	//  }

	  /// <summary>
	  /// Creates a forward-starting trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified period and tenor. For example, a period of
	  /// 3 months and a tenor of 5 years creates a swap starting three months after the spot date
	  /// and maturing 5 years later.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the flat leg is received, with the rate of the spread leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notionalSpreadLeg">  the notional amount for the spread leg </param>
	  /// <param name="notionalFlatLeg">  the notional amount for the flat leg </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade createTrade(java.time.LocalDate tradeDate, java.time.Period periodToStart, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notionalSpreadLeg, double notionalFlatLeg, double spread, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate spotValue = calculateSpotDateFromTradeDate(tradeDate, refData);
	//	LocalDate startDate = spotValue.plus(periodToStart);
	//	LocalDate endDate = startDate.plus(tenor.getPeriod());
	//	return toTrade(tradeDate, startDate, endDate, buySell, notionalSpreadLeg, notionalFlatLeg, spread);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the flat leg is received, with the rate of the spread leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notionalSpreadLeg">  the notional amount for the spread leg </param>
	  /// <param name="notionalFlatLeg">  the notional amount for the flat leg </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notionalSpreadLeg, double notionalFlatLeg, double spread)
	//  {
	//
	//	TradeInfo tradeInfo = TradeInfo.of(tradeDate);
	//	return toTrade(tradeInfo, startDate, endDate, buySell, notionalSpreadLeg, notionalFlatLeg, spread);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the flat leg is received, with the rate of the spread leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade. </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notionalSpreadLeg">  the notional amount for the spread leg </param>
	  /// <param name="notionalFlatLeg">  the notional amount for the flat leg </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <returns> the trade </returns>
	  SwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notionalSpreadLeg, double notionalFlatLeg, double spread);

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
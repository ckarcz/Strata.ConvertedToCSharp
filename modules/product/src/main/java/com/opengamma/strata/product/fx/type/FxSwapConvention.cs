/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx.type
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for FX Swap trades.
	/// <para>
	/// This defines the market convention for a FX swap based on a particular currency pair.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableFxSwapConvention"/>.
	/// To register a specific convention, see {@code FxSwapConvention.ini}.
	/// </para>
	/// </summary>
	public interface FxSwapConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FxSwapConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxSwapConvention of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<FxSwapConvention> extendedEnum()
	//  {
	//	return FxSwapConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair of the convention.
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  CurrencyPair CurrencyPair {get;}

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
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified periods.
	  /// For example, a '3M x 6M' FX swap has a period from spot to the start date of 3 months and
	  /// a period from spot to the end date of 6 months
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FX Swap, the amount in the first currency of the pair is received in the near leg and paid in the 
	  /// far leg, while the second currency is paid in the near leg and received in the far leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="periodToNear">  the period between the spot date and the near date </param>
	  /// <param name="periodToFar">  the period between the spot date and the far date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the first currency of the currency pair </param>
	  /// <param name="nearFxRate">  the FX rate for the near leg </param>
	  /// <param name="farLegForwardPoints">  the FX points to be added to the FX rate at the far leg </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fx.FxSwapTrade createTrade(java.time.LocalDate tradeDate, java.time.Period periodToNear, java.time.Period periodToFar, com.opengamma.strata.product.common.BuySell buySell, double notional, double nearFxRate, double farLegForwardPoints, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate spotValue = calculateSpotDateFromTradeDate(tradeDate, refData);
	//	LocalDate startDate = spotValue.plus(periodToNear);
	//	LocalDate endDate = spotValue.plus(periodToFar);
	//	return toTrade(tradeDate, startDate, endDate, buySell, notional, nearFxRate, farLegForwardPoints);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FX Swap, the amount in the first currency of the pair is received in the near leg and paid in the 
	  /// far leg, while the second currency is paid in the near leg and received in the far leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="nearFxRate">  the FX rate for the near leg </param>
	  /// <param name="farLegForwardPoints">  the FX points to be added to the FX rate at the far leg </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fx.FxSwapTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double nearFxRate, double farLegForwardPoints)
	//  {
	//
	//	TradeInfo tradeInfo = TradeInfo.of(tradeDate);
	//	return toTrade(tradeInfo, startDate, endDate, buySell, notional, nearFxRate, farLegForwardPoints);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FX Swap, the amount in the first currency of the pair is received in the near leg and paid in the 
	  /// far leg, while the second currency is paid in the near leg and received in the far leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="nearFxRate">  the FX rate for the near leg </param>
	  /// <param name="farLegForwardPoints">  the FX points to be added to the FX rate at the far leg </param>
	  /// <returns> the trade </returns>
	  FxSwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double nearFxRate, double farLegForwardPoints);

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
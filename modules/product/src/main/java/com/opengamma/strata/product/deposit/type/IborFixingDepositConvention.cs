/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A convention for Ibor fixing deposit trades.
	/// <para>
	/// This defines the convention for an Ibor fixing deposit against a particular index.
	/// In most cases, the index contains sufficient information to fully define the convention.
	/// As such, the convention is set to be created on the fly based on the index.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableIborFixingDepositConvention"/>.
	/// To register a specific convention, see {@code IborFixingDepositConvention.ini}.
	/// </para>
	/// </summary>
	public interface IborFixingDepositConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static IborFixingDepositConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFixingDepositConvention of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Obtains a convention based on the specified index.
	  /// <para>
	  /// This uses the index name to find the matching convention.
	  /// By default, this will always return a convention, however configuration may be added
	  /// to restrict the conventions that are registered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index, from which the index name is used to find the matching convention </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if no convention is registered for the index </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFixingDepositConvention of(com.opengamma.strata.basics.index.IborIndex index)
	//  {
	//	ArgChecker.notNull(index, "index");
	//	return extendedEnum().lookup(index.getName());
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<IborFixingDepositConvention> extendedEnum()
	//  {
	//	return IborFixingDepositConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The floating rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  IborIndex Index {get;}

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
	  /// This returns a trade based on the specified deposit period.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the Ibor fixing deposit, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// If selling the Ibor fixing deposit, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="depositPeriod">  the period between the start date and the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  IborFixingDepositTrade createTrade(LocalDate tradeDate, Period depositPeriod, BuySell buySell, double notional, double fixedRate, ReferenceData refData);

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the Ibor fixing deposit, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// If selling the Ibor fixing deposit, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.deposit.IborFixingDepositTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate)
	//  {
	//
	//	TradeInfo tradeInfo = TradeInfo.of(tradeDate);
	//	return toTrade(tradeInfo, startDate, endDate, buySell, notional, fixedRate);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the Ibor fixing deposit, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// If selling the Ibor fixing deposit, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the trade </returns>
	  IborFixingDepositTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate);

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
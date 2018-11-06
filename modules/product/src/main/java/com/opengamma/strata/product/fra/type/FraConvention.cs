/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
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
	/// A market convention for forward rate agreement (FRA) trades.
	/// <para>
	/// This defines the market convention for a FRA against a particular index.
	/// In most cases, the index contains sufficient information to fully define the convention.
	/// As such, the convention is set to be created on the fly based on the index.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableFraConvention"/>.
	/// To register a specific convention, see {@code FraConvention.ini}.
	/// </para>
	/// </summary>
	public interface FraConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FraConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FraConvention of(String uniqueName)
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
//	  public static FraConvention of(com.opengamma.strata.basics.index.IborIndex index)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<FraConvention> extendedEnum()
	//  {
	//	return FraConventions.ENUM_LOOKUP;
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
	  /// Creates a trade based on this convention, using the index tenor to define the end of the FRA.
	  /// <para>
	  /// This returns a trade based on the specified period to start.
	  /// For example, a '2 x 5' FRA has a period to the start date of 2 months.
	  /// The period to the end, 5 months, is implied by adding the tenor of the index,
	  /// 3 months, to the period to start.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FRA, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// If selling the FRA, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// </para>
	  /// <para>
	  /// The start date will be the trade date, plus spot offset, plus period to start, adjusted to a valid business day.
	  /// The end date will be the trade date, plus spot offset, plus period to start, plus index tenor, adjusted to a valid business day.
	  /// The adjustment of the start and end date occurs at trade creation.
	  /// The payment date offset is also applied at trade creation.
	  /// When the Fra is <seealso cref="Fra#resolve(ReferenceData) resolved"/>, the start and end date
	  /// are not adjusted again but the payment date is.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fra.FraTrade createTrade(java.time.LocalDate tradeDate, java.time.Period periodToStart, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	Period periodToEnd = periodToStart.plus(getIndex().getTenor());
	//	return createTrade(tradeDate, periodToStart, periodToEnd, buySell, notional, fixedRate, refData);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention, specifying the end of the FRA.
	  /// <para>
	  /// This returns a trade based on the specified periods.
	  /// For example, a '2 x 5' FRA has a period to the start date of 2 months and
	  /// a period to the end date of 5 months.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FRA, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// If selling the FRA, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// </para>
	  /// <para>
	  /// The start date will be the trade date, plus spot offset, plus period to start, adjusted to a valid business day.
	  /// The end date will be the trade date, plus spot offset, plus period to end, adjusted to a valid business day.
	  /// The adjustment of the start and end date occurs at trade creation.
	  /// The payment date offset is also applied at trade creation.
	  /// When the Fra is <seealso cref="Fra#resolve(ReferenceData) resolved"/>, the start and end date
	  /// are not adjusted again but the payment date is.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="periodToEnd">  the period between the spot date and the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  FraTrade createTrade(LocalDate tradeDate, Period periodToStart, Period periodToEnd, BuySell buySell, double notional, double fixedRate, ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FRA, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// If selling the FRA, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date, which should be adjusted to be a valid business day </param>
	  /// <param name="endDate">  the end date, which should be adjusted to be a valid business day </param>
	  /// <param name="paymentDate">  the payment date, which should be adjusted to be a valid business day </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.fra.FraTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, java.time.LocalDate paymentDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate)
	//  {
	//
	//	TradeInfo tradeInfo = TradeInfo.of(tradeDate);
	//	return toTrade(tradeInfo, startDate, endDate, paymentDate, buySell, notional, fixedRate);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the FRA, the floating rate is received from the counterparty, with the fixed rate being paid.
	  /// If selling the FRA, the floating rate is paid to the counterparty, with the fixed rate being received.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade </param>
	  /// <param name="startDate">  the start date, which should be adjusted to be a valid business day </param>
	  /// <param name="endDate">  the end date, which should be adjusted to be a valid business day </param>
	  /// <param name="paymentDate">  the payment date, which should be adjusted to be a valid business day </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="fixedRate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the trade </returns>
	  FraTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, LocalDate paymentDate, BuySell buySell, double notional, double fixedRate);

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
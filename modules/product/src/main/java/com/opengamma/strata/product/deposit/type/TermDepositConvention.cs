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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for term deposit trades.
	/// <para>
	/// This defines the market convention for a term deposit.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableTermDepositConvention"/>.
	/// To register a specific convention, see {@code TermDepositConvention.ini}.
	/// </para>
	/// </summary>
	public interface TermDepositConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static TermDepositConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static TermDepositConvention of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<TermDepositConvention> extendedEnum()
	//  {
	//	return TermDepositConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency.
	  /// <para>
	  /// This is the currency of the term deposit and the currency that payment is made in.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  Currency Currency {get;}

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
	  /// If buying the term deposit, the principal is paid at the start date and the
	  /// principal plus interest is received at the end date.
	  /// If selling the term deposit, the principal is received at the start date and the
	  /// principal plus interest is paid at the end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="depositPeriod">  the period between the start date and the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="rate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.deposit.TermDepositTrade createTrade(java.time.LocalDate tradeDate, java.time.Period depositPeriod, com.opengamma.strata.product.common.BuySell buySell, double notional, double rate, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate startDate = calculateSpotDateFromTradeDate(tradeDate, refData);
	//	LocalDate endDate = startDate.plus(depositPeriod);
	//	return toTrade(tradeDate, startDate, endDate, buySell, notional, rate);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the term deposit, the principal is paid at the start date and the
	  /// principal plus interest is received at the end date.
	  /// If selling the term deposit, the principal is received at the start date and the
	  /// principal plus interest is paid at the end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="rate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.deposit.TermDepositTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double rate)
	//  {
	//
	//	TradeInfo tradeInfo = TradeInfo.of(tradeDate);
	//	return toTrade(tradeInfo, startDate, endDate, buySell, notional, rate);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the term deposit, the principal is paid at the start date and the
	  /// principal plus interest is received at the end date.
	  /// If selling the term deposit, the principal is received at the start date and the
	  /// principal plus interest is paid at the end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="rate">  the fixed rate, typically derived from the market </param>
	  /// <returns> the trade </returns>
	  TermDepositTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double rate);

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
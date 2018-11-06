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
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for three leg basis swap trades.
	/// <para>
	/// This defines the market convention for a single currency basis swap.
	/// The convention is formed by combining three swap leg conventions in the same currency.
	/// </para>
	/// <para>
	/// The market price is for the difference (spread) between the values of the two floating legs.
	/// This convention has three legs, the "spread leg", the "spread floating leg" and the "flat floating leg". 
	/// The "spread leg" represented by the fixed leg will be added to the "spread floating leg" 
	/// which is typically the leg with the shorter underlying tenor.
	/// Thus the "spread leg" and "spread floating leg" will have the same pay/receive direction.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableThreeLegBasisSwapConvention"/>.
	/// To register a specific convention, see {@code ThreeLegBasisSwapConvention.ini}.
	/// </para>
	/// </summary>
	public interface ThreeLegBasisSwapConvention : SingleCurrencySwapConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ThreeLegBasisSwapConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ThreeLegBasisSwapConvention of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<ThreeLegBasisSwapConvention> extendedEnum()
	//  {
	//	return ThreeLegBasisSwapConventions.ENUM_LOOKUP;
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the spread leg.
	  /// </summary>
	  /// <returns> the spread leg convention </returns>
	  FixedRateSwapLegConvention SpreadLeg {get;}

	  /// <summary>
	  /// Gets the market convention of the floating leg to which the spread leg is added.
	  /// </summary>
	  /// <returns> the spread floating leg convention </returns>
	  IborRateSwapLegConvention SpreadFloatingLeg {get;}

	  /// <summary>
	  /// Gets the market convention of the floating leg that does not have the spread applied.
	  /// </summary>
	  /// <returns> the flat floating leg convention </returns>
	  IborRateSwapLegConvention FlatFloatingLeg {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a spot-starting trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified tenor. For example, a tenor
	  /// of 5 years creates a swap starting on the spot date and maturing 5 years later.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the floating flat leg is received from the counterparty,
	  /// with the rate of the floating spread leg and the spread of the fixed leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade createTrade(java.time.LocalDate tradeDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double spread, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	// override for Javadoc
	//	return SingleCurrencySwapConvention.this.createTrade(tradeDate, tenor, buySell, notional, spread, refData);
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
	  /// If buying the swap, the rate of the floating flat leg is received from the counterparty,
	  /// with the rate of the floating spread leg and the spread of the fixed leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="periodToStart">  the period between the spot date and the start date </param>
	  /// <param name="tenor">  the tenor of the swap </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade createTrade(java.time.LocalDate tradeDate, java.time.Period periodToStart, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double spread, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	// override for Javadoc
	//	return SingleCurrencySwapConvention.this.createTrade(tradeDate, periodToStart, tenor, buySell, notional, spread, refData);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the floating flat leg is received from the counterparty,
	  /// with the rate of the floating spread leg and the spread of the fixed leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <returns> the trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.swap.SwapTrade toTrade(java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double spread)
	//  {
	//
	//	// override for Javadoc
	//	return SingleCurrencySwapConvention.this.toTrade(tradeDate, startDate, endDate, buySell, notional, spread);
	//  }

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified dates.
	  /// </para>
	  /// <para>
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the swap, the rate of the floating flat leg is received from the counterparty,
	  /// with the rate of the floating spread leg and the spread of the fixed leg being paid.
	  /// If selling the swap, the opposite occurs.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeInfo">  additional information about the trade </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  the buy/sell flag </param>
	  /// <param name="notional">  the notional amount </param>
	  /// <param name="spread">  the spread, typically derived from the market </param>
	  /// <returns> the trade </returns>
	  SwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double spread);

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
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A market convention for Ibor Future trades.
	/// <para>
	/// This defines the market convention for a future against a particular index.
	/// </para>
	/// <para>
	/// To manually create a convention, see <seealso cref="ImmutableIborFutureConvention"/>.
	/// To register a specific convention, see {@code IborFutureConvention.ini}.
	/// </para>
	/// </summary>
	public interface IborFutureConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static IborFutureConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFutureConvention of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<IborFutureConvention> extendedEnum()
	//  {
	//	return IborFutureConventions.ENUM_LOOKUP;
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

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified minimum period and sequence number.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="securityId">  the identifier of the security </param>
	  /// <param name="minimumPeriod">  minimum period between the value date and the first future </param>
	  /// <param name="sequenceNumber">  the 1-based sequence number of the futures </param>
	  /// <param name="quantity">  the number of contracts traded, positive if buying, negative if selling </param>
	  /// <param name="notional">  the notional amount of one future contract </param>
	  /// <param name="price">  the trade price of the future </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, Period minimumPeriod, int sequenceNumber, double quantity, double notional, double price, ReferenceData refData);

	  /// <summary>
	  /// Creates a trade based on this convention.
	  /// <para>
	  /// This returns a trade based on the specified year-month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="securityId">  the identifier of the security </param>
	  /// <param name="yearMonth">  the year-month that the future is defined to be for </param>
	  /// <param name="quantity">  the number of contracts traded, positive if buying, negative if selling </param>
	  /// <param name="notional">  the notional amount of one future contract </param>
	  /// <param name="price">  the trade price of the future </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, YearMonth yearMonth, double quantity, double notional, double price, ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the reference date from the trade date.
	  /// <para>
	  /// This determines the date from the specified minimum period and sequence number.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="minimumPeriod">  minimum period between the trade date and the first future </param>
	  /// <param name="sequenceNumber">  the 1-based sequence number of the futures </param>
	  /// <param name="refData">  the reference data, used to resolve the date </param>
	  /// <returns> the future reference date </returns>
	  LocalDate calculateReferenceDateFromTradeDate(LocalDate tradeDate, Period minimumPeriod, int sequenceNumber, ReferenceData refData);

	  /// <summary>
	  /// Calculates the reference date from the trade date.
	  /// <para>
	  /// This determines the date from the specified year-month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="yearMonth">  the year-month that the future is defined to be for </param>
	  /// <param name="refData">  the reference data, used to resolve the date </param>
	  /// <returns> the future reference date </returns>
	  LocalDate calculateReferenceDateFromTradeDate(LocalDate tradeDate, YearMonth yearMonth, ReferenceData refData);

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
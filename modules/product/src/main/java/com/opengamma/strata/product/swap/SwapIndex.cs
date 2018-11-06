/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// A swap index.
	/// <para>
	/// Swap rates for CHF, EUR, GBP, JPY and USD are established by ISDA in co-operation with
	/// Reuters (now Thomson Reuters) and Intercapital Brokers (now ICAP plc). 
	/// Ref: https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="SwapIndices"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface SwapIndex : Index, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static SwapIndex of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SwapIndex of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the index to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<SwapIndex> extendedEnum()
	//  {
	//	return SwapIndices.ENUM_LOOKUP;
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the index is active.
	  /// <para>
	  /// Over time some indices become inactive and are no longer produced.
	  /// If this occurs, this method will return false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the index is active, false if inactive </returns>
	  bool Active {get;}

	  /// <summary>
	  /// Gets the fixing time of the index.
	  /// <para>
	  /// The fixing time is related to the fixing date and time-zone.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing time </returns>
	  LocalTime FixingTime {get;}

	  /// <summary>
	  /// Gets the time-zone of the fixing time.
	  /// <para>
	  /// The fixing time-zone is related to the fixing date and time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the time-zone of the fixing time </returns>
	  ZoneId FixingZone {get;}

	  /// <summary>
	  /// Gets the template for creating Fixed-Ibor swap.
	  /// </summary>
	  /// <returns> the template </returns>
	  FixedIborSwapTemplate Template {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the fixing date-time from the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The result combines the date with the time and zone stored in the index.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid fixing date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <returns> the fixing date-time </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.ZonedDateTime calculateFixingDateTime(java.time.LocalDate fixingDate)
	//  {
	//	return fixingDate.atTime(getFixingTime()).atZone(getFixingZone());
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this index.
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
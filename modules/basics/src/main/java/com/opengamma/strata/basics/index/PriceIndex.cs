/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Country = com.opengamma.strata.basics.location.Country;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An index of prices.
	/// <para>
	/// A price index is a normalized average of the prices of goods and/or services.
	/// Well-known price indices are published by Governments, such as the Consumer Price Index.
	/// The annualized percentage change in the index is a measure of inflation.
	/// </para>
	/// <para>
	/// This interface represents a price index for a specific region.
	/// The index is typically published monthly in arrears, however some regions
	/// choose quarterly publication.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="PriceIndices"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface PriceIndex : FloatingRateIndex, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static PriceIndex of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PriceIndex of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<PriceIndex> extendedEnum()
	//  {
	//	return PriceIndices.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the region that the index is defined for.
	  /// </summary>
	  /// <returns> the region of the index </returns>
	  Country Region {get;}

	  /// <summary>
	  /// Gets the day count convention of the index, which is '1/1'.
	  /// </summary>
	  /// <returns> the day count convention </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.date.DayCount getDayCount()
	//  {
	//	return DayCounts.ONE_ONE;
	//  }

	  /// <summary>
	  /// Gets the frequency that the index is published.
	  /// <para>
	  /// Most price indices are published monthly, but some are published quarterly.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the frequency of publication of the index </returns>
	  Frequency PublicationFrequency {get;}

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
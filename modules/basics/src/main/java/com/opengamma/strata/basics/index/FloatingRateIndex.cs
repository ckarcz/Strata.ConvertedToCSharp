/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An index used to provide floating rates, typically in interest rate swaps.
	/// <para>
	/// See <seealso cref="IborIndex"/>, <seealso cref="OvernightIndex"/> and  <seealso cref="PriceIndex"/> for more details.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface FloatingRateIndex : Index
	{

	  /// <summary>
	  /// Parses a string, handling different types of index.
	  /// <para>
	  /// This tries a number of ways to parse the input:
	  /// <ul>
	  /// <li><seealso cref="IborIndex#of(String)"/>
	  /// <li><seealso cref="OvernightIndex#of(String)"/>
	  /// <li><seealso cref="PriceIndex#of(String)"/>
	  /// <li><seealso cref="FloatingRateName#of(String)"/>
	  /// </ul>
	  /// If {@code FloatingRateName} is used to match an Ibor index, then a tenor is needed
	  /// to return an index. The tenor from <seealso cref="FloatingRateName#getDefaultTenor()"/> will be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexStr">  the index string to parse </param>
	  /// <returns> the floating rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FloatingRateIndex parse(String indexStr)
	//  {
	//	return parse(indexStr, null);
	//  }

	  /// <summary>
	  /// Parses a string, handling different types of index, optionally specifying a tenor for Ibor.
	  /// <para>
	  /// This tries a number of ways to parse the input:
	  /// <ul>
	  /// <li><seealso cref="IborIndex#of(String)"/>
	  /// <li><seealso cref="OvernightIndex#of(String)"/>
	  /// <li><seealso cref="PriceIndex#of(String)"/>
	  /// <li><seealso cref="FloatingRateName#of(String)"/>
	  /// </ul>
	  /// If {@code FloatingRateName} is used to match an Ibor index, then a tenor is needed
	  /// to return an index. The tenor can optionally be supplied. If needed and missing,
	  /// the result of <seealso cref="FloatingRateName#getDefaultTenor()"/> will be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexStr">  the index string to parse </param>
	  /// <param name="defaultIborTenor">  the tenor to use for Ibor if matched as a {@code FloatingRateName}, may be null </param>
	  /// <returns> the floating rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FloatingRateIndex parse(String indexStr, com.opengamma.strata.basics.date.Tenor defaultIborTenor)
	//  {
	//	ArgChecker.notNull(indexStr, "indexStr");
	//	return tryParse(indexStr, defaultIborTenor).orElseThrow(() -> new IllegalArgumentException("Floating rate index not known: " + indexStr));
	//  }

	  /// <summary>
	  /// Parses a string, handling different types of index.
	  /// <para>
	  /// This tries a number of ways to parse the input:
	  /// <ul>
	  /// <li><seealso cref="IborIndex#of(String)"/>
	  /// <li><seealso cref="OvernightIndex#of(String)"/>
	  /// <li><seealso cref="PriceIndex#of(String)"/>
	  /// <li><seealso cref="FloatingRateName#of(String)"/>
	  /// </ul>
	  /// If {@code FloatingRateName} is used to match an Ibor index, then a tenor is needed
	  /// to return an index. The tenor from <seealso cref="FloatingRateName#getDefaultTenor()"/> will be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexStr">  the index string to parse </param>
	  /// <returns> the floating rate index, empty if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static java.util.Optional<FloatingRateIndex> tryParse(String indexStr)
	//  {
	//	return tryParse(indexStr, null);
	//  }

	  /// <summary>
	  /// Parses a string, handling different types of index, optionally specifying a tenor for Ibor.
	  /// <para>
	  /// This tries a number of ways to parse the input:
	  /// <ul>
	  /// <li><seealso cref="IborIndex#of(String)"/>
	  /// <li><seealso cref="OvernightIndex#of(String)"/>
	  /// <li><seealso cref="PriceIndex#of(String)"/>
	  /// <li><seealso cref="FloatingRateName#of(String)"/>
	  /// </ul>
	  /// If {@code FloatingRateName} is used to match an Ibor index, then a tenor is needed
	  /// to return an index. The tenor can optionally be supplied. If needed and missing,
	  /// the result of <seealso cref="FloatingRateName#getDefaultTenor()"/> will be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexStr">  the index string to parse </param>
	  /// <param name="defaultIborTenor">  the tenor to use for Ibor if matched as a {@code FloatingRateName}, may be null </param>
	  /// <returns> the floating rate index, empty if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"rawtypes", "unchecked"}) public static java.util.Optional<FloatingRateIndex> tryParse(String indexStr, com.opengamma.strata.basics.date.Tenor defaultIborTenor)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static java.util.Optional<FloatingRateIndex> tryParse(String indexStr, com.opengamma.strata.basics.date.Tenor defaultIborTenor)
	//  {
	//	Optional<IborIndex> iborOpt = IborIndex.extendedEnum().find(indexStr);
	//	if (iborOpt.isPresent())
	//	{
	//	  return (Optional) iborOpt;
	//	}
	//	Optional<OvernightIndex> overnightOpt = OvernightIndex.extendedEnum().find(indexStr);
	//	if (overnightOpt.isPresent())
	//	{
	//	  return (Optional) overnightOpt;
	//	}
	//	Optional<PriceIndex> priceOpt = PriceIndex.extendedEnum().find(indexStr);
	//	if (priceOpt.isPresent())
	//	{
	//	  return (Optional) priceOpt;
	//	}
	//	Optional<FloatingRateName> frnOpt = FloatingRateName.extendedEnum().find(indexStr);
	//	if (frnOpt.isPresent())
	//	{
	//	  return frnOpt.map(frn -> frn.toFloatingRateIndex(defaultIborTenor != null ? defaultIborTenor : frn.getDefaultTenor()));
	//	}
	//	return Optional.empty();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the index.
	  /// </summary>
	  /// <returns> the currency of the index </returns>
	  Currency Currency {get;}

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
	  /// Gets the day count convention of the index.
	  /// </summary>
	  /// <returns> the day count convention </returns>
	  DayCount DayCount {get;}

	  /// <summary>
	  /// Gets the floating rate name for this index.
	  /// <para>
	  /// For an Ibor index, the <seealso cref="FloatingRateName"/> does not include the tenor.
	  /// It can be used to find the other tenors available for this index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the floating rate name </returns>
	  FloatingRateName FloatingRateName {get;}

	  /// <summary>
	  /// Gets the default day count convention for the associated fixed leg.
	  /// <para>
	  /// A rate index is often paid against a fixed leg, such as in a vanilla Swap.
	  /// The day count convention of the fixed leg often differs from that of the index,
	  /// and the default is value is available here.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the day count convention </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.date.DayCount getDefaultFixedLegDayCount()
	//  {
	//	return getDayCount();
	//  }

	}

}
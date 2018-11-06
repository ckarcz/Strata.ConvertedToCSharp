using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Iterables = com.google.common.collect.Iterables;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A floating rate index name, such as Libor, Euribor or US Fed Fund.
	/// <para>
	/// An index represented by this class relates to some form of floating rate.
	/// This can include <seealso cref="IborIndex"/> and <seealso cref="OvernightIndex"/> values.
	/// </para>
	/// <para>
	/// This class is designed to match the FpML/ISDA floating rate index concept.
	/// The FpML concept provides a single key for floating rates of a variety of
	/// types, mixing  Ibor, Overnight, Price and Swap indices.
	/// It also sometimes includes a source, such as 'Bloomberg' or 'Reuters'.
	/// This class matches the single concept and provided a bridge the more
	/// specific index implementations used for pricing.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="FloatingRateNames"/>.
	/// </para>
	/// <para>
	/// The set of supported values, and their mapping to {@code IborIndex}, {@code PriceIndex}
	/// and {@code OvernightIndex}, is defined in the {@code FloatingRateName.ini}
	/// config file.
	/// </para>
	/// </summary>
	public interface FloatingRateName : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the floating rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FloatingRateName of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FloatingRateName of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the floating rate to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<FloatingRateName> extendedEnum()
	//  {
	//	return FloatingRateNames.ENUM_LOOKUP;
	//  }

	  /// <summary>
	  /// Parses a string, with extended handling of indices.
	  /// <para>
	  /// This tries a number of ways to parse the input:
	  /// <ul>
	  /// <li><seealso cref="FloatingRateName#of(String)"/>
	  /// <li><seealso cref="IborIndex#of(String)"/>
	  /// <li><seealso cref="OvernightIndex#of(String)"/>
	  /// <li><seealso cref="PriceIndex#of(String)"/>
	  /// </ul>
	  /// Note that if an <seealso cref="IborIndex"/> is parsed, the tenor will be lost.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the floating rate </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FloatingRateName parse(String str)
	//  {
	//	ArgChecker.notNull(str, "str");
	//	return tryParse(str).orElseThrow(() -> new IllegalArgumentException("Floating rate name not known: " + str));
	//  }

	  /// <summary>
	  /// Tries to parse a string, with extended handling of indices.
	  /// <para>
	  /// This tries a number of ways to parse the input:
	  /// <ul>
	  /// <li><seealso cref="FloatingRateName#of(String)"/>
	  /// <li><seealso cref="IborIndex#of(String)"/>
	  /// <li><seealso cref="OvernightIndex#of(String)"/>
	  /// <li><seealso cref="PriceIndex#of(String)"/>
	  /// </ul>
	  /// Note that if an <seealso cref="IborIndex"/> is parsed, the tenor will be lost.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the string to parse </param>
	  /// <returns> the floating rate, empty if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static java.util.Optional<FloatingRateName> tryParse(String str)
	//  {
	//	Optional<FloatingRateName> frnOpt = FloatingRateName.extendedEnum().find(str);
	//	if (frnOpt.isPresent())
	//	{
	//	  return frnOpt;
	//	}
	//	return FloatingRateIndex.tryParse(str).map(FloatingRateIndex::getFloatingRateName);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the default Ibor index for a currency.
	  /// </summary>
	  /// <param name="currency">  the currency to find the default for </param>
	  /// <returns> the floating rate </returns>
	  /// <exception cref="IllegalArgumentException"> if there is no default for the currency </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FloatingRateName defaultIborIndex(com.opengamma.strata.basics.currency.Currency currency)
	//  {
	//	return FloatingRateNameIniLookup.INSTANCE.defaultIborIndex(currency);
	//  }

	  /// <summary>
	  /// Gets the default Overnight index for a currency.
	  /// </summary>
	  /// <param name="currency">  the currency to find the default for </param>
	  /// <returns> the floating rate </returns>
	  /// <exception cref="IllegalArgumentException"> if there is no default for the currency </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FloatingRateName defaultOvernightIndex(com.opengamma.strata.basics.currency.Currency currency)
	//  {
	//	return FloatingRateNameIniLookup.INSTANCE.defaultOvernightIndex(currency);
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this floating rate.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// It will be the external name, typically from FpML, such as 'GBP-LIBOR-BBA'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the external name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	  /// <summary>
	  /// Gets the type of the index - Ibor, Overnight or Price.
	  /// </summary>
	  /// <returns> index type - Ibor, Overnight or Price </returns>
	  FloatingRateType Type {get;}

	  /// <summary>
	  /// Gets the currency of the floating rate.
	  /// </summary>
	  /// <returns> the currency </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to return an index, which should
	  ///   only happen if the system is not configured correctly </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.Currency getCurrency()
	//  {
	//	return toFloatingRateIndex().getCurrency();
	//  }

	  /// <summary>
	  /// Gets the active tenors that are applicable for this floating rate.
	  /// <para>
	  /// Overnight and Price indices will return an empty set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the available tenors </returns>
	  ISet<Tenor> Tenors {get;}

	  /// <summary>
	  /// Gets a default tenor applicable for this floating rate.
	  /// <para>
	  /// This is useful for providing a basic default where errors need to be avoided.
	  /// The value returned is not intended to be based on market conventions.
	  /// </para>
	  /// <para>
	  /// Ibor floating rates will return 3M, or 13W if that is not available, otherwise
	  /// the first entry from the set of tenors.
	  /// Overnight floating rates will return 1D.
	  /// All other floating rates will return return 1Y.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the default tenor </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.date.Tenor getDefaultTenor()
	//  {
	//	switch (getType())
	//	{
	//	  case IBOR:
	//	  {
	//		Set<Tenor> tenors = getTenors();
	//		if (tenors.contains(Tenor.TENOR_3M))
	//		{
	//		  return Tenor.TENOR_3M;
	//		}
	//		if (tenors.contains(Tenor.TENOR_13W))
	//		{
	//		  return Tenor.TENOR_13W;
	//		}
	//		return tenors.iterator().next();
	//	  }
	//	  case OVERNIGHT_AVERAGED:
	//	  case OVERNIGHT_COMPOUNDED:
	//		return Tenor.TENOR_1D;
	//	  default:
	//		return Tenor.TENOR_1Y;
	//	}
	//  }

	  /// <summary>
	  /// Gets the normalized form of the floating rate name.
	  /// <para>
	  /// The normalized for is the name that Strata uses for the index.
	  /// For example, the normalized form of 'GBP-LIBOR-BBA' is 'GBP-LIBOR',
	  /// and the normalized form of 'EUR-EURIBOR-Reuters' is 'EUR-EURIBOR'.
	  /// Note that for Ibor indices, the tenor is not present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the normalized name </returns>
	  FloatingRateName normalized();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a floating rate index.
	  /// <para>
	  /// Returns a <seealso cref="FloatingRateIndex"/> for this rate name.
	  /// Only Ibor, Overnight and Price indices are handled.
	  /// If the rate name is an Ibor rate, the <seealso cref="#getDefaultTenor() default tenor"/> is used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to return an index, which should
	  ///   only happen if the system is not configured correctly </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default FloatingRateIndex toFloatingRateIndex()
	//  {
	//	// code copied to avoid calling getDefaultTenor() unless necessary
	//	switch (getType())
	//	{
	//	  case IBOR:
	//		return toIborIndex(getDefaultTenor());
	//	  case OVERNIGHT_COMPOUNDED:
	//	  case OVERNIGHT_AVERAGED:
	//		return toOvernightIndex();
	//	  case PRICE:
	//		return toPriceIndex();
	//	  default:
	//		throw new IllegalArgumentException("Floating rate index type not known: " + getType());
	//	}
	//  }

	  /// <summary>
	  /// Returns a floating rate index.
	  /// <para>
	  /// Returns a <seealso cref="FloatingRateIndex"/> for this rate name.
	  /// Only Ibor, Overnight and Price indices are handled.
	  /// If the rate name is an Ibor rate, the specified tenor is used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="iborTenor">  the tenor to use if this rate is Ibor </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to return an index, which should
	  ///   only happen if the system is not configured correctly </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default FloatingRateIndex toFloatingRateIndex(com.opengamma.strata.basics.date.Tenor iborTenor)
	//  {
	//	switch (getType())
	//	{
	//	  case IBOR:
	//		return toIborIndex(iborTenor);
	//	  case OVERNIGHT_COMPOUNDED:
	//	  case OVERNIGHT_AVERAGED:
	//		return toOvernightIndex();
	//	  case PRICE:
	//		return toPriceIndex();
	//	  default:
	//		throw new IllegalArgumentException("Floating rate index type not known: " + getType());
	//	}
	//  }

	  /// <summary>
	  /// Checks and returns an Ibor index.
	  /// <para>
	  /// If this name represents an Ibor index, then this method returns the matching <seealso cref="IborIndex"/>.
	  /// If not, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor of the index </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalStateException"> if the type is not an Ibor index type </exception>
	  IborIndex toIborIndex(Tenor tenor);

	  /// <summary>
	  /// Checks and returns the fixing offset associated with the Ibor index.
	  /// <para>
	  /// If this name represents an Ibor index, then this method returns the associated fixing offset.
	  /// If not, an exception is thrown.
	  /// </para>
	  /// <para>
	  /// This method exists primarily to handle DKK CIBOR, where there are two floating rate names but
	  /// only one underlying index. The CIBOR index itself has a convention where the fixing date is 2 days
	  /// before the reset date and the effective date is 2 days after the fixing date, matching the name "DKK-CIBOR2-DKNA13".
	  /// The alternative name, "DKK-CIBOR-DKNA13", has the fixing date equal to the reset date, but with
	  /// the effective date two days later.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing offset applicable to the index </returns>
	  /// <exception cref="IllegalStateException"> if the type is not an Ibor index type </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.date.DaysAdjustment toIborIndexFixingOffset()
	//  {
	//	return toIborIndex(Iterables.getFirst(getTenors(), Tenor.TENOR_3M)).getFixingDateOffset();
	//  }

	  /// <summary>
	  /// Converts to an <seealso cref="OvernightIndex"/>.
	  /// <para>
	  /// If this name represents an Overnight index, then this method returns the matching <seealso cref="OvernightIndex"/>.
	  /// If not, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalStateException"> if the type is not an Overnight index type </exception>
	  OvernightIndex toOvernightIndex();

	  /// <summary>
	  /// Converts to an <seealso cref="PriceIndex"/>.
	  /// <para>
	  /// If this name represents a price index, then this method returns the matching <seealso cref="PriceIndex"/>.
	  /// If not, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalStateException"> if the type is not a price index type </exception>
	  PriceIndex toPriceIndex();

	}

}
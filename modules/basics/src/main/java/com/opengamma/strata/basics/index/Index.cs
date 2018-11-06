/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An index of values, such as LIBOR, FED FUND or daily exchange rates.
	/// <para>
	/// An index is an agreed mechanism for determining certain financial indicators,
	/// such as exchange rate or interest rates. Most common indices are daily.
	/// </para>
	/// <para>
	/// This is a marker interface, see <seealso cref="FxIndex"/>, <seealso cref="IborIndex"/>
	/// and <seealso cref="OvernightIndex"/> for more information.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface Index : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static Index of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static Index of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return Indices.ENUM_LOOKUP.lookup(uniqueName);
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
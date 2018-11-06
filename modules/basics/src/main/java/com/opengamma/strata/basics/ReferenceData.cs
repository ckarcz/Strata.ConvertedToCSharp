/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{

	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendars = com.opengamma.strata.basics.date.HolidayCalendars;

	/// <summary>
	/// Provides access to reference data, such as holiday calendars and securities.
	/// <para>
	/// Reference data is looked up using implementations of <seealso cref="ReferenceDataId"/>.
	/// The identifier is parameterized with the type of the reference data to be returned.
	/// </para>
	/// <para>
	/// The standard implementation is <seealso cref="ImmutableReferenceData"/>.
	/// </para>
	/// </summary>
	public interface ReferenceData
	{

	  /// <summary>
	  /// Obtains an instance from a map of reference data.
	  /// <para>
	  /// Each entry in the map is a single piece of reference data, keyed by the matching identifier.
	  /// For example, a <seealso cref="HolidayCalendar"/> can be looked up using a <seealso cref="HolidayCalendarId"/>.
	  /// The caller must ensure that the each entry in the map corresponds with the parameterized
	  /// type on the identifier.
	  /// </para>
	  /// <para>
	  /// The resulting {@code ReferenceData} instance will include the <seealso cref="#minimal() minimal"/>
	  /// set of reference data that includes non-controversial identifiers that are essential for pricing.
	  /// To exclude the minimal set of identifiers, use <seealso cref="ImmutableReferenceData#of(Map)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="values">  the reference data values </param>
	  /// <returns> the reference data instance containing the values in the map </returns>
	  /// <exception cref="ClassCastException"> if a value does not match the parameterized type associated with the identifier </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ReferenceData of(java.util.Map<JavaToDotNetGenericWildcard extends ReferenceDataId<JavaToDotNetGenericWildcard>, JavaToDotNetGenericWildcard> values)
	//  {
	//	// hash map so that keys can overlap, with this instance taking priority
	//	Map<ReferenceDataId<?>, Object> combined = new HashMap<>();
	//	combined.putAll(StandardReferenceData.MINIMAL.getValues());
	//	combined.putAll(values);
	//	return ImmutableReferenceData.of(combined);
	//  }

	  /// <summary>
	  /// Obtains an instance of standard reference data.
	  /// <para>
	  /// Standard reference data is built into Strata and provides common holiday calendars and indices.
	  /// In most cases, production usage of Strata will not rely on this source of reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> standard reference data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ReferenceData standard()
	//  {
	//	return StandardReferenceData.STANDARD;
	//  }

	  /// <summary>
	  /// Obtains the minimal set of reference data.
	  /// <para>
	  /// The <seealso cref="#standard() standard"/> reference data contains common holiday calendars
	  /// and indices, but may not be suitable for production use. The minimal reference data contains
	  /// just those identifiers that are needed by Strata, and that are non-controversial.
	  /// These are <seealso cref="HolidayCalendars#NO_HOLIDAYS"/>, <seealso cref="HolidayCalendars#SAT_SUN"/>,
	  /// <seealso cref="HolidayCalendars#FRI_SAT"/> and <seealso cref="HolidayCalendars#THU_FRI"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> minimal reference data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ReferenceData minimal()
	//  {
	//	return StandardReferenceData.MINIMAL;
	//  }

	  /// <summary>
	  /// Obtains an instance containing no reference data.
	  /// </summary>
	  /// <returns> empty reference data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ReferenceData empty()
	//  {
	//	return ImmutableReferenceData.empty();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this reference data contains a value for the specified identifier.
	  /// </summary>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> true if the reference data contains a value for the identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean containsValue(ReferenceDataId<JavaToDotNetGenericWildcard> id)
	//  {
	//	return id.queryValueOrNull(this) != null;
	//  }

	  /// <summary>
	  /// Gets the reference data value associated with the specified identifier.
	  /// <para>
	  /// If this reference data instance contains the identifier, the value will be returned.
	  /// Otherwise, an exception will be thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the reference data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the reference data value </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if the identifier is not found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> T getValue(ReferenceDataId<T> id)
	//  {
	//	T value = id.queryValueOrNull(this);
	//	if (value == null)
	//	{
	//	  throw new ReferenceDataNotFoundException(ImmutableReferenceData.msgValueNotFound(id));
	//	}
	//	return value;
	//  }

	  /// <summary>
	  /// Finds the reference data value associated with the specified identifier.
	  /// <para>
	  /// If this reference data instance contains the identifier, the value will be returned.
	  /// Otherwise, an empty optional will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the reference data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the reference data value, empty if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> java.util.Optional<T> findValue(ReferenceDataId<T> id)
	//  {
	//	T value = id.queryValueOrNull(this);
	//	return Optional.ofNullable(value);
	//  }

	  /// <summary>
	  /// Low-level method to query the reference data value associated with the specified identifier,
	  /// returning null if not found.
	  /// <para>
	  /// This is a low-level method that obtains the reference data value, returning null instead of an error.
	  /// Applications should use <seealso cref="#getValue(ReferenceDataId)"/> in preference to this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the reference data value </param>
	  /// <param name="id">  the identifier to find </param>
	  /// <returns> the reference data value, null if not found </returns>
	  T queryValueOrNull<T>(ReferenceDataId<T> id);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this reference data with another.
	  /// <para>
	  /// The result combines both sets of reference data.
	  /// Values are taken from this set of reference data if available, otherwise they are taken
	  /// from the other set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other reference data </param>
	  /// <returns> the combined reference data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ReferenceData combinedWith(ReferenceData other)
	//  {
	//	return new CombinedReferenceData(this, other);
	//  }

	}

}
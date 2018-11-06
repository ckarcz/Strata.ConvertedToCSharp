using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{


	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Joiner = com.google.common.@base.Joiner;
	using Splitter = com.google.common.@base.Splitter;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An identifier for a holiday calendar.
	/// <para>
	/// This identifier is used to obtain a <seealso cref="HolidayCalendar"/> from <seealso cref="ReferenceData"/>.
	/// The holiday calendar itself is used to determine whether a day is a business day or not.
	/// </para>
	/// <para>
	/// Identifiers for common holiday calendars are provided in <seealso cref="HolidayCalendarIds"/>.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class HolidayCalendarId : ReferenceDataId<HolidayCalendar>, Resolvable<HolidayCalendar>, Named
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Instance cache. </summary>
	  private static readonly ConcurrentDictionary<string, HolidayCalendarId> CACHE = new ConcurrentDictionary<string, HolidayCalendarId>();

	  /// <summary>
	  /// The identifier, expressed as a normalized unique name.
	  /// </summary>
	  private readonly string name;
	  /// <summary>
	  /// The hash code.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  [NonSerialized]
	  private readonly int hashCode_Renamed;
	  /// <summary>
	  /// The resolver function.
	  /// Implementations of this function must only call <seealso cref="ReferenceData#queryValueOrNull(ReferenceDataId)"/>.
	  /// </summary>
	  [NonSerialized]
	  private readonly System.Func<HolidayCalendarId, ReferenceData, HolidayCalendar> resolver;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// <para>
	  /// The name uniquely identifies the calendar.
	  /// The <seealso cref="HolidayCalendar"/> is resolved from <seealso cref="ReferenceData"/> when required.
	  /// </para>
	  /// <para>
	  /// It is possible to combine two or more calendars using the '+' symbol.
	  /// For example, 'GBLO+USNY' will combine the separate 'GBLO' and 'USNY' calendars.
	  /// The resulting identifier will have the individual identifiers normalized into alphabetical order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static HolidayCalendarId of(String uniqueName)
	  public static HolidayCalendarId of(string uniqueName)
	  {
		HolidayCalendarId id = CACHE[uniqueName];
		return id != null ? id : create(uniqueName);
	  }

	  // create a new instance atomically, broken out to aid inlining
	  private static HolidayCalendarId create(string name)
	  {
		if (!name.Contains("+"))
		{
		  return CACHE.computeIfAbsent(name, n => new HolidayCalendarId(name));
		}
		// parse + separated names once and build resolver function to aid performance
		// name BBB+CCC+AAA changed to sorted form of AAA+BBB+CCC
		// dedicated resolver function created
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		IList<HolidayCalendarId> ids = Splitter.on('+').splitToList(name).Where(n => !n.Equals(HolidayCalendarIds.NO_HOLIDAYS.Name)).Select(n => HolidayCalendarId.of(n)).Distinct().OrderBy(comparing(HolidayCalendarId::getName)).ToList();
		string normalizedName = Joiner.on('+').join(ids);
		System.Func<HolidayCalendarId, ReferenceData, HolidayCalendar> resolver = (id, refData) =>
		{
	  HolidayCalendar cal = refData.queryValueOrNull(id);
	  if (cal != null)
	  {
		return cal;
	  }
	  cal = HolidayCalendars.NO_HOLIDAYS;
	  foreach (HolidayCalendarId splitId in ids)
	  {
		HolidayCalendar splitCal = refData.queryValueOrNull(splitId);
		if (splitCal == null)
		{
		  throw new ReferenceDataNotFoundException(Messages.format("Reference data not found for '{}' of type 'HolidayCalendarId' when finding '{}'", splitId, id));
		}
		cal = cal.combinedWith(splitCal);
	  }
	  return cal;
		};
		// cache under the normalized and non-normalized names
		HolidayCalendarId id = CACHE.computeIfAbsent(normalizedName, n => new HolidayCalendarId(normalizedName, resolver));
		CACHE.GetOrAdd(name, id);
		return id;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the default calendar for a currency.
	  /// <para>
	  /// This uses data from {@code HolidayCalendarDefaultData.ini} to provide a default.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to find the default for </param>
	  /// <returns> the holiday calendar </returns>
	  /// <exception cref="IllegalArgumentException"> if there is no default for the currency </exception>
	  public static HolidayCalendarId defaultByCurrency(Currency currency)
	  {
		return HolidayCalendarIniLookup.INSTANCE.defaultByCurrency(currency);
	  }

	  //-------------------------------------------------------------------------
	  // creates an identifier for a single calendar
	  private HolidayCalendarId(string normalizedName)
	  {
		this.name = normalizedName;
		this.hashCode_Renamed = normalizedName.GetHashCode();
		this.resolver = (id, refData) => refData.queryValueOrNull(this);
	  }

	  // creates an identifier for a combined calendar
	  private HolidayCalendarId(string normalizedName, System.Func<HolidayCalendarId, ReferenceData, HolidayCalendar> resolver)
	  {

		this.name = normalizedName;
		this.hashCode_Renamed = normalizedName.GetHashCode();
		this.resolver = resolver;
	  }

	  // resolve after deserialization
	  private object readResolve()
	  {
		return of(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this calendar.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String getName()
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  /// <summary>
	  /// Gets the type of data this identifier refers to.
	  /// <para>
	  /// A {@code HolidayCalendarId} refers to a {@code HolidayCalendar}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the reference data this identifier refers to </returns>
	  public Type<HolidayCalendar> ReferenceDataType
	  {
		  get
		  {
			return typeof(HolidayCalendar);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves this identifier to a holiday calendar using the specified reference data.
	  /// <para>
	  /// This returns an instance of <seealso cref="HolidayCalendar"/> that can perform calculations.
	  /// </para>
	  /// <para>
	  /// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	  /// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to resolve the reference </param>
	  /// <returns> the resolved holiday calendar </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if the identifier is not found </exception>
	  public HolidayCalendar resolve(ReferenceData refData)
	  {
		return refData.getValue(this);
	  }

	  public override HolidayCalendar queryValueOrNull(ReferenceData refData)
	  {
		return resolver.apply(this, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this holiday calendar identifier with another.
	  /// <para>
	  /// The resulting calendar will declare a day as a business day if it is a
	  /// business day in both source calendars.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other holiday calendar identifier </param>
	  /// <returns> the combined holiday calendar identifier </returns>
	  public HolidayCalendarId combinedWith(HolidayCalendarId other)
	  {
		if (this == other)
		{
		  return this;
		}
		if (this == HolidayCalendarIds.NO_HOLIDAYS)
		{
		  return ArgChecker.notNull(other, "other");
		}
		if (other == HolidayCalendarIds.NO_HOLIDAYS)
		{
		  return this;
		}
		return HolidayCalendarId.of(name + '+' + other.name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this identifier equals another identifier.
	  /// <para>
	  /// The comparison checks the name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other identifier, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		// could use (obj == this), but this code seems to be a little faster
		if (this == obj)
		{
		  return true;
		}
		if (obj == null || this.GetType() != obj.GetType())
		{
		  return false;
		}
		HolidayCalendarId that = (HolidayCalendarId) obj;
		return name.Equals(that.name);
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the identifier.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return hashCode_Renamed;
	  }

	  /// <summary>
	  /// Returns the name of the identifier.
	  /// </summary>
	  /// <returns> the name </returns>
	  public override string ToString()
	  {
		return name;
	  }

	}

}
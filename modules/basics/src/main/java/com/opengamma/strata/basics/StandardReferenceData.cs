using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendars = com.opengamma.strata.basics.date.HolidayCalendars;

	/// <summary>
	/// Provides standard reference data for holiday calendars in common currencies.
	/// </summary>
	internal sealed class StandardReferenceData
	{

	  /// <summary>
	  /// Standard reference data.
	  /// </summary>
	  internal static readonly ImmutableReferenceData STANDARD;
	  static StandardReferenceData()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> map = new java.util.HashMap<>();
		IDictionary<ReferenceDataId<object>, object> map = new Dictionary<ReferenceDataId<object>, object>();
		foreach (HolidayCalendar cal in HolidayCalendars.extendedEnum().lookupAllNormalized().values())
		{
		  map[cal.Id] = cal;
		}
		STANDARD = ImmutableReferenceData.of(map);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableMap.Builder<ReferenceDataId<?>, Object> builder = com.google.common.collect.ImmutableMap.builder();
		ImmutableMap.Builder<ReferenceDataId<object>, object> builder = ImmutableMap.builder();
		builder.put(HolidayCalendars.NO_HOLIDAYS.Id, HolidayCalendars.NO_HOLIDAYS);
		builder.put(HolidayCalendars.SAT_SUN.Id, HolidayCalendars.SAT_SUN);
		builder.put(HolidayCalendars.FRI_SAT.Id, HolidayCalendars.FRI_SAT);
		builder.put(HolidayCalendars.THU_FRI.Id, HolidayCalendars.THU_FRI);
		MINIMAL = ImmutableReferenceData.of(builder.build());
	  }
	  /// <summary>
	  /// Minimal reference data.
	  /// </summary>
	  internal static readonly ImmutableReferenceData MINIMAL;

	  // restricted constructor
	  private StandardReferenceData()
	  {
	  }

	}

}
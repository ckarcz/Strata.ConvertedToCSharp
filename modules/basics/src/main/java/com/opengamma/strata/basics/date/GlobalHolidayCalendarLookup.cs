using System;
using System.IO;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Provider that loads common global holiday calendars from binary form on the classpath.
	/// </summary>
	internal sealed class GlobalHolidayCalendarLookup : NamedLookup<HolidayCalendar>
	{

	  /// <summary>
	  /// The singleton instance of the lookup.
	  /// </summary>
	  public static readonly GlobalHolidayCalendarLookup INSTANCE = new GlobalHolidayCalendarLookup();

	  // lookup of conventions
	  internal static readonly ImmutableMap<string, HolidayCalendar> MAP;
	  static GlobalHolidayCalendarLookup()
	  {
		ImmutableMap.Builder<string, HolidayCalendar> builder = ImmutableMap.builder();
		ResourceLocator locator = ResourceLocator.ofClasspath("com/opengamma/strata/basics/date/GlobalHolidayCalendars.bin");
		try
		{
				using (Stream fis = locator.ByteSource.openStream())
				{
			  using (DataInputStream @in = new DataInputStream(fis))
			  {
				if (@in.readByte() != 'H' || @in.readByte() != 'C' || @in.readByte() != 'a' || @in.readByte() != 'l')
				{
				  Console.Error.WriteLine("ERROR: Corrupt holiday calendar data file");
				}
				else
				{
				  short calSize = @in.readShort();
				  for (int i = 0; i < calSize; i++)
				  {
					HolidayCalendar cal = ImmutableHolidayCalendar.readExternal(@in);
					builder.put(cal.Id.Name, cal);
				  }
				}
			  }
				}
		}
		catch (IOException ex)
		{
		  Console.Error.WriteLine("ERROR: Unable to parse holiday calendar data file: " + ex.Message);
		  Console.WriteLine(ex.ToString());
		  Console.Write(ex.StackTrace);
		}
		MAP = builder.build();
	  }

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private GlobalHolidayCalendarLookup()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableMap<string, HolidayCalendar> lookupAll()
	  {
		return MAP;
	  }

	}

}
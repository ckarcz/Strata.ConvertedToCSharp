using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using HolidayCalendars = com.opengamma.strata.basics.date.HolidayCalendars;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Standard roll convention implementations.
	/// <para>
	/// See <seealso cref="RollConventions"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class StandardRollConventions : RollConvention
	{

	  // no adjustment
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions NONE = new StandardRollConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  return com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
		  }
	  },

	  // last day of month
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions EOM = new StandardRollConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  return date.withDayOfMonth(date.lengthOfMonth());
		  }
		  public int getDayOfMonth()
		  {
			  return 31;
		  }
	  },

	  // 3rd Wednesday
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions IMM = new StandardRollConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  return date.with(java.time.temporal.TemporalAdjusters.dayOfWeekInMonth(3, java.time.DayOfWeek.WEDNESDAY));
		  }
	  },

	  // 2nd London banking day before 3rd Wednesday, adjusted by Montreal & Toronto holiday
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions IMMCAD = new StandardRollConventions()
	  {
		  private final com.opengamma.strata.basics.date.HolidayCalendar gblo = holidayCalendar(com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO);
		  private final com.opengamma.strata.basics.date.HolidayCalendar canada = holidayCalendar(com.opengamma.strata.basics.date.HolidayCalendarIds.CATO).combinedWith(holidayCalendar(com.opengamma.strata.basics.date.HolidayCalendarIds.CAMO));
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  java.time.LocalDate wed3 = date.with(java.time.temporal.TemporalAdjusters.dayOfWeekInMonth(3, java.time.DayOfWeek.WEDNESDAY));
			  return canada.previousOrSame(gblo.shift(wed3, -2));
		  }
	  },

	  // 1 Sydney business day before 2nd Friday
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions IMMAUD = new StandardRollConventions()
	  {
		  private final com.opengamma.strata.basics.date.HolidayCalendar ausy = holidayCalendar(com.opengamma.strata.basics.date.HolidayCalendarIds.AUSY);
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  return ausy.previous(date.with(java.time.temporal.TemporalAdjusters.dayOfWeekInMonth(2, java.time.DayOfWeek.FRIDAY)));
		  }
	  },

	  // Wednesday on or after 9th
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions IMMNZD = new StandardRollConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  return date.withDayOfMonth(9).with(java.time.temporal.TemporalAdjusters.nextOrSame(java.time.DayOfWeek.WEDNESDAY));
		  }
	  },

	  // 2nd Friday
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions SFE = new StandardRollConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  return date.with(java.time.temporal.TemporalAdjusters.dayOfWeekInMonth(2, java.time.DayOfWeek.FRIDAY));
		  }
	  },

	  // Each Monday, or Tuesday if USNY holiday
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions TBILL = new StandardRollConventions()
	  {
		  private final com.opengamma.strata.basics.date.HolidayCalendar usny = holidayCalendar(com.opengamma.strata.basics.date.HolidayCalendarIds.USNY);
		  public java.time.LocalDate adjust(java.time.LocalDate date)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
			  return usny.nextOrSame(date.with(java.time.temporal.TemporalAdjusters.nextOrSame(java.time.DayOfWeek.MONDAY)));
		  }
	  };

	  private static readonly IList<StandardRollConventions> valueList = new List<StandardRollConventions>();

	  static StandardRollConventions()
	  {
		  valueList.Add(NONE);
		  valueList.Add(EOM);
		  valueList.Add(IMM);
		  valueList.Add(IMMCAD);
		  valueList.Add(IMMAUD);
		  valueList.Add(IMMNZD);
		  valueList.Add(SFE);
		  valueList.Add(TBILL);
	  }

	  public enum InnerEnum
	  {
		  NONE,
		  EOM,
		  IMM,
		  IMMCAD,
		  IMMAUD,
		  IMMNZD,
		  SFE,
		  TBILL
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private StandardRollConventions(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // name
	  private readonly string name;

	  // create
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardRollConventions private = new StandardRollConventions()
	  {
		  this.name = name;
	  }
	  public String getName()
	  {
		  return name;
	  }
	  public String toString()
	  {
		  return name;
	  }
	  private static com.opengamma.strata.basics.date.HolidayCalendar holidayCalendar(com.opengamma.strata.basics.date.HolidayCalendarId id)
	  {
		  return com.opengamma.strata.basics.ReferenceData.standard().findValue(id).orElse(com.opengamma.strata.basics.date.HolidayCalendars.SAT_SUN);
	  }


		public static IList<StandardRollConventions> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static StandardRollConventions valueOf(string name)
		{
			foreach (StandardRollConventions enumInstance in StandardRollConventions.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}
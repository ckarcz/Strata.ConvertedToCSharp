using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Standard business day convention implementations.
	/// <para>
	/// See <seealso cref="BusinessDayConventions"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class StandardBusinessDayConventions : BusinessDayConvention
	{

	  // make no adjustment
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions NO_ADJUST = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  return com.opengamma.strata.collect.ArgChecker.notNull(date, "date");
		  }
	  },

	  // next business day
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions FOLLOWING = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  return calendar.nextOrSame(date);
		  }
	  },

	  // next business day unless over a month end
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions MODIFIED_FOLLOWING = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  return calendar.nextSameOrLastInMonth(date);
		  }
	  },

	  // next business day unless over a month end or mid
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions MODIFIED_FOLLOWING_BI_MONTHLY = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  java.time.LocalDate adjusted = calendar.nextOrSame(date);
			  if (adjusted.getMonthValue() != date.getMonthValue() || (adjusted.getDayOfMonth() > 15 && date.getDayOfMonth() <= 15))
			  {
				  adjusted = calendar.previous(date);
			  }
			  return adjusted;
		  }
	  },

	  // previous business day
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions PRECEDING = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  return calendar.previousOrSame(date);
		  }
	  },

	  // previous business day unless over a month end
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions MODIFIED_PRECEDING = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  java.time.LocalDate adjusted = calendar.previousOrSame(date);
			  if (adjusted.getMonth() != date.getMonth())
			  {
				  adjusted = calendar.next(date);
			  }
			  return adjusted;
		  }
	  },

	  // next business day if Sun/Mon, otherwise previous
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions NEAREST = new StandardBusinessDayConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate date, HolidayCalendar calendar)
		  {
			  if (calendar.isBusinessDay(date))
			  {
				  return date;
			  }
			  if (date.getDayOfWeek() == SUNDAY || date.getDayOfWeek() == MONDAY)
			  {
				  return calendar.next(date);
			  }
			  else
			  {
				  return calendar.previous(date);
			  }
		  }
	  };

	  private static readonly IList<StandardBusinessDayConventions> valueList = new List<StandardBusinessDayConventions>();

	  static StandardBusinessDayConventions()
	  {
		  valueList.Add(NO_ADJUST);
		  valueList.Add(FOLLOWING);
		  valueList.Add(MODIFIED_FOLLOWING);
		  valueList.Add(MODIFIED_FOLLOWING_BI_MONTHLY);
		  valueList.Add(PRECEDING);
		  valueList.Add(MODIFIED_PRECEDING);
		  valueList.Add(NEAREST);
	  }

	  public enum InnerEnum
	  {
		  NO_ADJUST,
		  FOLLOWING,
		  MODIFIED_FOLLOWING,
		  MODIFIED_FOLLOWING_BI_MONTHLY,
		  PRECEDING,
		  MODIFIED_PRECEDING,
		  NEAREST
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private StandardBusinessDayConventions(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // name
	  private readonly string name;

	  // create
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardBusinessDayConventions private = new StandardBusinessDayConventions()
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


		public static IList<StandardBusinessDayConventions> values()
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

		public static StandardBusinessDayConventions valueOf(string name)
		{
			foreach (StandardBusinessDayConventions enumInstance in StandardBusinessDayConventions.valueList)
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
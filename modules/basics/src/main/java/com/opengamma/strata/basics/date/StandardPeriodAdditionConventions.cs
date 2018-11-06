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
	/// Standard period addition implementations.
	/// <para>
	/// See <seealso cref="PeriodAdditionConventions"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class StandardPeriodAdditionConventions : PeriodAdditionConvention
	{

	  // no specific addition rule
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardPeriodAdditionConventions NONE = new StandardPeriodAdditionConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate baseDate, java.time.Period period, HolidayCalendar calendar)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(baseDate, "baseDate");
			  com.opengamma.strata.collect.ArgChecker.notNull(period, "period");
			  com.opengamma.strata.collect.ArgChecker.notNull(calendar, "calendar");
			  return baseDate.plus(period);
		  }
		  public boolean isMonthBased()
		  {
			  return false;
		  }
	  },

	  // last day of month
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardPeriodAdditionConventions LAST_DAY = new StandardPeriodAdditionConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate baseDate, java.time.Period period, HolidayCalendar calendar)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(baseDate, "baseDate");
			  com.opengamma.strata.collect.ArgChecker.notNull(period, "period");
			  com.opengamma.strata.collect.ArgChecker.notNull(calendar, "calendar");
			  java.time.LocalDate endDate = baseDate.plus(period);
			  if (baseDate.getDayOfMonth() == baseDate.lengthOfMonth())
			  {
				  return endDate.withDayOfMonth(endDate.lengthOfMonth());
			  }
			  return endDate;
		  }
		  public boolean isMonthBased()
		  {
			  return true;
		  }
	  },

	  // last business day of month
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardPeriodAdditionConventions LAST_BUSINESS_DAY = new StandardPeriodAdditionConventions()
	  {
		  public java.time.LocalDate adjust(java.time.LocalDate baseDate, java.time.Period period, HolidayCalendar calendar)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNull(baseDate, "baseDate");
			  com.opengamma.strata.collect.ArgChecker.notNull(period, "period");
			  com.opengamma.strata.collect.ArgChecker.notNull(calendar, "calendar");
			  java.time.LocalDate endDate = baseDate.plus(period);
			  if (calendar.isLastBusinessDayOfMonth(baseDate))
			  {
				  return calendar.lastBusinessDayOfMonth(endDate);
			  }
			  return endDate;
		  }
		  public boolean isMonthBased()
		  {
			  return true;
		  }
	  };

	  private static readonly IList<StandardPeriodAdditionConventions> valueList = new List<StandardPeriodAdditionConventions>();

	  static StandardPeriodAdditionConventions()
	  {
		  valueList.Add(NONE);
		  valueList.Add(LAST_DAY);
		  valueList.Add(LAST_BUSINESS_DAY);
	  }

	  public enum InnerEnum
	  {
		  NONE,
		  LAST_DAY,
		  LAST_BUSINESS_DAY
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private StandardPeriodAdditionConventions(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // name
	  private readonly string name;

	  // create
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardPeriodAdditionConventions private = new StandardPeriodAdditionConventions()
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


		public static IList<StandardPeriodAdditionConventions> values()
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

		public static StandardPeriodAdditionConventions valueOf(string name)
		{
			foreach (StandardPeriodAdditionConventions enumInstance in StandardPeriodAdditionConventions.valueList)
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
using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using NamedLookup = com.opengamma.strata.collect.named.NamedLookup;

	/// <summary>
	/// Standard roll convention implementations.
	/// <para>
	/// See <seealso cref="RollConventions"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class DayRollConventions : NamedLookup<RollConvention>
	{

	  // lookup of conventions
	  internal static readonly ImmutableMap<string, RollConvention> MAP;
	  static DayRollConventions()
	  {
		ImmutableMap.Builder<string, RollConvention> mapBuilder = ImmutableMap.builder();
		foreach (RollConvention roll in Dom.CONVENTIONS)
		{
		  mapBuilder.put(roll.Name, roll);
		  mapBuilder.put(roll.Name.ToUpper(Locale.ENGLISH), roll);
		}
		foreach (RollConvention roll in Dow.CONVENTIONS)
		{
		  mapBuilder.put(roll.Name, roll);
		  mapBuilder.put(roll.Name.ToUpper(Locale.ENGLISH), roll);
		}
		MAP = mapBuilder.build();
		  for (int i = 0; i < 30; i++)
		  {
			CONVENTIONS[i] = new Dom(i + 1);
		  }
		  for (int i = 0; i < 7; i++)
		  {
			DayOfWeek dow = DayOfWeek.of(i + 1);
			string name = NAMES.substring(i * 6, ((i + 1) * 6) - (i * 6));
			CONVENTIONS[i] = new Dow(dow, name);
		  }
	  }

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DayRollConventions()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableMap<string, RollConvention> lookupAll()
	  {
		return MAP;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Implementation of the day-of-month roll convention.
	  /// </summary>
	  [Serializable]
	  internal sealed class Dom : RollConvention
	  {
		// singleton, so no equals/hashCode

		// Serialization version
		internal const long serialVersionUID = 1L;
		// cache of conventions
		internal static readonly RollConvention[] CONVENTIONS = new RollConvention[30];

		// day-of-month
		internal readonly int day;
		// unique name
		internal readonly string name;

		// obtains instance
		internal static RollConvention of(int day)
		{
		  if (day == 31)
		  {
			return RollConventions.EOM;
		  }
		  else if (day < 1 || day > 30)
		  {
			throw new System.ArgumentException("Invalid day-of-month: " + day);
		  }
		  return CONVENTIONS[day - 1];
		}

		// create
		internal Dom(int day)
		{
		  this.day = day;
		  this.name = "Day" + day;
		}

		internal object readResolve()
		{
		  return Dom.of(day);
		}

		public override int DayOfMonth
		{
			get
			{
			  return day;
			}
		}

		public LocalDate adjust(LocalDate date)
		{
		  ArgChecker.notNull(date, "date");
		  if (day >= 29 && date.MonthValue == 2)
		  {
			return date.withDayOfMonth(date.lengthOfMonth());
		  }
		  return date.withDayOfMonth(day);
		}

		public override bool matches(LocalDate date)
		{
		  ArgChecker.notNull(date, "date");
		  return date.DayOfMonth == day || (date.MonthValue == 2 && day >= date.lengthOfMonth() && date.DayOfMonth == date.lengthOfMonth());
		}

		public string Name
		{
			get
			{
			  return name;
			}
		}

		public override string ToString()
		{
		  return name;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Implementation of the day-of-week roll convention.
	  /// </summary>
	  [Serializable]
	  internal sealed class Dow : RollConvention
	  {
		// singleton, so no equals/hashCode

		// Serialization version
		internal const long serialVersionUID = 1L;
		// convention names
		internal const string NAMES = "DayMonDayTueDayWedDayThuDayFriDaySatDaySun";
		// cache of conventions
		internal static readonly RollConvention[] CONVENTIONS = new RollConvention[7];

		// day-of-week
		internal readonly DayOfWeek day;
		// unique name
		internal readonly string name;

		// obtains instance
		internal static RollConvention of(DayOfWeek dayOfWeek)
		{
		  ArgChecker.notNull(dayOfWeek, "dayOfWeek");
		  return CONVENTIONS[dayOfWeek.Value - 1];
		}

		internal object readResolve()
		{
		  return Dow.of(day);
		}

		// create
		internal Dow(DayOfWeek dayOfWeek, string name)
		{
		  this.day = dayOfWeek;
		  this.name = name;
		}

		public LocalDate adjust(LocalDate date)
		{
		  ArgChecker.notNull(date, "date");
		  return date.with(TemporalAdjusters.nextOrSame(day));
		}

		public override bool matches(LocalDate date)
		{
		  ArgChecker.notNull(date, "date");
		  return date.DayOfWeek == day;
		}

		public override LocalDate next(LocalDate date, Frequency periodicFrequency)
		{
		  ArgChecker.notNull(date, "date");
		  ArgChecker.notNull(periodicFrequency, "periodicFrequency");
		  LocalDate calculated = date.plus(periodicFrequency);
		  return calculated.with(TemporalAdjusters.nextOrSame(day));
		}

		public override LocalDate previous(LocalDate date, Frequency periodicFrequency)
		{
		  ArgChecker.notNull(date, "date");
		  ArgChecker.notNull(periodicFrequency, "periodicFrequency");
		  LocalDate calculated = date.minus(periodicFrequency);
		  return calculated.with(TemporalAdjusters.previousOrSame(day));
		}

		public string Name
		{
			get
			{
			  return name;
			}
		}

		public override string ToString()
		{
		  return name;
		}
	  }

	}

}
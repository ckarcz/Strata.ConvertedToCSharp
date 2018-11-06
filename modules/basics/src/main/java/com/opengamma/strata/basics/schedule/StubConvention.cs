using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// A convention defining how to calculate stub periods.
	/// <para>
	/// A <seealso cref="PeriodicSchedule periodic schedule"/> is determined using a periodic frequency.
	/// This splits the schedule into "regular" periods of a fixed length, such as every 3 months.
	/// Any remaining days are allocated to irregular "stubs" at the start and/or end.
	/// </para>
	/// <para>
	/// The stub convention is provided as a simple declarative mechanism to define stubs.
	/// The convention handles the case of no stubs, or a single stub at the start or end.
	/// If there is a stub at both the start and end, then explicit stub dates must be used.
	/// </para>
	/// <para>
	/// For example, dividing a 24 month (2 year) swap into 3 month periods is easy as it splits exactly.
	/// However, a 23 month swap cannot be split into even 3 month periods.
	/// Instead, there will be a 2 month "initial" stub at the start, a 2 month "final" stub at the end
	/// or both an initial and final stub with a combined length of 2 months.
	/// </para>
	/// <para>
	/// The 'ShortInitial', 'LongInitial' or 'SmartInitial' convention causes the regular periods to be determined
	/// <i>backwards</i> from the end date of the schedule, with remaining days allocated to the stub.
	/// </para>
	/// <para>
	/// The 'ShortFinal', 'LongFinal' or 'SmartFinal' convention causes the regular periods to be determined
	/// <i>forwards</i> from the start date of the schedule, with remaining days allocated to the stub.
	/// </para>
	/// <para>
	/// The 'None' convention may be used to explicitly indicate there are no stubs.
	/// </para>
	/// <para>
	/// The 'Both' convention may be used to explicitly indicate there is both an initial and final stub.
	/// In this case, dates must be used to identify the stubs.
	/// </para>
	/// </summary>
	public abstract class StubConvention : NamedEnum
	{

	  /// <summary>
	  /// Explicitly states that there are no stubs.
	  /// <para>
	  /// This is used to indicate that the term of the schedule evenly divides by the
	  /// periodic frequency leaving no stubs.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// If the term of the schedule is less than the frequency, then only one period exists.
	  /// In this case, the period is not treated as a stub.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit stubs.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention NONE = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitInitialStub || explicitFinalStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit stub, but stub convention is 'None'");
			  }
			  return NONE;
		  }
	  },
	  /// <summary>
	  /// A short initial stub.
	  /// <para>
	  /// The schedule periods will be determined backwards from the regular period end date.
	  /// Any remaining period, shorter than the standard frequency, will be allocated at the start.
	  /// </para>
	  /// <para>
	  /// For example, an 8 month trade with a 3 month periodic frequency would result in
	  /// a 2 month initial short stub followed by two periods of 3 months.
	  /// </para>
	  /// <para>
	  /// If there is no remaining period when calculating, then there is no stub.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit final stub.
	  /// If there is an explicit initial stub, then this convention is considered to be matched
	  /// and the remaining period is calculated using the stub convention 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention SHORT_INITIAL = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitFinalStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit final stub, but stub convention is 'ShortInitial'");
			  }
			  return (explicitInitialStub ? NONE : SHORT_INITIAL);
		  }
	  },
	  /// <summary>
	  /// A long initial stub.
	  /// <para>
	  /// The schedule periods will be determined backwards from the regular period end date.
	  /// Any remaining period, shorter than the standard frequency, will be allocated at the start
	  /// and combined with the next period, making a total period longer than the standard frequency.
	  /// </para>
	  /// <para>
	  /// For example, an 8 month trade with a 3 month periodic frequency would result in
	  /// a 5 month initial long stub followed by one period of 3 months.
	  /// </para>
	  /// <para>
	  /// If there is no remaining period when calculating, then there is no stub.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit final stub.
	  /// If there is an explicit initial stub, then this convention is considered to be matched
	  /// and the remaining period is calculated using the stub convention 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention LONG_INITIAL = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitFinalStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit final stub, but stub convention is 'LongInitial'");
			  }
			  return (explicitInitialStub ? NONE : LONG_INITIAL);
		  }
		  boolean isStubLong(java.time.LocalDate date1, java.time.LocalDate date2)
		  {
			  return true;
		  }
	  },
	  /// <summary>
	  /// A smart initial stub.
	  /// <para>
	  /// The schedule periods will be determined backwards from the regular period end date.
	  /// Any remaining period, shorter than the standard frequency, will be allocated at the start.
	  /// If this results in a stub of less than 7 days, the stub will be combined with the next period.
	  /// If this results in a stub of 7 days or more, the stub will be retained.
	  /// This is the equivalent of <seealso cref="#LONG_INITIAL"/> up to 7 days and <seealso cref="#SHORT_INITIAL"/> beyond that.
	  /// The 7 days are calculated based on unadjusted dates.
	  /// This convention appears to match that used by Bloomberg.
	  /// </para>
	  /// <para>
	  /// If there is no remaining period when calculating, then there is no stub.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit final stub.
	  /// If there is an explicit initial stub, then this convention is considered to be matched
	  /// and the remaining period is calculated using the stub convention 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention SMART_INITIAL = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitFinalStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit final stub, but stub convention is 'SmartInitial'");
			  }
			  return (explicitInitialStub ? NONE : SMART_INITIAL);
		  }
		  boolean isStubLong(java.time.LocalDate date1, java.time.LocalDate date2)
		  {
			  return date1.plusDays(7).isAfter(date2);
		  }
	  },
	  /// <summary>
	  /// A short final stub.
	  /// <para>
	  /// The schedule periods will be determined forwards from the regular period start date.
	  /// Any remaining period, shorter than the standard frequency, will be allocated at the end.
	  /// </para>
	  /// <para>
	  /// For example, an 8 month trade with a 3 month periodic frequency would result in
	  /// two periods of 3 months followed by a 2 month final short stub.
	  /// </para>
	  /// <para>
	  /// If there is no remaining period when calculating, then there is no stub.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit initial stub.
	  /// If there is an explicit final stub, then this convention is considered to be matched
	  /// and the remaining period is calculated using the stub convention 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention SHORT_FINAL = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitInitialStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit initial stub, but stub convention is 'ShortFinal'");
			  }
			  return (explicitFinalStub ? NONE : SHORT_FINAL);
		  }
	  },
	  /// <summary>
	  /// A long final stub.
	  /// <para>
	  /// The schedule periods will be determined forwards from the regular period start date.
	  /// Any remaining period, shorter than the standard frequency, will be allocated at the end
	  /// and combined with the previous period, making a total period longer than the standard frequency.
	  /// </para>
	  /// <para>
	  /// For example, an 8 month trade with a 3 month periodic frequency would result in
	  /// one period of 3 months followed by a 5 month final long stub.
	  /// </para>
	  /// <para>
	  /// If there is no remaining period when calculating, then there is no stub.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit initial stub.
	  /// If there is an explicit final stub, then this convention is considered to be matched
	  /// and the remaining period is calculated using the stub convention 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention LONG_FINAL = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitInitialStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit initial stub, but stub convention is 'LongFinal'");
			  }
			  return (explicitFinalStub ? NONE : LONG_FINAL);
		  }
		  boolean isStubLong(java.time.LocalDate date1, java.time.LocalDate date2)
		  {
			  return true;
		  }
	  },
	  /// <summary>
	  /// A smart final stub.
	  /// <para>
	  /// The schedule periods will be determined forwards from the regular period start date.
	  /// Any remaining period, shorter than the standard frequency, will be allocated at the end.
	  /// If this results in a stub of less than 7 days, the stub will be combined with the next period.
	  /// If this results in a stub of 7 days or more, the stub will be retained.
	  /// This is the equivalent of <seealso cref="#LONG_FINAL"/> up to 7 days and <seealso cref="#SHORT_FINAL"/> beyond that.
	  /// The 7 days are calculated based on unadjusted dates.
	  /// This convention appears to match that used by Bloomberg.
	  /// </para>
	  /// <para>
	  /// If there is no remaining period when calculating, then there is no stub.
	  /// For example, a 6 month trade can be exactly divided by a 3 month frequency.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be no explicit initial stub.
	  /// If there is an explicit final stub, then this convention is considered to be matched
	  /// and the remaining period is calculated using the stub convention 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention SMART_FINAL = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if (explicitInitialStub)
			  {
				  throw new ScheduleException(definition, "Dates specify an explicit initial stub, but stub convention is 'SmartFinal'");
			  }
			  return (explicitFinalStub ? NONE : SMART_FINAL);
		  }
		  boolean isStubLong(java.time.LocalDate date1, java.time.LocalDate date2)
		  {
			  return date1.plusDays(7).isAfter(date2);
		  }
	  },
	  /// <summary>
	  /// Both ends of the schedule have a stub.
	  /// <para>
	  /// The schedule periods will be determined from two dates - the regular period start date
	  /// and the regular period end date.
	  /// Days before the first regular period start date form the initial stub.
	  /// Days after the last regular period end date form the final stub.
	  /// </para>
	  /// <para>
	  /// When creating a schedule, there must be both an explicit initial and final stub.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StubConvention BOTH = new StubConvention()
	  {
		  StubConvention toImplicit(PeriodicSchedule definition, boolean explicitInitialStub, boolean explicitFinalStub)
		  {
			  if ((explicitInitialStub && explicitFinalStub) == false)
			  {
				  throw new ScheduleException(definition, "Stub convention is 'Both' but explicit dates not specified");
			  }
			  return NONE;
		  }
	  };

	  private static readonly IList<StubConvention> valueList = new List<StubConvention>();

	  static StubConvention()
	  {
		  valueList.Add(NONE);
		  valueList.Add(SHORT_INITIAL);
		  valueList.Add(LONG_INITIAL);
		  valueList.Add(SMART_INITIAL);
		  valueList.Add(SHORT_FINAL);
		  valueList.Add(LONG_FINAL);
		  valueList.Add(SMART_FINAL);
		  valueList.Add(BOTH);
	  }

	  public enum InnerEnum
	  {
		  NONE,
		  SHORT_INITIAL,
		  LONG_INITIAL,
		  SMART_INITIAL,
		  SHORT_FINAL,
		  LONG_FINAL,
		  SMART_FINAL,
		  BOTH
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private StubConvention(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<StubConvention> NAMES = com.opengamma.strata.collect.named.EnumNames.of(StubConvention.class);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Parsing handles the mixed case form produced by <seealso cref="#toString()"/> and
	  /// the upper and lower case variants of the enum constant name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name to parse </param>
	  /// <returns> the type </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static StubConvention of(String name)
	  public static StubConvention of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this stub convention to the appropriate roll convention.
	  /// <para>
	  /// This converts a stub convention to a <seealso cref="RollConvention"/> based on the
	  /// start date, end date, frequency and preference for end-of-month.
	  /// The net result is to imply the roll convention from the schedule data.
	  /// </para>
	  /// <para>
	  /// The rules are as follows:
	  /// </para>
	  /// <para>
	  /// If the input frequency is month-based, then the implied convention is based on
	  /// the day-of-month of the initial date, where the initial date is the start date
	  /// if rolling forwards or the end date otherwise.
	  /// If that date is on the 31st day, or if the 'preferEndOfMonth' flag is true and
	  /// the relevant date is at the end of the month, then the implied convention is 'EOM'.
	  /// For example, if the initial date of the sequence is 2014-06-20 and the periodic
	  /// frequency is 'P3M' (month-based), then the implied convention is 'Day20'.
	  /// </para>
	  /// <para>
	  /// If the input frequency is week-based, then the implied convention is based on
	  /// the day-of-week of the initial date, where the initial date is the start date
	  /// if rolling forwards or the end date otherwise.
	  /// For example, if the initial date of the sequence is 2014-06-20 and the periodic
	  /// frequency is 'P2W' (week-based), then the implied convention is 'DayFri',
	  /// because 2014-06-20 is a Friday.
	  /// </para>
	  /// <para>
	  /// In all other cases, the implied convention is 'None'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="start">  the start date of the schedule </param>
	  /// <param name="end">  the end date of the schedule </param>
	  /// <param name="frequency">  the periodic frequency of the schedule </param>
	  /// <param name="preferEndOfMonth">  whether to prefer the end-of-month when rolling </param>
	  /// <returns> the derived roll convention </returns>
	  public RollConvention toRollConvention(java.time.LocalDate start, java.time.LocalDate end, Frequency frequency, bool preferEndOfMonth)
	  {

		ArgChecker.notNull(start, "start");
		ArgChecker.notNull(end, "end");
		ArgChecker.notNull(frequency, "frequency");
		// if the day-of-month differs, need to handle case where one or both
		// dates are at the end of the month, and in different months
		if (this == NONE && frequency.MonthBased)
		{
		  if (start.DayOfMonth != end.DayOfMonth && start.getLong(PROLEPTIC_MONTH) != end.getLong(PROLEPTIC_MONTH) && (start.DayOfMonth == start.lengthOfMonth() || end.DayOfMonth == end.lengthOfMonth()))
		  {

			return preferEndOfMonth ? RollConventions.EOM : RollConvention.ofDayOfMonth(Math.Max(start.DayOfMonth, end.DayOfMonth));
		  }
		}
		if (CalculateBackwards)
		{
		  return impliedRollConvention(end, start, frequency, preferEndOfMonth);
		}
		else
		{
		  return impliedRollConvention(start, end, frequency, preferEndOfMonth);
		}
	  }

	  // helper for converting to roll convention
	  private static RollConvention impliedRollConvention(java.time.LocalDate date, java.time.LocalDate otherDate, Frequency frequency, bool preferEndOfMonth)
	  {

		if (frequency.MonthBased)
		{
		  if (preferEndOfMonth && date.DayOfMonth == date.lengthOfMonth())
		  {
			return RollConventions.EOM;
		  }
		  return RollConvention.ofDayOfMonth(date.DayOfMonth);
		}
		else if (frequency.WeekBased)
		{
		  return RollConvention.ofDayOfWeek(date.DayOfWeek);
		}
		else
		{
		  // neither monthly nor weekly means no known roll convention
		  return RollConventions.NONE;
		}
	  }

	  /// <summary>
	  /// Converts this stub convention to one that creates implicit stubs, validating that
	  /// any explicit stubs are correct.
	  /// <para>
	  /// Stubs can be specified in two ways, using dates or using this convention.
	  /// This method is passed flags indicating whether explicit stubs have been specified using dates.
	  /// It validated that such stubs are compatible, and returns a convention suitable for
	  /// creating stubs implicitly during rolling.
	  /// </para>
	  /// <para>
	  /// For example, an invalid stub convention would be to specify two stubs using explicit dates but
	  /// declaring the convention as 'ShortFinal'.
	  /// </para>
	  /// <para>
	  /// The result is the implicit stub convention to apply between the two calculation dates.
	  /// For example, if an initial stub is defined by dates then it cannot also be created automatically,
	  /// thus the implicit stub convention is 'None'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="definition">  the schedule definition, for error messages </param>
	  /// <param name="explicitInitialStub">  an initial stub has been explicitly defined by dates </param>
	  /// <param name="explicitFinalStub">  a final stub has been explicitly defined by dates </param>
	  /// <returns> the effective stub convention </returns>
	  /// <exception cref="ScheduleException"> if the input data is invalid </exception>
	  internal abstract StubConvention toImplicit(PeriodicSchedule definition, bool explicitInitialStub, bool explicitFinalStub);

	  /// <summary>
	  /// Decides if the period between two dates implies a long stub.
	  /// <para>
	  /// This is used by the smart conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date1">  the first date </param>
	  /// <param name="date2">  the second date </param>
	  /// <returns> true if a long stub should be created by deleting one of the two input dates </returns>
	  internal bool isStubLong(java.time.LocalDate date1, java.time.LocalDate date2)
	  {
		return false;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the schedule is calculated forwards from the start date to the end date.
	  /// <para>
	  /// If true, then there will typically be a stub at the end of the schedule.
	  /// </para>
	  /// <para>
	  /// The 'None', 'ShortFinal', 'LongFinal' and 'SmartFinal' conventions return true.
	  /// Other conventions return false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if calculation occurs forwards from the start date to the end date </returns>
	  public bool CalculateForwards
	  {
		  get
		  {
			return this == SHORT_FINAL || this == LONG_FINAL || this == SMART_FINAL || this == NONE;
		  }
	  }

	  /// <summary>
	  /// Checks if the schedule is calculated backwards from the end date to the start date.
	  /// <para>
	  /// If true, then there will typically be a stub at the start of the schedule.
	  /// </para>
	  /// <para>
	  /// The 'ShortInitial', 'LongInitial' and 'SmartInitial' conventions return true.
	  /// Other conventions return false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if calculation occurs backwards from the end date to the start date </returns>
	  public bool CalculateBackwards
	  {
		  get
		  {
			return this == SHORT_INITIAL || this == LONG_INITIAL || this == SMART_INITIAL;
		  }
	  }

	  /// <summary>
	  /// Checks if this convention tries to produce a long stub.
	  /// <para>
	  /// The 'LongInitial' and 'LongFinal' conventions return true.
	  /// Other conventions return false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if there may be a long stub </returns>
	  public bool Long
	  {
		  get
		  {
			return this == LONG_INITIAL || this == LONG_FINAL;
		  }
	  }

	  /// <summary>
	  /// Checks if this convention tries to produce a short stub.
	  /// <para>
	  /// The 'ShortInitial' and 'ShortFinal' conventions return true.
	  /// Other conventions return false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if there may be a short stub </returns>
	  public bool Short
	  {
		  get
		  {
			return this == SHORT_INITIAL || this == SHORT_FINAL;
		  }
	  }

	  /// <summary>
	  /// Checks if this convention uses smart rules to create a stub.
	  /// <para>
	  /// The 'SmartInitial' and 'SmartFinal' conventions return true.
	  /// Other conventions return false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if there may be a long stub </returns>
	  public bool Smart
	  {
		  get
		  {
			return this == SMART_INITIAL || this == SMART_FINAL;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted name of the type.
	  /// </summary>
	  /// <returns> the formatted string representing the type </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return NAMES.format(this);
	  }


		public static IList<StubConvention> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static StubConvention valueOf(string name)
		{
			foreach (StubConvention enumInstance in StubConvention.valueList)
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
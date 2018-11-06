using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.schedule
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using MoreObjects = com.google.common.@base.MoreObjects;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Definition of a periodic schedule.
	/// <para>
	/// A periodic schedule is determined using a "periodic frequency".
	/// This splits the schedule into "regular" periods of a fixed length, such as every 3 months.
	/// Any remaining days are allocated to irregular "stubs" at the start and/or end.
	/// </para>
	/// <para>
	/// For example, a 24 month (2 year) swap might be divided into 3 month periods.
	/// The 24 month period is the overall schedule and the 3 month period is the periodic frequency.
	/// </para>
	/// <para>
	/// Note that a 23 month swap cannot be split into even 3 month periods.
	/// Instead, there will be a 2 month "initial" stub at the start, a 2 month "final" stub at the end
	/// or both an initial and final stub with a combined length of 2 months.
	/// 
	/// <h4>Example</h4>
	/// </para>
	/// <para>
	/// This example creates a schedule for a 13 month swap cannot be split into 3 month periods
	/// with a long initial stub rolling at end-of-month:
	/// <pre>
	///  // example swap using builder
	///  BusinessDayAdjustment businessDayAdj =
	///    BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, GlobalHolidayCalendars.EUTA);
	///  PeriodicSchedule definition = PeriodicSchedule.builder()
	///      .startDate(LocalDate.of(2014, 2, 12))
	///      .endDate(LocalDate.of(2015, 3, 31))
	///      .businessDayAdjustment(businessDayAdj)
	///      .frequency(Frequency.P3M)
	///      .stubConvention(StubConvention.LONG_INITIAL)
	///      .rollConvention(RollConventions.EOM)
	///      .build();
	///  Schedule schedule = definition.createSchedule();
	/// 
	///  // result
	///  period 1: 2014-02-12 to 2014-06-30
	///  period 2: 2014-06-30 to 2014-09-30
	///  period 3: 2014-09-30 to 2014-12-31
	///  period 4: 2014-12-31 to 2015-03-31
	/// </pre>
	/// 
	/// <h4>Details about stubs and date rolling</h4>
	/// </para>
	/// <para>
	/// The stubs are specified using a combination of the <seealso cref="StubConvention"/>, <seealso cref="RollConvention"/> and dates.
	/// </para>
	/// <para>
	/// The explicit stub dates are checked first. An explicit stub occurs if 'firstRegularStartDate' or
	/// 'lastRegularEndDate' is present and they differ from 'startDate' and 'endDate'.
	/// </para>
	/// <para>
	/// If explicit stub dates are specified then they are used to lock the initial or final stub.
	/// If the stub convention is present, it is matched and validated against the locked stub.
	/// For example, if an initial stub is specified by dates and the stub convention is 'ShortInitial',
	/// 'LongInitial' or 'SmartInitial' then the convention is considered to be matched, thus the periodic
	/// frequency is applied using the implicit stub convention 'None'.
	/// If the stub convention does not match the dates, then an exception will be thrown during schedule creation.
	/// If the stub convention is not present, then the periodic frequency is applied
	/// using the implicit stub convention 'None'.
	/// </para>
	/// <para>
	/// If explicit stub dates are not specified then the stub convention is used.
	/// The convention selects whether to use the start date or the end date as the beginning of the schedule calculation.
	/// The beginning of the calculation must match the roll convention, unless the convention is 'EOM',
	/// in which case 'EOM' is only applied if the calculation starts at the end of the month.
	/// </para>
	/// <para>
	/// In all cases, the roll convention is used to fine-tune the dates.
	/// If not present or 'None', the convention is effectively implied from the first date of the calculation.
	/// All calculated dates will match the roll convention.
	/// If this is not possible due to the dates specified then an exception will be thrown during schedule creation.
	/// </para>
	/// <para>
	/// It is permitted to have 'firstRegularStartDate' equal to 'endDate', or 'lastRegularEndDate' equal to 'startDate'.
	/// In both cases, the effect is to define a schedule that is entirely "stub" and has no regular periods.
	/// The resulting schedule will retain the frequency specified here, even though it is not used.
	/// </para>
	/// <para>
	/// The schedule operates primarily on "unadjusted" dates.
	/// An unadjusted date can be any day, including non-business days.
	/// When the unadjusted schedule has been determined, the appropriate business day adjustment
	/// is applied to create a parallel schedule of "adjusted" dates.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class PeriodicSchedule implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class PeriodicSchedule : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
		private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date, which is the end of the last schedule period.
	  /// <para>
	  /// This is the end date of the schedule.
	  /// It is is unadjusted and as such might be a weekend or holiday.
	  /// Any applicable business day adjustment will be applied when creating the schedule.
	  /// This is also known as the unadjusted maturity date or unadjusted termination date.
	  /// This date must be after the start date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The regular periodic frequency to use.
	  /// <para>
	  /// Most dates are calculated using a regular periodic frequency, such as every 3 months.
	  /// The actual day-of-month or day-of-week is selected using the roll and stub conventions.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Frequency frequency;
	  private readonly Frequency frequency;
	  /// <summary>
	  /// The business day adjustment to apply.
	  /// <para>
	  /// Each date in the calculated schedule is determined without taking into account weekends and holidays.
	  /// The adjustment specified here is used to convert those dates to valid business days.
	  /// </para>
	  /// <para>
	  /// The start date and end date may have their own business day adjustment rules.
	  /// If those are not present, then this adjustment is used instead.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The optional business day adjustment to apply to the start date.
	  /// <para>
	  /// The start date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the start date to a valid business day.
	  /// </para>
	  /// <para>
	  /// If this property is not present, the standard {@code businessDayAdjustment} property is used instead.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.BusinessDayAdjustment startDateBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment startDateBusinessDayAdjustment;
	  /// <summary>
	  /// The optional business day adjustment to apply to the end date.
	  /// <para>
	  /// The end date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the end date to a valid business day.
	  /// </para>
	  /// <para>
	  /// If this property is not present, the standard {@code businessDayAdjustment} property is used instead.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.BusinessDayAdjustment endDateBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment endDateBusinessDayAdjustment;
	  /// <summary>
	  /// The optional convention defining how to handle stubs.
	  /// <para>
	  /// The stub convention is used during schedule construction to determine whether the irregular
	  /// remaining period occurs at the start or end of the schedule.
	  /// It also determines whether the irregular period is shorter or longer than the regular period.
	  /// This property interacts with the "explicit dates" of <seealso cref="PeriodicSchedule#getFirstRegularStartDate()"/>
	  /// and <seealso cref="PeriodicSchedule#getLastRegularEndDate()"/>.
	  /// </para>
	  /// <para>
	  /// The convention 'None' may be used to explicitly indicate there are no stubs.
	  /// There must be no explicit dates.
	  /// This will be validated during schedule construction.
	  /// </para>
	  /// <para>
	  /// The convention 'Both' may be used to explicitly indicate there is both an initial and final stub.
	  /// The stubs themselves must be specified using explicit dates.
	  /// This will be validated during schedule construction.
	  /// </para>
	  /// <para>
	  /// The conventions 'ShortInitial', 'LongInitial', 'SmartInitial', 'ShortFinal', 'LongFinal'
	  /// and 'SmartFinal' are used to indicate the type of stub to be generated.
	  /// The exact behavior varies depending on whether there are explicit dates or not:
	  /// </para>
	  /// <para>
	  /// If explicit dates are specified, then the combination of stub convention an explicit date
	  /// will be validated during schedule construction. For example, the combination of an explicit dated
	  /// initial stub and a stub convention of 'ShortInitial', 'LongInitial' or 'SmartInitial' is valid,
	  /// but other stub conventions, such as 'ShortFinal' or 'None' would be invalid.
	  /// </para>
	  /// <para>
	  /// If explicit dates are not specified, then it is not required that a stub is generated.
	  /// The convention determines whether to generate dates from the start date forward, or the
	  /// end date backwards. Date generation may or may not result in a stub, but if it does then
	  /// the stub will be of the correct type.
	  /// </para>
	  /// <para>
	  /// When the stub convention is not present, the generation of stubs is based entirely on
	  /// the presence or absence of the explicit dates.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final StubConvention stubConvention;
	  private readonly StubConvention stubConvention;
	  /// <summary>
	  /// The optional convention defining how to roll dates.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to determine the schedule.
	  /// If not present, then the roll convention will be implied.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final RollConvention rollConvention;
	  private readonly RollConvention rollConvention;
	  /// <summary>
	  /// The optional start date of the first regular schedule period, which is the end date of the initial stub.
	  /// <para>
	  /// This is used to identify the boundary date between the initial stub and the first regular schedule period.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// This date must be on or after 'startDate'.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to determine the schedule.
	  /// If not present, then the overall schedule start date will be used instead, resulting in no initial stub.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate firstRegularStartDate;
	  private readonly LocalDate firstRegularStartDate;
	  /// <summary>
	  /// The optional end date of the last regular schedule period, which is the start date of the final stub.
	  /// <para>
	  /// This is used to identify the boundary date between the last regular schedule period and the final stub.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// This date must be after 'startDate' and after 'firstRegularStartDate'.
	  /// This date must be on or before 'endDate'.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to determine the schedule.
	  /// If not present, then the overall schedule end date will be used instead, resulting in no final stub.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate lastRegularEndDate;
	  private readonly LocalDate lastRegularEndDate;
	  /// <summary>
	  /// The optional start date of the first schedule period, overriding normal schedule generation.
	  /// <para>
	  /// This property is rarely used, and is generally needed when accrual starts before the effective date.
	  /// If specified, it overrides the start date of the first period once schedule generation has been completed.
	  /// Note that all schedule generation rules apply to 'startDate', with this applied as a final step.
	  /// This field primarily exists to support the FpML 'firstPeriodStartDate' concept.
	  /// </para>
	  /// <para>
	  /// If a roll convention is explicitly specified and the regular start date does not match it,
	  /// then the override will be used when generating regular periods.
	  /// </para>
	  /// <para>
	  /// If set, it should be different to the start date, although this is not validated.
	  /// Validation does check that it is before the 'firstRegularStartDate'.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to override the start date
	  /// of the first generated schedule period.
	  /// If not present, then the start of the first period will be the normal start date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.AdjustableDate overrideStartDate;
	  private readonly AdjustableDate overrideStartDate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a stub convention and end-of-month flag.
	  /// <para>
	  /// The business day adjustment is used for all dates.
	  /// The stub convention is used to determine whether there are any stubs.
	  /// If the end-of-month flag is true, then in any case of ambiguity the
	  /// end-of-month will be chosen.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="unadjustedStartDate">  start date, which is the start of the first schedule period </param>
	  /// <param name="unadjustedEndDate">  the end date, which is the end of the last schedule period </param>
	  /// <param name="frequency">  the regular periodic frequency </param>
	  /// <param name="businessDayAdjustment">  the business day adjustment to apply </param>
	  /// <param name="stubConvention">  the non-null convention defining how to handle stubs </param>
	  /// <param name="preferEndOfMonth">  whether to prefer the end-of-month when rolling </param>
	  /// <returns> the definition </returns>
	  public static PeriodicSchedule of(LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, Frequency frequency, BusinessDayAdjustment businessDayAdjustment, StubConvention stubConvention, bool preferEndOfMonth)
	  {
		ArgChecker.notNull(unadjustedStartDate, "unadjustedStartDate");
		ArgChecker.notNull(unadjustedEndDate, "unadjustedEndDate");
		ArgChecker.notNull(frequency, "frequency");
		ArgChecker.notNull(businessDayAdjustment, "businessDayAdjustment");
		ArgChecker.notNull(stubConvention, "stubConvention");
		return PeriodicSchedule.builder().startDate(unadjustedStartDate).endDate(unadjustedEndDate).frequency(frequency).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConvention).rollConvention(preferEndOfMonth ? RollConventions.EOM : null).build();
	  }

	  /// <summary>
	  /// Obtains an instance based on roll and stub conventions.
	  /// <para>
	  /// The business day adjustment is used for all dates.
	  /// The stub convention is used to determine whether there are any stubs.
	  /// The roll convention is used to fine tune each rolled date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="unadjustedStartDate">  start date, which is the start of the first schedule period </param>
	  /// <param name="unadjustedEndDate">  the end date, which is the end of the last schedule period </param>
	  /// <param name="frequency">  the regular periodic frequency </param>
	  /// <param name="businessDayAdjustment">  the business day adjustment to apply </param>
	  /// <param name="stubConvention">  the non-null convention defining how to handle stubs </param>
	  /// <param name="rollConvention">  the non-null convention defining how to roll dates </param>
	  /// <returns> the definition </returns>
	  public static PeriodicSchedule of(LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, Frequency frequency, BusinessDayAdjustment businessDayAdjustment, StubConvention stubConvention, RollConvention rollConvention)
	  {
		ArgChecker.notNull(unadjustedStartDate, "unadjustedStartDate");
		ArgChecker.notNull(unadjustedEndDate, "unadjustedEndDate");
		ArgChecker.notNull(frequency, "frequency");
		ArgChecker.notNull(businessDayAdjustment, "businessDayAdjustment");
		ArgChecker.notNull(stubConvention, "stubConvention");
		ArgChecker.notNull(rollConvention, "rollConvention");
		return PeriodicSchedule.builder().startDate(unadjustedStartDate).endDate(unadjustedEndDate).frequency(frequency).businessDayAdjustment(businessDayAdjustment).stubConvention(stubConvention).rollConvention(rollConvention).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		if (firstRegularStartDate != null)
		{
		  ArgChecker.inOrderOrEqual(startDate, firstRegularStartDate, "unadjusted", "firstRegularStartDate");
		  if (lastRegularEndDate != null)
		  {
			ArgChecker.inOrderNotEqual(firstRegularStartDate, lastRegularEndDate, "firstRegularStartDate", "lastRegularEndDate");
		  }
		  if (overrideStartDate != null)
		  {
			ArgChecker.inOrderNotEqual(overrideStartDate.Unadjusted, firstRegularStartDate, "overrideStartDate", "firstRegularStartDate");
		  }
		}
		if (lastRegularEndDate != null)
		{
		  ArgChecker.inOrderOrEqual(lastRegularEndDate, endDate, "lastRegularEndDate", "endDate");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the schedule from the definition.
	  /// <para>
	  /// The schedule consists of an optional initial stub, a number of regular periods
	  /// and an optional final stub.
	  /// </para>
	  /// <para>
	  /// The roll convention, stub convention and additional dates are all used to determine the schedule.
	  /// If the roll convention is not present it will be defaulted from the stub convention, with 'None' as the default.
	  /// If there are explicit stub dates then they will be used.
	  /// If the stub convention is present, then it will be validated against the stub dates.
	  /// If the stub convention and stub dates are not present, then no stubs are allowed.
	  /// </para>
	  /// <para>
	  /// There is special handling for pre-adjusted start dates to avoid creating incorrect stubs.
	  /// If all the following conditions hold true, then the unadjusted start date is treated
	  /// as being the day-of-month implied by the roll convention (the adjusted date is unaffected).
	  /// <ul>
	  /// <li>the {@code startDateBusinessDayAdjustment} property equals <seealso cref="BusinessDayAdjustment#NONE"/>
	  ///   or the roll convention is 'EOM'
	  /// <li>the roll convention is numeric or 'EOM'
	  /// <li>applying {@code businessDayAdjustment} to the day-of-month implied by the roll convention
	  ///  yields the specified start date
	  /// </ul>
	  /// </para>
	  /// <para>
	  /// There is additional special handling for pre-adjusted first/last regular dates and the end date.
	  /// If the following conditions hold true, then the unadjusted date is treated as being the
	  /// day-of-month implied by the roll convention (the adjusted date is unaffected).
	  /// <ul>
	  /// <li>the roll convention is numeric or 'EOM'
	  /// <li>applying {@code businessDayAdjustment} to the day-of-month implied by the roll convention
	  ///  yields the first/last regular date that was specified
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the schedule </returns>
	  /// <param name="refData">  the reference data, used to find the holiday calendars </param>
	  /// <exception cref="ScheduleException"> if the definition is invalid </exception>
	  public Schedule createSchedule(ReferenceData refData)
	  {
		LocalDate unadjStart = calculatedUnadjustedStartDate(refData);
		LocalDate unadjEnd = calculatedUnadjustedEndDate(refData);
		LocalDate regularStart = calculatedFirstRegularStartDate(unadjStart, refData);
		LocalDate regularEnd = calculatedLastRegularEndDate(unadjEnd, refData);
		RollConvention rollConv = calculatedRollConvention(regularStart, regularEnd);
		IList<LocalDate> unadj = generateUnadjustedDates(unadjStart, regularStart, regularEnd, unadjEnd, rollConv);
		IList<LocalDate> adj = applyBusinessDayAdjustment(unadj, refData);
		IList<SchedulePeriod> periods = new List<SchedulePeriod>();
		try
		{
		  // for performance, handle silly errors using exceptions
		  for (int i = 0; i < unadj.Count - 1; i++)
		  {
			periods.Add(SchedulePeriod.of(adj[i], adj[i + 1], unadj[i], unadj[i + 1]));
		  }
		}
		catch (System.ArgumentException ex)
		{
		  // check dates to throw a better exception for duplicate dates in schedule
		  createUnadjustedDates();
		  createAdjustedDates(refData);
		  // unknown exception
		  ScheduleException se = new ScheduleException(this, "Schedule calculation resulted in invalid period");
		  se.initCause(ex);
		  throw se;
		}
		return Schedule.builder().periods(periods).frequency(frequency).rollConvention(rollConv).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the list of unadjusted dates in the schedule.
	  /// <para>
	  /// The unadjusted date list will contain at least two elements, the start date and end date.
	  /// Between those dates will be the calculated periodic schedule.
	  /// </para>
	  /// <para>
	  /// The roll convention, stub convention and additional dates are all used to determine the schedule.
	  /// If the roll convention is not present it will be defaulted from the stub convention, with 'None' as the default.
	  /// If there are explicit stub dates then they will be used.
	  /// If the stub convention is present, then it will be validated against the stub dates.
	  /// If the stub convention and stub dates are not present, then no stubs are allowed.
	  /// If the frequency is 'Term' explicit stub dates are disallowed, and the roll and stub convention are ignored.
	  /// </para>
	  /// <para>
	  /// The special handling for last business day of month seen in
	  /// <seealso cref="#createUnadjustedDates(ReferenceData)"/> is not applied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the schedule of unadjusted dates </returns>
	  /// <exception cref="ScheduleException"> if the definition is invalid </exception>
	  public ImmutableList<LocalDate> createUnadjustedDates()
	  {
		LocalDate regularStart = calculatedFirstRegularStartDate();
		LocalDate regularEnd = calculatedLastRegularEndDate();
		RollConvention rollConv = calculatedRollConvention(regularStart, regularEnd);
		IList<LocalDate> unadj = generateUnadjustedDates(startDate, regularStart, regularEnd, endDate, rollConv);
		// ensure schedule is valid with no duplicated dates
		ImmutableList<LocalDate> deduplicated = ImmutableSet.copyOf(unadj).asList();
		if (deduplicated.size() < unadj.Count)
		{
		  throw new ScheduleException(this, "Schedule calculation resulted in duplicate unadjusted dates {}", unadj);
		}
		return deduplicated;
	  }

	  /// <summary>
	  /// Creates the list of unadjusted dates in the schedule.
	  /// <para>
	  /// The unadjusted date list will contain at least two elements, the start date and end date.
	  /// Between those dates will be the calculated periodic schedule.
	  /// </para>
	  /// <para>
	  /// The roll convention, stub convention and additional dates are all used to determine the schedule.
	  /// If the roll convention is not present it will be defaulted from the stub convention, with 'None' as the default.
	  /// If there are explicit stub dates then they will be used.
	  /// If the stub convention is present, then it will be validated against the stub dates.
	  /// If the stub convention and stub dates are not present, then no stubs are allowed.
	  /// If the frequency is 'Term' explicit stub dates are disallowed, and the roll and stub convention are ignored.
	  /// </para>
	  /// <para>
	  /// There is special handling for pre-adjusted start dates to avoid creating incorrect stubs.
	  /// If all the following conditions hold true, then the unadjusted start date is treated
	  /// as being the day-of-month implied by the roll convention (the adjusted date is unaffected).
	  /// <ul>
	  /// <li>the {@code startDateBusinessDayAdjustment} property equals <seealso cref="BusinessDayAdjustment#NONE"/>
	  ///   or the roll convention is 'EOM'
	  /// <li>the roll convention is numeric or 'EOM'
	  /// <li>applying {@code businessDayAdjustment} to the day-of-month implied by the roll convention
	  ///  yields the specified start date
	  /// </ul>
	  /// </para>
	  /// <para>
	  /// There is additional special handling for pre-adjusted first/last regular dates and the end date.
	  /// If the following conditions hold true, then the unadjusted date is treated as being the
	  /// day-of-month implied by the roll convention (the adjusted date is unaffected).
	  /// <ul>
	  /// <li>the roll convention is numeric or 'EOM'
	  /// <li>applying {@code businessDayAdjustment} to the day-of-month implied by the roll convention
	  ///  yields the first/last regular date that was specified
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to find the holiday calendars </param>
	  /// <returns> the schedule of unadjusted dates </returns>
	  /// <exception cref="ScheduleException"> if the definition is invalid </exception>
	  public ImmutableList<LocalDate> createUnadjustedDates(ReferenceData refData)
	  {
		LocalDate unadjStart = calculatedUnadjustedStartDate(refData);
		LocalDate unadjEnd = calculatedUnadjustedEndDate(refData);
		LocalDate regularStart = calculatedFirstRegularStartDate(unadjStart, refData);
		LocalDate regularEnd = calculatedLastRegularEndDate(unadjEnd, refData);
		RollConvention rollConv = calculatedRollConvention(regularStart, regularEnd);
		IList<LocalDate> unadj = generateUnadjustedDates(unadjStart, regularStart, regularEnd, unadjEnd, rollConv);
		// ensure schedule is valid with no duplicated dates
		ImmutableList<LocalDate> deduplicated = ImmutableSet.copyOf(unadj).asList();
		if (deduplicated.size() < unadj.Count)
		{
		  throw new ScheduleException(this, "Schedule calculation resulted in duplicate unadjusted dates {}", unadj);
		}
		return deduplicated;
	  }

	  // creates the unadjusted dates, returning the mutable list
	  private IList<LocalDate> generateUnadjustedDates(LocalDate start, LocalDate regStart, LocalDate regEnd, LocalDate end, RollConvention rollConv)
	  {

		LocalDate overrideStart = overrideStartDate != null ? overrideStartDate.Unadjusted : start;
		bool explicitInitStub = !start.Equals(regStart);
		bool explicitFinalStub = !end.Equals(regEnd);
		// handle case where whole period is stub
		if (regStart.Equals(end) || regEnd.Equals(start))
		{
		  return ImmutableList.of(overrideStart, end);
		}
		// handle TERM frequency
		if (frequency == Frequency.TERM)
		{
		  if (explicitInitStub || explicitFinalStub)
		  {
			throw new ScheduleException(this, "Explicit stubs must not be specified when using 'Term' frequency");
		  }
		  return ImmutableList.of(overrideStart, end);
		}
		// calculate base schedule excluding explicit stubs
		StubConvention stubConv = generateImplicitStubConvention(explicitInitStub, explicitFinalStub);
		// special fallback if there is an override start date with a specified roll convention
		if (overrideStartDate != null && rollConvention != null && firstRegularStartDate == null && !rollConv.matches(regStart) && rollConv.matches(overrideStart))
		{
		  return generateUnadjustedDates(overrideStart, regEnd, rollConv, stubConv, explicitInitStub, overrideStart, explicitFinalStub, end);
		}
		else
		{
		  return generateUnadjustedDates(regStart, regEnd, rollConv, stubConv, explicitInitStub, overrideStart, explicitFinalStub, end);
		}
	  }

	  // using knowledge of the explicit stubs, generate the correct convention for implicit stubs
	  private StubConvention generateImplicitStubConvention(bool explicitInitialStub, bool explicitFinalStub)
	  {
		// null is not same as NONE
		// NONE validates that there are no explicit stubs
		// null ensures that remainder after explicit stubs are removed has no stubs
		if (stubConvention != null)
		{
		  return stubConvention.toImplicit(this, explicitInitialStub, explicitFinalStub);
		}
		return StubConvention.NONE;
	  }

	  // generate dates, forwards or backwards
	  private IList<LocalDate> generateUnadjustedDates(LocalDate regStart, LocalDate regEnd, RollConvention rollCnv, StubConvention stubCnv, bool explicitInitStub, LocalDate overrideStart, bool explicitFinalStub, LocalDate end)
	  {

		if (stubCnv.CalculateBackwards)
		{
		  // called only when stub is initial
		  // validate that explicit stub flags have been resolved to stub convention of NONE
		  ArgChecker.isFalse(explicitInitStub, "Value explicitInitStub must be false");
		  ArgChecker.isFalse(explicitFinalStub, "Value explicitFinalStub must be false");
		  return generateBackwards(this, regStart, regEnd, frequency, rollCnv, stubCnv, overrideStart);
		}
		else
		{
		  return generateForwards(this, regStart, regEnd, frequency, rollCnv, stubCnv, explicitInitStub, overrideStart, explicitFinalStub, end);
		}
	  }

	  // generate the schedule of dates backwards from the end, only called when stub convention is initial
	  private static IList<LocalDate> generateBackwards(PeriodicSchedule schedule, LocalDate start, LocalDate end, Frequency frequency, RollConvention rollConv, StubConvention stubConv, LocalDate explicitStartDate)
	  {

		// validate
		if (rollConv.matches(end) == false)
		{
		  throw new ScheduleException(schedule, "Date '{}' does not match roll convention '{}' when starting to roll backwards", end, rollConv);
		}
		// generate
		BackwardsList dates = new BackwardsList(estimateNumberPeriods(start, end, frequency));
		dates.addFirst(end);
		LocalDate temp = rollConv.previous(end, frequency);
		while (temp.isAfter(start))
		{
		  dates.addFirst(temp);
		  temp = rollConv.previous(temp, frequency);
		}
		// convert to long stub, but only if we actually have a stub
		bool stub = temp.Equals(start) == false;
		if (stub && dates.Count > 1 && stubConv.isStubLong(start, dates[0]))
		{
		  dates.removeFirst();
		}
		dates.addFirst(explicitStartDate);
		return dates;
	  }

	  // dedicated list implementation for backwards looping for performance
	  // only implements those methods that are needed
	  private class BackwardsList : System.Collections.ObjectModel.Collection<LocalDate>
	  {
		internal int first;
		internal LocalDate[] array;

		internal BackwardsList(int capacity)
		{
		  this.array = new LocalDate[capacity];
		  this.first = array.Length;
		}

		public override LocalDate get(int index)
		{
		  return array[first + index];
		}

		public override int size()
		{
		  return array.Length - first;
		}

		internal virtual void addFirst(LocalDate date)
		{
		  array[--first] = date;
		}

		internal virtual void removeFirst()
		{
		  first++;
		}
	  }

	  // generate the schedule of dates forwards from the start, called when stub convention is not initial
	  private static IList<LocalDate> generateForwards(PeriodicSchedule schedule, LocalDate start, LocalDate end, Frequency frequency, RollConvention rollConv, StubConvention stubConv, bool explicitInitialStub, LocalDate explicitStartDate, bool explicitFinalStub, LocalDate explicitEndDate)
	  {

		// validate
		if (rollConv.matches(start) == false)
		{
		  throw new ScheduleException(schedule, "Date '{}' does not match roll convention '{}' when starting to roll forwards", start, rollConv);
		}
		// generate
		IList<LocalDate> dates = new List<LocalDate>(estimateNumberPeriods(start, end, frequency));
		if (explicitInitialStub)
		{
		  dates.Add(explicitStartDate);
		  dates.Add(start);
		}
		else
		{
		  dates.Add(explicitStartDate);
		}
		LocalDate temp = rollConv.next(start, frequency);
		while (temp.isBefore(end))
		{
		  dates.Add(temp);
		  temp = rollConv.next(temp, frequency);
		}
		// convert short stub to long stub, but only if we actually have a stub
		bool stub = temp.Equals(end) == false;
		if (stub && dates.Count > 1)
		{
		  if (stubConv == StubConvention.NONE)
		  {
			throw new ScheduleException(schedule, "Period '{}' to '{}' resulted in a disallowed stub with frequency '{}'", start, end, frequency);
		  }
		  if (stubConv.isStubLong(dates[dates.Count - 1], end))
		  {
			dates.RemoveAt(dates.Count - 1);
		  }
		}
		dates.Add(end);
		if (explicitFinalStub)
		{
		  dates.Add(explicitEndDate);
		}
		return dates;
	  }

	  // roughly estimate the number of periods (overestimating)
	  private static int estimateNumberPeriods(LocalDate start, LocalDate end, Frequency frequency)
	  {
		int termInYearsEstimate = end.Year - start.Year + 2;
		return (int)(Math.Max(frequency.eventsPerYearEstimate(), 1) * termInYearsEstimate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the list of adjusted dates in the schedule.
	  /// <para>
	  /// The adjusted date list will contain at least two elements, the start date and end date.
	  /// Between those dates will be the calculated periodic schedule.
	  /// Each date will be a valid business day as per the appropriate business day adjustment.
	  /// </para>
	  /// <para>
	  /// The roll convention, stub convention and additional dates are all used to determine the schedule.
	  /// If the roll convention is not present it will be defaulted from the stub convention, with 'None' as the default.
	  /// If there are explicit stub dates then they will be used.
	  /// If the stub convention is present, then it will be validated against the stub dates.
	  /// If the stub convention and stub dates are not present, then no stubs are allowed.
	  /// </para>
	  /// <para>
	  /// There is special handling for pre-adjusted start dates to avoid creating incorrect stubs.
	  /// If all the following conditions hold true, then the unadjusted start date is treated
	  /// as being the day-of-month implied by the roll convention (the adjusted date is unaffected).
	  /// <ul>
	  /// <li>the {@code startDateBusinessDayAdjustment} property equals <seealso cref="BusinessDayAdjustment#NONE"/>
	  ///   or the roll convention is 'EOM'
	  /// <li>the roll convention is numeric or 'EOM'
	  /// <li>applying {@code businessDayAdjustment} to the day-of-month implied by the roll convention
	  ///  yields the specified start date
	  /// </ul>
	  /// </para>
	  /// <para>
	  /// There is additional special handling for pre-adjusted first/last regular dates and the end date.
	  /// If the following conditions hold true, then the unadjusted date is treated as being the
	  /// day-of-month implied by the roll convention (the adjusted date is unaffected).
	  /// <ul>
	  /// <li>the roll convention is numeric or 'EOM'
	  /// <li>applying {@code businessDayAdjustment} to the day-of-month implied by the roll convention
	  ///  yields the first/last regular date that was specified
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the schedule of dates adjusted to valid business days </returns>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <exception cref="ScheduleException"> if the definition is invalid </exception>
	  public ImmutableList<LocalDate> createAdjustedDates(ReferenceData refData)
	  {
		LocalDate unadjStart = calculatedUnadjustedStartDate(refData);
		LocalDate unadjEnd = calculatedUnadjustedEndDate(refData);
		LocalDate regularStart = calculatedFirstRegularStartDate(unadjStart, refData);
		LocalDate regularEnd = calculatedLastRegularEndDate(unadjEnd, refData);
		RollConvention rollConv = calculatedRollConvention(regularStart, regularEnd);
		IList<LocalDate> unadj = generateUnadjustedDates(unadjStart, regularStart, regularEnd, unadjEnd, rollConv);
		IList<LocalDate> adj = applyBusinessDayAdjustment(unadj, refData);
		// ensure schedule is valid with no duplicated dates
		ImmutableList<LocalDate> deduplicated = ImmutableSet.copyOf(adj).asList();
		if (deduplicated.size() < adj.Count)
		{
		  throw new ScheduleException(this, "Schedule calculation resulted in duplicate adjusted dates {} from unadjusted dates {} using adjustment '{}'", adj, unadj, businessDayAdjustment);
		}
		return deduplicated;
	  }

	  // applies the appropriate business day adjustment to each date
	  private IList<LocalDate> applyBusinessDayAdjustment(IList<LocalDate> unadj, ReferenceData refData)
	  {
		IList<LocalDate> adj = new List<LocalDate>(unadj.Count);
		adj.Add(calculatedStartDate().adjusted(refData));
		for (int i = 1; i < unadj.Count - 1; i++)
		{
		  adj.Add(businessDayAdjustment.adjust(unadj[i], refData));
		}
		adj.Add(calculatedEndDate().adjusted(refData));
		return adj;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the applicable roll convention defining how to roll dates.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// The applicable roll convention is a non-null value.
	  /// If the roll convention property is not present, this is determined from the
	  /// stub convention, dates and frequency, defaulting to 'None' if necessary.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the non-null roll convention </returns>
	  public RollConvention calculatedRollConvention()
	  {
		return calculatedRollConvention(calculatedFirstRegularStartDate(), calculatedLastRegularEndDate());
	  }

	  // calculates the applicable roll convention
	  // the calculated start date parameter allows for influence by calculatedUnadjustedStartDate()
	  private RollConvention calculatedRollConvention(LocalDate calculatedFirstRegStartDate, LocalDate calculatedLastRegEndDate)
	  {

		// determine roll convention from stub convention
		StubConvention stubConv = MoreObjects.firstNonNull(stubConvention, StubConvention.NONE);
		// special handling for EOM as it is advisory rather than mandatory
		if (rollConvention == RollConventions.EOM)
		{
		  RollConvention derived = stubConv.toRollConvention(calculatedFirstRegStartDate, calculatedLastRegEndDate, frequency, true);
		  return (derived == RollConventions.NONE ? RollConventions.EOM : derived);
		}
		// avoid RollConventions.NONE if possible
		if (rollConvention == null || rollConvention == RollConventions.NONE)
		{
		  return stubConv.toRollConvention(calculatedFirstRegStartDate, calculatedLastRegEndDate, frequency, false);
		}
		// use RollConventions.NONE if nothing else applies
		return MoreObjects.firstNonNull(rollConvention, RollConventions.NONE);
	  }

	  //-------------------------------------------------------------------------
	  // calculates the applicable start date
	  // applies de facto rule where EOM means last business day for startDate
	  // and similar rule for numeric roll conventions
	  // http://www.fpml.org/forums/topic/can-a-roll-convention-imply-a-stub/#post-7659
	  // For 'StandardRollConventions', such as IMM, adjusted date is identified by finding the closest valid roll date
	  // and applying the the trade level business day adjustment
	  private LocalDate calculatedUnadjustedStartDate(ReferenceData refData)
	  {
		// change date if
		// reference data is available
		// and explicit start adjustment must be NONE or roll convention is EOM
		// and either
		// numeric roll convention and day-of-month actually differs
		// or
		// StandardDayConvention is used and the day is not a valid roll date

		if (refData != null && rollConvention != null && (BusinessDayAdjustment.NONE.Equals(startDateBusinessDayAdjustment) || rollConvention == RollConventions.EOM))
		{
		  return calculatedUnadjustedDateFromAdjusted(startDate, rollConvention, businessDayAdjustment, refData);
		}
		return startDate;
	  }

	  // calculates the applicable end date
	  private LocalDate calculatedUnadjustedEndDate(ReferenceData refData)
	  {
		if (refData != null && rollConvention != null)
		{
		  return calculatedUnadjustedDateFromAdjusted(endDate, rollConvention, calculatedEndDateBusinessDayAdjustment(), refData);
		}
		return endDate;
	  }

	  // calculates an unadjusted date
	  // for EOM and day of month roll conventions the unadjusted date is based on the roll day-of-month
	  // for other conventions, the nearest unadjusted roll date is calculated, adjusted and compared to the base date
	  // this is known not to work for day of week conventions if the passed date has been adjusted forwards
	  private static LocalDate calculatedUnadjustedDateFromAdjusted(LocalDate baseDate, RollConvention rollConvention, BusinessDayAdjustment businessDayAdjustment, ReferenceData refData)
	  {

		int rollDom = rollConvention.DayOfMonth;

		if (rollDom > 0 && baseDate.DayOfMonth != rollDom)
		{
		  int lengthOfMonth = baseDate.lengthOfMonth();
		  int actualDom = Math.Min(rollDom, lengthOfMonth);
		  // startDate is already the expected day, then nothing to do
		  if (baseDate.DayOfMonth != actualDom)
		  {
			LocalDate rollImpliedDate = baseDate.withDayOfMonth(actualDom);
			LocalDate adjDate = businessDayAdjustment.adjust(rollImpliedDate, refData);
			if (adjDate.Equals(baseDate))
			{
			  return rollImpliedDate;
			}
		  }
		}
		else if (rollDom == 0)
		{
		  //0 roll day implies that the roll date is calculated relative to the month or week

		  //Find the valid (unadjusted) roll date for the given month or week
		  LocalDate rollImpliedDate = rollConvention.adjust(baseDate);

		  if (!rollImpliedDate.Equals(baseDate))
		  {

			//If roll date is relative to the month the assumption is that the adjusted date is not in a different month to
			//the original unadjusted date. This is safe as the roll day produced by monthly roll conventions are typically
			//not close to the end of the month and hence any reasonable adjustment will not move into the next month.
			//adjust() method for "day of week" roll conventions will roll forward from the passed date; hence this logic
			//will not work for "day of week" conventions if the passed baseDate has been adjusted to be after the original
			//unadjusted date (i.e. has been rolled forward).

			//Calculate the expected adjusted roll date, based on the valid unadjusted roll date
			LocalDate adjDate = businessDayAdjustment.adjust(rollImpliedDate, refData);

			//If the adjusted roll date equals the original base date then that the base date is in fact an adjusted date
			//and hence return the unadjusted date for building the schedule.
			if (adjDate.Equals(baseDate))
			{
			  return rollImpliedDate;
			}
		  }

		}
		return baseDate;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the applicable first regular start date.
	  /// <para>
	  /// This will be either 'firstRegularStartDate' or 'startDate'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the non-null start date of the first regular period </returns>
	  public LocalDate calculatedFirstRegularStartDate()
	  {
		return MoreObjects.firstNonNull(firstRegularStartDate, startDate);
	  }

	  // calculates the first regular start date
	  // adjust when numeric roll convention present
	  private LocalDate calculatedFirstRegularStartDate(LocalDate unadjStart, ReferenceData refData)
	  {
		if (firstRegularStartDate == null)
		{
		  return unadjStart;
		}
		if (refData != null && rollConvention != null)
		{
		  return calculatedUnadjustedDateFromAdjusted(firstRegularStartDate, rollConvention, businessDayAdjustment, refData);
		}
		return firstRegularStartDate;
	  }

	  /// <summary>
	  /// Calculates the applicable last regular end date.
	  /// <para>
	  /// This will be either 'lastRegularEndDate' or 'endDate'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the non-null end date of the last regular period </returns>
	  public LocalDate calculatedLastRegularEndDate()
	  {
		return MoreObjects.firstNonNull(lastRegularEndDate, endDate);
	  }

	  // calculates the last regular end date
	  // adjust when numeric roll convention present
	  private LocalDate calculatedLastRegularEndDate(LocalDate unadjEnd, ReferenceData refData)
	  {
		if (lastRegularEndDate == null)
		{
		  return unadjEnd;
		}
		if (refData != null && rollConvention != null)
		{
		  return calculatedUnadjustedDateFromAdjusted(lastRegularEndDate, rollConvention, businessDayAdjustment, refData);
		}
		return lastRegularEndDate;
	  }

	  /// <summary>
	  /// Calculates the applicable business day adjustment to apply to the start date.
	  /// <para>
	  /// This will be either 'startDateBusinessDayAdjustment' or 'businessDayAdjustment'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the non-null business day adjustment to apply to the start date </returns>
	  private BusinessDayAdjustment calculatedStartDateBusinessDayAdjustment()
	  {
		return MoreObjects.firstNonNull(startDateBusinessDayAdjustment, businessDayAdjustment);
	  }

	  /// <summary>
	  /// Calculates the applicable business day adjustment to apply to the end date.
	  /// <para>
	  /// This will be either 'endDateBusinessDayAdjustment' or 'businessDayAdjustment'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the non-null business day adjustment to apply to the end date </returns>
	  private BusinessDayAdjustment calculatedEndDateBusinessDayAdjustment()
	  {
		return MoreObjects.firstNonNull(endDateBusinessDayAdjustment, businessDayAdjustment);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the applicable start date.
	  /// <para>
	  /// The result combines the start date and the appropriate business day adjustment.
	  /// If the override start date is present, it will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calculated start date </returns>
	  public AdjustableDate calculatedStartDate()
	  {
		if (overrideStartDate != null)
		{
		  return overrideStartDate;
		}
		return AdjustableDate.of(startDate, calculatedStartDateBusinessDayAdjustment());
	  }

	  /// <summary>
	  /// Calculates the applicable end date.
	  /// <para>
	  /// The result combines the end date and the appropriate business day adjustment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calculated end date </returns>
	  public AdjustableDate calculatedEndDate()
	  {
		return AdjustableDate.of(endDate, calculatedEndDateBusinessDayAdjustment());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code PeriodicSchedule}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static PeriodicSchedule.Meta meta()
	  {
		return PeriodicSchedule.Meta.INSTANCE;
	  }

	  static PeriodicSchedule()
	  {
		MetaBean.register(PeriodicSchedule.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static PeriodicSchedule.Builder builder()
	  {
		return new PeriodicSchedule.Builder();
	  }

	  private PeriodicSchedule(LocalDate startDate, LocalDate endDate, Frequency frequency, BusinessDayAdjustment businessDayAdjustment, BusinessDayAdjustment startDateBusinessDayAdjustment, BusinessDayAdjustment endDateBusinessDayAdjustment, StubConvention stubConvention, RollConvention rollConvention, LocalDate firstRegularStartDate, LocalDate lastRegularEndDate, AdjustableDate overrideStartDate)
	  {
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(frequency, "frequency");
		JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		this.startDate = startDate;
		this.endDate = endDate;
		this.frequency = frequency;
		this.businessDayAdjustment = businessDayAdjustment;
		this.startDateBusinessDayAdjustment = startDateBusinessDayAdjustment;
		this.endDateBusinessDayAdjustment = endDateBusinessDayAdjustment;
		this.stubConvention = stubConvention;
		this.rollConvention = rollConvention;
		this.firstRegularStartDate = firstRegularStartDate;
		this.lastRegularEndDate = lastRegularEndDate;
		this.overrideStartDate = overrideStartDate;
		validate();
	  }

	  public override PeriodicSchedule.Meta metaBean()
	  {
		return PeriodicSchedule.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date, which is the start of the first schedule period.
	  /// <para>
	  /// This is the start date of the schedule.
	  /// It is is unadjusted and as such might be a weekend or holiday.
	  /// Any applicable business day adjustment will be applied when creating the schedule.
	  /// This is also known as the unadjusted effective date.
	  /// </para>
	  /// <para>
	  /// In most cases, the start date of a financial instrument is just after the trade date,
	  /// such as two business days later. However, the start date of a schedule is permitted
	  /// to be any date, which includes dates before or after the trade date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date, which is the end of the last schedule period.
	  /// <para>
	  /// This is the end date of the schedule.
	  /// It is is unadjusted and as such might be a weekend or holiday.
	  /// Any applicable business day adjustment will be applied when creating the schedule.
	  /// This is also known as the unadjusted maturity date or unadjusted termination date.
	  /// This date must be after the start date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the regular periodic frequency to use.
	  /// <para>
	  /// Most dates are calculated using a regular periodic frequency, such as every 3 months.
	  /// The actual day-of-month or day-of-week is selected using the roll and stub conventions.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency Frequency
	  {
		  get
		  {
			return frequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply.
	  /// <para>
	  /// Each date in the calculated schedule is determined without taking into account weekends and holidays.
	  /// The adjustment specified here is used to convert those dates to valid business days.
	  /// </para>
	  /// <para>
	  /// The start date and end date may have their own business day adjustment rules.
	  /// If those are not present, then this adjustment is used instead.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment BusinessDayAdjustment
	  {
		  get
		  {
			return businessDayAdjustment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional business day adjustment to apply to the start date.
	  /// <para>
	  /// The start date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the start date to a valid business day.
	  /// </para>
	  /// <para>
	  /// If this property is not present, the standard {@code businessDayAdjustment} property is used instead.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<BusinessDayAdjustment> StartDateBusinessDayAdjustment
	  {
		  get
		  {
			return Optional.ofNullable(startDateBusinessDayAdjustment);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional business day adjustment to apply to the end date.
	  /// <para>
	  /// The end date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert the end date to a valid business day.
	  /// </para>
	  /// <para>
	  /// If this property is not present, the standard {@code businessDayAdjustment} property is used instead.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<BusinessDayAdjustment> EndDateBusinessDayAdjustment
	  {
		  get
		  {
			return Optional.ofNullable(endDateBusinessDayAdjustment);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional convention defining how to handle stubs.
	  /// <para>
	  /// The stub convention is used during schedule construction to determine whether the irregular
	  /// remaining period occurs at the start or end of the schedule.
	  /// It also determines whether the irregular period is shorter or longer than the regular period.
	  /// This property interacts with the "explicit dates" of <seealso cref="PeriodicSchedule#getFirstRegularStartDate()"/>
	  /// and <seealso cref="PeriodicSchedule#getLastRegularEndDate()"/>.
	  /// </para>
	  /// <para>
	  /// The convention 'None' may be used to explicitly indicate there are no stubs.
	  /// There must be no explicit dates.
	  /// This will be validated during schedule construction.
	  /// </para>
	  /// <para>
	  /// The convention 'Both' may be used to explicitly indicate there is both an initial and final stub.
	  /// The stubs themselves must be specified using explicit dates.
	  /// This will be validated during schedule construction.
	  /// </para>
	  /// <para>
	  /// The conventions 'ShortInitial', 'LongInitial', 'SmartInitial', 'ShortFinal', 'LongFinal'
	  /// and 'SmartFinal' are used to indicate the type of stub to be generated.
	  /// The exact behavior varies depending on whether there are explicit dates or not:
	  /// </para>
	  /// <para>
	  /// If explicit dates are specified, then the combination of stub convention an explicit date
	  /// will be validated during schedule construction. For example, the combination of an explicit dated
	  /// initial stub and a stub convention of 'ShortInitial', 'LongInitial' or 'SmartInitial' is valid,
	  /// but other stub conventions, such as 'ShortFinal' or 'None' would be invalid.
	  /// </para>
	  /// <para>
	  /// If explicit dates are not specified, then it is not required that a stub is generated.
	  /// The convention determines whether to generate dates from the start date forward, or the
	  /// end date backwards. Date generation may or may not result in a stub, but if it does then
	  /// the stub will be of the correct type.
	  /// </para>
	  /// <para>
	  /// When the stub convention is not present, the generation of stubs is based entirely on
	  /// the presence or absence of the explicit dates.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<StubConvention> StubConvention
	  {
		  get
		  {
			return Optional.ofNullable(stubConvention);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional convention defining how to roll dates.
	  /// <para>
	  /// The schedule periods are determined at the high level by repeatedly adding
	  /// the frequency to the start date, or subtracting it from the end date.
	  /// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to determine the schedule.
	  /// If not present, then the roll convention will be implied.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<RollConvention> RollConvention
	  {
		  get
		  {
			return Optional.ofNullable(rollConvention);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional start date of the first regular schedule period, which is the end date of the initial stub.
	  /// <para>
	  /// This is used to identify the boundary date between the initial stub and the first regular schedule period.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// This date must be on or after 'startDate'.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to determine the schedule.
	  /// If not present, then the overall schedule start date will be used instead, resulting in no initial stub.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> FirstRegularStartDate
	  {
		  get
		  {
			return Optional.ofNullable(firstRegularStartDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional end date of the last regular schedule period, which is the start date of the final stub.
	  /// <para>
	  /// This is used to identify the boundary date between the last regular schedule period and the final stub.
	  /// </para>
	  /// <para>
	  /// This is an unadjusted date, and as such it might not be a valid business day.
	  /// This date must be after 'startDate' and after 'firstRegularStartDate'.
	  /// This date must be on or before 'endDate'.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to determine the schedule.
	  /// If not present, then the overall schedule end date will be used instead, resulting in no final stub.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> LastRegularEndDate
	  {
		  get
		  {
			return Optional.ofNullable(lastRegularEndDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional start date of the first schedule period, overriding normal schedule generation.
	  /// <para>
	  /// This property is rarely used, and is generally needed when accrual starts before the effective date.
	  /// If specified, it overrides the start date of the first period once schedule generation has been completed.
	  /// Note that all schedule generation rules apply to 'startDate', with this applied as a final step.
	  /// This field primarily exists to support the FpML 'firstPeriodStartDate' concept.
	  /// </para>
	  /// <para>
	  /// If a roll convention is explicitly specified and the regular start date does not match it,
	  /// then the override will be used when generating regular periods.
	  /// </para>
	  /// <para>
	  /// If set, it should be different to the start date, although this is not validated.
	  /// Validation does check that it is before the 'firstRegularStartDate'.
	  /// </para>
	  /// <para>
	  /// During schedule generation, if this is present it will be used to override the start date
	  /// of the first generated schedule period.
	  /// If not present, then the start of the first period will be the normal start date.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<AdjustableDate> OverrideStartDate
	  {
		  get
		  {
			return Optional.ofNullable(overrideStartDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  PeriodicSchedule other = (PeriodicSchedule) obj;
		  return JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(frequency, other.frequency) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(startDateBusinessDayAdjustment, other.startDateBusinessDayAdjustment) && JodaBeanUtils.equal(endDateBusinessDayAdjustment, other.endDateBusinessDayAdjustment) && JodaBeanUtils.equal(stubConvention, other.stubConvention) && JodaBeanUtils.equal(rollConvention, other.rollConvention) && JodaBeanUtils.equal(firstRegularStartDate, other.firstRegularStartDate) && JodaBeanUtils.equal(lastRegularEndDate, other.lastRegularEndDate) && JodaBeanUtils.equal(overrideStartDate, other.overrideStartDate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(frequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDateBusinessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stubConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstRegularStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastRegularEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(overrideStartDate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(384);
		buf.Append("PeriodicSchedule{");
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("frequency").Append('=').Append(frequency).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("startDateBusinessDayAdjustment").Append('=').Append(startDateBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("endDateBusinessDayAdjustment").Append('=').Append(endDateBusinessDayAdjustment).Append(',').Append(' ');
		buf.Append("stubConvention").Append('=').Append(stubConvention).Append(',').Append(' ');
		buf.Append("rollConvention").Append('=').Append(rollConvention).Append(',').Append(' ');
		buf.Append("firstRegularStartDate").Append('=').Append(firstRegularStartDate).Append(',').Append(' ');
		buf.Append("lastRegularEndDate").Append('=').Append(lastRegularEndDate).Append(',').Append(' ');
		buf.Append("overrideStartDate").Append('=').Append(JodaBeanUtils.ToString(overrideStartDate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code PeriodicSchedule}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(PeriodicSchedule), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(PeriodicSchedule), typeof(LocalDate));
			  frequency_Renamed = DirectMetaProperty.ofImmutable(this, "frequency", typeof(PeriodicSchedule), typeof(Frequency));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(PeriodicSchedule), typeof(BusinessDayAdjustment));
			  startDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "startDateBusinessDayAdjustment", typeof(PeriodicSchedule), typeof(BusinessDayAdjustment));
			  endDateBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "endDateBusinessDayAdjustment", typeof(PeriodicSchedule), typeof(BusinessDayAdjustment));
			  stubConvention_Renamed = DirectMetaProperty.ofImmutable(this, "stubConvention", typeof(PeriodicSchedule), typeof(StubConvention));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(PeriodicSchedule), typeof(RollConvention));
			  firstRegularStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstRegularStartDate", typeof(PeriodicSchedule), typeof(LocalDate));
			  lastRegularEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastRegularEndDate", typeof(PeriodicSchedule), typeof(LocalDate));
			  overrideStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "overrideStartDate", typeof(PeriodicSchedule), typeof(AdjustableDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startDate", "endDate", "frequency", "businessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "firstRegularStartDate", "lastRegularEndDate", "overrideStartDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code frequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> frequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDateBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> startDateBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDateBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> endDateBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code stubConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StubConvention> stubConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code rollConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RollConvention> rollConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstRegularStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> firstRegularStartDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastRegularEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastRegularEndDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code overrideStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> overrideStartDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startDate", "endDate", "frequency", "businessDayAdjustment", "startDateBusinessDayAdjustment", "endDateBusinessDayAdjustment", "stubConvention", "rollConvention", "firstRegularStartDate", "lastRegularEndDate", "overrideStartDate");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 429197561: // startDateBusinessDayAdjustment
			  return startDateBusinessDayAdjustment_Renamed;
			case -734327136: // endDateBusinessDayAdjustment
			  return endDateBusinessDayAdjustment_Renamed;
			case -31408449: // stubConvention
			  return stubConvention_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 2011803076: // firstRegularStartDate
			  return firstRegularStartDate_Renamed;
			case -1540679645: // lastRegularEndDate
			  return lastRegularEndDate_Renamed;
			case -599936828: // overrideStartDate
			  return overrideStartDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override PeriodicSchedule.Builder builder()
		{
		  return new PeriodicSchedule.Builder();
		}

		public override Type beanType()
		{
		  return typeof(PeriodicSchedule);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code frequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> frequency()
		{
		  return frequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDateBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> startDateBusinessDayAdjustment()
		{
		  return startDateBusinessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDateBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> endDateBusinessDayAdjustment()
		{
		  return endDateBusinessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code stubConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StubConvention> stubConvention()
		{
		  return stubConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rollConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RollConvention> rollConvention()
		{
		  return rollConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstRegularStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> firstRegularStartDate()
		{
		  return firstRegularStartDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastRegularEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastRegularEndDate()
		{
		  return lastRegularEndDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code overrideStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> overrideStartDate()
		{
		  return overrideStartDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return ((PeriodicSchedule) bean).StartDate;
			case -1607727319: // endDate
			  return ((PeriodicSchedule) bean).EndDate;
			case -70023844: // frequency
			  return ((PeriodicSchedule) bean).Frequency;
			case -1065319863: // businessDayAdjustment
			  return ((PeriodicSchedule) bean).BusinessDayAdjustment;
			case 429197561: // startDateBusinessDayAdjustment
			  return ((PeriodicSchedule) bean).startDateBusinessDayAdjustment;
			case -734327136: // endDateBusinessDayAdjustment
			  return ((PeriodicSchedule) bean).endDateBusinessDayAdjustment;
			case -31408449: // stubConvention
			  return ((PeriodicSchedule) bean).stubConvention;
			case -10223666: // rollConvention
			  return ((PeriodicSchedule) bean).rollConvention;
			case 2011803076: // firstRegularStartDate
			  return ((PeriodicSchedule) bean).firstRegularStartDate;
			case -1540679645: // lastRegularEndDate
			  return ((PeriodicSchedule) bean).lastRegularEndDate;
			case -599936828: // overrideStartDate
			  return ((PeriodicSchedule) bean).overrideStartDate;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code PeriodicSchedule}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<PeriodicSchedule>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency frequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment startDateBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment endDateBusinessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StubConvention stubConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RollConvention rollConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate firstRegularStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastRegularEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal AdjustableDate overrideStartDate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(PeriodicSchedule beanToCopy)
		{
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.frequency_Renamed = beanToCopy.Frequency;
		  this.businessDayAdjustment_Renamed = beanToCopy.BusinessDayAdjustment;
		  this.startDateBusinessDayAdjustment_Renamed = beanToCopy.startDateBusinessDayAdjustment;
		  this.endDateBusinessDayAdjustment_Renamed = beanToCopy.endDateBusinessDayAdjustment;
		  this.stubConvention_Renamed = beanToCopy.stubConvention;
		  this.rollConvention_Renamed = beanToCopy.rollConvention;
		  this.firstRegularStartDate_Renamed = beanToCopy.firstRegularStartDate;
		  this.lastRegularEndDate_Renamed = beanToCopy.lastRegularEndDate;
		  this.overrideStartDate_Renamed = beanToCopy.overrideStartDate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 429197561: // startDateBusinessDayAdjustment
			  return startDateBusinessDayAdjustment_Renamed;
			case -734327136: // endDateBusinessDayAdjustment
			  return endDateBusinessDayAdjustment_Renamed;
			case -31408449: // stubConvention
			  return stubConvention_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 2011803076: // firstRegularStartDate
			  return firstRegularStartDate_Renamed;
			case -1540679645: // lastRegularEndDate
			  return lastRegularEndDate_Renamed;
			case -599936828: // overrideStartDate
			  return overrideStartDate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case -70023844: // frequency
			  this.frequency_Renamed = (Frequency) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case 429197561: // startDateBusinessDayAdjustment
			  this.startDateBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -734327136: // endDateBusinessDayAdjustment
			  this.endDateBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -31408449: // stubConvention
			  this.stubConvention_Renamed = (StubConvention) newValue;
			  break;
			case -10223666: // rollConvention
			  this.rollConvention_Renamed = (RollConvention) newValue;
			  break;
			case 2011803076: // firstRegularStartDate
			  this.firstRegularStartDate_Renamed = (LocalDate) newValue;
			  break;
			case -1540679645: // lastRegularEndDate
			  this.lastRegularEndDate_Renamed = (LocalDate) newValue;
			  break;
			case -599936828: // overrideStartDate
			  this.overrideStartDate_Renamed = (AdjustableDate) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override PeriodicSchedule build()
		{
		  return new PeriodicSchedule(startDate_Renamed, endDate_Renamed, frequency_Renamed, businessDayAdjustment_Renamed, startDateBusinessDayAdjustment_Renamed, endDateBusinessDayAdjustment_Renamed, stubConvention_Renamed, rollConvention_Renamed, firstRegularStartDate_Renamed, lastRegularEndDate_Renamed, overrideStartDate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the start date, which is the start of the first schedule period.
		/// <para>
		/// This is the start date of the schedule.
		/// It is is unadjusted and as such might be a weekend or holiday.
		/// Any applicable business day adjustment will be applied when creating the schedule.
		/// This is also known as the unadjusted effective date.
		/// </para>
		/// <para>
		/// In most cases, the start date of a financial instrument is just after the trade date,
		/// such as two business days later. However, the start date of a schedule is permitted
		/// to be any date, which includes dates before or after the trade date.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date, which is the end of the last schedule period.
		/// <para>
		/// This is the end date of the schedule.
		/// It is is unadjusted and as such might be a weekend or holiday.
		/// Any applicable business day adjustment will be applied when creating the schedule.
		/// This is also known as the unadjusted maturity date or unadjusted termination date.
		/// This date must be after the start date.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the regular periodic frequency to use.
		/// <para>
		/// Most dates are calculated using a regular periodic frequency, such as every 3 months.
		/// The actual day-of-month or day-of-week is selected using the roll and stub conventions.
		/// </para>
		/// </summary>
		/// <param name="frequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder frequency(Frequency frequency)
		{
		  JodaBeanUtils.notNull(frequency, "frequency");
		  this.frequency_Renamed = frequency;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply.
		/// <para>
		/// Each date in the calculated schedule is determined without taking into account weekends and holidays.
		/// The adjustment specified here is used to convert those dates to valid business days.
		/// </para>
		/// <para>
		/// The start date and end date may have their own business day adjustment rules.
		/// If those are not present, then this adjustment is used instead.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the optional business day adjustment to apply to the start date.
		/// <para>
		/// The start date property is an unadjusted date and as such might be a weekend or holiday.
		/// The adjustment specified here is used to convert the start date to a valid business day.
		/// </para>
		/// <para>
		/// If this property is not present, the standard {@code businessDayAdjustment} property is used instead.
		/// </para>
		/// </summary>
		/// <param name="startDateBusinessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDateBusinessDayAdjustment(BusinessDayAdjustment startDateBusinessDayAdjustment)
		{
		  this.startDateBusinessDayAdjustment_Renamed = startDateBusinessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the optional business day adjustment to apply to the end date.
		/// <para>
		/// The end date property is an unadjusted date and as such might be a weekend or holiday.
		/// The adjustment specified here is used to convert the end date to a valid business day.
		/// </para>
		/// <para>
		/// If this property is not present, the standard {@code businessDayAdjustment} property is used instead.
		/// </para>
		/// </summary>
		/// <param name="endDateBusinessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDateBusinessDayAdjustment(BusinessDayAdjustment endDateBusinessDayAdjustment)
		{
		  this.endDateBusinessDayAdjustment_Renamed = endDateBusinessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the optional convention defining how to handle stubs.
		/// <para>
		/// The stub convention is used during schedule construction to determine whether the irregular
		/// remaining period occurs at the start or end of the schedule.
		/// It also determines whether the irregular period is shorter or longer than the regular period.
		/// This property interacts with the "explicit dates" of <seealso cref="PeriodicSchedule#getFirstRegularStartDate()"/>
		/// and <seealso cref="PeriodicSchedule#getLastRegularEndDate()"/>.
		/// </para>
		/// <para>
		/// The convention 'None' may be used to explicitly indicate there are no stubs.
		/// There must be no explicit dates.
		/// This will be validated during schedule construction.
		/// </para>
		/// <para>
		/// The convention 'Both' may be used to explicitly indicate there is both an initial and final stub.
		/// The stubs themselves must be specified using explicit dates.
		/// This will be validated during schedule construction.
		/// </para>
		/// <para>
		/// The conventions 'ShortInitial', 'LongInitial', 'SmartInitial', 'ShortFinal', 'LongFinal'
		/// and 'SmartFinal' are used to indicate the type of stub to be generated.
		/// The exact behavior varies depending on whether there are explicit dates or not:
		/// </para>
		/// <para>
		/// If explicit dates are specified, then the combination of stub convention an explicit date
		/// will be validated during schedule construction. For example, the combination of an explicit dated
		/// initial stub and a stub convention of 'ShortInitial', 'LongInitial' or 'SmartInitial' is valid,
		/// but other stub conventions, such as 'ShortFinal' or 'None' would be invalid.
		/// </para>
		/// <para>
		/// If explicit dates are not specified, then it is not required that a stub is generated.
		/// The convention determines whether to generate dates from the start date forward, or the
		/// end date backwards. Date generation may or may not result in a stub, but if it does then
		/// the stub will be of the correct type.
		/// </para>
		/// <para>
		/// When the stub convention is not present, the generation of stubs is based entirely on
		/// the presence or absence of the explicit dates.
		/// </para>
		/// </summary>
		/// <param name="stubConvention">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder stubConvention(StubConvention stubConvention)
		{
		  this.stubConvention_Renamed = stubConvention;
		  return this;
		}

		/// <summary>
		/// Sets the optional convention defining how to roll dates.
		/// <para>
		/// The schedule periods are determined at the high level by repeatedly adding
		/// the frequency to the start date, or subtracting it from the end date.
		/// The roll convention provides the detailed rule to adjust the day-of-month or day-of-week.
		/// </para>
		/// <para>
		/// During schedule generation, if this is present it will be used to determine the schedule.
		/// If not present, then the roll convention will be implied.
		/// </para>
		/// </summary>
		/// <param name="rollConvention">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rollConvention(RollConvention rollConvention)
		{
		  this.rollConvention_Renamed = rollConvention;
		  return this;
		}

		/// <summary>
		/// Sets the optional start date of the first regular schedule period, which is the end date of the initial stub.
		/// <para>
		/// This is used to identify the boundary date between the initial stub and the first regular schedule period.
		/// </para>
		/// <para>
		/// This is an unadjusted date, and as such it might not be a valid business day.
		/// This date must be on or after 'startDate'.
		/// </para>
		/// <para>
		/// During schedule generation, if this is present it will be used to determine the schedule.
		/// If not present, then the overall schedule start date will be used instead, resulting in no initial stub.
		/// </para>
		/// </summary>
		/// <param name="firstRegularStartDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstRegularStartDate(LocalDate firstRegularStartDate)
		{
		  this.firstRegularStartDate_Renamed = firstRegularStartDate;
		  return this;
		}

		/// <summary>
		/// Sets the optional end date of the last regular schedule period, which is the start date of the final stub.
		/// <para>
		/// This is used to identify the boundary date between the last regular schedule period and the final stub.
		/// </para>
		/// <para>
		/// This is an unadjusted date, and as such it might not be a valid business day.
		/// This date must be after 'startDate' and after 'firstRegularStartDate'.
		/// This date must be on or before 'endDate'.
		/// </para>
		/// <para>
		/// During schedule generation, if this is present it will be used to determine the schedule.
		/// If not present, then the overall schedule end date will be used instead, resulting in no final stub.
		/// </para>
		/// </summary>
		/// <param name="lastRegularEndDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastRegularEndDate(LocalDate lastRegularEndDate)
		{
		  this.lastRegularEndDate_Renamed = lastRegularEndDate;
		  return this;
		}

		/// <summary>
		/// Sets the optional start date of the first schedule period, overriding normal schedule generation.
		/// <para>
		/// This property is rarely used, and is generally needed when accrual starts before the effective date.
		/// If specified, it overrides the start date of the first period once schedule generation has been completed.
		/// Note that all schedule generation rules apply to 'startDate', with this applied as a final step.
		/// This field primarily exists to support the FpML 'firstPeriodStartDate' concept.
		/// </para>
		/// <para>
		/// If a roll convention is explicitly specified and the regular start date does not match it,
		/// then the override will be used when generating regular periods.
		/// </para>
		/// <para>
		/// If set, it should be different to the start date, although this is not validated.
		/// Validation does check that it is before the 'firstRegularStartDate'.
		/// </para>
		/// <para>
		/// During schedule generation, if this is present it will be used to override the start date
		/// of the first generated schedule period.
		/// If not present, then the start of the first period will be the normal start date.
		/// </para>
		/// </summary>
		/// <param name="overrideStartDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder overrideStartDate(AdjustableDate overrideStartDate)
		{
		  this.overrideStartDate_Renamed = overrideStartDate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(384);
		  buf.Append("PeriodicSchedule.Builder{");
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("frequency").Append('=').Append(JodaBeanUtils.ToString(frequency_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("startDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(startDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("endDateBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(endDateBusinessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("stubConvention").Append('=').Append(JodaBeanUtils.ToString(stubConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("firstRegularStartDate").Append('=').Append(JodaBeanUtils.ToString(firstRegularStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("lastRegularEndDate").Append('=').Append(JodaBeanUtils.ToString(lastRegularEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("overrideStartDate").Append('=').Append(JodaBeanUtils.ToString(overrideStartDate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
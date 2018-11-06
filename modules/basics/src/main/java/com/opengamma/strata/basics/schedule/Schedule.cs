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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount_ScheduleInfo = com.opengamma.strata.basics.date.DayCount_ScheduleInfo;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A complete schedule of periods (date ranges), with both unadjusted and adjusted dates.
	/// <para>
	/// The schedule consists of one or more adjacent periods (date ranges).
	/// This is typically used as the basis for financial calculations, such as accrual of interest.
	/// </para>
	/// <para>
	/// It is recommended to create a <seealso cref="Schedule"/> using a <seealso cref="PeriodicSchedule"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class Schedule implements com.opengamma.strata.basics.date.DayCount_ScheduleInfo, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Schedule : DayCount_ScheduleInfo, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<SchedulePeriod> periods;
		private readonly ImmutableList<SchedulePeriod> periods;
	  /// <summary>
	  /// The periodic frequency used when building the schedule.
	  /// <para>
	  /// If the schedule was not built from a regular periodic frequency,
	  /// then the frequency should be a suitable estimate.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final Frequency frequency;
	  private readonly Frequency frequency;
	  /// <summary>
	  /// The roll convention used when building the schedule.
	  /// <para>
	  /// If the schedule was not built from a regular periodic frequency, then the convention should be 'None'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final RollConvention rollConvention;
	  private readonly RollConvention rollConvention;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a 'Term' instance based on a single period.
	  /// <para>
	  /// A 'Term' schedule has one period with a frequency of 'Term'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the single period </param>
	  /// <returns> the merged 'Term' schedule </returns>
	  public static Schedule ofTerm(SchedulePeriod period)
	  {
		ArgChecker.notNull(period, "period");
		return Schedule.builder().periods(ImmutableList.of(period)).frequency(Frequency.TERM).rollConvention(RollConventions.NONE).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of periods in the schedule.
	  /// <para>
	  /// This returns the number of periods, which will be at least one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of periods </returns>
	  public int size()
	  {
		return periods.size();
	  }

	  /// <summary>
	  /// Checks if this schedule represents a single 'Term' period.
	  /// <para>
	  /// A 'Term' schedule has one period and a frequency of 'Term'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is a 'Term' schedule </returns>
	  public bool Term
	  {
		  get
		  {
			return size() == 1 && frequency.Equals(Frequency.TERM);
		  }
	  }

	  /// <summary>
	  /// Checks if this schedule has a single period.
	  /// </summary>
	  /// <returns> true if this is a single period </returns>
	  public bool SinglePeriod
	  {
		  get
		  {
			return size() == 1;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets a schedule period by index.
	  /// <para>
	  /// This returns a period using a zero-based index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the zero-based period index </param>
	  /// <returns> the schedule period </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public SchedulePeriod getPeriod(int index)
	  {
		return periods.get(index);
	  }

	  /// <summary>
	  /// Gets the first schedule period.
	  /// </summary>
	  /// <returns> the first schedule period </returns>
	  public SchedulePeriod FirstPeriod
	  {
		  get
		  {
			return periods.get(0);
		  }
	  }

	  /// <summary>
	  /// Gets the last schedule period.
	  /// </summary>
	  /// <returns> the last schedule period </returns>
	  public SchedulePeriod LastPeriod
	  {
		  get
		  {
			return periods.get(periods.size() - 1);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the schedule.
	  /// <para>
	  /// The first date in the schedule, typically treated as inclusive.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the schedule start date </returns>
	  public override LocalDate StartDate
	  {
		  get
		  {
			return FirstPeriod.StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the end date of the schedule.
	  /// <para>
	  /// The last date in the schedule, typically treated as exclusive.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the schedule end date </returns>
	  public override LocalDate EndDate
	  {
		  get
		  {
			return LastPeriod.EndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted schedule start date </returns>
	  public LocalDate UnadjustedStartDate
	  {
		  get
		  {
			return FirstPeriod.UnadjustedStartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted schedule end date </returns>
	  public LocalDate UnadjustedEndDate
	  {
		  get
		  {
			return LastPeriod.UnadjustedEndDate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial stub if it exists.
	  /// <para>
	  /// There is an initial stub if the first period is a stub and the frequency is not 'Term'.
	  /// </para>
	  /// <para>
	  /// A period will be allocated to one and only one of <seealso cref="#getInitialStub()"/>,
	  /// <seealso cref="#getRegularPeriods()"/> and <seealso cref="#getFinalStub()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the initial stub, empty if no initial stub </returns>
	  public Optional<SchedulePeriod> InitialStub
	  {
		  get
		  {
			return (InitialStub ? FirstPeriod : null);
		  }
	  }

	  // checks if there is an initial stub
	  private bool InitialStub
	  {
		  get
		  {
			return !Term && !FirstPeriod.isRegular(frequency, rollConvention);
		  }
	  }

	  /// <summary>
	  /// Gets the final stub if it exists.
	  /// <para>
	  /// There is a final stub if there is more than one period and the last
	  /// period is a stub.
	  /// </para>
	  /// <para>
	  /// A period will be allocated to one and only one of <seealso cref="#getInitialStub()"/>,
	  /// <seealso cref="#getRegularPeriods()"/> and <seealso cref="#getFinalStub()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the final stub, empty if no final stub </returns>
	  public Optional<SchedulePeriod> FinalStub
	  {
		  get
		  {
			return (FinalStub ? LastPeriod : null);
		  }
	  }

	  // checks if there is a final stub
	  private bool FinalStub
	  {
		  get
		  {
			return !SinglePeriod && !LastPeriod.isRegular(frequency, rollConvention);
		  }
	  }

	  /// <summary>
	  /// Gets the regular schedule periods.
	  /// <para>
	  /// The regular periods exclude any initial or final stub.
	  /// In most cases, the periods returned will be regular, corresponding to the periodic
	  /// frequency and roll convention, however there are cases when this is not true.
	  /// This includes the case where <seealso cref="#isTerm()"/> returns true.
	  /// See <seealso cref="SchedulePeriod#isRegular(Frequency, RollConvention)"/>.
	  /// </para>
	  /// <para>
	  /// A period will be allocated to one and only one of <seealso cref="#getInitialStub()"/>,
	  /// <seealso cref="#getRegularPeriods()"/> and <seealso cref="#getFinalStub()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the non-stub schedule periods </returns>
	  public ImmutableList<SchedulePeriod> RegularPeriods
	  {
		  get
		  {
			if (Term)
			{
			  return periods;
			}
			int startStub = InitialStub ? 1 : 0;
			int endStub = FinalStub ? 1 : 0;
			return (startStub == 0 && endStub == 0 ? periods : periods.subList(startStub, periods.size() - endStub));
		  }
	  }

	  /// <summary>
	  /// Gets the complete list of unadjusted dates.
	  /// <para>
	  /// This returns a list including all the unadjusted period boundary dates.
	  /// This is the same as a list containing the unadjusted start date of the schedule
	  /// followed by the unadjusted end date of each period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the list of unadjusted dates, in order </returns>
	  public ImmutableList<LocalDate> UnadjustedDates
	  {
		  get
		  {
			ImmutableList.Builder<LocalDate> dates = ImmutableList.builder();
			dates.add(UnadjustedStartDate);
			foreach (SchedulePeriod period in periods)
			{
			  dates.add(period.UnadjustedEndDate);
			}
			return dates.build();
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the end of month convention is in use.
	  /// <para>
	  /// If true then when building a schedule, dates will be at the end-of-month if the
	  /// first date in the series is at the end-of-month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the end of month convention is in use </returns>
	  public override bool EndOfMonthConvention
	  {
		  get
		  {
			return rollConvention == RollConventions.EOM;
		  }
	  }

	  /// <summary>
	  /// Finds the period end date given a date in the period.
	  /// <para>
	  /// The first matching period is returned.
	  /// The adjusted start and end dates of each period are used in the comparison.
	  /// The start date is included, the end date is excluded.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to find </param>
	  /// <returns> the end date of the period that includes the specified date </returns>
	  public override LocalDate getPeriodEndDate(LocalDate date)
	  {
		return periods.Where(p => p.contains(date)).Select(p => p.EndDate).First().orElseThrow(() => new System.ArgumentException("Date is not contained in any period"));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Merges this schedule to form a new schedule with a single 'Term' period.
	  /// <para>
	  /// The result will have one period of type 'Term', with dates matching this schedule.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the merged 'Term' schedule </returns>
	  public Schedule mergeToTerm()
	  {
		if (Term)
		{
		  return this;
		}
		SchedulePeriod first = FirstPeriod;
		SchedulePeriod last = LastPeriod;
		return Schedule.ofTerm(SchedulePeriod.of(first.StartDate, last.EndDate, first.UnadjustedStartDate, last.UnadjustedEndDate));
	  }

	  /// <summary>
	  /// Merges this schedule to form a new schedule by combining the schedule periods.
	  /// <para>
	  /// This produces a schedule where some periods are merged together.
	  /// For example, this could be used to convert a 3 monthly schedule into a 6 monthly schedule.
	  /// </para>
	  /// <para>
	  /// The merging is controlled by the group size, which defines the number of periods
	  /// to merge together in the result. For example, to convert a 3 monthly schedule into
	  /// a 6 monthly schedule the group size would be 2 (6 divided by 3).
	  /// </para>
	  /// <para>
	  /// A group size of zero or less will throw an exception.
	  /// A group size of 1 will return this schedule providing that the specified start and end date match.
	  /// A larger group size will return a schedule where each group of regular periods are merged.
	  /// </para>
	  /// <para>
	  /// The specified dates must be one of the dates of this schedule (unadjusted or adjusted).
	  /// All periods of this schedule before the first regular start date, if any, will form a single period in the result.
	  /// All periods of this schedule after the last regular start date, if any, will form a single period in the result.
	  /// If this schedule has an initial or final stub, it may be merged with a regular period as part of the process.
	  /// </para>
	  /// <para>
	  /// For example, a schedule with an initial stub and 5 regular periods can be grouped by 2 if the
	  /// specified {@code firstRegularStartDate} equals the end of the first regular period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupSize">  the group size </param>
	  /// <param name="firstRegularStartDate">  the unadjusted start date of the first regular payment period </param>
	  /// <param name="lastRegularEndDate">  the unadjusted end date of the last regular payment period </param>
	  /// <returns> the merged schedule </returns>
	  /// <exception cref="IllegalArgumentException"> if the group size is zero or less </exception>
	  /// <exception cref="ScheduleException"> if the merged schedule cannot be created because the dates don't
	  ///   match this schedule or the regular periods don't match the grouping size </exception>
	  public Schedule merge(int groupSize, LocalDate firstRegularStartDate, LocalDate lastRegularEndDate)
	  {
		ArgChecker.notNegativeOrZero(groupSize, "groupSize");
		ArgChecker.inOrderOrEqual(firstRegularStartDate, lastRegularEndDate, "firstRegularStartDate", "lastRegularEndDate");
		if (SinglePeriod || groupSize == 1)
		{
		  return this;
		}
		// determine stubs and regular
		int startRegularIndex = -1;
		int endRegularIndex = -1;
		for (int i = 0; i < size(); i++)
		{
		  SchedulePeriod period = periods.get(i);
		  if (period.UnadjustedStartDate.Equals(firstRegularStartDate) || period.StartDate.Equals(firstRegularStartDate))
		  {
			startRegularIndex = i;
		  }
		  if (period.UnadjustedEndDate.Equals(lastRegularEndDate) || period.EndDate.Equals(lastRegularEndDate))
		  {
			endRegularIndex = i + 1;
		  }
		}
		if (startRegularIndex < 0)
		{
		  throw new ScheduleException("Unable to merge schedule, firstRegularStartDate {} does not match any date in the underlying schedule {}", firstRegularStartDate, UnadjustedDates);
		}
		if (endRegularIndex < 0)
		{
		  throw new ScheduleException("Unable to merge schedule, lastRegularEndDate {} does not match any date in the underlying schedule {}", lastRegularEndDate, UnadjustedDates);
		}
		int numberRegular = endRegularIndex - startRegularIndex;
		if ((numberRegular % groupSize) != 0)
		{
		  Period newFrequency = frequency.Period.multipliedBy(groupSize);
		  throw new ScheduleException("Unable to merge schedule, firstRegularStartDate {} and lastRegularEndDate {} cannot be used to " + "create regular periods of frequency '{}'", firstRegularStartDate, lastRegularEndDate, newFrequency);
		}
		IList<SchedulePeriod> newSchedule = new List<SchedulePeriod>();
		if (startRegularIndex > 0)
		{
		  newSchedule.Add(createSchedulePeriod(periods.subList(0, startRegularIndex)));
		}
		for (int i = startRegularIndex; i < endRegularIndex; i += groupSize)
		{
		  newSchedule.Add(createSchedulePeriod(periods.subList(i, i + groupSize)));
		}
		if (endRegularIndex < periods.size())
		{
		  newSchedule.Add(createSchedulePeriod(periods.subList(endRegularIndex, periods.size())));
		}
		// build schedule
		return Schedule.builder().periods(newSchedule).frequency(Frequency.of(frequency.Period.multipliedBy(groupSize))).rollConvention(rollConvention).build();
	  }

	  /// <summary>
	  /// Merges this schedule to form a new schedule by combining the regular schedule periods.
	  /// <para>
	  /// This produces a schedule where some periods are merged together.
	  /// For example, this could be used to convert a 3 monthly schedule into a 6 monthly schedule.
	  /// </para>
	  /// <para>
	  /// The merging is controlled by the group size, which defines the number of periods
	  /// to merge together in the result. For example, to convert a 3 monthly schedule into
	  /// a 6 monthly schedule the group size would be 2 (6 divided by 3).
	  /// </para>
	  /// <para>
	  /// A group size of zero or less will throw an exception.
	  /// A group size of 1 will return this schedule.
	  /// A larger group size will return a schedule where each group of regular periods are merged.
	  /// The roll flag is used to determine the direction in which grouping occurs.
	  /// </para>
	  /// <para>
	  /// Any existing stub periods are considered to be special, and are not merged.
	  /// Even if the grouping results in an excess period, such as 10 periods with a group size
	  /// of 3, the excess period will not be merged with a stub.
	  /// </para>
	  /// <para>
	  /// If this period is a 'Term' period, this schedule is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="groupSize">  the group size </param>
	  /// <param name="rollForwards">  whether to roll forwards (true) or backwards (false) </param>
	  /// <returns> the merged schedule </returns>
	  /// <exception cref="IllegalArgumentException"> if the group size is zero or less </exception>
	  public Schedule mergeRegular(int groupSize, bool rollForwards)
	  {
		ArgChecker.notNegativeOrZero(groupSize, "groupSize");
		if (SinglePeriod || groupSize == 1)
		{
		  return this;
		}
		IList<SchedulePeriod> newSchedule = new List<SchedulePeriod>();
		// retain initial stub
		Optional<SchedulePeriod> initialStub = InitialStub;
		if (initialStub.Present)
		{
		  newSchedule.Add(initialStub.get());
		}
		// merge regular, handling stubs via min/max
		ImmutableList<SchedulePeriod> regularPeriods = RegularPeriods;
		int regularSize = regularPeriods.size();
		int remainder = regularSize % groupSize;
		int startIndex = (rollForwards || remainder == 0 ? 0 : -(groupSize - remainder));
		for (int i = startIndex; i < regularSize; i += groupSize)
		{
		  int from = Math.Max(i, 0);
		  int to = Math.Min(i + groupSize, regularSize);
		  newSchedule.Add(createSchedulePeriod(regularPeriods.subList(from, to)));
		}
		// retain final stub
		Optional<SchedulePeriod> finalStub = FinalStub;
		if (finalStub.Present)
		{
		  newSchedule.Add(finalStub.get());
		}
		// build schedule
		return Schedule.builder().periods(newSchedule).frequency(Frequency.of(frequency.Period.multipliedBy(groupSize))).rollConvention(rollConvention).build();
	  }

	  // creates a schedule period
	  private SchedulePeriod createSchedulePeriod(IList<SchedulePeriod> accruals)
	  {
		SchedulePeriod first = accruals[0];
		if (accruals.Count == 1)
		{
		  return first;
		}
		SchedulePeriod last = accruals[accruals.Count - 1];
		return SchedulePeriod.of(first.StartDate, last.EndDate, first.UnadjustedStartDate, last.UnadjustedEndDate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this schedule to a schedule where all the start and end dates are
	  /// adjusted using the specified adjuster.
	  /// <para>
	  /// The result will have the same number of periods, but each start date and
	  /// end date is replaced by the adjusted date as returned by the adjuster.
	  /// The unadjusted start date and unadjusted end date of each period will not be changed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="adjuster">  the adjuster to use </param>
	  /// <returns> the adjusted schedule </returns>
	  public Schedule toAdjusted(DateAdjuster adjuster)
	  {
		// implementation needs to return 'this' if unchanged to optimize downstream code
		bool adjusted = false;
		ImmutableList.Builder<SchedulePeriod> builder = ImmutableList.builder();
		foreach (SchedulePeriod period in periods)
		{
		  SchedulePeriod adjPeriod = period.toAdjusted(adjuster);
		  builder.add(adjPeriod);
		  adjusted |= (adjPeriod != period);
		}
		return adjusted ? new Schedule(builder.build(), frequency, rollConvention) : this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this schedule to a schedule where every adjusted date is reset
	  /// to the unadjusted equivalent.
	  /// <para>
	  /// The result will have the same number of periods, but each start date and
	  /// end date is replaced by the matching unadjusted start or end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the equivalent unadjusted schedule </returns>
	  public Schedule toUnadjusted()
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return toBuilder().periods(periods.Select(p => p.toUnadjusted()).collect(toImmutableList())).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Schedule}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Schedule.Meta meta()
	  {
		return Schedule.Meta.INSTANCE;
	  }

	  static Schedule()
	  {
		MetaBean.register(Schedule.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Schedule.Builder builder()
	  {
		return new Schedule.Builder();
	  }

	  private Schedule(IList<SchedulePeriod> periods, Frequency frequency, RollConvention rollConvention)
	  {
		JodaBeanUtils.notEmpty(periods, "periods");
		JodaBeanUtils.notNull(frequency, "frequency");
		JodaBeanUtils.notNull(rollConvention, "rollConvention");
		this.periods = ImmutableList.copyOf(periods);
		this.frequency = frequency;
		this.rollConvention = rollConvention;
	  }

	  public override Schedule.Meta metaBean()
	  {
		return Schedule.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the schedule periods.
	  /// <para>
	  /// There will be at least one period.
	  /// The periods are ordered from earliest to latest.
	  /// It is intended that each period is adjacent to the next one, however each
	  /// period is independent and non-adjacent periods are allowed.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<SchedulePeriod> Periods
	  {
		  get
		  {
			return periods;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic frequency used when building the schedule.
	  /// <para>
	  /// If the schedule was not built from a regular periodic frequency,
	  /// then the frequency should be a suitable estimate.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override Frequency Frequency
	  {
		  get
		  {
			return frequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the roll convention used when building the schedule.
	  /// <para>
	  /// If the schedule was not built from a regular periodic frequency, then the convention should be 'None'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RollConvention RollConvention
	  {
		  get
		  {
			return rollConvention;
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
		  Schedule other = (Schedule) obj;
		  return JodaBeanUtils.equal(periods, other.periods) && JodaBeanUtils.equal(frequency, other.frequency) && JodaBeanUtils.equal(rollConvention, other.rollConvention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periods);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(frequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("Schedule{");
		buf.Append("periods").Append('=').Append(periods).Append(',').Append(' ');
		buf.Append("frequency").Append('=').Append(frequency).Append(',').Append(' ');
		buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Schedule}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  periods_Renamed = DirectMetaProperty.ofImmutable(this, "periods", typeof(Schedule), (Type) typeof(ImmutableList));
			  frequency_Renamed = DirectMetaProperty.ofImmutable(this, "frequency", typeof(Schedule), typeof(Frequency));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(Schedule), typeof(RollConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "periods", "frequency", "rollConvention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code periods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<SchedulePeriod>> periods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "periods", Schedule.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SchedulePeriod>> periods_Renamed;
		/// <summary>
		/// The meta-property for the {@code frequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> frequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code rollConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RollConvention> rollConvention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "periods", "frequency", "rollConvention");
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
			case -678739246: // periods
			  return periods_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override Schedule.Builder builder()
		{
		  return new Schedule.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Schedule);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code periods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SchedulePeriod>> periods()
		{
		  return periods_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code frequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> frequency()
		{
		  return frequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rollConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RollConvention> rollConvention()
		{
		  return rollConvention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -678739246: // periods
			  return ((Schedule) bean).Periods;
			case -70023844: // frequency
			  return ((Schedule) bean).Frequency;
			case -10223666: // rollConvention
			  return ((Schedule) bean).RollConvention;
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
	  /// The bean-builder for {@code Schedule}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Schedule>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<SchedulePeriod> periods_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency frequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RollConvention rollConvention_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(Schedule beanToCopy)
		{
		  this.periods_Renamed = beanToCopy.Periods;
		  this.frequency_Renamed = beanToCopy.Frequency;
		  this.rollConvention_Renamed = beanToCopy.RollConvention;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -678739246: // periods
			  return periods_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -678739246: // periods
			  this.periods_Renamed = (IList<SchedulePeriod>) newValue;
			  break;
			case -70023844: // frequency
			  this.frequency_Renamed = (Frequency) newValue;
			  break;
			case -10223666: // rollConvention
			  this.rollConvention_Renamed = (RollConvention) newValue;
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

		public override Schedule build()
		{
		  return new Schedule(periods_Renamed, frequency_Renamed, rollConvention_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the schedule periods.
		/// <para>
		/// There will be at least one period.
		/// The periods are ordered from earliest to latest.
		/// It is intended that each period is adjacent to the next one, however each
		/// period is independent and non-adjacent periods are allowed.
		/// </para>
		/// </summary>
		/// <param name="periods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periods(IList<SchedulePeriod> periods)
		{
		  JodaBeanUtils.notEmpty(periods, "periods");
		  this.periods_Renamed = periods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code periods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="periods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periods(params SchedulePeriod[] periods)
		{
		  return this.periods(ImmutableList.copyOf(periods));
		}

		/// <summary>
		/// Sets the periodic frequency used when building the schedule.
		/// <para>
		/// If the schedule was not built from a regular periodic frequency,
		/// then the frequency should be a suitable estimate.
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
		/// Sets the roll convention used when building the schedule.
		/// <para>
		/// If the schedule was not built from a regular periodic frequency, then the convention should be 'None'.
		/// </para>
		/// </summary>
		/// <param name="rollConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rollConvention(RollConvention rollConvention)
		{
		  JodaBeanUtils.notNull(rollConvention, "rollConvention");
		  this.rollConvention_Renamed = rollConvention;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("Schedule.Builder{");
		  buf.Append("periods").Append('=').Append(JodaBeanUtils.ToString(periods_Renamed)).Append(',').Append(' ');
		  buf.Append("frequency").Append('=').Append(JodaBeanUtils.ToString(frequency_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
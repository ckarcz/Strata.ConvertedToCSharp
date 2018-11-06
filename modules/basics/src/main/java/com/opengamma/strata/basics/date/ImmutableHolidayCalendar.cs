using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;
	using SerDeserializer = org.joda.beans.ser.SerDeserializer;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;
	using Iterables = com.google.common.collect.Iterables;
	using Sets = com.google.common.collect.Sets;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An immutable holiday calendar implementation.
	/// <para>
	/// A standard immutable implementation of <seealso cref="HolidayCalendar"/> that stores all
	/// dates that are holidays, plus a list of weekend days.
	/// </para>
	/// <para>
	/// Internally, the class uses a range to determine the range of known holiday dates.
	/// Beyond the range of known holiday dates, weekend days are used to determine business days.
	/// Dates may be queried from year zero to year 10,000.
	/// </para>
	/// <para>
	/// Applications should refer to holidays using <seealso cref="HolidayCalendarId"/>.
	/// The identifier must be <seealso cref="HolidayCalendarId#resolve(ReferenceData) resolved"/>
	/// to a <seealso cref="HolidayCalendar"/> before the holiday data methods can be accessed.
	/// See <seealso cref="HolidayCalendarIds"/> for a standard set of identifiers available in <seealso cref="ReferenceData#standard()"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ImmutableHolidayCalendar implements HolidayCalendar, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableHolidayCalendar : HolidayCalendar, ImmutableBean
	{
	  // optimized implementation of HolidayCalendar
	  // uses an int array where each int represents a month
	  // each bit within the int represents a date, where 0 is a holiday and 1 is a business day
	  // (most logic involves finding business days, finding 1 is easier than finding 0
	  // when using Integer.numberOfTrailingZeros and Integer.numberOfLeadingZeros)
	  // benchmarking showed nextOrSame() and previousOrSame() do not need to be overridden
	  // out-of-range and weekend-only (used in testing) are handled using exceptions to fast-path the common case

	  /// <summary>
	  /// The deserializer, for compatibility.
	  /// </summary>
	  public static readonly SerDeserializer DESERIALIZER = new ImmutableHolidayCalendarDeserializer();

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 2L;

	  /// <summary>
	  /// The identifier, such as 'GBLO'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final HolidayCalendarId id;
	  private readonly HolidayCalendarId id;
	  /// <summary>
	  /// The set of weekend days.
	  /// <para>
	  /// Each date that has a day-of-week matching one of these days is not a business day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "") private final int weekends;
	  private readonly int weekends;
	  /// <summary>
	  /// The start year.
	  /// Used as the base year for the lookup table.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "") private final int startYear;
	  private readonly int startYear;
	  /// <summary>
	  /// The lookup table, where each item represents a month from January of startYear onwards.
	  /// Bits 0 to 31 are used for each day-of-month, where 0 is a holiday and 1 is a business day.
	  /// Trailing bits are set to 0 so they act as holidays, avoiding month length logic.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "") private final int[] lookup;
	  private readonly int[] lookup;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a set of holiday dates and weekend days.
	  /// <para>
	  /// The holiday dates will be extracted into a set with duplicates ignored.
	  /// The minimum supported date for query is the start of the year of the earliest holiday.
	  /// The maximum supported date for query is the end of the year of the latest holiday.
	  /// </para>
	  /// <para>
	  /// The weekend days may both be the same.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="holidays">  the set of holiday dates </param>
	  /// <param name="firstWeekendDay">  the first weekend day </param>
	  /// <param name="secondWeekendDay">  the second weekend day, may be same as first </param>
	  /// <returns> the holiday calendar </returns>
	  public static ImmutableHolidayCalendar of(HolidayCalendarId id, IEnumerable<LocalDate> holidays, DayOfWeek firstWeekendDay, DayOfWeek secondWeekendDay)
	  {

		ImmutableSet<DayOfWeek> weekendDays = Sets.immutableEnumSet(firstWeekendDay, secondWeekendDay);
		return of(id, ImmutableSortedSet.copyOf(holidays), weekendDays);
	  }

	  /// <summary>
	  /// Obtains an instance from a set of holiday dates and weekend days.
	  /// <para>
	  /// The holiday dates will be extracted into a set with duplicates ignored.
	  /// The minimum supported date for query is the start of the year of the earliest holiday.
	  /// The maximum supported date for query is the end of the year of the latest holiday.
	  /// </para>
	  /// <para>
	  /// The weekend days may be empty, in which case the holiday dates should contain any weekends.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <param name="holidays">  the set of holiday dates </param>
	  /// <param name="weekendDays">  the days that define the weekend, if empty then weekends are treated as business days </param>
	  /// <returns> the holiday calendar </returns>
	  public static ImmutableHolidayCalendar of(HolidayCalendarId id, IEnumerable<LocalDate> holidays, IEnumerable<DayOfWeek> weekendDays)
	  {

		return of(id, ImmutableSortedSet.copyOf(holidays), Sets.immutableEnumSet(weekendDays));
	  }

	  /// <summary>
	  /// Obtains a combined holiday calendar instance.
	  /// <para>
	  /// This combines the two input calendars.
	  /// It is intended for up-front occasional use rather than continuous use, as it can be relatively slow.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cal1">  the first calendar </param>
	  /// <param name="cal2">  the second calendar </param>
	  /// <returns> the combined calendar </returns>
	  public static ImmutableHolidayCalendar combined(ImmutableHolidayCalendar cal1, ImmutableHolidayCalendar cal2)
	  {
		// do not override combinedWith(), as this is too slow
		if (cal1 == cal2)
		{
		  return ArgChecker.notNull(cal1, "cal1");
		}
		HolidayCalendarId newId = cal1.id.combinedWith(cal2.id);

		// use slow version if lookup arrays do not overlap
		int endYear1 = cal1.startYear + cal1.lookup.Length / 12;
		int endYear2 = cal2.startYear + cal2.lookup.Length / 12;
		if (endYear1 < cal2.startYear || endYear2 < cal1.startYear)
		{
		  ImmutableSortedSet<LocalDate> newHolidays = ImmutableSortedSet.copyOf(Iterables.concat(cal1.Holidays, cal2.Holidays));
		  ImmutableSet<DayOfWeek> newWeekends = ImmutableSet.copyOf(Iterables.concat(cal1.WeekendDays, cal2.WeekendDays));
		  return of(newId, newHolidays, newWeekends);
		}

		// merge calendars using bitwise operations
		// figure out which has the lower start year and use that as the base
		bool cal1Lower = cal1.startYear <= cal2.startYear;
		int[] lookup1 = cal1Lower ? cal1.lookup : cal2.lookup;
		int[] lookup2 = cal1Lower ? cal2.lookup : cal1.lookup;
		int newStartYear = cal1Lower ? cal1.startYear : cal2.startYear;
		int otherStartYear = cal1Lower ? cal2.startYear : cal1.startYear;
		// copy base array and map data from the other on top
		int newSize = Math.Max(lookup1.Length, lookup2.Length + (otherStartYear - newStartYear) * 12);
		int offset = (otherStartYear - newStartYear) * 12;
		int[] newLookup = Arrays.copyOf(lookup1, newSize);
		for (int i = 0; i < lookup2.Length; i++)
		{
		  newLookup[i + offset] &= lookup2[i]; // use & because 1 = business day (not holiday)
		}
		int newWeekends = cal1.weekends | cal2.weekends; // use | because 1 = weekend day
		return new ImmutableHolidayCalendar(newId, newWeekends, newStartYear, newLookup, false);
	  }

	  // creates an instance calculating the supported range
	  internal static ImmutableHolidayCalendar of(HolidayCalendarId id, SortedSet<LocalDate> holidays, ISet<DayOfWeek> weekendDays)
	  {
		ArgChecker.notNull(id, "id");
		ArgChecker.notNull(holidays, "holidays");
		ArgChecker.notNull(weekendDays, "weekendDays");
		int weekends = weekendDays.Select(dow => 1 << (dow.Value - 1)).Sum();
		int startYear = 0;
		int[] lookup = new int[0];
		if (holidays.Count == 0)
		{
		  // special case where no holiday dates are specified
		  startYear = 0;
		  lookup = new int[0];
		}
		else
		{
		  // normal case where holidays are specified
		  startYear = holidays.Min.Year;
		  int endYearExclusive = holidays.Max.Year + 1;
		  lookup = buildLookupArray(holidays, weekendDays, startYear, endYearExclusive);
		}
		return new ImmutableHolidayCalendar(id, weekends, startYear, lookup);
	  }

	  // create and populate the int[] lookup
	  // use 1 for business days and 0 for holidays
	  private static int[] buildLookupArray(IEnumerable<LocalDate> holidays, IEnumerable<DayOfWeek> weekendDays, int startYear, int endYearExclusive)
	  {

		// array that has one entry for each month
		int[] array = new int[(endYearExclusive - startYear) * 12];
		// loop through all months to handle end-of-month and weekends
		LocalDate firstOfMonth = LocalDate.of(startYear, 1, 1);
		for (int i = 0; i < array.Length; i++)
		{
		  int monthLen = firstOfMonth.lengthOfMonth();
		  // set each valid day-of-month to be a business day
		  // the bits for days beyond the end-of-month will be unset and thus treated as non-business days
		  // the minus one part converts a single set bit into each lower bit being set
		  array[i] = (1 << monthLen) - 1;
		  // unset the bits associated with a weekend
		  // can unset across whole month using repeating pattern of 7 bits
		  // just need to find the offset between the weekend and the day-of-week of the 1st of the month
		  foreach (DayOfWeek weekendDow in weekendDays)
		  {
			int daysDiff = weekendDow.Value - firstOfMonth.DayOfWeek.Value;
			int offset = (daysDiff < 0 ? daysDiff + 7 : daysDiff);
			array[i] &= ~(0b10000001000000100000010000001 << offset);
		  }
		  firstOfMonth = firstOfMonth.plusMonths(1);
		}
		// unset the bit associated with each holiday date
		foreach (LocalDate date in holidays)
		{
		  int index = (date.Year - startYear) * 12 + date.MonthValue - 1;
		  array[index] &= ~(1 << (date.DayOfMonth - 1));
		}
		return array;
	  }

	  //-------------------------------------------------------------------------
	  // writes the binary format
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: void writeExternal(java.io.DataOutput out) throws java.io.IOException
	  internal void writeExternal(DataOutput @out)
	  {
		@out.writeUTF(id.Name);
		@out.writeShort(weekends); // using short rather than byte helps align data with 4 char identifiers
		@out.writeShort(startYear);
		@out.writeShort(lookup.Length);
		for (int i = 0; i < lookup.Length; i++)
		{
		  @out.writeInt(lookup[i]);
		}
	  }

	  // reads the binary format
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static ImmutableHolidayCalendar readExternal(java.io.DataInput in) throws java.io.IOException
	  internal static ImmutableHolidayCalendar readExternal(DataInput @in)
	  {
		string id = @in.readUTF();
		int weekendDays = @in.readShort();
		int startYear = @in.readShort();
		int lookupSize = @in.readShort();
		// this logic was found to be the fastest way to deserialize the int array
		sbyte[] bytes = new sbyte[lookupSize * 4];
		int[] lookup = new int[lookupSize];
		@in.readFully(bytes);
		int offset = 0;
		for (int i = 0; i < lookupSize; i++)
		{
		  lookup[i] = ((bytes[offset++] & 0xFF) << 24) | ((bytes[offset++] & 0xFF) << 16) | ((bytes[offset++] & 0xFF) << 8) | (bytes[offset++] & 0xFF);
		}
		return new ImmutableHolidayCalendar(HolidayCalendarId.of(id), weekendDays, startYear, lookup, false);
	  }

	  //-------------------------------------------------------------------------
	  // creates an instance, not cloning the lookup
	  internal ImmutableHolidayCalendar(HolidayCalendarId id, int weekendDays, int startYear, int[] lookup, bool flag)
	  {
		this.id = ArgChecker.notNull(id, "id");
		this.weekends = weekendDays;
		this.startYear = startYear;
		this.lookup = ArgChecker.notNull(lookup, "lookup");
	  }

	  //-------------------------------------------------------------------------
	  // returns the holidays as a set
	  internal ImmutableSortedSet<LocalDate> Holidays
	  {
		  get
		  {
			if (startYear == 0)
			{
			  return ImmutableSortedSet.of();
			}
			ImmutableSortedSet.Builder<LocalDate> builder = ImmutableSortedSet.naturalOrder();
			LocalDate firstOfMonth = LocalDate.of(startYear, 1, 1);
			for (int i = 0; i < lookup.Length; i++)
			{
			  int monthData = lookup[i];
			  int monthLen = firstOfMonth.lengthOfMonth();
			  int dow0 = firstOfMonth.DayOfWeek.ordinal();
			  int bit = 1;
			  for (int j = 0; j < monthLen; j++)
			  {
				// if it is a holiday and not a weekend, then add the date
				if ((monthData & bit) == 0 && (weekends & (1 << dow0)) == 0)
				{
				  builder.add(firstOfMonth.withDayOfMonth(j + 1));
				}
				dow0 = (dow0 + 1) % 7;
				bit <<= 1;
			  }
			  firstOfMonth = firstOfMonth.plusMonths(1);
			}
			return builder.build();
		  }
	  }

	  // returns the weekend days as a set
	  internal ImmutableSet<DayOfWeek> WeekendDays
	  {
		  get
		  {
			return Stream.of(DayOfWeek.values()).filter(dow => (weekends & (1 << dow.ordinal())) != 0).collect(toImmutableSet());
		  }
	  }

	  //-------------------------------------------------------------------------
	  public bool isHoliday(LocalDate date)
	  {
		try
		{
		  // find data for month
		  int index = (date.Year - startYear) * 12 + date.MonthValue - 1;
		  // check if bit is 1 at zero-based day-of-month
		  return (lookup[index] & (1 << (date.DayOfMonth - 1))) == 0;

		}
		catch (System.IndexOutOfRangeException)
		{
		  return isHolidayOutOfRange(date);
		}
	  }

	  // pulled out to aid hotspot inlining
	  private bool isHolidayOutOfRange(LocalDate date)
	  {
		if (date.Year >= 0 && date.Year < 10000)
		{
		  return (weekends & (1 << date.DayOfWeek.ordinal())) != 0;
		}
		throw new System.ArgumentException("Date is outside the accepted range (year 0000 to 10,000): " + date);
	  }

	  //-------------------------------------------------------------------------
	  public override LocalDate shift(LocalDate date, int amount)
	  {
		try
		{
		  if (amount > 0)
		  {
			// day-of-month: minus one for zero-based day-of-month, plus one to start from next day
			return shiftNext(date.Year, date.MonthValue, date.DayOfMonth, amount);
		  }
		  else if (amount < 0)
		  {
			// day-of-month: minus one to start from previous day
			return shiftPrev(date.Year, date.MonthValue, date.DayOfMonth - 1, amount);
		  }
		  return date;

		}
		catch (System.IndexOutOfRangeException)
		{
		  return shiftOutOfRange(date, amount);
		}
	  }

	  // pulled out to aid hotspot inlining
	  private LocalDate shiftOutOfRange(LocalDate date, int amount)
	  {
		if (date.Year >= 0 && date.Year < 10000)
		{
		  return HolidayCalendar.this.shift(date, amount);
		}
		throw new System.ArgumentException("Date is outside the accepted range (year 0000 to 10,000): " + date);
	  }

	  //-------------------------------------------------------------------------
	  public override LocalDate next(LocalDate date)
	  {
		try
		{
		  // day-of-month: minus one for zero-based day-of-month, plus one to start from next day
		  return shiftNext(date.Year, date.MonthValue, date.DayOfMonth, 1);

		}
		catch (System.IndexOutOfRangeException)
		{
		  return HolidayCalendar.this.next(date);
		}
	  }

	  // shift to a later working day, following nextOrSame semantics
	  // input day-of-month is zero-based
	  private LocalDate shiftNext(int baseYear, int baseMonth, int baseDom0, int amount)
	  {
		// find data for month
		int index = (baseYear - startYear) * 12 + baseMonth - 1;
		int monthData = lookup[index];
		// loop around amount, the number of days to shift by
		// use domOffset to keep track of day-of-month
		int domOffset = baseDom0;
		for (int amt = amount; amt > 0; amt--)
		{
		  // shift to move the target day-of-month into bit-0, removing earlier days
		  int shifted = monthData >> domOffset;
		  // recurse to next month if no more business days in the month
		  if (shifted == 0)
		  {
			return baseMonth == 12 ? shiftNext(baseYear + 1, 1, 0, amt) : shiftNext(baseYear, baseMonth + 1, 0, amt);
		  }
		  // find least significant bit, which is next business day
		  // use JDK numberOfTrailingZeros() method which is mapped to a fast intrinsic
		  domOffset += (Integer.numberOfTrailingZeros(shifted) + 1);
		}
		return LocalDate.of(baseYear, baseMonth, domOffset);
	  }

	  //-------------------------------------------------------------------------
	  public override LocalDate previous(LocalDate date)
	  {
		try
		{
		  // day-of-month: minus one to start from previous day
		  return shiftPrev(date.Year, date.MonthValue, date.DayOfMonth - 1, -1);

		}
		catch (System.IndexOutOfRangeException)
		{
		  return previousOutOfRange(date);
		}
	  }

	  // shift to an earlier working day, following previousOrSame semantics
	  // input day-of-month is one-based and may be zero or negative
	  private LocalDate shiftPrev(int baseYear, int baseMonth, int baseDom, int amount)
	  {
		// find data for month
		int index = (baseYear - startYear) * 12 + baseMonth - 1;
		int monthData = lookup[index];
		// loop around amount, the number of days to shift by
		// use domOffset to keep track of day-of-month
		int domOffset = baseDom;
		for (int amt = amount; amt < 0; amt++)
		{
		  // shift to move the target day-of-month into bit-31, removing later days
		  int shifted = (monthData << (32 - domOffset));
		  // recurse to previous month if no more business days in the month
		  if (shifted == 0 || domOffset <= 0)
		  {
			return baseMonth == 1 ? shiftPrev(baseYear - 1, 12, 31, amt) : shiftPrev(baseYear, baseMonth - 1, 31, amt);
		  }
		  // find most significant bit, which is previous business day
		  // use JDK numberOfLeadingZeros() method which is mapped to a fast intrinsic
		  domOffset -= (Integer.numberOfLeadingZeros(shifted) + 1);
		}
		return LocalDate.of(baseYear, baseMonth, domOffset + 1);
	  }

	  // pulled out to aid hotspot inlining
	  private LocalDate previousOutOfRange(LocalDate date)
	  {
		if (date.Year >= 0 && date.Year < 10000)
		{
		  return HolidayCalendar.this.previous(date);
		}
		throw new System.ArgumentException("Date is outside the accepted range (year 0000 to 10,000): " + date);
	  }

	  //-------------------------------------------------------------------------
	  public override LocalDate nextSameOrLastInMonth(LocalDate date)
	  {
		try
		{
		  // day-of-month: no alteration as method is one-based and same is valid
		  return shiftNextSameLast(date);

		}
		catch (System.IndexOutOfRangeException)
		{
		  return HolidayCalendar.this.nextSameOrLastInMonth(date);
		}
	  }

	  // shift to a later working day, following nextOrSame semantics
	  // falling back to the last business day-of-month to avoid crossing a month boundary
	  // input day-of-month is one-based
	  private LocalDate shiftNextSameLast(LocalDate baseDate)
	  {
		int baseYear = baseDate.Year;
		int baseMonth = baseDate.MonthValue;
		int baseDom = baseDate.DayOfMonth;
		// find data for month
		int index = (baseYear - startYear) * 12 + baseMonth - 1;
		int monthData = lookup[index];
		// shift to move the target day-of-month into bit-0, removing earlier days
		int shifted = monthData >> (baseDom - 1);
		// return last business day-of-month if no more business days in the month
		int dom;
		if (shifted == 0)
		{
		  // need to find the most significant bit, which is the last business day
		  // use JDK numberOfLeadingZeros() method which is mapped to a fast intrinsic
		  int leading = Integer.numberOfLeadingZeros(monthData);
		  dom = 32 - leading;
		}
		else
		{
		  // find least significant bit, which is the next/same business day
		  // use JDK numberOfTrailingZeros() method which is mapped to a fast intrinsic
		  dom = baseDom + Integer.numberOfTrailingZeros(shifted);
		}
		// only one call to LocalDate to aid inlining
		return baseDate.withDayOfMonth(dom);
	  }

	  //-------------------------------------------------------------------------
	  public override bool isLastBusinessDayOfMonth(LocalDate date)
	  {
		try
		{
		  // find data for month
		  int index = (date.Year - startYear) * 12 + date.MonthValue - 1;
		  // shift right, leaving the input date as bit-0 and filling with 0 on the left
		  // if the result is 1, which is all zeroes and a final 1 (...0001) then it is last business day of month
		  return ((int)((uint)lookup[index] >> (date.DayOfMonth - 1))) == 1;

		}
		catch (System.IndexOutOfRangeException)
		{
		  return isLastBusinessDayOfMonthOutOfRange(date);
		}
	  }

	  // pulled out to aid hotspot inlining
	  private bool isLastBusinessDayOfMonthOutOfRange(LocalDate date)
	  {
		if (date.Year >= 0 && date.Year < 10000)
		{
		  return HolidayCalendar.this.isLastBusinessDayOfMonth(date);
		}
		throw new System.ArgumentException("Date is outside the accepted range (year 0000 to 10,000): " + date);
	  }

	  //-------------------------------------------------------------------------
	  public override LocalDate lastBusinessDayOfMonth(LocalDate date)
	  {
		try
		{
		  // find data for month
		  int index = (date.Year - startYear) * 12 + date.MonthValue - 1;
		  // need to find the most significant bit, which is the last business day
		  // use JDK numberOfLeadingZeros() method which is mapped to a fast intrinsic
		  int leading = Integer.numberOfLeadingZeros(lookup[index]);
		  return date.withDayOfMonth(32 - leading);

		}
		catch (System.IndexOutOfRangeException)
		{
		  return lastBusinessDayOfMonthOutOfRange(date);
		}
	  }

	  // pulled out to aid hotspot inlining
	  private LocalDate lastBusinessDayOfMonthOutOfRange(LocalDate date)
	  {
		if (date.Year >= 0 && date.Year < 10000)
		{
		  return HolidayCalendar.this.lastBusinessDayOfMonth(date);
		}
		throw new System.ArgumentException("Date is outside the accepted range (year 0000 to 10,000): " + date);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is ImmutableHolidayCalendar)
		{
		  return id.Equals(((ImmutableHolidayCalendar) obj).id);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return id.GetHashCode();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the name of the calendar.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		return "HolidayCalendar[" + Name + ']';
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableHolidayCalendar}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableHolidayCalendar.Meta meta()
	  {
		return ImmutableHolidayCalendar.Meta.INSTANCE;
	  }

	  static ImmutableHolidayCalendar()
	  {
		MetaBean.register(ImmutableHolidayCalendar.Meta.INSTANCE);
	  }

	  private ImmutableHolidayCalendar(HolidayCalendarId id, int weekends, int startYear, int[] lookup)
	  {
		JodaBeanUtils.notNull(id, "id");
		JodaBeanUtils.notNull(lookup, "lookup");
		this.id = id;
		this.weekends = weekends;
		this.startYear = startYear;
		this.lookup = lookup.Clone();
	  }

	  public override ImmutableHolidayCalendar.Meta metaBean()
	  {
		return ImmutableHolidayCalendar.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier, such as 'GBLO'. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public HolidayCalendarId Id
	  {
		  get
		  {
			return id;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableHolidayCalendar}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  id_Renamed = DirectMetaProperty.ofImmutable(this, "id", typeof(ImmutableHolidayCalendar), typeof(HolidayCalendarId));
			  weekends_Renamed = DirectMetaProperty.ofImmutable(this, "weekends", typeof(ImmutableHolidayCalendar), Integer.TYPE);
			  startYear_Renamed = DirectMetaProperty.ofImmutable(this, "startYear", typeof(ImmutableHolidayCalendar), Integer.TYPE);
			  lookup_Renamed = DirectMetaProperty.ofImmutable(this, "lookup", typeof(ImmutableHolidayCalendar), typeof(int[]));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "id", "weekends", "startYear", "lookup");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code id} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HolidayCalendarId> id_Renamed;
		/// <summary>
		/// The meta-property for the {@code weekends} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> weekends_Renamed;
		/// <summary>
		/// The meta-property for the {@code startYear} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> startYear_Renamed;
		/// <summary>
		/// The meta-property for the {@code lookup} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int[]> lookup_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "id", "weekends", "startYear", "lookup");
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
			case 3355: // id
			  return id_Renamed;
			case -621930260: // weekends
			  return weekends_Renamed;
			case -2129150017: // startYear
			  return startYear_Renamed;
			case -1097094790: // lookup
			  return lookup_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ImmutableHolidayCalendar> builder()
		public override BeanBuilder<ImmutableHolidayCalendar> builder()
		{
		  return new ImmutableHolidayCalendar.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableHolidayCalendar);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code id} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HolidayCalendarId> id()
		{
		  return id_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code weekends} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> weekends()
		{
		  return weekends_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startYear} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> startYear()
		{
		  return startYear_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lookup} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int[]> lookup()
		{
		  return lookup_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3355: // id
			  return ((ImmutableHolidayCalendar) bean).Id;
			case -621930260: // weekends
			  return ((ImmutableHolidayCalendar) bean).weekends;
			case -2129150017: // startYear
			  return ((ImmutableHolidayCalendar) bean).startYear;
			case -1097094790: // lookup
			  return ((ImmutableHolidayCalendar) bean).lookup;
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
	  /// The bean-builder for {@code ImmutableHolidayCalendar}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ImmutableHolidayCalendar>
	  {

		internal HolidayCalendarId id;
		internal int weekends;
		internal int startYear;
		internal int[] lookup;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3355: // id
			  return id;
			case -621930260: // weekends
			  return weekends;
			case -2129150017: // startYear
			  return startYear;
			case -1097094790: // lookup
			  return lookup;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3355: // id
			  this.id = (HolidayCalendarId) newValue;
			  break;
			case -621930260: // weekends
			  this.weekends = (int?) newValue.Value;
			  break;
			case -2129150017: // startYear
			  this.startYear = (int?) newValue.Value;
			  break;
			case -1097094790: // lookup
			  this.lookup = (int[]) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ImmutableHolidayCalendar build()
		{
		  return new ImmutableHolidayCalendar(id, weekends, startYear, lookup);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableHolidayCalendar.Builder{");
		  buf.Append("id").Append('=').Append(JodaBeanUtils.ToString(id)).Append(',').Append(' ');
		  buf.Append("weekends").Append('=').Append(JodaBeanUtils.ToString(weekends)).Append(',').Append(' ');
		  buf.Append("startYear").Append('=').Append(JodaBeanUtils.ToString(startYear)).Append(',').Append(' ');
		  buf.Append("lookup").Append('=').Append(JodaBeanUtils.ToString(lookup));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
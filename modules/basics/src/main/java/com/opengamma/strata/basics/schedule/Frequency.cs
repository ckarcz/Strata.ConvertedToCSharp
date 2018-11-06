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
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A periodic frequency used by financial products that have a specific event every so often.
	/// <para>
	/// Frequency is primarily intended to be used to subdivide events within a year.
	/// </para>
	/// <para>
	/// A frequency is allowed to be any non-negative period of days, weeks, month or years.
	/// This class provides constants for common frequencies which are best used by static import.
	/// </para>
	/// <para>
	/// A special value, 'Term', is provided for when there are no subdivisions of the entire term.
	/// This is also know as 'zero-coupon' or 'once'. It is represented using the period 10,000 years,
	/// which allows addition/subtraction to work, producing a date after the end of the term.
	/// </para>
	/// <para>
	/// Each frequency is based on a <seealso cref="Period"/>. The months and years of the period are not normalized,
	/// thus it is possible to have a frequency of 12 months and a different one of 1 year.
	/// When used, standard date addition rules apply, thus there is no difference between them.
	/// Call <seealso cref="#normalized()"/> to apply normalization.
	/// </para>
	/// <para>
	/// The periodic frequency is often expressed as a number of events per year.
	/// The <seealso cref="#eventsPerYear()"/> method can be used to obtain this for common frequencies.
	/// 
	/// <h4>Usage</h4>
	/// {@code Frequency} implements {@code TemporalAmount} allowing it to be directly added to a date:
	/// <pre>
	///  LocalDate later = baseDate.plus(frequency);
	/// </pre>
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class Frequency : TemporalAmount
	{

	  /// <summary>
	  /// Serialization version.
	  /// </summary>
	  private const long serialVersionUID = 1;
	  /// <summary>
	  /// The artificial maximum length of a normal tenor in years.
	  /// </summary>
	  private const int MAX_YEARS = 1_000;
	  /// <summary>
	  /// The artificial maximum length of a normal tenor in months.
	  /// </summary>
	  private static readonly int MAX_MONTHS = MAX_YEARS * 12;
	  /// <summary>
	  /// The artificial length in years of the 'Term' frequency.
	  /// </summary>
	  private const int TERM_YEARS = 10_000;

	  /// <summary>
	  /// A periodic frequency of one day.
	  /// Also known as daily.
	  /// There are considered to be 364 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P1D = ofDays(1);
	  /// <summary>
	  /// A periodic frequency of 1 week (7 days).
	  /// Also known as weekly.
	  /// There are considered to be 52 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P1W = new Frequency(Period.ofWeeks(1), "P1W");
	  /// <summary>
	  /// A periodic frequency of 2 weeks (14 days).
	  /// Also known as bi-weekly.
	  /// There are considered to be 26 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P2W = new Frequency(Period.ofWeeks(2), "P2W");
	  /// <summary>
	  /// A periodic frequency of 4 weeks (28 days).
	  /// Also known as lunar.
	  /// There are considered to be 13 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P4W = new Frequency(Period.ofWeeks(4), "P4W");
	  /// <summary>
	  /// A periodic frequency of 13 weeks (91 days).
	  /// There are considered to be 4 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P13W = new Frequency(Period.ofWeeks(13), "P13W");
	  /// <summary>
	  /// A periodic frequency of 26 weeks (182 days).
	  /// There are considered to be 2 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P26W = new Frequency(Period.ofWeeks(26), "P26W");
	  /// <summary>
	  /// A periodic frequency of 52 weeks (364 days).
	  /// There is considered to be 1 event per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P52W = new Frequency(Period.ofWeeks(52), "P52W");
	  /// <summary>
	  /// A periodic frequency of 1 month.
	  /// Also known as monthly.
	  /// There are 12 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P1M = new Frequency(Period.ofMonths(1));
	  /// <summary>
	  /// A periodic frequency of 2 months.
	  /// Also known as bi-monthly.
	  /// There are 6 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P2M = new Frequency(Period.ofMonths(2));
	  /// <summary>
	  /// A periodic frequency of 3 months.
	  /// Also known as quarterly.
	  /// There are 4 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P3M = new Frequency(Period.ofMonths(3));
	  /// <summary>
	  /// A periodic frequency of 4 months.
	  /// There are 3 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P4M = new Frequency(Period.ofMonths(4));
	  /// <summary>
	  /// A periodic frequency of 6 months.
	  /// Also known as semi-annual.
	  /// There are 2 events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P6M = new Frequency(Period.ofMonths(6));
	  /// <summary>
	  /// A periodic frequency of 12 months (1 year).
	  /// Also known as annual.
	  /// There is 1 event per year with this frequency.
	  /// </summary>
	  public static readonly Frequency P12M = new Frequency(Period.ofMonths(12));
	  /// <summary>
	  /// A periodic frequency matching the term.
	  /// Also known as zero-coupon.
	  /// This is represented using the period 10,000 years.
	  /// There are no events per year with this frequency.
	  /// </summary>
	  public static readonly Frequency TERM = new Frequency(Period.ofYears(TERM_YEARS), "Term");

	  /// <summary>
	  /// The period of the frequency.
	  /// </summary>
	  private readonly Period period;
	  /// <summary>
	  /// The name of the frequency.
	  /// </summary>
	  private readonly string name;
	  /// <summary>
	  /// The number of events per year.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  [NonSerialized]
	  private readonly int eventsPerYear_Renamed;
	  /// <summary>
	  /// The number of events per year.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  [NonSerialized]
	  private readonly double eventsPerYearEstimate_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a {@code Period}.
	  /// <para>
	  /// The period normally consists of either days and weeks, or months and years.
	  /// It must also be positive and non-zero.
	  /// </para>
	  /// <para>
	  /// If the number of days is an exact multiple of 7 it will be converted to weeks.
	  /// Months are not normalized into years.
	  /// </para>
	  /// <para>
	  /// The maximum tenor length is 1,000 years.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to convert to a periodic frequency </param>
	  /// <returns> the periodic frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if the period is negative, zero or too large </exception>
	  public static Frequency of(Period period)
	  {
		ArgChecker.notNull(period, "period");
		int days = period.Days;
		long months = period.toTotalMonths();
		if (months == 0 && days != 0)
		{
		  return ofDays(days);
		}
		if (months > MAX_MONTHS)
		{
		  throw new System.ArgumentException("Period must not exceed 1000 years");
		}
		return new Frequency(period);
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of days.
	  /// <para>
	  /// If the number of days is an exact multiple of 7 it will be converted to weeks.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="days">  the number of days </param>
	  /// <returns> the periodic frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if days is negative or zero </exception>
	  public static Frequency ofDays(int days)
	  {
		if (days % 7 == 0)
		{
		  return ofWeeks(days / 7);
		}
		return new Frequency(Period.ofDays(days));
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of weeks.
	  /// </summary>
	  /// <param name="weeks">  the number of weeks </param>
	  /// <returns> the periodic frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if weeks is negative or zero </exception>
	  public static Frequency ofWeeks(int weeks)
	  {
		switch (weeks)
		{
		  case 1:
			return P1W;
		  case 2:
			return P2W;
		  case 4:
			return P4W;
		  case 13:
			return P13W;
		  case 26:
			return P26W;
		  case 52:
			return P52W;
		  default:
			return new Frequency(Period.ofWeeks(weeks), "P" + weeks + "W");
		}
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of months.
	  /// <para>
	  /// Months are not normalized into years.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="months">  the number of months </param>
	  /// <returns> the periodic frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if months is negative, zero or over 12,000 </exception>
	  public static Frequency ofMonths(int months)
	  {
		switch (months)
		{
		  case 1:
			return P1M;
		  case 2:
			return P2M;
		  case 3:
			return P3M;
		  case 4:
			return P4M;
		  case 6:
			return P6M;
		  case 12:
			return P12M;
		  default:
			if (months > MAX_MONTHS)
			{
			  throw new System.ArgumentException(maxMonthMsg());
			}
			return new Frequency(Period.ofMonths(months));
		}
	  }

	  // extracted to aid inlining
	  private static string maxMonthMsg()
	  {
		DecimalFormat formatter = new DecimalFormat("#,###", new DecimalFormatSymbols(Locale.ENGLISH));
		return "Months must not exceed " + formatter.format(MAX_MONTHS);
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of years.
	  /// </summary>
	  /// <param name="years">  the number of years </param>
	  /// <returns> the periodic frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if years is negative, zero or over 1,000 </exception>
	  public static Frequency ofYears(int years)
	  {
		if (years > MAX_YEARS)
		{
		  throw new System.ArgumentException(maxYearMsg());
		}
		return new Frequency(Period.ofYears(years));
	  }

	  // extracted to aid inlining
	  private static string maxYearMsg()
	  {
		DecimalFormat formatter = new DecimalFormat("#,###", new DecimalFormatSymbols(Locale.ENGLISH));
		return "Years must not exceed " + formatter.format(MAX_YEARS);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a formatted string representing the frequency.
	  /// <para>
	  /// The format can either be based on ISO-8601, such as 'P3M'
	  /// or without the 'P' prefix e.g. '2W'.
	  /// </para>
	  /// <para>
	  /// The period must be positive and non-zero.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="toParse">  the string representing the frequency </param>
	  /// <returns> the frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if the frequency cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static Frequency parse(String toParse)
	  public static Frequency parse(string toParse)
	  {
		ArgChecker.notNull(toParse, "toParse");
		if (toParse.Equals("Term", StringComparison.OrdinalIgnoreCase) || toParse.Equals("0T", StringComparison.OrdinalIgnoreCase) || toParse.Equals("1T", StringComparison.OrdinalIgnoreCase))
		{
		  return TERM;
		}
		string prefixed = toParse.StartsWith("P", StringComparison.Ordinal) ? toParse : "P" + toParse;
		try
		{
		  return Frequency.of(Period.parse(prefixed));
		}
		catch (DateTimeParseException ex)
		{
		  throw new System.ArgumentException(ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a periodic frequency.
	  /// </summary>
	  /// <param name="period">  the period to represent </param>
	  private Frequency(Period period) : this(period, period.ToString())
	  {
	  }

	  /// <summary>
	  /// Creates a periodic frequency.
	  /// </summary>
	  /// <param name="period">  the period to represent </param>
	  /// <param name="name">  the name </param>
	  private Frequency(Period period, string name)
	  {
		ArgChecker.notNull(period, "period");
		ArgChecker.isFalse(period.Zero, "Frequency period must not be zero");
		ArgChecker.isFalse(period.Negative, "Frequency period must not be negative");
		this.period = period;
		this.name = name;
		// calculate events per year
		long monthsLong = period.toTotalMonths();
		if (monthsLong > MAX_MONTHS)
		{
		  eventsPerYear_Renamed = 0;
		  eventsPerYearEstimate_Renamed = 0;
		}
		else
		{
		  int months = (int) monthsLong;
		  int days = period.Days;
		  if (months > 0 && days == 0)
		  {
			eventsPerYear_Renamed = (12 % months == 0) ? 12 / months : -1;
			eventsPerYearEstimate_Renamed = 12d / months;
		  }
		  else if (days > 0 && months == 0)
		  {
			eventsPerYear_Renamed = (364 % days == 0) ? 364 / days : -1;
			eventsPerYearEstimate_Renamed = 364d / days;
		  }
		  else
		  {
			eventsPerYear_Renamed = -1;
			double estimatedSecs = months * MONTHS.Duration.Seconds + days * DAYS.Duration.Seconds;
			eventsPerYearEstimate_Renamed = YEARS.Duration.Seconds / estimatedSecs;
		  }
		}
	  }

	  // safe deserialization
	  private object readResolve()
	  {
		if (this.Equals(TERM))
		{
		  return TERM;
		}
		return of(period);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying period of the frequency.
	  /// </summary>
	  /// <returns> the period </returns>
	  public Period Period
	  {
		  get
		  {
			return period;
		  }
	  }

	  /// <summary>
	  /// Checks if the periodic frequency is the 'Term' instance.
	  /// <para>
	  /// The term instance corresponds to there being no subdivisions of the entire term.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is the 'Term' instance </returns>
	  public bool Term
	  {
		  get
		  {
			return this == TERM;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Normalizes the months and years of this tenor.
	  /// <para>
	  /// This method returns a tenor of an equivalent length but with any number
	  /// of months greater than 12 normalized into a combination of months and years.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the normalized tenor </returns>
	  public Frequency normalized()
	  {
		Period norm = period.normalized();
		return (norm != period ? Frequency.of(norm) : this);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the periodic frequency is week-based.
	  /// <para>
	  /// A week-based frequency consists of an integral number of weeks.
	  /// There must be no day, month or year element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is week-based </returns>
	  public bool WeekBased
	  {
		  get
		  {
			return period.toTotalMonths() == 0 && period.Days % 7 == 0;
		  }
	  }

	  /// <summary>
	  /// Checks if the periodic frequency is month-based.
	  /// <para>
	  /// A month-based frequency consists of an integral number of months.
	  /// Any year-based frequency is also counted as month-based.
	  /// There must be no day or week element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is month-based </returns>
	  public bool MonthBased
	  {
		  get
		  {
			return period.toTotalMonths() > 0 && period.Days == 0 && Term == false;
		  }
	  }

	  /// <summary>
	  /// Checks if the periodic frequency is annual.
	  /// <para>
	  /// An annual frequency consists of 12 months.
	  /// There must be no day or week element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is annual </returns>
	  public bool Annual
	  {
		  get
		  {
			return period.toTotalMonths() == 12 && period.Days == 0;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number of events that occur in a year.
	  /// <para>
	  /// The number of events per year is the number of times that the period occurs per year.
	  /// Not all periodic frequency instances can be converted to an integer events per year.
	  /// All constants declared on this class will return a result.
	  /// </para>
	  /// <para>
	  /// Month-based and year-based periodic frequencies are converted by dividing 12 by the number of months.
	  /// Only the following periodic frequencies return a value - P1M, P2M, P3M, P4M, P6M, P1Y.
	  /// </para>
	  /// <para>
	  /// Day-based and week-based periodic frequencies are converted by dividing 364 by the number of days.
	  /// Only the following periodic frequencies return a value - P1D, P2D, P4D, P1W, P2W, P4W, P13W, P26W, P52W.
	  /// </para>
	  /// <para>
	  /// The 'Term' periodic frequency returns zero.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of events per year </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to calculate the number of events per year </exception>
	  public int eventsPerYear()
	  {
		if (eventsPerYear_Renamed == -1)
		{
		  throw new System.ArgumentException("Unable to calculate events per year: " + this);
		}
		return eventsPerYear_Renamed;
	  }

	  /// <summary>
	  /// Estimates the number of events that occur in a year.
	  /// <para>
	  /// The number of events per year is the number of times that the period occurs per year.
	  /// This method returns an estimate without throwing an exception.
	  /// The exact number of events is returned by <seealso cref="#eventsPerYear()"/>.
	  /// </para>
	  /// <para>
	  /// The 'Term' periodic frequency returns zero.
	  /// Month-based and year-based periodic frequencies return 12 divided by the number of months.
	  /// Day-based and week-based periodic frequencies return 364 divided by the number of days.
	  /// Other frequencies are calculated using estimated durations, dividing the year by the period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the estimated number of events per year </returns>
	  public double eventsPerYearEstimate()
	  {
		return eventsPerYearEstimate_Renamed;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Exactly divides this frequency by another.
	  /// <para>
	  /// This calculates the integer division of this frequency by the specified frequency.
	  /// If the result is not an integer, an exception is thrown.
	  /// </para>
	  /// <para>
	  /// Month-based and year-based periodic frequencies are calculated by dividing the total number of months.
	  /// For example, P6M divided by P3M results in 2, and P2Y divided by P6M returns 4.
	  /// </para>
	  /// <para>
	  /// Day-based and week-based periodic frequencies are calculated by dividing the total number of days.
	  /// For example, P26W divided by P13W results in 2, and P2W divided by P1D returns 14.
	  /// </para>
	  /// <para>
	  /// The 'Term' frequency throws an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other frequency to divide into this one </param>
	  /// <returns> this frequency divided by the other frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if the frequency does not exactly divide into this one </exception>
	  public int exactDivide(Frequency other)
	  {
		ArgChecker.notNull(other, "other");
		if (MonthBased && other.MonthBased)
		{
		  long paymentMonths = Period.toTotalMonths();
		  long accrualMonths = other.Period.toTotalMonths();
		  if ((paymentMonths % accrualMonths) == 0)
		  {
			return Math.toIntExact(paymentMonths / accrualMonths);
		  }
		}
		else if (period.toTotalMonths() == 0 && other.period.toTotalMonths() == 0)
		{
		  long paymentDays = Period.Days;
		  long accrualDays = other.Period.Days;
		  if ((paymentDays % accrualDays) == 0)
		  {
			return Math.toIntExact(paymentDays / accrualDays);
		  }
		}
		throw new System.ArgumentException(Messages.format("Frequency '{}' is not a multiple of '{}'", this, other));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value of the specified unit.
	  /// <para>
	  /// This will return a value for the years, months and days units.
	  /// Note that weeks are not included.
	  /// All other units throw an exception.
	  /// </para>
	  /// <para>
	  /// The 'Term' period is returned as a period of 10,000 years.
	  /// </para>
	  /// <para>
	  /// This method implements <seealso cref="TemporalAmount"/>.
	  /// It is not intended to be called directly.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="unit">  the unit to query </param>
	  /// <returns> the value of the unit </returns>
	  /// <exception cref="UnsupportedTemporalTypeException"> if the unit is not supported </exception>
	  public override long get(TemporalUnit unit)
	  {
		return period.get(unit);
	  }

	  /// <summary>
	  /// Gets the unit of this periodic frequency.
	  /// <para>
	  /// This returns a list containing years, months and days.
	  /// Note that weeks are not included.
	  /// </para>
	  /// <para>
	  /// The 'Term' period is returned as a period of 10,000 years.
	  /// </para>
	  /// <para>
	  /// This method implements <seealso cref="TemporalAmount"/>.
	  /// It is not intended to be called directly.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a list containing the years, months and days units </returns>
	  public override IList<TemporalUnit> Units
	  {
		  get
		  {
			return period.Units;
		  }
	  }

	  /// <summary>
	  /// Adds the period of this frequency to the specified date.
	  /// <para>
	  /// This method implements <seealso cref="TemporalAmount"/>.
	  /// It is not intended to be called directly.
	  /// Use <seealso cref="LocalDate#plus(TemporalAmount)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="temporal">  the temporal object to add to </param>
	  /// <returns> the result with this frequency added </returns>
	  /// <exception cref="DateTimeException"> if unable to add </exception>
	  /// <exception cref="ArithmeticException"> if numeric overflow occurs </exception>
	  public override Temporal addTo(Temporal temporal)
	  {
		// special case for performance
		if (temporal is LocalDate)
		{
		  LocalDate date = (LocalDate) temporal;
		  return date.plusMonths(period.toTotalMonths()).plusDays(period.Days);
		}
		return period.addTo(temporal);
	  }

	  /// <summary>
	  /// Subtracts the period of this frequency from the specified date.
	  /// <para>
	  /// This method implements <seealso cref="TemporalAmount"/>.
	  /// It is not intended to be called directly.
	  /// Use <seealso cref="LocalDate#minus(TemporalAmount)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="temporal">  the temporal object to subtract from </param>
	  /// <returns> the result with this frequency subtracted </returns>
	  /// <exception cref="DateTimeException"> if unable to subtract </exception>
	  /// <exception cref="ArithmeticException"> if numeric overflow occurs </exception>
	  public override Temporal subtractFrom(Temporal temporal)
	  {
		// special case for performance
		if (temporal is LocalDate)
		{
		  LocalDate date = (LocalDate) temporal;
		  return date.minusMonths(period.toTotalMonths()).minusDays(period.Days);
		}
		return period.subtractFrom(temporal);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this periodic frequency equals another periodic frequency.
	  /// <para>
	  /// The comparison checks the frequency period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other frequency, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj == null || this.GetType() != obj.GetType())
		{
		  return false;
		}
		Frequency other = (Frequency) obj;
		return period.Equals(other.period);
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the periodic frequency.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return period.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a formatted string representing the periodic frequency.
	  /// <para>
	  /// The format is a combination of the quantity and unit, such as P1D, P2W, P3M, P4Y.
	  /// The 'Term' amount is returned as 'Term'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the formatted frequency </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return name;
	  }

	}

}
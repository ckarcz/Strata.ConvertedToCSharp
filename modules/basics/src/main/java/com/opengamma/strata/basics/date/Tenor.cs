using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.LocalDateUtils.plusDays;


	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A tenor indicating how long it will take for a financial instrument to reach maturity.
	/// <para>
	/// A tenor is allowed to be any non-negative non-zero period of days, weeks, month or years.
	/// This class provides constants for common tenors which are best used by static import.
	/// </para>
	/// <para>
	/// Each tenor is based on a <seealso cref="Period"/>. The months and years of the period are not normalized,
	/// thus it is possible to have a tenor of 12 months and a different one of 1 year.
	/// When used, standard date addition rules apply, thus there is no difference between them.
	/// Call <seealso cref="#normalized()"/> to apply normalization.
	/// 
	/// <h4>Usage</h4>
	/// {@code Tenor} implements {@code TemporalAmount} allowing it to be directly added to a date:
	/// <pre>
	///  LocalDate later = baseDate.plus(tenor);
	/// </pre>
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class Tenor : IComparable<Tenor>, TemporalAmount
	{

	  /// <summary>
	  /// Serialization version.
	  /// </summary>
	  private const long serialVersionUID = 1;

	  /// <summary>
	  /// A tenor of one day.
	  /// </summary>
	  public static readonly Tenor TENOR_1D = ofDays(1);
	  /// <summary>
	  /// A tenor of two days.
	  /// </summary>
	  public static readonly Tenor TENOR_2D = ofDays(2);
	  /// <summary>
	  /// A tenor of three days.
	  /// </summary>
	  public static readonly Tenor TENOR_3D = ofDays(3);
	  /// <summary>
	  /// A tenor of 1 week.
	  /// </summary>
	  public static readonly Tenor TENOR_1W = ofWeeks(1);
	  /// <summary>
	  /// A tenor of 2 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_2W = ofWeeks(2);
	  /// <summary>
	  /// A tenor of 3 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_3W = ofWeeks(3);
	  /// <summary>
	  /// A tenor of 4 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_4W = ofWeeks(4);
	  /// <summary>
	  /// A tenor of 6 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_6W = ofWeeks(6);
	  /// <summary>
	  /// A tenor of 13 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_13W = ofWeeks(13);
	  /// <summary>
	  /// A tenor of 26 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_26W = ofWeeks(26);
	  /// <summary>
	  /// A tenor of 52 weeks.
	  /// </summary>
	  public static readonly Tenor TENOR_52W = ofWeeks(52);
	  /// <summary>
	  /// A tenor of 1 month.
	  /// </summary>
	  public static readonly Tenor TENOR_1M = ofMonths(1);
	  /// <summary>
	  /// A tenor of 2 months.
	  /// </summary>
	  public static readonly Tenor TENOR_2M = ofMonths(2);
	  /// <summary>
	  /// A tenor of 3 months.
	  /// </summary>
	  public static readonly Tenor TENOR_3M = ofMonths(3);
	  /// <summary>
	  /// A tenor of 4 months.
	  /// </summary>
	  public static readonly Tenor TENOR_4M = ofMonths(4);
	  /// <summary>
	  /// A tenor of 5 months.
	  /// </summary>
	  public static readonly Tenor TENOR_5M = ofMonths(5);
	  /// <summary>
	  /// A tenor of 6 months.
	  /// </summary>
	  public static readonly Tenor TENOR_6M = ofMonths(6);
	  /// <summary>
	  /// A tenor of 7 months.
	  /// </summary>
	  public static readonly Tenor TENOR_7M = ofMonths(7);
	  /// <summary>
	  /// A tenor of 8 months.
	  /// </summary>
	  public static readonly Tenor TENOR_8M = ofMonths(8);
	  /// <summary>
	  /// A tenor of 9 months.
	  /// </summary>
	  public static readonly Tenor TENOR_9M = ofMonths(9);
	  /// <summary>
	  /// A tenor of 10 months.
	  /// </summary>
	  public static readonly Tenor TENOR_10M = ofMonths(10);
	  /// <summary>
	  /// A tenor of 11 months.
	  /// </summary>
	  public static readonly Tenor TENOR_11M = ofMonths(11);
	  /// <summary>
	  /// A tenor of 12 months.
	  /// </summary>
	  public static readonly Tenor TENOR_12M = ofMonths(12);
	  /// <summary>
	  /// A tenor of 15 months.
	  /// </summary>
	  public static readonly Tenor TENOR_15M = ofMonths(15);
	  /// <summary>
	  /// A tenor of 18 months.
	  /// </summary>
	  public static readonly Tenor TENOR_18M = ofMonths(18);
	  /// <summary>
	  /// A tenor of 21 months.
	  /// </summary>
	  public static readonly Tenor TENOR_21M = ofMonths(21);
	  /// <summary>
	  /// A tenor of 1 year.
	  /// </summary>
	  public static readonly Tenor TENOR_1Y = ofYears(1);
	  /// <summary>
	  /// A tenor of 2 years.
	  /// </summary>
	  public static readonly Tenor TENOR_2Y = ofYears(2);
	  /// <summary>
	  /// A tenor of 3 years.
	  /// </summary>
	  public static readonly Tenor TENOR_3Y = ofYears(3);
	  /// <summary>
	  /// A tenor of 4 years.
	  /// </summary>
	  public static readonly Tenor TENOR_4Y = ofYears(4);
	  /// <summary>
	  /// A tenor of 5 years.
	  /// </summary>
	  public static readonly Tenor TENOR_5Y = ofYears(5);
	  /// <summary>
	  /// A tenor of 6 years.
	  /// </summary>
	  public static readonly Tenor TENOR_6Y = ofYears(6);
	  /// <summary>
	  /// A tenor of 7 years.
	  /// </summary>
	  public static readonly Tenor TENOR_7Y = ofYears(7);
	  /// <summary>
	  /// A tenor of 8 years.
	  /// </summary>
	  public static readonly Tenor TENOR_8Y = ofYears(8);
	  /// <summary>
	  /// A tenor of 9 years.
	  /// </summary>
	  public static readonly Tenor TENOR_9Y = ofYears(9);
	  /// <summary>
	  /// A tenor of 10 years.
	  /// </summary>
	  public static readonly Tenor TENOR_10Y = ofYears(10);
	  /// <summary>
	  /// A tenor of 11 years.
	  /// </summary>
	  public static readonly Tenor TENOR_11Y = ofYears(11);
	  /// <summary>
	  /// A tenor of 12 years.
	  /// </summary>
	  public static readonly Tenor TENOR_12Y = ofYears(12);
	  /// <summary>
	  /// A tenor of 13 years.
	  /// </summary>
	  public static readonly Tenor TENOR_13Y = ofYears(13);
	  /// <summary>
	  /// A tenor of 14 years.
	  /// </summary>
	  public static readonly Tenor TENOR_14Y = ofYears(14);
	  /// <summary>
	  /// A tenor of 15 years.
	  /// </summary>
	  public static readonly Tenor TENOR_15Y = ofYears(15);
	  /// <summary>
	  /// A tenor of 20 years.
	  /// </summary>
	  public static readonly Tenor TENOR_20Y = ofYears(20);
	  /// <summary>
	  /// A tenor of 25 years.
	  /// </summary>
	  public static readonly Tenor TENOR_25Y = ofYears(25);
	  /// <summary>
	  /// A tenor of 30 years.
	  /// </summary>
	  public static readonly Tenor TENOR_30Y = ofYears(30);
	  /// <summary>
	  /// A tenor of 35 years.
	  /// </summary>
	  public static readonly Tenor TENOR_35Y = ofYears(35);
	  /// <summary>
	  /// A tenor of 40 years.
	  /// </summary>
	  public static readonly Tenor TENOR_40Y = ofYears(40);
	  /// <summary>
	  /// A tenor of 45 years.
	  /// </summary>
	  public static readonly Tenor TENOR_45Y = ofYears(45);
	  /// <summary>
	  /// A tenor of 50 years.
	  /// </summary>
	  public static readonly Tenor TENOR_50Y = ofYears(50);

	  /// <summary>
	  /// The period of the tenor.
	  /// </summary>
	  private readonly Period period;
	  /// <summary>
	  /// The name of the tenor.
	  /// </summary>
	  private readonly string name;

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
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the period to convert to a tenor </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if the period is negative or zero </exception>
	  public static Tenor of(Period period)
	  {
		int days = period.Days;
		long months = period.toTotalMonths();
		if (months == 0 && days != 0)
		{
		  return ofDays(days);
		}
		return new Tenor(period, period.ToString().Substring(1));
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of days.
	  /// <para>
	  /// If the number of days is an exact multiple of 7 it will be converted to weeks.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="days">  the number of days </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if days is negative or zero </exception>
	  public static Tenor ofDays(int days)
	  {
		if (days % 7 == 0)
		{
		  return ofWeeks(days / 7);
		}
		return new Tenor(Period.ofDays(days), days + "D");
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of weeks.
	  /// </summary>
	  /// <param name="weeks">  the number of weeks </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if weeks is negative or zero </exception>
	  public static Tenor ofWeeks(int weeks)
	  {
		return new Tenor(Period.ofWeeks(weeks), weeks + "W");
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of months.
	  /// <para>
	  /// Months are not normalized into years.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="months">  the number of months </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if months is negative or zero </exception>
	  public static Tenor ofMonths(int months)
	  {
		return new Tenor(Period.ofMonths(months), months + "M");
	  }

	  /// <summary>
	  /// Obtains an instance backed by a period of years.
	  /// </summary>
	  /// <param name="years">  the number of years </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if years is negative or zero </exception>
	  public static Tenor ofYears(int years)
	  {
		return new Tenor(Period.ofYears(years), years + "Y");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a formatted string representing the tenor.
	  /// <para>
	  /// The format can either be based on ISO-8601, such as 'P3M'
	  /// or without the 'P' prefix e.g. '2W'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="toParse">  the string representing the tenor </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if the tenor cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static Tenor parse(String toParse)
	  public static Tenor parse(string toParse)
	  {
		string prefixed = toParse.StartsWith("P", StringComparison.Ordinal) ? toParse : "P" + toParse;
		try
		{
		  return Tenor.of(Period.parse(prefixed));
		}
		catch (DateTimeParseException ex)
		{
		  throw new System.ArgumentException(ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a tenor.
	  /// </summary>
	  /// <param name="period">  the period to represent </param>
	  /// <param name="name">  the name </param>
	  private Tenor(Period period, string name)
	  {
		ArgChecker.notNull(period, "period");
		ArgChecker.isFalse(period.Zero, "Tenor period must not be zero");
		ArgChecker.isFalse(period.Negative, "Tenor period must not be negative");
		this.period = period;
		this.name = name;
	  }

	  // safe deserialization
	  private object readResolve()
	  {
		return new Tenor(period, name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying period of the tenor.
	  /// </summary>
	  /// <returns> the period </returns>
	  public Period Period
	  {
		  get
		  {
			return period;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Normalizes the months and years of this tenor.
	  /// <para>
	  /// This method returns a normalized tenor of an equivalent length.
	  /// If the period is exactly 1 year then the result will be expressed as 12 months.
	  /// Otherwise, the result will be expressed using <seealso cref="Period#normalized()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the normalized tenor </returns>
	  public Tenor normalized()
	  {
		if (period.Days == 0 && period.toTotalMonths() == 12)
		{
		  return TENOR_12M;
		}
		Period norm = period.normalized();
		return (norm != period ? Tenor.of(norm) : this);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the tenor is week-based.
	  /// <para>
	  /// A week-based tenor consists of an integral number of weeks.
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
	  /// Checks if the tenor is month-based.
	  /// <para>
	  /// A month-based tenor consists of an integral number of months.
	  /// Any year-based tenor is also counted as month-based.
	  /// There must be no day or week element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is month-based </returns>
	  public bool MonthBased
	  {
		  get
		  {
			return period.toTotalMonths() > 0 && period.Days == 0;
		  }
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
	  /// Gets the units supported by a tenor.
	  /// <para>
	  /// This returns a list containing years, months and days.
	  /// Note that weeks are not included.
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
	  /// Adds this tenor to the specified date.
	  /// <para>
	  /// This method implements <seealso cref="TemporalAmount"/>.
	  /// It is not intended to be called directly.
	  /// Use <seealso cref="LocalDate#plus(TemporalAmount)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="temporal">  the temporal object to add to </param>
	  /// <returns> the result with this tenor added </returns>
	  /// <exception cref="DateTimeException"> if unable to add </exception>
	  /// <exception cref="ArithmeticException"> if numeric overflow occurs </exception>
	  public override Temporal addTo(Temporal temporal)
	  {
		// special case for performance
		if (temporal is LocalDate)
		{
		  LocalDate date = (LocalDate) temporal;
		  return plusDays(date.plusMonths(period.toTotalMonths()), period.Days);
		}
		return period.addTo(temporal);
	  }

	  /// <summary>
	  /// Subtracts this tenor from the specified date.
	  /// <para>
	  /// This method implements <seealso cref="TemporalAmount"/>.
	  /// It is not intended to be called directly.
	  /// Use <seealso cref="LocalDate#minus(TemporalAmount)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="temporal">  the temporal object to subtract from </param>
	  /// <returns> the result with this tenor subtracted </returns>
	  /// <exception cref="DateTimeException"> if unable to subtract </exception>
	  /// <exception cref="ArithmeticException"> if numeric overflow occurs </exception>
	  public override Temporal subtractFrom(Temporal temporal)
	  {
		// special case for performance
		if (temporal is LocalDate)
		{
		  LocalDate date = (LocalDate) temporal;
		  return plusDays(date.minusMonths(period.toTotalMonths()), -period.Days);
		}
		return period.subtractFrom(temporal);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this tenor to another tenor.
	  /// <para>
	  /// Comparing tenors is a hard problem in general, but for commonly used tenors the outcome is as expected.
	  /// If the two tenors are both based on days, then comparison is easy.
	  /// If the two tenors are both based on months/years, then comparison is easy.
	  /// Otherwise, months are converted to days to form an estimated length in days which is compared.
	  /// The conversion from months to days divides by 12 and then multiplies by 365.25.
	  /// </para>
	  /// <para>
	  /// The resulting order places:
	  /// <ul>
	  /// <li>a 1 month tenor between 30 and 31 days
	  /// <li>a 2 month tenor between 60 and 61 days
	  /// <li>a 3 month tenor between 91 and 92 days
	  /// <li>a 6 month tenor between 182 and 183 days
	  /// <li>a 1 year tenor between 365 and 366 days
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other tenor </param>
	  /// <returns> negative if this is less than the other, zero if equal and positive if greater </returns>
	  public int CompareTo(Tenor other)
	  {
		int thisDays = this.Period.Days;
		long thisMonths = this.Period.toTotalMonths();
		int otherDays = other.Period.Days;
		long otherMonths = other.Period.toTotalMonths();
		// both day-only
		if (thisMonths == 0 && otherMonths == 0)
		{
		  return Integer.compare(thisDays, otherDays);
		}
		// both month-only
		if (thisDays == 0 && otherDays == 0)
		{
		  return Long.compare(thisMonths, otherMonths);
		}
		// complex
		double thisMonthsInDays = (thisMonths / 12d) * 365.25d;
		double otherMonthsInDays = (otherMonths / 12d) * 365.25d;
		return thisDays + thisMonthsInDays.CompareTo(otherDays + otherMonthsInDays);
	  }

	  /// <summary>
	  /// Checks if this tenor equals another tenor.
	  /// <para>
	  /// The comparison checks the tenor period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other tenor, null returns false </param>
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
		Tenor other = (Tenor) obj;
		return period.Equals(other.period);
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the tenor.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return period.GetHashCode();
	  }

	  /// <summary>
	  /// Returns a formatted string representing the tenor.
	  /// <para>
	  /// The format is a combination of the quantity and unit, such as 1D, 2W, 3M, 4Y.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the formatted tenor </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return name;
	  }

	}

}
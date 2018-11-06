/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A series of dates identified by name.
	/// <para>
	/// This interface encapsulates a sequence of dates as used in standard financial instruments.
	/// The most common are the quarterly IMM dates, which are on the third Wednesday of March,
	/// June, September and December.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="DateSequences"/>.
	/// </para>
	/// <para>
	/// Note that the dates produced by the sequence may not be business days.
	/// The application of a holiday calendar is typically the responsibility of the caller.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface DateSequence : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the date sequence </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static DateSequence of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static DateSequence of(String uniqueName)
	//  {
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the sequence to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<DateSequence> extendedEnum()
	//  {
	//	return DateSequences.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the next date in the sequence, always returning a date later than the input date.
	  /// <para>
	  /// Given an input date, this method returns the next date after it from the sequence.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the input date </param>
	  /// <returns> the next sequence date after the input date </returns>
	  /// <exception cref="IllegalArgumentException"> if there are no more sequence dates </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate next(java.time.LocalDate date)
	//  {
	//	LocalDate next = plusDays(date, 1);
	//	return nextOrSame(next);
	//  }

	  /// <summary>
	  /// Finds the next date in the sequence, returning the input date if it is a date in the sequence.
	  /// <para>
	  /// Given an input date, this method returns a date from the sequence.
	  /// If the input date is in the sequence, it is returned.
	  /// Otherwise, the next date in the sequence after the input date is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the input date </param>
	  /// <returns> the input date if it is a sequence date, otherwise the next sequence date </returns>
	  /// <exception cref="IllegalArgumentException"> if there are no more sequence dates </exception>
	  LocalDate nextOrSame(LocalDate date);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the nth date in the sequence after the input date,
	  /// always returning a date later than the input date.
	  /// <para>
	  /// Given an input date, this method returns a date from the sequence.
	  /// If the sequence number is 1, then the first date in the sequence after the input date is returned.
	  /// </para>
	  /// <para>
	  /// If the sequence number is 2 or larger, then the date referred to by sequence number 1
	  /// is calculated, and the nth matching sequence date after that date returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the input date </param>
	  /// <param name="sequenceNumber">  the 1-based index of the date to find </param>
	  /// <returns> the nth sequence date after the input date </returns>
	  /// <exception cref="IllegalArgumentException"> if the sequence number is zero or negative or if there are no more sequence dates </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate nth(java.time.LocalDate date, int sequenceNumber)
	//  {
	//	ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
	//	if (sequenceNumber == 1)
	//	{
	//	  return next(date);
	//	}
	//	else
	//	{
	//	  return nth(next(date), sequenceNumber - 1);
	//	}
	//  }

	  /// <summary>
	  /// Finds the nth date in the sequence on or after the input date,
	  /// returning the input date if it is a date in the sequence.
	  /// <para>
	  /// Given an input date, this method returns a date from the sequence.
	  /// If the sequence number is 1, then either the input date or the first date
	  /// in the sequence after the input date is returned.
	  /// </para>
	  /// <para>
	  /// If the sequence number is 2 or larger, then the date referred to by sequence number 1
	  /// is calculated, and the nth matching sequence date after that date returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the input date </param>
	  /// <param name="sequenceNumber">  the 1-based index of the date to find </param>
	  /// <returns> the nth sequence date on or after the input date </returns>
	  /// <exception cref="IllegalArgumentException"> if the sequence number is zero or negative or if there are no more sequence dates </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate nthOrSame(java.time.LocalDate date, int sequenceNumber)
	//  {
	//	ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
	//	if (sequenceNumber == 1)
	//	{
	//	  return nextOrSame(date);
	//	}
	//	else
	//	{
	//	  return nth(nextOrSame(date), sequenceNumber - 1);
	//	}
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the date in the sequence that corresponds to the specified year-month.
	  /// <para>
	  /// Given an input month, this method returns the date from the sequence that is associated with the year-month.
	  /// In most cases, the returned date will be in the same month as the input month,
	  /// but this is not guaranteed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearMonth">  the input year-month </param>
	  /// <returns> the next sequence date after the input date </returns>
	  /// <exception cref="IllegalArgumentException"> if there are no more sequence dates </exception>
	  LocalDate dateMatching(YearMonth yearMonth);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this sequence.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	}

}
/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An index of foreign exchange rates.
	/// <para>
	/// An FX rate is the conversion rate between two currencies.
	/// An FX index is the rate as published by a specific organization, typically
	/// at a well-known time-of-day.
	/// </para>
	/// <para>
	/// The index is defined by two dates.
	/// The fixing date is the date on which the index is to be observed.
	/// The maturity date is the date on which delivery of the implied exchange occurs.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="FxIndices"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface FxIndex : Index, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FxIndex of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxIndex of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the index to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<FxIndex> extendedEnum()
	//  {
	//	return FxIndices.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair of the index.
	  /// </summary>
	  /// <returns> the currency pair of the index </returns>
	  CurrencyPair CurrencyPair {get;}

	  /// <summary>
	  /// Gets the adjustment applied to the maturity date to obtain the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The maturity date is the date on which the implied amount is delivered/exchanged.
	  /// The maturity date is typically two business days after the fixing date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing date offset </returns>
	  DaysAdjustment FixingDateOffset {get;}

	  /// <summary>
	  /// Gets the adjustment applied to the fixing date to obtain the maturity date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The maturity date is the date on which the implied amount is delivered/exchanged.
	  /// The maturity date is typically two business days after the fixing date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the maturity date offset </returns>
	  DaysAdjustment MaturityDateOffset {get;}

	  /// <summary>
	  /// Gets the calendar that determines which dates are fixing dates.
	  /// <para>
	  /// The rate will be fixed on each business day in this calendar.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the calendar used to determine the fixing dates of the index </returns>
	  HolidayCalendarId FixingCalendar {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the maturity date from the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The maturity date is the date on which the implied amount is delivered/exchanged.
	  /// The maturity date is typically two days after the fixing date.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid fixing date.
	  /// Instead, the fixing date is moved to the next valid fixing date and then processed.
	  /// </para>
	  /// <para>
	  /// The maturity date is also known as the <i>value date</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the maturity date </returns>
	  LocalDate calculateMaturityFromFixing(LocalDate fixingDate, ReferenceData refData);

	  /// <summary>
	  /// Calculates the fixing date from the maturity date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The maturity date is the date on which the implied amount is delivered/exchanged.
	  /// The maturity date is typically two days after the fixing date.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid effective date.
	  /// Instead, the effective date is moved to the next valid effective date and then processed.
	  /// </para>
	  /// <para>
	  /// The maturity date is also known as the <i>value date</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="maturityDate">  the maturity date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the fixing date </returns>
	  LocalDate calculateFixingFromMaturity(LocalDate maturityDate, ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves this index using the specified reference data, returning a function.
	  /// <para>
	  /// This returns a <seealso cref="Function"/> that converts fixing dates to observations.
	  /// It binds the holiday calendar, looked up from the reference data, into the result.
	  /// As such, there is no need to pass the reference data in again.
	  /// </para>
	  /// <para>
	  /// This method is intended for use when looping to create multiple instances
	  /// of {@code FxIndexObservation}. Implementations of the method are intended
	  /// to optimize, avoiding repeated calls to resolve the holiday calendar
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> a function that converts fixing date to observation </returns>
	  System.Func<LocalDate, FxIndexObservation> resolve(ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this index.
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
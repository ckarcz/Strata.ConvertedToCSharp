/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using TenorAdjustment = com.opengamma.strata.basics.date.TenorAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An inter-bank lending rate index, such as Libor or Euribor.
	/// <para>
	/// An index represented by this class relates to inter-bank lending for periods
	/// from one day to one year. They are typically calculated and published as the
	/// trimmed arithmetic mean of estimated rates contributed by banks.
	/// </para>
	/// <para>
	/// The index is defined by three dates.
	/// The fixing date is the date on which the index is to be observed.
	/// The effective date is the date on which the implied deposit starts.
	/// The maturity date is the date on which the implied deposit ends.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="IborIndices"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface IborIndex : RateIndex, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static IborIndex of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborIndex of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<IborIndex> extendedEnum()
	//  {
	//	return IborIndices.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the adjustment applied to the effective date to obtain the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// In most cases, the fixing date is 0 or 2 days before the effective date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing date offset </returns>
	  DaysAdjustment FixingDateOffset {get;}

	  /// <summary>
	  /// Gets the adjustment applied to the fixing date to obtain the effective date.
	  /// <para>
	  /// The effective date is the start date of the indexed deposit.
	  /// In most cases, the effective date is 0 or 2 days after the fixing date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the effective date offset </returns>
	  DaysAdjustment EffectiveDateOffset {get;}

	  /// <summary>
	  /// Gets the adjustment applied to the effective date to obtain the maturity date.
	  /// <para>
	  /// The maturity date is the end date of the indexed deposit and is relative to the effective date.
	  /// This data structure allows the complex rules of some indices to be represented.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the maturity date offset </returns>
	  TenorAdjustment MaturityDateOffset {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts the fixing date-time from the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The fixing date-time is the specific date and time of the observation.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid fixing date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <returns>  the fixing date-time </returns>
	  ZonedDateTime calculateFixingDateTime(LocalDate fixingDate);

	  /// <summary>
	  /// Calculates the effective date from the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The effective date is the date on which the implied deposit starts.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid fixing date.
	  /// Instead, the fixing date is moved to the next valid fixing date and then processed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the effective date </returns>
	  LocalDate calculateEffectiveFromFixing(LocalDate fixingDate, ReferenceData refData);

	  /// <summary>
	  /// Calculates the maturity date from the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The maturity date is the date on which the implied deposit ends.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid fixing date.
	  /// Instead, the fixing date is moved to the next valid fixing date and then processed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the maturity date </returns>
	  LocalDate calculateMaturityFromFixing(LocalDate fixingDate, ReferenceData refData);

	  /// <summary>
	  /// Calculates the fixing date from the effective date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The effective date is the date on which the implied deposit starts.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid effective date.
	  /// Instead, the effective date is moved to the next valid effective date and then processed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="effectiveDate">  the effective date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the fixing date </returns>
	  LocalDate calculateFixingFromEffective(LocalDate effectiveDate, ReferenceData refData);

	  /// <summary>
	  /// Calculates the maturity date from the effective date.
	  /// <para>
	  /// The effective date is the date on which the implied deposit starts.
	  /// The maturity date is the date on which the implied deposit ends.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid effective date.
	  /// Instead, the effective date is moved to the next valid effective date and then processed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="effectiveDate">  the effective date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the maturity date </returns>
	  LocalDate calculateMaturityFromEffective(LocalDate effectiveDate, ReferenceData refData);

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
	  /// of {@code IborIndexObservation}. Implementations of the method are intended
	  /// to optimize, avoiding repeated calls to resolve the holiday calendar
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> a function that converts fixing date to observation </returns>
	  System.Func<LocalDate, IborIndexObservation> resolve(ReferenceData refData);

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
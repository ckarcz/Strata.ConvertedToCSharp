/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An Overnight index, such as Sonia or Eonia.
	/// <para>
	/// An index represented by this class relates to lending over one night.
	/// The rate typically refers to "Today/Tomorrow" but might refer to "Tomorrow/Next".
	/// </para>
	/// <para>
	/// The index is defined by four dates.
	/// The fixing date is the date on which the index is to be observed.
	/// The publication date is the date on which the fixed rate is actually published.
	/// The effective date is the date on which the implied deposit starts.
	/// The maturity date is the date on which the implied deposit ends.
	/// </para>
	/// <para>
	/// The most common implementations are provided in <seealso cref="OvernightIndices"/>.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface OvernightIndex : RateIndex, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static OvernightIndex of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static OvernightIndex of(String uniqueName)
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
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<OvernightIndex> extendedEnum()
	//  {
	//	return OvernightIndices.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days to add to the fixing date to obtain the publication date.
	  /// <para>
	  /// In most cases, the fixing rate is available on the fixing date.
	  /// In a few cases, publication of the fixing rate is delayed until the following business day.
	  /// This property is zero if publication is on the fixing date, or one if it is the next day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the publication date offset </returns>
	  int PublicationDateOffset {get;}

	  /// <summary>
	  /// Gets the number of days to add to the fixing date to obtain the effective date.
	  /// <para>
	  /// In most cases, the settlement date and start of the implied deposit is on the fixing date.
	  /// In a few cases, the settlement date is the following business day.
	  /// This property is zero if settlement is on the fixing date, or one if it is the next day.
	  /// Maturity is always one business day after the settlement date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the effective date offset </returns>
	  int EffectiveDateOffset {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the publication date from the fixing date.
	  /// <para>
	  /// The fixing date is the date on which the index is to be observed.
	  /// The publication date is the date on which the fixed rate is actually published.
	  /// </para>
	  /// <para>
	  /// No error is thrown if the input date is not a valid fixing date.
	  /// Instead, the fixing date is moved to the next valid fixing date and then processed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <param name="refData">  the reference data, used to resolve the holiday calendar </param>
	  /// <returns> the publication date </returns>
	  LocalDate calculatePublicationFromFixing(LocalDate fixingDate, ReferenceData refData);

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
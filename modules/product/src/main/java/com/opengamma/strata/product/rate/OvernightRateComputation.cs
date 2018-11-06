/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using HolidayCalendar = com.opengamma.strata.basics.date.HolidayCalendar;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using Messages = com.opengamma.strata.collect.Messages;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Defines the computation of a rate from a single Overnight index.
	/// </summary>
	public interface OvernightRateComputation : RateComputation
	{

	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="rateCutOffDays">  the rate cutoff days </param>
	  /// <param name="accrualMethod">  the accrual method </param>
	  /// <param name="referenceData">  the reference data </param>
	  /// <returns> the instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static OvernightRateComputation of(com.opengamma.strata.basics.index.OvernightIndex index, java.time.LocalDate startDate, java.time.LocalDate endDate, int rateCutOffDays, com.opengamma.strata.product.swap.OvernightAccrualMethod accrualMethod, com.opengamma.strata.basics.ReferenceData referenceData)
	//  {
	//
	//	switch (accrualMethod)
	//	{
	//	  case COMPOUNDED:
	//		return OvernightCompoundedRateComputation.of(index, startDate, endDate, rateCutOffDays, referenceData);
	//	  case AVERAGED:
	//		return OvernightAveragedRateComputation.of(index, startDate, endDate, rateCutOffDays, referenceData);
	//	  case AVERAGED_DAILY:
	//		return OvernightAveragedDailyRateComputation.of(index, startDate, endDate, referenceData);
	//	  default:
	//		throw new IllegalArgumentException(Messages.format("unsupported Overnight accrual method, {}", accrualMethod));
	//	}
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the Overnight index.
	  /// <para>
	  /// The rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-SONIA'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the overnight index </returns>
	  OvernightIndex Index {get;}

	  /// <summary>
	  /// Obtains the resolved calendar that the index uses.
	  /// </summary>
	  /// <returns> the fixing calendar </returns>
	  HolidayCalendar FixingCalendar {get;}

	  /// <summary>
	  /// Obtains the fixing date associated with the start date of the accrual period.
	  /// <para>
	  /// This is also the first fixing date.
	  /// The overnight rate is observed from this date onwards.
	  /// </para>
	  /// <para>
	  /// In general, the fixing dates and accrual dates are the same for an overnight index.
	  /// However, in the case of a Tomorrow/Next index, the fixing period is one business day
	  /// before the accrual period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date </returns>
	  LocalDate StartDate {get;}

	  /// <summary>
	  /// Obtains the fixing date associated with the end date of the accrual period.
	  /// <para>
	  /// The overnight rate is observed until this date.
	  /// </para>
	  /// <para>
	  /// In general, the fixing dates and accrual dates are the same for an overnight index.
	  /// However, in the case of a Tomorrow/Next index, the fixing period is one business day
	  /// before the accrual period.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date </returns>
	  LocalDate EndDate {get;}

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
	  /// <returns> the publication date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate calculatePublicationFromFixing(java.time.LocalDate fixingDate)
	//  {
	//	return getFixingCalendar().shift(getFixingCalendar().nextOrSame(fixingDate), getIndex().getPublicationDateOffset());
	//  }

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
	  /// <returns> the effective date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate calculateEffectiveFromFixing(java.time.LocalDate fixingDate)
	//  {
	//	return getFixingCalendar().shift(getFixingCalendar().nextOrSame(fixingDate), getIndex().getEffectiveDateOffset());
	//  }

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
	  /// <returns> the maturity date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate calculateMaturityFromFixing(java.time.LocalDate fixingDate)
	//  {
	//	return getFixingCalendar().shift(getFixingCalendar().nextOrSame(fixingDate), getIndex().getEffectiveDateOffset() + 1);
	//  }

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
	  /// <returns> the fixing date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate calculateFixingFromEffective(java.time.LocalDate effectiveDate)
	//  {
	//	return getFixingCalendar().shift(getFixingCalendar().nextOrSame(effectiveDate), -getIndex().getEffectiveDateOffset());
	//  }

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
	  /// <returns> the maturity date </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.time.LocalDate calculateMaturityFromEffective(java.time.LocalDate effectiveDate)
	//  {
	//	return getFixingCalendar().shift(getFixingCalendar().nextOrSame(effectiveDate), 1);
	//  }

	  /// <summary>
	  /// Creates an observation object for the specified fixing date.
	  /// </summary>
	  /// <param name="fixingDate">  the fixing date </param>
	  /// <returns> the index observation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.index.OvernightIndexObservation observeOn(java.time.LocalDate fixingDate)
	//  {
	//	LocalDate publicationDate = calculatePublicationFromFixing(fixingDate);
	//	LocalDate effectiveDate = calculateEffectiveFromFixing(fixingDate);
	//	LocalDate maturityDate = calculateMaturityFromEffective(effectiveDate);
	//	return OvernightIndexObservation.builder().index(getIndex()).fixingDate(fixingDate).publicationDate(publicationDate).effectiveDate(effectiveDate).maturityDate(maturityDate).yearFraction(getIndex().getDayCount().yearFraction(effectiveDate, maturityDate)).build();
	//  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default void collectIndices(com.google.common.collect.ImmutableSet.Builder<com.opengamma.strata.basics.index.Index> builder)
	//  {
	//	builder.add(getIndex());
	//  }

	}

}
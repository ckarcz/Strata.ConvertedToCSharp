/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Index = com.opengamma.strata.basics.index.Index;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;

	/// <summary>
	/// The accrual calculation part of an interest rate swap leg.
	/// <para>
	/// An interest rate swap leg is defined by <seealso cref="RateCalculationSwapLeg"/>.
	/// The rate to be paid is defined by the implementations of this interface.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface RateCalculation
	{

	  /// <summary>
	  /// Gets the type of the leg, such as Fixed or Ibor.
	  /// <para>
	  /// This provides a high level categorization of the swap leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the leg type </returns>
	  SwapLegType Type {get;}

	  /// <summary>
	  /// Gets the day count convention.
	  /// <para>
	  /// This is used to convert schedule period dates to a numerical value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the day count convention </returns>
	  DayCount DayCount {get;}

	  /// <summary>
	  /// Collects all the currencies referred to by this calculation.
	  /// <para>
	  /// This collects the complete set of currencies for the calculation, not just the payment currencies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to populate </param>
	  void collectCurrencies(ImmutableSet.Builder<Currency> builder);

	  /// <summary>
	  /// Collects all the indices referred to by this calculation.
	  /// <para>
	  /// A calculation will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Each index that is referred to must be added to the specified builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="builder">  the builder to use </param>
	  void collectIndices(ImmutableSet.Builder<Index> builder);

	  /// <summary>
	  /// Creates accrual periods based on the specified schedule.
	  /// <para>
	  /// The specified accrual schedule defines the period dates to be created.
	  /// One instance of <seealso cref="RateAccrualPeriod"/> must be created for each period in the schedule.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="accrualSchedule">  the accrual schedule </param>
	  /// <param name="paymentSchedule">  the payment schedule </param>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the accrual periods </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if the calculation is invalid </exception>
	  ImmutableList<RateAccrualPeriod> createAccrualPeriods(Schedule accrualSchedule, Schedule paymentSchedule, ReferenceData refData);

	}

}
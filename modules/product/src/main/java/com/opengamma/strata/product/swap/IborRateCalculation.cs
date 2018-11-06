using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.value.ValueSchedule.ALWAYS_0;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.value.ValueSchedule.ALWAYS_1;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.IborRateResetMethod.UNWEIGHTED;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using Index = com.opengamma.strata.basics.index.Index;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborAveragedFixing = com.opengamma.strata.product.rate.IborAveragedFixing;
	using IborAveragedRateComputation = com.opengamma.strata.product.rate.IborAveragedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Defines the calculation of a floating rate swap leg based on an Ibor index.
	/// <para>
	/// This defines the data necessary to calculate the amount payable on the leg.
	/// The amount is based on the observed value of an Ibor index such as 'GBP-LIBOR-3M' or 'EUR-EURIBOR-1M'.
	/// </para>
	/// <para>
	/// The index is observed once for each <i>reset period</i> and referred to as a <i>fixing</i>.
	/// The actual date of observation is the <i>fixing date</i>, which is relative to either
	/// the start or end of the reset period.
	/// </para>
	/// <para>
	/// The reset period is typically the same as the accrual period.
	/// In this case, the rate for the accrual period is based directly on the fixing.
	/// If the reset period is a subdivision of the accrual period then there are multiple fixings,
	/// one for each reset period.
	/// In that case, the rate for the accrual period is based on an average of the fixings.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborRateCalculation implements RateCalculation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborRateCalculation : RateCalculation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
		private readonly DayCount dayCount;
	  /// <summary>
	  /// The Ibor index.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndex index;
	  private readonly IborIndex index;
	  /// <summary>
	  /// The reset schedule, used when averaging rates, optional.
	  /// <para>
	  /// Most swaps have a single fixing for each accrual period.
	  /// This property allows multiple fixings to be defined by dividing the accrual periods into reset periods.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the reset period is the same as the accrual period.
	  /// If this property is present, then the accrual period is divided as per the information
	  /// in the reset schedule, multiple fixing dates are calculated, and rate averaging performed.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final ResetSchedule resetPeriods;
	  private readonly ResetSchedule resetPeriods;
	  /// <summary>
	  /// The base date that each fixing is made relative to, defaulted to 'PeriodStart'.
	  /// <para>
	  /// The fixing date is relative to either the start or end of each reset period.
	  /// </para>
	  /// <para>
	  /// Note that in most cases, the reset frequency matches the accrual frequency
	  /// and thus there is only one fixing for the accrual period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FixingRelativeTo fixingRelativeTo;
	  private readonly FixingRelativeTo fixingRelativeTo;
	  /// <summary>
	  /// The offset of the fixing date from each adjusted reset date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// Note that in most cases, the reset frequency matches the accrual frequency
	  /// and thus there is only one fixing for the accrual period.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing offset of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The negative rate method, defaulted to 'AllowNegative'.
	  /// <para>
	  /// This is used when the interest rate, observed or calculated, goes negative.
	  /// It does not apply if the rate is fixed, such as in a stub or using {@code firstRegularRate}.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.4.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final NegativeRateMethod negativeRateMethod;
	  private readonly NegativeRateMethod negativeRateMethod;

	  /// <summary>
	  /// The rate of the first regular reset period, optional.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree the rate of the first fixing
	  /// when the contract starts, and it is used in place of one observed fixing.
	  /// For all other fixings, the rate is observed via the normal fixing process.
	  /// </para>
	  /// <para>
	  /// This property allows the rate of the first reset period of the first <i>regular</i> accrual period
	  /// to be controlled. Note that if there is an initial stub, this will be the second reset period.
	  /// Other calculation elements, such as gearing or spread, still apply to the rate specified here.
	  /// </para>
	  /// <para>
	  /// If the first rate applies to the initial stub rather than the regular accrual periods
	  /// it must be specified using {@code initialStub}. Alternatively, {@code firstRate} can be used.
	  /// </para>
	  /// <para>
	  /// This property follows the definition in FpML. See also {@code firstRate}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> firstRegularRate;
	  private readonly double? firstRegularRate;
	  /// <summary>
	  /// The rate of the first reset period, which may be a stub, optional.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree the rate of the first fixing
	  /// when the contract starts, and it is used in place of one observed fixing.
	  /// For all other fixings, the rate is observed via the normal fixing process.
	  /// </para>
	  /// <para>
	  /// This property allows the rate of the first reset period to be controlled,
	  /// irrespective of whether that is an initial stub or a regular period.
	  /// Other calculation elements, such as gearing or spread, still apply to the rate specified here.
	  /// </para>
	  /// <para>
	  /// This property is similar to {@code firstRegularRate}.
	  /// This property operates on the first reset period, whether that is an initial stub or a regular period.
	  /// By contrast, {@code firstRegularRate} operates on the first regular period, and never on a stub.
	  /// </para>
	  /// <para>
	  /// If either {@code firstRegularRate} or {@code initialStub} are present, this property is ignored.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the first rate is observed via the normal fixing process.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> firstRate;
	  private readonly double? firstRate;
	  /// <summary>
	  /// The offset of the first fixing date from the first adjusted reset date, optional.
	  /// <para>
	  /// If present, this offset is used instead of {@code fixingDateOffset} for the first
	  /// reset period of the swap, which will be either an initial stub or the first reset
	  /// period of the first <i>regular</i> accrual period.
	  /// </para>
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the {@code fixingDateOffset} applies to all fixings.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.DaysAdjustment firstFixingDateOffset;
	  private readonly DaysAdjustment firstFixingDateOffset;
	  /// <summary>
	  /// The rate to be used in initial stub, optional.
	  /// <para>
	  /// The initial stub of a swap may have different rate rules to the regular accrual periods.
	  /// A fixed rate may be specified, a different floating rate or a linearly interpolated floating rate.
	  /// This may not be present if there is no initial stub, or if the index during the stub is the same
	  /// as the main floating rate index.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the main index applies during any initial stub.
	  /// If this property is present and there is no initial stub, it is ignored.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final IborRateStubCalculation initialStub;
	  private readonly IborRateStubCalculation initialStub;
	  /// <summary>
	  /// The rate to be used in final stub, optional.
	  /// <para>
	  /// The final stub of a swap may have different rate rules to the regular accrual periods.
	  /// A fixed rate may be specified, a different floating rate or a linearly interpolated floating rate.
	  /// This may not be present if there is no final stub, or if the index during the stub is the same
	  /// as the main floating rate index.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the main index applies during any final stub.
	  /// If this property is present and there is no final stub, it is ignored.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final IborRateStubCalculation finalStub;
	  private readonly IborRateStubCalculation finalStub;
	  /// <summary>
	  /// The gearing multiplier, optional.
	  /// <para>
	  /// This defines the gearing as an initial value and a list of adjustments.
	  /// The gearing is only permitted to change at accrual period boundaries.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the fixing rate is multiplied by the gearing.
	  /// A gearing of 1 has no effect.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then no gearing applies.
	  /// </para>
	  /// <para>
	  /// Gearing is also known as <i>leverage</i>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.value.ValueSchedule gearing;
	  private readonly ValueSchedule gearing;
	  /// <summary>
	  /// The spread rate, with a 5% rate expressed as 0.05, optional.
	  /// <para>
	  /// This defines the spread as an initial value and a list of adjustments.
	  /// The spread is only permitted to change at accrual period boundaries.
	  /// Spread is a per annum rate.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the spread is added to the fixing rate.
	  /// A spread of 0 has no effect.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then no spread applies.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2e.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.value.ValueSchedule spread;
	  private readonly ValueSchedule spread;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a rate calculation for the specified index.
	  /// <para>
	  /// The calculation will use the day count and fixing offset of the index.
	  /// All optional fields will be set to their default values.
	  /// Thus, fixing will be in advance, with no spread, gearing or reset periods.
	  /// If this method provides insufficient control, use the <seealso cref="#builder() builder"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <returns> the calculation </returns>
	  public static IborRateCalculation of(IborIndex index)
	  {
		return IborRateCalculation.builder().index(index).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.fixingRelativeTo(FixingRelativeTo.PERIOD_START);
		builder.negativeRateMethod(NegativeRateMethod.ALLOW_NEGATIVE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.index_Renamed != null)
		{
		  if (builder.dayCount_Renamed == null)
		  {
			builder.dayCount_Renamed = builder.index_Renamed.DayCount;
		  }
		  if (builder.fixingDateOffset_Renamed == null)
		  {
			builder.fixingDateOffset_Renamed = builder.index_Renamed.FixingDateOffset;
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public SwapLegType Type
	  {
		  get
		  {
			return SwapLegType.IBOR;
		  }
	  }

	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		builder.add(index.Currency);
		InitialStub.ifPresent(stub => stub.collectCurrencies(builder));
		FinalStub.ifPresent(stub => stub.collectCurrencies(builder));
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(index);
		InitialStub.ifPresent(stub => stub.collectIndices(builder));
		FinalStub.ifPresent(stub => stub.collectIndices(builder));
	  }

	  public ImmutableList<RateAccrualPeriod> createAccrualPeriods(Schedule accrualSchedule, Schedule paymentSchedule, ReferenceData refData)
	  {

		// resolve data by schedule
		DoubleArray resolvedGearings = firstNonNull(gearing, ALWAYS_1).resolveValues(accrualSchedule);
		DoubleArray resolvedSpreads = firstNonNull(spread, ALWAYS_0).resolveValues(accrualSchedule);
		// resolve against reference data once
		DateAdjuster fixingDateAdjuster = fixingDateOffset.resolve(refData);
		System.Func<SchedulePeriod, Schedule> resetScheduleFn = ResetPeriods.map(rp => rp.createSchedule(accrualSchedule.RollConvention, refData)).orElse(null);
		System.Func<LocalDate, IborIndexObservation> iborObservationFn = index.resolve(refData);
		// build accrual periods
		Optional<SchedulePeriod> scheduleInitialStub = accrualSchedule.InitialStub;
		Optional<SchedulePeriod> scheduleFinalStub = accrualSchedule.FinalStub;
		ImmutableList.Builder<RateAccrualPeriod> accrualPeriods = ImmutableList.builder();
		for (int i = 0; i < accrualSchedule.size(); i++)
		{
		  SchedulePeriod period = accrualSchedule.getPeriod(i);
		  RateComputation rateComputation = createRateComputation(period, fixingDateAdjuster, resetScheduleFn, iborObservationFn, i, scheduleInitialStub, scheduleFinalStub, refData);
		  double yearFraction = period.yearFraction(dayCount, accrualSchedule);
		  accrualPeriods.add(new RateAccrualPeriod(period, yearFraction, rateComputation, resolvedGearings.get(i), resolvedSpreads.get(i), negativeRateMethod));
		}
		return accrualPeriods.build();
	  }

	  // creates the rate computation
	  private RateComputation createRateComputation(SchedulePeriod period, DateAdjuster fixingDateAdjuster, System.Func<SchedulePeriod, Schedule> resetScheduleFn, System.Func<LocalDate, IborIndexObservation> iborObservationFn, int scheduleIndex, Optional<SchedulePeriod> scheduleInitialStub, Optional<SchedulePeriod> scheduleFinalStub, ReferenceData refData)
	  {

		LocalDate fixingDate = fixingDateAdjuster.adjust(fixingRelativeTo.selectBaseDate(period));
		if (scheduleIndex == 0 && firstFixingDateOffset != null)
		{
		  fixingDate = firstFixingDateOffset.resolve(refData).adjust(fixingRelativeTo.selectBaseDate(period));
		}
		// initial stub
		if (scheduleInitialStub.Present && scheduleIndex == 0)
		{
		  if (firstRate != null && firstRegularRate == null && (initialStub == null || IborRateStubCalculation.NONE.Equals(initialStub)))
		  {
			return FixedRateComputation.of(firstRate.Value);
		  }
		  return firstNonNull(initialStub, IborRateStubCalculation.NONE).createRateComputation(fixingDate, index, refData);
		}
		// final stub
		if (scheduleFinalStub.Present && scheduleFinalStub.get() == period)
		{
		  return firstNonNull(finalStub, IborRateStubCalculation.NONE).createRateComputation(fixingDate, index, refData);
		}
		// override rate
		double? overrideFirstRate = null;
		if (firstRegularRate != null)
		{
		  if (isFirstRegularPeriod(scheduleIndex, scheduleInitialStub.Present))
		  {
			overrideFirstRate = firstRegularRate;
		  }
		}
		else if (firstRate != null && scheduleIndex == 0)
		{
		  overrideFirstRate = firstRate;
		}
		// handle explicit reset periods, possible averaging
		if (resetScheduleFn != null)
		{
		  return createRateComputationWithResetPeriods(resetScheduleFn(period), fixingDateAdjuster, iborObservationFn, scheduleIndex, overrideFirstRate, refData);
		}
		// handle possible fixed rate
		if (overrideFirstRate != null)
		{
		  return FixedRateComputation.of(overrideFirstRate.Value);
		}
		// simple Ibor
		return IborRateComputation.of(iborObservationFn(fixingDate));
	  }

	  // reset periods have been specified, which may or may not imply averaging
	  private RateComputation createRateComputationWithResetPeriods(Schedule resetSchedule, DateAdjuster fixingDateAdjuster, System.Func<LocalDate, IborIndexObservation> iborObservationFn, int scheduleIndex, double? overrideFirstRate, ReferenceData refData)
	  {

		IList<IborAveragedFixing> fixings = new List<IborAveragedFixing>();
		for (int i = 0; i < resetSchedule.size(); i++)
		{
		  SchedulePeriod resetPeriod = resetSchedule.getPeriod(i);
		  LocalDate fixingDate = fixingDateAdjuster.adjust(fixingRelativeTo.selectBaseDate(resetPeriod));
		  if (scheduleIndex == 0 && i == 0 && firstFixingDateOffset != null)
		  {
			fixingDate = firstFixingDateOffset.resolve(refData).adjust(fixingRelativeTo.selectBaseDate(resetPeriod));
		  }
		  fixings.Add(IborAveragedFixing.builder().observation(iborObservationFn(fixingDate)).fixedRate(overrideFirstRate != null && i == 0 ? overrideFirstRate : null).weight(resetPeriods.ResetMethod == UNWEIGHTED ? 1 : resetPeriod.lengthInDays()).build());
		}
		return IborAveragedRateComputation.of(fixings);
	  }

	  // is the period the first regular period
	  private bool isFirstRegularPeriod(int scheduleIndex, bool hasInitialStub)
	  {
		if (hasInitialStub)
		{
		  return scheduleIndex == 1;
		}
		else
		{
		  return scheduleIndex == 0;
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborRateCalculation.Meta meta()
	  {
		return IborRateCalculation.Meta.INSTANCE;
	  }

	  static IborRateCalculation()
	  {
		MetaBean.register(IborRateCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborRateCalculation.Builder builder()
	  {
		return new IborRateCalculation.Builder();
	  }

	  private IborRateCalculation(DayCount dayCount, IborIndex index, ResetSchedule resetPeriods, FixingRelativeTo fixingRelativeTo, DaysAdjustment fixingDateOffset, NegativeRateMethod negativeRateMethod, double? firstRegularRate, double? firstRate, DaysAdjustment firstFixingDateOffset, IborRateStubCalculation initialStub, IborRateStubCalculation finalStub, ValueSchedule gearing, ValueSchedule spread)
	  {
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingRelativeTo, "fixingRelativeTo");
		JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		JodaBeanUtils.notNull(negativeRateMethod, "negativeRateMethod");
		this.dayCount = dayCount;
		this.index = index;
		this.resetPeriods = resetPeriods;
		this.fixingRelativeTo = fixingRelativeTo;
		this.fixingDateOffset = fixingDateOffset;
		this.negativeRateMethod = negativeRateMethod;
		this.firstRegularRate = firstRegularRate;
		this.firstRate = firstRate;
		this.firstFixingDateOffset = firstFixingDateOffset;
		this.initialStub = initialStub;
		this.finalStub = finalStub;
		this.gearing = gearing;
		this.spread = spread;
	  }

	  public override IborRateCalculation.Meta metaBean()
	  {
		return IborRateCalculation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the day count of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reset schedule, used when averaging rates, optional.
	  /// <para>
	  /// Most swaps have a single fixing for each accrual period.
	  /// This property allows multiple fixings to be defined by dividing the accrual periods into reset periods.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the reset period is the same as the accrual period.
	  /// If this property is present, then the accrual period is divided as per the information
	  /// in the reset schedule, multiple fixing dates are calculated, and rate averaging performed.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ResetSchedule> ResetPeriods
	  {
		  get
		  {
			return Optional.ofNullable(resetPeriods);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the base date that each fixing is made relative to, defaulted to 'PeriodStart'.
	  /// <para>
	  /// The fixing date is relative to either the start or end of each reset period.
	  /// </para>
	  /// <para>
	  /// Note that in most cases, the reset frequency matches the accrual frequency
	  /// and thus there is only one fixing for the accrual period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixingRelativeTo FixingRelativeTo
	  {
		  get
		  {
			return fixingRelativeTo;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the fixing date from each adjusted reset date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// Note that in most cases, the reset frequency matches the accrual frequency
	  /// and thus there is only one fixing for the accrual period.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing offset of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the negative rate method, defaulted to 'AllowNegative'.
	  /// <para>
	  /// This is used when the interest rate, observed or calculated, goes negative.
	  /// It does not apply if the rate is fixed, such as in a stub or using {@code firstRegularRate}.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.4.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NegativeRateMethod NegativeRateMethod
	  {
		  get
		  {
			return negativeRateMethod;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rate of the first regular reset period, optional.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree the rate of the first fixing
	  /// when the contract starts, and it is used in place of one observed fixing.
	  /// For all other fixings, the rate is observed via the normal fixing process.
	  /// </para>
	  /// <para>
	  /// This property allows the rate of the first reset period of the first <i>regular</i> accrual period
	  /// to be controlled. Note that if there is an initial stub, this will be the second reset period.
	  /// Other calculation elements, such as gearing or spread, still apply to the rate specified here.
	  /// </para>
	  /// <para>
	  /// If the first rate applies to the initial stub rather than the regular accrual periods
	  /// it must be specified using {@code initialStub}. Alternatively, {@code firstRate} can be used.
	  /// </para>
	  /// <para>
	  /// This property follows the definition in FpML. See also {@code firstRate}.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FirstRegularRate
	  {
		  get
		  {
			return firstRegularRate != null ? double?.of(firstRegularRate) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rate of the first reset period, which may be a stub, optional.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// In certain circumstances two counterparties agree the rate of the first fixing
	  /// when the contract starts, and it is used in place of one observed fixing.
	  /// For all other fixings, the rate is observed via the normal fixing process.
	  /// </para>
	  /// <para>
	  /// This property allows the rate of the first reset period to be controlled,
	  /// irrespective of whether that is an initial stub or a regular period.
	  /// Other calculation elements, such as gearing or spread, still apply to the rate specified here.
	  /// </para>
	  /// <para>
	  /// This property is similar to {@code firstRegularRate}.
	  /// This property operates on the first reset period, whether that is an initial stub or a regular period.
	  /// By contrast, {@code firstRegularRate} operates on the first regular period, and never on a stub.
	  /// </para>
	  /// <para>
	  /// If either {@code firstRegularRate} or {@code initialStub} are present, this property is ignored.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the first rate is observed via the normal fixing process.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FirstRate
	  {
		  get
		  {
			return firstRate != null ? double?.of(firstRate) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the first fixing date from the first adjusted reset date, optional.
	  /// <para>
	  /// If present, this offset is used instead of {@code fixingDateOffset} for the first
	  /// reset period of the swap, which will be either an initial stub or the first reset
	  /// period of the first <i>regular</i> accrual period.
	  /// </para>
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the {@code fixingDateOffset} applies to all fixings.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<DaysAdjustment> FirstFixingDateOffset
	  {
		  get
		  {
			return Optional.ofNullable(firstFixingDateOffset);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rate to be used in initial stub, optional.
	  /// <para>
	  /// The initial stub of a swap may have different rate rules to the regular accrual periods.
	  /// A fixed rate may be specified, a different floating rate or a linearly interpolated floating rate.
	  /// This may not be present if there is no initial stub, or if the index during the stub is the same
	  /// as the main floating rate index.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the main index applies during any initial stub.
	  /// If this property is present and there is no initial stub, it is ignored.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IborRateStubCalculation> InitialStub
	  {
		  get
		  {
			return Optional.ofNullable(initialStub);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rate to be used in final stub, optional.
	  /// <para>
	  /// The final stub of a swap may have different rate rules to the regular accrual periods.
	  /// A fixed rate may be specified, a different floating rate or a linearly interpolated floating rate.
	  /// This may not be present if there is no final stub, or if the index during the stub is the same
	  /// as the main floating rate index.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the main index applies during any final stub.
	  /// If this property is present and there is no final stub, it is ignored.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IborRateStubCalculation> FinalStub
	  {
		  get
		  {
			return Optional.ofNullable(finalStub);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the gearing multiplier, optional.
	  /// <para>
	  /// This defines the gearing as an initial value and a list of adjustments.
	  /// The gearing is only permitted to change at accrual period boundaries.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the fixing rate is multiplied by the gearing.
	  /// A gearing of 1 has no effect.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then no gearing applies.
	  /// </para>
	  /// <para>
	  /// Gearing is also known as <i>leverage</i>.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ValueSchedule> Gearing
	  {
		  get
		  {
			return Optional.ofNullable(gearing);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the spread rate, with a 5% rate expressed as 0.05, optional.
	  /// <para>
	  /// This defines the spread as an initial value and a list of adjustments.
	  /// The spread is only permitted to change at accrual period boundaries.
	  /// Spread is a per annum rate.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the spread is added to the fixing rate.
	  /// A spread of 0 has no effect.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then no spread applies.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2e.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ValueSchedule> Spread
	  {
		  get
		  {
			return Optional.ofNullable(spread);
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
		  IborRateCalculation other = (IborRateCalculation) obj;
		  return JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(resetPeriods, other.resetPeriods) && JodaBeanUtils.equal(fixingRelativeTo, other.fixingRelativeTo) && JodaBeanUtils.equal(fixingDateOffset, other.fixingDateOffset) && JodaBeanUtils.equal(negativeRateMethod, other.negativeRateMethod) && JodaBeanUtils.equal(firstRegularRate, other.firstRegularRate) && JodaBeanUtils.equal(firstRate, other.firstRate) && JodaBeanUtils.equal(firstFixingDateOffset, other.firstFixingDateOffset) && JodaBeanUtils.equal(initialStub, other.initialStub) && JodaBeanUtils.equal(finalStub, other.finalStub) && JodaBeanUtils.equal(gearing, other.gearing) && JodaBeanUtils.equal(spread, other.spread);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(resetPeriods);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingRelativeTo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(negativeRateMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstRegularRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstFixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialStub);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(finalStub);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(gearing);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spread);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(448);
		buf.Append("IborRateCalculation{");
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("resetPeriods").Append('=').Append(resetPeriods).Append(',').Append(' ');
		buf.Append("fixingRelativeTo").Append('=').Append(fixingRelativeTo).Append(',').Append(' ');
		buf.Append("fixingDateOffset").Append('=').Append(fixingDateOffset).Append(',').Append(' ');
		buf.Append("negativeRateMethod").Append('=').Append(negativeRateMethod).Append(',').Append(' ');
		buf.Append("firstRegularRate").Append('=').Append(firstRegularRate).Append(',').Append(' ');
		buf.Append("firstRate").Append('=').Append(firstRate).Append(',').Append(' ');
		buf.Append("firstFixingDateOffset").Append('=').Append(firstFixingDateOffset).Append(',').Append(' ');
		buf.Append("initialStub").Append('=').Append(initialStub).Append(',').Append(' ');
		buf.Append("finalStub").Append('=').Append(finalStub).Append(',').Append(' ');
		buf.Append("gearing").Append('=').Append(gearing).Append(',').Append(' ');
		buf.Append("spread").Append('=').Append(JodaBeanUtils.ToString(spread));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborRateCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(IborRateCalculation), typeof(DayCount));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(IborRateCalculation), typeof(IborIndex));
			  resetPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "resetPeriods", typeof(IborRateCalculation), typeof(ResetSchedule));
			  fixingRelativeTo_Renamed = DirectMetaProperty.ofImmutable(this, "fixingRelativeTo", typeof(IborRateCalculation), typeof(FixingRelativeTo));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(IborRateCalculation), typeof(DaysAdjustment));
			  negativeRateMethod_Renamed = DirectMetaProperty.ofImmutable(this, "negativeRateMethod", typeof(IborRateCalculation), typeof(NegativeRateMethod));
			  firstRegularRate_Renamed = DirectMetaProperty.ofImmutable(this, "firstRegularRate", typeof(IborRateCalculation), typeof(Double));
			  firstRate_Renamed = DirectMetaProperty.ofImmutable(this, "firstRate", typeof(IborRateCalculation), typeof(Double));
			  firstFixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "firstFixingDateOffset", typeof(IborRateCalculation), typeof(DaysAdjustment));
			  initialStub_Renamed = DirectMetaProperty.ofImmutable(this, "initialStub", typeof(IborRateCalculation), typeof(IborRateStubCalculation));
			  finalStub_Renamed = DirectMetaProperty.ofImmutable(this, "finalStub", typeof(IborRateCalculation), typeof(IborRateStubCalculation));
			  gearing_Renamed = DirectMetaProperty.ofImmutable(this, "gearing", typeof(IborRateCalculation), typeof(ValueSchedule));
			  spread_Renamed = DirectMetaProperty.ofImmutable(this, "spread", typeof(IborRateCalculation), typeof(ValueSchedule));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "dayCount", "index", "resetPeriods", "fixingRelativeTo", "fixingDateOffset", "negativeRateMethod", "firstRegularRate", "firstRate", "firstFixingDateOffset", "initialStub", "finalStub", "gearing", "spread");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code resetPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResetSchedule> resetPeriods_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixingRelativeTo> fixingRelativeTo_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code negativeRateMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<NegativeRateMethod> negativeRateMethod_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstRegularRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> firstRegularRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> firstRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstFixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> firstFixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code initialStub} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateStubCalculation> initialStub_Renamed;
		/// <summary>
		/// The meta-property for the {@code finalStub} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateStubCalculation> finalStub_Renamed;
		/// <summary>
		/// The meta-property for the {@code gearing} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> gearing_Renamed;
		/// <summary>
		/// The meta-property for the {@code spread} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> spread_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "dayCount", "index", "resetPeriods", "fixingRelativeTo", "fixingDateOffset", "negativeRateMethod", "firstRegularRate", "firstRate", "firstFixingDateOffset", "initialStub", "finalStub", "gearing", "spread");
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
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1272973693: // resetPeriods
			  return resetPeriods_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1969081334: // negativeRateMethod
			  return negativeRateMethod_Renamed;
			case 570227148: // firstRegularRate
			  return firstRegularRate_Renamed;
			case 132955056: // firstRate
			  return firstRate_Renamed;
			case 2022439998: // firstFixingDateOffset
			  return firstFixingDateOffset_Renamed;
			case 1233359378: // initialStub
			  return initialStub_Renamed;
			case 355242820: // finalStub
			  return finalStub_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
			case -895684237: // spread
			  return spread_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborRateCalculation.Builder builder()
		{
		  return new IborRateCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborRateCalculation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code resetPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResetSchedule> resetPeriods()
		{
		  return resetPeriods_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixingRelativeTo> fixingRelativeTo()
		{
		  return fixingRelativeTo_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code negativeRateMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<NegativeRateMethod> negativeRateMethod()
		{
		  return negativeRateMethod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstRegularRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> firstRegularRate()
		{
		  return firstRegularRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> firstRate()
		{
		  return firstRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstFixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> firstFixingDateOffset()
		{
		  return firstFixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code initialStub} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateStubCalculation> initialStub()
		{
		  return initialStub_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code finalStub} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateStubCalculation> finalStub()
		{
		  return finalStub_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code gearing} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> gearing()
		{
		  return gearing_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spread} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> spread()
		{
		  return spread_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1905311443: // dayCount
			  return ((IborRateCalculation) bean).DayCount;
			case 100346066: // index
			  return ((IborRateCalculation) bean).Index;
			case -1272973693: // resetPeriods
			  return ((IborRateCalculation) bean).resetPeriods;
			case 232554996: // fixingRelativeTo
			  return ((IborRateCalculation) bean).FixingRelativeTo;
			case 873743726: // fixingDateOffset
			  return ((IborRateCalculation) bean).FixingDateOffset;
			case 1969081334: // negativeRateMethod
			  return ((IborRateCalculation) bean).NegativeRateMethod;
			case 570227148: // firstRegularRate
			  return ((IborRateCalculation) bean).firstRegularRate;
			case 132955056: // firstRate
			  return ((IborRateCalculation) bean).firstRate;
			case 2022439998: // firstFixingDateOffset
			  return ((IborRateCalculation) bean).firstFixingDateOffset;
			case 1233359378: // initialStub
			  return ((IborRateCalculation) bean).initialStub;
			case 355242820: // finalStub
			  return ((IborRateCalculation) bean).finalStub;
			case -91774989: // gearing
			  return ((IborRateCalculation) bean).gearing;
			case -895684237: // spread
			  return ((IborRateCalculation) bean).spread;
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
	  /// The bean-builder for {@code IborRateCalculation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborRateCalculation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ResetSchedule resetPeriods_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixingRelativeTo fixingRelativeTo_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal NegativeRateMethod negativeRateMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? firstRegularRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? firstRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment firstFixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateStubCalculation initialStub_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateStubCalculation finalStub_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule gearing_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule spread_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(IborRateCalculation beanToCopy)
		{
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.index_Renamed = beanToCopy.Index;
		  this.resetPeriods_Renamed = beanToCopy.resetPeriods;
		  this.fixingRelativeTo_Renamed = beanToCopy.FixingRelativeTo;
		  this.fixingDateOffset_Renamed = beanToCopy.FixingDateOffset;
		  this.negativeRateMethod_Renamed = beanToCopy.NegativeRateMethod;
		  this.firstRegularRate_Renamed = beanToCopy.firstRegularRate;
		  this.firstRate_Renamed = beanToCopy.firstRate;
		  this.firstFixingDateOffset_Renamed = beanToCopy.firstFixingDateOffset;
		  this.initialStub_Renamed = beanToCopy.initialStub;
		  this.finalStub_Renamed = beanToCopy.finalStub;
		  this.gearing_Renamed = beanToCopy.gearing;
		  this.spread_Renamed = beanToCopy.spread;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1272973693: // resetPeriods
			  return resetPeriods_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1969081334: // negativeRateMethod
			  return negativeRateMethod_Renamed;
			case 570227148: // firstRegularRate
			  return firstRegularRate_Renamed;
			case 132955056: // firstRate
			  return firstRate_Renamed;
			case 2022439998: // firstFixingDateOffset
			  return firstFixingDateOffset_Renamed;
			case 1233359378: // initialStub
			  return initialStub_Renamed;
			case 355242820: // finalStub
			  return finalStub_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
			case -895684237: // spread
			  return spread_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case -1272973693: // resetPeriods
			  this.resetPeriods_Renamed = (ResetSchedule) newValue;
			  break;
			case 232554996: // fixingRelativeTo
			  this.fixingRelativeTo_Renamed = (FixingRelativeTo) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1969081334: // negativeRateMethod
			  this.negativeRateMethod_Renamed = (NegativeRateMethod) newValue;
			  break;
			case 570227148: // firstRegularRate
			  this.firstRegularRate_Renamed = (double?) newValue;
			  break;
			case 132955056: // firstRate
			  this.firstRate_Renamed = (double?) newValue;
			  break;
			case 2022439998: // firstFixingDateOffset
			  this.firstFixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1233359378: // initialStub
			  this.initialStub_Renamed = (IborRateStubCalculation) newValue;
			  break;
			case 355242820: // finalStub
			  this.finalStub_Renamed = (IborRateStubCalculation) newValue;
			  break;
			case -91774989: // gearing
			  this.gearing_Renamed = (ValueSchedule) newValue;
			  break;
			case -895684237: // spread
			  this.spread_Renamed = (ValueSchedule) newValue;
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

		public override IborRateCalculation build()
		{
		  preBuild(this);
		  return new IborRateCalculation(dayCount_Renamed, index_Renamed, resetPeriods_Renamed, fixingRelativeTo_Renamed, fixingDateOffset_Renamed, negativeRateMethod_Renamed, firstRegularRate_Renamed, firstRate_Renamed, firstFixingDateOffset_Renamed, initialStub_Renamed, finalStub_Renamed, gearing_Renamed, spread_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the day count convention.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// </para>
		/// <para>
		/// When building, this will default to the day count of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the Ibor index.
		/// <para>
		/// The rate to be paid is based on this index
		/// It will be a well known market index such as 'GBP-LIBOR-3M'.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the reset schedule, used when averaging rates, optional.
		/// <para>
		/// Most swaps have a single fixing for each accrual period.
		/// This property allows multiple fixings to be defined by dividing the accrual periods into reset periods.
		/// </para>
		/// <para>
		/// If this property is not present, then the reset period is the same as the accrual period.
		/// If this property is present, then the accrual period is divided as per the information
		/// in the reset schedule, multiple fixing dates are calculated, and rate averaging performed.
		/// </para>
		/// </summary>
		/// <param name="resetPeriods">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder resetPeriods(ResetSchedule resetPeriods)
		{
		  this.resetPeriods_Renamed = resetPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the base date that each fixing is made relative to, defaulted to 'PeriodStart'.
		/// <para>
		/// The fixing date is relative to either the start or end of each reset period.
		/// </para>
		/// <para>
		/// Note that in most cases, the reset frequency matches the accrual frequency
		/// and thus there is only one fixing for the accrual period.
		/// </para>
		/// </summary>
		/// <param name="fixingRelativeTo">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingRelativeTo(FixingRelativeTo fixingRelativeTo)
		{
		  JodaBeanUtils.notNull(fixingRelativeTo, "fixingRelativeTo");
		  this.fixingRelativeTo_Renamed = fixingRelativeTo;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the fixing date from each adjusted reset date.
		/// <para>
		/// The offset is applied to the base date specified by {@code fixingRelativeTo}.
		/// The offset is typically a negative number of business days.
		/// </para>
		/// <para>
		/// Note that in most cases, the reset frequency matches the accrual frequency
		/// and thus there is only one fixing for the accrual period.
		/// </para>
		/// <para>
		/// When building, this will default to the fixing offset of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the negative rate method, defaulted to 'AllowNegative'.
		/// <para>
		/// This is used when the interest rate, observed or calculated, goes negative.
		/// It does not apply if the rate is fixed, such as in a stub or using {@code firstRegularRate}.
		/// </para>
		/// <para>
		/// Defined by the 2006 ISDA definitions article 6.4.
		/// </para>
		/// </summary>
		/// <param name="negativeRateMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder negativeRateMethod(NegativeRateMethod negativeRateMethod)
		{
		  JodaBeanUtils.notNull(negativeRateMethod, "negativeRateMethod");
		  this.negativeRateMethod_Renamed = negativeRateMethod;
		  return this;
		}

		/// <summary>
		/// Sets the rate of the first regular reset period, optional.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// In certain circumstances two counterparties agree the rate of the first fixing
		/// when the contract starts, and it is used in place of one observed fixing.
		/// For all other fixings, the rate is observed via the normal fixing process.
		/// </para>
		/// <para>
		/// This property allows the rate of the first reset period of the first <i>regular</i> accrual period
		/// to be controlled. Note that if there is an initial stub, this will be the second reset period.
		/// Other calculation elements, such as gearing or spread, still apply to the rate specified here.
		/// </para>
		/// <para>
		/// If the first rate applies to the initial stub rather than the regular accrual periods
		/// it must be specified using {@code initialStub}. Alternatively, {@code firstRate} can be used.
		/// </para>
		/// <para>
		/// This property follows the definition in FpML. See also {@code firstRate}.
		/// </para>
		/// </summary>
		/// <param name="firstRegularRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstRegularRate(double? firstRegularRate)
		{
		  this.firstRegularRate_Renamed = firstRegularRate;
		  return this;
		}

		/// <summary>
		/// Sets the rate of the first reset period, which may be a stub, optional.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// In certain circumstances two counterparties agree the rate of the first fixing
		/// when the contract starts, and it is used in place of one observed fixing.
		/// For all other fixings, the rate is observed via the normal fixing process.
		/// </para>
		/// <para>
		/// This property allows the rate of the first reset period to be controlled,
		/// irrespective of whether that is an initial stub or a regular period.
		/// Other calculation elements, such as gearing or spread, still apply to the rate specified here.
		/// </para>
		/// <para>
		/// This property is similar to {@code firstRegularRate}.
		/// This property operates on the first reset period, whether that is an initial stub or a regular period.
		/// By contrast, {@code firstRegularRate} operates on the first regular period, and never on a stub.
		/// </para>
		/// <para>
		/// If either {@code firstRegularRate} or {@code initialStub} are present, this property is ignored.
		/// </para>
		/// <para>
		/// If this property is not present, then the first rate is observed via the normal fixing process.
		/// </para>
		/// </summary>
		/// <param name="firstRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstRate(double? firstRate)
		{
		  this.firstRate_Renamed = firstRate;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the first fixing date from the first adjusted reset date, optional.
		/// <para>
		/// If present, this offset is used instead of {@code fixingDateOffset} for the first
		/// reset period of the swap, which will be either an initial stub or the first reset
		/// period of the first <i>regular</i> accrual period.
		/// </para>
		/// <para>
		/// The offset is applied to the base date specified by {@code fixingRelativeTo}.
		/// The offset is typically a negative number of business days.
		/// </para>
		/// <para>
		/// If this property is not present, then the {@code fixingDateOffset} applies to all fixings.
		/// </para>
		/// </summary>
		/// <param name="firstFixingDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstFixingDateOffset(DaysAdjustment firstFixingDateOffset)
		{
		  this.firstFixingDateOffset_Renamed = firstFixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the rate to be used in initial stub, optional.
		/// <para>
		/// The initial stub of a swap may have different rate rules to the regular accrual periods.
		/// A fixed rate may be specified, a different floating rate or a linearly interpolated floating rate.
		/// This may not be present if there is no initial stub, or if the index during the stub is the same
		/// as the main floating rate index.
		/// </para>
		/// <para>
		/// If this property is not present, then the main index applies during any initial stub.
		/// If this property is present and there is no initial stub, it is ignored.
		/// </para>
		/// </summary>
		/// <param name="initialStub">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialStub(IborRateStubCalculation initialStub)
		{
		  this.initialStub_Renamed = initialStub;
		  return this;
		}

		/// <summary>
		/// Sets the rate to be used in final stub, optional.
		/// <para>
		/// The final stub of a swap may have different rate rules to the regular accrual periods.
		/// A fixed rate may be specified, a different floating rate or a linearly interpolated floating rate.
		/// This may not be present if there is no final stub, or if the index during the stub is the same
		/// as the main floating rate index.
		/// </para>
		/// <para>
		/// If this property is not present, then the main index applies during any final stub.
		/// If this property is present and there is no final stub, it is ignored.
		/// </para>
		/// </summary>
		/// <param name="finalStub">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder finalStub(IborRateStubCalculation finalStub)
		{
		  this.finalStub_Renamed = finalStub;
		  return this;
		}

		/// <summary>
		/// Sets the gearing multiplier, optional.
		/// <para>
		/// This defines the gearing as an initial value and a list of adjustments.
		/// The gearing is only permitted to change at accrual period boundaries.
		/// </para>
		/// <para>
		/// When calculating the rate, the fixing rate is multiplied by the gearing.
		/// A gearing of 1 has no effect.
		/// If both gearing and spread exist, then the gearing is applied first.
		/// </para>
		/// <para>
		/// If this property is not present, then no gearing applies.
		/// </para>
		/// <para>
		/// Gearing is also known as <i>leverage</i>.
		/// </para>
		/// </summary>
		/// <param name="gearing">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder gearing(ValueSchedule gearing)
		{
		  this.gearing_Renamed = gearing;
		  return this;
		}

		/// <summary>
		/// Sets the spread rate, with a 5% rate expressed as 0.05, optional.
		/// <para>
		/// This defines the spread as an initial value and a list of adjustments.
		/// The spread is only permitted to change at accrual period boundaries.
		/// Spread is a per annum rate.
		/// </para>
		/// <para>
		/// When calculating the rate, the spread is added to the fixing rate.
		/// A spread of 0 has no effect.
		/// If both gearing and spread exist, then the gearing is applied first.
		/// </para>
		/// <para>
		/// If this property is not present, then no spread applies.
		/// </para>
		/// <para>
		/// Defined by the 2006 ISDA definitions article 6.2e.
		/// </para>
		/// </summary>
		/// <param name="spread">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spread(ValueSchedule spread)
		{
		  this.spread_Renamed = spread;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(448);
		  buf.Append("IborRateCalculation.Builder{");
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("resetPeriods").Append('=').Append(JodaBeanUtils.ToString(resetPeriods_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingRelativeTo").Append('=').Append(JodaBeanUtils.ToString(fixingRelativeTo_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("negativeRateMethod").Append('=').Append(JodaBeanUtils.ToString(negativeRateMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("firstRegularRate").Append('=').Append(JodaBeanUtils.ToString(firstRegularRate_Renamed)).Append(',').Append(' ');
		  buf.Append("firstRate").Append('=').Append(JodaBeanUtils.ToString(firstRate_Renamed)).Append(',').Append(' ');
		  buf.Append("firstFixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(firstFixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("initialStub").Append('=').Append(JodaBeanUtils.ToString(initialStub_Renamed)).Append(',').Append(' ');
		  buf.Append("finalStub").Append('=').Append(JodaBeanUtils.ToString(finalStub_Renamed)).Append(',').Append(' ');
		  buf.Append("gearing").Append('=').Append(JodaBeanUtils.ToString(gearing_Renamed)).Append(',').Append(' ');
		  buf.Append("spread").Append('=').Append(JodaBeanUtils.ToString(spread_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
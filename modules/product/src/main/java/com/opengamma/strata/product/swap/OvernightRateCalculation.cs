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
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using OvernightRateComputation = com.opengamma.strata.product.rate.OvernightRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Defines the calculation of a floating rate swap leg based on an Overnight index.
	/// <para>
	/// This defines the data necessary to calculate the amount payable on the leg.
	/// The amount is based on the observed value of an Overnight index such as 'GBP-SONIA' or 'USD-FED-FUND'.
	/// </para>
	/// <para>
	/// The index is observed for each business day and averaged or compounded to produce a rate.
	/// The reset periods correspond to each business day and are inferred from the accrual period dates.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class OvernightRateCalculation implements RateCalculation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class OvernightRateCalculation : RateCalculation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
		private readonly DayCount dayCount;
	  /// <summary>
	  /// The Overnight index.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-SONIA'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.OvernightIndex index;
	  private readonly OvernightIndex index;
	  /// <summary>
	  /// The method of accruing overnight interest, defaulted to 'Compounded'.
	  /// <para>
	  /// Two methods of accrual are supported - compounding and averaging.
	  /// Averaging is primarily related to the 'USD-FED-FUND' index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final OvernightAccrualMethod accrualMethod;
	  private readonly OvernightAccrualMethod accrualMethod;
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
	  /// The number of business days before the end of the period that the rate is cut off, defaulted to zero.
	  /// <para>
	  /// When a rate cut-off applies, the final daily rate is determined this number of days
	  /// before the end of the period, with any subsequent days having the same rate.
	  /// </para>
	  /// <para>
	  /// The amount must be zero or positive.
	  /// A value of zero or one will have no effect on the standard calculation.
	  /// The fixing holiday calendar of the index is used to determine business days.
	  /// </para>
	  /// <para>
	  /// For example, a value of {@code 3} means that the rate observed on
	  /// {@code (periodEndDate - 3 business days)} is also to be used on
	  /// {@code (periodEndDate - 2 business days)} and {@code (periodEndDate - 1 business day)}.
	  /// </para>
	  /// <para>
	  /// If there are multiple accrual periods in the payment period, then this
	  /// will only apply to the last accrual period in the payment period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final int rateCutOffDays;
	  private readonly int rateCutOffDays;

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
	  /// The spread rate, optional.
	  /// A 5% rate will be expressed as 0.05.
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
	  /// Obtains a rate calculation for the specified index with accrual by compounding.
	  /// <para>
	  /// The calculation will use the day count of the index.
	  /// All optional fields will be set to their default values.
	  /// Thus, there will be no spread, gearing or rate cut-off.
	  /// If this method provides insufficient control, use the <seealso cref="#builder() builder"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <returns> the calculation </returns>
	  public static OvernightRateCalculation of(OvernightIndex index)
	  {
		return OvernightRateCalculation.builder().index(index).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.accrualMethod(OvernightAccrualMethod.COMPOUNDED);
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
		}
	  }

	  //-------------------------------------------------------------------------
	  public SwapLegType Type
	  {
		  get
		  {
			return SwapLegType.OVERNIGHT;
		  }
	  }

	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		builder.add(index.Currency);
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(index);
	  }

	  public ImmutableList<RateAccrualPeriod> createAccrualPeriods(Schedule accrualSchedule, Schedule paymentSchedule, ReferenceData refData)
	  {

		// resolve data by schedule
		DoubleArray resolvedGearings = firstNonNull(gearing, ALWAYS_1).resolveValues(accrualSchedule);
		DoubleArray resolvedSpreads = firstNonNull(spread, ALWAYS_0).resolveValues(accrualSchedule);
		// build accrual periods
		ImmutableList.Builder<RateAccrualPeriod> accrualPeriods = ImmutableList.builder();
		for (int i = 0; i < accrualSchedule.size(); i++)
		{
		  SchedulePeriod period = accrualSchedule.getPeriod(i);
		  double yearFraction = period.yearFraction(dayCount, accrualSchedule);
		  RateComputation rateComputation = createRateComputation(period, paymentSchedule, refData);
		  accrualPeriods.add(new RateAccrualPeriod(period, yearFraction, rateComputation, resolvedGearings.get(i), resolvedSpreads.get(i), negativeRateMethod));
		}
		return accrualPeriods.build();
	  }

	  // creates the rate computation
	  private RateComputation createRateComputation(SchedulePeriod period, Schedule paymentSchedule, ReferenceData refData)
	  {
		int effectiveRateCutOffDaysOffset = (isLastAccrualInPaymentPeriod(period, paymentSchedule) ? rateCutOffDays : 0);
		return OvernightRateComputation.of(index, period.StartDate, period.EndDate, effectiveRateCutOffDaysOffset, accrualMethod, refData);
	  }

	  // rate cut-off only applies to the last accrual period in a payment period
	  private bool isLastAccrualInPaymentPeriod(SchedulePeriod period, Schedule paymentSchedule)
	  {
		if (rateCutOffDays == 0)
		{
		  return true;
		}
		return paymentSchedule.Periods.Any(pp => pp.UnadjustedEndDate.Equals(period.UnadjustedEndDate));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightRateCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static OvernightRateCalculation.Meta meta()
	  {
		return OvernightRateCalculation.Meta.INSTANCE;
	  }

	  static OvernightRateCalculation()
	  {
		MetaBean.register(OvernightRateCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static OvernightRateCalculation.Builder builder()
	  {
		return new OvernightRateCalculation.Builder();
	  }

	  private OvernightRateCalculation(DayCount dayCount, OvernightIndex index, OvernightAccrualMethod accrualMethod, NegativeRateMethod negativeRateMethod, int rateCutOffDays, ValueSchedule gearing, ValueSchedule spread)
	  {
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(accrualMethod, "accrualMethod");
		JodaBeanUtils.notNull(negativeRateMethod, "negativeRateMethod");
		ArgChecker.notNegative(rateCutOffDays, "rateCutOffDays");
		this.dayCount = dayCount;
		this.index = index;
		this.accrualMethod = accrualMethod;
		this.negativeRateMethod = negativeRateMethod;
		this.rateCutOffDays = rateCutOffDays;
		this.gearing = gearing;
		this.spread = spread;
	  }

	  public override OvernightRateCalculation.Meta metaBean()
	  {
		return OvernightRateCalculation.Meta.INSTANCE;
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
	  /// Gets the Overnight index.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-SONIA'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the method of accruing overnight interest, defaulted to 'Compounded'.
	  /// <para>
	  /// Two methods of accrual are supported - compounding and averaging.
	  /// Averaging is primarily related to the 'USD-FED-FUND' index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightAccrualMethod AccrualMethod
	  {
		  get
		  {
			return accrualMethod;
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
	  /// Gets the number of business days before the end of the period that the rate is cut off, defaulted to zero.
	  /// <para>
	  /// When a rate cut-off applies, the final daily rate is determined this number of days
	  /// before the end of the period, with any subsequent days having the same rate.
	  /// </para>
	  /// <para>
	  /// The amount must be zero or positive.
	  /// A value of zero or one will have no effect on the standard calculation.
	  /// The fixing holiday calendar of the index is used to determine business days.
	  /// </para>
	  /// <para>
	  /// For example, a value of {@code 3} means that the rate observed on
	  /// {@code (periodEndDate - 3 business days)} is also to be used on
	  /// {@code (periodEndDate - 2 business days)} and {@code (periodEndDate - 1 business day)}.
	  /// </para>
	  /// <para>
	  /// If there are multiple accrual periods in the payment period, then this
	  /// will only apply to the last accrual period in the payment period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public int RateCutOffDays
	  {
		  get
		  {
			return rateCutOffDays;
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
	  /// Gets the spread rate, optional.
	  /// A 5% rate will be expressed as 0.05.
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
		  OvernightRateCalculation other = (OvernightRateCalculation) obj;
		  return JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(accrualMethod, other.accrualMethod) && JodaBeanUtils.equal(negativeRateMethod, other.negativeRateMethod) && (rateCutOffDays == other.rateCutOffDays) && JodaBeanUtils.equal(gearing, other.gearing) && JodaBeanUtils.equal(spread, other.spread);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(negativeRateMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateCutOffDays);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(gearing);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spread);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("OvernightRateCalculation{");
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("accrualMethod").Append('=').Append(accrualMethod).Append(',').Append(' ');
		buf.Append("negativeRateMethod").Append('=').Append(negativeRateMethod).Append(',').Append(' ');
		buf.Append("rateCutOffDays").Append('=').Append(rateCutOffDays).Append(',').Append(' ');
		buf.Append("gearing").Append('=').Append(gearing).Append(',').Append(' ');
		buf.Append("spread").Append('=').Append(JodaBeanUtils.ToString(spread));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightRateCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(OvernightRateCalculation), typeof(DayCount));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(OvernightRateCalculation), typeof(OvernightIndex));
			  accrualMethod_Renamed = DirectMetaProperty.ofImmutable(this, "accrualMethod", typeof(OvernightRateCalculation), typeof(OvernightAccrualMethod));
			  negativeRateMethod_Renamed = DirectMetaProperty.ofImmutable(this, "negativeRateMethod", typeof(OvernightRateCalculation), typeof(NegativeRateMethod));
			  rateCutOffDays_Renamed = DirectMetaProperty.ofImmutable(this, "rateCutOffDays", typeof(OvernightRateCalculation), Integer.TYPE);
			  gearing_Renamed = DirectMetaProperty.ofImmutable(this, "gearing", typeof(OvernightRateCalculation), typeof(ValueSchedule));
			  spread_Renamed = DirectMetaProperty.ofImmutable(this, "spread", typeof(OvernightRateCalculation), typeof(ValueSchedule));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "dayCount", "index", "accrualMethod", "negativeRateMethod", "rateCutOffDays", "gearing", "spread");
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
		internal MetaProperty<OvernightIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightAccrualMethod> accrualMethod_Renamed;
		/// <summary>
		/// The meta-property for the {@code negativeRateMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<NegativeRateMethod> negativeRateMethod_Renamed;
		/// <summary>
		/// The meta-property for the {@code rateCutOffDays} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> rateCutOffDays_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "dayCount", "index", "accrualMethod", "negativeRateMethod", "rateCutOffDays", "gearing", "spread");
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
			case -1335729296: // accrualMethod
			  return accrualMethod_Renamed;
			case 1969081334: // negativeRateMethod
			  return negativeRateMethod_Renamed;
			case -92095804: // rateCutOffDays
			  return rateCutOffDays_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
			case -895684237: // spread
			  return spread_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override OvernightRateCalculation.Builder builder()
		{
		  return new OvernightRateCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(OvernightRateCalculation);
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
		public MetaProperty<OvernightIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightAccrualMethod> accrualMethod()
		{
		  return accrualMethod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code negativeRateMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<NegativeRateMethod> negativeRateMethod()
		{
		  return negativeRateMethod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rateCutOffDays} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> rateCutOffDays()
		{
		  return rateCutOffDays_Renamed;
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
			  return ((OvernightRateCalculation) bean).DayCount;
			case 100346066: // index
			  return ((OvernightRateCalculation) bean).Index;
			case -1335729296: // accrualMethod
			  return ((OvernightRateCalculation) bean).AccrualMethod;
			case 1969081334: // negativeRateMethod
			  return ((OvernightRateCalculation) bean).NegativeRateMethod;
			case -92095804: // rateCutOffDays
			  return ((OvernightRateCalculation) bean).RateCutOffDays;
			case -91774989: // gearing
			  return ((OvernightRateCalculation) bean).gearing;
			case -895684237: // spread
			  return ((OvernightRateCalculation) bean).spread;
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
	  /// The bean-builder for {@code OvernightRateCalculation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<OvernightRateCalculation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightAccrualMethod accrualMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal NegativeRateMethod negativeRateMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal int rateCutOffDays_Renamed;
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
		internal Builder(OvernightRateCalculation beanToCopy)
		{
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.index_Renamed = beanToCopy.Index;
		  this.accrualMethod_Renamed = beanToCopy.AccrualMethod;
		  this.negativeRateMethod_Renamed = beanToCopy.NegativeRateMethod;
		  this.rateCutOffDays_Renamed = beanToCopy.RateCutOffDays;
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
			case -1335729296: // accrualMethod
			  return accrualMethod_Renamed;
			case 1969081334: // negativeRateMethod
			  return negativeRateMethod_Renamed;
			case -92095804: // rateCutOffDays
			  return rateCutOffDays_Renamed;
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
			  this.index_Renamed = (OvernightIndex) newValue;
			  break;
			case -1335729296: // accrualMethod
			  this.accrualMethod_Renamed = (OvernightAccrualMethod) newValue;
			  break;
			case 1969081334: // negativeRateMethod
			  this.negativeRateMethod_Renamed = (NegativeRateMethod) newValue;
			  break;
			case -92095804: // rateCutOffDays
			  this.rateCutOffDays_Renamed = (int?) newValue.Value;
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

		public override OvernightRateCalculation build()
		{
		  preBuild(this);
		  return new OvernightRateCalculation(dayCount_Renamed, index_Renamed, accrualMethod_Renamed, negativeRateMethod_Renamed, rateCutOffDays_Renamed, gearing_Renamed, spread_Renamed);
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
		/// Sets the Overnight index.
		/// <para>
		/// The rate to be paid is based on this index
		/// It will be a well known market index such as 'GBP-SONIA'.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(OvernightIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the method of accruing overnight interest, defaulted to 'Compounded'.
		/// <para>
		/// Two methods of accrual are supported - compounding and averaging.
		/// Averaging is primarily related to the 'USD-FED-FUND' index.
		/// </para>
		/// </summary>
		/// <param name="accrualMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualMethod(OvernightAccrualMethod accrualMethod)
		{
		  JodaBeanUtils.notNull(accrualMethod, "accrualMethod");
		  this.accrualMethod_Renamed = accrualMethod;
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
		/// Sets the number of business days before the end of the period that the rate is cut off, defaulted to zero.
		/// <para>
		/// When a rate cut-off applies, the final daily rate is determined this number of days
		/// before the end of the period, with any subsequent days having the same rate.
		/// </para>
		/// <para>
		/// The amount must be zero or positive.
		/// A value of zero or one will have no effect on the standard calculation.
		/// The fixing holiday calendar of the index is used to determine business days.
		/// </para>
		/// <para>
		/// For example, a value of {@code 3} means that the rate observed on
		/// {@code (periodEndDate - 3 business days)} is also to be used on
		/// {@code (periodEndDate - 2 business days)} and {@code (periodEndDate - 1 business day)}.
		/// </para>
		/// <para>
		/// If there are multiple accrual periods in the payment period, then this
		/// will only apply to the last accrual period in the payment period.
		/// </para>
		/// </summary>
		/// <param name="rateCutOffDays">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rateCutOffDays(int rateCutOffDays)
		{
		  ArgChecker.notNegative(rateCutOffDays, "rateCutOffDays");
		  this.rateCutOffDays_Renamed = rateCutOffDays;
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
		/// Sets the spread rate, optional.
		/// A 5% rate will be expressed as 0.05.
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
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("OvernightRateCalculation.Builder{");
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualMethod").Append('=').Append(JodaBeanUtils.ToString(accrualMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("negativeRateMethod").Append('=').Append(JodaBeanUtils.ToString(negativeRateMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("rateCutOffDays").Append('=').Append(JodaBeanUtils.ToString(rateCutOffDays_Renamed)).Append(',').Append(' ');
		  buf.Append("gearing").Append('=').Append(JodaBeanUtils.ToString(gearing_Renamed)).Append(',').Append(' ');
		  buf.Append("spread").Append('=').Append(JodaBeanUtils.ToString(spread_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
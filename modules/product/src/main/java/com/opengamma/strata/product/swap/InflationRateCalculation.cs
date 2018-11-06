using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.value.ValueSchedule.ALWAYS_1;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
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
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;
	using InflationInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationInterpolatedRateComputation;
	using InflationMonthlyRateComputation = com.opengamma.strata.product.rate.InflationMonthlyRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Defines the calculation of a swap leg of a zero-coupon inflation coupon based on a price index.
	/// <para>
	/// This defines the data necessary to calculate the amount payable on the leg.
	/// The amount is based on the observed value of a price index.
	/// </para>
	/// <para>
	/// The index for a given month is given in the yield curve or in the time series.
	/// The pay-off for a unit notional is {@code (Index_End / Index_Start - 1)}.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class InflationRateCalculation implements RateCalculation, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InflationRateCalculation : RateCalculation, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.PriceIndex index;
		private readonly PriceIndex index;
	  /// <summary>
	  /// The positive period between the price index and the accrual date,
	  /// typically a number of months.
	  /// <para>
	  /// A price index is typically published monthly and has a delay before publication.
	  /// The lag is subtracted from the accrual start and end date to locate the
	  /// month of the data to be observed.
	  /// </para>
	  /// <para>
	  /// For example, the September data may be published in October or November.
	  /// A 3 month lag will cause an accrual date in December to be based on the
	  /// observed data for September, which should be available by then.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period lag;
	  private readonly Period lag;
	  /// <summary>
	  /// Reference price index calculation method.
	  /// <para>
	  /// This specifies how the reference index calculation occurs.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final PriceIndexCalculationMethod indexCalculationMethod;
	  private readonly PriceIndexCalculationMethod indexCalculationMethod;
	  /// <summary>
	  /// The initial value of the index, optional.
	  /// <para>
	  /// This optional field specifies the initial value of the index.
	  /// The value is applicable for the first <i>regular</i> accrual period.
	  /// It is used in place of an observed fixing.
	  /// Other calculation elements, such as gearing or spread, still apply.
	  /// After the first accrual period, the rate is observed via the normal fixing process.
	  /// </para>
	  /// <para>
	  /// The method <seealso cref="InflationRateCalculation#createRateComputation(LocalDate)"/>
	  /// allows this field to be used as the base for any end date, as typically seen
	  /// in capital indexed bonds.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the first value is observed via the normal fixing process.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> firstIndexValue;
	  private readonly double? firstIndexValue;
	  /// <summary>
	  /// The gearing multiplier, optional.
	  /// <para>
	  /// This defines the gearing as an initial value and a list of adjustments.
	  /// </para>
	  /// <para>
	  /// When calculating the index, the gearing acts as a overall factor of pay-off.
	  /// The pay-off is {@code Gearing_Factor * (Index_End / Index_Start - 1)}.
	  /// A gearing of 1 has no effect.
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

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a rate calculation for the specified price index.
	  /// <para>
	  /// The calculation will use the specified month lag.
	  /// All optional fields will be set to their default values.
	  /// Thus, fixing will be in advance, with no gearing.
	  /// If this method provides insufficient control, use the <seealso cref="#builder() builder"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the price index </param>
	  /// <param name="monthLag">  the month lag </param>
	  /// <param name="indexCalculationMethod">  the reference price index calculation method </param>
	  /// <returns> the inflation rate calculation </returns>
	  public static InflationRateCalculation of(PriceIndex index, int monthLag, PriceIndexCalculationMethod indexCalculationMethod)
	  {

		return InflationRateCalculation.builder().index(index).lag(Period.ofMonths(monthLag)).indexCalculationMethod(indexCalculationMethod).build();
	  }

	  /// <summary>
	  /// Obtains a rate calculation for the specified price index with known start index value.
	  /// <para>
	  /// The calculation will use the specified month lag.
	  /// The first index value will be set to the specified value
	  /// All other optional fields will be set to their default values.
	  /// Thus, fixing will be in advance, with no gearing.
	  /// If this method provides insufficient control, use the <seealso cref="#builder() builder"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the price index </param>
	  /// <param name="monthLag">  the month lag </param>
	  /// <param name="indexCalculationMethod">  the reference price index calculation method </param>
	  /// <param name="firstIndexValue">  the first index value </param>
	  /// <returns> the inflation rate calculation </returns>
	  public static InflationRateCalculation of(PriceIndex index, int monthLag, PriceIndexCalculationMethod indexCalculationMethod, double firstIndexValue)
	  {

		return InflationRateCalculation.builder().index(index).lag(Period.ofMonths(monthLag)).indexCalculationMethod(indexCalculationMethod).firstIndexValue(firstIndexValue).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isFalse(lag.Zero || lag.Negative, "Lag must be positive");
	  }

	  //-------------------------------------------------------------------------
	  public SwapLegType Type
	  {
		  get
		  {
			return SwapLegType.INFLATION;
		  }
	  }

	  public DayCount DayCount
	  {
		  get
		  {
			return DayCounts.ONE_ONE; // inflation does not use a day count
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
		// build accrual periods
		ImmutableList.Builder<RateAccrualPeriod> accrualPeriods = ImmutableList.builder();
		for (int i = 0; i < accrualSchedule.size(); i++)
		{
		  SchedulePeriod period = accrualSchedule.getPeriod(i);
		  // inflation does not use a day count, so year fraction is 1d
		  accrualPeriods.add(new RateAccrualPeriod(period, 1d, createRateComputation(period, i), resolvedGearings.get(i), 0d, NegativeRateMethod.ALLOW_NEGATIVE));
		}
		return accrualPeriods.build();
	  }

	  // creates the rate computation
	  private RateComputation createRateComputation(SchedulePeriod period, int scheduleIndex)
	  {

		// handle where index value at start date is known
		LocalDate endDate = period.EndDate;
		if (firstIndexValue != null && scheduleIndex == 0)
		{
		  return createRateComputation(endDate);
		}
		YearMonth referenceStartMonth = YearMonth.from(period.StartDate.minus(lag));
		YearMonth referenceEndMonth = YearMonth.from(endDate.minus(lag));
		if (indexCalculationMethod.Equals(PriceIndexCalculationMethod.INTERPOLATED))
		{
		  // interpolate between data from two different months
		  double weight = 1d - (endDate.DayOfMonth - 1d) / endDate.lengthOfMonth();
		  return InflationInterpolatedRateComputation.of(index, referenceStartMonth, referenceEndMonth, weight);
		}
		else if (indexCalculationMethod.Equals(PriceIndexCalculationMethod.MONTHLY))
		{
		  // no interpolation
		  return InflationMonthlyRateComputation.of(index, referenceStartMonth, referenceEndMonth);
		}
		else
		{
		  throw new System.ArgumentException("PriceIndexCalculationMethod " + indexCalculationMethod.ToString() + " is not supported");
		}
	  }

	  /// <summary>
	  /// Creates a rate observation where the start index value is known.
	  /// <para>
	  /// This is typically used for capital indexed bonds.
	  /// The rate is calculated between the value of {@code firstIndexValue}
	  /// and the observed value at the end month linked to the specified end date.
	  /// This method requires that {@code firstIndexValue} is present.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <returns> the rate observation </returns>
	  public RateComputation createRateComputation(LocalDate endDate)
	  {
		if (firstIndexValue == null)
		{
		  throw new System.InvalidOperationException("First index value must be specified");
		}
		YearMonth referenceEndMonth = YearMonth.from(endDate.minus(lag));
		if (indexCalculationMethod.Equals(PriceIndexCalculationMethod.INTERPOLATED))
		{
		  // interpolate between data from two different months
		  double weight = 1d - (endDate.DayOfMonth - 1d) / endDate.lengthOfMonth();
		  return InflationEndInterpolatedRateComputation.of(index, firstIndexValue.Value, referenceEndMonth, weight);
		}
		else if (indexCalculationMethod.Equals(PriceIndexCalculationMethod.MONTHLY))
		{
		  // no interpolation
		  return InflationEndMonthRateComputation.of(index, firstIndexValue.Value, referenceEndMonth);
		}
		else if (indexCalculationMethod.Equals(PriceIndexCalculationMethod.INTERPOLATED_JAPAN))
		{
		  // interpolation, Japan
		  double weight = 1d;
		  int dayOfMonth = endDate.DayOfMonth;
		  if (dayOfMonth > 10)
		  {
			weight -= (dayOfMonth - 10d) / endDate.lengthOfMonth();
		  }
		  else if (dayOfMonth < 10)
		  {
			weight -= (dayOfMonth + endDate.minusMonths(1).lengthOfMonth() - 10d) / endDate.minusMonths(1).lengthOfMonth();
			referenceEndMonth = referenceEndMonth.minusMonths(1);
		  }
		  return InflationEndInterpolatedRateComputation.of(index, firstIndexValue.Value, referenceEndMonth, weight);
		}
		else
		{
		  throw new System.ArgumentException("PriceIndexCalculationMethod " + indexCalculationMethod.ToString() + " is not supported");
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationRateCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InflationRateCalculation.Meta meta()
	  {
		return InflationRateCalculation.Meta.INSTANCE;
	  }

	  static InflationRateCalculation()
	  {
		MetaBean.register(InflationRateCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static InflationRateCalculation.Builder builder()
	  {
		return new InflationRateCalculation.Builder();
	  }

	  private InflationRateCalculation(PriceIndex index, Period lag, PriceIndexCalculationMethod indexCalculationMethod, double? firstIndexValue, ValueSchedule gearing)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(lag, "lag");
		JodaBeanUtils.notNull(indexCalculationMethod, "indexCalculationMethod");
		this.index = index;
		this.lag = lag;
		this.indexCalculationMethod = indexCalculationMethod;
		this.firstIndexValue = firstIndexValue;
		this.gearing = gearing;
		validate();
	  }

	  public override InflationRateCalculation.Meta metaBean()
	  {
		return InflationRateCalculation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index of prices.
	  /// <para>
	  /// The pay-off is computed based on this index
	  /// The most common implementations are provided in <seealso cref="PriceIndices"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the positive period between the price index and the accrual date,
	  /// typically a number of months.
	  /// <para>
	  /// A price index is typically published monthly and has a delay before publication.
	  /// The lag is subtracted from the accrual start and end date to locate the
	  /// month of the data to be observed.
	  /// </para>
	  /// <para>
	  /// For example, the September data may be published in October or November.
	  /// A 3 month lag will cause an accrual date in December to be based on the
	  /// observed data for September, which should be available by then.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period Lag
	  {
		  get
		  {
			return lag;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets reference price index calculation method.
	  /// <para>
	  /// This specifies how the reference index calculation occurs.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndexCalculationMethod IndexCalculationMethod
	  {
		  get
		  {
			return indexCalculationMethod;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial value of the index, optional.
	  /// <para>
	  /// This optional field specifies the initial value of the index.
	  /// The value is applicable for the first <i>regular</i> accrual period.
	  /// It is used in place of an observed fixing.
	  /// Other calculation elements, such as gearing or spread, still apply.
	  /// After the first accrual period, the rate is observed via the normal fixing process.
	  /// </para>
	  /// <para>
	  /// The method <seealso cref="InflationRateCalculation#createRateComputation(LocalDate)"/>
	  /// allows this field to be used as the base for any end date, as typically seen
	  /// in capital indexed bonds.
	  /// </para>
	  /// <para>
	  /// If this property is not present, then the first value is observed via the normal fixing process.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FirstIndexValue
	  {
		  get
		  {
			return firstIndexValue != null ? double?.of(firstIndexValue) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the gearing multiplier, optional.
	  /// <para>
	  /// This defines the gearing as an initial value and a list of adjustments.
	  /// </para>
	  /// <para>
	  /// When calculating the index, the gearing acts as a overall factor of pay-off.
	  /// The pay-off is {@code Gearing_Factor * (Index_End / Index_Start - 1)}.
	  /// A gearing of 1 has no effect.
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
		  InflationRateCalculation other = (InflationRateCalculation) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(lag, other.lag) && JodaBeanUtils.equal(indexCalculationMethod, other.indexCalculationMethod) && JodaBeanUtils.equal(firstIndexValue, other.firstIndexValue) && JodaBeanUtils.equal(gearing, other.gearing);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lag);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(indexCalculationMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstIndexValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(gearing);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("InflationRateCalculation{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("lag").Append('=').Append(lag).Append(',').Append(' ');
		buf.Append("indexCalculationMethod").Append('=').Append(indexCalculationMethod).Append(',').Append(' ');
		buf.Append("firstIndexValue").Append('=').Append(firstIndexValue).Append(',').Append(' ');
		buf.Append("gearing").Append('=').Append(JodaBeanUtils.ToString(gearing));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationRateCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(InflationRateCalculation), typeof(PriceIndex));
			  lag_Renamed = DirectMetaProperty.ofImmutable(this, "lag", typeof(InflationRateCalculation), typeof(Period));
			  indexCalculationMethod_Renamed = DirectMetaProperty.ofImmutable(this, "indexCalculationMethod", typeof(InflationRateCalculation), typeof(PriceIndexCalculationMethod));
			  firstIndexValue_Renamed = DirectMetaProperty.ofImmutable(this, "firstIndexValue", typeof(InflationRateCalculation), typeof(Double));
			  gearing_Renamed = DirectMetaProperty.ofImmutable(this, "gearing", typeof(InflationRateCalculation), typeof(ValueSchedule));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "lag", "indexCalculationMethod", "firstIndexValue", "gearing");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code lag} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> lag_Renamed;
		/// <summary>
		/// The meta-property for the {@code indexCalculationMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndexCalculationMethod> indexCalculationMethod_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstIndexValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> firstIndexValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code gearing} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> gearing_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "lag", "indexCalculationMethod", "firstIndexValue", "gearing");
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
			case 100346066: // index
			  return index_Renamed;
			case 106898: // lag
			  return lag_Renamed;
			case -1409010088: // indexCalculationMethod
			  return indexCalculationMethod_Renamed;
			case 922631823: // firstIndexValue
			  return firstIndexValue_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override InflationRateCalculation.Builder builder()
		{
		  return new InflationRateCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InflationRateCalculation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lag} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> lag()
		{
		  return lag_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code indexCalculationMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndexCalculationMethod> indexCalculationMethod()
		{
		  return indexCalculationMethod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstIndexValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> firstIndexValue()
		{
		  return firstIndexValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code gearing} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> gearing()
		{
		  return gearing_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((InflationRateCalculation) bean).Index;
			case 106898: // lag
			  return ((InflationRateCalculation) bean).Lag;
			case -1409010088: // indexCalculationMethod
			  return ((InflationRateCalculation) bean).IndexCalculationMethod;
			case 922631823: // firstIndexValue
			  return ((InflationRateCalculation) bean).firstIndexValue;
			case -91774989: // gearing
			  return ((InflationRateCalculation) bean).gearing;
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
	  /// The bean-builder for {@code InflationRateCalculation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<InflationRateCalculation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PriceIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period lag_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PriceIndexCalculationMethod indexCalculationMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? firstIndexValue_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule gearing_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(InflationRateCalculation beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.lag_Renamed = beanToCopy.Lag;
		  this.indexCalculationMethod_Renamed = beanToCopy.IndexCalculationMethod;
		  this.firstIndexValue_Renamed = beanToCopy.firstIndexValue;
		  this.gearing_Renamed = beanToCopy.gearing;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 106898: // lag
			  return lag_Renamed;
			case -1409010088: // indexCalculationMethod
			  return indexCalculationMethod_Renamed;
			case 922631823: // firstIndexValue
			  return firstIndexValue_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (PriceIndex) newValue;
			  break;
			case 106898: // lag
			  this.lag_Renamed = (Period) newValue;
			  break;
			case -1409010088: // indexCalculationMethod
			  this.indexCalculationMethod_Renamed = (PriceIndexCalculationMethod) newValue;
			  break;
			case 922631823: // firstIndexValue
			  this.firstIndexValue_Renamed = (double?) newValue;
			  break;
			case -91774989: // gearing
			  this.gearing_Renamed = (ValueSchedule) newValue;
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

		public override InflationRateCalculation build()
		{
		  return new InflationRateCalculation(index_Renamed, lag_Renamed, indexCalculationMethod_Renamed, firstIndexValue_Renamed, gearing_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index of prices.
		/// <para>
		/// The pay-off is computed based on this index
		/// The most common implementations are provided in <seealso cref="PriceIndices"/>.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(PriceIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the positive period between the price index and the accrual date,
		/// typically a number of months.
		/// <para>
		/// A price index is typically published monthly and has a delay before publication.
		/// The lag is subtracted from the accrual start and end date to locate the
		/// month of the data to be observed.
		/// </para>
		/// <para>
		/// For example, the September data may be published in October or November.
		/// A 3 month lag will cause an accrual date in December to be based on the
		/// observed data for September, which should be available by then.
		/// </para>
		/// </summary>
		/// <param name="lag">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lag(Period lag)
		{
		  JodaBeanUtils.notNull(lag, "lag");
		  this.lag_Renamed = lag;
		  return this;
		}

		/// <summary>
		/// Sets reference price index calculation method.
		/// <para>
		/// This specifies how the reference index calculation occurs.
		/// </para>
		/// </summary>
		/// <param name="indexCalculationMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder indexCalculationMethod(PriceIndexCalculationMethod indexCalculationMethod)
		{
		  JodaBeanUtils.notNull(indexCalculationMethod, "indexCalculationMethod");
		  this.indexCalculationMethod_Renamed = indexCalculationMethod;
		  return this;
		}

		/// <summary>
		/// Sets the initial value of the index, optional.
		/// <para>
		/// This optional field specifies the initial value of the index.
		/// The value is applicable for the first <i>regular</i> accrual period.
		/// It is used in place of an observed fixing.
		/// Other calculation elements, such as gearing or spread, still apply.
		/// After the first accrual period, the rate is observed via the normal fixing process.
		/// </para>
		/// <para>
		/// The method <seealso cref="InflationRateCalculation#createRateComputation(LocalDate)"/>
		/// allows this field to be used as the base for any end date, as typically seen
		/// in capital indexed bonds.
		/// </para>
		/// <para>
		/// If this property is not present, then the first value is observed via the normal fixing process.
		/// </para>
		/// </summary>
		/// <param name="firstIndexValue">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstIndexValue(double? firstIndexValue)
		{
		  this.firstIndexValue_Renamed = firstIndexValue;
		  return this;
		}

		/// <summary>
		/// Sets the gearing multiplier, optional.
		/// <para>
		/// This defines the gearing as an initial value and a list of adjustments.
		/// </para>
		/// <para>
		/// When calculating the index, the gearing acts as a overall factor of pay-off.
		/// The pay-off is {@code Gearing_Factor * (Index_End / Index_Start - 1)}.
		/// A gearing of 1 has no effect.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("InflationRateCalculation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("lag").Append('=').Append(JodaBeanUtils.ToString(lag_Renamed)).Append(',').Append(' ');
		  buf.Append("indexCalculationMethod").Append('=').Append(JodaBeanUtils.ToString(indexCalculationMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("firstIndexValue").Append('=').Append(JodaBeanUtils.ToString(firstIndexValue_Renamed)).Append(',').Append(' ');
		  buf.Append("gearing").Append('=').Append(JodaBeanUtils.ToString(gearing_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
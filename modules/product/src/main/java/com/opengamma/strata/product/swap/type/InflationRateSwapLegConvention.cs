using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A market convention for the floating leg of rate swap trades based on a price index.
	/// <para>
	/// This defines the market convention for a floating leg based on the observed value
	/// of a Price index such as 'GB-HICP' or 'US-CPI-U'.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class InflationRateSwapLegConvention implements SwapLegConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InflationRateSwapLegConvention : SwapLegConvention, ImmutableBean
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
	  /// <para>
	  /// This will default to 'Monthly' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.PriceIndexCalculationMethod indexCalculationMethod;
	  private readonly PriceIndexCalculationMethod indexCalculationMethod;
	  /// <summary>
	  /// The flag indicating whether to exchange the notional.
	  /// <para>
	  /// If 'true', the notional there is both an initial exchange and a final exchange of notional.
	  /// </para>
	  /// <para>
	  /// This will default to 'false' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean notionalExchange;
	  private readonly bool notionalExchange;
	  /// <summary>
	  /// The offset of payment from the base date, optional with defaulting getter.
	  /// <para>
	  /// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// <para>
	  /// This will default to 'None' if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset;
	  private readonly DaysAdjustment paymentDateOffset;
	  /// <summary>
	  /// The business day adjustment to apply to accrual schedule dates.
	  /// <para>
	  /// Each date in the calculated schedule is determined without taking into account weekends and holidays.
	  /// The adjustment specified here is used to convert those dates to valid business days.
	  /// </para>
	  /// <para>
	  /// The start date and end date may have their own business day adjustment rules.
	  /// If those are not present, then this adjustment is used instead.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.BusinessDayAdjustment accrualBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment accrualBusinessDayAdjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified index.
	  /// <para>
	  /// The standard market convention for an Inflation rate leg is based on the index.
	  /// Use the <seealso cref="#builder() builder"/> for unusual conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index, the market convention values are extracted from the index </param>
	  /// <param name="lag">  the lag between the price index and the accrual date, typically a number of months </param>
	  /// <param name="priceIndexCalculationMethod">  the price index calculation method, typically interpolated or monthly </param>
	  /// <param name="businessDayAdjustment">  the business day </param>
	  /// <returns> the convention </returns>
	  public static InflationRateSwapLegConvention of(PriceIndex index, Period lag, PriceIndexCalculationMethod priceIndexCalculationMethod, BusinessDayAdjustment businessDayAdjustment)
	  {

		return new InflationRateSwapLegConvention(index, lag, priceIndexCalculationMethod, false, DaysAdjustment.NONE, businessDayAdjustment);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.indexCalculationMethod_Renamed = PriceIndexCalculationMethod.MONTHLY;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the leg from the index.
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return index.Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a leg based on this convention.
	  /// <para>
	  /// This returns a leg based on the specified date.
	  /// The notional is unsigned, with pay/receive determining the direction of the leg.
	  /// If the leg is 'Pay', the fixed rate is paid to the counterparty.
	  /// If the leg is 'Receive', the fixed rate is received from the counterparty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="payReceive">  determines if the leg is to be paid or received </param>
	  /// <param name="notional">  the business day adjustment to apply to accrual schedule dates </param>
	  /// <returns> the leg </returns>
	  public RateCalculationSwapLeg toLeg(LocalDate startDate, LocalDate endDate, PayReceive payReceive, double notional)
	  {

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(startDate).endDate(endDate).frequency(Frequency.TERM).businessDayAdjustment(accrualBusinessDayAdjustment).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(Frequency.TERM).paymentDateOffset(paymentDateOffset).build()).calculation(InflationRateCalculation.builder().index(index).indexCalculationMethod(indexCalculationMethod).lag(lag).build()).notionalSchedule(NotionalSchedule.of(Currency, notional)).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationRateSwapLegConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InflationRateSwapLegConvention.Meta meta()
	  {
		return InflationRateSwapLegConvention.Meta.INSTANCE;
	  }

	  static InflationRateSwapLegConvention()
	  {
		MetaBean.register(InflationRateSwapLegConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static InflationRateSwapLegConvention.Builder builder()
	  {
		return new InflationRateSwapLegConvention.Builder();
	  }

	  private InflationRateSwapLegConvention(PriceIndex index, Period lag, PriceIndexCalculationMethod indexCalculationMethod, bool notionalExchange, DaysAdjustment paymentDateOffset, BusinessDayAdjustment accrualBusinessDayAdjustment)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(lag, "lag");
		JodaBeanUtils.notNull(indexCalculationMethod, "indexCalculationMethod");
		this.index = index;
		this.lag = lag;
		this.indexCalculationMethod = indexCalculationMethod;
		this.notionalExchange = notionalExchange;
		this.paymentDateOffset = paymentDateOffset;
		this.accrualBusinessDayAdjustment = accrualBusinessDayAdjustment;
	  }

	  public override InflationRateSwapLegConvention.Meta metaBean()
	  {
		return InflationRateSwapLegConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Price index.
	  /// <para>
	  /// The floating rate to be paid is based on this price index
	  /// It will be a well known price index such as 'GB-HICP'.
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
	  /// <para>
	  /// This will default to 'Monthly' if not specified.
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
	  /// Gets the flag indicating whether to exchange the notional.
	  /// <para>
	  /// If 'true', the notional there is both an initial exchange and a final exchange of notional.
	  /// </para>
	  /// <para>
	  /// This will default to 'false' if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool NotionalExchange
	  {
		  get
		  {
			return notionalExchange;
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
		  InflationRateSwapLegConvention other = (InflationRateSwapLegConvention) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(lag, other.lag) && JodaBeanUtils.equal(indexCalculationMethod, other.indexCalculationMethod) && (notionalExchange == other.notionalExchange) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(accrualBusinessDayAdjustment, other.accrualBusinessDayAdjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lag);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(indexCalculationMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notionalExchange);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualBusinessDayAdjustment);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("InflationRateSwapLegConvention{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("lag").Append('=').Append(lag).Append(',').Append(' ');
		buf.Append("indexCalculationMethod").Append('=').Append(indexCalculationMethod).Append(',').Append(' ');
		buf.Append("notionalExchange").Append('=').Append(notionalExchange).Append(',').Append(' ');
		buf.Append("paymentDateOffset").Append('=').Append(paymentDateOffset).Append(',').Append(' ');
		buf.Append("accrualBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(accrualBusinessDayAdjustment));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationRateSwapLegConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(InflationRateSwapLegConvention), typeof(PriceIndex));
			  lag_Renamed = DirectMetaProperty.ofImmutable(this, "lag", typeof(InflationRateSwapLegConvention), typeof(Period));
			  indexCalculationMethod_Renamed = DirectMetaProperty.ofImmutable(this, "indexCalculationMethod", typeof(InflationRateSwapLegConvention), typeof(PriceIndexCalculationMethod));
			  notionalExchange_Renamed = DirectMetaProperty.ofImmutable(this, "notionalExchange", typeof(InflationRateSwapLegConvention), Boolean.TYPE);
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(InflationRateSwapLegConvention), typeof(DaysAdjustment));
			  accrualBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "accrualBusinessDayAdjustment", typeof(InflationRateSwapLegConvention), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "lag", "indexCalculationMethod", "notionalExchange", "paymentDateOffset", "accrualBusinessDayAdjustment");
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
		/// The meta-property for the {@code notionalExchange} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> notionalExchange_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> paymentDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> accrualBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "lag", "indexCalculationMethod", "notionalExchange", "paymentDateOffset", "accrualBusinessDayAdjustment");
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
			case -159410813: // notionalExchange
			  return notionalExchange_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case 896049114: // accrualBusinessDayAdjustment
			  return accrualBusinessDayAdjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override InflationRateSwapLegConvention.Builder builder()
		{
		  return new InflationRateSwapLegConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InflationRateSwapLegConvention);
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
		/// The meta-property for the {@code notionalExchange} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> notionalExchange()
		{
		  return notionalExchange_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> paymentDateOffset()
		{
		  return paymentDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> accrualBusinessDayAdjustment()
		{
		  return accrualBusinessDayAdjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((InflationRateSwapLegConvention) bean).Index;
			case 106898: // lag
			  return ((InflationRateSwapLegConvention) bean).Lag;
			case -1409010088: // indexCalculationMethod
			  return ((InflationRateSwapLegConvention) bean).IndexCalculationMethod;
			case -159410813: // notionalExchange
			  return ((InflationRateSwapLegConvention) bean).NotionalExchange;
			case -716438393: // paymentDateOffset
			  return ((InflationRateSwapLegConvention) bean).paymentDateOffset;
			case 896049114: // accrualBusinessDayAdjustment
			  return ((InflationRateSwapLegConvention) bean).accrualBusinessDayAdjustment;
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
	  /// The bean-builder for {@code InflationRateSwapLegConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<InflationRateSwapLegConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PriceIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period lag_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PriceIndexCalculationMethod indexCalculationMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool notionalExchange_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment paymentDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment accrualBusinessDayAdjustment_Renamed;

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
		internal Builder(InflationRateSwapLegConvention beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.lag_Renamed = beanToCopy.Lag;
		  this.indexCalculationMethod_Renamed = beanToCopy.IndexCalculationMethod;
		  this.notionalExchange_Renamed = beanToCopy.NotionalExchange;
		  this.paymentDateOffset_Renamed = beanToCopy.paymentDateOffset;
		  this.accrualBusinessDayAdjustment_Renamed = beanToCopy.accrualBusinessDayAdjustment;
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
			case -159410813: // notionalExchange
			  return notionalExchange_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case 896049114: // accrualBusinessDayAdjustment
			  return accrualBusinessDayAdjustment_Renamed;
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
			case -159410813: // notionalExchange
			  this.notionalExchange_Renamed = (bool?) newValue.Value;
			  break;
			case -716438393: // paymentDateOffset
			  this.paymentDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 896049114: // accrualBusinessDayAdjustment
			  this.accrualBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
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

		public override InflationRateSwapLegConvention build()
		{
		  return new InflationRateSwapLegConvention(index_Renamed, lag_Renamed, indexCalculationMethod_Renamed, notionalExchange_Renamed, paymentDateOffset_Renamed, accrualBusinessDayAdjustment_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Price index.
		/// <para>
		/// The floating rate to be paid is based on this price index
		/// It will be a well known price index such as 'GB-HICP'.
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
		/// <para>
		/// This will default to 'Monthly' if not specified.
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
		/// Sets the flag indicating whether to exchange the notional.
		/// <para>
		/// If 'true', the notional there is both an initial exchange and a final exchange of notional.
		/// </para>
		/// <para>
		/// This will default to 'false' if not specified.
		/// </para>
		/// </summary>
		/// <param name="notionalExchange">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notionalExchange(bool notionalExchange)
		{
		  this.notionalExchange_Renamed = notionalExchange;
		  return this;
		}

		/// <summary>
		/// Sets the offset of payment from the base date, optional with defaulting getter.
		/// <para>
		/// The offset is applied to the unadjusted date specified by {@code paymentRelativeTo}.
		/// Offset can be based on calendar days or business days.
		/// </para>
		/// <para>
		/// This will default to 'None' if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDateOffset(DaysAdjustment paymentDateOffset)
		{
		  this.paymentDateOffset_Renamed = paymentDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to accrual schedule dates.
		/// <para>
		/// Each date in the calculated schedule is determined without taking into account weekends and holidays.
		/// The adjustment specified here is used to convert those dates to valid business days.
		/// </para>
		/// <para>
		/// The start date and end date may have their own business day adjustment rules.
		/// If those are not present, then this adjustment is used instead.
		/// </para>
		/// <para>
		/// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
		/// </para>
		/// </summary>
		/// <param name="accrualBusinessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualBusinessDayAdjustment(BusinessDayAdjustment accrualBusinessDayAdjustment)
		{
		  this.accrualBusinessDayAdjustment_Renamed = accrualBusinessDayAdjustment;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("InflationRateSwapLegConvention.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("lag").Append('=').Append(JodaBeanUtils.ToString(lag_Renamed)).Append(',').Append(' ');
		  buf.Append("indexCalculationMethod").Append('=').Append(JodaBeanUtils.ToString(indexCalculationMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("notionalExchange").Append('=').Append(JodaBeanUtils.ToString(notionalExchange_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(accrualBusinessDayAdjustment_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
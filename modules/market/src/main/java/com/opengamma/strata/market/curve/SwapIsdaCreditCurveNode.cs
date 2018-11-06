using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;

	/// <summary>
	/// An ISDA compliant curve node whose instrument is a standard Fixed-Ibor interest rate swap.
	/// <para>
	/// This node contains the information on the fixed leg of the swap. 
	/// It is assumed that the compounding not involved, the common business day adjustment is applied to start date, 
	/// end date and accrual schedule, and the fixed rate is paid on the end date of each payment period. 
	/// </para>
	/// <para>
	/// {@code observableId} is used to access the market data value of the swap par rate. 
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class SwapIsdaCreditCurveNode implements IsdaCreditCurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SwapIsdaCreditCurveNode : IsdaCreditCurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String label;
		private readonly string label;
	  /// <summary>
	  /// The identifier of the market data value that provides the rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.ObservableId observableId;
	  private readonly ObservableId observableId;
	  /// <summary>
	  /// The tenor of the swap.
	  /// <para>
	  /// This is the period from the first accrual date to the last accrual date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.Tenor tenor;
	  private readonly Tenor tenor;
	  /// <summary>
	  /// The offset of the start date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date and is typically plus 2 business days.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment spotDateOffset;
	  private readonly DaysAdjustment spotDateOffset;
	  /// <summary>
	  /// The business day adjustment to apply to the start date, end date and accrual schedule.
	  /// <para>
	  /// The date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert a relevant date to a valid business day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The day count convention applicable.
	  /// <para>
	  /// This is used to convert schedule period dates to a numerical value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The periodic frequency of payments, optional with defaulting getter.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// The compounding is not allowed in this node. Thus the frequency is the same as the accrual periodic frequency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency paymentFrequency;
	  private readonly Frequency paymentFrequency;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a curve node for a standard fixed-Ibor swap.
	  /// <para>
	  /// The label will be created from {@code tenor}. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observableId">  the observable ID </param>
	  /// <param name="spotDateOffset">  the spot date offset </param>
	  /// <param name="businessDayAdjustment">  the business day adjustment </param>
	  /// <param name="tenor">  the tenor </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="paymentFrequency">  the payment frequency </param>
	  /// <returns> the curve node </returns>
	  public static SwapIsdaCreditCurveNode of(ObservableId observableId, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment, Tenor tenor, DayCount dayCount, Frequency paymentFrequency)
	  {

		return SwapIsdaCreditCurveNode.builder().businessDayAdjustment(businessDayAdjustment).dayCount(dayCount).observableId(observableId).spotDateOffset(spotDateOffset).tenor(tenor).paymentFrequency(paymentFrequency).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (string.ReferenceEquals(builder.label_Renamed, null) && builder.tenor_Renamed != null)
		{
		  builder.label_Renamed = builder.tenor_Renamed.ToString();
		}
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate date(LocalDate tradeDate, ReferenceData refData)
	  {
		LocalDate startDate = spotDateOffset.adjust(tradeDate, refData);
		LocalDate endDate = startDate.plus(tenor.Period);
		return businessDayAdjustment.adjust(endDate, refData);
	  }

	  public override TenorDateParameterMetadata metadata(LocalDate nodeDate)
	  {
		return TenorDateParameterMetadata.of(nodeDate, tenor);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SwapIsdaCreditCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SwapIsdaCreditCurveNode.Meta meta()
	  {
		return SwapIsdaCreditCurveNode.Meta.INSTANCE;
	  }

	  static SwapIsdaCreditCurveNode()
	  {
		MetaBean.register(SwapIsdaCreditCurveNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static SwapIsdaCreditCurveNode.Builder builder()
	  {
		return new SwapIsdaCreditCurveNode.Builder();
	  }

	  private SwapIsdaCreditCurveNode(string label, ObservableId observableId, Tenor tenor, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment, DayCount dayCount, Frequency paymentFrequency)
	  {
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(observableId, "observableId");
		JodaBeanUtils.notNull(tenor, "tenor");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(paymentFrequency, "paymentFrequency");
		this.label = label;
		this.observableId = observableId;
		this.tenor = tenor;
		this.spotDateOffset = spotDateOffset;
		this.businessDayAdjustment = businessDayAdjustment;
		this.dayCount = dayCount;
		this.paymentFrequency = paymentFrequency;
	  }

	  public override SwapIsdaCreditCurveNode.Meta metaBean()
	  {
		return SwapIsdaCreditCurveNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label to use for the node, defaulted.
	  /// <para>
	  /// When building, this will default based on the tenor if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Label
	  {
		  get
		  {
			return label;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier of the market data value that provides the rate. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableId ObservableId
	  {
		  get
		  {
			return observableId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the tenor of the swap.
	  /// <para>
	  /// This is the period from the first accrual date to the last accrual date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Tenor Tenor
	  {
		  get
		  {
			return tenor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the start date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date and is typically plus 2 business days.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment SpotDateOffset
	  {
		  get
		  {
			return spotDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply to the start date, end date and accrual schedule.
	  /// <para>
	  /// The date property is an unadjusted date and as such might be a weekend or holiday.
	  /// The adjustment specified here is used to convert a relevant date to a valid business day.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment BusinessDayAdjustment
	  {
		  get
		  {
			return businessDayAdjustment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable.
	  /// <para>
	  /// This is used to convert schedule period dates to a numerical value.
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
	  /// Gets the periodic frequency of payments, optional with defaulting getter.
	  /// <para>
	  /// Regular payments will be made at the specified periodic frequency.
	  /// The compounding is not allowed in this node. Thus the frequency is the same as the accrual periodic frequency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency PaymentFrequency
	  {
		  get
		  {
			return paymentFrequency;
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
		  SwapIsdaCreditCurveNode other = (SwapIsdaCreditCurveNode) obj;
		  return JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(observableId, other.observableId) && JodaBeanUtils.equal(tenor, other.tenor) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(paymentFrequency, other.paymentFrequency);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observableId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tenor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentFrequency);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("SwapIsdaCreditCurveNode{");
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("observableId").Append('=').Append(observableId).Append(',').Append(' ');
		buf.Append("tenor").Append('=').Append(tenor).Append(',').Append(' ');
		buf.Append("spotDateOffset").Append('=').Append(spotDateOffset).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("paymentFrequency").Append('=').Append(JodaBeanUtils.ToString(paymentFrequency));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SwapIsdaCreditCurveNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(SwapIsdaCreditCurveNode), typeof(string));
			  observableId_Renamed = DirectMetaProperty.ofImmutable(this, "observableId", typeof(SwapIsdaCreditCurveNode), typeof(ObservableId));
			  tenor_Renamed = DirectMetaProperty.ofImmutable(this, "tenor", typeof(SwapIsdaCreditCurveNode), typeof(Tenor));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(SwapIsdaCreditCurveNode), typeof(DaysAdjustment));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(SwapIsdaCreditCurveNode), typeof(BusinessDayAdjustment));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(SwapIsdaCreditCurveNode), typeof(DayCount));
			  paymentFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "paymentFrequency", typeof(SwapIsdaCreditCurveNode), typeof(Frequency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "label", "observableId", "tenor", "spotDateOffset", "businessDayAdjustment", "dayCount", "paymentFrequency");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code label} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> label_Renamed;
		/// <summary>
		/// The meta-property for the {@code observableId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ObservableId> observableId_Renamed;
		/// <summary>
		/// The meta-property for the {@code tenor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Tenor> tenor_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentFrequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> paymentFrequency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "label", "observableId", "tenor", "spotDateOffset", "businessDayAdjustment", "dayCount", "paymentFrequency");
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
			case 102727412: // label
			  return label_Renamed;
			case -518800962: // observableId
			  return observableId_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override SwapIsdaCreditCurveNode.Builder builder()
		{
		  return new SwapIsdaCreditCurveNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SwapIsdaCreditCurveNode);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code label} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> label()
		{
		  return label_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code observableId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ObservableId> observableId()
		{
		  return observableId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tenor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Tenor> tenor()
		{
		  return tenor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> spotDateOffset()
		{
		  return spotDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentFrequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> paymentFrequency()
		{
		  return paymentFrequency_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 102727412: // label
			  return ((SwapIsdaCreditCurveNode) bean).Label;
			case -518800962: // observableId
			  return ((SwapIsdaCreditCurveNode) bean).ObservableId;
			case 110246592: // tenor
			  return ((SwapIsdaCreditCurveNode) bean).Tenor;
			case 746995843: // spotDateOffset
			  return ((SwapIsdaCreditCurveNode) bean).SpotDateOffset;
			case -1065319863: // businessDayAdjustment
			  return ((SwapIsdaCreditCurveNode) bean).BusinessDayAdjustment;
			case 1905311443: // dayCount
			  return ((SwapIsdaCreditCurveNode) bean).DayCount;
			case 863656438: // paymentFrequency
			  return ((SwapIsdaCreditCurveNode) bean).PaymentFrequency;
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
	  /// The bean-builder for {@code SwapIsdaCreditCurveNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<SwapIsdaCreditCurveNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string label_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ObservableId observableId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Tenor tenor_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment spotDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency paymentFrequency_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(SwapIsdaCreditCurveNode beanToCopy)
		{
		  this.label_Renamed = beanToCopy.Label;
		  this.observableId_Renamed = beanToCopy.ObservableId;
		  this.tenor_Renamed = beanToCopy.Tenor;
		  this.spotDateOffset_Renamed = beanToCopy.SpotDateOffset;
		  this.businessDayAdjustment_Renamed = beanToCopy.BusinessDayAdjustment;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.paymentFrequency_Renamed = beanToCopy.PaymentFrequency;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 102727412: // label
			  return label_Renamed;
			case -518800962: // observableId
			  return observableId_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 863656438: // paymentFrequency
			  return paymentFrequency_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 102727412: // label
			  this.label_Renamed = (string) newValue;
			  break;
			case -518800962: // observableId
			  this.observableId_Renamed = (ObservableId) newValue;
			  break;
			case 110246592: // tenor
			  this.tenor_Renamed = (Tenor) newValue;
			  break;
			case 746995843: // spotDateOffset
			  this.spotDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 863656438: // paymentFrequency
			  this.paymentFrequency_Renamed = (Frequency) newValue;
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

		public override SwapIsdaCreditCurveNode build()
		{
		  preBuild(this);
		  return new SwapIsdaCreditCurveNode(label_Renamed, observableId_Renamed, tenor_Renamed, spotDateOffset_Renamed, businessDayAdjustment_Renamed, dayCount_Renamed, paymentFrequency_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the label to use for the node, defaulted.
		/// <para>
		/// When building, this will default based on the tenor if not specified.
		/// </para>
		/// </summary>
		/// <param name="label">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder label(string label)
		{
		  JodaBeanUtils.notEmpty(label, "label");
		  this.label_Renamed = label;
		  return this;
		}

		/// <summary>
		/// Sets the identifier of the market data value that provides the rate. </summary>
		/// <param name="observableId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder observableId(ObservableId observableId)
		{
		  JodaBeanUtils.notNull(observableId, "observableId");
		  this.observableId_Renamed = observableId;
		  return this;
		}

		/// <summary>
		/// Sets the tenor of the swap.
		/// <para>
		/// This is the period from the first accrual date to the last accrual date.
		/// </para>
		/// </summary>
		/// <param name="tenor">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder tenor(Tenor tenor)
		{
		  JodaBeanUtils.notNull(tenor, "tenor");
		  this.tenor_Renamed = tenor;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the start date from the trade date.
		/// <para>
		/// The offset is applied to the trade date and is typically plus 2 business days.
		/// </para>
		/// </summary>
		/// <param name="spotDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spotDateOffset(DaysAdjustment spotDateOffset)
		{
		  JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		  this.spotDateOffset_Renamed = spotDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the start date, end date and accrual schedule.
		/// <para>
		/// The date property is an unadjusted date and as such might be a weekend or holiday.
		/// The adjustment specified here is used to convert a relevant date to a valid business day.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable.
		/// <para>
		/// This is used to convert schedule period dates to a numerical value.
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
		/// Sets the periodic frequency of payments, optional with defaulting getter.
		/// <para>
		/// Regular payments will be made at the specified periodic frequency.
		/// The compounding is not allowed in this node. Thus the frequency is the same as the accrual periodic frequency.
		/// </para>
		/// </summary>
		/// <param name="paymentFrequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentFrequency(Frequency paymentFrequency)
		{
		  JodaBeanUtils.notNull(paymentFrequency, "paymentFrequency");
		  this.paymentFrequency_Renamed = paymentFrequency;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("SwapIsdaCreditCurveNode.Builder{");
		  buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label_Renamed)).Append(',').Append(' ');
		  buf.Append("observableId").Append('=').Append(JodaBeanUtils.ToString(observableId_Renamed)).Append(',').Append(' ');
		  buf.Append("tenor").Append('=').Append(JodaBeanUtils.ToString(tenor_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentFrequency").Append('=').Append(JodaBeanUtils.ToString(paymentFrequency_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
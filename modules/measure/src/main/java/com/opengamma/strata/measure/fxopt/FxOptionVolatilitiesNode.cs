using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ValueType = com.opengamma.strata.market.ValueType;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using Strike = com.opengamma.strata.market.option.Strike;

	/// <summary>
	/// A node in the configuration specifying how to build FX option volatilities.
	/// <para>
	/// Each node is not necessarily associated with an instrument, 
	/// but provides the necessary information to create {@code FxOptionVolatilities}. 
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxOptionVolatilitiesNode implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxOptionVolatilitiesNode : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
		private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The label to use for the node.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String label;
	  private readonly string label;
	  /// <summary>
	  /// The offset of the spot value date from the valuation date.
	  /// <para>
	  /// Typically this is the same as the standard convention of the spot date offset of the underlying FX forward.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment spotDateOffset;
	  private readonly DaysAdjustment spotDateOffset;
	  /// <summary>
	  /// The business day adjustment to apply to the delivery date.
	  /// <para>
	  /// Typically this is the same as the standard convention of the business day adjustment 
	  /// applied to the delivery date of the underlying FX forward.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The offset of the expiry date from the delivery date.
	  /// <para>
	  /// By default the expiry date offset is the inverse of {@code spotDateOffset}. 
	  /// In this case {@code BusinessDayAdjustment} in {@code spotDateOffset} must be {@code NONE}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment expiryDateOffset;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly DaysAdjustment expiryDateOffset_Renamed;
	  /// <summary>
	  /// The value type of the quote.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ValueType quoteValueType;
	  private readonly ValueType quoteValueType;
	  /// <summary>
	  /// The quote ID.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.observable.QuoteId quoteId;
	  private readonly QuoteId quoteId;
	  /// <summary>
	  /// The tenor.
	  /// <para>
	  /// Typically the tenor is coherent to that of the underlying FX forward. 
	  /// Thus it spans the period between spot date to delivery date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.Tenor tenor;
	  private readonly Tenor tenor;
	  /// <summary>
	  /// The strike.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.option.Strike strike;
	  private readonly Strike strike;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// The label is created from {@code quoteId}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="spotDateOffset">  the spot date offset </param>
	  /// <param name="businessDayAdjustment">  the business day adjustment </param>
	  /// <param name="quoteValueType">  the quote value type </param>
	  /// <param name="quoteId">  the quote ID </param>
	  /// <param name="tenor">  the tenor </param>
	  /// <param name="strike">  the strike </param>
	  /// <returns> the instance </returns>
	  public static FxOptionVolatilitiesNode of(CurrencyPair currencyPair, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment, ValueType quoteValueType, QuoteId quoteId, Tenor tenor, Strike strike)
	  {

		DaysAdjustment expiryDateOffset = FxOptionVolatilitiesNode.expiryDateOffset(spotDateOffset);

		return new FxOptionVolatilitiesNode(currencyPair, quoteId.ToString(), spotDateOffset, businessDayAdjustment, expiryDateOffset, quoteValueType, quoteId, tenor, strike);
	  }

	  private static DaysAdjustment expiryDateOffset(DaysAdjustment spotDateOffset)
	  {
		ArgChecker.isTrue(spotDateOffset.Adjustment.Equals(BusinessDayAdjustment.NONE), "BusinessDayAdjustment in spotDateOffset must be NONE if expiryDateOffset is created from spotDateOffset");
		DaysAdjustment adj = spotDateOffset.toBuilder().days(-spotDateOffset.Days).build();
		return adj;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.expiryDateOffset_Renamed == null)
		{
		  if (builder.spotDateOffset_Renamed != null)
		  {
			builder.expiryDateOffset_Renamed = expiryDateOffset(builder.spotDateOffset_Renamed);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the time to expiry for the valuation date time.
	  /// </summary>
	  /// <param name="valuationDateTime">  the valuation date time </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the time to expiry </returns>
	  public double timeToExpiry(ZonedDateTime valuationDateTime, DayCount dayCount, ReferenceData refData)
	  {
		LocalDate valuationDate = valuationDateTime.toLocalDate();
		LocalDate spotDate = spotDateOffset.adjust(valuationDate, refData);
		LocalDate deliveryDate = businessDayAdjustment.adjust(spotDate.plus(tenor), refData);
		LocalDate expiryDate = expiryDateOffset_Renamed.adjust(deliveryDate, refData);
		return dayCount.relativeYearFraction(valuationDate, expiryDate);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxOptionVolatilitiesNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxOptionVolatilitiesNode.Meta meta()
	  {
		return FxOptionVolatilitiesNode.Meta.INSTANCE;
	  }

	  static FxOptionVolatilitiesNode()
	  {
		MetaBean.register(FxOptionVolatilitiesNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxOptionVolatilitiesNode.Builder builder()
	  {
		return new FxOptionVolatilitiesNode.Builder();
	  }

	  private FxOptionVolatilitiesNode(CurrencyPair currencyPair, string label, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment, DaysAdjustment expiryDateOffset, ValueType quoteValueType, QuoteId quoteId, Tenor tenor, Strike strike)
	  {
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(label, "label");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		JodaBeanUtils.notNull(expiryDateOffset, "expiryDateOffset");
		JodaBeanUtils.notNull(quoteValueType, "quoteValueType");
		JodaBeanUtils.notNull(quoteId, "quoteId");
		JodaBeanUtils.notNull(tenor, "tenor");
		JodaBeanUtils.notNull(strike, "strike");
		this.currencyPair = currencyPair;
		this.label = label;
		this.spotDateOffset = spotDateOffset;
		this.businessDayAdjustment = businessDayAdjustment;
		this.expiryDateOffset_Renamed = expiryDateOffset;
		this.quoteValueType = quoteValueType;
		this.quoteId = quoteId;
		this.tenor = tenor;
		this.strike = strike;
	  }

	  public override FxOptionVolatilitiesNode.Meta metaBean()
	  {
		return FxOptionVolatilitiesNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair.
	  /// <para>
	  /// The quote must be based on this currency pair and direction.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label to use for the node. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Label
	  {
		  get
		  {
			return label;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the spot value date from the valuation date.
	  /// <para>
	  /// Typically this is the same as the standard convention of the spot date offset of the underlying FX forward.
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
	  /// Gets the business day adjustment to apply to the delivery date.
	  /// <para>
	  /// Typically this is the same as the standard convention of the business day adjustment
	  /// applied to the delivery date of the underlying FX forward.
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
	  /// Gets the offset of the expiry date from the delivery date.
	  /// <para>
	  /// By default the expiry date offset is the inverse of {@code spotDateOffset}.
	  /// In this case {@code BusinessDayAdjustment} in {@code spotDateOffset} must be {@code NONE}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment ExpiryDateOffset
	  {
		  get
		  {
			return expiryDateOffset_Renamed;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value type of the quote. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType QuoteValueType
	  {
		  get
		  {
			return quoteValueType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the quote ID. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public QuoteId QuoteId
	  {
		  get
		  {
			return quoteId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the tenor.
	  /// <para>
	  /// Typically the tenor is coherent to that of the underlying FX forward.
	  /// Thus it spans the period between spot date to delivery date.
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
	  /// Gets the strike. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Strike Strike
	  {
		  get
		  {
			return strike;
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
		  FxOptionVolatilitiesNode other = (FxOptionVolatilitiesNode) obj;
		  return JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(expiryDateOffset_Renamed, other.expiryDateOffset_Renamed) && JodaBeanUtils.equal(quoteValueType, other.quoteValueType) && JodaBeanUtils.equal(quoteId, other.quoteId) && JodaBeanUtils.equal(tenor, other.tenor) && JodaBeanUtils.equal(strike, other.strike);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryDateOffset_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quoteValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quoteId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tenor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strike);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("FxOptionVolatilitiesNode{");
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("spotDateOffset").Append('=').Append(spotDateOffset).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("expiryDateOffset").Append('=').Append(expiryDateOffset_Renamed).Append(',').Append(' ');
		buf.Append("quoteValueType").Append('=').Append(quoteValueType).Append(',').Append(' ');
		buf.Append("quoteId").Append('=').Append(quoteId).Append(',').Append(' ');
		buf.Append("tenor").Append('=').Append(tenor).Append(',').Append(' ');
		buf.Append("strike").Append('=').Append(JodaBeanUtils.ToString(strike));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxOptionVolatilitiesNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(FxOptionVolatilitiesNode), typeof(CurrencyPair));
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(FxOptionVolatilitiesNode), typeof(string));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(FxOptionVolatilitiesNode), typeof(DaysAdjustment));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(FxOptionVolatilitiesNode), typeof(BusinessDayAdjustment));
			  expiryDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "expiryDateOffset", typeof(FxOptionVolatilitiesNode), typeof(DaysAdjustment));
			  quoteValueType_Renamed = DirectMetaProperty.ofImmutable(this, "quoteValueType", typeof(FxOptionVolatilitiesNode), typeof(ValueType));
			  quoteId_Renamed = DirectMetaProperty.ofImmutable(this, "quoteId", typeof(FxOptionVolatilitiesNode), typeof(QuoteId));
			  tenor_Renamed = DirectMetaProperty.ofImmutable(this, "tenor", typeof(FxOptionVolatilitiesNode), typeof(Tenor));
			  strike_Renamed = DirectMetaProperty.ofImmutable(this, "strike", typeof(FxOptionVolatilitiesNode), typeof(Strike));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currencyPair", "label", "spotDateOffset", "businessDayAdjustment", "expiryDateOffset", "quoteValueType", "quoteId", "tenor", "strike");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code label} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> label_Renamed;
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
		/// The meta-property for the {@code expiryDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> expiryDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code quoteValueType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueType> quoteValueType_Renamed;
		/// <summary>
		/// The meta-property for the {@code quoteId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<QuoteId> quoteId_Renamed;
		/// <summary>
		/// The meta-property for the {@code tenor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Tenor> tenor_Renamed;
		/// <summary>
		/// The meta-property for the {@code strike} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Strike> strike_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currencyPair", "label", "spotDateOffset", "businessDayAdjustment", "expiryDateOffset", "quoteValueType", "quoteId", "tenor", "strike");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 102727412: // label
			  return label_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 508197748: // expiryDateOffset
			  return expiryDateOffset_Renamed;
			case 758636847: // quoteValueType
			  return quoteValueType_Renamed;
			case 664377527: // quoteId
			  return quoteId_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case -891985998: // strike
			  return strike_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxOptionVolatilitiesNode.Builder builder()
		{
		  return new FxOptionVolatilitiesNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxOptionVolatilitiesNode);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code label} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> label()
		{
		  return label_Renamed;
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
		/// The meta-property for the {@code expiryDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> expiryDateOffset()
		{
		  return expiryDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quoteValueType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueType> quoteValueType()
		{
		  return quoteValueType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quoteId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<QuoteId> quoteId()
		{
		  return quoteId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tenor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Tenor> tenor()
		{
		  return tenor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strike} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Strike> strike()
		{
		  return strike_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return ((FxOptionVolatilitiesNode) bean).CurrencyPair;
			case 102727412: // label
			  return ((FxOptionVolatilitiesNode) bean).Label;
			case 746995843: // spotDateOffset
			  return ((FxOptionVolatilitiesNode) bean).SpotDateOffset;
			case -1065319863: // businessDayAdjustment
			  return ((FxOptionVolatilitiesNode) bean).BusinessDayAdjustment;
			case 508197748: // expiryDateOffset
			  return ((FxOptionVolatilitiesNode) bean).ExpiryDateOffset;
			case 758636847: // quoteValueType
			  return ((FxOptionVolatilitiesNode) bean).QuoteValueType;
			case 664377527: // quoteId
			  return ((FxOptionVolatilitiesNode) bean).QuoteId;
			case 110246592: // tenor
			  return ((FxOptionVolatilitiesNode) bean).Tenor;
			case -891985998: // strike
			  return ((FxOptionVolatilitiesNode) bean).Strike;
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
	  /// The bean-builder for {@code FxOptionVolatilitiesNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxOptionVolatilitiesNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string label_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment spotDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment expiryDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueType quoteValueType_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal QuoteId quoteId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Tenor tenor_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Strike strike_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FxOptionVolatilitiesNode beanToCopy)
		{
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.label_Renamed = beanToCopy.Label;
		  this.spotDateOffset_Renamed = beanToCopy.SpotDateOffset;
		  this.businessDayAdjustment_Renamed = beanToCopy.BusinessDayAdjustment;
		  this.expiryDateOffset_Renamed = beanToCopy.ExpiryDateOffset;
		  this.quoteValueType_Renamed = beanToCopy.QuoteValueType;
		  this.quoteId_Renamed = beanToCopy.QuoteId;
		  this.tenor_Renamed = beanToCopy.Tenor;
		  this.strike_Renamed = beanToCopy.Strike;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 102727412: // label
			  return label_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 508197748: // expiryDateOffset
			  return expiryDateOffset_Renamed;
			case 758636847: // quoteValueType
			  return quoteValueType_Renamed;
			case 664377527: // quoteId
			  return quoteId_Renamed;
			case 110246592: // tenor
			  return tenor_Renamed;
			case -891985998: // strike
			  return strike_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case 102727412: // label
			  this.label_Renamed = (string) newValue;
			  break;
			case 746995843: // spotDateOffset
			  this.spotDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case 508197748: // expiryDateOffset
			  this.expiryDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 758636847: // quoteValueType
			  this.quoteValueType_Renamed = (ValueType) newValue;
			  break;
			case 664377527: // quoteId
			  this.quoteId_Renamed = (QuoteId) newValue;
			  break;
			case 110246592: // tenor
			  this.tenor_Renamed = (Tenor) newValue;
			  break;
			case -891985998: // strike
			  this.strike_Renamed = (Strike) newValue;
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

		public override FxOptionVolatilitiesNode build()
		{
		  preBuild(this);
		  return new FxOptionVolatilitiesNode(currencyPair_Renamed, label_Renamed, spotDateOffset_Renamed, businessDayAdjustment_Renamed, expiryDateOffset_Renamed, quoteValueType_Renamed, quoteId_Renamed, tenor_Renamed, strike_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the currency pair.
		/// <para>
		/// The quote must be based on this currency pair and direction.
		/// </para>
		/// </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
		  return this;
		}

		/// <summary>
		/// Sets the label to use for the node. </summary>
		/// <param name="label">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder label(string label)
		{
		  JodaBeanUtils.notNull(label, "label");
		  this.label_Renamed = label;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the spot value date from the valuation date.
		/// <para>
		/// Typically this is the same as the standard convention of the spot date offset of the underlying FX forward.
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
		/// Sets the business day adjustment to apply to the delivery date.
		/// <para>
		/// Typically this is the same as the standard convention of the business day adjustment
		/// applied to the delivery date of the underlying FX forward.
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
		/// Sets the offset of the expiry date from the delivery date.
		/// <para>
		/// By default the expiry date offset is the inverse of {@code spotDateOffset}.
		/// In this case {@code BusinessDayAdjustment} in {@code spotDateOffset} must be {@code NONE}.
		/// </para>
		/// </summary>
		/// <param name="expiryDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryDateOffset(DaysAdjustment expiryDateOffset)
		{
		  JodaBeanUtils.notNull(expiryDateOffset, "expiryDateOffset");
		  this.expiryDateOffset_Renamed = expiryDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the value type of the quote. </summary>
		/// <param name="quoteValueType">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder quoteValueType(ValueType quoteValueType)
		{
		  JodaBeanUtils.notNull(quoteValueType, "quoteValueType");
		  this.quoteValueType_Renamed = quoteValueType;
		  return this;
		}

		/// <summary>
		/// Sets the quote ID. </summary>
		/// <param name="quoteId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder quoteId(QuoteId quoteId)
		{
		  JodaBeanUtils.notNull(quoteId, "quoteId");
		  this.quoteId_Renamed = quoteId;
		  return this;
		}

		/// <summary>
		/// Sets the tenor.
		/// <para>
		/// Typically the tenor is coherent to that of the underlying FX forward.
		/// Thus it spans the period between spot date to delivery date.
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
		/// Sets the strike. </summary>
		/// <param name="strike">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strike(Strike strike)
		{
		  JodaBeanUtils.notNull(strike, "strike");
		  this.strike_Renamed = strike;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("FxOptionVolatilitiesNode.Builder{");
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryDateOffset").Append('=').Append(JodaBeanUtils.ToString(expiryDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("quoteValueType").Append('=').Append(JodaBeanUtils.ToString(quoteValueType_Renamed)).Append(',').Append(' ');
		  buf.Append("quoteId").Append('=').Append(JodaBeanUtils.ToString(quoteId_Renamed)).Append(',').Append(' ');
		  buf.Append("tenor").Append('=').Append(JodaBeanUtils.ToString(tenor_Renamed)).Append(',').Append(' ');
		  buf.Append("strike").Append('=').Append(JodaBeanUtils.ToString(strike_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
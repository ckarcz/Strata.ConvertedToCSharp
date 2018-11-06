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
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;

	/// <summary>
	/// Provides the definition of how to calibrate an ISDA compliant curve for credit.
	/// <para>
	/// An ISDA compliant curve is built from a number of parameters and described by metadata.
	/// Calibration is based on a list of <seealso cref="IsdaCreditCurveNode"/> instances, one for each parameter,
	/// that specify the underlying instruments.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IsdaCreditCurveDefinition implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IsdaCreditCurveDefinition : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CurveName name;
		private readonly CurveName name;
	  /// <summary>
	  /// The curve currency. 
	  /// <para>
	  /// The resultant curve will be used for discounting based on this currency. 
	  /// This is typically the same as the currency of the curve node instruments in {@code curveNodes}. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The curve valuation date.
	  /// <para>
	  /// The date on which the resultant curve is used for pricing. 
	  /// This date is not necessarily the same as the {@code valuationDate} of {@code MarketData} 
	  /// on which the market data was snapped.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate curveValuationDate;
	  private readonly LocalDate curveValuationDate;
	  /// <summary>
	  /// The day count.
	  /// <para>
	  /// If the x-value of the curve represents time as a year fraction, the day count
	  /// can be specified to define how the year fraction is calculated.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The curve nodes.
	  /// <para>
	  /// The nodes are used to find the par rates and calibrate the curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends IsdaCreditCurveNode>") private final com.google.common.collect.ImmutableList<IsdaCreditCurveNode> curveNodes;
	  private readonly ImmutableList<IsdaCreditCurveNode> curveNodes;
	  /// <summary>
	  /// The flag indicating if the Jacobian matrices should be computed and stored in metadata or not.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final boolean computeJacobian;
	  private readonly bool computeJacobian;
	  /// <summary>
	  /// The flag indicating if the node trade should be stored or not.
	  /// <para>
	  /// This property is used only for credit curve calibration.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final boolean storeNodeTrade;
	  private readonly bool storeNodeTrade;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <param name="currency">  the currency </param>
	  /// <param name="curveValuationDate">  the curve valuation date </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="curveNodes">  the curve nodes </param>
	  /// <param name="computeJacobian">  the Jacobian flag </param>
	  /// <param name="storeNodeTrade">  the node trade flag </param>
	  /// <returns> the instance </returns>
	  public static IsdaCreditCurveDefinition of<T1>(CurveName name, Currency currency, LocalDate curveValuationDate, DayCount dayCount, IList<T1> curveNodes, bool computeJacobian, bool storeNodeTrade) where T1 : IsdaCreditCurveNode
	  {

		return new IsdaCreditCurveDefinition(name, currency, curveValuationDate, dayCount, curveNodes, computeJacobian, storeNodeTrade);
	  }

	  /// <summary>
	  /// Creates the ISDA compliant curve.
	  /// <para>
	  /// The parameter metadata is not stored in the metadata of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFractions">  the year fraction values </param>
	  /// <param name="zeroRates">  the zero rate values </param>
	  /// <returns> the curve </returns>
	  public InterpolatedNodalCurve curve(DoubleArray yearFractions, DoubleArray zeroRates)
	  {
		CurveMetadata baseMetadata = Curves.zeroRates(name, dayCount);
		return InterpolatedNodalCurve.of(baseMetadata, yearFractions, zeroRates, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IsdaCreditCurveDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IsdaCreditCurveDefinition.Meta meta()
	  {
		return IsdaCreditCurveDefinition.Meta.INSTANCE;
	  }

	  static IsdaCreditCurveDefinition()
	  {
		MetaBean.register(IsdaCreditCurveDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IsdaCreditCurveDefinition<T1>(CurveName name, Currency currency, LocalDate curveValuationDate, DayCount dayCount, IList<T1> curveNodes, bool computeJacobian, bool storeNodeTrade) where T1 : IsdaCreditCurveNode
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(curveValuationDate, "curveValuationDate");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(curveNodes, "curveNodes");
		JodaBeanUtils.notNull(computeJacobian, "computeJacobian");
		JodaBeanUtils.notNull(storeNodeTrade, "storeNodeTrade");
		this.name = name;
		this.currency = currency;
		this.curveValuationDate = curveValuationDate;
		this.dayCount = dayCount;
		this.curveNodes = ImmutableList.copyOf(curveNodes);
		this.computeJacobian = computeJacobian;
		this.storeNodeTrade = storeNodeTrade;
	  }

	  public override IsdaCreditCurveDefinition.Meta metaBean()
	  {
		return IsdaCreditCurveDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve currency.
	  /// <para>
	  /// The resultant curve will be used for discounting based on this currency.
	  /// This is typically the same as the currency of the curve node instruments in {@code curveNodes}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve valuation date.
	  /// <para>
	  /// The date on which the resultant curve is used for pricing.
	  /// This date is not necessarily the same as the {@code valuationDate} of {@code MarketData}
	  /// on which the market data was snapped.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate CurveValuationDate
	  {
		  get
		  {
			return curveValuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count.
	  /// <para>
	  /// If the x-value of the curve represents time as a year fraction, the day count
	  /// can be specified to define how the year fraction is calculated.
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
	  /// Gets the curve nodes.
	  /// <para>
	  /// The nodes are used to find the par rates and calibrate the curve.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<IsdaCreditCurveNode> CurveNodes
	  {
		  get
		  {
			return curveNodes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating if the Jacobian matrices should be computed and stored in metadata or not. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public bool ComputeJacobian
	  {
		  get
		  {
			return computeJacobian;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating if the node trade should be stored or not.
	  /// <para>
	  /// This property is used only for credit curve calibration.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public bool StoreNodeTrade
	  {
		  get
		  {
			return storeNodeTrade;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  IsdaCreditCurveDefinition other = (IsdaCreditCurveDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(curveValuationDate, other.curveValuationDate) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(curveNodes, other.curveNodes) && (computeJacobian == other.computeJacobian) && (storeNodeTrade == other.storeNodeTrade);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveValuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveNodes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(computeJacobian);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(storeNodeTrade);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("IsdaCreditCurveDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("curveValuationDate").Append('=').Append(curveValuationDate).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("curveNodes").Append('=').Append(curveNodes).Append(',').Append(' ');
		buf.Append("computeJacobian").Append('=').Append(computeJacobian).Append(',').Append(' ');
		buf.Append("storeNodeTrade").Append('=').Append(JodaBeanUtils.ToString(storeNodeTrade));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IsdaCreditCurveDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(IsdaCreditCurveDefinition), typeof(CurveName));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IsdaCreditCurveDefinition), typeof(Currency));
			  curveValuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "curveValuationDate", typeof(IsdaCreditCurveDefinition), typeof(LocalDate));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(IsdaCreditCurveDefinition), typeof(DayCount));
			  curveNodes_Renamed = DirectMetaProperty.ofImmutable(this, "curveNodes", typeof(IsdaCreditCurveDefinition), (Type) typeof(ImmutableList));
			  computeJacobian_Renamed = DirectMetaProperty.ofImmutable(this, "computeJacobian", typeof(IsdaCreditCurveDefinition), Boolean.TYPE);
			  storeNodeTrade_Renamed = DirectMetaProperty.ofImmutable(this, "storeNodeTrade", typeof(IsdaCreditCurveDefinition), Boolean.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currency", "curveValuationDate", "dayCount", "curveNodes", "computeJacobian", "storeNodeTrade");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code curveValuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> curveValuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code curveNodes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<IsdaCreditCurveNode>> curveNodes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "curveNodes", IsdaCreditCurveDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<IsdaCreditCurveNode>> curveNodes_Renamed;
		/// <summary>
		/// The meta-property for the {@code computeJacobian} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> computeJacobian_Renamed;
		/// <summary>
		/// The meta-property for the {@code storeNodeTrade} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> storeNodeTrade_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currency", "curveValuationDate", "dayCount", "curveNodes", "computeJacobian", "storeNodeTrade");
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
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 318917792: // curveValuationDate
			  return curveValuationDate_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1863622910: // curveNodes
			  return curveNodes_Renamed;
			case -1730091410: // computeJacobian
			  return computeJacobian_Renamed;
			case 561141921: // storeNodeTrade
			  return storeNodeTrade_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IsdaCreditCurveDefinition> builder()
		public override BeanBuilder<IsdaCreditCurveDefinition> builder()
		{
		  return new IsdaCreditCurveDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IsdaCreditCurveDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code curveValuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> curveValuationDate()
		{
		  return curveValuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code curveNodes} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<IsdaCreditCurveNode>> curveNodes()
		{
		  return curveNodes_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code computeJacobian} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> computeJacobian()
		{
		  return computeJacobian_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code storeNodeTrade} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> storeNodeTrade()
		{
		  return storeNodeTrade_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((IsdaCreditCurveDefinition) bean).Name;
			case 575402001: // currency
			  return ((IsdaCreditCurveDefinition) bean).Currency;
			case 318917792: // curveValuationDate
			  return ((IsdaCreditCurveDefinition) bean).CurveValuationDate;
			case 1905311443: // dayCount
			  return ((IsdaCreditCurveDefinition) bean).DayCount;
			case -1863622910: // curveNodes
			  return ((IsdaCreditCurveDefinition) bean).CurveNodes;
			case -1730091410: // computeJacobian
			  return ((IsdaCreditCurveDefinition) bean).ComputeJacobian;
			case 561141921: // storeNodeTrade
			  return ((IsdaCreditCurveDefinition) bean).StoreNodeTrade;
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
	  /// The bean-builder for {@code IsdaCreditCurveDefinition}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IsdaCreditCurveDefinition>
	  {

		internal CurveName name;
		internal Currency currency;
		internal LocalDate curveValuationDate;
		internal DayCount dayCount;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends IsdaCreditCurveNode> curveNodes = com.google.common.collect.ImmutableList.of();
		internal IList<IsdaCreditCurveNode> curveNodes = ImmutableList.of();
		internal bool computeJacobian;
		internal bool storeNodeTrade;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name;
			case 575402001: // currency
			  return currency;
			case 318917792: // curveValuationDate
			  return curveValuationDate;
			case 1905311443: // dayCount
			  return dayCount;
			case -1863622910: // curveNodes
			  return curveNodes;
			case -1730091410: // computeJacobian
			  return computeJacobian;
			case 561141921: // storeNodeTrade
			  return storeNodeTrade;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name = (CurveName) newValue;
			  break;
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			case 318917792: // curveValuationDate
			  this.curveValuationDate = (LocalDate) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount = (DayCount) newValue;
			  break;
			case -1863622910: // curveNodes
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.curveNodes = (java.util.List<? extends IsdaCreditCurveNode>) newValue;
			  this.curveNodes = (IList<IsdaCreditCurveNode>) newValue;
			  break;
			case -1730091410: // computeJacobian
			  this.computeJacobian = (bool?) newValue.Value;
			  break;
			case 561141921: // storeNodeTrade
			  this.storeNodeTrade = (bool?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IsdaCreditCurveDefinition build()
		{
		  return new IsdaCreditCurveDefinition(name, currency, curveValuationDate, dayCount, curveNodes, computeJacobian, storeNodeTrade);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("IsdaCreditCurveDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("curveValuationDate").Append('=').Append(JodaBeanUtils.ToString(curveValuationDate)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount)).Append(',').Append(' ');
		  buf.Append("curveNodes").Append('=').Append(JodaBeanUtils.ToString(curveNodes)).Append(',').Append(' ');
		  buf.Append("computeJacobian").Append('=').Append(JodaBeanUtils.ToString(computeJacobian)).Append(',').Append(' ');
		  buf.Append("storeNodeTrade").Append('=').Append(JodaBeanUtils.ToString(storeNodeTrade));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;

	/// <summary>
	/// A CMS leg of a constant maturity swap (CMS) product, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="CmsLeg"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedCmLegs} from a {@code CmsLeg}
	/// using <seealso cref="CmsLeg#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// This defines a single leg for a CMS product and is formed from a number of periods.
	/// Each period may be a CMS coupon, caplet or floorlet.
	/// </para>
	/// <para>
	/// A {@code ResolvedCmsLeg} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedCmsLeg implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedCmsLeg : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.PayReceive payReceive;
		private readonly PayReceive payReceive;
	  /// <summary>
	  /// The periodic payments based on the successive observed values of a swap index.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<CmsPeriod> cmsPeriods;
	  private readonly ImmutableList<CmsPeriod> cmsPeriods;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ResolvedCmsLeg(com.opengamma.strata.product.common.PayReceive payReceive, java.util.List<CmsPeriod> cmsPeriods)
	  private ResolvedCmsLeg(PayReceive payReceive, IList<CmsPeriod> cmsPeriods)
	  {

		this.payReceive = ArgChecker.notNull(payReceive, "payReceive");
		this.cmsPeriods = ImmutableList.copyOf(cmsPeriods);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<Currency> currencies = this.cmsPeriods.Select(CmsPeriod::getCurrency).collect(Collectors.toSet());
		ArgChecker.isTrue(currencies.Count == 1, "Leg must have a single currency, found: " + currencies);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<SwapIndex> swapIndices = this.cmsPeriods.Select(CmsPeriod::getIndex).collect(Collectors.toSet());
		ArgChecker.isTrue(swapIndices.Count == 1, "Leg must have a single swap index: " + swapIndices);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the leg.
	  /// <para>
	  /// This is the first accrual date in the leg, often known as the effective date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the leg </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return cmsPeriods.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the end date of the leg.
	  /// <para>
	  /// This is the last accrual date in the leg, often known as the maturity date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the leg </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return cmsPeriods.get(cmsPeriods.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the leg.
	  /// <para>
	  /// All periods in the leg will have this currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return cmsPeriods.get(0).Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the swap index of the leg.
	  /// <para>
	  /// All periods in the leg will have this index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  public SwapIndex Index
	  {
		  get
		  {
			return cmsPeriods.get(0).Index;
		  }
	  }

	  /// <summary>
	  /// Gets the underlying Ibor index that the leg is based on.
	  /// <para>
	  /// All periods in the leg will have this index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  public IborIndex UnderlyingIndex
	  {
		  get
		  {
			return Index.Template.Convention.FloatingLeg.Index;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCmsLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedCmsLeg.Meta meta()
	  {
		return ResolvedCmsLeg.Meta.INSTANCE;
	  }

	  static ResolvedCmsLeg()
	  {
		MetaBean.register(ResolvedCmsLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedCmsLeg.Builder builder()
	  {
		return new ResolvedCmsLeg.Builder();
	  }

	  public override ResolvedCmsLeg.Meta metaBean()
	  {
		return ResolvedCmsLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative swap rates can result in a payment in the opposite direction
	  /// to that implied by this indicator.
	  /// </para>
	  /// <para>
	  /// The value of this flag should match the signs of the payment period notionals.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PayReceive PayReceive
	  {
		  get
		  {
			return payReceive;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic payments based on the successive observed values of a swap index.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<CmsPeriod> CmsPeriods
	  {
		  get
		  {
			return cmsPeriods;
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
		  ResolvedCmsLeg other = (ResolvedCmsLeg) obj;
		  return JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(cmsPeriods, other.cmsPeriods);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cmsPeriods);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ResolvedCmsLeg{");
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("cmsPeriods").Append('=').Append(JodaBeanUtils.ToString(cmsPeriods));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCmsLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(ResolvedCmsLeg), typeof(PayReceive));
			  cmsPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "cmsPeriods", typeof(ResolvedCmsLeg), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payReceive", "cmsPeriods");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code payReceive} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PayReceive> payReceive_Renamed;
		/// <summary>
		/// The meta-property for the {@code cmsPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CmsPeriod>> cmsPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "cmsPeriods", ResolvedCmsLeg.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CmsPeriod>> cmsPeriods_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payReceive", "cmsPeriods");
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
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case 2121598281: // cmsPeriods
			  return cmsPeriods_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedCmsLeg.Builder builder()
		{
		  return new ResolvedCmsLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedCmsLeg);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code payReceive} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PayReceive> payReceive()
		{
		  return payReceive_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code cmsPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<CmsPeriod>> cmsPeriods()
		{
		  return cmsPeriods_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return ((ResolvedCmsLeg) bean).PayReceive;
			case 2121598281: // cmsPeriods
			  return ((ResolvedCmsLeg) bean).CmsPeriods;
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
	  /// The bean-builder for {@code ResolvedCmsLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedCmsLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<CmsPeriod> cmsPeriods_Renamed = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedCmsLeg beanToCopy)
		{
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.cmsPeriods_Renamed = beanToCopy.CmsPeriods;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case 2121598281: // cmsPeriods
			  return cmsPeriods_Renamed;
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
			case -885469925: // payReceive
			  this.payReceive_Renamed = (PayReceive) newValue;
			  break;
			case 2121598281: // cmsPeriods
			  this.cmsPeriods_Renamed = (IList<CmsPeriod>) newValue;
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

		public override ResolvedCmsLeg build()
		{
		  return new ResolvedCmsLeg(payReceive_Renamed, cmsPeriods_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
		/// Note that negative swap rates can result in a payment in the opposite direction
		/// to that implied by this indicator.
		/// </para>
		/// <para>
		/// The value of this flag should match the signs of the payment period notionals.
		/// </para>
		/// </summary>
		/// <param name="payReceive">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder payReceive(PayReceive payReceive)
		{
		  JodaBeanUtils.notNull(payReceive, "payReceive");
		  this.payReceive_Renamed = payReceive;
		  return this;
		}

		/// <summary>
		/// Sets the periodic payments based on the successive observed values of a swap index.
		/// <para>
		/// Each payment period represents part of the life-time of the leg.
		/// In most cases, the periods do not overlap. However, since each payment period
		/// is essentially independent the data model allows overlapping periods.
		/// </para>
		/// </summary>
		/// <param name="cmsPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder cmsPeriods(IList<CmsPeriod> cmsPeriods)
		{
		  JodaBeanUtils.notEmpty(cmsPeriods, "cmsPeriods");
		  this.cmsPeriods_Renamed = cmsPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code cmsPeriods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="cmsPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder cmsPeriods(params CmsPeriod[] cmsPeriods)
		{
		  return this.cmsPeriods(ImmutableList.copyOf(cmsPeriods));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ResolvedCmsLeg.Builder{");
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("cmsPeriods").Append('=').Append(JodaBeanUtils.ToString(cmsPeriods_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
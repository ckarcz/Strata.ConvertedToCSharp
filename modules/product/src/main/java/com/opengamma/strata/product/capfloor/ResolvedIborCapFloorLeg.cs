using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.capfloor
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

	/// <summary>
	/// An Ibor cap/floor leg of an Ibor cap/floor product, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="IborCapFloorLeg"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedCmLegs} from a {@code IborCapFloorLeg}
	/// using <seealso cref="IborCapFloorLeg#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// This defines a single leg for an Ibor cap/floor product and is formed from a number of periods.
	/// Each period may be a caplet or floorlet.
	/// The cap/floor instruments are defined as a set of call/put options on successive Ibor index rates.
	/// </para>
	/// <para>
	/// A {@code ResolvedIborCapFloorLeg} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedIborCapFloorLeg implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedIborCapFloorLeg : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.PayReceive payReceive;
		private readonly PayReceive payReceive;
	  /// <summary>
	  /// The periodic payments based on the successive observed values of an Ibor index.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<IborCapletFloorletPeriod> capletFloorletPeriods;
	  private readonly ImmutableList<IborCapletFloorletPeriod> capletFloorletPeriods;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ResolvedIborCapFloorLeg(com.opengamma.strata.product.common.PayReceive payReceive, java.util.List<IborCapletFloorletPeriod> capletFloorletPeriods)
	  private ResolvedIborCapFloorLeg(PayReceive payReceive, IList<IborCapletFloorletPeriod> capletFloorletPeriods)
	  {

		this.payReceive = ArgChecker.notNull(payReceive, "payReceive");
		this.capletFloorletPeriods = ImmutableList.copyOf(capletFloorletPeriods);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<Currency> currencies = this.capletFloorletPeriods.Select(IborCapletFloorletPeriod::getCurrency).collect(Collectors.toSet());
		ArgChecker.isTrue(currencies.Count == 1, "Leg must have a single currency, found: " + currencies);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<IborIndex> iborIndices = this.capletFloorletPeriods.Select(IborCapletFloorletPeriod::getIndex).collect(Collectors.toSet());
		ArgChecker.isTrue(iborIndices.Count == 1, "Leg must have a single Ibor index: " + iborIndices);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start date of the leg.
	  /// <para>
	  /// This is the first accrual date in the leg, often known as the effective date.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the leg </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return capletFloorletPeriods.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the accrual end date of the leg.
	  /// <para>
	  /// This is the last accrual date in the leg, often known as the termination date.
	  /// This date has typically been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the leg </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return capletFloorletPeriods.get(capletFloorletPeriods.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the fixing date time of the final caplet/floorlet period.
	  /// </summary>
	  /// <returns> the fixing date time </returns>
	  public ZonedDateTime FinalFixingDateTime
	  {
		  get
		  {
			return capletFloorletPeriods.get(capletFloorletPeriods.size() - 1).FixingDateTime;
		  }
	  }

	  /// <summary>
	  /// Gets the final caplet/floorlet period.
	  /// </summary>
	  /// <returns> the final period </returns>
	  public IborCapletFloorletPeriod FinalPeriod
	  {
		  get
		  {
			return capletFloorletPeriods.get(capletFloorletPeriods.size() - 1);
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
			return capletFloorletPeriods.get(0).Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the Ibor index of the leg.
	  /// <para>
	  /// All periods in the leg will have this index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the index </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return capletFloorletPeriods.get(0).Index;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborCapFloorLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedIborCapFloorLeg.Meta meta()
	  {
		return ResolvedIborCapFloorLeg.Meta.INSTANCE;
	  }

	  static ResolvedIborCapFloorLeg()
	  {
		MetaBean.register(ResolvedIborCapFloorLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedIborCapFloorLeg.Builder builder()
	  {
		return new ResolvedIborCapFloorLeg.Builder();
	  }

	  public override ResolvedIborCapFloorLeg.Meta metaBean()
	  {
		return ResolvedIborCapFloorLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
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
	  /// Gets the periodic payments based on the successive observed values of an Ibor index.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<IborCapletFloorletPeriod> CapletFloorletPeriods
	  {
		  get
		  {
			return capletFloorletPeriods;
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
		  ResolvedIborCapFloorLeg other = (ResolvedIborCapFloorLeg) obj;
		  return JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(capletFloorletPeriods, other.capletFloorletPeriods);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(capletFloorletPeriods);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ResolvedIborCapFloorLeg{");
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("capletFloorletPeriods").Append('=').Append(JodaBeanUtils.ToString(capletFloorletPeriods));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborCapFloorLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(ResolvedIborCapFloorLeg), typeof(PayReceive));
			  capletFloorletPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "capletFloorletPeriods", typeof(ResolvedIborCapFloorLeg), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payReceive", "capletFloorletPeriods");
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
		/// The meta-property for the {@code capletFloorletPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<IborCapletFloorletPeriod>> capletFloorletPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "capletFloorletPeriods", ResolvedIborCapFloorLeg.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<IborCapletFloorletPeriod>> capletFloorletPeriods_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payReceive", "capletFloorletPeriods");
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
			case 1504863482: // capletFloorletPeriods
			  return capletFloorletPeriods_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedIborCapFloorLeg.Builder builder()
		{
		  return new ResolvedIborCapFloorLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedIborCapFloorLeg);
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
		/// The meta-property for the {@code capletFloorletPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<IborCapletFloorletPeriod>> capletFloorletPeriods()
		{
		  return capletFloorletPeriods_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return ((ResolvedIborCapFloorLeg) bean).PayReceive;
			case 1504863482: // capletFloorletPeriods
			  return ((ResolvedIborCapFloorLeg) bean).CapletFloorletPeriods;
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
	  /// The bean-builder for {@code ResolvedIborCapFloorLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedIborCapFloorLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<IborCapletFloorletPeriod> capletFloorletPeriods_Renamed = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedIborCapFloorLeg beanToCopy)
		{
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.capletFloorletPeriods_Renamed = beanToCopy.CapletFloorletPeriods;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case 1504863482: // capletFloorletPeriods
			  return capletFloorletPeriods_Renamed;
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
			case 1504863482: // capletFloorletPeriods
			  this.capletFloorletPeriods_Renamed = (IList<IborCapletFloorletPeriod>) newValue;
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

		public override ResolvedIborCapFloorLeg build()
		{
		  return new ResolvedIborCapFloorLeg(payReceive_Renamed, capletFloorletPeriods_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
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
		/// Sets the periodic payments based on the successive observed values of an Ibor index.
		/// <para>
		/// Each payment period represents part of the life-time of the leg.
		/// In most cases, the periods do not overlap. However, since each payment period
		/// is essentially independent the data model allows overlapping periods.
		/// </para>
		/// </summary>
		/// <param name="capletFloorletPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder capletFloorletPeriods(IList<IborCapletFloorletPeriod> capletFloorletPeriods)
		{
		  JodaBeanUtils.notEmpty(capletFloorletPeriods, "capletFloorletPeriods");
		  this.capletFloorletPeriods_Renamed = capletFloorletPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code capletFloorletPeriods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="capletFloorletPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder capletFloorletPeriods(params IborCapletFloorletPeriod[] capletFloorletPeriods)
		{
		  return this.capletFloorletPeriods(ImmutableList.copyOf(capletFloorletPeriods));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ResolvedIborCapFloorLeg.Builder{");
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("capletFloorletPeriods").Append('=').Append(JodaBeanUtils.ToString(capletFloorletPeriods_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
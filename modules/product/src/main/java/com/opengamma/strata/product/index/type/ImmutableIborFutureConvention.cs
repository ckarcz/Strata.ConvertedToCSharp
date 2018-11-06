using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;


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
	using DateSequence = com.opengamma.strata.basics.date.DateSequence;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;

	/// <summary>
	/// A market convention for Ibor Future trades.
	/// <para>
	/// This defines the market convention for a future against a particular index.
	/// In most cases, the index contains sufficient information to fully define the convention.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableIborFutureConvention implements IborFutureConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableIborFutureConvention : IborFutureConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.IborIndex index;
		private readonly IborIndex index;
	  /// <summary>
	  /// The convention name, such as 'USD-LIBOR-3M-Quarterly-IMM'.
	  /// <para>
	  /// This will default to the name of the index suffixed by the name of the date sequence if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
	  private readonly string name;
	  /// <summary>
	  /// The sequence of dates that the future is based on.
	  /// <para>
	  /// This is used to calculate the reference date of the future that is the start
	  /// date of the underlying synthetic deposit.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DateSequence dateSequence;
	  private readonly DateSequence dateSequence;
	  /// <summary>
	  /// The business day adjustment to apply to the reference date.
	  /// <para>
	  /// The reference date, which is often the third Wednesday of the month, will be adjusted as defined here.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a convention based on the specified index and the sequence of dates.
	  /// <para>
	  /// The standard market convention is based on the index.
	  /// The business day adjustment is set to be 'Following' using the effective date calendar from the index.
	  /// The convention name will default to the name of the index suffixed by the
	  /// name of the date sequence.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index, the calendar for the adjustment is extracted from the index </param>
	  /// <param name="dateSequence">  the sequence of dates </param>
	  /// <returns> the convention </returns>
	  public static ImmutableIborFutureConvention of(IborIndex index, DateSequence dateSequence)
	  {
		return ImmutableIborFutureConvention.builder().index(index).dateSequence(dateSequence).build();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.index_Renamed != null)
		{
		  if (string.ReferenceEquals(builder.name_Renamed, null) && builder.dateSequence_Renamed != null)
		  {
			builder.name_Renamed = builder.index_Renamed.Name + "-" + builder.dateSequence_Renamed.Name;
		  }
		  if (builder.businessDayAdjustment_Renamed == null)
		  {
			builder.businessDayAdjustment_Renamed = BusinessDayAdjustment.of(FOLLOWING, builder.index_Renamed.EffectiveDateOffset.Calendar);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, Period minimumPeriod, int sequenceNumber, double quantity, double notional, double price, ReferenceData refData)
	  {

		LocalDate referenceDate = calculateReferenceDateFromTradeDate(tradeDate, minimumPeriod, sequenceNumber, refData);
		LocalDate lastTradeDate = index.calculateFixingFromEffective(referenceDate, refData);
		YearMonth yearMonth = YearMonth.from(lastTradeDate);
		return createTrade(tradeDate, securityId, quantity, notional, price, yearMonth, lastTradeDate, referenceDate);
	  }

	  public IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, YearMonth yearMonth, double quantity, double notional, double price, ReferenceData refData)
	  {

		LocalDate referenceDate = calculateReferenceDateFromTradeDate(tradeDate, yearMonth, refData);
		LocalDate lastTradeDate = index.calculateFixingFromEffective(referenceDate, refData);
		return createTrade(tradeDate, securityId, quantity, notional, price, yearMonth, lastTradeDate, referenceDate);
	  }

	  private IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, double quantity, double notional, double price, YearMonth yearMonth, LocalDate lastTradeDate, LocalDate referenceDate)
	  {

		double accrualFactor = index.Tenor.get(ChronoUnit.MONTHS) / 12.0;
		IborFuture product = IborFuture.builder().securityId(securityId).index(index).accrualFactor(accrualFactor).lastTradeDate(lastTradeDate).notional(notional).build();
		TradeInfo info = TradeInfo.of(tradeDate);
		return IborFutureTrade.builder().info(info).product(product).quantity(quantity).price(price).build();
	  }

	  public LocalDate calculateReferenceDateFromTradeDate(LocalDate tradeDate, Period minimumPeriod, int sequenceNumber, ReferenceData refData)
	  {

		LocalDate earliestDate = tradeDate.plus(minimumPeriod);
		LocalDate referenceDate = dateSequence.nthOrSame(earliestDate, sequenceNumber);
		return businessDayAdjustment.adjust(referenceDate, refData);
	  }

	  public LocalDate calculateReferenceDateFromTradeDate(LocalDate tradeDate, YearMonth yearMonth, ReferenceData refData)
	  {

		LocalDate referenceDate = dateSequence.dateMatching(yearMonth);
		return businessDayAdjustment.adjust(referenceDate, refData);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableIborFutureConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableIborFutureConvention.Meta meta()
	  {
		return ImmutableIborFutureConvention.Meta.INSTANCE;
	  }

	  static ImmutableIborFutureConvention()
	  {
		MetaBean.register(ImmutableIborFutureConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableIborFutureConvention.Builder builder()
	  {
		return new ImmutableIborFutureConvention.Builder();
	  }

	  private ImmutableIborFutureConvention(IborIndex index, string name, DateSequence dateSequence, BusinessDayAdjustment businessDayAdjustment)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(dateSequence, "dateSequence");
		JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		this.index = index;
		this.name = name;
		this.dateSequence = dateSequence;
		this.businessDayAdjustment = businessDayAdjustment;
	  }

	  public override ImmutableIborFutureConvention.Meta metaBean()
	  {
		return ImmutableIborFutureConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The floating rate to be paid is based on this index
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
	  /// Gets the convention name, such as 'USD-LIBOR-3M-Quarterly-IMM'.
	  /// <para>
	  /// This will default to the name of the index suffixed by the name of the date sequence if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sequence of dates that the future is based on.
	  /// <para>
	  /// This is used to calculate the reference date of the future that is the start
	  /// date of the underlying synthetic deposit.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DateSequence DateSequence
	  {
		  get
		  {
			return dateSequence;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply to the reference date.
	  /// <para>
	  /// The reference date, which is often the third Wednesday of the month, will be adjusted as defined here.
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
		  ImmutableIborFutureConvention other = (ImmutableIborFutureConvention) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(dateSequence, other.dateSequence) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dateSequence);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableIborFutureConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(ImmutableIborFutureConvention), typeof(IborIndex));
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableIborFutureConvention), typeof(string));
			  dateSequence_Renamed = DirectMetaProperty.ofImmutable(this, "dateSequence", typeof(ImmutableIborFutureConvention), typeof(DateSequence));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(ImmutableIborFutureConvention), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "name", "dateSequence", "businessDayAdjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code dateSequence} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DateSequence> dateSequence_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "name", "dateSequence", "businessDayAdjustment");
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
			case 3373707: // name
			  return name_Renamed;
			case -258065009: // dateSequence
			  return dateSequence_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableIborFutureConvention.Builder builder()
		{
		  return new ImmutableIborFutureConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableIborFutureConvention);
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
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dateSequence} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DateSequence> dateSequence()
		{
		  return dateSequence_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((ImmutableIborFutureConvention) bean).Index;
			case 3373707: // name
			  return ((ImmutableIborFutureConvention) bean).Name;
			case -258065009: // dateSequence
			  return ((ImmutableIborFutureConvention) bean).DateSequence;
			case -1065319863: // businessDayAdjustment
			  return ((ImmutableIborFutureConvention) bean).BusinessDayAdjustment;
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
	  /// The bean-builder for {@code ImmutableIborFutureConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableIborFutureConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DateSequence dateSequence_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableIborFutureConvention beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.name_Renamed = beanToCopy.Name;
		  this.dateSequence_Renamed = beanToCopy.DateSequence;
		  this.businessDayAdjustment_Renamed = beanToCopy.BusinessDayAdjustment;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 3373707: // name
			  return name_Renamed;
			case -258065009: // dateSequence
			  return dateSequence_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case -258065009: // dateSequence
			  this.dateSequence_Renamed = (DateSequence) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
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

		public override ImmutableIborFutureConvention build()
		{
		  preBuild(this);
		  return new ImmutableIborFutureConvention(index_Renamed, name_Renamed, dateSequence_Renamed, businessDayAdjustment_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Ibor index.
		/// <para>
		/// The floating rate to be paid is based on this index
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
		/// Sets the convention name, such as 'USD-LIBOR-3M-Quarterly-IMM'.
		/// <para>
		/// This will default to the name of the index suffixed by the name of the date sequence if not specified.
		/// </para>
		/// </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the sequence of dates that the future is based on.
		/// <para>
		/// This is used to calculate the reference date of the future that is the start
		/// date of the underlying synthetic deposit.
		/// </para>
		/// </summary>
		/// <param name="dateSequence">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dateSequence(DateSequence dateSequence)
		{
		  JodaBeanUtils.notNull(dateSequence, "dateSequence");
		  this.dateSequence_Renamed = dateSequence;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the reference date.
		/// <para>
		/// The reference date, which is often the third Wednesday of the month, will be adjusted as defined here.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableIborFutureConvention.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("dateSequence").Append('=').Append(JodaBeanUtils.ToString(dateSequence_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.ArgChecker.inOrderOrEqual;


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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using FxProduct = com.opengamma.strata.product.fx.FxProduct;
	using FxSingle = com.opengamma.strata.product.fx.FxSingle;

	/// <summary>
	/// A vanilla FX option.
	/// <para>
	/// An FX option is a financial instrument that provides an option based on the future value of
	/// a foreign exchange. The option is European, exercised only on the exercise date.
	/// </para>
	/// <para>
	/// For example, a call on a 'EUR 1.00 / USD -1.41' exchange is the option to
	/// perform a foreign exchange on the expiry date, where USD 1.41 is paid to receive EUR 1.00.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxVanillaOption implements com.opengamma.strata.product.fx.FxProduct, com.opengamma.strata.basics.Resolvable<ResolvedFxVanillaOption>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxVanillaOption : FxProduct, Resolvable<ResolvedFxVanillaOption>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.LongShort longShort;
		private readonly LongShort longShort;
	  /// <summary>
	  /// The expiry date of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate expiryDate;
	  private readonly LocalDate expiryDate;
	  /// <summary>
	  /// The expiry time of the option.
	  /// <para>
	  /// The expiry time is related to the expiry date and time-zone.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalTime expiryTime;
	  private readonly LocalTime expiryTime;
	  /// <summary>
	  /// The time-zone of the expiry time.
	  /// <para>
	  /// The expiry time-zone is related to the expiry date and time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZoneId expiryZone;
	  private readonly ZoneId expiryZone;
	  /// <summary>
	  /// The underlying foreign exchange transaction.
	  /// <para>
	  /// At expiry, if the option is in the money, this foreign exchange will occur.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.fx.FxSingle underlying;
	  private readonly FxSingle underlying;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		inOrderOrEqual(expiryDate, underlying.PaymentDate, "expiryDate", "underlying.paymentDate");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets currency pair of the base currency and counter currency.
	  /// <para>
	  /// This currency pair is conventional, thus indifferent to the direction of FX.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return underlying.CurrencyPair;
		  }
	  }

	  /// <summary>
	  /// Gets the expiry date-time.
	  /// <para>
	  /// The option expires at this date and time.
	  /// </para>
	  /// <para>
	  /// The result is returned by combining the expiry date, time and time-zone.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the expiry date and time </returns>
	  public ZonedDateTime Expiry
	  {
		  get
		  {
			return expiryDate.atTime(expiryTime).atZone(expiryZone);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedFxVanillaOption resolve(ReferenceData refData)
	  {
		return ResolvedFxVanillaOption.builder().longShort(longShort).expiry(Expiry).underlying(underlying.resolve(refData)).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxVanillaOption}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxVanillaOption.Meta meta()
	  {
		return FxVanillaOption.Meta.INSTANCE;
	  }

	  static FxVanillaOption()
	  {
		MetaBean.register(FxVanillaOption.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxVanillaOption.Builder builder()
	  {
		return new FxVanillaOption.Builder();
	  }

	  private FxVanillaOption(LongShort longShort, LocalDate expiryDate, LocalTime expiryTime, ZoneId expiryZone, FxSingle underlying)
	  {
		JodaBeanUtils.notNull(longShort, "longShort");
		JodaBeanUtils.notNull(expiryDate, "expiryDate");
		JodaBeanUtils.notNull(expiryTime, "expiryTime");
		JodaBeanUtils.notNull(expiryZone, "expiryZone");
		JodaBeanUtils.notNull(underlying, "underlying");
		this.longShort = longShort;
		this.expiryDate = expiryDate;
		this.expiryTime = expiryTime;
		this.expiryZone = expiryZone;
		this.underlying = underlying;
		validate();
	  }

	  public override FxVanillaOption.Meta metaBean()
	  {
		return FxVanillaOption.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the option is long or short.
	  /// <para>
	  /// At expiry, the long party will have the option to enter in this transaction;
	  /// the short party will, at the option of the long party, potentially enter into the inverse transaction.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LongShort LongShort
	  {
		  get
		  {
			return longShort;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ExpiryDate
	  {
		  get
		  {
			return expiryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry time of the option.
	  /// <para>
	  /// The expiry time is related to the expiry date and time-zone.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalTime ExpiryTime
	  {
		  get
		  {
			return expiryTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time-zone of the expiry time.
	  /// <para>
	  /// The expiry time-zone is related to the expiry date and time.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZoneId ExpiryZone
	  {
		  get
		  {
			return expiryZone;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying foreign exchange transaction.
	  /// <para>
	  /// At expiry, if the option is in the money, this foreign exchange will occur.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxSingle Underlying
	  {
		  get
		  {
			return underlying;
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
		  FxVanillaOption other = (FxVanillaOption) obj;
		  return JodaBeanUtils.equal(longShort, other.longShort) && JodaBeanUtils.equal(expiryDate, other.expiryDate) && JodaBeanUtils.equal(expiryTime, other.expiryTime) && JodaBeanUtils.equal(expiryZone, other.expiryZone) && JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longShort);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiryZone);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("FxVanillaOption{");
		buf.Append("longShort").Append('=').Append(longShort).Append(',').Append(' ');
		buf.Append("expiryDate").Append('=').Append(expiryDate).Append(',').Append(' ');
		buf.Append("expiryTime").Append('=').Append(expiryTime).Append(',').Append(' ');
		buf.Append("expiryZone").Append('=').Append(expiryZone).Append(',').Append(' ');
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxVanillaOption}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  longShort_Renamed = DirectMetaProperty.ofImmutable(this, "longShort", typeof(FxVanillaOption), typeof(LongShort));
			  expiryDate_Renamed = DirectMetaProperty.ofImmutable(this, "expiryDate", typeof(FxVanillaOption), typeof(LocalDate));
			  expiryTime_Renamed = DirectMetaProperty.ofImmutable(this, "expiryTime", typeof(FxVanillaOption), typeof(LocalTime));
			  expiryZone_Renamed = DirectMetaProperty.ofImmutable(this, "expiryZone", typeof(FxVanillaOption), typeof(ZoneId));
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(FxVanillaOption), typeof(FxSingle));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "longShort", "expiryDate", "expiryTime", "expiryZone", "underlying");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code longShort} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LongShort> longShort_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> expiryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalTime> expiryTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiryZone} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZoneId> expiryZone_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxSingle> underlying_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "longShort", "expiryDate", "expiryTime", "expiryZone", "underlying");
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
			case 116685664: // longShort
			  return longShort_Renamed;
			case -816738431: // expiryDate
			  return expiryDate_Renamed;
			case -816254304: // expiryTime
			  return expiryTime_Renamed;
			case -816069761: // expiryZone
			  return expiryZone_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxVanillaOption.Builder builder()
		{
		  return new FxVanillaOption.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxVanillaOption);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code longShort} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LongShort> longShort()
		{
		  return longShort_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> expiryDate()
		{
		  return expiryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalTime> expiryTime()
		{
		  return expiryTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiryZone} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZoneId> expiryZone()
		{
		  return expiryZone_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxSingle> underlying()
		{
		  return underlying_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return ((FxVanillaOption) bean).LongShort;
			case -816738431: // expiryDate
			  return ((FxVanillaOption) bean).ExpiryDate;
			case -816254304: // expiryTime
			  return ((FxVanillaOption) bean).ExpiryTime;
			case -816069761: // expiryZone
			  return ((FxVanillaOption) bean).ExpiryZone;
			case -1770633379: // underlying
			  return ((FxVanillaOption) bean).Underlying;
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
	  /// The bean-builder for {@code FxVanillaOption}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxVanillaOption>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LongShort longShort_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate expiryDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalTime expiryTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZoneId expiryZone_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxSingle underlying_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FxVanillaOption beanToCopy)
		{
		  this.longShort_Renamed = beanToCopy.LongShort;
		  this.expiryDate_Renamed = beanToCopy.ExpiryDate;
		  this.expiryTime_Renamed = beanToCopy.ExpiryTime;
		  this.expiryZone_Renamed = beanToCopy.ExpiryZone;
		  this.underlying_Renamed = beanToCopy.Underlying;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return longShort_Renamed;
			case -816738431: // expiryDate
			  return expiryDate_Renamed;
			case -816254304: // expiryTime
			  return expiryTime_Renamed;
			case -816069761: // expiryZone
			  return expiryZone_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  this.longShort_Renamed = (LongShort) newValue;
			  break;
			case -816738431: // expiryDate
			  this.expiryDate_Renamed = (LocalDate) newValue;
			  break;
			case -816254304: // expiryTime
			  this.expiryTime_Renamed = (LocalTime) newValue;
			  break;
			case -816069761: // expiryZone
			  this.expiryZone_Renamed = (ZoneId) newValue;
			  break;
			case -1770633379: // underlying
			  this.underlying_Renamed = (FxSingle) newValue;
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

		public override FxVanillaOption build()
		{
		  return new FxVanillaOption(longShort_Renamed, expiryDate_Renamed, expiryTime_Renamed, expiryZone_Renamed, underlying_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the option is long or short.
		/// <para>
		/// At expiry, the long party will have the option to enter in this transaction;
		/// the short party will, at the option of the long party, potentially enter into the inverse transaction.
		/// </para>
		/// </summary>
		/// <param name="longShort">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder longShort(LongShort longShort)
		{
		  JodaBeanUtils.notNull(longShort, "longShort");
		  this.longShort_Renamed = longShort;
		  return this;
		}

		/// <summary>
		/// Sets the expiry date of the option.
		/// <para>
		/// The option is European, and can only be exercised on the expiry date.
		/// </para>
		/// </summary>
		/// <param name="expiryDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryDate(LocalDate expiryDate)
		{
		  JodaBeanUtils.notNull(expiryDate, "expiryDate");
		  this.expiryDate_Renamed = expiryDate;
		  return this;
		}

		/// <summary>
		/// Sets the expiry time of the option.
		/// <para>
		/// The expiry time is related to the expiry date and time-zone.
		/// </para>
		/// </summary>
		/// <param name="expiryTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryTime(LocalTime expiryTime)
		{
		  JodaBeanUtils.notNull(expiryTime, "expiryTime");
		  this.expiryTime_Renamed = expiryTime;
		  return this;
		}

		/// <summary>
		/// Sets the time-zone of the expiry time.
		/// <para>
		/// The expiry time-zone is related to the expiry date and time.
		/// </para>
		/// </summary>
		/// <param name="expiryZone">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiryZone(ZoneId expiryZone)
		{
		  JodaBeanUtils.notNull(expiryZone, "expiryZone");
		  this.expiryZone_Renamed = expiryZone;
		  return this;
		}

		/// <summary>
		/// Sets the underlying foreign exchange transaction.
		/// <para>
		/// At expiry, if the option is in the money, this foreign exchange will occur.
		/// </para>
		/// </summary>
		/// <param name="underlying">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlying(FxSingle underlying)
		{
		  JodaBeanUtils.notNull(underlying, "underlying");
		  this.underlying_Renamed = underlying;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("FxVanillaOption.Builder{");
		  buf.Append("longShort").Append('=').Append(JodaBeanUtils.ToString(longShort_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryDate").Append('=').Append(JodaBeanUtils.ToString(expiryDate_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryTime").Append('=').Append(JodaBeanUtils.ToString(expiryTime_Renamed)).Append(',').Append(' ');
		  buf.Append("expiryZone").Append('=').Append(JodaBeanUtils.ToString(expiryZone_Renamed)).Append(',').Append(' ');
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
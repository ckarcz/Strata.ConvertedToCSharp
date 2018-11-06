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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;

	/// <summary>
	/// A vanilla FX option, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="FxVanillaOption"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedFxVanillaOption} from a {@code FxVanillaOption}
	/// using <seealso cref="FxVanillaOption#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedFxVanillaOption} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedFxVanillaOption implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedFxVanillaOption : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.LongShort longShort;
		private readonly LongShort longShort;
	  /// <summary>
	  /// The expiry date-time of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZonedDateTime expiry;
	  private readonly ZonedDateTime expiry;
	  /// <summary>
	  /// The underlying foreign exchange transaction.
	  /// <para>
	  /// At expiry, if the option is in the money, this foreign exchange will occur.
	  /// A call option permits the transaction as specified to occur.
	  /// A put option permits the inverse transaction to occur.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.fx.ResolvedFxSingle underlying;
	  private readonly ResolvedFxSingle underlying;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		inOrderOrEqual(expiry.toLocalDate(), underlying.PaymentDate, "expiry.date", "underlying.paymentDate");
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
	  /// Gets the expiry date of the option.
	  /// </summary>
	  /// <returns> the expiry date </returns>
	  public LocalDate ExpiryDate
	  {
		  get
		  {
			return expiry.toLocalDate();
		  }
	  }

	  /// <summary>
	  /// Gets the strike rate.
	  /// </summary>
	  /// <returns> the strike </returns>
	  public double Strike
	  {
		  get
		  {
			return Math.Abs(underlying.CounterCurrencyPayment.Amount / underlying.BaseCurrencyPayment.Amount);
		  }
	  }

	  /// <summary>
	  /// Returns the put/call flag.
	  /// <para>
	  /// This is the put/call for the base currency.
	  /// If the amount for the base currency is positive, the option is a call on the base currency (put on counter currency). 
	  /// If the amount for the base currency is negative, the option is a put on the base currency (call on counter currency).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the put or call </returns>
	  public PutCall PutCall
	  {
		  get
		  {
			return underlying.CounterCurrencyPayment.Amount > 0d ? PutCall.PUT : PutCall.CALL;
		  }
	  }

	  /// <summary>
	  /// Get the counter currency of the underlying FX transaction.
	  /// </summary>
	  /// <returns> the counter currency </returns>
	  public Currency CounterCurrency
	  {
		  get
		  {
			return underlying.CounterCurrencyPayment.Currency;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxVanillaOption}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedFxVanillaOption.Meta meta()
	  {
		return ResolvedFxVanillaOption.Meta.INSTANCE;
	  }

	  static ResolvedFxVanillaOption()
	  {
		MetaBean.register(ResolvedFxVanillaOption.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedFxVanillaOption.Builder builder()
	  {
		return new ResolvedFxVanillaOption.Builder();
	  }

	  private ResolvedFxVanillaOption(LongShort longShort, ZonedDateTime expiry, ResolvedFxSingle underlying)
	  {
		JodaBeanUtils.notNull(longShort, "longShort");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(underlying, "underlying");
		this.longShort = longShort;
		this.expiry = expiry;
		this.underlying = underlying;
		validate();
	  }

	  public override ResolvedFxVanillaOption.Meta metaBean()
	  {
		return ResolvedFxVanillaOption.Meta.INSTANCE;
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
	  /// Gets the expiry date-time of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZonedDateTime Expiry
	  {
		  get
		  {
			return expiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying foreign exchange transaction.
	  /// <para>
	  /// At expiry, if the option is in the money, this foreign exchange will occur.
	  /// A call option permits the transaction as specified to occur.
	  /// A put option permits the inverse transaction to occur.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ResolvedFxSingle Underlying
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
		  ResolvedFxVanillaOption other = (ResolvedFxVanillaOption) obj;
		  return JodaBeanUtils.equal(longShort, other.longShort) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longShort);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ResolvedFxVanillaOption{");
		buf.Append("longShort").Append('=').Append(longShort).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxVanillaOption}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  longShort_Renamed = DirectMetaProperty.ofImmutable(this, "longShort", typeof(ResolvedFxVanillaOption), typeof(LongShort));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(ResolvedFxVanillaOption), typeof(ZonedDateTime));
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(ResolvedFxVanillaOption), typeof(ResolvedFxSingle));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "longShort", "expiry", "underlying");
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
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedFxSingle> underlying_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "longShort", "expiry", "underlying");
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
			case -1289159373: // expiry
			  return expiry_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedFxVanillaOption.Builder builder()
		{
		  return new ResolvedFxVanillaOption.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedFxVanillaOption);
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
		/// The meta-property for the {@code expiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> expiry()
		{
		  return expiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedFxSingle> underlying()
		{
		  return underlying_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return ((ResolvedFxVanillaOption) bean).LongShort;
			case -1289159373: // expiry
			  return ((ResolvedFxVanillaOption) bean).Expiry;
			case -1770633379: // underlying
			  return ((ResolvedFxVanillaOption) bean).Underlying;
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
	  /// The bean-builder for {@code ResolvedFxVanillaOption}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedFxVanillaOption>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LongShort longShort_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime expiry_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ResolvedFxSingle underlying_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedFxVanillaOption beanToCopy)
		{
		  this.longShort_Renamed = beanToCopy.LongShort;
		  this.expiry_Renamed = beanToCopy.Expiry;
		  this.underlying_Renamed = beanToCopy.Underlying;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return longShort_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
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
			case -1289159373: // expiry
			  this.expiry_Renamed = (ZonedDateTime) newValue;
			  break;
			case -1770633379: // underlying
			  this.underlying_Renamed = (ResolvedFxSingle) newValue;
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

		public override ResolvedFxVanillaOption build()
		{
		  return new ResolvedFxVanillaOption(longShort_Renamed, expiry_Renamed, underlying_Renamed);
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
		/// Sets the expiry date-time of the option.
		/// <para>
		/// The option is European, and can only be exercised on the expiry date.
		/// </para>
		/// </summary>
		/// <param name="expiry">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiry(ZonedDateTime expiry)
		{
		  JodaBeanUtils.notNull(expiry, "expiry");
		  this.expiry_Renamed = expiry;
		  return this;
		}

		/// <summary>
		/// Sets the underlying foreign exchange transaction.
		/// <para>
		/// At expiry, if the option is in the money, this foreign exchange will occur.
		/// A call option permits the transaction as specified to occur.
		/// A put option permits the inverse transaction to occur.
		/// </para>
		/// </summary>
		/// <param name="underlying">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlying(ResolvedFxSingle underlying)
		{
		  JodaBeanUtils.notNull(underlying, "underlying");
		  this.underlying_Renamed = underlying;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ResolvedFxVanillaOption.Builder{");
		  buf.Append("longShort").Append('=').Append(JodaBeanUtils.ToString(longShort_Renamed)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry_Renamed)).Append(',').Append(' ');
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
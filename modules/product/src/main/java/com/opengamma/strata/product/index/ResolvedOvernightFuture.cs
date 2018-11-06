using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using OvernightRateComputation = com.opengamma.strata.product.rate.OvernightRateComputation;

	/// <summary>
	/// A futures contract based on an Overnight index, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="OvernightFuture"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedOvernightFuture} from a {@code OvernightFuture}
	/// using <seealso cref="OvernightFuture#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedOvernightFuture} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// 
	/// <h4>Price</h4>
	/// The price of an Overnight rate future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Overnight rate futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class ResolvedOvernightFuture implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedOvernightFuture : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// The currency that the future is traded in.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// This is the full notional of the deposit, such as 5 million dollars.
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The accrual factor, defaulted from the index if not set.
	  /// <para>
	  /// This is the year fraction of the contract, typically 1/12 for a 30-day future.
	  /// The year fraction must be positive.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double accrualFactor;
	  private readonly double accrualFactor;
	  /// <summary>
	  /// The last date of trading.
	  /// <para>
	  /// This must be a valid business day on the fixing calendar of {@code index}.
	  /// The last trade date is typically the last business day of the month.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastTradeDate;
	  private readonly LocalDate lastTradeDate;
	  /// <summary>
	  /// The Overnight rate observation.
	  /// <para>
	  /// The future is based on this index.
	  /// It will be a well known market index such as 'USD-FED-FUND'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.rate.OvernightRateComputation overnightRate;
	  private readonly OvernightRateComputation overnightRate;
	  /// <summary>
	  /// The definition of how to round the futures price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 for a 0.2875% rate is
	  /// represented as 0.997125 which has 6 decimal places.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.Rounding rounding;
	  private readonly Rounding rounding;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.rounding(Rounding.none());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.overnightRate_Renamed != null)
		{
		  if (builder.currency_Renamed == null)
		  {
			builder.currency_Renamed = builder.overnightRate_Renamed.Index.Currency;
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight index that the future is based on.
	  /// </summary>
	  /// <returns> the Overnight index </returns>
	  public OvernightIndex Index
	  {
		  get
		  {
			return overnightRate.Index;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedOvernightFuture}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedOvernightFuture.Meta meta()
	  {
		return ResolvedOvernightFuture.Meta.INSTANCE;
	  }

	  static ResolvedOvernightFuture()
	  {
		MetaBean.register(ResolvedOvernightFuture.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedOvernightFuture.Builder builder()
	  {
		return new ResolvedOvernightFuture.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="securityId">  the value of the property, not null </param>
	  /// <param name="currency">  the value of the property, not null </param>
	  /// <param name="notional">  the value of the property </param>
	  /// <param name="accrualFactor">  the value of the property </param>
	  /// <param name="lastTradeDate">  the value of the property, not null </param>
	  /// <param name="overnightRate">  the value of the property, not null </param>
	  /// <param name="rounding">  the value of the property, not null </param>
	  internal ResolvedOvernightFuture(SecurityId securityId, Currency currency, double notional, double accrualFactor, LocalDate lastTradeDate, OvernightRateComputation overnightRate, Rounding rounding)
	  {
		JodaBeanUtils.notNull(securityId, "securityId");
		JodaBeanUtils.notNull(currency, "currency");
		ArgChecker.notNegativeOrZero(notional, "notional");
		ArgChecker.notNegativeOrZero(accrualFactor, "accrualFactor");
		JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		JodaBeanUtils.notNull(overnightRate, "overnightRate");
		JodaBeanUtils.notNull(rounding, "rounding");
		this.securityId = securityId;
		this.currency = currency;
		this.notional = notional;
		this.accrualFactor = accrualFactor;
		this.lastTradeDate = lastTradeDate;
		this.overnightRate = overnightRate;
		this.rounding = rounding;
	  }

	  public override ResolvedOvernightFuture.Meta metaBean()
	  {
		return ResolvedOvernightFuture.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency that the future is traded in. </summary>
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
	  /// Gets the notional amount.
	  /// <para>
	  /// This is the full notional of the deposit, such as 5 million dollars.
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual factor, defaulted from the index if not set.
	  /// <para>
	  /// This is the year fraction of the contract, typically 1/12 for a 30-day future.
	  /// The year fraction must be positive.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double AccrualFactor
	  {
		  get
		  {
			return accrualFactor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last date of trading.
	  /// <para>
	  /// This must be a valid business day on the fixing calendar of {@code index}.
	  /// The last trade date is typically the last business day of the month.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate LastTradeDate
	  {
		  get
		  {
			return lastTradeDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight rate observation.
	  /// <para>
	  /// The future is based on this index.
	  /// It will be a well known market index such as 'USD-FED-FUND'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightRateComputation OvernightRate
	  {
		  get
		  {
			return overnightRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the definition of how to round the futures price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 for a 0.2875% rate is
	  /// represented as 0.997125 which has 6 decimal places.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Rounding Rounding
	  {
		  get
		  {
			return rounding;
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
		  ResolvedOvernightFuture other = (ResolvedOvernightFuture) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(accrualFactor, other.accrualFactor) && JodaBeanUtils.equal(lastTradeDate, other.lastTradeDate) && JodaBeanUtils.equal(overnightRate, other.overnightRate) && JodaBeanUtils.equal(rounding, other.rounding);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualFactor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastTradeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(overnightRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rounding);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("ResolvedOvernightFuture{");
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("accrualFactor").Append('=').Append(accrualFactor).Append(',').Append(' ');
		buf.Append("lastTradeDate").Append('=').Append(lastTradeDate).Append(',').Append(' ');
		buf.Append("overnightRate").Append('=').Append(overnightRate).Append(',').Append(' ');
		buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedOvernightFuture}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(ResolvedOvernightFuture), typeof(SecurityId));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ResolvedOvernightFuture), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(ResolvedOvernightFuture), Double.TYPE);
			  accrualFactor_Renamed = DirectMetaProperty.ofImmutable(this, "accrualFactor", typeof(ResolvedOvernightFuture), Double.TYPE);
			  lastTradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastTradeDate", typeof(ResolvedOvernightFuture), typeof(LocalDate));
			  overnightRate_Renamed = DirectMetaProperty.ofImmutable(this, "overnightRate", typeof(ResolvedOvernightFuture), typeof(OvernightRateComputation));
			  rounding_Renamed = DirectMetaProperty.ofImmutable(this, "rounding", typeof(ResolvedOvernightFuture), typeof(Rounding));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "currency", "notional", "accrualFactor", "lastTradeDate", "overnightRate", "rounding");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualFactor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> accrualFactor_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastTradeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastTradeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code overnightRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightRateComputation> overnightRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code rounding} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Rounding> rounding_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "currency", "notional", "accrualFactor", "lastTradeDate", "overnightRate", "rounding");
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1540322338: // accrualFactor
			  return accrualFactor_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case -821605692: // overnightRate
			  return overnightRate_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedOvernightFuture.Builder builder()
		{
		  return new ResolvedOvernightFuture.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedOvernightFuture);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualFactor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> accrualFactor()
		{
		  return accrualFactor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastTradeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastTradeDate()
		{
		  return lastTradeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code overnightRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightRateComputation> overnightRate()
		{
		  return overnightRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rounding} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Rounding> rounding()
		{
		  return rounding_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((ResolvedOvernightFuture) bean).SecurityId;
			case 575402001: // currency
			  return ((ResolvedOvernightFuture) bean).Currency;
			case 1585636160: // notional
			  return ((ResolvedOvernightFuture) bean).Notional;
			case -1540322338: // accrualFactor
			  return ((ResolvedOvernightFuture) bean).AccrualFactor;
			case -1041950404: // lastTradeDate
			  return ((ResolvedOvernightFuture) bean).LastTradeDate;
			case -821605692: // overnightRate
			  return ((ResolvedOvernightFuture) bean).OvernightRate;
			case -142444: // rounding
			  return ((ResolvedOvernightFuture) bean).Rounding;
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
	  /// The bean-builder for {@code ResolvedOvernightFuture}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedOvernightFuture>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double accrualFactor_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastTradeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightRateComputation overnightRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Rounding rounding_Renamed;

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
		internal Builder(ResolvedOvernightFuture beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.accrualFactor_Renamed = beanToCopy.AccrualFactor;
		  this.lastTradeDate_Renamed = beanToCopy.LastTradeDate;
		  this.overnightRate_Renamed = beanToCopy.OvernightRate;
		  this.rounding_Renamed = beanToCopy.Rounding;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1540322338: // accrualFactor
			  return accrualFactor_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case -821605692: // overnightRate
			  return overnightRate_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -1540322338: // accrualFactor
			  this.accrualFactor_Renamed = (double?) newValue.Value;
			  break;
			case -1041950404: // lastTradeDate
			  this.lastTradeDate_Renamed = (LocalDate) newValue;
			  break;
			case -821605692: // overnightRate
			  this.overnightRate_Renamed = (OvernightRateComputation) newValue;
			  break;
			case -142444: // rounding
			  this.rounding_Renamed = (Rounding) newValue;
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

		public override ResolvedOvernightFuture build()
		{
		  preBuild(this);
		  return new ResolvedOvernightFuture(securityId_Renamed, currency_Renamed, notional_Renamed, accrualFactor_Renamed, lastTradeDate_Renamed, overnightRate_Renamed, rounding_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the security identifier.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
		  return this;
		}

		/// <summary>
		/// Sets the currency that the future is traded in. </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// This is the full notional of the deposit, such as 5 million dollars.
		/// The notional expressed here must be positive.
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  ArgChecker.notNegativeOrZero(notional, "notional");
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the accrual factor, defaulted from the index if not set.
		/// <para>
		/// This is the year fraction of the contract, typically 1/12 for a 30-day future.
		/// The year fraction must be positive.
		/// </para>
		/// </summary>
		/// <param name="accrualFactor">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualFactor(double accrualFactor)
		{
		  ArgChecker.notNegativeOrZero(accrualFactor, "accrualFactor");
		  this.accrualFactor_Renamed = accrualFactor;
		  return this;
		}

		/// <summary>
		/// Sets the last date of trading.
		/// <para>
		/// This must be a valid business day on the fixing calendar of {@code index}.
		/// The last trade date is typically the last business day of the month.
		/// </para>
		/// </summary>
		/// <param name="lastTradeDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastTradeDate(LocalDate lastTradeDate)
		{
		  JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		  this.lastTradeDate_Renamed = lastTradeDate;
		  return this;
		}

		/// <summary>
		/// Sets the Overnight rate observation.
		/// <para>
		/// The future is based on this index.
		/// It will be a well known market index such as 'USD-FED-FUND'.
		/// </para>
		/// </summary>
		/// <param name="overnightRate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder overnightRate(OvernightRateComputation overnightRate)
		{
		  JodaBeanUtils.notNull(overnightRate, "overnightRate");
		  this.overnightRate_Renamed = overnightRate;
		  return this;
		}

		/// <summary>
		/// Sets the definition of how to round the futures price, defaulted to no rounding.
		/// <para>
		/// The price is represented in decimal form, not percentage form.
		/// As such, the decimal places expressed by the rounding refers to this decimal form.
		/// For example, the common market price of 99.7125 for a 0.2875% rate is
		/// represented as 0.997125 which has 6 decimal places.
		/// </para>
		/// </summary>
		/// <param name="rounding">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rounding(Rounding rounding)
		{
		  JodaBeanUtils.notNull(rounding, "rounding");
		  this.rounding_Renamed = rounding;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("ResolvedOvernightFuture.Builder{");
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualFactor").Append('=').Append(JodaBeanUtils.ToString(accrualFactor_Renamed)).Append(',').Append(' ');
		  buf.Append("lastTradeDate").Append('=').Append(JodaBeanUtils.ToString(lastTradeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("overnightRate").Append('=').Append(JodaBeanUtils.ToString(overnightRate_Renamed)).Append(',').Append(' ');
		  buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
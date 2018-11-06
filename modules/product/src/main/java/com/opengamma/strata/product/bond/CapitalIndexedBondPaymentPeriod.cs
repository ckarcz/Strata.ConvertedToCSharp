using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;


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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// A coupon or nominal payment of capital indexed bonds.
	/// <para>
	/// A single payment period within a capital indexed bond, <seealso cref="ResolvedCapitalIndexedBond"/>.
	/// Since All the cash flows of the capital indexed bond are adjusted for inflation,  
	/// both of the periodic payments and nominal payment are represented by this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CapitalIndexedBondPaymentPeriod implements BondPaymentPeriod, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CapitalIndexedBondPaymentPeriod : BondPaymentPeriod, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The notional amount, must be non-zero.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The rate of real coupon.
	  /// <para>
	  /// The real coupon is the rate before taking the inflation into account.
	  /// For example, a real coupon of c for semi-annual payments is c/2.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double realCoupon;
	  private readonly double realCoupon;
	  /// <summary>
	  /// The start date of the payment period.
	  /// <para>
	  /// This is the first date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the payment period.
	  /// <para>
	  /// This is the last date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedStartDate;
	  private readonly LocalDate unadjustedStartDate;
	  /// <summary>
	  /// The unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate unadjustedEndDate;
	  private readonly LocalDate unadjustedEndDate;
	  /// <summary>
	  /// The detachment date.
	  /// <para>
	  /// Some bonds trade ex-coupon before the coupon payment.
	  /// The coupon is paid not to the owner of the bond on the payment date but to the
	  /// owner of the bond on the detachment date.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate detachmentDate;
	  private readonly LocalDate detachmentDate;
	  /// <summary>
	  /// The rate to be computed.
	  /// <para>
	  /// The value of the period is based on this rate.
	  /// This must be an inflation rate observation, specifically <seealso cref="InflationEndInterpolatedRateComputation"/>
	  /// or <seealso cref="InflationEndMonthRateComputation"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.rate.RateComputation rateComputation;
	  private readonly RateComputation rateComputation;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CapitalIndexedBondPaymentPeriod(com.opengamma.strata.basics.currency.Currency currency, double notional, double realCoupon, java.time.LocalDate startDate, java.time.LocalDate endDate, java.time.LocalDate unadjustedStartDate, java.time.LocalDate unadjustedEndDate, java.time.LocalDate detachmentDate, com.opengamma.strata.product.rate.RateComputation rateComputation)
	  private CapitalIndexedBondPaymentPeriod(Currency currency, double notional, double realCoupon, LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, LocalDate detachmentDate, RateComputation rateComputation)
	  {
		this.currency = ArgChecker.notNull(currency, "currency");
		this.notional = ArgChecker.notZero(notional, 0d, "notional");
		this.realCoupon = ArgChecker.notNegative(realCoupon, "realCoupon");
		this.startDate = ArgChecker.notNull(startDate, "startDate");
		this.endDate = ArgChecker.notNull(endDate, "endDate");
		this.unadjustedStartDate = firstNonNull(unadjustedStartDate, startDate);
		this.unadjustedEndDate = firstNonNull(unadjustedEndDate, endDate);
		this.detachmentDate = firstNonNull(detachmentDate, endDate);
		this.rateComputation = ArgChecker.notNull(rateComputation, "rateComputation");
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		ArgChecker.inOrderNotEqual(this.unadjustedStartDate, this.unadjustedEndDate, "unadjustedStartDate", "unadjustedEndDate");
		ArgChecker.inOrderOrEqual(this.detachmentDate, this.endDate, "detachmentDate", "endDate");
		ArgChecker.isTrue(rateComputation is InflationEndInterpolatedRateComputation || rateComputation is InflationEndMonthRateComputation, "rateComputation must be inflation rate observation");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a payment period with unit real coupon and 0 ex-coupon days from this instance.
	  /// <para>
	  /// The main use of this method is to create a nominal payment from the final periodic payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="unadjustedStartDate">  the unadjusted start date </param>
	  /// <returns> the payment period </returns>
	  internal CapitalIndexedBondPaymentPeriod withUnitCoupon(LocalDate startDate, LocalDate unadjustedStartDate)
	  {
		return new CapitalIndexedBondPaymentPeriod(currency, notional, 1d, startDate, endDate, unadjustedStartDate, unadjustedEndDate, endDate, rateComputation);
	  }

	  //-------------------------------------------------------------------------
	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		rateComputation.collectIndices(builder);
	  }

	  public CapitalIndexedBondPaymentPeriod adjustPaymentDate(TemporalAdjuster adjuster)
	  {
		return this;
	  }

	  public LocalDate PaymentDate
	  {
		  get
		  {
			return EndDate;
		  }
	  }

	  /// <summary>
	  /// Checks if there is an ex-coupon period.
	  /// </summary>
	  /// <returns> true if has an ex-coupon period </returns>
	  public bool hasExCouponPeriod()
	  {
		return !detachmentDate.Equals(endDate);
	  }

	  /// <summary>
	  /// Checks if this period contains the specified date, based on unadjusted dates.
	  /// <para>
	  /// The unadjusted start and end dates are used in the comparison.
	  /// The unadjusted start date is included, the unadjusted end date is excluded.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to check </param>
	  /// <returns> true if this period contains the date </returns>
	  internal bool contains(LocalDate date)
	  {
		return !date.isBefore(unadjustedStartDate) && date.isBefore(unadjustedEndDate);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CapitalIndexedBondPaymentPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CapitalIndexedBondPaymentPeriod.Meta meta()
	  {
		return CapitalIndexedBondPaymentPeriod.Meta.INSTANCE;
	  }

	  static CapitalIndexedBondPaymentPeriod()
	  {
		MetaBean.register(CapitalIndexedBondPaymentPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CapitalIndexedBondPaymentPeriod.Builder builder()
	  {
		return new CapitalIndexedBondPaymentPeriod.Builder();
	  }

	  public override CapitalIndexedBondPaymentPeriod.Meta metaBean()
	  {
		return CapitalIndexedBondPaymentPeriod.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency of the payment period.
	  /// <para>
	  /// The amounts of the notional are usually expressed in terms of this currency,
	  /// however they can be converted from amounts in a different currency.
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
	  /// Gets the notional amount, must be non-zero.
	  /// <para>
	  /// The notional amount applicable during the period.
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
	  /// Gets the rate of real coupon.
	  /// <para>
	  /// The real coupon is the rate before taking the inflation into account.
	  /// For example, a real coupon of c for semi-annual payments is c/2.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double RealCoupon
	  {
		  get
		  {
			return realCoupon;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the payment period.
	  /// <para>
	  /// This is the first date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date of the payment period.
	  /// <para>
	  /// This is the last date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unadjusted start date.
	  /// <para>
	  /// The start date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate UnadjustedStartDate
	  {
		  get
		  {
			return unadjustedStartDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unadjusted end date.
	  /// <para>
	  /// The end date before any business day adjustment is applied.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate UnadjustedEndDate
	  {
		  get
		  {
			return unadjustedEndDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the detachment date.
	  /// <para>
	  /// Some bonds trade ex-coupon before the coupon payment.
	  /// The coupon is paid not to the owner of the bond on the payment date but to the
	  /// owner of the bond on the detachment date.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the end date if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate DetachmentDate
	  {
		  get
		  {
			return detachmentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rate to be computed.
	  /// <para>
	  /// The value of the period is based on this rate.
	  /// This must be an inflation rate observation, specifically <seealso cref="InflationEndInterpolatedRateComputation"/>
	  /// or <seealso cref="InflationEndMonthRateComputation"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RateComputation RateComputation
	  {
		  get
		  {
			return rateComputation;
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
		  CapitalIndexedBondPaymentPeriod other = (CapitalIndexedBondPaymentPeriod) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(realCoupon, other.realCoupon) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate) && JodaBeanUtils.equal(detachmentDate, other.detachmentDate) && JodaBeanUtils.equal(rateComputation, other.rateComputation);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(realCoupon);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(detachmentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateComputation);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("CapitalIndexedBondPaymentPeriod{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("realCoupon").Append('=').Append(realCoupon).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(unadjustedEndDate).Append(',').Append(' ');
		buf.Append("detachmentDate").Append('=').Append(detachmentDate).Append(',').Append(' ');
		buf.Append("rateComputation").Append('=').Append(JodaBeanUtils.ToString(rateComputation));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CapitalIndexedBondPaymentPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CapitalIndexedBondPaymentPeriod), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(CapitalIndexedBondPaymentPeriod), Double.TYPE);
			  realCoupon_Renamed = DirectMetaProperty.ofImmutable(this, "realCoupon", typeof(CapitalIndexedBondPaymentPeriod), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(CapitalIndexedBondPaymentPeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(CapitalIndexedBondPaymentPeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(CapitalIndexedBondPaymentPeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(CapitalIndexedBondPaymentPeriod), typeof(LocalDate));
			  detachmentDate_Renamed = DirectMetaProperty.ofImmutable(this, "detachmentDate", typeof(CapitalIndexedBondPaymentPeriod), typeof(LocalDate));
			  rateComputation_Renamed = DirectMetaProperty.ofImmutable(this, "rateComputation", typeof(CapitalIndexedBondPaymentPeriod), typeof(RateComputation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "notional", "realCoupon", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "detachmentDate", "rateComputation");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

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
		/// The meta-property for the {@code realCoupon} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> realCoupon_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code unadjustedStartDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> unadjustedStartDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code unadjustedEndDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> unadjustedEndDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code detachmentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> detachmentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code rateComputation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RateComputation> rateComputation_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "notional", "realCoupon", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "detachmentDate", "rateComputation");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 1842278244: // realCoupon
			  return realCoupon_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -878940481: // detachmentDate
			  return detachmentDate_Renamed;
			case 625350855: // rateComputation
			  return rateComputation_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CapitalIndexedBondPaymentPeriod.Builder builder()
		{
		  return new CapitalIndexedBondPaymentPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CapitalIndexedBondPaymentPeriod);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
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
		/// The meta-property for the {@code realCoupon} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> realCoupon()
		{
		  return realCoupon_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code unadjustedStartDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> unadjustedStartDate()
		{
		  return unadjustedStartDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code unadjustedEndDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> unadjustedEndDate()
		{
		  return unadjustedEndDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code detachmentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> detachmentDate()
		{
		  return detachmentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rateComputation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RateComputation> rateComputation()
		{
		  return rateComputation_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((CapitalIndexedBondPaymentPeriod) bean).Currency;
			case 1585636160: // notional
			  return ((CapitalIndexedBondPaymentPeriod) bean).Notional;
			case 1842278244: // realCoupon
			  return ((CapitalIndexedBondPaymentPeriod) bean).RealCoupon;
			case -2129778896: // startDate
			  return ((CapitalIndexedBondPaymentPeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((CapitalIndexedBondPaymentPeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((CapitalIndexedBondPaymentPeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((CapitalIndexedBondPaymentPeriod) bean).UnadjustedEndDate;
			case -878940481: // detachmentDate
			  return ((CapitalIndexedBondPaymentPeriod) bean).DetachmentDate;
			case 625350855: // rateComputation
			  return ((CapitalIndexedBondPaymentPeriod) bean).RateComputation;
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
	  /// The bean-builder for {@code CapitalIndexedBondPaymentPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CapitalIndexedBondPaymentPeriod>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double realCoupon_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate detachmentDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RateComputation rateComputation_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(CapitalIndexedBondPaymentPeriod beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.realCoupon_Renamed = beanToCopy.RealCoupon;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		  this.detachmentDate_Renamed = beanToCopy.DetachmentDate;
		  this.rateComputation_Renamed = beanToCopy.RateComputation;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 1842278244: // realCoupon
			  return realCoupon_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -878940481: // detachmentDate
			  return detachmentDate_Renamed;
			case 625350855: // rateComputation
			  return rateComputation_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case 1842278244: // realCoupon
			  this.realCoupon_Renamed = (double?) newValue.Value;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case 1457691881: // unadjustedStartDate
			  this.unadjustedStartDate_Renamed = (LocalDate) newValue;
			  break;
			case 31758114: // unadjustedEndDate
			  this.unadjustedEndDate_Renamed = (LocalDate) newValue;
			  break;
			case -878940481: // detachmentDate
			  this.detachmentDate_Renamed = (LocalDate) newValue;
			  break;
			case 625350855: // rateComputation
			  this.rateComputation_Renamed = (RateComputation) newValue;
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

		public override CapitalIndexedBondPaymentPeriod build()
		{
		  return new CapitalIndexedBondPaymentPeriod(currency_Renamed, notional_Renamed, realCoupon_Renamed, startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed, detachmentDate_Renamed, rateComputation_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the primary currency of the payment period.
		/// <para>
		/// The amounts of the notional are usually expressed in terms of this currency,
		/// however they can be converted from amounts in a different currency.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount, must be non-zero.
		/// <para>
		/// The notional amount applicable during the period.
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the rate of real coupon.
		/// <para>
		/// The real coupon is the rate before taking the inflation into account.
		/// For example, a real coupon of c for semi-annual payments is c/2.
		/// </para>
		/// </summary>
		/// <param name="realCoupon">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder realCoupon(double realCoupon)
		{
		  this.realCoupon_Renamed = realCoupon;
		  return this;
		}

		/// <summary>
		/// Sets the start date of the payment period.
		/// <para>
		/// This is the first date in the period.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date of the payment period.
		/// <para>
		/// This is the last date in the period.
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the unadjusted start date.
		/// <para>
		/// The start date before any business day adjustment is applied.
		/// </para>
		/// <para>
		/// When building, this will default to the start date if not specified.
		/// </para>
		/// </summary>
		/// <param name="unadjustedStartDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder unadjustedStartDate(LocalDate unadjustedStartDate)
		{
		  JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		  this.unadjustedStartDate_Renamed = unadjustedStartDate;
		  return this;
		}

		/// <summary>
		/// Sets the unadjusted end date.
		/// <para>
		/// The end date before any business day adjustment is applied.
		/// </para>
		/// <para>
		/// When building, this will default to the end date if not specified.
		/// </para>
		/// </summary>
		/// <param name="unadjustedEndDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder unadjustedEndDate(LocalDate unadjustedEndDate)
		{
		  JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		  this.unadjustedEndDate_Renamed = unadjustedEndDate;
		  return this;
		}

		/// <summary>
		/// Sets the detachment date.
		/// <para>
		/// Some bonds trade ex-coupon before the coupon payment.
		/// The coupon is paid not to the owner of the bond on the payment date but to the
		/// owner of the bond on the detachment date.
		/// </para>
		/// <para>
		/// When building, this will default to the end date if not specified.
		/// </para>
		/// </summary>
		/// <param name="detachmentDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder detachmentDate(LocalDate detachmentDate)
		{
		  JodaBeanUtils.notNull(detachmentDate, "detachmentDate");
		  this.detachmentDate_Renamed = detachmentDate;
		  return this;
		}

		/// <summary>
		/// Sets the rate to be computed.
		/// <para>
		/// The value of the period is based on this rate.
		/// This must be an inflation rate observation, specifically <seealso cref="InflationEndInterpolatedRateComputation"/>
		/// or <seealso cref="InflationEndMonthRateComputation"/>.
		/// </para>
		/// </summary>
		/// <param name="rateComputation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rateComputation(RateComputation rateComputation)
		{
		  JodaBeanUtils.notNull(rateComputation, "rateComputation");
		  this.rateComputation_Renamed = rateComputation;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("CapitalIndexedBondPaymentPeriod.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("realCoupon").Append('=').Append(JodaBeanUtils.ToString(realCoupon_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("detachmentDate").Append('=').Append(JodaBeanUtils.ToString(detachmentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("rateComputation").Append('=').Append(JodaBeanUtils.ToString(rateComputation_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
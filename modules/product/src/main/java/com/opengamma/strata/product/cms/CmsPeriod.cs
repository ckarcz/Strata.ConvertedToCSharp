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

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;

	/// <summary>
	/// A period over which a CMS coupon or CMS caplet/floorlet payoff is paid.
	/// <para>
	/// This represents a single payment period within a CMS leg.
	/// This class specifies the data necessary to calculate the value of the period.
	/// The payment period contains the unique accrual period.
	/// The value of the period is based on the observed value of {@code SwapIndex}.
	/// </para>
	/// <para>
	/// The payment is a CMS coupon, CMS caplet or CMS floorlet.
	/// The pay-offs are, for a swap index on the fixingDate of 'S' and an year fraction 'a'<br>
	/// CMS Coupon: a * S<br>
	/// CMS Caplet: a * (S-K)^+ ; K=caplet<br>
	/// CMS Floorlet: a * (K-S)^+ ; K=floorlet
	/// </para>
	/// <para>
	/// If {@code caplet} ({@code floorlet}) is not null, the payment is a caplet (floorlet).
	/// If both of {@code caplet} and {@code floorlet} are null, this class represents a CMS coupon payment.
	/// Thus at least one of the fields must be null.
	/// </para>
	/// <para>
	/// A {@code CmsPeriod} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CmsPeriod implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CmsPeriod : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The notional amount, positive if receiving, negative if paying.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The start date of the payment period.
	  /// <para>
	  /// This is the first date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the payment period.
	  /// <para>
	  /// This is the last date in the period.
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
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
	  /// The year fraction that the accrual period represents.
	  /// <para>
	  /// The value is usually calculated using a <seealso cref="DayCount"/> which may be different to that of the index.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// The fraction may be greater than 1, but not less than 0.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double yearFraction;
	  private readonly double yearFraction;
	  /// <summary>
	  /// The date that payment occurs.
	  /// <para>
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate paymentDate;
	  private readonly LocalDate paymentDate;
	  /// <summary>
	  /// The date of the index fixing.
	  /// <para>
	  /// This is an adjusted date with any business day applied.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate fixingDate;
	  private readonly LocalDate fixingDate;
	  /// <summary>
	  /// The optional caplet strike.
	  /// <para>
	  /// This defines the strike value of a caplet.
	  /// </para>
	  /// <para>
	  /// If the period is not a caplet, this field will be absent.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> caplet;
	  private readonly double? caplet;
	  /// <summary>
	  /// The optional floorlet strike.
	  /// <para>
	  /// This defines the strike value of a floorlet.
	  /// </para>
	  /// <para>
	  /// If the period is not a floorlet, this field will be absent.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> floorlet;
	  private readonly double? floorlet;
	  /// <summary>
	  /// The day count of the period.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The swap index.
	  /// <para>
	  /// The swap rate to be paid is the observed value of this index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.SwapIndex index;
	  private readonly SwapIndex index;
	  /// <summary>
	  /// The underlying swap.
	  /// <para>
	  /// The interest rate swap for which the swap rate is referred.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.ResolvedSwap underlyingSwap;
	  private readonly ResolvedSwap underlyingSwap;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CmsPeriod(com.opengamma.strata.basics.currency.Currency currency, double notional, java.time.LocalDate startDate, java.time.LocalDate endDate, java.time.LocalDate unadjustedStartDate, java.time.LocalDate unadjustedEndDate, double yearFraction, java.time.LocalDate paymentDate, java.time.LocalDate fixingDate, System.Nullable<double> caplet, System.Nullable<double> floorlet, com.opengamma.strata.basics.date.DayCount dayCount, com.opengamma.strata.product.swap.SwapIndex index, com.opengamma.strata.product.swap.ResolvedSwap underlyingSwap)
	  private CmsPeriod(Currency currency, double notional, LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, double yearFraction, LocalDate paymentDate, LocalDate fixingDate, double? caplet, double? floorlet, DayCount dayCount, SwapIndex index, ResolvedSwap underlyingSwap)
	  {

		this.index = ArgChecker.notNull(index, "index");
		this.currency = ArgChecker.notNull(currency, "currency");
		this.notional = notional;
		this.startDate = ArgChecker.notNull(startDate, "startDate");
		this.endDate = ArgChecker.notNull(endDate, "endDate");
		this.unadjustedStartDate = firstNonNull(unadjustedStartDate, startDate);
		this.unadjustedEndDate = firstNonNull(unadjustedEndDate, endDate);
		this.yearFraction = ArgChecker.notNegative(yearFraction, "yearFraction");
		this.paymentDate = ArgChecker.notNull(paymentDate, "paymentDate");
		this.fixingDate = ArgChecker.notNull(fixingDate, "fixingDate");
		this.caplet = caplet;
		this.floorlet = floorlet;
		this.dayCount = ArgChecker.notNull(dayCount, "dayCount");
		this.underlyingSwap = ArgChecker.notNull(underlyingSwap, "underlyingSwap");
		ArgChecker.inOrderNotEqual(this.startDate, this.endDate, "startDate", "endDate");
		ArgChecker.inOrderNotEqual(this.unadjustedStartDate, this.unadjustedEndDate, "unadjustedStartDate", "unadjustedEndDate");
		ArgChecker.isFalse(this.Caplet.HasValue && this.Floorlet.HasValue, "At least one of cap and floor should be null");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the type of the CMS period.
	  /// <para>
	  /// The period type is caplet, floorlet or coupon.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the CMS period type </returns>
	  public CmsPeriodType CmsPeriodType
	  {
		  get
		  {
			if (Caplet.HasValue)
			{
			  return CmsPeriodType.CAPLET;
			}
			else if (Floorlet.HasValue)
			{
			  return CmsPeriodType.FLOORLET;
			}
			else
			{
			  return CmsPeriodType.COUPON;
			}
		  }
	  }

	  /// <summary>
	  /// Obtains the strike value.
	  /// <para>
	  /// If the CMS period type is coupon, 0 is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the strike value </returns>
	  public double Strike
	  {
		  get
		  {
			CmsPeriodType type = CmsPeriodType;
			if (type.Equals(CmsPeriodType.CAPLET))
			{
			  return caplet.Value;
			}
			if (type.Equals(CmsPeriodType.FLOORLET))
			{
			  return floorlet.Value;
			}
			return 0d;
		  }
	  }

	  /// <summary>
	  /// Return the CMS coupon equivalent to the period.
	  /// <para>
	  /// For cap or floor the result is the coupon with the same dates and index but with no cap or floor strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns>  the CMS coupon </returns>
	  public CmsPeriod toCouponEquivalent()
	  {
		return this.toBuilder().floorlet(null).caplet(null).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CmsPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CmsPeriod.Meta meta()
	  {
		return CmsPeriod.Meta.INSTANCE;
	  }

	  static CmsPeriod()
	  {
		MetaBean.register(CmsPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CmsPeriod.Builder builder()
	  {
		return new CmsPeriod.Builder();
	  }

	  public override CmsPeriod.Meta metaBean()
	  {
		return CmsPeriod.Meta.INSTANCE;
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
	  /// Gets the notional amount, positive if receiving, negative if paying.
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
	  /// Gets the year fraction that the accrual period represents.
	  /// <para>
	  /// The value is usually calculated using a <seealso cref="DayCount"/> which may be different to that of the index.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// The fraction may be greater than 1, but not less than 0.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date that payment occurs.
	  /// <para>
	  /// If the schedule adjusts for business days, then this is the adjusted date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return paymentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date of the index fixing.
	  /// <para>
	  /// This is an adjusted date with any business day applied.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FixingDate
	  {
		  get
		  {
			return fixingDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional caplet strike.
	  /// <para>
	  /// This defines the strike value of a caplet.
	  /// </para>
	  /// <para>
	  /// If the period is not a caplet, this field will be absent.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? Caplet
	  {
		  get
		  {
			return caplet != null ? double?.of(caplet) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional floorlet strike.
	  /// <para>
	  /// This defines the strike value of a floorlet.
	  /// </para>
	  /// <para>
	  /// If the period is not a floorlet, this field will be absent.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? Floorlet
	  {
		  get
		  {
			return floorlet != null ? double?.of(floorlet) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count of the period. </summary>
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
	  /// Gets the swap index.
	  /// <para>
	  /// The swap rate to be paid is the observed value of this index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwapIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying swap.
	  /// <para>
	  /// The interest rate swap for which the swap rate is referred.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ResolvedSwap UnderlyingSwap
	  {
		  get
		  {
			return underlyingSwap;
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
		  CmsPeriod other = (CmsPeriod) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate) && JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(fixingDate, other.fixingDate) && JodaBeanUtils.equal(caplet, other.caplet) && JodaBeanUtils.equal(floorlet, other.floorlet) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(underlyingSwap, other.underlyingSwap);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(caplet);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floorlet);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingSwap);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(480);
		buf.Append("CmsPeriod{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(unadjustedEndDate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("fixingDate").Append('=').Append(fixingDate).Append(',').Append(' ');
		buf.Append("caplet").Append('=').Append(caplet).Append(',').Append(' ');
		buf.Append("floorlet").Append('=').Append(floorlet).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("underlyingSwap").Append('=').Append(JodaBeanUtils.ToString(underlyingSwap));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CmsPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CmsPeriod), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(CmsPeriod), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(CmsPeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(CmsPeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(CmsPeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(CmsPeriod), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(CmsPeriod), Double.TYPE);
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(CmsPeriod), typeof(LocalDate));
			  fixingDate_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDate", typeof(CmsPeriod), typeof(LocalDate));
			  caplet_Renamed = DirectMetaProperty.ofImmutable(this, "caplet", typeof(CmsPeriod), typeof(Double));
			  floorlet_Renamed = DirectMetaProperty.ofImmutable(this, "floorlet", typeof(CmsPeriod), typeof(Double));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(CmsPeriod), typeof(DayCount));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(CmsPeriod), typeof(SwapIndex));
			  underlyingSwap_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingSwap", typeof(CmsPeriod), typeof(ResolvedSwap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "yearFraction", "paymentDate", "fixingDate", "caplet", "floorlet", "dayCount", "index", "underlyingSwap");
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
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> fixingDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code caplet} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> caplet_Renamed;
		/// <summary>
		/// The meta-property for the {@code floorlet} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> floorlet_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlyingSwap} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedSwap> underlyingSwap_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "yearFraction", "paymentDate", "fixingDate", "caplet", "floorlet", "dayCount", "index", "underlyingSwap");
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
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 1255202043: // fixingDate
			  return fixingDate_Renamed;
			case -1367656183: // caplet
			  return caplet_Renamed;
			case 2022994575: // floorlet
			  return floorlet_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 1497421456: // underlyingSwap
			  return underlyingSwap_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CmsPeriod.Builder builder()
		{
		  return new CmsPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CmsPeriod);
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
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> fixingDate()
		{
		  return fixingDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code caplet} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> caplet()
		{
		  return caplet_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code floorlet} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> floorlet()
		{
		  return floorlet_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlyingSwap} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedSwap> underlyingSwap()
		{
		  return underlyingSwap_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((CmsPeriod) bean).Currency;
			case 1585636160: // notional
			  return ((CmsPeriod) bean).Notional;
			case -2129778896: // startDate
			  return ((CmsPeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((CmsPeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((CmsPeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((CmsPeriod) bean).UnadjustedEndDate;
			case -1731780257: // yearFraction
			  return ((CmsPeriod) bean).YearFraction;
			case -1540873516: // paymentDate
			  return ((CmsPeriod) bean).PaymentDate;
			case 1255202043: // fixingDate
			  return ((CmsPeriod) bean).FixingDate;
			case -1367656183: // caplet
			  return ((CmsPeriod) bean).caplet;
			case 2022994575: // floorlet
			  return ((CmsPeriod) bean).floorlet;
			case 1905311443: // dayCount
			  return ((CmsPeriod) bean).DayCount;
			case 100346066: // index
			  return ((CmsPeriod) bean).Index;
			case 1497421456: // underlyingSwap
			  return ((CmsPeriod) bean).UnderlyingSwap;
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
	  /// The bean-builder for {@code CmsPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CmsPeriod>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedStartDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate unadjustedEndDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double yearFraction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate paymentDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate fixingDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? caplet_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? floorlet_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwapIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ResolvedSwap underlyingSwap_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(CmsPeriod beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		  this.fixingDate_Renamed = beanToCopy.FixingDate;
		  this.caplet_Renamed = beanToCopy.caplet;
		  this.floorlet_Renamed = beanToCopy.floorlet;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.index_Renamed = beanToCopy.Index;
		  this.underlyingSwap_Renamed = beanToCopy.UnderlyingSwap;
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
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 1457691881: // unadjustedStartDate
			  return unadjustedStartDate_Renamed;
			case 31758114: // unadjustedEndDate
			  return unadjustedEndDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 1255202043: // fixingDate
			  return fixingDate_Renamed;
			case -1367656183: // caplet
			  return caplet_Renamed;
			case 2022994575: // floorlet
			  return floorlet_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 1497421456: // underlyingSwap
			  return underlyingSwap_Renamed;
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
			case -1731780257: // yearFraction
			  this.yearFraction_Renamed = (double?) newValue.Value;
			  break;
			case -1540873516: // paymentDate
			  this.paymentDate_Renamed = (LocalDate) newValue;
			  break;
			case 1255202043: // fixingDate
			  this.fixingDate_Renamed = (LocalDate) newValue;
			  break;
			case -1367656183: // caplet
			  this.caplet_Renamed = (double?) newValue;
			  break;
			case 2022994575: // floorlet
			  this.floorlet_Renamed = (double?) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (SwapIndex) newValue;
			  break;
			case 1497421456: // underlyingSwap
			  this.underlyingSwap_Renamed = (ResolvedSwap) newValue;
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

		public override CmsPeriod build()
		{
		  return new CmsPeriod(currency_Renamed, notional_Renamed, startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed, yearFraction_Renamed, paymentDate_Renamed, fixingDate_Renamed, caplet_Renamed, floorlet_Renamed, dayCount_Renamed, index_Renamed, underlyingSwap_Renamed);
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
		/// Sets the notional amount, positive if receiving, negative if paying.
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
		/// Sets the year fraction that the accrual period represents.
		/// <para>
		/// The value is usually calculated using a <seealso cref="DayCount"/> which may be different to that of the index.
		/// Typically the value will be close to 1 for one year and close to 0.5 for six months.
		/// The fraction may be greater than 1, but not less than 0.
		/// </para>
		/// </summary>
		/// <param name="yearFraction">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yearFraction(double yearFraction)
		{
		  ArgChecker.notNegative(yearFraction, "yearFraction");
		  this.yearFraction_Renamed = yearFraction;
		  return this;
		}

		/// <summary>
		/// Sets the date that payment occurs.
		/// <para>
		/// If the schedule adjusts for business days, then this is the adjusted date.
		/// </para>
		/// </summary>
		/// <param name="paymentDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDate(LocalDate paymentDate)
		{
		  JodaBeanUtils.notNull(paymentDate, "paymentDate");
		  this.paymentDate_Renamed = paymentDate;
		  return this;
		}

		/// <summary>
		/// Sets the date of the index fixing.
		/// <para>
		/// This is an adjusted date with any business day applied.
		/// </para>
		/// </summary>
		/// <param name="fixingDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDate(LocalDate fixingDate)
		{
		  JodaBeanUtils.notNull(fixingDate, "fixingDate");
		  this.fixingDate_Renamed = fixingDate;
		  return this;
		}

		/// <summary>
		/// Sets the optional caplet strike.
		/// <para>
		/// This defines the strike value of a caplet.
		/// </para>
		/// <para>
		/// If the period is not a caplet, this field will be absent.
		/// </para>
		/// </summary>
		/// <param name="caplet">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder caplet(double? caplet)
		{
		  this.caplet_Renamed = caplet;
		  return this;
		}

		/// <summary>
		/// Sets the optional floorlet strike.
		/// <para>
		/// This defines the strike value of a floorlet.
		/// </para>
		/// <para>
		/// If the period is not a floorlet, this field will be absent.
		/// </para>
		/// </summary>
		/// <param name="floorlet">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder floorlet(double? floorlet)
		{
		  this.floorlet_Renamed = floorlet;
		  return this;
		}

		/// <summary>
		/// Sets the day count of the period. </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the swap index.
		/// <para>
		/// The swap rate to be paid is the observed value of this index.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(SwapIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the underlying swap.
		/// <para>
		/// The interest rate swap for which the swap rate is referred.
		/// </para>
		/// </summary>
		/// <param name="underlyingSwap">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlyingSwap(ResolvedSwap underlyingSwap)
		{
		  JodaBeanUtils.notNull(underlyingSwap, "underlyingSwap");
		  this.underlyingSwap_Renamed = underlyingSwap;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(480);
		  buf.Append("CmsPeriod.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDate").Append('=').Append(JodaBeanUtils.ToString(fixingDate_Renamed)).Append(',').Append(' ');
		  buf.Append("caplet").Append('=').Append(JodaBeanUtils.ToString(caplet_Renamed)).Append(',').Append(' ');
		  buf.Append("floorlet").Append('=').Append(JodaBeanUtils.ToString(floorlet_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("underlyingSwap").Append('=').Append(JodaBeanUtils.ToString(underlyingSwap_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
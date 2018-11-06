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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;


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

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// A period over which an Ibor caplet/floorlet payoff is paid.
	/// <para>
	/// This represents a single payment period within an Ibor cap/floor leg.
	/// This class specifies the data necessary to calculate the value of the period.
	/// The payment period contains the unique accrual period.
	/// The value of the period is based on the observed value of {@code IborRateComputation}.
	/// </para>
	/// <para>
	/// The pay-offs are, for an Ibor index on the fixingDate of 'I' and an year fraction 'a'<br>
	/// Ibor caplet: a * (I-K)^+ ; K=caplet<br>
	/// Ibor floorlet: a * (K-I)^+ ; K=floorlet
	/// </para>
	/// <para>
	/// The payment is a caplet or floorlet.
	/// If {@code caplet} ({@code floorlet}) is not null, the payment is a caplet (floorlet).
	/// Thus one of the two fields must be null.
	/// </para>
	/// <para>
	/// If start date and end date of the period, and payment date are not specified, a standard caplet/floorlet is created
	/// based on the data and convention in {@code rateComputation},  i.e., the Ibor is fixed in advance and paid in arrears.
	/// </para>
	/// <para>
	/// An {@code IborCapletFloorletPeriod} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborCapletFloorletPeriod implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborCapletFloorletPeriod : ImmutableBean
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
	  /// The rate to be observed.
	  /// <para>
	  /// The value of the period is based on this Ibor rate.
	  /// For example, it might be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.rate.IborRateComputation iborRate;
	  private readonly IborRateComputation iborRate;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.iborRate_Renamed != null)
		{
		  IborIndex index = builder.iborRate_Renamed.Index;
		  if (builder.currency_Renamed == null)
		  {
			builder.currency_Renamed = index.Currency;
		  }
		}
		if (builder.paymentDate_Renamed == null)
		{
		  builder.paymentDate_Renamed = builder.endDate_Renamed;
		}
		if (builder.unadjustedStartDate_Renamed == null)
		{
		  builder.unadjustedStartDate_Renamed = builder.startDate_Renamed;
		}
		if (builder.unadjustedEndDate_Renamed == null)
		{
		  builder.unadjustedEndDate_Renamed = builder.endDate_Renamed;
		}
		ArgChecker.isFalse(builder.caplet_Renamed != null && builder.floorlet_Renamed != null, "Only caplet or floorlet must be set, not both");
		ArgChecker.isFalse(builder.caplet_Renamed == null && builder.floorlet_Renamed == null, "Either caplet or floorlet must be set");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// </summary>
	  /// <returns> the ibor index </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return iborRate.Index;
		  }
	  }

	  /// <summary>
	  /// Gets the fixing date of the index.
	  /// </summary>
	  /// <returns> the fixing date </returns>
	  public LocalDate FixingDate
	  {
		  get
		  {
			return iborRate.FixingDate;
		  }
	  }

	  /// <summary>
	  /// Gets the fixing date-time of the index.
	  /// </summary>
	  /// <returns> the fixing date-time </returns>
	  public ZonedDateTime FixingDateTime
	  {
		  get
		  {
			return iborRate.Index.calculateFixingDateTime(iborRate.FixingDate);
		  }
	  }

	  /// <summary>
	  /// Gets the strike value.
	  /// </summary>
	  /// <returns> the strike </returns>
	  public double Strike
	  {
		  get
		  {
			return firstNonNull(caplet, floorlet);
		  }
	  }

	  /// <summary>
	  /// Gets put or call.
	  /// <para>
	  /// CALL is returned for a caplet, whereas PUT is returned for a floorlet.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> put or call </returns>
	  public PutCall PutCall
	  {
		  get
		  {
			return Caplet.HasValue ? PutCall.CALL : PutCall.PUT;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapletFloorletPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborCapletFloorletPeriod.Meta meta()
	  {
		return IborCapletFloorletPeriod.Meta.INSTANCE;
	  }

	  static IborCapletFloorletPeriod()
	  {
		MetaBean.register(IborCapletFloorletPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborCapletFloorletPeriod.Builder builder()
	  {
		return new IborCapletFloorletPeriod.Builder();
	  }

	  private IborCapletFloorletPeriod(Currency currency, double notional, LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, double yearFraction, LocalDate paymentDate, double? caplet, double? floorlet, IborRateComputation iborRate)
	  {
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(unadjustedStartDate, "unadjustedStartDate");
		JodaBeanUtils.notNull(unadjustedEndDate, "unadjustedEndDate");
		ArgChecker.notNegative(yearFraction, "yearFraction");
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		JodaBeanUtils.notNull(iborRate, "iborRate");
		this.currency = currency;
		this.notional = notional;
		this.startDate = startDate;
		this.endDate = endDate;
		this.unadjustedStartDate = unadjustedStartDate;
		this.unadjustedEndDate = unadjustedEndDate;
		this.yearFraction = yearFraction;
		this.paymentDate = paymentDate;
		this.caplet = caplet;
		this.floorlet = floorlet;
		this.iborRate = iborRate;
	  }

	  public override IborCapletFloorletPeriod.Meta metaBean()
	  {
		return IborCapletFloorletPeriod.Meta.INSTANCE;
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
	  /// Gets the rate to be observed.
	  /// <para>
	  /// The value of the period is based on this Ibor rate.
	  /// For example, it might be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateComputation IborRate
	  {
		  get
		  {
			return iborRate;
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
		  IborCapletFloorletPeriod other = (IborCapletFloorletPeriod) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate) && JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(caplet, other.caplet) && JodaBeanUtils.equal(floorlet, other.floorlet) && JodaBeanUtils.equal(iborRate, other.iborRate);
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
		hash = hash * 31 + JodaBeanUtils.GetHashCode(caplet);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floorlet);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(iborRate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(384);
		buf.Append("IborCapletFloorletPeriod{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(unadjustedEndDate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("caplet").Append('=').Append(caplet).Append(',').Append(' ');
		buf.Append("floorlet").Append('=').Append(floorlet).Append(',').Append(' ');
		buf.Append("iborRate").Append('=').Append(JodaBeanUtils.ToString(iborRate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapletFloorletPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborCapletFloorletPeriod), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(IborCapletFloorletPeriod), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(IborCapletFloorletPeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(IborCapletFloorletPeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(IborCapletFloorletPeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(IborCapletFloorletPeriod), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(IborCapletFloorletPeriod), Double.TYPE);
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(IborCapletFloorletPeriod), typeof(LocalDate));
			  caplet_Renamed = DirectMetaProperty.ofImmutable(this, "caplet", typeof(IborCapletFloorletPeriod), typeof(Double));
			  floorlet_Renamed = DirectMetaProperty.ofImmutable(this, "floorlet", typeof(IborCapletFloorletPeriod), typeof(Double));
			  iborRate_Renamed = DirectMetaProperty.ofImmutable(this, "iborRate", typeof(IborCapletFloorletPeriod), typeof(IborRateComputation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "yearFraction", "paymentDate", "caplet", "floorlet", "iborRate");
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
		/// The meta-property for the {@code iborRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateComputation> iborRate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "yearFraction", "paymentDate", "caplet", "floorlet", "iborRate");
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
			case -1367656183: // caplet
			  return caplet_Renamed;
			case 2022994575: // floorlet
			  return floorlet_Renamed;
			case -1621804100: // iborRate
			  return iborRate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborCapletFloorletPeriod.Builder builder()
		{
		  return new IborCapletFloorletPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborCapletFloorletPeriod);
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
		/// The meta-property for the {@code iborRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateComputation> iborRate()
		{
		  return iborRate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((IborCapletFloorletPeriod) bean).Currency;
			case 1585636160: // notional
			  return ((IborCapletFloorletPeriod) bean).Notional;
			case -2129778896: // startDate
			  return ((IborCapletFloorletPeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((IborCapletFloorletPeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((IborCapletFloorletPeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((IborCapletFloorletPeriod) bean).UnadjustedEndDate;
			case -1731780257: // yearFraction
			  return ((IborCapletFloorletPeriod) bean).YearFraction;
			case -1540873516: // paymentDate
			  return ((IborCapletFloorletPeriod) bean).PaymentDate;
			case -1367656183: // caplet
			  return ((IborCapletFloorletPeriod) bean).caplet;
			case 2022994575: // floorlet
			  return ((IborCapletFloorletPeriod) bean).floorlet;
			case -1621804100: // iborRate
			  return ((IborCapletFloorletPeriod) bean).IborRate;
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
	  /// The bean-builder for {@code IborCapletFloorletPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborCapletFloorletPeriod>
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
		internal double? caplet_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? floorlet_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateComputation iborRate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(IborCapletFloorletPeriod beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		  this.caplet_Renamed = beanToCopy.caplet;
		  this.floorlet_Renamed = beanToCopy.floorlet;
		  this.iborRate_Renamed = beanToCopy.IborRate;
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
			case -1367656183: // caplet
			  return caplet_Renamed;
			case 2022994575: // floorlet
			  return floorlet_Renamed;
			case -1621804100: // iborRate
			  return iborRate_Renamed;
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
			case -1367656183: // caplet
			  this.caplet_Renamed = (double?) newValue;
			  break;
			case 2022994575: // floorlet
			  this.floorlet_Renamed = (double?) newValue;
			  break;
			case -1621804100: // iborRate
			  this.iborRate_Renamed = (IborRateComputation) newValue;
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

		public override IborCapletFloorletPeriod build()
		{
		  preBuild(this);
		  return new IborCapletFloorletPeriod(currency_Renamed, notional_Renamed, startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed, yearFraction_Renamed, paymentDate_Renamed, caplet_Renamed, floorlet_Renamed, iborRate_Renamed);
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
		/// Sets the rate to be observed.
		/// <para>
		/// The value of the period is based on this Ibor rate.
		/// For example, it might be a well known market index such as 'GBP-LIBOR-3M'.
		/// </para>
		/// </summary>
		/// <param name="iborRate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder iborRate(IborRateComputation iborRate)
		{
		  JodaBeanUtils.notNull(iborRate, "iborRate");
		  this.iborRate_Renamed = iborRate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(384);
		  buf.Append("IborCapletFloorletPeriod.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("caplet").Append('=').Append(JodaBeanUtils.ToString(caplet_Renamed)).Append(',').Append(' ');
		  buf.Append("floorlet").Append('=').Append(JodaBeanUtils.ToString(floorlet_Renamed)).Append(',').Append(' ');
		  buf.Append("iborRate").Append('=').Append(JodaBeanUtils.ToString(iborRate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
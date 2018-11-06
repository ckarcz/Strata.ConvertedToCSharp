using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// A period over which a fixed or floating rate is accrued.
	/// <para>
	/// A swap leg consists of one or more periods that are the basis of accrual.
	/// This class represents one such period.
	/// </para>
	/// <para>
	/// This class specifies the data necessary to calculate the value of the period.
	/// The key property is the <seealso cref="#getRateComputation() rateComputation"/> which defines
	/// how the rate is observed.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class RateAccrualPeriod implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RateAccrualPeriod : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
		private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the accrual period.
	  /// <para>
	  /// This is the last accrual date in the period.
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
	  /// The rate to be computed.
	  /// <para>
	  /// The value of the period is based on this rate.
	  /// Different implementations of the {@code RateComputation} interface have different
	  /// approaches to computing the rate, including averaging, overnight and interpolation.
	  /// For example, it might be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.rate.RateComputation rateComputation;
	  private readonly RateComputation rateComputation;
	  /// <summary>
	  /// The gearing multiplier, defaulted to 1.
	  /// <para>
	  /// This defines the gearing, which is used to multiply the observed rate.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the observed rate is multiplied by the gearing.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// A gearing of 1 has no effect.
	  /// </para>
	  /// <para>
	  /// Gearing is also known as <i>leverage</i>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double gearing;
	  private readonly double gearing;
	  /// <summary>
	  /// The spread rate, defaulted to 0.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// This defines the spread, which is used to add an amount the observed rate.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the spread is added to the observed rate.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// A spread of 0 has no effect.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2e.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double spread;
	  private readonly double spread;
	  /// <summary>
	  /// The negative rate method, defaulted to 'AllowNegative'.
	  /// <para>
	  /// This is used when the interest rate, observed or calculated, goes negative.
	  /// </para>
	  /// <para>
	  /// When observing or calculating the rate, the value may go negative.
	  /// If it does, then this method is used to validate whether the negative rate is allowed.
	  /// It is applied after any applicable gearing or spread.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.4.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final NegativeRateMethod negativeRateMethod;
	  private readonly NegativeRateMethod negativeRateMethod;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.negativeRateMethod(NegativeRateMethod.ALLOW_NEGATIVE);
		builder.gearing(1d);
	  }

	  // could use @ImmutablePreBuild and @ImmutableValidate but faster inline
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private RateAccrualPeriod(java.time.LocalDate startDate, java.time.LocalDate endDate, java.time.LocalDate unadjustedStartDate, java.time.LocalDate unadjustedEndDate, double yearFraction, com.opengamma.strata.product.rate.RateComputation rateComputation, double gearing, double spread, NegativeRateMethod negativeRateMethod)
	  private RateAccrualPeriod(LocalDate startDate, LocalDate endDate, LocalDate unadjustedStartDate, LocalDate unadjustedEndDate, double yearFraction, RateComputation rateComputation, double gearing, double spread, NegativeRateMethod negativeRateMethod)
	  {
		this.startDate = ArgChecker.notNull(startDate, "startDate");
		this.endDate = ArgChecker.notNull(endDate, "endDate");
		this.unadjustedStartDate = firstNonNull(unadjustedStartDate, startDate);
		this.unadjustedEndDate = firstNonNull(unadjustedEndDate, endDate);
		this.yearFraction = ArgChecker.notNegative(yearFraction, "yearFraction");
		this.rateComputation = ArgChecker.notNull(rateComputation, "rateComputation");
		this.gearing = gearing;
		this.spread = spread;
		this.negativeRateMethod = ArgChecker.notNull(negativeRateMethod, "negativeRateMethod");
		// check for unadjusted must be after firstNonNull
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		ArgChecker.inOrderNotEqual(this.unadjustedStartDate, this.unadjustedEndDate, "unadjustedStartDate", "unadjustedEndDate");
	  }

	  // trusted constructor
	  internal RateAccrualPeriod(SchedulePeriod period, double yearFraction, RateComputation rateComputation) : this(period, yearFraction, rateComputation, 1d, 0d, NegativeRateMethod.ALLOW_NEGATIVE)
	  {
	  }

	  // trusted constructor
	  internal RateAccrualPeriod(SchedulePeriod period, double yearFraction, RateComputation rateComputation, double gearing, double spread, NegativeRateMethod negativeRateMethod)
	  {

		this.startDate = period.StartDate;
		this.endDate = period.EndDate;
		this.unadjustedStartDate = period.UnadjustedStartDate;
		this.unadjustedEndDate = period.UnadjustedEndDate;
		this.yearFraction = yearFraction;
		this.rateComputation = rateComputation;
		this.gearing = gearing;
		this.spread = spread;
		this.negativeRateMethod = negativeRateMethod;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder used to create an instance of the bean, based on a schedule period.
	  /// <para>
	  /// The start date and end date (adjusted and unadjusted) will be set in the builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="period">  the schedule period </param>
	  /// <returns> the builder, not null </returns>
	  public static RateAccrualPeriod.Builder builder(SchedulePeriod period)
	  {
		return builder().startDate(period.StartDate).endDate(period.EndDate).unadjustedStartDate(period.UnadjustedStartDate).unadjustedEndDate(period.UnadjustedEndDate);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RateAccrualPeriod}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RateAccrualPeriod.Meta meta()
	  {
		return RateAccrualPeriod.Meta.INSTANCE;
	  }

	  static RateAccrualPeriod()
	  {
		MetaBean.register(RateAccrualPeriod.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RateAccrualPeriod.Builder builder()
	  {
		return new RateAccrualPeriod.Builder();
	  }

	  public override RateAccrualPeriod.Meta metaBean()
	  {
		return RateAccrualPeriod.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the accrual period.
	  /// <para>
	  /// This is the first accrual date in the period.
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
	  /// Gets the end date of the accrual period.
	  /// <para>
	  /// This is the last accrual date in the period.
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
	  /// Gets the rate to be computed.
	  /// <para>
	  /// The value of the period is based on this rate.
	  /// Different implementations of the {@code RateComputation} interface have different
	  /// approaches to computing the rate, including averaging, overnight and interpolation.
	  /// For example, it might be a well known market index such as 'GBP-LIBOR-3M'.
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
	  /// Gets the gearing multiplier, defaulted to 1.
	  /// <para>
	  /// This defines the gearing, which is used to multiply the observed rate.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the observed rate is multiplied by the gearing.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// A gearing of 1 has no effect.
	  /// </para>
	  /// <para>
	  /// Gearing is also known as <i>leverage</i>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Gearing
	  {
		  get
		  {
			return gearing;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the spread rate, defaulted to 0.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// This defines the spread, which is used to add an amount the observed rate.
	  /// </para>
	  /// <para>
	  /// When calculating the rate, the spread is added to the observed rate.
	  /// If both gearing and spread exist, then the gearing is applied first.
	  /// A spread of 0 has no effect.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2e.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Spread
	  {
		  get
		  {
			return spread;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the negative rate method, defaulted to 'AllowNegative'.
	  /// <para>
	  /// This is used when the interest rate, observed or calculated, goes negative.
	  /// </para>
	  /// <para>
	  /// When observing or calculating the rate, the value may go negative.
	  /// If it does, then this method is used to validate whether the negative rate is allowed.
	  /// It is applied after any applicable gearing or spread.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.4.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NegativeRateMethod NegativeRateMethod
	  {
		  get
		  {
			return negativeRateMethod;
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
		  RateAccrualPeriod other = (RateAccrualPeriod) obj;
		  return JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(unadjustedStartDate, other.unadjustedStartDate) && JodaBeanUtils.equal(unadjustedEndDate, other.unadjustedEndDate) && JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(rateComputation, other.rateComputation) && JodaBeanUtils.equal(gearing, other.gearing) && JodaBeanUtils.equal(spread, other.spread) && JodaBeanUtils.equal(negativeRateMethod, other.negativeRateMethod);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedStartDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(unadjustedEndDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateComputation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(gearing);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spread);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(negativeRateMethod);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("RateAccrualPeriod{");
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("unadjustedStartDate").Append('=').Append(unadjustedStartDate).Append(',').Append(' ');
		buf.Append("unadjustedEndDate").Append('=').Append(unadjustedEndDate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("rateComputation").Append('=').Append(rateComputation).Append(',').Append(' ');
		buf.Append("gearing").Append('=').Append(gearing).Append(',').Append(' ');
		buf.Append("spread").Append('=').Append(spread).Append(',').Append(' ');
		buf.Append("negativeRateMethod").Append('=').Append(JodaBeanUtils.ToString(negativeRateMethod));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RateAccrualPeriod}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(RateAccrualPeriod), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(RateAccrualPeriod), typeof(LocalDate));
			  unadjustedStartDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedStartDate", typeof(RateAccrualPeriod), typeof(LocalDate));
			  unadjustedEndDate_Renamed = DirectMetaProperty.ofImmutable(this, "unadjustedEndDate", typeof(RateAccrualPeriod), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(RateAccrualPeriod), Double.TYPE);
			  rateComputation_Renamed = DirectMetaProperty.ofImmutable(this, "rateComputation", typeof(RateAccrualPeriod), typeof(RateComputation));
			  gearing_Renamed = DirectMetaProperty.ofImmutable(this, "gearing", typeof(RateAccrualPeriod), Double.TYPE);
			  spread_Renamed = DirectMetaProperty.ofImmutable(this, "spread", typeof(RateAccrualPeriod), Double.TYPE);
			  negativeRateMethod_Renamed = DirectMetaProperty.ofImmutable(this, "negativeRateMethod", typeof(RateAccrualPeriod), typeof(NegativeRateMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "yearFraction", "rateComputation", "gearing", "spread", "negativeRateMethod");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

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
		/// The meta-property for the {@code rateComputation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RateComputation> rateComputation_Renamed;
		/// <summary>
		/// The meta-property for the {@code gearing} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> gearing_Renamed;
		/// <summary>
		/// The meta-property for the {@code spread} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> spread_Renamed;
		/// <summary>
		/// The meta-property for the {@code negativeRateMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<NegativeRateMethod> negativeRateMethod_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "startDate", "endDate", "unadjustedStartDate", "unadjustedEndDate", "yearFraction", "rateComputation", "gearing", "spread", "negativeRateMethod");
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
			case 625350855: // rateComputation
			  return rateComputation_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
			case -895684237: // spread
			  return spread_Renamed;
			case 1969081334: // negativeRateMethod
			  return negativeRateMethod_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RateAccrualPeriod.Builder builder()
		{
		  return new RateAccrualPeriod.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RateAccrualPeriod);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
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
		/// The meta-property for the {@code rateComputation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RateComputation> rateComputation()
		{
		  return rateComputation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code gearing} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> gearing()
		{
		  return gearing_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spread} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> spread()
		{
		  return spread_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code negativeRateMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<NegativeRateMethod> negativeRateMethod()
		{
		  return negativeRateMethod_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -2129778896: // startDate
			  return ((RateAccrualPeriod) bean).StartDate;
			case -1607727319: // endDate
			  return ((RateAccrualPeriod) bean).EndDate;
			case 1457691881: // unadjustedStartDate
			  return ((RateAccrualPeriod) bean).UnadjustedStartDate;
			case 31758114: // unadjustedEndDate
			  return ((RateAccrualPeriod) bean).UnadjustedEndDate;
			case -1731780257: // yearFraction
			  return ((RateAccrualPeriod) bean).YearFraction;
			case 625350855: // rateComputation
			  return ((RateAccrualPeriod) bean).RateComputation;
			case -91774989: // gearing
			  return ((RateAccrualPeriod) bean).Gearing;
			case -895684237: // spread
			  return ((RateAccrualPeriod) bean).Spread;
			case 1969081334: // negativeRateMethod
			  return ((RateAccrualPeriod) bean).NegativeRateMethod;
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
	  /// The bean-builder for {@code RateAccrualPeriod}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RateAccrualPeriod>
	  {

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
		internal RateComputation rateComputation_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double gearing_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double spread_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal NegativeRateMethod negativeRateMethod_Renamed;

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
		internal Builder(RateAccrualPeriod beanToCopy)
		{
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.unadjustedStartDate_Renamed = beanToCopy.UnadjustedStartDate;
		  this.unadjustedEndDate_Renamed = beanToCopy.UnadjustedEndDate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		  this.rateComputation_Renamed = beanToCopy.RateComputation;
		  this.gearing_Renamed = beanToCopy.Gearing;
		  this.spread_Renamed = beanToCopy.Spread;
		  this.negativeRateMethod_Renamed = beanToCopy.NegativeRateMethod;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
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
			case 625350855: // rateComputation
			  return rateComputation_Renamed;
			case -91774989: // gearing
			  return gearing_Renamed;
			case -895684237: // spread
			  return spread_Renamed;
			case 1969081334: // negativeRateMethod
			  return negativeRateMethod_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
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
			case 625350855: // rateComputation
			  this.rateComputation_Renamed = (RateComputation) newValue;
			  break;
			case -91774989: // gearing
			  this.gearing_Renamed = (double?) newValue.Value;
			  break;
			case -895684237: // spread
			  this.spread_Renamed = (double?) newValue.Value;
			  break;
			case 1969081334: // negativeRateMethod
			  this.negativeRateMethod_Renamed = (NegativeRateMethod) newValue;
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

		public override RateAccrualPeriod build()
		{
		  return new RateAccrualPeriod(startDate_Renamed, endDate_Renamed, unadjustedStartDate_Renamed, unadjustedEndDate_Renamed, yearFraction_Renamed, rateComputation_Renamed, gearing_Renamed, spread_Renamed, negativeRateMethod_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the start date of the accrual period.
		/// <para>
		/// This is the first accrual date in the period.
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
		/// Sets the end date of the accrual period.
		/// <para>
		/// This is the last accrual date in the period.
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
		/// Sets the rate to be computed.
		/// <para>
		/// The value of the period is based on this rate.
		/// Different implementations of the {@code RateComputation} interface have different
		/// approaches to computing the rate, including averaging, overnight and interpolation.
		/// For example, it might be a well known market index such as 'GBP-LIBOR-3M'.
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

		/// <summary>
		/// Sets the gearing multiplier, defaulted to 1.
		/// <para>
		/// This defines the gearing, which is used to multiply the observed rate.
		/// </para>
		/// <para>
		/// When calculating the rate, the observed rate is multiplied by the gearing.
		/// If both gearing and spread exist, then the gearing is applied first.
		/// A gearing of 1 has no effect.
		/// </para>
		/// <para>
		/// Gearing is also known as <i>leverage</i>.
		/// </para>
		/// </summary>
		/// <param name="gearing">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder gearing(double gearing)
		{
		  this.gearing_Renamed = gearing;
		  return this;
		}

		/// <summary>
		/// Sets the spread rate, defaulted to 0.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// This defines the spread, which is used to add an amount the observed rate.
		/// </para>
		/// <para>
		/// When calculating the rate, the spread is added to the observed rate.
		/// If both gearing and spread exist, then the gearing is applied first.
		/// A spread of 0 has no effect.
		/// </para>
		/// <para>
		/// Defined by the 2006 ISDA definitions article 6.2e.
		/// </para>
		/// </summary>
		/// <param name="spread">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spread(double spread)
		{
		  this.spread_Renamed = spread;
		  return this;
		}

		/// <summary>
		/// Sets the negative rate method, defaulted to 'AllowNegative'.
		/// <para>
		/// This is used when the interest rate, observed or calculated, goes negative.
		/// </para>
		/// <para>
		/// When observing or calculating the rate, the value may go negative.
		/// If it does, then this method is used to validate whether the negative rate is allowed.
		/// It is applied after any applicable gearing or spread.
		/// </para>
		/// <para>
		/// Defined by the 2006 ISDA definitions article 6.4.
		/// </para>
		/// </summary>
		/// <param name="negativeRateMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder negativeRateMethod(NegativeRateMethod negativeRateMethod)
		{
		  JodaBeanUtils.notNull(negativeRateMethod, "negativeRateMethod");
		  this.negativeRateMethod_Renamed = negativeRateMethod;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("RateAccrualPeriod.Builder{");
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedStartDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedStartDate_Renamed)).Append(',').Append(' ');
		  buf.Append("unadjustedEndDate").Append('=').Append(JodaBeanUtils.ToString(unadjustedEndDate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed)).Append(',').Append(' ');
		  buf.Append("rateComputation").Append('=').Append(JodaBeanUtils.ToString(rateComputation_Renamed)).Append(',').Append(' ');
		  buf.Append("gearing").Append('=').Append(JodaBeanUtils.ToString(gearing_Renamed)).Append(',').Append(' ');
		  buf.Append("spread").Append('=').Append(JodaBeanUtils.ToString(spread_Renamed)).Append(',').Append(' ');
		  buf.Append("negativeRateMethod").Append('=').Append(JodaBeanUtils.ToString(negativeRateMethod_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
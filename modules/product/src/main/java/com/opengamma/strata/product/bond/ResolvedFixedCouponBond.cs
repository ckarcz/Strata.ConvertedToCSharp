using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.ensureOnlyOne;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCount_ScheduleInfo = com.opengamma.strata.basics.date.DayCount_ScheduleInfo;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A fixed coupon bond, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="FixedCouponBond"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedFixedCouponBond} from a {@code FixedCouponBond}
	/// using <seealso cref="FixedCouponBond#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// The list of <seealso cref="FixedCouponBondPaymentPeriod"/> represents the periodic coupon payments,
	/// whereas the nominal payment is defined by <seealso cref="Payment"/>.
	/// </para>
	/// <para>
	/// The legal entity of this fixed coupon bond is identified by <seealso cref="StandardId"/>.
	/// The enum, <seealso cref="FixedCouponBondYieldConvention"/>, specifies the yield computation convention.
	/// </para>
	/// <para>
	/// A {@code ResolvedFixedCouponBond} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bonds in the trade model, pricers and market data.
	/// For example, a price of 99.32% is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class ResolvedFixedCouponBond implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedFixedCouponBond : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// The nominal payment of the product.
	  /// <para>
	  /// The payment date of the nominal payment agrees with the final coupon payment date of the periodic payments.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment nominalPayment;
	  private readonly Payment nominalPayment;
	  /// <summary>
	  /// The periodic payments of the product.
	  /// <para>
	  /// Each payment period represents part of the life-time of the product.
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<FixedCouponBondPaymentPeriod> periodicPayments;
	  private readonly ImmutableList<FixedCouponBondPaymentPeriod> periodicPayments;
	  /// <summary>
	  /// The frequency of the bond payments.
	  /// <para>
	  /// This must match the frequency used to generate the payment schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency frequency;
	  private readonly Frequency frequency;
	  /// <summary>
	  /// The roll convention of the bond payments.
	  /// <para>
	  /// This must match the convention used to generate the payment schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.RollConvention rollConvention;
	  private readonly RollConvention rollConvention;
	  /// <summary>
	  /// The fixed coupon rate.
	  /// <para>
	  /// The periodic payments are based on this fixed coupon rate.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double fixedRate;
	  private readonly double fixedRate;
	  /// <summary>
	  /// The day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// For the fixed bond, the day count convention is used to compute accrued interest.
	  /// </para>
	  /// <para>
	  /// Note that the year fraction of a coupon payment is computed based on the unadjusted
	  /// dates in the schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// Yield convention.
	  /// <para>
	  /// The convention defines how to convert from yield to price and inversely.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FixedCouponBondYieldConvention yieldConvention;
	  private readonly FixedCouponBondYieldConvention yieldConvention;
	  /// <summary>
	  /// The legal entity identifier.
	  /// <para>
	  /// This identifier is used for the legal entity that issues the bond.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.LegalEntityId legalEntityId;
	  private readonly LegalEntityId legalEntityId;
	  /// <summary>
	  /// The number of days between valuation date and settlement date.
	  /// <para>
	  /// This is used to compute clean price.
	  /// The clean price is the relative price to be paid at the standard settlement date in exchange for the bond.
	  /// </para>
	  /// <para>
	  /// It is usually one business day for US treasuries and UK Gilts and three days for Euroland government bonds.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment settlementDateOffset;
	  private readonly DaysAdjustment settlementDateOffset;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the product.
	  /// <para>
	  /// This is the first coupon period date of the bond, often known as the effective date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return periodicPayments.get(0).StartDate;
		  }
	  }

	  /// <summary>
	  /// Gets the end date of the product.
	  /// <para>
	  /// This is the last coupon period date of the bond, often known as the maturity date.
	  /// This date has been adjusted to be a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return periodicPayments.get(periodicPayments.size() - 1).EndDate;
		  }
	  }

	  /// <summary>
	  /// The unadjusted start date.
	  /// <para>
	  /// This is the unadjusted first coupon period date of the bond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted start date </returns>
	  public LocalDate UnadjustedStartDate
	  {
		  get
		  {
			return periodicPayments.get(0).UnadjustedStartDate;
		  }
	  }

	  /// <summary>
	  /// The unadjusted end date.
	  /// <para>
	  /// This is the unadjusted last coupon period date of the bond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unadjusted end date </returns>
	  public LocalDate UnadjustedEndDate
	  {
		  get
		  {
			return periodicPayments.get(periodicPayments.size() - 1).UnadjustedEndDate;
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the product.
	  /// <para>
	  /// All payments in the bond will have this currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return nominalPayment.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the notional amount, must be positive.
	  /// <para>
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by <seealso cref="#getCurrency()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the notional amount </returns>
	  public double Notional
	  {
		  get
		  {
			return nominalPayment.Amount;
		  }
	  }

	  /// <summary>
	  /// Checks if there is an ex-coupon period.
	  /// </summary>
	  /// <returns> true if has an ex-coupon period </returns>
	  public bool hasExCouponPeriod()
	  {
		return periodicPayments.get(0).hasExCouponPeriod();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the period that contains the specified date.
	  /// <para>
	  /// The search is performed using unadjusted dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to find the period for </param>
	  /// <returns> the period, empty if not found </returns>
	  /// <exception cref="IllegalArgumentException"> if more than one period matches </exception>
	  public Optional<FixedCouponBondPaymentPeriod> findPeriod(LocalDate date)
	  {
		return periodicPayments.Where(p => p.contains(date)).Aggregate(ensureOnlyOne());
	  }

	  /// <summary>
	  /// Calculates the year fraction within the specified period.
	  /// <para>
	  /// Year fractions on bonds are calculated on unadjusted dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <returns> the year fraction </returns>
	  /// <exception cref="IllegalArgumentException"> if the dates are outside the range of the bond or start is after end </exception>
	  public double yearFraction(LocalDate startDate, LocalDate endDate)
	  {
		ArgChecker.inOrderOrEqual(UnadjustedStartDate, startDate, "bond.unadjustedStartDate", "startDate");
		ArgChecker.inOrderOrEqual(startDate, endDate, "startDate", "endDate");
		ArgChecker.inOrderOrEqual(endDate, UnadjustedEndDate, "endDate", "bond.unadjustedEndDate");
		// bond day counts often need ScheduleInfo
		DayCount_ScheduleInfo info = new DayCount_ScheduleInfoAnonymousInnerClass(this);
		return dayCount.yearFraction(startDate, endDate, info);
	  }

	  private class DayCount_ScheduleInfoAnonymousInnerClass : DayCount_ScheduleInfo
	  {
		  private readonly ResolvedFixedCouponBond outerInstance;

		  public DayCount_ScheduleInfoAnonymousInnerClass(ResolvedFixedCouponBond outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

		  public LocalDate StartDate
		  {
			  get
			  {
				return outerInstance.UnadjustedStartDate;
			  }
		  }

		  public LocalDate EndDate
		  {
			  get
			  {
				return outerInstance.UnadjustedEndDate;
			  }
		  }

		  public Frequency Frequency
		  {
			  get
			  {
				return outerInstance.frequency;
			  }
		  }

		  public LocalDate getPeriodEndDate(LocalDate date)
		  {
			// exception should not occur, because input is validated above
			return outerInstance.periodicPayments.Where(p => p.contains(date)).Select(p => p.UnadjustedEndDate).First().orElseThrow(() => new System.ArgumentException("Date is not contained in any period"));
		  }

		  public bool EndOfMonthConvention
		  {
			  get
			  {
				return outerInstance.rollConvention == RollConventions.EOM;
			  }
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFixedCouponBond}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedFixedCouponBond.Meta meta()
	  {
		return ResolvedFixedCouponBond.Meta.INSTANCE;
	  }

	  static ResolvedFixedCouponBond()
	  {
		MetaBean.register(ResolvedFixedCouponBond.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedFixedCouponBond.Builder builder()
	  {
		return new ResolvedFixedCouponBond.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="securityId">  the value of the property, not null </param>
	  /// <param name="nominalPayment">  the value of the property, not null </param>
	  /// <param name="periodicPayments">  the value of the property, not null </param>
	  /// <param name="frequency">  the value of the property, not null </param>
	  /// <param name="rollConvention">  the value of the property, not null </param>
	  /// <param name="fixedRate">  the value of the property </param>
	  /// <param name="dayCount">  the value of the property, not null </param>
	  /// <param name="yieldConvention">  the value of the property, not null </param>
	  /// <param name="legalEntityId">  the value of the property, not null </param>
	  /// <param name="settlementDateOffset">  the value of the property, not null </param>
	  internal ResolvedFixedCouponBond(SecurityId securityId, Payment nominalPayment, IList<FixedCouponBondPaymentPeriod> periodicPayments, Frequency frequency, RollConvention rollConvention, double fixedRate, DayCount dayCount, FixedCouponBondYieldConvention yieldConvention, LegalEntityId legalEntityId, DaysAdjustment settlementDateOffset)
	  {
		JodaBeanUtils.notNull(securityId, "securityId");
		JodaBeanUtils.notNull(nominalPayment, "nominalPayment");
		JodaBeanUtils.notNull(periodicPayments, "periodicPayments");
		JodaBeanUtils.notNull(frequency, "frequency");
		JodaBeanUtils.notNull(rollConvention, "rollConvention");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(yieldConvention, "yieldConvention");
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		this.securityId = securityId;
		this.nominalPayment = nominalPayment;
		this.periodicPayments = ImmutableList.copyOf(periodicPayments);
		this.frequency = frequency;
		this.rollConvention = rollConvention;
		this.fixedRate = fixedRate;
		this.dayCount = dayCount;
		this.yieldConvention = yieldConvention;
		this.legalEntityId = legalEntityId;
		this.settlementDateOffset = settlementDateOffset;
	  }

	  public override ResolvedFixedCouponBond.Meta metaBean()
	  {
		return ResolvedFixedCouponBond.Meta.INSTANCE;
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
	  /// Gets the nominal payment of the product.
	  /// <para>
	  /// The payment date of the nominal payment agrees with the final coupon payment date of the periodic payments.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Payment NominalPayment
	  {
		  get
		  {
			return nominalPayment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic payments of the product.
	  /// <para>
	  /// Each payment period represents part of the life-time of the product.
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<FixedCouponBondPaymentPeriod> PeriodicPayments
	  {
		  get
		  {
			return periodicPayments;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the frequency of the bond payments.
	  /// <para>
	  /// This must match the frequency used to generate the payment schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency Frequency
	  {
		  get
		  {
			return frequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the roll convention of the bond payments.
	  /// <para>
	  /// This must match the convention used to generate the payment schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RollConvention RollConvention
	  {
		  get
		  {
			return rollConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed coupon rate.
	  /// <para>
	  /// The periodic payments are based on this fixed coupon rate.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double FixedRate
	  {
		  get
		  {
			return fixedRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable.
	  /// <para>
	  /// The conversion from dates to a numerical value is made based on this day count.
	  /// For the fixed bond, the day count convention is used to compute accrued interest.
	  /// </para>
	  /// <para>
	  /// Note that the year fraction of a coupon payment is computed based on the unadjusted
	  /// dates in the schedule.
	  /// </para>
	  /// </summary>
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
	  /// Gets yield convention.
	  /// <para>
	  /// The convention defines how to convert from yield to price and inversely.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedCouponBondYieldConvention YieldConvention
	  {
		  get
		  {
			return yieldConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the legal entity that issues the bond.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LegalEntityId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of days between valuation date and settlement date.
	  /// <para>
	  /// This is used to compute clean price.
	  /// The clean price is the relative price to be paid at the standard settlement date in exchange for the bond.
	  /// </para>
	  /// <para>
	  /// It is usually one business day for US treasuries and UK Gilts and three days for Euroland government bonds.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment SettlementDateOffset
	  {
		  get
		  {
			return settlementDateOffset;
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
		  ResolvedFixedCouponBond other = (ResolvedFixedCouponBond) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(nominalPayment, other.nominalPayment) && JodaBeanUtils.equal(periodicPayments, other.periodicPayments) && JodaBeanUtils.equal(frequency, other.frequency) && JodaBeanUtils.equal(rollConvention, other.rollConvention) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(yieldConvention, other.yieldConvention) && JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(settlementDateOffset, other.settlementDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nominalPayment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodicPayments);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(frequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rollConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yieldConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(settlementDateOffset);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("ResolvedFixedCouponBond{");
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("nominalPayment").Append('=').Append(nominalPayment).Append(',').Append(' ');
		buf.Append("periodicPayments").Append('=').Append(periodicPayments).Append(',').Append(' ');
		buf.Append("frequency").Append('=').Append(frequency).Append(',').Append(' ');
		buf.Append("rollConvention").Append('=').Append(rollConvention).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("yieldConvention").Append('=').Append(yieldConvention).Append(',').Append(' ');
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFixedCouponBond}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(ResolvedFixedCouponBond), typeof(SecurityId));
			  nominalPayment_Renamed = DirectMetaProperty.ofImmutable(this, "nominalPayment", typeof(ResolvedFixedCouponBond), typeof(Payment));
			  periodicPayments_Renamed = DirectMetaProperty.ofImmutable(this, "periodicPayments", typeof(ResolvedFixedCouponBond), (Type) typeof(ImmutableList));
			  frequency_Renamed = DirectMetaProperty.ofImmutable(this, "frequency", typeof(ResolvedFixedCouponBond), typeof(Frequency));
			  rollConvention_Renamed = DirectMetaProperty.ofImmutable(this, "rollConvention", typeof(ResolvedFixedCouponBond), typeof(RollConvention));
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(ResolvedFixedCouponBond), Double.TYPE);
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ResolvedFixedCouponBond), typeof(DayCount));
			  yieldConvention_Renamed = DirectMetaProperty.ofImmutable(this, "yieldConvention", typeof(ResolvedFixedCouponBond), typeof(FixedCouponBondYieldConvention));
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(ResolvedFixedCouponBond), typeof(LegalEntityId));
			  settlementDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "settlementDateOffset", typeof(ResolvedFixedCouponBond), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "nominalPayment", "periodicPayments", "frequency", "rollConvention", "fixedRate", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset");
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
		/// The meta-property for the {@code nominalPayment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Payment> nominalPayment_Renamed;
		/// <summary>
		/// The meta-property for the {@code periodicPayments} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<FixedCouponBondPaymentPeriod>> periodicPayments = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "periodicPayments", ResolvedFixedCouponBond.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<FixedCouponBondPaymentPeriod>> periodicPayments_Renamed;
		/// <summary>
		/// The meta-property for the {@code frequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> frequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code rollConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RollConvention> rollConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code yieldConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedCouponBondYieldConvention> yieldConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LegalEntityId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> settlementDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "nominalPayment", "periodicPayments", "frequency", "rollConvention", "fixedRate", "dayCount", "yieldConvention", "legalEntityId", "settlementDateOffset");
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
			case -44199542: // nominalPayment
			  return nominalPayment_Renamed;
			case -367345944: // periodicPayments
			  return periodicPayments_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedFixedCouponBond.Builder builder()
		{
		  return new ResolvedFixedCouponBond.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedFixedCouponBond);
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
		/// The meta-property for the {@code nominalPayment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Payment> nominalPayment()
		{
		  return nominalPayment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code periodicPayments} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<FixedCouponBondPaymentPeriod>> periodicPayments()
		{
		  return periodicPayments_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code frequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> frequency()
		{
		  return frequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rollConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RollConvention> rollConvention()
		{
		  return rollConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yieldConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedCouponBondYieldConvention> yieldConvention()
		{
		  return yieldConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LegalEntityId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code settlementDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> settlementDateOffset()
		{
		  return settlementDateOffset_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((ResolvedFixedCouponBond) bean).SecurityId;
			case -44199542: // nominalPayment
			  return ((ResolvedFixedCouponBond) bean).NominalPayment;
			case -367345944: // periodicPayments
			  return ((ResolvedFixedCouponBond) bean).PeriodicPayments;
			case -70023844: // frequency
			  return ((ResolvedFixedCouponBond) bean).Frequency;
			case -10223666: // rollConvention
			  return ((ResolvedFixedCouponBond) bean).RollConvention;
			case 747425396: // fixedRate
			  return ((ResolvedFixedCouponBond) bean).FixedRate;
			case 1905311443: // dayCount
			  return ((ResolvedFixedCouponBond) bean).DayCount;
			case -1895216418: // yieldConvention
			  return ((ResolvedFixedCouponBond) bean).YieldConvention;
			case 866287159: // legalEntityId
			  return ((ResolvedFixedCouponBond) bean).LegalEntityId;
			case 135924714: // settlementDateOffset
			  return ((ResolvedFixedCouponBond) bean).SettlementDateOffset;
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
	  /// The bean-builder for {@code ResolvedFixedCouponBond}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedFixedCouponBond>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Payment nominalPayment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<FixedCouponBondPaymentPeriod> periodicPayments_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency frequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal RollConvention rollConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedCouponBondYieldConvention yieldConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LegalEntityId legalEntityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment settlementDateOffset_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedFixedCouponBond beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.nominalPayment_Renamed = beanToCopy.NominalPayment;
		  this.periodicPayments_Renamed = beanToCopy.PeriodicPayments;
		  this.frequency_Renamed = beanToCopy.Frequency;
		  this.rollConvention_Renamed = beanToCopy.RollConvention;
		  this.fixedRate_Renamed = beanToCopy.FixedRate;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.yieldConvention_Renamed = beanToCopy.YieldConvention;
		  this.legalEntityId_Renamed = beanToCopy.LegalEntityId;
		  this.settlementDateOffset_Renamed = beanToCopy.SettlementDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case -44199542: // nominalPayment
			  return nominalPayment_Renamed;
			case -367345944: // periodicPayments
			  return periodicPayments_Renamed;
			case -70023844: // frequency
			  return frequency_Renamed;
			case -10223666: // rollConvention
			  return rollConvention_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1895216418: // yieldConvention
			  return yieldConvention_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 135924714: // settlementDateOffset
			  return settlementDateOffset_Renamed;
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
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case -44199542: // nominalPayment
			  this.nominalPayment_Renamed = (Payment) newValue;
			  break;
			case -367345944: // periodicPayments
			  this.periodicPayments_Renamed = (IList<FixedCouponBondPaymentPeriod>) newValue;
			  break;
			case -70023844: // frequency
			  this.frequency_Renamed = (Frequency) newValue;
			  break;
			case -10223666: // rollConvention
			  this.rollConvention_Renamed = (RollConvention) newValue;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue.Value;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -1895216418: // yieldConvention
			  this.yieldConvention_Renamed = (FixedCouponBondYieldConvention) newValue;
			  break;
			case 866287159: // legalEntityId
			  this.legalEntityId_Renamed = (LegalEntityId) newValue;
			  break;
			case 135924714: // settlementDateOffset
			  this.settlementDateOffset_Renamed = (DaysAdjustment) newValue;
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

		public override ResolvedFixedCouponBond build()
		{
		  return new ResolvedFixedCouponBond(securityId_Renamed, nominalPayment_Renamed, periodicPayments_Renamed, frequency_Renamed, rollConvention_Renamed, fixedRate_Renamed, dayCount_Renamed, yieldConvention_Renamed, legalEntityId_Renamed, settlementDateOffset_Renamed);
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
		/// Sets the nominal payment of the product.
		/// <para>
		/// The payment date of the nominal payment agrees with the final coupon payment date of the periodic payments.
		/// </para>
		/// </summary>
		/// <param name="nominalPayment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nominalPayment(Payment nominalPayment)
		{
		  JodaBeanUtils.notNull(nominalPayment, "nominalPayment");
		  this.nominalPayment_Renamed = nominalPayment;
		  return this;
		}

		/// <summary>
		/// Sets the periodic payments of the product.
		/// <para>
		/// Each payment period represents part of the life-time of the product.
		/// The start date and end date of the leg are determined from the first and last period.
		/// As such, the periods should be sorted.
		/// </para>
		/// </summary>
		/// <param name="periodicPayments">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodicPayments(IList<FixedCouponBondPaymentPeriod> periodicPayments)
		{
		  JodaBeanUtils.notNull(periodicPayments, "periodicPayments");
		  this.periodicPayments_Renamed = periodicPayments;
		  return this;
		}

		/// <summary>
		/// Sets the {@code periodicPayments} property in the builder
		/// from an array of objects. </summary>
		/// <param name="periodicPayments">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder periodicPayments(params FixedCouponBondPaymentPeriod[] periodicPayments)
		{
		  return this.periodicPayments(ImmutableList.copyOf(periodicPayments));
		}

		/// <summary>
		/// Sets the frequency of the bond payments.
		/// <para>
		/// This must match the frequency used to generate the payment schedule.
		/// </para>
		/// </summary>
		/// <param name="frequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder frequency(Frequency frequency)
		{
		  JodaBeanUtils.notNull(frequency, "frequency");
		  this.frequency_Renamed = frequency;
		  return this;
		}

		/// <summary>
		/// Sets the roll convention of the bond payments.
		/// <para>
		/// This must match the convention used to generate the payment schedule.
		/// </para>
		/// </summary>
		/// <param name="rollConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rollConvention(RollConvention rollConvention)
		{
		  JodaBeanUtils.notNull(rollConvention, "rollConvention");
		  this.rollConvention_Renamed = rollConvention;
		  return this;
		}

		/// <summary>
		/// Sets the fixed coupon rate.
		/// <para>
		/// The periodic payments are based on this fixed coupon rate.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable.
		/// <para>
		/// The conversion from dates to a numerical value is made based on this day count.
		/// For the fixed bond, the day count convention is used to compute accrued interest.
		/// </para>
		/// <para>
		/// Note that the year fraction of a coupon payment is computed based on the unadjusted
		/// dates in the schedule.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets yield convention.
		/// <para>
		/// The convention defines how to convert from yield to price and inversely.
		/// </para>
		/// </summary>
		/// <param name="yieldConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yieldConvention(FixedCouponBondYieldConvention yieldConvention)
		{
		  JodaBeanUtils.notNull(yieldConvention, "yieldConvention");
		  this.yieldConvention_Renamed = yieldConvention;
		  return this;
		}

		/// <summary>
		/// Sets the legal entity identifier.
		/// <para>
		/// This identifier is used for the legal entity that issues the bond.
		/// </para>
		/// </summary>
		/// <param name="legalEntityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legalEntityId(LegalEntityId legalEntityId)
		{
		  JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		  this.legalEntityId_Renamed = legalEntityId;
		  return this;
		}

		/// <summary>
		/// Sets the number of days between valuation date and settlement date.
		/// <para>
		/// This is used to compute clean price.
		/// The clean price is the relative price to be paid at the standard settlement date in exchange for the bond.
		/// </para>
		/// <para>
		/// It is usually one business day for US treasuries and UK Gilts and three days for Euroland government bonds.
		/// </para>
		/// </summary>
		/// <param name="settlementDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder settlementDateOffset(DaysAdjustment settlementDateOffset)
		{
		  JodaBeanUtils.notNull(settlementDateOffset, "settlementDateOffset");
		  this.settlementDateOffset_Renamed = settlementDateOffset;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("ResolvedFixedCouponBond.Builder{");
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("nominalPayment").Append('=').Append(JodaBeanUtils.ToString(nominalPayment_Renamed)).Append(',').Append(' ');
		  buf.Append("periodicPayments").Append('=').Append(JodaBeanUtils.ToString(periodicPayments_Renamed)).Append(',').Append(' ');
		  buf.Append("frequency").Append('=').Append(JodaBeanUtils.ToString(frequency_Renamed)).Append(',').Append(' ');
		  buf.Append("rollConvention").Append('=').Append(JodaBeanUtils.ToString(rollConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("yieldConvention").Append('=').Append(JodaBeanUtils.ToString(yieldConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId_Renamed)).Append(',').Append(' ');
		  buf.Append("settlementDateOffset").Append('=').Append(JodaBeanUtils.ToString(settlementDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
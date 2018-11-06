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

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixingRelativeTo = com.opengamma.strata.product.swap.FixingRelativeTo;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using IborRateSwapLegConvention = com.opengamma.strata.product.swap.type.IborRateSwapLegConvention;

	/// <summary>
	/// A CMS leg of a constant maturity swap (CMS) product.
	/// <para>
	/// This defines a single CMS leg for CMS or CMS cap/floor.
	/// The CMS leg of CMS periodically pays coupons based on swap rate, which is the observed
	/// value of a <seealso cref="SwapIndex swap index"/>.
	/// A CMS cap/floor instruments are defined as a set of call/put options on successive swap
	/// rates, creating CMS caplets/floorlets.
	/// </para>
	/// <para>
	/// The periodic payments in the resolved leg are CMS coupons, CMS caplets or
	/// CMS floorlets depending on the data in this leg.
	/// The {@code capSchedule} field is used to represent strike values of individual caplets,
	/// whereas {@code floorSchedule} is used to represent strike values of individual floorlets.
	/// Thus at least one of {@code capSchedule} and {@code floorSchedule} must be empty.
	/// If both the fields are absent, the periodic payments in this leg are CMS coupons.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CmsLeg implements com.opengamma.strata.basics.Resolvable<ResolvedCmsLeg>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CmsLeg : Resolvable<ResolvedCmsLeg>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.PayReceive payReceive;
		private readonly PayReceive payReceive;
	  /// <summary>
	  /// The periodic payment schedule.
	  /// <para>
	  /// This is used to define the periodic payment periods.
	  /// These are used directly or indirectly to determine other dates in the leg.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.PeriodicSchedule paymentSchedule;
	  private readonly PeriodicSchedule paymentSchedule;
	  /// <summary>
	  /// The offset of payment from the base calculation period date.
	  /// <para>
	  /// The offset is applied to the adjusted end date of each payment period.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the payment offset of the swap convention in the swap index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset;
	  private readonly DaysAdjustment paymentDateOffset;
	  /// <summary>
	  /// The currency of the leg associated with the notional.
	  /// <para>
	  /// This is the currency of the leg and the currency that swap rate calculation is made in.
	  /// The amounts of the notional are expressed in terms of this currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The notional amount, must be non-negative.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.ValueSchedule notional;
	  private readonly ValueSchedule notional;
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
	  /// The base date that each fixing is made relative to, defaulted to 'PeriodStart'.
	  /// <para>
	  /// The fixing date is relative to either the start or end of each period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.FixingRelativeTo fixingRelativeTo;
	  private readonly FixingRelativeTo fixingRelativeTo;
	  /// <summary>
	  /// The offset of the fixing date from each adjusted reset date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing offset of the swap convention in the swap index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the day count of the swap convention in the swap index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The cap schedule, optional.
	  /// <para>
	  /// This defines the strike value of a cap as an initial value and a list of adjustments.
	  /// Thus individual caplets may have different strike values.
	  /// The cap rate is only allowed to change at payment period boundaries.
	  /// </para>
	  /// <para>
	  /// If the product is not a cap, the cap schedule will be absent.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.value.ValueSchedule capSchedule;
	  private readonly ValueSchedule capSchedule;
	  /// <summary>
	  /// The floor schedule, optional.
	  /// <para>
	  /// This defines the strike value of a floor as an initial value and a list of adjustments.
	  /// Thus individual floorlets may have different strike values.
	  /// The floor rate is only allowed to change at payment period boundaries.
	  /// </para>
	  /// <para>
	  /// If the product is not a floor, the floor schedule will be absent.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.value.ValueSchedule floorSchedule;
	  private readonly ValueSchedule floorSchedule;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.fixingRelativeTo_Renamed = FixingRelativeTo.PERIOD_START;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.index_Renamed != null)
		{
		  IborRateSwapLegConvention iborLeg = builder.index_Renamed.Template.Convention.FloatingLeg;
		  if (builder.fixingDateOffset_Renamed == null)
		  {
			builder.fixingDateOffset_Renamed = iborLeg.FixingDateOffset;
		  }
		  if (builder.dayCount_Renamed == null)
		  {
			builder.dayCount_Renamed = iborLeg.DayCount;
		  }
		  if (builder.paymentDateOffset_Renamed == null)
		  {
			builder.paymentDateOffset_Renamed = iborLeg.PaymentDateOffset;
		  }
		  if (builder.currency_Renamed == null)
		  {
			builder.currency_Renamed = iborLeg.Currency;
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CmsLeg(com.opengamma.strata.product.common.PayReceive payReceive, com.opengamma.strata.basics.schedule.PeriodicSchedule paymentSchedule, com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset, com.opengamma.strata.basics.currency.Currency currency, com.opengamma.strata.basics.value.ValueSchedule notional, com.opengamma.strata.product.swap.SwapIndex index, com.opengamma.strata.product.swap.FixingRelativeTo fixingRelativeTo, com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset, com.opengamma.strata.basics.date.DayCount dayCount, com.opengamma.strata.basics.value.ValueSchedule capSchedule, com.opengamma.strata.basics.value.ValueSchedule floorSchedule)
	  private CmsLeg(PayReceive payReceive, PeriodicSchedule paymentSchedule, DaysAdjustment paymentDateOffset, Currency currency, ValueSchedule notional, SwapIndex index, FixingRelativeTo fixingRelativeTo, DaysAdjustment fixingDateOffset, DayCount dayCount, ValueSchedule capSchedule, ValueSchedule floorSchedule)
	  {

		this.payReceive = ArgChecker.notNull(payReceive, "payReceive");
		this.paymentSchedule = ArgChecker.notNull(paymentSchedule, "paymentSchedule");
		this.paymentDateOffset = paymentDateOffset;
		this.currency = currency;
		this.notional = ArgChecker.notNull(notional, "notional");
		this.index = ArgChecker.notNull(index, "index");
		this.fixingRelativeTo = fixingRelativeTo;
		this.fixingDateOffset = fixingDateOffset;
		this.dayCount = dayCount;
		this.capSchedule = capSchedule;
		this.floorSchedule = floorSchedule;
		ArgChecker.isTrue(!this.PaymentSchedule.StubConvention.Present || this.PaymentSchedule.StubConvention.get().Equals(StubConvention.NONE), "Stub period is not allowed");
		ArgChecker.isFalse(this.CapSchedule.Present && this.FloorSchedule.Present, "At least one of cap schedule and floor schedule should be empty");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start date of the leg.
	  /// <para>
	  /// This is the first accrual date in the leg, often known as the effective date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the leg </returns>
	  public AdjustableDate StartDate
	  {
		  get
		  {
			return paymentSchedule.calculatedStartDate();
		  }
	  }

	  /// <summary>
	  /// Gets the accrual end date of the leg.
	  /// <para>
	  /// This is the last accrual date in the leg, often known as the termination date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the leg </returns>
	  public AdjustableDate EndDate
	  {
		  get
		  {
			return paymentSchedule.calculatedEndDate();
		  }
	  }

	  /// <summary>
	  /// Gets the underlying Ibor index that the leg is based on.
	  /// </summary>
	  /// <returns> the index </returns>
	  public IborIndex UnderlyingIndex
	  {
		  get
		  {
			return index.Template.Convention.FloatingLeg.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedCmsLeg resolve(ReferenceData refData)
	  {
		Schedule adjustedSchedule = paymentSchedule.createSchedule(refData);
		DoubleArray cap = CapSchedule.Present ? capSchedule.resolveValues(adjustedSchedule) : null;
		DoubleArray floor = FloorSchedule.Present ? floorSchedule.resolveValues(adjustedSchedule) : null;
		DoubleArray notionals = notional.resolveValues(adjustedSchedule);
		DateAdjuster fixingDateAdjuster = fixingDateOffset.resolve(refData);
		DateAdjuster paymentDateAdjuster = paymentDateOffset.resolve(refData);
		ImmutableList.Builder<CmsPeriod> cmsPeriodsBuild = ImmutableList.builder();
		for (int i = 0; i < adjustedSchedule.size(); i++)
		{
		  SchedulePeriod period = adjustedSchedule.getPeriod(i);
		  LocalDate fixingDate = fixingDateAdjuster.adjust((fixingRelativeTo.Equals(FixingRelativeTo.PERIOD_START)) ? period.StartDate : period.EndDate);
		  LocalDate paymentDate = paymentDateAdjuster.adjust(period.EndDate);
		  double signedNotional = payReceive.normalize(notionals.get(i));
		  cmsPeriodsBuild.add(CmsPeriod.builder().currency(currency).notional(signedNotional).startDate(period.StartDate).endDate(period.EndDate).unadjustedStartDate(period.UnadjustedStartDate).unadjustedEndDate(period.UnadjustedEndDate).yearFraction(period.yearFraction(dayCount, adjustedSchedule)).paymentDate(paymentDate).fixingDate(fixingDate).caplet(cap != null ? cap.get(i) : null).floorlet(floor != null ? floor.get(i) : null).dayCount(dayCount).index(index).underlyingSwap(createUnderlyingSwap(fixingDate, refData)).build());
		}
		return ResolvedCmsLeg.builder().cmsPeriods(cmsPeriodsBuild.build()).payReceive(payReceive).build();
	  }

	  // creates and resolves the underlying swap
	  private ResolvedSwap createUnderlyingSwap(LocalDate fixingDate, ReferenceData refData)
	  {
		FixedIborSwapConvention conv = index.Template.Convention;
		LocalDate effectiveDate = conv.calculateSpotDateFromTradeDate(fixingDate, refData);
		LocalDate maturityDate = effectiveDate.plus(index.Template.Tenor);
		Swap swap = conv.toTrade(fixingDate, effectiveDate, maturityDate, BuySell.BUY, 1d, 1d).Product;
		return swap.resolve(refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CmsLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CmsLeg.Meta meta()
	  {
		return CmsLeg.Meta.INSTANCE;
	  }

	  static CmsLeg()
	  {
		MetaBean.register(CmsLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CmsLeg.Builder builder()
	  {
		return new CmsLeg.Builder();
	  }

	  public override CmsLeg.Meta metaBean()
	  {
		return CmsLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative swap rates can result in a payment in the opposite direction
	  /// to that implied by this indicator.
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
	  /// Gets the periodic payment schedule.
	  /// <para>
	  /// This is used to define the periodic payment periods.
	  /// These are used directly or indirectly to determine other dates in the leg.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PeriodicSchedule PaymentSchedule
	  {
		  get
		  {
			return paymentSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of payment from the base calculation period date.
	  /// <para>
	  /// The offset is applied to the adjusted end date of each payment period.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the payment offset of the swap convention in the swap index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment PaymentDateOffset
	  {
		  get
		  {
			return paymentDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the leg associated with the notional.
	  /// <para>
	  /// This is the currency of the leg and the currency that swap rate calculation is made in.
	  /// The amounts of the notional are expressed in terms of this currency.
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
	  /// Gets the notional amount, must be non-negative.
	  /// <para>
	  /// The notional amount applicable during the period.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueSchedule Notional
	  {
		  get
		  {
			return notional;
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
	  /// Gets the base date that each fixing is made relative to, defaulted to 'PeriodStart'.
	  /// <para>
	  /// The fixing date is relative to either the start or end of each period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixingRelativeTo FixingRelativeTo
	  {
		  get
		  {
			return fixingRelativeTo;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the fixing date from each adjusted reset date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing offset of the swap convention in the swap index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the day count of the swap convention in the swap index if not specified.
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
	  /// Gets the cap schedule, optional.
	  /// <para>
	  /// This defines the strike value of a cap as an initial value and a list of adjustments.
	  /// Thus individual caplets may have different strike values.
	  /// The cap rate is only allowed to change at payment period boundaries.
	  /// </para>
	  /// <para>
	  /// If the product is not a cap, the cap schedule will be absent.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ValueSchedule> CapSchedule
	  {
		  get
		  {
			return Optional.ofNullable(capSchedule);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the floor schedule, optional.
	  /// <para>
	  /// This defines the strike value of a floor as an initial value and a list of adjustments.
	  /// Thus individual floorlets may have different strike values.
	  /// The floor rate is only allowed to change at payment period boundaries.
	  /// </para>
	  /// <para>
	  /// If the product is not a floor, the floor schedule will be absent.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ValueSchedule> FloorSchedule
	  {
		  get
		  {
			return Optional.ofNullable(floorSchedule);
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
		  CmsLeg other = (CmsLeg) obj;
		  return JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(paymentSchedule, other.paymentSchedule) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fixingRelativeTo, other.fixingRelativeTo) && JodaBeanUtils.equal(fixingDateOffset, other.fixingDateOffset) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(capSchedule, other.capSchedule) && JodaBeanUtils.equal(floorSchedule, other.floorSchedule);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingRelativeTo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(capSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floorSchedule);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(384);
		buf.Append("CmsLeg{");
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("paymentSchedule").Append('=').Append(paymentSchedule).Append(',').Append(' ');
		buf.Append("paymentDateOffset").Append('=').Append(paymentDateOffset).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("fixingRelativeTo").Append('=').Append(fixingRelativeTo).Append(',').Append(' ');
		buf.Append("fixingDateOffset").Append('=').Append(fixingDateOffset).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("capSchedule").Append('=').Append(capSchedule).Append(',').Append(' ');
		buf.Append("floorSchedule").Append('=').Append(JodaBeanUtils.ToString(floorSchedule));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CmsLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(CmsLeg), typeof(PayReceive));
			  paymentSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "paymentSchedule", typeof(CmsLeg), typeof(PeriodicSchedule));
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(CmsLeg), typeof(DaysAdjustment));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CmsLeg), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(CmsLeg), typeof(ValueSchedule));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(CmsLeg), typeof(SwapIndex));
			  fixingRelativeTo_Renamed = DirectMetaProperty.ofImmutable(this, "fixingRelativeTo", typeof(CmsLeg), typeof(FixingRelativeTo));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(CmsLeg), typeof(DaysAdjustment));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(CmsLeg), typeof(DayCount));
			  capSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "capSchedule", typeof(CmsLeg), typeof(ValueSchedule));
			  floorSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "floorSchedule", typeof(CmsLeg), typeof(ValueSchedule));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payReceive", "paymentSchedule", "paymentDateOffset", "currency", "notional", "index", "fixingRelativeTo", "fixingDateOffset", "dayCount", "capSchedule", "floorSchedule");
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
		/// The meta-property for the {@code paymentSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PeriodicSchedule> paymentSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> paymentDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixingRelativeTo> fixingRelativeTo_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code capSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> capSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code floorSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> floorSchedule_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payReceive", "paymentSchedule", "paymentDateOffset", "currency", "notional", "index", "fixingRelativeTo", "fixingDateOffset", "dayCount", "capSchedule", "floorSchedule");
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
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -596212599: // capSchedule
			  return capSchedule_Renamed;
			case -1562227005: // floorSchedule
			  return floorSchedule_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CmsLeg.Builder builder()
		{
		  return new CmsLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CmsLeg);
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
		/// The meta-property for the {@code paymentSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PeriodicSchedule> paymentSchedule()
		{
		  return paymentSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> paymentDateOffset()
		{
		  return paymentDateOffset_Renamed;
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
		public MetaProperty<ValueSchedule> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixingRelativeTo> fixingRelativeTo()
		{
		  return fixingRelativeTo_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code capSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> capSchedule()
		{
		  return capSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code floorSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> floorSchedule()
		{
		  return floorSchedule_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return ((CmsLeg) bean).PayReceive;
			case -1499086147: // paymentSchedule
			  return ((CmsLeg) bean).PaymentSchedule;
			case -716438393: // paymentDateOffset
			  return ((CmsLeg) bean).PaymentDateOffset;
			case 575402001: // currency
			  return ((CmsLeg) bean).Currency;
			case 1585636160: // notional
			  return ((CmsLeg) bean).Notional;
			case 100346066: // index
			  return ((CmsLeg) bean).Index;
			case 232554996: // fixingRelativeTo
			  return ((CmsLeg) bean).FixingRelativeTo;
			case 873743726: // fixingDateOffset
			  return ((CmsLeg) bean).FixingDateOffset;
			case 1905311443: // dayCount
			  return ((CmsLeg) bean).DayCount;
			case -596212599: // capSchedule
			  return ((CmsLeg) bean).capSchedule;
			case -1562227005: // floorSchedule
			  return ((CmsLeg) bean).floorSchedule;
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
	  /// The bean-builder for {@code CmsLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CmsLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PeriodicSchedule paymentSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment paymentDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwapIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixingRelativeTo fixingRelativeTo_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule capSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule floorSchedule_Renamed;

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
		internal Builder(CmsLeg beanToCopy)
		{
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.paymentSchedule_Renamed = beanToCopy.PaymentSchedule;
		  this.paymentDateOffset_Renamed = beanToCopy.PaymentDateOffset;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.index_Renamed = beanToCopy.Index;
		  this.fixingRelativeTo_Renamed = beanToCopy.FixingRelativeTo;
		  this.fixingDateOffset_Renamed = beanToCopy.FixingDateOffset;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.capSchedule_Renamed = beanToCopy.capSchedule;
		  this.floorSchedule_Renamed = beanToCopy.floorSchedule;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -596212599: // capSchedule
			  return capSchedule_Renamed;
			case -1562227005: // floorSchedule
			  return floorSchedule_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  this.payReceive_Renamed = (PayReceive) newValue;
			  break;
			case -1499086147: // paymentSchedule
			  this.paymentSchedule_Renamed = (PeriodicSchedule) newValue;
			  break;
			case -716438393: // paymentDateOffset
			  this.paymentDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (ValueSchedule) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (SwapIndex) newValue;
			  break;
			case 232554996: // fixingRelativeTo
			  this.fixingRelativeTo_Renamed = (FixingRelativeTo) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -596212599: // capSchedule
			  this.capSchedule_Renamed = (ValueSchedule) newValue;
			  break;
			case -1562227005: // floorSchedule
			  this.floorSchedule_Renamed = (ValueSchedule) newValue;
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

		public override CmsLeg build()
		{
		  preBuild(this);
		  return new CmsLeg(payReceive_Renamed, paymentSchedule_Renamed, paymentDateOffset_Renamed, currency_Renamed, notional_Renamed, index_Renamed, fixingRelativeTo_Renamed, fixingDateOffset_Renamed, dayCount_Renamed, capSchedule_Renamed, floorSchedule_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
		/// Note that negative swap rates can result in a payment in the opposite direction
		/// to that implied by this indicator.
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
		/// Sets the periodic payment schedule.
		/// <para>
		/// This is used to define the periodic payment periods.
		/// These are used directly or indirectly to determine other dates in the leg.
		/// </para>
		/// </summary>
		/// <param name="paymentSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentSchedule(PeriodicSchedule paymentSchedule)
		{
		  JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		  this.paymentSchedule_Renamed = paymentSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the offset of payment from the base calculation period date.
		/// <para>
		/// The offset is applied to the adjusted end date of each payment period.
		/// Offset can be based on calendar days or business days.
		/// </para>
		/// <para>
		/// When building, this will default to the payment offset of the swap convention in the swap index if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDateOffset(DaysAdjustment paymentDateOffset)
		{
		  JodaBeanUtils.notNull(paymentDateOffset, "paymentDateOffset");
		  this.paymentDateOffset_Renamed = paymentDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the currency of the leg associated with the notional.
		/// <para>
		/// This is the currency of the leg and the currency that swap rate calculation is made in.
		/// The amounts of the notional are expressed in terms of this currency.
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
		/// Sets the notional amount, must be non-negative.
		/// <para>
		/// The notional amount applicable during the period.
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(ValueSchedule notional)
		{
		  JodaBeanUtils.notNull(notional, "notional");
		  this.notional_Renamed = notional;
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
		/// Sets the base date that each fixing is made relative to, defaulted to 'PeriodStart'.
		/// <para>
		/// The fixing date is relative to either the start or end of each period.
		/// </para>
		/// </summary>
		/// <param name="fixingRelativeTo">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingRelativeTo(FixingRelativeTo fixingRelativeTo)
		{
		  JodaBeanUtils.notNull(fixingRelativeTo, "fixingRelativeTo");
		  this.fixingRelativeTo_Renamed = fixingRelativeTo;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the fixing date from each adjusted reset date.
		/// <para>
		/// The offset is applied to the base date specified by {@code fixingRelativeTo}.
		/// The offset is typically a negative number of business days.
		/// </para>
		/// <para>
		/// When building, this will default to the fixing offset of the swap convention in the swap index if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// </para>
		/// <para>
		/// When building, this will default to the day count of the swap convention in the swap index if not specified.
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
		/// Sets the cap schedule, optional.
		/// <para>
		/// This defines the strike value of a cap as an initial value and a list of adjustments.
		/// Thus individual caplets may have different strike values.
		/// The cap rate is only allowed to change at payment period boundaries.
		/// </para>
		/// <para>
		/// If the product is not a cap, the cap schedule will be absent.
		/// </para>
		/// </summary>
		/// <param name="capSchedule">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder capSchedule(ValueSchedule capSchedule)
		{
		  this.capSchedule_Renamed = capSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the floor schedule, optional.
		/// <para>
		/// This defines the strike value of a floor as an initial value and a list of adjustments.
		/// Thus individual floorlets may have different strike values.
		/// The floor rate is only allowed to change at payment period boundaries.
		/// </para>
		/// <para>
		/// If the product is not a floor, the floor schedule will be absent.
		/// </para>
		/// </summary>
		/// <param name="floorSchedule">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder floorSchedule(ValueSchedule floorSchedule)
		{
		  this.floorSchedule_Renamed = floorSchedule;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(384);
		  buf.Append("CmsLeg.Builder{");
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentSchedule").Append('=').Append(JodaBeanUtils.ToString(paymentSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingRelativeTo").Append('=').Append(JodaBeanUtils.ToString(fixingRelativeTo_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("capSchedule").Append('=').Append(JodaBeanUtils.ToString(capSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("floorSchedule").Append('=').Append(JodaBeanUtils.ToString(floorSchedule_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
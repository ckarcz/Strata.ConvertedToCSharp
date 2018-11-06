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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using FixingRelativeTo = com.opengamma.strata.product.swap.FixingRelativeTo;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// An Ibor cap/floor leg of a cap/floor product.
	/// <para>
	/// This defines a single cap/floor leg for an Ibor cap/floor product.
	/// The cap/floor instruments are defined as a set of call/put options on successive Ibor index rates,
	/// known as Ibor caplets/floorlets.
	/// </para>
	/// <para>
	/// The periodic payments in the resolved leg are caplets or floorlets depending on the data in this leg.
	/// The {@code capSchedule} field is used to represent strike values of individual caplets,
	/// whereas {@code floorSchedule} is used to represent strike values of individual floorlets.
	/// Either {@code capSchedule} or {@code floorSchedule} must be present, and not both.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class IborCapFloorLeg implements com.opengamma.strata.basics.Resolvable<ResolvedIborCapFloorLeg>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IborCapFloorLeg : Resolvable<ResolvedIborCapFloorLeg>, ImmutableBean
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
	  /// The offset of payment from the base calculation period date, defaulted to 'None'.
	  /// <para>
	  /// The offset is applied to the adjusted end date of each payment period.
	  /// Offset can be based on calendar days or business days.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset;
	  private readonly DaysAdjustment paymentDateOffset;
	  /// <summary>
	  /// The currency of the leg associated with the notional.
	  /// <para>
	  /// This is the currency of the leg and the currency that payoff calculation is made in.
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
	  /// The interest rate accrual calculation.
	  /// <para>
	  /// The interest rate accrual is based on Ibor index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.IborRateCalculation calculation;
	  private readonly IborRateCalculation calculation;
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
		builder.paymentDateOffset(DaysAdjustment.NONE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private IborCapFloorLeg(com.opengamma.strata.product.common.PayReceive payReceive, com.opengamma.strata.basics.schedule.PeriodicSchedule paymentSchedule, com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset, com.opengamma.strata.basics.currency.Currency currency, com.opengamma.strata.basics.value.ValueSchedule notional, com.opengamma.strata.product.swap.IborRateCalculation calculation, com.opengamma.strata.basics.value.ValueSchedule capSchedule, com.opengamma.strata.basics.value.ValueSchedule floorSchedule)
	  private IborCapFloorLeg(PayReceive payReceive, PeriodicSchedule paymentSchedule, DaysAdjustment paymentDateOffset, Currency currency, ValueSchedule notional, IborRateCalculation calculation, ValueSchedule capSchedule, ValueSchedule floorSchedule)
	  {
		this.payReceive = ArgChecker.notNull(payReceive, "payReceive");
		this.paymentSchedule = ArgChecker.notNull(paymentSchedule, "paymentSchedule");
		this.paymentDateOffset = ArgChecker.notNull(paymentDateOffset, "paymentDateOffset");
		this.currency = currency != null ? currency : calculation.Index.Currency;
		this.notional = notional;
		this.calculation = ArgChecker.notNull(calculation, "calculation");
		this.capSchedule = capSchedule;
		this.floorSchedule = floorSchedule;
		ArgChecker.isTrue(!this.PaymentSchedule.StubConvention.Present || this.PaymentSchedule.StubConvention.get().Equals(StubConvention.NONE), "Stub period is not allowed");
		ArgChecker.isFalse(this.CapSchedule.Present == this.FloorSchedule.Present, "One of cap schedule and floor schedule should be empty");
		ArgChecker.isTrue(this.Calculation.Index.Tenor.Period.Equals(this.PaymentSchedule.Frequency.Period), "Payment frequency period should be the same as index tenor period");
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
	  /// Gets the Ibor index.
	  /// <para>
	  /// The rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the Ibor index </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return calculation.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedIborCapFloorLeg resolve(ReferenceData refData)
	  {
		Schedule adjustedSchedule = paymentSchedule.createSchedule(refData);
		DoubleArray cap = CapSchedule.Present ? capSchedule.resolveValues(adjustedSchedule) : null;
		DoubleArray floor = FloorSchedule.Present ? floorSchedule.resolveValues(adjustedSchedule) : null;
		DoubleArray notionals = notional.resolveValues(adjustedSchedule);
		DateAdjuster fixingDateAdjuster = calculation.FixingDateOffset.resolve(refData);
		DateAdjuster paymentDateAdjuster = paymentDateOffset.resolve(refData);
		System.Func<LocalDate, IborIndexObservation> obsFn = calculation.Index.resolve(refData);
		ImmutableList.Builder<IborCapletFloorletPeriod> periodsBuild = ImmutableList.builder();
		for (int i = 0; i < adjustedSchedule.size(); i++)
		{
		  SchedulePeriod period = adjustedSchedule.getPeriod(i);
		  LocalDate paymentDate = paymentDateAdjuster.adjust(period.EndDate);
		  LocalDate fixingDate = fixingDateAdjuster.adjust((calculation.FixingRelativeTo.Equals(FixingRelativeTo.PERIOD_START)) ? period.StartDate : period.EndDate);
		  double signedNotional = payReceive.normalize(notionals.get(i));
		  periodsBuild.add(IborCapletFloorletPeriod.builder().unadjustedStartDate(period.UnadjustedStartDate).unadjustedEndDate(period.UnadjustedEndDate).startDate(period.StartDate).endDate(period.EndDate).iborRate(IborRateComputation.of(obsFn(fixingDate))).paymentDate(paymentDate).notional(signedNotional).currency(currency).yearFraction(period.yearFraction(calculation.DayCount, adjustedSchedule)).caplet(cap != null ? cap.get(i) : null).floorlet(floor != null ? floor.get(i) : null).build());
		}
		return ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(periodsBuild.build()).payReceive(payReceive).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapFloorLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IborCapFloorLeg.Meta meta()
	  {
		return IborCapFloorLeg.Meta.INSTANCE;
	  }

	  static IborCapFloorLeg()
	  {
		MetaBean.register(IborCapFloorLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static IborCapFloorLeg.Builder builder()
	  {
		return new IborCapFloorLeg.Builder();
	  }

	  public override IborCapFloorLeg.Meta metaBean()
	  {
		return IborCapFloorLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
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
	  /// Gets the offset of payment from the base calculation period date, defaulted to 'None'.
	  /// <para>
	  /// The offset is applied to the adjusted end date of each payment period.
	  /// Offset can be based on calendar days or business days.
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
	  /// This is the currency of the leg and the currency that payoff calculation is made in.
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
	  /// Gets the interest rate accrual calculation.
	  /// <para>
	  /// The interest rate accrual is based on Ibor index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateCalculation Calculation
	  {
		  get
		  {
			return calculation;
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
		  IborCapFloorLeg other = (IborCapFloorLeg) obj;
		  return JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(paymentSchedule, other.paymentSchedule) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(calculation, other.calculation) && JodaBeanUtils.equal(capSchedule, other.capSchedule) && JodaBeanUtils.equal(floorSchedule, other.floorSchedule);
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
		hash = hash * 31 + JodaBeanUtils.GetHashCode(calculation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(capSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floorSchedule);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("IborCapFloorLeg{");
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("paymentSchedule").Append('=').Append(paymentSchedule).Append(',').Append(' ');
		buf.Append("paymentDateOffset").Append('=').Append(paymentDateOffset).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("calculation").Append('=').Append(calculation).Append(',').Append(' ');
		buf.Append("capSchedule").Append('=').Append(capSchedule).Append(',').Append(' ');
		buf.Append("floorSchedule").Append('=').Append(JodaBeanUtils.ToString(floorSchedule));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IborCapFloorLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(IborCapFloorLeg), typeof(PayReceive));
			  paymentSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "paymentSchedule", typeof(IborCapFloorLeg), typeof(PeriodicSchedule));
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(IborCapFloorLeg), typeof(DaysAdjustment));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IborCapFloorLeg), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(IborCapFloorLeg), typeof(ValueSchedule));
			  calculation_Renamed = DirectMetaProperty.ofImmutable(this, "calculation", typeof(IborCapFloorLeg), typeof(IborRateCalculation));
			  capSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "capSchedule", typeof(IborCapFloorLeg), typeof(ValueSchedule));
			  floorSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "floorSchedule", typeof(IborCapFloorLeg), typeof(ValueSchedule));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payReceive", "paymentSchedule", "paymentDateOffset", "currency", "notional", "calculation", "capSchedule", "floorSchedule");
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
		/// The meta-property for the {@code calculation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateCalculation> calculation_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payReceive", "paymentSchedule", "paymentDateOffset", "currency", "notional", "calculation", "capSchedule", "floorSchedule");
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
			case -934682935: // calculation
			  return calculation_Renamed;
			case -596212599: // capSchedule
			  return capSchedule_Renamed;
			case -1562227005: // floorSchedule
			  return floorSchedule_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override IborCapFloorLeg.Builder builder()
		{
		  return new IborCapFloorLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IborCapFloorLeg);
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
		/// The meta-property for the {@code calculation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateCalculation> calculation()
		{
		  return calculation_Renamed;
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
			  return ((IborCapFloorLeg) bean).PayReceive;
			case -1499086147: // paymentSchedule
			  return ((IborCapFloorLeg) bean).PaymentSchedule;
			case -716438393: // paymentDateOffset
			  return ((IborCapFloorLeg) bean).PaymentDateOffset;
			case 575402001: // currency
			  return ((IborCapFloorLeg) bean).Currency;
			case 1585636160: // notional
			  return ((IborCapFloorLeg) bean).Notional;
			case -934682935: // calculation
			  return ((IborCapFloorLeg) bean).Calculation;
			case -596212599: // capSchedule
			  return ((IborCapFloorLeg) bean).capSchedule;
			case -1562227005: // floorSchedule
			  return ((IborCapFloorLeg) bean).floorSchedule;
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
	  /// The bean-builder for {@code IborCapFloorLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<IborCapFloorLeg>
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
		internal IborRateCalculation calculation_Renamed;
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
		internal Builder(IborCapFloorLeg beanToCopy)
		{
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.paymentSchedule_Renamed = beanToCopy.PaymentSchedule;
		  this.paymentDateOffset_Renamed = beanToCopy.PaymentDateOffset;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.calculation_Renamed = beanToCopy.Calculation;
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
			case -934682935: // calculation
			  return calculation_Renamed;
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
			case -934682935: // calculation
			  this.calculation_Renamed = (IborRateCalculation) newValue;
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

		public override IborCapFloorLeg build()
		{
		  return new IborCapFloorLeg(payReceive_Renamed, paymentSchedule_Renamed, paymentDateOffset_Renamed, currency_Renamed, notional_Renamed, calculation_Renamed, capSchedule_Renamed, floorSchedule_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
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
		/// Sets the offset of payment from the base calculation period date, defaulted to 'None'.
		/// <para>
		/// The offset is applied to the adjusted end date of each payment period.
		/// Offset can be based on calendar days or business days.
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
		/// This is the currency of the leg and the currency that payoff calculation is made in.
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
		/// Sets the interest rate accrual calculation.
		/// <para>
		/// The interest rate accrual is based on Ibor index.
		/// </para>
		/// </summary>
		/// <param name="calculation">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder calculation(IborRateCalculation calculation)
		{
		  JodaBeanUtils.notNull(calculation, "calculation");
		  this.calculation_Renamed = calculation;
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
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("IborCapFloorLeg.Builder{");
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentSchedule").Append('=').Append(JodaBeanUtils.ToString(paymentSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("calculation").Append('=').Append(JodaBeanUtils.ToString(calculation_Renamed)).Append(',').Append(' ');
		  buf.Append("capSchedule").Append('=').Append(JodaBeanUtils.ToString(capSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("floorSchedule").Append('=').Append(JodaBeanUtils.ToString(floorSchedule_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
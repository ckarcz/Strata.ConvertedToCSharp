using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.google.common.@base.MoreObjects.firstNonNull;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Iterables = com.google.common.collect.Iterables;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using Index = com.opengamma.strata.basics.index.Index;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using OvernightCompoundedRateComputation = com.opengamma.strata.product.rate.OvernightCompoundedRateComputation;

	/// <summary>
	/// A rate swap leg defined using payment and accrual periods.
	/// <para>
	/// This defines a single swap leg paying a rate, such as an interest rate.
	/// The rate may be fixed or floating, for examples see <seealso cref="FixedRateComputation"/>,
	/// <seealso cref="IborRateComputation"/> and <seealso cref="OvernightCompoundedRateComputation"/>.
	/// </para>
	/// <para>
	/// The swap is built up of one or more <i>payment periods</i>, each of which produces a single payment.
	/// Each payment period is made up of one or more <i>accrual periods</i>.
	/// If there is more than one accrual period in a payment period then compounding may apply.
	/// See <seealso cref="RatePaymentPeriod"/> and <seealso cref="RateAccrualPeriod"/> for more details.
	/// </para>
	/// <para>
	/// This class allows the entire structure of the payment and accrual periods to be defined.
	/// This permits weird and wonderful swaps to be created, however it is important to note
	/// that there is no ability to adjust the accrual period dates if the holiday calendar changes.
	/// Provision is provided to adjust the payment dates if the holiday calendar changes.
	/// Note however that it is intended that the dates on {@code RatePaymentPeriod} and
	/// {@code RateAccrualPeriod} are already adjusted to be valid business days.
	/// </para>
	/// <para>
	/// In general, it is recommended to use the parameterized <seealso cref="RateCalculationSwapLeg"/>
	/// instead of this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class RatePeriodSwapLeg implements SwapLeg, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RatePeriodSwapLeg : SwapLeg, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SwapLegType type;
		private readonly SwapLegType type;
	  /// <summary>
	  /// Whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative interest rates can result in a payment in the opposite
	  /// direction to that implied by this indicator.
	  /// </para>
	  /// <para>
	  /// The value of this flag should match the signs of the payment period notionals.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.common.PayReceive payReceive;
	  private readonly PayReceive payReceive;
	  /// <summary>
	  /// The payment periods that combine to form the swap leg.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// <para>
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<RatePaymentPeriod> paymentPeriods;
	  private readonly ImmutableList<RatePaymentPeriod> paymentPeriods;
	  /// <summary>
	  /// The flag indicating whether to exchange the initial notional.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred at the start of the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
	  /// </para>
	  /// <para>
	  /// This flag controls whether a notional exchange object is created when the leg is resolved.
	  /// It covers an exchange on the initial payment date of the swap leg, treated as the start date.
	  /// If there is an FX reset, then this flag is ignored, see {@code intermediateExchange}.
	  /// If there is no FX reset and the flag is true, then a <seealso cref="NotionalExchange"/> object will be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean initialExchange;
	  private readonly bool initialExchange;
	  /// <summary>
	  /// The flag indicating whether to exchange the differences in the notional during the lifetime of the swap.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred when it changes during the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
	  /// </para>
	  /// <para>
	  /// This flag controls whether a notional exchange object is created when the leg is resolved.
	  /// It covers an exchange on each intermediate payment date of the swap leg.
	  /// If set to true, the behavior depends on whether an FX reset payment period is defined.
	  /// If there is an FX reset, then an <seealso cref="FxResetNotionalExchange"/> object will be created.
	  /// If there is no FX reset, then a <seealso cref="NotionalExchange"/> object will be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean intermediateExchange;
	  private readonly bool intermediateExchange;
	  /// <summary>
	  /// The flag indicating whether to exchange the final notional.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred at the end of the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
	  /// </para>
	  /// <para>
	  /// This flag controls whether a notional exchange object is created when the leg is resolved.
	  /// It covers an exchange on the final payment date of the swap leg.
	  /// If there is an FX reset, then this flag is ignored, see {@code intermediateExchange}.
	  /// If there is no FX reset and the flag is true, then a <seealso cref="NotionalExchange"/> object will be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean finalExchange;
	  private readonly bool finalExchange;
	  /// <summary>
	  /// The additional payment events that are associated with the swap leg.
	  /// <para>
	  /// Payment events include fees.
	  /// Notional exchange may also be specified here instead of via the dedicated fields.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<SwapPaymentEvent> paymentEvents;
	  private readonly ImmutableList<SwapPaymentEvent> paymentEvents;
	  /// <summary>
	  /// The business day date adjustment to be applied to each payment date, default is to apply no adjustment.
	  /// <para>
	  /// The business day adjustment is applied to period, exchange and event payment dates.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment paymentBusinessDayAdjustment;
	  private readonly BusinessDayAdjustment paymentBusinessDayAdjustment;
	  /// <summary>
	  /// The currency of the leg.
	  /// </summary>
	  [NonSerialized]
	  private readonly Currency currency; // not a property, derived and cached from input data

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private RatePeriodSwapLeg(SwapLegType type, com.opengamma.strata.product.common.PayReceive payReceive, java.util.List<RatePaymentPeriod> paymentPeriods, boolean initialExchange, boolean intermediateExchange, boolean finalExchange, java.util.List<SwapPaymentEvent> paymentEvents, com.opengamma.strata.basics.date.BusinessDayAdjustment paymentBusinessDayAdjustment)
	  private RatePeriodSwapLeg(SwapLegType type, PayReceive payReceive, IList<RatePaymentPeriod> paymentPeriods, bool initialExchange, bool intermediateExchange, bool finalExchange, IList<SwapPaymentEvent> paymentEvents, BusinessDayAdjustment paymentBusinessDayAdjustment)
	  {

		JodaBeanUtils.notNull(type, "type");
		JodaBeanUtils.notNull(payReceive, "payReceive");
		JodaBeanUtils.notEmpty(paymentPeriods, "paymentPeriods");
		JodaBeanUtils.notNull(paymentEvents, "paymentEvents");
		this.type = type;
		this.payReceive = payReceive;
		this.paymentPeriods = ImmutableList.copyOf(paymentPeriods);
		this.initialExchange = initialExchange;
		this.intermediateExchange = intermediateExchange;
		this.finalExchange = finalExchange;
		this.paymentBusinessDayAdjustment = firstNonNull(paymentBusinessDayAdjustment, BusinessDayAdjustment.NONE);
		this.paymentEvents = ImmutableList.copyOf(paymentEvents);
		// determine and validate currency, with explicit error message
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		Stream<Currency> periodCurrencies = paymentPeriods.Select(SwapPaymentPeriod::getCurrency);
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		Stream<Currency> eventCurrencies = paymentEvents.Select(SwapPaymentEvent::getCurrency);
		ISet<Currency> currencies = Stream.concat(periodCurrencies, eventCurrencies).collect(Collectors.toSet());
		if (currencies.Count > 1)
		{
		  throw new System.ArgumentException("Swap leg must have a single currency, found: " + currencies);
		}
		this.currency = Iterables.getOnlyElement(currencies);
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new RatePeriodSwapLeg(type, payReceive, paymentPeriods, initialExchange, intermediateExchange, finalExchange, paymentEvents, paymentBusinessDayAdjustment);
	  }

	  //-------------------------------------------------------------------------
	  public AdjustableDate StartDate
	  {
		  get
		  {
			return AdjustableDate.of(paymentPeriods.get(0).StartDate);
		  }
	  }

	  public AdjustableDate EndDate
	  {
		  get
		  {
			return AdjustableDate.of(paymentPeriods.get(paymentPeriods.size() - 1).EndDate);
		  }
	  }

	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		builder.add(currency);
		foreach (RatePaymentPeriod paymentPeriod in paymentPeriods)
		{
		  builder.add(paymentPeriod.Currency);
		  paymentPeriod.FxReset.ifPresent(fxr => builder.add(fxr.ReferenceCurrency));
		}
		paymentEvents.forEach(ev => builder.add(ev.Currency));
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		paymentPeriods.ForEach(period => period.collectIndices(builder));
	  }

	  /// <summary>
	  /// Converts this swap leg to the equivalent {@code ResolvedSwapLeg}.
	  /// <para>
	  /// An <seealso cref="ResolvedSwapLeg"/> represents the same data as this leg, but with
	  /// the schedules resolved to be <seealso cref="SwapPaymentPeriod"/> instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the equivalent resolved swap leg </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  public ResolvedSwapLeg resolve(ReferenceData refData)
	  {
		DateAdjuster paymentDateAdjuster = paymentBusinessDayAdjustment.resolve(refData);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<NotionalPaymentPeriod> adjusted = paymentPeriods.Select(pp => pp.adjustPaymentDate(paymentDateAdjuster)).collect(toImmutableList());
		ImmutableList<SwapPaymentEvent> payEvents = createEvents(adjusted, paymentDateAdjuster, refData);
		return new ResolvedSwapLeg(type, payReceive, adjusted, payEvents, currency);
	  }

	  // notional exchange events
	  private ImmutableList<SwapPaymentEvent> createEvents(IList<NotionalPaymentPeriod> adjPaymentPeriods, DateAdjuster paymentDateAdjuster, ReferenceData refData)
	  {

		ImmutableList.Builder<SwapPaymentEvent> events = ImmutableList.builder();
		LocalDate initialExchangeDate = paymentDateAdjuster.adjust(adjPaymentPeriods[0].StartDate);
		events.addAll(NotionalSchedule.createEvents(adjPaymentPeriods, initialExchangeDate, initialExchange, intermediateExchange, finalExchange, refData));
		paymentEvents.forEach(pe => events.add(pe.adjustPaymentDate(paymentDateAdjuster)));
		return events.build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatePeriodSwapLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RatePeriodSwapLeg.Meta meta()
	  {
		return RatePeriodSwapLeg.Meta.INSTANCE;
	  }

	  static RatePeriodSwapLeg()
	  {
		MetaBean.register(RatePeriodSwapLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static RatePeriodSwapLeg.Builder builder()
	  {
		return new RatePeriodSwapLeg.Builder();
	  }

	  public override RatePeriodSwapLeg.Meta metaBean()
	  {
		return RatePeriodSwapLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the leg, such as Fixed or Ibor.
	  /// <para>
	  /// This provides a high level categorization of the swap leg.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwapLegType Type
	  {
		  get
		  {
			return type;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the leg is pay or receive.
	  /// <para>
	  /// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
	  /// A value of 'Receive' implies that the resulting amount is received from the counterparty.
	  /// Note that negative interest rates can result in a payment in the opposite
	  /// direction to that implied by this indicator.
	  /// </para>
	  /// <para>
	  /// The value of this flag should match the signs of the payment period notionals.
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
	  /// Gets the payment periods that combine to form the swap leg.
	  /// <para>
	  /// Each payment period represents part of the life-time of the leg.
	  /// In most cases, the periods do not overlap. However, since each payment period
	  /// is essentially independent the data model allows overlapping periods.
	  /// </para>
	  /// <para>
	  /// The start date and end date of the leg are determined from the first and last period.
	  /// As such, the periods should be sorted.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<RatePaymentPeriod> PaymentPeriods
	  {
		  get
		  {
			return paymentPeriods;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating whether to exchange the initial notional.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred at the start of the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
	  /// </para>
	  /// <para>
	  /// This flag controls whether a notional exchange object is created when the leg is resolved.
	  /// It covers an exchange on the initial payment date of the swap leg, treated as the start date.
	  /// If there is an FX reset, then this flag is ignored, see {@code intermediateExchange}.
	  /// If there is no FX reset and the flag is true, then a <seealso cref="NotionalExchange"/> object will be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool InitialExchange
	  {
		  get
		  {
			return initialExchange;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating whether to exchange the differences in the notional during the lifetime of the swap.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred when it changes during the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
	  /// </para>
	  /// <para>
	  /// This flag controls whether a notional exchange object is created when the leg is resolved.
	  /// It covers an exchange on each intermediate payment date of the swap leg.
	  /// If set to true, the behavior depends on whether an FX reset payment period is defined.
	  /// If there is an FX reset, then an <seealso cref="FxResetNotionalExchange"/> object will be created.
	  /// If there is no FX reset, then a <seealso cref="NotionalExchange"/> object will be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool IntermediateExchange
	  {
		  get
		  {
			return intermediateExchange;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the flag indicating whether to exchange the final notional.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred at the end of the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
	  /// </para>
	  /// <para>
	  /// This flag controls whether a notional exchange object is created when the leg is resolved.
	  /// It covers an exchange on the final payment date of the swap leg.
	  /// If there is an FX reset, then this flag is ignored, see {@code intermediateExchange}.
	  /// If there is no FX reset and the flag is true, then a <seealso cref="NotionalExchange"/> object will be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public bool FinalExchange
	  {
		  get
		  {
			return finalExchange;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional payment events that are associated with the swap leg.
	  /// <para>
	  /// Payment events include fees.
	  /// Notional exchange may also be specified here instead of via the dedicated fields.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<SwapPaymentEvent> PaymentEvents
	  {
		  get
		  {
			return paymentEvents;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day date adjustment to be applied to each payment date, default is to apply no adjustment.
	  /// <para>
	  /// The business day adjustment is applied to period, exchange and event payment dates.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment PaymentBusinessDayAdjustment
	  {
		  get
		  {
			return paymentBusinessDayAdjustment;
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
		  RatePeriodSwapLeg other = (RatePeriodSwapLeg) obj;
		  return JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(paymentPeriods, other.paymentPeriods) && (initialExchange == other.initialExchange) && (intermediateExchange == other.intermediateExchange) && (finalExchange == other.finalExchange) && JodaBeanUtils.equal(paymentEvents, other.paymentEvents) && JodaBeanUtils.equal(paymentBusinessDayAdjustment, other.paymentBusinessDayAdjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentPeriods);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialExchange);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(intermediateExchange);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(finalExchange);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentEvents);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentBusinessDayAdjustment);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("RatePeriodSwapLeg{");
		buf.Append("type").Append('=').Append(type).Append(',').Append(' ');
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("paymentPeriods").Append('=').Append(paymentPeriods).Append(',').Append(' ');
		buf.Append("initialExchange").Append('=').Append(initialExchange).Append(',').Append(' ');
		buf.Append("intermediateExchange").Append('=').Append(intermediateExchange).Append(',').Append(' ');
		buf.Append("finalExchange").Append('=').Append(finalExchange).Append(',').Append(' ');
		buf.Append("paymentEvents").Append('=').Append(paymentEvents).Append(',').Append(' ');
		buf.Append("paymentBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(paymentBusinessDayAdjustment));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RatePeriodSwapLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(RatePeriodSwapLeg), typeof(SwapLegType));
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(RatePeriodSwapLeg), typeof(PayReceive));
			  paymentPeriods_Renamed = DirectMetaProperty.ofImmutable(this, "paymentPeriods", typeof(RatePeriodSwapLeg), (Type) typeof(ImmutableList));
			  initialExchange_Renamed = DirectMetaProperty.ofImmutable(this, "initialExchange", typeof(RatePeriodSwapLeg), Boolean.TYPE);
			  intermediateExchange_Renamed = DirectMetaProperty.ofImmutable(this, "intermediateExchange", typeof(RatePeriodSwapLeg), Boolean.TYPE);
			  finalExchange_Renamed = DirectMetaProperty.ofImmutable(this, "finalExchange", typeof(RatePeriodSwapLeg), Boolean.TYPE);
			  paymentEvents_Renamed = DirectMetaProperty.ofImmutable(this, "paymentEvents", typeof(RatePeriodSwapLeg), (Type) typeof(ImmutableList));
			  paymentBusinessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "paymentBusinessDayAdjustment", typeof(RatePeriodSwapLeg), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "type", "payReceive", "paymentPeriods", "initialExchange", "intermediateExchange", "finalExchange", "paymentEvents", "paymentBusinessDayAdjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapLegType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code payReceive} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PayReceive> payReceive_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<RatePaymentPeriod>> paymentPeriods = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "paymentPeriods", RatePeriodSwapLeg.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<RatePaymentPeriod>> paymentPeriods_Renamed;
		/// <summary>
		/// The meta-property for the {@code initialExchange} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> initialExchange_Renamed;
		/// <summary>
		/// The meta-property for the {@code intermediateExchange} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> intermediateExchange_Renamed;
		/// <summary>
		/// The meta-property for the {@code finalExchange} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<bool> finalExchange_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentEvents} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<SwapPaymentEvent>> paymentEvents = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "paymentEvents", RatePeriodSwapLeg.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SwapPaymentEvent>> paymentEvents_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentBusinessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> paymentBusinessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "type", "payReceive", "paymentPeriods", "initialExchange", "intermediateExchange", "finalExchange", "paymentEvents", "paymentBusinessDayAdjustment");
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
			case 3575610: // type
			  return type_Renamed;
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case -1674414612: // paymentPeriods
			  return paymentPeriods_Renamed;
			case -511982201: // initialExchange
			  return initialExchange_Renamed;
			case -2147112388: // intermediateExchange
			  return intermediateExchange_Renamed;
			case -1048781383: // finalExchange
			  return finalExchange_Renamed;
			case 1031856831: // paymentEvents
			  return paymentEvents_Renamed;
			case -1420083229: // paymentBusinessDayAdjustment
			  return paymentBusinessDayAdjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override RatePeriodSwapLeg.Builder builder()
		{
		  return new RatePeriodSwapLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RatePeriodSwapLeg);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapLegType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code payReceive} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PayReceive> payReceive()
		{
		  return payReceive_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentPeriods} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<RatePaymentPeriod>> paymentPeriods()
		{
		  return paymentPeriods_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code initialExchange} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> initialExchange()
		{
		  return initialExchange_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code intermediateExchange} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> intermediateExchange()
		{
		  return intermediateExchange_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code finalExchange} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<bool> finalExchange()
		{
		  return finalExchange_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentEvents} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SwapPaymentEvent>> paymentEvents()
		{
		  return paymentEvents_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentBusinessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> paymentBusinessDayAdjustment()
		{
		  return paymentBusinessDayAdjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return ((RatePeriodSwapLeg) bean).Type;
			case -885469925: // payReceive
			  return ((RatePeriodSwapLeg) bean).PayReceive;
			case -1674414612: // paymentPeriods
			  return ((RatePeriodSwapLeg) bean).PaymentPeriods;
			case -511982201: // initialExchange
			  return ((RatePeriodSwapLeg) bean).InitialExchange;
			case -2147112388: // intermediateExchange
			  return ((RatePeriodSwapLeg) bean).IntermediateExchange;
			case -1048781383: // finalExchange
			  return ((RatePeriodSwapLeg) bean).FinalExchange;
			case 1031856831: // paymentEvents
			  return ((RatePeriodSwapLeg) bean).PaymentEvents;
			case -1420083229: // paymentBusinessDayAdjustment
			  return ((RatePeriodSwapLeg) bean).PaymentBusinessDayAdjustment;
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
	  /// The bean-builder for {@code RatePeriodSwapLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<RatePeriodSwapLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwapLegType type_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<RatePaymentPeriod> paymentPeriods_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool initialExchange_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool intermediateExchange_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool finalExchange_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<SwapPaymentEvent> paymentEvents_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment paymentBusinessDayAdjustment_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(RatePeriodSwapLeg beanToCopy)
		{
		  this.type_Renamed = beanToCopy.Type;
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.paymentPeriods_Renamed = beanToCopy.PaymentPeriods;
		  this.initialExchange_Renamed = beanToCopy.InitialExchange;
		  this.intermediateExchange_Renamed = beanToCopy.IntermediateExchange;
		  this.finalExchange_Renamed = beanToCopy.FinalExchange;
		  this.paymentEvents_Renamed = beanToCopy.PaymentEvents;
		  this.paymentBusinessDayAdjustment_Renamed = beanToCopy.PaymentBusinessDayAdjustment;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return type_Renamed;
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case -1674414612: // paymentPeriods
			  return paymentPeriods_Renamed;
			case -511982201: // initialExchange
			  return initialExchange_Renamed;
			case -2147112388: // intermediateExchange
			  return intermediateExchange_Renamed;
			case -1048781383: // finalExchange
			  return finalExchange_Renamed;
			case 1031856831: // paymentEvents
			  return paymentEvents_Renamed;
			case -1420083229: // paymentBusinessDayAdjustment
			  return paymentBusinessDayAdjustment_Renamed;
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
			case 3575610: // type
			  this.type_Renamed = (SwapLegType) newValue;
			  break;
			case -885469925: // payReceive
			  this.payReceive_Renamed = (PayReceive) newValue;
			  break;
			case -1674414612: // paymentPeriods
			  this.paymentPeriods_Renamed = (IList<RatePaymentPeriod>) newValue;
			  break;
			case -511982201: // initialExchange
			  this.initialExchange_Renamed = (bool?) newValue.Value;
			  break;
			case -2147112388: // intermediateExchange
			  this.intermediateExchange_Renamed = (bool?) newValue.Value;
			  break;
			case -1048781383: // finalExchange
			  this.finalExchange_Renamed = (bool?) newValue.Value;
			  break;
			case 1031856831: // paymentEvents
			  this.paymentEvents_Renamed = (IList<SwapPaymentEvent>) newValue;
			  break;
			case -1420083229: // paymentBusinessDayAdjustment
			  this.paymentBusinessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
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

		public override RatePeriodSwapLeg build()
		{
		  return new RatePeriodSwapLeg(type_Renamed, payReceive_Renamed, paymentPeriods_Renamed, initialExchange_Renamed, intermediateExchange_Renamed, finalExchange_Renamed, paymentEvents_Renamed, paymentBusinessDayAdjustment_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the type of the leg, such as Fixed or Ibor.
		/// <para>
		/// This provides a high level categorization of the swap leg.
		/// </para>
		/// </summary>
		/// <param name="type">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder type(SwapLegType type)
		{
		  JodaBeanUtils.notNull(type, "type");
		  this.type_Renamed = type;
		  return this;
		}

		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
		/// Note that negative interest rates can result in a payment in the opposite
		/// direction to that implied by this indicator.
		/// </para>
		/// <para>
		/// The value of this flag should match the signs of the payment period notionals.
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
		/// Sets the payment periods that combine to form the swap leg.
		/// <para>
		/// Each payment period represents part of the life-time of the leg.
		/// In most cases, the periods do not overlap. However, since each payment period
		/// is essentially independent the data model allows overlapping periods.
		/// </para>
		/// <para>
		/// The start date and end date of the leg are determined from the first and last period.
		/// As such, the periods should be sorted.
		/// </para>
		/// </summary>
		/// <param name="paymentPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentPeriods(IList<RatePaymentPeriod> paymentPeriods)
		{
		  JodaBeanUtils.notEmpty(paymentPeriods, "paymentPeriods");
		  this.paymentPeriods_Renamed = paymentPeriods;
		  return this;
		}

		/// <summary>
		/// Sets the {@code paymentPeriods} property in the builder
		/// from an array of objects. </summary>
		/// <param name="paymentPeriods">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentPeriods(params RatePaymentPeriod[] paymentPeriods)
		{
		  return this.paymentPeriods(ImmutableList.copyOf(paymentPeriods));
		}

		/// <summary>
		/// Sets the flag indicating whether to exchange the initial notional.
		/// <para>
		/// Setting this to true indicates that the notional is transferred at the start of the trade.
		/// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
		/// </para>
		/// <para>
		/// This flag controls whether a notional exchange object is created when the leg is resolved.
		/// It covers an exchange on the initial payment date of the swap leg, treated as the start date.
		/// If there is an FX reset, then this flag is ignored, see {@code intermediateExchange}.
		/// If there is no FX reset and the flag is true, then a <seealso cref="NotionalExchange"/> object will be created.
		/// </para>
		/// </summary>
		/// <param name="initialExchange">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialExchange(bool initialExchange)
		{
		  this.initialExchange_Renamed = initialExchange;
		  return this;
		}

		/// <summary>
		/// Sets the flag indicating whether to exchange the differences in the notional during the lifetime of the swap.
		/// <para>
		/// Setting this to true indicates that the notional is transferred when it changes during the trade.
		/// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
		/// </para>
		/// <para>
		/// This flag controls whether a notional exchange object is created when the leg is resolved.
		/// It covers an exchange on each intermediate payment date of the swap leg.
		/// If set to true, the behavior depends on whether an FX reset payment period is defined.
		/// If there is an FX reset, then an <seealso cref="FxResetNotionalExchange"/> object will be created.
		/// If there is no FX reset, then a <seealso cref="NotionalExchange"/> object will be created.
		/// </para>
		/// </summary>
		/// <param name="intermediateExchange">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder intermediateExchange(bool intermediateExchange)
		{
		  this.intermediateExchange_Renamed = intermediateExchange;
		  return this;
		}

		/// <summary>
		/// Sets the flag indicating whether to exchange the final notional.
		/// <para>
		/// Setting this to true indicates that the notional is transferred at the end of the trade.
		/// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
		/// </para>
		/// <para>
		/// This flag controls whether a notional exchange object is created when the leg is resolved.
		/// It covers an exchange on the final payment date of the swap leg.
		/// If there is an FX reset, then this flag is ignored, see {@code intermediateExchange}.
		/// If there is no FX reset and the flag is true, then a <seealso cref="NotionalExchange"/> object will be created.
		/// </para>
		/// </summary>
		/// <param name="finalExchange">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder finalExchange(bool finalExchange)
		{
		  this.finalExchange_Renamed = finalExchange;
		  return this;
		}

		/// <summary>
		/// Sets the additional payment events that are associated with the swap leg.
		/// <para>
		/// Payment events include fees.
		/// Notional exchange may also be specified here instead of via the dedicated fields.
		/// </para>
		/// </summary>
		/// <param name="paymentEvents">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentEvents(IList<SwapPaymentEvent> paymentEvents)
		{
		  JodaBeanUtils.notNull(paymentEvents, "paymentEvents");
		  this.paymentEvents_Renamed = paymentEvents;
		  return this;
		}

		/// <summary>
		/// Sets the {@code paymentEvents} property in the builder
		/// from an array of objects. </summary>
		/// <param name="paymentEvents">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentEvents(params SwapPaymentEvent[] paymentEvents)
		{
		  return this.paymentEvents(ImmutableList.copyOf(paymentEvents));
		}

		/// <summary>
		/// Sets the business day date adjustment to be applied to each payment date, default is to apply no adjustment.
		/// <para>
		/// The business day adjustment is applied to period, exchange and event payment dates.
		/// </para>
		/// </summary>
		/// <param name="paymentBusinessDayAdjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentBusinessDayAdjustment(BusinessDayAdjustment paymentBusinessDayAdjustment)
		{
		  JodaBeanUtils.notNull(paymentBusinessDayAdjustment, "paymentBusinessDayAdjustment");
		  this.paymentBusinessDayAdjustment_Renamed = paymentBusinessDayAdjustment;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("RatePeriodSwapLeg.Builder{");
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type_Renamed)).Append(',').Append(' ');
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentPeriods").Append('=').Append(JodaBeanUtils.ToString(paymentPeriods_Renamed)).Append(',').Append(' ');
		  buf.Append("initialExchange").Append('=').Append(JodaBeanUtils.ToString(initialExchange_Renamed)).Append(',').Append(' ');
		  buf.Append("intermediateExchange").Append('=').Append(JodaBeanUtils.ToString(intermediateExchange_Renamed)).Append(',').Append(' ');
		  buf.Append("finalExchange").Append('=').Append(JodaBeanUtils.ToString(finalExchange_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentEvents").Append('=').Append(JodaBeanUtils.ToString(paymentEvents_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentBusinessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(paymentBusinessDayAdjustment_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
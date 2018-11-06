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

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using Index = com.opengamma.strata.basics.index.Index;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A fixed swap leg defined in terms of known amounts.
	/// <para>
	/// Most fixed swap legs are calculated based on a fixed rate of interest.
	/// By contrast, this leg defines a known payment amount for each period.
	/// </para>
	/// <para>
	/// Each payment occurs relative to a <i>payment period</i>.
	/// The payment periods are calculated relative to the <i>accrual periods</i>.
	/// While the model allows the frequency of the accrual and payment periods to differ,
	/// this will have no effect, as the amounts to be paid at each payment date are known.
	/// This design is intended to match FpML.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= RateCalculationSwapLeg </seealso>
	/// <seealso cref= FixedRateCalculation </seealso>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class KnownAmountSwapLeg implements SwapLeg, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class KnownAmountSwapLeg : SwapLeg, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.common.PayReceive payReceive;
		private readonly PayReceive payReceive;
	  /// <summary>
	  /// The accrual period schedule.
	  /// <para>
	  /// This is used to define the accrual periods.
	  /// These are used directly or indirectly to determine other dates in the swap.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.PeriodicSchedule accrualSchedule;
	  private readonly PeriodicSchedule accrualSchedule;
	  /// <summary>
	  /// The payment period schedule.
	  /// <para>
	  /// This is used to define the payment periods, including any compounding.
	  /// The payment period dates are based on the accrual schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final PaymentSchedule paymentSchedule;
	  private readonly PaymentSchedule paymentSchedule;
	  /// <summary>
	  /// The known amount schedule.
	  /// <para>
	  /// This defines the schedule of known amounts, relative to the payment schedule.
	  /// The schedule is defined as an initial amount, with optional changes during the tenor of the swap.
	  /// The amount is only permitted to change at payment period boundaries.
	  /// </para>
	  /// <para>
	  /// Note that the date of the payment is implied by the payment schedule.
	  /// Any dates in the known amount schedule refer to the payment schedule, not the payment date.
	  /// </para>
	  /// <para>
	  /// For example, consider a two year swap where each payment period is 3 months long.
	  /// This schedule could define two entries, one that defines the payment amounts as GBP 1000 for
	  /// the first year and one that defines the amount as GBP 500 for the second year.
	  /// In this case there will be eight payments in total, four payments of GBP 1000 in the first
	  /// year and four payments of GBP 500 in the second year.
	  /// Each payment will occur on the date specified using the offset in <seealso cref="PaymentSchedule"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.ValueSchedule amount;
	  private readonly ValueSchedule amount;
	  /// <summary>
	  /// The currency of the swap leg.
	  /// <para>
	  /// This is the currency of the known payments.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public SwapLegType getType()
	  public SwapLegType Type
	  {
		  get
		  {
			return SwapLegType.FIXED;
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.date.AdjustableDate getStartDate()
	  public AdjustableDate StartDate
	  {
		  get
		  {
			return accrualSchedule.calculatedStartDate();
		  }
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.date.AdjustableDate getEndDate()
	  public AdjustableDate EndDate
	  {
		  get
		  {
			return accrualSchedule.calculatedEndDate();
		  }
	  }

	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		builder.add(currency);
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		// no indices
	  }

	  /// <summary>
	  /// Converts this swap leg to the equivalent {@code ResolvedSwapLeg}.
	  /// <para>
	  /// An <seealso cref="ResolvedSwapLeg"/> represents the same data as this leg, but with
	  /// a complete schedule of dates defined using <seealso cref="KnownAmountSwapPaymentPeriod"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the equivalent resolved swap leg </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid swap schedule or definition </exception>
	  public ResolvedSwapLeg resolve(ReferenceData refData)
	  {
		Schedule resolvedAccruals = accrualSchedule.createSchedule(refData);
		Schedule resolvedPayments = paymentSchedule.createSchedule(resolvedAccruals, refData);
		IList<SwapPaymentPeriod> payPeriods = createPaymentPeriods(resolvedPayments, refData);
		return new ResolvedSwapLeg(Type, payReceive, payPeriods, ImmutableList.of(), currency);
	  }

	  // create the payment period
	  private IList<SwapPaymentPeriod> createPaymentPeriods(Schedule resolvedPayments, ReferenceData refData)
	  {
		// resolve amount schedule against payment schedule
		DoubleArray amounts = amount.resolveValues(resolvedPayments);
		// resolve against reference data once
		DateAdjuster paymentDateAdjuster = paymentSchedule.PaymentDateOffset.resolve(refData);
		// build up payment periods using schedule
		ImmutableList.Builder<SwapPaymentPeriod> paymentPeriods = ImmutableList.builder();
		for (int index = 0; index < resolvedPayments.size(); index++)
		{
		  SchedulePeriod paymentPeriod = resolvedPayments.getPeriod(index);
		  LocalDate baseDate = paymentSchedule.PaymentRelativeTo.selectBaseDate(paymentPeriod);
		  LocalDate paymentDate = paymentDateAdjuster.adjust(baseDate);
		  double amount = payReceive.normalize(amounts.get(index));
		  Payment payment = Payment.of(CurrencyAmount.of(currency, amount), paymentDate);
		  paymentPeriods.add(KnownAmountSwapPaymentPeriod.of(payment, paymentPeriod));
		}
		return paymentPeriods.build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code KnownAmountSwapLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static KnownAmountSwapLeg.Meta meta()
	  {
		return KnownAmountSwapLeg.Meta.INSTANCE;
	  }

	  static KnownAmountSwapLeg()
	  {
		MetaBean.register(KnownAmountSwapLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static KnownAmountSwapLeg.Builder builder()
	  {
		return new KnownAmountSwapLeg.Builder();
	  }

	  private KnownAmountSwapLeg(PayReceive payReceive, PeriodicSchedule accrualSchedule, PaymentSchedule paymentSchedule, ValueSchedule amount, Currency currency)
	  {
		JodaBeanUtils.notNull(payReceive, "payReceive");
		JodaBeanUtils.notNull(accrualSchedule, "accrualSchedule");
		JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		JodaBeanUtils.notNull(amount, "amount");
		JodaBeanUtils.notNull(currency, "currency");
		this.payReceive = payReceive;
		this.accrualSchedule = accrualSchedule;
		this.paymentSchedule = paymentSchedule;
		this.amount = amount;
		this.currency = currency;
	  }

	  public override KnownAmountSwapLeg.Meta metaBean()
	  {
		return KnownAmountSwapLeg.Meta.INSTANCE;
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
	  /// Gets the accrual period schedule.
	  /// <para>
	  /// This is used to define the accrual periods.
	  /// These are used directly or indirectly to determine other dates in the swap.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PeriodicSchedule AccrualSchedule
	  {
		  get
		  {
			return accrualSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment period schedule.
	  /// <para>
	  /// This is used to define the payment periods, including any compounding.
	  /// The payment period dates are based on the accrual schedule.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PaymentSchedule PaymentSchedule
	  {
		  get
		  {
			return paymentSchedule;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the known amount schedule.
	  /// <para>
	  /// This defines the schedule of known amounts, relative to the payment schedule.
	  /// The schedule is defined as an initial amount, with optional changes during the tenor of the swap.
	  /// The amount is only permitted to change at payment period boundaries.
	  /// </para>
	  /// <para>
	  /// Note that the date of the payment is implied by the payment schedule.
	  /// Any dates in the known amount schedule refer to the payment schedule, not the payment date.
	  /// </para>
	  /// <para>
	  /// For example, consider a two year swap where each payment period is 3 months long.
	  /// This schedule could define two entries, one that defines the payment amounts as GBP 1000 for
	  /// the first year and one that defines the amount as GBP 500 for the second year.
	  /// In this case there will be eight payments in total, four payments of GBP 1000 in the first
	  /// year and four payments of GBP 500 in the second year.
	  /// Each payment will occur on the date specified using the offset in <seealso cref="PaymentSchedule"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueSchedule Amount
	  {
		  get
		  {
			return amount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the swap leg.
	  /// <para>
	  /// This is the currency of the known payments.
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
		  KnownAmountSwapLeg other = (KnownAmountSwapLeg) obj;
		  return JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(accrualSchedule, other.accrualSchedule) && JodaBeanUtils.equal(paymentSchedule, other.paymentSchedule) && JodaBeanUtils.equal(amount, other.amount) && JodaBeanUtils.equal(currency, other.currency);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentSchedule);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(amount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("KnownAmountSwapLeg{");
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("accrualSchedule").Append('=').Append(accrualSchedule).Append(',').Append(' ');
		buf.Append("paymentSchedule").Append('=').Append(paymentSchedule).Append(',').Append(' ');
		buf.Append("amount").Append('=').Append(amount).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code KnownAmountSwapLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(KnownAmountSwapLeg), typeof(PayReceive));
			  accrualSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "accrualSchedule", typeof(KnownAmountSwapLeg), typeof(PeriodicSchedule));
			  paymentSchedule_Renamed = DirectMetaProperty.ofImmutable(this, "paymentSchedule", typeof(KnownAmountSwapLeg), typeof(PaymentSchedule));
			  amount_Renamed = DirectMetaProperty.ofImmutable(this, "amount", typeof(KnownAmountSwapLeg), typeof(ValueSchedule));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(KnownAmountSwapLeg), typeof(Currency));
			  type_Renamed = DirectMetaProperty.ofDerived(this, "type", typeof(KnownAmountSwapLeg), typeof(SwapLegType));
			  startDate_Renamed = DirectMetaProperty.ofDerived(this, "startDate", typeof(KnownAmountSwapLeg), typeof(AdjustableDate));
			  endDate_Renamed = DirectMetaProperty.ofDerived(this, "endDate", typeof(KnownAmountSwapLeg), typeof(AdjustableDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payReceive", "accrualSchedule", "paymentSchedule", "amount", "currency", "type", "startDate", "endDate");
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
		/// The meta-property for the {@code accrualSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PeriodicSchedule> accrualSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentSchedule} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PaymentSchedule> paymentSchedule_Renamed;
		/// <summary>
		/// The meta-property for the {@code amount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> amount_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code type} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapLegType> type_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> endDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payReceive", "accrualSchedule", "paymentSchedule", "amount", "currency", "type", "startDate", "endDate");
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
			case 304659814: // accrualSchedule
			  return accrualSchedule_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case -1413853096: // amount
			  return amount_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 3575610: // type
			  return type_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override KnownAmountSwapLeg.Builder builder()
		{
		  return new KnownAmountSwapLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(KnownAmountSwapLeg);
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
		/// The meta-property for the {@code accrualSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PeriodicSchedule> accrualSchedule()
		{
		  return accrualSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentSchedule} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PaymentSchedule> paymentSchedule()
		{
		  return paymentSchedule_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code amount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> amount()
		{
		  return amount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code type} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapLegType> type()
		{
		  return type_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> endDate()
		{
		  return endDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return ((KnownAmountSwapLeg) bean).PayReceive;
			case 304659814: // accrualSchedule
			  return ((KnownAmountSwapLeg) bean).AccrualSchedule;
			case -1499086147: // paymentSchedule
			  return ((KnownAmountSwapLeg) bean).PaymentSchedule;
			case -1413853096: // amount
			  return ((KnownAmountSwapLeg) bean).Amount;
			case 575402001: // currency
			  return ((KnownAmountSwapLeg) bean).Currency;
			case 3575610: // type
			  return ((KnownAmountSwapLeg) bean).Type;
			case -2129778896: // startDate
			  return ((KnownAmountSwapLeg) bean).StartDate;
			case -1607727319: // endDate
			  return ((KnownAmountSwapLeg) bean).EndDate;
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
	  /// The bean-builder for {@code KnownAmountSwapLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<KnownAmountSwapLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PeriodicSchedule accrualSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PaymentSchedule paymentSchedule_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule amount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(KnownAmountSwapLeg beanToCopy)
		{
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.accrualSchedule_Renamed = beanToCopy.AccrualSchedule;
		  this.paymentSchedule_Renamed = beanToCopy.PaymentSchedule;
		  this.amount_Renamed = beanToCopy.Amount;
		  this.currency_Renamed = beanToCopy.Currency;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -885469925: // payReceive
			  return payReceive_Renamed;
			case 304659814: // accrualSchedule
			  return accrualSchedule_Renamed;
			case -1499086147: // paymentSchedule
			  return paymentSchedule_Renamed;
			case -1413853096: // amount
			  return amount_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
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
			case 304659814: // accrualSchedule
			  this.accrualSchedule_Renamed = (PeriodicSchedule) newValue;
			  break;
			case -1499086147: // paymentSchedule
			  this.paymentSchedule_Renamed = (PaymentSchedule) newValue;
			  break;
			case -1413853096: // amount
			  this.amount_Renamed = (ValueSchedule) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
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

		public override KnownAmountSwapLeg build()
		{
		  return new KnownAmountSwapLeg(payReceive_Renamed, accrualSchedule_Renamed, paymentSchedule_Renamed, amount_Renamed, currency_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the leg is pay or receive.
		/// <para>
		/// A value of 'Pay' implies that the resulting amount is paid to the counterparty.
		/// A value of 'Receive' implies that the resulting amount is received from the counterparty.
		/// Note that negative interest rates can result in a payment in the opposite
		/// direction to that implied by this indicator.
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
		/// Sets the accrual period schedule.
		/// <para>
		/// This is used to define the accrual periods.
		/// These are used directly or indirectly to determine other dates in the swap.
		/// </para>
		/// </summary>
		/// <param name="accrualSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualSchedule(PeriodicSchedule accrualSchedule)
		{
		  JodaBeanUtils.notNull(accrualSchedule, "accrualSchedule");
		  this.accrualSchedule_Renamed = accrualSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the payment period schedule.
		/// <para>
		/// This is used to define the payment periods, including any compounding.
		/// The payment period dates are based on the accrual schedule.
		/// </para>
		/// </summary>
		/// <param name="paymentSchedule">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentSchedule(PaymentSchedule paymentSchedule)
		{
		  JodaBeanUtils.notNull(paymentSchedule, "paymentSchedule");
		  this.paymentSchedule_Renamed = paymentSchedule;
		  return this;
		}

		/// <summary>
		/// Sets the known amount schedule.
		/// <para>
		/// This defines the schedule of known amounts, relative to the payment schedule.
		/// The schedule is defined as an initial amount, with optional changes during the tenor of the swap.
		/// The amount is only permitted to change at payment period boundaries.
		/// </para>
		/// <para>
		/// Note that the date of the payment is implied by the payment schedule.
		/// Any dates in the known amount schedule refer to the payment schedule, not the payment date.
		/// </para>
		/// <para>
		/// For example, consider a two year swap where each payment period is 3 months long.
		/// This schedule could define two entries, one that defines the payment amounts as GBP 1000 for
		/// the first year and one that defines the amount as GBP 500 for the second year.
		/// In this case there will be eight payments in total, four payments of GBP 1000 in the first
		/// year and four payments of GBP 500 in the second year.
		/// Each payment will occur on the date specified using the offset in <seealso cref="PaymentSchedule"/>.
		/// </para>
		/// </summary>
		/// <param name="amount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder amount(ValueSchedule amount)
		{
		  JodaBeanUtils.notNull(amount, "amount");
		  this.amount_Renamed = amount;
		  return this;
		}

		/// <summary>
		/// Sets the currency of the swap leg.
		/// <para>
		/// This is the currency of the known payments.
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("KnownAmountSwapLeg.Builder{");
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualSchedule").Append('=').Append(JodaBeanUtils.ToString(accrualSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentSchedule").Append('=').Append(JodaBeanUtils.ToString(paymentSchedule_Renamed)).Append(',').Append(' ');
		  buf.Append("amount").Append('=').Append(JodaBeanUtils.ToString(amount_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
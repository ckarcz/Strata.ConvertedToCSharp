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

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Defines the schedule of notional amounts.
	/// <para>
	/// Interest rate swaps are based on a notional amount of money.
	/// The notional can vary during the lifetime of the swap, but only at payment period boundaries.
	/// It is not permitted to vary at an intermediate accrual (compounding) period boundary.
	/// </para>
	/// <para>
	/// In most cases, the notional amount is not exchanged, with only the net difference being exchanged.
	/// However, in certain cases, initial, final or intermediate amounts are exchanged.
	/// In this case, the notional can be referred to as the <i>principal</i>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class NotionalSchedule implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class NotionalSchedule : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The FX reset definition, optional.
	  /// <para>
	  /// This property is used when the defined amount of the notional is specified in
	  /// a currency other than the currency of the swap leg. When this occurs, the notional
	  /// amount has to be converted using an FX rate to the swap leg currency. This conversion
	  /// occurs at each payment period boundary and usually corresponds to an actual
	  /// exchange of money between the counterparties.
	  /// </para>
	  /// <para>
	  /// When building the notional schedule, if an {@code FxResetCalculation} is present,
	  /// then at least one of the notional exchange flags should be set to true. If all notional
	  /// exchange flags are false then an IllegalArgumentException is thrown.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final FxResetCalculation fxReset;
	  private readonly FxResetCalculation fxReset;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// This defines the notional as an initial amount and a list of adjustments.
	  /// The notional expressed here is intended to always be positive.
	  /// </para>
	  /// <para>
	  /// The notional is only allowed to change at payment period boundaries.
	  /// As such, the {@code ValueSchedule} steps are defined relative to the payment schedule.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.ValueSchedule amount;
	  private readonly ValueSchedule amount;
	  /// <summary>
	  /// The flag indicating whether to exchange the initial notional.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred at the start of the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
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
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final boolean finalExchange;
	  private readonly bool finalExchange;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with a single amount that does not change over time.
	  /// </summary>
	  /// <param name="notional">  the single notional that does not change over time </param>
	  /// <returns> the notional amount </returns>
	  public static NotionalSchedule of(CurrencyAmount notional)
	  {
		ArgChecker.notNull(notional, "notional");
		return NotionalSchedule.builder().currency(notional.Currency).amount(ValueSchedule.of(notional.Amount)).build();
	  }

	  /// <summary>
	  /// Obtains an instance with a single amount that does not change over time.
	  /// </summary>
	  /// <param name="currency">  the currency of the notional and swap payments </param>
	  /// <param name="amount">  the single notional amount that does not change over time </param>
	  /// <returns> the notional amount </returns>
	  public static NotionalSchedule of(Currency currency, double amount)
	  {
		ArgChecker.notNull(currency, "currency");
		return NotionalSchedule.builder().currency(currency).amount(ValueSchedule.of(amount)).build();
	  }

	  /// <summary>
	  /// Obtains an instance with a notional amount that can change over time.
	  /// </summary>
	  /// <param name="currency">  the currency of the notional and swap payments </param>
	  /// <param name="amountSchedule">  the schedule describing how the notional changes over time </param>
	  /// <returns> the notional amount </returns>
	  public static NotionalSchedule of(Currency currency, ValueSchedule amountSchedule)
	  {
		ArgChecker.notNull(currency, "currency");
		ArgChecker.notNull(amountSchedule, "amountSchedule");
		return NotionalSchedule.builder().currency(currency).amount(amountSchedule).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (fxReset != null)
		{
		  if (fxReset.ReferenceCurrency.Equals(currency))
		  {
			throw new System.ArgumentException(Messages.format("Currency {} must not equal FxResetCalculation reference currency {}", currency, fxReset.ReferenceCurrency));
		  }
		  if (!fxReset.Index.CurrencyPair.contains(currency))
		  {
			throw new System.ArgumentException(Messages.format("Currency {} must be one of those in the FxResetCalculation index {}", currency, fxReset.Index));
		  }

		  if (!(initialExchange || intermediateExchange || finalExchange))
		  {
			throw new System.ArgumentException(Messages.format("FxResetCalculation index {} was specified but schedule does not include any notional exchanges", fxReset.Index));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds notional exchange events from the payment periods and notional exchange flags.
	  /// </summary>
	  /// <param name="payPeriods">  the payment periods </param>
	  /// <param name="initialExchangeDate">  the date of the initial notional exchange </param>
	  /// <param name="refData">  the reference data to use </param>
	  /// <returns> the list of payment events </returns>
	  internal ImmutableList<SwapPaymentEvent> createEvents(IList<NotionalPaymentPeriod> payPeriods, LocalDate initialExchangeDate, ReferenceData refData)
	  {

		return createEvents(payPeriods, initialExchangeDate, initialExchange, intermediateExchange, finalExchange, refData);
	  }

	  /// <summary>
	  /// Builds notional exchange events from the payment periods and notional exchange flags.
	  /// <para>
	  /// FX reset is only processed if all three flags are true.
	  /// </para>
	  /// <para>
	  /// The {@code initialExchangeDate} is only used of {@code initialExchange} is true,
	  /// however it is intended that the value is always set to an appropriate date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payPeriods">  the payment periods </param>
	  /// <param name="initialExchangeDate">  the date of the initial notional exchange </param>
	  /// <param name="initialExchange">  whether there is an initial exchange </param>
	  /// <param name="intermediateExchange">  whether there is an intermediate exchange </param>
	  /// <param name="finalExchange">  whether there is an final exchange </param>
	  /// <param name="refData">  the reference data to use </param>
	  /// <returns> the list of payment events </returns>
	  internal static ImmutableList<SwapPaymentEvent> createEvents(IList<NotionalPaymentPeriod> payPeriods, LocalDate initialExchangeDate, bool initialExchange, bool intermediateExchange, bool finalExchange, ReferenceData refData)
	  {

		bool fxResetFound = payPeriods.Where(pp => pp.FxResetObservation.Present).First().Present;
		if (fxResetFound)
		{
		  return createFxResetEvents(payPeriods, initialExchangeDate, initialExchange, intermediateExchange, finalExchange);
		}
		else if (initialExchange || intermediateExchange || finalExchange)
		{
		  return createStandardEvents(payPeriods, initialExchangeDate, initialExchange, intermediateExchange, finalExchange);
		}
		else
		{
		  return ImmutableList.of();
		}
	  }

	  // create notional exchange events when FxReset specified
	  private static ImmutableList<SwapPaymentEvent> createFxResetEvents(IList<NotionalPaymentPeriod> payPeriods, LocalDate initialExchangeDate, bool initialExchange, bool intermediateExchange, bool finalExchange)
	  {

		ImmutableList.Builder<SwapPaymentEvent> events = ImmutableList.builder();
		for (int i = 0; i < payPeriods.Count; i++)
		{
		  NotionalPaymentPeriod period = payPeriods[i];
		  LocalDate startPaymentDate = (i == 0 ? initialExchangeDate : payPeriods[i - 1].PaymentDate);

		  bool includeStartPayment = i == 0 ? initialExchange : intermediateExchange;
		  bool includeEndPayment = i == payPeriods.Count - 1 ? finalExchange : intermediateExchange;

		  if (period.FxResetObservation.Present)
		  {

			FxIndexObservation observation = period.FxResetObservation.get();

			// notional out at start of period
			if (includeStartPayment)
			{
			  events.add(FxResetNotionalExchange.of(period.NotionalAmount.negated(), startPaymentDate, observation));
			}

			// notional in at end of period
			if (includeEndPayment)
			{
			  events.add(FxResetNotionalExchange.of(period.NotionalAmount, period.PaymentDate, observation));
			}
		  }
		  else
		  {
			// handle weird swap where only some periods have FX reset

			// notional out at start of period
			if (includeStartPayment)
			{
			  events.add(NotionalExchange.of(CurrencyAmount.of(period.Currency, -period.NotionalAmount.Amount), startPaymentDate));
			}
			// notional in at end of period
			if (includeEndPayment)
			{
			  events.add(NotionalExchange.of(CurrencyAmount.of(period.Currency, period.NotionalAmount.Amount), period.PaymentDate));
			}
		  }
		}
		return events.build();
	  }

	  // create notional exchange events when no FxReset
	  private static ImmutableList<SwapPaymentEvent> createStandardEvents(IList<NotionalPaymentPeriod> payPeriods, LocalDate initialExchangePaymentDate, bool initialExchange, bool intermediateExchange, bool finalExchange)
	  {

		NotionalPaymentPeriod firstPeriod = payPeriods[0];
		ImmutableList.Builder<SwapPaymentEvent> events = ImmutableList.builder();
		if (initialExchange)
		{
		  events.add(NotionalExchange.of(firstPeriod.NotionalAmount.negated(), initialExchangePaymentDate));
		}
		if (intermediateExchange)
		{
		  for (int i = 0; i < payPeriods.Count - 1; i++)
		  {
			NotionalPaymentPeriod period1 = payPeriods[i];
			NotionalPaymentPeriod period2 = payPeriods[i + 1];
			if (period1.NotionalAmount.Amount != period2.NotionalAmount.Amount)
			{
			  events.add(NotionalExchange.of(period1.NotionalAmount.minus(period2.NotionalAmount), period1.PaymentDate));
			}
		  }
		}
		if (finalExchange)
		{
		  NotionalPaymentPeriod lastPeriod = payPeriods[payPeriods.Count - 1];
		  events.add(NotionalExchange.of(lastPeriod.NotionalAmount, lastPeriod.PaymentDate));
		}
		return events.build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code NotionalSchedule}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static NotionalSchedule.Meta meta()
	  {
		return NotionalSchedule.Meta.INSTANCE;
	  }

	  static NotionalSchedule()
	  {
		MetaBean.register(NotionalSchedule.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static NotionalSchedule.Builder builder()
	  {
		return new NotionalSchedule.Builder();
	  }

	  private NotionalSchedule(Currency currency, FxResetCalculation fxReset, ValueSchedule amount, bool initialExchange, bool intermediateExchange, bool finalExchange)
	  {
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(amount, "amount");
		this.currency = currency;
		this.fxReset = fxReset;
		this.amount = amount;
		this.initialExchange = initialExchange;
		this.intermediateExchange = intermediateExchange;
		this.finalExchange = finalExchange;
		validate();
	  }

	  public override NotionalSchedule.Meta metaBean()
	  {
		return NotionalSchedule.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the swap leg associated with the notional.
	  /// <para>
	  /// This is the currency of the swap leg and the currency that interest calculation is made in.
	  /// </para>
	  /// <para>
	  /// The amounts of the notional are usually expressed in terms of this currency,
	  /// however they can be converted from amounts in a different currency.
	  /// See the optional {@code fxReset} property.
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
	  /// Gets the FX reset definition, optional.
	  /// <para>
	  /// This property is used when the defined amount of the notional is specified in
	  /// a currency other than the currency of the swap leg. When this occurs, the notional
	  /// amount has to be converted using an FX rate to the swap leg currency. This conversion
	  /// occurs at each payment period boundary and usually corresponds to an actual
	  /// exchange of money between the counterparties.
	  /// </para>
	  /// <para>
	  /// When building the notional schedule, if an {@code FxResetCalculation} is present,
	  /// then at least one of the notional exchange flags should be set to true. If all notional
	  /// exchange flags are false then an IllegalArgumentException is thrown.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<FxResetCalculation> FxReset
	  {
		  get
		  {
			return Optional.ofNullable(fxReset);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount.
	  /// <para>
	  /// This defines the notional as an initial amount and a list of adjustments.
	  /// The notional expressed here is intended to always be positive.
	  /// </para>
	  /// <para>
	  /// The notional is only allowed to change at payment period boundaries.
	  /// As such, the {@code ValueSchedule} steps are defined relative to the payment schedule.
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
	  /// Gets the flag indicating whether to exchange the initial notional.
	  /// <para>
	  /// Setting this to true indicates that the notional is transferred at the start of the trade.
	  /// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
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
		  NotionalSchedule other = (NotionalSchedule) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(fxReset, other.fxReset) && JodaBeanUtils.equal(amount, other.amount) && (initialExchange == other.initialExchange) && (intermediateExchange == other.intermediateExchange) && (finalExchange == other.finalExchange);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxReset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(amount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialExchange);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(intermediateExchange);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(finalExchange);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("NotionalSchedule{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("fxReset").Append('=').Append(fxReset).Append(',').Append(' ');
		buf.Append("amount").Append('=').Append(amount).Append(',').Append(' ');
		buf.Append("initialExchange").Append('=').Append(initialExchange).Append(',').Append(' ');
		buf.Append("intermediateExchange").Append('=').Append(intermediateExchange).Append(',').Append(' ');
		buf.Append("finalExchange").Append('=').Append(JodaBeanUtils.ToString(finalExchange));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code NotionalSchedule}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(NotionalSchedule), typeof(Currency));
			  fxReset_Renamed = DirectMetaProperty.ofImmutable(this, "fxReset", typeof(NotionalSchedule), typeof(FxResetCalculation));
			  amount_Renamed = DirectMetaProperty.ofImmutable(this, "amount", typeof(NotionalSchedule), typeof(ValueSchedule));
			  initialExchange_Renamed = DirectMetaProperty.ofImmutable(this, "initialExchange", typeof(NotionalSchedule), Boolean.TYPE);
			  intermediateExchange_Renamed = DirectMetaProperty.ofImmutable(this, "intermediateExchange", typeof(NotionalSchedule), Boolean.TYPE);
			  finalExchange_Renamed = DirectMetaProperty.ofImmutable(this, "finalExchange", typeof(NotionalSchedule), Boolean.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "fxReset", "amount", "initialExchange", "intermediateExchange", "finalExchange");
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
		/// The meta-property for the {@code fxReset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxResetCalculation> fxReset_Renamed;
		/// <summary>
		/// The meta-property for the {@code amount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueSchedule> amount_Renamed;
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
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "fxReset", "amount", "initialExchange", "intermediateExchange", "finalExchange");
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
			case -449555555: // fxReset
			  return fxReset_Renamed;
			case -1413853096: // amount
			  return amount_Renamed;
			case -511982201: // initialExchange
			  return initialExchange_Renamed;
			case -2147112388: // intermediateExchange
			  return intermediateExchange_Renamed;
			case -1048781383: // finalExchange
			  return finalExchange_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override NotionalSchedule.Builder builder()
		{
		  return new NotionalSchedule.Builder();
		}

		public override Type beanType()
		{
		  return typeof(NotionalSchedule);
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
		/// The meta-property for the {@code fxReset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxResetCalculation> fxReset()
		{
		  return fxReset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code amount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueSchedule> amount()
		{
		  return amount_Renamed;
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

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((NotionalSchedule) bean).Currency;
			case -449555555: // fxReset
			  return ((NotionalSchedule) bean).fxReset;
			case -1413853096: // amount
			  return ((NotionalSchedule) bean).Amount;
			case -511982201: // initialExchange
			  return ((NotionalSchedule) bean).InitialExchange;
			case -2147112388: // intermediateExchange
			  return ((NotionalSchedule) bean).IntermediateExchange;
			case -1048781383: // finalExchange
			  return ((NotionalSchedule) bean).FinalExchange;
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
	  /// The bean-builder for {@code NotionalSchedule}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<NotionalSchedule>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxResetCalculation fxReset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueSchedule amount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool initialExchange_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool intermediateExchange_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal bool finalExchange_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(NotionalSchedule beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.fxReset_Renamed = beanToCopy.fxReset;
		  this.amount_Renamed = beanToCopy.Amount;
		  this.initialExchange_Renamed = beanToCopy.InitialExchange;
		  this.intermediateExchange_Renamed = beanToCopy.IntermediateExchange;
		  this.finalExchange_Renamed = beanToCopy.FinalExchange;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return currency_Renamed;
			case -449555555: // fxReset
			  return fxReset_Renamed;
			case -1413853096: // amount
			  return amount_Renamed;
			case -511982201: // initialExchange
			  return initialExchange_Renamed;
			case -2147112388: // intermediateExchange
			  return intermediateExchange_Renamed;
			case -1048781383: // finalExchange
			  return finalExchange_Renamed;
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
			case -449555555: // fxReset
			  this.fxReset_Renamed = (FxResetCalculation) newValue;
			  break;
			case -1413853096: // amount
			  this.amount_Renamed = (ValueSchedule) newValue;
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

		public override NotionalSchedule build()
		{
		  return new NotionalSchedule(currency_Renamed, fxReset_Renamed, amount_Renamed, initialExchange_Renamed, intermediateExchange_Renamed, finalExchange_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the currency of the swap leg associated with the notional.
		/// <para>
		/// This is the currency of the swap leg and the currency that interest calculation is made in.
		/// </para>
		/// <para>
		/// The amounts of the notional are usually expressed in terms of this currency,
		/// however they can be converted from amounts in a different currency.
		/// See the optional {@code fxReset} property.
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
		/// Sets the FX reset definition, optional.
		/// <para>
		/// This property is used when the defined amount of the notional is specified in
		/// a currency other than the currency of the swap leg. When this occurs, the notional
		/// amount has to be converted using an FX rate to the swap leg currency. This conversion
		/// occurs at each payment period boundary and usually corresponds to an actual
		/// exchange of money between the counterparties.
		/// </para>
		/// <para>
		/// When building the notional schedule, if an {@code FxResetCalculation} is present,
		/// then at least one of the notional exchange flags should be set to true. If all notional
		/// exchange flags are false then an IllegalArgumentException is thrown.
		/// </para>
		/// </summary>
		/// <param name="fxReset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fxReset(FxResetCalculation fxReset)
		{
		  this.fxReset_Renamed = fxReset;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// This defines the notional as an initial amount and a list of adjustments.
		/// The notional expressed here is intended to always be positive.
		/// </para>
		/// <para>
		/// The notional is only allowed to change at payment period boundaries.
		/// As such, the {@code ValueSchedule} steps are defined relative to the payment schedule.
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
		/// Sets the flag indicating whether to exchange the initial notional.
		/// <para>
		/// Setting this to true indicates that the notional is transferred at the start of the trade.
		/// This should typically be set to true in the case of an FX reset swap, or one with a varying notional.
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
		/// </summary>
		/// <param name="finalExchange">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder finalExchange(bool finalExchange)
		{
		  this.finalExchange_Renamed = finalExchange;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("NotionalSchedule.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("fxReset").Append('=').Append(JodaBeanUtils.ToString(fxReset_Renamed)).Append(',').Append(' ');
		  buf.Append("amount").Append('=').Append(JodaBeanUtils.ToString(amount_Renamed)).Append(',').Append(' ');
		  buf.Append("initialExchange").Append('=').Append(JodaBeanUtils.ToString(initialExchange_Renamed)).Append(',').Append(' ');
		  buf.Append("intermediateExchange").Append('=').Append(JodaBeanUtils.ToString(intermediateExchange_Renamed)).Append(',').Append(' ');
		  buf.Append("finalExchange").Append('=').Append(JodaBeanUtils.ToString(finalExchange_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;
	using SerDeserializer = org.joda.beans.ser.SerDeserializer;

	using Ordering = com.google.common.collect.Ordering;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A single foreign exchange, such as an FX forward or FX spot.
	/// <para>
	/// An FX is a financial instrument that represents the exchange of an equivalent amount
	/// in two different currencies between counterparties on a specific date.
	/// For example, it might represent the payment of USD 1,000 and the receipt of EUR 932.
	/// </para>
	/// <para>
	/// FX spot and FX forward are essentially equivalent, simply with a different way to obtain the payment date; 
	/// they are both represented using this class.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FxSingle implements FxProduct, com.opengamma.strata.basics.Resolvable<ResolvedFxSingle>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxSingle : FxProduct, Resolvable<ResolvedFxSingle>, ImmutableBean
	{

	  /// <summary>
	  /// The deserializer, for compatibility.
	  /// </summary>
	  public static readonly SerDeserializer DESERIALIZER = new FxSingleDeserializer();

	  /// <summary>
	  /// The payment in the base currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// </para>
	  /// <para>
	  /// The payment date is usually the same as {@code counterCurrencyPayment}.
	  /// It is typically a valid business day, however the {@code businessDayAdjustment}
	  /// property may be used to adjust it.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment baseCurrencyPayment;
	  private readonly Payment baseCurrencyPayment;
	  /// <summary>
	  /// The payment in the counter currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// </para>
	  /// <para>
	  /// The payment date is usually the same as {@code baseCurrencyPayment}.
	  /// It is typically a valid business day, however the {@code businessDayAdjustment}
	  /// property may be used to adjust it.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment counterCurrencyPayment;
	  private readonly Payment counterCurrencyPayment;
	  /// <summary>
	  /// The payment date adjustment, optional.
	  /// <para>
	  /// If present, the adjustment will be applied to the payment date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.BusinessDayAdjustment paymentDateAdjustment;
	  private readonly BusinessDayAdjustment paymentDateAdjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an {@code FxSingle} from two payments.
	  /// <para>
	  /// The payments must be of the correct type, one pay and one receive.
	  /// The currencies of the payments must differ.
	  /// The payment dates may differ.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// </para>
	  /// <para>
	  /// No payment date adjustments apply.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment1">  the payment in the first currency </param>
	  /// <param name="payment2">  the payment in the second currency </param>
	  /// <returns> the FX </returns>
	  public static FxSingle of(Payment payment1, Payment payment2)
	  {
		return create(payment1, payment2, null);
	  }

	  /// <summary>
	  /// Creates an {@code FxSingle} from two payments, specifying a date adjustment.
	  /// <para>
	  /// The payments must be of the correct type, one pay and one receive.
	  /// The currencies of the payments must differ.
	  /// The payment dates may differ.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment1">  the payment in the first currency </param>
	  /// <param name="payment2">  the payment in the second currency </param>
	  /// <param name="paymentDateAdjustment">  the adjustment to apply to the payment date </param>
	  /// <returns> the FX </returns>
	  public static FxSingle of(Payment payment1, Payment payment2, BusinessDayAdjustment paymentDateAdjustment)
	  {
		ArgChecker.notNull(paymentDateAdjustment, "paymentDateAdjustment");
		return create(payment1, payment2, paymentDateAdjustment);
	  }

	  /// <summary>
	  /// Creates an {@code FxSingle} from two amounts and the value date.
	  /// <para>
	  /// The amounts must be of the correct type, one pay and one receive.
	  /// The currencies of the payments must differ.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// </para>
	  /// <para>
	  /// No payment date adjustments apply.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount1">  the amount in the first currency </param>
	  /// <param name="amount2">  the amount in the second currency </param>
	  /// <param name="paymentDate">  the date that the FX settles </param>
	  /// <returns> the FX </returns>
	  public static FxSingle of(CurrencyAmount amount1, CurrencyAmount amount2, LocalDate paymentDate)
	  {
		return create(amount1, amount2, paymentDate, null);
	  }

	  /// <summary>
	  /// Creates an {@code FxSingle} from two amounts and the value date, specifying a date adjustment.
	  /// <para>
	  /// The amounts must be of the correct type, one pay and one receive.
	  /// The currencies of the payments must differ.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount1">  the amount in the first currency </param>
	  /// <param name="amount2">  the amount in the second currency </param>
	  /// <param name="paymentDate">  the date that the FX settles </param>
	  /// <param name="paymentDateAdjustment">  the adjustment to apply to the payment date </param>
	  /// <returns> the FX </returns>
	  public static FxSingle of(CurrencyAmount amount1, CurrencyAmount amount2, LocalDate paymentDate, BusinessDayAdjustment paymentDateAdjustment)
	  {

		ArgChecker.notNull(paymentDateAdjustment, "paymentDateAdjustment");
		return create(amount1, amount2, paymentDate, paymentDateAdjustment);
	  }

	  /// <summary>
	  /// Creates an {@code FxSingle} using a rate.
	  /// <para>
	  /// This creates a single foreign exchange specifying the amount, FX rate and value date.
	  /// The amount must be specified using one of the currencies of the FX rate.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// </para>
	  /// <para>
	  /// No payment date adjustments apply.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount being exchanged, positive if being received, negative if being paid </param>
	  /// <param name="fxRate">  the FX rate </param>
	  /// <param name="paymentDate">  the date that the FX settles </param>
	  /// <returns> the FX </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate and amount do not have a currency in common </exception>
	  public static FxSingle of(CurrencyAmount amount, FxRate fxRate, LocalDate paymentDate)
	  {
		return create(amount, fxRate, paymentDate, null);
	  }

	  /// <summary>
	  /// Creates an {@code FxSingle} using a rate, specifying a date adjustment.
	  /// <para>
	  /// This creates a single foreign exchange specifying the amount, FX rate and value date.
	  /// The amount must be specified using one of the currencies of the FX rate.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount being exchanged, positive if being received, negative if being paid </param>
	  /// <param name="fxRate">  the FX rate </param>
	  /// <param name="paymentDate">  the date that the FX settles </param>
	  /// <param name="paymentDateAdjustment">  the adjustment to apply to the payment date </param>
	  /// <returns> the FX </returns>
	  /// <exception cref="IllegalArgumentException"> if the FX rate and amount do not have a currency in common </exception>
	  public static FxSingle of(CurrencyAmount amount, FxRate fxRate, LocalDate paymentDate, BusinessDayAdjustment paymentDateAdjustment)
	  {

		ArgChecker.notNull(paymentDateAdjustment, "paymentDateAdjustment");
		return create(amount, fxRate, paymentDate, paymentDateAdjustment);
	  }

	  // internal method where adjustment may be null
	  private static FxSingle create(CurrencyAmount amount, FxRate fxRate, LocalDate paymentDate, BusinessDayAdjustment paymentDateAdjustment)
	  {

		ArgChecker.notNull(amount, "amount");
		ArgChecker.notNull(fxRate, "fxRate");
		ArgChecker.notNull(paymentDate, "paymentDate");
		CurrencyPair pair = fxRate.Pair;
		if (!pair.contains(amount.Currency))
		{
		  throw new System.ArgumentException(Messages.format("FxRate '{}' and CurrencyAmount '{}' must have a currency in common", fxRate, amount));
		}
		Currency currency2 = pair.Base.Equals(amount.Currency) ? pair.Counter : pair.Base;
		CurrencyAmount amountCurrency2 = amount.convertedTo(currency2, fxRate).negated();
		return create(amount, amountCurrency2, paymentDate, paymentDateAdjustment);
	  }

	  // internal method where adjustment may be null
	  private static FxSingle create(CurrencyAmount amount1, CurrencyAmount amount2, LocalDate paymentDate, BusinessDayAdjustment paymentDateAdjustment)
	  {

		ArgChecker.notNull(amount1, "amount1");
		ArgChecker.notNull(amount2, "amount2");
		ArgChecker.notNull(paymentDate, "paymentDate");
		return create(Payment.of(amount1, paymentDate), Payment.of(amount2, paymentDate), paymentDateAdjustment);
	  }

	  // internal method where adjustment may be null
	  private static FxSingle create(Payment payment1, Payment payment2, BusinessDayAdjustment paymentDateAdjustment)
	  {

		ArgChecker.notNull(payment1, "payment1");
		ArgChecker.notNull(payment2, "payment2");
		CurrencyPair pair = CurrencyPair.of(payment1.Currency, payment2.Currency);
		if (pair.Conventional)
		{
		  return new FxSingle(payment1, payment2, paymentDateAdjustment);
		}
		else
		{
		  return new FxSingle(payment2, payment1, paymentDateAdjustment);
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (baseCurrencyPayment.Currency.Equals(counterCurrencyPayment.Currency))
		{
		  throw new System.ArgumentException("Amounts must have different currencies");
		}
		if ((baseCurrencyPayment.Amount != 0d || counterCurrencyPayment.Amount != 0d) && Math.Sign(baseCurrencyPayment.Amount) != -Math.Sign(counterCurrencyPayment.Amount))
		{
		  throw new System.ArgumentException("Amounts must have different signs");
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		// swap order to be base/counter if reverse is conventional
		// this handles deserialization where the base/counter rules differ from those applicable at serialization
		Payment @base = builder.baseCurrencyPayment;
		Payment counter = builder.counterCurrencyPayment;
		CurrencyPair pair = CurrencyPair.of(counter.Currency, @base.Currency);
		if (pair.Conventional)
		{
		  builder.baseCurrencyPayment = counter;
		  builder.counterCurrencyPayment = @base;
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets currency pair of the base currency and counter currency.
	  /// <para>
	  /// This currency pair is conventional, thus indifferent to the direction of FX.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return CurrencyPair.of(baseCurrencyPayment.Currency, counterCurrencyPayment.Currency);
		  }
	  }

	  /// <summary>
	  /// Gets the amount in the base currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the amount </returns>
	  public CurrencyAmount BaseCurrencyAmount
	  {
		  get
		  {
			return baseCurrencyPayment.Value;
		  }
	  }

	  /// <summary>
	  /// Gets the amount in the counter currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the amount </returns>
	  public CurrencyAmount CounterCurrencyAmount
	  {
		  get
		  {
			return counterCurrencyPayment.Value;
		  }
	  }

	  /// <summary>
	  /// Gets the currency amount in which the amount is paid.
	  /// <para>
	  /// This returns the currency amount whose amount is negative or zero.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the pay currency amount </returns>
	  public CurrencyAmount PayCurrencyAmount
	  {
		  get
		  {
			if (baseCurrencyPayment.Amount <= 0d)
			{
			  return baseCurrencyPayment.Value;
			}
			return counterCurrencyPayment.Value;
		  }
	  }

	  /// <summary>
	  /// Gets the currency amount in which the amount is received.
	  /// <para>
	  /// This returns the currency amount whose amount is non-negative.
	  /// If both are zero, {@code counterCurrencyAmount} is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the receive currency amount </returns>
	  public CurrencyAmount ReceiveCurrencyAmount
	  {
		  get
		  {
			if (baseCurrencyPayment.Amount > 0d)
			{
			  return baseCurrencyPayment.Value;
			}
			return counterCurrencyPayment.Value;
		  }
	  }

	  /// <summary>
	  /// Gets the last payment date.
	  /// <para>
	  /// The payment date is normally the same for the base and counter currencies.
	  /// If it differs, this method returns the latest of the two dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the latest payment date </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return Ordering.natural().max(baseCurrencyPayment.Date, counterCurrencyPayment.Date);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedFxSingle resolve(ReferenceData refData)
	  {
		if (paymentDateAdjustment == null)
		{
		  return ResolvedFxSingle.of(baseCurrencyPayment, counterCurrencyPayment);
		}
		DateAdjuster adjuster = paymentDateAdjustment.resolve(refData);
		return ResolvedFxSingle.of(baseCurrencyPayment.adjustDate(adjuster), counterCurrencyPayment.adjustDate(adjuster));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSingle}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxSingle.Meta meta()
	  {
		return FxSingle.Meta.INSTANCE;
	  }

	  static FxSingle()
	  {
		MetaBean.register(FxSingle.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FxSingle(Payment baseCurrencyPayment, Payment counterCurrencyPayment, BusinessDayAdjustment paymentDateAdjustment)
	  {
		JodaBeanUtils.notNull(baseCurrencyPayment, "baseCurrencyPayment");
		JodaBeanUtils.notNull(counterCurrencyPayment, "counterCurrencyPayment");
		this.baseCurrencyPayment = baseCurrencyPayment;
		this.counterCurrencyPayment = counterCurrencyPayment;
		this.paymentDateAdjustment = paymentDateAdjustment;
		validate();
	  }

	  public override FxSingle.Meta metaBean()
	  {
		return FxSingle.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment in the base currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// </para>
	  /// <para>
	  /// The payment date is usually the same as {@code counterCurrencyPayment}.
	  /// It is typically a valid business day, however the {@code businessDayAdjustment}
	  /// property may be used to adjust it.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Payment BaseCurrencyPayment
	  {
		  get
		  {
			return baseCurrencyPayment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment in the counter currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// </para>
	  /// <para>
	  /// The payment date is usually the same as {@code baseCurrencyPayment}.
	  /// It is typically a valid business day, however the {@code businessDayAdjustment}
	  /// property may be used to adjust it.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Payment CounterCurrencyPayment
	  {
		  get
		  {
			return counterCurrencyPayment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment date adjustment, optional.
	  /// <para>
	  /// If present, the adjustment will be applied to the payment date.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<BusinessDayAdjustment> PaymentDateAdjustment
	  {
		  get
		  {
			return Optional.ofNullable(paymentDateAdjustment);
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  FxSingle other = (FxSingle) obj;
		  return JodaBeanUtils.equal(baseCurrencyPayment, other.baseCurrencyPayment) && JodaBeanUtils.equal(counterCurrencyPayment, other.counterCurrencyPayment) && JodaBeanUtils.equal(paymentDateAdjustment, other.paymentDateAdjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(baseCurrencyPayment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(counterCurrencyPayment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateAdjustment);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("FxSingle{");
		buf.Append("baseCurrencyPayment").Append('=').Append(baseCurrencyPayment).Append(',').Append(' ');
		buf.Append("counterCurrencyPayment").Append('=').Append(counterCurrencyPayment).Append(',').Append(' ');
		buf.Append("paymentDateAdjustment").Append('=').Append(JodaBeanUtils.ToString(paymentDateAdjustment));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSingle}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  baseCurrencyPayment_Renamed = DirectMetaProperty.ofImmutable(this, "baseCurrencyPayment", typeof(FxSingle), typeof(Payment));
			  counterCurrencyPayment_Renamed = DirectMetaProperty.ofImmutable(this, "counterCurrencyPayment", typeof(FxSingle), typeof(Payment));
			  paymentDateAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateAdjustment", typeof(FxSingle), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "baseCurrencyPayment", "counterCurrencyPayment", "paymentDateAdjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code baseCurrencyPayment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Payment> baseCurrencyPayment_Renamed;
		/// <summary>
		/// The meta-property for the {@code counterCurrencyPayment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Payment> counterCurrencyPayment_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDateAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> paymentDateAdjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "baseCurrencyPayment", "counterCurrencyPayment", "paymentDateAdjustment");
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
			case 765258148: // baseCurrencyPayment
			  return baseCurrencyPayment_Renamed;
			case -863240423: // counterCurrencyPayment
			  return counterCurrencyPayment_Renamed;
			case 737375073: // paymentDateAdjustment
			  return paymentDateAdjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FxSingle> builder()
		public override BeanBuilder<FxSingle> builder()
		{
		  return new FxSingle.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxSingle);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code baseCurrencyPayment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Payment> baseCurrencyPayment()
		{
		  return baseCurrencyPayment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code counterCurrencyPayment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Payment> counterCurrencyPayment()
		{
		  return counterCurrencyPayment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDateAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> paymentDateAdjustment()
		{
		  return paymentDateAdjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 765258148: // baseCurrencyPayment
			  return ((FxSingle) bean).BaseCurrencyPayment;
			case -863240423: // counterCurrencyPayment
			  return ((FxSingle) bean).CounterCurrencyPayment;
			case 737375073: // paymentDateAdjustment
			  return ((FxSingle) bean).paymentDateAdjustment;
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
	  /// The bean-builder for {@code FxSingle}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FxSingle>
	  {

		internal Payment baseCurrencyPayment;
		internal Payment counterCurrencyPayment;
		internal BusinessDayAdjustment paymentDateAdjustment;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 765258148: // baseCurrencyPayment
			  return baseCurrencyPayment;
			case -863240423: // counterCurrencyPayment
			  return counterCurrencyPayment;
			case 737375073: // paymentDateAdjustment
			  return paymentDateAdjustment;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 765258148: // baseCurrencyPayment
			  this.baseCurrencyPayment = (Payment) newValue;
			  break;
			case -863240423: // counterCurrencyPayment
			  this.counterCurrencyPayment = (Payment) newValue;
			  break;
			case 737375073: // paymentDateAdjustment
			  this.paymentDateAdjustment = (BusinessDayAdjustment) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FxSingle build()
		{
		  preBuild(this);
		  return new FxSingle(baseCurrencyPayment, counterCurrencyPayment, paymentDateAdjustment);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FxSingle.Builder{");
		  buf.Append("baseCurrencyPayment").Append('=').Append(JodaBeanUtils.ToString(baseCurrencyPayment)).Append(',').Append(' ');
		  buf.Append("counterCurrencyPayment").Append('=').Append(JodaBeanUtils.ToString(counterCurrencyPayment)).Append(',').Append(' ');
		  buf.Append("paymentDateAdjustment").Append('=').Append(JodaBeanUtils.ToString(paymentDateAdjustment));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
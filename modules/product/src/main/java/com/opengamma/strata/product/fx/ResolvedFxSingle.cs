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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A single FX transaction, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="FxSingle"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedFxSingle} from a {@code FxSingle}
	/// using <seealso cref="FxSingle#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// The two payments are identified as the base and counter currencies in a standardized currency pair.
	/// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	/// See <seealso cref="CurrencyPair"/> for details of the configuration that determines the ordering.
	/// </para>
	/// <para>
	/// A {@code ResolvedFxSingle} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ResolvedFxSingle implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedFxSingle : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment baseCurrencyPayment;
		private readonly Payment baseCurrencyPayment;
	  /// <summary>
	  /// The payment in the counter currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The payment amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment counterCurrencyPayment;
	  private readonly Payment counterCurrencyPayment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an {@code ResolvedFxSingle} from two equivalent payments in different currencies.
	  /// <para>
	  /// The payments must be of the correct type, one pay and one receive.
	  /// The currencies of the payments must differ.
	  /// </para>
	  /// <para>
	  /// This factory identifies the currency pair of the exchange and assigns the payments
	  /// to match the base or counter currency of the standardized currency pair.
	  /// For example, a EUR/USD exchange always has EUR as the base payment and USD as the counter payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payment1">  the first payment </param>
	  /// <param name="payment2">  the second payment </param>
	  /// <returns> the resolved foreign exchange transaction </returns>
	  public static ResolvedFxSingle of(Payment payment1, Payment payment2)
	  {
		CurrencyPair pair = CurrencyPair.of(payment2.Currency, payment1.Currency);
		if (pair.Conventional)
		{
		  return new ResolvedFxSingle(payment2, payment1);
		}
		else
		{
		  return new ResolvedFxSingle(payment1, payment2);
		}
	  }

	  /// <summary>
	  /// Creates an {@code ResolvedFxSingle} from two amounts and the value date.
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
	  /// <param name="valueDate">  the value date </param>
	  /// <returns> the resolved foreign exchange transaction </returns>
	  public static ResolvedFxSingle of(CurrencyAmount amount1, CurrencyAmount amount2, LocalDate valueDate)
	  {
		return ResolvedFxSingle.of(Payment.of(amount1, valueDate), Payment.of(amount2, valueDate));
	  }

	  /// <summary>
	  /// Creates an {@code ResolvedFxSingle} using a rate.
	  /// <para>
	  /// This create an FX specifying a value date, notional in one currency, the second currency
	  /// and the FX rate between the two.
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
	  /// <param name="amountCurrency1">  the amount of the near leg in the first currency </param>
	  /// <param name="fxRate">  the near FX rate </param>
	  /// <param name="paymentDate">  date that the FX settles </param>
	  /// <returns> the resolved foreign exchange transaction </returns>
	  public static ResolvedFxSingle of(CurrencyAmount amountCurrency1, FxRate fxRate, LocalDate paymentDate)
	  {
		CurrencyPair pair = fxRate.Pair;
		ArgChecker.isTrue(pair.contains(amountCurrency1.Currency));
		Currency currency2 = pair.Base.Equals(amountCurrency1.Currency) ? pair.Counter : pair.Base;
		CurrencyAmount amountCurrency2 = amountCurrency1.convertedTo(currency2, fxRate).negated();
		return ResolvedFxSingle.of(Payment.of(amountCurrency1, paymentDate), Payment.of(amountCurrency2, paymentDate));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (baseCurrencyPayment.Currency.Equals(counterCurrencyPayment.Currency))
		{
		  throw new System.ArgumentException("Payments must have different currencies");
		}
		if ((baseCurrencyPayment.Amount != 0d || counterCurrencyPayment.Amount != 0d) && Math.Sign(baseCurrencyPayment.Amount) != -Math.Sign(counterCurrencyPayment.Amount))
		{
		  throw new System.ArgumentException("Payments must have different signs");
		}
		ArgChecker.inOrderOrEqual(baseCurrencyPayment.Date, counterCurrencyPayment.Date, "baseCurrencyPayment.date", "counterCurrencyPayment.date");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		// swap order to be base/counter if reverse is conventional
		// this handled deserialization where the base/counter rules differ from those applicable at serialization
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
	  /// Gets the currency amount in which the amount is received.
	  /// <para>
	  /// This returns the currency amount whose amount is non-negative.
	  /// If both are zero, the currency amount of {@code counterCurrencyPayment} is returned.
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
			  return CurrencyAmount.of(baseCurrencyPayment.Currency, baseCurrencyPayment.Amount);
			}
			return CurrencyAmount.of(counterCurrencyPayment.Currency, counterCurrencyPayment.Amount);
		  }
	  }

	  /// <summary>
	  /// Returns the date that the transaction settles.
	  /// <para>
	  /// This returns the settlement date of the base currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the value date </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return baseCurrencyPayment.Date;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the inverse transaction.
	  /// <para>
	  /// The result has the base and counter payments negated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the inverse transaction </returns>
	  public ResolvedFxSingle inverse()
	  {
		return new ResolvedFxSingle(baseCurrencyPayment.negated(), counterCurrencyPayment.negated());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxSingle}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedFxSingle.Meta meta()
	  {
		return ResolvedFxSingle.Meta.INSTANCE;
	  }

	  static ResolvedFxSingle()
	  {
		MetaBean.register(ResolvedFxSingle.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ResolvedFxSingle(Payment baseCurrencyPayment, Payment counterCurrencyPayment)
	  {
		JodaBeanUtils.notNull(baseCurrencyPayment, "baseCurrencyPayment");
		JodaBeanUtils.notNull(counterCurrencyPayment, "counterCurrencyPayment");
		this.baseCurrencyPayment = baseCurrencyPayment;
		this.counterCurrencyPayment = counterCurrencyPayment;
		validate();
	  }

	  public override ResolvedFxSingle.Meta metaBean()
	  {
		return ResolvedFxSingle.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment in the base currency, positive if receiving, negative if paying.
	  /// <para>
	  /// The payment amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
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
	  /// The payment amount is signed.
	  /// A positive amount indicates the payment is to be received.
	  /// A negative amount indicates the payment is to be paid.
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
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  ResolvedFxSingle other = (ResolvedFxSingle) obj;
		  return JodaBeanUtils.equal(baseCurrencyPayment, other.baseCurrencyPayment) && JodaBeanUtils.equal(counterCurrencyPayment, other.counterCurrencyPayment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(baseCurrencyPayment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(counterCurrencyPayment);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ResolvedFxSingle{");
		buf.Append("baseCurrencyPayment").Append('=').Append(baseCurrencyPayment).Append(',').Append(' ');
		buf.Append("counterCurrencyPayment").Append('=').Append(JodaBeanUtils.ToString(counterCurrencyPayment));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedFxSingle}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  baseCurrencyPayment_Renamed = DirectMetaProperty.ofImmutable(this, "baseCurrencyPayment", typeof(ResolvedFxSingle), typeof(Payment));
			  counterCurrencyPayment_Renamed = DirectMetaProperty.ofImmutable(this, "counterCurrencyPayment", typeof(ResolvedFxSingle), typeof(Payment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "baseCurrencyPayment", "counterCurrencyPayment");
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
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "baseCurrencyPayment", "counterCurrencyPayment");
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
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ResolvedFxSingle> builder()
		public override BeanBuilder<ResolvedFxSingle> builder()
		{
		  return new ResolvedFxSingle.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedFxSingle);
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

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 765258148: // baseCurrencyPayment
			  return ((ResolvedFxSingle) bean).BaseCurrencyPayment;
			case -863240423: // counterCurrencyPayment
			  return ((ResolvedFxSingle) bean).CounterCurrencyPayment;
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
	  /// The bean-builder for {@code ResolvedFxSingle}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ResolvedFxSingle>
	  {

		internal Payment baseCurrencyPayment;
		internal Payment counterCurrencyPayment;

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
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ResolvedFxSingle build()
		{
		  preBuild(this);
		  return new ResolvedFxSingle(baseCurrencyPayment, counterCurrencyPayment);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ResolvedFxSingle.Builder{");
		  buf.Append("baseCurrencyPayment").Append('=').Append(JodaBeanUtils.ToString(baseCurrencyPayment)).Append(',').Append(' ');
		  buf.Append("counterCurrencyPayment").Append('=').Append(JodaBeanUtils.ToString(counterCurrencyPayment));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
﻿using System;
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
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// An exchange of notionals between two counterparties.
	/// <para>
	/// In most swaps, the notional amount is not exchanged, with only the net difference being exchanged.
	/// However, in certain cases, initial, final or intermediate amounts are exchanged.
	/// In this case, the notional can be referred to as the principal.
	/// </para>
	/// <para>
	/// This class represents a notional exchange where the amount is known in advance.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class NotionalExchange implements SwapPaymentEvent, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class NotionalExchange : SwapPaymentEvent, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Payment payment;
		private readonly Payment payment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the amount and date.
	  /// </summary>
	  /// <param name="paymentAmount">  the amount of the notional exchange </param>
	  /// <param name="paymentDate">  the date that the payment is made </param>
	  /// <returns> the notional exchange </returns>
	  public static NotionalExchange of(CurrencyAmount paymentAmount, LocalDate paymentDate)
	  {
		return new NotionalExchange(Payment.of(paymentAmount, paymentDate));
	  }

	  /// <summary>
	  /// Obtains an instance from the payment.
	  /// </summary>
	  /// <param name="payment">  the payment to be made </param>
	  /// <returns> the notional exchange </returns>
	  public static NotionalExchange of(Payment payment)
	  {
		return new NotionalExchange(payment);
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return payment.Date;
		  }
	  }

	  /// <summary>
	  /// Gets the payment amount.
	  /// </summary>
	  /// <returns> the payment amount </returns>
	  public CurrencyAmount PaymentAmount
	  {
		  get
		  {
			return payment.Value;
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the event.
	  /// <para>
	  /// The currency of the event is the currency of the payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency of the event </returns>
	  public Currency Currency
	  {
		  get
		  {
			return payment.Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public NotionalExchange adjustPaymentDate(TemporalAdjuster adjuster)
	  {
		LocalDate adjusted = payment.Date.with(adjuster);
		return of(payment.Value, adjusted);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code NotionalExchange}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static NotionalExchange.Meta meta()
	  {
		return NotionalExchange.Meta.INSTANCE;
	  }

	  static NotionalExchange()
	  {
		MetaBean.register(NotionalExchange.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private NotionalExchange(Payment payment)
	  {
		JodaBeanUtils.notNull(payment, "payment");
		this.payment = payment;
	  }

	  public override NotionalExchange.Meta metaBean()
	  {
		return NotionalExchange.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional exchange payment.
	  /// <para>
	  /// This contains the amount to be paid and the date that payment occurs.
	  /// This date has been adjusted to be a valid business day.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Payment Payment
	  {
		  get
		  {
			return payment;
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
		  NotionalExchange other = (NotionalExchange) obj;
		  return JodaBeanUtils.equal(payment, other.payment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payment);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("NotionalExchange{");
		buf.Append("payment").Append('=').Append(JodaBeanUtils.ToString(payment));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code NotionalExchange}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  payment_Renamed = DirectMetaProperty.ofImmutable(this, "payment", typeof(NotionalExchange), typeof(Payment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "payment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code payment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Payment> payment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "payment");
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
			case -786681338: // payment
			  return payment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends NotionalExchange> builder()
		public override BeanBuilder<NotionalExchange> builder()
		{
		  return new NotionalExchange.Builder();
		}

		public override Type beanType()
		{
		  return typeof(NotionalExchange);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code payment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Payment> payment()
		{
		  return payment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -786681338: // payment
			  return ((NotionalExchange) bean).Payment;
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
	  /// The bean-builder for {@code NotionalExchange}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<NotionalExchange>
	  {

		internal Payment payment;

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
			case -786681338: // payment
			  return payment;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -786681338: // payment
			  this.payment = (Payment) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override NotionalExchange build()
		{
		  return new NotionalExchange(payment);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("NotionalExchange.Builder{");
		  buf.Append("payment").Append('=').Append(JodaBeanUtils.ToString(payment));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
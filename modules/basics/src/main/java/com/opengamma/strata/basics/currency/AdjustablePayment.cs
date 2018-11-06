using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
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

	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;

	/// <summary>
	/// A single payment of a known amount on a date, with business day adjustment rules.
	/// <para>
	/// This class represents a payment, where the payment date and amount are known.
	/// The date is specified using an <seealso cref="AdjustableDate"/> which allows holidays and weekends to be handled.
	/// A negative value indicates the amount is to be paid while a positive value indicates the amount is received.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class AdjustablePayment implements com.opengamma.strata.basics.Resolvable<Payment>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class AdjustablePayment : Resolvable<Payment>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CurrencyAmount value;
		private readonly CurrencyAmount value;
	  /// <summary>
	  /// The date that the payment is made.
	  /// <para>
	  /// This date should normally be a valid business day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.AdjustableDate date;
	  private readonly AdjustableDate date;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance representing an amount where the date is fixed.
	  /// <para>
	  /// Whether the payment is pay or receive is determined by the sign of the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the payment </param>
	  /// <param name="amount">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment of(Currency currency, double amount, LocalDate date)
	  {
		return new AdjustablePayment(CurrencyAmount.of(currency, amount), AdjustableDate.of(date));
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount where the date is adjustable.
	  /// <para>
	  /// Whether the payment is pay or receive is determined by the sign of the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the payment </param>
	  /// <param name="amount">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment of(Currency currency, double amount, AdjustableDate date)
	  {
		return new AdjustablePayment(CurrencyAmount.of(currency, amount), date);
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount where the date is fixed.
	  /// <para>
	  /// Whether the payment is pay or receive is determined by the sign of the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment of(CurrencyAmount value, LocalDate date)
	  {
		return new AdjustablePayment(value, AdjustableDate.of(date));
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount where the date is adjustable.
	  /// <para>
	  /// Whether the payment is pay or receive is determined by the sign of the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment of(CurrencyAmount value, AdjustableDate date)
	  {
		return new AdjustablePayment(value, date);
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount to be paid where the date is fixed.
	  /// <para>
	  /// The sign of the amount will be normalized to be negative, indicating a payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment ofPay(CurrencyAmount value, LocalDate date)
	  {
		return new AdjustablePayment(value.negative(), AdjustableDate.of(date));
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount to be paid where the date is adjustable.
	  /// <para>
	  /// The sign of the amount will be normalized to be negative, indicating a payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment ofPay(CurrencyAmount value, AdjustableDate date)
	  {
		return new AdjustablePayment(value.negative(), date);
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount to be received where the date is fixed.
	  /// <para>
	  /// The sign of the amount will be normalized to be positive, indicating receipt.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment ofReceive(CurrencyAmount value, LocalDate date)
	  {
		return new AdjustablePayment(value.positive(), AdjustableDate.of(date));
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount to be received where the date is adjustable.
	  /// <para>
	  /// The sign of the amount will be normalized to be positive, indicating receipt.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment ofReceive(CurrencyAmount value, AdjustableDate date)
	  {
		return new AdjustablePayment(value.positive(), date);
	  }

	  /// <summary>
	  /// Obtains an instance based on a {@code Payment}.
	  /// </summary>
	  /// <param name="payment">  the fixed payment </param>
	  /// <returns> the adjustable payment instance </returns>
	  public static AdjustablePayment of(Payment payment)
	  {
		return of(payment.Value, payment.Date);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the payment.
	  /// <para>
	  /// This simply returns {@code getValue().getCurrency()}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency of the payment </returns>
	  public Currency Currency
	  {
		  get
		  {
			return value.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the amount of the payment.
	  /// <para>
	  /// The payment value is signed.
	  /// A negative value indicates a payment while a positive value indicates receipt.
	  /// </para>
	  /// <para>
	  /// This simply returns {@code getValue().getAmount()}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the amount of the payment </returns>
	  public double Amount
	  {
		  get
		  {
			return value.Amount;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves the date on this payment, returning a payment with a fixed date.
	  /// <para>
	  /// This returns a <seealso cref="Payment"/> with the same amount and resolved date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the resolved payment </returns>
	  public Payment resolve(ReferenceData refData)
	  {
		return Payment.of(value, date.adjusted(refData));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this payment with the value negated.
	  /// <para>
	  /// This takes this payment and negates it.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a payment based on this with the value negated </returns>
	  public AdjustablePayment negated()
	  {
		return AdjustablePayment.of(value.negated(), date);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code AdjustablePayment}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static AdjustablePayment.Meta meta()
	  {
		return AdjustablePayment.Meta.INSTANCE;
	  }

	  static AdjustablePayment()
	  {
		MetaBean.register(AdjustablePayment.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private AdjustablePayment(CurrencyAmount value, AdjustableDate date)
	  {
		JodaBeanUtils.notNull(value, "value");
		JodaBeanUtils.notNull(date, "date");
		this.value = value;
		this.date = date;
	  }

	  public override AdjustablePayment.Meta metaBean()
	  {
		return AdjustablePayment.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the amount of the payment.
	  /// <para>
	  /// The amount is signed.
	  /// A negative value indicates the amount is to be paid while a positive value indicates the amount is received.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmount Value
	  {
		  get
		  {
			return value;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date that the payment is made.
	  /// <para>
	  /// This date should normally be a valid business day.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public AdjustableDate Date
	  {
		  get
		  {
			return date;
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
		  AdjustablePayment other = (AdjustablePayment) obj;
		  return JodaBeanUtils.equal(value, other.value) && JodaBeanUtils.equal(date, other.date);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("AdjustablePayment{");
		buf.Append("value").Append('=').Append(value).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code AdjustablePayment}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  value_Renamed = DirectMetaProperty.ofImmutable(this, "value", typeof(AdjustablePayment), typeof(CurrencyAmount));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(AdjustablePayment), typeof(AdjustableDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "value", "date");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> value_Renamed;
		/// <summary>
		/// The meta-property for the {@code date} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> date_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "value", "date");
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
			case 111972721: // value
			  return value_Renamed;
			case 3076014: // date
			  return date_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends AdjustablePayment> builder()
		public override BeanBuilder<AdjustablePayment> builder()
		{
		  return new AdjustablePayment.Builder();
		}

		public override Type beanType()
		{
		  return typeof(AdjustablePayment);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> value()
		{
		  return value_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code date} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> date()
		{
		  return date_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  return ((AdjustablePayment) bean).Value;
			case 3076014: // date
			  return ((AdjustablePayment) bean).Date;
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
	  /// The bean-builder for {@code AdjustablePayment}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<AdjustablePayment>
	  {

		internal CurrencyAmount value;
		internal AdjustableDate date;

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
			case 111972721: // value
			  return value;
			case 3076014: // date
			  return date;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  this.value = (CurrencyAmount) newValue;
			  break;
			case 3076014: // date
			  this.date = (AdjustableDate) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override AdjustablePayment build()
		{
		  return new AdjustablePayment(value, date);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("AdjustablePayment.Builder{");
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value)).Append(',').Append(' ');
		  buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
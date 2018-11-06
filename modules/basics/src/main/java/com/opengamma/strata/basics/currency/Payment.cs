using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{

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

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// A single payment of a known amount on a specific date.
	/// <para>
	/// This class represents a payment, where the payment date and amount are known.
	/// A negative value indicates the amount is to be paid while a positive value indicates the amount is received.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class Payment implements FxConvertible<Payment>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Payment : FxConvertible<Payment>, ImmutableBean
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate date;
	  private readonly LocalDate date;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance representing an amount.
	  /// <para>
	  /// Whether the payment is pay or receive is determined by the sign of the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency of the payment </param>
	  /// <param name="amount">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the payment instance </returns>
	  public static Payment of(Currency currency, double amount, LocalDate date)
	  {
		return new Payment(CurrencyAmount.of(currency, amount), date);
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount.
	  /// <para>
	  /// Whether the payment is pay or receive is determined by the sign of the specified amount.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the payment instance </returns>
	  public static Payment of(CurrencyAmount value, LocalDate date)
	  {
		return new Payment(value, date);
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount to be paid.
	  /// <para>
	  /// The sign of the amount will be normalized to be negative, indicating a payment.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the payment instance </returns>
	  public static Payment ofPay(CurrencyAmount value, LocalDate date)
	  {
		return new Payment(value.negative(), date);
	  }

	  /// <summary>
	  /// Obtains an instance representing an amount to be received.
	  /// <para>
	  /// The sign of the amount will be normalized to be positive, indicating receipt.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the amount of the payment </param>
	  /// <param name="date">  the date that the payment is made </param>
	  /// <returns> the payment instance </returns>
	  public static Payment ofReceive(CurrencyAmount value, LocalDate date)
	  {
		return new Payment(value.positive(), date);
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
	  /// Adjusts the payment date using the rules of the specified adjuster.
	  /// <para>
	  /// The adjuster is typically an instance of <seealso cref="BusinessDayAdjustment"/>.
	  /// If the date is unchanged by the adjuster, {@code this} payment will be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="adjuster">  the adjuster to apply to the payment date </param>
	  /// <returns> the adjusted payment </returns>
	  public Payment adjustDate(TemporalAdjuster adjuster)
	  {
		LocalDate adjusted = date.with(adjuster);
		return adjusted.Equals(date) ? this : toBuilder().date(adjusted).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this {@code Payment} with the value negated.
	  /// <para>
	  /// This takes this payment and negates it.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a payment based on this with the value negated </returns>
	  public Payment negated()
	  {
		return Payment.of(value.negated(), date);
	  }

	  /// <summary>
	  /// Converts this payment to an equivalent payment in the specified currency.
	  /// <para>
	  /// The result will be expressed in terms of the given currency.
	  /// If conversion is needed, the provider will be used to supply the FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public Payment convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		if (Currency.Equals(resultCurrency))
		{
		  return this;
		}
		return Payment.of(value.convertedTo(resultCurrency, rateProvider), date);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Payment}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Payment.Meta meta()
	  {
		return Payment.Meta.INSTANCE;
	  }

	  static Payment()
	  {
		MetaBean.register(Payment.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Payment.Builder builder()
	  {
		return new Payment.Builder();
	  }

	  private Payment(CurrencyAmount value, LocalDate date)
	  {
		JodaBeanUtils.notNull(value, "value");
		JodaBeanUtils.notNull(date, "date");
		this.value = value;
		this.date = date;
	  }

	  public override Payment.Meta metaBean()
	  {
		return Payment.Meta.INSTANCE;
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
	  public LocalDate Date
	  {
		  get
		  {
			return date;
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
		  Payment other = (Payment) obj;
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
		buf.Append("Payment{");
		buf.Append("value").Append('=').Append(value).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Payment}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  value_Renamed = DirectMetaProperty.ofImmutable(this, "value", typeof(Payment), typeof(CurrencyAmount));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(Payment), typeof(LocalDate));
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
		internal MetaProperty<LocalDate> date_Renamed;
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

		public override Payment.Builder builder()
		{
		  return new Payment.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Payment);
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
		public MetaProperty<LocalDate> date()
		{
		  return date_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  return ((Payment) bean).Value;
			case 3076014: // date
			  return ((Payment) bean).Date;
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
	  /// The bean-builder for {@code Payment}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Payment>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyAmount value_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate date_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(Payment beanToCopy)
		{
		  this.value_Renamed = beanToCopy.Value;
		  this.date_Renamed = beanToCopy.Date;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  return value_Renamed;
			case 3076014: // date
			  return date_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  this.value_Renamed = (CurrencyAmount) newValue;
			  break;
			case 3076014: // date
			  this.date_Renamed = (LocalDate) newValue;
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

		public override Payment build()
		{
		  return new Payment(value_Renamed, date_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the amount of the payment.
		/// <para>
		/// The amount is signed.
		/// A negative value indicates the amount is to be paid while a positive value indicates the amount is received.
		/// </para>
		/// </summary>
		/// <param name="value">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder value(CurrencyAmount value)
		{
		  JodaBeanUtils.notNull(value, "value");
		  this.value_Renamed = value;
		  return this;
		}

		/// <summary>
		/// Sets the date that the payment is made.
		/// <para>
		/// This date should normally be a valid business day.
		/// </para>
		/// </summary>
		/// <param name="date">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder date(LocalDate date)
		{
		  JodaBeanUtils.notNull(date, "date");
		  this.date_Renamed = date;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("Payment.Builder{");
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value_Renamed)).Append(',').Append(' ');
		  buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
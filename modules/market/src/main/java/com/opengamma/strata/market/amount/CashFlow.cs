using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.amount
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

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// A single cash flow of a currency amount on a specific date.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CashFlow implements com.opengamma.strata.basics.currency.FxConvertible<CashFlow>, Comparable<CashFlow>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CashFlow : FxConvertible<CashFlow>, IComparable<CashFlow>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate paymentDate;
		private readonly LocalDate paymentDate;
	  /// <summary>
	  /// The present value of the cash flow.
	  /// <para>
	  /// The present value is signed.
	  /// A negative value indicates a payment while a positive value indicates receipt.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyAmount presentValue;
	  private readonly CurrencyAmount presentValue;
	  /// <summary>
	  /// The forecast value of the cash flow.
	  /// <para>
	  /// The forecast value is signed.
	  /// A negative value indicates a payment while a positive value indicates receipt.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyAmount forecastValue;
	  private readonly CurrencyAmount forecastValue;
	  /// <summary>
	  /// The discount factor.
	  /// <para>
	  /// This is the discount factor between valuation date and the payment date.
	  /// Thus present value is the forecast value multiplied by the discount factor.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double discountFactor;
	  private readonly double discountFactor;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a {@code CashFlow} representing a single cash flow from
	  /// payment date, present value and discount factor.
	  /// </summary>
	  /// <param name="paymentDate">  the payment date </param>
	  /// <param name="presentValue">  the present value as a currency amount </param>
	  /// <param name="discountFactor">  the discount factor </param>
	  /// <returns> the cash flow instance </returns>
	  public static CashFlow ofPresentValue(LocalDate paymentDate, CurrencyAmount presentValue, double discountFactor)
	  {
		return new CashFlow(paymentDate, presentValue, presentValue.multipliedBy(1d / discountFactor), discountFactor);
	  }

	  /// <summary>
	  /// Creates a {@code CashFlow} representing a single cash flow from payment date, present value amount, 
	  /// discount factor and currency.
	  /// </summary>
	  /// <param name="paymentDate">  the payment date </param>
	  /// <param name="currency">  the currency </param>
	  /// <param name="presentValue">  the amount of the present value </param>
	  /// <param name="discountFactor">  the discount factor </param>
	  /// <returns> the cash flow instance </returns>
	  public static CashFlow ofPresentValue(LocalDate paymentDate, Currency currency, double presentValue, double discountFactor)
	  {
		return ofPresentValue(paymentDate, CurrencyAmount.of(currency, presentValue), discountFactor);
	  }

	  /// <summary>
	  /// Creates a {@code CashFlow} representing a single cash flow from
	  /// payment date, forecast value and discount factor.
	  /// </summary>
	  /// <param name="paymentDate">  the payment date </param>
	  /// <param name="forecastValue">  the forecast value as a currency amount </param>
	  /// <param name="discountFactor">  the discount factor </param>
	  /// <returns> the cash flow instance </returns>
	  public static CashFlow ofForecastValue(LocalDate paymentDate, CurrencyAmount forecastValue, double discountFactor)
	  {
		return new CashFlow(paymentDate, forecastValue.multipliedBy(discountFactor), forecastValue, discountFactor);
	  }

	  /// <summary>
	  /// Creates a {@code CashFlow} representing a single cash flow from payment date, forecast value amount,
	  /// discount factor and currency.
	  /// </summary>
	  /// <param name="paymentDate">  the payment date </param>
	  /// <param name="currency">  the currency </param>
	  /// <param name="forecastValue">  the amount of the forecast value </param>
	  /// <param name="discountFactor">  the discount factor </param>
	  /// <returns> the cash flow instance </returns>
	  public static CashFlow ofForecastValue(LocalDate paymentDate, Currency currency, double forecastValue, double discountFactor)
	  {
		return ofForecastValue(paymentDate, CurrencyAmount.of(currency, forecastValue), discountFactor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this cash flow to an equivalent amount in the specified currency.
	  /// <para>
	  /// The result will have both the present and forecast value expressed in terms of the given currency.
	  /// If conversion is needed, the provider will be used to supply the FX rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
	  public CashFlow convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		if (presentValue.Currency.Equals(resultCurrency) && forecastValue.Currency.Equals(resultCurrency))
		{
		  return this;
		}
		CurrencyAmount pv = presentValue.convertedTo(resultCurrency, rateProvider);
		CurrencyAmount fv = forecastValue.convertedTo(resultCurrency, rateProvider);
		return new CashFlow(paymentDate, pv, fv, discountFactor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this cash flow to another, first by date, then value.
	  /// </summary>
	  /// <param name="other">  the other instance </param>
	  /// <returns> the comparison </returns>
	  public int CompareTo(CashFlow other)
	  {
		return ComparisonChain.start().compare(paymentDate, other.paymentDate).compare(presentValue, other.presentValue).compare(forecastValue, other.forecastValue).compare(discountFactor, other.discountFactor).result();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CashFlow}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CashFlow.Meta meta()
	  {
		return CashFlow.Meta.INSTANCE;
	  }

	  static CashFlow()
	  {
		MetaBean.register(CashFlow.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CashFlow(LocalDate paymentDate, CurrencyAmount presentValue, CurrencyAmount forecastValue, double discountFactor)
	  {
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		JodaBeanUtils.notNull(presentValue, "presentValue");
		JodaBeanUtils.notNull(forecastValue, "forecastValue");
		this.paymentDate = paymentDate;
		this.presentValue = presentValue;
		this.forecastValue = forecastValue;
		this.discountFactor = discountFactor;
	  }

	  public override CashFlow.Meta metaBean()
	  {
		return CashFlow.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment date.
	  /// <para>
	  /// This is the date on which the cash flow occurs.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate PaymentDate
	  {
		  get
		  {
			return paymentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the present value of the cash flow.
	  /// <para>
	  /// The present value is signed.
	  /// A negative value indicates a payment while a positive value indicates receipt.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmount PresentValue
	  {
		  get
		  {
			return presentValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the forecast value of the cash flow.
	  /// <para>
	  /// The forecast value is signed.
	  /// A negative value indicates a payment while a positive value indicates receipt.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmount ForecastValue
	  {
		  get
		  {
			return forecastValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount factor.
	  /// <para>
	  /// This is the discount factor between valuation date and the payment date.
	  /// Thus present value is the forecast value multiplied by the discount factor.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double DiscountFactor
	  {
		  get
		  {
			return discountFactor;
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
		  CashFlow other = (CashFlow) obj;
		  return JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(presentValue, other.presentValue) && JodaBeanUtils.equal(forecastValue, other.forecastValue) && JodaBeanUtils.equal(discountFactor, other.discountFactor);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(presentValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(forecastValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountFactor);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("CashFlow{");
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("presentValue").Append('=').Append(presentValue).Append(',').Append(' ');
		buf.Append("forecastValue").Append('=').Append(forecastValue).Append(',').Append(' ');
		buf.Append("discountFactor").Append('=').Append(JodaBeanUtils.ToString(discountFactor));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CashFlow}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(CashFlow), typeof(LocalDate));
			  presentValue_Renamed = DirectMetaProperty.ofImmutable(this, "presentValue", typeof(CashFlow), typeof(CurrencyAmount));
			  forecastValue_Renamed = DirectMetaProperty.ofImmutable(this, "forecastValue", typeof(CashFlow), typeof(CurrencyAmount));
			  discountFactor_Renamed = DirectMetaProperty.ofImmutable(this, "discountFactor", typeof(CashFlow), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "paymentDate", "presentValue", "forecastValue", "discountFactor");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code presentValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> presentValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code forecastValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> forecastValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountFactor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> discountFactor_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "paymentDate", "presentValue", "forecastValue", "discountFactor");
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
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 686253430: // presentValue
			  return presentValue_Renamed;
			case 1310579766: // forecastValue
			  return forecastValue_Renamed;
			case -557144592: // discountFactor
			  return discountFactor_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CashFlow> builder()
		public override BeanBuilder<CashFlow> builder()
		{
		  return new CashFlow.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CashFlow);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code presentValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> presentValue()
		{
		  return presentValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code forecastValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> forecastValue()
		{
		  return forecastValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountFactor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> discountFactor()
		{
		  return discountFactor_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1540873516: // paymentDate
			  return ((CashFlow) bean).PaymentDate;
			case 686253430: // presentValue
			  return ((CashFlow) bean).PresentValue;
			case 1310579766: // forecastValue
			  return ((CashFlow) bean).ForecastValue;
			case -557144592: // discountFactor
			  return ((CashFlow) bean).DiscountFactor;
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
	  /// The bean-builder for {@code CashFlow}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CashFlow>
	  {

		internal LocalDate paymentDate;
		internal CurrencyAmount presentValue;
		internal CurrencyAmount forecastValue;
		internal double discountFactor;

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
			case -1540873516: // paymentDate
			  return paymentDate;
			case 686253430: // presentValue
			  return presentValue;
			case 1310579766: // forecastValue
			  return forecastValue;
			case -557144592: // discountFactor
			  return discountFactor;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1540873516: // paymentDate
			  this.paymentDate = (LocalDate) newValue;
			  break;
			case 686253430: // presentValue
			  this.presentValue = (CurrencyAmount) newValue;
			  break;
			case 1310579766: // forecastValue
			  this.forecastValue = (CurrencyAmount) newValue;
			  break;
			case -557144592: // discountFactor
			  this.discountFactor = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CashFlow build()
		{
		  return new CashFlow(paymentDate, presentValue, forecastValue, discountFactor);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("CashFlow.Builder{");
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate)).Append(',').Append(' ');
		  buf.Append("presentValue").Append('=').Append(JodaBeanUtils.ToString(presentValue)).Append(',').Append(' ');
		  buf.Append("forecastValue").Append('=').Append(JodaBeanUtils.ToString(forecastValue)).Append(',').Append(' ');
		  buf.Append("discountFactor").Append('=').Append(JodaBeanUtils.ToString(discountFactor));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
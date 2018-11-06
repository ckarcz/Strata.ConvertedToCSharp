using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{

	using BeanBuilder = org.joda.beans.BeanBuilder;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BufferingBeanBuilder = org.joda.beans.impl.BufferingBeanBuilder;
	using StandaloneMetaProperty = org.joda.beans.impl.StandaloneMetaProperty;
	using DefaultDeserializer = org.joda.beans.ser.DefaultDeserializer;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Deserialize {@code FxSingle} handling old format.
	/// </summary>
	internal sealed class FxSingleDeserializer : DefaultDeserializer
	{

	  private static readonly MetaProperty<Payment> BASE_CURRENCY_PAYMENT = FxSingle.meta().baseCurrencyPayment();
	  private static readonly MetaProperty<Payment> COUNTER_CURRENCY_PAYMENT = FxSingle.meta().counterCurrencyPayment();
	  private static readonly MetaProperty<BusinessDayAdjustment> PAYMENT_ADJUSTMENT_DATE = FxSingle.meta().paymentDateAdjustment();
	  private static readonly MetaProperty<CurrencyAmount> BASE_CURRENCY_AMOUNT = StandaloneMetaProperty.of("baseCurrencyAmount", FxSingle.meta(), typeof(CurrencyAmount));
	  private static readonly MetaProperty<CurrencyAmount> COUNTER_CURRENCY_AMOUNT = StandaloneMetaProperty.of("counterCurrencyAmount", FxSingle.meta(), typeof(CurrencyAmount));
	  private static readonly MetaProperty<LocalDate> PAYMENT_DATE = StandaloneMetaProperty.of("paymentDate", FxSingle.meta(), typeof(LocalDate));

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  internal FxSingleDeserializer()
	  {
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<?> createBuilder(Class beanType, org.joda.beans.MetaBean metaBean)
	  public override BeanBuilder<object> createBuilder(Type beanType, MetaBean metaBean)
	  {
		return BufferingBeanBuilder.of(metaBean);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.MetaProperty<?> findMetaProperty(Class beanType, org.joda.beans.MetaBean metaBean, String propertyName)
	  public override MetaProperty<object> findMetaProperty(Type beanType, MetaBean metaBean, string propertyName)
	  {
		try
		{
		  return metaBean.metaProperty(propertyName);
		}
		catch (NoSuchElementException ex)
		{
		  if (BASE_CURRENCY_AMOUNT.name().Equals(propertyName))
		  {
			return BASE_CURRENCY_AMOUNT;
		  }
		  if (COUNTER_CURRENCY_AMOUNT.name().Equals(propertyName))
		  {
			return COUNTER_CURRENCY_AMOUNT;
		  }
		  if (PAYMENT_DATE.name().Equals(propertyName))
		  {
			return PAYMENT_DATE;
		  }
		  throw ex;
		}
	  }

	  public override object build<T1>(Type beanType, BeanBuilder<T1> builder)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.impl.BufferingBeanBuilder<?> bld = (org.joda.beans.impl.BufferingBeanBuilder<?>) builder;
		BufferingBeanBuilder<object> bld = (BufferingBeanBuilder<object>) builder;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.concurrent.ConcurrentMap<org.joda.beans.MetaProperty<?>, Object> buffer = bld.getBuffer();
		ConcurrentMap<MetaProperty<object>, object> buffer = bld.Buffer;
		BusinessDayAdjustment bda = (BusinessDayAdjustment) buffer.getOrDefault(PAYMENT_ADJUSTMENT_DATE, null);
		if (buffer.containsKey(BASE_CURRENCY_AMOUNT) && buffer.containsKey(COUNTER_CURRENCY_AMOUNT) && buffer.containsKey(PAYMENT_DATE))
		{

		  CurrencyAmount baseAmount = (CurrencyAmount) builder.get(BASE_CURRENCY_AMOUNT);
		  CurrencyAmount counterAmount = (CurrencyAmount) builder.get(COUNTER_CURRENCY_AMOUNT);
		  LocalDate paymentDate = (LocalDate) builder.get(PAYMENT_DATE);
		  return bda != null ? FxSingle.of(baseAmount, counterAmount, paymentDate, bda) : FxSingle.of(baseAmount, counterAmount, paymentDate);

		}
		else
		{
		  Payment basePayment = (Payment) buffer.get(BASE_CURRENCY_PAYMENT);
		  Payment counterPayment = (Payment) buffer.get(COUNTER_CURRENCY_PAYMENT);
		  return bda != null ? FxSingle.of(basePayment, counterPayment, bda) : FxSingle.of(basePayment, counterPayment);
		}
	  }

	}

}
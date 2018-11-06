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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.IBOR;


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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using FxIndices = com.opengamma.strata.basics.index.FxIndices;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;

	/// <summary>
	/// Mock.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class MockSwapLeg implements SwapLeg, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class MockSwapLeg : SwapLeg, ImmutableBean
	{

	  public static readonly SwapLeg MOCK_GBP1 = MockSwapLeg.of(FIXED, PAY, date(2012, 1, 15), date(2012, 8, 15), GBP);
	  public static readonly ResolvedSwapLeg MOCK_EXPANDED_GBP1 = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(RatePaymentPeriod.builder().paymentDate(date(2012, 8, 15)).accrualPeriods(RateAccrualPeriod.builder().startDate(date(2012, 1, 15)).endDate(date(2012, 8, 15)).rateComputation(FixedRateComputation.of(0.012d)).build()).dayCount(ACT_365F).notional(1_000_000d).currency(GBP).build()).build();
	  public static readonly SwapLeg MOCK_GBP2 = MockSwapLeg.of(FIXED, PAY, date(2012, 1, 15), date(2012, 6, 15), GBP);
	  public static readonly SwapLeg MOCK_USD1 = MockSwapLeg.of(IBOR, RECEIVE, date(2012, 1, 15), date(2012, 8, 15), USD);
	  public static readonly ResolvedSwapLeg MOCK_EXPANDED_USD1 = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RatePaymentPeriod.builder().paymentDate(date(2012, 8, 15)).accrualPeriods(RateAccrualPeriod.builder().startDate(date(2012, 1, 15)).endDate(date(2012, 8, 15)).rateComputation(FixedRateComputation.of(0.012d)).build()).dayCount(ACT_365F).notional(1_000_000d).currency(USD).build()).build();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final SwapLegType type;
	  private readonly SwapLegType type;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final com.opengamma.strata.product.common.PayReceive payReceive;
	  private readonly PayReceive payReceive;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final com.opengamma.strata.basics.date.AdjustableDate startDate;
	  private readonly AdjustableDate startDate;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final com.opengamma.strata.basics.date.AdjustableDate endDate;
	  private readonly AdjustableDate endDate;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;

	  public static MockSwapLeg of(SwapLegType type, PayReceive payReceive, LocalDate startDate, LocalDate endDate, Currency currency)
	  {
		return new MockSwapLeg(type, payReceive, AdjustableDate.of(startDate), AdjustableDate.of(endDate), currency);
	  }

	  public void collectCurrencies(ImmutableSet.Builder<Currency> builder)
	  {
		if (this == MOCK_USD1)
		{
		  builder.add(GBP, EUR, USD);
		}
		else
		{
		  builder.add(GBP);
		}
	  }

	  public void collectIndices(ImmutableSet.Builder<Index> builder)
	  {
		builder.add(IborIndices.GBP_LIBOR_3M, FxIndices.EUR_GBP_ECB, OvernightIndices.EUR_EONIA);
	  }

	  public ResolvedSwapLeg resolve(ReferenceData refData)
	  {
		if (this == MOCK_GBP1)
		{
		  return MOCK_EXPANDED_GBP1;
		}
		if (this == MOCK_USD1)
		{
		  return MOCK_EXPANDED_USD1;
		}
		throw new System.NotSupportedException();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MockSwapLeg}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MockSwapLeg.Meta meta()
	  {
		return MockSwapLeg.Meta.INSTANCE;
	  }

	  static MockSwapLeg()
	  {
		MetaBean.register(MockSwapLeg.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static MockSwapLeg.Builder builder()
	  {
		return new MockSwapLeg.Builder();
	  }

	  private MockSwapLeg(SwapLegType type, PayReceive payReceive, AdjustableDate startDate, AdjustableDate endDate, Currency currency)
	  {
		this.type = type;
		this.payReceive = payReceive;
		this.startDate = startDate;
		this.endDate = endDate;
		this.currency = currency;
	  }

	  public override MockSwapLeg.Meta metaBean()
	  {
		return MockSwapLeg.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type. </summary>
	  /// <returns> the value of the property </returns>
	  public SwapLegType Type
	  {
		  get
		  {
			return type;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payReceive. </summary>
	  /// <returns> the value of the property </returns>
	  public PayReceive PayReceive
	  {
		  get
		  {
			return payReceive;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the startDate. </summary>
	  /// <returns> the value of the property </returns>
	  public AdjustableDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the endDate. </summary>
	  /// <returns> the value of the property </returns>
	  public AdjustableDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency. </summary>
	  /// <returns> the value of the property </returns>
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
		  MockSwapLeg other = (MockSwapLeg) obj;
		  return JodaBeanUtils.equal(type, other.type) && JodaBeanUtils.equal(payReceive, other.payReceive) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(currency, other.currency);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(type);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payReceive);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("MockSwapLeg{");
		buf.Append("type").Append('=').Append(type).Append(',').Append(' ');
		buf.Append("payReceive").Append('=').Append(payReceive).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code MockSwapLeg}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  type_Renamed = DirectMetaProperty.ofImmutable(this, "type", typeof(MockSwapLeg), typeof(SwapLegType));
			  payReceive_Renamed = DirectMetaProperty.ofImmutable(this, "payReceive", typeof(MockSwapLeg), typeof(PayReceive));
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(MockSwapLeg), typeof(AdjustableDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(MockSwapLeg), typeof(AdjustableDate));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(MockSwapLeg), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "type", "payReceive", "startDate", "endDate", "currency");
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
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "type", "payReceive", "startDate", "endDate", "currency");
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
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override MockSwapLeg.Builder builder()
		{
		  return new MockSwapLeg.Builder();
		}

		public override Type beanType()
		{
		  return typeof(MockSwapLeg);
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

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3575610: // type
			  return ((MockSwapLeg) bean).Type;
			case -885469925: // payReceive
			  return ((MockSwapLeg) bean).PayReceive;
			case -2129778896: // startDate
			  return ((MockSwapLeg) bean).StartDate;
			case -1607727319: // endDate
			  return ((MockSwapLeg) bean).EndDate;
			case 575402001: // currency
			  return ((MockSwapLeg) bean).Currency;
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
	  /// The bean-builder for {@code MockSwapLeg}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<MockSwapLeg>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwapLegType type_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PayReceive payReceive_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal AdjustableDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal AdjustableDate endDate_Renamed;
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
		internal Builder(MockSwapLeg beanToCopy)
		{
		  this.type_Renamed = beanToCopy.Type;
		  this.payReceive_Renamed = beanToCopy.PayReceive;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.currency_Renamed = beanToCopy.Currency;
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
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
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
			case 3575610: // type
			  this.type_Renamed = (SwapLegType) newValue;
			  break;
			case -885469925: // payReceive
			  this.payReceive_Renamed = (PayReceive) newValue;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (AdjustableDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (AdjustableDate) newValue;
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

		public override MockSwapLeg build()
		{
		  return new MockSwapLeg(type_Renamed, payReceive_Renamed, startDate_Renamed, endDate_Renamed, currency_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the type. </summary>
		/// <param name="type">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder type(SwapLegType type)
		{
		  this.type_Renamed = type;
		  return this;
		}

		/// <summary>
		/// Sets the payReceive. </summary>
		/// <param name="payReceive">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder payReceive(PayReceive payReceive)
		{
		  this.payReceive_Renamed = payReceive;
		  return this;
		}

		/// <summary>
		/// Sets the startDate. </summary>
		/// <param name="startDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(AdjustableDate startDate)
		{
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the endDate. </summary>
		/// <param name="endDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(AdjustableDate endDate)
		{
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the currency. </summary>
		/// <param name="currency">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  this.currency_Renamed = currency;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("MockSwapLeg.Builder{");
		  buf.Append("type").Append('=').Append(JodaBeanUtils.ToString(type_Renamed)).Append(',').Append(' ');
		  buf.Append("payReceive").Append('=').Append(JodaBeanUtils.ToString(payReceive_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
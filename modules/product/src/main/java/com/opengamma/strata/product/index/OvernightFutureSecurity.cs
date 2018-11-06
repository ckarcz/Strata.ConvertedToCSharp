using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// A security representing a futures contract based on an Overnight rate index.
	/// <para>
	/// An Overnight rate future is a financial instrument that is based on the future value of
	/// an Overnight index interest rate. The profit or loss of an Overnight rate future is settled daily.
	/// This class represents the structure of a single futures contract.
	/// </para>
	/// <para>
	/// For example, the widely traded "30-Day Federal Funds futures contract" has a notional
	/// of 5 million USD, is based on the US Federal Funds Effective Rate 'USD-FED-FUND',
	/// expiring the last business day of each month.
	/// 
	/// <h4>Price</h4>
	/// The price of an Overnight rate future is based on the interest rate of the underlying index.
	/// It is defined as {@code (100 - percentRate)}.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for Overnight rate futures in the trade model, pricers and market data.
	/// The decimal price is based on the decimal rate equivalent to the percentage.
	/// For example, a price of 99.32 implies an interest rate of 0.68% which is represented in Strata by 0.9932.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class OvernightFutureSecurity implements RateIndexSecurity, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class OvernightFutureSecurity : RateIndexSecurity, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityInfo info;
		private readonly SecurityInfo info;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// This is the full notional of the deposit, such as 5 million dollars.
	  /// The notional expressed here must be positive.
	  /// The currency of the notional the same as the currency of the index.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The accrual factor, defaulted from the index if not set.
	  /// <para>
	  /// This is the year fraction of the contract, typically 1/12 for a 30-day future.
	  /// As such, it is often unrelated to the day count of the index.
	  /// The year fraction must be positive.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double accrualFactor;
	  private readonly double accrualFactor;
	  /// <summary>
	  /// The last date of trading.
	  /// <para>
	  /// This must be a valid business day on the fixing calendar of {@code index}.
	  /// For example, the last trade date is often the last business day of the month.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastTradeDate;
	  private readonly LocalDate lastTradeDate;
	  /// <summary>
	  /// The first date of the rate calculation period.
	  /// <para>
	  /// This is not necessarily a valid business day on the fixing calendar of {@code index}.
	  /// However, it will be adjusted in {@code OvernightRateComputation} if needed.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The last date of the rate calculation period. 
	  /// <para>
	  /// This is not necessarily a valid business day on the fixing calendar of {@code index}.
	  /// However, it will be adjusted in {@code OvernightRateComputation} if needed.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The underlying Overnight index.
	  /// <para>
	  /// The future is based on this index.
	  /// It will be a well known market index such as 'USD-FED-FUND'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.OvernightIndex index;
	  private readonly OvernightIndex index;
	  /// <summary>
	  /// The method of accruing Overnight interest.
	  /// <para>
	  /// The average rate is calculated based on this method over the period between {@code startDate} and {@code endDate}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.OvernightAccrualMethod accrualMethod;
	  private readonly OvernightAccrualMethod accrualMethod;
	  /// <summary>
	  /// The definition of how to round the futures price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 for a 0.2875% rate is
	  /// represented as 0.997125 which has 6 decimal places.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.Rounding rounding;
	  private readonly Rounding rounding;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.currency.Currency getCurrency()
	  public override Currency Currency
	  {
		  get
		  {
			return index.Currency;
		  }
	  }

	  public ImmutableSet<SecurityId> UnderlyingIds
	  {
		  get
		  {
			return ImmutableSet.of();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public OvernightFutureSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  //-------------------------------------------------------------------------
	  public OvernightFuture createProduct(ReferenceData refData)
	  {
		return OvernightFuture.builder().securityId(SecurityId).notional(notional).accrualFactor(accrualFactor).index(index).accrualMethod(accrualMethod).lastTradeDate(lastTradeDate).startDate(startDate).endDate(endDate).rounding(rounding).build();
	  }

	  public OvernightFutureTrade createTrade(TradeInfo info, double quantity, double tradePrice, ReferenceData refData)
	  {

		return new OvernightFutureTrade(info, createProduct(refData), quantity, tradePrice);
	  }

	  public OvernightFuturePosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return OvernightFuturePosition.ofNet(positionInfo, createProduct(refData), quantity);
	  }

	  public OvernightFuturePosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return OvernightFuturePosition.ofLongShort(positionInfo, createProduct(refData), longQuantity, shortQuantity);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightFutureSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static OvernightFutureSecurity.Meta meta()
	  {
		return OvernightFutureSecurity.Meta.INSTANCE;
	  }

	  static OvernightFutureSecurity()
	  {
		MetaBean.register(OvernightFutureSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static OvernightFutureSecurity.Builder builder()
	  {
		return new OvernightFutureSecurity.Builder();
	  }

	  private OvernightFutureSecurity(SecurityInfo info, double notional, double accrualFactor, LocalDate lastTradeDate, LocalDate startDate, LocalDate endDate, OvernightIndex index, OvernightAccrualMethod accrualMethod, Rounding rounding)
	  {
		JodaBeanUtils.notNull(info, "info");
		ArgChecker.notNegativeOrZero(notional, "notional");
		ArgChecker.notNegativeOrZero(accrualFactor, "accrualFactor");
		JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(accrualMethod, "accrualMethod");
		JodaBeanUtils.notNull(rounding, "rounding");
		this.info = info;
		this.notional = notional;
		this.accrualFactor = accrualFactor;
		this.lastTradeDate = lastTradeDate;
		this.startDate = startDate;
		this.endDate = endDate;
		this.index = index;
		this.accrualMethod = accrualMethod;
		this.rounding = rounding;
	  }

	  public override OvernightFutureSecurity.Meta metaBean()
	  {
		return OvernightFutureSecurity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the standard security information.
	  /// <para>
	  /// This includes the security identifier.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount.
	  /// <para>
	  /// This is the full notional of the deposit, such as 5 million dollars.
	  /// The notional expressed here must be positive.
	  /// The currency of the notional the same as the currency of the index.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual factor, defaulted from the index if not set.
	  /// <para>
	  /// This is the year fraction of the contract, typically 1/12 for a 30-day future.
	  /// As such, it is often unrelated to the day count of the index.
	  /// The year fraction must be positive.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double AccrualFactor
	  {
		  get
		  {
			return accrualFactor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last date of trading.
	  /// <para>
	  /// This must be a valid business day on the fixing calendar of {@code index}.
	  /// For example, the last trade date is often the last business day of the month.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate LastTradeDate
	  {
		  get
		  {
			return lastTradeDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first date of the rate calculation period.
	  /// <para>
	  /// This is not necessarily a valid business day on the fixing calendar of {@code index}.
	  /// However, it will be adjusted in {@code OvernightRateComputation} if needed.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last date of the rate calculation period.
	  /// <para>
	  /// This is not necessarily a valid business day on the fixing calendar of {@code index}.
	  /// However, it will be adjusted in {@code OvernightRateComputation} if needed.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying Overnight index.
	  /// <para>
	  /// The future is based on this index.
	  /// It will be a well known market index such as 'USD-FED-FUND'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the method of accruing Overnight interest.
	  /// <para>
	  /// The average rate is calculated based on this method over the period between {@code startDate} and {@code endDate}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightAccrualMethod AccrualMethod
	  {
		  get
		  {
			return accrualMethod;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the definition of how to round the futures price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 for a 0.2875% rate is
	  /// represented as 0.997125 which has 6 decimal places.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Rounding Rounding
	  {
		  get
		  {
			return rounding;
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
		  OvernightFutureSecurity other = (OvernightFutureSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(accrualFactor, other.accrualFactor) && JodaBeanUtils.equal(lastTradeDate, other.lastTradeDate) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(accrualMethod, other.accrualMethod) && JodaBeanUtils.equal(rounding, other.rounding);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualFactor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastTradeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(accrualMethod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rounding);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("OvernightFutureSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("accrualFactor").Append('=').Append(accrualFactor).Append(',').Append(' ');
		buf.Append("lastTradeDate").Append('=').Append(lastTradeDate).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("accrualMethod").Append('=').Append(accrualMethod).Append(',').Append(' ');
		buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightFutureSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(OvernightFutureSecurity), typeof(SecurityInfo));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(OvernightFutureSecurity), Double.TYPE);
			  accrualFactor_Renamed = DirectMetaProperty.ofImmutable(this, "accrualFactor", typeof(OvernightFutureSecurity), Double.TYPE);
			  lastTradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastTradeDate", typeof(OvernightFutureSecurity), typeof(LocalDate));
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(OvernightFutureSecurity), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(OvernightFutureSecurity), typeof(LocalDate));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(OvernightFutureSecurity), typeof(OvernightIndex));
			  accrualMethod_Renamed = DirectMetaProperty.ofImmutable(this, "accrualMethod", typeof(OvernightFutureSecurity), typeof(OvernightAccrualMethod));
			  rounding_Renamed = DirectMetaProperty.ofImmutable(this, "rounding", typeof(OvernightFutureSecurity), typeof(Rounding));
			  currency_Renamed = DirectMetaProperty.ofDerived(this, "currency", typeof(OvernightFutureSecurity), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "notional", "accrualFactor", "lastTradeDate", "startDate", "endDate", "index", "accrualMethod", "rounding", "currency");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualFactor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> accrualFactor_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastTradeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastTradeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code accrualMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightAccrualMethod> accrualMethod_Renamed;
		/// <summary>
		/// The meta-property for the {@code rounding} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Rounding> rounding_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "notional", "accrualFactor", "lastTradeDate", "startDate", "endDate", "index", "accrualMethod", "rounding", "currency");
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
			case 3237038: // info
			  return info_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1540322338: // accrualFactor
			  return accrualFactor_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1335729296: // accrualMethod
			  return accrualMethod_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override OvernightFutureSecurity.Builder builder()
		{
		  return new OvernightFutureSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(OvernightFutureSecurity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualFactor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> accrualFactor()
		{
		  return accrualFactor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastTradeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastTradeDate()
		{
		  return lastTradeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code accrualMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightAccrualMethod> accrualMethod()
		{
		  return accrualMethod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rounding} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Rounding> rounding()
		{
		  return rounding_Renamed;
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
			case 3237038: // info
			  return ((OvernightFutureSecurity) bean).Info;
			case 1585636160: // notional
			  return ((OvernightFutureSecurity) bean).Notional;
			case -1540322338: // accrualFactor
			  return ((OvernightFutureSecurity) bean).AccrualFactor;
			case -1041950404: // lastTradeDate
			  return ((OvernightFutureSecurity) bean).LastTradeDate;
			case -2129778896: // startDate
			  return ((OvernightFutureSecurity) bean).StartDate;
			case -1607727319: // endDate
			  return ((OvernightFutureSecurity) bean).EndDate;
			case 100346066: // index
			  return ((OvernightFutureSecurity) bean).Index;
			case -1335729296: // accrualMethod
			  return ((OvernightFutureSecurity) bean).AccrualMethod;
			case -142444: // rounding
			  return ((OvernightFutureSecurity) bean).Rounding;
			case 575402001: // currency
			  return ((OvernightFutureSecurity) bean).Currency;
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
	  /// The bean-builder for {@code OvernightFutureSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<OvernightFutureSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double accrualFactor_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastTradeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightAccrualMethod accrualMethod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Rounding rounding_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(OvernightFutureSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.accrualFactor_Renamed = beanToCopy.AccrualFactor;
		  this.lastTradeDate_Renamed = beanToCopy.LastTradeDate;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.index_Renamed = beanToCopy.Index;
		  this.accrualMethod_Renamed = beanToCopy.AccrualMethod;
		  this.rounding_Renamed = beanToCopy.Rounding;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1540322338: // accrualFactor
			  return accrualFactor_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1335729296: // accrualMethod
			  return accrualMethod_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  this.info_Renamed = (SecurityInfo) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -1540322338: // accrualFactor
			  this.accrualFactor_Renamed = (double?) newValue.Value;
			  break;
			case -1041950404: // lastTradeDate
			  this.lastTradeDate_Renamed = (LocalDate) newValue;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (OvernightIndex) newValue;
			  break;
			case -1335729296: // accrualMethod
			  this.accrualMethod_Renamed = (OvernightAccrualMethod) newValue;
			  break;
			case -142444: // rounding
			  this.rounding_Renamed = (Rounding) newValue;
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

		public override OvernightFutureSecurity build()
		{
		  return new OvernightFutureSecurity(info_Renamed, notional_Renamed, accrualFactor_Renamed, lastTradeDate_Renamed, startDate_Renamed, endDate_Renamed, index_Renamed, accrualMethod_Renamed, rounding_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the standard security information.
		/// <para>
		/// This includes the security identifier.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(SecurityInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// This is the full notional of the deposit, such as 5 million dollars.
		/// The notional expressed here must be positive.
		/// The currency of the notional the same as the currency of the index.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  ArgChecker.notNegativeOrZero(notional, "notional");
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the accrual factor, defaulted from the index if not set.
		/// <para>
		/// This is the year fraction of the contract, typically 1/12 for a 30-day future.
		/// As such, it is often unrelated to the day count of the index.
		/// The year fraction must be positive.
		/// </para>
		/// </summary>
		/// <param name="accrualFactor">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualFactor(double accrualFactor)
		{
		  ArgChecker.notNegativeOrZero(accrualFactor, "accrualFactor");
		  this.accrualFactor_Renamed = accrualFactor;
		  return this;
		}

		/// <summary>
		/// Sets the last date of trading.
		/// <para>
		/// This must be a valid business day on the fixing calendar of {@code index}.
		/// For example, the last trade date is often the last business day of the month.
		/// </para>
		/// </summary>
		/// <param name="lastTradeDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastTradeDate(LocalDate lastTradeDate)
		{
		  JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		  this.lastTradeDate_Renamed = lastTradeDate;
		  return this;
		}

		/// <summary>
		/// Sets the first date of the rate calculation period.
		/// <para>
		/// This is not necessarily a valid business day on the fixing calendar of {@code index}.
		/// However, it will be adjusted in {@code OvernightRateComputation} if needed.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the last date of the rate calculation period.
		/// <para>
		/// This is not necessarily a valid business day on the fixing calendar of {@code index}.
		/// However, it will be adjusted in {@code OvernightRateComputation} if needed.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the underlying Overnight index.
		/// <para>
		/// The future is based on this index.
		/// It will be a well known market index such as 'USD-FED-FUND'.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(OvernightIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the method of accruing Overnight interest.
		/// <para>
		/// The average rate is calculated based on this method over the period between {@code startDate} and {@code endDate}.
		/// </para>
		/// </summary>
		/// <param name="accrualMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder accrualMethod(OvernightAccrualMethod accrualMethod)
		{
		  JodaBeanUtils.notNull(accrualMethod, "accrualMethod");
		  this.accrualMethod_Renamed = accrualMethod;
		  return this;
		}

		/// <summary>
		/// Sets the definition of how to round the futures price, defaulted to no rounding.
		/// <para>
		/// The price is represented in decimal form, not percentage form.
		/// As such, the decimal places expressed by the rounding refers to this decimal form.
		/// For example, the common market price of 99.7125 for a 0.2875% rate is
		/// represented as 0.997125 which has 6 decimal places.
		/// </para>
		/// </summary>
		/// <param name="rounding">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rounding(Rounding rounding)
		{
		  JodaBeanUtils.notNull(rounding, "rounding");
		  this.rounding_Renamed = rounding;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("OvernightFutureSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualFactor").Append('=').Append(JodaBeanUtils.ToString(accrualFactor_Renamed)).Append(',').Append(' ');
		  buf.Append("lastTradeDate").Append('=').Append(JodaBeanUtils.ToString(lastTradeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("accrualMethod").Append('=').Append(JodaBeanUtils.ToString(accrualMethod_Renamed)).Append(',').Append(' ');
		  buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
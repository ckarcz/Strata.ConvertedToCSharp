using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;


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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for FX swap trades
	/// <para>
	/// This defines the market convention for a FX swap based on a particular currency pair.
	/// </para>
	/// <para>
	/// The convention is defined by four dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Spot date, the base for date calculations, typically 2 business days after the trade date
	/// <li>Near date, the date on which the near leg of the swap is exchanged, typically equal to the spot date
	/// <li>Far date, the date on which the far leg of the swap is exchanged, typically a number of months or years after the spot date
	/// </ul>
	/// The period between the spot date and the start/end date is specified by <seealso cref="FxSwapTemplate"/>, not by this convention.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableFxSwapConvention implements FxSwapConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableFxSwapConvention : FxSwapConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
		private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The convention name, such as 'EUR/USD', optional with defaulting getter.
	  /// <para>
	  /// This will default to the name of the currency pair if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final String name;
	  private readonly string name;
	  /// <summary>
	  /// The offset of the spot value date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date and is typically plus 2 business days
	  /// in the joint calendar of the two currencies.
	  /// The start and end date of the FX swap are relative to the spot date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment spotDateOffset;
	  private readonly DaysAdjustment spotDateOffset;
	  /// <summary>
	  /// The business day adjustment to apply to the start and end date, optional with defaulting getter.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the spot date offset calendar if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified currency pair and spot date offset.
	  /// <para>
	  /// Use the <seealso cref="#builder() builder"/> for unusual conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair associated to the convention </param>
	  /// <param name="spotDateOffset">  the spot date offset </param>
	  /// <returns> the convention </returns>
	  public static ImmutableFxSwapConvention of(CurrencyPair currencyPair, DaysAdjustment spotDateOffset)
	  {
		return ImmutableFxSwapConvention.builder().currencyPair(currencyPair).spotDateOffset(spotDateOffset).build();
	  }

	  /// <summary>
	  /// Obtains a convention based on the specified currency pair, spot date offset and adjustment.
	  /// <para>
	  /// Use the <seealso cref="#builder() builder"/> for unusual conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair associated to the convention </param>
	  /// <param name="spotDateOffset">  the spot date offset </param>
	  /// <param name="businessDayAdjustment">  the business day adjustment to apply </param>
	  /// <returns> the convention </returns>
	  public static ImmutableFxSwapConvention of(CurrencyPair currencyPair, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment)
	  {

		ArgChecker.notNull(businessDayAdjustment, "businessDayAdjustment");
		return ImmutableFxSwapConvention.builder().currencyPair(currencyPair).spotDateOffset(spotDateOffset).businessDayAdjustment(businessDayAdjustment).build();
	  }

	  public string Name
	  {
		  get
		  {
			return !string.ReferenceEquals(name, null) ? name : currencyPair.ToString();
		  }
	  }

	  /// <summary>
	  /// Gets the business day adjustment to apply to the start and end date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the spot date offset calendar if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the business day adjustment, not null </returns>
	  public BusinessDayAdjustment BusinessDayAdjustment
	  {
		  get
		  {
			return businessDayAdjustment != null ? businessDayAdjustment : BusinessDayAdjustment.of(MODIFIED_FOLLOWING, spotDateOffset.Calendar);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public FxSwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double nearFxRate, double farLegForwardPoints)
	  {

		Optional<LocalDate> tradeDate = tradeInfo.TradeDate;
		if (tradeDate.Present)
		{
		  ArgChecker.inOrderOrEqual(tradeDate.get(), startDate, "tradeDate", "startDate");
		}
		double amount1 = BuySell.BUY.normalize(notional);
		return FxSwapTrade.builder().info(tradeInfo).product(FxSwap.ofForwardPoints(CurrencyAmount.of(currencyPair.Base, amount1), FxRate.of(currencyPair, nearFxRate), farLegForwardPoints, startDate, endDate, BusinessDayAdjustment)).build();
	  }

	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFxSwapConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableFxSwapConvention.Meta meta()
	  {
		return ImmutableFxSwapConvention.Meta.INSTANCE;
	  }

	  static ImmutableFxSwapConvention()
	  {
		MetaBean.register(ImmutableFxSwapConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableFxSwapConvention.Builder builder()
	  {
		return new ImmutableFxSwapConvention.Builder();
	  }

	  private ImmutableFxSwapConvention(CurrencyPair currencyPair, string name, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment)
	  {
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		this.currencyPair = currencyPair;
		this.name = name;
		this.spotDateOffset = spotDateOffset;
		this.businessDayAdjustment = businessDayAdjustment;
	  }

	  public override ImmutableFxSwapConvention.Meta metaBean()
	  {
		return ImmutableFxSwapConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair associated with the convention. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the spot value date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date and is typically plus 2 business days
	  /// in the joint calendar of the two currencies.
	  /// The start and end date of the FX swap are relative to the spot date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment SpotDateOffset
	  {
		  get
		  {
			return spotDateOffset;
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
		  ImmutableFxSwapConvention other = (ImmutableFxSwapConvention) obj;
		  return JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFxSwapConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(ImmutableFxSwapConvention), typeof(CurrencyPair));
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableFxSwapConvention), typeof(string));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(ImmutableFxSwapConvention), typeof(DaysAdjustment));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(ImmutableFxSwapConvention), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currencyPair", "name", "spotDateOffset", "businessDayAdjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currencyPair", "name", "spotDateOffset", "businessDayAdjustment");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 3373707: // name
			  return name_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableFxSwapConvention.Builder builder()
		{
		  return new ImmutableFxSwapConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableFxSwapConvention);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> spotDateOffset()
		{
		  return spotDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return ((ImmutableFxSwapConvention) bean).CurrencyPair;
			case 3373707: // name
			  return ((ImmutableFxSwapConvention) bean).name;
			case 746995843: // spotDateOffset
			  return ((ImmutableFxSwapConvention) bean).SpotDateOffset;
			case -1065319863: // businessDayAdjustment
			  return ((ImmutableFxSwapConvention) bean).businessDayAdjustment;
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
	  /// The bean-builder for {@code ImmutableFxSwapConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableFxSwapConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment spotDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableFxSwapConvention beanToCopy)
		{
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.name_Renamed = beanToCopy.name;
		  this.spotDateOffset_Renamed = beanToCopy.SpotDateOffset;
		  this.businessDayAdjustment_Renamed = beanToCopy.businessDayAdjustment;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 3373707: // name
			  return name_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case 746995843: // spotDateOffset
			  this.spotDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
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

		public override ImmutableFxSwapConvention build()
		{
		  return new ImmutableFxSwapConvention(currencyPair_Renamed, name_Renamed, spotDateOffset_Renamed, businessDayAdjustment_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the currency pair associated with the convention. </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
		  return this;
		}

		/// <summary>
		/// Sets the convention name, such as 'EUR/USD', optional with defaulting getter.
		/// <para>
		/// This will default to the name of the currency pair if not specified.
		/// </para>
		/// </summary>
		/// <param name="name">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the spot value date from the trade date.
		/// <para>
		/// The offset is applied to the trade date and is typically plus 2 business days
		/// in the joint calendar of the two currencies.
		/// The start and end date of the FX swap are relative to the spot date.
		/// </para>
		/// </summary>
		/// <param name="spotDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spotDateOffset(DaysAdjustment spotDateOffset)
		{
		  JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		  this.spotDateOffset_Renamed = spotDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the start and end date, optional with defaulting getter.
		/// <para>
		/// The start and end date are typically defined as valid business days and thus
		/// do not need to be adjusted. If this optional property is present, then the
		/// start and end date will be adjusted as defined here.
		/// </para>
		/// <para>
		/// This will default to 'ModifiedFollowing' using the spot date offset calendar if not specified.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableFxSwapConvention.Builder{");
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
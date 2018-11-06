using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.dsf
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	/// A security representing a deliverable swap futures security.
	/// <para>
	/// A deliverable swap future is a financial instrument that physically settles
	/// an interest rate swap on a future date.
	/// The delivered swap is cleared by a central counterparty.
	/// The last future price before delivery is quoted in term of the underlying swap present value.
	/// The futures product is margined on a daily basis.
	/// 
	/// <h4>Price</h4>
	/// The price of a DSF is based on the present value (NPV) of the underlying swap on the delivery date.
	/// For example, a price of 100.182 represents a present value of $100,182.00, if the notional is $100,000.
	/// This price can also be viewed as a percentage present value - {@code (100 + percentPv)}, or 0.182% in this example.
	/// </para>
	/// <para>
	/// Strata uses <i>decimal prices</i> for DSFs in the trade model, pricers and market data.
	/// The decimal price is based on the decimal multiplier equivalent to the implied percentage.
	/// Thus the market price of 100.182 is represented in Strata by 1.00182.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class DsfSecurity implements com.opengamma.strata.product.Security, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DsfSecurity : Security, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityInfo info;
		private readonly SecurityInfo info;
	  /// <summary>
	  /// The notional.
	  /// <para>
	  /// This is also called face value or contract value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The last date of trading.
	  /// <para>
	  /// This date must be before the delivery date of the underlying swap.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastTradeDate;
	  private readonly LocalDate lastTradeDate;
	  /// <summary>
	  /// The underlying swap.
	  /// <para>
	  /// The delivery date of the future is the start date of the swap.
	  /// The swap must be a single currency swap with a notional of 1.
	  /// There must be two legs, the fixed leg must be received and the floating rate must be paid.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.Swap underlyingSwap;
	  private readonly Swap underlyingSwap;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isFalse(underlyingSwap.CrossCurrency, "Underlying swap must not be cross currency");
		foreach (SwapLeg swapLeg in underlyingSwap.Legs)
		{
		  if (swapLeg.Type.Equals(SwapLegType.FIXED))
		  {
			ArgChecker.isTrue(swapLeg.PayReceive.Receive, "Underlying swap must receive the fixed leg");
		  }
		  if (swapLeg is RateCalculationSwapLeg)
		  {
			RateCalculationSwapLeg leg = (RateCalculationSwapLeg) swapLeg;
			ArgChecker.isTrue(Math.Abs(leg.NotionalSchedule.Amount.InitialValue) == 1d, "Underlying swap must have a notional of 1");
		  }
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @DerivedProperty public com.opengamma.strata.basics.currency.Currency getCurrency()
	  public override Currency Currency
	  {
		  get
		  {
			return underlyingSwap.Legs.get(0).Currency;
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
	  public DsfSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  //-------------------------------------------------------------------------
	  public Dsf createProduct(ReferenceData refData)
	  {
		LocalDate deliveryDate = underlyingSwap.StartDate.Unadjusted;
		return new Dsf(SecurityId, notional, lastTradeDate, deliveryDate, underlyingSwap);
	  }

	  public DsfTrade createTrade(TradeInfo info, double quantity, double tradePrice, ReferenceData refData)
	  {

		return new DsfTrade(info, createProduct(refData), quantity, tradePrice);
	  }

	  public DsfPosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return DsfPosition.ofNet(positionInfo, createProduct(refData), quantity);
	  }

	  public DsfPosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return DsfPosition.ofLongShort(positionInfo, createProduct(refData), longQuantity, shortQuantity);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DsfSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DsfSecurity.Meta meta()
	  {
		return DsfSecurity.Meta.INSTANCE;
	  }

	  static DsfSecurity()
	  {
		MetaBean.register(DsfSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static DsfSecurity.Builder builder()
	  {
		return new DsfSecurity.Builder();
	  }

	  private DsfSecurity(SecurityInfo info, double notional, LocalDate lastTradeDate, Swap underlyingSwap)
	  {
		JodaBeanUtils.notNull(info, "info");
		ArgChecker.notNegativeOrZero(notional, "notional");
		JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		JodaBeanUtils.notNull(underlyingSwap, "underlyingSwap");
		this.info = info;
		this.notional = notional;
		this.lastTradeDate = lastTradeDate;
		this.underlyingSwap = underlyingSwap;
		validate();
	  }

	  public override DsfSecurity.Meta metaBean()
	  {
		return DsfSecurity.Meta.INSTANCE;
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
	  /// Gets the notional.
	  /// <para>
	  /// This is also called face value or contract value.
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
	  /// Gets the last date of trading.
	  /// <para>
	  /// This date must be before the delivery date of the underlying swap.
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
	  /// Gets the underlying swap.
	  /// <para>
	  /// The delivery date of the future is the start date of the swap.
	  /// The swap must be a single currency swap with a notional of 1.
	  /// There must be two legs, the fixed leg must be received and the floating rate must be paid.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Swap UnderlyingSwap
	  {
		  get
		  {
			return underlyingSwap;
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
		  DsfSecurity other = (DsfSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(lastTradeDate, other.lastTradeDate) && JodaBeanUtils.equal(underlyingSwap, other.underlyingSwap);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastTradeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingSwap);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("DsfSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("lastTradeDate").Append('=').Append(lastTradeDate).Append(',').Append(' ');
		buf.Append("underlyingSwap").Append('=').Append(JodaBeanUtils.ToString(underlyingSwap));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DsfSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(DsfSecurity), typeof(SecurityInfo));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(DsfSecurity), Double.TYPE);
			  lastTradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastTradeDate", typeof(DsfSecurity), typeof(LocalDate));
			  underlyingSwap_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingSwap", typeof(DsfSecurity), typeof(Swap));
			  currency_Renamed = DirectMetaProperty.ofDerived(this, "currency", typeof(DsfSecurity), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "notional", "lastTradeDate", "underlyingSwap", "currency");
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
		/// The meta-property for the {@code lastTradeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastTradeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlyingSwap} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Swap> underlyingSwap_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "notional", "lastTradeDate", "underlyingSwap", "currency");
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
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case 1497421456: // underlyingSwap
			  return underlyingSwap_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override DsfSecurity.Builder builder()
		{
		  return new DsfSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DsfSecurity);
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
		/// The meta-property for the {@code lastTradeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastTradeDate()
		{
		  return lastTradeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlyingSwap} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Swap> underlyingSwap()
		{
		  return underlyingSwap_Renamed;
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
			  return ((DsfSecurity) bean).Info;
			case 1585636160: // notional
			  return ((DsfSecurity) bean).Notional;
			case -1041950404: // lastTradeDate
			  return ((DsfSecurity) bean).LastTradeDate;
			case 1497421456: // underlyingSwap
			  return ((DsfSecurity) bean).UnderlyingSwap;
			case 575402001: // currency
			  return ((DsfSecurity) bean).Currency;
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
	  /// The bean-builder for {@code DsfSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<DsfSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastTradeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Swap underlyingSwap_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(DsfSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.lastTradeDate_Renamed = beanToCopy.LastTradeDate;
		  this.underlyingSwap_Renamed = beanToCopy.UnderlyingSwap;
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
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case 1497421456: // underlyingSwap
			  return underlyingSwap_Renamed;
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
			case -1041950404: // lastTradeDate
			  this.lastTradeDate_Renamed = (LocalDate) newValue;
			  break;
			case 1497421456: // underlyingSwap
			  this.underlyingSwap_Renamed = (Swap) newValue;
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

		public override DsfSecurity build()
		{
		  return new DsfSecurity(info_Renamed, notional_Renamed, lastTradeDate_Renamed, underlyingSwap_Renamed);
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
		/// Sets the notional.
		/// <para>
		/// This is also called face value or contract value.
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
		/// Sets the last date of trading.
		/// <para>
		/// This date must be before the delivery date of the underlying swap.
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
		/// Sets the underlying swap.
		/// <para>
		/// The delivery date of the future is the start date of the swap.
		/// The swap must be a single currency swap with a notional of 1.
		/// There must be two legs, the fixed leg must be received and the floating rate must be paid.
		/// </para>
		/// </summary>
		/// <param name="underlyingSwap">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlyingSwap(Swap underlyingSwap)
		{
		  JodaBeanUtils.notNull(underlyingSwap, "underlyingSwap");
		  this.underlyingSwap_Renamed = underlyingSwap;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("DsfSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("lastTradeDate").Append('=').Append(JodaBeanUtils.ToString(lastTradeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("underlyingSwap").Append('=').Append(JodaBeanUtils.ToString(underlyingSwap_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
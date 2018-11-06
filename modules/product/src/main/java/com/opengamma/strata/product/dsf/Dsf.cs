using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	/// A deliverable swap futures contract.
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
//ORIGINAL LINE: @BeanDefinition(constructorScope = "package") public final class Dsf implements com.opengamma.strata.product.SecuritizedProduct, com.opengamma.strata.basics.Resolvable<ResolvedDsf>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Dsf : SecuritizedProduct, Resolvable<ResolvedDsf>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// The notional of the futures.
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
	  /// The delivery date.
	  /// <para>
	  /// The underlying swap is delivered on this date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate deliveryDate;
	  private readonly LocalDate deliveryDate;
	  /// <summary>
	  /// The underlying swap.
	  /// <para>
	  /// The delivery date of the future is typically the first accrual date of the underlying swap.
	  /// The swap should be a receiver swap of notional 1.
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
		ArgChecker.inOrderOrEqual(deliveryDate, underlyingSwap.StartDate.Unadjusted, "deliveryDate", "underlyingSwap.startDate.unadjusted");
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
		ArgChecker.inOrderOrEqual(lastTradeDate, deliveryDate, "lastTradeDate", "deliveryDate");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the underlying swap.
	  /// <para>
	  /// The underlying swap must have a single currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency of the swap </returns>
	  public Currency Currency
	  {
		  get
		  {
			return underlyingSwap.ReceiveLeg.get().Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedDsf resolve(ReferenceData refData)
	  {
		ResolvedSwap resolvedSwap = underlyingSwap.resolve(refData);
		return new ResolvedDsf(securityId, notional, deliveryDate, lastTradeDate, resolvedSwap);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Dsf}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Dsf.Meta meta()
	  {
		return Dsf.Meta.INSTANCE;
	  }

	  static Dsf()
	  {
		MetaBean.register(Dsf.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Dsf.Builder builder()
	  {
		return new Dsf.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="securityId">  the value of the property, not null </param>
	  /// <param name="notional">  the value of the property </param>
	  /// <param name="lastTradeDate">  the value of the property, not null </param>
	  /// <param name="deliveryDate">  the value of the property, not null </param>
	  /// <param name="underlyingSwap">  the value of the property, not null </param>
	  internal Dsf(SecurityId securityId, double notional, LocalDate lastTradeDate, LocalDate deliveryDate, Swap underlyingSwap)
	  {
		JodaBeanUtils.notNull(securityId, "securityId");
		ArgChecker.notNegativeOrZero(notional, "notional");
		JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		JodaBeanUtils.notNull(deliveryDate, "deliveryDate");
		JodaBeanUtils.notNull(underlyingSwap, "underlyingSwap");
		this.securityId = securityId;
		this.notional = notional;
		this.lastTradeDate = lastTradeDate;
		this.deliveryDate = deliveryDate;
		this.underlyingSwap = underlyingSwap;
		validate();
	  }

	  public override Dsf.Meta metaBean()
	  {
		return Dsf.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional of the futures.
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
	  /// Gets the delivery date.
	  /// <para>
	  /// The underlying swap is delivered on this date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate DeliveryDate
	  {
		  get
		  {
			return deliveryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying swap.
	  /// <para>
	  /// The delivery date of the future is typically the first accrual date of the underlying swap.
	  /// The swap should be a receiver swap of notional 1.
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
		  Dsf other = (Dsf) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(lastTradeDate, other.lastTradeDate) && JodaBeanUtils.equal(deliveryDate, other.deliveryDate) && JodaBeanUtils.equal(underlyingSwap, other.underlyingSwap);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastTradeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(deliveryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingSwap);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("Dsf{");
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("lastTradeDate").Append('=').Append(lastTradeDate).Append(',').Append(' ');
		buf.Append("deliveryDate").Append('=').Append(deliveryDate).Append(',').Append(' ');
		buf.Append("underlyingSwap").Append('=').Append(JodaBeanUtils.ToString(underlyingSwap));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Dsf}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(Dsf), typeof(SecurityId));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(Dsf), Double.TYPE);
			  lastTradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastTradeDate", typeof(Dsf), typeof(LocalDate));
			  deliveryDate_Renamed = DirectMetaProperty.ofImmutable(this, "deliveryDate", typeof(Dsf), typeof(LocalDate));
			  underlyingSwap_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingSwap", typeof(Dsf), typeof(Swap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "notional", "lastTradeDate", "deliveryDate", "underlyingSwap");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
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
		/// The meta-property for the {@code deliveryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> deliveryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlyingSwap} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Swap> underlyingSwap_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "notional", "lastTradeDate", "deliveryDate", "underlyingSwap");
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case 681469378: // deliveryDate
			  return deliveryDate_Renamed;
			case 1497421456: // underlyingSwap
			  return underlyingSwap_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override Dsf.Builder builder()
		{
		  return new Dsf.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Dsf);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
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
		/// The meta-property for the {@code deliveryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> deliveryDate()
		{
		  return deliveryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlyingSwap} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Swap> underlyingSwap()
		{
		  return underlyingSwap_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((Dsf) bean).SecurityId;
			case 1585636160: // notional
			  return ((Dsf) bean).Notional;
			case -1041950404: // lastTradeDate
			  return ((Dsf) bean).LastTradeDate;
			case 681469378: // deliveryDate
			  return ((Dsf) bean).DeliveryDate;
			case 1497421456: // underlyingSwap
			  return ((Dsf) bean).UnderlyingSwap;
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
	  /// The bean-builder for {@code Dsf}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Dsf>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastTradeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate deliveryDate_Renamed;
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
		internal Builder(Dsf beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.lastTradeDate_Renamed = beanToCopy.LastTradeDate;
		  this.deliveryDate_Renamed = beanToCopy.DeliveryDate;
		  this.underlyingSwap_Renamed = beanToCopy.UnderlyingSwap;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case 681469378: // deliveryDate
			  return deliveryDate_Renamed;
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
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -1041950404: // lastTradeDate
			  this.lastTradeDate_Renamed = (LocalDate) newValue;
			  break;
			case 681469378: // deliveryDate
			  this.deliveryDate_Renamed = (LocalDate) newValue;
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

		public override Dsf build()
		{
		  return new Dsf(securityId_Renamed, notional_Renamed, lastTradeDate_Renamed, deliveryDate_Renamed, underlyingSwap_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the security identifier.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
		  return this;
		}

		/// <summary>
		/// Sets the notional of the futures.
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
		/// Sets the delivery date.
		/// <para>
		/// The underlying swap is delivered on this date.
		/// </para>
		/// </summary>
		/// <param name="deliveryDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder deliveryDate(LocalDate deliveryDate)
		{
		  JodaBeanUtils.notNull(deliveryDate, "deliveryDate");
		  this.deliveryDate_Renamed = deliveryDate;
		  return this;
		}

		/// <summary>
		/// Sets the underlying swap.
		/// <para>
		/// The delivery date of the future is typically the first accrual date of the underlying swap.
		/// The swap should be a receiver swap of notional 1.
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
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("Dsf.Builder{");
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("lastTradeDate").Append('=').Append(JodaBeanUtils.ToString(lastTradeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("deliveryDate").Append('=').Append(JodaBeanUtils.ToString(deliveryDate_Renamed)).Append(',').Append(' ');
		  buf.Append("underlyingSwap").Append('=').Append(JodaBeanUtils.ToString(underlyingSwap_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
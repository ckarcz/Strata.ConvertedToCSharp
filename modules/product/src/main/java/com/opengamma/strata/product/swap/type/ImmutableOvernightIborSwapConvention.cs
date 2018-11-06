using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
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

	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A market convention for Fixed-Overnight swap trades.
	/// <para>
	/// This defines the market convention for a Fixed-Overnight single currency swap.
	/// This is often known as an <i>OIS swap</i>, although <i>Fed Fund swaps</i> are also covered.
	/// The convention is formed by combining two swap leg conventions in the same currency.
	/// </para>
	/// <para>
	/// The convention is defined by four key dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Spot date, the base for date calculations, typically 2 business days after the trade date
	/// <li>Start date, the date on which the interest calculation starts, often the same as the spot date
	/// <li>End date, the date on which the interest calculation ends, typically a number of years after the start date
	/// </ul>
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableOvernightIborSwapConvention implements OvernightIborSwapConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableOvernightIborSwapConvention : OvernightIborSwapConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The market convention of the floating leg.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final OvernightRateSwapLegConvention overnightLeg;
	  private readonly OvernightRateSwapLegConvention overnightLeg;
	  /// <summary>
	  /// The market convention of the floating leg.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborRateSwapLegConvention iborLeg;
	  private readonly IborRateSwapLegConvention iborLeg;
	  /// <summary>
	  /// The offset of the spot value date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date to find the start date.
	  /// A typical value is "plus 2 business days".
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DaysAdjustment spotDateOffset;
	  private readonly DaysAdjustment spotDateOffset;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified name and leg conventions.
	  /// <para>
	  /// The two leg conventions must be in the same currency.
	  /// The spot date offset is set to be the effective date offset of the Ibor index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="overnightLeg">  the market convention for the overnight leg </param>
	  /// <param name="iborLeg">  the market convention for the ibor leg </param>
	  /// <returns> the convention </returns>
	  public static ImmutableOvernightIborSwapConvention of(string name, OvernightRateSwapLegConvention overnightLeg, IborRateSwapLegConvention iborLeg)
	  {

		return new ImmutableOvernightIborSwapConvention(name, overnightLeg, iborLeg, iborLeg.Index.EffectiveDateOffset);
	  }

	  /// <summary>
	  /// Obtains a convention based on the specified name and leg conventions.
	  /// <para>
	  /// The two leg conventions must be in the same currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="overnightLeg">  the market convention for the overnight leg </param>
	  /// <param name="iborLeg">  the market convention for the ibor leg </param>
	  /// <param name="spotDateOffset">  the offset of the spot value date from the trade date </param>
	  /// <returns> the convention </returns>
	  public static ImmutableOvernightIborSwapConvention of(string name, OvernightRateSwapLegConvention overnightLeg, IborRateSwapLegConvention iborLeg, DaysAdjustment spotDateOffset)
	  {

		return new ImmutableOvernightIborSwapConvention(name, overnightLeg, iborLeg, spotDateOffset);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(overnightLeg.Currency.Equals(iborLeg.Currency), "Conventions must have same currency");
	  }

	  //-------------------------------------------------------------------------
	  public SwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double spread)
	  {

		Optional<LocalDate> tradeDate = tradeInfo.TradeDate;
		if (tradeDate.Present)
		{
		  ArgChecker.inOrderOrEqual(tradeDate.get(), startDate, "tradeDate", "startDate");
		}
		SwapLeg leg1 = overnightLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Buy), notional, spread);
		SwapLeg leg2 = iborLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Sell), notional);
		return SwapTrade.builder().info(tradeInfo).product(Swap.of(leg1, leg2)).build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableOvernightIborSwapConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableOvernightIborSwapConvention.Meta meta()
	  {
		return ImmutableOvernightIborSwapConvention.Meta.INSTANCE;
	  }

	  static ImmutableOvernightIborSwapConvention()
	  {
		MetaBean.register(ImmutableOvernightIborSwapConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableOvernightIborSwapConvention.Builder builder()
	  {
		return new ImmutableOvernightIborSwapConvention.Builder();
	  }

	  private ImmutableOvernightIborSwapConvention(string name, OvernightRateSwapLegConvention overnightLeg, IborRateSwapLegConvention iborLeg, DaysAdjustment spotDateOffset)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(overnightLeg, "overnightLeg");
		JodaBeanUtils.notNull(iborLeg, "iborLeg");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		this.name = name;
		this.overnightLeg = overnightLeg;
		this.iborLeg = iborLeg;
		this.spotDateOffset = spotDateOffset;
		validate();
	  }

	  public override ImmutableOvernightIborSwapConvention.Meta metaBean()
	  {
		return ImmutableOvernightIborSwapConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention name, such as 'USD-FED-FUND-AA-LIBOR-3M'. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightRateSwapLegConvention OvernightLeg
	  {
		  get
		  {
			return overnightLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateSwapLegConvention IborLeg
	  {
		  get
		  {
			return iborLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the spot value date from the trade date.
	  /// <para>
	  /// The offset is applied to the trade date to find the start date.
	  /// A typical value is "plus 2 business days".
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
		  ImmutableOvernightIborSwapConvention other = (ImmutableOvernightIborSwapConvention) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(overnightLeg, other.overnightLeg) && JodaBeanUtils.equal(iborLeg, other.iborLeg) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(overnightLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(iborLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableOvernightIborSwapConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableOvernightIborSwapConvention), typeof(string));
			  overnightLeg_Renamed = DirectMetaProperty.ofImmutable(this, "overnightLeg", typeof(ImmutableOvernightIborSwapConvention), typeof(OvernightRateSwapLegConvention));
			  iborLeg_Renamed = DirectMetaProperty.ofImmutable(this, "iborLeg", typeof(ImmutableOvernightIborSwapConvention), typeof(IborRateSwapLegConvention));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(ImmutableOvernightIborSwapConvention), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "overnightLeg", "iborLeg", "spotDateOffset");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code overnightLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightRateSwapLegConvention> overnightLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code iborLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateSwapLegConvention> iborLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "overnightLeg", "iborLeg", "spotDateOffset");
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
			case 3373707: // name
			  return name_Renamed;
			case 1774606250: // overnightLeg
			  return overnightLeg_Renamed;
			case 1610246066: // iborLeg
			  return iborLeg_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableOvernightIborSwapConvention.Builder builder()
		{
		  return new ImmutableOvernightIborSwapConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableOvernightIborSwapConvention);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code overnightLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightRateSwapLegConvention> overnightLeg()
		{
		  return overnightLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code iborLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateSwapLegConvention> iborLeg()
		{
		  return iborLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> spotDateOffset()
		{
		  return spotDateOffset_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ImmutableOvernightIborSwapConvention) bean).Name;
			case 1774606250: // overnightLeg
			  return ((ImmutableOvernightIborSwapConvention) bean).OvernightLeg;
			case 1610246066: // iborLeg
			  return ((ImmutableOvernightIborSwapConvention) bean).IborLeg;
			case 746995843: // spotDateOffset
			  return ((ImmutableOvernightIborSwapConvention) bean).SpotDateOffset;
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
	  /// The bean-builder for {@code ImmutableOvernightIborSwapConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableOvernightIborSwapConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal OvernightRateSwapLegConvention overnightLeg_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateSwapLegConvention iborLeg_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment spotDateOffset_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableOvernightIborSwapConvention beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.overnightLeg_Renamed = beanToCopy.OvernightLeg;
		  this.iborLeg_Renamed = beanToCopy.IborLeg;
		  this.spotDateOffset_Renamed = beanToCopy.SpotDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1774606250: // overnightLeg
			  return overnightLeg_Renamed;
			case 1610246066: // iborLeg
			  return iborLeg_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case 1774606250: // overnightLeg
			  this.overnightLeg_Renamed = (OvernightRateSwapLegConvention) newValue;
			  break;
			case 1610246066: // iborLeg
			  this.iborLeg_Renamed = (IborRateSwapLegConvention) newValue;
			  break;
			case 746995843: // spotDateOffset
			  this.spotDateOffset_Renamed = (DaysAdjustment) newValue;
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

		public override ImmutableOvernightIborSwapConvention build()
		{
		  return new ImmutableOvernightIborSwapConvention(name_Renamed, overnightLeg_Renamed, iborLeg_Renamed, spotDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the convention name, such as 'USD-FED-FUND-AA-LIBOR-3M'. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg. </summary>
		/// <param name="overnightLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder overnightLeg(OvernightRateSwapLegConvention overnightLeg)
		{
		  JodaBeanUtils.notNull(overnightLeg, "overnightLeg");
		  this.overnightLeg_Renamed = overnightLeg;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg. </summary>
		/// <param name="iborLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder iborLeg(IborRateSwapLegConvention iborLeg)
		{
		  JodaBeanUtils.notNull(iborLeg, "iborLeg");
		  this.iborLeg_Renamed = iborLeg;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the spot value date from the trade date.
		/// <para>
		/// The offset is applied to the trade date to find the start date.
		/// A typical value is "plus 2 business days".
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

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableOvernightIborSwapConvention.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("overnightLeg").Append('=').Append(JodaBeanUtils.ToString(overnightLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("iborLeg").Append('=').Append(JodaBeanUtils.ToString(iborLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
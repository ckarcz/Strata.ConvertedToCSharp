using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// A market convention for Ibor-Ibor swap trades.
	/// <para>
	/// This defines the market convention for a Ibor-Ibor single currency swap.
	/// The convention is formed by combining two swap leg conventions in the same currency.
	/// </para>
	/// <para>
	/// The market price is for the difference (spread) between the values of the two legs.
	/// This convention has two legs, the "spread leg" and the "flat leg". The spread will be
	/// added to the "spread leg", which is typically the leg with the shorter underlying tenor.
	/// The payment frequency is typically determined by the longer underlying tenor, with
	/// compounding applied.
	/// </para>
	/// <para>
	/// For example, a 'USD 3s1s' basis swap has 'USD-LIBOR-1M' as the spread leg and 'USD-LIBOR-3M'
	/// as the flat leg. Payment is every 3 months, with the one month leg compounded.
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
//ORIGINAL LINE: @BeanDefinition public final class ImmutableIborIborSwapConvention implements IborIborSwapConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableIborIborSwapConvention : IborIborSwapConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The market convention of the floating leg that has the spread applied.
	  /// <para>
	  /// The spread is the market price of the instrument.
	  /// It is added to the observed interest rate.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborRateSwapLegConvention spreadLeg;
	  private readonly IborRateSwapLegConvention spreadLeg;
	  /// <summary>
	  /// The market convention of the floating leg that does not have the spread applied.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborRateSwapLegConvention flatLeg;
	  private readonly IborRateSwapLegConvention flatLeg;
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
	  /// The spot date offset is set to be the effective date offset of the index of the spread leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="spreadLeg">  the market convention for the leg that the spread is added to </param>
	  /// <param name="flatLeg">  the market convention for the other leg, known as the flat leg </param>
	  /// <returns> the convention </returns>
	  public static ImmutableIborIborSwapConvention of(string name, IborRateSwapLegConvention spreadLeg, IborRateSwapLegConvention flatLeg)
	  {

		return of(name, spreadLeg, flatLeg, spreadLeg.Index.EffectiveDateOffset);
	  }

	  /// <summary>
	  /// Obtains a convention based on the specified name and leg conventions.
	  /// <para>
	  /// The two leg conventions must be in the same currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="spreadLeg">  the market convention for the leg that the spread is added to </param>
	  /// <param name="flatLeg">  the market convention for the other leg, known as the flat leg </param>
	  /// <param name="spotDateOffset">  the offset of the spot value date from the trade date </param>
	  /// <returns> the convention </returns>
	  public static ImmutableIborIborSwapConvention of(string name, IborRateSwapLegConvention spreadLeg, IborRateSwapLegConvention flatLeg, DaysAdjustment spotDateOffset)
	  {

		return new ImmutableIborIborSwapConvention(name, spreadLeg, flatLeg, spotDateOffset);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(spreadLeg.Currency.Equals(flatLeg.Currency), "Conventions must have same currency");
	  }

	  //-------------------------------------------------------------------------
	  public SwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double spread)
	  {

		Optional<LocalDate> tradeDate = tradeInfo.TradeDate;
		if (tradeDate.Present)
		{
		  ArgChecker.inOrderOrEqual(tradeDate.get(), startDate, "tradeDate", "startDate");
		}
		SwapLeg leg1 = spreadLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Buy), notional, spread);
		SwapLeg leg2 = flatLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Sell), notional);
		return SwapTrade.builder().info(tradeInfo).product(Swap.of(leg1, leg2)).build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableIborIborSwapConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableIborIborSwapConvention.Meta meta()
	  {
		return ImmutableIborIborSwapConvention.Meta.INSTANCE;
	  }

	  static ImmutableIborIborSwapConvention()
	  {
		MetaBean.register(ImmutableIborIborSwapConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableIborIborSwapConvention.Builder builder()
	  {
		return new ImmutableIborIborSwapConvention.Builder();
	  }

	  private ImmutableIborIborSwapConvention(string name, IborRateSwapLegConvention spreadLeg, IborRateSwapLegConvention flatLeg, DaysAdjustment spotDateOffset)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(spreadLeg, "spreadLeg");
		JodaBeanUtils.notNull(flatLeg, "flatLeg");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		this.name = name;
		this.spreadLeg = spreadLeg;
		this.flatLeg = flatLeg;
		this.spotDateOffset = spotDateOffset;
		validate();
	  }

	  public override ImmutableIborIborSwapConvention.Meta metaBean()
	  {
		return ImmutableIborIborSwapConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention name, such as 'USD-LIBOR-3M-LIBOR-6M'. </summary>
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
	  /// Gets the market convention of the floating leg that has the spread applied.
	  /// <para>
	  /// The spread is the market price of the instrument.
	  /// It is added to the observed interest rate.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateSwapLegConvention SpreadLeg
	  {
		  get
		  {
			return spreadLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg that does not have the spread applied. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateSwapLegConvention FlatLeg
	  {
		  get
		  {
			return flatLeg;
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
		  ImmutableIborIborSwapConvention other = (ImmutableIborIborSwapConvention) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(spreadLeg, other.spreadLeg) && JodaBeanUtils.equal(flatLeg, other.flatLeg) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spreadLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(flatLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableIborIborSwapConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableIborIborSwapConvention), typeof(string));
			  spreadLeg_Renamed = DirectMetaProperty.ofImmutable(this, "spreadLeg", typeof(ImmutableIborIborSwapConvention), typeof(IborRateSwapLegConvention));
			  flatLeg_Renamed = DirectMetaProperty.ofImmutable(this, "flatLeg", typeof(ImmutableIborIborSwapConvention), typeof(IborRateSwapLegConvention));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(ImmutableIborIborSwapConvention), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "spreadLeg", "flatLeg", "spotDateOffset");
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
		/// The meta-property for the {@code spreadLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateSwapLegConvention> spreadLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code flatLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateSwapLegConvention> flatLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "spreadLeg", "flatLeg", "spotDateOffset");
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
			case 1302781851: // spreadLeg
			  return spreadLeg_Renamed;
			case -778843179: // flatLeg
			  return flatLeg_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableIborIborSwapConvention.Builder builder()
		{
		  return new ImmutableIborIborSwapConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableIborIborSwapConvention);
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
		/// The meta-property for the {@code spreadLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateSwapLegConvention> spreadLeg()
		{
		  return spreadLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code flatLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateSwapLegConvention> flatLeg()
		{
		  return flatLeg_Renamed;
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
			  return ((ImmutableIborIborSwapConvention) bean).Name;
			case 1302781851: // spreadLeg
			  return ((ImmutableIborIborSwapConvention) bean).SpreadLeg;
			case -778843179: // flatLeg
			  return ((ImmutableIborIborSwapConvention) bean).FlatLeg;
			case 746995843: // spotDateOffset
			  return ((ImmutableIborIborSwapConvention) bean).SpotDateOffset;
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
	  /// The bean-builder for {@code ImmutableIborIborSwapConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableIborIborSwapConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateSwapLegConvention spreadLeg_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateSwapLegConvention flatLeg_Renamed;
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
		internal Builder(ImmutableIborIborSwapConvention beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.spreadLeg_Renamed = beanToCopy.SpreadLeg;
		  this.flatLeg_Renamed = beanToCopy.FlatLeg;
		  this.spotDateOffset_Renamed = beanToCopy.SpotDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1302781851: // spreadLeg
			  return spreadLeg_Renamed;
			case -778843179: // flatLeg
			  return flatLeg_Renamed;
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
			case 1302781851: // spreadLeg
			  this.spreadLeg_Renamed = (IborRateSwapLegConvention) newValue;
			  break;
			case -778843179: // flatLeg
			  this.flatLeg_Renamed = (IborRateSwapLegConvention) newValue;
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

		public override ImmutableIborIborSwapConvention build()
		{
		  return new ImmutableIborIborSwapConvention(name_Renamed, spreadLeg_Renamed, flatLeg_Renamed, spotDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the convention name, such as 'USD-LIBOR-3M-LIBOR-6M'. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg that has the spread applied.
		/// <para>
		/// The spread is the market price of the instrument.
		/// It is added to the observed interest rate.
		/// </para>
		/// </summary>
		/// <param name="spreadLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spreadLeg(IborRateSwapLegConvention spreadLeg)
		{
		  JodaBeanUtils.notNull(spreadLeg, "spreadLeg");
		  this.spreadLeg_Renamed = spreadLeg;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg that does not have the spread applied. </summary>
		/// <param name="flatLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder flatLeg(IborRateSwapLegConvention flatLeg)
		{
		  JodaBeanUtils.notNull(flatLeg, "flatLeg");
		  this.flatLeg_Renamed = flatLeg;
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
		  buf.Append("ImmutableIborIborSwapConvention.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("spreadLeg").Append('=').Append(JodaBeanUtils.ToString(spreadLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("flatLeg").Append('=').Append(JodaBeanUtils.ToString(flatLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
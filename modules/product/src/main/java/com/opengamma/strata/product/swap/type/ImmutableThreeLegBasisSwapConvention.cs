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
	/// A market convention for three leg basis swap trades.
	/// <para>
	/// This defines the market convention for a single currency basis swap.
	/// The convention is formed by combining three swap leg conventions in the same currency.
	/// </para>
	/// <para>
	/// The market price is for the difference (spread) between the values of the two floating legs.
	/// This convention has three legs, the "spread leg", the "spread floating leg" and the "flat floating leg". 
	/// The "spread leg" represented by the fixed leg will be added to the "spread floating leg" 
	/// which is typically the leg with the shorter underlying tenor.
	/// Thus the "spread leg" and "spread floating leg" will have the same pay/receive direction.
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
//ORIGINAL LINE: @BeanDefinition public final class ImmutableThreeLegBasisSwapConvention implements ThreeLegBasisSwapConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableThreeLegBasisSwapConvention : ThreeLegBasisSwapConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The market convention of the fixed leg for the spread.
	  /// <para>
	  /// This is to be applied to {@code floatingSpreadLeg}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FixedRateSwapLegConvention spreadLeg;
	  private readonly FixedRateSwapLegConvention spreadLeg;
	  /// <summary>
	  /// The market convention of the floating leg to which the spread leg is added.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborRateSwapLegConvention spreadFloatingLeg;
	  private readonly IborRateSwapLegConvention spreadFloatingLeg;
	  /// <summary>
	  /// The market convention of the floating leg that does not have the spread applied.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborRateSwapLegConvention flatFloatingLeg;
	  private readonly IborRateSwapLegConvention flatFloatingLeg;
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
	  /// The spot date offset is set to be the effective date offset of the index of the spread floating leg.
	  /// </para>
	  /// <para>
	  /// The spread is represented by {@code FixedRateSwapLegConvention} and to be applied to {@code floatingSpreadLeg}. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="spreadLeg">  the market convention for the spread leg added to one of the floating leg </param>
	  /// <param name="spreadFloatingLeg">  the market convention for the spread floating leg </param>
	  /// <param name="flatFloatingLeg">  the market convention for the flat floating leg </param>
	  /// <returns> the convention </returns>
	  public static ImmutableThreeLegBasisSwapConvention of(string name, FixedRateSwapLegConvention spreadLeg, IborRateSwapLegConvention spreadFloatingLeg, IborRateSwapLegConvention flatFloatingLeg)
	  {

		return of(name, spreadLeg, spreadFloatingLeg, flatFloatingLeg, spreadFloatingLeg.Index.EffectiveDateOffset);
	  }

	  /// <summary>
	  /// Obtains a convention based on the specified name and leg conventions.
	  /// <para>
	  /// The two leg conventions must be in the same currency.
	  /// </para>
	  /// <para>
	  /// The spread is represented by {@code FixedRateSwapLegConvention} and to be applied to {@code floatingSpreadLeg}. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="spreadLeg">  the market convention for the spread leg added to one of the floating leg </param>
	  /// <param name="spreadFloatingLeg">  the market convention for the spread floating leg </param>
	  /// <param name="flatFloatingLeg">  the market convention for the flat floating leg </param>
	  /// <param name="spotDateOffset">  the offset of the spot value date from the trade date </param>
	  /// <returns> the convention </returns>
	  public static ImmutableThreeLegBasisSwapConvention of(string name, FixedRateSwapLegConvention spreadLeg, IborRateSwapLegConvention spreadFloatingLeg, IborRateSwapLegConvention flatFloatingLeg, DaysAdjustment spotDateOffset)
	  {

		return new ImmutableThreeLegBasisSwapConvention(name, spreadLeg, spreadFloatingLeg, flatFloatingLeg, spotDateOffset);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(spreadFloatingLeg.Currency.Equals(spreadLeg.Currency), "Conventions must have same currency");
		ArgChecker.isTrue(spreadFloatingLeg.Currency.Equals(flatFloatingLeg.Currency), "Conventions must have same currency");
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
		SwapLeg leg2 = spreadFloatingLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Buy), notional);
		SwapLeg leg3 = flatFloatingLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Sell), notional);
		return SwapTrade.builder().info(tradeInfo).product(Swap.of(leg1, leg2, leg3)).build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableThreeLegBasisSwapConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableThreeLegBasisSwapConvention.Meta meta()
	  {
		return ImmutableThreeLegBasisSwapConvention.Meta.INSTANCE;
	  }

	  static ImmutableThreeLegBasisSwapConvention()
	  {
		MetaBean.register(ImmutableThreeLegBasisSwapConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableThreeLegBasisSwapConvention.Builder builder()
	  {
		return new ImmutableThreeLegBasisSwapConvention.Builder();
	  }

	  private ImmutableThreeLegBasisSwapConvention(string name, FixedRateSwapLegConvention spreadLeg, IborRateSwapLegConvention spreadFloatingLeg, IborRateSwapLegConvention flatFloatingLeg, DaysAdjustment spotDateOffset)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(spreadLeg, "spreadLeg");
		JodaBeanUtils.notNull(spreadFloatingLeg, "spreadFloatingLeg");
		JodaBeanUtils.notNull(flatFloatingLeg, "flatFloatingLeg");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		this.name = name;
		this.spreadLeg = spreadLeg;
		this.spreadFloatingLeg = spreadFloatingLeg;
		this.flatFloatingLeg = flatFloatingLeg;
		this.spotDateOffset = spotDateOffset;
		validate();
	  }

	  public override ImmutableThreeLegBasisSwapConvention.Meta metaBean()
	  {
		return ImmutableThreeLegBasisSwapConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention name. </summary>
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
	  /// Gets the market convention of the fixed leg for the spread.
	  /// <para>
	  /// This is to be applied to {@code floatingSpreadLeg}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedRateSwapLegConvention SpreadLeg
	  {
		  get
		  {
			return spreadLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg to which the spread leg is added. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateSwapLegConvention SpreadFloatingLeg
	  {
		  get
		  {
			return spreadFloatingLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg that does not have the spread applied. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateSwapLegConvention FlatFloatingLeg
	  {
		  get
		  {
			return flatFloatingLeg;
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
		  ImmutableThreeLegBasisSwapConvention other = (ImmutableThreeLegBasisSwapConvention) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(spreadLeg, other.spreadLeg) && JodaBeanUtils.equal(spreadFloatingLeg, other.spreadFloatingLeg) && JodaBeanUtils.equal(flatFloatingLeg, other.flatFloatingLeg) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spreadLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spreadFloatingLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(flatFloatingLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableThreeLegBasisSwapConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableThreeLegBasisSwapConvention), typeof(string));
			  spreadLeg_Renamed = DirectMetaProperty.ofImmutable(this, "spreadLeg", typeof(ImmutableThreeLegBasisSwapConvention), typeof(FixedRateSwapLegConvention));
			  spreadFloatingLeg_Renamed = DirectMetaProperty.ofImmutable(this, "spreadFloatingLeg", typeof(ImmutableThreeLegBasisSwapConvention), typeof(IborRateSwapLegConvention));
			  flatFloatingLeg_Renamed = DirectMetaProperty.ofImmutable(this, "flatFloatingLeg", typeof(ImmutableThreeLegBasisSwapConvention), typeof(IborRateSwapLegConvention));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(ImmutableThreeLegBasisSwapConvention), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "spreadLeg", "spreadFloatingLeg", "flatFloatingLeg", "spotDateOffset");
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
		internal MetaProperty<FixedRateSwapLegConvention> spreadLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code spreadFloatingLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateSwapLegConvention> spreadFloatingLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code flatFloatingLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateSwapLegConvention> flatFloatingLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "spreadLeg", "spreadFloatingLeg", "flatFloatingLeg", "spotDateOffset");
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
			case -1969210187: // spreadFloatingLeg
			  return spreadFloatingLeg_Renamed;
			case 274878191: // flatFloatingLeg
			  return flatFloatingLeg_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableThreeLegBasisSwapConvention.Builder builder()
		{
		  return new ImmutableThreeLegBasisSwapConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableThreeLegBasisSwapConvention);
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
		public MetaProperty<FixedRateSwapLegConvention> spreadLeg()
		{
		  return spreadLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spreadFloatingLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateSwapLegConvention> spreadFloatingLeg()
		{
		  return spreadFloatingLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code flatFloatingLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateSwapLegConvention> flatFloatingLeg()
		{
		  return flatFloatingLeg_Renamed;
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
			  return ((ImmutableThreeLegBasisSwapConvention) bean).Name;
			case 1302781851: // spreadLeg
			  return ((ImmutableThreeLegBasisSwapConvention) bean).SpreadLeg;
			case -1969210187: // spreadFloatingLeg
			  return ((ImmutableThreeLegBasisSwapConvention) bean).SpreadFloatingLeg;
			case 274878191: // flatFloatingLeg
			  return ((ImmutableThreeLegBasisSwapConvention) bean).FlatFloatingLeg;
			case 746995843: // spotDateOffset
			  return ((ImmutableThreeLegBasisSwapConvention) bean).SpotDateOffset;
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
	  /// The bean-builder for {@code ImmutableThreeLegBasisSwapConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableThreeLegBasisSwapConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedRateSwapLegConvention spreadLeg_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateSwapLegConvention spreadFloatingLeg_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateSwapLegConvention flatFloatingLeg_Renamed;
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
		internal Builder(ImmutableThreeLegBasisSwapConvention beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.spreadLeg_Renamed = beanToCopy.SpreadLeg;
		  this.spreadFloatingLeg_Renamed = beanToCopy.SpreadFloatingLeg;
		  this.flatFloatingLeg_Renamed = beanToCopy.FlatFloatingLeg;
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
			case -1969210187: // spreadFloatingLeg
			  return spreadFloatingLeg_Renamed;
			case 274878191: // flatFloatingLeg
			  return flatFloatingLeg_Renamed;
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
			  this.spreadLeg_Renamed = (FixedRateSwapLegConvention) newValue;
			  break;
			case -1969210187: // spreadFloatingLeg
			  this.spreadFloatingLeg_Renamed = (IborRateSwapLegConvention) newValue;
			  break;
			case 274878191: // flatFloatingLeg
			  this.flatFloatingLeg_Renamed = (IborRateSwapLegConvention) newValue;
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

		public override ImmutableThreeLegBasisSwapConvention build()
		{
		  return new ImmutableThreeLegBasisSwapConvention(name_Renamed, spreadLeg_Renamed, spreadFloatingLeg_Renamed, flatFloatingLeg_Renamed, spotDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the convention name. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the fixed leg for the spread.
		/// <para>
		/// This is to be applied to {@code floatingSpreadLeg}.
		/// </para>
		/// </summary>
		/// <param name="spreadLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spreadLeg(FixedRateSwapLegConvention spreadLeg)
		{
		  JodaBeanUtils.notNull(spreadLeg, "spreadLeg");
		  this.spreadLeg_Renamed = spreadLeg;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg to which the spread leg is added. </summary>
		/// <param name="spreadFloatingLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spreadFloatingLeg(IborRateSwapLegConvention spreadFloatingLeg)
		{
		  JodaBeanUtils.notNull(spreadFloatingLeg, "spreadFloatingLeg");
		  this.spreadFloatingLeg_Renamed = spreadFloatingLeg;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg that does not have the spread applied. </summary>
		/// <param name="flatFloatingLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder flatFloatingLeg(IborRateSwapLegConvention flatFloatingLeg)
		{
		  JodaBeanUtils.notNull(flatFloatingLeg, "flatFloatingLeg");
		  this.flatFloatingLeg_Renamed = flatFloatingLeg;
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
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("ImmutableThreeLegBasisSwapConvention.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("spreadLeg").Append('=').Append(JodaBeanUtils.ToString(spreadLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("spreadFloatingLeg").Append('=').Append(JodaBeanUtils.ToString(spreadFloatingLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("flatFloatingLeg").Append('=').Append(JodaBeanUtils.ToString(flatFloatingLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
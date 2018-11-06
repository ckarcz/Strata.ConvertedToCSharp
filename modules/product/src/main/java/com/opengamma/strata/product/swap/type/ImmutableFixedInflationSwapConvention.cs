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
	using Messages = com.opengamma.strata.collect.Messages;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// A market convention for Fixed-Inflation swap trades.
	/// <para>
	/// This defines the market convention for a Fixed-Inflation single currency swap.
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
//ORIGINAL LINE: @BeanDefinition public final class ImmutableFixedInflationSwapConvention implements FixedInflationSwapConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableFixedInflationSwapConvention : FixedInflationSwapConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final String name;
		private readonly string name;
	  /// <summary>
	  /// The market convention of the fixed leg.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FixedRateSwapLegConvention fixedLeg;
	  private readonly FixedRateSwapLegConvention fixedLeg;
	  /// <summary>
	  /// The market convention of the floating leg.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final InflationRateSwapLegConvention floatingLeg;
	  private readonly InflationRateSwapLegConvention floatingLeg;
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
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the unique name of the convention </param>
	  /// <param name="fixedLeg">  the market convention for the fixed leg </param>
	  /// <param name="floatingLeg">  the market convention for the floating leg </param>
	  /// <param name="spotDateOffset">  the offset of the spot value date from the trade date </param>
	  /// <returns> the convention </returns>
	  public static ImmutableFixedInflationSwapConvention of(string name, FixedRateSwapLegConvention fixedLeg, InflationRateSwapLegConvention floatingLeg, DaysAdjustment spotDateOffset)
	  {

		return new ImmutableFixedInflationSwapConvention(name, fixedLeg, floatingLeg, spotDateOffset);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(fixedLeg.Currency.Equals(floatingLeg.Currency), Messages.format("Swap leg conventions must have same currency but found {} and {}, conventions {} and {}", fixedLeg.Currency, floatingLeg.Currency, fixedLeg, floatingLeg));
	  }

	  //-------------------------------------------------------------------------
	  public SwapTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate)
	  {

		Optional<LocalDate> tradeDate = tradeInfo.TradeDate;
		if (tradeDate.Present)
		{
		  ArgChecker.inOrderOrEqual(tradeDate.get(), startDate, "tradeDate", "startDate");
		}
		SwapLeg leg1 = fixedLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Buy), notional, fixedRate);
		SwapLeg leg2 = floatingLeg.toLeg(startDate, endDate, PayReceive.ofPay(buySell.Sell), notional);
		return SwapTrade.builder().info(tradeInfo).product(Swap.of(leg1, leg2)).build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFixedInflationSwapConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableFixedInflationSwapConvention.Meta meta()
	  {
		return ImmutableFixedInflationSwapConvention.Meta.INSTANCE;
	  }

	  static ImmutableFixedInflationSwapConvention()
	  {
		MetaBean.register(ImmutableFixedInflationSwapConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableFixedInflationSwapConvention.Builder builder()
	  {
		return new ImmutableFixedInflationSwapConvention.Builder();
	  }

	  private ImmutableFixedInflationSwapConvention(string name, FixedRateSwapLegConvention fixedLeg, InflationRateSwapLegConvention floatingLeg, DaysAdjustment spotDateOffset)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(fixedLeg, "fixedLeg");
		JodaBeanUtils.notNull(floatingLeg, "floatingLeg");
		JodaBeanUtils.notNull(spotDateOffset, "spotDateOffset");
		this.name = name;
		this.fixedLeg = fixedLeg;
		this.floatingLeg = floatingLeg;
		this.spotDateOffset = spotDateOffset;
		validate();
	  }

	  public override ImmutableFixedInflationSwapConvention.Meta metaBean()
	  {
		return ImmutableFixedInflationSwapConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention name, such as 'USD-FIXED-6M-LIBOR-3M'. </summary>
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
	  /// Gets the market convention of the fixed leg. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedRateSwapLegConvention FixedLeg
	  {
		  get
		  {
			return fixedLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market convention of the floating leg. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public InflationRateSwapLegConvention FloatingLeg
	  {
		  get
		  {
			return floatingLeg;
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
		  ImmutableFixedInflationSwapConvention other = (ImmutableFixedInflationSwapConvention) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(fixedLeg, other.fixedLeg) && JodaBeanUtils.equal(floatingLeg, other.floatingLeg) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floatingLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFixedInflationSwapConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableFixedInflationSwapConvention), typeof(string));
			  fixedLeg_Renamed = DirectMetaProperty.ofImmutable(this, "fixedLeg", typeof(ImmutableFixedInflationSwapConvention), typeof(FixedRateSwapLegConvention));
			  floatingLeg_Renamed = DirectMetaProperty.ofImmutable(this, "floatingLeg", typeof(ImmutableFixedInflationSwapConvention), typeof(InflationRateSwapLegConvention));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(ImmutableFixedInflationSwapConvention), typeof(DaysAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "fixedLeg", "floatingLeg", "spotDateOffset");
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
		/// The meta-property for the {@code fixedLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedRateSwapLegConvention> fixedLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code floatingLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<InflationRateSwapLegConvention> floatingLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "fixedLeg", "floatingLeg", "spotDateOffset");
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
			case -391537158: // fixedLeg
			  return fixedLeg_Renamed;
			case -1177101272: // floatingLeg
			  return floatingLeg_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableFixedInflationSwapConvention.Builder builder()
		{
		  return new ImmutableFixedInflationSwapConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableFixedInflationSwapConvention);
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
		/// The meta-property for the {@code fixedLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedRateSwapLegConvention> fixedLeg()
		{
		  return fixedLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code floatingLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<InflationRateSwapLegConvention> floatingLeg()
		{
		  return floatingLeg_Renamed;
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
			  return ((ImmutableFixedInflationSwapConvention) bean).Name;
			case -391537158: // fixedLeg
			  return ((ImmutableFixedInflationSwapConvention) bean).FixedLeg;
			case -1177101272: // floatingLeg
			  return ((ImmutableFixedInflationSwapConvention) bean).FloatingLeg;
			case 746995843: // spotDateOffset
			  return ((ImmutableFixedInflationSwapConvention) bean).SpotDateOffset;
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
	  /// The bean-builder for {@code ImmutableFixedInflationSwapConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableFixedInflationSwapConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedRateSwapLegConvention fixedLeg_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal InflationRateSwapLegConvention floatingLeg_Renamed;
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
		internal Builder(ImmutableFixedInflationSwapConvention beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.fixedLeg_Renamed = beanToCopy.FixedLeg;
		  this.floatingLeg_Renamed = beanToCopy.FloatingLeg;
		  this.spotDateOffset_Renamed = beanToCopy.SpotDateOffset;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case -391537158: // fixedLeg
			  return fixedLeg_Renamed;
			case -1177101272: // floatingLeg
			  return floatingLeg_Renamed;
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
			case -391537158: // fixedLeg
			  this.fixedLeg_Renamed = (FixedRateSwapLegConvention) newValue;
			  break;
			case -1177101272: // floatingLeg
			  this.floatingLeg_Renamed = (InflationRateSwapLegConvention) newValue;
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

		public override ImmutableFixedInflationSwapConvention build()
		{
		  return new ImmutableFixedInflationSwapConvention(name_Renamed, fixedLeg_Renamed, floatingLeg_Renamed, spotDateOffset_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the convention name, such as 'USD-FIXED-6M-LIBOR-3M'. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the fixed leg. </summary>
		/// <param name="fixedLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedLeg(FixedRateSwapLegConvention fixedLeg)
		{
		  JodaBeanUtils.notNull(fixedLeg, "fixedLeg");
		  this.fixedLeg_Renamed = fixedLeg;
		  return this;
		}

		/// <summary>
		/// Sets the market convention of the floating leg. </summary>
		/// <param name="floatingLeg">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder floatingLeg(InflationRateSwapLegConvention floatingLeg)
		{
		  JodaBeanUtils.notNull(floatingLeg, "floatingLeg");
		  this.floatingLeg_Renamed = floatingLeg;
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
		  buf.Append("ImmutableFixedInflationSwapConvention.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedLeg").Append('=').Append(JodaBeanUtils.ToString(fixedLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("floatingLeg").Append('=').Append(JodaBeanUtils.ToString(floatingLeg_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
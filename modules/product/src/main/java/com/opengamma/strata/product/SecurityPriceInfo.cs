using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Defines the meaning of the security price.
	/// <para>
	/// A value of a security is measured in terms of a <i>price</i> which is not typically a monetary amount.
	/// Instead the price is an arbitrary number that can be converted to a monetary amount.
	/// The price will move up and down in <i>ticks</i>. This class provides the size and monetary
	/// value of each tick, allowing changes in the price to be converted to a monetary amount.
	/// </para>
	/// <para>
	/// Three properties define the necessary information:
	/// Tick size is the minimum movement in the price of the security (the tick).
	/// Tick value is the monetary value gained or lost when the price changes by one tick.
	/// Contract size is the quantity of the underlying present in each derivative contract,
	/// which acts as a multiplier.
	/// </para>
	/// <para>
	/// For example, the price of an ICE Brent Crude future is based on the price of a barrel of crude oil in USD.
	/// The tick size of this contract is 0.01 (equivalent to 1 cent).
	/// The contract size is 1,000 barrels.
	/// Therefore the tick value is 0.01 * 1,000 = 10 USD.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SecurityPriceInfo implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SecurityPriceInfo : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double tickSize;
		private readonly double tickSize;
	  /// <summary>
	  /// The monetary value of one tick.
	  /// <para>
	  /// Tick value is the monetary value of the minimum movement in the price of the security.
	  /// When the price changes by one tick, this amount is gained or lost.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyAmount tickValue;
	  private readonly CurrencyAmount tickValue;
	  /// <summary>
	  /// The size of each contract.
	  /// <para>
	  /// Contract size is the quantity of the underlying present in each derivative contract.
	  /// For example, an equity option typically consists of 100 shares.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegativeOrZero") private final double contractSize;
	  private readonly double contractSize;
	  /// <summary>
	  /// Multiplier to apply to the price.
	  /// </summary>
	  [NonSerialized]
	  private readonly double tradeUnitValue; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the tick size and tick value.
	  /// <para>
	  /// The contract size will be set to 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tickSize">  the size of each tick, not negative or zero </param>
	  /// <param name="tickValue">  the value of each tick </param>
	  /// <returns> the security price information </returns>
	  public static SecurityPriceInfo of(double tickSize, CurrencyAmount tickValue)
	  {
		return new SecurityPriceInfo(tickSize, tickValue, 1);
	  }

	  /// <summary>
	  /// Obtains an instance from the tick size, tick value and contract size.
	  /// </summary>
	  /// <param name="tickSize">  the size of each tick, not negative or zero </param>
	  /// <param name="tickValue">  the value of each tick </param>
	  /// <param name="contractSize">  the contract size </param>
	  /// <returns> the security price information </returns>
	  public static SecurityPriceInfo of(double tickSize, CurrencyAmount tickValue, double contractSize)
	  {
		return new SecurityPriceInfo(tickSize, tickValue, contractSize);
	  }

	  /// <summary>
	  /// Obtains an instance from the currency and the value of a single tradeable unit.
	  /// </summary>
	  /// <param name="currency">  the currency in which the security is traded </param>
	  /// <param name="tradeUnitValue">  the value of a single tradeable unit of the security </param>
	  /// <returns> the security price information </returns>
	  public static SecurityPriceInfo of(Currency currency, double tradeUnitValue)
	  {
		return new SecurityPriceInfo(1, CurrencyAmount.of(currency, 1), tradeUnitValue);
	  }

	  /// <summary>
	  /// Obtains an instance from the currency.
	  /// <para>
	  /// This sets the tick size and tick value to the minor unit of the currency.
	  /// For example, for USD this will set the tick size to 0.01 and the tick value to $0.01.
	  /// This typically matches the conventions of equities and bonds.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to derive the price information from </param>
	  /// <returns> the security price information </returns>
	  public static SecurityPriceInfo ofCurrencyMinorUnit(Currency currency)
	  {
		int digits = currency.MinorUnitDigits;
		double unitAmount = Math.Pow(10, -digits);
		return new SecurityPriceInfo(unitAmount, CurrencyAmount.of(currency, unitAmount), 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private SecurityPriceInfo(double tickSize, com.opengamma.strata.basics.currency.CurrencyAmount tickValue, double contractSize)
	  private SecurityPriceInfo(double tickSize, CurrencyAmount tickValue, double contractSize)
	  {
		ArgChecker.notNegativeOrZero(tickSize, "tickSize");
		JodaBeanUtils.notNull(tickValue, "tickValue");
		ArgChecker.notNegativeOrZero(contractSize, "contractSize");
		this.tickSize = tickSize;
		this.tickValue = tickValue;
		this.contractSize = contractSize;
		this.tradeUnitValue = (tickValue.Amount * contractSize) / tickSize;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new SecurityPriceInfo(tickSize, tickValue, contractSize);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency that the security is traded in.
	  /// <para>
	  /// The currency is derived from the tick value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DerivedProperty public com.opengamma.strata.basics.currency.Currency getCurrency()
	  public Currency Currency
	  {
		  get
		  {
			return tickValue.Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the monetary value of the specified quantity and price.
	  /// <para>
	  /// This calculates a monetary value using the stored price information.
	  /// For equities, this is the premium that will be paid.
	  /// For bonds, this will be the premium if the price specified is the <i>dirty</i> price.
	  /// For margined ETDs, the profit or loss per day is the monetary difference
	  /// between two calls to this method with the price on each day.
	  /// </para>
	  /// <para>
	  /// This returns <seealso cref="#calculateMonetaryValue(double, double)"/> as a <seealso cref="CurrencyAmount"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="quantity">  the quantity, such as the number of shares or number of future contracts </param>
	  /// <param name="price">  the price, typically from the market </param>
	  /// <returns> the monetary value combining the tick size, tick value, contract size, quantity and price. </returns>
	  public CurrencyAmount calculateMonetaryAmount(double quantity, double price)
	  {
		return CurrencyAmount.of(tickValue.Currency, calculateMonetaryValue(quantity, price));
	  }

	  /// <summary>
	  /// Calculates the monetary value of the specified quantity and price.
	  /// <para>
	  /// This calculates a monetary value using the stored price information.
	  /// For equities, this is the premium that will be paid.
	  /// For bonds, this will be the premium if the price specified is the <i>dirty</i> price.
	  /// For margined ETDs, the profit or loss per day is the monetary difference
	  /// between two calls to this method with the price on each day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="quantity">  the quantity, such as the number of shares or number of future contracts </param>
	  /// <param name="price">  the price, typically from the market </param>
	  /// <returns> the monetary value combining the tick size, tick value, contract size, quantity and price. </returns>
	  public double calculateMonetaryValue(double quantity, double price)
	  {
		return price * quantity * tradeUnitValue;
	  }

	  /// <summary>
	  /// Returns the value of a single tradeable unit of the security.
	  /// <para>
	  /// The monetary value of a position in a security is
	  /// <pre>tradeUnitValue * price * quantity</pre>
	  /// </para>
	  /// <para>
	  /// This value is normally derived from the tick size, tick value and contract size:
	  /// <pre>tradeUnitValue = tickValue * contractSize / tickSize</pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the value of a single tradeable unit of the security </returns>
	  public double TradeUnitValue
	  {
		  get
		  {
			return tradeUnitValue;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SecurityPriceInfo}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SecurityPriceInfo.Meta meta()
	  {
		return SecurityPriceInfo.Meta.INSTANCE;
	  }

	  static SecurityPriceInfo()
	  {
		MetaBean.register(SecurityPriceInfo.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override SecurityPriceInfo.Meta metaBean()
	  {
		return SecurityPriceInfo.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the size of each tick.
	  /// <para>
	  /// Tick size is the minimum movement in the price of the security.
	  /// For example, the price might move up or down in units of 0.01
	  /// It must be a positive decimal number.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double TickSize
	  {
		  get
		  {
			return tickSize;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the monetary value of one tick.
	  /// <para>
	  /// Tick value is the monetary value of the minimum movement in the price of the security.
	  /// When the price changes by one tick, this amount is gained or lost.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmount TickValue
	  {
		  get
		  {
			return tickValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the size of each contract.
	  /// <para>
	  /// Contract size is the quantity of the underlying present in each derivative contract.
	  /// For example, an equity option typically consists of 100 shares.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double ContractSize
	  {
		  get
		  {
			return contractSize;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  SecurityPriceInfo other = (SecurityPriceInfo) obj;
		  return JodaBeanUtils.equal(tickSize, other.tickSize) && JodaBeanUtils.equal(tickValue, other.tickValue) && JodaBeanUtils.equal(contractSize, other.contractSize);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tickSize);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tickValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(contractSize);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("SecurityPriceInfo{");
		buf.Append("tickSize").Append('=').Append(tickSize).Append(',').Append(' ');
		buf.Append("tickValue").Append('=').Append(tickValue).Append(',').Append(' ');
		buf.Append("contractSize").Append('=').Append(JodaBeanUtils.ToString(contractSize));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SecurityPriceInfo}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  tickSize_Renamed = DirectMetaProperty.ofImmutable(this, "tickSize", typeof(SecurityPriceInfo), Double.TYPE);
			  tickValue_Renamed = DirectMetaProperty.ofImmutable(this, "tickValue", typeof(SecurityPriceInfo), typeof(CurrencyAmount));
			  contractSize_Renamed = DirectMetaProperty.ofImmutable(this, "contractSize", typeof(SecurityPriceInfo), Double.TYPE);
			  currency_Renamed = DirectMetaProperty.ofDerived(this, "currency", typeof(SecurityPriceInfo), typeof(Currency));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "tickSize", "tickValue", "contractSize", "currency");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code tickSize} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> tickSize_Renamed;
		/// <summary>
		/// The meta-property for the {@code tickValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> tickValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code contractSize} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> contractSize_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "tickSize", "tickValue", "contractSize", "currency");
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
			case 1936822078: // tickSize
			  return tickSize_Renamed;
			case -85538348: // tickValue
			  return tickValue_Renamed;
			case -1402368973: // contractSize
			  return contractSize_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SecurityPriceInfo> builder()
		public override BeanBuilder<SecurityPriceInfo> builder()
		{
		  return new SecurityPriceInfo.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SecurityPriceInfo);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code tickSize} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> tickSize()
		{
		  return tickSize_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code tickValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> tickValue()
		{
		  return tickValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code contractSize} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> contractSize()
		{
		  return contractSize_Renamed;
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
			case 1936822078: // tickSize
			  return ((SecurityPriceInfo) bean).TickSize;
			case -85538348: // tickValue
			  return ((SecurityPriceInfo) bean).TickValue;
			case -1402368973: // contractSize
			  return ((SecurityPriceInfo) bean).ContractSize;
			case 575402001: // currency
			  return ((SecurityPriceInfo) bean).Currency;
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
	  /// The bean-builder for {@code SecurityPriceInfo}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SecurityPriceInfo>
	  {

		internal double tickSize;
		internal CurrencyAmount tickValue;
		internal double contractSize;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1936822078: // tickSize
			  return tickSize;
			case -85538348: // tickValue
			  return tickValue;
			case -1402368973: // contractSize
			  return contractSize;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1936822078: // tickSize
			  this.tickSize = (double?) newValue.Value;
			  break;
			case -85538348: // tickValue
			  this.tickValue = (CurrencyAmount) newValue;
			  break;
			case -1402368973: // contractSize
			  this.contractSize = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SecurityPriceInfo build()
		{
		  return new SecurityPriceInfo(tickSize, tickValue, contractSize);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("SecurityPriceInfo.Builder{");
		  buf.Append("tickSize").Append('=').Append(JodaBeanUtils.ToString(tickSize)).Append(',').Append(' ');
		  buf.Append("tickValue").Append('=').Append(JodaBeanUtils.ToString(tickValue)).Append(',').Append(' ');
		  buf.Append("contractSize").Append('=').Append(JodaBeanUtils.ToString(contractSize));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
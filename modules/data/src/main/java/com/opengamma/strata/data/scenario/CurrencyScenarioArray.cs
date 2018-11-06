using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyAmountArray = com.opengamma.strata.basics.currency.CurrencyAmountArray;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A currency-convertible scenario array for a single currency, holding one amount for each scenario.
	/// <para>
	/// This contains a list of amounts in a single currency, one amount for each scenario.
	/// The calculation runner is able to convert the currency of the values if required.
	/// </para>
	/// <para>
	/// This class uses less memory than an instance based on a list of <seealso cref="CurrencyAmount"/> instances.
	/// Internally, it stores the data using a single currency and a <seealso cref="DoubleArray"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurrencyScenarioArray implements ScenarioArray<com.opengamma.strata.basics.currency.CurrencyAmount>, ScenarioFxConvertible<CurrencyScenarioArray>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurrencyScenarioArray : ScenarioArray<CurrencyAmount>, ScenarioFxConvertible<CurrencyScenarioArray>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.CurrencyAmountArray amounts;
		private readonly CurrencyAmountArray amounts;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified currency and array of values.
	  /// </summary>
	  /// <param name="amounts">  the amounts, one for each scenario </param>
	  /// <returns> an instance with the specified currency and values </returns>
	  public static CurrencyScenarioArray of(CurrencyAmountArray amounts)
	  {
		return new CurrencyScenarioArray(amounts);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified currency and array of values.
	  /// </summary>
	  /// <param name="currency">  the currency of the values </param>
	  /// <param name="values">  the values, one for each scenario </param>
	  /// <returns> an instance with the specified currency and values </returns>
	  public static CurrencyScenarioArray of(Currency currency, DoubleArray values)
	  {
		return new CurrencyScenarioArray(CurrencyAmountArray.of(currency, values));
	  }

	  /// <summary>
	  /// Obtains an instance from the specified list of amounts.
	  /// <para>
	  /// All amounts must have the same currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amounts">  the amounts, one for each scenario </param>
	  /// <returns> an instance with the specified amounts </returns>
	  /// <exception cref="IllegalArgumentException"> if multiple currencies are found </exception>
	  public static CurrencyScenarioArray of(IList<CurrencyAmount> amounts)
	  {
		return new CurrencyScenarioArray(CurrencyAmountArray.of(amounts));
	  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the scenario index and returns the {@code CurrencyAmount} for that index.
	  /// </para>
	  /// <para>
	  /// In some cases it may be possible to specify the currency with a function providing a {@code double}.
	  /// To do this, use <seealso cref="DoubleArray#of(int, java.util.function.IntToDoubleFunction)"/> and
	  /// then call <seealso cref="#of(Currency, DoubleArray)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the number of elements, at least size one </param>
	  /// <param name="amountFunction">  the function used to obtain each amount </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
	  public static CurrencyScenarioArray of(int size, System.Func<int, CurrencyAmount> amountFunction)
	  {
		return new CurrencyScenarioArray(CurrencyAmountArray.of(size, amountFunction));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency.
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return amounts.Currency;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public int ScenarioCount
	  {
		  get
		  {
			return amounts.size();
		  }
	  }

	  public CurrencyAmount get(int index)
	  {
		return amounts.get(index);
	  }

	  public override Stream<CurrencyAmount> stream()
	  {
		return amounts.stream();
	  }

	  public CurrencyScenarioArray convertedTo(Currency reportingCurrency, ScenarioFxRateProvider fxRateProvider)
	  {
		if (Currency.Equals(reportingCurrency))
		{
		  return this;
		}
		if (fxRateProvider.ScenarioCount != amounts.size())
		{
		  throw new System.ArgumentException(Messages.format("Expected {} FX rates but received {}", amounts.size(), fxRateProvider.ScenarioCount));
		}
		DoubleArray convertedValues = amounts.Values.mapWithIndex((i, v) => v * fxRateProvider.fxRate(Currency, reportingCurrency, i));
		return of(reportingCurrency, convertedValues);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new array containing the values from this array added to the values in the other array.
	  /// <para>
	  /// The amounts are added to the matching element in this array.
	  /// The currency must be the same as the currency of this array.
	  /// The arrays must have the same size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another array of multiple currency values. </param>
	  /// <returns> a new array containing the values from this array added to the values in the other array </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes or different currencies </exception>
	  public CurrencyScenarioArray plus(CurrencyScenarioArray other)
	  {
		return CurrencyScenarioArray.of(amounts.plus(other.amounts));
	  }

	  /// <summary>
	  /// Returns a new array containing the values from this array with the specified amount added.
	  /// <para>
	  /// The amount is added to each element in this array.
	  /// The currency must be the same as the currency of this array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to add </param>
	  /// <returns> a new array containing the values from this array with the specified amount added </returns>
	  /// <exception cref="IllegalArgumentException"> if the array and the amount have different currencies </exception>
	  public CurrencyScenarioArray plus(CurrencyAmount amount)
	  {
		return CurrencyScenarioArray.of(amounts.plus(amount));
	  }

	  /// <summary>
	  /// Returns a new array containing the values from this array with the values from the other array subtracted.
	  /// <para>
	  /// The amounts are subtracted from the matching element in this array.
	  /// The currency must be the same as the currency of this array.
	  /// The arrays must have the same size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another array of multiple currency values. </param>
	  /// <returns> a new array containing the values from this array with the values from the other array subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes or different currencies </exception>
	  public CurrencyScenarioArray minus(CurrencyScenarioArray other)
	  {
		return CurrencyScenarioArray.of(amounts.minus(other.amounts));
	  }

	  /// <summary>
	  /// Returns a new array containing the values from this array with the specified amount subtracted.
	  /// <para>
	  /// The amount is subtracted from each element in this array.
	  /// The currency must be the same as the currency of this array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to subtract </param>
	  /// <returns> a new array containing the values from this array with the specified amount subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the array and the amount have different currencies </exception>
	  public CurrencyScenarioArray minus(CurrencyAmount amount)
	  {
		return CurrencyScenarioArray.of(amounts.minus(amount));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurrencyScenarioArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurrencyScenarioArray.Meta meta()
	  {
		return CurrencyScenarioArray.Meta.INSTANCE;
	  }

	  static CurrencyScenarioArray()
	  {
		MetaBean.register(CurrencyScenarioArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CurrencyScenarioArray(CurrencyAmountArray amounts)
	  {
		JodaBeanUtils.notNull(amounts, "amounts");
		this.amounts = amounts;
	  }

	  public override CurrencyScenarioArray.Meta metaBean()
	  {
		return CurrencyScenarioArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency amounts, one per scenario. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyAmountArray Amounts
	  {
		  get
		  {
			return amounts;
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
		  CurrencyScenarioArray other = (CurrencyScenarioArray) obj;
		  return JodaBeanUtils.equal(amounts, other.amounts);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(amounts);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("CurrencyScenarioArray{");
		buf.Append("amounts").Append('=').Append(JodaBeanUtils.ToString(amounts));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurrencyScenarioArray}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  amounts_Renamed = DirectMetaProperty.ofImmutable(this, "amounts", typeof(CurrencyScenarioArray), typeof(CurrencyAmountArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "amounts");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code amounts} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmountArray> amounts_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "amounts");
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
			case -879772901: // amounts
			  return amounts_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurrencyScenarioArray> builder()
		public override BeanBuilder<CurrencyScenarioArray> builder()
		{
		  return new CurrencyScenarioArray.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurrencyScenarioArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code amounts} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmountArray> amounts()
		{
		  return amounts_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -879772901: // amounts
			  return ((CurrencyScenarioArray) bean).Amounts;
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
	  /// The bean-builder for {@code CurrencyScenarioArray}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurrencyScenarioArray>
	  {

		internal CurrencyAmountArray amounts;

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
			case -879772901: // amounts
			  return amounts;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -879772901: // amounts
			  this.amounts = (CurrencyAmountArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurrencyScenarioArray build()
		{
		  return new CurrencyScenarioArray(amounts);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("CurrencyScenarioArray.Builder{");
		  buf.Append("amounts").Append('=').Append(JodaBeanUtils.ToString(amounts));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
using System;
using System.Collections;
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
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MultiCurrencyAmountArray = com.opengamma.strata.basics.currency.MultiCurrencyAmountArray;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A currency-convertible scenario array for multi-currency amounts, holding one amount for each scenario.
	/// <para>
	/// This contains a list of amounts in a multiple currencies, one amount for each scenario.
	/// The calculation runner is able to convert the currency of the values if required.
	/// </para>
	/// <para>
	/// This class uses less memory than an instance based on a list of <seealso cref="MultiCurrencyAmount"/> instances.
	/// Internally, it stores the data using a map of currency to <seealso cref="DoubleArray"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class MultiCurrencyScenarioArray implements ScenarioArray<com.opengamma.strata.basics.currency.MultiCurrencyAmount>, ScenarioFxConvertible<CurrencyScenarioArray>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class MultiCurrencyScenarioArray : ScenarioArray<MultiCurrencyAmount>, ScenarioFxConvertible<CurrencyScenarioArray>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.MultiCurrencyAmountArray amounts;
		private readonly MultiCurrencyAmountArray amounts;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified currency and array of values.
	  /// </summary>
	  /// <param name="amounts">  the amounts, one for each scenario </param>
	  /// <returns> an instance with the specified currency and values </returns>
	  public static MultiCurrencyScenarioArray of(MultiCurrencyAmountArray amounts)
	  {
		return new MultiCurrencyScenarioArray(amounts);
	  }

	  /// <summary>
	  /// Returns an instance containing the values from the amounts.
	  /// </summary>
	  /// <param name="amounts">  the amounts, one for each scenario </param>
	  /// <returns> an instance containing the values from the list of amounts </returns>
	  public static MultiCurrencyScenarioArray of(params MultiCurrencyAmount[] amounts)
	  {
		return of(Arrays.asList(amounts));
	  }

	  /// <summary>
	  /// Returns an instance containing the values from the list of amounts.
	  /// </summary>
	  /// <param name="amounts">  the amounts, one for each scenario </param>
	  /// <returns> an instance containing the values from the list of amounts </returns>
	  public static MultiCurrencyScenarioArray of(IList<MultiCurrencyAmount> amounts)
	  {
		return new MultiCurrencyScenarioArray(MultiCurrencyAmountArray.of(amounts));
	  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the scenario index and returns the value for that index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <param name="amountFunction">  the function used to obtain each amount </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
	  public static MultiCurrencyScenarioArray of(int size, System.Func<int, MultiCurrencyAmount> amountFunction)
	  {
		return new MultiCurrencyScenarioArray(MultiCurrencyAmountArray.of(size, amountFunction));
	  }

	  /// <summary>
	  /// Returns an instance containing the values from a map of amounts with the same number of elements in each array.
	  /// </summary>
	  /// <param name="values">  map of currencies to values </param>
	  /// <returns> an instance containing the values from the map </returns>
	  public static MultiCurrencyScenarioArray of(IDictionary<Currency, DoubleArray> values)
	  {
		return new MultiCurrencyScenarioArray(MultiCurrencyAmountArray.of(values));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of currencies for which this object contains values.
	  /// </summary>
	  /// <returns> the set of currencies for which this object contains values </returns>
	  public ISet<Currency> Currencies
	  {
		  get
		  {
			return amounts.Currencies;
		  }
	  }

	  /// <summary>
	  /// Returns the values for the specified currency, throws an exception if there are no values for the currency.
	  /// </summary>
	  /// <param name="currency">  the currency for which values are required </param>
	  /// <returns> the values for the specified currency, throws an exception if there are none </returns>
	  /// <exception cref="IllegalArgumentException"> if there are no values for the currency </exception>
	  public DoubleArray getValues(Currency currency)
	  {
		DoubleArray currencyValues = amounts.getValues(currency);
		if (currencyValues == null)
		{
		  throw new System.ArgumentException("No values available for " + currency);
		}
		return currencyValues;
	  }

	  /// <summary>
	  /// Returns the number of currency values for each currency.
	  /// </summary>
	  /// <returns> the number of currency values for each currency </returns>
	  public int ScenarioCount
	  {
		  get
		  {
			return amounts.size();
		  }
	  }

	  /// <summary>
	  /// Returns a <seealso cref="MultiCurrencyAmount"/> at the specified index.
	  /// <para>
	  /// This method is not very efficient for large sizes as a new object must be created at each index.
	  /// Consider using <seealso cref="#getValues(Currency)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index that should be returned </param>
	  /// <returns> a multi currency amount containing the currency values at the specified index </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public MultiCurrencyAmount get(int index)
	  {
		IList<CurrencyAmount> currencyAmounts = amounts.Currencies.Select(ccy => CurrencyAmount.of(ccy, amounts.getValues(ccy).get(index))).ToList();
		return MultiCurrencyAmount.of(currencyAmounts);
	  }

	  /// <summary>
	  /// Returns a stream of <seealso cref="MultiCurrencyAmount"/> instances containing the values from this object.
	  /// <para>
	  /// This method is not very efficient for large sizes as a new object must be created for each value.
	  /// Consider using <seealso cref="#getValues(Currency)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream of multi currency amounts containing the currency values from this object </returns>
	  public override Stream<MultiCurrencyAmount> stream()
	  {
		return amounts.stream();
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyScenarioArray convertedTo(Currency reportingCurrency, ScenarioFxRateProvider fxRateProvider)
	  {
		int size = ScenarioCount;
		if (fxRateProvider.ScenarioCount != size)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} FX rates but received {}", size, fxRateProvider.ScenarioCount));
		}

		double[] singleCurrencyValues = new double[size];
		foreach (KeyValuePair<Currency, DoubleArray> entry in amounts.Values.entrySet())
		{
		  Currency currency = entry.Key;
		  DoubleArray currencyValues = entry.Value;

		  for (int i = 0; i < size; i++)
		  {
			double convertedValue = currencyValues.get(i) * fxRateProvider.fxRate(currency, reportingCurrency, i);
			singleCurrencyValues[i] += convertedValue;
		  }
		}
		return CurrencyScenarioArray.of(reportingCurrency, DoubleArray.ofUnsafe(singleCurrencyValues));
	  }

	  /// <summary>
	  /// Returns a multi currency scenario array representing the total of the input arrays.
	  /// <para>
	  /// If the input contains the same currency more than once, the amounts are added together.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="arrays">  the amount arrays </param>
	  /// <returns> the total amounts </returns>
	  public static MultiCurrencyScenarioArray total(IEnumerable<CurrencyScenarioArray> arrays)
	  {
		return Guavate.stream(arrays).collect(toMultiCurrencyScenarioArray());
	  }

	  /// <summary>
	  /// Returns a collector which creates a multi currency scenario array by combining a stream of
	  /// currency scenario arrays.
	  /// <para>
	  /// The arrays in the stream must all have the same length.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the collector </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static java.util.stream.Collector<CurrencyScenarioArray, ?, MultiCurrencyScenarioArray> toMultiCurrencyScenarioArray()
	  public static Collector<CurrencyScenarioArray, ?, MultiCurrencyScenarioArray> toMultiCurrencyScenarioArray()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return Collector.of<CurrencyScenarioArray, IDictionary<Currency, CurrencyAmountArray>, MultiCurrencyScenarioArray>(Hashtable::new, (map, ca) => map.merge(ca.Currency, ca.Amounts, CurrencyAmountArray::plus), (map1, map2) =>
		{
		map2.values().forEach((ca2) => map1.merge(ca2.Currency, ca2, CurrencyAmountArray.plus));
		return map1;
		}, map =>
		{
		IDictionary<Currency, DoubleArray> currencyArrayMap = MapStream.of(map).mapValues(caa => caa.Values).toMap();
		return MultiCurrencyScenarioArray.of(currencyArrayMap);
	}, UNORDERED);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code MultiCurrencyScenarioArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static MultiCurrencyScenarioArray.Meta meta()
	  {
		return MultiCurrencyScenarioArray.Meta.INSTANCE;
	  }

	  static MultiCurrencyScenarioArray()
	  {
		MetaBean.register(MultiCurrencyScenarioArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private MultiCurrencyScenarioArray(MultiCurrencyAmountArray amounts)
	  {
		JodaBeanUtils.notNull(amounts, "amounts");
		this.amounts = amounts;
	  }

	  public override MultiCurrencyScenarioArray.Meta metaBean()
	  {
		return MultiCurrencyScenarioArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the multi-currency amounts, one per scenario. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MultiCurrencyAmountArray Amounts
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
		  MultiCurrencyScenarioArray other = (MultiCurrencyScenarioArray) obj;
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
		buf.Append("MultiCurrencyScenarioArray{");
		buf.Append("amounts").Append('=').Append(JodaBeanUtils.ToString(amounts));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code MultiCurrencyScenarioArray}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  amounts_Renamed = DirectMetaProperty.ofImmutable(this, "amounts", typeof(MultiCurrencyScenarioArray), typeof(MultiCurrencyAmountArray));
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
		internal MetaProperty<MultiCurrencyAmountArray> amounts_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends MultiCurrencyScenarioArray> builder()
		public override BeanBuilder<MultiCurrencyScenarioArray> builder()
		{
		  return new MultiCurrencyScenarioArray.Builder();
		}

		public override Type beanType()
		{
		  return typeof(MultiCurrencyScenarioArray);
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
		public MetaProperty<MultiCurrencyAmountArray> amounts()
		{
		  return amounts_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -879772901: // amounts
			  return ((MultiCurrencyScenarioArray) bean).Amounts;
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
	  /// The bean-builder for {@code MultiCurrencyScenarioArray}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<MultiCurrencyScenarioArray>
	  {

		internal MultiCurrencyAmountArray amounts;

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
			  this.amounts = (MultiCurrencyAmountArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override MultiCurrencyScenarioArray build()
		{
		  return new MultiCurrencyScenarioArray(amounts);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("MultiCurrencyScenarioArray.Builder{");
		  buf.Append("amounts").Append('=').Append(JodaBeanUtils.ToString(amounts));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
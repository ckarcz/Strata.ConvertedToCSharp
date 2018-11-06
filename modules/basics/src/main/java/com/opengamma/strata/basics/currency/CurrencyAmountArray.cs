using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.ensureOnlyOne;


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

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// An array of currency amounts with the same currency.
	/// <para>
	/// This represents an array of <seealso cref="CurrencyAmount"/> in a single currency.
	/// Internally, it stores the data using a single <seealso cref="Currency"/> and a <seealso cref="DoubleArray"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CurrencyAmountArray implements FxConvertible<CurrencyAmountArray>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CurrencyAmountArray : FxConvertible<CurrencyAmountArray>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The values.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray values;
	  private readonly DoubleArray values;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified currency and array of values.
	  /// </summary>
	  /// <param name="currency">  the currency of the values </param>
	  /// <param name="values">  the values </param>
	  /// <returns> an instance with the specified currency and values </returns>
	  public static CurrencyAmountArray of(Currency currency, DoubleArray values)
	  {
		return new CurrencyAmountArray(currency, values);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified list of amounts.
	  /// <para>
	  /// All amounts must have the same currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amounts">  the amounts, at least size one </param>
	  /// <returns> an instance with the specified amounts </returns>
	  /// <exception cref="IllegalArgumentException"> if multiple currencies are found </exception>
	  public static CurrencyAmountArray of(IList<CurrencyAmount> amounts)
	  {
		Currency currency = amounts.Select(ca => ca.Currency).Distinct().Aggregate(ensureOnlyOne()).get();
		double[] values = amounts.Select(ca => ca.Amount).ToArray();
		return new CurrencyAmountArray(currency, DoubleArray.ofUnsafe(values));
	  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the index and returns the {@code CurrencyAmount} for that index.
	  /// </para>
	  /// <para>
	  /// In some cases it may be possible to specify the currency with a function providing a {@code double}.
	  /// To do this, use <seealso cref="DoubleArray#of(int, java.util.function.IntToDoubleFunction)"/> and
	  /// then call <seealso cref="#of(Currency, DoubleArray)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the number of elements, at least size one </param>
	  /// <param name="valueFunction">  the function used to obtain each value </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
	  public static CurrencyAmountArray of(int size, System.Func<int, CurrencyAmount> valueFunction)
	  {
		ArgChecker.notNegativeOrZero(size, "size");
		double[] array = new double[size];
		CurrencyAmount ca0 = valueFunction(0);
		Currency currency = ca0.Currency;
		array[0] = ca0.Amount;
		for (int i = 1; i < size; i++)
		{
		  CurrencyAmount ca = valueFunction(i);
		  if (!ca.Currency.Equals(currency))
		  {
			throw new System.ArgumentException(Messages.format("Currencies differ: {} and {}", currency, ca.Currency));
		  }
		  array[i] = ca.Amount;
		}
		return new CurrencyAmountArray(currency, DoubleArray.ofUnsafe(array));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the size of the array.
	  /// </summary>
	  /// <returns> the array size </returns>
	  public int size()
	  {
		return values.size();
	  }

	  /// <summary>
	  /// Gets the amount at the specified index.
	  /// </summary>
	  /// <param name="index">  the zero-based index to retrieve </param>
	  /// <returns> the amount at the specified index </returns>
	  public CurrencyAmount get(int index)
	  {
		return CurrencyAmount.of(currency, values.get(index));
	  }

	  /// <summary>
	  /// Returns a stream of the amounts.
	  /// </summary>
	  /// <returns> a stream of the amounts </returns>
	  public Stream<CurrencyAmount> stream()
	  {
		return values.stream().mapToObj(amount => CurrencyAmount.of(currency, amount));
	  }

	  public CurrencyAmountArray convertedTo(Currency resultCurrency, FxRateProvider fxRateProvider)
	  {
		if (currency.Equals(resultCurrency))
		{
		  return this;
		}
		double fxRate = fxRateProvider.fxRate(currency, resultCurrency);
		DoubleArray convertedValues = values.multipliedBy(fxRate);
		return new CurrencyAmountArray(resultCurrency, convertedValues);
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
	  public CurrencyAmountArray plus(CurrencyAmountArray other)
	  {
		if (other.size() != size())
		{
		  throw new System.ArgumentException(Messages.format("Sizes must be equal, this size is {}, other size is {}", size(), other.size()));
		}
		if (!other.currency.Equals(currency))
		{
		  throw new System.ArgumentException(Messages.format("Currencies must be equal, this currency is {}, other currency is {}", currency, other.currency));
		}
		return CurrencyAmountArray.of(currency, values.plus(other.values));
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
	  public CurrencyAmountArray plus(CurrencyAmount amount)
	  {
		if (!amount.Currency.Equals(currency))
		{
		  throw new System.ArgumentException(Messages.format("Currencies must be equal, this currency is {}, other currency is {}", currency, amount.Currency));
		}
		return CurrencyAmountArray.of(currency, values.plus(amount.Amount));
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
	  public CurrencyAmountArray minus(CurrencyAmountArray other)
	  {
		if (other.size() != size())
		{
		  throw new System.ArgumentException(Messages.format("Sizes must be equal, this size is {}, other size is {}", size(), other.size()));
		}
		if (!other.currency.Equals(currency))
		{
		  throw new System.ArgumentException(Messages.format("Currencies must be equal, this currency is {}, other currency is {}", currency, other.currency));
		}
		return CurrencyAmountArray.of(currency, values.minus(other.values));
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
	  public CurrencyAmountArray minus(CurrencyAmount amount)
	  {
		if (!amount.Currency.Equals(currency))
		{
		  throw new System.ArgumentException(Messages.format("Currencies must be equal, this currency is {}, other currency is {}", currency, amount.Currency));
		}
		return CurrencyAmountArray.of(currency, values.minus(amount.Amount));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurrencyAmountArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CurrencyAmountArray.Meta meta()
	  {
		return CurrencyAmountArray.Meta.INSTANCE;
	  }

	  static CurrencyAmountArray()
	  {
		MetaBean.register(CurrencyAmountArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CurrencyAmountArray(Currency currency, DoubleArray values)
	  {
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(values, "values");
		this.currency = currency;
		this.values = values;
	  }

	  public override CurrencyAmountArray.Meta metaBean()
	  {
		return CurrencyAmountArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency.
	  /// All amounts have the same currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the values. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Values
	  {
		  get
		  {
			return values;
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
		  CurrencyAmountArray other = (CurrencyAmountArray) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(values, other.values);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(values);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CurrencyAmountArray{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CurrencyAmountArray}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(CurrencyAmountArray), typeof(Currency));
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(CurrencyAmountArray), typeof(DoubleArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "values");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> values_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "values");
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
			case 575402001: // currency
			  return currency_Renamed;
			case -823812830: // values
			  return values_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CurrencyAmountArray> builder()
		public override BeanBuilder<CurrencyAmountArray> builder()
		{
		  return new CurrencyAmountArray.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CurrencyAmountArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> values()
		{
		  return values_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((CurrencyAmountArray) bean).Currency;
			case -823812830: // values
			  return ((CurrencyAmountArray) bean).Values;
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
	  /// The bean-builder for {@code CurrencyAmountArray}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CurrencyAmountArray>
	  {

		internal Currency currency;
		internal DoubleArray values;

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
			case 575402001: // currency
			  return currency;
			case -823812830: // values
			  return values;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			case -823812830: // values
			  this.values = (DoubleArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CurrencyAmountArray build()
		{
		  return new CurrencyAmountArray(currency, values);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CurrencyAmountArray.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
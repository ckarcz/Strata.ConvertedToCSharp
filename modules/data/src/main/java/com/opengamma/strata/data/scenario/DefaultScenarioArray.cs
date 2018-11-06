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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.zipWithIndex;


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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A scenario array holding one value for each scenario.
	/// <para>
	/// This contains a list of values, one value for each scenario.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of each individual value </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") final class DefaultScenarioArray<T> implements ScenarioArray<T>, ScenarioFxConvertible<ScenarioArray<?>>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultScenarioArray<T> : ScenarioArray<T>, ScenarioFxConvertible<ScenarioArray<JavaToDotNetGenericWildcard>>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<T> values;
		private readonly ImmutableList<T> values;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified array of values.
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="values">  the values, one value for each scenario </param>
	  /// <returns> an instance with the specified values </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static <T> DefaultScenarioArray<T> of(T... values)
	  public static DefaultScenarioArray<T> of<T>(params T[] values)
	  {
		return new DefaultScenarioArray<T>(ImmutableList.copyOf(values));
	  }

	  /// <summary>
	  /// Obtains an instance from the specified list of values.
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="values">  the values, one value for each scenario </param>
	  /// <returns> an instance with the specified values </returns>
	  public static DefaultScenarioArray<T> of<T>(IList<T> values)
	  {
		return new DefaultScenarioArray<T>(values);
	  }

	  /// <summary>
	  /// Obtains an instance using a function to create the entries.
	  /// <para>
	  /// The function is passed the scenario index and returns the value for that index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="valueFunction">  the function used to obtain each value </param>
	  /// <returns> an instance initialized using the function </returns>
	  /// <exception cref="IllegalArgumentException"> is size is zero or less </exception>
	  public static DefaultScenarioArray<T> of<T>(int scenarioCount, System.Func<int, T> valueFunction)
	  {
		ArgChecker.notNegativeOrZero(scenarioCount, "scenarioCount");
		ImmutableList.Builder<T> builder = ImmutableList.builder();
		for (int i = 0; i < scenarioCount; i++)
		{
		  builder.add(valueFunction(i));
		}
		return new DefaultScenarioArray<T>(builder.build());
	  }

	  //-------------------------------------------------------------------------
	  public int ScenarioCount
	  {
		  get
		  {
			return values.size();
		  }
	  }

	  public T get(int index)
	  {
		return values.get(index);
	  }

	  public override Stream<T> stream()
	  {
		return values.stream();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public ScenarioArray<?> convertedTo(com.opengamma.strata.basics.currency.Currency resultCurrency, ScenarioFxRateProvider fxRateProvider)
	  public ScenarioArray<object> convertedTo(Currency resultCurrency, ScenarioFxRateProvider fxRateProvider)
	  {
		int scenarioCount = ScenarioCount;
		if (fxRateProvider.ScenarioCount != scenarioCount)
		{
		  throw new System.ArgumentException(Messages.format("Expected {} FX rates but received {}", scenarioCount, fxRateProvider.ScenarioCount));
		}
		ImmutableList<object> converted = zipWithIndex(values.stream()).map(tp => convert(resultCurrency, fxRateProvider, tp.First, tp.Second)).collect(toImmutableList());
		return DefaultScenarioArray.of(converted);
	  }

	  // convert value if possible
	  private object convert(Currency reportingCurrency, ScenarioFxRateProvider fxRateProvider, object @base, int index)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: if (super instanceof com.opengamma.strata.basics.currency.FxConvertible<?>)
		if (@base is FxConvertible<object>)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.basics.currency.FxConvertible<?> convertible = (com.opengamma.strata.basics.currency.FxConvertible<?>) super;
		  FxConvertible<object> convertible = (FxConvertible<object>) @base;
		  return convertible.convertedTo(reportingCurrency, fxRateProvider.fxRateProvider(index));
		}
		return @base;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultScenarioArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static DefaultScenarioArray.Meta meta()
	  public static DefaultScenarioArray.Meta meta()
	  {
		return DefaultScenarioArray.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code DefaultScenarioArray}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> DefaultScenarioArray.Meta<R> metaDefaultScenarioArray(Class<R> cls)
	  public static DefaultScenarioArray.Meta<R> metaDefaultScenarioArray<R>(Type<R> cls)
	  {
		return DefaultScenarioArray.Meta.INSTANCE;
	  }

	  static DefaultScenarioArray()
	  {
		MetaBean.register(DefaultScenarioArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultScenarioArray(IList<T> values)
	  {
		JodaBeanUtils.notNull(values, "values");
		this.values = ImmutableList.copyOf(values);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public DefaultScenarioArray.Meta<T> metaBean()
	  public override DefaultScenarioArray.Meta<T> metaBean()
	  {
		return DefaultScenarioArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the values, one per scenario. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<T> Values
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
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: DefaultScenarioArray<?> other = (DefaultScenarioArray<?>) obj;
		  DefaultScenarioArray<object> other = (DefaultScenarioArray<object>) obj;
		  return JodaBeanUtils.equal(values, other.values);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(values);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("DefaultScenarioArray{");
		buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultScenarioArray}. </summary>
	  /// @param <T>  the type </param>
	  internal sealed class Meta<T> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  values_Renamed = DirectMetaProperty.ofImmutable(this, "values", typeof(DefaultScenarioArray), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "values");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") static final Meta INSTANCE = new Meta();
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code values} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<T>> values = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "values", DefaultScenarioArray.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<T>> values_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "values");
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
			case -823812830: // values
			  return values_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DefaultScenarioArray<T>> builder()
		public override BeanBuilder<DefaultScenarioArray<T>> builder()
		{
		  return new DefaultScenarioArray.Builder<>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(DefaultScenarioArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code values} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<T>> values()
		{
		  return values_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823812830: // values
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((DefaultScenarioArray<?>) bean).getValues();
			  return ((DefaultScenarioArray<object>) bean).Values;
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
	  /// The bean-builder for {@code DefaultScenarioArray}. </summary>
	  /// @param <T>  the type </param>
	  private sealed class Builder<T> : DirectPrivateBeanBuilder<DefaultScenarioArray<T>>
	  {

		internal IList<T> values = ImmutableList.of();

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
			case -823812830: // values
			  return values;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder<T> set(String propertyName, Object newValue)
		public override Builder<T> set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -823812830: // values
			  this.values = (IList<T>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DefaultScenarioArray<T> build()
		{
		  return new DefaultScenarioArray<T>(values);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("DefaultScenarioArray.Builder{");
		  buf.Append("values").Append('=').Append(JodaBeanUtils.ToString(values));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
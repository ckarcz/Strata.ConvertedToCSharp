using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


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
	/// A scenario array holding one value that is valid for all scenarios.
	/// <para>
	/// This contains a single value where the same value is the result of each scenario.
	/// The calculation runner will not attempt to convert the currency of the value.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the result </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") final class SingleScenarioArray<T> implements ScenarioArray<T>, ScenarioFxConvertible<ScenarioArray<?>>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class SingleScenarioArray<T> : ScenarioArray<T>, ScenarioFxConvertible<ScenarioArray<JavaToDotNetGenericWildcard>>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final int scenarioCount;
		private readonly int scenarioCount;
	  /// <summary>
	  /// The single value that applies to all scenarios.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final T value;
	  private readonly T value;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a single value and scenario count.
	  /// <para>
	  /// The single value is valid for each scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="scenarioCount">  the number of scenarios </param>
	  /// <param name="value">  the single value valid for all scenarios </param>
	  /// <returns> an instance with the specified value and count </returns>
	  public static SingleScenarioArray<T> of<T>(int scenarioCount, T value)
	  {
		return new SingleScenarioArray<T>(scenarioCount, value);
	  }

	  //-------------------------------------------------------------------------
	  public T get(int index)
	  {
		ArgChecker.inRange(index, 0, scenarioCount, "index");
		return value;
	  }

	  public override Stream<T> stream()
	  {
		return Collections.nCopies(scenarioCount, value).stream();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public ScenarioArray<?> convertedTo(com.opengamma.strata.basics.currency.Currency reportingCurrency, ScenarioFxRateProvider fxRateProvider)
	  public ScenarioArray<object> convertedTo(Currency reportingCurrency, ScenarioFxRateProvider fxRateProvider)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: if (value instanceof com.opengamma.strata.basics.currency.FxConvertible<?>)
		if (value is FxConvertible<object>)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.basics.currency.FxConvertible<?> convertible = (com.opengamma.strata.basics.currency.FxConvertible<?>) value;
		  FxConvertible<object> convertible = (FxConvertible<object>) value;
		  if (fxRateProvider.ScenarioCount != scenarioCount)
		  {
			throw new System.ArgumentException(Messages.format("Expected {} FX rates but received {}", scenarioCount, fxRateProvider.ScenarioCount));
		  }
		  ImmutableList<object> converted = IntStream.range(0, scenarioCount).mapToObj(i => convertible.convertedTo(reportingCurrency, fxRateProvider.fxRateProvider(i))).collect(toImmutableList());
		  return DefaultScenarioArray.of(converted);
		}
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SingleScenarioArray}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static SingleScenarioArray.Meta meta()
	  public static SingleScenarioArray.Meta meta()
	  {
		return SingleScenarioArray.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code SingleScenarioArray}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> SingleScenarioArray.Meta<R> metaSingleScenarioArray(Class<R> cls)
	  public static SingleScenarioArray.Meta<R> metaSingleScenarioArray<R>(Type<R> cls)
	  {
		return SingleScenarioArray.Meta.INSTANCE;
	  }

	  static SingleScenarioArray()
	  {
		MetaBean.register(SingleScenarioArray.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SingleScenarioArray(int scenarioCount, T value)
	  {
		JodaBeanUtils.notNull(scenarioCount, "scenarioCount");
		JodaBeanUtils.notNull(value, "value");
		this.scenarioCount = scenarioCount;
		this.value = value;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public SingleScenarioArray.Meta<T> metaBean()
	  public override SingleScenarioArray.Meta<T> metaBean()
	  {
		return SingleScenarioArray.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of scenarios. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public int ScenarioCount
	  {
		  get
		  {
			return scenarioCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the single value that applies to all scenarios. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public T Value
	  {
		  get
		  {
			return value;
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
//ORIGINAL LINE: SingleScenarioArray<?> other = (SingleScenarioArray<?>) obj;
		  SingleScenarioArray<object> other = (SingleScenarioArray<object>) obj;
		  return (scenarioCount == other.scenarioCount) && JodaBeanUtils.equal(value, other.value);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(scenarioCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("SingleScenarioArray{");
		buf.Append("scenarioCount").Append('=').Append(scenarioCount).Append(',').Append(' ');
		buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SingleScenarioArray}. </summary>
	  /// @param <T>  the type </param>
	  internal sealed class Meta<T> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  scenarioCount_Renamed = DirectMetaProperty.ofImmutable(this, "scenarioCount", typeof(SingleScenarioArray), Integer.TYPE);
			  value_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "value", typeof(SingleScenarioArray), typeof(object));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "scenarioCount", "value");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") static final Meta INSTANCE = new Meta();
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code scenarioCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> scenarioCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<T> value = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "value", SingleScenarioArray.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<T> value_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "scenarioCount", "value");
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
			case -1203198113: // scenarioCount
			  return scenarioCount_Renamed;
			case 111972721: // value
			  return value_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SingleScenarioArray<T>> builder()
		public override BeanBuilder<SingleScenarioArray<T>> builder()
		{
		  return new SingleScenarioArray.Builder<>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(SingleScenarioArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code scenarioCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> scenarioCount()
		{
		  return scenarioCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<T> value()
		{
		  return value_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1203198113: // scenarioCount
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((SingleScenarioArray<?>) bean).getScenarioCount();
			  return ((SingleScenarioArray<object>) bean).ScenarioCount;
			case 111972721: // value
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((SingleScenarioArray<?>) bean).getValue();
			  return ((SingleScenarioArray<object>) bean).Value;
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
	  /// The bean-builder for {@code SingleScenarioArray}. </summary>
	  /// @param <T>  the type </param>
	  private sealed class Builder<T> : DirectPrivateBeanBuilder<SingleScenarioArray<T>>
	  {

		internal int scenarioCount;
		internal T value;

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
			case -1203198113: // scenarioCount
			  return scenarioCount;
			case 111972721: // value
			  return value;
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
			case -1203198113: // scenarioCount
			  this.scenarioCount = (int?) newValue.Value;
			  break;
			case 111972721: // value
			  this.value = (T) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SingleScenarioArray<T> build()
		{
		  return new SingleScenarioArray<T>(scenarioCount, value);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("SingleScenarioArray.Builder{");
		  buf.Append("scenarioCount").Append('=').Append(JodaBeanUtils.ToString(scenarioCount)).Append(',').Append(' ');
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
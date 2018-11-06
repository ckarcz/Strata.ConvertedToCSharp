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


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using ObjIntFunction = com.opengamma.strata.collect.function.ObjIntFunction;

	/// <summary>
	/// A market data box containing an object which can provide market data for multiple scenarios.
	/// </summary>
	/// @param <T>  the type of data held in the box </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition final class ScenarioMarketDataBox<T> implements org.joda.beans.ImmutableBean, MarketDataBox<T>, java.io.Serializable
	[Serializable]
	internal sealed class ScenarioMarketDataBox<T> : ImmutableBean, MarketDataBox<T>
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ScenarioArray<T> value;
		private readonly ScenarioArray<T> value;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance containing the specified value.
	  /// </summary>
	  /// @param <T> the type of the market data value </param>
	  /// <param name="value">  the market data value which can provide data for multiple scenarios </param>
	  /// <returns> a market data box containing the value </returns>
	  public static ScenarioMarketDataBox<T> of<T>(ScenarioArray<T> value)
	  {
		return new ScenarioMarketDataBox<T>(value);
	  }

	  /// <summary>
	  /// Obtains an instance containing the specified market data values, one for each scenario.
	  /// </summary>
	  /// @param <T> the type of the market data value </param>
	  /// <param name="values">  the single market data values, one for each scenario </param>
	  /// <returns> a scenario market data box containing single market data values, one for each scenario </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static <T> ScenarioMarketDataBox<T> of(T... values)
	  public static ScenarioMarketDataBox<T> of<T>(params T[] values)
	  {
		return new ScenarioMarketDataBox<T>(ScenarioArray.of(values));
	  }

	  /// <summary>
	  /// Obtains an instance containing the specified market data values, one for each scenario.
	  /// </summary>
	  /// @param <T> the type of the market data value </param>
	  /// <param name="values">  single market data values, one for each scenario </param>
	  /// <returns> a scenario market data box containing single market data values, one for each scenario </returns>
	  public static ScenarioMarketDataBox<T> of<T>(IList<T> values)
	  {
		return new ScenarioMarketDataBox<T>(ScenarioArray.of(values));
	  }

	  //-------------------------------------------------------------------------
	  public T SingleValue
	  {
		  get
		  {
			throw new System.InvalidOperationException("This box does not contain a single value");
		  }
	  }

	  public ScenarioArray<T> ScenarioValue
	  {
		  get
		  {
			return value;
		  }
	  }

	  public T getValue(int scenarioIndex)
	  {
		ArgChecker.inRange(scenarioIndex, 0, value.ScenarioCount, "scenarioIndex");
		return value.get(scenarioIndex);
	  }

	  public bool SingleValue
	  {
		  get
		  {
			return false;
		  }
	  }

	  public int ScenarioCount
	  {
		  get
		  {
			return value.ScenarioCount;
		  }
	  }

	  public Type MarketDataType
	  {
		  get
		  {
			return value.get(0).GetType();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public MarketDataBox<R> map<R>(System.Func<T, R> fn)
	  {
		return applyToScenarios(i => fn(value.get(i)));
	  }

	  public MarketDataBox<R> mapWithIndex<R>(int scenarioCount, ObjIntFunction<T, R> fn)
	  {
		if (scenarioCount != ScenarioCount)
		{
		  throw new System.ArgumentException(Messages.format("Scenario count {} does not equal the scenario count of the value {}", scenarioCount, ScenarioCount));
		}
		IList<R> perturbedValues = IntStream.range(0, scenarioCount).mapToObj(idx => fn.apply(getValue(idx), idx)).collect(toImmutableList());
		return MarketDataBox.ofScenarioValues(perturbedValues);
	  }

	  public MarketDataBox<R> combineWith<U, R>(MarketDataBox<U> other, System.Func<T, U, R> fn)
	  {
		return other.SingleValue ? combineWithSingle(other, fn) : combineWithMultiple(other, fn);
	  }

	  private MarketDataBox<R> combineWithMultiple<R, U>(MarketDataBox<U> other, System.Func<T, U, R> fn)
	  {
		ScenarioArray<U> otherValue = other.ScenarioValue;

		if (otherValue.ScenarioCount != value.ScenarioCount)
		{
		  string message = Messages.format("Scenario values must have the same number of scenarios. {} has {} scenarios, {} has {}", value, value.ScenarioCount, otherValue, otherValue.ScenarioCount);
		  throw new System.ArgumentException(message);
		}
		return applyToScenarios(i => fn(value.get(i), otherValue.get(i)));
	  }

	  private MarketDataBox<R> combineWithSingle<U, R>(MarketDataBox<U> other, System.Func<T, U, R> fn)
	  {
		U otherValue = other.SingleValue;
		return applyToScenarios(i => fn(value.get(i), otherValue));
	  }

	  private MarketDataBox<R> applyToScenarios<R>(System.Func<int, R> fn)
	  {
		IList<R> results = IntStream.range(0, value.ScenarioCount).mapToObj(fn.apply).collect(toImmutableList());
		return MarketDataBox.ofScenarioValues(results);
	  }

	  public Stream<T> stream()
	  {
		return value.stream();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ScenarioMarketDataBox}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static ScenarioMarketDataBox.Meta meta()
	  public static ScenarioMarketDataBox.Meta meta()
	  {
		return ScenarioMarketDataBox.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code ScenarioMarketDataBox}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> ScenarioMarketDataBox.Meta<R> metaScenarioMarketDataBox(Class<R> cls)
	  public static ScenarioMarketDataBox.Meta<R> metaScenarioMarketDataBox<R>(Type<R> cls)
	  {
		return ScenarioMarketDataBox.Meta.INSTANCE;
	  }

	  static ScenarioMarketDataBox()
	  {
		MetaBean.register(ScenarioMarketDataBox.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// @param <T>  the type </param>
	  /// <returns> the builder, not null </returns>
	  internal static ScenarioMarketDataBox.Builder<T> builder<T>()
	  {
		return new ScenarioMarketDataBox.Builder<T>();
	  }

	  private ScenarioMarketDataBox(ScenarioArray<T> value)
	  {
		JodaBeanUtils.notNull(value, "value");
		this.value = value;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public ScenarioMarketDataBox.Meta<T> metaBean()
	  public override ScenarioMarketDataBox.Meta<T> metaBean()
	  {
		return ScenarioMarketDataBox.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data value which provides data for multiple scenarios. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ScenarioArray<T> Value
	  {
		  get
		  {
			return value;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  internal Builder<T> toBuilder()
	  {
		return new Builder<T>(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ScenarioMarketDataBox<?> other = (ScenarioMarketDataBox<?>) obj;
		  ScenarioMarketDataBox<object> other = (ScenarioMarketDataBox<object>) obj;
		  return JodaBeanUtils.equal(value, other.value);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("ScenarioMarketDataBox{");
		buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ScenarioMarketDataBox}. </summary>
	  /// @param <T>  the type </param>
	  internal sealed class Meta<T> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  value_Renamed = DirectMetaProperty.ofImmutable(this, "value", typeof(ScenarioMarketDataBox), (Type) typeof(ScenarioArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "value");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") static final Meta INSTANCE = new Meta();
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<ScenarioArray<T>> value = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "value", ScenarioMarketDataBox.class, (Class) ScenarioArray.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ScenarioArray<T>> value_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "value");
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
			case 111972721: // value
			  return value_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ScenarioMarketDataBox.Builder<T> builder()
		{
		  return new ScenarioMarketDataBox.Builder<T>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(ScenarioMarketDataBox);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ScenarioArray<T>> value()
		{
		  return value_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((ScenarioMarketDataBox<?>) bean).getValue();
			  return ((ScenarioMarketDataBox<object>) bean).Value;
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
	  /// The bean-builder for {@code ScenarioMarketDataBox}. </summary>
	  /// @param <T>  the type </param>
	  internal sealed class Builder<T> : DirectFieldsBeanBuilder<ScenarioMarketDataBox<T>>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ScenarioArray<T> value_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ScenarioMarketDataBox<T> beanToCopy)
		{
		  this.value_Renamed = beanToCopy.Value;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
			  return value_Renamed;
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
			case 111972721: // value
			  this.value_Renamed = (ScenarioArray<T>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder<T> set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override ScenarioMarketDataBox<T> build()
		{
		  return new ScenarioMarketDataBox<T>(value_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the market data value which provides data for multiple scenarios. </summary>
		/// <param name="value">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder<T> value(ScenarioArray<T> value)
		{
		  JodaBeanUtils.notNull(value, "value");
		  this.value_Renamed = value;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("ScenarioMarketDataBox.Builder{");
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
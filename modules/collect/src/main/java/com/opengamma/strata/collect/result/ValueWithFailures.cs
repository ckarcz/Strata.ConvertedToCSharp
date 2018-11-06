using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.concatToList;


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

	/// <summary>
	/// A value with associated failures.
	/// <para>
	/// This captures a common use case where an operation can tolerate some failure.
	/// This is often referred to as partial success or partial failure.
	/// The class stores the value, of any object type, and a list of failures that may be empty.
	/// </para>
	/// <para>
	/// The success value must be able to handle the case where everything fails.
	/// In most cases, the success value will be a collection type, such as <seealso cref="List"/>
	/// or <seealso cref="Map"/>, which can be empty if the operation failed completely.
	/// </para>
	/// <para>
	/// The classic example is loading rows from a file, when some rows are valid and some are invalid.
	/// The partial result would contain the successful rows, with the list of failures containing an
	/// entry for each row that failed to parse.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the underlying success value, typically a collection type </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ValueWithFailures<T> implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ValueWithFailures<T> : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final T value;
		private readonly T value;
	  /// <summary>
	  /// The failure items.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<FailureItem> failures;
	  private readonly ImmutableList<FailureItem> failures;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance wrapping the success value and failures.
	  /// </summary>
	  /// @param <T>  the type of the success value </param>
	  /// <param name="successValue">  the success value </param>
	  /// <param name="failures">  the failures </param>
	  /// <returns> an instance wrapping the value and failures </returns>
	  public static ValueWithFailures<T> of<T>(T successValue, params FailureItem[] failures)
	  {
		return new ValueWithFailures<T>(successValue, ImmutableList.copyOf(failures));
	  }

	  /// <summary>
	  /// Creates an instance wrapping the success value and failures.
	  /// </summary>
	  /// @param <T>  the type of the success value </param>
	  /// <param name="successValue">  the success value </param>
	  /// <param name="failures">  the failures </param>
	  /// <returns> an instance wrapping the value and failures </returns>
	  public static ValueWithFailures<T> of<T>(T successValue, IList<FailureItem> failures)
	  {
		return new ValueWithFailures<T>(successValue, failures);
	  }

	  /// <summary>
	  /// Creates an instance using a supplier.
	  /// <para>
	  /// If the supplier succeeds normally, the supplied value will be returned.
	  /// If the supplier fails, the empty value will be returned along with a failure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the value </param>
	  /// <param name="emptyValue">  the empty value </param>
	  /// <param name="supplier">  supplier of the result value </param>
	  /// <returns> an instance containing the supplied value, or a failure if an exception is thrown </returns>
	  public static ValueWithFailures<T> of<T>(T emptyValue, System.Func<T> supplier)
	  {
		try
		{
		  return of(supplier());
		}
		catch (Exception ex)
		{
		  return ValueWithFailures.of(emptyValue, FailureItem.of(FailureReason.ERROR, ex));
		}
	  }

	  /// <summary>
	  /// Returns a collector that can be used to create a ValueWithFailure instance from a stream of ValueWithFailure
	  /// instances.
	  /// <para>
	  /// The <seealso cref="Collector"/> returned performs a reduction of its <seealso cref="ValueWithFailures"/> input elements under a
	  /// specified <seealso cref="BinaryOperator"/> using the provided identity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the success value in the <seealso cref="ValueWithFailures"/> </param>
	  /// <param name="identityValue">  the identity value </param>
	  /// <param name="operator">  the operator used for the reduction. </param>
	  /// <returns> a <seealso cref="Collector"/> </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <T> java.util.stream.Collector<ValueWithFailures<T>, ?, ValueWithFailures<T>> toValueWithFailures(T identityValue, java.util.function.BinaryOperator<T> operator)
	  public static Collector<ValueWithFailures<T>, ?, ValueWithFailures<T>> toValueWithFailures<T>(T identityValue, System.Func<T, T, T> @operator)
	  {

		System.Func<ValueWithFailures<T>, ValueWithFailures<T>, ValueWithFailures<T>> reduceFunction = (result1, result2) => result1.combinedWith(result2, @operator);

		return Collectors.reducing(ValueWithFailures.of(identityValue), reduceFunction);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if there are any failures.
	  /// </summary>
	  /// <returns> true if there are any failures </returns>
	  public bool hasFailures()
	  {
		return !failures.Empty;
	  }

	  /// <summary>
	  /// Processes the value by applying a function that alters the value.
	  /// <para>
	  /// This operation allows post-processing of a result value.
	  /// The specified function represents a conversion to be performed on the value.
	  /// </para>
	  /// <para>
	  /// It is strongly advised to ensure that the function cannot throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value in the returned result </param>
	  /// <param name="function">  the function to transform the value with </param>
	  /// <returns> the transformed instance of value and failures </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> ValueWithFailures<R> map(java.util.function.Function<? super T, ? extends R> function)
	  public ValueWithFailures<R> map<R, T1>(System.Func<T1> function) where T1 : R
	  {
		R transformedValue = Objects.requireNonNull(function(value));
		return ValueWithFailures.of(transformedValue, this.failures);
	  }

	  /// <summary>
	  /// Processes the value by applying a function that returns another result.
	  /// <para>
	  /// This operation allows post-processing of a result value.
	  /// This is similar to <seealso cref="#map(Function)"/> but the function returns a {@code ValueWithFailures}.
	  /// The result of this method consists of the transformed value, and the combined list of failures.
	  /// </para>
	  /// <para>
	  /// It is strongly advised to ensure that the function cannot throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value in the returned result </param>
	  /// <param name="function">  the function to transform the value with </param>
	  /// <returns> the transformed instance of value and failures </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> ValueWithFailures<R> flatMap(java.util.function.Function<? super T, ValueWithFailures<R>> function)
	  public ValueWithFailures<R> flatMap<R, T1>(System.Func<T1> function)
	  {
		ValueWithFailures<R> transformedValue = Objects.requireNonNull(function(value));
		ImmutableList<FailureItem> combinedFailures = ImmutableList.builder<FailureItem>().addAll(this.failures).addAll(transformedValue.failures).build();
		return ValueWithFailures.of(transformedValue.value, combinedFailures);
	  }

	  /// <summary>
	  /// Combines this instance with another.
	  /// <para>
	  /// If both instances contain lists of the same type, the combining function will
	  /// often be {@code Guavate::concatToList}.
	  /// </para>
	  /// <para>
	  /// It is strongly advised to ensure that the function cannot throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <U>  the type of the value in the other instance </param>
	  /// @param <R>  the type of the value in the returned result </param>
	  /// <param name="other">  the other instance </param>
	  /// <param name="combiner">  the function that combines the two values </param>
	  /// <returns> the combined instance of value and failures </returns>
	  public ValueWithFailures<R> combinedWith<U, R>(ValueWithFailures<U> other, System.Func<T, U, R> combiner)
	  {
		R combinedValue = Objects.requireNonNull(combiner(value, other.value));
		ImmutableList<FailureItem> combinedFailures = concatToList(failures, other.failures);
		return ValueWithFailures.of(combinedValue, combinedFailures);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueWithFailures}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static ValueWithFailures.Meta meta()
	  public static ValueWithFailures.Meta meta()
	  {
		return ValueWithFailures.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code ValueWithFailures}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> ValueWithFailures.Meta<R> metaValueWithFailures(Class<R> cls)
	  public static ValueWithFailures.Meta<R> metaValueWithFailures<R>(Type<R> cls)
	  {
		return ValueWithFailures.Meta.INSTANCE;
	  }

	  static ValueWithFailures()
	  {
		MetaBean.register(ValueWithFailures.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ValueWithFailures(T value, IList<FailureItem> failures)
	  {
		JodaBeanUtils.notNull(value, "value");
		JodaBeanUtils.notNull(failures, "failures");
		this.value = value;
		this.failures = ImmutableList.copyOf(failures);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public ValueWithFailures.Meta<T> metaBean()
	  public override ValueWithFailures.Meta<T> metaBean()
	  {
		return ValueWithFailures.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the success value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public T Value
	  {
		  get
		  {
			return value;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the failure items. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<FailureItem> Failures
	  {
		  get
		  {
			return failures;
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
//ORIGINAL LINE: ValueWithFailures<?> other = (ValueWithFailures<?>) obj;
		  ValueWithFailures<object> other = (ValueWithFailures<object>) obj;
		  return JodaBeanUtils.equal(value, other.value) && JodaBeanUtils.equal(failures, other.failures);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(value);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(failures);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ValueWithFailures{");
		buf.Append("value").Append('=').Append(value).Append(',').Append(' ');
		buf.Append("failures").Append('=').Append(JodaBeanUtils.ToString(failures));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ValueWithFailures}. </summary>
	  /// @param <T>  the type </param>
	  public sealed class Meta<T> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  value_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "value", typeof(ValueWithFailures), typeof(object));
			  failures_Renamed = DirectMetaProperty.ofImmutable(this, "failures", typeof(ValueWithFailures), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "value", "failures");
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
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<T> value = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "value", ValueWithFailures.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<T> value_Renamed;
		/// <summary>
		/// The meta-property for the {@code failures} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<FailureItem>> failures = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "failures", ValueWithFailures.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<FailureItem>> failures_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "value", "failures");
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
			case 675938345: // failures
			  return failures_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ValueWithFailures<T>> builder()
		public override BeanBuilder<ValueWithFailures<T>> builder()
		{
		  return new ValueWithFailures.Builder<>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(ValueWithFailures);
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
		public MetaProperty<T> value()
		{
		  return value_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code failures} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<FailureItem>> failures()
		{
		  return failures_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 111972721: // value
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((ValueWithFailures<?>) bean).getValue();
			  return ((ValueWithFailures<object>) bean).Value;
			case 675938345: // failures
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((ValueWithFailures<?>) bean).getFailures();
			  return ((ValueWithFailures<object>) bean).Failures;
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
	  /// The bean-builder for {@code ValueWithFailures}. </summary>
	  /// @param <T>  the type </param>
	  private sealed class Builder<T> : DirectPrivateBeanBuilder<ValueWithFailures<T>>
	  {

		internal T value;
		internal IList<FailureItem> failures = ImmutableList.of();

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
			case 111972721: // value
			  return value;
			case 675938345: // failures
			  return failures;
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
			  this.value = (T) newValue;
			  break;
			case 675938345: // failures
			  this.failures = (IList<FailureItem>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ValueWithFailures<T> build()
		{
		  return new ValueWithFailures<T>(value, failures);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ValueWithFailures.Builder{");
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value)).Append(',').Append(' ');
		  buf.Append("failures").Append('=').Append(JodaBeanUtils.ToString(failures));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
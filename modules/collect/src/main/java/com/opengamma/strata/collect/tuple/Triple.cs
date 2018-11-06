using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.tuple
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

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// An immutable triple consisting of three elements.
	/// <para>
	/// This implementation refers to the elements as 'first', 'second' and 'third'.
	/// The elements cannot be null.
	/// </para>
	/// <para>
	/// Although the implementation is immutable, there is no restriction on the objects
	/// that may be stored. If mutable objects are stored in the triple, then the triple
	/// itself effectively becomes mutable.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe if the stored objects are immutable.
	/// 
	/// </para>
	/// </summary>
	/// @param <A> the first element type </param>
	/// @param <B> the second element type </param>
	/// @param <C> the third element type </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class Triple<A, B, C> implements org.joda.beans.ImmutableBean, Tuple, Comparable<Triple<A, B, C>>, java.io.Serializable
	[Serializable]
	public sealed class Triple<A, B, C> : ImmutableBean, Tuple, IComparable<Triple<A, B, C>>
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final A first;
		private readonly A first;
	  /// <summary>
	  /// The second element in this pair.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final B second;
	  private readonly B second;
	  /// <summary>
	  /// The third element in this pair.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final C third;
	  private readonly C third;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a triple inferring the types.
	  /// </summary>
	  /// @param <A> the first element type </param>
	  /// @param <B> the second element type </param>
	  /// @param <C> the third element type </param>
	  /// <param name="first">  the first element </param>
	  /// <param name="second">  the second element </param>
	  /// <param name="third">  the third element </param>
	  /// <returns> a triple formed from the three parameters </returns>
	  public static Triple<A, B, C> of<A, B, C>(A first, B second, C third)
	  {
		return new Triple<A, B, C>(first, second, third);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of elements held by this triple.
	  /// </summary>
	  /// <returns> size 3 </returns>
	  public int size()
	  {
		return 3;
	  }

	  /// <summary>
	  /// Gets the elements from this triple as a list.
	  /// <para>
	  /// The list returns each element in the triple in order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the elements as an immutable list </returns>
	  public ImmutableList<object> elements()
	  {
		return ImmutableList.of(first, second, third);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares the triple based on the first element followed by the second
	  /// element followed by the third element.
	  /// <para>
	  /// The element types must be {@code Comparable}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other pair </param>
	  /// <returns> negative if this is less, zero if equal, positive if greater </returns>
	  /// <exception cref="ClassCastException"> if either object is not comparable </exception>
	  public int CompareTo(Triple<A, B, C> other)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return com.google.common.collect.ComparisonChain.start().compare((Comparable<?>) first, (Comparable<?>) other.first).compare((Comparable<?>) second, (Comparable<?>) other.second).compare((Comparable<?>) third, (Comparable<?>) other.third).result();
		return ComparisonChain.start().compare((IComparable<object>) first, (IComparable<object>) other.first).compare((IComparable<object>) second, (IComparable<object>) other.second).compare((IComparable<object>) third, (IComparable<object>) other.third).result();
	  }

	  /// <summary>
	  /// Gets the pair using a standard string format.
	  /// <para>
	  /// The standard format is '[$first, $second, $third]'. Spaces around the values are trimmed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the pair as a string </returns>
	  public override string ToString()
	  {
		return (new StringBuilder()).Append('[').Append(first).Append(", ").Append(second).Append(", ").Append(third).Append(']').ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Triple}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static Triple.Meta meta()
	  public static Triple.Meta meta()
	  {
		return Triple.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code Triple}. </summary>
	  /// @param <R>  the first generic type </param>
	  /// @param <S>  the second generic type </param>
	  /// @param <T>  the second generic type </param>
	  /// <param name="cls1">  the first generic type </param>
	  /// <param name="cls2">  the second generic type </param>
	  /// <param name="cls3">  the third generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R, S, T> Triple.Meta<R, S, T> metaTriple(Class<R> cls1, Class<S> cls2, Class<T> cls3)
	  public static Triple.Meta<R, S, T> metaTriple<R, S, T>(Type<R> cls1, Type<S> cls2, Type<T> cls3)
	  {
		return Triple.Meta.INSTANCE;
	  }

	  static Triple()
	  {
		MetaBean.register(Triple.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private Triple(A first, B second, C third)
	  {
		JodaBeanUtils.notNull(first, "first");
		JodaBeanUtils.notNull(second, "second");
		JodaBeanUtils.notNull(third, "third");
		this.first = first;
		this.second = second;
		this.third = third;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Triple.Meta<A, B, C> metaBean()
	  public override Triple.Meta<A, B, C> metaBean()
	  {
		return Triple.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first element in this pair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public A First
	  {
		  get
		  {
			return first;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the second element in this pair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public B Second
	  {
		  get
		  {
			return second;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the third element in this pair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public C Third
	  {
		  get
		  {
			return third;
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
//ORIGINAL LINE: Triple<?, ?, ?> other = (Triple<?, ?, ?>) obj;
		  Triple<object, ?, ?> other = (Triple<object, ?, ?>) obj;
		  return JodaBeanUtils.equal(first, other.first) && JodaBeanUtils.equal(second, other.second) && JodaBeanUtils.equal(third, other.third);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(first);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(second);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(third);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Triple}. </summary>
	  /// @param <A>  the type </param>
	  /// @param <B>  the type </param>
	  /// @param <C>  the type </param>
	  public sealed class Meta<A, B, C> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  first_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "first", typeof(Triple), typeof(object));
			  second_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "second", typeof(Triple), typeof(object));
			  third_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "third", typeof(Triple), typeof(object));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "first", "second", "third");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") static final Meta INSTANCE = new Meta();
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code first} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<A> first = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "first", Triple.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<A> first_Renamed;
		/// <summary>
		/// The meta-property for the {@code second} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<B> second = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "second", Triple.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<B> second_Renamed;
		/// <summary>
		/// The meta-property for the {@code third} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<C> third = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "third", Triple.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<C> third_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "first", "second", "third");
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
			case 97440432: // first
			  return first_Renamed;
			case -906279820: // second
			  return second_Renamed;
			case 110331239: // third
			  return third_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends Triple<A, B, C>> builder()
		public override BeanBuilder<Triple<A, B, C>> builder()
		{
		  return new Triple.Builder<>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(Triple);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code first} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<A> first()
		{
		  return first_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code second} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<B> second()
		{
		  return second_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code third} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<C> third()
		{
		  return third_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 97440432: // first
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((Triple<?, ?, ?>) bean).getFirst();
			  return ((Triple<object, ?, ?>) bean).First;
			case -906279820: // second
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((Triple<?, ?, ?>) bean).getSecond();
			  return ((Triple<object, ?, ?>) bean).Second;
			case 110331239: // third
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((Triple<?, ?, ?>) bean).getThird();
			  return ((Triple<object, ?, ?>) bean).Third;
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
	  /// The bean-builder for {@code Triple}. </summary>
	  /// @param <A>  the type </param>
	  /// @param <B>  the type </param>
	  /// @param <C>  the type </param>
	  private sealed class Builder<A, B, C> : DirectPrivateBeanBuilder<Triple<A, B, C>>
	  {

		internal A first;
		internal B second;
		internal C third;

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
			case 97440432: // first
			  return first;
			case -906279820: // second
			  return second;
			case 110331239: // third
			  return third;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder<A, B, C> set(String propertyName, Object newValue)
		public override Builder<A, B, C> set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 97440432: // first
			  this.first = (A) newValue;
			  break;
			case -906279820: // second
			  this.second = (B) newValue;
			  break;
			case 110331239: // third
			  this.third = (C) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Triple<A, B, C> build()
		{
		  return new Triple<A, B, C>(first, second, third);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("Triple.Builder{");
		  buf.Append("first").Append('=').Append(JodaBeanUtils.ToString(first)).Append(',').Append(' ');
		  buf.Append("second").Append('=').Append(JodaBeanUtils.ToString(second)).Append(',').Append(' ');
		  buf.Append("third").Append('=').Append(JodaBeanUtils.ToString(third));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
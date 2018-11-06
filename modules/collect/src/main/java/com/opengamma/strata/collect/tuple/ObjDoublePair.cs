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
	/// An immutable pair consisting of an {@code Object} and a {@code double}.
	/// <para>
	/// This class is similar to <seealso cref="Pair"/> but includes a primitive element.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// 
	/// </para>
	/// </summary>
	/// @param <A> the type of the object </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ObjDoublePair<A> implements org.joda.beans.ImmutableBean, Tuple, Comparable<ObjDoublePair<A>>, java.io.Serializable
	[Serializable]
	public sealed class ObjDoublePair<A> : ImmutableBean, Tuple, IComparable<ObjDoublePair<A>>
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final A first;
		private readonly A first;
	  /// <summary>
	  /// The second element in this pair.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double second;
	  private readonly double second;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from an {@code Object} and a {@code double}.
	  /// </summary>
	  /// @param <A> the first element type </param>
	  /// <param name="first">  the first element </param>
	  /// <param name="second">  the second element </param>
	  /// <returns> a pair formed from the two parameters </returns>
	  public static ObjDoublePair<A> of<A>(A first, double second)
	  {
		return new ObjDoublePair<A>(first, second);
	  }

	  /// <summary>
	  /// Obtains an instance from a {@code Pair}.
	  /// </summary>
	  /// @param <A> the first element type </param>
	  /// <param name="pair">  the pair to convert </param>
	  /// <returns> a pair formed by extracting values from the pair </returns>
	  public static ObjDoublePair<A> ofPair<A>(Pair<A, double> pair)
	  {
		ArgChecker.notNull(pair, "pair");
		return new ObjDoublePair<A>(pair.First, pair.Second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of elements held by this pair.
	  /// </summary>
	  /// <returns> size 2 </returns>
	  public int size()
	  {
		return 2;
	  }

	  /// <summary>
	  /// Gets the elements from this pair as a list.
	  /// <para>
	  /// The list returns each element in the pair in order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the elements as an immutable list </returns>
	  public ImmutableList<object> elements()
	  {
		return ImmutableList.of(first, second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this pair to an object-based {@code Pair}.
	  /// </summary>
	  /// <returns> the object-based pair </returns>
	  public Pair<A, double> toPair()
	  {
		return Pair.of(first, second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares the pair based on the first element followed by the second element.
	  /// <para>
	  /// The first element must be {@code Comparable}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other pair </param>
	  /// <returns> negative if this is less, zero if equal, positive if greater </returns>
	  /// <exception cref="ClassCastException"> if the object is not comparable </exception>
	  public int CompareTo(ObjDoublePair<A> other)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return com.google.common.collect.ComparisonChain.start().compare((Comparable<?>) first, (Comparable<?>) other.first).compare(second, other.second).result();
		return ComparisonChain.start().compare((IComparable<object>) first, (IComparable<object>) other.first).compare(second, other.second).result();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the pair using a standard string format.
	  /// <para>
	  /// The standard format is '[$first, $second]'. Spaces around the values are trimmed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the pair as a string </returns>
	  public override string ToString()
	  {
		return (new StringBuilder()).Append('[').Append(first).Append(", ").Append(second).Append(']').ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ObjDoublePair}. </summary>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("rawtypes") public static ObjDoublePair.Meta meta()
	  public static ObjDoublePair.Meta meta()
	  {
		return ObjDoublePair.Meta.INSTANCE;
	  }

	  /// <summary>
	  /// The meta-bean for {@code ObjDoublePair}. </summary>
	  /// @param <R>  the bean's generic type </param>
	  /// <param name="cls">  the bean's generic type </param>
	  /// <returns> the meta-bean, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <R> ObjDoublePair.Meta<R> metaObjDoublePair(Class<R> cls)
	  public static ObjDoublePair.Meta<R> metaObjDoublePair<R>(Type<R> cls)
	  {
		return ObjDoublePair.Meta.INSTANCE;
	  }

	  static ObjDoublePair()
	  {
		MetaBean.register(ObjDoublePair.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ObjDoublePair(A first, double second)
	  {
		JodaBeanUtils.notNull(first, "first");
		this.first = first;
		this.second = second;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public ObjDoublePair.Meta<A> metaBean()
	  public override ObjDoublePair.Meta<A> metaBean()
	  {
		return ObjDoublePair.Meta.INSTANCE;
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
	  /// <returns> the value of the property </returns>
	  public double Second
	  {
		  get
		  {
			return second;
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
//ORIGINAL LINE: ObjDoublePair<?> other = (ObjDoublePair<?>) obj;
		  ObjDoublePair<object> other = (ObjDoublePair<object>) obj;
		  return JodaBeanUtils.equal(first, other.first) && JodaBeanUtils.equal(second, other.second);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(first);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(second);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ObjDoublePair}. </summary>
	  /// @param <A>  the type </param>
	  public sealed class Meta<A> : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  first_Renamed = (DirectMetaProperty) DirectMetaProperty.ofImmutable(this, "first", typeof(ObjDoublePair), typeof(object));
			  second_Renamed = DirectMetaProperty.ofImmutable(this, "second", typeof(ObjDoublePair), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "first", "second");
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
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<A> first = (org.joda.beans.impl.direct.DirectMetaProperty) org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "first", ObjDoublePair.class, Object.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<A> first_Renamed;
		/// <summary>
		/// The meta-property for the {@code second} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> second_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "first", "second");
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
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ObjDoublePair<A>> builder()
		public override BeanBuilder<ObjDoublePair<A>> builder()
		{
		  return new ObjDoublePair.Builder<>();
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) @Override public Class beanType()
		public override Type beanType()
		{
		  return (Type) typeof(ObjDoublePair);
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
		public MetaProperty<double> second()
		{
		  return second_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 97440432: // first
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((ObjDoublePair<?>) bean).getFirst();
			  return ((ObjDoublePair<object>) bean).First;
			case -906279820: // second
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: return ((ObjDoublePair<?>) bean).getSecond();
			  return ((ObjDoublePair<object>) bean).Second;
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
	  /// The bean-builder for {@code ObjDoublePair}. </summary>
	  /// @param <A>  the type </param>
	  private sealed class Builder<A> : DirectPrivateBeanBuilder<ObjDoublePair<A>>
	  {

		internal A first;
		internal double second;

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
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder<A> set(String propertyName, Object newValue)
		public override Builder<A> set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 97440432: // first
			  this.first = (A) newValue;
			  break;
			case -906279820: // second
			  this.second = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ObjDoublePair<A> build()
		{
		  return new ObjDoublePair<A>(first, second);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ObjDoublePair.Builder{");
		  buf.Append("first").Append('=').Append(JodaBeanUtils.ToString(first)).Append(',').Append(' ');
		  buf.Append("second").Append('=').Append(JodaBeanUtils.ToString(second));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
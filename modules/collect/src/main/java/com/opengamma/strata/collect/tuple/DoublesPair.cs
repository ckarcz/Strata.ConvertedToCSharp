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
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Splitter = com.google.common.@base.Splitter;
	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// An immutable pair consisting of two {@code double} elements.
	/// <para>
	/// This class is similar to <seealso cref="Pair"/> but is based on two primitive {@code double} elements.
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class DoublesPair implements org.joda.beans.ImmutableBean, Tuple, Comparable<DoublesPair>, java.io.Serializable
	[Serializable]
	public sealed class DoublesPair : ImmutableBean, Tuple, IComparable<DoublesPair>
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double first;
		private readonly double first;
	  /// <summary>
	  /// The second element in this pair.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double second;
	  private readonly double second;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from two {@code double} elements.
	  /// </summary>
	  /// <param name="first">  the first element </param>
	  /// <param name="second">  the second element </param>
	  /// <returns> a pair formed from the two parameters </returns>
	  public static DoublesPair of(double first, double second)
	  {
		return new DoublesPair(first, second);
	  }

	  /// <summary>
	  /// Obtains an instance from a {@code Pair}.
	  /// </summary>
	  /// <param name="pair">  the pair to convert </param>
	  /// <returns> a pair formed by extracting values from the pair </returns>
	  public static DoublesPair ofPair(Pair<double, double> pair)
	  {
		ArgChecker.notNull(pair, "pair");
		return new DoublesPair(pair.First, pair.Second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a {@code DoublesPair} from the standard string format.
	  /// <para>
	  /// The standard format is '[$first, $second]'. Spaces around the values are trimmed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pairStr">  the text to parse </param>
	  /// <returns> the parsed pair </returns>
	  /// <exception cref="IllegalArgumentException"> if the pair cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static DoublesPair parse(String pairStr)
	  public static DoublesPair parse(string pairStr)
	  {
		ArgChecker.notNull(pairStr, "pairStr");
		if (pairStr.Length < 5)
		{
		  throw new System.ArgumentException("Invalid pair format, too short: " + pairStr);
		}
		if (pairStr[0] != '[')
		{
		  throw new System.ArgumentException("Invalid pair format, must start with [: " + pairStr);
		}
		if (pairStr[pairStr.Length - 1] != ']')
		{
		  throw new System.ArgumentException("Invalid pair format, must end with ]: " + pairStr);
		}
		string content = pairStr.Substring(1, (pairStr.Length - 1) - 1);
		IList<string> split = Splitter.on(',').trimResults().splitToList(content);
		if (split.Count != 2)
		{
		  throw new System.ArgumentException("Invalid pair format, must have two values: " + pairStr);
		}
		double first = double.Parse(split[0]);
		double second = double.Parse(split[1]);
		return new DoublesPair(first, second);
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
	  public Pair<double, double> toPair()
	  {
		return Pair.of(first, second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares the pair based on the first element followed by the second element.
	  /// </summary>
	  /// <param name="other">  the other pair </param>
	  /// <returns> negative if this is less, zero if equal, positive if greater </returns>
	  public int CompareTo(DoublesPair other)
	  {
		int cmp = first.CompareTo(other.first);
		if (cmp == 0)
		{
		  cmp = second.CompareTo(other.second);
		}
		return cmp;
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is DoublesPair)
		{
		  DoublesPair other = (DoublesPair) obj;
		  return JodaBeanUtils.equal(first, other.first) && JodaBeanUtils.equal(second, other.second);
		}
		return base.Equals(obj);
	  }

	  public override int GetHashCode()
	  {
		// see Map.Entry API specification
		long f = System.BitConverter.DoubleToInt64Bits(first);
		long s = System.BitConverter.DoubleToInt64Bits(second);
		return ((int)(f ^ ((long)((ulong)f >> 32)))) ^ ((int)(s ^ ((long)((ulong)s >> 32))));
	  }

	  /// <summary>
	  /// Gets the pair using a standard string format.
	  /// <para>
	  /// The standard format is '[$first, $second]'. Spaces around the values are trimmed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the pair as a string </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public String toString()
	  public override string ToString()
	  {
		return (new StringBuilder()).Append('[').Append(first).Append(", ").Append(second).Append(']').ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DoublesPair}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DoublesPair.Meta meta()
	  {
		return DoublesPair.Meta.INSTANCE;
	  }

	  static DoublesPair()
	  {
		MetaBean.register(DoublesPair.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DoublesPair(double first, double second)
	  {
		this.first = first;
		this.second = second;
	  }

	  public override DoublesPair.Meta metaBean()
	  {
		return DoublesPair.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first element in this pair. </summary>
	  /// <returns> the value of the property </returns>
	  public double First
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
	  /// <summary>
	  /// The meta-bean for {@code DoublesPair}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  first_Renamed = DirectMetaProperty.ofImmutable(this, "first", typeof(DoublesPair), Double.TYPE);
			  second_Renamed = DirectMetaProperty.ofImmutable(this, "second", typeof(DoublesPair), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "first", "second");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code first} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> first_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DoublesPair> builder()
		public override BeanBuilder<DoublesPair> builder()
		{
		  return new DoublesPair.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DoublesPair);
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
		public MetaProperty<double> first()
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
			  return ((DoublesPair) bean).First;
			case -906279820: // second
			  return ((DoublesPair) bean).Second;
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
	  /// The bean-builder for {@code DoublesPair}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DoublesPair>
	  {

		internal double first;
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

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 97440432: // first
			  this.first = (double?) newValue.Value;
			  break;
			case -906279820: // second
			  this.second = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DoublesPair build()
		{
		  return new DoublesPair(first, second);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("DoublesPair.Builder{");
		  buf.Append("first").Append('=').Append(JodaBeanUtils.ToString(first)).Append(',').Append(' ');
		  buf.Append("second").Append('=').Append(JodaBeanUtils.ToString(second));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
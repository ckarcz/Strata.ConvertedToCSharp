using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using PercentEscaper = com.google.common.net.PercentEscaper;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// An immutable standard identifier for an item.
	/// <para>
	/// A standard identifier is used to uniquely identify domain objects.
	/// It is formed from two parts, the scheme and value.
	/// </para>
	/// <para>
	/// The scheme defines a single way of identifying items, while the value is an identifier
	/// within that scheme. A value from one scheme may refer to a completely different
	/// real-world item than the same value from a different scheme.
	/// </para>
	/// <para>
	/// Real-world examples of {@code StandardId} include instances of:
	/// <ul>
	///   <li>Cusip</li>
	///   <li>Isin</li>
	///   <li>Reuters RIC</li>
	///   <li>Bloomberg BUID</li>
	///   <li>Bloomberg Ticker</li>
	///   <li>Trading system OTC trade ID</li>
	/// </ul>
	/// </para>
	/// <para>
	/// This class is immutable and thread-safe.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class StandardId implements Comparable<StandardId>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class StandardId : IComparable<StandardId>, ImmutableBean
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Matcher for checking the scheme.
	  /// It must only contains the characters A-Z, a-z, 0-9 and selected special characters.
	  /// </summary>
	  private static readonly CharMatcher SCHEME_MATCHER = CharMatcher.inRange('A', 'Z').or(CharMatcher.inRange('a', 'z')).or(CharMatcher.inRange('0', '9')).or(CharMatcher.@is(':')).or(CharMatcher.@is('/')).or(CharMatcher.@is('+')).or(CharMatcher.@is('.')).or(CharMatcher.@is('=')).or(CharMatcher.@is('_')).or(CharMatcher.@is('-')).precomputed();
	  /// <summary>
	  /// Matcher for checking the value.
	  /// It must contain ASCII printable characters excluding curly brackets, pipe and tilde.
	  /// </summary>
	  private static readonly CharMatcher VALUE_MATCHER = CharMatcher.inRange(' ', 'z').precomputed();
	  /// <summary>
	  /// The escaper.
	  /// </summary>
	  private static readonly PercentEscaper SCHEME_ESCAPER = new PercentEscaper(":/+.=_-", false);

	  /// <summary>
	  /// The scheme that categorizes the identifier value.
	  /// <para>
	  /// This provides the universe within which the identifier value has meaning.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String scheme;
	  private readonly string scheme;
	  /// <summary>
	  /// The value of the identifier within the scheme.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final String value;
	  private readonly string value;
	  /// <summary>
	  /// The hash code.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  [NonSerialized]
	  private readonly int hashCode_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a scheme and value.
	  /// <para>
	  /// The scheme must be non-empty and match the regular expression '{@code [A-Za-z0-9:/+.=_-]*}'.
	  /// This permits letters, numbers, colon, forward-slash, plus, dot, equals, underscore and dash.
	  /// If necessary, the scheme can be encoded using <seealso cref="StandardId#encodeScheme(String)"/>.
	  /// </para>
	  /// <para>
	  /// The value must be non-empty and match the regular expression '{@code [!-z][ -z]*}'.
	  /// This includes all standard printable ASCII characters excluding curly brackets, pipe and tilde.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scheme">  the scheme of the identifier, not empty </param>
	  /// <param name="value">  the value of the identifier, not empty </param>
	  /// <returns> the identifier </returns>
	  public static StandardId of(string scheme, string value)
	  {
		return new StandardId(scheme, value);
	  }

	  /// <summary>
	  /// Parses an {@code StandardId} from a formatted scheme and value.
	  /// <para>
	  /// This parses the identifier from the form produced by {@code toString()}
	  /// which is '{@code $scheme~$value}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the identifier to parse </param>
	  /// <returns> the identifier </returns>
	  /// <exception cref="IllegalArgumentException"> if the identifier cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static StandardId parse(String str)
	  public static StandardId parse(string str)
	  {
		int pos = ArgChecker.notNull(str, "str").IndexOf("~", StringComparison.Ordinal);
		if (pos < 0)
		{
		  throw new System.ArgumentException("Invalid identifier format: " + str);
		}
		return new StandardId(str.Substring(0, pos), str.Substring(pos + 1));
	  }

	  /// <summary>
	  /// Encode a string suitable for use as the scheme.
	  /// <para>
	  /// This uses percent encoding, just like URI.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scheme">  the scheme to encode </param>
	  /// <returns> the encoded scheme </returns>
	  public static string encodeScheme(string scheme)
	  {
		return SCHEME_ESCAPER.escape(scheme);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an identifier.
	  /// </summary>
	  /// <param name="scheme">  the scheme of the identifier, not empty </param>
	  /// <param name="value">  the value of the identifier, not empty </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private StandardId(String scheme, String value)
	  private StandardId(string scheme, string value)
	  {
		ArgChecker.matches(SCHEME_MATCHER, 1, int.MaxValue, scheme, "scheme", "[A-Za-z0-9:/+.=_-]+");
		ArgChecker.matches(VALUE_MATCHER, 1, int.MaxValue, value, "value", "[!-z][ -z]+");
		if (value[0] == ' ')
		{
		  throw new System.ArgumentException(Messages.format("Invalid initial space in value '{}' must match regex '[!-z][ -z]*'", value));
		}
		this.scheme = scheme;
		this.value = value;
		this.hashCode_Renamed = scheme.GetHashCode() ^ value.GetHashCode();
	  }

	  // resolve after deserialization
	  private object readResolve()
	  {
		return new StandardId(scheme, value);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares the external identifiers, sorting alphabetically by scheme followed by value.
	  /// </summary>
	  /// <param name="other">  the other external identifier </param>
	  /// <returns> negative if this is less, zero if equal, positive if greater </returns>
	  public int CompareTo(StandardId other)
	  {
		return ComparisonChain.start().compare(scheme, other.scheme).compare(value, other.value).result();
	  }

	  /// <summary>
	  /// Checks if this identifier equals another, comparing the scheme and value.
	  /// </summary>
	  /// <param name="obj">  the other object </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is StandardId)
		{
		  StandardId other = (StandardId) obj;
		  return scheme.Equals(other.scheme) && value.Equals(other.value);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code, based on the scheme and value.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return hashCode_Renamed;
	  }

	  /// <summary>
	  /// Returns the identifier in a standard string format.
	  /// <para>
	  /// The returned string is in the form '{@code $scheme~$value}'.
	  /// This is suitable for use with <seealso cref="#parse(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a parsable representation of the identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public String toString()
	  public override string ToString()
	  {
		return scheme + "~" + value;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code StandardId}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static StandardId.Meta meta()
	  {
		return StandardId.Meta.INSTANCE;
	  }

	  static StandardId()
	  {
		MetaBean.register(StandardId.Meta.INSTANCE);
	  }

	  public override StandardId.Meta metaBean()
	  {
		return StandardId.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the scheme that categorizes the identifier value.
	  /// <para>
	  /// This provides the universe within which the identifier value has meaning.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Scheme
	  {
		  get
		  {
			return scheme;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value of the identifier within the scheme. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public string Value
	  {
		  get
		  {
			return value;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code StandardId}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  scheme_Renamed = DirectMetaProperty.ofImmutable(this, "scheme", typeof(StandardId), typeof(string));
			  value_Renamed = DirectMetaProperty.ofImmutable(this, "value", typeof(StandardId), typeof(string));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "scheme", "value");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code scheme} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> scheme_Renamed;
		/// <summary>
		/// The meta-property for the {@code value} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> value_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "scheme", "value");
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
			case -907987547: // scheme
			  return scheme_Renamed;
			case 111972721: // value
			  return value_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends StandardId> builder()
		public override BeanBuilder<StandardId> builder()
		{
		  return new StandardId.Builder();
		}

		public override Type beanType()
		{
		  return typeof(StandardId);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code scheme} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> scheme()
		{
		  return scheme_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code value} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> value()
		{
		  return value_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -907987547: // scheme
			  return ((StandardId) bean).Scheme;
			case 111972721: // value
			  return ((StandardId) bean).Value;
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
	  /// The bean-builder for {@code StandardId}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<StandardId>
	  {

		internal string scheme;
		internal string value;

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
			case -907987547: // scheme
			  return scheme;
			case 111972721: // value
			  return value;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -907987547: // scheme
			  this.scheme = (string) newValue;
			  break;
			case 111972721: // value
			  this.value = (string) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override StandardId build()
		{
		  return new StandardId(scheme, value);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("StandardId.Builder{");
		  buf.Append("scheme").Append('=').Append(JodaBeanUtils.ToString(scheme)).Append(',').Append(' ');
		  buf.Append("value").Append('=').Append(JodaBeanUtils.ToString(value));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
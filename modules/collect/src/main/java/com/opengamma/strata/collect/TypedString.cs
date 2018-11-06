using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	using ToString = org.joda.convert.ToString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An abstract class designed to enable typed strings.
	/// <para>
	/// The purpose of {@code TypedString} is to provide a Java type to a concept
	/// that might otherwise be represented as a string.
	/// It could be thought of as a way to provide a type alias for a string.
	/// </para>
	/// <para>
	/// The string wrapped by this type must not be empty.
	/// </para>
	/// <para>
	/// Subclasses must be written as follows:
	/// <pre>
	///  public final class FooType
	///      extends TypedString&lt;FooType&gt; {
	///    private static final long serialVersionUID = 1L;
	///    &#64;FromString
	///    public static FooType of(String name) {
	///      return new FooType(name);
	///    }
	///    private FooType(String name) {
	///      super(name);
	///    }
	///  }
	/// </pre>
	/// </para>
	/// <para>
	/// The net result is that an API can be written with methods taking
	/// {@code FooType} as a method parameter instead of {@code String}.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the implementation subclass of this class </param>
	[Serializable]
	public abstract class TypedString<T> : Named, IComparable<T> where T : TypedString<T>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The name.
	  /// </summary>
	  private readonly string name;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name, not empty </param>
	  protected internal TypedString(string name)
	  {
		this.name = ArgChecker.notEmpty(name, "name");
	  }

	  /// <summary>
	  /// Creates an instance, validating the name against a regex.
	  /// <para>
	  /// In most cases, a <seealso cref="CharMatcher"/> will be faster than a regex <seealso cref="Pattern"/>,
	  /// typically by over an order of magnitude.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name, not empty </param>
	  /// <param name="pattern">  the regex pattern for validating the name </param>
	  /// <param name="msg">  the message to use to explain validation failure </param>
	  protected internal TypedString(string name, Pattern pattern, string msg)
	  {
		ArgChecker.notEmpty(name, "name");
		ArgChecker.notNull(pattern, "pattern");
		ArgChecker.notEmpty(msg, "msg");
		if (pattern.matcher(name).matches() == false)
		{
		  throw new System.ArgumentException(msg);
		}
		this.name = name;
	  }

	  /// <summary>
	  /// Creates an instance, validating the name against a matcher.
	  /// <para>
	  /// In most cases, a <seealso cref="CharMatcher"/> will be faster than a regex <seealso cref="Pattern"/>,
	  /// typically by over an order of magnitude.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name, not empty </param>
	  /// <param name="matcher">  the matcher for validating the name </param>
	  /// <param name="msg">  the message to use to explain validation failure </param>
	  protected internal TypedString(string name, CharMatcher matcher, string msg)
	  {
		ArgChecker.notEmpty(name, "name");
		ArgChecker.notNull(matcher, "pattern");
		ArgChecker.notEmpty(msg, "msg");
		if (matcher.matchesAllOf(name) == false)
		{
		  throw new System.ArgumentException(msg);
		}
		this.name = name;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name.
	  /// </summary>
	  /// <returns> the name </returns>
	  public virtual string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  /// <summary>
	  /// Compares this type to another.
	  /// <para>
	  /// Instances are compared in alphabetical order based on the name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the object to compare to </param>
	  /// <returns> the comparison </returns>
	  public int CompareTo(T other)
	  {
		return name.CompareTo(other.ToString());
	  }

	  /// <summary>
	  /// Checks if this type equals another.
	  /// <para>
	  /// Instances are compared based on the name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the object to compare to, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override sealed bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: TypedString<?> other = (TypedString<?>) obj;
		  TypedString<object> other = (TypedString<object>) obj;
		  return name.Equals(other.name);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code.
	  /// </summary>
	  /// <returns> a suitable hash code </returns>
	  public override sealed int GetHashCode()
	  {
		return name.GetHashCode() ^ this.GetType().GetHashCode();
	  }

	  /// <summary>
	  /// Returns the name.
	  /// </summary>
	  /// <returns> the string form, not empty </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public final String toString()
	  public override sealed string ToString()
	  {
		return name;
	  }

	}

}
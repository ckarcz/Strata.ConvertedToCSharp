using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using FromString = org.joda.convert.FromString;

	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The type that provides meaning to an attribute.
	/// <para>
	/// Attributes provide the ability to associate arbitrary information with the trade model in a key-value map.
	/// For example, it might be used to provide information about the trading platform.
	/// </para>
	/// <para>
	/// Applications that wish to use attributes should declare a static constant declaring the
	/// {@code AttributeType} instance, the type parameter and a lowerCamelCase name. For example:
	/// <pre>
	///  public static final AttributeType&lt;String&gt; DEALER = AttributeType.of("dealer");
	/// </pre>
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the attribute value </param>
	[Serializable]
	public sealed class AttributeType<T> : TypedString<AttributeType<T>>
	{

	  /// <summary>
	  /// Key used to access the description.
	  /// </summary>
	  public static readonly AttributeType<string> DESCRIPTION = AttributeType.of("description");
	  /// <summary>
	  /// Key used to access the name.
	  /// </summary>
	  public static readonly AttributeType<string> NAME = AttributeType.of("name");

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// The name may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type associated with the info </param>
	  /// <param name="name">  the name </param>
	  /// <returns> a type instance with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static <T> AttributeType<T> of(String name)
	  public static AttributeType<T> of<T>(string name)
	  {
		return new AttributeType<T>(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private AttributeType(string name) : base(name)
	  {
	  }

	}

}
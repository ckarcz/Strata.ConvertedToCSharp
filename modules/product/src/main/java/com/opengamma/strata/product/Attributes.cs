/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Additional attributes that can be associated with a model object.
	/// </summary>
	public interface Attributes
	{

	  /// <summary>
	  /// Obtains an empty instance.
	  /// <para>
	  /// The <seealso cref="#withAttribute(AttributeType, Object)"/> method can be used on
	  /// the instance to add attributes.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the empty instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static Attributes empty()
	//  {
	//	return SimpleAttributes.EMPTY;
	//  }

	  /// <summary>
	  /// Obtains an empty instance.
	  /// <para>
	  /// The <seealso cref="#withAttribute(AttributeType, Object)"/> method can be used on
	  /// the instance to add attributes.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the attribute value </param>
	  /// <param name="type">  the type providing meaning to the value </param>
	  /// <param name="value">  the value </param>
	  /// <returns> the instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> Attributes of(AttributeType<T> type, T value)
	//  {
	//	return new SimpleAttributes(ImmutableMap.of(type, value));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the attribute associated with the specified type.
	  /// <para>
	  /// This method obtains the specified attribute.
	  /// This allows an attribute to be obtained if available.
	  /// </para>
	  /// <para>
	  /// If the attribute is not found, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the attribute value </param>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the attribute value </returns>
	  /// <exception cref="IllegalArgumentException"> if the attribute is not found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> T getAttribute(AttributeType<T> type)
	//  {
	//	return findAttribute(type).orElseThrow(() -> new IllegalArgumentException(Messages.format("Attribute not found for type '{}'", type)));
	//  }

	  /// <summary>
	  /// Finds the attribute associated with the specified type.
	  /// <para>
	  /// This method obtains the specified attribute.
	  /// This allows an attribute to be obtained if available.
	  /// </para>
	  /// <para>
	  /// If the attribute is not found, optional empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the result </param>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the attribute value </returns>
	  Optional<T> findAttribute<T>(AttributeType<T> type);

	  /// <summary>
	  /// Returns a copy of this instance with the attribute added.
	  /// <para>
	  /// This returns a new instance with the specified attribute added.
	  /// The attribute is added using {@code Map.put(type, value)} semantics.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the attribute value </param>
	  /// <param name="type">  the type providing meaning to the value </param>
	  /// <param name="value">  the value </param>
	  /// <returns> a new instance based on this one with the attribute added </returns>
	  Attributes withAttribute<T>(AttributeType<T> type, T value);

	}

}
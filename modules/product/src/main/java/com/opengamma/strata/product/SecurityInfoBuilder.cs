using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Builder to create {@code SecurityInfo}.
	/// <para>
	/// This builder allows a <seealso cref="SecurityInfo"/> to be created.
	/// </para>
	/// </summary>
	public sealed class SecurityInfoBuilder
	{

	  /// <summary>
	  /// The security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private SecurityId id_Renamed;
	  /// <summary>
	  /// The information about the security price.
	  /// <para>
	  /// This provides information about the security price.
	  /// This can be used to convert the price into a monetary value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private SecurityPriceInfo priceInfo_Renamed;
	  /// <summary>
	  /// The security attributes.
	  /// <para>
	  /// Security attributes, provide the ability to associate arbitrary information
	  /// with a security in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<AttributeType<?>, Object> attributes = new java.util.HashMap<>();
	  private readonly IDictionary<AttributeType<object>, object> attributes = new Dictionary<AttributeType<object>, object>();

	  // creates an empty instance
	  internal SecurityInfoBuilder()
	  {
	  }

	  // creates a populated instance
	  internal SecurityInfoBuilder<T1>(SecurityId id, SecurityPriceInfo priceInfo, IDictionary<T1> attributes)
	  {
		this.id_Renamed = id;
		this.priceInfo_Renamed = priceInfo;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.attributes.putAll(attributes);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Sets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <returns> this, for chaining </returns>
	  public SecurityInfoBuilder id(SecurityId id)
	  {
		this.id_Renamed = ArgChecker.notNull(id, "id");
		return this;
	  }

	  /// <summary>
	  /// Sets the information about the security price.
	  /// <para>
	  /// This provides information about the security price.
	  /// This can be used to convert the price into a monetary value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="priceInfo">  the price info </param>
	  /// <returns> this, for chaining </returns>
	  public SecurityInfoBuilder priceInfo(SecurityPriceInfo priceInfo)
	  {
		this.priceInfo_Renamed = ArgChecker.notNull(priceInfo, "priceInfo");
		return this;
	  }

	  /// <summary>
	  /// Adds a security attribute to the map of attributes.
	  /// <para>
	  /// The attribute is added using {@code Map.put(type, value)} semantics.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the value </param>
	  /// <param name="type">  the type providing meaning to the value </param>
	  /// <param name="value">  the value </param>
	  /// <returns> this, for chaining </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> SecurityInfoBuilder addAttribute(AttributeType<T> type, T value)
	  public SecurityInfoBuilder addAttribute<T>(AttributeType<T> type, T value)
	  {
		ArgChecker.notNull(type, "type");
		ArgChecker.notNull(value, "value");
		// ImmutableMap.Builder would not provide Map.put semantics
		attributes[type] = value;
		return this;
	  }

	  /// <summary>
	  /// Builds the security information.
	  /// </summary>
	  /// <returns> the security information </returns>
	  public SecurityInfo build()
	  {
		return new SecurityInfo(id_Renamed, priceInfo_Renamed, attributes);
	  }

	}

}
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Builder to create {@code PositionInfo}.
	/// <para>
	/// This builder allows a <seealso cref="PositionInfo"/> to be created.
	/// </para>
	/// </summary>
	public sealed class PositionInfoBuilder
	{

	  /// <summary>
	  /// The primary identifier for the position.
	  /// <para>
	  /// The identifier is used to identify the position.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private StandardId id_Renamed;
	  /// <summary>
	  /// The position attributes.
	  /// <para>
	  /// Position attributes, provide the ability to associate arbitrary information
	  /// with a position in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<AttributeType<?>, Object> attributes = new java.util.HashMap<>();
	  private readonly IDictionary<AttributeType<object>, object> attributes = new Dictionary<AttributeType<object>, object>();

	  // creates an empty instance
	  internal PositionInfoBuilder()
	  {
	  }

	  // creates a populated instance
	  internal PositionInfoBuilder<T1>(StandardId id, IDictionary<T1> attributes)
	  {

		this.id_Renamed = id;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.attributes.putAll(attributes);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Sets the primary identifier for the position, optional.
	  /// <para>
	  /// The identifier is used to identify the position.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <returns> this, for chaining </returns>
	  public PositionInfoBuilder id(StandardId id)
	  {
		this.id_Renamed = id;
		return this;
	  }

	  /// <summary>
	  /// Adds a position attribute to the map of attributes.
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
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> PositionInfoBuilder addAttribute(AttributeType<T> type, T value)
	  public PositionInfoBuilder addAttribute<T>(AttributeType<T> type, T value)
	  {
		ArgChecker.notNull(type, "type");
		ArgChecker.notNull(value, "value");
		// ImmutableMap.Builder would not provide Map.put semantics
		attributes[type] = value;
		return this;
	  }

	  /// <summary>
	  /// Builds the position information.
	  /// </summary>
	  /// <returns> the position information </returns>
	  public PositionInfo build()
	  {
		return new PositionInfo(id_Renamed, attributes);
	  }

	}

}
/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Additional information about a portfolio item.
	/// <para>
	/// This allows additional information to be associated with an item.
	/// It is kept in a separate object as the information is generally optional for pricing.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface PortfolioItemInfo : Attributes
	{

	  /// <summary>
	  /// Obtains an empty info instance.
	  /// <para>
	  /// The resulting instance implements this interface and is useful for classes that
	  /// extend <seealso cref="PortfolioItem"/> but are not trades or positions.
	  /// The returned instance can be customized using {@code with} methods.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the empty info instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PortfolioItemInfo empty()
	//  {
	//	return ItemInfo.empty();
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary identifier for the portfolio item, optional.
	  /// <para>
	  /// The identifier is used to identify the portfolio item.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// A portfolio item may have multiple active identifiers. Any identifier may be chosen here.
	  /// Certain uses of the identifier, such as storage in a database, require that the
	  /// identifier does not change over time, and this should be considered best practice.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the identifier, optional </returns>
	  Optional<StandardId> Id {get;}

	  /// <summary>
	  /// Returns a copy of this instance with the identifier changed.
	  /// <para>
	  /// This returns a new instance with the identifier changed.
	  /// If the specified identifier is null, the existing identifier will be removed.
	  /// If the specified identifier is non-null, it will become the identifier of the resulting info.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="identifier">  the identifier to set </param>
	  /// <returns> a new instance based on this one with the identifier set </returns>
	  PortfolioItemInfo withId(StandardId identifier);

	  /// <summary>
	  /// Gets the attribute types that the info contains.
	  /// </summary>
	  /// <returns> the attribute types </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.google.common.collect.ImmutableSet<AttributeType<?>> getAttributeTypes();
	  ImmutableSet<AttributeType<object>> AttributeTypes {get;}

	  PortfolioItemInfo withAttribute<T>(AttributeType<T> type, T value);

	}

}
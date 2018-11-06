using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{

	/// <summary>
	/// A lookup for named instances.
	/// <para>
	/// This interface is used to lookup objects that can be <seealso cref="Named identified by a unique name"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the named type </param>
	public interface NamedLookup<T> where T : Named
	{
	  // this interface is unusual in that the methods returns null and Map rather than Optional and ImmutableMap
	  // this design choice is intended to avoid boxing/copying as this is performance sensitive code

	  /// <summary>
	  /// Looks up an instance by name, returning null if not found.
	  /// <para>
	  /// The name contains enough information to be able to recreate the instance.
	  /// The lookup should return null if the name is not known.
	  /// The lookup must match the name where the match is case sensitive.
	  /// Where possible implementations should match the upper-case form of the name, using <seealso cref="Locale#ENGLISH"/>.
	  /// Implementations can match completely case insensitive if desired, however this is not required.
	  /// An exception should only be thrown if an error occurs during lookup.
	  /// </para>
	  /// <para>
	  /// The default implementation uses <seealso cref="#lookupAll()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name to lookup </param>
	  /// <returns> the named object, null if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default T lookup(String name)
	//  {
	//	return lookupAll().get(name);
	//  }

	  /// <summary>
	  /// Returns the immutable map of known instances by name.
	  /// <para>
	  /// This method returns all known instances.
	  /// It is permitted for an implementation to return an empty map, however this will
	  /// reduce the usefulness of the matching method on <seealso cref="ExtendedEnum"/>.
	  /// The map may include instances keyed under an alternate name.
	  /// For example, the map will often contain the same entry keyed under the upper-cased form of the name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the immutable map of enum instance by name </returns>
	  IDictionary<string, T> lookupAll();

	}

}
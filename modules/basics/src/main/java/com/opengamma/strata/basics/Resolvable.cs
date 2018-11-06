/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	/// <summary>
	/// An object that can be resolved against reference data.
	/// <para>
	/// Interface marking those objects that can be resolved using <seealso cref="ReferenceData"/>.
	/// Implementations of this interface will use <seealso cref="ReferenceDataId identifiers"/>
	/// to refer to key concepts, such as holiday calendars and securities.
	/// </para>
	/// <para>
	/// When the {@code resolve(ReferenceData)} method is called, the identifiers are resolved.
	/// The resolving process will take each identifier, look it up using the {@code ReferenceData},
	/// and return a new "resolved" instance.
	/// Typically the result is of a type that is optimized for pricing.
	/// </para>
	/// <para>
	/// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the resolved result </param>
	public interface Resolvable<T>
	{

	  /// <summary>
	  /// Resolves this object using the specified reference data.
	  /// <para>
	  /// This converts the object implementing this interface to the equivalent resolved form.
	  /// All <seealso cref="ReferenceDataId"/> identifiers in this instance will be resolved.
	  /// The resolved form will typically be a type that is optimized for pricing.
	  /// </para>
	  /// <para>
	  /// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	  /// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved instance </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  T resolve(ReferenceData refData);

	}

}
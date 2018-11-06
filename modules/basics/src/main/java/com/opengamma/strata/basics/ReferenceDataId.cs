using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	/// <summary>
	/// An identifier for a unique item of reference data.
	/// <para>
	/// Reference data is obtained from an instance of <seealso cref="ReferenceData"/> using this identifier.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the reference data this identifier refers to </param>
	public interface ReferenceDataId<T>
	{

	  /// <summary>
	  /// Gets the type of data this identifier refers to.
	  /// </summary>
	  /// <returns> the type of the reference data this identifier refers to </returns>
	  Type<T> ReferenceDataType {get;}

	  /// <summary>
	  /// Low-level method to query the reference data value associated with this identifier,
	  /// returning null if not found.
	  /// <para>
	  /// This is a low-level method that obtains the reference data value, returning null instead of an error.
	  /// Applications should use <seealso cref="ReferenceData#getValue(ReferenceDataId)"/> in preference to this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to lookup the value in </param>
	  /// <returns> the reference data value, null if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default T queryValueOrNull(ReferenceData refData)
	//  {
	//	return refData.queryValueOrNull(this);
	//  }

	}

}
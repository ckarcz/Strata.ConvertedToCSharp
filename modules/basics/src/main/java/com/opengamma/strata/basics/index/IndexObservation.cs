/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	/// <summary>
	/// A single observation of an index.
	/// <para>
	/// Implementations of this interface represent observations of an index.
	/// For example, an observation of 'GBP-LIBOR-3M' at a specific fixing date.
	/// </para>
	/// </summary>
	public interface IndexObservation
	{

	  /// <summary>
	  /// Gets the index to be observed.
	  /// </summary>
	  /// <returns> the index </returns>
	  Index Index {get;}

	}

}
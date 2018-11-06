/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
{
	using RateIndex = com.opengamma.strata.basics.index.RateIndex;

	/// <summary>
	/// An instrument representing a security associated with a rate index.
	/// <para>
	/// Examples include Ibor or Overnight rate futures.
	/// </para>
	/// </summary>
	public interface RateIndexSecurity : Security
	{

	  /// <summary>
	  /// Get the rate index.
	  /// <para>
	  /// The index of the rate to be observed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the rate index </returns>
	  RateIndex Index {get;}

	  //-------------------------------------------------------------------------
	  RateIndexSecurity withInfo(SecurityInfo info);

	}

}
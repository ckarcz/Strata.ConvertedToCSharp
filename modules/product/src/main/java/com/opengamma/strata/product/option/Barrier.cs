/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.option
{

	/// <summary>
	/// Definition of barrier event of option instruments.
	/// <para>
	/// The barrier type, knock type, barrier level and other relevant information are specified in this class.
	/// </para>
	/// <para>
	/// The barrier level can be date dependent. <br>
	/// For forward starting barrier, the barrier level can be set to very high or low level in the initial period. <br>
	/// The barrier is continuously monitored.
	/// </para>
	/// </summary>
	public interface Barrier
	{

	  /// <summary>
	  /// Obtains the barrier type.
	  /// </summary>
	  /// <returns> the barrier type </returns>
	  BarrierType BarrierType {get;}

	  /// <summary>
	  /// Obtains the knock type.
	  /// </summary>
	  /// <returns> the knock type </returns>
	  KnockType KnockType {get;}

	  /// <summary>
	  /// Obtains the barrier level for a given observation date.
	  /// </summary>
	  /// <param name="date">  the observation date </param>
	  /// <returns> the barrier level </returns>
	  double getBarrierLevel(LocalDate date);

	  /// <summary>
	  /// Obtains an instance with knock type inverted.
	  /// </summary>
	  /// <returns> the instance </returns>
	  Barrier inverseKnockType();

	}

}
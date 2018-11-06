/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	/// <summary>
	/// A group of curves.
	/// <para>
	/// This is used to hold a group of related curves, typically forming a logical set.
	/// It is often used to hold the results of a curve calibration.
	/// </para>
	/// <para>
	/// Curve groups can also be created from a set of existing curves.
	/// </para>
	/// <para>
	/// In Strata v2, this type was converted to an interface.
	/// If migrating, change your code to <seealso cref="RatesCurveGroup"/>.
	/// </para>
	/// </summary>
	public interface CurveGroup
	{

	  /// <summary>
	  /// Gets the name of the curve group.
	  /// </summary>
	  /// <returns> the group name </returns>
	  CurveGroupName Name {get;}

	  /// <summary>
	  /// Finds the curve with the specified name.
	  /// <para>
	  /// If the curve cannot be found, empty is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <returns> the curve, empty if not found </returns>
	  Optional<Curve> findCurve(CurveName name);

	  /// <summary>
	  /// Returns a stream of all curves in the group.
	  /// </summary>
	  /// <returns> a stream of all curves in the group </returns>
	  Stream<Curve> stream();

	}

}
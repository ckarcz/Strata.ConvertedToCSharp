/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	/// <summary>
	/// Interface for classes that extend the functionality of <seealso cref="Minimizer"/> by providing
	/// a method that allows the search area for the minimum to be bounded. 
	/// </summary>
	public interface ScalarMinimizer : Minimizer<System.Func<double, double>, double>
	{

	  /// <param name="function"> The function to minimize, not null </param>
	  /// <param name="startPosition"> The start position </param>
	  /// <param name="lowerBound"> The lower bound </param>
	  /// <param name="upperBound"> The upper bound, must be greater than the upper bound </param>
	  /// <returns> The minimum </returns>
	  double minimize(System.Func<double, double> function, double startPosition, double lowerBound, double upperBound);

	}

}
using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Calculates the population standard deviation of a series of data.
	/// The population standard deviation of a series of data is defined as the square root of 
	/// the population variance (see <seealso cref="PopulationVarianceCalculator"/>).
	/// </summary>
	public class PopulationStandardDeviationCalculator : System.Func<double[], double>
	{

	  private static readonly System.Func<double[], double> VARIANCE = new PopulationVarianceCalculator();

	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Length > 1, "Need at least two points to calculate standard deviation");
		return Math.Sqrt(VARIANCE.apply(x));
	  }

	}

}
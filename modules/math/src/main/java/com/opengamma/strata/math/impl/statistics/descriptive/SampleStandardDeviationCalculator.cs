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
	/// Calculates the sample standard deviation of a series of data. The sample standard deviation of a series of data is defined as the square root of 
	/// the sample variance (see <seealso cref="SampleVarianceCalculator"/>).
	/// </summary>
	public class SampleStandardDeviationCalculator : System.Func<double[], double>
	{

	  private static readonly System.Func<double[], double> VARIANCE = new SampleVarianceCalculator();

	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Length >= 2, "Need at least two points to calculate standard deviation");
		return Math.Sqrt(VARIANCE.apply(x));
	  }

	}

}
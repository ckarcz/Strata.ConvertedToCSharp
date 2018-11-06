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
	/// For a series of data $x_1, x_2, \dots, x_n$, the percentile is the value $x$
	/// below which a certain percentage of the data fall. 
	/// </summary>
	public class PercentileCalculator : System.Func<double[], double>
	{

	  private double _percentile;

	  /// <param name="percentile"> The percentile, must be between 0 and 1 </param>
	  public PercentileCalculator(double percentile)
	  {
		ArgChecker.isTrue(percentile > 0 && percentile < 1, "Percentile must be between 0 and 1");
		_percentile = percentile;
	  }

	  /// <param name="percentile"> The percentile, must be between 0 and 1 </param>
	  public virtual double Percentile
	  {
		  set
		  {
			ArgChecker.isTrue(value > 0 && value < 1, "Percentile must be between 0 and 1");
			_percentile = value;
		  }
	  }

	  /// <param name="x"> The data, not null or empty </param>
	  /// <returns> The percentile </returns>
	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Length > 0, "x cannot be empty");
		int length = x.Length;
		double[] copy = Arrays.copyOf(x, length);
		Arrays.sort(copy);
		double n = _percentile * (length - 1) + 1;
		if ((long)Math.Round(n, MidpointRounding.AwayFromZero) == 1)
		{
		  return copy[0];
		}
		if ((long)Math.Round(n, MidpointRounding.AwayFromZero) == length)
		{
		  return copy[length - 1];
		}
		double d = n % 1;
		int k = (int) (long)Math.Round(n - d, MidpointRounding.AwayFromZero);
		return copy[k - 1] + d * (copy[k] - copy[k - 1]);
	  }

	}

}
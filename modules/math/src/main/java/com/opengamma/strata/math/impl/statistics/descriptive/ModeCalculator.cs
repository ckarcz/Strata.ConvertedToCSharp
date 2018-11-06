using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// The mode of a series of data is the value that occurs more frequently in the data set.
	/// </summary>
	public class ModeCalculator : System.Func<double[], double>
	{
	  private const double EPS = 1e-16;

	  //TODO more than one value can be the mode
	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Length > 0, "x cannot be empty");
		if (x.Length == 1)
		{
		  return x[0];
		}
		double[] x1 = Arrays.copyOf(x, x.Length);
		Arrays.sort(x1);
		SortedDictionary<int, double> counts = new SortedDictionary<int, double>();
		int count = 1;
		for (int i = 1; i < x1.Length; i++)
		{
		  if (Math.Abs(x1[i] - x1[i - 1]) < EPS)
		  {
			count++;
		  }
		  else
		  {
			counts[count] = x1[i - 1];
			count = 1;
		  }
		}
		if (counts.lastKey() == 1)
		{
		  throw new MathException("Could not find mode for array; no repeated values");
		}
		return counts.lastEntry().Value;
	  }

	}

}
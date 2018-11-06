﻿using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// The sample skewness gives a measure of the asymmetry of the probability
	/// distribution of a variable. For a series of data $x_1, x_2, \dots, x_n$, an
	/// unbiased estimator of the sample skewness is
	/// $$
	/// \begin{align*}
	/// \mu_3 = \frac{\sqrt{n(n-1)}}{n-2}\frac{\frac{1}{n}\sum_{i=1}^n (x_i - \overline{x})^3}{\left(\frac{1}{n}\sum_{i=1}^n (x_i - \overline{x})^2\right)^\frac{3}{2}}
	/// \end{align*}
	/// $$
	/// where $\overline{x}$ is the sample mean.
	/// </summary>
	public class SampleSkewnessCalculator : System.Func<double[], double>
	{

	  private static readonly System.Func<double[], double> MEAN = new MeanCalculator();

	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Length >= 3, "Need at least three points to calculate sample skewness");
		double sum = 0;
		double variance = 0;
		double mean = MEAN.apply(x);
		foreach (double d in x)
		{
		  double diff = d - mean;
		  variance += diff * diff;
		  sum += diff * diff * diff;
		}
		int n = x.Length;
		variance /= n - 1;
		return Math.Sqrt(n - 1.0) * sum / (Math.Pow(variance, 1.5) * Math.Sqrt(n) * (n - 2));
	  }

	}

}
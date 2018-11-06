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
	/// Calculates the geometric mean of a series of data. 
	/// <para>
	/// The geometric mean $\mu$ of a series of elements $x_1, x_2, \dots, x_n$ is given by:
	/// $$
	/// \begin{align*}
	/// \mu = \left({\prod\limits_{i=1}^n x_i}\right)^{\frac{1}{n}}
	/// \end{align*}
	/// $$
	/// 
	/// </para>
	/// </summary>
	public class GeometricMeanCalculator : System.Func<double[], double>
	{

	  /// <param name="x"> The array of data, not null or empty </param>
	  /// <returns> The geometric mean </returns>
	  public override double? apply(double[] x)
	  {
		ArgChecker.notEmpty(x, "x");
		int n = x.Length;
		double mult = x[0];
		for (int i = 1; i < n; i++)
		{
		  mult *= x[i];
		}
		return Math.Pow(mult, 1.0 / n);
	  }

	}

}
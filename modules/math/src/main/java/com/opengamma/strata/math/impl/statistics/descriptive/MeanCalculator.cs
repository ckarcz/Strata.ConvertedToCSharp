/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Calculates the arithmetic mean of a series of data.
	/// <para>
	/// The arithmetic mean $\mu$ of a series of elements $x_1, x_2, \dots, x_n$ is given by:
	/// $$
	/// \begin{align*}
	/// \mu = \frac{1}{n}\left({\sum\limits_{i=1}^n x_i}\right)
	/// \end{align*}
	/// $$
	/// </para>
	/// </summary>
	public class MeanCalculator : System.Func<double[], double>
	{

	  public override double? apply(double[] x)
	  {
		ArgChecker.notEmpty(x, "x");
		if (x.Length == 1)
		{
		  return x[0];
		}
		double sum = 0;
		foreach (double d in x)
		{
		  sum += d;
		}
		return sum / x.Length;
	  }

	}

}
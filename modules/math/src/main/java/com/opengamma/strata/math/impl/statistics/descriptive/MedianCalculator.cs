/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Calculates the median of a series of data.
	/// <para>
	/// If the data are sorted from lowest to highest $(x_1, x_2, \dots, x_n)$, the median is given by
	/// $$
	/// \begin{align*}
	/// m =
	/// \begin{cases}
	/// x_{\frac{n+1}{2}}\quad & n \text{ odd}\\
	/// \frac{1}{2}\left(x_{\frac{n}{2}} + x_{\frac{n}{2} + 1}\right)\quad & n \text{ even}
	/// \end{cases} 
	/// \end{align*}
	/// $$
	/// </para>
	/// </summary>
	public class MedianCalculator : System.Func<double[], double>
	{

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
		int mid = x1.Length / 2;
		if (x1.Length % 2 == 1)
		{
		  return x1[mid];
		}
		return (x1[mid] + x1[mid - 1]) / 2.0;
	  }

	}

}
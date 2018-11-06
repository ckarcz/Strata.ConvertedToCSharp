/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Calculates the sample variance of a series of data. 
	/// <para> 
	/// The unbiased sample variance $\mathrm{var}$ of a series $x_1, x_2, \dots, x_n$ is given by:
	/// $$
	/// \begin{align*}
	/// \text{var} = \frac{1}{n-1}\sum_{i=1}^{n}(x_i - \overline{x})^2
	/// \end{align*}
	/// $$
	/// where $\overline{x}$ is the sample mean. For the population variance, see <seealso cref="PopulationVarianceCalculator"/>.
	/// </para>
	/// </summary>
	public class SampleVarianceCalculator : System.Func<double[], double>
	{

	  private static readonly System.Func<double[], double> MEAN = new MeanCalculator();

	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Length >= 2, "Need at least two points to calculate the sample variance");
		double? mean = MEAN.apply(x);
		double sum = 0;
		foreach (double value in x)
		{
		  double diff = value - mean;
		  sum += diff * diff;
		}
		int n = x.Length;
		return sum / (n - 1);
	  }

	}

}
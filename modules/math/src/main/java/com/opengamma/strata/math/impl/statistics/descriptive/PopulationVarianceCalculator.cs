/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Calculates the population variance of a series of data.
	/// <para>
	/// The unbiased population variance $\mathrm{var}$ of a series $x_1, x_2, \dots, x_n$ is given by:
	/// $$
	/// \begin{align*}
	/// \text{var} = \frac{1}{n}\sum_{i=1}^{n}(x_i - \overline{x})^2
	/// \end{align*}
	/// $$
	/// where $\overline{x}$ is the sample mean. For the sample variance, see <seealso cref="SampleVarianceCalculator"/>.
	/// </para>
	/// </summary>
	public class PopulationVarianceCalculator : System.Func<double[], double>
	{

	  private readonly System.Func<double[], double> _variance = new SampleVarianceCalculator();

	  public override double? apply(double[] x)
	  {
		ArgChecker.notNull(x, "x");
		int n = x.Length;
		ArgChecker.isTrue(n >= 2, "Need at least two points to calculate the population variance");
		return _variance.apply(x) * (n - 1) / n;
	  }

	}

}
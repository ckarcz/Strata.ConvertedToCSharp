using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Implementation of a quantile and expected shortfall estimator for series with exponentially weighted probabilities.
	/// <para>
	/// Reference: "Value-at-risk", OpenGamma Documentation 31, Version 0.2, January 2016. Section A.4.
	/// </para>
	/// </summary>
	public sealed class ExponentiallyWeightedInterpolationQuantileMethod : QuantileCalculationMethod
	{

	  /// <summary>
	  /// The exponential weight. </summary>
	  private readonly double lambda;

	  /// <summary>
	  /// Constructor.
	  /// <para>
	  /// The exponential weight lambda must be > 0 and < 1.0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="lambda">  the exponential weight </param>
	  public ExponentiallyWeightedInterpolationQuantileMethod(double lambda)
	  {
		ArgChecker.inRangeExclusive(lambda, 0.0d, 1.0d, "exponential weight");
		this.lambda = lambda;
	  }

	  public override QuantileResult quantileResultFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileDetails(level, sample, false, false);
	  }

	  public override QuantileResult quantileResultWithExtrapolationFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileDetails(level, sample, true, false);
	  }

	  public override double quantileFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileResultFromUnsorted(level, sample).Value;
	  }

	  public override double quantileWithExtrapolationFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileResultWithExtrapolationFromUnsorted(level, sample).Value;
	  }

	  public override QuantileResult expectedShortfallResultFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileDetails(level, sample, true, true);
	  }

	  public override double expectedShortfallFromUnsorted(double level, DoubleArray sample)
	  {
		return expectedShortfallResultFromUnsorted(level, sample).Value;
	  }

	  /// <summary>
	  /// Compute the quantile estimation and the details used in the result.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// The details consists on the indices of the samples actually used in the quantile computation - indices in the
	  /// input sample - and the weights for each of those samples. The details are sufficient to recompute the
	  /// quantile directly from the input sample.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted, the first step is to sort the data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> The quantile estimation and its details </returns>
	  public QuantileResult quantileDetailsFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileDetails(level, sample, true, false);
	  }

	  /// <summary>
	  /// Compute the expected shortfall and the details used in the result.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the expected shortfall with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range, the nearest data point is used, i.e.,
	  /// expected short fall is computed with flat extrapolation.
	  /// Thus this is coherent to <seealso cref="#quantileWithExtrapolationFromUnsorted(double, DoubleArray)"/>.
	  /// </para>
	  /// <para>
	  /// The details consists on the indices of the samples actually used in the expected shortfall computation - indices
	  /// in the input sample - and the weights for each of those samples. The details are sufficient to recompute the
	  /// expected shortfall directly from the input sample.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted, the first step is to sort the data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> The expected shortfall estimation and its detail </returns>
	  public QuantileResult expectedShortfallDetailsFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileDetails(level, sample, true, true);
	  }

	  // Generic quantile computation with quantile details.
	  private QuantileResult quantileDetails(double level, DoubleArray sample, bool isExtrapolated, bool isEs)
	  {
		int nbData = sample.size();
		double[] w = weights(nbData);
		/* Sorting data and keeping weight information. The arrays are modified */
		double[] s = sample.toArray();
		DoubleArrayMath.sortPairs(s, w);

		double[] s2 = sample.toArray();
		double[] order = new double[s2.Length];
		for (int i = 0; i < s2.Length; i++)
		{
		  order[i] = i;
		}
		DoubleArrayMath.sortPairs(s2, order);
		/* Find the index. */
		double runningWeight = 0.0d;
		int index = nbData;
		while (runningWeight < 1.0d - level)
		{
		  index--;
		  runningWeight += w[index];
		}
		if (isEs)
		{
		  return esFromIndexRunningWeight(index, runningWeight, s2, w, order, level);
		}
		return quantileFromIndexRunningWeight(index, runningWeight, isExtrapolated, s2, w, order, level);
	  }

	  /// <summary>
	  /// Computes value-at-risk.
	  /// </summary>
	  /// <param name="index">  the index from which the VaR should be computed </param>
	  /// <param name="runningWeight">  the running weight up to index </param>
	  /// <param name="isExtrapolated">  flag indicating if value should be extrapolated (flat) beyond the last value </param>
	  /// <param name="s">  the sorted sample </param>
	  /// <param name="w">  the sorted weights </param>
	  /// <param name="order">  the order of the sorted sample in the unsorted sample </param>
	  /// <param name="level">  the level at which the VaR should be computed </param>
	  /// <returns> the VaR and the details of sample data used to compute it </returns>
	  private QuantileResult quantileFromIndexRunningWeight(int index, double runningWeight, bool isExtrapolated, double[] s, double[] w, double[] order, double level)
	  {
		int nbData = s.Length;
		if ((index == nbData - 1) || (index == nbData))
		{
		  ArgChecker.isTrue(isExtrapolated, "Quantile can not be computed above the highest probability level.");
		  return QuantileResult.of(s[nbData - 1], new int[]{(int) (long)Math.Round(order[nbData - 1], MidpointRounding.AwayFromZero)}, DoubleArray.of(1.0d));
		}
		double alpha = (runningWeight - (1.0 - level)) / w[index];
		int[] indices = new int[nbData - index];
		double[] impacts = new double[nbData - index];
		for (int i = 0; i < nbData - index; i++)
		{
		  indices[i] = (int) (long)Math.Round(order[index + i], MidpointRounding.AwayFromZero);
		}
		impacts[0] = 1 - alpha;
		impacts[1] = alpha;
		return QuantileResult.of((1 - alpha) * s[index] + alpha * s[index + 1], indices, DoubleArray.ofUnsafe(impacts));
	  }

	  /// <summary>
	  /// Computes expected shortfall.
	  /// </summary>
	  /// <param name="index">  the index from which the ES should be computed </param>
	  /// <param name="runningWeight">  the running weight up to index </param>
	  /// <param name="s">  the sorted sample </param>
	  /// <param name="w">  the sorted weights </param>
	  /// <param name="order">  the order of the sorted sample in the unsorted sample </param>
	  /// <param name="level">  the level at which the ES should be computed </param>
	  /// <returns> the expected shortfall and the details of sample data used to compute it </returns>
	  private QuantileResult esFromIndexRunningWeight(int index, double runningWeight, double[] s, double[] w, double[] order, double level)
	  {
		int nbData = s.Length;
		if ((index == nbData - 1) || (index == nbData))
		{
		  return QuantileResult.of(s[nbData - 1], new int[]{(int) (long)Math.Round(order[nbData - 1], MidpointRounding.AwayFromZero)}, DoubleArray.of(1.0d));
		}
		double alpha = (runningWeight - (1.0 - level)) / w[index];
		int[] indices = new int[nbData - index];
		double[] impacts = new double[nbData - index];
		for (int i = 0; i < nbData - index; i++)
		{
		  indices[i] = (int) (long)Math.Round(order[index + i], MidpointRounding.AwayFromZero);
		}
		impacts[0] = 0.5 * (1 - alpha) * (1 - alpha) * w[index] / (1.0 - level);
		impacts[1] = (alpha + 1) * 0.5 * (1 - alpha) * w[index] / (1.0 - level);
		for (int i = 1; i < nbData - index - 1; i++)
		{
		  impacts[i] += 0.5 * w[index + i] / (1.0 - level);
		  impacts[i + 1] += 0.5 * w[index + i] / (1.0 - level);
		}
		impacts[nbData - index - 1] += w[nbData - 1] / (1.0 - level);
		double es = 0;
		for (int i = 0; i < nbData - index; i++)
		{
		  es += s[index + i] * impacts[i];
		}
		return QuantileResult.of(es, indices, DoubleArray.ofUnsafe(impacts));
	  }

	  protected internal override QuantileResult quantile(double level, DoubleArray sortedSample, bool isExtrapolated)
	  {
		throw new System.NotSupportedException("Quantile available only from unsorted sample due to weights.");
	  }

	  protected internal override QuantileResult expectedShortfall(double level, DoubleArray sortedSample)
	  {
		throw new System.NotSupportedException("Expected Shortfall only from unsorted sample due to weights.");
	  }

	  /// <summary>
	  /// Returns the weights for a given sample size.
	  /// </summary>
	  /// <param name="size">  the sample size </param>
	  /// <returns> the weights </returns>
	  public double[] weights(int size)
	  {
		double w1 = (1.0 - 1.0D / lambda) / (1.0d - Math.Pow(lambda, -size));
		double[] w = new double[size];
		for (int i = 0; i < size; i++)
		{
		  w[i] = w1 / Math.Pow(lambda, i);
		}
		return w;
	  }

	}

}
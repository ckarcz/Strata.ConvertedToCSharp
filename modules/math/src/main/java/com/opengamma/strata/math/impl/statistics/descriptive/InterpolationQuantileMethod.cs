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
	/// Implementation of a quantile estimator.
	/// <para>
	/// The quantile is linearly interpolated between two sample values. The probability dimension
	/// <i>p<subscript>i</subscript></i> on which the interpolation take place (X axis) varies between actual implementation
	/// of the abstract class. For each probability <i>p<subscript>i</subscript></i>, the cumulative distribution value is
	/// the sample value with same index. The index used above are the Java index plus 1.
	/// </para>
	/// <para>
	/// Reference: Value-At-Risk, OpenGamma Documentation 31, Version 0.1, April 2015.
	/// </para>
	/// </summary>

	public abstract class InterpolationQuantileMethod : QuantileCalculationMethod
	{

	  protected internal override QuantileResult quantile(double level, DoubleArray sample, bool isExtrapolated)
	  {
		ArgChecker.isTrue(level > 0, "Quantile should be above 0.");
		ArgChecker.isTrue(level < 1, "Quantile should be below 1.");
		int sampleSize = sampleCorrection(sample.size());
		double adjustedLevel = checkIndex(level * sampleSize + indexCorrection(), sample.size(), isExtrapolated);
		double[] order = createIndexArray(sample.size());
		double[] s = sample.toArray();
		DoubleArrayMath.sortPairs(s, order);
		int lowerIndex = (int) Math.Floor(adjustedLevel);
		int upperIndex = (int) Math.Ceiling(adjustedLevel);
		double lowerWeight = upperIndex - adjustedLevel;
		double upperWeight = 1d - lowerWeight;
		return QuantileResult.of(lowerWeight * s[lowerIndex - 1] + upperWeight * s[upperIndex - 1], new int[]{(int) order[lowerIndex - 1], (int) order[upperIndex - 1]}, DoubleArray.of(lowerWeight, upperWeight));
	  }

	  protected internal override QuantileResult expectedShortfall(double level, DoubleArray sample)
	  {
		ArgChecker.isTrue(level > 0, "Quantile should be above 0.");
		ArgChecker.isTrue(level < 1, "Quantile should be below 1.");
		int sampleSize = sampleCorrection(sample.size());
		double fractionalIndex = level * sampleSize + indexCorrection();
		double adjustedLevel = checkIndex(fractionalIndex, sample.size(), true);
		double[] order = createIndexArray(sample.size());
		double[] s = sample.toArray();
		DoubleArrayMath.sortPairs(s, order);
		int lowerIndex = (int) Math.Floor(adjustedLevel);
		int upperIndex = (int) Math.Ceiling(adjustedLevel);
		int[] indices = new int[upperIndex];
		double[] weights = new double[upperIndex];
		double interval = 1d / (double) sampleSize;
		weights[0] = interval * (Math.Min(fractionalIndex, 1d) - indexCorrection());
		double losses = s[0] * weights[0];
		for (int i = 0; i < lowerIndex - 1; i++)
		{
		  losses += 0.5 * (s[i] + s[i + 1]) * interval;
		  indices[i] = (int) order[i];
		  weights[i] += 0.5 * interval;
		  weights[i + 1] += 0.5 * interval;
		}
		if (lowerIndex != upperIndex)
		{
		  double lowerWeight = upperIndex - adjustedLevel;
		  double upperWeight = 1d - lowerWeight;
		  double quantile = lowerWeight * s[lowerIndex - 1] + upperWeight * s[upperIndex - 1];
		  losses += 0.5 * (s[lowerIndex - 1] + quantile) * interval * upperWeight;
		  indices[lowerIndex - 1] = (int) order[lowerIndex - 1];
		  indices[upperIndex - 1] = (int) order[upperIndex - 1];
		  weights[lowerIndex - 1] += 0.5 * (1d + lowerWeight) * interval * upperWeight;
		  weights[upperIndex - 1] = 0.5 * upperWeight * interval * upperWeight;
		}
		if (fractionalIndex > sample.size())
		{
		  losses += s[sample.size() - 1] * (fractionalIndex - sample.size()) * interval;
		  indices[sample.size() - 1] = (int) order[sample.size() - 1];
		  weights[sample.size() - 1] += (fractionalIndex - sample.size()) * interval;
		}
		return QuantileResult.of(losses / level, indices, DoubleArray.ofUnsafe(weights).dividedBy(level));
	  }

	  //-------------------------------------------------------------------------

	  /// <summary>
	  /// Internal method returning the index correction for the specific implementation.
	  /// </summary>
	  /// <returns> the correction </returns>
	  internal abstract double indexCorrection();

	  /// <summary>
	  /// Internal method returning the sample size correction for the specific implementation.
	  /// </summary>
	  /// <param name="sampleSize">  the sample size </param>
	  /// <returns> the correction </returns>
	  internal abstract int sampleCorrection(int sampleSize);

	  /// <summary>
	  /// Generate an index of doubles.
	  /// <para>
	  /// Creates an index of doubles from 1.0 to a stipulated number, in increments of 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="indexArrayLength">  length of index array to be created </param>
	  /// <returns> array of indices </returns>
	  private double[] createIndexArray(int indexArrayLength)
	  {
		double[] indexArray = new double[indexArrayLength];
		for (int i = 0; i < indexArrayLength; i++)
		{
		  indexArray[i] = i;
		}
		return indexArray;
	  }

	}

}
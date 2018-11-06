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
	/// The estimation is one of the sorted sample data.
	/// </para>
	/// <para>
	/// Reference: Value-At-Risk, OpenGamma Documentation 31, Version 0.1, April 2015.
	/// </para>
	/// </summary>
	public abstract class DiscreteQuantileMethod : QuantileCalculationMethod
	{

	  protected internal override QuantileResult quantile(double level, DoubleArray sample, bool isExtrapolated)
	  {
		ArgChecker.isTrue(level > 0, "Quantile should be above 0.");
		ArgChecker.isTrue(level < 1, "Quantile should be below 1.");
		int sampleSize = sampleCorrection(sample.size());
		double[] order = createIndexArray(sample.size());
		double[] s = sample.toArray();
		DoubleArrayMath.sortPairs(s, order);
		int index = (int) checkIndex(this.index(level * sampleSize), sample.size(), isExtrapolated);
		int[] ind = new int[1];
		ind[0] = (int) order[index - 1];
		return QuantileResult.of(s[index - 1], ind, DoubleArray.of(1));
	  }

	  protected internal override QuantileResult expectedShortfall(double level, DoubleArray sample)
	  {
		ArgChecker.isTrue(level > 0, "Quantile should be above 0.");
		ArgChecker.isTrue(level < 1, "Quantile should be below 1.");
		int sampleSize = sampleCorrection(sample.size());
		double[] order = createIndexArray(sample.size());
		double[] s = sample.toArray();
		DoubleArrayMath.sortPairs(s, order);
		double fractionalIndex = level * sampleSize;
		int index = (int) checkIndex(this.index(fractionalIndex), sample.size(), true);
		int[] indices = new int[index];
		double[] weights = new double[index];
		double interval = 1d / (double) sampleSize;
		double losses = s[0] * interval * indexShift();
		for (int i = 0; i < index - 1; i++)
		{
		  losses += s[i] * interval;
		  indices[i] = (int) order[i];
		  weights[i] = interval;
		}
		losses += s[index - 1] * (fractionalIndex - index + 1 - indexShift()) * interval;
		indices[index - 1] = (int) order[index - 1];
		weights[0] += interval * indexShift();
		weights[index - 1] = (fractionalIndex - index + 1 - indexShift()) * interval;
		return QuantileResult.of(losses / level, indices, DoubleArray.ofUnsafe(weights).dividedBy(level));
	  }

	  //-------------------------------------------------------------------------

	  /// <summary>
	  /// Internal method computing the index for a give quantile multiply by sample size.
	  /// <para>
	  /// The quantile size is given by quantile * sample size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="quantileSize">  the quantile size </param>
	  /// <returns> the index in the sample </returns>
	  internal abstract int index(double quantileSize);

	  /// <summary>
	  /// Internal method returning the sample size correction for the specific implementation.
	  /// </summary>
	  /// <param name="sampleSize">  the sample size </param>
	  /// <returns> the correction </returns>
	  internal abstract int sampleCorrection(int sampleSize);

	  /// <summary>
	  /// Shift added to/subtracted from index during intermediate steps in the expected shortfall computation.
	  /// </summary>
	  /// <returns> the index shift </returns>
	  internal abstract double indexShift();

	  /// <summary>
	  /// Generate an index of doubles.
	  /// <para>
	  /// Creates an index of doubles from 0.0 to a stipulated number, in increments of 1.
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
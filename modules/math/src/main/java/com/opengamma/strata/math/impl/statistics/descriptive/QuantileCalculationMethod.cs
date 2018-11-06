using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Abstract method to estimate quantiles and expected shortfalls from sample observations.
	/// </summary>
	public abstract class QuantileCalculationMethod
	{

	  /// <summary>
	  /// Compute the quantile estimation.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations and 1% of the observation are above that level.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range,
	  /// {@code IllegalArgumentException} is thrown.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted.
	  /// </para>
	  /// <para>
	  /// The quantile result produced contains the quantile value, the indices of the data points used to compute
	  /// it as well as the weights assigned to each point in the computation. The indices are based on the original,
	  /// unsorted array. Additionally, the indices start from 0 and so do not need to be shifted to account for java
	  /// indexing, when using them to reference the data points in the quantile calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> the quantile estimation </returns>
	  public virtual QuantileResult quantileResultFromUnsorted(double level, DoubleArray sample)
	  {
		return quantile(level, sample, false);
	  }

	  /// <summary>
	  /// Compute the quantile estimation.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations and 1% of the observation are above that level.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range, the nearest data point is used, i.e.,
	  /// quantile is computed with flat extrapolation.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted.
	  /// </para>
	  /// <para>
	  /// The quantile result produced contains the quantile value, the indices of the data points used to compute
	  /// it as well as the weights assigned to each point in the computation. The indices are based on the original,
	  /// unsorted array. Additionally, the indices start from 0 and so do not need to be shifted to account for java
	  /// indexing, when using them to reference the data points in the quantile calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> the quantile estimation </returns>
	  public virtual QuantileResult quantileResultWithExtrapolationFromUnsorted(double level, DoubleArray sample)
	  {
		return quantile(level, sample, true);
	  }

	  /// <summary>
	  /// Compute the quantile estimation.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range,
	  /// {@code IllegalArgumentException} is thrown.
	  /// </para>
	  /// <para>
	  /// The sample observations are sorted from the smallest to the largest.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sortedSample">  the sample observations </param>
	  /// <returns> the quantile estimation </returns>
	  public virtual double quantileFromSorted(double level, DoubleArray sortedSample)
	  {
		return quantileResultFromUnsorted(level, sortedSample).Value;
	  }

	  /// <summary>
	  /// Compute the quantile estimation.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range,
	  /// {@code IllegalArgumentException} is thrown.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted, the first step is to sort the data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> The quantile estimation </returns>
	  public virtual double quantileFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileFromSorted(level, sample.sorted());
	  }

	  /// <summary>
	  /// Compute the quantile estimation.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range, the nearest data point is used, i.e.,
	  /// quantile is computed with flat extrapolation.
	  /// </para>
	  /// <para>
	  /// The sample observations are sorted from the smallest to the largest.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sortedSample">  the sample observations </param>
	  /// <returns> the quantile estimation </returns>
	  public virtual double quantileWithExtrapolationFromSorted(double level, DoubleArray sortedSample)
	  {
		return quantileResultWithExtrapolationFromUnsorted(level, sortedSample).Value;
	  }

	  /// <summary>
	  /// Compute the quantile estimation.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the quantile estimation with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range, the nearest data point is used, i.e.,
	  /// quantile is computed with flat extrapolation.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted, the first step is to sort the data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> The quantile estimation </returns>
	  public virtual double quantileWithExtrapolationFromUnsorted(double level, DoubleArray sample)
	  {
		return quantileWithExtrapolationFromSorted(level, sample.sorted());
	  }

	  //-------------------------------------------------------------------------

	  /// <summary>
	  /// Compute the expected shortfall.
	  /// <para>
	  /// The shortfall level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, the expected shortfall with the level 99% corresponds to
	  /// the average of the smallest 99% of the observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range, the nearest data point is used, i.e.,
	  /// expected short fall is computed with flat extrapolation.
	  /// Thus this is coherent to <seealso cref="#quantileWithExtrapolationFromUnsorted(double, DoubleArray)"/>.
	  /// </para>
	  /// <para>
	  /// The sample observations are supposed to be unsorted.
	  /// </para>
	  /// <para>
	  /// The quantile result produced contains the expected shortfall value, the indices of the data points used to compute
	  /// it as well as the weights assigned to each point in the computation. The indices are based on the original,
	  /// unsorted array. Additionally, the indices start from 0 and so do not need to be shifted to account for java
	  /// indexing, when using them to reference the data points in the quantile calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> the quantile estimation </returns>
	  public virtual QuantileResult expectedShortfallResultFromUnsorted(double level, DoubleArray sample)
	  {
		return expectedShortfall(level, sample);
	  }

	  /// <summary>
	  /// Compute the expected shortfall.
	  /// <para>
	  /// The quantile level is in decimal, i.e. 99% = 0.99 and 0 < level < 1 should be satisfied.
	  /// This is measured from the bottom, that is, Thus the expected shortfall with the level 99% corresponds to
	  /// the smallest 99% observations.
	  /// </para>
	  /// <para>
	  /// If index value computed from the level is outside of the sample data range, the nearest data point is used, i.e.,
	  /// expected short fall is computed with flat extrapolation.
	  /// Thus this is coherent to <seealso cref="#quantileWithExtrapolationFromSorted(double, DoubleArray)"/>.
	  /// </para>
	  /// <para>
	  /// The sample observations are sorted from the smallest to the largest.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sortedSample">  the sample observations </param>
	  /// <returns> the quantile estimation </returns>
	  public virtual double expectedShortfallFromSorted(double level, DoubleArray sortedSample)
	  {
		return expectedShortfallResultFromUnsorted(level, sortedSample).Value;
	  }

	  /// <summary>
	  /// Compute the expected shortfall.
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
	  /// The sample observations are supposed to be unsorted, the first step is to sort the data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> The expected shortfall estimation </returns>
	  public virtual double expectedShortfallFromUnsorted(double level, DoubleArray sample)
	  {
		return expectedShortfallFromSorted(level, sample.sorted());
	  }

	  //-------------------------------------------------------------------------

	  /// <summary>
	  /// Computed the quantile.
	  /// <para>
	  /// This protected method should be implemented in subclasses.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <param name="isExtrapolated">  extrapolated if true, not extrapolated otherwise. </param>
	  /// <returns> the quantile </returns>
	  protected internal abstract QuantileResult quantile(double level, DoubleArray sample, bool isExtrapolated);

	  /// <summary>
	  /// Computed the expected shortfall.
	  /// <para>
	  /// This protected method should be implemented in subclasses
	  /// and coherent to <seealso cref="#quantile(double, DoubleArray, boolean)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="level">  the quantile level </param>
	  /// <param name="sample">  the sample observations </param>
	  /// <returns> the expected shortfall </returns>
	  protected internal abstract QuantileResult expectedShortfall(double level, DoubleArray sample);

	  /// <summary>
	  /// Check the index is within the sample data range.
	  /// <para>
	  /// If the index is outside the data range, the nearest data point is used in case of {@code isExtrapolated == true} or
	  /// an exception is thrown in case of {@code isExtrapolated == false}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="size">  the sample size </param>
	  /// <param name="isExtrapolated">  extrapolated if true, not extrapolated otherwise </param>
	  /// <returns> the index </returns>
	  protected internal virtual double checkIndex(double index, int size, bool isExtrapolated)
	  {
		if (isExtrapolated)
		{
		  return Math.Min(Math.Max(index, 1), size);
		}
		ArgChecker.isTrue(index >= 1, "Quantile can not be computed below the lowest probability level.");
		ArgChecker.isTrue(index <= size, "Quantile can not be computed above the highest probability level.");
		return index;
	  }
	}

}
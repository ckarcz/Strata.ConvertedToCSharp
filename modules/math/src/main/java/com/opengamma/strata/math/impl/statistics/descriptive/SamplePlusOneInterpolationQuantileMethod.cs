/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{
	/// <summary>
	/// Implementation of a quantile estimator.
	/// <para>
	/// The quantile is linearly interpolated between two sample values.
	/// The probability dimension on which the interpolation take place (X axis) is the ratio of the sample index and the
	/// number of elements in the sample plus one ( <i>p<subscript>i</subscript> = i / (n+1)</i>). For each probability
	/// <i>p<subscript>i</subscript></i>, the distribution value is the sample value with same index.
	/// The index used above are the Java index plus 1.
	/// </para>
	/// <para>
	/// Reference: Value-At-Risk, OpenGamma Documentation 31, Version 0.1, April 2015.
	/// </para>
	/// </summary>
	public sealed class SamplePlusOneInterpolationQuantileMethod : InterpolationQuantileMethod
	{

	  /// <summary>
	  /// Default implementation. </summary>
	  public static readonly SamplePlusOneInterpolationQuantileMethod DEFAULT = new SamplePlusOneInterpolationQuantileMethod();

	  protected internal override double indexCorrection()
	  {
		return 0d;
	  }

	  internal override int sampleCorrection(int sampleSize)
	  {
		return sampleSize + 1;
	  }

	}

}
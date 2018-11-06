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
	/// The probability dimension on which the interpolation take place (X axis) is the ratio of the sample index - 0.5
	/// and the number of elements in the sample ( <i>p<subscript>i</subscript> = (i-0.5) / n</i>). The index correction
	/// is 0.5. For each probability <i>p<subscript>i</subscript></i>, the distribution value is the sample value with same
	/// index. The index used above are the Java index plus 1.
	/// </para>
	/// <para>
	/// Reference: Value-At-Risk, OpenGamma Documentation 31, Version 0.1, April 2015.
	/// </para>
	/// </summary>
	public sealed class MidwayInterpolationQuantileMethod : InterpolationQuantileMethod
	{

	  /// <summary>
	  /// Default implementation. </summary>
	  public static readonly MidwayInterpolationQuantileMethod DEFAULT = new MidwayInterpolationQuantileMethod();

	  protected internal override double indexCorrection()
	  {
		return 0.5d;
	  }

	  internal override int sampleCorrection(int sampleSize)
	  {
		return sampleSize;
	  }

	}

}
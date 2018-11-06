using System;

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
	/// The estimation is one of the sorted sample data. Its index is given by the nearest (Math.round) integer to the
	/// quantile multiplied by the size of the sample plus one.
	/// The Java index is the above index minus 1 (array index start at 0 and not 1).
	/// </para>
	/// <para>
	/// Reference: Value-At-Risk, OpenGamma Documentation 31, Version 0.1, April 2015.
	/// </para>
	/// </summary>
	public sealed class SamplePlusOneNearestIndexQuantileMethod : DiscreteQuantileMethod
	{

	  /// <summary>
	  /// Default implementation. </summary>
	  public static readonly SamplePlusOneNearestIndexQuantileMethod DEFAULT = new SamplePlusOneNearestIndexQuantileMethod();

	  internal override int index(double quantileSize)
	  {
		return (int) (long)Math.Round(quantileSize, MidpointRounding.AwayFromZero);
	  }

	  internal override int sampleCorrection(int sampleSize)
	  {
		return sampleSize + 1;
	  }

	  protected internal override double indexShift()
	  {
		return 0.5;
	  }

	}

}
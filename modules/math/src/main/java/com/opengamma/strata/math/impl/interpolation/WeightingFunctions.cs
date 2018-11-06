/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	/// <summary>
	/// Constants and implementations for standard weighting functions.
	/// <para>
	/// Each constant returns a standard weighting function.
	/// </para>
	/// </summary>
	public sealed class WeightingFunctions
	{

	  /// <summary>
	  /// Weighting function.
	  /// </summary>
	  public static readonly WeightingFunction LINEAR = LinearWeightingFunction.INSTANCE;
	  /// <summary>
	  /// Weighting function based on {@code Math.sin}.
	  /// </summary>
	  public static readonly WeightingFunction SINE = SineWeightingFunction.INSTANCE;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private WeightingFunctions()
	  {
	  }

	}

}
/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	/// 
	public class LogNaturalSplineHelper : NaturalSplineInterpolator
	{

	  /// <summary>
	  /// In contrast with the original natural spline, the tridiagonal algorithm is used by passing
	  /// <seealso cref="LogCubicSplineNaturalSolver"/>. Note that the data are NOT log-scaled at this stage.
	  /// </summary>
	  public LogNaturalSplineHelper() : base(new LogCubicSplineNaturalSolver())
	  {
	  }

	}

}
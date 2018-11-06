using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	/// 
	public class LognormalSkewnessFromVolatilityCalculator : System.Func<double, double, double>
	{

	  public override double applyAsDouble(double sigma, double t)
	  {
		double y = Math.Sqrt(Math.Exp(sigma * sigma * t) - 1);
		return y * (3 + y * y);
	  }

	}

}
using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.descriptive
{

	/// 
	public class LognormalFisherKurtosisFromVolatilityCalculator : System.Func<double, double, double>
	{

	  public override double applyAsDouble(double sigma, double t)
	  {
		double y = Math.Sqrt(Math.Exp(sigma * sigma * t) - 1);
		double y2 = y * y;
		return y2 * (16 + y2 * (15 + y2 * (6 + y2)));
	  }

	}

}
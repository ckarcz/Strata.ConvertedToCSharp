/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// 
	//CSOFF: JavadocMethod
	public abstract class MinimumBracketer
	{
	  private const double ZERO = 1e-15;
	  /// 
	  protected internal const double GOLDEN = 0.61803399;

	  public abstract double[] getBracketedPoints(System.Func<double, double> f, double xLower, double xUpper);

	  protected internal virtual void checkInputs(System.Func<double, double> f, double xLower, double xUpper)
	  {
		ArgChecker.notNull(f, "function");
		if (DoubleMath.fuzzyEquals(xLower, xUpper, ZERO))
		{
		  throw new System.ArgumentException("Lower and upper values were not distinct");
		}
	  }

	}

}
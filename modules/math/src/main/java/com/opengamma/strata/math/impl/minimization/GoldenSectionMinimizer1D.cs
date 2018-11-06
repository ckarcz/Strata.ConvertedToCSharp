using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// 
	public class GoldenSectionMinimizer1D : ScalarMinimizer
	{

	  private const double GOLDEN = 0.61803399;
	  private static readonly MinimumBracketer BRACKETER = new ParabolicMinimumBracketer();
	  private const int MAX_ITER = 10000;
	  private const double EPS = 1e-12;

	  public virtual double minimize(System.Func<double, double> f, double startPosition, double lower, double upper)
	  {
		return minimize(f, lower, upper);
	  }

	  /// <summary>
	  /// Minimize.
	  /// </summary>
	  /// <param name="f">  the function </param>
	  /// <param name="lower">  the lower bound </param>
	  /// <param name="upper">  the upper bound </param>
	  /// <returns> the result </returns>
	  public virtual double minimize(System.Func<double, double> f, double lower, double upper)
	  {
		ArgChecker.notNull(f, "function");
		double x0, x1, x2, x3, f1, f2, temp;
		int i = 0;
		double[] triplet = BRACKETER.getBracketedPoints(f, lower, upper);
		x0 = triplet[0];
		x3 = triplet[2];
		if (Math.Abs(triplet[2] - triplet[1]) > Math.Abs(triplet[1] - triplet[0]))
		{
		  x1 = triplet[1];
		  x2 = triplet[2] + GOLDEN * (triplet[1] - triplet[2]);
		}
		else
		{
		  x2 = triplet[1];
		  x1 = triplet[0] + GOLDEN * (triplet[1] - triplet[0]);
		}
		f1 = f(x1);
		f2 = f(x2);
		while (Math.Abs(x3 - x0) > EPS * (Math.Abs(x1) + Math.Abs(x2)))
		{
		  if (f2 < f1)
		  {
			temp = GOLDEN * (x2 - x3) + x3;
			x0 = x1;
			x1 = x2;
			x2 = temp;
			f1 = f2;
			f2 = f(temp);
		  }
		  else
		  {
			temp = GOLDEN * (x1 - x0) + x0;
			x3 = x2;
			x2 = x1;
			x1 = temp;
			f2 = f1;
			f1 = f(temp);
		  }
		  i++;
		  if (i > MAX_ITER)
		  {
			throw new MathException("Could not find minimum: this should not happen because minimum should have been successfully bracketed");
		  }
		}
		if (f1 < f2)
		{
		  return x1;
		}
		return x2;
	  }

	  public override double? minimize(System.Func<double, double> function, double? startPosition)
	  {
		throw new System.NotSupportedException("Need lower and upper bounds to use this minimization method");
	  }
	}

}
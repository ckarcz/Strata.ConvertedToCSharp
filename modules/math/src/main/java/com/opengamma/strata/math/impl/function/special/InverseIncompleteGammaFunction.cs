using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// 
	public class InverseIncompleteGammaFunction : System.Func<double, double, double>
	{
	//TODO either find another implementation or delete this class

	  private readonly System.Func<double, double> _lnGamma = new NaturalLogGammaFunction();
	  private const double EPS = 1e-8;

	  //-------------------------------------------------------------------------
	  public override double applyAsDouble(double a, double p)
	  {
		ArgChecker.notNegativeOrZero(a, "a");
		ArgChecker.inRangeExclusive(p, 0d, 1d, "p");
		double x;
		double err;
		double t;
		double u;
		double pp, lna1 = 0, afac = 0;
		double a1 = a - 1;
		System.Func<double, double> gammaIncomplete = new IncompleteGammaFunction(a);
		double gln = _lnGamma.apply(a);
		if (a > 1)
		{
		  lna1 = Math.Log(a1);
		  afac = Math.Exp(a1 * (lna1 - 1) - gln);
		  pp = p < 0.5 ? p : 1 - p;
		  t = Math.Sqrt(-2 * Math.Log(pp));
		  x = (2.30753 + t * 0.27061) / (1 + t * (0.99229 + t * 0.04481)) - t;
		  if (p < 0.5)
		  {
			x = -x;
		  }
		  x = Math.Max(0.001, a * Math.Pow(1 - 1.0 / (9 * a) - x / (3 * Math.Sqrt(a)), 3));
		}
		else
		{
		  t = 1 - a * (0.253 + a * 0.12);
		  if (p < t)
		  {
			x = Math.Pow(p / t, 1.0 / a);
		  }
		  else
		  {
			x = 1.0 - Math.Log(1 - (p - t) / (1.0 - t));
		  }
		}
		for (int i = 0; i < 12; i++)
		{
		  if (x <= 0)
		  {
			return 0.0;
		  }
		  err = gammaIncomplete(x) - p;
		  if (a > 1)
		  {
			t = afac * Math.Exp(-(x - a1) + a1 * (Math.Log(x) - lna1));
		  }
		  else
		  {
			t = Math.Exp(-x + a1 * Math.Log(x) - gln);
		  }
		  u = err / t;
		  t = u / (1 - 0.5 * Math.Min(1, u * ((a - 1) / x - 1)));
		  x -= t;
		  if (x <= 0)
		  {
			x = 0.05 * (x + t);
		  }
		  if (Math.Abs(t) < EPS * x)
		  {
			break;
		  }
		}
		return x;
	  }

	}

}
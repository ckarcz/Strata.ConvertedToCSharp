using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// 
	public class InverseIncompleteBetaFunction : System.Func<double, double>
	{
	//TODO either find another implementation or delete this class

	  private readonly double _a;
	  private readonly double _b;
	  private readonly System.Func<double, double> _lnGamma = new NaturalLogGammaFunction();
	  private readonly System.Func<double, double> _beta;
	  private const double EPS = 1e-9;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="a">  the a value </param>
	  /// <param name="b">  the b value </param>
	  public InverseIncompleteBetaFunction(double a, double b)
	  {
		ArgChecker.notNegativeOrZero(a, "a");
		ArgChecker.notNegativeOrZero(b, "b");
		_a = a;
		_b = b;
		_beta = new IncompleteBetaFunction(a, b);
	  }

	  //-------------------------------------------------------------------------
	  public override double? apply(double? x)
	  {
		ArgChecker.inRangeInclusive(x, 0d, 1d, "x");
		double pp, p, t, h, w, lnA, lnB, u, a1 = _a - 1;
		double b1 = _b - 1;
		if (_a >= 1 && _b >= 1)
		{
		  pp = x < 0.5 ? x.Value : 1 - x;
		  t = Math.Sqrt(-2 * Math.Log(pp));
		  p = (2.30753 + t * 0.27061) / (1 + t * (0.99229 + t * 0.04481)) - t;
		  if (p < 0.5)
		  {
			p *= -1;
		  }
		  a1 = (Math.Sqrt(p) - 3.0) / 6.0;
		  double tempA = 1.0 / (2 * _a - 1);
		  double tempB = 1.0 / (2 * _b - 1);
		  h = 2.0 / (tempA + tempB);
		  w = p * Math.Sqrt(a1 + h) / h - (tempB - tempA) * (a1 + 5.0 / 6 - 2.0 / (3 * h));
		  p = _a / (_a + _b + Math.Exp(2 * w));
		}
		else
		{
		  lnA = Math.Log(_a / (_a + _b));
		  lnB = Math.Log(_b / (_a + _b));
		  t = Math.Exp(_a * lnA) / _a;
		  u = Math.Exp(_b * lnB) / _b;
		  w = t + u;
		  if (x.Value < t / w)
		  {
			p = Math.Pow(_a * w * x, 1.0 / _a);
		  }
		  else
		  {
			p = 1 - Math.Pow(_b * w * (1 - x), 1.0 / _b);
		  }
		}
		double afac = -_lnGamma.apply(_a) - _lnGamma.apply(_b) + _lnGamma.apply(_a + _b);
		double error;
		for (int j = 0; j < 10; j++)
		{
		  if (DoubleMath.fuzzyEquals(p, 0d, 1e-16) || DoubleMath.fuzzyEquals(p, (double) 1, 1e-16))
		  {
			throw new MathException("a or b too small for accurate evaluation");
		  }
		  error = _beta.apply(p) - x;
		  t = Math.Exp(a1 * Math.Log(p) + b1 * Math.Log(1 - p) + afac);
		  u = error / t;
		  t = u / (1 - 0.5 * Math.Min(1, u * (a1 / p - b1 / (1 - p))));
		  p -= t;
		  if (p <= 0)
		  {
			p = 0.5 * (p + t);
		  }
		  if (p >= 1)
		  {
			p = 0.5 * (p + t + 1);
		  }
		  if (Math.Abs(t) < EPS * p && j > 0)
		  {
			break;
		  }
		}
		return p;
	  }

	}

}
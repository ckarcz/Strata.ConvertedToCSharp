using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
/*
 * This code is copied from the original library from the `cern.jet.stat` package.
 * Changes:
 * - class name (was Gamma)
 * - package name
 * - missing Javadoc param tags
 * - reformat
 * - make package scoped
 */
/*
Copyright � 1999 CERN - European Organization for Nuclear Research.
Permission to use, copy, modify, distribute and sell this software and its documentation for any purpose
is hereby granted without fee, provided that the above copyright notice appear in all copies and
that both that copyright notice and this permission notice appear in supporting documentation.
CERN makes no representations about the suitability of this software for any purpose.
It is provided "as is" without expressed or implied warranty.
*/
namespace com.opengamma.strata.math.impl.cern
{
	// CSOFF: ALL
	/// <summary>
	/// Gamma and Beta functions.
	/// <para>
	/// <b>Implementation:</b>
	/// <dt>
	/// Some code taken and adapted from the <A HREF="http://www.sci.usq.edu.au/staff/leighb/graph/Top.html">Java 2D Graph Package 2.4</A>,
	/// which in turn is a port from the <A HREF="http://people.ne.mediaone.net/moshier/index.html#Cephes">Cephes 2.2</A> Math Library (C).
	/// Most Cephes code (missing from the 2D Graph Package) directly ported.
	/// 
	/// @author wolfgang.hoschek@cern.ch
	/// @version 0.9, 22-Jun-99
	/// </para>
	/// </summary>
	internal class GammaFunctions : Constants
	{

	  /// <summary>
	  /// Makes this class non instantiable, but still let's others inherit from it.
	  /// </summary>
	  protected internal GammaFunctions()
	  {
	  }

	  /// <summary>
	  /// Returns the beta function of the arguments.
	  /// <pre>
	  ///                   -     -
	  ///                  | (a) | (b)
	  /// beta( a, b )  =  -----------.
	  ///                     -
	  ///                    | (a+b)
	  /// </pre>
	  /// </summary>
	  /// <param name="a"> a </param>
	  /// <param name="b"> b </param>
	  /// <returns> result </returns>
	  /// <exception cref="ArithmeticException"> error </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double beta(double a, double b) throws ArithmeticException
	  public static double beta(double a, double b)
	  {
		double y;

		y = a + b;
		y = gamma(y);
		if (y == 0.0)
		{
		  return 1.0;
		}

		if (a > b)
		{
		  y = gamma(a) / y;
		  y *= gamma(b);
		}
		else
		{
		  y = gamma(b) / y;
		  y *= gamma(a);
		}

		return (y);
	  }

	  /// <summary>
	  /// Returns the Gamma function of the argument.
	  /// </summary>
	  /// <param name="inX"> x </param>
	  /// <returns> result </returns>
	  /// <exception cref="ArithmeticException"> error </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double gamma(double inX) throws ArithmeticException
	  public static double gamma(double inX)
	  {
		double x = inX;

		double[] P = new double[] {1.60119522476751861407E-4, 1.19135147006586384913E-3, 1.04213797561761569935E-2, 4.76367800457137231464E-2, 2.07448227648435975150E-1, 4.94214826801497100753E-1, 9.99999999999999996796E-1};
		double[] Q = new double[] {-2.31581873324120129819E-5, 5.39605580493303397842E-4, -4.45641913851797240494E-3, 1.18139785222060435552E-2, 3.58236398605498653373E-2, -2.34591795718243348568E-1, 7.14304917030273074085E-2, 1.00000000000000000320E0};
	//double MAXGAM = 171.624376956302725;
	//double LOGPI  = 1.14472988584940017414;

		double p, z;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unused") int i;
		int i;

		double q = Math.Abs(x);

		if (q > 33.0)
		{
		  if (x < 0.0)
		  {
			p = Math.Floor(q);
			if (p == q)
			{
			  throw new ArithmeticException("gamma: overflow");
			}
			i = (int) p;
			z = q - p;
			if (z > 0.5)
			{
			  p += 1.0;
			  z = q - p;
			}
			z = q * Math.Sin(Math.PI * z);
			if (z == 0.0)
			{
			  throw new ArithmeticException("gamma: overflow");
			}
			z = Math.Abs(z);
			z = Math.PI / (z * stirlingFormula(q));

			return -z;
		  }
		  else
		  {
			return stirlingFormula(x);
		  }
		}

		z = 1.0;
		while (x >= 3.0)
		{
		  x -= 1.0;
		  z *= x;
		}

		while (x < 0.0)
		{
		  if (x == 0.0)
		  {
			throw new ArithmeticException("gamma: singular");
		  }
		  else if (x > -1.E-9)
		  {
			return (z / ((1.0 + 0.5772156649015329 * x) * x));
		  }
		  z /= x;
		  x += 1.0;
		}

		while (x < 2.0)
		{
		  if (x == 0.0)
		  {
			throw new ArithmeticException("gamma: singular");
		  }
		  else if (x < 1.e-9)
		  {
			return (z / ((1.0 + 0.5772156649015329 * x) * x));
		  }
		  z /= x;
		  x += 1.0;
		}

		if ((x == 2.0) || (x == 3.0))
		{
		  return z;
		}

		x -= 2.0;
		p = Polynomial.polevl(x, P, 6);
		q = Polynomial.polevl(x, Q, 7);
		return z * p / q;

	  }

	  /// <summary>
	  /// Returns the Incomplete Beta Function evaluated from zero to <tt>xx</tt>; formerly named <tt>ibeta</tt>.
	  /// </summary>
	  /// <param name="aa"> the alpha parameter of the beta distribution. </param>
	  /// <param name="bb"> the beta parameter of the beta distribution. </param>
	  /// <param name="xx"> the integration end point. </param>
	  /// <returns> result </returns>
	  /// <exception cref="ArithmeticException"> error </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double incompleteBeta(double aa, double bb, double xx) throws ArithmeticException
	  public static double incompleteBeta(double aa, double bb, double xx)
	  {
		double a, b, t, x, xc, w, y;
		bool flag;

		if (aa <= 0.0 || bb <= 0.0)
		{
		  throw new ArithmeticException("ibeta: Domain error!");
		}

		if ((xx <= 0.0) || (xx >= 1.0))
		{
		  if (xx == 0.0)
		  {
			return 0.0;
		  }
		  if (xx == 1.0)
		  {
			return 1.0;
		  }
		  throw new ArithmeticException("ibeta: Domain error!");
		}

		flag = false;
		if ((bb * xx) <= 1.0 && xx <= 0.95)
		{
		  t = powerSeries(aa, bb, xx);
		  return t;
		}

		w = 1.0 - xx;

		/* Reverse a and b if x is greater than the mean. */
		if (xx > (aa / (aa + bb)))
		{
		  flag = true;
		  a = bb;
		  b = aa;
		  xc = xx;
		  x = w;
		}
		else
		{
		  a = aa;
		  b = bb;
		  xc = w;
		  x = xx;
		}

		if (flag && (b * x) <= 1.0 && x <= 0.95)
		{
		  t = powerSeries(a, b, x);
		  if (t <= MACHEP)
		  {
			t = 1.0 - MACHEP;
		  }
		  else
		  {
			t = 1.0 - t;
		  }
		  return t;
		}

		/* Choose expansion for better convergence. */
		y = x * (a + b - 2.0) - (a - 1.0);
		if (y < 0.0)
		{
		  w = incompleteBetaFraction1(a, b, x);
		}
		else
		{
		  w = incompleteBetaFraction2(a, b, x) / xc;
		}

		/* Multiply w by the factor
		   a      b   _             _     _
		  x  (1-x)   | (a+b) / ( a | (a) | (b) ) .   */

		y = a * Math.Log(x);
		t = b * Math.Log(xc);
		if ((a + b) < MAXGAM && Math.Abs(y) < MAXLOG && Math.Abs(t) < MAXLOG)
		{
		  t = Math.Pow(xc, b);
		  t *= Math.Pow(x, a);
		  t /= a;
		  t *= w;
		  t *= gamma(a + b) / (gamma(a) * gamma(b));
		  if (flag)
		  {
			if (t <= MACHEP)
			{
			  t = 1.0 - MACHEP;
			}
			else
			{
			  t = 1.0 - t;
			}
		  }
		  return t;
		}
		/* Resort to logarithms.  */
		y += t + logGamma(a + b) - logGamma(a) - logGamma(b);
		y += Math.Log(w / a);
		if (y < MINLOG)
		{
		  t = 0.0;
		}
		else
		{
		  t = Math.Exp(y);
		}

		if (flag)
		{
		  if (t <= MACHEP)
		  {
			t = 1.0 - MACHEP;
		  }
		  else
		  {
			t = 1.0 - t;
		  }
		}
		return t;
	  }

	  /// <summary>
	  /// Continued fraction expansion #1 for incomplete beta integral; formerly named <tt>incbcf</tt>.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static double incompleteBetaFraction1(double a, double b, double x) throws ArithmeticException
	  internal static double incompleteBetaFraction1(double a, double b, double x)
	  {
		double xk, pk, pkm1, pkm2, qk, qkm1, qkm2;
		double k1, k2, k3, k4, k5, k6, k7, k8;
		double r, t, ans, thresh;
		int n;

		k1 = a;
		k2 = a + b;
		k3 = a;
		k4 = a + 1.0;
		k5 = 1.0;
		k6 = b - 1.0;
		k7 = k4;
		k8 = a + 2.0;

		pkm2 = 0.0;
		qkm2 = 1.0;
		pkm1 = 1.0;
		qkm1 = 1.0;
		ans = 1.0;
		r = 1.0;
		n = 0;
		thresh = 3.0 * MACHEP;
		do
		{
		  xk = -(x * k1 * k2) / (k3 * k4);
		  pk = pkm1 + pkm2 * xk;
		  qk = qkm1 + qkm2 * xk;
		  pkm2 = pkm1;
		  pkm1 = pk;
		  qkm2 = qkm1;
		  qkm1 = qk;

		  xk = (x * k5 * k6) / (k7 * k8);
		  pk = pkm1 + pkm2 * xk;
		  qk = qkm1 + qkm2 * xk;
		  pkm2 = pkm1;
		  pkm1 = pk;
		  qkm2 = qkm1;
		  qkm1 = qk;

		  if (qk != 0)
		  {
			r = pk / qk;
		  }
		  if (r != 0)
		  {
			t = Math.Abs((ans - r) / r);
			ans = r;
		  }
		  else
		  {
			t = 1.0;
		  }

		  if (t < thresh)
		  {
			return ans;
		  }

		  k1 += 1.0;
		  k2 += 1.0;
		  k3 += 2.0;
		  k4 += 2.0;
		  k5 += 1.0;
		  k6 -= 1.0;
		  k7 += 2.0;
		  k8 += 2.0;

		  if ((Math.Abs(qk) + Math.Abs(pk)) > big)
		  {
			pkm2 *= biginv;
			pkm1 *= biginv;
			qkm2 *= biginv;
			qkm1 *= biginv;
		  }
		  if ((Math.Abs(qk) < biginv) || (Math.Abs(pk) < biginv))
		  {
			pkm2 *= big;
			pkm1 *= big;
			qkm2 *= big;
			qkm1 *= big;
		  }
		} while (++n < 300);

		return ans;
	  }

	  /// <summary>
	  /// Continued fraction expansion #2 for incomplete beta integral; formerly named <tt>incbd</tt>.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static double incompleteBetaFraction2(double a, double b, double x) throws ArithmeticException
	  internal static double incompleteBetaFraction2(double a, double b, double x)
	  {
		double xk, pk, pkm1, pkm2, qk, qkm1, qkm2;
		double k1, k2, k3, k4, k5, k6, k7, k8;
		double r, t, ans, z, thresh;
		int n;

		k1 = a;
		k2 = b - 1.0;
		k3 = a;
		k4 = a + 1.0;
		k5 = 1.0;
		k6 = a + b;
		k7 = a + 1.0;
		;
		k8 = a + 2.0;

		pkm2 = 0.0;
		qkm2 = 1.0;
		pkm1 = 1.0;
		qkm1 = 1.0;
		z = x / (1.0 - x);
		ans = 1.0;
		r = 1.0;
		n = 0;
		thresh = 3.0 * MACHEP;
		do
		{
		  xk = -(z * k1 * k2) / (k3 * k4);
		  pk = pkm1 + pkm2 * xk;
		  qk = qkm1 + qkm2 * xk;
		  pkm2 = pkm1;
		  pkm1 = pk;
		  qkm2 = qkm1;
		  qkm1 = qk;

		  xk = (z * k5 * k6) / (k7 * k8);
		  pk = pkm1 + pkm2 * xk;
		  qk = qkm1 + qkm2 * xk;
		  pkm2 = pkm1;
		  pkm1 = pk;
		  qkm2 = qkm1;
		  qkm1 = qk;

		  if (qk != 0)
		  {
			r = pk / qk;
		  }
		  if (r != 0)
		  {
			t = Math.Abs((ans - r) / r);
			ans = r;
		  }
		  else
		  {
			t = 1.0;
		  }

		  if (t < thresh)
		  {
			return ans;
		  }

		  k1 += 1.0;
		  k2 -= 1.0;
		  k3 += 2.0;
		  k4 += 2.0;
		  k5 += 1.0;
		  k6 += 1.0;
		  k7 += 2.0;
		  k8 += 2.0;

		  if ((Math.Abs(qk) + Math.Abs(pk)) > big)
		  {
			pkm2 *= biginv;
			pkm1 *= biginv;
			qkm2 *= biginv;
			qkm1 *= biginv;
		  }
		  if ((Math.Abs(qk) < biginv) || (Math.Abs(pk) < biginv))
		  {
			pkm2 *= big;
			pkm1 *= big;
			qkm2 *= big;
			qkm1 *= big;
		  }
		} while (++n < 300);

		return ans;
	  }

	  /// <summary>
	  /// Returns the Incomplete Gamma function; formerly named <tt>igamma</tt>. </summary>
	  /// <param name="a"> the parameter of the gamma distribution. </param>
	  /// <param name="x"> the integration end point. </param>
	  /// <returns> result </returns>
	  /// <exception cref="ArithmeticException"> error </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double incompleteGamma(double a, double x) throws ArithmeticException
	  public static double incompleteGamma(double a, double x)
	  {

		double ans, ax, c, r;

		if (x <= 0 || a <= 0)
		{
		  return 0.0;
		}

		if (x > 1.0 && x > a)
		{
		  return 1.0 - incompleteGammaComplement(a, x);
		}

		/* Compute  x**a * exp(-x) / gamma(a)  */
		ax = a * Math.Log(x) - x - logGamma(a);
		if (ax < -MAXLOG)
		{
		  return (0.0);
		}

		ax = Math.Exp(ax);

		/* power series */
		r = a;
		c = 1.0;
		ans = 1.0;

		do
		{
		  r += 1.0;
		  c *= x / r;
		  ans += c;
		} while (c / ans > MACHEP);

		return (ans * ax / a);

	  }

	  /// <summary>
	  /// Returns the Complemented Incomplete Gamma function; formerly named <tt>igamc</tt>. </summary>
	  /// <param name="a"> the parameter of the gamma distribution. </param>
	  /// <param name="x"> the integration start point. </param>
	  /// <returns> result </returns>
	  /// <exception cref="ArithmeticException"> error </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double incompleteGammaComplement(double a, double x) throws ArithmeticException
	  public static double incompleteGammaComplement(double a, double x)
	  {
		double ans, ax, c, yc, r, t, y, z;
		double pk, pkm1, pkm2, qk, qkm1, qkm2;

		if (x <= 0 || a <= 0)
		{
		  return 1.0;
		}

		if (x < 1.0 || x < a)
		{
		  return 1.0 - incompleteGamma(a, x);
		}

		ax = a * Math.Log(x) - x - logGamma(a);
		if (ax < -MAXLOG)
		{
		  return 0.0;
		}

		ax = Math.Exp(ax);

		/* continued fraction */
		y = 1.0 - a;
		z = x + y + 1.0;
		c = 0.0;
		pkm2 = 1.0;
		qkm2 = x;
		pkm1 = x + 1.0;
		qkm1 = z * x;
		ans = pkm1 / qkm1;

		do
		{
		  c += 1.0;
		  y += 1.0;
		  z += 2.0;
		  yc = y * c;
		  pk = pkm1 * z - pkm2 * yc;
		  qk = qkm1 * z - qkm2 * yc;
		  if (qk != 0)
		  {
			r = pk / qk;
			t = Math.Abs((ans - r) / r);
			ans = r;
		  }
		  else
		  {
			t = 1.0;
		  }

		  pkm2 = pkm1;
		  pkm1 = pk;
		  qkm2 = qkm1;
		  qkm1 = qk;
		  if (Math.Abs(pk) > big)
		  {
			pkm2 *= biginv;
			pkm1 *= biginv;
			qkm2 *= biginv;
			qkm1 *= biginv;
		  }
		} while (t > MACHEP);

		return ans * ax;
	  }

	  /// <summary>
	  /// Returns the natural logarithm of the gamma function; formerly named <tt>lgamma</tt>. </summary>
	  /// <param name="inX"> x </param>
	  /// <returns> result </returns>
	  /// <exception cref="ArithmeticException"> error </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static double logGamma(double inX) throws ArithmeticException
	  public static double logGamma(double inX)
	  {
		double x = inX;
		double p, q, w, z;

		double[] A = new double[] {8.11614167470508450300E-4, -5.95061904284301438324E-4, 7.93650340457716943945E-4, -2.77777777730099687205E-3, 8.33333333333331927722E-2};
		double[] B = new double[] {-1.37825152569120859100E3, -3.88016315134637840924E4, -3.31612992738871184744E5, -1.16237097492762307383E6, -1.72173700820839662146E6, -8.53555664245765465627E5};
		double[] C = new double[] {-3.51815701436523470549E2, -1.70642106651881159223E4, -2.20528590553854454839E5, -1.13933444367982507207E6, -2.53252307177582951285E6, -2.01889141433532773231E6};

		if (x < -34.0)
		{
		  q = -x;
		  w = logGamma(q);
		  p = Math.Floor(q);
		  if (p == q)
		  {
			throw new ArithmeticException("lgam: Overflow");
		  }
		  z = q - p;
		  if (z > 0.5)
		  {
			p += 1.0;
			z = p - q;
		  }
		  z = q * Math.Sin(Math.PI * z);
		  if (z == 0.0)
		  {
			throw new ArithmeticException("lgamma: Overflow");
		  }
		  z = LOGPI - Math.Log(z) - w;
		  return z;
		}

		if (x < 13.0)
		{
		  z = 1.0;
		  while (x >= 3.0)
		  {
			x -= 1.0;
			z *= x;
		  }
		  while (x < 2.0)
		  {
			if (x == 0.0)
			{
			  throw new ArithmeticException("lgamma: Overflow");
			}
			z /= x;
			x += 1.0;
		  }
		  if (z < 0.0)
		  {
			z = -z;
		  }
		  if (x == 2.0)
		  {
			return Math.Log(z);
		  }
		  x -= 2.0;
		  p = x * Polynomial.polevl(x, B, 5) / Polynomial.p1evl(x, C, 6);
		  return (Math.Log(z) + p);
		}

		if (x > 2.556348e305)
		{
		  throw new ArithmeticException("lgamma: Overflow");
		}

		q = (x - 0.5) * Math.Log(x) - x + 0.91893853320467274178;
		//if( x > 1.0e8 ) return( q );
		if (x > 1.0e8)
		{
		  return (q);
		}

		p = 1.0 / (x * x);
		if (x >= 1000.0)
		{
		  q += ((7.9365079365079365079365e-4 * p - 2.7777777777777777777778e-3) * p + 0.0833333333333333333333) / x;
		}
		else
		{
		  q += Polynomial.polevl(p, A, 4) / x;
		}
		return q;
	  }

	  /// <summary>
	  /// Power series for incomplete beta integral; formerly named <tt>pseries</tt>.
	  /// Use when b*x is small and x not too close to 1.  
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static double powerSeries(double a, double b, double x) throws ArithmeticException
	  internal static double powerSeries(double a, double b, double x)
	  {
		double s, t, u, v, n, t1, z, ai;

		ai = 1.0 / a;
		u = (1.0 - b) * x;
		v = u / (a + 1.0);
		t1 = v;
		t = u;
		n = 2.0;
		s = 0.0;
		z = MACHEP * ai;
		while (Math.Abs(v) > z)
		{
		  u = (n - b) * x / n;
		  t *= u;
		  v = t / (a + n);
		  s += v;
		  n += 1.0;
		}
		s += t1;
		s += ai;

		u = a * Math.Log(x);
		if ((a + b) < MAXGAM && Math.Abs(u) < MAXLOG)
		{
		  t = GammaFunctions.gamma(a + b) / (GammaFunctions.gamma(a) * GammaFunctions.gamma(b));
		  s = s * t * Math.Pow(x, a);
		}
		else
		{
		  t = GammaFunctions.logGamma(a + b) - GammaFunctions.logGamma(a) - GammaFunctions.logGamma(b) + u + Math.Log(s);
		  if (t < MINLOG)
		  {
			s = 0.0;
		  }
		  else
		  {
			s = Math.Exp(t);
		  }
		}
		return s;
	  }

	  /// <summary>
	  /// Returns the Gamma function computed by Stirling's formula; formerly named <tt>stirf</tt>.
	  /// The polynomial STIR is valid for 33 <= x <= 172.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: static double stirlingFormula(double x) throws ArithmeticException
	  internal static double stirlingFormula(double x)
	  {
		double[] STIR = new double[] {7.87311395793093628397E-4, -2.29549961613378126380E-4, -2.68132617805781232825E-3, 3.47222221605458667310E-3, 8.33333333333482257126E-2};
		double MAXSTIR = 143.01608;

		double w = 1.0 / x;
		double y = Math.Exp(x);

		w = 1.0 + w * Polynomial.polevl(w, STIR, 4);

		if (x > MAXSTIR)
		{
		  /* Avoid overflow in Math.pow() */
		  double v = Math.Pow(x, 0.5 * x - 0.25);
		  y = v * (v / y);
		}
		else
		{
		  y = Math.Pow(x, x - 0.5) / y;
		}
		y = SQTPI * y * w;
		return y;
	  }
	}

}
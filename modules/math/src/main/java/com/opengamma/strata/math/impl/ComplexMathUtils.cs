using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Utilities for working with complex numbers.
	/// </summary>
	public class ComplexMathUtils
	{
	// CSOFF: JavadocMethod

	  public static ComplexNumber add(ComplexNumber z1, ComplexNumber z2)
	  {
		ArgChecker.notNull(z1, "z1");
		ArgChecker.notNull(z2, "z2");
		return new ComplexNumber(z1.Real + z2.Real, z1.Imaginary + z2.Imaginary);
	  }

	  public static ComplexNumber add(params ComplexNumber[] z)
	  {
		ArgChecker.notNull(z, "z");
		double res = 0.0;
		double img = 0.0;
		foreach (ComplexNumber aZ in z)
		{
		  res += aZ.Real;
		  img += aZ.Imaginary;
		}
		return new ComplexNumber(res, img);
	  }

	  public static ComplexNumber add(ComplexNumber z, double x)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real + x, z.Imaginary);
	  }

	  public static ComplexNumber add(double x, ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real + x, z.Imaginary);
	  }

	  public static double arg(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return Math.Atan2(z.Imaginary, z.Real);
	  }

	  public static ComplexNumber conjugate(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real, -z.Imaginary);
	  }

	  public static ComplexNumber divide(ComplexNumber z1, ComplexNumber z2)
	  {
		ArgChecker.notNull(z1, "z1");
		ArgChecker.notNull(z2, "z2");
		double a = z1.Real;
		double b = z1.Imaginary;
		double c = z2.Real;
		double d = z2.Imaginary;
		if (Math.Abs(c) > Math.Abs(d))
		{
		  double dOverC = d / c;
		  double denom = c + d * dOverC;
		  return new ComplexNumber((a + b * dOverC) / denom, (b - a * dOverC) / denom);
		}
		double cOverD = c / d;
		double denom = c * cOverD + d;
		return new ComplexNumber((a * cOverD + b) / denom, (b * cOverD - a) / denom);
	  }

	  public static ComplexNumber divide(ComplexNumber z, double x)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real / x, z.Imaginary / x);
	  }

	  public static ComplexNumber divide(double x, ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double c = z.Real;
		double d = z.Imaginary;
		if (Math.Abs(c) > Math.Abs(d))
		{
		  double dOverC = d / c;
		  double denom = c + d * dOverC;
		  return new ComplexNumber(x / denom, -x * dOverC / denom);
		}
		double cOverD = c / d;
		double denom = c * cOverD + d;
		return new ComplexNumber(x * cOverD / denom, -x / denom);
	  }

	  public static ComplexNumber exp(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double mult = Math.Exp(z.Real);
		return new ComplexNumber(mult * Math.Cos(z.Imaginary), mult * Math.Sin(z.Imaginary));
	  }

	  public static ComplexNumber inverse(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double c = z.Real;
		double d = z.Imaginary;
		if (Math.Abs(c) > Math.Abs(d))
		{
		  double dOverC = d / c;
		  double denom = c + d * dOverC;
		  return new ComplexNumber(1 / denom, -dOverC / denom);
		}
		double cOverD = c / d;
		double denom = c * cOverD + d;
		return new ComplexNumber(cOverD / denom, -1 / denom);
	  }

	  /// <summary>
	  /// Returns the principal value of log, with z the principal argument of z defined to lie in the interval (-pi, pi]. </summary>
	  /// <param name="z"> ComplexNumber </param>
	  /// <returns> The log </returns>
	  public static ComplexNumber log(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(Math.Log(Math.hypot(z.Real, z.Imaginary)), Math.Atan2(z.Imaginary, z.Real));
	  }

	  public static double mod(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return Math.hypot(z.Real, z.Imaginary);
	  }

	  public static ComplexNumber square(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double a = z.Real;
		double b = z.Imaginary;
		return new ComplexNumber(a * a - b * b, 2 * a * b);
	  }

	  public static ComplexNumber multiply(ComplexNumber z1, ComplexNumber z2)
	  {
		ArgChecker.notNull(z1, "z1");
		ArgChecker.notNull(z2, "z2");
		double a = z1.Real;
		double b = z1.Imaginary;
		double c = z2.Real;
		double d = z2.Imaginary;
		return new ComplexNumber(a * c - b * d, a * d + b * c);
	  }

	  public static ComplexNumber multiply(params ComplexNumber[] z)
	  {
		ArgChecker.notNull(z, "z");
		int n = z.Length;
		ArgChecker.isTrue(n > 0, "nothing to multiply");
		if (n == 1)
		{
		  return z[0];
		}
		else if (n == 2)
		{
		  return multiply(z[0], z[1]);
		}
		else
		{
		  ComplexNumber product = multiply(z[0], z[1]);
		  for (int i = 2; i < n; i++)
		  {
			product = multiply(product, z[i]);
		  }
		  return product;
		}
	  }

	  public static ComplexNumber multiply(double x, params ComplexNumber[] z)
	  {
		ComplexNumber product = multiply(z);
		return multiply(x, product);
	  }

	  public static ComplexNumber multiply(ComplexNumber z, double x)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real * x, z.Imaginary * x);
	  }

	  public static ComplexNumber multiply(double x, ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real * x, z.Imaginary * x);
	  }

	  public static ComplexNumber pow(ComplexNumber z1, ComplexNumber z2)
	  {
		ArgChecker.notNull(z1, "z1");
		ArgChecker.notNull(z2, "z2");
		double mod = ComplexMathUtils.mod(z1);
		double arg = ComplexMathUtils.arg(z1);
		double mult = Math.Pow(mod, z2.Real) * Math.Exp(-z2.Imaginary * arg);
		double theta = z2.Real * arg + z2.Imaginary * Math.Log(mod);
		return new ComplexNumber(mult * Math.Cos(theta), mult * Math.Sin(theta));
	  }

	  public static ComplexNumber pow(ComplexNumber z, double x)
	  {
		double mod = ComplexMathUtils.mod(z);
		double arg = ComplexMathUtils.arg(z);
		double mult = Math.Pow(mod, x);
		return new ComplexNumber(mult * Math.Cos(x * arg), mult * Math.Sin(x * arg));
	  }

	  public static ComplexNumber pow(double x, ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return pow(new ComplexNumber(x, 0), z);
	  }

	  public static ComplexNumber sqrt(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double c = z.Real;
		double d = z.Imaginary;
		if (c == 0.0 && d == 0.0)
		{
		  return z;
		}
		double w;
		if (Math.Abs(c) > Math.Abs(d))
		{
		  double dOverC = d / c;
		  w = Math.Sqrt(Math.Abs(c)) * Math.Sqrt((1 + Math.Sqrt(1 + dOverC * dOverC)) / 2);
		}
		else
		{
		  double cOverD = c / d;
		  w = Math.Sqrt(Math.Abs(d)) * Math.Sqrt((Math.Abs(cOverD) + Math.Sqrt(1 + cOverD * cOverD)) / 2);
		}
		if (c >= 0.0)
		{
		  return new ComplexNumber(w, d / 2 / w);
		}
		if (d >= 0.0)
		{
		  return new ComplexNumber(d / 2 / w, w);
		}
		return new ComplexNumber(-d / 2 / w, -w);
	  }

	  public static ComplexNumber subtract(ComplexNumber z1, ComplexNumber z2)
	  {
		ArgChecker.notNull(z1, "z1");
		ArgChecker.notNull(z2, "z2");
		return new ComplexNumber(z1.Real - z2.Real, z1.Imaginary - z2.Imaginary);
	  }

	  public static ComplexNumber subtract(ComplexNumber z, double x)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(z.Real - x, z.Imaginary);
	  }

	  public static ComplexNumber subtract(double x, ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(x - z.Real, -z.Imaginary);
	  }

	}

}
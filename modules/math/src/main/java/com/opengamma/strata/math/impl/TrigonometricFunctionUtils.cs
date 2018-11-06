using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.ComplexNumber.I;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Trigonometric utilities.
	/// </summary>
	public class TrigonometricFunctionUtils
	{
	// CSOFF: JavadocMethod

	  private static readonly ComplexNumber NEGATIVE_I = new ComplexNumber(0, -1);

	  public static double acos(double x)
	  {
		return Math.Acos(x);
	  }

	  /// <summary>
	  /// arccos - the inverse of cos. </summary>
	  /// <param name="z"> A complex number </param>
	  /// <returns> acos(z) </returns>
	  public static ComplexNumber acos(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return ComplexMathUtils.multiply(NEGATIVE_I, ComplexMathUtils.log(ComplexMathUtils.add(z, ComplexMathUtils.sqrt(ComplexMathUtils.subtract(ComplexMathUtils.multiply(z, z), 1)))));
	  }

	  public static double acosh(double x)
	  {
		double y = x * x - 1;
		ArgChecker.isTrue(y >= 0, "|x|>=1.0 for real solution");
		return Math.Log(x + Math.Sqrt(x * x - 1));
	  }

	  public static ComplexNumber acosh(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return ComplexMathUtils.log(ComplexMathUtils.add(z, ComplexMathUtils.sqrt(ComplexMathUtils.subtract(ComplexMathUtils.multiply(z, z), 1))));
	  }

	  public static double asin(double x)
	  {

		return Math.Asin(x);
	  }

	  public static ComplexNumber asin(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return ComplexMathUtils.multiply(NEGATIVE_I, ComplexMathUtils.log(ComplexMathUtils.add(ComplexMathUtils.multiply(I, z), ComplexMathUtils.sqrt(ComplexMathUtils.subtract(1, ComplexMathUtils.multiply(z, z))))));
	  }

	  public static double asinh(double x)
	  {
		return Math.Log(x + Math.Sqrt(x * x + 1));
	  }

	  public static ComplexNumber asinh(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return ComplexMathUtils.log(ComplexMathUtils.add(z, ComplexMathUtils.sqrt(ComplexMathUtils.add(ComplexMathUtils.multiply(z, z), 1))));
	  }

	  public static double atan(double x)
	  {
		return Math.Atan(x);
	  }

	  public static ComplexNumber atan(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		ComplexNumber iZ = ComplexMathUtils.multiply(z, I);
		ComplexNumber half = new ComplexNumber(0, 0.5);
		return ComplexMathUtils.multiply(half, ComplexMathUtils.log(ComplexMathUtils.divide(ComplexMathUtils.subtract(1, iZ), ComplexMathUtils.add(1, iZ))));
	  }

	  public static double atanh(double x)
	  {
		return 0.5 * Math.Log((1 + x) / (1 - x));
	  }

	  public static ComplexNumber atanh(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return ComplexMathUtils.multiply(0.5, ComplexMathUtils.log(ComplexMathUtils.divide(ComplexMathUtils.add(1, z), ComplexMathUtils.subtract(1, z))));
	  }

	  public static double cos(double x)
	  {
		return Math.Cos(x);
	  }

	  public static ComplexNumber cos(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double x = z.Real;
		double y = z.Imaginary;
		return new ComplexNumber(Math.Cos(x) * Math.Cosh(y), -Math.Sin(x) * Math.Sinh(y));
	  }

	  public static double cosh(double x)
	  {
		return Math.Cosh(x);
	  }

	  public static ComplexNumber cosh(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(Math.Cosh(z.Real) * Math.Cos(z.Imaginary), Math.Sinh(z.Real) * Math.Sin(z.Imaginary));
	  }

	  public static double sin(double x)
	  {
		return Math.Sin(x);
	  }

	  public static ComplexNumber sin(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		double x = z.Real;
		double y = z.Imaginary;
		return new ComplexNumber(Math.Sin(x) * Math.Cosh(y), Math.Cos(x) * Math.Sinh(y));
	  }

	  public static double sinh(double x)
	  {
		return Math.Sinh(x);
	  }

	  public static ComplexNumber sinh(ComplexNumber z)
	  {
		ArgChecker.notNull(z, "z");
		return new ComplexNumber(Math.Sinh(z.Real) * Math.Cos(z.Imaginary), Math.Cosh(z.Real) * Math.Sin(z.Imaginary));
	  }

	  public static double tan(double x)
	  {
		return Math.Tan(x);
	  }

	  public static ComplexNumber tan(ComplexNumber z)
	  {
		ComplexNumber b = ComplexMathUtils.exp(ComplexMathUtils.multiply(ComplexMathUtils.multiply(I, 2), z));
		return ComplexMathUtils.divide(ComplexMathUtils.subtract(b, 1), ComplexMathUtils.multiply(I, ComplexMathUtils.add(b, 1)));
	  }

	  public static double tanh(double x)
	  {
		return Math.Tanh(x);
	  }

	  public static ComplexNumber tanh(ComplexNumber z)
	  {
		ComplexNumber z2 = ComplexMathUtils.exp(z);
		ComplexNumber z3 = ComplexMathUtils.exp(ComplexMathUtils.multiply(z, -1));
		return ComplexMathUtils.divide(ComplexMathUtils.subtract(z2, z3), ComplexMathUtils.add(z2, z3));
	  }

	}

}
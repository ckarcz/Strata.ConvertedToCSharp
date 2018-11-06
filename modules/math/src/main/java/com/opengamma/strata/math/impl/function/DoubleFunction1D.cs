/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FiniteDifferenceType = com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType;

	/// <summary>
	/// Defines a family of functions that take real arguments and return real values.
	/// The functionality of <seealso cref="Function"/> is extended; this class allows arithmetic
	/// operations on functions and defines a derivative function.
	/// </summary>
	public interface DoubleFunction1D : System.Func<double, double>
	{

	  /// <summary>
	  /// Returns a function that calculates the first derivative.
	  /// <para>
	  /// The method used is central finite difference, with $\epsilon = 10^{-5}$.
	  /// Implementing classes can override this method to return a function that
	  /// is the exact functional representation of the first derivative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a function that calculates the first derivative of this function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D derivative()
	//  {
	//	return derivative(FiniteDifferenceType.CENTRAL, 1e-5);
	//  }

	  /// <summary>
	  /// Returns a function that calculates the first derivative. The method used
	  /// is finite difference, with the differencing type and $\epsilon$ as arguments.
	  /// </summary>
	  /// <param name="differenceType">  the differencing type to use </param>
	  /// <param name="eps">  the $\epsilon$ to use </param>
	  /// <returns> a function that calculates the first derivative of this function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D derivative(com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType differenceType, double eps)
	//  {
	//	ArgChecker.notNull(differenceType, "difference type");
	//	switch (differenceType)
	//	{
	//	  case CENTRAL:
	//		return new DoubleFunction1D()
	//		{
	//
	//		  @@Override public double applyAsDouble(double x)
	//		  {
	//			return (DoubleFunction1D.this.applyAsDouble(x + eps) - DoubleFunction1D.this.applyAsDouble(x - eps)) / 2 / eps;
	//		  }
	//
	//		};
	//	  case BACKWARD:
	//		return new DoubleFunction1D()
	//		{
	//
	//		  @@Override public double applyAsDouble(double x)
	//		  {
	//			return (DoubleFunction1D.this.applyAsDouble(x) - DoubleFunction1D.this.applyAsDouble(x - eps)) / eps;
	//		  }
	//
	//		};
	//	  case FORWARD:
	//		return new DoubleFunction1D()
	//		{
	//
	//		  @@Override public double applyAsDouble(double x)
	//		  {
	//			return (DoubleFunction1D.this.applyAsDouble(x + eps) - DoubleFunction1D.this.applyAsDouble(x)) / eps;
	//		  }
	//
	//		};
	//	  default:
	//		throw new IllegalArgumentException("Unhandled FiniteDifferenceType " + differenceType);
	//	}
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, adding a function $f(x)$ returns the
	  /// function $h(x) = f(x) + g(x)$.
	  /// </summary>
	  /// <param name="f">  the function to add </param>
	  /// <returns> a function $h(x) = f(x) + g(x)$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D add(DoubleFunction1D f)
	//  {
	//	ArgChecker.notNull(f, "f");
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) + f.applyAsDouble(x);
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, adding a constant $a$ returns the function
	  /// $h(x) = g(x) + a$.
	  /// </summary>
	  /// <param name="a">  the constant to add </param>
	  /// <returns> a function $h(x) = g(x) + a$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D add(double a)
	//  {
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) + a;
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, dividing by a function $f(x)$ returns the
	  /// function $h(x) = \frac{g(x)}{f(x)}$.
	  /// </summary>
	  /// <param name="f">  the function to divide by </param>
	  /// <returns> a function $h(x) = \frac{f(x)}{g(x)}$ </returns>

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D divide(DoubleFunction1D f)
	//  {
	//	ArgChecker.notNull(f, "f");
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) / f.applyAsDouble(x);
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, dividing by a constant $a$ returns the
	  /// function $h(x) = \frac{g(x)}{a}$.
	  /// </summary>
	  /// <param name="a">  the constant to add </param>
	  /// <returns> a function $h(x) = \frac{g(x)}{a}$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D divide(double a)
	//  {
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) / a;
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, multiplying by a function $f(x)$ returns
	  /// the function $h(x) = f(x) g(x)$.
	  /// </summary>
	  /// <param name="f">  the function to multiply by </param>
	  /// <returns> a function $h(x) = f(x) g(x)$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D multiply(DoubleFunction1D f)
	//  {
	//	ArgChecker.notNull(f, "f");
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) * f.applyAsDouble(x);
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, multiplying by a constant $a$ returns the
	  /// function $h(x) = a g(x)$.
	  /// </summary>
	  /// <param name="a">  the constant to add </param>
	  /// <returns> a function $h(x) = a g(x)$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D multiply(double a)
	//  {
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) * a;
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, subtracting a function $f(x)$ returns the
	  /// function $h(x) = f(x) - g(x)$.
	  /// </summary>
	  /// <param name="f">  the function to subtract </param>
	  /// <returns> a function $h(x) = g(x) - f(x)$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D subtract(DoubleFunction1D f)
	//  {
	//	ArgChecker.notNull(f, "f");
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) - f.applyAsDouble(x);
	//	  }
	//
	//	};
	//  }

	  /// <summary>
	  /// For a DoubleFunction1D $g(x)$, subtracting a constant $a$ returns the
	  /// function $h(x) = g(x) - a$.
	  /// </summary>
	  /// <param name="a">  the constant to add </param>
	  /// <returns> a function $h(x) = g(x) - a$ </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default DoubleFunction1D subtract(double a)
	//  {
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return DoubleFunction1D.this.applyAsDouble(x) - a;
	//	  }
	//
	//	};
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a Function&lt;Double, Double> into a DoubleFunction1D.
	  /// </summary>
	  /// <param name="f">  the function to convert </param>
	  /// <returns> the converted function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static DoubleFunction1D from(System.Func<double, double> f)
	//  {
	//	ArgChecker.notNull(f, "f");
	//	return new DoubleFunction1D()
	//	{
	//
	//	  @@Override public double applyAsDouble(double x)
	//	  {
	//		return f.apply(x);
	//	  }
	//
	//	};
	//  }

	}

}
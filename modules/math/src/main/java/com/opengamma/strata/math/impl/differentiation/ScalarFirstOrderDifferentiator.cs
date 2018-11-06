using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Differentiates a scalar function with respect to its argument using finite difference.
	/// <para>
	/// For a function $y = f(x)$ where $x$ and $y$ are scalars, this class produces
	/// a gradient function $g(x)$, i.e. a function that returns the gradient for
	/// each point $x$, where $g$ is the scalar $\frac{dy}{dx}$.
	/// </para>
	/// </summary>
	public class ScalarFirstOrderDifferentiator : Differentiator<double, double, double>
	{

	  private const double DEFAULT_EPS = 1e-5;
	  private static readonly double MIN_EPS = Math.Sqrt(Double.MIN_NORMAL);

	  private readonly double eps;
	  private readonly double twoEps;
	  private readonly FiniteDifferenceType differenceType;

	  /// <summary>
	  /// Creates an instance using the default value of eps (10<sup>-5</sup>) and central differencing type.
	  /// </summary>
	  public ScalarFirstOrderDifferentiator() : this(FiniteDifferenceType.CENTRAL, DEFAULT_EPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance using the default value of eps (10<sup>-5</sup>).
	  /// </summary>
	  /// <param name="differenceType">  the differencing type to be used in calculating the gradient function </param>
	  public ScalarFirstOrderDifferentiator(FiniteDifferenceType differenceType) : this(differenceType, DEFAULT_EPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// <para>
	  /// If the size of the domain is very small or very large, consider re-scaling first.
	  /// If this value is too small, the result will most likely be dominated by noise.
	  /// Use around 10<sup>-5</sup> times the domain size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="differenceType">  the differencing type to be used in calculating the gradient function </param>
	  /// <param name="eps">  the step size used to approximate the derivative </param>
	  public ScalarFirstOrderDifferentiator(FiniteDifferenceType differenceType, double eps)
	  {
		ArgChecker.notNull(differenceType, "differenceType");
		ArgChecker.isTrue(eps >= MIN_EPS, "eps of {} is too small. Please choose a value > {}, such as 1e-5*size of domain", eps, MIN_EPS);
		this.differenceType = differenceType;
		this.eps = eps;
		this.twoEps = 2 * eps;
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<double, double> differentiate(System.Func<double, double> function)
	  {
		ArgChecker.notNull(function, "function");
		switch (differenceType)
		{
		  case com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType.FORWARD:
			return new FuncAnonymousInnerClass(this, function);
		  case com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType.CENTRAL:
			return new FuncAnonymousInnerClass2(this, function);
		  case com.opengamma.strata.math.impl.differentiation.FiniteDifferenceType.BACKWARD:
			return new FuncAnonymousInnerClass3(this, function);
		  default:
			throw new System.ArgumentException("Can only handle forward, backward and central differencing");
		}
	  }

	  private class FuncAnonymousInnerClass : System.Func<double, double>
	  {
		  private readonly ScalarFirstOrderDifferentiator outerInstance;

		  private System.Func<double, double> function;

		  public FuncAnonymousInnerClass(ScalarFirstOrderDifferentiator outerInstance, System.Func<double, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			ArgChecker.notNull(x, "x");
			return (function(x + outerInstance.eps) - function(x)) / outerInstance.eps;
		  }
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<double, double>
	  {
		  private readonly ScalarFirstOrderDifferentiator outerInstance;

		  private System.Func<double, double> function;

		  public FuncAnonymousInnerClass2(ScalarFirstOrderDifferentiator outerInstance, System.Func<double, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			ArgChecker.notNull(x, "x");
			return (function(x + outerInstance.eps) - function(x - outerInstance.eps)) / outerInstance.twoEps;
		  }
	  }

	  private class FuncAnonymousInnerClass3 : System.Func<double, double>
	  {
		  private readonly ScalarFirstOrderDifferentiator outerInstance;

		  private System.Func<double, double> function;

		  public FuncAnonymousInnerClass3(ScalarFirstOrderDifferentiator outerInstance, System.Func<double, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			ArgChecker.notNull(x, "x");
			return (function(x) - function(x - outerInstance.eps)) / outerInstance.eps;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<double, double> differentiate(System.Func<double, double> function, System.Func<double, bool> domain)
	  {

		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(domain, "domain");
		double[] wFwd = new double[] {-3.0 / twoEps, 4.0 / twoEps, -1.0 / twoEps};
		double[] wCent = new double[] {-1.0 / twoEps, 0.0, 1.0 / twoEps};
		double[] wBack = new double[] {1.0 / twoEps, -4.0 / twoEps, 3.0 / twoEps};

		return new FuncAnonymousInnerClass4(this, function, domain, wFwd, wCent, wBack);
	  }

	  private class FuncAnonymousInnerClass4 : System.Func<double, double>
	  {
		  private readonly ScalarFirstOrderDifferentiator outerInstance;

		  private System.Func<double, double> function;
		  private System.Func<double, bool> domain;
		  private double[] wFwd;
		  private double[] wCent;
		  private double[] wBack;

		  public FuncAnonymousInnerClass4(ScalarFirstOrderDifferentiator outerInstance, System.Func<double, double> function, System.Func<double, bool> domain, double[] wFwd, double[] wCent, double[] wBack)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
			  this.domain = domain;
			  this.wFwd = wFwd;
			  this.wCent = wCent;
			  this.wBack = wBack;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			ArgChecker.notNull(x, "x");
			ArgChecker.isTrue(domain(x), "point {} is not in the function domain", x.ToString());

			double[] y = new double[3];
			double[] w;

			if (!domain(x + outerInstance.eps))
			{
			  if (!domain(x - outerInstance.eps))
			  {
				throw new MathException("cannot get derivative at point " + x.ToString());
			  }
			  y[0] = function(x - outerInstance.twoEps);
			  y[1] = function(x - outerInstance.eps);
			  y[2] = function(x);
			  w = wBack;
			}
			else
			{
			  if (!domain(x - outerInstance.eps))
			  {
				y[0] = function(x);
				y[1] = function(x + outerInstance.eps);
				y[2] = function(x + outerInstance.twoEps);
				w = wFwd;
			  }
			  else
			  {
				y[0] = function(x - outerInstance.eps);
				y[2] = function(x + outerInstance.eps);
				w = wCent;
			  }
			}

			double res = y[0] * w[0] + y[2] * w[2];
			if (w[1] != 0)
			{
			  res += y[1] * w[1];
			}
			return res;
		  }
	  }

	}

}
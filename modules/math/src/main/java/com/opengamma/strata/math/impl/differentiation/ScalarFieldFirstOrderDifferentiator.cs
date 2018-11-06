using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Differentiates a scalar field (i.e. there is a scalar value for every point
	/// in some vector space) with respect to the vector space using finite difference.
	/// <para>
	/// For a function $y = f(\mathbf{x})$ where $\mathbf{x}$ is a n-dimensional
	/// vector and $y$ is a scalar, this class produces a gradient function
	/// $\mathbf{g}(\mathbf{x})$, i.e. a function that returns the gradient for each
	/// point $\mathbf{x}$, where $\mathbf{g}$ is the n-dimensional vector
	/// $\frac{dy}{dx_i}$.
	/// </para>
	/// </summary>
	public class ScalarFieldFirstOrderDifferentiator : Differentiator<DoubleArray, double, DoubleArray>
	{

	  private const double DEFAULT_EPS = 1e-5;
	  private static readonly double MIN_EPS = Math.Sqrt(Double.MIN_NORMAL);

	  private readonly double eps;
	  private readonly double twoEps;
	  private readonly FiniteDifferenceType differenceType;

	  /// <summary>
	  /// Creates an instance using the default values of differencing type (central) and eps (10<sup>-5</sup>).
	  /// </summary>
	  public ScalarFieldFirstOrderDifferentiator() : this(FiniteDifferenceType.CENTRAL, DEFAULT_EPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance that approximates the derivative of a scalar function by finite difference.
	  /// <para>
	  /// If the size of the domain is very small or very large, consider re-scaling first.
	  /// If this value is too small, the result will most likely be dominated by noise.
	  /// Use around 10<sup>-5</sup> times the domain size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="differenceType">  the type, forward, backward or central. In most situations, central is best </param>
	  /// <param name="eps">  the step size used to approximate the derivative </param>
	  public ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType differenceType, double eps)
	  {
		ArgChecker.notNull(differenceType, "differenceType");
		ArgChecker.isTrue(eps >= MIN_EPS, "eps of {} is too small. Please choose a value > {}, such as 1e-5*size of domain", eps, MIN_EPS);
		this.differenceType = differenceType;
		this.eps = eps;
		this.twoEps = 2 * eps;
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleArray> differentiate(System.Func<DoubleArray, double> function)
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

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleArray>
	  {
		  private readonly ScalarFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, double> function;

		  public FuncAnonymousInnerClass(ScalarFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleArray apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleArray apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			double y = function(x);
			return DoubleArray.of(x.size(), i =>
			{
			double up = function(x.with(i, x.get(i) + outerInstance.eps));
			return (up - y) / outerInstance.eps;
			});
		  }
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleArray>
	  {
		  private readonly ScalarFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, double> function;

		  public FuncAnonymousInnerClass2(ScalarFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleArray apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleArray apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			return DoubleArray.of(x.size(), i =>
			{
			double up = function(x.with(i, x.get(i) + outerInstance.eps));
			double down = function(x.with(i, x.get(i) - outerInstance.eps));
			return (up - down) / outerInstance.twoEps;
			});
		  }
	  }

	  private class FuncAnonymousInnerClass3 : System.Func<DoubleArray, DoubleArray>
	  {
		  private readonly ScalarFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, double> function;

		  public FuncAnonymousInnerClass3(ScalarFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleArray apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleArray apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			double y = function(x);
			return DoubleArray.of(x.size(), i =>
			{
			double down = function(x.with(i, x.get(i) - outerInstance.eps));
			return (y - down) / outerInstance.eps;
			});
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleArray> differentiate(System.Func<DoubleArray, double> function, System.Func<DoubleArray, bool> domain)
	  {

		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(domain, "domain");

		double[] wFwd = new double[] {-3.0 / twoEps, 4.0 / twoEps, -1.0 / twoEps};
		double[] wCent = new double[] {-1.0 / twoEps, 0.0, 1.0 / twoEps};
		double[] wBack = new double[] {1.0 / twoEps, -4.0 / twoEps, 3.0 / twoEps};

		return new FuncAnonymousInnerClass4(this, function, domain, wFwd, wCent, wBack);
	  }

	  private class FuncAnonymousInnerClass4 : System.Func<DoubleArray, DoubleArray>
	  {
		  private readonly ScalarFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, double> function;
		  private System.Func<DoubleArray, bool> domain;
		  private double[] wFwd;
		  private double[] wCent;
		  private double[] wBack;

		  public FuncAnonymousInnerClass4(ScalarFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, double> function, System.Func<DoubleArray, bool> domain, double[] wFwd, double[] wCent, double[] wBack)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
			  this.domain = domain;
			  this.wFwd = wFwd;
			  this.wCent = wCent;
			  this.wBack = wBack;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleArray apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleArray apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			ArgChecker.isTrue(domain(x), "point {} is not in the function domain", x.ToString());

			return DoubleArray.of(x.size(), i =>
			{
			double xi = x.get(i);
			DoubleArray xPlusOneEps = x.with(i, xi + outerInstance.eps);
			DoubleArray xMinusOneEps = x.with(i, xi - outerInstance.eps);
			double y0, y1, y2;
			double[] w;
			if (!domain(xPlusOneEps))
			{
				DoubleArray xMinusTwoEps = x.with(i, xi - outerInstance.twoEps);
				if (!domain(xMinusTwoEps))
				{
					throw new MathException("cannot get derivative at point " + x.ToString() + " in direction " + i);
				}
				y0 = function(xMinusTwoEps);
				y2 = function(x);
				y1 = function(xMinusOneEps);
				w = wBack;
			}
			else
			{
				double temp = function(xPlusOneEps);
				if (!domain(xMinusOneEps))
				{
					y1 = temp;
					y0 = function(x);
					y2 = function(x.with(i, xi + outerInstance.twoEps));
					w = wFwd;
				}
				else
				{
					y1 = 0;
					y2 = temp;
					y0 = function(xMinusOneEps);
					w = wCent;
				}
			}
			double res = y0 * w[0] + y2 * w[2];
			if (w[1] != 0)
			{
				res += y1 * w[1];
			}
			return res;
			});
		  }
	  }

	}

}
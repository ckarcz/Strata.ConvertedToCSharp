/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// a function that returns the second derivative value for each point, i.e., $\frac{d^2 f}{dx^2}$.
	/// </para>
	/// </summary>
	public class ScalarSecondOrderDifferentiator : Differentiator<double, double, double>
	{

	  /// <summary>
	  /// Default steps size.
	  /// </summary>
	  private const double DEFAULT_EPS = 1e-4;

	  private readonly double eps;
	  private readonly double epsSqr;
	  private readonly double twoEps;
	  private readonly double threeEps;

	  /// <summary>
	  /// Creates an instance using the default values.
	  /// </summary>
	  public ScalarSecondOrderDifferentiator() : this(DEFAULT_EPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance specifying the step size. 
	  /// </summary>
	  /// <param name="eps">  the step size </param>
	  public ScalarSecondOrderDifferentiator(double eps)
	  {
		this.eps = eps;
		this.epsSqr = eps * eps;
		this.twoEps = 2d * eps;
		this.threeEps = 3d * eps;
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<double, double> differentiate(System.Func<double, double> function)
	  {
		ArgChecker.notNull(function, "function");
		return new FuncAnonymousInnerClass(this, function);
	  }

	  private class FuncAnonymousInnerClass : System.Func<double, double>
	  {
		  private readonly ScalarSecondOrderDifferentiator outerInstance;

		  private System.Func<double, double> function;

		  public FuncAnonymousInnerClass(ScalarSecondOrderDifferentiator outerInstance, System.Func<double, double> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			ArgChecker.notNull(x, "x");
			return (function(x + outerInstance.eps) + function(x - outerInstance.eps) - 2d * function(x)) / outerInstance.epsSqr;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<double, double> differentiate(System.Func<double, double> function, System.Func<double, bool> domain)
	  {
		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(domain, "domain");
		return new FuncAnonymousInnerClass2(this, function, domain);
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<double, double>
	  {
		  private readonly ScalarSecondOrderDifferentiator outerInstance;

		  private System.Func<double, double> function;
		  private System.Func<double, bool> domain;

		  public FuncAnonymousInnerClass2(ScalarSecondOrderDifferentiator outerInstance, System.Func<double, double> function, System.Func<double, bool> domain)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
			  this.domain = domain;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			ArgChecker.notNull(x, "x");
			ArgChecker.isTrue(domain(x), "point {} is not in the function domain", x.ToString());
			if (!domain(x + outerInstance.threeEps))
			{
			  if (!domain(x - outerInstance.threeEps))
			  {
				throw new System.ArgumentException("cannot get derivative at point " + x.ToString());
			  }
			  return (-function(x - outerInstance.threeEps) + 4d * function(x - outerInstance.twoEps) - 5d * function(x - outerInstance.eps) + 2d * function(x)) / outerInstance.epsSqr;
			}
			else
			{
			  if (!domain(x - outerInstance.eps))
			  {
				return (-function(x + outerInstance.threeEps) + 4d * function(x + outerInstance.twoEps) - 5d * function(x + outerInstance.eps) + 2d * function(x)) / outerInstance.epsSqr;
			  }
			  return (function(x + outerInstance.eps) + function(x - outerInstance.eps) - 2d * function(x)) / outerInstance.epsSqr;
			}
		  }
	  }
	}

}
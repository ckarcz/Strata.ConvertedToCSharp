/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Differentiates a vector field (i.e. there is a vector value for every point
	/// in some vector space) with respect to the vector space using finite difference.
	/// <para>
	/// For a function $\mathbf{y} = f(\mathbf{x})$ where $\mathbf{x}$ is a
	/// n-dimensional vector and $\mathbf{y}$ is a m-dimensional vector, this class
	/// produces the Jacobian function $\mathbf{J}(\mathbf{x})$, i.e. a function
	/// that returns the Jacobian for each point $\mathbf{x}$, where
	/// $\mathbf{J}$ is the $m \times n$ matrix $\frac{dy_i}{dx_j}$
	/// </para>
	/// </summary>
	public class VectorFieldFirstOrderDifferentiator : Differentiator<DoubleArray, DoubleArray, DoubleMatrix>
	{

	  private const double DEFAULT_EPS = 1e-5;

	  private readonly double eps;
	  private readonly double twoEps;
	  private readonly FiniteDifferenceType differenceType;

	  /// <summary>
	  /// Creates an instance using the default value of eps (10<sup>-5</sup>) and central differencing type.
	  /// </summary>
	  public VectorFieldFirstOrderDifferentiator() : this(FiniteDifferenceType.CENTRAL, DEFAULT_EPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance using the default value of eps (10<sup>-5</sup>).
	  /// </summary>
	  /// <param name="differenceType">  the differencing type to be used in calculating the gradient function </param>
	  public VectorFieldFirstOrderDifferentiator(FiniteDifferenceType differenceType) : this(differenceType, DEFAULT_EPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance using the central differencing type.
	  /// <para>
	  /// If the size of the domain is very small or very large, consider re-scaling first.
	  /// If this value is too small, the result will most likely be dominated by noise.
	  /// Use around 10<sup>-5</sup> times the domain size.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="eps">  the step size used to approximate the derivative </param>
	  public VectorFieldFirstOrderDifferentiator(double eps) : this(FiniteDifferenceType.CENTRAL, eps)
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
	  public VectorFieldFirstOrderDifferentiator(FiniteDifferenceType differenceType, double eps)
	  {
		ArgChecker.notNull(differenceType, "differenceType");
		this.differenceType = differenceType;
		this.eps = eps;
		this.twoEps = 2 * eps;
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleMatrix> differentiate(System.Func<DoubleArray, DoubleArray> function)
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

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleMatrix>
	  {
		  private readonly VectorFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleArray> function;

		  public FuncAnonymousInnerClass(VectorFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleArray> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			DoubleArray y = function(x);
			int n = x.size();
			int m = y.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[m][n];
			double[][] res = RectangularArrays.ReturnRectangularDoubleArray(m, n);
			for (int j = 0; j < n; j++)
			{
			  double xj = x.get(j);
			  DoubleArray up = function(x.with(j, xj + outerInstance.eps));
			  for (int i = 0; i < m; i++)
			  {
				res[i][j] = (up.get(i) - y.get(i)) / outerInstance.eps;
			  }
			}
			return DoubleMatrix.copyOf(res);
		  }
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleMatrix>
	  {
		  private readonly VectorFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleArray> function;

		  public FuncAnonymousInnerClass2(VectorFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleArray> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			DoubleArray y = function(x); // need this unused evaluation to get size of y
			int n = x.size();
			int m = y.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[m][n];
			double[][] res = RectangularArrays.ReturnRectangularDoubleArray(m, n);
			for (int j = 0; j < n; j++)
			{
			  double xj = x.get(j);
			  DoubleArray up = function(x.with(j, xj + outerInstance.eps));
			  DoubleArray down = function(x.with(j, xj - outerInstance.eps));
			  for (int i = 0; i < m; i++)
			  {
				res[i][j] = (up.get(i) - down.get(i)) / outerInstance.twoEps;
			  }
			}
			return DoubleMatrix.copyOf(res);
		  }
	  }

	  private class FuncAnonymousInnerClass3 : System.Func<DoubleArray, DoubleMatrix>
	  {
		  private readonly VectorFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleArray> function;

		  public FuncAnonymousInnerClass3(VectorFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleArray> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			DoubleArray y = function(x);
			int n = x.size();
			int m = y.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[m][n];
			double[][] res = RectangularArrays.ReturnRectangularDoubleArray(m, n);
			for (int j = 0; j < n; j++)
			{
			  double xj = x.get(j);
			  DoubleArray down = function(x.with(j, xj - outerInstance.eps));
			  for (int i = 0; i < m; i++)
			  {
				res[i][j] = (y.get(i) - down.get(i)) / outerInstance.eps;
			  }
			}
			return DoubleMatrix.copyOf(res);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleMatrix> differentiate(System.Func<DoubleArray, DoubleArray> function, System.Func<DoubleArray, bool> domain)
	  {

		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(domain, "domain");
		double[] wFwd = new double[] {-3.0, 4.0, -1.0};
		double[] wCent = new double[] {-1.0, 0.0, 1.0};
		double[] wBack = new double[] {1.0, -4.0, 3.0};

		return new FuncAnonymousInnerClass4(this, function, domain, wFwd, wCent, wBack);
	  }

	  private class FuncAnonymousInnerClass4 : System.Func<DoubleArray, DoubleMatrix>
	  {
		  private readonly VectorFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleArray> function;
		  private System.Func<DoubleArray, bool> domain;
		  private double[] wFwd;
		  private double[] wCent;
		  private double[] wBack;

		  public FuncAnonymousInnerClass4(VectorFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleArray> function, System.Func<DoubleArray, bool> domain, double[] wFwd, double[] wCent, double[] wBack)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
			  this.domain = domain;
			  this.wFwd = wFwd;
			  this.wCent = wCent;
			  this.wBack = wBack;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			ArgChecker.isTrue(domain(x), "point {} is not in the function domain", x.ToString());

			DoubleArray mid = function(x); // need this unused evaluation to get size of y
			int n = x.size();
			int m = mid.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[m][n];
			double[][] res = RectangularArrays.ReturnRectangularDoubleArray(m, n);
			DoubleArray[] y = new DoubleArray[3];
			double[] w;

			for (int j = 0; j < n; j++)
			{
			  double xj = x.get(j);
			  DoubleArray xPlusOneEps = x.with(j, xj + outerInstance.eps);
			  DoubleArray xMinusOneEps = x.with(j, xj - outerInstance.eps);
			  if (!domain(xPlusOneEps))
			  {
				DoubleArray xMinusTwoEps = x.with(j, xj - outerInstance.twoEps);
				if (!domain(xMinusTwoEps))
				{
				  throw new MathException("cannot get derivative at point " + x.ToString() + " in direction " + j);
				}
				y[2] = mid;
				y[0] = function(xMinusTwoEps);
				y[1] = function(xMinusOneEps);
				w = wBack;
			  }
			  else
			  {
				if (!domain(xMinusOneEps))
				{
				  y[0] = mid;
				  y[1] = function(xPlusOneEps);
				  y[2] = function(x.with(j, xj + outerInstance.twoEps));
				  w = wFwd;
				}
				else
				{
				  y[2] = function(xPlusOneEps);
				  y[0] = function(xMinusOneEps);
				  y[1] = mid;
				  w = wCent;
				}
			  }

			  for (int i = 0; i < m; i++)
			  {
				double sum = 0;
				for (int k = 0; k < 3; k++)
				{
				  if (w[k] != 0.0)
				  {
					sum += w[k] * y[k].get(i);
				  }
				}
				res[i][j] = sum / outerInstance.twoEps;
			  }
			}
			return DoubleMatrix.copyOf(res);
		  }
	  }

	}

}
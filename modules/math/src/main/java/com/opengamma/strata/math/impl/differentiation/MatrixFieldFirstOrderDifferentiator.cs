/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;

	/// <summary>
	/// Matrix field first order differentiator.
	/// </summary>
	public class MatrixFieldFirstOrderDifferentiator : Differentiator<DoubleArray, DoubleMatrix, DoubleMatrix[]>
	{

	  private static readonly MatrixAlgebra MA = new OGMatrixAlgebra();
	  private const double DEFAULT_EPS = 1e-5;

	  private readonly double eps;
	  private readonly double twoEps;
	  private readonly double oneOverTwpEps;

	  /// <summary>
	  /// Creates an instance using the default value of eps (10<sup>-5</sup>).
	  /// </summary>
	  public MatrixFieldFirstOrderDifferentiator()
	  {
		eps = DEFAULT_EPS;
		twoEps = 2 * DEFAULT_EPS;
		oneOverTwpEps = 1.0 / twoEps;
	  }

	  /// <summary>
	  /// Creates an instance specifying the value of eps.
	  /// </summary>
	  /// <param name="eps">  the step size used to approximate the derivative </param>
	  public MatrixFieldFirstOrderDifferentiator(double eps)
	  {
		ArgChecker.isTrue(eps > 1e-15, "eps of {} is below machine tolerance of 1e-15. Please choose a higher value", eps);
		this.eps = eps;
		this.twoEps = 2 * eps;
		this.oneOverTwpEps = 1.0 / twoEps;
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleMatrix[]> differentiate(System.Func<DoubleArray, DoubleMatrix> function)
	  {

		ArgChecker.notNull(function, "function");
		return new FuncAnonymousInnerClass(this, function);
	  }

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleMatrix[]>
	  {
		  private readonly MatrixFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleMatrix> function;

		  public FuncAnonymousInnerClass(MatrixFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleMatrix> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix[] apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix[] apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			int n = x.size();

			DoubleMatrix[] res = new DoubleMatrix[n];
			for (int i = 0; i < n; i++)
			{
			  double xi = x.get(i);
			  DoubleMatrix up = function(x.with(i, xi + outerInstance.eps));
			  DoubleMatrix down = function(x.with(i, xi - outerInstance.eps));
			  res[i] = (DoubleMatrix) MA.scale(MA.subtract(up, down), outerInstance.oneOverTwpEps); //TODO have this in one operation
			}
			return res;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleMatrix[]> differentiate(System.Func<DoubleArray, DoubleMatrix> function, System.Func<DoubleArray, bool> domain)
	  {

		ArgChecker.notNull(function, "function");
		ArgChecker.notNull(domain, "domain");

		double[] wFwd = new double[] {-3.0 / twoEps, 4.0 / twoEps, -1.0 / twoEps};
		double[] wCent = new double[] {-1.0 / twoEps, 0.0, 1.0 / twoEps};
		double[] wBack = new double[] {1.0 / twoEps, -4.0 / twoEps, 3.0 / twoEps};

		return new FuncAnonymousInnerClass2(this, function, domain, wFwd, wCent, wBack);

	  }

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleMatrix[]>
	  {
		  private readonly MatrixFieldFirstOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleMatrix> function;
		  private System.Func<DoubleArray, bool> domain;
		  private double[] wFwd;
		  private double[] wCent;
		  private double[] wBack;

		  public FuncAnonymousInnerClass2(MatrixFieldFirstOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleMatrix> function, System.Func<DoubleArray, bool> domain, double[] wFwd, double[] wCent, double[] wBack)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
			  this.domain = domain;
			  this.wFwd = wFwd;
			  this.wCent = wCent;
			  this.wBack = wBack;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix[] apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix[] apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			ArgChecker.isTrue(domain(x), "point {} is not in the function domain", x.ToString());

			int n = x.size();
			DoubleMatrix[] y = new DoubleMatrix[3];
			DoubleMatrix[] res = new DoubleMatrix[n];
			double[] w;
			for (int i = 0; i < n; i++)
			{
			  double xi = x.get(i);
			  DoubleArray xPlusOneEps = x.with(i, xi + outerInstance.eps);
			  DoubleArray xMinusOneEps = x.with(i, xi - outerInstance.eps);
			  if (!domain(xPlusOneEps))
			  {
				DoubleArray xMinusTwoEps = x.with(i, xi - outerInstance.twoEps);
				if (!domain(xMinusTwoEps))
				{
				  throw new MathException("cannot get derivative at point " + x.ToString() + " in direction " + i);
				}
				y[0] = function(xMinusTwoEps);
				y[2] = function(x);
				y[1] = function(xMinusOneEps);
				w = wBack;
			  }
			  else
			  {
				if (!domain(xMinusOneEps))
				{
				  y[1] = function(xPlusOneEps);
				  y[0] = function(x);
				  y[2] = function(x.with(i, xi + outerInstance.twoEps));
				  w = wFwd;
				}
				else
				{
				  y[2] = function(xPlusOneEps);
				  y[0] = function(xMinusOneEps);
				  w = wCent;
				}
			  }
			  res[i] = (DoubleMatrix) MA.add(MA.scale(y[0], w[0]), MA.scale(y[2], w[2]));
			  if (w[1] != 0)
			  {
				res[i] = (DoubleMatrix) MA.add(res[i], MA.scale(y[1], w[1]));
			  }
			}
			return res;
		  }
	  }

	}

}
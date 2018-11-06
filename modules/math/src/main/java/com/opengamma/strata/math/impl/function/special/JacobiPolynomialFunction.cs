/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// 
	public class JacobiPolynomialFunction : OrthogonalPolynomialFunctionGenerator
	{

	  public override DoubleFunction1D[] getPolynomials(int n)
	  {
		throw new System.NotSupportedException("Need values for alpha and beta for Jacobi polynomial function generation");
	  }

	  public override Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n)
	  {
		throw new System.NotSupportedException("Need values for alpha and beta for Jacobi polynomial function generation");
	  }

	  /// <summary>
	  /// Calculates polynomials. </summary>
	  /// <param name="n">  the n value </param>
	  /// <param name="alpha">  the alpha value </param>
	  /// <param name="beta">  the beta value </param>
	  /// <returns> the result </returns>
	  public virtual DoubleFunction1D[] getPolynomials(int n, double alpha, double beta)
	  {
		ArgChecker.isTrue(n >= 0);
		DoubleFunction1D[] polynomials = new DoubleFunction1D[n + 1];
		for (int i = 0; i <= n; i++)
		{
		  if (i == 0)
		  {
			polynomials[i] = One;
		  }
		  else if (i == 1)
		  {
			polynomials[i] = new RealPolynomialFunction1D(new double[] {(alpha - beta) / 2, (alpha + beta + 2) / 2});
		  }
		  else
		  {
			int j = i - 1;
			polynomials[i] = (polynomials[j].multiply(getB(alpha, beta, j)).add(polynomials[j].multiply(X).multiply(getC(alpha, beta, j)).add(polynomials[j - 1].multiply(getD(alpha, beta, j))))).divide(getA(alpha, beta, j));
		  }
		}
		return polynomials;
	  }

	  /// <summary>
	  /// Calculates polynomials and derivative. </summary>
	  /// <param name="n">  the n value </param>
	  /// <param name="alpha">  the alpha value </param>
	  /// <param name="beta">  the beta value </param>
	  /// <returns> the result </returns>
	  public virtual Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n, double alpha, double beta)
	  {
		ArgChecker.isTrue(n >= 0);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.math.impl.function.DoubleFunction1D, com.opengamma.strata.math.impl.function.DoubleFunction1D>[] polynomials = new com.opengamma.strata.collect.tuple.Pair[n + 1];
		Pair<DoubleFunction1D, DoubleFunction1D>[] polynomials = new Pair[n + 1];
		DoubleFunction1D p, dp, p1, p2;
		for (int i = 0; i <= n; i++)
		{
		  if (i == 0)
		  {
			polynomials[i] = Pair.of(One, Zero);
		  }
		  else if (i == 1)
		  {
			double a1 = (alpha + beta + 2) / 2;
			polynomials[i] = Pair.of((DoubleFunction1D) new RealPolynomialFunction1D(new double[] {(alpha - beta) / 2, a1}), (DoubleFunction1D) new RealPolynomialFunction1D(new double[] {a1}));
		  }
		  else
		  {
			int j = i - 1;
			p1 = polynomials[j].First;
			p2 = polynomials[j - 1].First;
			DoubleFunction1D temp1 = p1.multiply(getB(alpha, beta, j));
			DoubleFunction1D temp2 = p1.multiply(X).multiply(getC(alpha, beta, j));
			DoubleFunction1D temp3 = p2.multiply(getD(alpha, beta, j));
			p = (temp1.add(temp2).add(temp3)).divide(getA(alpha, beta, j));
			dp = p.derivative();
			polynomials[i] = Pair.of(p, dp);
		  }
		}
		return polynomials;
	  }

	  private double getA(double alpha, double beta, int n)
	  {
		return 2 * (n + 1) * (n + alpha + beta + 1) * (2 * n + alpha + beta);

	  }

	  private double getB(double alpha, double beta, int n)
	  {
		return (2 * n + alpha + beta + 1) * (alpha * alpha - beta * beta);
	  }

	  private double getC(double alpha, double beta, int n)
	  {
		double x = 2 * n + alpha + beta;
		return x * (x + 1) * (x + 2);
	  }

	  private double getD(double alpha, double beta, int n)
	  {
		return -2 * (n + alpha) * (n + beta) * (2 * n + alpha + beta + 2);
	  }

	}

}
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
	public class LaguerrePolynomialFunction : OrthogonalPolynomialFunctionGenerator
	{

	  private static readonly DoubleFunction1D F1 = new RealPolynomialFunction1D(new double[] {1, -1});
	  private static readonly DoubleFunction1D DF1 = new RealPolynomialFunction1D(new double[] {-1});

	  public override DoubleFunction1D[] getPolynomials(int n)
	  {
		return getPolynomials(n, 0);
	  }

	  public override Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n)
	  {
		return getPolynomialsAndFirstDerivative(n, 0);
	  }

	  /// <summary>
	  /// Gets the polynomials.
	  /// </summary>
	  /// <param name="n">  the n value </param>
	  /// <param name="alpha">  the alpha value </param>
	  /// <returns> the result </returns>
	  public virtual DoubleFunction1D[] getPolynomials(int n, double alpha)
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
			polynomials[i] = new RealPolynomialFunction1D(new double[] {1 + alpha, -1});
		  }
		  else
		  {
			polynomials[i] = (polynomials[i - 1].multiply(2.0 * i + alpha - 1).subtract(polynomials[i - 1].multiply(X)).subtract(polynomials[i - 2].multiply((i - 1.0 + alpha))).divide(i));
		  }
		}
		return polynomials;
	  }

	  /// <summary>
	  /// Gets the polynomials and derivative.
	  /// </summary>
	  /// <param name="n">  the n value </param>
	  /// <param name="alpha">  the alpha value </param>
	  /// <returns> the result </returns>
	  public virtual Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n, double alpha)
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
			polynomials[i] = Pair.of(F1, DF1);
		  }
		  else
		  {
			p1 = polynomials[i - 1].First;
			p2 = polynomials[i - 2].First;
			p = (p1.multiply(2.0 * i + alpha - 1).subtract(p1.multiply(X)).subtract(p2.multiply((i - 1.0 + alpha))).divide(i));
			dp = (p.multiply(i).subtract(p1.multiply(i + alpha))).divide(X);
			polynomials[i] = Pair.of(p, dp);
		  }
		}
		return polynomials;
	  }

	}

}
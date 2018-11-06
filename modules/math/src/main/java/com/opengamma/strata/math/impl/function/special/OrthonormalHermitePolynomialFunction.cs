using System;

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
	public class OrthonormalHermitePolynomialFunction : OrthogonalPolynomialFunctionGenerator
	{

	  private static readonly double C1 = 1.0 / Math.Pow(Math.PI, 0.25);
	  private static readonly double C2 = Math.Sqrt(2) * C1;
	  private static readonly RealPolynomialFunction1D F0 = new RealPolynomialFunction1D(new double[] {C1});
	  private static readonly RealPolynomialFunction1D DF1 = new RealPolynomialFunction1D(new double[] {C2});

	  public override DoubleFunction1D[] getPolynomials(int n)
	  {
		ArgChecker.isTrue(n >= 0);
		DoubleFunction1D[] polynomials = new DoubleFunction1D[n + 1];
		for (int i = 0; i <= n; i++)
		{
		  if (i == 0)
		  {
			polynomials[i] = F0;
		  }
		  else if (i == 1)
		  {
			polynomials[i] = polynomials[0].multiply(Math.Sqrt(2)).multiply(X);
		  }
		  else
		  {
			polynomials[i] = polynomials[i - 1].multiply(X).multiply(Math.Sqrt(2.0 / i)).subtract(polynomials[i - 2].multiply(Math.Sqrt((i - 1.0) / i)));
		  }
		}
		return polynomials;
	  }

	  public override Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n)
	  {
		ArgChecker.isTrue(n >= 0);
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.math.impl.function.DoubleFunction1D, com.opengamma.strata.math.impl.function.DoubleFunction1D>[] polynomials = new com.opengamma.strata.collect.tuple.Pair[n + 1];
		Pair<DoubleFunction1D, DoubleFunction1D>[] polynomials = new Pair[n + 1];
		DoubleFunction1D p, dp, p1, p2;
		double sqrt2 = Math.Sqrt(2);
		DoubleFunction1D x = X;
		for (int i = 0; i <= n; i++)
		{
		  if (i == 0)
		  {
			polynomials[i] = Pair.of((DoubleFunction1D) F0, Zero);
		  }
		  else if (i == 1)
		  {
			polynomials[i] = Pair.of(polynomials[0].First.multiply(sqrt2).multiply(x), (DoubleFunction1D) DF1);
		  }
		  else
		  {
			p1 = polynomials[i - 1].First;
			p2 = polynomials[i - 2].First;
			p = p1.multiply(x).multiply(Math.Sqrt(2.0 / i)).subtract(p2.multiply(Math.Sqrt((i - 1.0) / i)));
			dp = p1.multiply(Math.Sqrt(2 * i));
			polynomials[i] = Pair.of(p, dp);
		  }
		}
		return polynomials;
	  }

	}

}
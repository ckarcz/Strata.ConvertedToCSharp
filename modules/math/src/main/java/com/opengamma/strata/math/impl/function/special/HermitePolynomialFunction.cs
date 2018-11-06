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
	public class HermitePolynomialFunction : OrthogonalPolynomialFunctionGenerator
	{

	  private static readonly DoubleFunction1D TWO_X = x => 2 * x;

	  public override DoubleFunction1D[] getPolynomials(int n)
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
			polynomials[i] = TWO_X;
		  }
		  else
		  {
			polynomials[i] = polynomials[i - 1].multiply(2).multiply(X).subtract(polynomials[i - 2].multiply(2 * i - 2));
		  }
		}
		return polynomials;
	  }

	  public override Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n)
	  {
		throw new System.NotSupportedException();
	  }

	}

}
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
	public class LegendrePolynomialFunction : OrthogonalPolynomialFunctionGenerator
	{

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
			polynomials[i] = X;
		  }
		  else
		  {
			polynomials[i] = (polynomials[i - 1].multiply(X).multiply(2 * i - 1).subtract(polynomials[i - 2].multiply(i - 1))).multiply(1.0 / i);
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
		DoubleFunction1D p, dp;
		for (int i = 0; i <= n; i++)
		{
		  if (i == 0)
		  {
			polynomials[i] = Pair.of(One, Zero);
		  }
		  else if (i == 1)
		  {
			polynomials[i] = Pair.of(X, One);
		  }
		  else
		  {
			p = (polynomials[i - 1].First.multiply(X).multiply(2 * i - 1).subtract(polynomials[i - 2].First.multiply(i - 1))).multiply(1.0 / i);
			dp = p.derivative();
			polynomials[i] = Pair.of(p, dp);
		  }
		}
		return polynomials;
	  }

	}

}
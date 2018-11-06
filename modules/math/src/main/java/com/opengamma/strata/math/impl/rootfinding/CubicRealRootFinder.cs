using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Root finder that calculates the roots of a cubic equation using <seealso cref="CubicRootFinder"/>
	/// and returns only the real roots. If there are no real roots, an exception is thrown.
	/// </summary>
	public class CubicRealRootFinder : Polynomial1DRootFinder<double>
	{

	  private static readonly double?[] EMPTY_ARRAY = new double?[0];
	  private static readonly Polynomial1DRootFinder<ComplexNumber> ROOT_FINDER = new CubicRootFinder();

	  public virtual double?[] getRoots(RealPolynomialFunction1D function)
	  {
		ArgChecker.notNull(function, "function");
		double[] coefficients = function.Coefficients;
		if (coefficients.Length != 4)
		{
		  throw new System.ArgumentException("Function is not a cubic");
		}
		ComplexNumber[] result = ROOT_FINDER.getRoots(function);
		IList<double> reals = new List<double>();
		foreach (ComplexNumber c in result)
		{
		  if (DoubleMath.fuzzyEquals(c.Imaginary, 0d, 1e-16))
		  {
			reals.Add(c.Real);
		  }
		}
		ArgChecker.isTrue(reals.Count > 0, "Could not find any real roots");
		return reals.toArray(EMPTY_ARRAY);
	  }

	}

}
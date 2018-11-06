using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using LaguerreSolver = org.apache.commons.math3.analysis.solvers.LaguerreSolver;
	using Complex = org.apache.commons.math3.complex.Complex;
	using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// Class that calculates the real roots of a polynomial using Laguerre's method. This class is a wrapper for the
	/// <a href="http://commons.apache.org/math/api-2.1/org/apache/commons/math/analysis/solvers/LaguerreSolver.html">Commons Math library implementation</a>
	/// of Laguerre's method.
	/// </summary>
	//TODO Have a complex and real root finder
	public class LaguerrePolynomialRealRootFinder : Polynomial1DRootFinder<double>
	{

	  private static readonly LaguerreSolver ROOT_FINDER = new LaguerreSolver();
	  private const double EPS = 1e-16;

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="MathException"> If there are no real roots; if the Commons method could not evaluate the function; if the Commons method could not converge. </exception>
	  public virtual double?[] getRoots(RealPolynomialFunction1D function)
	  {
		ArgChecker.notNull(function, "function");
		try
		{
		  Complex[] roots = ROOT_FINDER.solveAllComplex(function.Coefficients, 0);
		  IList<double> realRoots = new List<double>();
		  foreach (Complex c in roots)
		  {
			if (DoubleMath.fuzzyEquals(c.Imaginary, 0d, EPS))
			{
			  realRoots.Add(c.Real);
			}
		  }
		  if (realRoots.Count == 0)
		  {
			throw new MathException("Could not find any real roots");
		  }
		  return realRoots.ToArray();
		}
		catch (TooManyEvaluationsException e)
		{
		  throw new MathException(e);
		}
	  }

	}

}
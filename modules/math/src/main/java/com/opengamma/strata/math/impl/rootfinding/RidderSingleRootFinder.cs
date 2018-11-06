using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	using UnivariateFunction = org.apache.commons.math3.analysis.UnivariateFunction;
	using RiddersSolver = org.apache.commons.math3.analysis.solvers.RiddersSolver;
	using NoBracketingException = org.apache.commons.math3.exception.NoBracketingException;
	using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;

	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;

	/// <summary>
	/// Finds a single root of a function using Ridder's method. This class is a wrapper for the
	/// <a href="http://commons.apache.org/proper/commons-math/javadocs/api-3.5/org/apache/commons/math3/analysis/solvers/RiddersSolver.html">Commons Math library implementation</a>
	/// of Ridder's method.
	/// </summary>
	public class RidderSingleRootFinder : RealSingleRootFinder
	{

	  private const int MAX_ITER = 100000;
	  private readonly RiddersSolver _ridder;

	  /// <summary>
	  /// Sets the accuracy to 10<sup>-15</sup>.
	  /// </summary>
	  public RidderSingleRootFinder() : this(1e-15)
	  {
	  }

	  /// <param name="functionValueAccuracy"> The accuracy of the function evaluations. </param>
	  public RidderSingleRootFinder(double functionValueAccuracy)
	  {
		_ridder = new RiddersSolver(functionValueAccuracy);
	  }

	  /// <param name="functionValueAccuracy"> The accuracy of the function evaluations. </param>
	  /// <param name="absoluteAccurary"> The maximum absolute error of the variable. </param>
	  public RidderSingleRootFinder(double functionValueAccuracy, double absoluteAccurary)
	  {
		_ridder = new RiddersSolver(functionValueAccuracy, absoluteAccurary);
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="MathException"> If the Commons method could not evaluate the function;
	  ///   if the Commons method could not converge. </exception>
	  public override double? getRoot(System.Func<double, double> function, double? xLow, double? xHigh)
	  {
		checkInputs(function, xLow, xHigh);
		UnivariateFunction wrapped = CommonsMathWrapper.wrapUnivariate(function);
		try
		{
		  return _ridder.solve(MAX_ITER, wrapped, xLow, xHigh);
		}
		catch (Exception e) when (e is TooManyEvaluationsException || e is NoBracketingException)
		{
		  throw new MathException(e);
		}
	  }

	}

}
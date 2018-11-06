/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{
	using LUDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// A root finder that attempts find the multi-dimensional root of a series of N equations with N variables (a square problem).
	/// If the analytic Jacobian is not known, it will be calculated using central difference 
	/// </summary>
	public class NewtonDefaultVectorRootFinder : BaseNewtonVectorRootFinder
	{

	  /// <summary>
	  /// The default tolerance.
	  /// </summary>
	  private const double DEF_TOL = 1e-7;
	  /// <summary>
	  /// The default maximum number of steps.
	  /// </summary>
	  private const int MAX_STEPS = 100;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  public NewtonDefaultVectorRootFinder() : this(DEF_TOL, DEF_TOL, MAX_STEPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  public NewtonDefaultVectorRootFinder(double absoluteTol, double relativeTol, int maxSteps) : this(absoluteTol, relativeTol, maxSteps, new LUDecompositionCommons())
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  /// <param name="decomp">  the decomposition </param>
	  public NewtonDefaultVectorRootFinder<T1>(double absoluteTol, double relativeTol, int maxSteps, Decomposition<T1> decomp) : base(absoluteTol, relativeTol, maxSteps, new JacobianDirectionFunction(decomp), new JacobianEstimateInitializationFunction(), new NewtonDefaultUpdateFunction())
	  {
	  }

	}

}
/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{
	using LUDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// A root finder that uses the Sherman-Morrison formula to invert Broyden's Jacobian update formula,
	/// thus providing a direct update formula for the inverse Jacobian.
	/// </summary>
	public class ShermanMorrisonVectorRootFinder : BaseNewtonVectorRootFinder
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
	  public ShermanMorrisonVectorRootFinder() : this(DEF_TOL, DEF_TOL, MAX_STEPS)
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  public ShermanMorrisonVectorRootFinder(double absoluteTol, double relativeTol, int maxSteps) : this(absoluteTol, relativeTol, maxSteps, new LUDecompositionCommons())
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  /// <param name="decomp">  the decomposition </param>
	  public ShermanMorrisonVectorRootFinder<T1>(double absoluteTol, double relativeTol, int maxSteps, Decomposition<T1> decomp) : this(absoluteTol, relativeTol, maxSteps, decomp, new OGMatrixAlgebra())
	  {
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  /// <param name="decomp">  the decomposition </param>
	  /// <param name="algebra">  the instance of matrix algebra </param>
	  public ShermanMorrisonVectorRootFinder<T1>(double absoluteTol, double relativeTol, int maxSteps, Decomposition<T1> decomp, MatrixAlgebra algebra) : base(absoluteTol, relativeTol, maxSteps, new InverseJacobianDirectionFunction(algebra), new InverseJacobianEstimateInitializationFunction(decomp), new ShermanMorrisonMatrixUpdateFunction(algebra))
	  {

	  }

	}

}
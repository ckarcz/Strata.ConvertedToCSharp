/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.rootfind
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using SVDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.SVDecompositionCommons;
	using BroydenVectorRootFinder = com.opengamma.strata.math.impl.rootfinding.newton.BroydenVectorRootFinder;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Performs Newton-Raphson style multi-dimensional root finding.
	/// <para>
	/// This uses the Jacobian matrix as a basis for some parts of the iterative process.
	/// </para>
	/// </summary>
	public interface NewtonVectorRootFinder
	{

	  /// <summary>
	  /// Obtains an instance of the Broyden root finder.
	  /// <para>
	  /// This uses SV decomposition and standard tolerances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the root finder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static NewtonVectorRootFinder broyden()
	//  {
	//	return new BroydenVectorRootFinder(new SVDecompositionCommons());
	//  }

	  /// <summary>
	  /// Obtains an instance of the Broyden root finder specifying the tolerances.
	  /// <para>
	  /// This uses SV decomposition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  /// <returns> the root finder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static NewtonVectorRootFinder broyden(double absoluteTol, double relativeTol, int maxSteps)
	//  {
	//	return new BroydenVectorRootFinder(absoluteTol, relativeTol, maxSteps, new SVDecompositionCommons());
	//  }

	  /// <summary>
	  /// Obtains an instance of the Broyden root finder specifying the tolerances.
	  /// </summary>
	  /// <param name="absoluteTol">  the absolute tolerance </param>
	  /// <param name="relativeTol">  the relative tolerance </param>
	  /// <param name="maxSteps">  the maximum steps </param>
	  /// <param name="decomposition">  the decomposition function </param>
	  /// <returns> the root finder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static NewtonVectorRootFinder broyden(double absoluteTol, double relativeTol, int maxSteps, com.opengamma.strata.math.linearalgebra.Decomposition<JavaToDotNetGenericWildcard> decomposition)
	//  {
	//
	//	return new BroydenVectorRootFinder(absoluteTol, relativeTol, maxSteps, decomposition);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the root from the specified start position.
	  /// <para>
	  /// This applies the specified function to find the root.
	  /// Note if multiple roots exist which one is found will depend on the start position.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="function">   the vector function </param>
	  /// <param name="startPosition">  the start position of the root finder for </param>
	  /// <returns> the vector root of the collection of functions </returns>
	  /// <exception cref="MathException"> if unable to find the root, such as if unable to converge </exception>
	  DoubleArray findRoot(System.Func<DoubleArray, DoubleArray> function, DoubleArray startPosition);

	  /// <summary>
	  /// Finds the root from the specified start position.
	  /// <para>
	  /// This applies the specified function and Jacobian function to find the root.
	  /// Note if multiple roots exist which one is found will depend on the start position.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="function">   the vector function </param>
	  /// <param name="jacobianFunction">  the function to calculate the Jacobian </param>
	  /// <param name="startPosition">  the start position of the root finder for </param>
	  /// <returns> the vector root of the collection of functions </returns>
	  /// <exception cref="MathException"> if unable to find the root, such as if unable to converge </exception>
	  DoubleArray findRoot(System.Func<DoubleArray, DoubleArray> function, System.Func<DoubleArray, DoubleMatrix> jacobianFunction, DoubleArray startPosition);

	}

}
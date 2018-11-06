/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// 
	public class InverseJacobianEstimateInitializationFunction : NewtonRootFinderMatrixInitializationFunction
	{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.math.linearalgebra.Decomposition<?> _decomposition;
	  private readonly Decomposition<object> _decomposition;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="decomposition">  the decomposition </param>
	  public InverseJacobianEstimateInitializationFunction<T1>(Decomposition<T1> decomposition)
	  {
		ArgChecker.notNull(decomposition, "decomposition");
		_decomposition = decomposition;
	  }

	  public virtual DoubleMatrix getInitializedMatrix(System.Func<DoubleArray, DoubleMatrix> jacobianFunction, DoubleArray x)
	  {
		ArgChecker.notNull(jacobianFunction, "jacobianFunction");
		ArgChecker.notNull(x, "x");
		DoubleMatrix estimate = jacobianFunction(x);
		DecompositionResult decompositionResult = _decomposition.apply(estimate);
		return decompositionResult.solve(DoubleMatrix.identity(x.size()));
	  }

	}

}
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
	public class JacobianDirectionFunction : NewtonRootFinderDirectionFunction
	{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final com.opengamma.strata.math.linearalgebra.Decomposition<?> _decomposition;
	  private readonly Decomposition<object> _decomposition;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="decomposition">  the decomposition </param>
	  public JacobianDirectionFunction<T1>(Decomposition<T1> decomposition)
	  {
		ArgChecker.notNull(decomposition, "decomposition");
		_decomposition = decomposition;
	  }

	  public virtual DoubleArray getDirection(DoubleMatrix estimate, DoubleArray y)
	  {
		ArgChecker.notNull(estimate, "estimate");
		ArgChecker.notNull(y, "y");
		DecompositionResult result = _decomposition.apply(estimate);
		return result.solve(y);
	  }

	}

}
using System;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using CholeskyDecomposition = org.apache.commons.math3.linear.CholeskyDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// This class is a wrapper for the <a href="http://commons.apache.org/math/api-2.1/org/apache/commons/math/linear/CholeskyDecompositionImpl.html">Commons Math library implementation</a> 
	/// of Cholesky decomposition.
	/// </summary>
	public class CholeskyDecompositionCommons : Decomposition<CholeskyDecompositionResult>
	{

	  /// <summary>
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual CholeskyDecompositionResult apply(DoubleMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		RealMatrix temp = CommonsMathWrapper.wrap(x);
		CholeskyDecomposition cholesky;
		try
		{
		  cholesky = new CholeskyDecomposition(temp);
		}
		catch (Exception e)
		{
		  throw new MathException(e.ToString());
		}
		return new CholeskyDecompositionCommonsResult(cholesky);
	  }

	}

}
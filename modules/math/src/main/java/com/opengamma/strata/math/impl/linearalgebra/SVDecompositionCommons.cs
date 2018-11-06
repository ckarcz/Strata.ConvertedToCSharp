/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;
	using SingularValueDecomposition = org.apache.commons.math3.linear.SingularValueDecomposition;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// This class is a wrapper for the <a href="http://commons.apache.org/math/api-2.1/org/apache/commons/math/linear/SingularValueDecompositionImpl.html">Commons Math library implementation</a>
	/// of singular value decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class SVDecompositionCommons : Decomposition<SVDecompositionResult>
	{

	  public virtual SVDecompositionResult apply(DoubleMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		MatrixValidate.notNaNOrInfinite(x);
		RealMatrix commonsMatrix = CommonsMathWrapper.wrap(x);
		SingularValueDecomposition svd = new SingularValueDecomposition(commonsMatrix);
		return new SVDecompositionCommonsResult(svd);
	  }

	}

}
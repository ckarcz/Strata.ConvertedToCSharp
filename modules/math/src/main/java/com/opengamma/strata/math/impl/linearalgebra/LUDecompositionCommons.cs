/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using LUDecomposition = org.apache.commons.math3.linear.LUDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// This class is a wrapper for the
	/// <a href="http://commons.apache.org/proper/commons-math/javadocs/api-3.5/org/apache/commons/math3/linear/LUDecomposition.html">Commons Math3 library implementation</a> 
	/// of LU decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class LUDecompositionCommons : Decomposition<LUDecompositionResult>
	{

	  public virtual LUDecompositionResult apply(DoubleMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		RealMatrix temp = CommonsMathWrapper.wrap(x);
		LUDecomposition lu = new LUDecomposition(temp);
		return new LUDecompositionCommonsResult(lu);
	  }

	}

}
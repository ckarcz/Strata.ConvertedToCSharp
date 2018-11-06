/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using QRDecomposition = org.apache.commons.math3.linear.QRDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CommonsMathWrapper = com.opengamma.strata.math.impl.util.CommonsMathWrapper;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// This class is a wrapper for the <a href="http://commons.apache.org/math/api-2.1/org/apache/commons/math/linear/QRDecompositionImpl.html">Commons Math library implementation</a> 
	/// of QR decomposition.
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	public class QRDecompositionCommons : Decomposition<QRDecompositionResult>
	{

	  public virtual QRDecompositionResult apply(DoubleMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		RealMatrix temp = CommonsMathWrapper.wrap(x);
		QRDecomposition qr = new QRDecomposition(temp);
		return new QRDecompositionCommonsResult(qr);
	  }

	}

}
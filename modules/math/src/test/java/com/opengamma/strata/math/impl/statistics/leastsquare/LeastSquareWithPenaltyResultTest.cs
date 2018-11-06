/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LeastSquareWithPenaltyResultTest
	public class LeastSquareWithPenaltyResultTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
		public virtual void test()
		{

		double chi2 = 13.234324;
		double pen = 2.3445;
		int nParms = 12;
		DoubleArray parms = DoubleArray.filled(nParms, 0.5);
		DoubleMatrix cov = DoubleMatrix.filled(nParms, nParms);

		LeastSquareWithPenaltyResults res = new LeastSquareWithPenaltyResults(chi2, pen, parms, cov);
		assertEquals(chi2, res.ChiSq);
		assertEquals(pen, res.Penalty);

		DoubleMatrix invJac = DoubleMatrix.filled(nParms, 5);
		res = new LeastSquareWithPenaltyResults(chi2, pen, parms, cov, invJac);
		assertEquals(chi2, res.ChiSq);
		assertEquals(pen, res.Penalty);
		}

	}

}
/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LeastSquareResultsTest
	public class LeastSquareResultsTest
	{
	  private static readonly DoubleArray PARAMS = DoubleArray.of(1.0, 2.0);
	  private static readonly DoubleMatrix COVAR = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {0.1, 0.2},
		  new double[] {0.2, 0.3}
	  });
	  private static readonly DoubleMatrix INV_JAC = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {0.5, 0.6},
		  new double[] {0.7, 0.8}
	  });

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeChiSq1()
	  public virtual void testNegativeChiSq1()
	  {
		new LeastSquareResults(-1, PARAMS, COVAR);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullParams1()
	  public virtual void testNullParams1()
	  {
		new LeastSquareResults(1, null, COVAR);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullCovar1()
	  public virtual void testNullCovar1()
	  {
		new LeastSquareResults(1, PARAMS, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullWrongSize1()
	  public virtual void testNullWrongSize1()
	  {
		new LeastSquareResults(1, DoubleArray.of(1.2), COVAR);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNotSquare1()
	  public virtual void testNotSquare1()
	  {
		new LeastSquareResults(1, PARAMS, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.2, 0.3}
		}));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeChiSq2()
	  public virtual void testNegativeChiSq2()
	  {
		new LeastSquareResults(-1, PARAMS, COVAR, INV_JAC);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullParams2()
	  public virtual void testNullParams2()
	  {
		new LeastSquareResults(1, null, COVAR, INV_JAC);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullCovar2()
	  public virtual void testNullCovar2()
	  {
		new LeastSquareResults(1, PARAMS, null, INV_JAC);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullWrongSize2()
	  public virtual void testNullWrongSize2()
	  {
		new LeastSquareResults(1, DoubleArray.of(1.2), COVAR, INV_JAC);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNotSquare2()
	  public virtual void testNotSquare2()
	  {
		new LeastSquareResults(1, PARAMS, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.2, 0.3}
		}), INV_JAC);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testRecall()
	  public virtual void testRecall()
	  {
		const double chiSq = 12.46;
		LeastSquareResults res = new LeastSquareResults(chiSq, PARAMS, COVAR);
		assertEquals(chiSq, res.ChiSq, 0.0);
		for (int i = 0; i < 2; i++)
		{
		  assertEquals(PARAMS.get(i), res.FitParameters.get(i), 0);
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(COVAR.get(i, j), res.Covariance.get(i, j), 0);
		  }
		}
		res = new LeastSquareResults(chiSq, PARAMS, COVAR, INV_JAC);
		assertEquals(chiSq, res.ChiSq, 0.0);
		for (int i = 0; i < 2; i++)
		{
		  assertEquals(PARAMS.get(i), res.FitParameters.get(i), 0);
		  for (int j = 0; j < 2; j++)
		  {
			assertEquals(COVAR.get(i, j), res.Covariance.get(i, j), 0);
			assertEquals(INV_JAC.get(i, j), res.FittingParameterSensitivityToData.get(i, j), 0);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHashCode()
	  public virtual void testHashCode()
	  {
		LeastSquareResults ls1 = new LeastSquareResults(1.0, PARAMS, COVAR);
		LeastSquareResults ls2 = new LeastSquareResults(1.0, DoubleArray.of(1.0, 2.0), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.1, 0.2},
			new double[] {0.2, 0.3}
		}));
		assertEquals(ls1.GetHashCode(), ls2.GetHashCode(), 0);
		ls2 = new LeastSquareResults(1.0, DoubleArray.of(1.0, 2.0), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.1, 0.2},
			new double[] {0.2, 0.3}
		}), null);
		assertEquals(ls1.GetHashCode(), ls2.GetHashCode(), 0);
		ls1 = new LeastSquareResults(1.0, PARAMS, COVAR, INV_JAC);
		ls2 = new LeastSquareResults(1.0, DoubleArray.of(1.0, 2.0), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.1, 0.2},
			new double[] {0.2, 0.3}
		}), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.5, 0.6},
			new double[] {0.7, 0.8}
		}));
		assertEquals(ls1.GetHashCode(), ls2.GetHashCode(), 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testEquals()
	  public virtual void testEquals()
	  {
		LeastSquareResults ls1 = new LeastSquareResults(1.0, PARAMS, COVAR);
		LeastSquareResults ls2 = new LeastSquareResults(1.0, DoubleArray.of(1.0, 2.0), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.1, 0.2},
			new double[] {0.2, 0.3}
		}));
		assertEquals(ls1, ls2);
		ls2 = new LeastSquareResults(1.0, PARAMS, COVAR, null);
		assertEquals(ls1, ls2);
		ls2 = new LeastSquareResults(1.1, PARAMS, COVAR);
		assertFalse(ls1.Equals(ls2));
		ls2 = new LeastSquareResults(1.0, DoubleArray.of(1.1, 2.0), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.1, 0.2},
			new double[] {0.2, 0.3}
		}));
		assertFalse(ls1.Equals(ls2));
		ls2 = new LeastSquareResults(1.0, DoubleArray.of(1.0, 2.0), DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.1, 0.2},
			new double[] {0.2, 0.4}
		}));
		assertFalse(ls1.Equals(ls2));
		ls2 = new LeastSquareResults(1.0, PARAMS, COVAR, INV_JAC);
		assertFalse(ls1.Equals(ls2));
		ls1 = new LeastSquareResults(1, PARAMS, COVAR, INV_JAC);
		ls2 = new LeastSquareResults(1, PARAMS, COVAR, COVAR);
		assertFalse(ls1.Equals(ls2));
	  }

	}

}
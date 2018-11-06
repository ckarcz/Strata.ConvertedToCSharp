/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{
	using Assert = org.testng.Assert;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LeastSquaresRegressionTest
	public class LeastSquaresRegressionTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
		public virtual void test()
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LeastSquaresRegression regression = new OrdinaryLeastSquaresRegression();
		LeastSquaresRegression regression = new OrdinaryLeastSquaresRegression();
		try
		{
		  regression.checkData(null, null);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		double[][] x = new double[0][];
		try
		{
		  regression.checkData(x, null);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		double[] y = new double[0];
		try
		{
		  regression.checkData(x, (double[]) null, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: x = new double[1][2];
		x = RectangularArrays.ReturnRectangularDoubleArray(1, 2);
		y = new double[3];
		try
		{
		  regression.checkData(x, (double[]) null, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		x = new double[][]
		{
			new double[] {1.0, 2.0, 3.0},
			new double[] {4.0, 5.0},
			new double[] {6.0, 7.0, 8.0},
			new double[] {9.0, 0.0, 0.0}
		};
		try
		{
		  regression.checkData(x, (double[]) null, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		x[1] = new double[] {4.0, 5.0, 6.0};
		try
		{
		  regression.checkData(x, (double[]) null, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		y = new double[] {1.0, 2.0, 3.0, 4.0};
		double[] w1 = new double[0];
		try
		{
		  regression.checkData(x, w1, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		double[][] w = new double[0][];
		try
		{
		  regression.checkData(x, w, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		w1 = new double[3];
		try
		{
		  regression.checkData(x, w1, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: w = new double[3][0];
		w = RectangularArrays.ReturnRectangularDoubleArray(3, 0);
		try
		{
		  regression.checkData(x, w, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		w = new double[][]
		{
			new double[] {1.0, 2.0, 3.0},
			new double[] {4.0, 5.0},
			new double[] {6.0, 7.0, 8.0},
			new double[] {9.0, 0.0, 0.0}
		};
		try
		{
		  regression.checkData(x, w, y);
		  Assert.fail();
		}
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final IllegalArgumentException e)
		catch (legalArgumentException)
		{
		  // Expected
		}
		}
	}

}
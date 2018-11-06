using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using DoubleFunction1D = com.opengamma.strata.math.impl.function.DoubleFunction1D;
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;
	using LeastSquaresRegressionResult = com.opengamma.strata.math.impl.regression.LeastSquaresRegressionResult;
	using MeanCalculator = com.opengamma.strata.math.impl.statistics.descriptive.MeanCalculator;
	using SampleStandardDeviationCalculator = com.opengamma.strata.math.impl.statistics.descriptive.SampleStandardDeviationCalculator;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PolynomialsLeastSquaresFitterTest
	public class PolynomialsLeastSquaresFitterTest
	{
	  private const double EPS = 1e-14;

	  private readonly System.Func<double[], double> _meanCal = new MeanCalculator();
	  private readonly System.Func<double[], double> _stdCal = new SampleStandardDeviationCalculator();

	  /// <summary>
	  /// Checks coefficients of polynomial f(x) are recovered and residuals, { y_i -f(x_i) }, are accurate
	  /// </summary>
	  public virtual void PolynomialFunctionRecoverTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] coeff = new double[] {3.4, 5.6, 1.0, -4.0 };
		double[] coeff = new double[] {3.4, 5.6, 1.0, -4.0};

		DoubleFunction1D func = new RealPolynomialFunction1D(coeff);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int degree = coeff.length - 1;
		int degree = coeff.Length - 1;

		const int nPts = 7;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = -5.0 + 10 * i / (nPts - 1);
		  yValues[i] = func.applyAsDouble(xValues[i]);
		}

		double[] yValuesNorm = new double[nPts];

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double mean = _meanCal.apply(xValues);
		double mean = _meanCal.apply(xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double std = _stdCal.apply(xValues);
		double std = _stdCal.apply(xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double ratio = mean / std;
		double ratio = mean / std;

		for (int i = 0; i < nPts; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp = xValues[i] / std - ratio;
		  double tmp = xValues[i] / std - ratio;
		  yValuesNorm[i] = func.applyAsDouble(tmp);
		}

		/// <summary>
		/// Tests for regress(..)
		/// </summary>

		LeastSquaresRegressionResult result = regObj.regress(xValues, yValues, degree);

		double[] coeffResult = result.Betas;

		for (int i = 0; i < degree + 1; ++i)
		{
		  assertEquals(coeff[i], coeffResult[i], EPS * Math.Abs(coeff[i]));
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] residuals = result.getResiduals();
		double[] residuals = result.Residuals;
		func = new RealPolynomialFunction1D(coeffResult);
		double[] yValuesFit = new double[nPts];
		for (int i = 0; i < nPts; ++i)
		{
		  yValuesFit[i] = func.applyAsDouble(xValues[i]);
		}

		for (int i = 0; i < nPts; ++i)
		{
		  assertEquals(Math.Abs(yValuesFit[i] - yValues[i]), 0.0, Math.Abs(yValues[i]) * EPS);
		}

		for (int i = 0; i < nPts; ++i)
		{
		  assertEquals(Math.Abs(yValuesFit[i] - yValues[i]), Math.Abs(residuals[i]), Math.Abs(yValues[i]) * EPS);
		}

		double sum = 0.0;
		for (int i = 0; i < nPts; ++i)
		{
		  sum += residuals[i] * residuals[i];
		}
		sum = Math.Sqrt(sum);

		/// <summary>
		/// Tests for regressVerbose(.., false)
		/// </summary>

		PolynomialsLeastSquaresFitterResult resultVer = regObj.regressVerbose(xValues, yValues, degree, false);
		coeffResult = resultVer.Coeff;
		func = new RealPolynomialFunction1D(coeffResult);
		for (int i = 0; i < nPts; ++i)
		{
		  yValuesFit[i] = func.applyAsDouble(xValues[i]);
		}

		assertEquals(nPts - (degree + 1), resultVer.Dof, 0);
		for (int i = 0; i < degree + 1; ++i)
		{
		  assertEquals(coeff[i], coeffResult[i], EPS * Math.Abs(coeff[i]));
		}

		for (int i = 0; i < nPts; ++i)
		{
		  assertEquals(Math.Abs(yValuesFit[i] - yValues[i]), 0.0, Math.Abs(yValues[i]) * EPS);
		}

		assertEquals(sum, resultVer.DiffNorm, EPS);

		/// <summary>
		/// Tests for regressVerbose(.., true)
		/// </summary>

		PolynomialsLeastSquaresFitterResult resultNorm = regObj.regressVerbose(xValues, yValuesNorm, degree, true);

		coeffResult = resultNorm.Coeff;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] meanAndStd = resultNorm.getMeanAndStd();
		double[] meanAndStd = resultNorm.MeanAndStd;

		assertEquals(nPts - (degree + 1), resultNorm.Dof, 0);
		assertEquals(mean, meanAndStd[0], EPS);
		assertEquals(std, meanAndStd[1], EPS);
		for (int i = 0; i < degree + 1; ++i)
		{
		  assertEquals(coeff[i], coeffResult[i], EPS * Math.Abs(coeff[i]));
		}

		func = new RealPolynomialFunction1D(coeffResult);
		for (int i = 0; i < nPts; ++i)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double tmp = xValues[i] / std - ratio;
		  double tmp = xValues[i] / std - ratio;
		  yValuesFit[i] = func.applyAsDouble(tmp);
		}

		for (int i = 0; i < nPts; ++i)
		{
		  assertEquals(Math.Abs(yValuesFit[i] - yValuesNorm[i]), 0.0, Math.Abs(yValuesNorm[i]) * EPS);
		}

		sum = 0.0;
		for (int i = 0; i < nPts; ++i)
		{
		  sum += (yValuesFit[i] - yValuesNorm[i]) * (yValuesFit[i] - yValuesNorm[i]);
		}
		sum = Math.Sqrt(sum);

		assertEquals(sum, resultNorm.DiffNorm, EPS);

	  }

	  /// 
	  public virtual void RmatrixTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj1 = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj1 = new PolynomialsLeastSquaresFitter();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {-1.0, 0, 1.0 };
		double[] xValues = new double[] {-1.0, 0, 1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1.0, 0, 1.0 };
		double[] yValues = new double[] {1.0, 0, 1.0};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] rMatrix = new double[][] { {-Math.sqrt(3.0), 0.0, -2.0 / Math.sqrt(3.0)}, {0.0, -Math.sqrt(2.0), 0.0}, {0.0, 0.0, -Math.sqrt(2.0 / 3.0)} };
		double[][] rMatrix = new double[][]
		{
			new double[] {-Math.Sqrt(3.0), 0.0, -2.0 / Math.Sqrt(3.0)},
			new double[] {0.0, -Math.Sqrt(2.0), 0.0},
			new double[] {0.0, 0.0, -Math.Sqrt(2.0 / 3.0)}
		};

		const int degree = 2;

		PolynomialsLeastSquaresFitterResult resultVer = regObj1.regressVerbose(xValues, yValues, degree, false);
		DoubleMatrix rMatResult = resultVer.RMat;

		for (int i = 0; i < 3; ++i)
		{
		  for (int j = 0; j < 3; ++j)
		  {
			assertEquals(rMatrix[i][j], rMatResult.get(i, j), EPS);
		  }
		}

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj2 = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj2 = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitterResult resultNorm = regObj2.regressVerbose(xValues, yValues, degree, true);
		rMatResult = resultNorm.RMat;

		for (int i = 0; i < 3; ++i)
		{
		  for (int j = 0; j < 3; ++j)
		  {
			assertEquals(rMatrix[i][j], rMatResult.get(i, j), EPS);
		  }
		}

	  }

	  /// <summary>
	  /// An error is thrown if rescaling of xValues is NOT used and we try to access data, mean and standard deviation 
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NormalisationErrorTest()
	  public virtual void NormalisationErrorTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		PolynomialsLeastSquaresFitterResult result = regObj.regressVerbose(xValues, yValues, degree, false);
		result.MeanAndStd;

	  }

	  /// <summary>
	  /// Number of data points should be larger than (degree + 1) of a polynomial
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortTest()
	  public virtual void DataShortTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 6;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortVerboseFalseTest()
	  public virtual void DataShortVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 6;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void DataShortVerboseTrueTest()
	  public virtual void DataShortVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 6;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// <summary>
	  /// Degree of polynomial must be positive 
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void MinusDegreeTest()
	  public virtual void MinusDegreeTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = -4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void MinusDegreeVerboseFalseTest()
	  public virtual void MinusDegreeVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = -4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void MinusDegreeVerboseTrueTest()
	  public virtual void MinusDegreeVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = -4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1};

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// <summary>
	  /// xValues length should be the same as yValues length
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void WrongDataLengthTest()
	  public virtual void WrongDataLengthTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1, 2 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1, 2};

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void WrongDataLengthVerboseFalseTest()
	  public virtual void WrongDataLengthVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1, 2 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1, 2};

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void WrongDataLengthVerboseTureTest()
	  public virtual void WrongDataLengthVerboseTureTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 5, 6 };
		double[] xValues = new double[] {0, 1, 2, 3, 5, 6};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 1, 2 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 1, 2};

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// <summary>
	  /// An error is thrown if too many repeated data are found
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void RepeatDataTest()
	  public virtual void RepeatDataTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 1, 1 };
		double[] xValues = new double[] {0, 1, 2, 3, 1, 1};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 2 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 2};

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void RepeatDataVerboseFalseTest()
	  public virtual void RepeatDataVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 1, 1 };
		double[] xValues = new double[] {0, 1, 2, 3, 1, 1};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 2 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 2};

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void RepeatDataVerboseTrueTest()
	  public virtual void RepeatDataVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1, 2, 3, 1, 1 };
		double[] xValues = new double[] {0, 1, 2, 3, 1, 1};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 2, 2 };
		double[] yValues = new double[] {1, 2, 3, 4, 2, 2};

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void ExtremeValueTest()
	  public virtual void ExtremeValueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1e-307, 2e-307, 3e18, 4 };
		double[] xValues = new double[] {0, 1e-307, 2e-307, 3e18, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 5 };
		double[] yValues = new double[] {1, 2, 3, 4, 5};

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void ExtremeValueVerboseFalseTest()
	  public virtual void ExtremeValueVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1e-307, 2e-307, 3e18, 4 };
		double[] xValues = new double[] {0, 1e-307, 2e-307, 3e18, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 5 };
		double[] yValues = new double[] {1, 2, 3, 4, 5};

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void ExtremeValueVerboseTrueAlphaTest()
	  public virtual void ExtremeValueVerboseTrueAlphaTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] xValues = new double[] {0, 1e-307, 2e-307, 3e-307, 4 };
		double[] xValues = new double[] {0, 1e-307, 2e-307, 3e-307, 4};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] yValues = new double[] {1, 2, 3, 4, 5 };
		double[] yValues = new double[] {1, 2, 3, 4, 5};

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullTest()
	  public virtual void NullTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		xValues = null;
		yValues = null;

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullVerboseFalseTest()
	  public virtual void NullVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		xValues = null;
		yValues = null;

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NullVerboseTrueTest()
	  public virtual void NullVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		xValues = null;
		yValues = null;

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityTest()
	  public virtual void InfinityTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = 1.0 / zero;
		  yValues[i] = 1.0 / zero;
		}

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityVerboseFalseTest()
	  public virtual void InfinityVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = 1.0 / zero;
		  yValues[i] = 1.0 / zero;
		}

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void InfinityVerboseTrueTest()
	  public virtual void InfinityVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		const double zero = 0.0;

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = 1.0 / zero;
		  yValues[i] = 1.0 / zero;
		}

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNTest()
	  public virtual void NaNTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = Double.NaN;
		  yValues[i] = Double.NaN;
		}

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNVerboseFalseTest()
	  public virtual void NaNVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = Double.NaN;
		  yValues[i] = Double.NaN;
		}

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void NaNVerboseTrueTest()
	  public virtual void NaNVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		const int nPts = 5;
		double[] xValues = new double[nPts];
		double[] yValues = new double[nPts];

		for (int i = 0; i < nPts; ++i)
		{
		  xValues[i] = Double.NaN;
		  yValues[i] = Double.NaN;
		}

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeNumberTest()
	  public virtual void LargeNumberTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		double[] xValues = new double[] {1, 2, 3, 4e2, 5, 6, 7};
		double[] yValues = new double[] {1, 2, 3, 4, 5, 6, 7};

		regObj.regress(xValues, yValues, degree);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeNumberVerboseFalseTest()
	  public virtual void LargeNumberVerboseFalseTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 4;

		double[] xValues = new double[] {1, 2, 3, 4e2, 5, 6, 7};
		double[] yValues = new double[] {1, 2, 3, 4, 5, 6, 7};

		regObj.regressVerbose(xValues, yValues, degree, false);

	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void LargeNumberVerboseTrueTest()
	  public virtual void LargeNumberVerboseTrueTest()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();
		PolynomialsLeastSquaresFitter regObj = new PolynomialsLeastSquaresFitter();

		const int degree = 6;

		double[] xValues = new double[] {1, 2, 3, 4e17, 5, 6, 7};
		double[] yValues = new double[] {1, 2, 3, 4, 5, 6, 7};

		regObj.regressVerbose(xValues, yValues, degree, true);

	  }

	}

}
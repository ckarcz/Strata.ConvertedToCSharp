using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.COMMONS_ALGEBRA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;


	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using QRDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.QRDecompositionCommons;
	using QRDecompositionResult = com.opengamma.strata.math.impl.linearalgebra.QRDecompositionResult;
	using LeastSquaresRegressionResult = com.opengamma.strata.math.impl.regression.LeastSquaresRegressionResult;
	using MeanCalculator = com.opengamma.strata.math.impl.statistics.descriptive.MeanCalculator;
	using SampleStandardDeviationCalculator = com.opengamma.strata.math.impl.statistics.descriptive.SampleStandardDeviationCalculator;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;

	/// <summary>
	/// Derive coefficients of n-degree polynomial that minimizes least squares error of fit by
	/// using QR decomposition and back substitution.
	/// </summary>
	public class PolynomialsLeastSquaresFitter
	{

	  private QRDecompositionResult _qrResult;
	  private readonly double[] _renorm = new double[2];

	  /// <summary>
	  /// Given a set of data (X_i, Y_i) and degrees of a polynomial, determines optimal coefficients of the polynomial. </summary>
	  /// <param name="xData"> X values of data </param>
	  /// <param name="yData"> Y values of data </param>
	  /// <param name="degree"> Degree of polynomial which fits the given data </param>
	  /// <returns> LeastSquaresRegressionResult Containing optimal coefficients of the polynomial and difference between yData[i] and f(xData[i]),
	  ///   where f() is the polynomial with the derived coefficients </returns>
	  public virtual LeastSquaresRegressionResult regress(double[] xData, double[] yData, int degree)
	  {

		return regress(xData, yData, degree, false);
	  }

	  /// <summary>
	  /// Alternative regression method with different output. </summary>
	  /// <param name="xData"> X values of data </param>
	  /// <param name="yData"> Y values of data </param>
	  /// <param name="degree"> Degree of polynomial which fits the given data </param>
	  /// <param name="normalize"> Normalize xData by mean and standard deviation if normalize == true </param>
	  /// <returns> PolynomialsLeastSquaresRegressionResult containing coefficients, rMatrix, degrees of freedom, norm of residuals, and mean, standard deviation </returns>
	  public virtual PolynomialsLeastSquaresFitterResult regressVerbose(double[] xData, double[] yData, int degree, bool normalize)
	  {

		LeastSquaresRegressionResult result = regress(xData, yData, degree, normalize);

		int nData = xData.Length;
		DoubleMatrix rMatriX = _qrResult.R;

		DoubleArray resResult = DoubleArray.copyOf(result.Residuals);
		double resNorm = OG_ALGEBRA.getNorm2(resResult);

		if (normalize == true)
		{
		  return new PolynomialsLeastSquaresFitterResult(result.Betas, rMatriX, nData - degree - 1, resNorm, _renorm);
		}
		return new PolynomialsLeastSquaresFitterResult(result.Betas, rMatriX, nData - degree - 1, resNorm);
	  }

	  /// <summary>
	  /// This regression method is private and called in other regression methods </summary>
	  /// <param name="xData"> X values of data </param>
	  /// <param name="yData"> Y values of data </param>
	  /// <param name="degree"> Degree of polynomial which fits the given data </param>
	  /// <param name="normalize"> Normalize xData by mean and standard deviation if normalize == true </param>
	  /// <returns> LeastSquaresRegressionResult Containing optimal coefficients of the polynomial and difference between yData[i] and f(xData[i]) </returns>
	  private LeastSquaresRegressionResult regress(double[] xData, double[] yData, int degree, bool normalize)
	  {

		ArgChecker.notNull(xData, "xData");
		ArgChecker.notNull(yData, "yData");

		ArgChecker.isTrue(degree >= 0, "Minus degree");
		ArgChecker.isTrue(xData.Length == yData.Length, "xData length should be the same as yData length");
		ArgChecker.isTrue(xData.Length > degree, "Not enough amount of data");

		int nData = xData.Length;

		for (int i = 0; i < nData; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(xData[i]), "xData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(xData[i]), "xData containing Infinity");
		  ArgChecker.isFalse(double.IsNaN(yData[i]), "yData containing NaN");
		  ArgChecker.isFalse(double.IsInfinity(yData[i]), "yData containing Infinity");
		}

		for (int i = 0; i < nData; ++i)
		{
		  for (int j = i + 1; j < nData; ++j)
		  {
			ArgChecker.isFalse(xData[i] == xData[j] && yData[i] != yData[j], "Two distinct data on x=const. line");
		  }
		}

		int nRepeat = 0;
		for (int i = 0; i < nData; ++i)
		{
		  for (int j = i + 1; j < nData; ++j)
		  {
			if (xData[i] == xData[j] && yData[i] == yData[j])
			{
			  ++nRepeat;
			}
		  }
		}
		ArgChecker.isFalse(nRepeat > nData - degree - 1, "Too many repeated data");

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] tmpMatrix = new double[nData][degree + 1];
		double[][] tmpMatrix = RectangularArrays.ReturnRectangularDoubleArray(nData, degree + 1);

		if (normalize == true)
		{
		  double[] normData = normaliseData(xData);
		  for (int i = 0; i < nData; ++i)
		  {
			for (int j = 0; j < degree + 1; ++j)
			{
			  tmpMatrix[i][j] = Math.Pow(normData[i], j);
			}
		  }
		}
		else
		{
		  for (int i = 0; i < nData; ++i)
		  {
			for (int j = 0; j < degree + 1; ++j)
			{
			  tmpMatrix[i][j] = Math.Pow(xData[i], j);
			}
		  }
		}

		DoubleMatrix xDataMatrix = DoubleMatrix.copyOf(tmpMatrix);
		DoubleArray yDataVector = DoubleArray.copyOf(yData);

		double vandNorm = COMMONS_ALGEBRA.getNorm2(xDataMatrix);
		ArgChecker.isFalse(vandNorm > 1e9, "Too large input data or too many degrees");

		return regress(xDataMatrix, yDataVector, nData, degree);

	  }

	  /// <summary>
	  /// This regression method is private and called in other regression methods </summary>
	  /// <param name="xDataMatrix"> _nData x (_degree + 1) matrix whose low vector is (xData[i]^0, xData[i]^1, ..., xData[i]^{_degree}) </param>
	  /// <param name="yDataVector"> the y-values </param>
	  /// <param name="nData"> Number of data points </param>
	  /// <param name="degree">  the degree </param>
	  private LeastSquaresRegressionResult regress(DoubleMatrix xDataMatrix, DoubleArray yDataVector, int nData, int degree)
	  {

		Decomposition<QRDecompositionResult> qrComm = new QRDecompositionCommons();

		DecompositionResult decompResult = qrComm.apply(xDataMatrix);
		_qrResult = (QRDecompositionResult) decompResult;

		DoubleMatrix qMatrix = _qrResult.Q;
		DoubleMatrix rMatrix = _qrResult.R;

		double[] betas = backSubstitution(qMatrix, rMatrix, yDataVector, degree);
		double[] residuals = residualsSolver(xDataMatrix, betas, yDataVector);

		for (int i = 0; i < degree + 1; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(betas[i]), "Input is too large or small");
		}
		for (int i = 0; i < nData; ++i)
		{
		  ArgChecker.isFalse(double.IsNaN(residuals[i]), "Input is too large or small");
		}

		return new LeastSquaresRegressionResult(betas, residuals, 0.0, null, 0.0, 0.0, null, null, true);

	  }

	  /// <summary>
	  /// Under the QR decomposition, xDataMatrix = qMatrix * rMatrix, optimal coefficients of the
	  /// polynomial are computed by back substitution </summary>
	  /// <param name="qMatrix">  the q-matrix </param>
	  /// <param name="rMatrix">  the r-matrix </param>
	  /// <param name="yDataVector">  the y-values </param>
	  /// <param name="degree">  the degree </param>
	  /// <returns> Coefficients of the polynomial which minimize least square </returns>
	  private double[] backSubstitution(DoubleMatrix qMatrix, DoubleMatrix rMatrix, DoubleArray yDataVector, int degree)
	  {

		double[] res = new double[degree + 1];
		Arrays.fill(res, 0.0);

		DoubleMatrix tpMatrix = OG_ALGEBRA.getTranspose(qMatrix);
		DoubleArray yDataVecConv = (DoubleArray) OG_ALGEBRA.multiply(tpMatrix, yDataVector);

		for (int i = 0; i < degree + 1; ++i)
		{
		  double tmp = 0.0;
		  for (int j = 0; j < i; ++j)
		  {
			tmp -= rMatrix.get(degree - i, degree - j) * res[degree - j] / rMatrix.get(degree - i, degree - i);
		  }
		  res[degree - i] = yDataVecConv.get(degree - i) / rMatrix.get(degree - i, degree - i) + tmp;
		}

		return res;
	  }

	  /// 
	  /// <param name="xDataMatrix">  the x-matrix </param>
	  /// <param name="betas"> Optimal coefficients of the polynomial </param>
	  /// <param name="yDataVector">  the y-vlaues </param>
	  /// <returns> Difference between yData[i] and f(xData[i]), where f() is the polynomial with derived coefficients </returns>
	  private double[] residualsSolver(DoubleMatrix xDataMatrix, double[] betas, DoubleArray yDataVector)
	  {

		DoubleArray betasVector = DoubleArray.copyOf(betas);

		DoubleArray modelValuesVector = (DoubleArray) OG_ALGEBRA.multiply(xDataMatrix, betasVector);
		DoubleArray res = (DoubleArray) OG_ALGEBRA.subtract(yDataVector, modelValuesVector);

		return res.toArray();

	  }

	  /// <summary>
	  /// Normalize x_i as x_i -> (x_i - mean)/(standard deviation) </summary>
	  /// <param name="xData"> X values of data </param>
	  /// <returns> Normalized X values </returns>
	  private double[] normaliseData(double[] xData)
	  {

		int nData = xData.Length;
		double[] res = new double[nData];

		System.Func<double[], double> calculator = new MeanCalculator();
		_renorm[0] = calculator(xData);
		calculator = new SampleStandardDeviationCalculator();
		_renorm[1] = calculator(xData);

		double tmp = _renorm[0] / _renorm[1];
		for (int i = 0; i < nData; ++i)
		{
		  res[i] = xData[i] / _renorm[1] - tmp;
		}

		return res;
	  }

	}

}
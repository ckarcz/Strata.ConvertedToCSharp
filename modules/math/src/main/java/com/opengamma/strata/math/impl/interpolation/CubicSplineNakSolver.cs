using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Solves cubic spline problem with Not-A-Knot endpoint conditions, where the third derivative
	/// at the endpoints is the same as that of their adjacent points.
	/// </summary>
	public class CubicSplineNakSolver : CubicSplineSolver
	{

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix solve(final double[] xValues, final double[] yValues)
	  public override DoubleMatrix solve(double[] xValues, double[] yValues)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = getDiffs(xValues);
		double[] intervals = getDiffs(xValues);

		return getSplineCoeffs(xValues, yValues, intervals, matrixEqnSolver(getMatrix(intervals), getVector(yValues, intervals)));
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix[] solveWithSensitivity(final double[] xValues, final double[] yValues)
	  public override DoubleMatrix[] solveWithSensitivity(double[] xValues, double[] yValues)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = getDiffs(xValues);
		double[] intervals = getDiffs(xValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] toBeInv = getMatrix(intervals);
		double[][] toBeInv = getMatrix(intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] vector = getVector(yValues, intervals);
		double[] vector = getVector(yValues, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] vecSensitivity = getVectorSensitivity(intervals);
		double[][] vecSensitivity = getVectorSensitivity(intervals);

		return getSplineCoeffsWithSensitivity(xValues, yValues, intervals, toBeInv, vector, vecSensitivity);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix[] solveMultiDim(final double[] xValues, final com.opengamma.strata.collect.array.DoubleMatrix yValuesMatrix)
	  public override DoubleMatrix[] solveMultiDim(double[] xValues, DoubleMatrix yValuesMatrix)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = yValuesMatrix.rowCount();
		int dim = yValuesMatrix.rowCount();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] coefMatrix = new com.opengamma.strata.collect.array.DoubleMatrix[dim];
		DoubleMatrix[] coefMatrix = new DoubleMatrix[dim];

		for (int i = 0; i < dim; ++i)
		{
		  coefMatrix[i] = solve(xValues, yValuesMatrix.row(i).toArray());
		}

		return coefMatrix;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleArray getKnotsMat1D(final double[] xValues)
	  public override DoubleArray getKnotsMat1D(double[] xValues)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;
		if (nData == 2)
		{
		  return DoubleArray.of(xValues[0], xValues[nData - 1]);
		}
		if (nData == 3)
		{
		  return DoubleArray.of(xValues[0], xValues[nData - 1]);
		}
		return DoubleArray.copyOf(xValues);
	  }

	  /// <param name="xValues"> X values of Data </param>
	  /// <param name="yValues"> Y values of Data </param>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <param name="solnVector"> Values of second derivative at knots </param>
	  /// <returns> Coefficient matrix whose i-th row vector is (a_0,a_1,...) for i-th intervals, where a_0,a_1,... are coefficients of f(x) = a_0 + a_1 x^1 + .... </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleMatrix getSplineCoeffs(final double[] xValues, final double[] yValues, final double[] intervals, final double[] solnVector)
	  private DoubleMatrix getSplineCoeffs(double[] xValues, double[] yValues, double[] intervals, double[] solnVector)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;

		if (nData == 2)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] res = new double[][] {{ yValues[1] / intervals[0] - yValues[0] / intervals[0] - intervals[0] * solnVector[0] / 2.0 - intervals[0] * solnVector[1] / 6.0 + intervals[0] * solnVector[0] / 6.0, yValues[0] } };
		  double[][] res = new double[][]
		  {
			  new double[] {yValues[1] / intervals[0] - yValues[0] / intervals[0] - intervals[0] * solnVector[0] / 2.0 - intervals[0] * solnVector[1] / 6.0 + intervals[0] * solnVector[0] / 6.0, yValues[0]}
		  };
		  return DoubleMatrix.copyOf(res);
		}
		if (nData == 3)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] res = new double[][] {{solnVector[0] / 2.0, yValues[1] / intervals[0] - yValues[0] / intervals[0] - intervals[0] * solnVector[0] / 2.0, yValues[0] } };
		  double[][] res = new double[][]
		  {
			  new double[] {solnVector[0] / 2.0, yValues[1] / intervals[0] - yValues[0] / intervals[0] - intervals[0] * solnVector[0] / 2.0, yValues[0]}
		  };
		  return DoubleMatrix.copyOf(res);
		}
		return getCommonSplineCoeffs(xValues, yValues, intervals, solnVector);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.array.DoubleMatrix[] getSplineCoeffsWithSensitivity(final double[] xValues, final double[] yValues, final double[] intervals, final double[][] toBeInv, final double[] vector, final double[][] vecSensitivity)
	  private DoubleMatrix[] getSplineCoeffsWithSensitivity(double[] xValues, double[] yValues, double[] intervals, double[][] toBeInv, double[] vector, double[][] vecSensitivity)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = xValues.length;
		int nData = xValues.Length;

		if (nData == 2)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] res = new com.opengamma.strata.collect.array.DoubleMatrix[nData];
		  DoubleMatrix[] res = new DoubleMatrix[nData];
		  res[0] = DoubleMatrix.of(1, 1, yValues[1] / intervals[0] - yValues[0] / intervals[0], yValues[0]);
		  res[1] = DoubleMatrix.of(2, 2, -1d / intervals[0], 1d / intervals[0], 1d, 0d);
		  return res;
		}
		if (nData == 3)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] res = new com.opengamma.strata.collect.array.DoubleMatrix[2];
		  DoubleMatrix[] res = new DoubleMatrix[2];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] soln = combinedMatrixEqnSolver(toBeInv, vector, vecSensitivity);
		  DoubleArray[] soln = combinedMatrixEqnSolver(toBeInv, vector, vecSensitivity);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coef = new double[][] {{soln[0].get(0) / 2.0, yValues[1] / intervals[0] - yValues[0] / intervals[0] - intervals[0] * soln[0].get(0) / 2.0, yValues[0]}};
		  double[][] coef = new double[][]
		  {
			  new double[] {soln[0].get(0) / 2.0, yValues[1] / intervals[0] - yValues[0] / intervals[0] - intervals[0] * soln[0].get(0) / 2.0, yValues[0]}
		  };
		  res[0] = DoubleMatrix.copyOf(coef);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] coefSense = new double[3][0];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] coefSense = new double[3][0];
		  double[][] coefSense = RectangularArrays.ReturnRectangularDoubleArray(3, 0);
		  coefSense[0] = new double[] {soln[1].get(0) / 2.0, soln[2].get(0) / 2.0, soln[3].get(0) / 2.0};
		  coefSense[1] = new double[] {-1.0 / intervals[0] - intervals[0] * soln[1].get(0) / 2.0, 1.0 / intervals[0] - intervals[0] * soln[2].get(0) / 2.0, -intervals[0] * soln[3].get(0) / 2.0};
		  coefSense[2] = new double[] {1.0, 0.0, 0.0};
		  res[1] = DoubleMatrix.copyOf(coefSense);
		  return res;
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] res = new com.opengamma.strata.collect.array.DoubleMatrix[nData];
		DoubleMatrix[] res = new DoubleMatrix[nData];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] soln = combinedMatrixEqnSolver(toBeInv, vector, vecSensitivity);
		DoubleArray[] soln = combinedMatrixEqnSolver(toBeInv, vector, vecSensitivity);
		res[0] = getCommonSplineCoeffs(xValues, yValues, intervals, soln[0].toArray());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] solnMatrix = new double[nData][nData];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] solnMatrix = new double[nData][nData];
		double[][] solnMatrix = RectangularArrays.ReturnRectangularDoubleArray(nData, nData);
		for (int i = 0; i < nData; ++i)
		{
		  for (int j = 0; j < nData; ++j)
		  {
			solnMatrix[i][j] = soln[j + 1].get(i);
		  }
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix[] tmp = getCommonSensitivityCoeffs(intervals, solnMatrix);
		DoubleMatrix[] tmp = getCommonSensitivityCoeffs(intervals, solnMatrix);
		Array.Copy(tmp, 0, res, 1, nData - 1);
		return res;
	  }

	  /// <summary>
	  /// Cubic spline is obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector </summary>
	  /// <param name="yValues"> Y Values of data </param>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <returns> Vector b </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[] getVector(final double[] yValues, final double[] intervals)
	  private double[] getVector(double[] yValues, double[] intervals)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = yValues.length;
		int nData = yValues.Length;
		double[] res = new double[nData];

		if (nData == 3)
		{
		  for (int i = 0; i < nData; ++i)
		  {
			res[i] = 2.0 * yValues[2] / (intervals[0] + intervals[1]) - 2.0 * yValues[0] / (intervals[0] + intervals[1]) - 2.0 * yValues[1] / (intervals[0]) + 2.0 * yValues[0] / (intervals[0]);
		  }
		}
		else
		{
		  res = getCommonVectorElements(yValues, intervals);
		}
		return res;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[][] getVectorSensitivity(final double[] intervals)
	  private double[][] getVectorSensitivity(double[] intervals)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = intervals.length + 1;
		int nData = intervals.Length + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nData][nData];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nData, nData);

		if (nData == 3)
		{
		  for (int i = 0; i < nData; ++i)
		  {
			res[i][0] = -2.0 / (intervals[0] + intervals[1]) + 2.0 / (intervals[0]);
			res[i][1] = -2.0 / (intervals[0]);
			res[i][2] = 2.0 / (intervals[0] + intervals[1]);
		  }
		}
		else
		{
		  res = getCommonVectorSensitivity(intervals);
		}
		return res;
	  }

	  /// <summary>
	  /// Cubic spline is obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector </summary>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <returns> Matrix A </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private double[][] getMatrix(final double[] intervals)
	  private double[][] getMatrix(double[] intervals)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData = intervals.length + 1;
		int nData = intervals.Length + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nData][nData];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nData, nData);

		for (int i = 0; i < nData; ++i)
		{
		  Arrays.fill(res[i], 0.0);
		}

		if (nData == 2)
		{
		  res[0][1] = intervals[0];
		  res[1][0] = intervals[0];
		  return res;
		}
		if (nData == 3)
		{
		  res[0][0] = intervals[1];
		  res[1][1] = intervals[1];
		  res[2][2] = intervals[1];
		  return res;
		}
		res = getCommonMatrixElements(intervals);
		res[0][0] = -intervals[1];
		res[0][1] = intervals[0] + intervals[1];
		res[0][2] = -intervals[0];
		res[nData - 1][nData - 3] = -intervals[nData - 2];
		res[nData - 1][nData - 2] = intervals[nData - 3] + intervals[nData - 2];
		res[nData - 1][nData - 1] = -intervals[nData - 3];
		return res;
	  }

	}

}
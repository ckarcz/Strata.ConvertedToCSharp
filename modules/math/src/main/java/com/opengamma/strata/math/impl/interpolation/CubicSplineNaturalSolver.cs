/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Solves cubic spline problem with natural endpoint conditions, where the second derivative at the endpoints is 0.
	/// </summary>
	public class CubicSplineNaturalSolver : CubicSplineSolver
	{

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix solve(final double[] xValues, final double[] yValues)
	  public override DoubleMatrix solve(double[] xValues, double[] yValues)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = getDiffs(xValues);
		double[] intervals = getDiffs(xValues);
		return getCommonSplineCoeffs(xValues, yValues, intervals, matrixEqnSolver(getMatrix(intervals), getCommonVectorElements(yValues, intervals)));
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
//ORIGINAL LINE: final double[] commonVector = getCommonVectorElements(yValues, intervals);
		double[] commonVector = getCommonVectorElements(yValues, intervals);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[][] commonVecSensitivity = getCommonVectorSensitivity(intervals);
		double[][] commonVecSensitivity = getCommonVectorSensitivity(intervals);

		return getCommonCoefficientWithSensitivity(xValues, yValues, intervals, toBeInv, commonVector, commonVecSensitivity);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix[] solveMultiDim(final double[] xValues, final com.opengamma.strata.collect.array.DoubleMatrix yValuesMatrix)
	  public override DoubleMatrix[] solveMultiDim(double[] xValues, DoubleMatrix yValuesMatrix)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = yValuesMatrix.rowCount();
		int dim = yValuesMatrix.rowCount();
		DoubleMatrix[] coefMatrix = new DoubleMatrix[dim];

		for (int i = 0; i < dim; ++i)
		{
		  coefMatrix[i] = solve(xValues, yValuesMatrix.row(i).toArray());
		}

		return coefMatrix;
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

		res = getCommonMatrixElements(intervals);
		res[0][0] = 1.0;
		res[nData - 1][nData - 1] = 1.0;

		return res;
	  }
	}

}
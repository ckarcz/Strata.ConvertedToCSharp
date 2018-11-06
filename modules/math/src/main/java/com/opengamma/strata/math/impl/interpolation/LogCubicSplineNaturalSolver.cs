/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using TridiagonalMatrix = com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix;
	using TridiagonalSolver = com.opengamma.strata.math.impl.linearalgebra.TridiagonalSolver;

	/// <summary>
	/// For specific cubic spline interpolations, polynomial coefficients are determined by the tridiagonal algorithm.
	/// </summary>
	public class LogCubicSplineNaturalSolver : CubicSplineSolver
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

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override protected double[] matrixEqnSolver(final double[][] doubMat, final double[] doubVec)
	  protected internal override double[] matrixEqnSolver(double[][] doubMat, double[] doubVec)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int sizeM1 = doubMat.length - 1;
		int sizeM1 = doubMat.Length - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] a = new double[sizeM1];
		double[] a = new double[sizeM1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] b = new double[sizeM1 + 1];
		double[] b = new double[sizeM1 + 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] c = new double[sizeM1];
		double[] c = new double[sizeM1];

		for (int i = 0; i < sizeM1; ++i)
		{
		  a[i] = doubMat[i][i + 1];
		  b[i] = doubMat[i][i];
		  c[i] = doubMat[i + 1][i];
		}
		b[sizeM1] = doubMat[sizeM1][sizeM1];

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix m = new com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix(b, a, c);
		TridiagonalMatrix m = new TridiagonalMatrix(b, a, c);

		return TridiagonalSolver.solvTriDag(m, doubVec);
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override protected com.opengamma.strata.collect.array.DoubleArray[] combinedMatrixEqnSolver(final double[][] doubMat1, final double[] doubVec, final double[][] doubMat2)
	  protected internal override DoubleArray[] combinedMatrixEqnSolver(double[][] doubMat1, double[] doubVec, double[][] doubMat2)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int size = doubVec.length;
		int size = doubVec.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] res = new com.opengamma.strata.collect.array.DoubleArray[size + 1];
		DoubleArray[] res = new DoubleArray[size + 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleMatrix doubMat2Matrix = com.opengamma.strata.collect.array.DoubleMatrix.copyOf(doubMat2);
		DoubleMatrix doubMat2Matrix = DoubleMatrix.copyOf(doubMat2);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] u = new double[size - 1];
		double[] u = new double[size - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] d = new double[size];
		double[] d = new double[size];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] l = new double[size - 1];
		double[] l = new double[size - 1];

		for (int i = 0; i < size - 1; ++i)
		{
		  u[i] = doubMat1[i][i + 1];
		  d[i] = doubMat1[i][i];
		  l[i] = doubMat1[i + 1][i];
		}
		d[size - 1] = doubMat1[size - 1][size - 1];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix m = new com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix(d, u, l);
		TridiagonalMatrix m = new TridiagonalMatrix(d, u, l);
		res[0] = DoubleArray.copyOf(TridiagonalSolver.solvTriDag(m, doubVec));
		for (int i = 0; i < size; ++i)
		{
		  DoubleArray doubMat2Colum = doubMat2Matrix.column(i);
		  res[i + 1] = TridiagonalSolver.solvTriDag(m, doubMat2Colum);
		}
		return res;
	  }

	}

}
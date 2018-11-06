using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using LUDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionCommons;
	using LUDecompositionResult = com.opengamma.strata.math.impl.linearalgebra.LUDecompositionResult;
	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Abstract class of solving cubic spline problem
	/// Implementation depends on endpoint conditions
	/// "solve" for 1-dimensional problem and "solveMultiDim" for  multi-dimensional problem should be implemented in inherited classes
	/// "getKnotsMat1D" is overridden in certain cases 
	/// </summary>
	internal abstract class CubicSplineSolver
	{

	  private readonly Decomposition<LUDecompositionResult> _luObj = new LUDecompositionCommons();

	  /// <summary>
	  /// One-dimensional cubic spline
	  /// If (xValues length) = (yValues length), Not-A-Knot endpoint conditions are used
	  /// If (xValues length) + 2 = (yValues length), Clamped endpoint conditions are used </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> Coefficient matrix whose i-th row vector is (a_0,a_1,...) for i-th intervals, where a_0,a_1,... are coefficients of f(x) = a_0 + a_1 x^1 + ....
	  ///   Note that the degree of polynomial is NOT necessarily 3 </returns>
	  public abstract DoubleMatrix solve(double[] xValues, double[] yValues);

	  /// <summary>
	  /// One-dimensional cubic spline
	  /// If (xValues length) = (yValues length), Not-A-Knot endpoint conditions are used
	  /// If (xValues length) + 2 = (yValues length), Clamped endpoint conditions are used </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <returns> Array of  matrices: the 0-th element is Coefficient Matrix (same as the solve method above), the i-th element is \frac{\partial a^{i-1}_j}{\partial yValues_k} 
	  ///   where a_0^i,a_1^i,... are coefficients of f^i(x) = a_0^i + a_1^i (x - xValues_{i}) + .... with x \in [xValues_{i}, xValues_{i+1}] </returns>
	  public abstract DoubleMatrix[] solveWithSensitivity(double[] xValues, double[] yValues);

	  /// <summary>
	  /// Multi-dimensional cubic spline
	  /// If (xValues length) = (yValuesMatrix NumberOfColumn), Not-A-Knot endpoint conditions are used
	  /// If (xValues length) + 2 = (yValuesMatrix NumberOfColumn), Clamped endpoint conditions are used </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValuesMatrix"> Y values of data, where NumberOfRow defines dimension of the spline </param>
	  /// <returns> A set of coefficient matrices whose i-th row vector is (a_0,a_1,...) for the i-th interval, where a_0,a_1,... are coefficients of f(x) = a_0 + a_1 x^1 + .... 
	  ///   Each matrix corresponds to an interpolation (xValues, yValuesMatrix RowVector)
	  ///   Note that the degree of polynomial is NOT necessarily 3 </returns>
	  public abstract DoubleMatrix[] solveMultiDim(double[] xValues, DoubleMatrix yValuesMatrix);

	  /// <param name="xValues"> X values of data </param>
	  /// <returns> X values of knots (Note that these are NOT necessarily xValues if nDataPts=2,3) </returns>
	  public virtual DoubleArray getKnotsMat1D(double[] xValues)
	  {
		return DoubleArray.copyOf(xValues);
	  }

	  /// <param name="xValues"> X values of Data </param>
	  /// <returns> {xValues[1]-xValues[0], xValues[2]-xValues[1],...}
	  ///   xValues (and corresponding yValues) should be sorted before calling this method </returns>
	  protected internal virtual double[] getDiffs(double[] xValues)
	  {

		int nDataPts = xValues.Length;
		double[] res = new double[nDataPts - 1];

		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  res[i] = xValues[i + 1] - xValues[i];
		}

		return res;
	  }

	  /// <param name="xValues"> X values of Data </param>
	  /// <param name="yValues"> Y values of Data </param>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <param name="solnVector"> Values of second derivative at knots </param>
	  /// <returns> Coefficient matrix whose i-th row vector is {a_0,a_1,...} for i-th intervals, where a_0,a_1,... are coefficients of f(x) = a_0 + a_1 x^1 + .... </returns>
	  protected internal virtual DoubleMatrix getCommonSplineCoeffs(double[] xValues, double[] yValues, double[] intervals, double[] solnVector)
	  {

		int nDataPts = xValues.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nDataPts - 1][4];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, 4);
		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  res[i][0] = solnVector[i + 1] / 6.0 / intervals[i] - solnVector[i] / 6.0 / intervals[i];
		  res[i][1] = 0.5 * solnVector[i];
		  res[i][2] = yValues[i + 1] / intervals[i] - yValues[i] / intervals[i] - intervals[i] * solnVector[i] / 2.0 - intervals[i] * solnVector[i + 1] / 6.0 + intervals[i] * solnVector[i] / 6.0;
		  res[i][3] = yValues[i];
		}
		return DoubleMatrix.copyOf(res);
	  }

	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <param name="solnMatrix"> Sensitivity of second derivatives (x 0.5) </param>
	  /// <returns> Array of i coefficient matrices \frac{\partial a^i_j}{\partial y_k} </returns>
	  protected internal virtual DoubleMatrix[] getCommonSensitivityCoeffs(double[] intervals, double[][] solnMatrix)
	  {

		int nDataPts = intervals.Length + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] res = new double[nDataPts - 1][4][nDataPts];
		double[][][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts - 1, 4, nDataPts);
		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  res[i][3][i] = 1.0;
		  res[i][2][i + 1] = 1.0 / intervals[i];
		  res[i][2][i] = -1.0 / intervals[i];
		  for (int k = 0; k < nDataPts; ++k)
		  {
			res[i][0][k] = solnMatrix[i + 1][k] / 6.0 / intervals[i] - solnMatrix[i][k] / 6.0 / intervals[i];
			res[i][1][k] = 0.5 * solnMatrix[i][k];
			res[i][2][k] += -intervals[i] * solnMatrix[i][k] / 2.0 - intervals[i] * solnMatrix[i + 1][k] / 6.0 + intervals[i] * solnMatrix[i][k] / 6.0;
		  }
		}

		DoubleMatrix[] resMat = new DoubleMatrix[nDataPts - 1];
		for (int i = 0; i < nDataPts - 1; ++i)
		{
		  resMat[i] = DoubleMatrix.copyOf(res[i]);
		}
		return resMat;
	  }

	  /// <summary>
	  /// Cubic spline and its node sensitivity are respectively obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector and AN=L where N,L are matrices </summary>
	  /// <param name="xValues"> X values of data </param>
	  /// <param name="yValues"> Y values of data </param>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <param name="toBeInv"> The matrix A </param>
	  /// <param name="commonVector"> The vector b </param>
	  /// <param name="commonVecSensitivity"> The matrix L </param>
	  /// <returns> Coefficient matrices of interpolant (x) and its node sensitivity (N) </returns>
	  protected internal virtual DoubleMatrix[] getCommonCoefficientWithSensitivity(double[] xValues, double[] yValues, double[] intervals, double[][] toBeInv, double[] commonVector, double[][] commonVecSensitivity)
	  {
		int nDataPts = xValues.Length;

		DoubleArray[] soln = this.combinedMatrixEqnSolver(toBeInv, commonVector, commonVecSensitivity);
		DoubleMatrix[] res = new DoubleMatrix[nDataPts];

		res[0] = getCommonSplineCoeffs(xValues, yValues, intervals, soln[0].toArray());
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] solnMatrix = new double[nDataPts][nDataPts];
		double[][] solnMatrix = RectangularArrays.ReturnRectangularDoubleArray(nDataPts, nDataPts);
		for (int i = 0; i < nDataPts; ++i)
		{
		  for (int j = 0; j < nDataPts; ++j)
		  {
			solnMatrix[i][j] = soln[j + 1].get(i);
		  }
		}
		DoubleMatrix[] tmp = getCommonSensitivityCoeffs(intervals, solnMatrix);
		Array.Copy(tmp, 0, res, 1, nDataPts - 1);

		return res;
	  }

	  /// <summary>
	  /// Cubic spline is obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector </summary>
	  /// <param name="intervals">  the intervals </param>
	  /// <returns> Endpoint-independent part of the matrix A </returns>
	  protected internal virtual double[][] getCommonMatrixElements(double[] intervals)
	  {

		int nDataPts = intervals.Length + 1;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nDataPts][nDataPts];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts, nDataPts);

		for (int i = 0; i < nDataPts; ++i)
		{
		  Arrays.fill(res[i], 0.0);
		}

		for (int i = 1; i < nDataPts - 1; ++i)
		{
		  res[i][i - 1] = intervals[i - 1];
		  res[i][i] = 2.0 * (intervals[i - 1] + intervals[i]);
		  res[i][i + 1] = intervals[i];
		}

		return res;
	  }

	  /// <summary>
	  /// Cubic spline is obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector </summary>
	  /// <param name="yValues"> Y values of Data </param>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <returns> Endpoint-independent part of vector b </returns>
	  protected internal virtual double[] getCommonVectorElements(double[] yValues, double[] intervals)
	  {

		int nDataPts = yValues.Length;
		double[] res = new double[nDataPts];
		Arrays.fill(res, 0.0);

		for (int i = 1; i < nDataPts - 1; ++i)
		{
		  res[i] = 6.0 * yValues[i + 1] / intervals[i] - 6.0 * yValues[i] / intervals[i] - 6.0 * yValues[i] / intervals[i - 1] + 6.0 * yValues[i - 1] / intervals[i - 1];
		}

		return res;
	  }

	  /// <summary>
	  /// Node sensitivity is obtained by solving a linear problem AN = L where A,N,L are matrices </summary>
	  /// <param name="intervals"> {xValues[1]-xValues[0], xValues[2]-xValues[1],...} </param>
	  /// <returns> The matrix L </returns>
	  protected internal virtual double[][] getCommonVectorSensitivity(double[] intervals)
	  {
		int nDataPts = intervals.Length + 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nDataPts][nDataPts];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nDataPts, nDataPts);
		for (int i = 0; i < nDataPts; ++i)
		{
		  Arrays.fill(res[i], 0.0);
		}

		for (int i = 1; i < nDataPts - 1; ++i)
		{
		  res[i][i - 1] = 6.0 / intervals[i - 1];
		  res[i][i] = -6.0 / intervals[i] - 6.0 / intervals[i - 1];
		  res[i][i + 1] = 6.0 / intervals[i];
		}

		return res;
	  }

	  /// <summary>
	  /// Cubic spline is obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector
	  /// This can be done by LU decomposition </summary>
	  /// <param name="doubMat"> Matrix A </param>
	  /// <param name="doubVec"> Vector B </param>
	  /// <returns> Solution to the linear equation, x </returns>
	  protected internal virtual double[] matrixEqnSolver(double[][] doubMat, double[] doubVec)
	  {
		LUDecompositionResult result = _luObj.apply(DoubleMatrix.copyOf(doubMat));

		double[][] lMat = result.L.toArray();
		double[][] uMat = result.U.toArray();
		DoubleArray doubVecMod = ((DoubleArray) OG_ALGEBRA.multiply(result.P, DoubleArray.copyOf(doubVec)));

		return backSubstitution(uMat, forwardSubstitution(lMat, doubVecMod));
	  }

	  /// <summary>
	  /// Cubic spline and its node sensitivity are respectively obtained by solving a linear problem Ax=b where A is a square matrix and x,b are vector and AN=L where N,L are matrices </summary>
	  /// <param name="doubMat1"> The matrix A </param>
	  /// <param name="doubVec"> The vector b </param>
	  /// <param name="doubMat2"> The matrix L </param>
	  /// <returns> The solutions to the linear systems, x,N </returns>
	  protected internal virtual DoubleArray[] combinedMatrixEqnSolver(double[][] doubMat1, double[] doubVec, double[][] doubMat2)
	  {
		int nDataPts = doubVec.Length;
		LUDecompositionResult result = _luObj.apply(DoubleMatrix.copyOf(doubMat1));

		double[][] lMat = result.L.toArray();
		double[][] uMat = result.U.toArray();
		DoubleMatrix pMat = result.P;
		DoubleArray doubVecMod = ((DoubleArray) OG_ALGEBRA.multiply(pMat, DoubleArray.copyOf(doubVec)));

		DoubleMatrix doubMat2Matrix = DoubleMatrix.copyOf(doubMat2);
		DoubleArray[] res = new DoubleArray[nDataPts + 1];
		res[0] = DoubleArray.copyOf(backSubstitution(uMat, forwardSubstitution(lMat, doubVecMod)));
		for (int i = 0; i < nDataPts; ++i)
		{
		  DoubleArray doubMat2Colum = doubMat2Matrix.column(i);
		  DoubleArray doubVecMod2 = ((DoubleArray) OG_ALGEBRA.multiply(pMat, doubMat2Colum));
		  res[i + 1] = DoubleArray.copyOf(backSubstitution(uMat, forwardSubstitution(lMat, doubVecMod2)));
		}
		return res;
	  }

	  /// <summary>
	  /// Linear problem Ax=b is solved by forward substitution if A is lower triangular.
	  /// </summary>
	  /// <param name="lMat"> Lower triangular matrix </param>
	  /// <param name="doubVec"> Vector b </param>
	  /// <returns> Solution to the linear equation, x </returns>
	  private double[] forwardSubstitution(double[][] lMat, DoubleArray doubVec)
	  {
		int size = lMat.Length;
		double[] res = new double[size];
		for (int i = 0; i < size; ++i)
		{
		  double tmp = doubVec.get(i) / lMat[i][i];
		  for (int j = 0; j < i; ++j)
		  {
			tmp -= lMat[i][j] * res[j] / lMat[i][i];
		  }
		  res[i] = tmp;
		}
		return res;
	  }

	  /// <summary>
	  /// Linear problem Ax=b is solved by backward substitution if A is upper triangular.
	  /// </summary>
	  /// <param name="uMat"> Upper triangular matrix </param>
	  /// <param name="doubVec"> Vector b </param>
	  /// <returns> Solution to the linear equation, x </returns>
	  private double[] backSubstitution(double[][] uMat, double[] doubVec)
	  {
		int size = uMat.Length;
		double[] res = new double[size];
		for (int i = size - 1; i > -1; --i)
		{
		  double tmp = doubVec[i] / uMat[i][i];
		  for (int j = i + 1; j < size; ++j)
		  {
			tmp -= uMat[i][j] * res[j] / uMat[i][i];
		  }
		  res[i] = tmp;
		}
		return res;
	  }

	}

}
/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Solves cubic spline problem with clamped endpoint conditions, where the first derivative is specified at endpoints.
	/// </summary>
	public class CubicSplineClampedSolver : CubicSplineSolver
	{

	  private double[] _iniConds;
	  private double[] _finConds;
	  private double _iniCondUse;
	  private double _finCondUse;

	  /// <summary>
	  /// Constructor for a one-dimensional problem. </summary>
	  /// <param name="iniCond"> Left endpoint condition </param>
	  /// <param name="finCond"> Right endpoint condition </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public CubicSplineClampedSolver(final double iniCond, final double finCond)
	  public CubicSplineClampedSolver(double iniCond, double finCond)
	  {

		_iniCondUse = iniCond;
		_finCondUse = finCond;
	  }

	  /// <summary>
	  /// Constructor for a multi-dimensional problem. </summary>
	  /// <param name="iniConds"> Set of left endpoint conditions </param>
	  /// <param name="finConds"> Set of right endpoint conditions  </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public CubicSplineClampedSolver(final double[] iniConds, final double[] finConds)
	  public CubicSplineClampedSolver(double[] iniConds, double[] finConds)
	  {

		_iniConds = iniConds;
		_finConds = finConds;
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public com.opengamma.strata.collect.array.DoubleMatrix solve(final double[] xValues, final double[] yValues)
	  public override DoubleMatrix solve(double[] xValues, double[] yValues)
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] intervals = getDiffs(xValues);
		double[] intervals = getDiffs(xValues);

		return getCommonSplineCoeffs(xValues, yValues, intervals, matrixEqnSolver(getMatrix(intervals), getVector(yValues, intervals)));
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

		return getCommonCoefficientWithSensitivity(xValues, yValues, intervals, toBeInv, vector, vecSensitivity);
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
		  resetConds(i);
		  coefMatrix[i] = solve(xValues, yValuesMatrix.row(i).toArray());
		}

		return coefMatrix;
	  }

	  /// <summary>
	  /// Reset endpoint conditions. </summary>
	  /// <param name="i">   the index </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private void resetConds(final int i)
	  private void resetConds(int i)
	  {
		_iniCondUse = _iniConds[i];
		_finCondUse = _finConds[i];
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
		res[0][0] = 2.0 * intervals[0];
		res[0][1] = intervals[0];
		res[nData - 1][nData - 2] = intervals[nData - 2];
		res[nData - 1][nData - 1] = 2.0 * intervals[nData - 2];
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
		res = getCommonVectorElements(yValues, intervals);

		res[0] = 6.0 * yValues[1] / intervals[0] - 6.0 * yValues[0] / intervals[0] - 6.0 * _iniCondUse;
		res[nData - 1] = 6.0 * _finCondUse - 6.0 * yValues[nData - 1] / intervals[nData - 2] + 6.0 * yValues[nData - 2] / intervals[nData - 2];

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
		res = getCommonVectorSensitivity(intervals);

		res[0][0] = -6.0 / intervals[0];
		res[0][1] = 6.0 / intervals[0];
		res[nData - 1][nData - 1] = -6.0 / intervals[nData - 2];
		res[nData - 1][nData - 2] = 6.0 / intervals[nData - 2];

		return res;
	  }
	}

}
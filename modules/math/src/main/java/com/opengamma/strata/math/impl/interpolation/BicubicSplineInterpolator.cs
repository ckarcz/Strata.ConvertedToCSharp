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

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using PiecewisePolynomialFunction1D = com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D;

	/// <summary>
	///  Given a set of data (x0Values_i, x1Values_j, yValues_{ij}), derive the piecewise bicubic function, f(x0,x1) = sum_{i=0}^{3} sum_{j=0}^{3} coefMat_{ij} (x0-x0Values_i)^{3-i} (x1-x1Values_j)^{3-j},
	///  for the region x0Values_i < x0 < x0Values_{i+1}, x1Values_j < x1 < x1Values_{j+1}  such that f(x0Values_a, x1Values_b) = yValues_{ab} where a={i,i+1}, b={j,j+1}. 
	///  1D piecewise polynomial interpolation methods are called to determine first derivatives and cross derivative at data points
	///  Note that the value of the cross derivative at {ij} is not "accurate" if yValues_{ij} = 0.
	/// </summary>
	public class BicubicSplineInterpolator : PiecewisePolynomialInterpolator2D
	{

	  private const double ERROR = 1.e-13;

	  private PiecewisePolynomialInterpolator[] _method;
	  private static DoubleMatrix INV_MAT;

	  static BicubicSplineInterpolator()
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] invMat = new double[16][16];
		double[][] invMat = RectangularArrays.ReturnRectangularDoubleArray(16, 16);
		invMat[0] = new double[] {1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[1] = new double[] {0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[2] = new double[] {-3.0, 3.0, 0.0, 0.0, -2.0, -1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[3] = new double[] {2.0, -2.0, 0.0, 0.0, 1.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[4] = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[5] = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0};
		invMat[6] = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -3.0, 3.0, 0.0, 0.0, -2.0, -1.0, 0.0, 0.0};
		invMat[7] = new double[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 2.0, -2.0, 0.0, 0.0, 1.0, 1.0, 0.0, 0.0};
		invMat[8] = new double[] {-3.0, 0.0, 3.0, 0.0, 0.0, 0.0, 0.0, 0.0, -2.0, 0.0, -1.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[9] = new double[] {0.0, 0.0, 0.0, 0.0, -3.0, 0.0, 3.0, 0.0, 0.0, 0.0, 0.0, 0.0, -2.0, 0.0, -1.0, 0.0};
		invMat[10] = new double[] {9.0, -9.0, -9.0, 9.0, 6.0, 3.0, -6.0, -3.0, 6.0, -6.0, 3.0, -3.0, 4.0, 2.0, 2.0, 1.0};
		invMat[11] = new double[] {-6.0, 6.0, 6.0, -6.0, -3.0, -3.0, 3.0, 3.0, -4.0, 4.0, -2.0, 2.0, -2.0, -2.0, -1.0, -1.0};
		invMat[12] = new double[] {2.0, 0.0, -2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 0.0};
		invMat[13] = new double[] {0.0, 0.0, 0.0, 0.0, 2.0, 0.0, -2.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 1.0, 0.0};
		invMat[14] = new double[] {-6.0, 6.0, 6.0, -6.0, -4.0, -2.0, 4.0, 2.0, -3.0, 3.0, -3.0, 3.0, -2.0, -1.0, -2.0, -1.0};
		invMat[15] = new double[] {4.0, -4.0, -4.0, 4.0, 2.0, 2.0, -2.0, -2.0, 2.0, -2.0, 2.0, -2.0, 1.0, 1.0, 1.0, 1.0};
		INV_MAT = DoubleMatrix.ofUnsafe(invMat);
	  }

	  /// <summary>
	  /// Constructor which can take different methods for x0 and x1. </summary>
	  /// <param name="method"> Choose 2 of <seealso cref="PiecewisePolynomialInterpolator"/> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public BicubicSplineInterpolator(final PiecewisePolynomialInterpolator[] method)
	  public BicubicSplineInterpolator(PiecewisePolynomialInterpolator[] method)
	  {
		ArgChecker.notNull(method, "method");
		ArgChecker.isTrue(method.Length == 2, "two methods should be chosen");

		_method = new PiecewisePolynomialInterpolator[2];
		for (int i = 0; i < 2; ++i)
		{
		  _method[i] = method[i];
		}
	  }

	  /// <summary>
	  /// Constructor using the same interpolation method for x0 and x1. </summary>
	  /// <param name="method"> <seealso cref="PiecewisePolynomialInterpolator"/> </param>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public BicubicSplineInterpolator(final PiecewisePolynomialInterpolator method)
	  public BicubicSplineInterpolator(PiecewisePolynomialInterpolator method)
	  {
		_method = new PiecewisePolynomialInterpolator[] {method, method};
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public PiecewisePolynomialResult2D interpolate(final double[] x0Values, final double[] x1Values, final double[][] yValues)
	  public override PiecewisePolynomialResult2D interpolate(double[] x0Values, double[] x1Values, double[][] yValues)
	  {

		ArgChecker.notNull(x0Values, "x0Values");
		ArgChecker.notNull(x1Values, "x1Values");
		ArgChecker.notNull(yValues, "yValues");

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData0 = x0Values.length;
		int nData0 = x0Values.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nData1 = x1Values.length;
		int nData1 = x1Values.Length;

		DoubleMatrix yValuesMatrix = DoubleMatrix.copyOf(yValues);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D func = new com.opengamma.strata.math.impl.function.PiecewisePolynomialFunction1D();
		PiecewisePolynomialFunction1D func = new PiecewisePolynomialFunction1D();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] diff0 = new double[nData1][nData0];
		double[][] diff0 = RectangularArrays.ReturnRectangularDoubleArray(nData1, nData0);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] diff1 = new double[nData0][nData1];
		double[][] diff1 = RectangularArrays.ReturnRectangularDoubleArray(nData0, nData1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] cross = new double[nData0][nData1];
		double[][] cross = RectangularArrays.ReturnRectangularDoubleArray(nData0, nData1);

		PiecewisePolynomialResult result0 = _method[0].interpolate(x0Values, OG_ALGEBRA.getTranspose(yValuesMatrix).toArray());
		diff0 = func.differentiate(result0, x0Values).toArray();

		PiecewisePolynomialResult result1 = _method[1].interpolate(x1Values, yValuesMatrix.toArray());
		diff1 = func.differentiate(result1, x1Values).toArray();

		const int order = 4;

		for (int i = 0; i < nData0; ++i)
		{
		  for (int j = 0; j < nData1; ++j)
		  {
			if (yValues[i][j] == 0.0)
			{
			  if (diff0[j][i] == 0.0)
			  {
				cross[i][j] = diff1[i][j];
			  }
			  else
			  {
				if (diff1[i][j] == 0.0)
				{
				  cross[i][j] = diff0[j][i];
				}
				else
				{
				  cross[i][j] = Math.Sign(diff0[j][i] * diff1[i][j]) * Math.Sqrt(Math.Abs(diff0[j][i] * diff1[i][j]));
				}
			  }
			}
			else
			{
			  cross[i][j] = diff0[j][i] * diff1[i][j] / yValues[i][j];
			}
		  }
		}

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: DoubleMatrix[][] coefMat = new DoubleMatrix[nData0 - 1][nData1 - 1];
		DoubleMatrix[][] coefMat = RectangularArrays.ReturnRectangularDoubleMatrixArray(nData0 - 1, nData1 - 1);
		for (int i = 0; i < nData0 - 1; ++i)
		{
		  for (int j = 0; j < nData1 - 1; ++j)
		  {
			double[] diffsVec = new double[16];
			for (int l = 0; l < 2; ++l)
			{
			  for (int m = 0; m < 2; ++m)
			  {
				diffsVec[l + 2 * m] = yValues[i + l][j + m];
			  }
			}
			for (int l = 0; l < 2; ++l)
			{
			  for (int m = 0; m < 2; ++m)
			  {
				diffsVec[4 + l + 2 * m] = diff0[j + m][i + l];
			  }
			}
			for (int l = 0; l < 2; ++l)
			{
			  for (int m = 0; m < 2; ++m)
			  {
				diffsVec[8 + l + 2 * m] = diff1[i + l][j + m];
			  }
			}
			for (int l = 0; l < 2; ++l)
			{
			  for (int m = 0; m < 2; ++m)
			  {
				diffsVec[12 + l + 2 * m] = cross[i + l][j + m];
			  }
			}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray diffs = com.opengamma.strata.collect.array.DoubleArray.copyOf(diffsVec);
			DoubleArray diffs = DoubleArray.copyOf(diffsVec);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray ansVec = ((com.opengamma.strata.collect.array.DoubleArray) OG_ALGEBRA.multiply(INV_MAT, diffs));
			DoubleArray ansVec = ((DoubleArray) OG_ALGEBRA.multiply(INV_MAT, diffs));

			double @ref = 0.0;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] coefMatTmp = new double[order][order];
			double[][] coefMatTmp = RectangularArrays.ReturnRectangularDoubleArray(order, order);
			for (int l = 0; l < order; ++l)
			{
			  for (int m = 0; m < order; ++m)
			  {
				coefMatTmp[order - l - 1][order - m - 1] = ansVec.get(l + m * (order)) / Math.Pow((x0Values[i + 1] - x0Values[i]), l) / Math.Pow((x1Values[j + 1] - x1Values[j]), m);
				ArgChecker.isFalse(double.IsNaN(coefMatTmp[order - l - 1][order - m - 1]), "Too large/small input");
				ArgChecker.isFalse(double.IsInfinity(coefMatTmp[order - l - 1][order - m - 1]), "Too large/small input");
				@ref += coefMatTmp[order - l - 1][order - m - 1] * Math.Pow((x0Values[i + 1] - x0Values[i]), l) * Math.Pow((x1Values[j + 1] - x1Values[j]), m);
			  }
			}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double bound = Math.max(Math.abs(ref) + Math.abs(yValues[i + 1][j + 1]), 0.1);
			double bound = Math.Max(Math.Abs(@ref) + Math.Abs(yValues[i + 1][j + 1]), 0.1);
			ArgChecker.isTrue(Math.Abs(@ref - yValues[i + 1][j + 1]) < ERROR * bound, "Input is too large/small or data points are too close");
			coefMat[i][j] = DoubleMatrix.copyOf(coefMatTmp);
		  }
		}

		return new PiecewisePolynomialResult2D(DoubleArray.copyOf(x0Values), DoubleArray.copyOf(x1Values), coefMat, new int[] {order, order});
	  }

	}

}
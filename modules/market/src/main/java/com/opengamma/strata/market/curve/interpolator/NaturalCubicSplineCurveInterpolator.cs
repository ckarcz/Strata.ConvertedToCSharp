using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.matrix.MatrixAlgebraFactory.OG_ALGEBRA;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MathException = com.opengamma.strata.math.MathException;
	using InverseTridiagonalMatrixCalculator = com.opengamma.strata.math.impl.linearalgebra.InverseTridiagonalMatrixCalculator;
	using TridiagonalMatrix = com.opengamma.strata.math.impl.linearalgebra.TridiagonalMatrix;

	/// <summary>
	/// Natural cubic spline interpolator.
	/// </summary>
	[Serializable]
	internal sealed class NaturalCubicSplineCurveInterpolator : CurveInterpolator
	{

	  /// <summary>
	  /// The interpolator name.
	  /// </summary>
	  public const string NAME = "NaturalCubicSpline";
	  /// <summary>
	  /// The interpolator instance.
	  /// </summary>
	  public static readonly CurveInterpolator INSTANCE = new NaturalCubicSplineCurveInterpolator();
	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Level below which the value is consider to be 0.
	  /// </summary>
	  private const double EPS = 1e-12;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private NaturalCubicSplineCurveInterpolator()
	  {
	  }

	  // resolve instance
	  private object readResolve()
	  {
		return INSTANCE;
	  }

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return NAME;
		  }
	  }

	  public BoundCurveInterpolator bind(DoubleArray xValues, DoubleArray yValues)
	  {
		return new Bound(xValues, yValues);
	  }

	  //-----------------------------------------------------------------------
	  public override string ToString()
	  {
		return NAME;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Bound interpolator.
	  /// </summary>
	  internal class Bound : AbstractBoundCurveInterpolator
	  {
		internal readonly double[] xValues;
		internal readonly double[] yValues;
		internal readonly int dataSize;
		internal readonly double leftFirstDev;
		internal readonly double rightFirstDev;
		internal readonly bool leftNatural;
		internal readonly bool rightNatural;

		internal Bound(DoubleArray xValues, DoubleArray yValues) : base(xValues, yValues)
		{
		  this.xValues = xValues.toArrayUnsafe();
		  this.yValues = yValues.toArrayUnsafe();
		  this.dataSize = xValues.size();
		  this.leftFirstDev = 0;
		  this.rightFirstDev = 0;
		  this.leftNatural = true;
		  this.rightNatural = true;
		}

		internal Bound(Bound @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight) : base(@base, extrapolatorLeft, extrapolatorRight)
		{
		  this.xValues = @base.xValues;
		  this.yValues = @base.yValues;
		  this.leftFirstDev = @base.leftFirstDev;
		  this.rightFirstDev = @base.rightFirstDev;
		  this.leftNatural = @base.leftNatural;
		  this.rightNatural = @base.rightNatural;
		  this.dataSize = xValues.Length;
		}

		//-------------------------------------------------------------------------
		internal static double[] calculateSecondDerivative(double[] xValues, double[] yValues, int dataSize, double leftFirstDev, double rightFirstDev, bool leftNatural, bool rightNatural)
		{

		  double[] deltaX = new double[dataSize - 1];
		  double[] deltaYOverDeltaX = new double[dataSize - 1];
		  double[] oneOverDeltaX = new double[dataSize - 1];

		  for (int i = 0; i < dataSize - 1; i++)
		  {
			deltaX[i] = xValues[i + 1] - xValues[i];
			oneOverDeltaX[i] = 1.0 / deltaX[i];
			deltaYOverDeltaX[i] = (yValues[i + 1] - yValues[i]) * oneOverDeltaX[i];
		  }
		  DoubleMatrix inverseTriDiag = getInverseTridiagonalMatrix(deltaX, leftNatural, rightNatural);
		  DoubleArray rhsVector = getRightVector(deltaYOverDeltaX, leftFirstDev, rightFirstDev, leftNatural, rightNatural);
		  return ((DoubleArray) OG_ALGEBRA.multiply(inverseTriDiag, rhsVector)).toArray();
		}

		internal static double[][] getSecondDerivativesSensitivities(double[] xValues, double[] yValues, int dataSize, bool leftNatural, bool rightNatural)
		{

		  double[] deltaX = new double[dataSize - 1];
		  double[] deltaYOverDeltaX = new double[dataSize - 1];
		  double[] oneOverDeltaX = new double[dataSize - 1];

		  for (int i = 0; i < dataSize - 1; i++)
		  {
			deltaX[i] = xValues[i + 1] - xValues[i];
			oneOverDeltaX[i] = 1.0 / deltaX[i];
			deltaYOverDeltaX[i] = (yValues[i + 1] - yValues[i]) * oneOverDeltaX[i];
		  }

		  DoubleMatrix inverseTriDiag = getInverseTridiagonalMatrix(deltaX, leftNatural, rightNatural);
		  DoubleMatrix rhsMatrix = getRightMatrix(oneOverDeltaX, leftNatural, rightNatural);
		  return ((DoubleMatrix) OG_ALGEBRA.multiply(inverseTriDiag, rhsMatrix)).toArray();
		}

		internal static DoubleMatrix getInverseTridiagonalMatrix(double[] deltaX, bool leftNatural, bool rightNatural)
		{
		  InverseTridiagonalMatrixCalculator invertor = new InverseTridiagonalMatrixCalculator();
		  int n = deltaX.Length + 1;
		  double[] a = new double[n];
		  double[] b = new double[n - 1];
		  double[] c = new double[n - 1];
		  for (int i = 1; i < n - 1; i++)
		  {
			a[i] = (deltaX[i - 1] + deltaX[i]) / 3.0;
			b[i] = deltaX[i] / 6.0;
			c[i - 1] = deltaX[i - 1] / 6.0;
		  }

		  // Boundary condition
		  if (leftNatural)
		  {
			a[0] = 1.0;
			b[0] = 0.0;
		  }
		  else
		  {
			a[0] = -deltaX[0] / 3.0;
			b[0] = deltaX[0] / 6.0;
		  }
		  if (rightNatural)
		  {
			a[n - 1] = 1.0;
			c[n - 2] = 0.0;
		  }
		  else
		  {
			a[n - 1] = deltaX[n - 2] / 3.0;
			c[n - 2] = deltaX[n - 2] / 6.0;
		  }

		  TridiagonalMatrix tridiagonal = new TridiagonalMatrix(a, b, c);
		  return invertor.apply(tridiagonal);
		}

		internal static DoubleArray getRightVector(double[] deltaYOverDeltaX, double leftFirstDev, double rightFirstDev, bool leftNatural, bool rightNatural)
		{

		  int n = deltaYOverDeltaX.Length + 1;
		  double[] res = new double[n];

		  for (int i = 1; i < n - 1; i++)
		  {
			res[i] = deltaYOverDeltaX[i] - deltaYOverDeltaX[i - 1];
		  }
		  if (!leftNatural)
		  {
			res[0] = leftFirstDev - deltaYOverDeltaX[0];
		  }

		  if (!rightNatural)
		  {
			res[n - 1] = rightFirstDev - deltaYOverDeltaX[n - 2];
		  }
		  return DoubleArray.copyOf(res);
		}

		internal static DoubleMatrix getRightMatrix(double[] oneOverDeltaX, bool leftNatural, bool rightNatural)
		{
		  int n = oneOverDeltaX.Length + 1;

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[n][n];
		  double[][] res = RectangularArrays.ReturnRectangularDoubleArray(n, n);
		  for (int i = 1; i < n - 1; i++)
		  {
			res[i][i - 1] = oneOverDeltaX[i - 1];
			res[i][i] = -oneOverDeltaX[i] - oneOverDeltaX[i - 1];
			res[i][i + 1] = oneOverDeltaX[i];
		  }
		  if (!leftNatural)
		  {
			res[0][0] = oneOverDeltaX[0];
			res[0][1] = -oneOverDeltaX[0];
		  }

		  if (!rightNatural)
		  {
			res[n - 1][n - 1] = -oneOverDeltaX[n - 2];
			res[n - 2][n - 2] = oneOverDeltaX[n - 2];
		  }
		  return DoubleMatrix.copyOf(res);
		}

		//-------------------------------------------------------------------------
		protected internal override double doInterpolate(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int low = lowerBoundIndex(xValue, xValues);
		  int high = low + 1;
		  int n = dataSize - 1;
		  if (low == n)
		  {
			return yValues[n];
		  }
		  double delta = xValues[high] - xValues[low];
		  if (Math.Abs(delta) < EPS)
		  {
			throw new MathException("x data points were not distinct");
		  }
		  double a = (xValues[high] - xValue) / delta;
		  double b = (xValue - xValues[low]) / delta;
		  double[] y2 = calculateSecondDerivative(xValues, yValues, dataSize, leftFirstDev, rightFirstDev, leftNatural, rightNatural);
		  return a * yValues[low] + b * yValues[high] + (a * (a * a - 1) * y2[low] + b * (b * b - 1) * y2[high]) * delta * delta / 6.0;
		}

		protected internal override double doFirstDerivative(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int low = lowerBoundIndex(xValue, xValues);
		  int high = low + 1;
		  int n = dataSize - 1;
		  if (low == n)
		  {
			low = n - 1;
			high = n;
		  }
		  double delta = xValues[high] - xValues[low];
		  if (Math.Abs(delta) < EPS)
		  {
			throw new MathException("x data points were not distinct");
		  }
		  double a = (xValues[high] - xValue) / delta;
		  double b = (xValue - xValues[low]) / delta;
		  double[] y2 = calculateSecondDerivative(xValues, yValues, dataSize, leftFirstDev, rightFirstDev, leftNatural, rightNatural);
		  return (yValues[high] - yValues[low]) / delta + ((-3.0 * a * a + 1.0) * y2[low] + (3.0 * b * b - 1.0) * y2[high]) * delta / 6.0;
		}

		protected internal override DoubleArray doParameterSensitivity(double xValue)
		{
		  // x-value is less than the x-value of the last node (lowerIndex < intervalCount)
		  int low = lowerBoundIndex(xValue, xValues);
		  double[] result = new double[dataSize];
		  if (low == dataSize - 1)
		  {
			result[dataSize - 1] = 1.0;
			return DoubleArray.ofUnsafe(result);
		  }
		  int high = low + 1;
		  double delta = xValues[high] - xValues[low];
		  double a = (xValues[high] - xValue) / delta;
		  double b = (xValue - xValues[low]) / delta;
		  double c = a * (a * a - 1) * delta * delta / 6.0;
		  double d = b * (b * b - 1) * delta * delta / 6.0;
		  double[][] y2Sensitivities = getSecondDerivativesSensitivities(xValues, yValues, dataSize, leftNatural, rightNatural);
		  for (int i = 0; i < dataSize; i++)
		  {
			result[i] = c * y2Sensitivities[low][i] + d * y2Sensitivities[high][i];
		  }
		  result[low] += a;
		  result[high] += b;

		  return DoubleArray.ofUnsafe(result);
		}

		public override BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{

		  return new Bound(this, extrapolatorLeft, extrapolatorRight);
		}
	  }

	}

}
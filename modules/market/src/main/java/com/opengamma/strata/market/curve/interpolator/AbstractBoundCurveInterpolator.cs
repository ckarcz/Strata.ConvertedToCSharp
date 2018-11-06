/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Abstract interpolator implementation.
	/// </summary>
	public abstract class AbstractBoundCurveInterpolator : BoundCurveInterpolator
	{
		public abstract BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight);

	  /// <summary>
	  /// Negative zero.
	  /// </summary>
	  private static long NEGATIVE_ZERO_BITS = Double.doubleToRawLongBits(-0d);

	  /// <summary>
	  /// The left extrapolator.
	  /// </summary>
	  private readonly BoundCurveExtrapolator extrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator.
	  /// </summary>
	  private readonly BoundCurveExtrapolator extrapolatorRight;
	  /// <summary>
	  /// The x-value of the first node.
	  /// </summary>
	  private readonly double firstXValue;
	  /// <summary>
	  /// The x-value of the last node.
	  /// </summary>
	  private readonly double lastXValue;
	  /// <summary>
	  /// The y-value of the last node.
	  /// </summary>
	  private readonly double lastYValue;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="xValues">  the x-values of the curve, must be sorted from low to high </param>
	  /// <param name="yValues">  the y-values of the curve </param>
	  protected internal AbstractBoundCurveInterpolator(DoubleArray xValues, DoubleArray yValues)
	  {
		ArgChecker.notNull(xValues, "xValues");
		ArgChecker.notNull(yValues, "yValues");
		int size = xValues.size();
		ArgChecker.isTrue(size == yValues.size(), "Curve node arrays must have same size");
		ArgChecker.isTrue(size > 1, "Curve node arrays must have at least two nodes");
		this.extrapolatorLeft = ExceptionCurveExtrapolator.INSTANCE;
		this.extrapolatorRight = ExceptionCurveExtrapolator.INSTANCE;
		this.firstXValue = xValues.get(0);
		this.lastXValue = xValues.get(size - 1);
		this.lastYValue = yValues.get(size - 1);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="base">  the base interpolator </param>
	  /// <param name="extrapolatorLeft">  the extrapolator for x-values on the left </param>
	  /// <param name="extrapolatorRight">  the extrapolator for x-values on the right </param>
	  protected internal AbstractBoundCurveInterpolator(AbstractBoundCurveInterpolator @base, BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
	  {

		this.extrapolatorLeft = ArgChecker.notNull(extrapolatorLeft, "extrapolatorLeft");
		this.extrapolatorRight = ArgChecker.notNull(extrapolatorRight, "extrapolatorRight");
		this.firstXValue = @base.firstXValue;
		this.lastXValue = @base.lastXValue;
		this.lastYValue = @base.lastYValue;
	  }

	  //-------------------------------------------------------------------------
	  public double interpolate(double xValue)
	  {
		if (xValue < firstXValue)
		{
		  return extrapolatorLeft.leftExtrapolate(xValue);
		}
		else if (xValue > lastXValue)
		{
		  return extrapolatorRight.rightExtrapolate(xValue);
		}
		else if (xValue == lastXValue)
		{
		  return lastYValue;
		}
		return doInterpolate(xValue);
	  }

	  /// <summary>
	  /// Method for subclasses to calculate the interpolated value.
	  /// <para>
	  /// Callers can assume that {@code xValue} is less than the x-value of the last node.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value </param>
	  /// <returns> the interpolated y-value </returns>
	  protected internal abstract double doInterpolate(double xValue);

	  /// <summary>
	  /// Method for {@code InterpolatorCurveExtrapolator} to calculate the interpolated value.
	  /// <para>
	  /// This is separated from <seealso cref="#doInterpolate(double)"/> to allow the check for x-values
	  /// beyond the last node to be treated separately.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  the x-value </param>
	  /// <returns> the interpolated y-value </returns>
	  protected internal virtual double doInterpolateFromExtrapolator(double xValue)
	  {
		// calling this method may fail on right extrapolation depending on the implementation
		// if it fails, then this method should be overridden to fix the problem
		return doInterpolate(xValue);
	  }

	  public double firstDerivative(double xValue)
	  {
		if (xValue < firstXValue)
		{
		  return extrapolatorLeft.leftExtrapolateFirstDerivative(xValue);
		}
		else if (xValue > lastXValue)
		{
		  return extrapolatorRight.rightExtrapolateFirstDerivative(xValue);
		}
		return doFirstDerivative(xValue);
	  }

	  /// <summary>
	  /// Method for subclasses to calculate the first derivative.
	  /// </summary>
	  /// <param name="xValue">  the x-value </param>
	  /// <returns> the first derivative </returns>
	  protected internal abstract double doFirstDerivative(double xValue);

	  public DoubleArray parameterSensitivity(double xValue)
	  {
		if (xValue < firstXValue)
		{
		  return extrapolatorLeft.leftExtrapolateParameterSensitivity(xValue);
		}
		else if (xValue > lastXValue)
		{
		  return extrapolatorRight.rightExtrapolateParameterSensitivity(xValue);
		}
		return doParameterSensitivity(xValue);
	  }

	  /// <summary>
	  /// Method for subclasses to calculate parameter sensitivity.
	  /// </summary>
	  /// <param name="xValue">  the x-value </param>
	  /// <returns> the parameter sensitivity </returns>
	  protected internal abstract DoubleArray doParameterSensitivity(double xValue);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the index of the last value in the input array which is lower than the specified value.
	  /// <para>
	  /// The following conditions must be true for this method to work correctly:
	  /// <ul>
	  ///   <li>{@code xValues} is sorted in ascending order</li>
	  ///   <li>{@code xValue} is greater or equal to the first element of {@code xValues}</li>
	  ///   <li>{@code xValue} is less than or equal to the last element of {@code xValues}</li>
	  /// </ul>
	  /// The returned value satisfies:
	  /// <pre>
	  ///   0 <= value < xValues.length
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// The x-values must not be NaN.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValue">  a value which is less than the last element in {@code xValues} </param>
	  /// <param name="xValues">  an array of values sorted in ascending order </param>
	  /// <returns> the index of the last value in {@code xValues} which is lower than {@code xValue} </returns>
	  protected internal static int lowerBoundIndex(double xValue, double[] xValues)
	  {
		// handle -zero, ensure same result as +zero
		if (Double.doubleToRawLongBits(xValue) == NEGATIVE_ZERO_BITS)
		{
		  return lowerBoundIndex(0d, xValues);
		}
		// manual inline of binary search to avoid NaN checks and negation
		int lo = 1;
		int hi = xValues.Length - 1;
		while (lo <= hi)
		{
		  // find the middle
		  int mid = (int)((uint)(lo + hi) >> 1);
		  double midVal = xValues[mid];
		  // decide where to search next
		  if (midVal < xValue)
		  {
			lo = mid + 1; // search top half
		  }
		  else if (midVal > xValue)
		  {
			hi = mid - 1; // search bottom half
		  }
		  else
		  {
			return mid; // found
		  }
		}
		return lo - 1;
	  }

	}

}
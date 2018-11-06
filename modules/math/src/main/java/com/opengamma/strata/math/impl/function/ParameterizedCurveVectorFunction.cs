/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// This is simply a <seealso cref="VectorFunction"/> backed by a <seealso cref="ParameterizedCurve"/>.
	/// </summary>
	public class ParameterizedCurveVectorFunction : VectorFunction
	{

	  private readonly double[] _samplePoints;
	  private readonly ParameterizedCurve _curve;

	  /// <summary>
	  /// Creates an instance with a sampled (parameterised) curve.
	  /// </summary>
	  /// <param name="samplePoints">  the points where we sample the curve </param>
	  /// <param name="curve">  a parameterised curve  </param>
	  public ParameterizedCurveVectorFunction(double[] samplePoints, ParameterizedCurve curve)
	  {
		ArgChecker.notEmpty(samplePoints, "samplePoints");
		ArgChecker.notNull(curve, "curve");
		_samplePoints = Arrays.copyOf(samplePoints, samplePoints.Length);
		_curve = curve;
	  }

	  //-------------------------------------------------------------------------
	  public override DoubleMatrix calculateJacobian(DoubleArray x)
	  {
		System.Func<double, DoubleArray> sense = _curve.getYParameterSensitivity(x);
		return DoubleMatrix.ofArrayObjects(LengthOfRange, LengthOfDomain, i => sense(_samplePoints[i]));
	  }

	  public override int LengthOfDomain
	  {
		  get
		  {
			return _curve.NumberOfParameters;
		  }
	  }

	  public override int LengthOfRange
	  {
		  get
		  {
			return _samplePoints.Length;
		  }
	  }

	  /// <summary>
	  /// Build a curve given the parameters, then return its value at the sample points.
	  /// </summary>
	  /// <param name="curveParameters">  the curve parameters </param>
	  /// <returns> the curve value at the sample points  </returns>
	  public override DoubleArray apply(DoubleArray curveParameters)
	  {
		System.Func<double, double> func = _curve.asFunctionOfArguments(curveParameters);
		return DoubleArray.of(_samplePoints.Length, i => func(_samplePoints[i]));
	  }

	}

}
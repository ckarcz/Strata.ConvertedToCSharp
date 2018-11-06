/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A provider of a <seealso cref="ParameterizedCurveVectorFunction"/>.
	/// </summary>
	public class ParameterizedCurveVectorFunctionProvider : DoublesVectorFunctionProvider
	{

	  private readonly ParameterizedCurve _pCurve;

	  /// <summary>
	  /// Creates an instance backed by a <seealso cref="ParameterizedCurve"/>.
	  /// </summary>
	  /// <param name="pCurve">  the parameterised curve  </param>
	  public ParameterizedCurveVectorFunctionProvider(ParameterizedCurve pCurve)
	  {
		ArgChecker.notNull(pCurve, "pCurve");
		_pCurve = pCurve;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Produces a <seealso cref="VectorFunction"/> which builds a <seealso cref="ParameterizedCurve"/> from the input vector
	  /// (treated as curve parameters), then samples the curve at the smaplePoints, to produce the output vector.
	  /// </summary>
	  /// <param name="samplePoints"> the points where we sample the curve </param>
	  /// <returns> a <seealso cref="ParameterizedCurveVectorFunction"/> </returns>
	  public override VectorFunction from(double[] samplePoints)
	  {
		return new ParameterizedCurveVectorFunction(samplePoints, _pCurve);
	  }

	}

}
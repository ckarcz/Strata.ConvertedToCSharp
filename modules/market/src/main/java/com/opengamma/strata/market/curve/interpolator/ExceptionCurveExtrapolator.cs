using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Extrapolator implementation that always throws an exception.
	/// <para>
	/// This is used to prevent extrapolation from being used.
	/// </para>
	/// </summary>
	[Serializable]
	internal sealed class ExceptionCurveExtrapolator : CurveExtrapolator, BoundCurveExtrapolator
	{

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// The extrapolator name.
	  /// </summary>
	  public const string NAME = "Exception";
	  /// <summary>
	  /// The extrapolator instance.
	  /// </summary>
	  public static readonly ExceptionCurveExtrapolator INSTANCE = new ExceptionCurveExtrapolator();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private ExceptionCurveExtrapolator()
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

	  public BoundCurveExtrapolator bind(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator)
	  {
		return this;
	  }

	  //-------------------------------------------------------------------------
	  public double leftExtrapolate(double xValue)
	  {
		throw new System.NotSupportedException("Extrapolation is not permitted");
	  }

	  public double leftExtrapolateFirstDerivative(double xValue)
	  {
		throw new System.NotSupportedException("Extrapolation is not permitted");
	  }

	  public DoubleArray leftExtrapolateParameterSensitivity(double xValue)
	  {
		throw new System.NotSupportedException("Extrapolation is not permitted");
	  }

	  //-------------------------------------------------------------------------
	  public double rightExtrapolate(double xValue)
	  {
		throw new System.NotSupportedException("Extrapolation is not permitted");
	  }

	  public double rightExtrapolateFirstDerivative(double xValue)
	  {
		throw new System.NotSupportedException("Extrapolation is not permitted");
	  }

	  public DoubleArray rightExtrapolateParameterSensitivity(double xValue)
	  {
		throw new System.NotSupportedException("Extrapolation is not permitted");
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return NAME;
	  }

	}

}
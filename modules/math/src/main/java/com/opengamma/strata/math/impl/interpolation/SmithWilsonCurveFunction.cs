using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Smith-Wilson curve function.
	/// <para>
	/// The curve represents the discount factor values as a function of time. 
	/// </para>
	/// <para>
	/// The curve is controlled by {@code omega}, {@code alpha} and {@code weights}: 
	/// {@code omega} is related to the ultimate forward rate (UFR) by {@code omega = log(1 + UFR)}, 
	/// the {@code alpha} parameter determines the rate of convergence to the UFR, 
	/// and the {@code weights} are the parameters to be calibrated to market data.
	/// </para>
	/// </summary>
	public class SmithWilsonCurveFunction
	{

	  /// <summary>
	  /// Default implementation with UFR = 4.2%
	  /// </summary>
	  public static readonly SmithWilsonCurveFunction DEFAULT = SmithWilsonCurveFunction.of(0.042);

	  /// <summary>
	  /// The omega parameter.
	  /// </summary>
	  private readonly double omega;

	  /// <summary>
	  /// Creates an instance with UFR.
	  /// </summary>
	  /// <param name="ufr">  the UFR </param>
	  /// <returns> the instance </returns>
	  public static SmithWilsonCurveFunction of(double ufr)
	  {
		return new SmithWilsonCurveFunction(ufr);
	  }

	  // private constructor
	  private SmithWilsonCurveFunction(double ufr)
	  {
		this.omega = Math.Log(1d + ufr);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the gap from the UFR at x value.
	  /// <para>
	  /// The {@code nodes} must be sorted in ascending order and coherent to {@code weights}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x value </param>
	  /// <param name="alpha">  the alpha parameter </param>
	  /// <param name="nodes">  the nodes </param>
	  /// <param name="weights">  the weights </param>
	  /// <returns> the gap </returns>
	  public static double gap(double x, double alpha, DoubleArray nodes, DoubleArray weights)
	  {
		int size = nodes.size();
		ArgChecker.isTrue(size == weights.size(), "nodes and weights must be the same size");
		double num = 1d;
		double den = 0d;
		for (int i = 0; i < size; ++i)
		{
		  num += alpha * nodes.get(i) * weights.get(i);
		  den += Math.Sinh(alpha * nodes.get(i)) * weights.get(i);
		}
		return alpha / Math.Abs(1d - num * Math.Exp(alpha * x) / den);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Evaluates the Smith-Wilson curve function at a x value. 
	  /// <para>
	  /// The {@code nodes} must be sorted in ascending order and coherent to {@code weights}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x value </param>
	  /// <param name="alpha">  the alpha parameter </param>
	  /// <param name="nodes">  the nodes </param>
	  /// <param name="weights">  the weights </param>
	  /// <returns> the value </returns>
	  public virtual double value(double x, double alpha, DoubleArray nodes, DoubleArray weights)
	  {
		int size = nodes.size();
		ArgChecker.isTrue(size == weights.size(), "nodes and weights must be the same size");
		double res = 1d;
		int bound = x < nodes.get(0) ? 0 : FunctionUtils.getLowerBoundIndex(nodes, x) + 1;
		for (int i = 0; i < bound; ++i)
		{
		  res += weights.get(i) * wilsonFunctionLeft(x, alpha, nodes.get(i));
		}
		for (int i = bound; i < size; ++i)
		{
		  res += weights.get(i) * wilsonFunctionRight(x, alpha, nodes.get(i));
		}
		res *= Math.Exp(-omega * x);
		return res;
	  }

	  /// <summary>
	  /// Computes the gradient of the Smith-Wilson curve function at a x value. 
	  /// <para>
	  /// The {@code nodes} must be sorted in ascending order and coherent to {@code weights}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x value </param>
	  /// <param name="alpha">  the alpha parameter </param>
	  /// <param name="nodes">  the nodes </param>
	  /// <param name="weights">  the weights </param>
	  /// <returns> the gradient </returns>
	  public virtual double firstDerivative(double x, double alpha, DoubleArray nodes, DoubleArray weights)
	  {
		int size = nodes.size();
		ArgChecker.isTrue(size == weights.size(), "nodes and weights must be the same size");
		double res = -omega;
		int bound = x < nodes.get(0) ? 0 : FunctionUtils.getLowerBoundIndex(nodes, x) + 1;
		for (int i = 0; i < bound; ++i)
		{
		  res += weights.get(i) * wilsonFunctionLeftDerivative(x, alpha, nodes.get(i));
		}
		for (int i = bound; i < size; ++i)
		{
		  res += weights.get(i) * wilsonFunctionRightDerivative(x, alpha, nodes.get(i));
		}
		res *= Math.Exp(-omega * x);
		return res;
	  }

	  /// <summary>
	  /// Computes the sensitivity of the Smith-Wilson curve function to weights parameters at a x value. 
	  /// <para>
	  /// The {@code nodes} must be sorted in ascending order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="x">  the x value </param>
	  /// <param name="alpha">  the alpha parameter </param>
	  /// <param name="nodes">  the nodes </param>
	  /// <returns> the value </returns>
	  public virtual DoubleArray parameterSensitivity(double x, double alpha, DoubleArray nodes)
	  {
		int size = nodes.size();
		double[] res = new double[size];
		double expOmega = Math.Exp(-omega * x);
		int bound = x < nodes.get(0) ? 0 : FunctionUtils.getLowerBoundIndex(nodes, x) + 1;
		for (int i = 0; i < bound; ++i)
		{
		  res[i] = expOmega * wilsonFunctionLeft(x, alpha, nodes.get(i));
		}
		for (int i = bound; i < size; ++i)
		{
		  res[i] = expOmega * wilsonFunctionRight(x, alpha, nodes.get(i));
		}
		return DoubleArray.ofUnsafe(res);
	  }

	  //-------------------------------------------------------------------------
	  // x < node
	  private double wilsonFunctionRight(double x, double alpha, double node)
	  {
		double alphaX = alpha * x;
		return alphaX - Math.Exp(-alpha * node) * Math.Sinh(alphaX);
	  }

	  // x < node, includes derivative of Math.exp(-omega * x)
	  private double wilsonFunctionRightDerivative(double x, double alpha, double node)
	  {
		double alphaX = alpha * x;
		double expAlphaNode = Math.Exp(-alpha * node);
		return -omega * (alphaX - expAlphaNode * Math.Sinh(alphaX)) + alpha * (1d - expAlphaNode * Math.Cosh(alphaX));
	  }

	  // x > node
	  private double wilsonFunctionLeft(double x, double alpha, double node)
	  {
		double alphaNode = alpha * node;
		return alphaNode - Math.Exp(-alpha * x) * Math.Sinh(alphaNode);
	  }

	  // x > node, includes derivative of Math.exp(-omega * x)
	  private double wilsonFunctionLeftDerivative(double x, double alpha, double node)
	  {
		double alphaNode = alpha * node;
		double expAlphaX = Math.Exp(-alpha * x);
		return -omega * (alphaNode - expAlphaX * Math.Sinh(alphaNode)) + alpha * expAlphaX * Math.Sinh(alphaNode);
	  }

	}

}
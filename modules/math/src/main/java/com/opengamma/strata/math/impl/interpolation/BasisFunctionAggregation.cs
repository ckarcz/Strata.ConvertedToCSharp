using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// 
	/// @param <T> The domain type of the function (e.g. Double, double[], DoubleArray etc)  </param>
	public class BasisFunctionAggregation<T> : System.Func<T, double>
	{

	  private readonly IList<System.Func<T, double>> _f;
	  private readonly double[] _w;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="functions">  the functions </param>
	  /// <param name="weights">  the weights </param>
	  public BasisFunctionAggregation(IList<System.Func<T, double>> functions, double[] weights)
	  {
		ArgChecker.notEmpty(functions, "no functions");
		ArgChecker.notNull(weights, "no weights");
		ArgChecker.isTrue(functions.Count == weights.Length);
		_f = functions;
		_w = weights.Clone();
	  }

	  public override double? apply(T x)
	  {
		ArgChecker.notNull(x, "x");
		double sum = 0;
		int n = _w.Length;
		for (int i = 0; i < n; i++)
		{
		  double temp = _f[i].apply(x);
		  if (temp != 0.0)
		  {
			sum += _w[i] * temp;
		  }
		}
		return sum;
	  }

	  /// <summary>
	  /// The sensitivity of the value at a point x to the weights of the basis functions.
	  /// </summary>
	  /// <param name="x"> value to be evaluated </param>
	  /// <returns> sensitivity w </returns>
	  public virtual DoubleArray weightSensitivity(T x)
	  {
		ArgChecker.notNull(x, "x");
		return DoubleArray.of(_w.Length, i => _f[i].apply(x));
	  }

	  /// <summary>
	  /// The value of the function at the given point and its sensitivity to the weights of the basis functions.
	  /// </summary>
	  /// <param name="x"> value to be evaluated </param>
	  /// <returns> value and weight sensitivity  </returns>
	  public virtual Pair<double, DoubleArray> valueAndWeightSensitivity(T x)
	  {
		ArgChecker.notNull(x, "x");
		int n = _w.Length;
		double sum = 0;
		double[] data = new double[n];
		for (int i = 0; i < n; i++)
		{
		  double temp = _f[i].apply(x);
		  if (temp != 0.0)
		  {
			sum += _w[i] * temp;
			data[i] = temp;
		  }
		}
		return Pair.of(sum, DoubleArray.ofUnsafe(data));
	  }

	}

}
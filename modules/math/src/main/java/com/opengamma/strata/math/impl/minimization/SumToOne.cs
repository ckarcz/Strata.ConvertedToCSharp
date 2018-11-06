using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// For a set of N-1 "fit" parameters, produces N "model" parameters that sum to one.
	/// </summary>
	public class SumToOne
	{

	  private const double TOL = 1e-9;
	  private static readonly IDictionary<int, int[][]> SETS = new Dictionary<int, int[][]>();

	  private readonly int[][] _set;
	  private readonly int _n;

	  /// <summary>
	  /// For a set of N-1 "fit" parameters, produces N "model" parameters that sum to one. </summary>
	  /// <param name="n"> The number of "model" parameters, N </param>
	  public SumToOne(int n)
	  {
		_set = getSet(n);
		_n = n;
	  }

	  /// <summary>
	  /// Transform from the N-1 "fit" parameters to the N "model" parameters. </summary>
	  /// <param name="fitParms"> The N-1 "fit" parameters </param>
	  /// <returns> The N "model" parameters </returns>
	  public virtual double[] transform(double[] fitParms)
	  {
		ArgChecker.isTrue(fitParms.Length == _n - 1, "length of fitParms is {}, but must be {} ", fitParms.Length, _n - 1);
		double[] s2 = new double[_n - 1];
		double[] c2 = new double[_n - 1];
		for (int j = 0; j < _n - 1; j++)
		{
		  double temp = Math.Sin(fitParms[j]);
		  temp *= temp;
		  s2[j] = temp;
		  c2[j] = 1.0 - temp;
		}

		double[] res = new double[_n];
		for (int i = 0; i < _n; i++)
		{
		  double prod = 1.0;
		  for (int j = 0; j < _n - 1; j++)
		  {
			if (_set[i][j] == 1)
			{
			  prod *= s2[j];
			}
			else if (_set[i][j] == -1)
			{
			  prod *= c2[j];
			}
		  }
		  res[i] = prod;
		}
		return res;
	  }

	  /// <summary>
	  /// Transform from the N-1 "fit" parameters to the N "model" parameters. </summary>
	  /// <param name="fitParms"> The N-1 "fit" parameters </param>
	  /// <returns> The N "model" parameters </returns>
	  public virtual DoubleArray transform(DoubleArray fitParms)
	  {
		return DoubleArray.copyOf(transform(fitParms.toArray()));
	  }

	  /// <summary>
	  /// Inverse transform from the N "model" parameters to the N-1 "fit" parameters.
	  /// Used mainly to find the start position of a optimisation routine.
	  /// </summary>
	  /// <param name="modelParms"> The N "model" parameters. <b>These must sum to one</b> </param>
	  /// <returns> The N-1 "fit" parameters </returns>
	  public virtual double[] inverseTransform(double[] modelParms)
	  {
		ArgChecker.isTrue(modelParms.Length == _n, "length of modelParms is {}, but must be {} ", modelParms.Length, _n);

		double[] res = new double[_n - 1];
		double[] cum = new double[_n + 1];

		double sum = 0.0;
		for (int i = 0; i < _n; i++)
		{
		  sum += modelParms[i];
		  cum[i + 1] = sum;
		}
		ArgChecker.isTrue(Math.Abs(sum - 1.0) < TOL, "sum of elements is {}. Must be 1.0", sum);

		cal(cum, 1.0, 0, _n, 0, res);

		for (int i = 0; i < _n - 1; i++)
		{
		  res[i] = Math.Asin(Math.Sqrt(res[i]));
		}
		return res;
	  }

	  /// <summary>
	  /// Inverse transform from the N "model" parameters to the N-1 "fit" parameters.
	  /// Used mainly to find the start position of a optimisation routine.
	  /// </summary>
	  /// <param name="modelParms"> The N "model" parameters. <b>These must sum to one</b> </param>
	  /// <returns> The N-1 "fit" parameters </returns>
	  public virtual DoubleArray inverseTransform(DoubleArray modelParms)
	  {
		return DoubleArray.copyOf(inverseTransform(modelParms.toArray()));
	  }

	  /// <summary>
	  /// The N by N-1 Jacobian matrix between the N "model" parameters (that sum to one) and the N-1 "fit" parameters. </summary>
	  /// <param name="fitParms">  The N-1 "fit" parameters </param>
	  /// <returns> The N by N-1 Jacobian matrix </returns>
	  public virtual double[][] jacobian(double[] fitParms)
	  {
		ArgChecker.isTrue(fitParms.Length == _n - 1, "length of fitParms is {}, but must be {} ", fitParms.Length, _n - 1);
		double[] sin = new double[_n - 1];
		double[] cos = new double[_n - 1];
		for (int j = 0; j < _n - 1; j++)
		{
		  sin[j] = Math.Sin(fitParms[j]);
		  cos[j] = Math.Cos(fitParms[j]);
		}

		double[] a = new double[_n];
		for (int i = 0; i < _n; i++)
		{
		  double prod = 1.0;
		  for (int j = 0; j < _n - 1; j++)
		  {
			if (_set[i][j] == 1)
			{
			  prod *= sin[j];
			}
			else if (_set[i][j] == -1)
			{
			  prod *= cos[j];
			}
		  }
		  a[i] = 2 * prod * prod;
		}

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[_n][_n - 1];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(_n, _n - 1);
		for (int i = 0; i < _n; i++)
		{
		  for (int j = 0; j < _n - 1; j++)
		  {
			if (_set[i][j] == 1 && a[i] != 0.0)
			{
			  res[i][j] = a[i] * cos[j] / sin[j];
			}
			else if (_set[i][j] == -1 && a[i] != 0.0)
			{
			  res[i][j] = -a[i] * sin[j] / cos[j];
			}
		  }
		}
		return res;
	  }

	  /// <summary>
	  /// The N by N-1 Jacobian matrix between the N "model" parameters (that sum to one) and the N-1 "fit" parameters. </summary>
	  /// <param name="fitParms">  The N-1 "fit" parameters </param>
	  /// <returns> The N by N-1 Jacobian matrix </returns>
	  public virtual DoubleMatrix jacobian(DoubleArray fitParms)
	  {
		return DoubleMatrix.copyOf(jacobian(fitParms.toArray()));
	  }

	  private void cal(double[] cum, double factor, int d, int n, int p1, double[] res)
	  {
		if (n == 1)
		{
		  return;
		}
		int n1 = n / 2;
		int n2 = n - n1;
		double s = (cum[p1 + n1] - cum[p1]) * factor;
		double c = 1 - s;
		res[d] = s;
		cal(cum, factor / s, d + 1, n1, p1, res);
		cal(cum, factor / c, d + n1, n2, p1 + n1, res);
	  }

	  protected internal static int[][] getSet(int n)
	  {
		ArgChecker.isTrue(n > 1, "need n>1");
		if (SETS.ContainsKey(n))
		{
		  return SETS[n];
		}
		int[][] res = new int[n][];
		switch (n)
		{
		  case 2:
			res[0] = new int[] {1};
			res[1] = new int[] {-1};
			break;
		  case 3:
			res[0] = new int[] {1, 0};
			res[1] = new int[] {-1, 1};
			res[2] = new int[] {-1, -1};
			break;
		  case 4:
			res[0] = new int[] {1, 1, 0};
			res[1] = new int[] {1, -1, 0};
			res[2] = new int[] {-1, 0, 1};
			res[3] = new int[] {-1, 0, -1};
			break;
		  default:
			int n1 = n / 2;
			int n2 = n - n1;
			int[][] set1 = getSet(n1);
			int[][] set2 = (n1 == n2 ? set1 : getSet(n2));
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: res = new int[n][n - 1];
			res = RectangularArrays.ReturnRectangularIntArray(n, n - 1);

			for (int i = 0; i < n1; i++)
			{
			  res[i][0] = 1;
			  Array.Copy(set1[i], 0, res[i], 1, n1 - 1);
			}
			for (int i = 0; i < n2; i++)
			{
			  res[i + n1][0] = -1;
			  Array.Copy(set2[i], 0, res[i + n1], n1, n2 - 1);
			}
		break;
		}
		SETS[n] = res;
		return res;
	  }

	}

}
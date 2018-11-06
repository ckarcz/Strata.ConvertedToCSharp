using System;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.util
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Computes the numerical difference between adjacent elements in vector. 
	/// </summary>
	public class Diff
	{

	  /// <summary>
	  /// Finds the numerical difference between value at position (i+1) and (i)
	  /// returning a vector of what would be needed to be added to the first (n-1) elements
	  /// of the original vector to get the original vector. 
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static double[] values(double[] v)
	  {
		ArgChecker.notNull(v, "v");
		int n = v.Length - 1;
		double[] tmp = new double[n];
		for (int i = 0; i < n; i++)
		{
		  tmp[i] = v[i + 1] - v[i];
		}
		return tmp;
	  }

	  /// <summary>
	  /// Finds the t^{th} numerical difference between value at position (i+1) and (i)
	  /// (effectively recurses #values "t" times).
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <param name="t">  the number of differences to be taken (t positive) </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static double[] values(double[] v, int t)
	  {
		ArgChecker.notNull(v, "v");
		ArgChecker.isTrue((t > -1), "Invalid number of differences requested, t must be positive or 0, but was {}", t);
		ArgChecker.isTrue((t < v.Length), "Invalid number of differences requested, 't' is greater than the number of " + "elements in 'v'. The given 't' was: {} and 'v' contains {} elements", t, v.Length);
		double[] tmp;
		if (t == 0)
		{ // no differencing done
		  tmp = new double[v.Length];
		  Array.Copy(v, 0, tmp, 0, v.Length);
		}
		else
		{
		  tmp = values(v);
		  for (int i = 0; i < t - 1; i++)
		  {
			tmp = values(tmp);
		  }
		}
		return tmp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the numerical difference between value at position (i+1) and (i)
	  /// returning a vector of what would be needed to be added to the first (n-1) elements
	  /// of the original vector to get the original vector. 
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static float[] values(float[] v)
	  {
		ArgChecker.notNull(v, "v");
		int n = v.Length - 1;
		float[] tmp = new float[n];
		for (int i = 0; i < n; i++)
		{
		  tmp[i] = v[i + 1] - v[i];
		}
		return tmp;
	  }

	  /// <summary>
	  /// Finds the t^{th} numerical difference between value at position (i+1) and (i)
	  /// (effectively recurses #values "t" times).
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <param name="t">  the number of differences to be taken (t positive) </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static float[] values(float[] v, int t)
	  {
		ArgChecker.notNull(v, "v");
		ArgChecker.isTrue((t > -1), "Invalid number of differences requested, t must be positive or 0, but was {}", t);
		ArgChecker.isTrue((t < v.Length), "Invalid number of differences requested, 't' is greater than the number of " + "elements in 'v'. The given 't' was: {} and 'v' contains {} elements", t, v.Length);
		float[] tmp;
		if (t == 0)
		{ // no differencing done
		  tmp = new float[v.Length];
		  Array.Copy(v, 0, tmp, 0, v.Length);
		}
		else
		{
		  tmp = values(v);
		  for (int i = 0; i < t - 1; i++)
		  {
			tmp = values(tmp);
		  }
		}
		return tmp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the numerical difference between value at position (i+1) and (i)
	  /// returning a vector of what would be needed to be added to the first (n-1) elements
	  /// of the original vector to get the original vector. 
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static int[] values(int[] v)
	  {
		ArgChecker.notNull(v, "v");
		int n = v.Length - 1;
		int[] tmp = new int[n];
		for (int i = 0; i < n; i++)
		{
		  tmp[i] = v[i + 1] - v[i];
		}
		return tmp;
	  }

	  /// <summary>
	  /// Finds the t^{th} numerical difference between value at position (i+1) and (i)
	  /// (effectively recurses #values "t" times).
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <param name="t">  the number of differences to be taken (t positive) </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static int[] values(int[] v, int t)
	  {
		ArgChecker.notNull(v, "v");
		ArgChecker.isTrue((t > -1), "Invalid number of differences requested, t must be positive or 0, but was {}", t);
		ArgChecker.isTrue((t < v.Length), "Invalid number of differences requested, 't' is greater than the number of " + "elements in 'v'. The given 't' was: {} and 'v' contains {} elements", t, v.Length);
		int[] tmp;
		if (t == 0)
		{ // no differencing done
		  tmp = new int[v.Length];
		  Array.Copy(v, 0, tmp, 0, v.Length);
		}
		else
		{
		  tmp = values(v);
		  for (int i = 0; i < t - 1; i++)
		  {
			tmp = values(tmp);
		  }
		}
		return tmp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the numerical difference between value at position (i+1) and (i)
	  /// returning a vector of what would be needed to be added to the first (n-1) elements
	  /// of the original vector to get the original vector. 
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static long[] values(long[] v)
	  {
		ArgChecker.notNull(v, "v");
		int n = v.Length - 1;
		long[] tmp = new long[n];
		for (int i = 0; i < n; i++)
		{
		  tmp[i] = v[i + 1] - v[i];
		}
		return tmp;
	  }

	  /// <summary>
	  /// Finds the t^{th} numerical difference between value at position (i+1) and (i)
	  /// (effectively recurses #values "t" times).
	  /// </summary>
	  /// <param name="v">  the vector </param>
	  /// <param name="t">  the number of differences to be taken (t positive) </param>
	  /// <returns> the numerical difference between adjacent elements in v </returns>
	  public static long[] values(long[] v, int t)
	  {
		ArgChecker.notNull(v, "v");
		ArgChecker.isTrue((t > -1), "Invalid number of differences requested, t must be positive or 0, but was {}", t);
		ArgChecker.isTrue((t < v.Length), "Invalid number of differences requested, 't' is greater than the number of " + "elements in 'v'. The given 't' was: {} and 'v' contains {} elements", t, v.Length);
		long[] tmp;
		if (t == 0)
		{ // no differencing done
		  tmp = new long[v.Length];
		  Array.Copy(v, 0, tmp, 0, v.Length);
		}
		else
		{
		  tmp = values(v);
		  for (int i = 0; i < t - 1; i++)
		  {
			tmp = values(tmp);
		  }
		}
		return tmp;
	  }

	}

}
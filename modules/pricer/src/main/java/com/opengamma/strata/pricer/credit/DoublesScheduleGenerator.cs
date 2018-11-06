using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// The Doubles schedule generator.
	/// <para>
	/// This is a utility for combining and truncating ascending arrays of doubles.
	/// The main application of this class is {@code IsdaCdsProductPricer}.
	/// </para>
	/// </summary>
	public class DoublesScheduleGenerator
	{

	  /// <summary>
	  /// The tolerance.
	  /// </summary>
	  private static readonly double TOL = 1d / 730d;

	  /// <summary>
	  /// Combines the discount curve nodes and credit curve nodes. 
	  /// <para>
	  /// The combined numbers are returned only if the values strictly between the specified start and end. 
	  /// </para>
	  /// <para>
	  /// The start and end values are added at the beginning and end of the list. 
	  /// If two times are very close (defined as less than half a day - 1/730 years different) only the smaller value is kept 
	  /// (with the exception of the end value which takes precedence). 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="start">  the first time in the list </param>
	  /// <param name="end">  the last time in the list </param>
	  /// <param name="discountCurveNodes">  the discount curve node set </param>
	  /// <param name="creditCurveNodes">  the credit curve node </param>
	  /// <returns> the combined list between start and end </returns>
	  public static DoubleArray getIntegrationsPoints(double start, double end, DoubleArray discountCurveNodes, DoubleArray creditCurveNodes)
	  {

		double[] set1 = truncateSetExclusive(start, end, discountCurveNodes.toArray());
		double[] set2 = truncateSetExclusive(start, end, creditCurveNodes.toArray());
		int n1 = set1.Length;
		int n2 = set2.Length;
		int n = n1 + n2;
		double[] set = new double[n];
		Array.Copy(set1, 0, set, 0, n1);
		Array.Copy(set2, 0, set, n1, n2);
		Arrays.sort(set);

		double[] temp = new double[n + 2];
		temp[0] = start;
		int pos = 0;
		for (int i = 0; i < n; i++)
		{
		  if (different(temp[pos], set[i]))
		  {
			temp[++pos] = set[i];
		  }
		}
		if (different(temp[pos], end))
		{
		  pos++;
		}
		temp[pos] = end; // add the end point (this may replace the last entry in temp if that is not significantly different)
		int resLength = pos + 1;
		return DoubleArray.copyOf(temp, 0, resLength);
	  }

	  private static bool different(double a, double b)
	  {
		return Math.Abs(a - b) > TOL;
	  }

	  // the resulting array can be modified
	  private static double[] truncateSetExclusive(double lower, double upper, double[] set)
	  {

		int n = set.Length;
		if (upper < set[0] || lower > set[n - 1])
		{
		  return new double[0];
		}

		int lIndex;
		if (lower < set[0])
		{
		  lIndex = 0;
		}
		else
		{
		  int temp = Arrays.binarySearch(set, lower);
		  lIndex = temp >= 0 ? temp + 1 : -(temp + 1);
		}

		int uIndex;
		if (upper > set[n - 1])
		{
		  uIndex = n;
		}
		else
		{
		  int temp = Arrays.binarySearch(set, lIndex, n, upper);
		  uIndex = temp >= 0 ? temp : -(temp + 1);
		}

		int m = uIndex - lIndex;
		if (m == n)
		{
		  return set;
		}

		double[] trunc = new double[m];
		Array.Copy(set, lIndex, trunc, 0, m);
		return trunc;
	  }

	  /// <summary>
	  /// Truncates an array of doubles. 
	  /// <para>
	  /// The number set is truncated so that it contains only the values between lower and upper, 
	  /// plus the values of lower and higher (as the first and last entry respectively). 
	  /// </para>
	  /// <para>
	  /// If no values met this criteria an array just containing lower and upper is returned. 
	  /// If the first (last) entry of set is too close to lower (upper), defined by TOL, 
	  /// the first (last) entry of set is replaced by lower (upper).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="lower">  the lower value </param>
	  /// <param name="upper">  the upper value </param>
	  /// <param name="set">  the numbers must be sorted in ascending order </param>
	  /// <returns> the truncated array  </returns>
	  public static DoubleArray truncateSetInclusive(double lower, double upper, DoubleArray set)
	  {

		double[] temp = truncateSetExclusive(lower, upper, set.toArray());
		int n = temp.Length;
		if (n == 0)
		{
		  return DoubleArray.of(lower, upper);
		}
		bool addLower = different(lower, temp[0]);
		bool addUpper = different(upper, temp[n - 1]);
		if (!addLower && !addUpper)
		{ // replace first and last entries of set
		  temp[0] = lower;
		  temp[n - 1] = upper;
		  return DoubleArray.ofUnsafe(temp);
		}

		int m = n + (addLower ? 1 : 0) + (addUpper ? 1 : 0);
		double[] res = new double[m];
		Array.Copy(temp, 0, res, (addLower ? 1 : 0), n);
		res[0] = lower;
		res[m - 1] = upper;

		return DoubleArray.ofUnsafe(res);
	  }

	}

}
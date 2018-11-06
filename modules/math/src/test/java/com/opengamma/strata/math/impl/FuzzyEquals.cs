using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */

namespace com.opengamma.strata.math.impl
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	// NOTE: This is from OG-Maths

	/// <summary>
	/// Tests for values being equal allowing for a level of floating point fuzz
	/// Based on the OG-Maths C++ fuzzy equals code .
	/// </summary>
	public class FuzzyEquals
	{

	  private static bool __LOCALDEBUG = false;
	  private static bool DEBUG = false;

	  private static double float64_eps;
	  private static double default_tolerance;
	  static FuzzyEquals()
	  {
		float64_eps = float64_t_machineEpsilon();
		default_tolerance = 10 * float64_eps;
	  }

	  /// <summary>
	  /// The logger instance
	  /// </summary>
	  private static Logger s_log = LoggerFactory.getLogger(typeof(FuzzyEquals));

	  /// <summary>
	  /// Gets machine precision for double precision floating point numbers on this machine. </summary>
	  /// <returns> machine precision for double precision floating point numbers on this machine. </returns>
	  public static double Eps
	  {
		  get
		  {
			return float64_eps;
		  }
	  }

	  /// <summary>
	  /// Get the default tolerance used in this class. </summary>
	  /// <returns> the default tolerance. </returns>
	  public static double DefaultTolerance
	  {
		  get
		  {
			return default_tolerance;
		  }
	  }

	  /// <summary>
	  /// Checks if two double precision floating point numbers are approximately "equal" </summary>
	  /// <param name="val1"> the first value </param>
	  /// <param name="val2"> the second value </param>
	  /// <param name="maxabserror"> determines the minimum threshold for "equal" in terms of the two numbers being very small in magnitude. </param>
	  /// <param name="maxrelerror"> determines the minimum threshold for "equal" in terms of the relative magnitude of the numbers.
	  /// i.e. invariant of the magnitude of the numbers what is the maximum level of magnitude difference acceptable. </param>
	  /// <returns> true if they are considered equal, else false </returns>
	  public static bool SingleValueFuzzyEquals(double val1, double val2, double maxabserror, double maxrelerror)
	  {

		if (__LOCALDEBUG)
		{
		  DEBUG_PRINT("FuzzyEquals: Comparing %24.16f and %24.16f\n", val1, val2);
		}

		if (double.IsNaN(val1))
		{
		  if (__LOCALDEBUG)
		  {
			DEBUG_PRINT("FuzzyEquals: Failed as value 1 is NaN\n");
		  }
		  return false;
		}

		if (double.IsNaN(val2))
		{
		  if (__LOCALDEBUG)
		  {
			DEBUG_PRINT("FuzzyEquals: Failed as value 2 is NaN\n");
		  }
		  return false;
		}

		// deal with infs in debug mode
		if (__LOCALDEBUG)
		{
		  if (DEBUG)
		  {
			bool val1isinf = double.IsInfinity(val1);
			bool val2isinf = double.IsInfinity(val2);
			if (val1isinf || val2isinf)
			{
			  if (val1isinf && val2isinf)
			  {
				if (Math.Sign(val2) == Math.Sign(val1))
				{
				  DEBUG_PRINT("FuzzyEquals: Inf Branch. Success as both inf of same sign\n");
				  return true;
				}
			  }

			  DEBUG_PRINT("FuzzyEquals: Inf Branch. Fail, non matching infs\n");
			  return false;
			}
		  }
		}

		if (val1 == val2)
		{
		  return true; // (+/-)inf compares == as does (+/-)0.e0
		}

		// check if they are below max absolute error bounds (i.e. small in the first place)
		double diff = (val1 - val2);
		if (maxabserror > Math.Abs(diff))
		{
		  if (__LOCALDEBUG)
		  {
			DEBUG_PRINT("FuzzyEquals: Match as below diff bounds. maxabserror > diff. (%24.16f >%24.16f)\n", maxabserror, Math.Abs(diff));
		  }
		  return true;
		}
		if (__LOCALDEBUG)
		{
		  DEBUG_PRINT("FuzzyEquals: Failed as diff > maxabserror. (%24.16f >  %24.16f)\n", Math.Abs(diff), maxabserror);
		}

		// check if they are within a relative error bound, div difference by largest of the 2
		double divisor = Math.Abs(val1) > Math.Abs(val2) ? val1 : val2;
		double relerror = Math.Abs(diff / divisor);
		if (maxrelerror > relerror)
		{
		  if (__LOCALDEBUG)
		  {
			DEBUG_PRINT("FuzzyEquals: Match as maxrelerror > relerror. (%24.16f >  %24.16f)\n", maxrelerror, relerror);
		  }
		  return true;
		}
		;

		if (__LOCALDEBUG)
		{
		  DEBUG_PRINT("FuzzyEquals: Fail as relerror > maxrelerror. (%24.16f >  %24.16f)\n", relerror, maxrelerror);
		}

		return false;
	  }

	  /// <summary>
	  /// Checks if two double precision floating point numbers are approximately "equal"
	  /// Default values are used for tolerances </summary>
	  /// <param name="val1"> the first value </param>
	  /// <param name="val2"> the second value </param>
	  /// <returns> true if they are considered equal, else false </returns>
	  public static bool SingleValueFuzzyEquals(double val1, double val2)
	  {
		return SingleValueFuzzyEquals(val1, val2, default_tolerance, default_tolerance);
	  }

	  /// <summary>
	  /// Checks if two double precision floating point arrays are approximately "equal"
	  /// Equal means the arrays have values the are considered fuzzy equals appearing in the same order
	  /// and the arrays the same length.
	  /// </summary>
	  /// <param name="arr1"> the first value </param>
	  /// <param name="arr2"> the second value </param>
	  /// <param name="maxabserror"> determines the minimum threshold for "equal" in terms of the two numbers being very small in magnitude. </param>
	  /// <param name="maxrelerror"> determines the minimum threshold for "equal" in terms of the relative magnitude of the numbers.
	  ///  i.e. invariant of the magnitude of the numbers what is the maximum level of magnitude difference acceptable. </param>
	  /// <returns> true if they are considered equal, else false </returns>
	  public static bool ArrayFuzzyEquals(double[] arr1, double[] arr2, double maxabserror, double maxrelerror)
	  {
		if (arr1.Length != arr2.Length)
		{
		  return false;
		}
		for (int i = 0; i < arr1.Length; i++)
		{
		  if (!SingleValueFuzzyEquals(arr1[i], arr2[i], maxabserror, maxrelerror))
		  {
			return false;
		  }
		}
		return true;
	  }

	  /// <summary>
	  /// Checks if two double precision floating point arrays are approximately "equal"
	  /// Equal means the arrays have values the are considered fuzzy equals appearing in the same order
	  /// and the arrays the same length.
	  /// Default values are used for tolerances.
	  /// </summary>
	  /// <param name="arr1"> the first value </param>
	  /// <param name="arr2"> the second value </param>
	  /// <returns> true if they are considered equal, else false </returns>
	  public static bool ArrayFuzzyEquals(double[] arr1, double[] arr2)
	  {
		return ArrayFuzzyEquals(arr1, arr2, default_tolerance, default_tolerance);
	  }

	  /// <summary>
	  /// Checks if two double precision floating point array of arrays are approximately "equal"
	  /// Equal means the arrays have values the are considered fuzzy equals appearing in the same order and the arrays the same dimension.
	  /// Default values are used for tolerances. </summary>
	  /// <param name="arr1"> the first value </param>
	  /// <param name="arr2"> the second value </param>
	  /// <returns> true if they are considered equal, else false </returns>
	  public static bool ArrayFuzzyEquals(double[][] arr1, double[][] arr2)
	  {
		return ArrayFuzzyEquals(arr1, arr2, default_tolerance, default_tolerance);
	  }

	  /// <summary>
	  /// Checks if two double precision floating point array of arrays are approximately "equal"
	  /// Equal means the arrays have values the are considered fuzzy equals appearing in the same order
	  /// and the arrays the same dimension.
	  /// </summary>
	  /// <param name="arr1"> the first value </param>
	  /// <param name="arr2"> the second value </param>
	  /// <param name="maxabserror"> determines the minimum threshold for "equal" in terms of the two numbers being very small in magnitude. </param>
	  /// <param name="maxrelerror"> determines the minimum threshold for "equal" in terms of the relative magnitude of the numbers.
	  ///  i.e. invariant of the magnitude of the numbers what is the maximum level of magnitude difference acceptable. </param>
	  /// <returns> true if they are considered equal, else false </returns>
	  public static bool ArrayFuzzyEquals(double[][] arr1, double[][] arr2, double maxabserror, double maxrelerror)
	  {
		if (arr1.Length != arr2.Length)
		{
		  return false;
		}
		int rows = arr1.Length;
		for (int k = 0; k < rows; k++)
		{
		  if (arr1[k].Length != arr2[k].Length)
		  {
			return false;
		  }
		  if (ArrayFuzzyEquals(arr1[k], arr2[k], maxabserror, maxrelerror) == false)
		  {
			return false;
		  }
		}
		return true;

	  }

	  /// <summary>
	  /// Debug helpers </summary>
	  /// <param name="str"> </param>

	  private static void DEBUG_PRINT(string str)
	  {
		s_log.debug(str);
	  }

	  private static void DEBUG_PRINT(string str, double a, double b)
	  {
		s_log.debug(string.format(Locale.ENGLISH, str, a, b));
	  }

	  private static double float64_t_machineEpsilon()
	  {
		double eps = 1.e0;
		while ((1.e0 + (eps / 2.e0)) != 1.e0)
		{
		  eps /= 2.e0;
		}
		return eps;
	  }

	}

}
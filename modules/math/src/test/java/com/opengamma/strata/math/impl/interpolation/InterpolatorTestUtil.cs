using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	/// 
	public abstract class InterpolatorTestUtil
	{

	  /// <summary>
	  /// Test double array with relative tolerance </summary>
	  /// <param name="message"> The message </param>
	  /// <param name="expected"> The expected values </param>
	  /// <param name="obtained"> The obtained values </param>
	  /// <param name="relativeTol"> The relative tolerance </param>
	  public static void assertArrayRelative(string message, double[] expected, double[] obtained, double relativeTol)
	  {
		int nData = expected.Length;
		assertEquals(message, nData, obtained.Length);
		for (int i = 0; i < nData; ++i)
		{
		  assertRelative(message, expected[i], obtained[i], relativeTol);
		}
	  }

	  /// <summary>
	  /// Test double with relative tolerance </summary>
	  /// <param name="message"> The message </param>
	  /// <param name="expected"> The expected value </param>
	  /// <param name="obtained"> The obtained value </param>
	  /// <param name="relativeTol"> The relative tolerance </param>
	  public static void assertRelative(string message, double expected, double obtained, double relativeTol)
	  {
		double @ref = Math.Max(Math.Abs(expected), 1.0);
		assertEquals(message, expected, obtained, @ref * relativeTol);
	  }
	}

}
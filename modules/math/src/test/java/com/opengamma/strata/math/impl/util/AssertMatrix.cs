/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.util
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Tests for whether vectors and matrices are equal within  some tolerance 
	/// </summary>
	public abstract class AssertMatrix
	{

	  /// <summary>
	  /// Assert that two vectors (as <seealso cref="DoubleArray"/>) equal concerning a delta. To be equal the vectors
	  /// must be the same length, and each element must match to within delta. 
	  /// If they are not equal an AssertionFailedError is thrown. </summary>
	  /// <param name="v1"> expected vector </param>
	  /// <param name="v2"> actual vector </param>
	  /// <param name="delta"> the allowed difference between the elements  </param>
	  public static void assertEqualsVectors(DoubleArray v1, DoubleArray v2, double delta)
	  {
		ArgChecker.notNull(v1, "v1");
		ArgChecker.notNull(v2, "v2");
		int size = v1.size();
		assertEquals("sizes:", size, v2.size());

		for (int i = 0; i < size; i++)
		{
		  assertEquals("", v1.get(i), v2.get(i), delta);
		}
	  }

	  /// <summary>
	  /// Assert that two matrices (as <seealso cref="DoubleMatrix"/>) equal concerning a delta. To be equal the matrices
	  /// must be the same size, and each element must match to within delta. 
	  /// If they are not equal an AssertionFailedError is thrown. </summary>
	  /// <param name="m1"> expected matrix </param>
	  /// <param name="m2"> actual matrix </param>
	  /// <param name="delta"> the allowed difference between the elements  </param>
	  public static void assertEqualsMatrix(DoubleMatrix m1, DoubleMatrix m2, double delta)
	  {
		ArgChecker.notNull(m1, "m1");
		ArgChecker.notNull(m2, "m2");
		int rows = m1.rowCount();
		int cols = m1.columnCount();
		assertEquals("Number of rows:", rows, m2.rowCount());
		assertEquals("Number of columns:", cols, m2.columnCount());
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < cols; j++)
		  {
			assertEquals("", m1.get(i, j), m2.get(i, j), delta);
		  }
		}
	  }

	}

}
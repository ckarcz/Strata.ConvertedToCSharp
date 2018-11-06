/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A collection of basic useful maths functions.
	/// </summary>
	public sealed class FunctionUtils
	{
	// CSOFF: JavadocMethod

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FunctionUtils()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the square of a number.
	  /// </summary>
	  /// <param name="x">  the number to square </param>
	  /// <returns> x*x </returns>
	  public static double square(double x)
	  {
		return x * x;
	  }

	  /// <summary>
	  /// Returns the cube of a number.
	  /// </summary>
	  /// <param name="x">  the number to cube </param>
	  /// <returns> x*x*x </returns>
	  public static double cube(double x)
	  {
		return x * x * x;
	  }

	  //-------------------------------------------------------------------------
	  public static int toTensorIndex(int[] indices, int[] dimensions)
	  {
		ArgChecker.notNull(indices, "indices");
		ArgChecker.notNull(dimensions, "dimensions");
		int dim = indices.Length;
		ArgChecker.isTrue(dim == dimensions.Length);
		int sum = 0;
		int product = 1;
		for (int i = 0; i < dim; i++)
		{
		  ArgChecker.isTrue(indices[i] < dimensions[i], "index out of bounds");
		  sum += indices[i] * product;
		  product *= dimensions[i];
		}
		return sum;
	  }

	  public static int[] fromTensorIndex(int index, int[] dimensions)
	  {
		ArgChecker.notNull(dimensions, "dimensions");
		int dim = dimensions.Length;
		int[] res = new int[dim];

		int product = 1;
		int[] products = new int[dim - 1];
		for (int i = 0; i < dim - 1; i++)
		{
		  product *= dimensions[i];
		  products[i] = product;
		}

		int a = index;
		for (int i = dim - 1; i > 0; i--)
		{
		  res[i] = a / products[i - 1];
		  a -= res[i] * products[i - 1];
		}
		res[0] = a;

		return res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Same behaviour as mathlab unique.
	  /// </summary>
	  /// <param name="in">  the input array </param>
	  /// <returns> a sorted array with no duplicates values </returns>
	  public static double[] unique(double[] @in)
	  {
		Arrays.sort(@in);
		int n = @in.Length;
		double[] temp = new double[n];
		temp[0] = @in[0];
		int count = 1;
		for (int i = 1; i < n; i++)
		{
		  if (@in[i].CompareTo(@in[i - 1]) != 0)
		  {
			temp[count++] = @in[i];
		  }
		}
		if (count == n)
		{
		  return temp;
		}
		return Arrays.copyOf(temp, count);
	  }

	  /// <summary>
	  /// Same behaviour as mathlab unique.
	  /// </summary>
	  /// <param name="in">  the input array </param>
	  /// <returns> a sorted array with no duplicates values </returns>
	  public static int[] unique(int[] @in)
	  {
		Arrays.sort(@in);
		int n = @in.Length;
		int[] temp = new int[n];
		temp[0] = @in[0];
		int count = 1;
		for (int i = 1; i < n; i++)
		{
		  if (@in[i] != @in[i - 1])
		  {
			temp[count++] = @in[i];
		  }
		}
		if (count == n)
		{
		  return temp;
		}
		return Arrays.copyOf(@in, count);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Find the index of a <b>sorted</b> set that is less than or equal to a given value.
	  /// If the given value is lower than the lowest member (i.e. the first)
	  /// of the set, zero is returned.  This uses Arrays.binarySearch.
	  /// </summary>
	  /// <param name="set">  a <b>sorted</b> array of numbers. </param>
	  /// <param name="value">  the value to search for </param>
	  /// <returns> the index in the array </returns>
	  public static int getLowerBoundIndex(DoubleArray set, double value)
	  {
		int n = set.size();
		if (value < set.get(0))
		{
		  return 0;
		}
		if (value > set.get(n - 1))
		{
		  return n - 1;
		}
		int index = Arrays.binarySearch(set.toArrayUnsafe(), value);
		if (index >= 0)
		{
		  // Fast break out if it's an exact match.
		  return index;
		}
		index = -(index + 1);
		index--;
		if (value == -0.0 && index < n - 1 && set.get(index + 1) == 0.0)
		{
		  ++index;
		}
		return index;
	  }

	}

}
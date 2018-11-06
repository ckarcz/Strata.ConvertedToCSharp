using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	using DoubleMath = com.google.common.math.DoubleMath;

	/// <summary>
	/// Contains utility methods for maths on double arrays.
	/// <para>
	/// This utility is used throughout the system when working with double arrays.
	/// </para>
	/// </summary>
	public sealed class DoubleArrayMath
	{

	  /// <summary>
	  /// An empty {@code double} array.
	  /// </summary>
	  public static readonly double[] EMPTY_DOUBLE_ARRAY = new double[0];
	  /// <summary>
	  /// An empty {@code Double} array.
	  /// </summary>
	  public static readonly double?[] EMPTY_DOUBLE_OBJECT_ARRAY = new double?[0];

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DoubleArrayMath()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a {@code double} array to a {@code Double} array.
	  /// </summary>
	  /// <param name="array">  the array to convert </param>
	  /// <returns> the converted array </returns>
	  public static double?[] toObject(double[] array)
	  {
		if (array.Length == 0)
		{
		  return EMPTY_DOUBLE_OBJECT_ARRAY;
		}
		double?[] result = new double?[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = Convert.ToDouble(array[i]);
		}
		return result;
	  }

	  /// <summary>
	  /// Converts a {@code Double} array to a {@code double} array.
	  /// <para>
	  /// Throws an exception if null is found.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to convert </param>
	  /// <returns> the converted array </returns>
	  /// <exception cref="NullPointerException"> if null found </exception>
	  public static double[] toPrimitive(double?[] array)
	  {
		if (array.Length == 0)
		{
		  return EMPTY_DOUBLE_ARRAY;
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i].Value;
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the sum total of all the elements in the array.
	  /// <para>
	  /// The input array is not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to sum </param>
	  /// <returns> the sum total of all the elements </returns>
	  public static double sum(double[] array)
	  {
		double total = 0d;
		for (int i = 0; i < array.Length; i++)
		{
		  total += array[i];
		}
		return total;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Applies an addition to each element in the array, returning a new array.
	  /// <para>
	  /// The result is always a new array. The input array is not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the input array, not mutated </param>
	  /// <param name="valueToAdd">  the value to add </param>
	  /// <returns> the resulting array </returns>
	  public static double[] applyAddition(double[] array, double valueToAdd)
	  {
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] + valueToAdd;
		}
		return result;
	  }

	  /// <summary>
	  /// Applies a multiplication to each element in the array, returning a new array.
	  /// <para>
	  /// The result is always a new array. The input array is not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the input array, not mutated </param>
	  /// <param name="valueToMultiplyBy">  the value to multiply by </param>
	  /// <returns> the resulting array </returns>
	  public static double[] applyMultiplication(double[] array, double valueToMultiplyBy)
	  {
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] * valueToMultiplyBy;
		}
		return result;
	  }

	  /// <summary>
	  /// Applies an operator to each element in the array, returning a new array.
	  /// <para>
	  /// The result is always a new array. The input array is not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the input array, not mutated </param>
	  /// <param name="operator">  the operator to use </param>
	  /// <returns> the resulting array </returns>
	  public static double[] apply(double[] array, System.Func<double, double> @operator)
	  {
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = @operator(array[i]);
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a constant value to each element in the array by mutation.
	  /// <para>
	  /// The input array is mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to mutate </param>
	  /// <param name="valueToAdd">  the value to add </param>
	  public static void mutateByAddition(double[] array, double valueToAdd)
	  {
		for (int i = 0; i < array.Length; i++)
		{
		  array[i] += valueToAdd;
		}
	  }

	  /// <summary>
	  /// Adds values in two arrays together, mutating the first array.
	  /// <para>
	  /// The arrays must be the same length. Each value in {@code arrayToAdd} is added to the value at the
	  /// corresponding index in {@code array}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to mutate </param>
	  /// <param name="arrayToAdd">  the array containing values to add </param>
	  public static void mutateByAddition(double[] array, double[] arrayToAdd)
	  {
		int length = DoubleArrayMath.length(array, arrayToAdd);
		for (int i = 0; i < length; i++)
		{
		  array[i] += arrayToAdd[i];
		}
	  }

	  /// <summary>
	  /// Multiplies each element in the array by a value by mutation.
	  /// <para>
	  /// The input array is mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to mutate </param>
	  /// <param name="valueToMultiplyBy">  the value to multiply by </param>
	  public static void mutateByMultiplication(double[] array, double valueToMultiplyBy)
	  {
		for (int i = 0; i < array.Length; i++)
		{
		  array[i] *= valueToMultiplyBy;
		}
	  }

	  /// <summary>
	  /// Multiplies values in two arrays, mutating the first array.
	  /// <para>
	  /// The arrays must be the same length. Each value in {@code array} is multiplied by the value at the
	  /// corresponding index in {@code arrayToMultiplyBy}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to mutate </param>
	  /// <param name="arrayToMultiplyBy">  the array containing values to multiply by </param>
	  public static void mutateByMultiplication(double[] array, double[] arrayToMultiplyBy)
	  {
		int length = DoubleArrayMath.length(array, arrayToMultiplyBy);
		for (int i = 0; i < length; i++)
		{
		  array[i] *= arrayToMultiplyBy[i];
		}
	  }

	  /// <summary>
	  /// Mutates each element in the array using an operator by mutation.
	  /// <para>
	  /// The input array is mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to mutate </param>
	  /// <param name="operator">  the operator to use to perform the mutation </param>
	  public static void mutate(double[] array, System.Func<double, double> @operator)
	  {
		for (int i = 0; i < array.Length; i++)
		{
		  array[i] = @operator(array[i]);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines two arrays, returning an array where each element is the sum of the two matching inputs.
	  /// <para>
	  /// Each element in the result will be the sum of the matching index in the two input arrays.
	  /// The two input arrays must have the same length.
	  /// </para>
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  double[] array1 = {1, 5, 9};
	  ///  double[] array2 = {2, 3, 2};
	  ///  double[] result = DoubleArrayMath.combineByAddition(array1, array2);
	  ///  // result contains {3, 8, 11}
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// The result is always a new array. The input arrays are not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array1">  the first array </param>
	  /// <param name="array2">  the second array </param>
	  /// <returns> an array combining the two input arrays using the plus operator </returns>
	  public static double[] combineByAddition(double[] array1, double[] array2)
	  {
		return combine(array1, array2, (a, b) => a + b);
	  }

	  /// <summary>
	  /// Combines two arrays, returning an array where each element is the multiplication of the two matching inputs.
	  /// <para>
	  /// Each element in the result will be the multiplication of the matching index in the two input arrays.
	  /// The two input arrays must have the same length.
	  /// </para>
	  /// <para>
	  /// For example:
	  /// <pre>
	  ///  double[] array1 = {1, 5, 9};
	  ///  double[] array2 = {2, 3, 4};
	  ///  double[] result = DoubleArrayMath.combineByMultiplication(array1, array2);
	  ///  // result contains {2, 15, 36}
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// The result is always a new array. The input arrays are not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array1">  the first array </param>
	  /// <param name="array2">  the second array </param>
	  /// <returns> an array combining the two input arrays using the multiply operator </returns>
	  public static double[] combineByMultiplication(double[] array1, double[] array2)
	  {
		return combine(array1, array2, (a, b) => a * b);
	  }

	  /// <summary>
	  /// Combines two arrays, returning an array where each element is the combination of the two matching inputs.
	  /// <para>
	  /// Each element in the result will be the combination of the matching index in the two
	  /// input arrays using the operator. The two input arrays must have the same length.
	  /// </para>
	  /// <para>
	  /// The result is always a new array. The input arrays are not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array1">  the first array </param>
	  /// <param name="array2">  the second array </param>
	  /// <param name="operator">  the operator to use when combining values </param>
	  /// <returns> an array combining the two input arrays using the operator </returns>
	  public static double[] combine(double[] array1, double[] array2, System.Func<double, double, double> @operator)
	  {
		int length = DoubleArrayMath.length(array1, array2);
		double[] result = new double[length];
		for (int i = 0; i < length; i++)
		{
		  result[i] = @operator(array1[i], array2[i]);
		}
		return result;
	  }

	  /// <summary>
	  /// Combines two arrays, returning an array where each element is the combination of the two matching inputs.
	  /// <para>
	  /// Each element in the result will be the combination of the matching index in the two
	  /// input arrays using the operator.
	  /// The result will have the length of the longest of the two inputs.
	  /// Where one array is longer than the other, the values from the longer array will be used.
	  /// </para>
	  /// <para>
	  /// The result is always a new array. The input arrays are not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array1">  the first array </param>
	  /// <param name="array2">  the second array </param>
	  /// <param name="operator">  the operator to use when combining values </param>
	  /// <returns> an array combining the two input arrays using the operator </returns>
	  public static double[] combineLenient(double[] array1, double[] array2, System.Func<double, double, double> @operator)
	  {
		int len1 = array1.Length;
		int len2 = array2.Length;
		if (len1 == len2)
		{
		  return combine(array1, array2, @operator);
		}
		int size = Math.Max(len1, len2);
		double[] result = new double[size];
		for (int i = 0; i < size; i++)
		{
		  if (i < len1)
		  {
			if (i < len2)
			{
			  result[i] = @operator(array1[i], array2[i]);
			}
			else
			{
			  result[i] = array1[i];
			}
		  }
		  else
		  {
			result[i] = array2[i];
		  }
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares each element in the array to zero within a tolerance.
	  /// <para>
	  /// An empty array returns true;
	  /// </para>
	  /// <para>
	  /// The input array is not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to check </param>
	  /// <param name="tolerance">  the tolerance to use </param>
	  /// <returns> true if the array is effectively equal to zero </returns>
	  public static bool fuzzyEqualsZero(double[] array, double tolerance)
	  {
		for (int i = 0; i < array.Length; i++)
		{
		  if (!DoubleMath.fuzzyEquals(array[i], 0, tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  /// <summary>
	  /// Compares each element in the first array to the matching index in the second array within a tolerance.
	  /// <para>
	  /// If the arrays differ in length, false is returned.
	  /// </para>
	  /// <para>
	  /// The input arrays are not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array1">  the first array to check </param>
	  /// <param name="array2">  the second array to check </param>
	  /// <param name="tolerance">  the tolerance to use </param>
	  /// <returns> true if the arrays are effectively equal </returns>
	  public static bool fuzzyEquals(double[] array1, double[] array2, double tolerance)
	  {
		if (array1.Length != array2.Length)
		{
		  return false;
		}
		for (int i = 0; i < array1.Length; i++)
		{
		  if (!DoubleMath.fuzzyEquals(array1[i], array2[i], tolerance))
		  {
			return false;
		  }
		}
		return true;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sorts the two arrays, retaining the associated values with the sorted keys.
	  /// <para>
	  /// The two arrays must be the same size and represent a pair of key to value.
	  /// The sort order is determined by the array of keys.
	  /// The position of each value is changed to match that of the sorted keys.
	  /// </para>
	  /// <para>
	  /// The input arrays are mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="keys">  the array of keys to sort </param>
	  /// <param name="values">  the array of associated values to retain </param>
	  public static void sortPairs(double[] keys, double[] values)
	  {
		int len1 = keys.Length;
		if (len1 != values.Length)
		{
		  throw new System.ArgumentException("Arrays cannot be sorted as they differ in length");
		}
		dualArrayQuickSort(keys, values, 0, len1 - 1);
	  }

	  private static void dualArrayQuickSort(double[] keys, double[] values, int left, int right)
	  {
		if (right > left)
		{
		  int pivot = (left + right) >> 1;
		  int pivotNewIndex = partition(keys, values, left, right, pivot);
		  dualArrayQuickSort(keys, values, left, pivotNewIndex - 1);
		  dualArrayQuickSort(keys, values, pivotNewIndex + 1, right);
		}
	  }

	  private static int partition(double[] keys, double[] values, int left, int right, int pivot)
	  {
		double pivotValue = keys[pivot];
		swap(keys, values, pivot, right);
		int storeIndex = left;
		for (int i = left; i < right; i++)
		{
		  if (keys[i] <= pivotValue)
		  {
			swap(keys, values, i, storeIndex);
			storeIndex++;
		  }
		}
		swap(keys, values, storeIndex, right);
		return storeIndex;
	  }

	  private static void swap(double[] keys, double[] values, int first, int second)
	  {
		double t = keys[first];
		keys[first] = keys[second];
		keys[second] = t;
		t = values[first];
		values[first] = values[second];
		values[second] = t;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sorts the two arrays, retaining the associated values with the sorted keys.
	  /// <para>
	  /// The two arrays must be the same size and represent a pair of key to value.
	  /// The sort order is determined by the array of keys.
	  /// The position of each value is changed to match that of the sorted keys.
	  /// </para>
	  /// <para>
	  /// The input arrays are mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <V>  the type of the values </param>
	  /// <param name="keys">  the array of keys to sort </param>
	  /// <param name="values">  the array of associated values to retain </param>
	  public static void sortPairs<V>(double[] keys, V[] values)
	  {
		int len1 = keys.Length;
		if (len1 != values.Length)
		{
		  throw new System.ArgumentException("Arrays cannot be sorted as they differ in length");
		}
		dualArrayQuickSort(keys, values, 0, len1 - 1);
	  }

	  private static void dualArrayQuickSort<T>(double[] keys, T[] values, int left, int right)
	  {
		if (right > left)
		{
		  int pivot = (left + right) >> 1;
		  int pivotNewIndex = partition(keys, values, left, right, pivot);
		  dualArrayQuickSort(keys, values, left, pivotNewIndex - 1);
		  dualArrayQuickSort(keys, values, pivotNewIndex + 1, right);
		}
	  }

	  private static int partition<T>(double[] keys, T[] values, int left, int right, int pivot)
	  {
		double pivotValue = keys[pivot];
		swap(keys, values, pivot, right);
		int storeIndex = left;
		for (int i = left; i < right; i++)
		{
		  if (keys[i] <= pivotValue)
		  {
			swap(keys, values, i, storeIndex);
			storeIndex++;
		  }
		}
		swap(keys, values, storeIndex, right);
		return storeIndex;
	  }

	  private static void swap<T>(double[] keys, T[] values, int first, int second)
	  {
		double x = keys[first];
		keys[first] = keys[second];
		keys[second] = x;
		T t = values[first];
		values[first] = values[second];
		values[second] = t;
	  }

	  /// <summary>
	  /// Return the array lengths if they are the same, otherwise throws an {@code IllegalArgumentException}.
	  /// </summary>
	  private static int length(double[] array1, double[] array2)
	  {
		int len1 = array1.Length;
		int len2 = array2.Length;
		if (len1 != len2)
		{
		  throw new System.ArgumentException("Arrays cannot be combined as they differ in length");
		}
		return len1;
	  }

	}

}
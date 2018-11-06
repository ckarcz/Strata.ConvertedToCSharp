using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.array
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using Property = org.joda.beans.Property;
	using PropertyStyle = org.joda.beans.PropertyStyle;
	using BasicImmutableBeanBuilder = org.joda.beans.impl.BasicImmutableBeanBuilder;
	using BasicMetaBean = org.joda.beans.impl.BasicMetaBean;
	using BasicMetaProperty = org.joda.beans.impl.BasicMetaProperty;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Ints = com.google.common.primitives.Ints;
	using IntIntConsumer = com.opengamma.strata.collect.function.IntIntConsumer;
	using IntTernaryOperator = com.opengamma.strata.collect.function.IntTernaryOperator;

	/// <summary>
	/// An immutable array of {@code int} values.
	/// <para>
	/// This provides functionality similar to <seealso cref="List"/> but for {@code int[]}.
	/// </para>
	/// <para>
	/// In mathematical terms, this is a vector, or one-dimensional matrix.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class IntArray : Matrix, ImmutableBean
	{

	  /// <summary>
	  /// An empty int array.
	  /// </summary>
	  private static readonly int[] EMPTY_INT_ARRAY = new int[0];
	  /// <summary>
	  /// An empty array.
	  /// </summary>
	  public static readonly IntArray EMPTY = new IntArray(EMPTY_INT_ARRAY);

	  /// <summary>
	  /// Serialization version.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  static IntArray()
	  {
		MetaBean.register(Meta.META);
	  }

	  /// <summary>
	  /// The underlying array of ints.
	  /// </summary>
	  private readonly int[] array;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty immutable array.
	  /// </summary>
	  /// <returns> the empty immutable array </returns>
	  public static IntArray of()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an immutable array with a single value.
	  /// </summary>
	  /// <param name="value">  the single value </param>
	  /// <returns> an array containing the specified value </returns>
	  public static IntArray of(int value)
	  {
		return new IntArray(new int[] {value});
	  }

	  /// <summary>
	  /// Obtains an immutable array with two values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2)
	  {
		return new IntArray(new int[] {value1, value2});
	  }

	  /// <summary>
	  /// Obtains an immutable array with three values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3)
	  {
		return new IntArray(new int[] {value1, value2, value3});
	  }

	  /// <summary>
	  /// Obtains an immutable array with four values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3, int value4)
	  {
		return new IntArray(new int[] {value1, value2, value3, value4});
	  }

	  /// <summary>
	  /// Obtains an immutable array with five values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <param name="value5">  the fifth value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3, int value4, int value5)
	  {
		return new IntArray(new int[] {value1, value2, value3, value4, value5});
	  }

	  /// <summary>
	  /// Obtains an immutable array with six values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <param name="value5">  the fifth value </param>
	  /// <param name="value6">  the sixth value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3, int value4, int value5, int value6)
	  {
		return new IntArray(new int[] {value1, value2, value3, value4, value5, value6});
	  }

	  /// <summary>
	  /// Obtains an immutable array with seven values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <param name="value5">  the fifth value </param>
	  /// <param name="value6">  the sixth value </param>
	  /// <param name="value7">  the seventh value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3, int value4, int value5, int value6, int value7)
	  {
		return new IntArray(new int[] {value1, value2, value3, value4, value5, value6, value7});
	  }

	  /// <summary>
	  /// Obtains an immutable array with eight values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <param name="value5">  the fifth value </param>
	  /// <param name="value6">  the sixth value </param>
	  /// <param name="value7">  the seventh value </param>
	  /// <param name="value8">  the eighth value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8)
	  {
		return new IntArray(new int[] {value1, value2, value3, value4, value5, value6, value7, value8});
	  }

	  /// <summary>
	  /// Obtains an immutable array with more than eight values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <param name="value5">  the fifth value </param>
	  /// <param name="value6">  the sixth value </param>
	  /// <param name="value7">  the seventh value </param>
	  /// <param name="value8">  the eighth value </param>
	  /// <param name="otherValues">  the other values </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray of(int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8, params int[] otherValues)
	  {
		int[] @base = new int[otherValues.Length + 8];
		@base[0] = value1;
		@base[1] = value2;
		@base[2] = value3;
		@base[3] = value4;
		@base[4] = value5;
		@base[5] = value6;
		@base[6] = value7;
		@base[7] = value8;
		Array.Copy(otherValues, 0, @base, 8, otherValues.Length);
		return new IntArray(@base);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with entries filled using a function.
	  /// <para>
	  /// The function is passed the array index and returns the value for that index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <param name="valueFunction">  the function used to populate the value </param>
	  /// <returns> an array initialized using the function </returns>
	  public static IntArray of(int size, System.Func<int, int> valueFunction)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
		int[] array = new int[size];
		Arrays.setAll(array, valueFunction);
		return new IntArray(array);
	  }

	  /// <summary>
	  /// Obtains an instance with entries filled from a stream.
	  /// <para>
	  /// The stream is converted to an array using <seealso cref="IntStream#toArray()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="stream">  the stream of elements </param>
	  /// <returns> an array initialized using the stream </returns>
	  public static IntArray of(IntStream stream)
	  {
		return ofUnsafe(stream.toArray());
	  }

	  /// <summary>
	  /// Obtains an instance by wrapping an array.
	  /// <para>
	  /// This method is inherently unsafe as it relies on good behavior by callers.
	  /// Callers must never make any changes to the passed in array after calling this method.
	  /// Doing so would violate the immutability of this class.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to assign </param>
	  /// <returns> an array instance wrapping the specified array </returns>
	  public static IntArray ofUnsafe(int[] array)
	  {
		if (array.Length == 0)
		{
		  return EMPTY;
		}
		return new IntArray(array);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a collection of {@code Integer}.
	  /// <para>
	  /// The order of the values in the returned array is the order in which elements are returned
	  /// from the iterator of the collection.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="collection">  the collection to initialize from </param>
	  /// <returns> an array containing the values from the collection in iteration order </returns>
	  public static IntArray copyOf(ICollection<int> collection)
	  {
		if (collection.Count == 0)
		{
		  return EMPTY;
		}
		if (collection is ImmList)
		{
		  return ((ImmList) collection).underlying;
		}
		return new IntArray(Ints.toArray(collection));
	  }

	  /// <summary>
	  /// Obtains an instance from an array of {@code int}.
	  /// <para>
	  /// The input array is copied and not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to copy, cloned </param>
	  /// <returns> an array containing the specified values </returns>
	  public static IntArray copyOf(int[] array)
	  {
		if (array.Length == 0)
		{
		  return EMPTY;
		}
		return new IntArray(array.Clone());
	  }

	  /// <summary>
	  /// Obtains an instance by copying part of an array.
	  /// <para>
	  /// The input array is copied and not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to copy </param>
	  /// <param name="fromIndex">  the offset from the start of the array </param>
	  /// <returns> an array containing the specified values </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public static IntArray copyOf(int[] array, int fromIndex)
	  {
		return copyOf(array, fromIndex, array.Length);
	  }

	  /// <summary>
	  /// Obtains an instance by copying part of an array.
	  /// <para>
	  /// The input array is copied and not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to copy </param>
	  /// <param name="fromIndexInclusive">  the start index of the input array to copy from </param>
	  /// <param name="toIndexExclusive">  the end index of the input array to copy to </param>
	  /// <returns> an array containing the specified values </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public static IntArray copyOf(int[] array, int fromIndexInclusive, int toIndexExclusive)
	  {
		if (fromIndexInclusive > array.Length)
		{
		  throw new System.IndexOutOfRangeException("Array index out of bounds: " + fromIndexInclusive + " > " + array.Length);
		}
		if (toIndexExclusive > array.Length)
		{
		  throw new System.IndexOutOfRangeException("Array index out of bounds: " + toIndexExclusive + " > " + array.Length);
		}
		if ((toIndexExclusive - fromIndexInclusive) == 0)
		{
		  return EMPTY;
		}
		return new IntArray(Arrays.copyOfRange(array, fromIndexInclusive, toIndexExclusive));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with all entries equal to the zero.
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <returns> an array filled with zeroes </returns>
	  public static IntArray filled(int size)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
		return new IntArray(new int[size]);
	  }

	  /// <summary>
	  /// Obtains an instance with all entries equal to the same value.
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <param name="value">  the value of all the elements </param>
	  /// <returns> an array filled with the specified value </returns>
	  public static IntArray filled(int size, int value)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
		int[] array = new int[size];
		Arrays.fill(array, value);
		return new IntArray(array);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from a {@code int[}.
	  /// </summary>
	  /// <param name="array">  the array, assigned not cloned </param>
	  private IntArray(int[] array)
	  {
		this.array = array;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of dimensions of this array.
	  /// </summary>
	  /// <returns> one </returns>
	  public int dimensions()
	  {
		return 1;
	  }

	  /// <summary>
	  /// Gets the size of this array.
	  /// </summary>
	  /// <returns> the array size, zero or greater </returns>
	  public int size()
	  {
		return array.Length;
	  }

	  /// <summary>
	  /// Checks if this array is empty.
	  /// </summary>
	  /// <returns> true if empty </returns>
	  public bool Empty
	  {
		  get
		  {
			return array.Length == 0;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value at the specified index in this array.
	  /// </summary>
	  /// <param name="index">  the zero-based index to retrieve </param>
	  /// <returns> the value at the index </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public int get(int index)
	  {
		return array[index];
	  }

	  /// <summary>
	  /// Checks if this array contains the specified value.
	  /// </summary>
	  /// <param name="value">  the value to find </param>
	  /// <returns> true if the value is contained in this array </returns>
	  public bool contains(int value)
	  {
		return Ints.contains(array, value);
	  }

	  /// <summary>
	  /// Find the index of the first occurrence of the specified value.
	  /// </summary>
	  /// <param name="value">  the value to find </param>
	  /// <returns> the index of the value, -1 if not found </returns>
	  public int indexOf(int value)
	  {
		return Ints.IndexOf(array, value);
	  }

	  /// <summary>
	  /// Find the index of the first occurrence of the specified value.
	  /// </summary>
	  /// <param name="value">  the value to find </param>
	  /// <returns> the index of the value, -1 if not found </returns>
	  public int lastIndexOf(int value)
	  {
		return Ints.LastIndexOf(array, value);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Copies this array into the specified array.
	  /// <para>
	  /// The specified array must be at least as large as this array.
	  /// If it is larger, then the remainder of the array will be untouched.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="destination">  the array to copy into </param>
	  /// <param name="offset">  the offset in the destination array to start from </param>
	  /// <exception cref="IndexOutOfBoundsException"> if the destination array is not large enough
	  ///   or the offset is negative </exception>
	  public void copyInto(int[] destination, int offset)
	  {
		if (destination.Length < array.Length + offset)
		{
		  throw new System.IndexOutOfRangeException("Destination array is not large enough");
		}
		Array.Copy(array, 0, destination, offset, array.Length);
	  }

	  /// <summary>
	  /// Returns an array holding the values from the specified index onwards.
	  /// </summary>
	  /// <param name="fromIndexInclusive">  the start index of the array to copy from </param>
	  /// <returns> an array instance with the specified bounds </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IntArray subArray(int fromIndexInclusive)
	  {
		return subArray(fromIndexInclusive, array.Length);
	  }

	  /// <summary>
	  /// Returns an array holding the values between the specified from and to indices.
	  /// </summary>
	  /// <param name="fromIndexInclusive">  the start index of the array to copy from </param>
	  /// <param name="toIndexExclusive">  the end index of the array to copy to </param>
	  /// <returns> an array instance with the specified bounds </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IntArray subArray(int fromIndexInclusive, int toIndexExclusive)
	  {
		return copyOf(array, fromIndexInclusive, toIndexExclusive);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this instance to an independent {@code int[]}.
	  /// </summary>
	  /// <returns> a copy of the underlying array </returns>
	  public int[] toArray()
	  {
		return array.Clone();
	  }

	  /// <summary>
	  /// Returns the underlying array.
	  /// <para>
	  /// This method is inherently unsafe as it relies on good behavior by callers.
	  /// Callers must never make any changes to the array returned by this method.
	  /// Doing so would violate the immutability of this class.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the raw array </returns>
	  public int[] toArrayUnsafe()
	  {
		return array;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a list equivalent to this array.
	  /// </summary>
	  /// <returns> a list wrapping this array </returns>
	  public IList<int> toList()
	  {
		return new ImmList(this);
	  }

	  /// <summary>
	  /// Returns a stream over the array values.
	  /// </summary>
	  /// <returns> a stream over the values in the array </returns>
	  public IntStream stream()
	  {
		return IntStream.of(array);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Applies an action to each value in the array.
	  /// <para>
	  /// This is used to perform an action on the contents of this array.
	  /// The action receives both the index and the value.
	  /// For example, the action could print out the array.
	  /// <pre>
	  ///   base.forEach((index, value) -&gt; System.out.println(index + ": " + value));
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="action">  the action to be applied </param>
	  public void forEach(IntIntConsumer action)
	  {
		for (int i = 0; i < array.Length; i++)
		{
		  action(i, array[i]);
		}
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the value at the specified index changed.
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the zero-based index to set </param>
	  /// <param name="newValue">  the new value to store </param>
	  /// <returns> a copy of this array with the value at the index changed </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IntArray with(int index, int newValue)
	  {
		if (array[index] == newValue)
		{
		  return this;
		}
		int[] result = array.Clone();
		result[index] = newValue;
		return new IntArray(result);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified amount added to each value.
	  /// <para>
	  /// This is used to add to the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(IntUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to add, may be negative </param>
	  /// <returns> a copy of this array with the amount added to each value </returns>
	  public IntArray plus(int amount)
	  {
		if (amount == 0)
		{
		  return this;
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] + amount;
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with the specified amount subtracted from each value.
	  /// <para>
	  /// This is used to subtract from the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(IntUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to subtract, may be negative </param>
	  /// <returns> a copy of this array with the amount subtracted from each value </returns>
	  public IntArray minus(int amount)
	  {
		if (amount == 0)
		{
		  return this;
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] - amount;
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with each value multiplied by the specified factor.
	  /// <para>
	  /// This is used to multiply the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(IntUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> a copy of this array with the each value multiplied by the factor </returns>
	  public IntArray multipliedBy(int factor)
	  {
		if (factor == 1)
		{
		  return this;
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] * factor;
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with each value divided by the specified divisor.
	  /// <para>
	  /// This is used to divide the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(IntUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="divisor">  the value by which the array values are divided </param>
	  /// <returns> a copy of this array with the each value divided by the divisor </returns>
	  public IntArray dividedBy(int divisor)
	  {
		if (divisor == 1)
		{
		  return this;
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] / divisor;
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with an operation applied to each value in the array.
	  /// <para>
	  /// This is used to perform an operation on the contents of this array, returning a new array.
	  /// The operator only receives the value.
	  /// For example, the operator could multiply and add each element.
	  /// <pre>
	  ///   result = base.map(value -&gt; value * 3 + 4);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied </param>
	  /// <returns> a copy of this array with the operator applied to the original values </returns>
	  public IntArray map(System.Func<int, int> @operator)
	  {
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = @operator(array[i]);
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with an operation applied to each indexed value in the array.
	  /// <para>
	  /// This is used to perform an operation on the contents of this array, returning a new array.
	  /// The function receives both the index and the value.
	  /// For example, the operator could multiply the value by the index.
	  /// <pre>
	  ///   result = base.mapWithIndex((index, value) -&gt; index * value);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="function">  the function to be applied </param>
	  /// <returns> a copy of this array with the operator applied to the original values </returns>
	  public IntArray mapWithIndex(System.Func<int, int, int> function)
	  {
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = function(i, array[i]);
		}
		return new IntArray(result);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance where each element is the sum of the matching values
	  /// in this array and the other array.
	  /// <para>
	  /// This is used to add two arrays, returning a new array.
	  /// Element {@code n} in the resulting array is equal to element {@code n} in this array
	  /// plus element {@code n} in the other array.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#combine(IntArray, IntBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements added </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public IntArray plus(IntArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] + other.array[i];
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance where each element is equal to the difference between the
	  /// matching values in this array and the other array.
	  /// <para>
	  /// This is used to subtract the second array from the first, returning a new array.
	  /// Element {@code n} in the resulting array is equal to element {@code n} in this array
	  /// minus element {@code n} in the other array.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#combine(IntArray, IntBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public IntArray minus(IntArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] - other.array[i];
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance where each element is equal to the product of the
	  /// matching values in this array and the other array.
	  /// <para>
	  /// This is used to multiply each value in this array by the corresponding value in the other array,
	  /// returning a new array.
	  /// </para>
	  /// <para>
	  /// Element {@code n} in the resulting array is equal to element {@code n} in this array
	  /// multiplied by element {@code n} in the other array.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#combine(IntArray, IntBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements multiplied </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public IntArray multipliedBy(IntArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] * other.array[i];
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance where each element is calculated by dividing values in this array by values in the other array.
	  /// <para>
	  /// This is used to divide each value in this array by the corresponding value in the other array,
	  /// returning a new array.
	  /// </para>
	  /// <para>
	  /// Element {@code n} in the resulting array is equal to element {@code n} in this array
	  /// divided by element {@code n} in the other array.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#combine(IntArray, IntBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements divided </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public IntArray dividedBy(IntArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] / other.array[i];
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an instance where each element is formed by some combination of the matching
	  /// values in this array and the other array.
	  /// <para>
	  /// This is used to combine two arrays, returning a new array.
	  /// Element {@code n} in the resulting array is equal to the result of the operator
	  /// when applied to element {@code n} in this array and element {@code n} in the other array.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <param name="operator">  the operator used to combine each pair of values </param>
	  /// <returns> a copy of this array combined with the specified array </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public IntArray combine(IntArray other, System.Func<int, int, int> @operator)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		int[] result = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = @operator(array[i], other.array[i]);
		}
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Combines this array and the other array returning a reduced value.
	  /// <para>
	  /// This is used to combine two arrays, returning a single reduced value.
	  /// The operator is called once for each element in the arrays.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// The first argument to the operator is the running total of the reduction, starting from zero.
	  /// The second argument to the operator is the element from this array.
	  /// The third argument to the operator is the element from the other array.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <param name="operator">  the operator used to combine each pair of values with the current total </param>
	  /// <returns> the result of the reduction </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public int combineReduce(IntArray other, IntTernaryOperator @operator)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		int result = 0;
		for (int i = 0; i < array.Length; i++)
		{
		  result = @operator(result, array[i], other.array[i]);
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an array that combines this array and the specified array.
	  /// <para>
	  /// The result will have a length equal to {@code this.size() + arrayToConcat.length}.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="arrayToConcat">  the array to add to the end of this array </param>
	  /// <returns> a copy of this array with the specified array added at the end </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IntArray concat(params int[] arrayToConcat)
	  {
		if (array.Length == 0)
		{
		  return copyOf(arrayToConcat);
		}
		if (arrayToConcat.Length == 0)
		{
		  return this;
		}
		int[] result = new int[array.Length + arrayToConcat.Length];
		Array.Copy(array, 0, result, 0, array.Length);
		Array.Copy(arrayToConcat, 0, result, array.Length, arrayToConcat.Length);
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns an array that combines this array and the specified array.
	  /// <para>
	  /// The result will have a length equal to {@code this.size() + newArray.length}.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="arrayToConcat">  the new array to add to the end of this array </param>
	  /// <returns> a copy of this array with the specified array added at the end </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if the index is invalid </exception>
	  public IntArray concat(IntArray arrayToConcat)
	  {
		if (array.Length == 0)
		{
		  return arrayToConcat;
		}
		if (arrayToConcat.array.Length == 0)
		{
		  return this;
		}
		return concat(arrayToConcat.array);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a sorted copy of this array.
	  /// <para>
	  /// This uses <seealso cref="Arrays#sort(int[])"/>.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a sorted copy of this array </returns>
	  public IntArray sorted()
	  {
		if (array.Length < 2)
		{
		  return this;
		}
		int[] result = array.Clone();
		Arrays.sort(result);
		return new IntArray(result);
	  }

	  /// <summary>
	  /// Returns the minimum value held in the array.
	  /// <para>
	  /// If the array is empty, then an exception is thrown.
	  /// If the array contains NaN, then the result is NaN.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the minimum value </returns>
	  /// <exception cref="IllegalStateException"> if the array is empty </exception>
	  public int min()
	  {
		if (array.Length == 0)
		{
		  throw new System.InvalidOperationException("Unable to find minimum of an empty array");
		}
		if (array.Length == 1)
		{
		  return array[0];
		}
		int min = int.MaxValue;
		for (int i = 0; i < array.Length; i++)
		{
		  min = Math.Min(min, array[i]);
		}
		return min;
	  }

	  /// <summary>
	  /// Returns the minimum value held in the array.
	  /// <para>
	  /// If the array is empty, then an exception is thrown.
	  /// If the array contains NaN, then the result is NaN.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the maximum value </returns>
	  /// <exception cref="IllegalStateException"> if the array is empty </exception>
	  public int max()
	  {
		if (array.Length == 0)
		{
		  throw new System.InvalidOperationException("Unable to find maximum of an empty array");
		}
		if (array.Length == 1)
		{
		  return array[0];
		}
		int max = int.MinValue;
		for (int i = 0; i < array.Length; i++)
		{
		  max = Math.Max(max, array[i]);
		}
		return max;
	  }

	  /// <summary>
	  /// Returns the sum of all the values in the array.
	  /// <para>
	  /// This is a special case of <seealso cref="#reduce(int, IntBinaryOperator)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the total of all the values </returns>
	  public int sum()
	  {
		int total = 0;
		for (int i = 0; i < array.Length; i++)
		{
		  total += array[i];
		}
		return total;
	  }

	  /// <summary>
	  /// Reduces this array returning a single value.
	  /// <para>
	  /// This is used to reduce the values in this array to a single value.
	  /// The operator is called once for each element in the arrays.
	  /// The first argument to the operator is the running total of the reduction, starting from zero.
	  /// The second argument to the operator is the element.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="identity">  the identity value to start from </param>
	  /// <param name="operator">  the operator used to combine the value with the current total </param>
	  /// <returns> the result of the reduction </returns>
	  public int reduce(int identity, System.Func<int, int, int> @operator)
	  {
		int result = identity;
		for (int i = 0; i < array.Length; i++)
		{
		  result = @operator(result, array[i]);
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  public override MetaBean metaBean()
	  {
		return Meta.META;
	  }

	  public override Property<R> property<R>(string propertyName)
	  {
		return metaBean().metaProperty<R>(propertyName).createProperty(this);
	  }

	  public override ISet<string> propertyNames()
	  {
		return metaBean().metaPropertyMap().Keys;
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is IntArray)
		{
		  IntArray other = (IntArray) obj;
		  return Arrays.Equals(array, other.array);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return Arrays.GetHashCode(array);
	  }

	  public override string ToString()
	  {
		return Arrays.ToString(array);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Immutable {@code Iterator} representation of the array.
	  /// </summary>
	  internal class ImmIterator : IEnumerator<int>
	  {
		internal readonly int[] array;
		internal int index;

		public ImmIterator(int[] array)
		{
		  this.array = array;
		}

		public override bool hasNext()
		{
		  return index < array.Length;
		}

		public override bool hasPrevious()
		{
		  return index > 0;
		}

		public override int? next()
		{
		  if (hasNext())
		  {
			return array[index++];
		  }
		  throw new NoSuchElementException("Iteration has reached the last element");
		}

		public override int? previous()
		{
		  if (hasPrevious())
		  {
			return array[--index];
		  }
		  throw new NoSuchElementException("Iteration has reached the first element");
		}

		public override int nextIndex()
		{
		  return index;
		}

		public override int previousIndex()
		{
		  return index - 1;
		}

		public override void remove()
		{
		  throw new System.NotSupportedException("Unable to remove from IntArray");
		}

		public override void set(int? value)
		{
		  throw new System.NotSupportedException("Unable to set value in IntArray");
		}

		public override void add(int? value)
		{
		  throw new System.NotSupportedException("Unable to add value to IntArray");
		}
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Immutable {@code List} representation of the array.
	  /// </summary>
	  [Serializable]
	  internal class ImmList : System.Collections.ObjectModel.Collection<int>, RandomAccess
	  {
		internal const long serialVersionUID = 1L;

		internal readonly IntArray underlying;

		internal ImmList(IntArray underlying)
		{
		  this.underlying = underlying;
		}

		public override int size()
		{
		  return underlying.size();
		}

		public override int? get(int index)
		{
		  return underlying.get(index);
		}

		public override bool contains(object obj)
		{
		  return (obj is int? ? underlying.contains((int?) obj.Value) : false);
		}

		public override int indexOf(object obj)
		{
		  return (obj is int? ? underlying.indexOf((int?) obj.Value) : -1);
		}

		public override int lastIndexOf(object obj)
		{
		  return (obj is int? ? underlying.lastIndexOf((int?) obj.Value) : -1);
		}

		public override IEnumerator<int> iterator()
		{
		  return listIterator();
		}

		public override IEnumerator<int> listIterator()
		{
		  return new ImmIterator(underlying.array);
		}

		protected internal override void removeRange(int fromIndex, int toIndex)
		{
		  throw new System.NotSupportedException("Unable to remove range from IntArray");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Meta bean.
	  /// </summary>
	  internal sealed class Meta : BasicMetaBean
	  {

		internal static readonly MetaBean META = new Meta();
		internal static readonly MetaProperty<int[]> ARRAY = new BasicMetaPropertyAnonymousInnerClass();

		private class BasicMetaPropertyAnonymousInnerClass : BasicMetaProperty<int[]>
		{
			public BasicMetaPropertyAnonymousInnerClass() : base("array")
			{
			}


			public override MetaBean metaBean()
			{
			  return META;
			}

			public override Type declaringType()
			{
			  return typeof(IntArray);
			}

			public override Type<int[]> propertyType()
			{
			  return typeof(int[]);
			}

			public override Type propertyGenericType()
			{
			  return typeof(int[]);
			}

			public override PropertyStyle style()
			{
			  return PropertyStyle.IMMUTABLE;
			}

			public override IList<Annotation> annotations()
			{
			  return ImmutableList.of();
			}

			public override int[] get(Bean bean)
			{
			  return ((IntArray) bean).toArray();
			}

			public override void set(Bean bean, object value)
			{
			  throw new System.NotSupportedException("Property cannot be written: " + name());
			}
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.google.common.collect.ImmutableMap<String, org.joda.beans.MetaProperty<?>> MAP = com.google.common.collect.ImmutableMap.of("array", ARRAY);
		internal static readonly ImmutableMap<string, MetaProperty<object>> MAP = ImmutableMap.of("array", ARRAY);

		internal Meta()
		{
		}

		public override bool Buildable
		{
			get
			{
			  return true;
			}
		}

		public override BeanBuilder<IntArray> builder()
		{
		  return new BasicImmutableBeanBuilderAnonymousInnerClass(this);
		}

		private class BasicImmutableBeanBuilderAnonymousInnerClass : BasicImmutableBeanBuilder<IntArray>
		{
			private readonly Meta outerInstance;

			public BasicImmutableBeanBuilderAnonymousInnerClass(Meta outerInstance) : base(outerInstance)
			{
				this.outerInstance = outerInstance;
				outerInstance.outerInstance.array = EMPTY_INT_ARRAY;
			}

			private int[] outerInstance.outerInstance.array;

			public override object get(string propertyName)
			{
			  if (propertyName.Equals(ARRAY.name()))
			  {
				return outerInstance.outerInstance.array.Clone();
			  }
			  else
			  {
				throw new NoSuchElementException("Unknown property: " + propertyName);
			  }
			}

			public override BeanBuilder<IntArray> set(string propertyName, object value)
			{
			  if (propertyName.Equals(ARRAY.name()))
			  {
				this.array = ((int[]) ArgChecker.notNull(value, "value")).Clone();
			  }
			  else
			  {
				throw new NoSuchElementException("Unknown property: " + propertyName);
			  }
			  return this;
			}

			public override IntArray build()
			{
			  return new IntArray(outerInstance.outerInstance.array);
			}
		}

		public override Type beanType()
		{
		  return typeof(IntArray);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return MAP;
		}
	  }

	}

}
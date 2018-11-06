using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using Doubles = com.google.common.primitives.Doubles;
	using DoubleTernaryOperator = com.opengamma.strata.collect.function.DoubleTernaryOperator;
	using IntDoubleConsumer = com.opengamma.strata.collect.function.IntDoubleConsumer;
	using IntDoubleToDoubleFunction = com.opengamma.strata.collect.function.IntDoubleToDoubleFunction;

	/// <summary>
	/// An immutable array of {@code double} values.
	/// <para>
	/// This provides functionality similar to <seealso cref="List"/> but for {@code double[]}.
	/// </para>
	/// <para>
	/// In mathematical terms, this is a vector, or one-dimensional matrix.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class DoubleArray : Matrix, ImmutableBean
	{

	  /// <summary>
	  /// An empty array.
	  /// </summary>
	  public static readonly DoubleArray EMPTY = new DoubleArray(new double[0]);

	  /// <summary>
	  /// Serialization version.
	  /// </summary>
	  private const long serialVersionUID = 1L;
	  static DoubleArray()
	  {
		MetaBean.register(Meta.META);
	  }

	  /// <summary>
	  /// The underlying array of doubles.
	  /// </summary>
	  private readonly double[] array;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty immutable array.
	  /// </summary>
	  /// <returns> the empty immutable array </returns>
	  public static DoubleArray of()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an immutable array with a single value.
	  /// </summary>
	  /// <param name="value">  the single value </param>
	  /// <returns> an array containing the specified value </returns>
	  public static DoubleArray of(double value)
	  {
		return new DoubleArray(new double[] {value});
	  }

	  /// <summary>
	  /// Obtains an immutable array with two values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static DoubleArray of(double value1, double value2)
	  {
		return new DoubleArray(new double[] {value1, value2});
	  }

	  /// <summary>
	  /// Obtains an immutable array with three values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static DoubleArray of(double value1, double value2, double value3)
	  {
		return new DoubleArray(new double[] {value1, value2, value3});
	  }

	  /// <summary>
	  /// Obtains an immutable array with four values.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value </param>
	  /// <param name="value3">  the third value </param>
	  /// <param name="value4">  the fourth value </param>
	  /// <returns> an array containing the specified values </returns>
	  public static DoubleArray of(double value1, double value2, double value3, double value4)
	  {
		return new DoubleArray(new double[] {value1, value2, value3, value4});
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
	  public static DoubleArray of(double value1, double value2, double value3, double value4, double value5)
	  {
		return new DoubleArray(new double[] {value1, value2, value3, value4, value5});
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
	  public static DoubleArray of(double value1, double value2, double value3, double value4, double value5, double value6)
	  {
		return new DoubleArray(new double[] {value1, value2, value3, value4, value5, value6});
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
	  public static DoubleArray of(double value1, double value2, double value3, double value4, double value5, double value6, double value7)
	  {
		return new DoubleArray(new double[] {value1, value2, value3, value4, value5, value6, value7});
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
	  public static DoubleArray of(double value1, double value2, double value3, double value4, double value5, double value6, double value7, double value8)
	  {
		return new DoubleArray(new double[] {value1, value2, value3, value4, value5, value6, value7, value8});
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
	  public static DoubleArray of(double value1, double value2, double value3, double value4, double value5, double value6, double value7, double value8, params double[] otherValues)
	  {
		double[] @base = new double[otherValues.Length + 8];
		@base[0] = value1;
		@base[1] = value2;
		@base[2] = value3;
		@base[3] = value4;
		@base[4] = value5;
		@base[5] = value6;
		@base[6] = value7;
		@base[7] = value8;
		Array.Copy(otherValues, 0, @base, 8, otherValues.Length);
		return new DoubleArray(@base);
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
	  public static DoubleArray of(int size, System.Func<int, double> valueFunction)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
		double[] array = new double[size];
		Arrays.setAll(array, valueFunction);
		return new DoubleArray(array);
	  }

	  /// <summary>
	  /// Obtains an instance with entries filled from a stream.
	  /// <para>
	  /// The stream is converted to an array using <seealso cref="DoubleStream#toArray()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="stream">  the stream of elements </param>
	  /// <returns> an array initialized using the stream </returns>
	  public static DoubleArray of(DoubleStream stream)
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
	  public static DoubleArray ofUnsafe(double[] array)
	  {
		if (array.Length == 0)
		{
		  return EMPTY;
		}
		return new DoubleArray(array);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a collection of {@code Double}.
	  /// <para>
	  /// The order of the values in the returned array is the order in which elements are returned
	  /// from the iterator of the collection.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="collection">  the collection to initialize from </param>
	  /// <returns> an array containing the values from the collection in iteration order </returns>
	  public static DoubleArray copyOf(ICollection<double> collection)
	  {
		if (collection.Count == 0)
		{
		  return EMPTY;
		}
		if (collection is ImmList)
		{
		  return ((ImmList) collection).underlying;
		}
		return new DoubleArray(Doubles.toArray(collection));
	  }

	  /// <summary>
	  /// Obtains an instance from an array of {@code double}.
	  /// <para>
	  /// The input array is copied and not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to copy, cloned </param>
	  /// <returns> an array containing the specified values </returns>
	  public static DoubleArray copyOf(double[] array)
	  {
		if (array.Length == 0)
		{
		  return EMPTY;
		}
		return new DoubleArray(array.Clone());
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
	  public static DoubleArray copyOf(double[] array, int fromIndex)
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
	  public static DoubleArray copyOf(double[] array, int fromIndexInclusive, int toIndexExclusive)
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
		return new DoubleArray(Arrays.copyOfRange(array, fromIndexInclusive, toIndexExclusive));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with all entries equal to the zero.
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <returns> an array filled with zeroes </returns>
	  public static DoubleArray filled(int size)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
		return new DoubleArray(new double[size]);
	  }

	  /// <summary>
	  /// Obtains an instance with all entries equal to the same value.
	  /// </summary>
	  /// <param name="size">  the number of elements </param>
	  /// <param name="value">  the value of all the elements </param>
	  /// <returns> an array filled with the specified value </returns>
	  public static DoubleArray filled(int size, double value)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
		double[] array = new double[size];
		Arrays.fill(array, value);
		return new DoubleArray(array);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from a {@code double[}.
	  /// </summary>
	  /// <param name="array">  the array, assigned not cloned </param>
	  private DoubleArray(double[] array)
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
	  public double get(int index)
	  {
		return array[index];
	  }

	  /// <summary>
	  /// Checks if this array contains the specified value.
	  /// <para>
	  /// The value is checked using {@code Double.doubleToLongBits} in order to match {@code equals}.
	  /// This also allow this method to be used to find any occurrences of NaN.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the value to find </param>
	  /// <returns> true if the value is contained in this array </returns>
	  public bool contains(double value)
	  {
		if (array.Length > 0)
		{
		  long bits = System.BitConverter.DoubleToInt64Bits(value);
		  for (int i = 0; i < array.Length; i++)
		  {
			if (System.BitConverter.DoubleToInt64Bits(array[i]) == bits)
			{
			  return true;
			}
		  }
		}
		return false;
	  }

	  /// <summary>
	  /// Find the index of the first occurrence of the specified value.
	  /// <para>
	  /// The value is checked using {@code Double.doubleToLongBits} in order to match {@code equals}.
	  /// This also allow this method to be used to find any occurrences of NaN.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the value to find </param>
	  /// <returns> the index of the value, -1 if not found </returns>
	  public int indexOf(double value)
	  {
		if (array.Length > 0)
		{
		  long bits = System.BitConverter.DoubleToInt64Bits(value);
		  for (int i = 0; i < array.Length; i++)
		  {
			if (System.BitConverter.DoubleToInt64Bits(array[i]) == bits)
			{
			  return i;
			}
		  }
		}
		return -1;
	  }

	  /// <summary>
	  /// Find the index of the first occurrence of the specified value.
	  /// <para>
	  /// The value is checked using {@code Double.doubleToLongBits} in order to match {@code equals}.
	  /// This also allow this method to be used to find any occurrences of NaN.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="value">  the value to find </param>
	  /// <returns> the index of the value, -1 if not found </returns>
	  public int lastIndexOf(double value)
	  {
		if (array.Length > 0)
		{
		  long bits = System.BitConverter.DoubleToInt64Bits(value);
		  for (int i = array.Length - 1; i >= 0; i--)
		  {
			if (System.BitConverter.DoubleToInt64Bits(array[i]) == bits)
			{
			  return i;
			}
		  }
		}
		return -1;
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
	  public void copyInto(double[] destination, int offset)
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
	  public DoubleArray subArray(int fromIndexInclusive)
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
	  public DoubleArray subArray(int fromIndexInclusive, int toIndexExclusive)
	  {
		return copyOf(array, fromIndexInclusive, toIndexExclusive);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this instance to an independent {@code double[]}.
	  /// </summary>
	  /// <returns> a copy of the underlying array </returns>
	  public double[] toArray()
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
	  public double[] toArrayUnsafe()
	  {
		return array;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a list equivalent to this array.
	  /// </summary>
	  /// <returns> a list wrapping this array </returns>
	  public IList<double> toList()
	  {
		return new ImmList(this);
	  }

	  /// <summary>
	  /// Returns a stream over the array values.
	  /// </summary>
	  /// <returns> a stream over the values in the array </returns>
	  public DoubleStream stream()
	  {
		return DoubleStream.of(array);
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
	  public void forEach(IntDoubleConsumer action)
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
	  public DoubleArray with(int index, double newValue)
	  {
		if (System.BitConverter.DoubleToInt64Bits(array[index]) == Double.doubleToLongBits(newValue))
		{
		  return this;
		}
		double[] result = array.Clone();
		result[index] = newValue;
		return new DoubleArray(result);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified amount added to each value.
	  /// <para>
	  /// This is used to add to the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(DoubleUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to add, may be negative </param>
	  /// <returns> a copy of this array with the amount added to each value </returns>
	  public DoubleArray plus(double amount)
	  {
		if (amount == 0d)
		{
		  return this;
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] + amount;
		}
		return new DoubleArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with the specified amount subtracted from each value.
	  /// <para>
	  /// This is used to subtract from the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(DoubleUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to subtract, may be negative </param>
	  /// <returns> a copy of this array with the amount subtracted from each value </returns>
	  public DoubleArray minus(double amount)
	  {
		if (amount == 0d)
		{
		  return this;
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] - amount;
		}
		return new DoubleArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with each value multiplied by the specified factor.
	  /// <para>
	  /// This is used to multiply the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(DoubleUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> a copy of this array with the each value multiplied by the factor </returns>
	  public DoubleArray multipliedBy(double factor)
	  {
		if (factor == 1d)
		{
		  return this;
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] * factor;
		}
		return new DoubleArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with each value divided by the specified divisor.
	  /// <para>
	  /// This is used to divide the contents of this array, returning a new array.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(DoubleUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="divisor">  the value by which the array values are divided </param>
	  /// <returns> a copy of this array with the each value divided by the divisor </returns>
	  public DoubleArray dividedBy(double divisor)
	  {
		if (divisor == 1d)
		{
		  return this;
		}
		// multiplication is cheaper than division so it is more efficient to do the division once and multiply each element
		double factor = 1 / divisor;
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] * factor;
		}
		return new DoubleArray(result);
	  }

	  /// <summary>
	  /// Returns an instance with an operation applied to each value in the array.
	  /// <para>
	  /// This is used to perform an operation on the contents of this array, returning a new array.
	  /// The operator only receives the value.
	  /// For example, the operator could take the inverse of each element.
	  /// <pre>
	  ///   result = base.map(value -&gt; 1 / value);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied </param>
	  /// <returns> a copy of this array with the operator applied to the original values </returns>
	  public DoubleArray map(System.Func<double, double> @operator)
	  {
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = @operator(array[i]);
		}
		return new DoubleArray(result);
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
	  public DoubleArray mapWithIndex(IntDoubleToDoubleFunction function)
	  {
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = function(i, array[i]);
		}
		return new DoubleArray(result);
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
	  /// This is a special case of <seealso cref="#combine(DoubleArray, DoubleBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements added </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public DoubleArray plus(DoubleArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] + other.array[i];
		}
		return new DoubleArray(result);
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
	  /// This is a special case of <seealso cref="#combine(DoubleArray, DoubleBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public DoubleArray minus(DoubleArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] - other.array[i];
		}
		return new DoubleArray(result);
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
	  /// This is a special case of <seealso cref="#combine(DoubleArray, DoubleBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements multiplied </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public DoubleArray multipliedBy(DoubleArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] * other.array[i];
		}
		return new DoubleArray(result);
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
	  /// This is a special case of <seealso cref="#combine(DoubleArray, DoubleBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <returns> a copy of this array with matching elements divided </returns>
	  /// <exception cref="IllegalArgumentException"> if the arrays have different sizes </exception>
	  public DoubleArray dividedBy(DoubleArray other)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = array[i] / other.array[i];
		}
		return new DoubleArray(result);
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
	  public DoubleArray combine(DoubleArray other, System.Func<double, double, double> @operator)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		double[] result = new double[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
		  result[i] = @operator(array[i], other.array[i]);
		}
		return new DoubleArray(result);
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
	  public double combineReduce(DoubleArray other, DoubleTernaryOperator @operator)
	  {
		if (array.Length != other.array.Length)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
		double result = 0;
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
	  public DoubleArray concat(params double[] arrayToConcat)
	  {
		if (array.Length == 0)
		{
		  return copyOf(arrayToConcat);
		}
		if (arrayToConcat.Length == 0)
		{
		  return this;
		}
		double[] result = new double[array.Length + arrayToConcat.Length];
		Array.Copy(array, 0, result, 0, array.Length);
		Array.Copy(arrayToConcat, 0, result, array.Length, arrayToConcat.Length);
		return new DoubleArray(result);
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
	  public DoubleArray concat(DoubleArray arrayToConcat)
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
	  /// This uses <seealso cref="Arrays#sort(double[])"/>.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a sorted copy of this array </returns>
	  public DoubleArray sorted()
	  {
		if (array.Length < 2)
		{
		  return this;
		}
		double[] result = array.Clone();
		Arrays.sort(result);
		return new DoubleArray(result);
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
	  public double min()
	  {
		if (array.Length == 0)
		{
		  throw new System.InvalidOperationException("Unable to find minimum of an empty array");
		}
		if (array.Length == 1)
		{
		  return array[0];
		}
		double min = double.PositiveInfinity;
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
	  public double max()
	  {
		if (array.Length == 0)
		{
		  throw new System.InvalidOperationException("Unable to find maximum of an empty array");
		}
		if (array.Length == 1)
		{
		  return array[0];
		}
		double max = double.NegativeInfinity;
		for (int i = 0; i < array.Length; i++)
		{
		  max = Math.Max(max, array[i]);
		}
		return max;
	  }

	  /// <summary>
	  /// Returns the sum of all the values in the array.
	  /// <para>
	  /// This is a special case of <seealso cref="#reduce(double, DoubleBinaryOperator)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the total of all the values </returns>
	  public double sum()
	  {
		double total = 0;
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
	  public double reduce(double identity, System.Func<double, double, double> @operator)
	  {
		double result = identity;
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
	  /// <summary>
	  /// Checks if this array equals another within the specified tolerance.
	  /// <para>
	  /// This returns true if the two instances have {@code double} values that are
	  /// equal within the specified tolerance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other array </param>
	  /// <param name="tolerance">  the tolerance </param>
	  /// <returns> true if equal up to the tolerance </returns>
	  public bool equalWithTolerance(DoubleArray other, double tolerance)
	  {
		return DoubleArrayMath.fuzzyEquals(array, other.array, tolerance);
	  }

	  /// <summary>
	  /// Checks if this array equals zero within the specified tolerance.
	  /// <para>
	  /// This returns true if all the {@code double} values equal zero within the specified tolerance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tolerance">  the tolerance </param>
	  /// <returns> true if equal up to the tolerance </returns>
	  public bool equalZeroWithTolerance(double tolerance)
	  {
		return DoubleArrayMath.fuzzyEqualsZero(array, tolerance);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is DoubleArray)
		{
		  DoubleArray other = (DoubleArray) obj;
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
	  internal class ImmIterator : IEnumerator<double>
	  {
		internal readonly double[] array;
		internal int index;

		public ImmIterator(double[] array)
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

		public override double? next()
		{
		  if (hasNext())
		  {
			return array[index++];
		  }
		  throw new NoSuchElementException("Iteration has reached the last element");
		}

		public override double? previous()
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
		  throw new System.NotSupportedException("Unable to remove from DoubleArray");
		}

		public override void set(double? value)
		{
		  throw new System.NotSupportedException("Unable to set value in DoubleArray");
		}

		public override void add(double? value)
		{
		  throw new System.NotSupportedException("Unable to add value to DoubleArray");
		}
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Immutable {@code List} representation of the array.
	  /// </summary>
	  [Serializable]
	  internal class ImmList : System.Collections.ObjectModel.Collection<double>, RandomAccess
	  {
		internal const long serialVersionUID = 1L;

		internal readonly DoubleArray underlying;

		internal ImmList(DoubleArray underlying)
		{
		  this.underlying = underlying;
		}

		public override int size()
		{
		  return underlying.size();
		}

		public override double? get(int index)
		{
		  return underlying.get(index);
		}

		public override bool contains(object obj)
		{
		  return (obj is double? ? underlying.contains((double?) obj.Value) : false);
		}

		public override int indexOf(object obj)
		{
		  return (obj is double? ? underlying.indexOf((double?) obj.Value) : -1);
		}

		public override int lastIndexOf(object obj)
		{
		  return (obj is double? ? underlying.lastIndexOf((double?) obj.Value) : -1);
		}

		public override IEnumerator<double> iterator()
		{
		  return listIterator();
		}

		public override IEnumerator<double> listIterator()
		{
		  return new ImmIterator(underlying.array);
		}

		protected internal override void removeRange(int fromIndex, int toIndex)
		{
		  throw new System.NotSupportedException("Unable to remove range from DoubleArray");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Meta bean.
	  /// </summary>
	  internal sealed class Meta : BasicMetaBean
	  {

		internal static readonly MetaBean META = new Meta();
		internal static readonly MetaProperty<double[]> ARRAY = new BasicMetaPropertyAnonymousInnerClass();

		private class BasicMetaPropertyAnonymousInnerClass : BasicMetaProperty<double[]>
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
			  return typeof(DoubleArray);
			}

			public override Type<double[]> propertyType()
			{
			  return typeof(double[]);
			}

			public override Type propertyGenericType()
			{
			  return typeof(double[]);
			}

			public override PropertyStyle style()
			{
			  return PropertyStyle.IMMUTABLE;
			}

			public override IList<Annotation> annotations()
			{
			  return ImmutableList.of();
			}

			public override double[] get(Bean bean)
			{
			  return ((DoubleArray) bean).toArray();
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

		public override BeanBuilder<DoubleArray> builder()
		{
		  return new BasicImmutableBeanBuilderAnonymousInnerClass(this);
		}

		private class BasicImmutableBeanBuilderAnonymousInnerClass : BasicImmutableBeanBuilder<DoubleArray>
		{
			private readonly Meta outerInstance;

			public BasicImmutableBeanBuilderAnonymousInnerClass(Meta outerInstance) : base(outerInstance)
			{
				this.outerInstance = outerInstance;
				outerInstance.outerInstance.array = DoubleArrayMath.EMPTY_DOUBLE_ARRAY;
			}

			private double[] outerInstance.outerInstance.array;

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

			public override BeanBuilder<DoubleArray> set(string propertyName, object value)
			{
			  if (propertyName.Equals(ARRAY.name()))
			  {
				this.array = ((double[]) ArgChecker.notNull(value, "value")).Clone();
			  }
			  else
			  {
				throw new NoSuchElementException("Unknown property: " + propertyName);
			  }
			  return this;
			}

			public override DoubleArray build()
			{
			  return new DoubleArray(outerInstance.outerInstance.array);
			}
		}

		public override Type beanType()
		{
		  return typeof(DoubleArray);
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
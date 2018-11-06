using System;
using System.Collections.Generic;
using System.Text;

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
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using IntIntDoubleConsumer = com.opengamma.strata.collect.function.IntIntDoubleConsumer;
	using IntIntDoubleToDoubleFunction = com.opengamma.strata.collect.function.IntIntDoubleToDoubleFunction;
	using IntIntToDoubleFunction = com.opengamma.strata.collect.function.IntIntToDoubleFunction;

	/// <summary>
	/// An immutable two-dimensional array of {@code double} values.
	/// <para>
	/// This provides functionality similar to <seealso cref="List"/> but for a rectangular {@code double[][]}.
	/// </para>
	/// <para>
	/// In mathematical terms, this is a two-dimensional matrix.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class DoubleMatrix implements Matrix, java.io.Serializable, org.joda.beans.ImmutableBean
	[Serializable]
	public sealed class DoubleMatrix : Matrix, ImmutableBean
	{

	  /// <summary>
	  /// An empty array.
	  /// </summary>
	  public static readonly DoubleMatrix EMPTY = new DoubleMatrix(new double[0][], 0, 0);

	  /// <summary>
	  /// Serialization version.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The underlying array of doubles.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "") private final double[][] array;
	  private readonly double[][] array;
	  /// <summary>
	  /// The number of rows.
	  /// </summary>
	  [NonSerialized]
	  private readonly int rows; // derived, not a property
	  /// <summary>
	  /// The number of columns.
	  /// </summary>
	  [NonSerialized]
	  private readonly int columns; // derived, not a property
	  /// <summary>
	  /// The number of elements.
	  /// </summary>
	  [NonSerialized]
	  private readonly int elements; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty instance.
	  /// </summary>
	  /// <returns> the empty immutable matrix </returns>
	  public static DoubleMatrix of()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an immutable array with the specified size and values.
	  /// <para>
	  /// The first two arguments specify the size.
	  /// The remaining arguments specify the values, all of row 0, then row 1, and so on.
	  /// There must be be {@code rows * columns} values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  /// <param name="values">  the values </param>
	  /// <returns> an array containing the specified value </returns>
	  /// <exception cref="IllegalArgumentException"> if the values array if the incorrect length </exception>
	  public static DoubleMatrix of(int rows, int columns, params double[] values)
	  {
		if (values.Length != rows * columns)
		{
		  throw new System.ArgumentException("Values array not of length rows * columns");
		}
		if (rows == 0 || columns == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] array = new double[rows][columns];
		double[][] array = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < values.Length; i++)
		{
		  array[i / columns][i % columns] = values[i];
		}
		return new DoubleMatrix(array, rows, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with entries filled using a function.
	  /// <para>
	  /// The function is passed the row and column index, returning the value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  /// <param name="valueFunction">  the function used to populate the value </param>
	  /// <returns> a matrix initialized using the function </returns>
	  public static DoubleMatrix of(int rows, int columns, IntIntToDoubleFunction valueFunction)
	  {
		if (rows == 0 || columns == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] array = new double[rows][columns];
		double[][] array = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < array.Length; i++)
		{
		  double[] inner = array[i];
		  for (int j = 0; j < inner.Length; j++)
		  {
			inner[j] = valueFunction(i, j);
		  }
		}
		return new DoubleMatrix(array, rows, columns);
	  }

	  /// <summary>
	  /// Obtains an instance with entries filled using a function.
	  /// <para>
	  /// The function is passed the row index, returning the column values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  /// <param name="valuesFunction">  the function used to populate the values </param>
	  /// <returns> a matrix initialized using the function </returns>
	  public static DoubleMatrix ofArrays(int rows, int columns, System.Func<int, double[]> valuesFunction)
	  {
		if (rows == 0 || columns == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] array = new double[rows][columns];
		double[][] array = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < array.Length; i++)
		{
		  double[] values = valuesFunction(i);
		  if (values.Length != columns)
		  {
			throw new System.ArgumentException(Messages.format("Function returned array of incorrect length {}, expected {}", values.Length, columns));
		  }
		  array[i] = values.Clone();
		}
		return new DoubleMatrix(array, rows, columns);
	  }

	  /// <summary>
	  /// Obtains an instance with entries filled using a function.
	  /// <para>
	  /// The function is passed the row index, returning the column values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  /// <param name="valuesFunction">  the function used to populate the values </param>
	  /// <returns> a matrix initialized using the function </returns>
	  public static DoubleMatrix ofArrayObjects(int rows, int columns, System.Func<int, DoubleArray> valuesFunction)
	  {
		if (rows == 0 || columns == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] array = new double[rows][columns];
		double[][] array = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < array.Length; i++)
		{
		  DoubleArray values = valuesFunction(i);
		  if (values.size() != columns)
		  {
			throw new System.ArgumentException(Messages.format("Function returned array of incorrect length {}, expected {}", values.size(), columns));
		  }
		  array[i] = values.toArrayUnsafe();
		}
		return new DoubleMatrix(array, rows, columns);
	  }

	  /// <summary>
	  /// Obtains an instance by wrapping a {@code double[][]}.
	  /// <para>
	  /// This method is inherently unsafe as it relies on good behavior by callers.
	  /// Callers must never make any changes to the passed in array after calling this method.
	  /// Doing so would violate the immutability of this class.
	  /// </para>
	  /// <para>
	  /// The {@code double[][]} must be rectangular, with the same length for each row.
	  /// This is not validated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to assign </param>
	  /// <returns> a matrix wrapping the specified array </returns>
	  public static DoubleMatrix ofUnsafe(double[][] array)
	  {
		int rows = array.Length;
		if (rows == 0 || array[0].Length == 0)
		{
		  return EMPTY;
		}
		return new DoubleMatrix(array, rows, array[0].Length);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a {@code double[][]}.
	  /// <para>
	  /// The input array is copied and not mutated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to copy, cloned </param>
	  /// <returns> a matrix containing the specified values </returns>
	  public static DoubleMatrix copyOf(double[][] array)
	  {
		int rows = array.Length;
		if (rows == 0 || array[0].Length == 0)
		{
		  return EMPTY;
		}
		int columns = array[0].Length;
		return new DoubleMatrix(deepClone(array, rows, columns), rows, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with all entries equal to the zero.
	  /// </summary>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  /// <returns> a matrix filled with zeroes </returns>
	  public static DoubleMatrix filled(int rows, int columns)
	  {
		if (rows == 0 || columns == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: return new DoubleMatrix(new double[rows][columns], rows, columns);
		return new DoubleMatrix(RectangularArrays.ReturnRectangularDoubleArray(rows, columns), rows, columns);
	  }

	  /// <summary>
	  /// Obtains an instance with all entries equal to the same value.
	  /// </summary>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  /// <param name="value">  the value of all the elements </param>
	  /// <returns> a matrix filled with the specified value </returns>
	  public static DoubleMatrix filled(int rows, int columns, double value)
	  {
		if (rows == 0 || columns == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] array = new double[rows][columns];
		double[][] array = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < array.Length; i++)
		{
		  Arrays.fill(array[i], value);
		}
		return new DoubleMatrix(array, rows, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an identity matrix.
	  /// <para>
	  /// An identity matrix is square. It has every value equal to zero, except those
	  /// on the primary diagonal, which are one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the size of the matrix </param>
	  /// <returns> an identity matrix of the specified size </returns>
	  public static DoubleMatrix identity(int size)
	  {
		if (size == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] array = new double[size][size];
		double[][] array = RectangularArrays.ReturnRectangularDoubleArray(size, size);
		for (int i = 0; i < size; i++)
		{
		  array[i][i] = 1d;
		}
		return new DoubleMatrix(array, size, size);
	  }

	  /// <summary>
	  /// Obtains a diagonal matrix from the specified array.
	  /// <para>
	  /// A diagonal matrix is square. It only has values on the primary diagonal,
	  /// and those values are taken from the specified array.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to use to create the matrix </param>
	  /// <returns> an identity matrix of the specified size </returns>
	  public static DoubleMatrix diagonal(DoubleArray array)
	  {
		int size = array.size();
		if (size == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] data = new double[size][size];
		double[][] data = RectangularArrays.ReturnRectangularDoubleArray(size, size);
		for (int i = 0; i < size; i++)
		{
		  data[i][i] = array.get(i);
		}
		return new DoubleMatrix(data, size, size);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a matrix.
	  /// </summary>
	  /// <param name="data">  the data </param>
	  /// <param name="rows">  the number of rows </param>
	  /// <param name="columns">  the number of columns </param>
	  internal DoubleMatrix(double[][] data, int rows, int columns)
	  {
		this.rows = rows;
		this.columns = columns;
		this.array = data;
		this.elements = rows * columns;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DoubleMatrix(double[][] array)
	  private DoubleMatrix(double[][] array)
	  {
		ArgChecker.notNull(array, "array");
		if (array.Length == 0)
		{
		  this.array = EMPTY.array;
		  this.rows = 0;
		  this.columns = 0;
		}
		else
		{
		  this.array = array;
		  this.rows = array.Length;
		  this.columns = array[0].Length;
		}
		this.elements = rows * columns;
	  }

	  // depp clone a double[][]
	  private static double[][] deepClone(double[][] input, int rows, int columns)
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] cloned = new double[rows][columns];
		double[][] cloned = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  cloned[i] = input[i].clone();
		}
		return cloned;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new DoubleMatrix(array);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of dimensions of this matrix.
	  /// </summary>
	  /// <returns> two </returns>
	  public int dimensions()
	  {
		return 2;
	  }

	  /// <summary>
	  /// Gets the size of this matrix.
	  /// <para>
	  /// This is the total number of elements.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the matrix size, zero or greater </returns>
	  public int size()
	  {
		return elements;
	  }

	  /// <summary>
	  /// Gets the number of rows of this matrix.
	  /// </summary>
	  /// <returns> the number of rows </returns>
	  public int rowCount()
	  {
		return rows;
	  }

	  /// <summary>
	  /// Gets the number of columns of this matrix.
	  /// </summary>
	  /// <returns> the number of columns </returns>
	  public int columnCount()
	  {
		return columns;
	  }

	  /// <summary>
	  /// Checks if this matrix is square.
	  /// <para>
	  /// A square matrix has the same number of rows and columns.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if square </returns>
	  public bool Square
	  {
		  get
		  {
			return rows == columns;
		  }
	  }

	  /// <summary>
	  /// Checks if this matrix is empty.
	  /// </summary>
	  /// <returns> true if empty </returns>
	  public bool Empty
	  {
		  get
		  {
			return elements == 0;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value at the specified row and column in this matrix.
	  /// </summary>
	  /// <param name="row">  the zero-based row index to retrieve </param>
	  /// <param name="column">  the zero-based column index to retrieve </param>
	  /// <returns> the value at the row and column </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if either index is invalid </exception>
	  public double get(int row, int column)
	  {
		return array[row][column];
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the row at the specified index.
	  /// </summary>
	  /// <param name="row">  the zero-based row index to retrieve </param>
	  /// <returns> the row </returns>
	  public DoubleArray row(int row)
	  {
		return DoubleArray.ofUnsafe(array[row]);
	  }

	  /// <summary>
	  /// Gets the row at the specified index as an independent array.
	  /// </summary>
	  /// <param name="row">  the zero-based row index to retrieve </param>
	  /// <returns> the row as a cloned array </returns>
	  public double[] rowArray(int row)
	  {
		return array[row].clone();
	  }

	  /// <summary>
	  /// Gets the column at the specified index.
	  /// </summary>
	  /// <param name="column">  the zero-based column index to retrieve </param>
	  /// <returns> the column </returns>
	  public DoubleArray column(int column)
	  {
		return DoubleArray.of(rows, i => array[i][column]);
	  }

	  /// <summary>
	  /// Gets the column at the specified index as an independent array.
	  /// </summary>
	  /// <param name="column">  the zero-based column index to retrieve </param>
	  /// <returns> the column as a cloned array </returns>
	  public double[] columnArray(int column)
	  {
		return this.column(column).toArrayUnsafe();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this instance to an independent {@code double[][]}.
	  /// </summary>
	  /// <returns> an array of arrays containing a copy of matrix elements </returns>
	  public double[][] toArray()
	  {
		return deepClone(array, rows, columns);
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
	  public double[][] toArrayUnsafe()
	  {
		return array;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Applies an action to each value in the matrix.
	  /// <para>
	  /// This is used to perform an action on the contents of this matrix.
	  /// The action receives the row, the column and the value.
	  /// For example, the action could print out the matrix.
	  /// <pre>
	  ///   base.forEach((row, col, value) -&gt; System.out.println(row + ": " + col + ": " + value));
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="action">  the action to be applied </param>
	  public void forEach(IntIntDoubleConsumer action)
	  {
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			action(i, j, array[i][j]);
		  }
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
	  /// <param name="row">  the zero-based row index to retrieve </param>
	  /// <param name="column">  the zero-based column index to retrieve </param>
	  /// <param name="newValue">  the new value to store </param>
	  /// <returns> a copy of this matrix with the value at the index changed </returns>
	  /// <exception cref="IndexOutOfBoundsException"> if either index is invalid </exception>
	  public DoubleMatrix with(int row, int column, double newValue)
	  {
		if (System.BitConverter.DoubleToInt64Bits(array[row][column]) == Double.doubleToLongBits(newValue))
		{
		  return this;
		}
		double[][] result = array.Clone(); // shallow clone rows array
		result[row] = result[row].clone(); // clone the column actually being changed, share the rest
		result[row][column] = newValue;
		return new DoubleMatrix(result, rows, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with each value multiplied by the specified factor.
	  /// <para>
	  /// This is used to multiply the contents of this matrix, returning a new matrix.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#map(DoubleUnaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> a copy of this matrix with the each value multiplied by the factor </returns>
	  public DoubleMatrix multipliedBy(double factor)
	  {
		if (factor == 1d)
		{
		  return this;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[rows][columns];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result[i][j] = array[i][j] * factor;
		  }
		}
		return new DoubleMatrix(result, rows, columns);
	  }

	  /// <summary>
	  /// Returns an instance with an operation applied to each value in the matrix.
	  /// <para>
	  /// This is used to perform an operation on the contents of this matrix, returning a new matrix.
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
	  /// <returns> a copy of this matrix with the operator applied to the original values </returns>
	  public DoubleMatrix map(System.Func<double, double> @operator)
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[rows][columns];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result[i][j] = @operator(array[i][j]);
		  }
		}
		return new DoubleMatrix(result, rows, columns);
	  }

	  /// <summary>
	  /// Returns an instance with an operation applied to each indexed value in the matrix.
	  /// <para>
	  /// This is used to perform an operation on the contents of this matrix, returning a new matrix.
	  /// The function receives the row index, column index and the value.
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
	  /// <returns> a copy of this matrix with the operator applied to the original values </returns>
	  public DoubleMatrix mapWithIndex(IntIntDoubleToDoubleFunction function)
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[rows][columns];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result[i][j] = function(i, j, array[i][j]);
		  }
		}
		return new DoubleMatrix(result, rows, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance where each element is the sum of the matching values
	  /// in this array and the other matrix.
	  /// <para>
	  /// This is used to add two matrices, returning a new matrix.
	  /// Element {@code (i,j)} in the resulting matrix is equal to element {@code (i,j)} in this matrix
	  /// plus element {@code (i,j)} in the other matrix.
	  /// The matrices must be of the same size.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#combine(DoubleMatrix, DoubleBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other matrix </param>
	  /// <returns> a copy of this matrix with matching elements added </returns>
	  /// <exception cref="IllegalArgumentException"> if the matrices have different sizes </exception>
	  public DoubleMatrix plus(DoubleMatrix other)
	  {
		if (rows != other.rows || columns != other.columns)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[rows][columns];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result[i][j] = array[i][j] + other.array[i][j];
		  }
		}
		return new DoubleMatrix(result, rows, columns);
	  }

	  /// <summary>
	  /// Returns an instance where each element is equal to the difference between the
	  /// matching values in this matrix and the other matrix.
	  /// <para>
	  /// This is used to subtract the second matrix from the first, returning a new matrix.
	  /// Element {@code (i,j)} in the resulting matrix is equal to element {@code (i,j)} in this matrix
	  /// minus element {@code (i,j)} in the other matrix.
	  /// The matrices must be of the same size.
	  /// </para>
	  /// <para>
	  /// This is a special case of <seealso cref="#combine(DoubleMatrix, DoubleBinaryOperator)"/>.
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other matrix </param>
	  /// <returns> a copy of this matrix with matching elements subtracted </returns>
	  /// <exception cref="IllegalArgumentException"> if the matrices have different sizes </exception>
	  public DoubleMatrix minus(DoubleMatrix other)
	  {
		if (rows != other.rows || columns != other.columns)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[rows][columns];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result[i][j] = array[i][j] - other.array[i][j];
		  }
		}
		return new DoubleMatrix(result, rows, columns);
	  }

	  /// <summary>
	  /// Returns an instance where each element is formed by some combination of the matching
	  /// values in this matrix and the other matrix.
	  /// <para>
	  /// This is used to combine two matrices, returning a new matrix.
	  /// Element {@code (i,j)} in the resulting matrix is equal to the result of the operator
	  /// when applied to element {@code (i,j)} in this array and element {@code (i,j)} in the other array.
	  /// The arrays must be of the same size.
	  /// </para>
	  /// <para>
	  /// This instance is immutable and unaffected by this method.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other matrix </param>
	  /// <param name="operator">  the operator used to combine each pair of values </param>
	  /// <returns> a copy of this matrix combined with the specified matrix </returns>
	  /// <exception cref="IllegalArgumentException"> if the matrices have different sizes </exception>
	  public DoubleMatrix combine(DoubleMatrix other, System.Func<double, double, double> @operator)
	  {
		if (rows != other.rows || columns != other.columns)
		{
		  throw new System.ArgumentException("Arrays have different sizes");
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[rows][columns];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(rows, columns);
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result[i][j] = @operator(array[i][j], other.array[i][j]);
		  }
		}
		return new DoubleMatrix(result, rows, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the total of all the values in the matrix.
	  /// <para>
	  /// This is a special case of <seealso cref="#reduce(double, DoubleBinaryOperator)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the total of all the values </returns>
	  public double total()
	  {
		double total = 0;
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			total += array[i][j];
		  }
		}
		return total;
	  }

	  /// <summary>
	  /// Reduces this matrix returning a single value.
	  /// <para>
	  /// This is used to reduce the values in this matrix to a single value.
	  /// The operator is called once for each element in the matrix.
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
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < columns; j++)
		  {
			result = @operator(result, array[i][j]);
		  }
		}
		return result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Transposes the matrix.
	  /// <para>
	  /// This converts a matrix of {@code m x n} into a matrix of {@code n x m}.
	  /// Each element is moved to the opposite position.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the transposed matrix </returns>
	  public DoubleMatrix transpose()
	  {
		return DoubleMatrix.of(columns, rows, (i, j) => array[j][i]);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is DoubleMatrix)
		{
		  DoubleMatrix other = (DoubleMatrix) obj;
		  if (columns != other.columns || rows != other.rows)
		  {
			return false;
		  }
		  for (int i = 0; i < rows; i++)
		  {
			for (int j = 0; j < columns; j++)
			{
			  if (System.BitConverter.DoubleToInt64Bits(array[i][j]) != Double.doubleToLongBits(other.array[i][j]))
			  {
				return false;
			  }
			}
		  }
		  return true;
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int result = 1;
		for (int i = 0; i < rows; i++)
		{
		  result = 31 * result + Arrays.GetHashCode(array[i]);
		}
		return result;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder();
		foreach (double[] d in array)
		{
		  for (int i = 0; i < d.Length; i++)
		  {
			buf.Append(d[i]);
			buf.Append(i == d.Length - 1 ? "\n" : " ");
		  }
		}
		return buf.ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DoubleMatrix}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DoubleMatrix.Meta meta()
	  {
		return DoubleMatrix.Meta.INSTANCE;
	  }

	  static DoubleMatrix()
	  {
		MetaBean.register(DoubleMatrix.Meta.INSTANCE);
	  }

	  public override DoubleMatrix.Meta metaBean()
	  {
		return DoubleMatrix.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DoubleMatrix}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  array_Renamed = DirectMetaProperty.ofImmutable(this, "array", typeof(DoubleMatrix), typeof(double[][]));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "array");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code array} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double[][]> array_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "array");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 93090393: // array
			  return array_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DoubleMatrix> builder()
		public override BeanBuilder<DoubleMatrix> builder()
		{
		  return new DoubleMatrix.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DoubleMatrix);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code array} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double[][]> array()
		{
		  return array_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 93090393: // array
			  return ((DoubleMatrix) bean).array;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code DoubleMatrix}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DoubleMatrix>
	  {

		internal double[][] array;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 93090393: // array
			  return array;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 93090393: // array
			  this.array = (double[][]) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DoubleMatrix build()
		{
		  return new DoubleMatrix(array);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("DoubleMatrix.Builder{");
		  buf.Append("array").Append('=').Append(JodaBeanUtils.ToString(array));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
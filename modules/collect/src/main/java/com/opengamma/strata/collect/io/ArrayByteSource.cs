using System.IO;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using Optional = com.google.common.@base.Optional;
	using HashCode = com.google.common.hash.HashCode;
	using HashFunction = com.google.common.hash.HashFunction;
	using ByteProcessor = com.google.common.io.ByteProcessor;
	using ByteSource = com.google.common.io.ByteSource;
	using ByteStreams = com.google.common.io.ByteStreams;
	using CharSource = com.google.common.io.CharSource;
	using CheckedSupplier = com.opengamma.strata.collect.function.CheckedSupplier;

	/// <summary>
	/// A byte source implementation that explicitly wraps a byte array.
	/// <para>
	/// This implementation allows <seealso cref="IOException"/> to be avoided in many cases,
	/// and to be able to create and retrieve the internal array unsafely.
	/// </para>
	/// </summary>
	public sealed class ArrayByteSource : ByteSource
	{

	  /// <summary>
	  /// An empty source.
	  /// </summary>
	  public static readonly ArrayByteSource EMPTY = new ArrayByteSource(new sbyte[0]);

	  /// <summary>
	  /// The byte array.
	  /// </summary>
	  private readonly sbyte[] array;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, copying the array.
	  /// </summary>
	  /// <param name="array">  the array, copied </param>
	  /// <returns> the byte source </returns>
	  public static ArrayByteSource copyOf(sbyte[] array)
	  {
		return new ArrayByteSource(array.Clone());
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
	  public static ArrayByteSource copyOf(sbyte[] array, int fromIndex)
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
	  public static ArrayByteSource copyOf(sbyte[] array, int fromIndexInclusive, int toIndexExclusive)
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
		return new ArrayByteSource(Arrays.copyOfRange(array, fromIndexInclusive, toIndexExclusive));
	  }

	  /// <summary>
	  /// Creates an instance, not copying the array.
	  /// <para>
	  /// This method is inherently unsafe as it relies on good behavior by callers.
	  /// Callers must never make any changes to the passed in array after calling this method.
	  /// Doing so would violate the immutability of this class.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array, not copied </param>
	  /// <returns> the byte source </returns>
	  public static ArrayByteSource ofUnsafe(sbyte[] array)
	  {
		return new ArrayByteSource(array);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from another byte source.
	  /// </summary>
	  /// <param name="other">  the other byte source </param>
	  /// <returns> the byte source </returns>
	  /// <exception cref="UncheckedIOException"> if an IO error occurs </exception>
	  public static ArrayByteSource from(ByteSource other)
	  {
		if (other is ArrayByteSource)
		{
		  return (ArrayByteSource) other;
		}
		return new ArrayByteSource(Unchecked.wrap(() => other.read()));
	  }

	  /// <summary>
	  /// Creates an instance from an input stream.
	  /// <para>
	  /// This method use the supplier to open the input stream, extract the bytes and close the stream.
	  /// It is intended that invoking the supplier opens the stream.
	  /// It is not intended that an already open stream is supplied.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="inputStreamSupplier">  the supplier of the input stream </param>
	  /// <returns> the byte source </returns>
	  /// <exception cref="UncheckedIOException"> if an IO error occurs </exception>
	  public static ArrayByteSource from(CheckedSupplier<Stream> inputStreamSupplier)
	  {
		return Unchecked.wrap(() =>
		{
		using (Stream @in = inputStreamSupplier())
		{
			sbyte[] bytes = Unchecked.wrap(() => ByteStreams.toByteArray(@in));
			return new ArrayByteSource(bytes);
		}
		});
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, without copying the array.
	  /// </summary>
	  /// <param name="array">  the array, not copied </param>
	  private ArrayByteSource(sbyte[] array)
	  {
		this.array = array;
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
	  public sbyte[] readUnsafe()
	  {
		return array;
	  }

	  /// <summary>
	  /// Reads the source, converting to UTF-8.
	  /// </summary>
	  /// <returns> the UTF-8 string </returns>
	  public string readUtf8()
	  {
		return StringHelper.NewString(array, StandardCharsets.UTF_8);
	  }

	  /// <summary>
	  /// Reads the source, converting to UTF-8 using a Byte-Order Mark if available.
	  /// </summary>
	  /// <returns> the UTF-8 string </returns>
	  public string readUtf8UsingBom()
	  {
		return UnicodeBom.ToString(array);
	  }

	  /// <summary>
	  /// Returns a {@code CharSource} for the same bytes, converted to UTF-8 using a Byte-Order Mark if available.
	  /// </summary>
	  /// <returns> the equivalent {@code CharSource} </returns>
	  public CharSource asCharSourceUtf8UsingBom()
	  {
		return CharSource.wrap(readUtf8UsingBom());
	  }

	  //-------------------------------------------------------------------------
	  public override MemoryStream openStream()
	  {
		return new MemoryStream(array);
	  }

	  public override MemoryStream openBufferedStream()
	  {
		return openStream();
	  }

	  public override bool Empty
	  {
		  get
		  {
			return array.Length == 0;
		  }
	  }

	  /// <summary>
	  /// Gets the size, which is always known.
	  /// </summary>
	  /// <returns> the size, which is always known </returns>
	  public override long? sizeIfKnown()
	  {
		return Optional.of(size());
	  }

	  public override long size()
	  {
		return array.Length;
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public long copyTo(java.io.OutputStream output) throws java.io.IOException
	  public override long copyTo(Stream output)
	  {
		output.Write(array, 0, array.Length);
		return array.Length;
	  }

	  public override sbyte[] read()
	  {
		return array.Clone();
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public <T> T read(com.google.common.io.ByteProcessor<T> processor) throws java.io.IOException
	  public override T read<T>(ByteProcessor<T> processor)
	  {
		processor.processBytes(array, 0, array.Length);
		return processor.Result;
	  }

	  public override HashCode hash(HashFunction hashFunction)
	  {
		return hashFunction.hashBytes(array);
	  }

	  public override string ToString()
	  {
		return "ArrayByteSource[" + size() + " bytes]";
	  }

	}

}
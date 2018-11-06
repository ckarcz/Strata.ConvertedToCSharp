using System.IO;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{

	using ByteSource = com.google.common.io.ByteSource;
	using ByteStreams = com.google.common.io.ByteStreams;
	using CharSource = com.google.common.io.CharSource;

	/// <summary>
	/// Utilities that allow code to use the Unicode Byte Order Mark.
	/// <para>
	/// A Unicode file may contain a Byte Order Mark (BOM) that specifies which
	/// encoding is used. Sadly, neither the JDK nor Guava handle this properly.
	/// </para>
	/// <para>
	/// This class supports the BOM for UTF-8, UTF-16LE and UTF-16BE.
	/// The UTF-32 formats are rarely seen and cannot be easily determined as
	/// the UTF-32 BOMs are similar to the UTF-16 BOMs.
	/// </para>
	/// </summary>
	public sealed class UnicodeBom
	{

	  private static readonly sbyte X_FE = unchecked((sbyte) 0xFE);
	  private static readonly sbyte X_EF = unchecked((sbyte) 0xEF);
	  private static readonly sbyte X_FF = unchecked((sbyte) 0xFF);
	  private static readonly sbyte X_BF = unchecked((sbyte) 0xBF);
	  private static readonly sbyte X_BB = unchecked((sbyte) 0xBB);

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private UnicodeBom()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a {@code byte[]} to a {@code String}.
	  /// <para>
	  /// This ensures that any Unicode byte order marker is used correctly.
	  /// The default encoding is UTF-8 if no BOM is found.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="input">  the input byte array </param>
	  /// <returns> the equivalent string </returns>
	  public static string ToString(sbyte[] input)
	  {
		if (input.Length >= 3 && input[0] == X_EF && input[1] == X_BB && input[2] == X_BF)
		{
		  return StringHelper.NewString(input, 3, input.Length - 3, StandardCharsets.UTF_8);

		}
		else if (input.Length >= 2 && input[0] == X_FE && input[1] == X_FF)
		{
		  return StringHelper.NewString(input, 2, input.Length - 2, StandardCharsets.UTF_16BE);

		}
		else if (input.Length >= 2 && input[0] == X_FF && input[1] == X_FE)
		{
		  return StringHelper.NewString(input, 2, input.Length - 2, StandardCharsets.UTF_16LE);

		}
		else
		{
		  return StringHelper.NewString(input, StandardCharsets.UTF_8);
		}
	  }

	  /// <summary>
	  /// Converts a {@code ByteSource} to a {@code CharSource}.
	  /// <para>
	  /// This ensures that any Unicode byte order marker is used correctly.
	  /// The default encoding is UTF-8 if no BOM is found.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="byteSource">  the byte source </param>
	  /// <returns> the char source, that uses the BOM to determine the encoding </returns>
	  public static CharSource toCharSource(ByteSource byteSource)
	  {
		return new CharSourceAnonymousInnerClass(byteSource);
	  }

	  private class CharSourceAnonymousInnerClass : CharSource
	  {
		  private ByteSource byteSource;

		  public CharSourceAnonymousInnerClass(ByteSource byteSource)
		  {
			  this.byteSource = byteSource;
		  }


		  public override ByteSource asByteSource(Charset charset)
		  {
			return byteSource;
		  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public java.io.Reader openStream() throws java.io.IOException
		  public override Reader openStream()
		  {
			return toReader(byteSource.openStream());
		  }

		  public override string ToString()
		  {
			return "UnicodeBom.toCharSource(" + byteSource.ToString() + ")";
		  }
	  }

	  /// <summary>
	  /// Converts an {@code InputStream} to a {@code Reader}.
	  /// <para>
	  /// This ensures that any Unicode byte order marker is used correctly.
	  /// The default encoding is UTF-8 if no BOM is found.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="inputStream">  the input stream to wrap </param>
	  /// <returns> the reader, that uses the BOM to determine the encoding </returns>
	  /// <exception cref="IOException"> if an IO error occurs </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static java.io.Reader toReader(java.io.InputStream inputStream) throws java.io.IOException
	  public static Reader toReader(Stream inputStream)
	  {
		return new BomReader(inputStream);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Reader that manages the BOM.
	  /// </summary>
	  private sealed class BomReader : Reader
	  {

		internal const int MAX_BOM_SIZE = 4;

		internal readonly StreamReader underlying;

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: BomReader(java.io.InputStream inputStream) throws java.io.IOException
		internal BomReader(Stream inputStream) : base(inputStream)
		{

		  Charset encoding;
		  sbyte[] bom = new sbyte[MAX_BOM_SIZE];

		  // read first 3 bytes such that they can be pushed back later
		  PushbackInputStream pushbackStream = new PushbackInputStream(inputStream, MAX_BOM_SIZE);
		  int bytesRead = ByteStreams.read(pushbackStream, bom, 0, 3);

		  // look for BOM and adapt, defauling to UTF-8
		  if (bytesRead >= 3 && bom[0] == X_EF && bom[1] == X_BB && bom[2] == X_BF)
		  {
			encoding = StandardCharsets.UTF_8;
			pushbackStream.unread(bom, 3, (bytesRead - 3));

		  }
		  else if (bytesRead >= 2 && bom[0] == X_FE && bom[1] == X_FF)
		  {
			encoding = StandardCharsets.UTF_16BE;
			pushbackStream.unread(bom, 2, (bytesRead - 2));

		  }
		  else if (bytesRead >= 2 && bom[0] == X_FF && bom[1] == X_FE)
		  {
			encoding = StandardCharsets.UTF_16LE;
			pushbackStream.unread(bom, 2, (bytesRead - 2));

		  }
		  else
		  {
			encoding = StandardCharsets.UTF_8;
			pushbackStream.unread(bom, 0, bytesRead);
		  }

		  // use Java standard code now we know the encoding
		  this.underlying = new StreamReader(pushbackStream, encoding);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public int read(java.nio.CharBuffer target) throws java.io.IOException
		public override int read(CharBuffer target)
		{
		  return underlying.Read(target, 0, target.Length);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public int read() throws java.io.IOException
		public override int read()
		{
		  return underlying.Read();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public int read(char[] cbuf) throws java.io.IOException
		public override int read(char[] cbuf)
		{
		  return underlying.Read(cbuf, 0, cbuf.Length);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public int read(char[] cbuf, int off, int len) throws java.io.IOException
		public override int read(char[] cbuf, int off, int len)
		{
		  return underlying.Read(cbuf, off, len);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public long skip(long n) throws java.io.IOException
		public override long skip(long n)
		{
		  return underlying.skip(n);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public boolean ready() throws java.io.IOException
		public override bool ready()
		{
		  return underlying.ready();
		}

		public override bool markSupported()
		{
		  return underlying.markSupported();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void mark(int readAheadLimit) throws java.io.IOException
		public override void mark(int readAheadLimit)
		{
		  underlying.mark(readAheadLimit);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void reset() throws java.io.IOException
		public override void reset()
		{
		  underlying.reset();
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void close() throws java.io.IOException
		public override void close()
		{
		  underlying.Close();
		}
	  }

	}

}
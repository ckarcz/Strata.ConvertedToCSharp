using System.IO;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;


	using Test = org.testng.annotations.Test;

	using ByteSource = com.google.common.io.ByteSource;
	using CheckedSupplier = com.opengamma.strata.collect.function.CheckedSupplier;

	/// <summary>
	/// Test <seealso cref="ArrayByteSource"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ArrayByteSourceTest
	public class ArrayByteSourceTest
	{

	  public virtual void test_EMPTY()
	  {
		ArrayByteSource test = ArrayByteSource.EMPTY;
		assertEquals(test.Empty, true);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_copyOf()
	  {
		sbyte[] bytes = new sbyte[] {1, 2, 3};
		ArrayByteSource test = ArrayByteSource.copyOf(bytes);
		assertEquals(test.size(), 3);
		assertEquals(test.read()[0], 1);
		assertEquals(test.read()[1], 2);
		assertEquals(test.read()[2], 3);
		bytes[0] = 4;
		assertEquals(test.read()[0], 1);
	  }

	  public virtual void test_copyOf_from()
	  {
		sbyte[] bytes = new sbyte[] {1, 2, 3};
		ArrayByteSource test = ArrayByteSource.copyOf(bytes, 1);
		assertEquals(test.size(), 2);
		assertEquals(test.read()[0], 2);
		assertEquals(test.read()[1], 3);
	  }

	  public virtual void test_copyOf_fromTo()
	  {
		sbyte[] bytes = new sbyte[] {1, 2, 3};
		ArrayByteSource test = ArrayByteSource.copyOf(bytes, 1, 2);
		assertEquals(test.size(), 1);
		assertEquals(test.read()[0], 2);
	  }

	  public virtual void test_copyOf_fromTo_empty()
	  {
		sbyte[] bytes = new sbyte[] {1, 2, 3};
		ArrayByteSource test = ArrayByteSource.copyOf(bytes, 1, 1);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_copyOf_fromTo_bad()
	  {
		sbyte[] bytes = new sbyte[] {1, 2, 3};
		assertThrows(typeof(System.IndexOutOfRangeException), () => ArrayByteSource.copyOf(bytes, -1, 2));
		assertThrows(typeof(System.IndexOutOfRangeException), () => ArrayByteSource.copyOf(bytes, 0, 4));
		assertThrows(typeof(System.IndexOutOfRangeException), () => ArrayByteSource.copyOf(bytes, 4, 5));
	  }

	  public virtual void test_ofUnsafe()
	  {
		sbyte[] bytes = new sbyte[] {1, 2, 3};
		ArrayByteSource test = ArrayByteSource.ofUnsafe(bytes);
		assertEquals(test.size(), 3);
		assertEquals(test.read()[0], 1);
		assertEquals(test.read()[1], 2);
		assertEquals(test.read()[2], 3);
		bytes[0] = 4; // abusing the unsafe factory
		assertEquals(test.read()[0], 4);
	  }

	  public virtual void test_from_ByteSource()
	  {
		ByteSource source = ByteSource.wrap(new sbyte[] {1, 2, 3});
		ArrayByteSource test = ArrayByteSource.from(source);
		assertEquals(test.size(), 3);
		assertEquals(test.read()[0], 1);
		assertEquals(test.read()[1], 2);
		assertEquals(test.read()[2], 3);
	  }

	  public virtual void test_from_ByteSource_alreadyArrayByteSource()
	  {
		ArrayByteSource @base = ArrayByteSource.copyOf(new sbyte[] {1, 2, 3});
		ArrayByteSource test = ArrayByteSource.from(@base);
		assertSame(test, @base);
	  }

	  public virtual void test_from_Supplier()
	  {
		ByteSource source = ByteSource.wrap(new sbyte[] {1, 2, 3});
		ArrayByteSource test = ArrayByteSource.from(() => source.openStream());
		assertEquals(test.size(), 3);
		assertEquals(test.read()[0], 1);
		assertEquals(test.read()[1], 2);
		assertEquals(test.read()[2], 3);
	  }

	  public virtual void test_from_SupplierExceptionOnCreate()
	  {
		CheckedSupplier<Stream> supplier = () =>
		{
	  throw new IOException();
		};
		assertThrows(typeof(UncheckedIOException), () => ArrayByteSource.from(supplier));
	  }

	  public virtual void test_from_SupplierExceptionOnRead()
	  {
		CheckedSupplier<Stream> supplier = () =>
		{
	  return new InputStreamAnonymousInnerClass(this);
		};
		assertThrows(typeof(UncheckedIOException), () => ArrayByteSource.from(supplier));
	  }

	  private class InputStreamAnonymousInnerClass : Stream
	  {
		  private readonly ArrayByteSourceTest outerInstance;

		  public InputStreamAnonymousInnerClass(ArrayByteSourceTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public int read() throws java.io.IOException
		  public override int read()
		  {
			throw new IOException();
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_read()
	  {
		ArrayByteSource test = ArrayByteSource.copyOf(new sbyte[] {1, 2, 3});
		assertEquals(test.size(), 3);
		sbyte[] safeArray = test.read();
		safeArray[0] = 4;
		assertEquals(test.read()[0], 1);
		assertEquals(test.read()[1], 2);
		assertEquals(test.read()[2], 3);
	  }

	  public virtual void test_readUnsafe()
	  {
		ArrayByteSource test = ArrayByteSource.copyOf(new sbyte[] {1, 2, 3});
		assertEquals(test.size(), 3);
		sbyte[] unsafeArray = test.readUnsafe();
		unsafeArray[0] = 4; // abusing the unsafe array
		assertEquals(test.read()[0], 4);
		assertEquals(test.read()[1], 2);
		assertEquals(test.read()[2], 3);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_methods() throws java.io.IOException
	  public virtual void test_methods()
	  {
		ArrayByteSource test = ArrayByteSource.copyOf(new sbyte[] {65, 66, 67});
		assertEquals(test.size(), 3);
		assertEquals(test.Empty, false);
		assertEquals(test.sizeIfKnown().Present, true);
		assertEquals(test.sizeIfKnown().get(), (long?) 3L);
		assertEquals(test.readUtf8(), "ABC");
		assertEquals(test.readUtf8UsingBom(), "ABC");
		assertEquals(test.asCharSourceUtf8UsingBom().read(), "ABC");
		assertEquals(test.ToString(), "ArrayByteSource[3 bytes]");
	  }

	}

}
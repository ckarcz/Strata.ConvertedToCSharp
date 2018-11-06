/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="TypedString"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TypedStringTest
	public class TypedStringTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of()
	  {
		SampleType test = SampleType.of("A");
		assertEquals(test.ToString(), "A");
	  }

	  public virtual void test_of_invalid()
	  {
		assertThrowsIllegalArg(() => SampleType.of(null));
		assertThrowsIllegalArg(() => SampleType.of(""));
	  }

	  public virtual void test_of_validated()
	  {
		SampleValidatedType test = SampleValidatedType.of("ABC");
		assertEquals(test.ToString(), "ABC");
	  }

	  public virtual void test_of_validated_invalid()
	  {
		assertThrowsIllegalArg(() => SampleValidatedType.of(null));
		assertThrowsIllegalArg(() => SampleValidatedType.of("ABc"));
	  }

	  public virtual void test_equalsHashCode()
	  {
		SampleType a1 = SampleType.of("A");
		SampleType a2 = SampleType.of("A");
		SampleType b = SampleType.of("B");

		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_compareTo()
	  {
		SampleType a = SampleType.of("A");
		SampleType b = SampleType.of("B");
		SampleType c = SampleType.of("C");

		assertEquals(a.CompareTo(a) == 0, true);
		assertEquals(a.CompareTo(b) < 0, true);
		assertEquals(a.CompareTo(c) < 0, true);

		assertEquals(b.CompareTo(a) > 0, true);
		assertEquals(b.CompareTo(b) == 0, true);
		assertEquals(b.CompareTo(c) < 0, true);

		assertEquals(c.CompareTo(a) > 0, true);
		assertEquals(c.CompareTo(b) > 0, true);
		assertEquals(c.CompareTo(c) == 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(SampleType.of("A"));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(SampleType), SampleType.of("A"));
	  }

	}

}
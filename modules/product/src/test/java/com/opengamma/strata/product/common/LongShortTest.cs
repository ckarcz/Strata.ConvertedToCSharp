/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="LongShort"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LongShortTest
	public class LongShortTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_ofLong()
	  {
		assertEquals(LongShort.ofLong(true), LongShort.LONG);
		assertEquals(LongShort.ofLong(false), LongShort.SHORT);
	  }

	  public virtual void test_isLong()
	  {
		assertEquals(LongShort.LONG.Long, true);
		assertEquals(LongShort.SHORT.Long, false);
	  }

	  public virtual void test_isShort()
	  {
		assertEquals(LongShort.LONG.Short, false);
		assertEquals(LongShort.SHORT.Short, true);
	  }

	  public virtual void test_sign()
	  {
		assertEquals(LongShort.LONG.sign(), 1);
		assertEquals(LongShort.SHORT.sign(), -1);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {LongShort.LONG, "Long"},
			new object[] {LongShort.SHORT, "Short"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(LongShort convention, String name)
	  public virtual void test_toString(LongShort convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(LongShort convention, String name)
	  public virtual void test_of_lookup(LongShort convention, string name)
	  {
		assertEquals(LongShort.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(LongShort convention, String name)
	  public virtual void test_of_lookupUpperCase(LongShort convention, string name)
	  {
		assertEquals(LongShort.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(LongShort convention, String name)
	  public virtual void test_of_lookupLowerCase(LongShort convention, string name)
	  {
		assertEquals(LongShort.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => LongShort.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => LongShort.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(LongShort));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(LongShort.LONG);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(LongShort), LongShort.LONG);
	  }

	}

}
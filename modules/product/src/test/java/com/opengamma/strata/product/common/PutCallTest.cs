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
	/// Test <seealso cref="PutCall"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PutCallTest
	public class PutCallTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_ofPut()
	  {
		assertEquals(PutCall.ofPut(true), PutCall.PUT);
		assertEquals(PutCall.ofPut(false), PutCall.CALL);
	  }

	  public virtual void test_isPut()
	  {
		assertEquals(PutCall.PUT.Put, true);
		assertEquals(PutCall.CALL.Put, false);
	  }

	  public virtual void test_isCall()
	  {
		assertEquals(PutCall.PUT.Call, false);
		assertEquals(PutCall.CALL.Call, true);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {PutCall.PUT, "Put"},
			new object[] {PutCall.CALL, "Call"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(PutCall convention, String name)
	  public virtual void test_toString(PutCall convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(PutCall convention, String name)
	  public virtual void test_of_lookup(PutCall convention, string name)
	  {
		assertEquals(PutCall.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(PutCall convention, String name)
	  public virtual void test_of_lookupUpperCase(PutCall convention, string name)
	  {
		assertEquals(PutCall.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(PutCall convention, String name)
	  public virtual void test_of_lookupLowerCase(PutCall convention, string name)
	  {
		assertEquals(PutCall.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => PutCall.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => PutCall.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(PutCall));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PutCall.PUT);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(PutCall), PutCall.PUT);
	  }

	}

}
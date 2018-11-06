/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.option
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
	/// Test <seealso cref="KnockType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class KnockTypeTest
	public class KnockTypeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_isKnockIn()
	  {
		assertEquals(KnockType.KNOCK_IN.KnockIn, true);
		assertEquals(KnockType.KNOCK_OUT.KnockIn, false);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {KnockType.KNOCK_IN, "KnockIn"},
			new object[] {KnockType.KNOCK_OUT, "KnockOut"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(KnockType type, String name)
	  public virtual void test_toString(KnockType type, string name)
	  {
		assertEquals(type.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(KnockType type, String name)
	  public virtual void test_of_lookup(KnockType type, string name)
	  {
		assertEquals(KnockType.of(name), type);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => KnockType.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => KnockType.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(KnockType));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(KnockType.KNOCK_OUT);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(KnockType), KnockType.KNOCK_IN);
	  }

	}

}
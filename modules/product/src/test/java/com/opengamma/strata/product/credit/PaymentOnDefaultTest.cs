/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="PaymentOnDefault"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PaymentOnDefaultTest
	public class PaymentOnDefaultTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {PaymentOnDefault.NONE, "None"},
			new object[] {PaymentOnDefault.ACCRUED_PREMIUM, "AccruedPremium"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(PaymentOnDefault convention, String name)
	  public virtual void test_toString(PaymentOnDefault convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(PaymentOnDefault convention, String name)
	  public virtual void test_of_lookup(PaymentOnDefault convention, string name)
	  {
		assertEquals(PaymentOnDefault.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => PaymentOnDefault.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => PaymentOnDefault.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(PaymentOnDefault));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PaymentOnDefault.ACCRUED_PREMIUM);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(PaymentOnDefault), PaymentOnDefault.ACCRUED_PREMIUM);
	  }

	}

}
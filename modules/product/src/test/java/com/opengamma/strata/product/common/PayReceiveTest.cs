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
	/// Test <seealso cref="PayReceive"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PayReceiveTest
	public class PayReceiveTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_ofPay()
	  {
		assertEquals(PayReceive.ofPay(true), PayReceive.PAY);
		assertEquals(PayReceive.ofPay(false), PayReceive.RECEIVE);
	  }

	  public virtual void test_ofSignedAmount()
	  {
		assertEquals(PayReceive.ofSignedAmount(-1d), PayReceive.PAY);
		assertEquals(PayReceive.ofSignedAmount(-0d), PayReceive.PAY);
		assertEquals(PayReceive.ofSignedAmount(0d), PayReceive.RECEIVE);
		assertEquals(PayReceive.ofSignedAmount(+0d), PayReceive.RECEIVE);
		assertEquals(PayReceive.ofSignedAmount(1d), PayReceive.RECEIVE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize_pay()
	  {
		assertEquals(PayReceive.PAY.normalize(1d), -1d, 0d);
		assertEquals(PayReceive.PAY.normalize(0d), -0d, 0d);
		assertEquals(PayReceive.PAY.normalize(-1d), -1d, 0d);
	  }

	  public virtual void test_normalize_receive()
	  {
		assertEquals(PayReceive.RECEIVE.normalize(1d), 1d, 0d);
		assertEquals(PayReceive.RECEIVE.normalize(0d), 0d, 0d);
		assertEquals(PayReceive.RECEIVE.normalize(-1d), 1d, 0d);
	  }

	  public virtual void test_isPay()
	  {
		assertEquals(PayReceive.PAY.Pay, true);
		assertEquals(PayReceive.RECEIVE.Pay, false);
	  }

	  public virtual void test_isReceive()
	  {
		assertEquals(PayReceive.PAY.Receive, false);
		assertEquals(PayReceive.RECEIVE.Receive, true);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {PayReceive.PAY, "Pay"},
			new object[] {PayReceive.RECEIVE, "Receive"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(PayReceive convention, String name)
	  public virtual void test_toString(PayReceive convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(PayReceive convention, String name)
	  public virtual void test_of_lookup(PayReceive convention, string name)
	  {
		assertEquals(PayReceive.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(PayReceive convention, String name)
	  public virtual void test_of_lookupUpperCase(PayReceive convention, string name)
	  {
		assertEquals(PayReceive.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(PayReceive convention, String name)
	  public virtual void test_of_lookupLowerCase(PayReceive convention, string name)
	  {
		assertEquals(PayReceive.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => PayReceive.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => PayReceive.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(PayReceive));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PayReceive.PAY);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(PayReceive), PayReceive.PAY);
	  }

	}

}
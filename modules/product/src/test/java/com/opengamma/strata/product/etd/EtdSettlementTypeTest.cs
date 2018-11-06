/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
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
	/// Test <seealso cref="EtdSettlementType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdSettlementTypeTest
	public class EtdSettlementTypeTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {EtdSettlementType.CASH, "Cash"},
			new object[] {EtdSettlementType.PHYSICAL, "Physical"},
			new object[] {EtdSettlementType.DERIVATIVE, "Derivative"},
			new object[] {EtdSettlementType.PAYMENT_VS_PAYMENT, "PaymentVsPayment"},
			new object[] {EtdSettlementType.NOTIONAL, "Notional"},
			new object[] {EtdSettlementType.STOCK, "Stock"},
			new object[] {EtdSettlementType.CASCADE, "Cascade"},
			new object[] {EtdSettlementType.ALTERNATE, "Alternate"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(EtdSettlementType convention, String name)
	  public virtual void test_toString(EtdSettlementType convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(EtdSettlementType convention, String name)
	  public virtual void test_of_lookup(EtdSettlementType convention, string name)
	  {
		assertEquals(EtdSettlementType.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => EtdSettlementType.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => EtdSettlementType.of(null), typeof(System.ArgumentException));
	  }

	  public virtual void test_getCode()
	  {
		assertEquals(EtdSettlementType.CASH.Code, "C");
		assertEquals(EtdSettlementType.PHYSICAL.Code, "E");
		assertEquals(EtdSettlementType.DERIVATIVE.Code, "D");
		assertEquals(EtdSettlementType.NOTIONAL.Code, "N");
		assertEquals(EtdSettlementType.PAYMENT_VS_PAYMENT.Code, "P");
		assertEquals(EtdSettlementType.STOCK.Code, "S");
		assertEquals(EtdSettlementType.CASCADE.Code, "T");
		assertEquals(EtdSettlementType.ALTERNATE.Code, "A");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(EtdSettlementType));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(EtdSettlementType.DERIVATIVE);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(EtdSettlementType), EtdSettlementType.STOCK);
	  }

	}

}
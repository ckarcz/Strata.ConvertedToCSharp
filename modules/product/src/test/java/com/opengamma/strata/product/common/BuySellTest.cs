/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="BuySell"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BuySellTest
	public class BuySellTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_ofBuy()
	  {
		assertEquals(BuySell.ofBuy(true), BuySell.BUY);
		assertEquals(BuySell.ofBuy(false), BuySell.SELL);
	  }

	  public virtual void test_normalize_sell()
	  {
		assertEquals(BuySell.SELL.normalize(1d), -1d, 0d);
		assertEquals(BuySell.SELL.normalize(0d), -0d, 0d);
		assertEquals(BuySell.SELL.normalize(-1d), -1d, 0d);
	  }

	  public virtual void test_normalize_buy()
	  {
		assertEquals(BuySell.BUY.normalize(1d), 1d, 0d);
		assertEquals(BuySell.BUY.normalize(0d), 0d, 0d);
		assertEquals(BuySell.BUY.normalize(-1d), 1d, 0d);
	  }

	  public virtual void test_isBuy()
	  {
		assertEquals(BuySell.BUY.Buy, true);
		assertEquals(BuySell.SELL.Buy, false);
	  }

	  public virtual void test_isSell()
	  {
		assertEquals(BuySell.BUY.Sell, false);
		assertEquals(BuySell.SELL.Sell, true);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {BuySell.BUY, "Buy"},
			new object[] {BuySell.SELL, "Sell"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(BuySell convention, String name)
	  public virtual void test_toString(BuySell convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(BuySell convention, String name)
	  public virtual void test_of_lookup(BuySell convention, string name)
	  {
		assertEquals(BuySell.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(BuySell convention, String name)
	  public virtual void test_of_lookupUpperCase(BuySell convention, string name)
	  {
		assertEquals(BuySell.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(BuySell convention, String name)
	  public virtual void test_of_lookupLowerCase(BuySell convention, string name)
	  {
		assertEquals(BuySell.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => BuySell.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => BuySell.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(BuySell));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(BuySell.BUY);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(BuySell), BuySell.BUY);
	  }

	}

}
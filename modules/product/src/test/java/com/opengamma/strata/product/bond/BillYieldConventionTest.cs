/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
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
	/// Test <seealso cref="BillYieldConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BillYieldConventionTest
	public class BillYieldConventionTest
	{

	  public const double PRICE = 0.99;
	  public const double YIELD = 0.03;
	  public const double ACCRUAL_FACTOR = 0.123;
	  public const double TOLERANCE = 1.0E-10;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {BillYieldConvention.DISCOUNT, "Discount"},
			new object[] {BillYieldConvention.FRANCE_CD, "France-CD"},
			new object[] {BillYieldConvention.INTEREST_AT_MATURITY, "Interest-At-Maturity"},
			new object[] {BillYieldConvention.JAPAN_BILLS, "Japan-Bills"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(BillYieldConvention convention, String name)
	  public virtual void test_toString(BillYieldConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(BillYieldConvention convention, String name)
	  public virtual void test_of_lookup(BillYieldConvention convention, string name)
	  {
		assertEquals(BillYieldConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(BillYieldConvention convention, String name)
	  public virtual void test_of_lookupUpperCase(BillYieldConvention convention, string name)
	  {
		assertEquals(BillYieldConvention.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(BillYieldConvention convention, String name)
	  public virtual void test_of_lookupLowerCase(BillYieldConvention convention, string name)
	  {
		assertEquals(BillYieldConvention.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupStandard(BillYieldConvention convention, String name)
	  public virtual void test_of_lookupStandard(BillYieldConvention convention, string name)
	  {
		assertEquals(BillYieldConvention.of(convention.name()), convention);
	  }

	  public virtual void test_price_yield_discount()
	  {
		assertEquals(BillYieldConvention.DISCOUNT.priceFromYield(YIELD, ACCRUAL_FACTOR), 1.0d - ACCRUAL_FACTOR * YIELD, TOLERANCE);
	  }

	  public virtual void test_price_yield_france()
	  {
		assertEquals(BillYieldConvention.FRANCE_CD.priceFromYield(YIELD, ACCRUAL_FACTOR), 1.0d / (1.0d + ACCRUAL_FACTOR * YIELD), TOLERANCE);
	  }

	  public virtual void test_price_yield_intatmaturity()
	  {
		assertEquals(BillYieldConvention.INTEREST_AT_MATURITY.priceFromYield(YIELD, ACCRUAL_FACTOR), 1.0d / (1.0d + ACCRUAL_FACTOR * YIELD), TOLERANCE);
	  }

	  public virtual void test_price_yield_japan()
	  {
		assertEquals(BillYieldConvention.JAPAN_BILLS.priceFromYield(YIELD, ACCRUAL_FACTOR), 1.0d / (1.0d + ACCRUAL_FACTOR * YIELD), TOLERANCE);
	  }

	  public virtual void test_yield_price_discount()
	  {
		assertEquals(BillYieldConvention.DISCOUNT.yieldFromPrice(PRICE, ACCRUAL_FACTOR), (1.0d - PRICE) / ACCRUAL_FACTOR, TOLERANCE);
	  }

	  public virtual void test_yield_price_france()
	  {
		assertEquals(BillYieldConvention.FRANCE_CD.yieldFromPrice(PRICE, ACCRUAL_FACTOR), (1.0d / PRICE - 1.0d) / ACCRUAL_FACTOR, TOLERANCE);
	  }

	  public virtual void test_yield_price_intatmaturity()
	  {
		assertEquals(BillYieldConvention.INTEREST_AT_MATURITY.yieldFromPrice(PRICE, ACCRUAL_FACTOR), (1.0d / PRICE - 1.0d) / ACCRUAL_FACTOR, TOLERANCE);
	  }

	  public virtual void test_yield_price_japan()
	  {
		assertEquals(BillYieldConvention.JAPAN_BILLS.yieldFromPrice(PRICE, ACCRUAL_FACTOR), (1.0d / PRICE - 1.0d) / ACCRUAL_FACTOR, TOLERANCE);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => BillYieldConvention.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => BillYieldConvention.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(BillYieldConvention));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(typeof(BillYieldConvention));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(BillYieldConvention), BillYieldConvention.DISCOUNT);
	  }

	}

}
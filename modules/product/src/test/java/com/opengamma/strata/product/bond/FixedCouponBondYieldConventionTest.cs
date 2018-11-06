/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="FixedCouponBondYieldConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedCouponBondYieldConventionTest
	public class FixedCouponBondYieldConventionTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {FixedCouponBondYieldConvention.GB_BUMP_DMO, "GB-Bump-DMO"},
			new object[] {FixedCouponBondYieldConvention.US_STREET, "US-Street"},
			new object[] {FixedCouponBondYieldConvention.DE_BONDS, "DE-Bonds"},
			new object[] {FixedCouponBondYieldConvention.JP_SIMPLE, "JP-Simple"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FixedCouponBondYieldConvention convention, String name)
	  public virtual void test_toString(FixedCouponBondYieldConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FixedCouponBondYieldConvention convention, String name)
	  public virtual void test_of_lookup(FixedCouponBondYieldConvention convention, string name)
	  {
		assertEquals(FixedCouponBondYieldConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(FixedCouponBondYieldConvention convention, String name)
	  public virtual void test_of_lookupUpperCase(FixedCouponBondYieldConvention convention, string name)
	  {
		assertEquals(FixedCouponBondYieldConvention.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(FixedCouponBondYieldConvention convention, String name)
	  public virtual void test_of_lookupLowerCase(FixedCouponBondYieldConvention convention, string name)
	  {
		assertEquals(FixedCouponBondYieldConvention.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupStandard(FixedCouponBondYieldConvention convention, String name)
	  public virtual void test_of_lookupStandard(FixedCouponBondYieldConvention convention, string name)
	  {
		assertEquals(FixedCouponBondYieldConvention.of(convention.name()), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => FixedCouponBondYieldConvention.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => FixedCouponBondYieldConvention.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(FixedCouponBondYieldConvention));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FixedCouponBondYieldConvention.GB_BUMP_DMO);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FixedCouponBondYieldConvention), FixedCouponBondYieldConvention.GB_BUMP_DMO);
	  }

	}

}
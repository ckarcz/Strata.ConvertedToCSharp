/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="CapitalIndexedBondYieldConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CapitalIndexedBondYieldConventionTest
	public class CapitalIndexedBondYieldConventionTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {CapitalIndexedBondYieldConvention.US_IL_REAL, "US-I/L-Real"},
			new object[] {CapitalIndexedBondYieldConvention.GB_IL_FLOAT, "GB-I/L-Float"},
			new object[] {CapitalIndexedBondYieldConvention.GB_IL_BOND, "GB-I/L-Bond"},
			new object[] {CapitalIndexedBondYieldConvention.JP_IL_SIMPLE, "JP-I/L-Simple"},
			new object[] {CapitalIndexedBondYieldConvention.JP_IL_COMPOUND, "JP-I/L-Compound"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(CapitalIndexedBondYieldConvention convention, String name)
	  public virtual void test_toString(CapitalIndexedBondYieldConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(CapitalIndexedBondYieldConvention convention, String name)
	  public virtual void test_of_lookup(CapitalIndexedBondYieldConvention convention, string name)
	  {
		assertEquals(CapitalIndexedBondYieldConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(CapitalIndexedBondYieldConvention convention, String name)
	  public virtual void test_of_lookupUpperCase(CapitalIndexedBondYieldConvention convention, string name)
	  {
		assertEquals(CapitalIndexedBondYieldConvention.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(CapitalIndexedBondYieldConvention convention, String name)
	  public virtual void test_of_lookupLowerCase(CapitalIndexedBondYieldConvention convention, string name)
	  {
		assertEquals(CapitalIndexedBondYieldConvention.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupStandard(CapitalIndexedBondYieldConvention convention, String name)
	  public virtual void test_of_lookupStandard(CapitalIndexedBondYieldConvention convention, string name)
	  {
		assertEquals(CapitalIndexedBondYieldConvention.of(convention.name()), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => CapitalIndexedBondYieldConvention.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => CapitalIndexedBondYieldConvention.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(CapitalIndexedBondYieldConvention));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(CapitalIndexedBondYieldConvention.US_IL_REAL);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(CapitalIndexedBondYieldConvention), CapitalIndexedBondYieldConvention.GB_IL_BOND);
	  }

	}

}
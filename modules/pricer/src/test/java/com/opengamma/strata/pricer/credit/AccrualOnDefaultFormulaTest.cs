/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
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
	/// Test <seealso cref="AccrualOnDefaultFormula"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AccrualOnDefaultFormulaTest
	public class AccrualOnDefaultFormulaTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {AccrualOnDefaultFormula.ORIGINAL_ISDA, "OriginalISDA"},
			new object[] {AccrualOnDefaultFormula.MARKIT_FIX, "MarkitFix"},
			new object[] {AccrualOnDefaultFormula.CORRECT, "Correct"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(AccrualOnDefaultFormula convention, String name)
	  public virtual void test_toString(AccrualOnDefaultFormula convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(AccrualOnDefaultFormula convention, String name)
	  public virtual void test_of_lookup(AccrualOnDefaultFormula convention, string name)
	  {
		assertEquals(AccrualOnDefaultFormula.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(AccrualOnDefaultFormula convention, String name)
	  public virtual void test_of_lookupUpperCase(AccrualOnDefaultFormula convention, string name)
	  {
		assertEquals(AccrualOnDefaultFormula.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(AccrualOnDefaultFormula convention, String name)
	  public virtual void test_of_lookupLowerCase(AccrualOnDefaultFormula convention, string name)
	  {
		assertEquals(AccrualOnDefaultFormula.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => AccrualOnDefaultFormula.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => AccrualOnDefaultFormula.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(AccrualOnDefaultFormula));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(AccrualOnDefaultFormula.ORIGINAL_ISDA);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(AccrualOnDefaultFormula), AccrualOnDefaultFormula.MARKIT_FIX);
	  }

	}

}
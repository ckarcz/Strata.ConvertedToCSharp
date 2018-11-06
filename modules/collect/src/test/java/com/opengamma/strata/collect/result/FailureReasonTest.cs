/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
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
	/// Test <seealso cref="FailureReason"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FailureReasonTest
	public class FailureReasonTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {FailureReason.CALCULATION_FAILED, "CALCULATION_FAILED"},
			new object[] {FailureReason.CURRENCY_CONVERSION, "CURRENCY_CONVERSION"},
			new object[] {FailureReason.ERROR, "ERROR"},
			new object[] {FailureReason.INVALID, "INVALID"},
			new object[] {FailureReason.MISSING_DATA, "MISSING_DATA"},
			new object[] {FailureReason.MULTIPLE, "MULTIPLE"},
			new object[] {FailureReason.NOT_APPLICABLE, "NOT_APPLICABLE"},
			new object[] {FailureReason.OTHER, "OTHER"},
			new object[] {FailureReason.PARSING, "PARSING"},
			new object[] {FailureReason.UNSUPPORTED, "UNSUPPORTED"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FailureReason convention, String name)
	  public virtual void test_toString(FailureReason convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FailureReason convention, String name)
	  public virtual void test_of_lookup(FailureReason convention, string name)
	  {
		assertEquals(FailureReason.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(FailureReason convention, String name)
	  public virtual void test_of_lookupUpperCase(FailureReason convention, string name)
	  {
		assertEquals(FailureReason.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(FailureReason convention, String name)
	  public virtual void test_of_lookupLowerCase(FailureReason convention, string name)
	  {
		assertEquals(FailureReason.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FailureReason.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FailureReason.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(FailureReason));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FailureReason.CALCULATION_FAILED);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FailureReason), FailureReason.CURRENCY_CONVERSION);
	  }

	}

}
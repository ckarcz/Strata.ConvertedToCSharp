/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
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
	/// Test <seealso cref="ValueAdjustmentType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueAdjustmentTypeTest
	public class ValueAdjustmentTypeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_adjust()
	  {
		assertEquals(ValueAdjustmentType.DELTA_AMOUNT.adjust(2d, 3d), 5d, 1e-12);
		assertEquals(ValueAdjustmentType.DELTA_MULTIPLIER.adjust(2d, 1.5d), 5d, 1e-12);
		assertEquals(ValueAdjustmentType.MULTIPLIER.adjust(2d, 1.5d), 3d, 1e-12);
		assertEquals(ValueAdjustmentType.REPLACE.adjust(2d, 1.5d), 1.5d, 1e-12);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {ValueAdjustmentType.DELTA_AMOUNT, "DeltaAmount"},
			new object[] {ValueAdjustmentType.DELTA_MULTIPLIER, "DeltaMultiplier"},
			new object[] {ValueAdjustmentType.MULTIPLIER, "Multiplier"},
			new object[] {ValueAdjustmentType.REPLACE, "Replace"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(ValueAdjustmentType convention, String name)
	  public virtual void test_toString(ValueAdjustmentType convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(ValueAdjustmentType convention, String name)
	  public virtual void test_of_lookup(ValueAdjustmentType convention, string name)
	  {
		assertEquals(ValueAdjustmentType.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupUpperCase(ValueAdjustmentType convention, String name)
	  public virtual void test_of_lookupUpperCase(ValueAdjustmentType convention, string name)
	  {
		assertEquals(ValueAdjustmentType.of(name.ToUpper(Locale.ENGLISH)), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookupLowerCase(ValueAdjustmentType convention, String name)
	  public virtual void test_of_lookupLowerCase(ValueAdjustmentType convention, string name)
	  {
		assertEquals(ValueAdjustmentType.of(name.ToLower(Locale.ENGLISH)), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => ValueAdjustmentType.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => ValueAdjustmentType.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(ValueAdjustmentType));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(ValueAdjustmentType.DELTA_AMOUNT);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(ValueAdjustmentType), ValueAdjustmentType.DELTA_AMOUNT);
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.LOG_LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="CurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveExtrapolatorTest
	public class CurveExtrapolatorTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {CurveExtrapolators.EXCEPTION, "Exception"},
			new object[] {CurveExtrapolators.EXPONENTIAL, "Exponential"},
			new object[] {CurveExtrapolators.FLAT, "Flat"},
			new object[] {CurveExtrapolators.INTERPOLATOR, "Interpolator"},
			new object[] {CurveExtrapolators.LINEAR, "Linear"},
			new object[] {CurveExtrapolators.LOG_LINEAR, "LogLinear"},
			new object[] {CurveExtrapolators.PRODUCT_LINEAR, "ProductLinear"},
			new object[] {CurveExtrapolators.QUADRATIC_LEFT, "QuadraticLeft"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(CurveExtrapolator convention, String name)
	  public virtual void test_name(CurveExtrapolator convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(CurveExtrapolator convention, String name)
	  public virtual void test_toString(CurveExtrapolator convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(CurveExtrapolator convention, String name)
	  public virtual void test_of_lookup(CurveExtrapolator convention, string name)
	  {
		assertEquals(CurveExtrapolator.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(CurveExtrapolator convention, String name)
	  public virtual void test_extendedEnum(CurveExtrapolator convention, string name)
	  {
		ImmutableMap<string, CurveExtrapolator> map = CurveExtrapolator.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => CurveExtrapolator.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => CurveExtrapolator.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(CurveExtrapolators));
		coverPrivateConstructor(typeof(StandardCurveExtrapolators));
		assertFalse(FLAT.Equals(null));
		assertFalse(FLAT.Equals(ANOTHER_TYPE));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FLAT);
		assertSerialization(LINEAR);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(CurveExtrapolator), FLAT);
		assertJodaConvert(typeof(CurveExtrapolator), LOG_LINEAR);
	  }

	}

}
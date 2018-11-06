/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
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
//	import static com.opengamma.strata.measure.fxopt.FxVanillaOptionMethod.BLACK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using FxVanillaOptionTrade = com.opengamma.strata.product.fxopt.FxVanillaOptionTrade;

	/// <summary>
	/// Test <seealso cref="FxVanillaOptionMethod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxVanillaOptionMethodTest
	public class FxVanillaOptionMethodTest
	{

	  private static readonly FxVanillaOptionTrade TRADE = FxVanillaOptionTradeCalculationFunctionTest.TRADE;
	  private static readonly CalculationTarget TARGET = new CalculationTargetAnonymousInnerClass();

	  private class CalculationTargetAnonymousInnerClass : CalculationTarget
	  {
		  public CalculationTargetAnonymousInnerClass()
		  {
		  }

	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {FxVanillaOptionMethod.BLACK, "Black"},
			new object[] {FxVanillaOptionMethod.VANNA_VOLGA, "VannaVolga"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(FxVanillaOptionMethod convention, String name)
	  public virtual void test_toString(FxVanillaOptionMethod convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(FxVanillaOptionMethod convention, String name)
	  public virtual void test_of_lookup(FxVanillaOptionMethod convention, string name)
	  {
		assertEquals(FxVanillaOptionMethod.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrows(() => FxVanillaOptionMethod.of("Rubbish"), typeof(System.ArgumentException));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrows(() => FxVanillaOptionMethod.of(null), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_filter()
	  {
		assertEquals(BLACK.filter(TRADE, Measures.PRESENT_VALUE), BLACK);
		assertEquals(BLACK.filter(TARGET, Measures.PRESENT_VALUE), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverEnum(typeof(FxVanillaOptionMethod));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FxVanillaOptionMethod.BLACK);
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FxVanillaOptionMethod), FxVanillaOptionMethod.BLACK);
	  }

	}

}
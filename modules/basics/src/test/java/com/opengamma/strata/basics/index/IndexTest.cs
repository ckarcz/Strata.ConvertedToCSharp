/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Index"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IndexTest
	public class IndexTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
		public static object[][] data_name()
		{
		return new object[][]
		{
			new object[] {IborIndices.GBP_LIBOR_6M, "GBP-LIBOR-6M"},
			new object[] {IborIndices.CHF_LIBOR_6M, "CHF-LIBOR-6M"},
			new object[] {IborIndices.EUR_LIBOR_6M, "EUR-LIBOR-6M"},
			new object[] {IborIndices.JPY_LIBOR_6M, "JPY-LIBOR-6M"},
			new object[] {IborIndices.USD_LIBOR_6M, "USD-LIBOR-6M"},
			new object[] {OvernightIndices.GBP_SONIA, "GBP-SONIA"},
			new object[] {OvernightIndices.CHF_TOIS, "CHF-TOIS"},
			new object[] {OvernightIndices.EUR_EONIA, "EUR-EONIA"},
			new object[] {OvernightIndices.JPY_TONAR, "JPY-TONAR"},
			new object[] {OvernightIndices.USD_FED_FUND, "USD-FED-FUND"},
			new object[] {PriceIndices.GB_HICP, "GB-HICP"},
			new object[] {PriceIndices.CH_CPI, "CH-CPI"},
			new object[] {PriceIndices.EU_AI_CPI, "EU-AI-CPI"},
			new object[] {FxIndices.EUR_CHF_ECB, "EUR/CHF-ECB"},
			new object[] {FxIndices.EUR_GBP_ECB, "EUR/GBP-ECB"},
			new object[] {FxIndices.GBP_USD_WM, "GBP/USD-WM"},
			new object[] {FxIndices.USD_JPY_WM, "USD/JPY-WM"}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(Index convention, String name)
	  public virtual void test_name(Index convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(Index convention, String name)
	  public virtual void test_toString(Index convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(Index convention, String name)
	  public virtual void test_of_lookup(Index convention, string name)
	  {
		assertEquals(Index.of(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => Index.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => Index.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(Indices));
	  }

	}

}
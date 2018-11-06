/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.DKCO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.MXMC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// Test <seealso cref="FloatingRateName"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FloatingRateNameTest
	public class FloatingRateNameTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "nameType") public static Object[][] data_name_type()
		public static object[][] data_name_type()
		{
		return new object[][]
		{
			new object[] {"GBP-LIBOR", "GBP-LIBOR-", FloatingRateType.IBOR},
			new object[] {"GBP-LIBOR-BBA", "GBP-LIBOR-", FloatingRateType.IBOR},
			new object[] {"CHF-LIBOR", "CHF-LIBOR-", FloatingRateType.IBOR},
			new object[] {"CHF-LIBOR-BBA", "CHF-LIBOR-", FloatingRateType.IBOR},
			new object[] {"EUR-LIBOR", "EUR-LIBOR-", FloatingRateType.IBOR},
			new object[] {"EUR-LIBOR-BBA", "EUR-LIBOR-", FloatingRateType.IBOR},
			new object[] {"JPY-LIBOR", "JPY-LIBOR-", FloatingRateType.IBOR},
			new object[] {"JPY-LIBOR-BBA", "JPY-LIBOR-", FloatingRateType.IBOR},
			new object[] {"USD-LIBOR", "USD-LIBOR-", FloatingRateType.IBOR},
			new object[] {"USD-LIBOR-BBA", "USD-LIBOR-", FloatingRateType.IBOR},
			new object[] {"EUR-EURIBOR", "EUR-EURIBOR-", FloatingRateType.IBOR},
			new object[] {"EUR-EURIBOR-Reuters", "EUR-EURIBOR-", FloatingRateType.IBOR},
			new object[] {"JPY-TIBOR-JAPAN", "JPY-TIBOR-JAPAN-", FloatingRateType.IBOR},
			new object[] {"JPY-TIBOR-TIBM", "JPY-TIBOR-JAPAN-", FloatingRateType.IBOR},
			new object[] {"GBP-SONIA", "GBP-SONIA", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"GBP-WMBA-SONIA-COMPOUND", "GBP-SONIA", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"GBP-SONIA-COMPOUND", "GBP-SONIA", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"CHF-SARON", "CHF-SARON", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"CHF-SARON-OIS-COMPOUND", "CHF-SARON", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"CHF-TOIS", "CHF-TOIS", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"CHF-TOIS-OIS-COMPOUND", "CHF-TOIS", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"EUR-EONIA", "EUR-EONIA", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"EUR-EONIA-OIS-COMPOUND", "EUR-EONIA", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"JPY-TONAR", "JPY-TONAR", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"JPY-TONA-OIS-COMPOUND", "JPY-TONAR", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"USD-FED-FUND", "USD-FED-FUND", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"USD-Federal Funds-H.15-OIS-COMPOUND", "USD-FED-FUND", FloatingRateType.OVERNIGHT_COMPOUNDED},
			new object[] {"USD-FED-FUND-AVG", "USD-FED-FUND-AVG", FloatingRateType.OVERNIGHT_AVERAGED},
			new object[] {"USD-Federal Funds-H.15", "USD-FED-FUND-AVG", FloatingRateType.OVERNIGHT_AVERAGED},
			new object[] {"GB-HICP", "GB-HICP", FloatingRateType.PRICE},
			new object[] {"UK-HICP", "GB-HICP", FloatingRateType.PRICE},
			new object[] {"GB-RPI", "GB-RPI", FloatingRateType.PRICE},
			new object[] {"UK-RPI", "GB-RPI", FloatingRateType.PRICE},
			new object[] {"GB-RPIX", "GB-RPIX", FloatingRateType.PRICE},
			new object[] {"UK-RPIX", "GB-RPIX", FloatingRateType.PRICE},
			new object[] {"CH-CPI", "CH-CPI", FloatingRateType.PRICE},
			new object[] {"SWF-CPI", "CH-CPI", FloatingRateType.PRICE},
			new object[] {"EU-AI-CPI", "EU-AI-CPI", FloatingRateType.PRICE},
			new object[] {"EUR-AI-CPI", "EU-AI-CPI", FloatingRateType.PRICE},
			new object[] {"EU-EXT-CPI", "EU-EXT-CPI", FloatingRateType.PRICE},
			new object[] {"EUR-EXT-CPI", "EU-EXT-CPI", FloatingRateType.PRICE},
			new object[] {"JP-CPI-EXF", "JP-CPI-EXF", FloatingRateType.PRICE},
			new object[] {"JPY-CPI-EXF", "JP-CPI-EXF", FloatingRateType.PRICE},
			new object[] {"US-CPI-U", "US-CPI-U", FloatingRateType.PRICE},
			new object[] {"USA-CPI-U", "US-CPI-U", FloatingRateType.PRICE},
			new object[] {"FR-EXT-CPI", "FR-EXT-CPI", FloatingRateType.PRICE},
			new object[] {"FRC-EXT-CPI", "FR-EXT-CPI", FloatingRateType.PRICE},
			new object[] {"AUD-BBR-BBSW", "AUD-BBSW", FloatingRateType.IBOR},
			new object[] {"CAD-BA-CDOR", "CAD-CDOR", FloatingRateType.IBOR},
			new object[] {"CNY-CNREPOFIX=CFXS-Reuters", "CNY-REPO", FloatingRateType.IBOR},
			new object[] {"CZK-PRIBOR-PRBO", "CZK-PRIBOR", FloatingRateType.IBOR},
			new object[] {"DKK-CIBOR-DKNA13", "DKK-CIBOR", FloatingRateType.IBOR},
			new object[] {"HKD-HIBOR-ISDC", "HKD-HIBOR", FloatingRateType.IBOR},
			new object[] {"HKD-HIBOR-HIBOR=", "HKD-HIBOR", FloatingRateType.IBOR},
			new object[] {"HUF-BUBOR-Reuters", "HUF-BUBOR", FloatingRateType.IBOR},
			new object[] {"KRW-CD-KSDA-Bloomberg", "KRW-CD", FloatingRateType.IBOR},
			new object[] {"MXN-TIIE-Banxico", "MZN-TIIE", FloatingRateType.IBOR},
			new object[] {"NOK-NIBOR-OIBOR", "NOK-NIBOR", FloatingRateType.IBOR},
			new object[] {"NZD-BBR-FRA", "NZD-BBR", FloatingRateType.IBOR},
			new object[] {"PLN-WIBOR-WIBO", "PLN-WIBOR", FloatingRateType.IBOR},
			new object[] {"SEK-STIBOR-Bloomberg", "SEK-STIBOR", FloatingRateType.IBOR},
			new object[] {"SGD-SOR-VWAP", "SGD-SOR", FloatingRateType.IBOR},
			new object[] {"ZAR-JIBAR-SAFEX", "ZAR-JIBAR", FloatingRateType.IBOR},
			new object[] {"INR-MIBOR-OIS-COMPOUND", "INR-OMIBOR", FloatingRateType.OVERNIGHT_COMPOUNDED}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nameType") public void test_name(String name, String indexName, FloatingRateType type)
	  public virtual void test_name(string name, string indexName, FloatingRateType type)
	  {
		FloatingRateName test = FloatingRateName.of(name);
		assertEquals(test.Name, name);
		assertEquals(test.Type, type);
		assertEquals(test.Currency, test.toFloatingRateIndex().Currency);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nameType") public void test_toString(String name, String indexName, FloatingRateType type)
	  public virtual void test_toString(string name, string indexName, FloatingRateType type)
	  {
		FloatingRateName test = FloatingRateName.of(name);
		assertEquals(test.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "nameType") public void test_of_lookup(String name, String indexName, FloatingRateType type)
	  public virtual void test_of_lookup(string name, string indexName, FloatingRateType type)
	  {
		FloatingRateName test = FloatingRateName.of(name);
		assertEquals(FloatingRateName.of(name), test);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => FloatingRateName.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => FloatingRateName.of(null));
	  }

	  public virtual void test_parse()
	  {
		assertEquals(FloatingRateName.parse("GBP-LIBOR"), FloatingRateNames.GBP_LIBOR);
		assertEquals(FloatingRateName.parse("GBP-LIBOR-3M"), FloatingRateNames.GBP_LIBOR);
		assertEquals(FloatingRateName.parse("GBP-SONIA"), FloatingRateNames.GBP_SONIA);
		assertEquals(FloatingRateName.parse("GB-RPI"), FloatingRateNames.GB_RPI);
		assertThrowsIllegalArg(() => FloatingRateName.parse(null));
		assertThrowsIllegalArg(() => FloatingRateName.parse("NotAnIndex"));
	  }

	  public virtual void test_tryParse()
	  {
		assertEquals(FloatingRateName.tryParse("GBP-LIBOR"), FloatingRateNames.GBP_LIBOR);
		assertEquals(FloatingRateName.tryParse("GBP-LIBOR-3M"), FloatingRateNames.GBP_LIBOR);
		assertEquals(FloatingRateName.tryParse("GBP-SONIA"), FloatingRateNames.GBP_SONIA);
		assertEquals(FloatingRateName.tryParse("GB-RPI"), FloatingRateNames.GB_RPI);
		assertEquals(FloatingRateName.tryParse(null), null);
		assertEquals(FloatingRateName.tryParse("NotAnIndex"), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultIborIndex()
	  {
		assertEquals(FloatingRateName.defaultIborIndex(Currency.GBP), FloatingRateNames.GBP_LIBOR);
		assertEquals(FloatingRateName.defaultIborIndex(Currency.EUR), FloatingRateNames.EUR_EURIBOR);
		assertEquals(FloatingRateName.defaultIborIndex(Currency.USD), FloatingRateNames.USD_LIBOR);
		assertEquals(FloatingRateName.defaultIborIndex(Currency.AUD), FloatingRateNames.AUD_BBSW);
		assertEquals(FloatingRateName.defaultIborIndex(Currency.CAD), FloatingRateNames.CAD_CDOR);
		assertEquals(FloatingRateName.defaultIborIndex(Currency.NZD), FloatingRateNames.NZD_BKBM);
	  }

	  public virtual void test_defaultOvernightIndex()
	  {
		assertEquals(FloatingRateName.defaultOvernightIndex(Currency.GBP), FloatingRateName.of("GBP-SONIA"));
		assertEquals(FloatingRateName.defaultOvernightIndex(Currency.EUR), FloatingRateName.of("EUR-EONIA"));
		assertEquals(FloatingRateName.defaultOvernightIndex(Currency.USD), FloatingRateName.of("USD-FED-FUND"));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalized()
	  {
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").normalized(), FloatingRateName.of("GBP-LIBOR"));
		assertEquals(FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").normalized(), FloatingRateName.of("GBP-SONIA"));
		foreach (FloatingRateName name in FloatingRateName.extendedEnum().lookupAll().values())
		{
		  assertNotNull(name.normalized());
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_iborIndex_tenor()
	  {
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").DefaultTenor, Tenor.TENOR_3M);
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").toFloatingRateIndex(), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").toFloatingRateIndex(Tenor.TENOR_1M), IborIndices.GBP_LIBOR_1M);
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").toIborIndex(Tenor.TENOR_6M), IborIndices.GBP_LIBOR_6M);
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").toIborIndex(Tenor.TENOR_12M), IborIndices.GBP_LIBOR_12M);
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").toIborIndex(Tenor.TENOR_1Y), IborIndices.GBP_LIBOR_12M);
		assertThrows(() => FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").toIborIndex(Tenor.TENOR_6M), typeof(System.InvalidOperationException));
		assertEquals(ImmutableList.copyOf(FloatingRateName.of("GBP-LIBOR-BBA").Tenors), ImmutableList.of(Tenor.TENOR_1W, Tenor.TENOR_1M, Tenor.TENOR_2M, Tenor.TENOR_3M, Tenor.TENOR_6M, Tenor.TENOR_12M));
		assertEquals(FloatingRateName.of("GBP-LIBOR-BBA").toIborIndexFixingOffset(), DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(PRECEDING, GBLO)));
	  }

	  public virtual void test_overnightIndex()
	  {
		assertEquals(FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").DefaultTenor, Tenor.TENOR_1D);
		assertEquals(FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").toFloatingRateIndex(), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").toFloatingRateIndex(Tenor.TENOR_1M), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").toOvernightIndex(), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateNames.USD_FED_FUND.toOvernightIndex(), OvernightIndices.USD_FED_FUND);
		assertEquals(FloatingRateNames.USD_FED_FUND_AVG.toOvernightIndex(), OvernightIndices.USD_FED_FUND);
		assertThrows(() => FloatingRateName.of("GBP-LIBOR-BBA").toOvernightIndex(), typeof(System.InvalidOperationException));
		assertEquals(FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").Tenors, ImmutableSet.of());
		assertThrows(() => FloatingRateName.of("GBP-WMBA-SONIA-COMPOUND").toIborIndexFixingOffset(), typeof(System.InvalidOperationException));
	  }

	  public virtual void test_priceIndex()
	  {
		assertEquals(FloatingRateName.of("UK-HICP").DefaultTenor, Tenor.TENOR_1Y);
		assertEquals(FloatingRateName.of("UK-HICP").toFloatingRateIndex(), PriceIndices.GB_HICP);
		assertEquals(FloatingRateName.of("UK-HICP").toFloatingRateIndex(Tenor.TENOR_1M), PriceIndices.GB_HICP);
		assertEquals(FloatingRateName.of("UK-HICP").toPriceIndex(), PriceIndices.GB_HICP);
		assertThrows(() => FloatingRateName.of("GBP-LIBOR-BBA").toPriceIndex(), typeof(System.InvalidOperationException));
		assertEquals(FloatingRateName.of("UK-HICP").Tenors, ImmutableSet.of());
		assertThrows(() => FloatingRateName.of("UK-HICP").toIborIndexFixingOffset(), typeof(System.InvalidOperationException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cibor()
	  {
		assertEquals(FloatingRateName.of("DKK-CIBOR-DKNA13").DefaultTenor, Tenor.TENOR_3M);
		assertEquals(FloatingRateName.of("DKK-CIBOR-DKNA13").toFloatingRateIndex(), IborIndices.DKK_CIBOR_3M);
		assertEquals(FloatingRateName.of("DKK-CIBOR-DKNA13").toFloatingRateIndex(Tenor.TENOR_1M), IborIndices.DKK_CIBOR_1M);
		assertEquals(FloatingRateName.of("DKK-CIBOR-DKNA13").toIborIndex(Tenor.TENOR_6M), IborIndices.DKK_CIBOR_6M);
		assertEquals(FloatingRateName.of("DKK-CIBOR2-DKNA13").toIborIndex(Tenor.TENOR_6M), IborIndices.DKK_CIBOR_6M);
		assertEquals(FloatingRateName.of("DKK-CIBOR-DKNA13").toIborIndexFixingOffset(), DaysAdjustment.ofCalendarDays(0, BusinessDayAdjustment.of(PRECEDING, DKCO)));
		assertEquals(FloatingRateName.of("DKK-CIBOR2-DKNA13").toIborIndexFixingOffset(), DaysAdjustment.ofBusinessDays(-2, DKCO));
	  }

	  public virtual void test_tiee()
	  {
		assertEquals(FloatingRateName.of("MXN-TIIE").DefaultTenor, Tenor.TENOR_13W);
		assertEquals(FloatingRateName.of("MXN-TIIE").toFloatingRateIndex(), IborIndices.MXN_TIIE_13W);
		assertEquals(FloatingRateName.of("MXN-TIIE").toFloatingRateIndex(Tenor.TENOR_4W), IborIndices.MXN_TIIE_4W);
		assertEquals(FloatingRateName.of("MXN-TIIE").toIborIndex(Tenor.TENOR_4W), IborIndices.MXN_TIIE_4W);
		assertEquals(FloatingRateName.of("MXN-TIIE").toIborIndexFixingOffset(), DaysAdjustment.ofBusinessDays(-1, MXMC));
	  }

	  public virtual void test_nzd()
	  {
		assertEquals(FloatingRateName.of("NZD-BKBM").Currency, Currency.NZD);
		assertEquals(FloatingRateName.of("NZD-NZIONA").Currency, Currency.NZD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_types()
	  {
		// ensure no stupid copy and paste errors
		System.Reflection.FieldInfo[] fields = typeof(FloatingRateNames).GetFields();
		foreach (System.Reflection.FieldInfo field in fields)
		{
		  if (Modifier.isPublic(field.Modifiers) && Modifier.isStatic(field.Modifiers))
		  {
			assertEquals(field.Type, typeof(FloatingRateName));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FloatingRateNames));
		ImmutableBean test = (ImmutableBean) FloatingRateName.of("GBP-LIBOR-BBA");
		coverImmutableBean(test);
		coverBeanEquals(test, (ImmutableBean) FloatingRateName.of("USD-Federal Funds-H.15"));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(FloatingRateName), FloatingRateName.of("GBP-LIBOR-BBA"));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FloatingRateName.of("GBP-LIBOR-BBA"));
	  }

	}

}
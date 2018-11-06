/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.BRL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.DKK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.INR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.NZD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.PLN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.SEK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.SGD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.ZAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.AUSY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.BRBD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.CHZU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.DKCO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.PLWA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SEST;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USGS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.ZAJO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;

	/// <summary>
	/// Test Overnight Index.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightIndexTest
	public class OvernightIndexTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test_gbpSonia()
	  {
		OvernightIndex test = OvernightIndex.of("GBP-SONIA");
		assertEquals(test.Name, "GBP-SONIA");
		assertEquals(test.Currency, GBP);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, GBLO);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.FloatingRateName, FloatingRateName.of("GBP-SONIA"));
		assertEquals(test.ToString(), "GBP-SONIA");
	  }

	  public virtual void test_gbpSonia_dates()
	  {
		OvernightIndex test = OvernightIndex.of("GBP-SONIA");
		assertEquals(test.calculatePublicationFromFixing(date(2014, 10, 13), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 13), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 13), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 13), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 13), REF_DATA), date(2014, 10, 14));
		// weekend
		assertEquals(test.calculatePublicationFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 10), REF_DATA), date(2014, 10, 13));
		// input date is Sunday
		assertEquals(test.calculatePublicationFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 13));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 14));
	  }

	  public virtual void test_chfSaron()
	  {
		OvernightIndex test = OvernightIndex.of("CHF-SARON");
		assertEquals(test.Name, "CHF-SARON");
		assertEquals(test.Currency, CHF);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, CHZU);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.FloatingRateName, FloatingRateName.of("CHF-SARON"));
		assertEquals(test.ToString(), "CHF-SARON");
	  }

	  public virtual void test_getFloatingRateName()
	  {
		foreach (OvernightIndex index in OvernightIndex.extendedEnum().lookupAll().values())
		{
		  assertEquals(index.FloatingRateName, FloatingRateName.of(index.Name));
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_usdFedFund3m()
	  {
		OvernightIndex test = OvernightIndex.of("USD-FED-FUND");
		assertEquals(test.Currency, USD);
		assertEquals(test.Name, "USD-FED-FUND");
		assertEquals(test.FixingCalendar, USNY);
		assertEquals(test.PublicationDateOffset, 1);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.ToString(), "USD-FED-FUND");
	  }

	  public virtual void test_usdFedFund_dates()
	  {
		OvernightIndex test = OvernightIndex.of("USD-FED-FUND");
		assertEquals(test.calculatePublicationFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 28));
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 27));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 27), REF_DATA), date(2014, 10, 28));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 27), REF_DATA), date(2014, 10, 27));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 27), REF_DATA), date(2014, 10, 28));
		// weekend and US holiday
		assertEquals(test.calculatePublicationFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 10), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 10), REF_DATA), date(2014, 10, 10));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 10), REF_DATA), date(2014, 10, 14));
		// input date is Sunday, 13th is US holiday
		assertEquals(test.calculatePublicationFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateEffectiveFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateMaturityFromFixing(date(2014, 10, 12), REF_DATA), date(2014, 10, 15));
		assertEquals(test.calculateFixingFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 14));
		assertEquals(test.calculateMaturityFromEffective(date(2014, 10, 12), REF_DATA), date(2014, 10, 15));
	  }

	  public virtual void test_usdSofr()
	  {
		OvernightIndex test = OvernightIndex.of("USD-SOFR");
		assertEquals(test.Name, "USD-SOFR");
		assertEquals(test.Currency, USD);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, USGS);
		assertEquals(test.PublicationDateOffset, 1);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.ToString(), "USD-SOFR");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_audAonia()
	  {
		OvernightIndex test = OvernightIndex.of("AUD-AONIA");
		assertEquals(test.Name, "AUD-AONIA");
		assertEquals(test.Currency, AUD);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, AUSY);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "AUD-AONIA");
	  }

	  public virtual void test_brlCdi()
	  {
		OvernightIndex test = OvernightIndex.of("BRL-CDI");
		assertEquals(test.Name, "BRL-CDI");
		assertEquals(test.Currency, BRL);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, BRBD);
		assertEquals(test.PublicationDateOffset, 1);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, DayCount.ofBus252(BRBD));
		assertEquals(test.ToString(), "BRL-CDI");
	  }

	  public virtual void test_dkkOis()
	  {
		OvernightIndex test = OvernightIndex.of("DKK-TNR");
		assertEquals(test.Name, "DKK-TNR");
		assertEquals(test.Currency, DKK);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, DKCO);
		assertEquals(test.PublicationDateOffset, 1);
		assertEquals(test.EffectiveDateOffset, 1);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.ToString(), "DKK-TNR");
	  }

	  public virtual void test_inrOis()
	  {
		OvernightIndex test = OvernightIndex.of("INR-OMIBOR");
		assertEquals(test.Name, "INR-OMIBOR");
		assertEquals(test.Currency, INR);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, HolidayCalendarId.of("INMU"));
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "INR-OMIBOR");
	  }

	  public virtual void test_nzdOis()
	  {
		OvernightIndex test = OvernightIndex.of("NZD-NZIONA");
		assertEquals(test.Name, "NZD-NZIONA");
		assertEquals(test.Currency, NZD);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, HolidayCalendarId.of("NZBD"));
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "NZD-NZIONA");
	  }

	  public virtual void test_plnOis()
	  {
		OvernightIndex test = OvernightIndex.of("PLN-POLONIA");
		assertEquals(test.Name, "PLN-POLONIA");
		assertEquals(test.Currency, PLN);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, PLWA);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "PLN-POLONIA");
	  }

	  public virtual void test_sekOis()
	  {
		OvernightIndex test = OvernightIndex.of("SEK-SIOR");
		assertEquals(test.Name, "SEK-SIOR");
		assertEquals(test.Currency, SEK);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, SEST);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 1);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.DefaultFixedLegDayCount, ACT_360);
		assertEquals(test.ToString(), "SEK-SIOR");
	  }

	  public virtual void test_sgdSonar()
	  {
		HolidayCalendarId SGSI = HolidayCalendarId.of("SGSI");
		OvernightIndex test = OvernightIndex.of("SGD-SONAR");
		assertEquals(test.Name, "SGD-SONAR");
		assertEquals(test.Currency, SGD);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, SGSI);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "SGD-SONAR");
	  }

	  public virtual void test_zarSabor()
	  {
		OvernightIndex test = OvernightIndex.of("ZAR-SABOR");
		assertEquals(test.Name, "ZAR-SABOR");
		assertEquals(test.Currency, ZAR);
		assertEquals(test.Active, true);
		assertEquals(test.FixingCalendar, ZAJO);
		assertEquals(test.PublicationDateOffset, 0);
		assertEquals(test.EffectiveDateOffset, 0);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.DefaultFixedLegDayCount, ACT_365F);
		assertEquals(test.ToString(), "ZAR-SABOR");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_alternateNames()
	  {
		assertEquals(OvernightIndex.of("JPY-TONA"), OvernightIndices.JPY_TONAR);
		assertEquals(OvernightIndex.of("USD-FED-FUNDS"), OvernightIndices.USD_FED_FUND);
		assertEquals(OvernightIndex.of("USD-FEDFUNDS"), OvernightIndices.USD_FED_FUND);
		assertEquals(OvernightIndex.of("USD-FEDFUND"), OvernightIndices.USD_FED_FUND);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {OvernightIndices.GBP_SONIA, "GBP-SONIA"},
			new object[] {OvernightIndices.CHF_TOIS, "CHF-TOIS"},
			new object[] {OvernightIndices.EUR_EONIA, "EUR-EONIA"},
			new object[] {OvernightIndices.JPY_TONAR, "JPY-TONAR"},
			new object[] {OvernightIndices.USD_FED_FUND, "USD-FED-FUND"},
			new object[] {OvernightIndices.AUD_AONIA, "AUD-AONIA"},
			new object[] {OvernightIndices.BRL_CDI, "BRL-CDI"},
			new object[] {OvernightIndices.DKK_TNR, "DKK-TNR"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(OvernightIndex convention, String name)
	  public virtual void test_name(OvernightIndex convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(OvernightIndex convention, String name)
	  public virtual void test_toString(OvernightIndex convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(OvernightIndex convention, String name)
	  public virtual void test_of_lookup(OvernightIndex convention, string name)
	  {
		assertEquals(OvernightIndex.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(OvernightIndex convention, String name)
	  public virtual void test_extendedEnum(OvernightIndex convention, string name)
	  {
		ImmutableMap<string, OvernightIndex> map = OvernightIndex.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => OvernightIndex.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => OvernightIndex.of(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equals()
	  {
		ImmutableOvernightIndex a = ImmutableOvernightIndex.builder().name("Test").currency(Currency.GBP).fixingCalendar(GBLO).publicationDateOffset(0).effectiveDateOffset(0).dayCount(ACT_360).build();
		OvernightIndex b = a.toBuilder().name("Rubbish").build();
		assertEquals(a.Equals(b), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableOvernightIndex index = ImmutableOvernightIndex.builder().name("Test").currency(Currency.GBP).fixingCalendar(GBLO).publicationDateOffset(0).effectiveDateOffset(0).dayCount(ACT_360).build();
		coverImmutableBean(index);
		coverPrivateConstructor(typeof(OvernightIndices));
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(OvernightIndex), OvernightIndices.GBP_SONIA);
	  }

	  public virtual void test_serialization()
	  {
		OvernightIndex index = ImmutableOvernightIndex.builder().name("Test").currency(Currency.GBP).fixingCalendar(GBLO).publicationDateOffset(0).effectiveDateOffset(0).dayCount(ACT_360).build();
		assertSerialization(index);
	  }

	}

}
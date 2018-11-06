/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.location.Country.GB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Country = com.opengamma.strata.basics.location.Country;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Test <seealso cref="PriceIndex"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PriceIndexTest
	public class PriceIndexTest
	{

	  public virtual void test_gbpHicp()
	  {
		PriceIndex test = PriceIndex.of("GB-HICP");
		assertEquals(test.Name, "GB-HICP");
		assertEquals(test.Currency, GBP);
		assertEquals(test.Region, GB);
		assertEquals(test.Active, true);
		assertEquals(test.PublicationFrequency, Frequency.P1M);
		assertEquals(test.FloatingRateName, FloatingRateName.of("GB-HICP"));
		assertEquals(test.ToString(), "GB-HICP");
	  }

	  public virtual void test_getFloatingRateName()
	  {
		foreach (PriceIndex index in PriceIndex.extendedEnum().lookupAll().values())
		{
		  assertEquals(index.FloatingRateName, FloatingRateName.of(index.Name));
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {PriceIndices.GB_HICP, "GB-HICP"},
			new object[] {PriceIndices.GB_RPI, "GB-RPI"},
			new object[] {PriceIndices.GB_RPIX, "GB-RPIX"},
			new object[] {PriceIndices.CH_CPI, "CH-CPI"},
			new object[] {PriceIndices.EU_AI_CPI, "EU-AI-CPI"},
			new object[] {PriceIndices.EU_EXT_CPI, "EU-EXT-CPI"},
			new object[] {PriceIndices.JP_CPI_EXF, "JP-CPI-EXF"},
			new object[] {PriceIndices.US_CPI_U, "US-CPI-U"},
			new object[] {PriceIndices.FR_EXT_CPI, "FR-EXT-CPI"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(PriceIndex convention, String name)
	  public virtual void test_name(PriceIndex convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(PriceIndex convention, String name)
	  public virtual void test_toString(PriceIndex convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(PriceIndex convention, String name)
	  public virtual void test_of_lookup(PriceIndex convention, string name)
	  {
		assertEquals(PriceIndex.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(PriceIndex convention, String name)
	  public virtual void test_extendedEnum(PriceIndex convention, string name)
	  {
		ImmutableMap<string, PriceIndex> map = PriceIndex.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => PriceIndex.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => PriceIndex.of(null));
	  }

	  public virtual void test_gb_rpi()
	  {
		assertEquals(PriceIndices.GB_RPI.Currency, GBP);
		assertEquals(PriceIndices.GB_RPI.DayCount, DayCounts.ONE_ONE);
		assertEquals(PriceIndices.GB_RPI.DefaultFixedLegDayCount, DayCounts.ONE_ONE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(PriceIndices));
		coverImmutableBean((ImmutableBean) PriceIndices.US_CPI_U);
		coverBeanEquals((ImmutableBean) PriceIndices.US_CPI_U, ImmutablePriceIndex.builder().name("Test").region(Country.AR).currency(Currency.ARS).publicationFrequency(Frequency.P6M).build());
	  }

	  public virtual void test_jodaConvert()
	  {
		assertJodaConvert(typeof(PriceIndex), PriceIndices.US_CPI_U);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(PriceIndices.US_CPI_U);
	  }

	}

}
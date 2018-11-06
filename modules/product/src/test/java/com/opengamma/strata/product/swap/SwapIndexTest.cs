using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FxIndices = com.opengamma.strata.basics.index.FxIndices;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndices = com.opengamma.strata.basics.index.OvernightIndices;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// Test <seealso cref="SwapIndex"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapIndexTest
	public class SwapIndexTest
	{

	  private static ZoneId LONDON = ZoneId.of("Europe/London");
	  private static ZoneId NEY_YORK = ZoneId.of("America/New_York");
	  private static ZoneId FRANKFURT = ZoneId.of("Europe/Berlin"); // Frankfurt not defined in TZDB

	  public virtual void test_notFound()
	  {
		assertThrowsIllegalArg(() => SwapIndex.of("foo"));
	  }

	  public virtual void test_swapIndicies()
	  {
		ImmutableMap<string, SwapIndex> mapAll = SwapIndices.ENUM_LOOKUP.lookupAll();
		ImmutableList<SwapIndex> indexAll = mapAll.values().asList();
		ImmutableList<string> nameAll = mapAll.Keys.asList();
		int size = indexAll.size();
		for (int i = 0; i < size; ++i)
		{
		  // check no duplication
		  for (int j = i + 1; j < size; ++j)
		  {
			assertFalse(nameAll.get(i).Equals(nameAll.get(j)));
			assertFalse(indexAll.get(i).Equals(indexAll.get(j)));
		  }
		}
		foreach (string name in nameAll)
		{
		  SwapIndex index = mapAll.get(name);
		  assertEquals(SwapIndex.of(name), index);
		  assertEquals(index.Active, true);
		  FixedIborSwapTemplate temp = index.Template;
		  FixedIborSwapConvention conv = temp.Convention;
		  Tenor tenor = temp.Tenor;
		  LocalTime time = index.FixingTime;
		  ZoneId zone = index.FixingZone;
		  // test consistency between name and template
		  assertTrue(name.Contains(tenor.ToString()));
		  if (name.StartsWith("USD", StringComparison.Ordinal))
		  {
			assertTrue(name.Contains("1100") || name.Contains("1500"));
			assertTrue(conv.Equals(FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M));
			assertTrue(zone.Equals(NEY_YORK));
		  }
		  if (name.StartsWith("GBP", StringComparison.Ordinal))
		  {
			assertTrue(name.Contains("1100"));
			if (tenor.Equals(Tenor.TENOR_1Y))
			{
			  assertTrue(conv.Equals(FixedIborSwapConventions.GBP_FIXED_1Y_LIBOR_3M));
			}
			else
			{
			  assertTrue(conv.Equals(FixedIborSwapConventions.GBP_FIXED_6M_LIBOR_6M));
			}
			assertTrue(zone.Equals(LONDON));
		  }
		  if (name.StartsWith("EUR", StringComparison.Ordinal))
		  {
			assertTrue(name.Contains("1100") || name.Contains("1200"));
			if (tenor.Equals(Tenor.TENOR_1Y))
			{
			  assertTrue(conv.Equals(FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M));
			}
			else
			{
			  assertTrue(conv.Equals(FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M));
			}
			assertTrue(zone.Equals(FRANKFURT));
		  }
		  if (name.Contains("1100"))
		  {
			assertTrue(time.Equals(LocalTime.of(11, 0)));
		  }
		  if (name.Contains("1200"))
		  {
			assertTrue(time.Equals(LocalTime.of(12, 0)));
		  }
		  if (name.Contains("1500"))
		  {
			assertTrue(time.Equals(LocalTime.of(15, 0)));
		  }
		  assertEquals(index.calculateFixingDateTime(date(2015, 6, 30)), date(2015, 6, 30).atTime(time).atZone(zone));
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "indexName") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {IborIndices.GBP_LIBOR_6M, "GBP-LIBOR-6M"},
			new object[] {OvernightIndices.GBP_SONIA, "GBP-SONIA"},
			new object[] {PriceIndices.GB_HICP, "GB-HICP"},
			new object[] {FxIndices.EUR_CHF_ECB, "EUR/CHF-ECB"},
			new object[] {SwapIndices.EUR_EURIBOR_1100_12Y, "EUR-EURIBOR-1100-12Y"},
			new object[] {SwapIndices.GBP_LIBOR_1100_2Y, "GBP-LIBOR-1100-2Y"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "indexName") public void test_name(com.opengamma.strata.basics.index.Index convention, String name)
	  public virtual void test_name(Index convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "indexName") public void test_toString(com.opengamma.strata.basics.index.Index convention, String name)
	  public virtual void test_toString(Index convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "indexName") public void test_of_lookup(com.opengamma.strata.basics.index.Index convention, String name)
	  public virtual void test_of_lookup(Index convention, string name)
	  {
		assertEquals(Index.of(name), convention);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableSwapIndex index = ImmutableSwapIndex.builder().name("FooIndex").fixingTime(LocalTime.of(12, 30)).fixingZone(ZoneId.of("Africa/Abidjan")).template(FixedIborSwapTemplate.of(Tenor.TENOR_9M, FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M)).build();
		coverImmutableBean(index);
		coverPrivateConstructor(typeof(SwapIndices));
	  }

	  public virtual void test_serialization()
	  {
		SwapIndex index = ImmutableSwapIndex.builder().name("FooIndex").fixingTime(LocalTime.of(12, 30)).fixingZone(ZoneId.of("Africa/Abidjan")).template(FixedIborSwapTemplate.of(Tenor.TENOR_9M, FixedIborSwapConventions.CHF_FIXED_1Y_LIBOR_3M)).build();
		assertSerialization(index);
	  }

	}

}
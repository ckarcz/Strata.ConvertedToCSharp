/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Test <seealso cref="IborIborSwapConventions"/>.
	/// <para>
	/// These tests  match the table 18.1 in the following guide:
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborIborSwapConventionsTest
	public class IborIborSwapConventionsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotLag") public static Object[][] data_spot_lag()
	  public static object[][] data_spot_lag()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, 2},
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, 2},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, 2},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, 2},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, 2},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, 2},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, 2},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, 2},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, 2},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, 2}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotLag") public void test_spot_lag(ImmutableIborIborSwapConvention convention, int lag)
	  public virtual void test_spot_lag(ImmutableIborIborSwapConvention convention, int lag)
	  {
		assertEquals(convention.SpotDateOffset.Days, lag);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "period") public static Object[][] data_period()
	  public static object[][] data_period()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, Frequency.P6M},
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, Frequency.P3M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, Frequency.P1M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, Frequency.P3M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, Frequency.P6M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, Frequency.P6M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, Frequency.P1M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, Frequency.P3M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, Frequency.P1M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, Frequency.P3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "period") public void test_period(IborIborSwapConvention convention, com.opengamma.strata.basics.schedule.Frequency frequency)
	  public virtual void test_period(IborIborSwapConvention convention, Frequency frequency)
	  {
		assertEquals(convention.SpreadLeg.PaymentFrequency, frequency);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayCount") public static Object[][] data_day_count()
	  public static object[][] data_day_count()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, CompoundingMethod.FLAT},
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, CompoundingMethod.FLAT},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, CompoundingMethod.NONE},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, CompoundingMethod.NONE}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayCount") public void test_composition(IborIborSwapConvention convention, com.opengamma.strata.product.swap.CompoundingMethod comp)
	  public virtual void test_composition(IborIborSwapConvention convention, CompoundingMethod comp)
	  {
		assertEquals(convention.SpreadLeg.CompoundingMethod, comp);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spreadLeg") public static Object[][] data_spread_leg()
	  public static object[][] data_spread_leg()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, IborIndices.USD_LIBOR_3M},
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, IborIndices.USD_LIBOR_1M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, IborIndices.JPY_LIBOR_1M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, IborIndices.JPY_LIBOR_3M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, IborIndices.JPY_LIBOR_6M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, IborIndices.JPY_LIBOR_6M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, IborIndices.JPY_TIBOR_EUROYEN_1M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, IborIndices.JPY_TIBOR_EUROYEN_3M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, IborIndices.JPY_TIBOR_JAPAN_1M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, IborIndices.JPY_TIBOR_JAPAN_3M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spreadLeg") public void test_float_leg(IborIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex index)
	  public virtual void test_float_leg(IborIborSwapConvention convention, IborIndex index)
	  {
		assertEquals(convention.SpreadLeg.Index, index);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "flatLeg") public static Object[][] data_flat_leg()
	  public static object[][] data_flat_leg()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, IborIndices.USD_LIBOR_6M},
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, IborIndices.USD_LIBOR_3M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, IborIndices.JPY_LIBOR_6M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, IborIndices.JPY_LIBOR_6M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, IborIndices.JPY_TIBOR_EUROYEN_6M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, IborIndices.JPY_TIBOR_JAPAN_6M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, IborIndices.JPY_TIBOR_EUROYEN_6M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, IborIndices.JPY_TIBOR_EUROYEN_6M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, IborIndices.JPY_TIBOR_JAPAN_6M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, IborIndices.JPY_TIBOR_JAPAN_6M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "flatLeg") public void test_flat_leg(IborIborSwapConvention convention, com.opengamma.strata.basics.index.IborIndex index)
	  public virtual void test_flat_leg(IborIborSwapConvention convention, IborIndex index)
	  {
		assertEquals(convention.FlatLeg.Index, index);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "dayConvention") public static Object[][] data_day_convention()
	  public static object[][] data_day_convention()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, BusinessDayConventions.MODIFIED_FOLLOWING}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "dayConvention") public void test_day_convention(IborIborSwapConvention convention, com.opengamma.strata.basics.date.BusinessDayConvention dayConvention)
	  public virtual void test_day_convention(IborIborSwapConvention convention, BusinessDayConvention dayConvention)
	  {
		assertEquals(convention.SpreadLeg.AccrualBusinessDayAdjustment.Convention, dayConvention);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "stubIbor") public static Object[][] data_stub_ibor()
	  public static object[][] data_stub_ibor()
	  {
		return new object[][]
		{
			new object[] {IborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M, Tenor.TENOR_8M},
			new object[] {IborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M, Tenor.TENOR_8M}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "stubIbor") public void test_stub_ibor(IborIborSwapConvention convention, com.opengamma.strata.basics.date.Tenor tenor)
	  public virtual void test_stub_ibor(IborIborSwapConvention convention, Tenor tenor)
	  {
		LocalDate tradeDate = LocalDate.of(2015, 10, 20);
		SwapTrade swap = convention.createTrade(tradeDate, tenor, BuySell.BUY, 1, 0.01, REF_DATA);
		ResolvedSwap swapResolved = swap.Product.resolve(REF_DATA);
		LocalDate endDate = swapResolved.getLeg(PayReceive.PAY).get().EndDate;
		assertTrue(endDate.isAfter(tradeDate.plus(tenor).minusMonths(1)));
		assertTrue(endDate.isBefore(tradeDate.plus(tenor).plusMonths(1)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(IborIborSwapConventions));
		coverPrivateConstructor(typeof(StandardIborIborSwapConventions));
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// Test <seealso cref="TermDepositConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TermDepositConventionTest
	public class TermDepositConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly BusinessDayAdjustment BDA_MOD_FOLLOW = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly DaysAdjustment PLUS_TWO_DAYS = DaysAdjustment.ofBusinessDays(2, EUTA);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_full()
	  {
		ImmutableTermDepositConvention test = ImmutableTermDepositConvention.builder().name("Test").businessDayAdjustment(BDA_MOD_FOLLOW).currency(EUR).dayCount(ACT_360).spotDateOffset(PLUS_TWO_DAYS).build();
		assertEquals(test.Name, "Test");
		assertEquals(test.BusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.Currency, EUR);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.SpotDateOffset, PLUS_TWO_DAYS);
	  }

	  public virtual void test_of()
	  {
		ImmutableTermDepositConvention test = ImmutableTermDepositConvention.of("EUR-Deposit", EUR, BDA_MOD_FOLLOW, ACT_360, PLUS_TWO_DAYS);
		assertEquals(test.Name, "EUR-Deposit");
		assertEquals(test.BusinessDayAdjustment, BDA_MOD_FOLLOW);
		assertEquals(test.Currency, EUR);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.SpotDateOffset, PLUS_TWO_DAYS);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade()
	  {
		TermDepositConvention convention = ImmutableTermDepositConvention.builder().name("EUR-Dep").businessDayAdjustment(BDA_MOD_FOLLOW).currency(EUR).dayCount(ACT_360).spotDateOffset(PLUS_TWO_DAYS).build();
		LocalDate tradeDate = LocalDate.of(2015, 1, 22);
		Period period3M = Period.ofMonths(3);
		BuySell buy = BuySell.BUY;
		double notional = 2_000_000d;
		double rate = 0.0125;
		TermDepositTrade trade = convention.createTrade(tradeDate, period3M, buy, notional, rate, REF_DATA);
		LocalDate startDateExpected = PLUS_TWO_DAYS.adjust(tradeDate, REF_DATA);
		LocalDate endDateExpected = startDateExpected.plus(period3M);
		TermDeposit termDepositExpected = TermDeposit.builder().buySell(buy).currency(EUR).notional(notional).startDate(startDateExpected).endDate(endDateExpected).businessDayAdjustment(BDA_MOD_FOLLOW).rate(rate).dayCount(ACT_360).build();
		TradeInfo tradeInfoExpected = TradeInfo.of(tradeDate);
		assertEquals(trade.Product, termDepositExpected);
		assertEquals(trade.Info, tradeInfoExpected);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {TermDepositConventions.USD_DEPOSIT_T2, "USD-Deposit-T2"},
			new object[] {TermDepositConventions.EUR_DEPOSIT_T2, "EUR-Deposit-T2"},
			new object[] {TermDepositConventions.GBP_DEPOSIT_T0, "GBP-Deposit-T0"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(TermDepositConvention convention, String name)
	  public virtual void test_name(TermDepositConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(TermDepositConvention convention, String name)
	  public virtual void test_toString(TermDepositConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(TermDepositConvention convention, String name)
	  public virtual void test_of_lookup(TermDepositConvention convention, string name)
	  {
		assertEquals(TermDepositConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(TermDepositConvention convention, String name)
	  public virtual void test_extendedEnum(TermDepositConvention convention, string name)
	  {
		TermDepositConvention.of(name); // ensures map is populated
		ImmutableMap<string, TermDepositConvention> map = TermDepositConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => TermDepositConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => TermDepositConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotAndConv") public static Object[][] data_spotAndConv()
	  public static object[][] data_spotAndConv()
	  {
		return new object[][]
		{
			new object[] {TermDepositConventions.GBP_DEPOSIT_T0, 0, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {TermDepositConventions.GBP_SHORT_DEPOSIT_T0, 0, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.GBP_SHORT_DEPOSIT_T1, 1, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.EUR_DEPOSIT_T2, 2, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {TermDepositConventions.EUR_SHORT_DEPOSIT_T0, 0, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.EUR_SHORT_DEPOSIT_T1, 1, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.EUR_SHORT_DEPOSIT_T2, 2, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.USD_DEPOSIT_T2, 2, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {TermDepositConventions.USD_SHORT_DEPOSIT_T0, 0, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.USD_SHORT_DEPOSIT_T1, 1, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.USD_SHORT_DEPOSIT_T2, 2, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.CHF_DEPOSIT_T2, 2, BusinessDayConventions.MODIFIED_FOLLOWING},
			new object[] {TermDepositConventions.CHF_SHORT_DEPOSIT_T0, 0, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.CHF_SHORT_DEPOSIT_T1, 1, BusinessDayConventions.FOLLOWING},
			new object[] {TermDepositConventions.CHF_SHORT_DEPOSIT_T2, 2, BusinessDayConventions.FOLLOWING}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotAndConv") public void test_spotAndConv(ImmutableTermDepositConvention convention, int spotT, com.opengamma.strata.basics.date.BusinessDayConvention conv)
	  public virtual void test_spotAndConv(ImmutableTermDepositConvention convention, int spotT, BusinessDayConvention conv)
	  {
		assertEquals(convention.SpotDateOffset.Days, spotT);
		assertEquals(convention.BusinessDayAdjustment.Convention, conv);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableTermDepositConvention test1 = ImmutableTermDepositConvention.of("EUR-Deposit", EUR, BDA_MOD_FOLLOW, ACT_360, PLUS_TWO_DAYS);
		coverImmutableBean(test1);
		ImmutableTermDepositConvention test2 = ImmutableTermDepositConvention.of("GBP-Deposit", GBP, BDA_MOD_FOLLOW, ACT_365F, DaysAdjustment.ofBusinessDays(0, GBLO));
		coverBeanEquals(test1, test2);

		coverPrivateConstructor(typeof(TermDepositConventions));
		coverPrivateConstructor(typeof(StandardTermDepositConventions));
	  }

	  public virtual void test_serialization()
	  {
		ImmutableTermDepositConvention test = ImmutableTermDepositConvention.of("EUR-Deposit", EUR, BDA_MOD_FOLLOW, ACT_360, PLUS_TWO_DAYS);
		assertSerialization(test);
	  }

	}

}
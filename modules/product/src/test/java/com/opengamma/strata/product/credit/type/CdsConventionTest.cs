/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Test <seealso cref="CdsConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsConventionTest
	public class CdsConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly DaysAdjustment SETTLE_DAY_ADJ = DaysAdjustment.ofBusinessDays(3, GBLO);
	  private static readonly DaysAdjustment SETTLE_DAY_ADJ_STD = DaysAdjustment.ofBusinessDays(3, SAT_SUN);
	  private static readonly DaysAdjustment STEPIN_DAY_ADJ = DaysAdjustment.ofCalendarDays(1);
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private const double COUPON = 0.05;
	  private const double NOTIONAL = 1.0e9;

	  private static readonly BusinessDayAdjustment BUSI_ADJ = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, GBLO);
	  private static readonly BusinessDayAdjustment BUSI_ADJ_STD = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
	  private const string NAME = "GB_CDS";

	  public virtual void test_of()
	  {
		ImmutableCdsConvention test = ImmutableCdsConvention.of(NAME, GBP, ACT_365F, P3M, BUSI_ADJ, SETTLE_DAY_ADJ);
		assertEquals(test.BusinessDayAdjustment, BUSI_ADJ);
		assertEquals(test.StartDateBusinessDayAdjustment, BUSI_ADJ);
		assertEquals(test.EndDateBusinessDayAdjustment, BusinessDayAdjustment.NONE);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Name, NAME);
		assertEquals(test.PaymentFrequency, P3M);
		assertEquals(test.PaymentOnDefault, PaymentOnDefault.ACCRUED_PREMIUM);
		assertEquals(test.ProtectionStart, ProtectionStartOfDay.BEGINNING);
		assertEquals(test.RollConvention, RollConventions.DAY_20);
		assertEquals(test.SettlementDateOffset, SETTLE_DAY_ADJ);
		assertEquals(test.StepinDateOffset, DaysAdjustment.ofCalendarDays(1));
		assertEquals(test.StubConvention, StubConvention.SMART_INITIAL);
	  }

	  public virtual void test_builder()
	  {
		ImmutableCdsConvention test = ImmutableCdsConvention.builder().businessDayAdjustment(BUSI_ADJ).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BUSI_ADJ).currency(GBP).dayCount(ACT_365F).name(NAME).paymentFrequency(P6M).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).rollConvention(RollConventions.NONE).settlementDateOffset(DaysAdjustment.ofCalendarDays(7)).stepinDateOffset(DaysAdjustment.NONE).stubConvention(StubConvention.LONG_INITIAL).build();
		assertEquals(test.BusinessDayAdjustment, BUSI_ADJ);
		assertEquals(test.StartDateBusinessDayAdjustment, BusinessDayAdjustment.NONE);
		assertEquals(test.EndDateBusinessDayAdjustment, BUSI_ADJ);
		assertEquals(test.Currency, GBP);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Name, NAME);
		assertEquals(test.PaymentFrequency, P6M);
		assertEquals(test.PaymentOnDefault, PaymentOnDefault.NONE);
		assertEquals(test.ProtectionStart, ProtectionStartOfDay.NONE);
		assertEquals(test.RollConvention, RollConventions.NONE);
		assertEquals(test.SettlementDateOffset, DaysAdjustment.ofCalendarDays(7));
		assertEquals(test.StepinDateOffset, DaysAdjustment.NONE);
		assertEquals(test.StubConvention, StubConvention.LONG_INITIAL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toTrade()
	  {
		LocalDate tradeDate = LocalDate.of(2015, 12, 21); // 19, 20 weekend
		LocalDate startDate = LocalDate.of(2015, 12, 20);
		LocalDate endDate = LocalDate.of(2020, 12, 20);
		LocalDate settlementDate = LocalDate.of(2015, 12, 24);
		TradeInfo info = TradeInfo.builder().tradeDate(tradeDate).settlementDate(settlementDate).build();
		Tenor tenor = Tenor.TENOR_5Y;
		ImmutableCdsConvention @base = ImmutableCdsConvention.of(NAME, GBP, ACT_360, P3M, BUSI_ADJ_STD, SETTLE_DAY_ADJ_STD);
		Cds product = Cds.builder().legalEntityId(LEGAL_ENTITY).paymentSchedule(PeriodicSchedule.builder().startDate(startDate).endDate(endDate).frequency(P3M).businessDayAdjustment(BUSI_ADJ_STD).startDateBusinessDayAdjustment(BUSI_ADJ_STD).endDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).stubConvention(StubConvention.SMART_INITIAL).rollConvention(RollConventions.DAY_20).build()).buySell(BUY).currency(GBP).dayCount(ACT_360).notional(NOTIONAL).fixedRate(COUPON).paymentOnDefault(PaymentOnDefault.ACCRUED_PREMIUM).protectionStart(ProtectionStartOfDay.BEGINNING).stepinDateOffset(STEPIN_DAY_ADJ).settlementDateOffset(SETTLE_DAY_ADJ_STD).build();
		CdsTrade expected = CdsTrade.builder().info(info).product(product).build();
		CdsTrade test1 = @base.createTrade(LEGAL_ENTITY, tradeDate, tenor, BUY, NOTIONAL, COUPON, REF_DATA);
		assertEquals(test1, expected);
		CdsTrade test2 = @base.createTrade(LEGAL_ENTITY, tradeDate, startDate, tenor, BUY, NOTIONAL, COUPON, REF_DATA);
		assertEquals(test2, expected);
		CdsTrade test3 = @base.createTrade(LEGAL_ENTITY, tradeDate, startDate, endDate, BUY, NOTIONAL, COUPON, REF_DATA);
		assertEquals(test3, expected);
		CdsTrade test4 = @base.toTrade(LEGAL_ENTITY, info, startDate, endDate, BUY, NOTIONAL, COUPON);
		assertEquals(test4, expected);

		AdjustablePayment upfront = AdjustablePayment.of(CurrencyAmount.of(GBP, 0.1 * NOTIONAL), settlementDate);
		CdsTrade expectedWithUf = CdsTrade.builder().info(info).product(product).upfrontFee(upfront).build();
		CdsTrade test5 = @base.createTrade(LEGAL_ENTITY, tradeDate, tenor, BUY, NOTIONAL, COUPON, upfront, REF_DATA);
		assertEquals(test5, expectedWithUf);
		CdsTrade test6 = @base.createTrade(LEGAL_ENTITY, tradeDate, startDate, tenor, BUY, NOTIONAL, COUPON, upfront, REF_DATA);
		assertEquals(test6, expectedWithUf);
		CdsTrade test7 = @base.createTrade(LEGAL_ENTITY, tradeDate, startDate, endDate, BUY, NOTIONAL, COUPON, upfront, REF_DATA);
		assertEquals(test7, expectedWithUf);
		CdsTrade test8 = @base.toTrade(LEGAL_ENTITY, info, startDate, endDate, BUY, NOTIONAL, COUPON, upfront);
		assertEquals(test8, expectedWithUf);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {CdsConventions.USD_STANDARD, "USD-STANDARD"},
			new object[] {CdsConventions.JPY_US_GB_STANDARD, "JPY-US-GB-STANDARD"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(CdsConvention convention, String name)
	  public virtual void test_name(CdsConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(CdsConvention convention, String name)
	  public virtual void test_toString(CdsConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(CdsConvention convention, String name)
	  public virtual void test_of_lookup(CdsConvention convention, string name)
	  {
		assertEquals(CdsConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(CdsConvention convention, String name)
	  public virtual void test_extendedEnum(CdsConvention convention, string name)
	  {
		CdsConvention.of(name); // ensures map is populated
		ImmutableMap<string, CdsConvention> map = CdsConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => CdsConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => CdsConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableCdsConvention test1 = ImmutableCdsConvention.of(NAME, GBP, ACT_360, P3M, BUSI_ADJ_STD, SETTLE_DAY_ADJ_STD);
		coverImmutableBean(test1);
		ImmutableCdsConvention test2 = ImmutableCdsConvention.builder().businessDayAdjustment(BUSI_ADJ).startDateBusinessDayAdjustment(BusinessDayAdjustment.NONE).endDateBusinessDayAdjustment(BUSI_ADJ).currency(USD).dayCount(ACT_365F).name("another").paymentFrequency(P6M).paymentOnDefault(PaymentOnDefault.NONE).protectionStart(ProtectionStartOfDay.NONE).rollConvention(RollConventions.NONE).settlementDateOffset(DaysAdjustment.ofCalendarDays(7)).stepinDateOffset(DaysAdjustment.NONE).stubConvention(StubConvention.LONG_INITIAL).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ImmutableCdsConvention test = ImmutableCdsConvention.of(NAME, GBP, ACT_360, P3M, BUSI_ADJ_STD, SETTLE_DAY_ADJ_STD);
		assertSerialization(test);
	  }

	}

}
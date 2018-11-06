/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.SAT_SUN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using Rounding = com.opengamma.strata.basics.value.Rounding;

	/// <summary>
	/// Test <seealso cref="BondFuture"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Test public class BondFutureTest
	public class BondFutureTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // Underlying bonds
	  private static readonly LegalEntityId ISSUER_ID = LegalEntityId.of("OG-Ticker", "GOVT1");
	  private const FixedCouponBondYieldConvention YIELD_CONVENTION = FixedCouponBondYieldConvention.US_STREET;
	  private const double NOTIONAL = 100000d;
	  private static readonly DaysAdjustment SETTLEMENT_DAYS = DaysAdjustment.ofBusinessDays(1, SAT_SUN);
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_ACT_ICMA;
	  private static readonly BusinessDayAdjustment BUSINESS_ADJUST = BusinessDayAdjustment.of(FOLLOWING, SAT_SUN);
	  private static readonly DaysAdjustment EX_COUPON = DaysAdjustment.NONE;
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "BondFuture");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "BondFuture2");

	  private const int NB_BOND = 7;
	  private static readonly double[] RATE = new double[] {0.01375, 0.02125, 0.0200, 0.02125, 0.0225, 0.0200, 0.0175};
	  private static readonly LocalDate[] START_DATE = new LocalDate[] {LocalDate.of(2010, 11, 30), LocalDate.of(2010, 12, 31), LocalDate.of(2011, 1, 31), LocalDate.of(2008, 2, 29), LocalDate.of(2011, 3, 31), LocalDate.of(2011, 4, 30), LocalDate.of(2011, 5, 31)};
	  private static readonly Period[] BOND_TENOR = new Period[] {Period.ofYears(5), Period.ofYears(5), Period.ofYears(5), Period.ofYears(8), Period.ofYears(5), Period.ofYears(5), Period.ofYears(5)};
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private static final FixedCouponBond[] BOND_PRODUCT = new FixedCouponBond[NB_BOND];
	  private static readonly FixedCouponBond[] BOND_PRODUCT = new FixedCouponBond[NB_BOND];
	  private static readonly ResolvedFixedCouponBond[] RESOLVED_BASKET = new ResolvedFixedCouponBond[NB_BOND];

	  static BondFutureTest()
	  {
		for (int i = 0; i < NB_BOND; ++i)
		{
		  LocalDate endDate = START_DATE[i].plus(BOND_TENOR[i]);
		  PeriodicSchedule periodSchedule = PeriodicSchedule.of(START_DATE[i], endDate, Frequency.P6M, BUSINESS_ADJUST, StubConvention.SHORT_INITIAL, false);
		  FixedCouponBond product = FixedCouponBond.builder().securityId(SecurityId.of("OG-Test", "Bond " + i)).dayCount(DAY_COUNT).fixedRate(RATE[i]).legalEntityId(ISSUER_ID).currency(USD).notional(NOTIONAL).accrualSchedule(periodSchedule).settlementDateOffset(SETTLEMENT_DAYS).yieldConvention(YIELD_CONVENTION).exCouponPeriod(EX_COUPON).build();
		  BOND_PRODUCT[i] = product;
		  RESOLVED_BASKET[i] = product.resolve(REF_DATA);
		}
	  }

	  // future specification
	  private static readonly double?[] CONVERSION_FACTOR = new double?[] {.8317, .8565, .8493, .8516, .8540, .8417, .8292};
	  private static readonly LocalDate LAST_TRADING_DATE = LocalDate.of(2011, 9, 30);
	  private static readonly LocalDate FIRST_NOTICE_DATE = LocalDate.of(2011, 8, 31);
	  private static readonly LocalDate LAST_NOTICE_DATE = LocalDate.of(2011, 10, 4);
	  private static readonly LocalDate FIRST_DELIVERY_DATE = SETTLEMENT_DAYS.adjust(FIRST_NOTICE_DATE, REF_DATA);
	  private static readonly LocalDate LAST_DELIVERY_DATE = SETTLEMENT_DAYS.adjust(LAST_NOTICE_DATE, REF_DATA);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(3);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_full()
	  {
		BondFuture test = BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(BOND_PRODUCT).conversionFactors(CONVERSION_FACTOR).firstNoticeDate(FIRST_NOTICE_DATE).firstDeliveryDate(FIRST_DELIVERY_DATE).lastNoticeDate(LAST_NOTICE_DATE).lastDeliveryDate(LAST_DELIVERY_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build();
		assertEquals(test.DeliveryBasket, ImmutableList.copyOf(BOND_PRODUCT));
		assertEquals(test.ConversionFactors, ImmutableList.copyOf(CONVERSION_FACTOR));
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.FirstNoticeDate, FIRST_NOTICE_DATE);
		assertEquals(test.LastNoticeDate, LAST_NOTICE_DATE);
		assertEquals(test.FirstDeliveryDate, FIRST_DELIVERY_DATE);
		assertEquals(test.LastDeliveryDate, LAST_DELIVERY_DATE);
		assertEquals(test.LastTradeDate, LAST_TRADING_DATE);
		assertEquals(test.Rounding, ROUNDING);
	  }

	  public virtual void test_builder_noDeliveryDate()
	  {
		BondFuture test = BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(BOND_PRODUCT).conversionFactors(CONVERSION_FACTOR).firstNoticeDate(FIRST_NOTICE_DATE).lastNoticeDate(LAST_NOTICE_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build();
		assertEquals(test.DeliveryBasket, ImmutableList.copyOf(BOND_PRODUCT));
		assertEquals(test.ConversionFactors, ImmutableList.copyOf(CONVERSION_FACTOR));
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.FirstNoticeDate, FIRST_NOTICE_DATE);
		assertEquals(test.LastNoticeDate, LAST_NOTICE_DATE);
		assertEquals(test.FirstDeliveryDate, null);
		assertEquals(test.LastDeliveryDate, null);
		assertEquals(test.LastTradeDate, LAST_TRADING_DATE);
		assertEquals(test.Rounding, ROUNDING);

	  }

	  public virtual void test_builder_fail()
	  {
		// wrong size
		assertThrowsIllegalArg(() => BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(BOND_PRODUCT[0]).conversionFactors(CONVERSION_FACTOR).firstNoticeDate(FIRST_NOTICE_DATE).lastNoticeDate(LAST_NOTICE_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build());
		// first notice date missing
		assertThrowsIllegalArg(() => BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(BOND_PRODUCT).conversionFactors(CONVERSION_FACTOR).lastNoticeDate(LAST_NOTICE_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build());
		// last notice date missing
		assertThrowsIllegalArg(() => BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(BOND_PRODUCT).conversionFactors(CONVERSION_FACTOR).firstNoticeDate(FIRST_NOTICE_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build());
		// basket list empty
		assertThrowsIllegalArg(() => BondFuture.builder().securityId(SECURITY_ID).conversionFactors(CONVERSION_FACTOR).firstNoticeDate(FIRST_NOTICE_DATE).lastNoticeDate(LAST_NOTICE_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build());
		// notional mismatch
		FixedCouponBond bond0 = BOND_PRODUCT[0];
		FixedCouponBond bond1 = bond0.toBuilder().notional(100).build();
		FixedCouponBond bond2 = bond0.toBuilder().currency(Currency.CAD).build();
		assertThrowsIllegalArg(() => BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(bond0, bond1).conversionFactors(CONVERSION_FACTOR[0], CONVERSION_FACTOR[1]).firstNoticeDate(FIRST_NOTICE_DATE).lastNoticeDate(LAST_NOTICE_DATE).firstDeliveryDate(FIRST_DELIVERY_DATE).lastDeliveryDate(LAST_DELIVERY_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build());
		// currency mismatch
		assertThrowsIllegalArg(() => BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(bond0, bond2).conversionFactors(CONVERSION_FACTOR[0], CONVERSION_FACTOR[1]).firstNoticeDate(FIRST_NOTICE_DATE).lastNoticeDate(LAST_NOTICE_DATE).firstDeliveryDate(FIRST_DELIVERY_DATE).lastDeliveryDate(LAST_DELIVERY_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		ResolvedBondFuture expected = ResolvedBondFuture.builder().securityId(SECURITY_ID).deliveryBasket(RESOLVED_BASKET).conversionFactors(CONVERSION_FACTOR).lastTradeDate(LAST_TRADING_DATE).firstNoticeDate(FIRST_NOTICE_DATE).lastNoticeDate(LAST_NOTICE_DATE).firstDeliveryDate(FIRST_DELIVERY_DATE).lastDeliveryDate(LAST_DELIVERY_DATE).rounding(ROUNDING).build();
		assertEquals(sut().resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static BondFuture sut()
	  {
		return BondFuture.builder().securityId(SECURITY_ID).deliveryBasket(BOND_PRODUCT).conversionFactors(ImmutableList.copyOf(CONVERSION_FACTOR)).firstNoticeDate(FIRST_NOTICE_DATE).firstDeliveryDate(FIRST_DELIVERY_DATE).lastNoticeDate(LAST_NOTICE_DATE).lastDeliveryDate(LAST_DELIVERY_DATE).lastTradeDate(LAST_TRADING_DATE).rounding(ROUNDING).build();
	  }

	  internal static BondFuture sut2()
	  {
		return BondFuture.builder().securityId(SECURITY_ID2).conversionFactors(0.9187).deliveryBasket(BOND_PRODUCT[3]).firstNoticeDate(FIRST_NOTICE_DATE.plusDays(7)).lastNoticeDate(LAST_NOTICE_DATE.plusDays(7)).lastTradeDate(LAST_TRADING_DATE.plusDays(7)).build();
	  }

	}

}
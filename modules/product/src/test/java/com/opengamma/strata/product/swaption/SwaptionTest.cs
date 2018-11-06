/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.LONG;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.LongShort.SHORT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;
	using FixedOvernightSwapConventions = com.opengamma.strata.product.swap.type.FixedOvernightSwapConventions;
	using IborIborSwapConventions = com.opengamma.strata.product.swap.type.IborIborSwapConventions;
	using XCcyIborIborSwapConventions = com.opengamma.strata.product.swap.type.XCcyIborIborSwapConventions;

	/// <summary>
	/// Test <seealso cref="Swaption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionTest
	public class SwaptionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2014, 6, 12); // starts on 2014/6/19
	  private const double FIXED_RATE = 0.015;
	  private const double NOTIONAL = 100000000d;
	  private static readonly Swap SWAP = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(TRADE_DATE, Tenor.TENOR_10Y, BuySell.BUY, NOTIONAL, FIXED_RATE, REF_DATA).Product;
	  private static readonly BusinessDayAdjustment ADJUSTMENT = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, GBLO.combinedWith(USNY));
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2014, 6, 14);
	  private static readonly LocalTime EXPIRY_TIME = LocalTime.of(11, 0);
	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly AdjustableDate ADJUSTABLE_EXPIRY_DATE = AdjustableDate.of(EXPIRY_DATE, ADJUSTMENT);
	  private static readonly SwaptionSettlement PHYSICAL_SETTLE = PhysicalSwaptionSettlement.DEFAULT;
	  private static readonly SwaptionSettlement CASH_SETTLE = CashSwaptionSettlement.of(SWAP.StartDate.Unadjusted, CashSwaptionSettlementMethod.PAR_YIELD);
	  private static readonly Swap SWAP_OIS = FixedOvernightSwapConventions.USD_FIXED_1Y_FED_FUND_OIS.createTrade(TRADE_DATE, Tenor.TENOR_10Y, BuySell.BUY, NOTIONAL, FIXED_RATE, REF_DATA).Product;
	  private static readonly Swap SWAP_BASIS = IborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M.createTrade(TRADE_DATE, Tenor.TENOR_10Y, BuySell.BUY, NOTIONAL, FIXED_RATE, REF_DATA).Product;
	  private static readonly Swap SWAP_XCCY = XCcyIborIborSwapConventions.EUR_EURIBOR_3M_USD_LIBOR_3M.createTrade(TRADE_DATE, Tenor.TENOR_10Y, BuySell.BUY, NOTIONAL, NOTIONAL * 1.1, FIXED_RATE, REF_DATA).Product;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		Swaption test = sut();
		assertEquals(test.ExpiryDate, ADJUSTABLE_EXPIRY_DATE);
		assertEquals(test.ExpiryTime, EXPIRY_TIME);
		assertEquals(test.ExpiryZone, ZONE);
		assertEquals(test.Expiry, EXPIRY_DATE.atTime(EXPIRY_TIME).atZone(ZONE));
		assertEquals(test.LongShort, LONG);
		assertEquals(test.SwaptionSettlement, PHYSICAL_SETTLE);
		assertEquals(test.Underlying, SWAP);
		assertEquals(test.Currency, USD);
		assertEquals(test.Index, IborIndices.USD_LIBOR_3M);
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(USD));
		assertEquals(test.allCurrencies(), ImmutableSet.of(USD));
	  }

	  public virtual void test_builder_expiryAfterStart()
	  {
		assertThrowsIllegalArg(() => Swaption.builder().expiryDate(AdjustableDate.of(LocalDate.of(2014, 6, 17), ADJUSTMENT)).expiryTime(EXPIRY_TIME).expiryZone(ZONE).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP).build());
	  }

	  public virtual void test_builder_invalidSwapOis()
	  {
		assertThrowsIllegalArg(() => Swaption.builder().expiryDate(ADJUSTABLE_EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(ZONE).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_OIS).build());
	  }

	  public virtual void test_builder_invalidSwapBasis()
	  {
		assertThrowsIllegalArg(() => Swaption.builder().expiryDate(ADJUSTABLE_EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(ZONE).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_BASIS).build());
	  }

	  public virtual void test_builder_invalidSwapXCcy()
	  {
		assertThrowsIllegalArg(() => Swaption.builder().expiryDate(ADJUSTABLE_EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(ZONE).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP_XCCY).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		Swaption @base = sut();
		ResolvedSwaption test = @base.resolve(REF_DATA);
		assertEquals(test.Expiry, ADJUSTMENT.adjust(EXPIRY_DATE, REF_DATA).atTime(EXPIRY_TIME).atZone(ZONE));
		assertEquals(test.LongShort, LONG);
		assertEquals(test.SwaptionSettlement, PHYSICAL_SETTLE);
		assertEquals(test.Underlying, SWAP.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static Swaption sut()
	  {
		return Swaption.builder().expiryDate(ADJUSTABLE_EXPIRY_DATE).expiryTime(EXPIRY_TIME).expiryZone(ZONE).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP).build();
	  }

	  internal static Swaption sut2()
	  {
		return Swaption.builder().expiryDate(AdjustableDate.of(LocalDate.of(2014, 6, 10), ADJUSTMENT)).expiryTime(LocalTime.of(14, 0)).expiryZone(ZoneId.of("GMT")).longShort(SHORT).swaptionSettlement(CASH_SETTLE).underlying(FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(LocalDate.of(2014, 6, 10), Tenor.TENOR_10Y, BuySell.BUY, 1d, FIXED_RATE, REF_DATA).Product).build();
	  }

	}

}
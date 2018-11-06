/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using OvernightRateComputation = com.opengamma.strata.product.rate.OvernightRateComputation;
	using OvernightAccrualMethod = com.opengamma.strata.product.swap.OvernightAccrualMethod;

	/// <summary>
	/// Test <seealso cref="OvernightFuture"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightFutureTest
	public class OvernightFutureTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL = 5_000_000d;
	  private const double NOTIONAL2 = 10_000_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly double ACCRUAL_FACTOR2 = TENOR_3M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 9, 28);
	  private static readonly LocalDate START_DATE = date(2018, 9, 1);
	  private static readonly LocalDate END_DATE = date(2018, 9, 30);
	  private static readonly LocalDate LAST_TRADE_DATE2 = date(2018, 6, 15);
	  private static readonly LocalDate START_DATE2 = date(2018, 3, 15);
	  private static readonly LocalDate END_DATE2 = date(2018, 6, 15);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(5);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "OnFuture2");

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		OvernightFuture test = sut();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Index, USD_FED_FUND);
		assertEquals(test.Rounding, ROUNDING);
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.AccrualMethod, OvernightAccrualMethod.AVERAGED_DAILY);
	  }

	  public virtual void test_builder_default()
	  {
		OvernightFuture test = OvernightFuture.builder().securityId(SECURITY_ID).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).build();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Index, USD_FED_FUND);
		assertEquals(test.Rounding, Rounding.none());
		assertEquals(test.StartDate, START_DATE);
		assertEquals(test.EndDate, END_DATE);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.AccrualMethod, OvernightAccrualMethod.AVERAGED_DAILY);
	  }

	  public virtual void test_builder_noIndex()
	  {
		assertThrowsIllegalArg(() => OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build());
	  }

	  public virtual void test_builder_wrongDateOrderDate()
	  {
		assertThrowsIllegalArg(() => OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(END_DATE).endDate(START_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		OvernightFuture @base = sut();
		ResolvedOvernightFuture expected = ResolvedOvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).overnightRate(OvernightRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 0, OvernightAccrualMethod.AVERAGED_DAILY, REF_DATA)).lastTradeDate(LAST_TRADE_DATE).rounding(ROUNDING).build();
		assertEquals(@base.resolve(REF_DATA), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightFuture test1 = sut();
		coverImmutableBean(test1);
		OvernightFuture test2 = sut2();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightFuture test = sut();
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal static OvernightFuture sut()
	  {
		return OvernightFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).startDate(START_DATE).endDate(END_DATE).lastTradeDate(LAST_TRADE_DATE).index(USD_FED_FUND).accrualMethod(OvernightAccrualMethod.AVERAGED_DAILY).rounding(ROUNDING).build();
	  }

	  internal static OvernightFuture sut2()
	  {
		return OvernightFuture.builder().securityId(SECURITY_ID2).currency(GBP).notional(NOTIONAL2).accrualFactor(ACCRUAL_FACTOR2).startDate(START_DATE2).endDate(END_DATE2).lastTradeDate(LAST_TRADE_DATE2).index(GBP_SONIA).accrualMethod(OvernightAccrualMethod.COMPOUNDED).rounding(Rounding.none()).build();
	  }

	}

}
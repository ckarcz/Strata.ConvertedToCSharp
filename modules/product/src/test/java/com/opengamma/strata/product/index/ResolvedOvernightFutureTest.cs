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
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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
	/// Test <seealso cref="ResolvedOvernightFuture"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedOvernightFutureTest
	public class ResolvedOvernightFutureTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL = 1_000_000d;
	  private static readonly double ACCRUAL_FACTOR_1M = TENOR_1M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2018, 6, 29);
	  private static readonly LocalDate START_DATE = date(2018, 6, 1);
	  private static readonly LocalDate END_DATE = date(2018, 6, 29);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(6);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "OnFuture");
	  private static readonly OvernightRateComputation RATE_COMPUTATION = OvernightRateComputation.of(USD_FED_FUND, START_DATE, END_DATE, 0, OvernightAccrualMethod.AVERAGED_DAILY, REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedOvernightFuture test = ResolvedOvernightFuture.builder().currency(USD).accrualFactor(ACCRUAL_FACTOR_1M).lastTradeDate(LAST_TRADE_DATE).overnightRate(RATE_COMPUTATION).notional(NOTIONAL).rounding(ROUNDING).securityId(SECURITY_ID).build();
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR_1M);
		assertEquals(test.Currency, USD);
		assertEquals(test.Index, USD_FED_FUND);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.OvernightRate, RATE_COMPUTATION);
		assertEquals(test.Rounding, ROUNDING);
		assertEquals(test.SecurityId, SECURITY_ID);
	  }

	  public virtual void test_builder_default()
	  {
		ResolvedOvernightFuture test = ResolvedOvernightFuture.builder().accrualFactor(ACCRUAL_FACTOR_1M).lastTradeDate(LAST_TRADE_DATE).overnightRate(RATE_COMPUTATION).notional(NOTIONAL).securityId(SECURITY_ID).build();
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR_1M);
		assertEquals(test.Currency, USD);
		assertEquals(test.Index, USD_FED_FUND);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.OvernightRate, RATE_COMPUTATION);
		assertEquals(test.Rounding, Rounding.none());
		assertEquals(test.SecurityId, SECURITY_ID);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedOvernightFuture test1 = ResolvedOvernightFuture.builder().currency(USD).accrualFactor(ACCRUAL_FACTOR_1M).lastTradeDate(LAST_TRADE_DATE).overnightRate(RATE_COMPUTATION).notional(NOTIONAL).rounding(ROUNDING).securityId(SECURITY_ID).build();
		coverImmutableBean(test1);
		ResolvedOvernightFuture test2 = ResolvedOvernightFuture.builder().currency(GBP).accrualFactor(0.25).lastTradeDate(date(2018, 9, 28)).overnightRate(OvernightRateComputation.of(GBP_SONIA, date(2018, 9, 1), date(2018, 9, 30), 0, OvernightAccrualMethod.AVERAGED_DAILY, REF_DATA)).notional(1.0e8).securityId(SecurityId.of("OG-Test", "OnFuture2")).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedOvernightFuture test = ResolvedOvernightFuture.builder().currency(USD).accrualFactor(ACCRUAL_FACTOR_1M).lastTradeDate(LAST_TRADE_DATE).overnightRate(RATE_COMPUTATION).notional(NOTIONAL).rounding(ROUNDING).securityId(SECURITY_ID).build();
		assertSerialization(test);
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
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
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="IborFuture"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureTest
	public class IborFutureTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL = 1_000d;
	  private const double NOTIONAL2 = 2_000d;
	  private static readonly double ACCRUAL_FACTOR = TENOR_3M.Period.toTotalMonths() / 12.0;
	  private static readonly double ACCRUAL_FACTOR2 = TENOR_2M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2015, 6, 15);
	  private static readonly LocalDate LAST_TRADE_DATE2 = date(2015, 6, 18);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(6);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "IborFuture");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "IborFuture2");

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		IborFuture test = sut();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Currency, USD);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Rounding, ROUNDING);
		assertEquals(test.FixingDate, LAST_TRADE_DATE);
	  }

	  public virtual void test_builder_defaults()
	  {
		IborFuture test = IborFuture.builder().securityId(SECURITY_ID).currency(GBP).notional(NOTIONAL).lastTradeDate(LAST_TRADE_DATE).index(GBP_LIBOR_2M).build();
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR2);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Index, GBP_LIBOR_2M);
		assertEquals(test.Rounding, Rounding.none());
		assertEquals(test.FixingDate, LAST_TRADE_DATE);
	  }

	  public virtual void test_builder_noIndex()
	  {
		assertThrowsIllegalArg(() => IborFuture.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(GBP).lastTradeDate(LAST_TRADE_DATE).rounding(ROUNDING).build());
	  }

	  public virtual void test_builder_noCurrency()
	  {
		IborFuture test = IborFuture.builder().securityId(SECURITY_ID).notional(NOTIONAL).index(GBP_LIBOR_2M).lastTradeDate(LAST_TRADE_DATE).rounding(ROUNDING).build();
		assertEquals(GBP, test.Currency);
	  }

	  public virtual void test_builder_noLastTradeDate()
	  {
		assertThrowsIllegalArg(() => IborFuture.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(GBP).index(GBP_LIBOR_2M).rounding(ROUNDING).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		IborFuture test = sut();
		ResolvedIborFuture expected = ResolvedIborFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).iborRate(IborRateComputation.of(USD_LIBOR_3M, LAST_TRADE_DATE, REF_DATA)).rounding(ROUNDING).build();
		assertEquals(test.resolve(REF_DATA), expected);
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
	  internal static IborFuture sut()
	  {
		return IborFuture.builder().securityId(SECURITY_ID).currency(USD).notional(NOTIONAL).accrualFactor(ACCRUAL_FACTOR).lastTradeDate(LAST_TRADE_DATE).index(USD_LIBOR_3M).rounding(ROUNDING).build();
	  }

	  internal static IborFuture sut2()
	  {
		return IborFuture.builder().securityId(SECURITY_ID2).currency(GBP).notional(NOTIONAL2).accrualFactor(ACCRUAL_FACTOR2).lastTradeDate(LAST_TRADE_DATE2).index(GBP_LIBOR_2M).build();
	  }

	}

}
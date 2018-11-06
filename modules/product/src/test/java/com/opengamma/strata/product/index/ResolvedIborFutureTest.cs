/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_2M;
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
	/// Test <seealso cref="ResolvedIborFuture"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborFutureTest
	public class ResolvedIborFutureTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborFuture PRODUCT = IborFutureTest.sut();
	  private static readonly IborFuture PRODUCT2 = IborFutureTest.sut2();
	  private const double NOTIONAL = 1_000d;
	  private static readonly double ACCRUAL_FACTOR_2M = TENOR_2M.Period.toTotalMonths() / 12.0;
	  private static readonly LocalDate LAST_TRADE_DATE = date(2015, 6, 15);
	  private static readonly Rounding ROUNDING = Rounding.ofDecimalPlaces(6);
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "IborFuture");

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedIborFuture test = sut();
		assertEquals(test.Currency, PRODUCT.Currency);
		assertEquals(test.Notional, PRODUCT.Notional);
		assertEquals(test.AccrualFactor, PRODUCT.AccrualFactor);
		assertEquals(test.LastTradeDate, PRODUCT.LastTradeDate);
		assertEquals(test.Index, PRODUCT.Index);
		assertEquals(test.Rounding, PRODUCT.Rounding);
		assertEquals(test.IborRate, IborRateComputation.of(PRODUCT.Index, PRODUCT.LastTradeDate, REF_DATA));
	  }

	  public virtual void test_builder_defaults()
	  {
		ResolvedIborFuture test = ResolvedIborFuture.builder().securityId(SECURITY_ID).currency(GBP).notional(NOTIONAL).iborRate(IborRateComputation.of(GBP_LIBOR_2M, LAST_TRADE_DATE, REF_DATA)).build();
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.AccrualFactor, ACCRUAL_FACTOR_2M);
		assertEquals(test.LastTradeDate, LAST_TRADE_DATE);
		assertEquals(test.Index, GBP_LIBOR_2M);
		assertEquals(test.Rounding, Rounding.none());
		assertEquals(test.IborRate, IborRateComputation.of(GBP_LIBOR_2M, LAST_TRADE_DATE, REF_DATA));
	  }

	  public virtual void test_builder_noObservation()
	  {
		assertThrowsIllegalArg(() => ResolvedIborFuture.builder().securityId(SECURITY_ID).notional(NOTIONAL).currency(GBP).rounding(ROUNDING).build());
	  }

	  public virtual void test_builder_noCurrency()
	  {
		ResolvedIborFuture test = ResolvedIborFuture.builder().securityId(SECURITY_ID).notional(NOTIONAL).iborRate(IborRateComputation.of(GBP_LIBOR_2M, LAST_TRADE_DATE, REF_DATA)).rounding(ROUNDING).build();
		assertEquals(GBP, test.Currency);
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
	  internal static ResolvedIborFuture sut()
	  {
		return PRODUCT.resolve(REF_DATA);
	  }

	  internal static ResolvedIborFuture sut2()
	  {
		return PRODUCT2.resolve(REF_DATA);
	  }

	}

}
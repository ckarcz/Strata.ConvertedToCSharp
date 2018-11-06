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
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;

	/// <summary>
	/// Test <seealso cref="ResolvedSwaption"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedSwaptionTest
	public class ResolvedSwaptionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate TRADE_DATE = LocalDate.of(2014, 6, 12); // starts on 2014/6/19
	  private const double FIXED_RATE = 0.015;
	  private const double NOTIONAL = 100000000d;
	  private static readonly ResolvedSwap SWAP = FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(TRADE_DATE, Tenor.TENOR_10Y, BuySell.BUY, NOTIONAL, FIXED_RATE, REF_DATA).Product.resolve(REF_DATA);
	  private static readonly ZoneId EUROPE_LONDON = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime EXPIRY = ZonedDateTime.of(2014, 6, 13, 11, 0, 0, 0, EUROPE_LONDON);
	  private static readonly SwaptionSettlement PHYSICAL_SETTLE = PhysicalSwaptionSettlement.DEFAULT;
	  private static readonly SwaptionSettlement CASH_SETTLE = CashSwaptionSettlement.of(SWAP.Legs.get(0).StartDate, CashSwaptionSettlementMethod.PAR_YIELD);

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedSwaption test = sut();
		assertEquals(test.ExpiryDate, EXPIRY.toLocalDate());
		assertEquals(test.Expiry, EXPIRY);
		assertEquals(test.LongShort, LONG);
		assertEquals(test.SwaptionSettlement, PHYSICAL_SETTLE);
		assertEquals(test.Underlying, SWAP);
		assertEquals(test.Currency, USD);
		assertEquals(test.Index, USD_LIBOR_3M);
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
	  internal static ResolvedSwaption sut()
	  {
		return ResolvedSwaption.builder().expiry(EXPIRY).longShort(LONG).swaptionSettlement(PHYSICAL_SETTLE).underlying(SWAP).build();
	  }

	  internal static ResolvedSwaption sut2()
	  {
		return ResolvedSwaption.builder().expiry(EXPIRY.plusHours(1)).longShort(SHORT).swaptionSettlement(CASH_SETTLE).underlying(FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M.createTrade(LocalDate.of(2014, 6, 10), Tenor.TENOR_10Y, BuySell.BUY, 1d, FIXED_RATE, REF_DATA).Product.resolve(REF_DATA)).build();
	  }

	}

}
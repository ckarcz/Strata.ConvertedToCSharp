/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_6M;
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


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Test <seealso cref="IborFixingDepositTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFixingDepositTemplateTest
	public class IborFixingDepositTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborFixingDepositConvention CONVENTION = IborFixingDepositConvention.of(EUR_LIBOR_3M);

	  public virtual void test_builder()
	  {
		IborFixingDepositTemplate test = IborFixingDepositTemplate.builder().convention(CONVENTION).depositPeriod(Period.ofMonths(1)).build();
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DepositPeriod, Period.ofMonths(1));
	  }

	  public virtual void test_builder_noPeriod()
	  {
		IborFixingDepositTemplate test = IborFixingDepositTemplate.builder().convention(CONVENTION).build();
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DepositPeriod, EUR_LIBOR_3M.Tenor.Period);
	  }

	  public virtual void test_build_negativePeriod()
	  {
		assertThrowsIllegalArg(() => IborFixingDepositTemplate.builder().convention(CONVENTION).depositPeriod(Period.ofMonths(-3)).build());
	  }

	  public virtual void test_of_index()
	  {
		IborFixingDepositTemplate test = IborFixingDepositTemplate.of(EUR_LIBOR_3M);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DepositPeriod, EUR_LIBOR_3M.Tenor.Period);
	  }

	  public virtual void test_of_periodAndIndex()
	  {
		IborFixingDepositTemplate test = IborFixingDepositTemplate.of(Period.ofMonths(1), EUR_LIBOR_3M);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DepositPeriod, Period.ofMonths(1));
	  }

	  public virtual void test_createTrade()
	  {
		IborFixingDepositTemplate template = IborFixingDepositTemplate.of(EUR_LIBOR_3M);
		double notional = 1d;
		double fixedRate = 0.045;
		LocalDate tradeDate = LocalDate.of(2015, 1, 22);
		IborFixingDepositTrade trade = template.createTrade(tradeDate, BUY, notional, fixedRate, REF_DATA);
		ImmutableIborFixingDepositConvention conv = (ImmutableIborFixingDepositConvention) template.Convention;
		LocalDate startExpected = conv.SpotDateOffset.adjust(tradeDate, REF_DATA);
		LocalDate endExpected = startExpected.plus(template.DepositPeriod);
		IborFixingDeposit productExpected = IborFixingDeposit.builder().businessDayAdjustment(conv.BusinessDayAdjustment).buySell(BUY).startDate(startExpected).endDate(endExpected).fixedRate(fixedRate).index(EUR_LIBOR_3M).notional(notional).build();
		TradeInfo tradeInfoExpected = TradeInfo.builder().tradeDate(tradeDate).build();
		assertEquals(trade.Info, tradeInfoExpected);
		assertEquals(trade.Product, productExpected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborFixingDepositTemplate test1 = IborFixingDepositTemplate.of(EUR_LIBOR_3M);
		coverImmutableBean(test1);
		IborFixingDepositTemplate test2 = IborFixingDepositTemplate.of(GBP_LIBOR_6M);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborFixingDepositTemplate test = IborFixingDepositTemplate.of(EUR_LIBOR_3M);
		assertSerialization(test);
	  }

	}

}
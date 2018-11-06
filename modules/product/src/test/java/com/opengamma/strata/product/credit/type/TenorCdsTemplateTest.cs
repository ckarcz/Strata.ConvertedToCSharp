/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_2Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;

	/// <summary>
	/// Test <seealso cref="TenorCdsTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TenorCdsTemplateTest
	public class TenorCdsTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly CdsConvention CONV1 = CdsConventions.EUR_GB_STANDARD;
	  private static readonly CdsConvention CONV2 = CdsConventions.USD_STANDARD;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "BCD");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		TenorCdsTemplate test = TenorCdsTemplate.of(TENOR_10Y, CONV1);
		assertEquals(test.AccrualStart, AccrualStart.IMM_DATE);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV1);
	  }

	  public virtual void test_of_accStart()
	  {
		TenorCdsTemplate test = TenorCdsTemplate.of(AccrualStart.NEXT_DAY, TENOR_10Y, CONV2);
		assertEquals(test.AccrualStart, AccrualStart.NEXT_DAY);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Convention, CONV2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		TenorCdsTemplate base1 = TenorCdsTemplate.of(TENOR_10Y, CONV1);
		TenorCdsTemplate base2 = TenorCdsTemplate.of(AccrualStart.NEXT_DAY, TENOR_2Y, CONV2);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		LocalDate startDate1 = date(2015, 3, 20);
		LocalDate endDate1 = date(2025, 6, 20);
		LocalDate startDate2 = date(2015, 5, 6);
		LocalDate endDate2 = date(2017, 6, 20);
		CdsTrade test1 = base1.createTrade(LEGAL_ENTITY, tradeDate, BUY, NOTIONAL_2M, 0.05d, REF_DATA);
		CdsTrade test2 = base2.createTrade(LEGAL_ENTITY, tradeDate, BUY, NOTIONAL_2M, 0.05d, REF_DATA);
		Cds expected1 = Cds.of(BUY, LEGAL_ENTITY, CONV1.Currency, NOTIONAL_2M, startDate1, endDate1, Frequency.P3M, CONV1.SettlementDateOffset.Calendar, 0.05d);
		PeriodicSchedule sch1 = expected1.PaymentSchedule;
		expected1 = expected1.toBuilder().paymentSchedule(sch1.toBuilder().startDateBusinessDayAdjustment(sch1.BusinessDayAdjustment).rollConvention(RollConventions.DAY_20).build()).build();
		Cds expected2 = Cds.of(BUY, LEGAL_ENTITY, CONV2.Currency, NOTIONAL_2M, startDate2, endDate2, Frequency.P3M, CONV2.SettlementDateOffset.Calendar, 0.05d);
		PeriodicSchedule sch2 = expected2.PaymentSchedule;
		expected2 = expected2.toBuilder().paymentSchedule(sch2.toBuilder().startDateBusinessDayAdjustment(sch2.BusinessDayAdjustment).rollConvention(RollConventions.DAY_20).build()).build();
		assertEquals(test1.Info.TradeDate, tradeDate);
		assertEquals(test1.Product, expected1);
		assertEquals(test1.UpfrontFee, null);
		assertEquals(test2.Info.TradeDate, tradeDate);
		assertEquals(test2.UpfrontFee, null);
		assertEquals(test2.Product, expected2);
	  }

	  public virtual void test_createTrade_withFee()
	  {
		TenorCdsTemplate base1 = TenorCdsTemplate.of(TENOR_10Y, CONV1);
		TenorCdsTemplate base2 = TenorCdsTemplate.of(AccrualStart.NEXT_DAY, TENOR_2Y, CONV2);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		AdjustablePayment payment1 = AdjustablePayment.of(EUR, NOTIONAL_2M, CONV1.SettlementDateOffset.adjust(tradeDate, REF_DATA));
		AdjustablePayment payment2 = AdjustablePayment.of(USD, NOTIONAL_2M, CONV2.SettlementDateOffset.adjust(tradeDate, REF_DATA));
		LocalDate startDate1 = date(2015, 3, 20);
		LocalDate endDate1 = date(2025, 6, 20);
		LocalDate startDate2 = date(2015, 5, 6);
		LocalDate endDate2 = date(2017, 6, 20);
		CdsTrade test1 = base1.createTrade(LEGAL_ENTITY, tradeDate, BUY, NOTIONAL_2M, 0.05d, payment1, REF_DATA);
		CdsTrade test2 = base2.createTrade(LEGAL_ENTITY, tradeDate, BUY, NOTIONAL_2M, 0.05d, payment2, REF_DATA);
		Cds expected1 = Cds.of(BUY, LEGAL_ENTITY, CONV1.Currency, NOTIONAL_2M, startDate1, endDate1, Frequency.P3M, CONV1.SettlementDateOffset.Calendar, 0.05d);
		PeriodicSchedule sch1 = expected1.PaymentSchedule;
		expected1 = expected1.toBuilder().paymentSchedule(sch1.toBuilder().startDateBusinessDayAdjustment(sch1.BusinessDayAdjustment).rollConvention(RollConventions.DAY_20).build()).build();
		Cds expected2 = Cds.of(BUY, LEGAL_ENTITY, CONV2.Currency, NOTIONAL_2M, startDate2, endDate2, Frequency.P3M, CONV2.SettlementDateOffset.Calendar, 0.05d);
		PeriodicSchedule sch2 = expected2.PaymentSchedule;
		expected2 = expected2.toBuilder().paymentSchedule(sch2.toBuilder().startDateBusinessDayAdjustment(sch2.BusinessDayAdjustment).rollConvention(RollConventions.DAY_20).build()).build();
		assertEquals(test1.Info.TradeDate, tradeDate);
		assertEquals(test1.UpfrontFee, payment1);
		assertEquals(test1.Product, expected1);
		assertEquals(test2.Info.TradeDate, tradeDate);
		assertEquals(test2.UpfrontFee, payment2);
		assertEquals(test2.Product, expected2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TenorCdsTemplate test1 = TenorCdsTemplate.of(TENOR_10Y, CONV1);
		coverImmutableBean(test1);
		TenorCdsTemplate test2 = TenorCdsTemplate.of(AccrualStart.NEXT_DAY, TENOR_10Y, CONV2);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		TenorCdsTemplate test = TenorCdsTemplate.of(TENOR_10Y, CONV1);
		assertSerialization(test);
	  }

	}

}
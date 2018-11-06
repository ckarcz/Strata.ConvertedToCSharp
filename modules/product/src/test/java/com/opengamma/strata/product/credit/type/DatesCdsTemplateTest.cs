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
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;

	/// <summary>
	/// Test <seealso cref="DatesCdsTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DatesCdsTemplateTest
	public class DatesCdsTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_2M = 2_000_000d;
	  private static readonly LocalDate START = LocalDate.of(2016, 2, 21);
	  private static readonly LocalDate END = LocalDate.of(2019, 5, 16);
	  private static readonly CdsConvention CONV1 = CdsConventions.EUR_GB_STANDARD;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "BCD");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		DatesCdsTemplate test = DatesCdsTemplate.of(START, END, CONV1);
		assertEquals(test.StartDate, START);
		assertEquals(test.EndDate, END);
		assertEquals(test.Convention, CONV1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		DatesCdsTemplate @base = DatesCdsTemplate.of(START, END, CONV1);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		CdsTrade test = @base.createTrade(LEGAL_ENTITY, tradeDate, BUY, NOTIONAL_2M, 0.05d, REF_DATA);
		Cds expected = Cds.of(BUY, LEGAL_ENTITY, CONV1.Currency, NOTIONAL_2M, START, END, Frequency.P3M, CONV1.SettlementDateOffset.Calendar, 0.05d);
		PeriodicSchedule sch1 = expected.PaymentSchedule;
		expected = expected.toBuilder().paymentSchedule(sch1.toBuilder().startDateBusinessDayAdjustment(sch1.BusinessDayAdjustment).rollConvention(RollConventions.DAY_20).build()).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.Product, expected);
		assertEquals(test.UpfrontFee, null);
	  }

	  public virtual void test_createTrade_withFee()
	  {
		DatesCdsTemplate @base = DatesCdsTemplate.of(START, END, CONV1);
		LocalDate tradeDate = LocalDate.of(2015, 5, 5);
		AdjustablePayment payment = AdjustablePayment.of(EUR, NOTIONAL_2M, CONV1.SettlementDateOffset.adjust(tradeDate, REF_DATA));
		CdsTrade test = @base.createTrade(LEGAL_ENTITY, tradeDate, BUY, NOTIONAL_2M, 0.05d, payment, REF_DATA);
		Cds expected = Cds.of(BUY, LEGAL_ENTITY, CONV1.Currency, NOTIONAL_2M, START, END, Frequency.P3M, CONV1.SettlementDateOffset.Calendar, 0.05d);
		PeriodicSchedule sch1 = expected.PaymentSchedule;
		expected = expected.toBuilder().paymentSchedule(sch1.toBuilder().startDateBusinessDayAdjustment(sch1.BusinessDayAdjustment).rollConvention(RollConventions.DAY_20).build()).build();
		assertEquals(test.Info.TradeDate, tradeDate);
		assertEquals(test.UpfrontFee, payment);
		assertEquals(test.Product, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DatesCdsTemplate test1 = DatesCdsTemplate.of(START, END, CONV1);
		coverImmutableBean(test1);
		DatesCdsTemplate test2 = DatesCdsTemplate.of(LocalDate.of(2015, 5, 20), LocalDate.of(2025, 6, 20), CdsConventions.USD_STANDARD);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		DatesCdsTemplate test = DatesCdsTemplate.of(START, END, CONV1);
		assertSerialization(test);
	  }

	}

}
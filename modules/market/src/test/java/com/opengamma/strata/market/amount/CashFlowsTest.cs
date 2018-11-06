using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.amount
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;

	/// <summary>
	/// Test <seealso cref="CashFlows"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CashFlowsTest
	public class CashFlowsTest
	{

	  private const double TOLERANCE = 1e-8;
	  private static readonly LocalDate PAYMENT_DATE_1 = LocalDate.of(2015, 6, 22);
	  private static readonly LocalDate PAYMENT_DATE_2 = LocalDate.of(2015, 12, 21);
	  private const double FORECAST_VALUE_1 = 0.0132;
	  private const double FORECAST_VALUE_2 = -0.0108;
	  private const double FORECAST_VALUE_3 = 0.0126;
	  private const double DISCOUNT_FACTOR_1 = 0.96d;
	  private const double DISCOUNT_FACTOR_2 = 0.9d;

	  private static readonly CashFlow CASH_FLOW_1 = CashFlow.ofForecastValue(PAYMENT_DATE_1, USD, FORECAST_VALUE_1, DISCOUNT_FACTOR_1);
	  private static readonly CashFlow CASH_FLOW_2 = CashFlow.ofForecastValue(PAYMENT_DATE_1, GBP, FORECAST_VALUE_2, DISCOUNT_FACTOR_1);
	  private static readonly CashFlow CASH_FLOW_3 = CashFlow.ofForecastValue(PAYMENT_DATE_2, USD, FORECAST_VALUE_3, DISCOUNT_FACTOR_2);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_singleFlow()
	  {
		CashFlows test = CashFlows.of(CASH_FLOW_1);
		assertEquals(test.getCashFlows().size(), 1);
		assertEquals(test.getCashFlows().get(0), CASH_FLOW_1);
		assertEquals(test.getCashFlow(0), CASH_FLOW_1);
	  }

	  public virtual void test_of_listFlows()
	  {
		IList<CashFlow> list = ImmutableList.builder<CashFlow>().add(CASH_FLOW_1, CASH_FLOW_2).build();
		CashFlows test = CashFlows.of(list);
		assertEquals(test.getCashFlows(), list);
		assertEquals(test.getCashFlow(0), CASH_FLOW_1);
		assertEquals(test.getCashFlow(1), CASH_FLOW_2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_singleFlow()
	  {
		CashFlows @base = CashFlows.of(CASH_FLOW_1);
		CashFlows test = @base.combinedWith(CASH_FLOW_2);
		CashFlows expected = CashFlows.of(ImmutableList.of(CASH_FLOW_1, CASH_FLOW_2));
		assertEquals(test, expected);
	  }

	  public virtual void test_combinedWith_listFlows()
	  {
		CashFlows @base = CashFlows.of(CASH_FLOW_1);
		CashFlows other = CashFlows.of(ImmutableList.of(CASH_FLOW_2, CASH_FLOW_3));
		CashFlows test = @base.combinedWith(other);
		CashFlows expected = CashFlows.of(ImmutableList.of(CASH_FLOW_1, CASH_FLOW_2, CASH_FLOW_3));
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_sorted_listFlows()
	  {
		CashFlows @base = CashFlows.of(ImmutableList.of(CASH_FLOW_1, CASH_FLOW_2, CASH_FLOW_3));
		CashFlows test = @base.sorted();
		CashFlows expected = CashFlows.of(ImmutableList.of(CASH_FLOW_2, CASH_FLOW_1, CASH_FLOW_3));
		assertEquals(test, expected);
		assertEquals(test.sorted(), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		CashFlows @base = CashFlows.of(ImmutableList.of(CASH_FLOW_1, CASH_FLOW_2));
		CashFlows test = @base.convertedTo(USD, FxRate.of(GBP, USD, 1.5));
		assertEquals(test.getCashFlow(0), CASH_FLOW_1);
		CashFlow converted = test.getCashFlow(1);
		assertEquals(converted.PaymentDate, CASH_FLOW_2.PaymentDate);
		assertEquals(converted.DiscountFactor, CASH_FLOW_2.DiscountFactor, TOLERANCE);
		assertEquals(converted.PresentValue.Currency, USD);
		assertEquals(converted.PresentValue.Amount, CASH_FLOW_2.PresentValue.Amount * 1.5, TOLERANCE);
		assertEquals(converted.ForecastValue.Currency, USD);
		assertEquals(converted.ForecastValue.Amount, CASH_FLOW_2.ForecastValue.Amount * 1.5, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CashFlows test1 = CashFlows.of(CASH_FLOW_1);
		coverImmutableBean(test1);
		CashFlows test2 = CashFlows.of(ImmutableList.of(CASH_FLOW_2, CASH_FLOW_3));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CashFlows test = CashFlows.of(ImmutableList.of(CASH_FLOW_1, CASH_FLOW_2, CASH_FLOW_3));
		assertSerialization(test);
	  }

	}

}
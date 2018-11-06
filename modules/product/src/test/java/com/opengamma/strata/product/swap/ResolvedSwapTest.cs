/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.IBOR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.OTHER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.OVERNIGHT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedSwapTest
	public class ResolvedSwapTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);
	  private static readonly LocalDate DATE_2014_09_30 = date(2014, 9, 30);
	  private static readonly LocalDate DATE_2014_10_01 = date(2014, 10, 1);
	  private static readonly IborRateComputation GBP_LIBOR_3M_2014_06_28 = IborRateComputation.of(GBP_LIBOR_3M, date(2014, 6, 28), REF_DATA);
	  private static readonly NotionalExchange NOTIONAL_EXCHANGE = NotionalExchange.of(CurrencyAmount.of(GBP, 2000d), DATE_2014_10_01);
	  private static readonly RateAccrualPeriod RAP = RateAccrualPeriod.builder().startDate(DATE_2014_06_30).endDate(DATE_2014_09_30).yearFraction(0.25d).rateComputation(GBP_LIBOR_3M_2014_06_28).build();
	  private static readonly RatePaymentPeriod RPP1 = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP).dayCount(ACT_365F).currency(GBP).notional(5000d).build();
	  private static readonly RatePaymentPeriod RPP2 = RatePaymentPeriod.builder().paymentDate(DATE_2014_10_01).accrualPeriods(RAP).dayCount(ACT_365F).currency(USD).notional(6000d).build();
	  internal static readonly ResolvedSwapLeg LEG1 = ResolvedSwapLeg.builder().type(FIXED).payReceive(PAY).paymentPeriods(RPP1).paymentEvents(NOTIONAL_EXCHANGE).build();
	  internal static readonly ResolvedSwapLeg LEG2 = ResolvedSwapLeg.builder().type(IBOR).payReceive(RECEIVE).paymentPeriods(RPP2).build();

	  public virtual void test_of()
	  {
		ResolvedSwap test = ResolvedSwap.of(LEG1, LEG2);
		assertEquals(test.Legs, ImmutableSet.of(LEG1, LEG2));
		assertEquals(test.getLegs(SwapLegType.FIXED), ImmutableList.of(LEG1));
		assertEquals(test.getLegs(SwapLegType.IBOR), ImmutableList.of(LEG2));
		assertEquals(test.getLeg(PayReceive.PAY), LEG1);
		assertEquals(test.getLeg(PayReceive.RECEIVE), LEG2);
		assertEquals(test.PayLeg, LEG1);
		assertEquals(test.ReceiveLeg, LEG2);
		assertEquals(test.StartDate, LEG1.StartDate);
		assertEquals(test.EndDate, LEG1.EndDate);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP, USD));
		assertEquals(test.allIndices(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  public virtual void test_of_singleCurrency()
	  {
		ResolvedSwap test = ResolvedSwap.of(LEG1);
		assertEquals(test.Legs, ImmutableSet.of(LEG1));
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allIndices(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  public virtual void test_builder()
	  {
		ResolvedSwap test = ResolvedSwap.builder().legs(LEG1).build();
		assertEquals(test.Legs, ImmutableSet.of(LEG1));
		assertEquals(test.CrossCurrency, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP));
		assertEquals(test.allIndices(), ImmutableSet.of(GBP_LIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getLegs_SwapLegType()
	  {
		assertEquals(ResolvedSwap.of(LEG1, LEG2).getLegs(FIXED), ImmutableList.of(LEG1));
		assertEquals(ResolvedSwap.of(LEG1, LEG2).getLegs(IBOR), ImmutableList.of(LEG2));
		assertEquals(ResolvedSwap.of(LEG1, LEG2).getLegs(OVERNIGHT), ImmutableList.of());
		assertEquals(ResolvedSwap.of(LEG1, LEG2).getLegs(OTHER), ImmutableList.of());
	  }

	  public virtual void test_getLeg_PayReceive()
	  {
		assertEquals(ResolvedSwap.of(LEG1, LEG2).getLeg(PAY), LEG1);
		assertEquals(ResolvedSwap.of(LEG1, LEG2).getLeg(RECEIVE), LEG2);
		assertEquals(ResolvedSwap.of(LEG1).getLeg(PAY), LEG1);
		assertEquals(ResolvedSwap.of(LEG2).getLeg(PAY), null);
		assertEquals(ResolvedSwap.of(LEG1).getLeg(RECEIVE), null);
		assertEquals(ResolvedSwap.of(LEG2).getLeg(RECEIVE), LEG2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedSwap test = ResolvedSwap.builder().legs(LEG1).build();
		coverImmutableBean(test);
		ResolvedSwap test2 = ResolvedSwap.builder().legs(LEG2).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedSwap test = ResolvedSwap.builder().legs(LEG1).build();
		assertSerialization(test);
	  }

	}

}
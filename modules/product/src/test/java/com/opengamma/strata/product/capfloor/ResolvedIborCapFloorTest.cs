/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.SwapLegType.FIXED;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;

	/// <summary>
	/// Test <seealso cref="ResolvedIborCapFloor"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborCapFloorTest
	public class ResolvedIborCapFloorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double STRIKE = 0.0125;
	  private const double NOTIONAL = 1.0e6;
	  private static readonly IborCapletFloorletPeriod PERIOD_1 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 3, 17)).endDate(LocalDate.of(2011, 6, 17)).unadjustedStartDate(LocalDate.of(2011, 3, 17)).unadjustedEndDate(LocalDate.of(2011, 6, 17)).paymentDate(LocalDate.of(2011, 6, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 6, 15), REF_DATA)).yearFraction(0.2556).build();
	  private static readonly IborCapletFloorletPeriod PERIOD_2 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 6, 17)).endDate(LocalDate.of(2011, 9, 19)).unadjustedStartDate(LocalDate.of(2011, 6, 17)).unadjustedEndDate(LocalDate.of(2011, 9, 17)).paymentDate(LocalDate.of(2011, 9, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 9, 15), REF_DATA)).yearFraction(0.2611).build();
	  private static readonly IborCapletFloorletPeriod PERIOD_3 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 9, 19)).endDate(LocalDate.of(2011, 12, 19)).unadjustedStartDate(LocalDate.of(2011, 9, 17)).unadjustedEndDate(LocalDate.of(2011, 12, 17)).paymentDate(LocalDate.of(2011, 12, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 12, 15), REF_DATA)).yearFraction(0.2528).build();
	  private static readonly IborCapletFloorletPeriod PERIOD_4 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 12, 19)).endDate(LocalDate.of(2012, 3, 19)).unadjustedStartDate(LocalDate.of(2011, 12, 17)).unadjustedEndDate(LocalDate.of(2012, 3, 17)).paymentDate(LocalDate.of(2012, 3, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2012, 3, 15), REF_DATA)).yearFraction(0.2528).build();
	  internal static readonly ResolvedIborCapFloorLeg CAPFLOOR_LEG = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1, PERIOD_2, PERIOD_3, PERIOD_4).payReceive(RECEIVE).build();

	  private const double RATE = 0.015;
	  private static readonly RatePaymentPeriod PAY_PERIOD_1 = RatePaymentPeriod.builder().paymentDate(LocalDate.of(2011, 9, 21)).accrualPeriods(RateAccrualPeriod.builder().startDate(LocalDate.of(2011, 3, 17)).endDate(LocalDate.of(2011, 9, 19)).yearFraction(0.517).rateComputation(FixedRateComputation.of(RATE)).build()).dayCount(ACT_365F).currency(EUR).notional(-NOTIONAL).build();
	  private static readonly RatePaymentPeriod PAY_PERIOD_2 = RatePaymentPeriod.builder().paymentDate(LocalDate.of(2012, 3, 21)).accrualPeriods(RateAccrualPeriod.builder().startDate(LocalDate.of(2011, 9, 19)).endDate(LocalDate.of(2012, 3, 19)).yearFraction(0.505).rateComputation(FixedRateComputation.of(RATE)).build()).dayCount(ACT_365F).currency(EUR).notional(-NOTIONAL).build();
	  internal static readonly ResolvedSwapLeg PAY_LEG = ResolvedSwapLeg.builder().paymentPeriods(PAY_PERIOD_1, PAY_PERIOD_2).type(FIXED).payReceive(PAY).build();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_oneLeg()
	  {
		ResolvedIborCapFloor test = ResolvedIborCapFloor.of(CAPFLOOR_LEG);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG);
		assertEquals(test.PayLeg.Present, false);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(EUR));
		assertEquals(test.allIndices(), ImmutableSet.of(EUR_EURIBOR_3M));
	  }

	  public virtual void test_of_twoLegs()
	  {
		ResolvedIborCapFloor test = ResolvedIborCapFloor.of(CAPFLOOR_LEG, PAY_LEG);
		assertEquals(test.CapFloorLeg, CAPFLOOR_LEG);
		assertEquals(test.PayLeg.get(), PAY_LEG);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(EUR));
		assertEquals(test.allIndices(), ImmutableSet.of(EUR_EURIBOR_3M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedIborCapFloor test1 = ResolvedIborCapFloor.of(CAPFLOOR_LEG, PAY_LEG);
		coverImmutableBean(test1);
		ResolvedIborCapFloorLeg capFloor = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1).payReceive(PAY).build();
		ResolvedIborCapFloor test2 = ResolvedIborCapFloor.of(capFloor);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedIborCapFloor test = ResolvedIborCapFloor.of(CAPFLOOR_LEG);
		assertSerialization(test);
	  }

	}

}
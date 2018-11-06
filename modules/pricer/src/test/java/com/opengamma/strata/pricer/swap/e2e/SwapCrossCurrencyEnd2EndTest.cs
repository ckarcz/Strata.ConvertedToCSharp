using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swap.e2e
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.schedule.Frequency.P3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.AssertionsForInterfaceTypes.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndices = com.opengamma.strata.basics.index.FxIndices;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using StandardDataSets = com.opengamma.strata.pricer.datasets.StandardDataSets;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using FxResetCalculation = com.opengamma.strata.product.swap.FxResetCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;

	/// <summary>
	/// Test end to end for cross currency swaps.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwapCrossCurrencyEnd2EndTest
	public class SwapCrossCurrencyEnd2EndTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard().combinedWith(ImmutableReferenceData.of(CalendarUSD.NYC, CalendarUSD.NYC_CALENDAR));
	  private static readonly IborIndex EUR_EURIBOR_3M = IborIndices.EUR_EURIBOR_3M;
	  private static readonly IborIndex USD_LIBOR_3M = IborIndices.USD_LIBOR_3M;
	  private static readonly FxIndex EUR_USD_WM = FxIndices.EUR_USD_WM;
	  private const double NOTIONAL_USD = 120_000_000;
	  private const double NOTIONAL_EUR = 100_000_000;
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, CalendarUSD.NYC);
	  private static readonly BusinessDayAdjustment BDA_P = BusinessDayAdjustment.of(PRECEDING, CalendarUSD.NYC);

	  // tolerance
	  private const double TOLERANCE_PV = 1.0E-4;

	  //-----------------------------------------------------------------------
	  // XCcy swap with exchange of notional
	  public virtual void test_XCcyEur3MSpreadVsUSD3M()
	  {
		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2016, 1, 24)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().finalExchange(true).initialExchange(true).amount(ValueSchedule.of(NOTIONAL_EUR)).currency(EUR).build()).calculation(IborRateCalculation.builder().index(EUR_EURIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).spread(ValueSchedule.of(0.0020)).build()).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2016, 1, 24)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().finalExchange(true).initialExchange(true).amount(ValueSchedule.of(NOTIONAL_USD)).currency(USD).build()).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

		ResolvedSwapTrade trade = SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 9, 10)).build()).product(Swap.of(payLeg, receiveLeg)).build().resolve(REF_DATA);

		double pvUsdExpected = 431944.6868;
		double pvEurExpected = -731021.1778;

		DiscountingSwapTradePricer pricer = swapPricer();
		MultiCurrencyAmount pv = pricer.presentValue(trade, provider());
		assertEquals(pv.getAmount(USD).Amount, pvUsdExpected, TOLERANCE_PV);
		assertEquals(pv.getAmount(EUR).Amount, pvEurExpected, TOLERANCE_PV);
	  }

	  // XCcy swap with exchange of notional and FX Reset on the USD leg
	  public virtual void test_XCcyEur3MSpreadVsUSD3MFxReset()
	  {
		//Test all possible combinations of exchange flags
		bool[] allBoolean = new bool[] {true, false};
		foreach (bool initialExchange in allBoolean)
		{
		  foreach (bool intermediateExchange in allBoolean)
		  {
			foreach (bool finalExchange in allBoolean)
			{
			  //Skip the case where all exchanges are false, this is tested separately
			  if (initialExchange || intermediateExchange || finalExchange)
			  {
				test_XCcyEurUSDFxReset(initialExchange, intermediateExchange, finalExchange);
			  }
			}
		  }
		}
	  }

	  // XCcy swap with exchange of notional and FX Reset on the USD leg
	  public virtual void test_XCcyFixedInitialNotional()
	  {

		DiscountingSwapTradePricer pricer = swapPricer();

		//Create an MTM swap with initial notional override
		double notional = 1_000_000d;
		ResolvedSwapTrade fixedNotionalMtmTrade = getMtmTrade(true, true, true, notional).resolve(REF_DATA);
		ExplainMap explainMap = pricer.explainPresentValue(fixedNotionalMtmTrade, provider());

		CurrencyAmount fixedNotional = CurrencyAmount.of(Currency.USD, notional);
		ExplainMap fixedNotionalMtmLeg = explainMap.get(ExplainKey.LEGS).get().get(1);

		IList<ExplainMap> events = fixedNotionalMtmLeg.get(ExplainKey.PAYMENT_EVENTS).get();

		//First two payment events should use fixed initial notional
		ExplainMap firstPaymentEvent = events[0];
		assertFixedNotionalPaymentEvent(firstPaymentEvent, fixedNotional.negated());
		ExplainMap secondPaymentEvent = events[1];
		assertFixedNotionalPaymentEvent(secondPaymentEvent, fixedNotional);

		//First coupon also uses fixed notional
		ExplainMap firstCoupon = fixedNotionalMtmLeg.get(ExplainKey.PAYMENT_PERIODS).get().get(0);
		assertEquals(firstCoupon.get(ExplainKey.TRADE_NOTIONAL), fixedNotional);
		assertEquals(firstCoupon.get(ExplainKey.NOTIONAL), fixedNotional);

		//Sum of all pv amounts which are impacted by overriding the first period with a  fixed notional
		CurrencyAmount firstPaymentPv = firstPaymentEvent.get(ExplainKey.PRESENT_VALUE).get();
		CurrencyAmount secondPaymentPv = secondPaymentEvent.get(ExplainKey.PRESENT_VALUE).get();
		CurrencyAmount firstCouponPv = firstCoupon.get(ExplainKey.PRESENT_VALUE).get();
		CurrencyAmount fixedNotionalImpactedEventsPv = firstPaymentPv.plus(secondPaymentPv).plus(firstCouponPv);

		//----------------------------------------------------------------------------------------------------------

		//Build identical trade but with no fixed notional
		ResolvedSwapTrade noFixedNotionalMtmTrade = getMtmTrade(true, true, true, null).resolve(REF_DATA);
		ExplainMap noFixedNotionalMtmLeg = pricer.explainPresentValue(noFixedNotionalMtmTrade, provider()).get(ExplainKey.LEGS).get().get(1);

		//Sum the pvs for the same combination of payments and events that are impacted by fixed notional in first trade
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		CurrencyAmount noFixedNotionalEventsPv = noFixedNotionalMtmLeg.get(ExplainKey.PAYMENT_EVENTS).get().subList(0, 2).Select(payment => payment.get(ExplainKey.PRESENT_VALUE).get()).Aggregate(CurrencyAmount.zero(Currency.USD), CurrencyAmount::plus);

		CurrencyAmount noFixedNotionalCouponsPv = noFixedNotionalMtmLeg.get(ExplainKey.PAYMENT_PERIODS).get().get(0).get(ExplainKey.PRESENT_VALUE).get();

		CurrencyAmount noFixedNotionalImpactedEventsPv = noFixedNotionalCouponsPv.plus(noFixedNotionalEventsPv);

		//----------------------------------------------------------------------------------------------------------

		//PV difference of the events impacted by fixing notional
		CurrencyAmount paymentsPvDifference = fixedNotionalImpactedEventsPv.minus(noFixedNotionalImpactedEventsPv);

		//Calculate PV of the full trades
		MultiCurrencyAmount fixedNotionalLegPv = pricer.presentValue(fixedNotionalMtmTrade, provider());
		MultiCurrencyAmount noFixedNotionalLegPv = pricer.presentValue(noFixedNotionalMtmTrade, provider());

		//EUR PV should not have changed
		assertEquals(fixedNotionalLegPv.getAmount(Currency.EUR).Amount, noFixedNotionalLegPv.getAmount(Currency.EUR).Amount, TOLERANCE_PV);

		//Difference in USD PV should be equal the difference in PV of the three events impacted by the initial notional
		//All else should remain equal
		CurrencyAmount tradePvDifference = fixedNotionalLegPv.getAmount(Currency.USD).minus(noFixedNotionalLegPv.getAmount(Currency.USD));
		assertEquals(tradePvDifference.Amount, paymentsPvDifference.Amount, TOLERANCE_PV);
	  }

	  private void assertFixedNotionalPaymentEvent(ExplainMap paymentEvent, CurrencyAmount expectedNotional)
	  {

		assertEquals(paymentEvent.get(ExplainKey.TRADE_NOTIONAL), expectedNotional);
		assertEquals(paymentEvent.get(ExplainKey.FORECAST_VALUE), expectedNotional);
		double firstDiscountFactor = paymentEvent.get(ExplainKey.DISCOUNT_FACTOR).Value;

		//Fixed notional, so PV is notional * DCF
		CurrencyAmount expectedPv = expectedNotional.multipliedBy(firstDiscountFactor);
		assertEquals(paymentEvent.get(ExplainKey.PRESENT_VALUE), expectedPv);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "FxResetCalculation index EUR/USD-WM was specified but schedule does not include any notional exchanges") public void test_FxResetWithNoExchanges()
	  public virtual void test_FxResetWithNoExchanges()
	  {
		//specifying an FX reset with no exchanges throws an exception
		test_XCcyEurUSDFxReset(false, false, false);
	  }

	  private void test_XCcyEurUSDFxReset(bool initialExchange, bool intermediateExchange, bool finalExchange)
	  {

		ResolvedSwapTrade trade = getMtmTrade(initialExchange, intermediateExchange, finalExchange, null).resolve(REF_DATA);

		DiscountingSwapTradePricer pricer = swapPricer();
		MultiCurrencyAmount pv = pricer.presentValue(trade, provider());

		//Coupons are always included, so base is the total coupon pvs
		double pvUsdExpected = 1447799.5318;
		double pvEurExpected = -1020648.6461;
		int usdExpectedPaymentEvents = 0;
		int eurExpectedPaymentEvents = 0;

		//Add PV amounts of included exchanges to arrive at total expected pv
		if (initialExchange)
		{
		  pvUsdExpected += -143998710.0091;
		  pvEurExpected += 99999104.1730;
		  ++usdExpectedPaymentEvents;
		  ++eurExpectedPaymentEvents;
		}

		if (intermediateExchange)
		{
		  pvUsdExpected += -344525.1458;
		  usdExpectedPaymentEvents += 14;
		}

		if (finalExchange)
		{
		  pvUsdExpected += 143414059.1395;
		  pvEurExpected += -99709476.7047;
		  ++usdExpectedPaymentEvents;
		  ++eurExpectedPaymentEvents;
		}

		assertEquals(pv.getAmount(USD).Amount, pvUsdExpected, TOLERANCE_PV);
		assertEquals(pv.getAmount(EUR).Amount, pvEurExpected, TOLERANCE_PV);

		//Assert the payment event (exchange) count on each leg
		IList<ExplainMap> legs = pricer.explainPresentValue(trade, provider()).get(ExplainKey.LEGS).get();
		assertThat(legs[0].get(ExplainKey.PAYMENT_EVENTS).orElse(ImmutableList.of())).hasSize(eurExpectedPaymentEvents);
		assertThat(legs[1].get(ExplainKey.PAYMENT_EVENTS).orElse(ImmutableList.of())).hasSize(usdExpectedPaymentEvents);
	  }

	  private SwapTrade getMtmTrade(bool initialExchange, bool intermediateExchange, bool finalExchange, double? initialNotional)
	  {

		SwapLeg payLeg = RateCalculationSwapLeg.builder().payReceive(PAY).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2016, 1, 24)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().finalExchange(finalExchange).initialExchange(initialExchange).amount(ValueSchedule.of(NOTIONAL_EUR)).currency(EUR).build()).calculation(IborRateCalculation.builder().index(EUR_EURIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).spread(ValueSchedule.of(0.0020)).build()).build();

		SwapLeg receiveLeg = RateCalculationSwapLeg.builder().payReceive(RECEIVE).accrualSchedule(PeriodicSchedule.builder().startDate(LocalDate.of(2014, 1, 24)).endDate(LocalDate.of(2016, 1, 24)).frequency(P3M).businessDayAdjustment(BDA_MF).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(P3M).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.builder().finalExchange(finalExchange).initialExchange(initialExchange).intermediateExchange(intermediateExchange).amount(ValueSchedule.of(NOTIONAL_USD)).currency(USD).fxReset(FxResetCalculation.builder().fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).referenceCurrency(EUR).index(EUR_USD_WM).initialNotionalValue(initialNotional).build()).build()).calculation(IborRateCalculation.builder().index(USD_LIBOR_3M).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, CalendarUSD.NYC, BDA_P)).build()).build();

		return SwapTrade.builder().info(TradeInfo.builder().tradeDate(LocalDate.of(2014, 9, 10)).build()).product(Swap.of(payLeg, receiveLeg)).build();
	  }

	  //-------------------------------------------------------------------------
	  // pricer
	  private DiscountingSwapTradePricer swapPricer()
	  {
		return DiscountingSwapTradePricer.DEFAULT;
	  }

	  // rates provider
	  private static RatesProvider provider()
	  {
		return StandardDataSets.providerUsdEurDscL3();
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.deposit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using IborFixingDeposit = com.opengamma.strata.product.deposit.IborFixingDeposit;
	using IborFixingDepositTrade = com.opengamma.strata.product.deposit.IborFixingDepositTrade;
	using ResolvedIborFixingDeposit = com.opengamma.strata.product.deposit.ResolvedIborFixingDeposit;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;

	/// <summary>
	/// Tests <seealso cref="DiscountingIborFixingDepositTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingIborFixingDepositTradePricerTest
	public class DiscountingIborFixingDepositTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 16);

	  private static readonly LocalDate START_DATE = LocalDate.of(2014, 1, 24);
	  private static readonly LocalDate END_DATE = LocalDate.of(2014, 7, 24);
	  private const double NOTIONAL = 100_000_000d;
	  private const double RATE = 0.0150;
	  private static readonly BusinessDayAdjustment BD_ADJ = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly IborFixingDeposit DEPOSIT_PRODUCT = IborFixingDeposit.builder().buySell(BuySell.BUY).notional(NOTIONAL).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BD_ADJ).index(EUR_EURIBOR_6M).fixedRate(RATE).build();
	  private static readonly ResolvedIborFixingDeposit RDEPOSIT_PRODUCT = DEPOSIT_PRODUCT.resolve(REF_DATA);
	  private static readonly IborFixingDepositTrade DEPOSIT_TRADE = IborFixingDepositTrade.builder().product(DEPOSIT_PRODUCT).info(TradeInfo.empty()).build();
	  private static readonly ResolvedIborFixingDepositTrade RDEPOSIT_TRADE = DEPOSIT_TRADE.resolve(REF_DATA);

	  private static readonly ImmutableRatesProvider IMM_PROV;
	  static DiscountingIborFixingDepositTradePricerTest()
	  {
		CurveInterpolator interp = CurveInterpolators.DOUBLE_QUADRATIC;
		DoubleArray time_eur = DoubleArray.of(0.0, 0.1, 0.25, 0.5, 0.75, 1.0, 2.0);
		DoubleArray rate_eur = DoubleArray.of(0.0160, 0.0165, 0.0155, 0.0155, 0.0155, 0.0150, 0.014);
		InterpolatedNodalCurve dscCurve = InterpolatedNodalCurve.of(Curves.zeroRates("EUR-Discount", ACT_ACT_ISDA), time_eur, rate_eur, interp);
		DoubleArray time_index = DoubleArray.of(0.0, 0.25, 0.5, 1.0);
		DoubleArray rate_index = DoubleArray.of(0.0180, 0.0180, 0.0175, 0.0165);
		InterpolatedNodalCurve indexCurve = InterpolatedNodalCurve.of(Curves.zeroRates("EUR-EURIBOR6M", ACT_ACT_ISDA), time_index, rate_index, interp);
		IMM_PROV = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(EUR, dscCurve).iborIndexCurve(EUR_EURIBOR_6M, indexCurve).build();
	  }

	  private static readonly DiscountingIborFixingDepositProductPricer PRICER_PRODUCT = DiscountingIborFixingDepositProductPricer.DEFAULT;
	  private static readonly DiscountingIborFixingDepositTradePricer PRICER_TRADE = DiscountingIborFixingDepositTradePricer.DEFAULT;


	  private const double TOLERANCE_PV = 1E-2;
	  private const double TOLERANCE_PV_DELTA = 1E-2;
	  private const double TOLERANCE_RATE = 1E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue()
	  {
		CurrencyAmount pvTrade = PRICER_TRADE.presentValue(RDEPOSIT_TRADE, IMM_PROV);
		CurrencyAmount pvProduct = PRICER_PRODUCT.presentValue(RDEPOSIT_PRODUCT, IMM_PROV);
		assertEquals(pvTrade.Currency, pvProduct.Currency);
		assertEquals(pvTrade.Amount, pvProduct.Amount, TOLERANCE_PV);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities ptsTrade = PRICER_TRADE.presentValueSensitivity(RDEPOSIT_TRADE, IMM_PROV);
		PointSensitivities ptsProduct = PRICER_PRODUCT.presentValueSensitivity(RDEPOSIT_PRODUCT, IMM_PROV);
		assertTrue(ptsTrade.equalWithTolerance(ptsProduct, TOLERANCE_PV_DELTA));
	  }


	  //-------------------------------------------------------------------------
	  public virtual void test_parRate()
	  {
		double psTrade = PRICER_TRADE.parRate(RDEPOSIT_TRADE, IMM_PROV);
		double psProduct = PRICER_PRODUCT.parRate(RDEPOSIT_PRODUCT, IMM_PROV);
		assertEquals(psTrade, psProduct, TOLERANCE_RATE);

	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parRateSensitivity()
	  {
		PointSensitivities ptsTrade = PRICER_TRADE.parRateSensitivity(RDEPOSIT_TRADE, IMM_PROV);
		PointSensitivities ptsProduct = PRICER_PRODUCT.parRateSensitivity(RDEPOSIT_PRODUCT, IMM_PROV);
		assertTrue(ptsTrade.equalWithTolerance(ptsProduct, TOLERANCE_PV_DELTA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpread()
	  {
		double psTrade = PRICER_TRADE.parSpread(RDEPOSIT_TRADE, IMM_PROV);
		double psProduct = PRICER_PRODUCT.parSpread(RDEPOSIT_PRODUCT, IMM_PROV);
		assertEquals(psTrade, psProduct, TOLERANCE_RATE);

	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parSpreadSensitivity()
	  {
		PointSensitivities ptsTrade = PRICER_TRADE.parSpreadSensitivity(RDEPOSIT_TRADE, IMM_PROV);
		PointSensitivities ptsProduct = PRICER_PRODUCT.parSpreadSensitivity(RDEPOSIT_PRODUCT, IMM_PROV);
		assertTrue(ptsTrade.equalWithTolerance(ptsProduct, TOLERANCE_PV_DELTA));
	  }

	}

}
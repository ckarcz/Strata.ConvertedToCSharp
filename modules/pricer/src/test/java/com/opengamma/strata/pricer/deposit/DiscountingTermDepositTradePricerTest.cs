/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedTermDeposit = com.opengamma.strata.product.deposit.ResolvedTermDeposit;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;
	using TermDepositTrade = com.opengamma.strata.product.deposit.TermDepositTrade;

	/// <summary>
	/// Tests <seealso cref="DiscountingTermDepositTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingTermDepositTradePricerTest
	public class DiscountingTermDepositTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2014, 1, 22);

	  private static readonly LocalDate START_DATE = date(2014, 1, 24);
	  private static readonly LocalDate END_DATE = date(2014, 7, 24);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0750;
	  private static readonly double INTEREST = NOTIONAL * RATE * ACT_360.yearFraction(START_DATE, END_DATE);
	  private static readonly BusinessDayAdjustment BD_ADJ = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly TermDeposit DEPOSIT_PRODUCT = TermDeposit.builder().buySell(BuySell.BUY).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BD_ADJ).dayCount(ACT_360).notional(NOTIONAL).currency(EUR).rate(RATE).build();
	  private static readonly ResolvedTermDeposit RDEPOSIT_PRODUCT = DEPOSIT_PRODUCT.resolve(REF_DATA);
	  private static readonly TermDepositTrade DEPOSIT_TRADE = TermDepositTrade.builder().product(DEPOSIT_PRODUCT).info(TradeInfo.empty()).build();
	  private static readonly ResolvedTermDepositTrade RDEPOSIT_TRADE = DEPOSIT_TRADE.resolve(REF_DATA);

	  private static readonly Curve CURVE;
	  private static readonly ImmutableRatesProvider IMM_PROV;
	  static DiscountingTermDepositTradePricerTest()
	  {
		CurveInterpolator interp = CurveInterpolators.DOUBLE_QUADRATIC;
		DoubleArray time_eur = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
		DoubleArray rate_eur = DoubleArray.of(0.0160, 0.0135, 0.0160, 0.0185, 0.0185, 0.0195, 0.0200, 0.0210);
		CURVE = InterpolatedNodalCurve.of(Curves.zeroRates("EUR-Discount", ACT_360), time_eur, rate_eur, interp);
		IMM_PROV = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(EUR, CURVE).build();
	  }
	  internal double DF_END = 0.94;

	  private static readonly DiscountingTermDepositProductPricer PRICER_PRODUCT = DiscountingTermDepositProductPricer.DEFAULT;
	  private static readonly DiscountingTermDepositTradePricer PRICER_TRADE = DiscountingTermDepositTradePricer.DEFAULT;


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

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		assertEquals(PRICER_TRADE.currencyExposure(RDEPOSIT_TRADE, IMM_PROV), MultiCurrencyAmount.of(PRICER_TRADE.presentValue(RDEPOSIT_TRADE, IMM_PROV)));
	  }

	  public virtual void test_currentCash_onStartDate()
	  {
		RatesProvider prov = ImmutableRatesProvider.builder(RDEPOSIT_TRADE.Product.StartDate).discountCurve(EUR, CURVE).build();
		assertEquals(PRICER_TRADE.currentCash(RDEPOSIT_TRADE, prov), CurrencyAmount.of(EUR, -NOTIONAL));
	  }

	  public virtual void test_currentCash_onEndDate()
	  {
		RatesProvider prov = ImmutableRatesProvider.builder(RDEPOSIT_TRADE.Product.EndDate).discountCurve(EUR, CURVE).build();
		assertEquals(PRICER_TRADE.currentCash(RDEPOSIT_TRADE, prov), CurrencyAmount.of(EUR, NOTIONAL + INTEREST));
	  }

	  public virtual void test_currentCash_otherDate()
	  {
		assertEquals(PRICER_TRADE.currentCash(RDEPOSIT_TRADE, IMM_PROV), CurrencyAmount.zero(EUR));
	  }

	}

}
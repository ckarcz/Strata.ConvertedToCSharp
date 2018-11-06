/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using ImmutableFxIndex = com.opengamma.strata.basics.index.ImmutableFxIndex;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using ResolvedFxNdf = com.opengamma.strata.product.fx.ResolvedFxNdf;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;

	/// <summary>
	/// Test <seealso cref="DiscountingFxNdfProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFxNdfProductPricerTest
	public class DiscountingFxNdfProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly FxMatrix FX_MATRIX = RatesProviderFxDataSets.fxMatrix();
	  private static readonly RatesProvider PROVIDER = RatesProviderFxDataSets.createProvider();
	  private static readonly Currency KRW = Currency.KRW;
	  private static readonly Currency USD = Currency.USD;
	  private static readonly LocalDate PAYMENT_DATE = RatesProviderFxDataSets.VAL_DATE_2014_01_22.plusWeeks(8);
	  private static readonly LocalDate PAYMENT_DATE_PAST = RatesProviderFxDataSets.VAL_DATE_2014_01_22.minusDays(1);
	  private const double NOMINAL_USD = 100_000_000;
	  private static readonly CurrencyAmount CURRENCY_NOTIONAL = CurrencyAmount.of(USD, NOMINAL_USD);
	  private const double FX_RATE = 1123.45;
	  private static readonly CurrencyAmount CURRENCY_NOTIONAL_INVERSE = CurrencyAmount.of(KRW, NOMINAL_USD * FX_RATE);
	  private static readonly FxIndex INDEX = ImmutableFxIndex.builder().name("USD/KRW").currencyPair(CurrencyPair.of(USD, KRW)).fixingCalendar(USNY).maturityDateOffset(DaysAdjustment.ofBusinessDays(2, USNY)).build();
	  private static readonly LocalDate FIXING_DATE = INDEX.calculateFixingFromMaturity(PAYMENT_DATE, REF_DATA);
	  private static readonly LocalDate FIXING_DATE_PAST = INDEX.calculateFixingFromMaturity(PAYMENT_DATE_PAST, REF_DATA);

	  private static readonly ResolvedFxNdf NDF = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL).agreedFxRate(FxRate.of(USD, KRW, FX_RATE)).observation(FxIndexObservation.of(INDEX, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).build();
	  private static readonly ResolvedFxNdf NDF_INVERSE = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL_INVERSE).agreedFxRate(FxRate.of(USD, KRW, FX_RATE)).observation(FxIndexObservation.of(INDEX, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).build();

	  private static readonly DiscountingFxNdfProductPricer PRICER = DiscountingFxNdfProductPricer.DEFAULT;
	  private const double TOL = 1.0E-12;
	  private const double EPS_FD = 1E-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);

	  public virtual void test_presentValue()
	  {
		CurrencyAmount computed = PRICER.presentValue(NDF, PROVIDER);
		double dscUsd = PROVIDER.discountFactor(USD, NDF.PaymentDate);
		double dscKrw = PROVIDER.discountFactor(KRW, NDF.PaymentDate);
		double expected = NOMINAL_USD * (dscUsd - dscKrw * FX_RATE / PROVIDER.fxRate(CurrencyPair.of(USD, KRW)));
		assertEquals(computed.Currency, USD);
		assertEquals(computed.Amount, expected, NOMINAL_USD * TOL);
	  }

	  public virtual void test_presentValue_inverse()
	  {
		CurrencyAmount computed = PRICER.presentValue(NDF_INVERSE, PROVIDER);
		double dscUsd = PROVIDER.discountFactor(USD, NDF_INVERSE.PaymentDate);
		double dscKrw = PROVIDER.discountFactor(KRW, NDF_INVERSE.PaymentDate);
		double expected = NOMINAL_USD * FX_RATE * (dscKrw - dscUsd * 1 / FX_RATE / PROVIDER.fxRate(CurrencyPair.of(KRW, USD)));
		assertEquals(computed.Currency, KRW);
		assertEquals(computed.Amount, expected, NOMINAL_USD * FX_RATE * TOL);
	  }

	  public virtual void test_presentValue_ended()
	  {
		ResolvedFxNdf ndf = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL).agreedFxRate(FxRate.of(USD, KRW, FX_RATE)).observation(FxIndexObservation.of(INDEX, FIXING_DATE_PAST, REF_DATA)).paymentDate(PAYMENT_DATE_PAST).build();
		CurrencyAmount computed = PRICER.presentValue(ndf, PROVIDER);
		assertEquals(computed.Amount, 0d);
	  }

	  public virtual void test_forwardValue()
	  {
		FxRate computed = PRICER.forwardFxRate(NDF, PROVIDER);
		ResolvedFxNdf ndfFwd = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL).agreedFxRate(computed).observation(FxIndexObservation.of(INDEX, FIXING_DATE, REF_DATA)).paymentDate(PAYMENT_DATE).build();
		CurrencyAmount computedFwd = PRICER.presentValue(ndfFwd, PROVIDER);
		assertEquals(computedFwd.Amount, 0d, NOMINAL_USD * TOL);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities point = PRICER.presentValueSensitivity(NDF, PROVIDER);
		CurrencyParameterSensitivities computed = PROVIDER.parameterSensitivity(point);
		CurrencyParameterSensitivities expected = CAL_FD.sensitivity(PROVIDER, (p) => PRICER.presentValue(NDF, (p)));
		assertTrue(computed.equalWithTolerance(expected, NOMINAL_USD * EPS_FD));
	  }

	  public virtual void test_presentValueSensitivity_ended()
	  {
		ResolvedFxNdf ndf = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL).agreedFxRate(FxRate.of(USD, KRW, FX_RATE)).observation(FxIndexObservation.of(INDEX, FIXING_DATE_PAST, REF_DATA)).paymentDate(PAYMENT_DATE_PAST).build();
		PointSensitivities computed = PRICER.presentValueSensitivity(ndf, PROVIDER);
		assertEquals(computed, PointSensitivities.empty());
	  }

	  //-------------------------------------------------------------------------

	  public virtual void test_currencyExposure()
	  {
		CurrencyAmount pv = PRICER.presentValue(NDF, PROVIDER);
		MultiCurrencyAmount ce = PRICER.currencyExposure(NDF, PROVIDER);
		CurrencyAmount ceConverted = ce.convertedTo(pv.Currency, PROVIDER);
		assertEquals(pv.Amount, ceConverted.Amount, NOMINAL_USD * TOL);
	  }

	  public virtual void test_currencyExposure_ended()
	  {
		ResolvedFxNdf ndf = ResolvedFxNdf.builder().settlementCurrencyNotional(CURRENCY_NOTIONAL).agreedFxRate(FxRate.of(USD, KRW, FX_RATE)).observation(FxIndexObservation.of(INDEX, LocalDate.of(2011, 5, 2), REF_DATA)).paymentDate(LocalDate.of(2011, 5, 4)).build();
		MultiCurrencyAmount computed = PRICER.currencyExposure(ndf, PROVIDER);
		assertEquals(computed.size(), 0);
	  }

	  public virtual void test_currencyExposure_from_pt_sensitivity()
	  {
		MultiCurrencyAmount ceDirect = PRICER.currencyExposure(NDF, PROVIDER);
		PointSensitivities pts = PRICER.presentValueSensitivity(NDF, PROVIDER);
		MultiCurrencyAmount cePts = PROVIDER.currencyExposure(pts);
		CurrencyAmount cePv = PRICER.presentValue(NDF, PROVIDER);
		MultiCurrencyAmount ceExpected = cePts.plus(cePv);
		assertEquals(ceDirect.getAmount(USD).Amount, ceExpected.getAmount(USD).Amount, NOMINAL_USD * TOL);
		assertEquals(ceDirect.getAmount(KRW).Amount, ceExpected.getAmount(KRW).Amount, NOMINAL_USD * TOL * FX_MATRIX.fxRate(USD, KRW));
	  }

	  public virtual void test_currencyExposure_from_pt_sensitivity_inverse()
	  {
		MultiCurrencyAmount ceDirect = PRICER.currencyExposure(NDF_INVERSE, PROVIDER);
		PointSensitivities pts = PRICER.presentValueSensitivity(NDF_INVERSE, PROVIDER);
		MultiCurrencyAmount cePts = PROVIDER.currencyExposure(pts);
		CurrencyAmount cePv = PRICER.presentValue(NDF_INVERSE, PROVIDER);
		MultiCurrencyAmount ceExpected = cePts.plus(cePv);
		assertEquals(ceDirect.getAmount(USD).Amount, ceExpected.getAmount(USD).Amount, NOMINAL_USD * TOL);
		assertEquals(ceDirect.getAmount(KRW).Amount, ceExpected.getAmount(KRW).Amount, NOMINAL_USD * TOL * FX_MATRIX.fxRate(USD, KRW));
	  }

	  //-------------------------------------------------------------------------
	  private static readonly ResolvedFxSingle FOREX = ResolvedFxSingle.of(CurrencyAmount.of(USD, NOMINAL_USD), FxRate.of(USD, KRW, FX_RATE), PAYMENT_DATE);
	  private static readonly DiscountingFxSingleProductPricer PRICER_FX = DiscountingFxSingleProductPricer.DEFAULT;

	  // Checks that the NDF present value is coherent with the standard FX forward present value.
	  public virtual void test_presentValueVsForex()
	  {
		CurrencyAmount pvNDF = PRICER.presentValue(NDF, PROVIDER);
		MultiCurrencyAmount pvFX = PRICER_FX.presentValue(FOREX, PROVIDER);
		assertEquals(pvNDF.Amount, pvFX.getAmount(USD).Amount + pvFX.getAmount(KRW).Amount * FX_MATRIX.fxRate(KRW, USD), NOMINAL_USD * TOL);
	  }

	  // Checks that the NDF currency exposure is coherent with the standard FX forward present value.
	  public virtual void test_currencyExposureVsForex()
	  {
		MultiCurrencyAmount pvNDF = PRICER.currencyExposure(NDF, PROVIDER);
		MultiCurrencyAmount pvFX = PRICER_FX.currencyExposure(FOREX, PROVIDER);
		assertEquals(pvNDF.getAmount(USD).Amount, pvFX.getAmount(USD).Amount, NOMINAL_USD * TOL);
		assertEquals(pvNDF.getAmount(KRW).Amount, pvFX.getAmount(KRW).Amount, NOMINAL_USD * TOL * FX_MATRIX.fxRate(USD, KRW));
	  }

	  // Checks that the NDF forward rate is coherent with the standard FX forward present value.
	  public virtual void test_forwardRateVsForex()
	  {
		FxRate fwdNDF = PRICER.forwardFxRate(NDF, PROVIDER);
		FxRate fwdFX = PRICER_FX.forwardFxRate(FOREX, PROVIDER);
		assertEquals(fwdNDF.fxRate(fwdNDF.Pair), fwdFX.fxRate(fwdFX.Pair), 1e-10);
	  }

	  // Checks that the NDF present value sensitivity is coherent with the standard FX forward present value.
	  public virtual void test_presentValueCurveSensitivityVsForex()
	  {
		PointSensitivities pvcsNDF = PRICER.presentValueSensitivity(NDF, PROVIDER).normalized();
		CurrencyParameterSensitivities sensiNDF = PROVIDER.parameterSensitivity(pvcsNDF);
		PointSensitivities pvcsFX = PRICER_FX.presentValueSensitivity(FOREX, PROVIDER).normalized();
		CurrencyParameterSensitivities sensiFX = PROVIDER.parameterSensitivity(pvcsFX);
		assertTrue(sensiNDF.equalWithTolerance(sensiFX.convertedTo(USD, PROVIDER), NOMINAL_USD * TOL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currentCash_zero()
	  {
		CurrencyAmount computed = PRICER.currentCash(NDF, PROVIDER);
		assertEquals(computed, CurrencyAmount.zero(NDF.SettlementCurrency));
	  }

	  public virtual void test_currentCash_onPayment()
	  {
		double rate = 1111.2;
		LocalDate paymentDate = NDF.PaymentDate;
		RatesProvider provider = RatesProviderFxDataSets.createProvider(paymentDate, NDF.Index, rate);
		CurrencyAmount computed = PRICER.currentCash(NDF, provider);
		assertEquals(computed, CurrencyAmount.of(NDF.SettlementCurrency, NOMINAL_USD * (1d - FX_RATE / rate)));
	  }
	}

}
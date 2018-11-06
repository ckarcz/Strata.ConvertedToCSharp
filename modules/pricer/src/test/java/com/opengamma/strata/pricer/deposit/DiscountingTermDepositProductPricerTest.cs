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
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
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
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using RatesFiniteDifferenceSensitivityCalculator = com.opengamma.strata.pricer.sensitivity.RatesFiniteDifferenceSensitivityCalculator;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using ResolvedTermDeposit = com.opengamma.strata.product.deposit.ResolvedTermDeposit;
	using TermDeposit = com.opengamma.strata.product.deposit.TermDeposit;

	/// <summary>
	/// Test <seealso cref="DiscountingTermDepositProductPricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingTermDepositProductPricerTest
	public class DiscountingTermDepositProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = date(2014, 1, 22);
	  private static readonly LocalDate START_DATE = date(2014, 1, 24);
	  private static readonly LocalDate END_DATE = date(2014, 7, 24);
	  private const double NOTIONAL = 100000000d;
	  private const double RATE = 0.0750;
	  private static readonly BusinessDayAdjustment BD_ADJ = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, EUTA);
	  private static readonly TermDeposit TERM_DEPOSIT = TermDeposit.builder().buySell(BuySell.BUY).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BD_ADJ).dayCount(ACT_360).notional(NOTIONAL).currency(EUR).rate(RATE).build();
	  private static readonly ResolvedTermDeposit RTERM_DEPOSIT = TERM_DEPOSIT.resolve(REF_DATA);
	  private static readonly DiscountingTermDepositProductPricer PRICER = DiscountingTermDepositProductPricer.DEFAULT;
	  private const double TOLERANCE = 1E-12;

	  private const double EPS_FD = 1E-7;
	  private static readonly RatesFiniteDifferenceSensitivityCalculator CAL_FD = new RatesFiniteDifferenceSensitivityCalculator(EPS_FD);
	  private static readonly ImmutableRatesProvider IMM_PROV;
	  static DiscountingTermDepositProductPricerTest()
	  {
		CurveInterpolator interp = CurveInterpolators.DOUBLE_QUADRATIC;
		DoubleArray time_eur = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 3.0, 4.0, 5.0, 10.0);
		DoubleArray rate_eur = DoubleArray.of(0.0160, 0.0135, 0.0160, 0.0185, 0.0185, 0.0195, 0.0200, 0.0210);
		InterpolatedNodalCurve dscCurve = InterpolatedNodalCurve.of(Curves.zeroRates("EUR-Discount", ACT_360), time_eur, rate_eur, interp);
		IMM_PROV = ImmutableRatesProvider.builder(VAL_DATE).discountCurve(EUR, dscCurve).build();
	  }
	  private const double DF_START = 0.99;
	  internal double DF_END = 0.94;

	  //-------------------------------------------------------------------------
	  public virtual void test_presentValue_notStarted()
	  {
		SimpleRatesProvider prov = provider(VAL_DATE, DF_START, DF_END);
		CurrencyAmount computed = PRICER.presentValue(RTERM_DEPOSIT, prov);
		double expected = ((1d + RATE * RTERM_DEPOSIT.YearFraction) * DF_END - DF_START) * NOTIONAL;
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE * NOTIONAL);
	  }

	  public virtual void test_presentValue_onStart()
	  {
		SimpleRatesProvider prov = provider(START_DATE, 1.0d, DF_END);
		CurrencyAmount computed = PRICER.presentValue(RTERM_DEPOSIT, prov);
		double expected = ((1d + RATE * RTERM_DEPOSIT.YearFraction) * DF_END - 1.0d) * NOTIONAL;
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE * NOTIONAL);
	  }

	  public virtual void test_presentValue_started()
	  {
		SimpleRatesProvider prov = provider(date(2014, 2, 22), 1.2d, DF_END);
		CurrencyAmount computed = PRICER.presentValue(RTERM_DEPOSIT, prov);
		double expected = (1d + RATE * RTERM_DEPOSIT.YearFraction) * DF_END * NOTIONAL;
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE * NOTIONAL);
	  }

	  public virtual void test_presentValue_onEnd()
	  {
		SimpleRatesProvider prov = provider(END_DATE, 1.2d, 1.0d);
		CurrencyAmount computed = PRICER.presentValue(RTERM_DEPOSIT, prov);
		double expected = (1d + RATE * RTERM_DEPOSIT.YearFraction) * 1.0d * NOTIONAL;
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, expected, TOLERANCE * NOTIONAL);
	  }

	  public virtual void test_presentValue_ended()
	  {
		SimpleRatesProvider prov = provider(date(2014, 9, 22), 1.2d, 1.1d);
		CurrencyAmount computed = PRICER.presentValue(RTERM_DEPOSIT, prov);
		assertEquals(computed.Currency, EUR);
		assertEquals(computed.Amount, 0.0d, TOLERANCE * NOTIONAL);
	  }

	  public virtual void test_presentValueSensitivity()
	  {
		PointSensitivities computed = PRICER.presentValueSensitivity(RTERM_DEPOSIT, IMM_PROV);
		CurrencyParameterSensitivities sensiComputed = IMM_PROV.parameterSensitivity(computed);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV, (p) => PRICER.presentValue(RTERM_DEPOSIT, (p)));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, NOTIONAL * EPS_FD));
	  }

	  public virtual void test_parRate()
	  {
		SimpleRatesProvider prov = provider(VAL_DATE, DF_START, DF_END);
		double parRate = PRICER.parRate(RTERM_DEPOSIT, prov);
		TermDeposit depositPar = TermDeposit.builder().buySell(BuySell.BUY).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BD_ADJ).dayCount(ACT_360).notional(NOTIONAL).currency(EUR).rate(parRate).build();
		double pvPar = PRICER.presentValue(depositPar.resolve(REF_DATA), prov).Amount;
		assertEquals(pvPar, 0.0, NOTIONAL * TOLERANCE);
	  }

	  public virtual void test_parSpread()
	  {
		SimpleRatesProvider prov = provider(VAL_DATE, DF_START, DF_END);
		double parSpread = PRICER.parSpread(RTERM_DEPOSIT, prov);
		TermDeposit depositPar = TermDeposit.builder().buySell(BuySell.BUY).startDate(START_DATE).endDate(END_DATE).businessDayAdjustment(BD_ADJ).dayCount(ACT_360).notional(NOTIONAL).currency(EUR).rate(RATE + parSpread).build();
		double pvPar = PRICER.presentValue(depositPar.resolve(REF_DATA), prov).Amount;
		assertEquals(pvPar, 0.0, NOTIONAL * TOLERANCE);
	  }

	  public virtual void test_parSpreadSensitivity()
	  {
		PointSensitivities computed = PRICER.parSpreadSensitivity(RTERM_DEPOSIT, IMM_PROV);
		CurrencyParameterSensitivities sensiComputed = IMM_PROV.parameterSensitivity(computed);
		CurrencyParameterSensitivities sensiExpected = CAL_FD.sensitivity(IMM_PROV, (p) => CurrencyAmount.of(EUR, PRICER.parSpread(RTERM_DEPOSIT, (p))));
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, NOTIONAL * EPS_FD));
	  }

	  public virtual void test_parRateSensitivity()
	  {
		PointSensitivities computedSpread = PRICER.parSpreadSensitivity(RTERM_DEPOSIT, IMM_PROV);
		PointSensitivities computedRate = PRICER.parRateSensitivity(RTERM_DEPOSIT, IMM_PROV);
		assertTrue(computedSpread.equalWithTolerance(computedRate, NOTIONAL * EPS_FD));
	  }

	  private SimpleRatesProvider provider(LocalDate valuationDate, double dfStart, double dfEnd)
	  {
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		when(mockDf.discountFactor(START_DATE)).thenReturn(dfStart);
		when(mockDf.discountFactor(END_DATE)).thenReturn(dfEnd);
		SimpleRatesProvider prov = new SimpleRatesProvider(valuationDate, mockDf);
		return prov;
	  }

	}

}
/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.PRECEDING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.THIRTY_U_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using Iterables = com.google.common.collect.Iterables;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;
	using Swap = com.opengamma.strata.product.swap.Swap;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Test <seealso cref="CurveGammaCalculator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveGammaCalculatorTest
	public class CurveGammaCalculatorTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  // Data, based on RatesProviderDataSets.SINGLE_USD but different valuation date
	  private static readonly LocalDate VAL_DATE_2015_04_27 = LocalDate.of(2015, 4, 27);
	  private static readonly InterpolatedNodalCurve USD_SINGLE_CURVE = InterpolatedNodalCurve.of(Curves.zeroRates(RatesProviderDataSets.USD_SINGLE_NAME, ACT_360), RatesProviderDataSets.TIMES_1, RatesProviderDataSets.RATES_1_1, RatesProviderDataSets.INTERPOLATOR);
	  private static readonly ImmutableRatesProvider SINGLE = ImmutableRatesProvider.builder(VAL_DATE_2015_04_27).discountCurve(USD, USD_SINGLE_CURVE).overnightIndexCurve(USD_FED_FUND, USD_SINGLE_CURVE).iborIndexCurve(USD_LIBOR_3M, USD_SINGLE_CURVE).iborIndexCurve(USD_LIBOR_6M, USD_SINGLE_CURVE).build();
	  private static readonly Currency SINGLE_CURRENCY = Currency.USD;
	  // Conventions
	  private static readonly BusinessDayAdjustment BDA_MF = BusinessDayAdjustment.of(MODIFIED_FOLLOWING, USNY);
	  private static readonly BusinessDayAdjustment BDA_P = BusinessDayAdjustment.of(PRECEDING, USNY);
	  // Instrument
	  private static readonly ResolvedSwap SWAP = swapUsd(LocalDate.of(2016, 6, 30), LocalDate.of(2022, 6, 30), RECEIVE, NotionalSchedule.of(USD, 10_000_000), 0.01).resolve(REF_DATA);
	  // Calculators and pricers
	  private static readonly DiscountingSwapProductPricer PRICER_SWAP = DiscountingSwapProductPricer.DEFAULT;
	  private const double FD_SHIFT = 1.0E-5;
	  private static readonly CurveGammaCalculator GAMMA_CAL = CurveGammaCalculator.ofCentralDifference(FD_SHIFT);
	  // Constants
	  private const double TOLERANCE_GAMMA = 1.0E+1;

	  //-------------------------------------------------------------------------
	  public virtual void semiParallelGammaValue()
	  {
		ImmutableRatesProvider provider = SINGLE;
		Currency curveCurrency = SINGLE_CURRENCY;
		DoubleArray y = USD_SINGLE_CURVE.YValues;
		int nbNode = y.size();
		DoubleArray gammaExpected = DoubleArray.of(nbNode, i =>
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] yBumped = new double[2][2][nbNode];
		double[][][] yBumped = RectangularArrays.ReturnRectangularDoubleArray(2, 2, nbNode);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] pv = new double[2][2];
		double[][] pv = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		for (int pmi = 0; pmi < 2; pmi++)
		{
			for (int pmP = 0; pmP < 2; pmP++)
			{
				yBumped[pmi][pmP] = y.toArray();
				yBumped[pmi][pmP][i] += (pmi == 0 ? 1.0 : -1.0) * FD_SHIFT;
				for (int j = 0; j < nbNode; j++)
				{
					yBumped[pmi][pmP][j] += (pmP == 0 ? 1.0 : -1.0) * FD_SHIFT;
				}
				Curve curveBumped = USD_SINGLE_CURVE.withYValues(DoubleArray.copyOf(yBumped[pmi][pmP]));
				ImmutableRatesProvider providerBumped = provider.toBuilder().discountCurves(provider.DiscountCurves.Keys.collect(toImmutableMap(Function.identity(), k => curveBumped))).indexCurves(provider.IndexCurves.Keys.collect(toImmutableMap(Function.identity(), k => curveBumped))).build();
				pv[pmi][pmP] = PRICER_SWAP.presentValue(SWAP, providerBumped).getAmount(USD).Amount;
			}
		}
		return (pv[1][1] - pv[1][0] - pv[0][1] + pv[0][0]) / (4 * FD_SHIFT * FD_SHIFT);
		});
		CurrencyParameterSensitivity sensitivityComputed = GAMMA_CAL.calculateSemiParallelGamma(USD_SINGLE_CURVE, curveCurrency, c => buildSensitivities(c, provider));
		assertEquals(sensitivityComputed.MarketDataName, USD_SINGLE_CURVE.Name);
		DoubleArray gammaComputed = sensitivityComputed.Sensitivity;
		assertTrue(gammaComputed.equalWithTolerance(gammaExpected, TOLERANCE_GAMMA));
	  }

	  // Checks that different finite difference types and shifts give similar results.
	  public virtual void semiParallelGammaCoherency()
	  {
		ImmutableRatesProvider provider = SINGLE;
		Curve curve = Iterables.getOnlyElement(provider.DiscountCurves.values());
		Currency curveCurrency = SINGLE_CURRENCY;
		double toleranceCoherency = 1.0E+5;
		CurveGammaCalculator calculatorForward5 = CurveGammaCalculator.ofForwardDifference(FD_SHIFT);
		CurveGammaCalculator calculatorBackward5 = CurveGammaCalculator.ofBackwardDifference(FD_SHIFT);
		CurveGammaCalculator calculatorCentral4 = CurveGammaCalculator.ofCentralDifference(1.0E-4);
		DoubleArray gammaCentral5 = GAMMA_CAL.calculateSemiParallelGamma(curve, curveCurrency, c => buildSensitivities(c, provider)).Sensitivity;

		DoubleArray gammaForward5 = calculatorForward5.calculateSemiParallelGamma(curve, curveCurrency, c => buildSensitivities(c, provider)).Sensitivity;
		assertTrue(gammaForward5.equalWithTolerance(gammaCentral5, toleranceCoherency));

		DoubleArray gammaBackward5 = calculatorBackward5.calculateSemiParallelGamma(curve, curveCurrency, c => buildSensitivities(c, provider)).Sensitivity;
		assertTrue(gammaForward5.equalWithTolerance(gammaBackward5, toleranceCoherency));

		DoubleArray gammaCentral4 = calculatorCentral4.calculateSemiParallelGamma(curve, curveCurrency, c => buildSensitivities(c, provider)).Sensitivity;
		assertTrue(gammaForward5.equalWithTolerance(gammaCentral4, toleranceCoherency));
	  }

	  //-------------------------------------------------------------------------
	  private static CurrencyParameterSensitivity buildSensitivities(Curve bumpedCurve, ImmutableRatesProvider ratesProvider)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		RatesProvider bumpedRatesProvider = ratesProvider.toBuilder().discountCurves(ratesProvider.DiscountCurves.Keys.collect(toImmutableMap(System.Func.identity(), k => bumpedCurve))).indexCurves(ratesProvider.IndexCurves.Keys.collect(toImmutableMap(System.Func.identity(), k => bumpedCurve))).build();
		PointSensitivities pointSensitivities = PRICER_SWAP.presentValueSensitivity(SWAP, bumpedRatesProvider).build();
		CurrencyParameterSensitivities paramSensitivities = bumpedRatesProvider.parameterSensitivity(pointSensitivities);
		return Iterables.getOnlyElement(paramSensitivities.Sensitivities);
	  }

	  // swap USD standard conventions- TODO: replace by a template when available
	  private static Swap swapUsd(LocalDate start, LocalDate end, PayReceive payReceive, NotionalSchedule notional, double fixedRate)
	  {
		SwapLeg fixedLeg = CurveGammaCalculatorTest.fixedLeg(start, end, Frequency.P6M, payReceive, notional, fixedRate, StubConvention.SHORT_INITIAL);
		SwapLeg iborLeg = CurveGammaCalculatorTest.iborLeg(start, end, USD_LIBOR_3M, (payReceive == PAY) ? RECEIVE : PAY, notional, StubConvention.SHORT_INITIAL);
		return Swap.of(fixedLeg, iborLeg);
	  }

	  // fixed rate leg
	  private static SwapLeg fixedLeg(LocalDate start, LocalDate end, Frequency frequency, PayReceive payReceive, NotionalSchedule notional, double fixedRate, StubConvention stubConvention)
	  {

		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(start).endDate(end).frequency(frequency).businessDayAdjustment(BDA_MF).stubConvention(stubConvention).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(frequency).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(FixedRateCalculation.builder().dayCount(THIRTY_U_360).rate(ValueSchedule.of(fixedRate)).build()).build();
	  }

	  // fixed rate leg
	  private static SwapLeg iborLeg(LocalDate start, LocalDate end, IborIndex index, PayReceive payReceive, NotionalSchedule notional, StubConvention stubConvention)
	  {
		Frequency freq = Frequency.of(index.Tenor.Period);
		return RateCalculationSwapLeg.builder().payReceive(payReceive).accrualSchedule(PeriodicSchedule.builder().startDate(start).endDate(end).frequency(freq).businessDayAdjustment(BDA_MF).stubConvention(stubConvention).build()).paymentSchedule(PaymentSchedule.builder().paymentFrequency(freq).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(notional).calculation(IborRateCalculation.builder().index(index).fixingDateOffset(DaysAdjustment.ofBusinessDays(-2, index.FixingCalendar, BDA_P)).build()).build();
	  }

	}

}
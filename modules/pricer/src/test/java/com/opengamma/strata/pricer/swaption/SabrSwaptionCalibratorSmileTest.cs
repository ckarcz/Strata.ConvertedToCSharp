using System.Collections;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.swap.type.FixedIborSwapConventions.USD_FIXED_6M_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using NormalFormulaRepository = com.opengamma.strata.pricer.impl.option.NormalFormulaRepository;
	using SabrFormulaData = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Tests <seealso cref="SabrSwaptionCalibrator"/> with single smile.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionCalibratorSmileTest
	public class SabrSwaptionCalibratorSmileTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly LocalDate CALIBRATION_DATE = LocalDate.of(2015, 8, 7);
	  private static readonly ZonedDateTime CALIBRATION_TIME = CALIBRATION_DATE.atTime(11, 0).atZone(ZoneId.of("America/New_York"));

	  private static readonly SabrVolatilityFormula SABR_FORMULA = SabrVolatilityFormula.hagan();
	  private static readonly SabrSwaptionCalibrator SABR_CALIBRATION = SabrSwaptionCalibrator.DEFAULT;

	  private static readonly Period EXPIRY_PERIOD = Period.ofYears(5);
	  private static readonly BusinessDayAdjustment BDA = USD_FIXED_6M_LIBOR_3M.FloatingLeg.StartDateBusinessDayAdjustment;

	  private static readonly LocalDate SWAPTION_EXERCISE_DATE = USD_FIXED_6M_LIBOR_3M.FixedLeg.StartDateBusinessDayAdjustment.adjust(CALIBRATION_DATE.plus(EXPIRY_PERIOD), REF_DATA);
	  private static readonly double TIME_EXPIRY = ACT_365F.relativeYearFraction(CALIBRATION_DATE, SWAPTION_EXERCISE_DATE);
	  private const double FORWARD = 0.02075;
	  private static readonly DoubleArray MONEYNESS_5 = DoubleArray.of(-0.0100, -0.0050, 0.0000, 0.0050, 0.0100);
	  private static readonly DoubleArray MONEYNESS_3 = DoubleArray.of(-0.0100, 0.0000, 0.0100);
	  private static readonly DoubleArray VOLATILITY_BLACK_5 = DoubleArray.of(0.34, 0.32, 0.30, 0.29, 0.29);
	  private static readonly DoubleArray VOLATILITY_BLACK_3 = DoubleArray.of(0.335, 0.30, 0.29);
	  private static readonly DoubleArray VOLATILITY_NORMAL_5 = DoubleArray.of(0.0110, 0.0098, 0.0092, 0.0103, 0.0120);
	  private static readonly DoubleArray VOLATILITY_NORMAL_3 = DoubleArray.of(0.0110, 0.0092, 0.0120);

	  private const double TOLERANCE_PRICE_CALIBRATION_LS = 1.0E-4; // Calibration Least Square; result not exact
	  private const double TOLERANCE_PRICE_CALIBRATION_EX = 1.0E-6; // With 3 points, calibration should be almost exact

	  public virtual void calibrate_smile_normal_beta_fixed_5()
	  {
		double beta = 0.50;
		double shift = 0.0100; // 100 bps
		DoubleArray startParameters = DoubleArray.of(0.05, beta, 0.0, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		checkCalibrationNormal(MONEYNESS_5, VOLATILITY_NORMAL_5, startParameters, @fixed, shift, TOLERANCE_PRICE_CALIBRATION_LS);
	  }

	  public virtual void calibrate_smile_normal_beta_fixed_3()
	  {
		double beta = 0.50;
		double shift = 0.0100; // 100 bps
		DoubleArray startParameters = DoubleArray.of(0.05, beta, 0.0, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		checkCalibrationNormal(MONEYNESS_3, VOLATILITY_NORMAL_3, startParameters, @fixed, shift, TOLERANCE_PRICE_CALIBRATION_EX);
	  }

	  public virtual void calibrate_smile_normal_rho_fixed_5()
	  {
		double rho = 0.25;
		double shift = 0.0100; // 100 bps
		DoubleArray startParameters = DoubleArray.of(0.05, 0.50, rho, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(2, true); // Rho fixed
		checkCalibrationNormal(MONEYNESS_5, VOLATILITY_NORMAL_5, startParameters, @fixed, shift, 20 * TOLERANCE_PRICE_CALIBRATION_LS);
	  }

	  public virtual void calibrate_smile_black_no_shift_beta_fixed_5()
	  {
		double beta = 0.50;
		DoubleArray startParameters = DoubleArray.of(0.05, beta, 0.0, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		checkCalibrationBlack(MONEYNESS_5, VOLATILITY_BLACK_5, startParameters, @fixed, 0.0, TOLERANCE_PRICE_CALIBRATION_LS);
	  }

	  public virtual void calibrate_smile_black_no_shift_beta_fixed_3()
	  {
		double beta = 0.50;
		DoubleArray startParameters = DoubleArray.of(0.05, beta, 0.0, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		checkCalibrationBlack(MONEYNESS_3, VOLATILITY_BLACK_3, startParameters, @fixed, 0.0, TOLERANCE_PRICE_CALIBRATION_EX);
	  }

	  public virtual void calibrate_smile_price_beta_fixed_5()
	  {
		double beta = 0.50;
		double shift = 0.0100; // 100 bps
		DoubleArray startParameters = DoubleArray.of(0.05, beta, 0.0, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		checkCalibrationPrice(MONEYNESS_5, VOLATILITY_BLACK_5, startParameters, @fixed, shift, TOLERANCE_PRICE_CALIBRATION_LS);
	  }

	  public virtual void calibrate_smile_price_beta_fixed_3()
	  {
		double beta = 0.50;
		double shift = 0.0100; // 100 bps
		DoubleArray startParameters = DoubleArray.of(0.05, beta, 0.0, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		checkCalibrationPrice(MONEYNESS_3, VOLATILITY_BLACK_3, startParameters, @fixed, shift, TOLERANCE_PRICE_CALIBRATION_EX);
	  }

	  public virtual void calibrate_smile_price_rho_fixed_5()
	  {
		double rho = 0.25;
		double shift = 0.0100; // 100 bps
		DoubleArray startParameters = DoubleArray.of(0.05, 0.50, rho, 0.1);
		BitArray @fixed = new BitArray();
		@fixed.Set(2, true); // Rho fixed
		checkCalibrationPrice(MONEYNESS_5, VOLATILITY_BLACK_5, startParameters, @fixed, shift, TOLERANCE_PRICE_CALIBRATION_LS);
	  }

	  private void checkCalibrationNormal(DoubleArray moneyness, DoubleArray normalVol, DoubleArray startParameters, BitArray @fixed, double shift, double tolerance)
	  {
		Pair<LeastSquareResultsWithTransform, DoubleArray> rComputed = SABR_CALIBRATION.calibrateLsShiftedFromNormalVolatilities(BDA, CALIBRATION_TIME, ACT_365F, EXPIRY_PERIOD, FORWARD, moneyness, ValueType.SIMPLE_MONEYNESS, normalVol, startParameters, @fixed, shift);
		SabrFormulaData sabrComputed = SabrFormulaData.of(rComputed.First.ModelParameters.toArrayUnsafe());
		for (int i = 0; i < moneyness.size(); i++)
		{
		  double ivComputed = SABR_FORMULA.volatility(FORWARD + shift, FORWARD + moneyness.get(i) + shift, TIME_EXPIRY, sabrComputed.Alpha, sabrComputed.Beta, sabrComputed.Rho, sabrComputed.Nu);
		  double priceComputed = BlackFormulaRepository.price(FORWARD + shift, FORWARD + moneyness.get(i) + shift, TIME_EXPIRY, ivComputed, true);
		  double priceNormal = NormalFormulaRepository.price(FORWARD, FORWARD + moneyness.get(i), TIME_EXPIRY, normalVol.get(i), PutCall.CALL);
		  assertEquals(priceComputed, priceNormal, tolerance);
		}
	  }

	  private void checkCalibrationBlack(DoubleArray moneyness, DoubleArray blackVol, DoubleArray startParameters, BitArray @fixed, double shift, double tolerance)
	  {
		Pair<LeastSquareResultsWithTransform, DoubleArray> rComputed = SABR_CALIBRATION.calibrateLsShiftedFromBlackVolatilities(BDA, CALIBRATION_TIME, ACT_365F, EXPIRY_PERIOD, FORWARD, moneyness, ValueType.SIMPLE_MONEYNESS, blackVol, 0.0, startParameters, @fixed, shift);
		SabrFormulaData sabrComputed = SabrFormulaData.of(rComputed.First.ModelParameters.toArrayUnsafe());
		for (int i = 0; i < moneyness.size(); i++)
		{
		  double ivComputed = SABR_FORMULA.volatility(FORWARD + shift, FORWARD + moneyness.get(i) + shift, TIME_EXPIRY, sabrComputed.Alpha, sabrComputed.Beta, sabrComputed.Rho, sabrComputed.Nu);
		  double priceComputed = BlackFormulaRepository.price(FORWARD + shift, FORWARD + moneyness.get(i) + shift, TIME_EXPIRY, ivComputed, true);
		  double priceBlack = BlackFormulaRepository.price(FORWARD, FORWARD + moneyness.get(i), TIME_EXPIRY, blackVol.get(i), true);
		  assertEquals(priceComputed, priceBlack, tolerance);
	//      System.out.println("Black: " + priceComputed + " / " + priceBlack);
		}
	  }

	  private void checkCalibrationPrice(DoubleArray moneyness, DoubleArray blackVol, DoubleArray startParameters, BitArray @fixed, double shift, double tolerance)
	  {
		double[] prices = new double[moneyness.size()];
		for (int i = 0; i < moneyness.size(); i++)
		{
		  prices[i] = BlackFormulaRepository.price(FORWARD, FORWARD + moneyness.get(i), TIME_EXPIRY, blackVol.get(i), true);
		  // Prices generated from Black implied volatilities
		}
		Pair<LeastSquareResultsWithTransform, DoubleArray> rComputed = SABR_CALIBRATION.calibrateLsShiftedFromPrices(BDA, CALIBRATION_TIME, ACT_365F, EXPIRY_PERIOD, FORWARD, moneyness, ValueType.SIMPLE_MONEYNESS, DoubleArray.ofUnsafe(prices), startParameters, @fixed, shift);
		SabrFormulaData sabrComputed = SabrFormulaData.of(rComputed.First.ModelParameters.toArrayUnsafe());
		for (int i = 0; i < moneyness.size(); i++)
		{
		  double ivComputed = SABR_FORMULA.volatility(FORWARD + shift, FORWARD + moneyness.get(i) + shift, TIME_EXPIRY, sabrComputed.Alpha, sabrComputed.Beta, sabrComputed.Rho, sabrComputed.Nu);
		  double priceComputed = BlackFormulaRepository.price(FORWARD + shift, FORWARD + moneyness.get(i) + shift, TIME_EXPIRY, ivComputed, true);
		  assertEquals(priceComputed, prices[i], tolerance);
		}
	  }

	}

}
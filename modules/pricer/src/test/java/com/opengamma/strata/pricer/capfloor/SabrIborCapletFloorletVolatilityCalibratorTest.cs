using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PCHIP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Test <seealso cref="SabrIborCapletFloorletVolatilityCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrIborCapletFloorletVolatilityCalibratorTest extends CapletStrippingSetup
	public class SabrIborCapletFloorletVolatilityCalibratorTest : CapletStrippingSetup
	{

	  private static readonly SabrIborCapletFloorletVolatilityCalibrator CALIBRATOR = SabrIborCapletFloorletVolatilityCalibrator.DEFAULT;
	  private static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("test");
	  private static readonly SabrVolatilityFormula HAGAN = SabrVolatilityFormula.hagan();
	  // choose nodes close to expiries of caps - 0.25y before end dates
	  private static readonly DoubleArray ALPHA_KNOTS = DoubleArray.of(0.75, 1.75, 2.75, 4.75, 6.75, 9.75);
	  private static readonly DoubleArray BETA_RHO_KNOTS = DoubleArray.of(0.75, 2.75, 4.75);
	  private static readonly DoubleArray NU_KNOTS = DoubleArray.of(0.75, 1.75, 2.75, 4.75, 6.75, 9.75);
	  private const double TOL = 1.0e-3;

	  public virtual void recovery_test_black()
	  {
		double beta = 0.7;
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, beta, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, DOUBLE_QUADRATIC, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix volData = createFullBlackDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < NUM_BLACK_STRIKES; ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsBlackVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			BlackIborCapletFloorletExpiryStrikeVolatilities constVol = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 3d);
		  }
		}
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVols.ParameterCount, ALPHA_KNOTS.size() + BETA_RHO_KNOTS.size() + NU_KNOTS.size() + 2); // beta, shift counted
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
		assertEquals(resVols.Parameters.BetaCurve, definition.BetaCurve.get());
	  }

	  public virtual void recovery_test_black_fixedRho()
	  {
		double rho = 0.15;
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedRho(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, rho, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, DOUBLE_QUADRATIC, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix volData = createFullBlackDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < NUM_BLACK_STRIKES; ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsBlackVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			BlackIborCapletFloorletExpiryStrikeVolatilities constVol = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVols.ParameterCount, ALPHA_KNOTS.size() + BETA_RHO_KNOTS.size() + NU_KNOTS.size() + 2); // beta, shift counted
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
		assertEquals(resVols.Parameters.RhoCurve, definition.RhoCurve.get());
	  }

	  public virtual void recovery_test_black_shift()
	  {
		double shift = 0.05;
		DoubleArray initial = DoubleArray.of(0.03, 0.7, 0.15, 0.9);
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, shift, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, initial, PCHIP, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix volData = createFullBlackDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < NUM_BLACK_STRIKES; ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsBlackVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			BlackIborCapletFloorletExpiryStrikeVolatilities constVol = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
		assertEquals(resVols.Parameters.BetaCurve, definition.BetaCurve.get());
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
	  }

	  public virtual void recovery_test_black_shift_fixedRho()
	  {
		double shift = 0.05;
		DoubleArray initial = DoubleArray.of(0.03, 0.7, 0.35, 0.9);
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedRho(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, shift, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, initial, PCHIP, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix volData = createFullBlackDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < NUM_BLACK_STRIKES; ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsBlackVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			BlackIborCapletFloorletExpiryStrikeVolatilities constVol = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
		assertEquals(resVols.Parameters.RhoCurve, definition.RhoCurve.get());
	  }

	  public virtual void recovery_test_flat()
	  {
		DoubleArray initial = DoubleArray.of(0.4, 0.95, 0.5, 0.05);
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, initial, LINEAR, FLAT, FLAT, HAGAN);
		DoubleArray strikes = createBlackStrikes();
		RawOptionData data = RawOptionData.of(createBlackMaturities(), strikes, ValueType.STRIKE, createFullFlatBlackDataMatrix(), ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrIborCapletFloorletVolatilities resVol = (SabrIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < NUM_BLACK_STRIKES; ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsFlatBlackVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			BlackIborCapletFloorletExpiryStrikeVolatilities constVol = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
	  }

	  public virtual void recovery_test_normal()
	  {
		double beta = 0.7;
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, beta, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, DOUBLE_QUADRATIC, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createNormalEquivMaturities();
		DoubleArray strikes = createNormalEquivStrikes();
		DoubleMatrix volData = createFullNormalEquivDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrIborCapletFloorletVolatilities resVols = (SabrIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsNormalEquivVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.normalVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			NormalIborCapletFloorletExpiryStrikeVolatilities constVol = NormalIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 3d);
		  }
		}
	  }

	  public virtual void recovery_test_normal_fixedRho()
	  {
		DoubleArray initial = DoubleArray.of(0.05, 0.7, 0.35, 0.9);
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedRho(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, initial, DOUBLE_QUADRATIC, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createNormalEquivMaturities();
		DoubleArray strikes = createNormalEquivStrikes();
		DoubleMatrix volData = createFullNormalEquivDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrIborCapletFloorletVolatilities resVols = (SabrIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsNormalEquivVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.normalVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			NormalIborCapletFloorletExpiryStrikeVolatilities constVol = NormalIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
	  }

	  public virtual void recovery_test_normal_shift()
	  {
		double shift = 0.02;
		DoubleArray initial = DoubleArray.of(0.05, 0.7, 0.35, 0.9);
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, shift, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, initial, DOUBLE_QUADRATIC, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createNormalEquivMaturities();
		DoubleArray strikes = createNormalEquivStrikes();
		DoubleMatrix volData = createFullNormalEquivDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrIborCapletFloorletVolatilities resVols = (SabrIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsNormalEquivVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.normalVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			NormalIborCapletFloorletExpiryStrikeVolatilities constVol = NormalIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
	  }

	  public virtual void recovery_test_normal_shift_fixedRho()
	  {
		double shift = 0.02;
		DoubleArray initial = DoubleArray.of(0.05, 0.35, 0.0, 0.9);
		SabrIborCapletFloorletVolatilityCalibrationDefinition definition = SabrIborCapletFloorletVolatilityCalibrationDefinition.ofFixedRho(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, shift, ALPHA_KNOTS, BETA_RHO_KNOTS, NU_KNOTS, initial, DOUBLE_QUADRATIC, FLAT, FLAT, HAGAN);
		ImmutableList<Period> maturities = createNormalEquivMaturities();
		DoubleArray strikes = createNormalEquivStrikes();
		DoubleMatrix volData = createFullNormalEquivDataMatrix();
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), 1.0e-3);
		RawOptionData data = RawOptionData.of(maturities, strikes, ValueType.STRIKE, volData, error, ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrIborCapletFloorletVolatilities resVols = (SabrIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsNormalEquivVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.normalVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			NormalIborCapletFloorletExpiryStrikeVolatilities constVol = NormalIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
	  }

	}

}
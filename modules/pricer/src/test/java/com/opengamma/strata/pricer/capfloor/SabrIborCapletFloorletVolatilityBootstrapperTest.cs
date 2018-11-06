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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using SabrHaganVolatilityFunctionProvider = com.opengamma.strata.pricer.impl.volatility.smile.SabrHaganVolatilityFunctionProvider;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Test <seealso cref="SabrIborCapletFloorletVolatilityBootstrapper"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrIborCapletFloorletVolatilityBootstrapperTest extends CapletStrippingSetup
	public class SabrIborCapletFloorletVolatilityBootstrapperTest : CapletStrippingSetup
	{

	  private static readonly SabrIborCapletFloorletVolatilityBootstrapper CALIBRATOR = SabrIborCapletFloorletVolatilityBootstrapper.DEFAULT;
	  private const double TOL = 1.0e-3;

	  public virtual void test_recovery_black()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, 0.85, CurveInterpolators.STEP_UPPER, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		DoubleMatrix volData = createFullBlackDataMatrix();
		double errorValue = 1.0e-3;
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), errorValue);
		RawOptionData data = RawOptionData.of(createBlackMaturities(), createBlackStrikes(), ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		double expSq = 0d;
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
			expSq += Math.Pow((priceOrg - priceCalib) / priceOrg / errorValue, 2);
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 3d);
		  }
		}
		assertEquals(res.ChiSquare, expSq, expSq * 1.0e-14);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
		assertEquals(resVols.Parameters.BetaCurve, definition.BetaCurve.get());
	  }

	  public virtual void test_recovery_black_fixedRho()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedRho(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, 0.0, CurveInterpolators.STEP_UPPER, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		DoubleMatrix volData = createFullBlackDataMatrix();
		double errorValue = 1.0e-3;
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), errorValue);
		RawOptionData data = RawOptionData.of(createBlackMaturities(), createBlackStrikes(), ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		double expSq = 0d;
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
			expSq += Math.Pow((priceOrg - priceCalib) / priceOrg / errorValue, 2);
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 3d);
		  }
		}
		assertEquals(res.ChiSquare, expSq, expSq * 1.0e-14);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
		assertEquals(resVols.Parameters.RhoCurve, definition.RhoCurve.get());
	  }

	  public virtual void test_invalid_data()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, 0.85, CurveInterpolators.LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		RawOptionData data = RawOptionData.of(createBlackMaturities(), createBlackStrikes(), ValueType.STRIKE, createFullBlackDataMatrixInvalid(), ValueType.BLACK_VOLATILITY);
		assertThrowsIllegalArg(() => CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER));
	  }

	  public virtual void test_recovery_black_shift()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, 0.95, 0.02, CurveInterpolators.LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		DoubleMatrix volData = createFullBlackDataMatrix();
		double errorValue = 1.0e-3;
		DoubleMatrix error = DoubleMatrix.filled(volData.rowCount(), volData.columnCount(), errorValue);
		RawOptionData data = RawOptionData.of(createBlackMaturities(), createBlackStrikes(), ValueType.STRIKE, volData, error, ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		double expSq = 0d;
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
			expSq += Math.Pow((priceOrg - priceCalib) / priceOrg / errorValue, 2);
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 10d);
		  }
		}
		assertEquals(res.ChiSquare, expSq, expSq * 1.0e-14);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVols.Parameters.ShiftCurve, definition.ShiftCurve);
		assertEquals(resVols.Parameters.BetaCurve, definition.BetaCurve.get());
	  }

	  public virtual void test_recovery_normal()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, 0.85, CurveInterpolators.LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		RawOptionData data = RawOptionData.of(createNormalEquivMaturities(), createNormalEquivStrikes(), ValueType.STRIKE, createFullNormalEquivDataMatrix(), ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 1; i < NUM_BLACK_STRIKES; ++i)
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
		assertTrue(res.ChiSquare > 0d);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
	  }

	  public virtual void test_recovery_normal_fixedRho()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedRho(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, 0.0, CurveInterpolators.LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		RawOptionData data = RawOptionData.of(createNormalEquivMaturities(), createNormalEquivStrikes(), ValueType.STRIKE, createFullNormalEquivDataMatrix(), ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
		for (int i = 1; i < NUM_BLACK_STRIKES; ++i)
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
		assertTrue(res.ChiSquare > 0d);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
	  }

	  public virtual void test_recovery_flatVol()
	  {
		double beta = 0.8;
		SabrIborCapletFloorletVolatilityBootstrapDefinition definition = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, beta, CurveInterpolators.STEP_UPPER, CurveExtrapolators.FLAT, CurveExtrapolators.FLAT, SabrHaganVolatilityFunctionProvider.DEFAULT);
		RawOptionData data = RawOptionData.of(createBlackMaturities(), createBlackStrikes(), ValueType.STRIKE, createFullFlatBlackDataMatrix(), ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		SabrParametersIborCapletFloorletVolatilities resVols = (SabrParametersIborCapletFloorletVolatilities) res.Volatilities;
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
			double priceCalib = LEG_PRICER_SABR.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
	  }

	}

}
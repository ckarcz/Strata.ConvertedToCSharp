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
//	import static com.opengamma.strata.market.ValueType.BLACK_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.STRIKE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Test <seealso cref="DirectIborCapletFloorletVolatilityCalibrator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DirectIborCapletFloorletVolatilityCalibratorTest extends CapletStrippingSetup
	public class DirectIborCapletFloorletVolatilityCalibratorTest : CapletStrippingSetup
	{

	  private static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("test");
	  private static readonly GridSurfaceInterpolator INTERPOLATOR = GridSurfaceInterpolator.of(CurveInterpolators.LINEAR, CurveInterpolators.LINEAR);
	  private static readonly DirectIborCapletFloorletVolatilityCalibrator CALIBRATOR = DirectIborCapletFloorletVolatilityCalibrator.standard();
	  private new static readonly BlackIborCapFloorLegPricer LEG_PRICER_BLACK = BlackIborCapFloorLegPricer.DEFAULT;
	  private new static readonly NormalIborCapFloorLegPricer LEG_PRICER_NORMAL = NormalIborCapFloorLegPricer.DEFAULT;
	  private const double TOL = 1.0e-4;

	  public virtual void test_recovery_black()
	  {
		double lambdaT = 0.07;
		double lambdaK = 0.07;
		double error = 1.0e-5;
		DirectIborCapletFloorletVolatilityDefinition definition = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, lambdaT, lambdaK, INTERPOLATOR);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix errorMatrix = DoubleMatrix.filled(maturities.size(), strikes.size(), error);
		RawOptionData data = RawOptionData.of(maturities, strikes, STRIKE, createFullBlackDataMatrix(), errorMatrix, BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		BlackIborCapletFloorletExpiryStrikeVolatilities resVols = (BlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
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
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL * 5d);
		  }
		}
		assertTrue(res.ChiSquare > 0d);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
	  }

	  public virtual void recovery_test_shiftedBlack()
	  {
		double lambdaT = 0.07;
		double lambdaK = 0.07;
		double error = 1.0e-5;
		ConstantCurve shiftCurve = ConstantCurve.of("Black shift", 0.02);
		DirectIborCapletFloorletVolatilityDefinition definition = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, lambdaT, lambdaK, INTERPOLATOR, shiftCurve);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix errorMatrix = DoubleMatrix.filled(maturities.size(), strikes.size(), error);
		RawOptionData data = RawOptionData.of(maturities, strikes, STRIKE, createFullBlackDataMatrix(), errorMatrix, BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities resVols = (ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
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
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVols).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
		assertTrue(res.ChiSquare > 0d);
		assertEquals(resVols.Index, USD_LIBOR_3M);
		assertEquals(resVols.Name, definition.Name);
		assertEquals(resVols.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVols.ShiftCurve, definition.ShiftCurve.get());
	  }

	  public virtual void recovery_test_flat()
	  {
		double lambdaT = 0.01;
		double lambdaK = 0.01;
		double error = 1.0e-3;
		DirectIborCapletFloorletVolatilityDefinition definition = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, lambdaT, lambdaK, INTERPOLATOR);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix errorMatrix = DoubleMatrix.filled(maturities.size(), strikes.size(), error);
		RawOptionData data = RawOptionData.of(maturities, strikes, STRIKE, createFullFlatBlackDataMatrix(), errorMatrix, BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		BlackIborCapletFloorletExpiryStrikeVolatilities resVol = (BlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
		Surface resSurface = resVol.Surface;
		int nParams = resSurface.ParameterCount;
		for (int i = 0; i < nParams; ++i)
		{
		  assertEquals(resSurface.getParameter(i), 0.5, 1.0e-11);
		}
	  }

	  public virtual void recovery_test_normalFlat()
	  {
		double lambdaT = 0.01;
		double lambdaK = 0.01;
		double error = 1.0e-3;
		DirectIborCapletFloorletVolatilityDefinition definition = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, lambdaT, lambdaK, INTERPOLATOR);
		ImmutableList<Period> maturities = createBlackMaturities();
		DoubleArray strikes = createBlackStrikes();
		DoubleMatrix errorMatrix = DoubleMatrix.filled(maturities.size(), strikes.size(), error);
		RawOptionData data = RawOptionData.of(maturities, strikes, STRIKE, createFullFlatBlackDataMatrix(), errorMatrix, NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		NormalIborCapletFloorletExpiryStrikeVolatilities resVol = (NormalIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
		Surface resSurface = resVol.Surface;
		int nParams = resSurface.ParameterCount;
		for (int i = 0; i < nParams; ++i)
		{
		  assertEquals(resSurface.getParameter(i), 0.5, 1.0e-12);
		}
	  }

	  public virtual void recovery_test_normal()
	  {
		double lambdaT = 0.07;
		double lambdaK = 0.07;
		double error = 1.0e-5;
		DirectIborCapletFloorletVolatilityDefinition definition = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, lambdaT, lambdaK, INTERPOLATOR);
		ImmutableList<Period> maturities = createNormalMaturities();
		DoubleArray strikes = createNormalStrikes();
		DoubleMatrix errorMatrix = DoubleMatrix.filled(maturities.size(), strikes.size(), error);
		RawOptionData data = RawOptionData.of(maturities, strikes, STRIKE, createFullNormalDataMatrix(), errorMatrix, NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		NormalIborCapletFloorletExpiryStrikeVolatilities resVol = (NormalIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
		{
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsNormalVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.normalVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			NormalIborCapletFloorletExpiryStrikeVolatilities constVol = NormalIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
	  }

	  public virtual void recovery_test_normalToBlack()
	  {
		double lambdaT = 0.07;
		double lambdaK = 0.07;
		double error = 1.0e-5;
		ConstantCurve shiftCurve = ConstantCurve.of("Black shift", 0.02);
		DirectIborCapletFloorletVolatilityDefinition definition = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, lambdaT, lambdaK, INTERPOLATOR, shiftCurve);
		ImmutableList<Period> maturities = createNormalEquivMaturities();
		DoubleArray strikes = createNormalEquivStrikes();
		DoubleMatrix errorMatrix = DoubleMatrix.filled(maturities.size(), strikes.size(), error);
		RawOptionData data = RawOptionData.of(maturities, strikes, STRIKE, createFullNormalEquivDataMatrix(), errorMatrix, NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities resVol = (ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
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
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
	  }

	}

}
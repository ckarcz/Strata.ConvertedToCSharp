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
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GenericVolatilitySurfacePeriodParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfacePeriodParameterMetadata;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Test <seealso cref="SurfaceIborCapletFloorletVolatilityBootstrapper"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SurfaceIborCapletFloorletVolatilityBootstrapperTest extends CapletStrippingSetup
	public class SurfaceIborCapletFloorletVolatilityBootstrapperTest : CapletStrippingSetup
	{

	  private static readonly SurfaceIborCapletFloorletVolatilityBootstrapper CALIBRATOR = SurfaceIborCapletFloorletVolatilityBootstrapper.DEFAULT;
	  private const double TOL = 1.0e-14;

	  public virtual void recovery_test_blackSurface()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, LINEAR);
		DoubleArray strikes = createBlackStrikes();
		RawOptionData data = RawOptionData.of(createBlackMaturities(), strikes, ValueType.STRIKE, createFullBlackDataMatrix(), ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		BlackIborCapletFloorletExpiryStrikeVolatilities resVol = (BlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
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
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
		assertEquals(res.ChiSquare, 0d);
		assertEquals(resVol.Index, USD_LIBOR_3M);
		assertEquals(resVol.Name, definition.Name);
		assertEquals(resVol.ValuationDateTime, CALIBRATION_TIME);
		InterpolatedNodalSurface surface = (InterpolatedNodalSurface) resVol.Surface;
		for (int i = 0; i < surface.ParameterCount; ++i)
		{
		  GenericVolatilitySurfacePeriodParameterMetadata metadata = (GenericVolatilitySurfacePeriodParameterMetadata) surface.getParameterMetadata(i);
		  assertEquals(metadata.Strike.Value, surface.YValues.get(i));
		}
	  }

	  public virtual void test_invalid_data()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, LINEAR);
		DoubleArray strikes = createBlackStrikes();
		RawOptionData data = RawOptionData.of(createBlackMaturities(), strikes, ValueType.STRIKE, createFullBlackDataMatrixInvalid(), ValueType.BLACK_VOLATILITY);
		assertThrowsIllegalArg(() => CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER));
	  }

	  public virtual void recovery_test_blackSurface_shift()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, LINEAR, ConstantCurve.of("Black shift", 0.02));
		DoubleArray strikes = createBlackStrikes();
		RawOptionData data = RawOptionData.of(createBlackMaturities(), strikes, ValueType.STRIKE, createFullBlackDataMatrix(), ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities resVol = (ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
		for (int i = 0; i < strikes.size(); ++i)
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
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
		assertEquals(res.ChiSquare, 0d);
		assertEquals(resVol.Index, USD_LIBOR_3M);
		assertEquals(resVol.Name, definition.Name);
		assertEquals(resVol.ValuationDateTime, CALIBRATION_TIME);
		assertEquals(resVol.ShiftCurve, definition.ShiftCurve.get());
		InterpolatedNodalSurface surface = (InterpolatedNodalSurface) resVol.Surface;
		for (int i = 0; i < surface.ParameterCount; ++i)
		{
		  GenericVolatilitySurfacePeriodParameterMetadata metadata = (GenericVolatilitySurfacePeriodParameterMetadata) surface.getParameterMetadata(i);
		  assertEquals(metadata.Strike.Value + 0.02, surface.YValues.get(i));
		}
	  }

	  public virtual void recovery_test_blackCurve()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, LINEAR);
		DoubleArray strikes = createBlackStrikes();
		for (int i = 0; i < strikes.size(); ++i)
		{
		  Pair<IList<Period>, DoubleMatrix> trimedData = trimData(createBlackMaturities(), createBlackDataMatrixForStrike(i));
		  RawOptionData data = RawOptionData.of(trimedData.First, DoubleArray.of(strikes.get(i)), ValueType.STRIKE, trimedData.Second, ValueType.BLACK_VOLATILITY);
		  IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		  BlackIborCapletFloorletExpiryStrikeVolatilities resVol = (BlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
		  Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> capsAndVols = getCapsBlackVols(i);
		  IList<ResolvedIborCapFloorLeg> caps = capsAndVols.First;
		  IList<double> vols = capsAndVols.Second;
		  int nCaps = caps.Count;
		  for (int j = 0; j < nCaps; ++j)
		  {
			ConstantSurface volSurface = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("test", ACT_ACT_ISDA), vols[j]);
			BlackIborCapletFloorletExpiryStrikeVolatilities constVol = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, CALIBRATION_TIME, volSurface);
			double priceOrg = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, constVol).Amount;
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
	  }

	  public virtual void recovery_test_flat()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, LINEAR);
		DoubleArray strikes = createBlackStrikes();
		RawOptionData data = RawOptionData.of(createBlackMaturities(), strikes, ValueType.STRIKE, createFullFlatBlackDataMatrix(), ValueType.BLACK_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		BlackIborCapletFloorletExpiryStrikeVolatilities resVol = (BlackIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
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
			double priceCalib = LEG_PRICER_BLACK.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
		assertEquals(res.ChiSquare, 0d);
	  }

	  public virtual void recovery_test_normal1()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		DoubleArray strikes = createNormalStrikes();
		RawOptionData data = RawOptionData.of(createNormalMaturities(), strikes, ValueType.STRIKE, createFullNormalDataMatrix(), ValueType.NORMAL_VOLATILITY);
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
		assertEquals(res.ChiSquare, 0d);
		assertEquals(res.ChiSquare, 0d);
		assertEquals(resVol.Index, USD_LIBOR_3M);
		assertEquals(resVol.Name, definition.Name);
		assertEquals(resVol.ValuationDateTime, CALIBRATION_TIME);
		InterpolatedNodalSurface surface = (InterpolatedNodalSurface) resVol.Surface;
		for (int i = 0; i < surface.ParameterCount; ++i)
		{
		  GenericVolatilitySurfacePeriodParameterMetadata metadata = (GenericVolatilitySurfacePeriodParameterMetadata) surface.getParameterMetadata(i);
		  assertEquals(metadata.Strike.Value, surface.YValues.get(i));
		}
	  }

	  public virtual void recovery_test_normal2()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		DoubleArray strikes = createNormalEquivStrikes();
		RawOptionData data = RawOptionData.of(createNormalEquivMaturities(), strikes, ValueType.STRIKE, createFullNormalEquivDataMatrix(), ValueType.NORMAL_VOLATILITY);
		IborCapletFloorletVolatilityCalibrationResult res = CALIBRATOR.calibrate(definition, CALIBRATION_TIME, data, RATES_PROVIDER);
		NormalIborCapletFloorletExpiryStrikeVolatilities resVol = (NormalIborCapletFloorletExpiryStrikeVolatilities) res.Volatilities;
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
			double priceCalib = LEG_PRICER_NORMAL.presentValue(caps[j], RATES_PROVIDER, resVol).Amount;
			assertEquals(priceOrg, priceCalib, Math.Max(priceOrg, 1d) * TOL);
		  }
		}
		assertEquals(res.ChiSquare, 0d);
	  }

	  public virtual void recovery_test_normal2_shift()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition definition = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("test"), USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC, ConstantCurve.of("Black shift", 0.02));
		DoubleArray strikes = createNormalEquivStrikes();
		RawOptionData data = RawOptionData.of(createNormalEquivMaturities(), strikes, ValueType.STRIKE, createFullNormalEquivDataMatrix(), ValueType.NORMAL_VOLATILITY);
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
		assertEquals(res.ChiSquare, 0d);
	  }

	  //-------------------------------------------------------------------------
	  // remove null for one-dimensional bootstrapping
	  private Pair<IList<Period>, DoubleMatrix> trimData(IList<Period> expiries, DoubleMatrix vols)
	  {
		IList<Period> resExpiries = new List<Period>();
		IList<double> resVols = new List<double>();
		int size = vols.size();
		for (int i = 0; i < size; ++i)
		{
		  if (Double.isFinite(vols.get(i, 0)))
		  {
			resExpiries.Add(expiries[i]);
			resVols.Add(vols.get(i, 0));
		  }
		}
		return Pair.of(resExpiries, DoubleMatrix.of(resExpiries.Count, 1, DoubleArray.copyOf(resVols).toArray()));
	  }

	}

}
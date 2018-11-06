using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GenericImpliedVolatiltySolver = com.opengamma.strata.pricer.impl.option.GenericImpliedVolatiltySolver;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Caplet volatilities calibration to cap volatilities based on interpolated surface.
	/// <para>
	/// The caplet volatilities are computed by bootstrapping along the expiry time dimension. 
	/// The result is an interpolated surface spanned by expiry and strike.  
	/// The position of the node points on the resultant surface corresponds to last expiry date of market caps. 
	/// The nodes should be interpolated by a local interpolation scheme along the time direction.  
	/// See <seealso cref="SurfaceIborCapletFloorletVolatilityBootstrapDefinition"/> for detail.
	/// </para>
	/// <para>
	/// If the shift curve is not present in {@code SurfaceIborCapletFloorletBootstrapVolatilityDefinition}, 
	/// the resultant volatility type is the same as the input volatility type, i.e.,
	/// Black caplet volatilities are returned if Black cap volatilities are plugged in, and normal caplet volatilities are
	/// returned otherwise. 
	/// On the other hand, if the shift curve is present in {@code SurfaceIborCapletFloorletBootstrapVolatilityDefinition}, 
	/// Black caplet volatilities are returned for any input volatility type. 
	/// </para>
	/// </summary>
	public class SurfaceIborCapletFloorletVolatilityBootstrapper : IborCapletFloorletVolatilityCalibrator
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SurfaceIborCapletFloorletVolatilityBootstrapper DEFAULT = of(VolatilityIborCapFloorLegPricer.DEFAULT, ReferenceData.standard());

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="pricer">  the cap pricer </param>
	  /// <param name="referenceData">  the reference data </param>
	  /// <returns> the instance </returns>
	  public static SurfaceIborCapletFloorletVolatilityBootstrapper of(VolatilityIborCapFloorLegPricer pricer, ReferenceData referenceData)
	  {

		return new SurfaceIborCapletFloorletVolatilityBootstrapper(pricer, referenceData);
	  }

	  // private constructor
	  private SurfaceIborCapletFloorletVolatilityBootstrapper(VolatilityIborCapFloorLegPricer pricer, ReferenceData referenceData) : base(pricer, referenceData)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override IborCapletFloorletVolatilityCalibrationResult calibrate(IborCapletFloorletVolatilityDefinition definition, ZonedDateTime calibrationDateTime, RawOptionData capFloorData, RatesProvider ratesProvider)
	  {

		ArgChecker.isTrue(ratesProvider.ValuationDate.Equals(calibrationDateTime.toLocalDate()), "valuationDate of ratesProvider should be coherent to calibrationDateTime");
		ArgChecker.isTrue(definition is SurfaceIborCapletFloorletVolatilityBootstrapDefinition, "definition should be SurfaceIborCapletFloorletVolatilityBootstrapDefinition");
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition bsDefinition = (SurfaceIborCapletFloorletVolatilityBootstrapDefinition) definition;
		IborIndex index = bsDefinition.Index;
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate baseDate = index.EffectiveDateOffset.adjust(calibrationDate, ReferenceData);
		LocalDate startDate = baseDate.plus(index.Tenor);
		System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction = this.volatilitiesFunction(bsDefinition, calibrationDateTime, capFloorData);
		SurfaceMetadata metadata = bsDefinition.createMetadata(capFloorData);
		IList<Period> expiries = capFloorData.Expiries;
		int nExpiries = expiries.Count;
		DoubleArray strikes = capFloorData.Strikes;
		DoubleMatrix errorsMatrix = capFloorData.Error.orElse(DoubleMatrix.filled(nExpiries, strikes.size(), 1d));
		IList<double> timeList = new List<double>();
		IList<double> strikeList = new List<double>();
		IList<double> volList = new List<double>();
		IList<ResolvedIborCapFloorLeg> capList = new List<ResolvedIborCapFloorLeg>();
		IList<double> priceList = new List<double>();
		IList<double> errorList = new List<double>();
		int[] startIndex = new int[nExpiries + 1];
		for (int i = 0; i < nExpiries; ++i)
		{
		  LocalDate endDate = baseDate.plus(expiries[i]);
		  DoubleArray volatilityData = capFloorData.Data.row(i);
		  DoubleArray errors = errorsMatrix.row(i);
		  reduceRawData(bsDefinition, ratesProvider, strikes, volatilityData, errors, startDate, endDate, metadata, volatilitiesFunction, timeList, strikeList, volList, capList, priceList, errorList);
		  startIndex[i + 1] = volList.Count;
		  ArgChecker.isTrue(startIndex[i + 1] > startIndex[i], "no valid option data for {}", expiries[i]);
		}
		int nTotal = startIndex[nExpiries];
		IborCapletFloorletVolatilities vols;
		int start;
		ZonedDateTime prevExpiry;
		DoubleArray initialVol = DoubleArray.copyOf(volList);
		if (bsDefinition.ShiftCurve.Present)
		{
		  Curve shiftCurve = bsDefinition.ShiftCurve.get();
		  DoubleArray strikeShifted = DoubleArray.of(nTotal, n => strikeList[n] + shiftCurve.yValue(timeList[n]));
		  if (capFloorData.DataType.Equals(NORMAL_VOLATILITY))
		  { // correct initial surface
			metadata = Surfaces.blackVolatilityByExpiryStrike(bsDefinition.Name.Name, bsDefinition.DayCount).withParameterMetadata(metadata.ParameterMetadata.get());
			initialVol = DoubleArray.of(nTotal, n => volList[n] / (ratesProvider.iborIndexRates(index).rate(capList[n].FinalPeriod.IborRate.Observation) + shiftCurve.yValue(timeList[n])));
		  }
		  InterpolatedNodalSurface surface = InterpolatedNodalSurface.of(metadata, DoubleArray.copyOf(timeList), strikeShifted, initialVol, bsDefinition.Interpolator);
		  vols = ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.of(index, calibrationDateTime, surface, bsDefinition.ShiftCurve.get());
		  start = 0;
		  prevExpiry = calibrationDateTime.minusDays(1L); // included if calibrationDateTime == fixingDateTime
		}
		else
		{
		  InterpolatedNodalSurface surface = InterpolatedNodalSurface.of(metadata, DoubleArray.copyOf(timeList), DoubleArray.copyOf(strikeList), initialVol, bsDefinition.Interpolator);
		  vols = volatilitiesFunction(surface);
		  start = 1;
		  prevExpiry = capList[startIndex[1] - 1].FinalFixingDateTime;
		}
		for (int i = start; i < nExpiries; ++i)
		{
		  for (int j = startIndex[i]; j < startIndex[i + 1]; ++j)
		  {
			System.Func<double, double[]> func = getValueVegaFunction(capList[j], ratesProvider, vols, prevExpiry, j);
			GenericImpliedVolatiltySolver solver = new GenericImpliedVolatiltySolver(func);
			double priceFixed = i == 0 ? 0d : this.priceFixed(capList[j], ratesProvider, vols, prevExpiry);
			double capletVol = solver.impliedVolatility(priceList[j] - priceFixed, initialVol.get(j));
			vols = vols.withParameter(j, capletVol);
		  }
		  prevExpiry = capList[startIndex[i + 1] - 1].FinalFixingDateTime;
		}
		return IborCapletFloorletVolatilityCalibrationResult.ofRootFind(vols);
	  }

	  //-------------------------------------------------------------------------
	  // price and vega function
	  private System.Func<double, double[]> getValueVegaFunction(ResolvedIborCapFloorLeg cap, RatesProvider ratesProvider, IborCapletFloorletVolatilities vols, ZonedDateTime prevExpiry, int nodeIndex)
	  {

		VolatilityIborCapletFloorletPeriodPricer periodPricer = LegPricer.PeriodPricer;
		System.Func<double, double[]> priceAndVegaFunction = (double? x) =>
		{
	IborCapletFloorletVolatilities newVols = vols.withParameter(nodeIndex, x.Value);
	double price = cap.CapletFloorletPeriods.Where(p => p.FixingDateTime.isAfter(prevExpiry)).Select(p => periodPricer.presentValue(p, ratesProvider, newVols).Amount).Sum();
	PointSensitivities point = cap.CapletFloorletPeriods.Where(p => p.FixingDateTime.isAfter(prevExpiry)).Select(p => periodPricer.presentValueSensitivityModelParamsVolatility(p, ratesProvider, newVols)).Aggregate((c1, c2) => c1.combinedWith(c2)).get().build();
	CurrencyParameterSensitivities sensi = newVols.parameterSensitivity(point);
	double vega = sensi.Sensitivities.get(0).Sensitivity.get(nodeIndex);
	return new double[] {price, vega};
		};
		return priceAndVegaFunction;
	  }

	  // sum of caplet prices which are already fixed
	  private double priceFixed(ResolvedIborCapFloorLeg cap, RatesProvider ratesProvider, IborCapletFloorletVolatilities vols, ZonedDateTime prevExpiry)
	  {

		VolatilityIborCapletFloorletPeriodPricer periodPricer = LegPricer.PeriodPricer;
		return cap.CapletFloorletPeriods.Where(p => !p.FixingDateTime.isAfter(prevExpiry)).Select(p => periodPricer.presentValue(p, ratesProvider, vols).Amount).Sum();
	  }

	}

}
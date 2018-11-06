using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.local
{

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using DefaultSurfaceMetadata = com.opengamma.strata.market.surface.DefaultSurfaceMetadata;
	using DeformedSurface = com.opengamma.strata.market.surface.DeformedSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using ScalarFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarFirstOrderDifferentiator;
	using ScalarSecondOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.ScalarSecondOrderDifferentiator;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using VectorFieldSecondOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldSecondOrderDifferentiator;

	/// <summary>
	/// Local volatility computation based on the exact formula.
	/// <para>
	/// Bruno Dupire, "Pricing with a Smile", Risk (1994).
	/// </para>
	/// </summary>
	public class DupireLocalVolatilityCalculator : LocalVolatilityCalculator
	{

	  private const double SMALL = 1.0e-10;
	  private static readonly ScalarFirstOrderDifferentiator FIRST_DERIV = new ScalarFirstOrderDifferentiator();
	  private static readonly ScalarSecondOrderDifferentiator SECOND_DERIV = new ScalarSecondOrderDifferentiator();
	  private static readonly VectorFieldFirstOrderDifferentiator FIRST_DERIV_SENSI = new VectorFieldFirstOrderDifferentiator();
	  private static readonly VectorFieldSecondOrderDifferentiator SECOND_DERIV_SENSI = new VectorFieldSecondOrderDifferentiator();

	  public virtual DeformedSurface localVolatilityFromImpliedVolatility(Surface impliedVolatilitySurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate)
	  {

		System.Func<DoublesPair, ValueDerivatives> func = (DoublesPair x) =>
		{
	double t = x.First;
	double k = x.Second;
	double r = interestRate(t);
	double q = dividendRate(t);
	double vol = impliedVolatilitySurface.zValue(t, k);
	DoubleArray volSensi = impliedVolatilitySurface.zValueParameterSensitivity(t, k).Sensitivity;
	double divT = FIRST_DERIV.differentiate(u => impliedVolatilitySurface.zValue(u, k)).apply(t);
	DoubleArray divTSensi = FIRST_DERIV_SENSI.differentiate(u => impliedVolatilitySurface.zValueParameterSensitivity(u.get(0), k).Sensitivity).apply(DoubleArray.of(t)).column(0);
	double localVol;
	DoubleArray localVolSensi = DoubleArray.of();
	if (k < SMALL)
	{
	  localVol = Math.Sqrt(vol * vol + 2 * vol * t * (divT));
	  localVolSensi = volSensi.multipliedBy((vol + t * divT) / localVol).plus(divTSensi.multipliedBy(vol * t / localVol));
	}
	else
	{
	  double divK = FIRST_DERIV.differentiate(l => impliedVolatilitySurface.zValue(t, l)).apply(k);
	  DoubleArray divKSensi = FIRST_DERIV_SENSI.differentiate(l => impliedVolatilitySurface.zValueParameterSensitivity(t, l.get(0)).Sensitivity).apply(DoubleArray.of(k)).column(0);
	  double divK2 = SECOND_DERIV.differentiate(l => impliedVolatilitySurface.zValue(t, l)).apply(k);
	  DoubleArray divK2Sensi = SECOND_DERIV_SENSI.differentiateNoCross(l => impliedVolatilitySurface.zValueParameterSensitivity(t, l.get(0)).Sensitivity).apply(DoubleArray.of(k)).column(0);
	  double rq = r - q;
	  double h1 = (Math.Log(spot / k) + (rq + 0.5 * vol * vol) * t) / vol;
	  double h2 = h1 - vol * t;
	  double den = 1d + 2d * h1 * k * divK + k * k * (h1 * h2 * divK * divK + t * vol * divK2);
	  double var = (vol * vol + 2d * vol * t * (divT + k * rq * divK)) / den;
	  if (var < 0d)
	  {
		throw new System.ArgumentException("Negative variance");
	  }
	  localVol = Math.Sqrt(var);
	  localVolSensi = volSensi.multipliedBy(localVol * k * h2 * divK * (1d + 0.5 * k * h2 * divK) / vol / den + 0.5 * localVol * Math.Pow(k * h1 * divK, 2) / vol / den + (vol + divT * t + rq * t * k * divK) / (localVol * den) - 0.5 * divK2 * localVol * k * k * t / den).plus(divKSensi.multipliedBy((vol * t * rq * k / localVol - localVol * k * h1 * (1d + k * h2 * divK)) / den)).plus(divTSensi.multipliedBy(vol * t / (localVol * den))).plus(divK2Sensi.multipliedBy(-0.5 * vol * localVol * k * k * t / den));
	}
	return ValueDerivatives.of(localVol, localVolSensi);
		};
		SurfaceMetadata metadata = DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.LOCAL_VOLATILITY).surfaceName(SurfaceName.of("localVol_" + impliedVolatilitySurface.Name)).build();
		return DeformedSurface.of(metadata, impliedVolatilitySurface, func);
	  }

	  public virtual DeformedSurface localVolatilityFromPrice(Surface callPriceSurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate)
	  {

		System.Func<DoublesPair, ValueDerivatives> func = (DoublesPair x) =>
		{
	double t = x.First;
	double k = x.Second;
	double r = interestRate(t);
	double q = dividendRate(t);
	double price = callPriceSurface.zValue(t, k);
	DoubleArray priceSensi = callPriceSurface.zValueParameterSensitivity(t, k).Sensitivity;
	double divT = FIRST_DERIV.differentiate(u => callPriceSurface.zValue(u, k)).apply(t);
	DoubleArray divTSensi = FIRST_DERIV_SENSI.differentiate(u => callPriceSurface.zValueParameterSensitivity(u.get(0), k).Sensitivity).apply(DoubleArray.of(t)).column(0);
	double divK = FIRST_DERIV.differentiate(l => callPriceSurface.zValue(t, l)).apply(k);
	DoubleArray divKSensi = FIRST_DERIV_SENSI.differentiate(l => callPriceSurface.zValueParameterSensitivity(t, l.get(0)).Sensitivity).apply(DoubleArray.of(k)).column(0);
	double divK2 = SECOND_DERIV.differentiate(l => callPriceSurface.zValue(t, l)).apply(k);
	DoubleArray divK2Sensi = SECOND_DERIV_SENSI.differentiateNoCross(l => callPriceSurface.zValueParameterSensitivity(t, l.get(0)).Sensitivity).apply(DoubleArray.of(k)).column(0);
	double var = 2d * (divT + q * price + (r - q) * k * divK) / (k * k * divK2);
	if (var < 0d)
	{
	  throw new System.ArgumentException("Negative variance");
	}
	double localVol = Math.Sqrt(var);
	double factor = 1d / (localVol * k * k * divK2);
	DoubleArray localVolSensi = divTSensi.multipliedBy(factor).plus(divKSensi.multipliedBy((r - q) * k * factor)).plus(priceSensi.multipliedBy(q * factor)).plus(divK2Sensi.multipliedBy(-0.5 * localVol / divK2));
	return ValueDerivatives.of(localVol, localVolSensi);
		};
		SurfaceMetadata metadata = DefaultSurfaceMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.LOCAL_VOLATILITY).surfaceName(SurfaceName.of("localVol_" + callPriceSurface.Name)).build();
		return DeformedSurface.of(metadata, callPriceSurface, func);
	  }

	}

}
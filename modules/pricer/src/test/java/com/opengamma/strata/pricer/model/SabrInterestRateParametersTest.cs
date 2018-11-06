/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;

	/// <summary>
	/// Test <seealso cref="SabrInterestRateParameters"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrInterestRateParametersTest
	public class SabrInterestRateParametersTest
	{

	  private static readonly GridSurfaceInterpolator GRID = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly InterpolatedNodalSurface ALPHA_SURFACE = InterpolatedNodalSurface.of(Surfaces.sabrParameterByExpiryTenor("SabrAlpha", ACT_ACT_ISDA, ValueType.SABR_ALPHA), DoubleArray.of(0, 0, 10, 10), DoubleArray.of(0, 10, 0, 10), DoubleArray.of(0.2, 0.2, 0.2, 0.2), GRID);
	  private static readonly InterpolatedNodalSurface BETA_SURFACE = InterpolatedNodalSurface.of(Surfaces.sabrParameterByExpiryTenor("SabrBeta", ACT_ACT_ISDA, ValueType.SABR_BETA), DoubleArray.of(0, 0, 10, 10), DoubleArray.of(0, 10, 0, 10), DoubleArray.of(1, 1, 1, 1), GRID);
	  private static readonly InterpolatedNodalSurface RHO_SURFACE = InterpolatedNodalSurface.of(Surfaces.sabrParameterByExpiryTenor("SabrRho", ACT_ACT_ISDA, ValueType.SABR_RHO), DoubleArray.of(0, 0, 10, 10), DoubleArray.of(0, 10, 0, 10), DoubleArray.of(-0.5, -0.5, -0.5, -0.5), GRID);
	  private static readonly InterpolatedNodalSurface NU_SURFACE = InterpolatedNodalSurface.of(Surfaces.sabrParameterByExpiryTenor("SabrNu", ACT_ACT_ISDA, ValueType.SABR_NU), DoubleArray.of(0, 0, 10, 10), DoubleArray.of(0, 10, 0, 10), DoubleArray.of(0.5, 0.5, 0.5, 0.5), GRID);
	  private static readonly SabrVolatilityFormula FORMULA = SabrVolatilityFormula.hagan();
	  private static readonly SabrInterestRateParameters PARAMETERS = SabrInterestRateParameters.of(ALPHA_SURFACE, BETA_SURFACE, RHO_SURFACE, NU_SURFACE, FORMULA);

	  public virtual void hashEqualGetter()
	  {
		assertEquals(PARAMETERS.AlphaSurface, ALPHA_SURFACE);
		assertEquals(PARAMETERS.BetaSurface, BETA_SURFACE);
		assertEquals(PARAMETERS.RhoSurface, RHO_SURFACE);
		assertEquals(PARAMETERS.NuSurface, NU_SURFACE);
		assertEquals(PARAMETERS.SabrVolatilityFormula, FORMULA);
		assertEquals(PARAMETERS.ShiftSurface.Name, SurfaceName.of("Zero shift"));
		double expiry = 2.0;
		double tenor = 3.0;
		double alpha = ALPHA_SURFACE.zValue(expiry, tenor);
		double beta = BETA_SURFACE.zValue(expiry, tenor);
		double rho = RHO_SURFACE.zValue(expiry, tenor);
		double nu = NU_SURFACE.zValue(expiry, tenor);
		assertEquals(PARAMETERS.alpha(expiry, tenor), alpha);
		assertEquals(PARAMETERS.beta(expiry, tenor), beta);
		assertEquals(PARAMETERS.rho(expiry, tenor), rho);
		assertEquals(PARAMETERS.nu(expiry, tenor), nu);
		double strike = 1.1;
		double forward = 1.05;
		assertEquals(PARAMETERS.volatility(expiry, tenor, strike, forward), FORMULA.volatility(forward, strike, expiry, alpha, beta, rho, nu));
		double[] adjCmp = PARAMETERS.volatilityAdjoint(expiry, tenor, strike, forward).Derivatives.toArray();
		double[] adjExp = FORMULA.volatilityAdjoint(forward, strike, expiry, alpha, beta, rho, nu).Derivatives.toArray();
		for (int i = 0; i < 6; ++i)
		{
		  assertEquals(adjCmp[i], adjExp[i]);
		}
		SabrInterestRateParameters other = SabrInterestRateParameters.of(ALPHA_SURFACE, BETA_SURFACE, RHO_SURFACE, NU_SURFACE, FORMULA);
		assertEquals(PARAMETERS, other);
		assertEquals(PARAMETERS.GetHashCode(), other.GetHashCode());
	  }

	  public virtual void negativeRates()
	  {
		double shift = 0.05;
		Surface surface = ConstantSurface.of("shfit", shift);
		SabrInterestRateParameters @params = SabrInterestRateParameters.of(ALPHA_SURFACE, BETA_SURFACE, RHO_SURFACE, NU_SURFACE, surface, FORMULA);
		double expiry = 2.0;
		double tenor = 3.0;
		assertEquals(@params.alpha(expiry, tenor), ALPHA_SURFACE.zValue(expiry, tenor));
		assertEquals(@params.beta(expiry, tenor), BETA_SURFACE.zValue(expiry, tenor));
		assertEquals(@params.rho(expiry, tenor), RHO_SURFACE.zValue(expiry, tenor));
		assertEquals(@params.nu(expiry, tenor), NU_SURFACE.zValue(expiry, tenor));
		double strike = -0.02;
		double forward = 0.015;
		double alpha = ALPHA_SURFACE.zValue(expiry, tenor);
		double beta = BETA_SURFACE.zValue(expiry, tenor);
		double rho = RHO_SURFACE.zValue(expiry, tenor);
		double nu = NU_SURFACE.zValue(expiry, tenor);
		assertEquals(@params.volatility(expiry, tenor, strike, forward), FORMULA.volatility(forward + shift, strike + shift, expiry, alpha, beta, rho, nu));
		double[] adjCmp = @params.volatilityAdjoint(expiry, tenor, strike, forward).Derivatives.toArray();
		double[] adjExp = FORMULA.volatilityAdjoint(forward + shift, strike + shift, expiry, alpha, beta, rho, nu).Derivatives.toArray();
		for (int i = 0; i < 4; ++i)
		{
		  assertEquals(adjCmp[i], adjExp[i]);
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(PARAMETERS);
	  }

	}

}
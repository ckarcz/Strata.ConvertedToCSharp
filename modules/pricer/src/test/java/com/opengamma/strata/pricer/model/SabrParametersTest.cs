/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="SabrParameters"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrParametersTest
	public class SabrParametersTest
	{

	  private static readonly InterpolatedNodalCurve ALPHA_CURVE = InterpolatedNodalCurve.of(Curves.sabrParameterByExpiry("SabrAlpha", ACT_ACT_ISDA, ValueType.SABR_ALPHA), DoubleArray.of(0, 10), DoubleArray.of(0.2, 0.2), LINEAR);
	  private static readonly InterpolatedNodalCurve BETA_CURVE = InterpolatedNodalCurve.of(Curves.sabrParameterByExpiry("SabrBeta", ACT_ACT_ISDA, ValueType.SABR_BETA), DoubleArray.of(0, 10), DoubleArray.of(1, 1), LINEAR);
	  private static readonly InterpolatedNodalCurve RHO_CURVE = InterpolatedNodalCurve.of(Curves.sabrParameterByExpiry("SabrRho", ACT_ACT_ISDA, ValueType.SABR_RHO), DoubleArray.of(0, 10), DoubleArray.of(-0.5, -0.5), LINEAR);
	  private static readonly InterpolatedNodalCurve NU_CURVE = InterpolatedNodalCurve.of(Curves.sabrParameterByExpiry("SabrNu", ACT_ACT_ISDA, ValueType.SABR_NU), DoubleArray.of(0, 10), DoubleArray.of(0.5, 0.5), LINEAR);
	  private static readonly SabrVolatilityFormula FORMULA = SabrVolatilityFormula.hagan();
	  private static readonly SabrParameters PARAMETERS = SabrParameters.of(ALPHA_CURVE, BETA_CURVE, RHO_CURVE, NU_CURVE, FORMULA);

	  public virtual void getter()
	  {
		assertEquals(PARAMETERS.AlphaCurve, ALPHA_CURVE);
		assertEquals(PARAMETERS.BetaCurve, BETA_CURVE);
		assertEquals(PARAMETERS.RhoCurve, RHO_CURVE);
		assertEquals(PARAMETERS.NuCurve, NU_CURVE);
		assertEquals(PARAMETERS.SabrVolatilityFormula, FORMULA);
		assertEquals(PARAMETERS.ShiftCurve.Name, CurveName.of("Zero shift"));
		assertEquals(PARAMETERS.DayCount, ACT_ACT_ISDA);
		assertEquals(PARAMETERS.ParameterCount, 9);
		double expiry = 2.0;
		double alpha = ALPHA_CURVE.yValue(expiry);
		double beta = BETA_CURVE.yValue(expiry);
		double rho = RHO_CURVE.yValue(expiry);
		double nu = NU_CURVE.yValue(expiry);
		assertEquals(PARAMETERS.alpha(expiry), alpha);
		assertEquals(PARAMETERS.beta(expiry), beta);
		assertEquals(PARAMETERS.rho(expiry), rho);
		assertEquals(PARAMETERS.nu(expiry), nu);
		double strike = 1.1;
		double forward = 1.05;
		assertEquals(PARAMETERS.volatility(expiry, strike, forward), FORMULA.volatility(forward, strike, expiry, alpha, beta, rho, nu));
		double[] adjCmp = PARAMETERS.volatilityAdjoint(expiry, strike, forward).Derivatives.toArray();
		double[] adjExp = FORMULA.volatilityAdjoint(forward, strike, expiry, alpha, beta, rho, nu).Derivatives.toArray();
		for (int i = 0; i < 6; ++i)
		{
		  assertEquals(adjCmp[i], adjExp[i]);
		}
		for (int i = 0; i < 9; ++i)
		{
		  if (i < 2)
		  {
			assertEquals(PARAMETERS.getParameterMetadata(i), ALPHA_CURVE.getParameterMetadata(i));
			assertEquals(PARAMETERS.getParameter(i), ALPHA_CURVE.getParameter(i));
		  }
		  else if (i < 4)
		  {
			assertEquals(PARAMETERS.getParameterMetadata(i), BETA_CURVE.getParameterMetadata(i - 2));
			assertEquals(PARAMETERS.getParameter(i), BETA_CURVE.getParameter(i - 2));
		  }
		  else if (i < 6)
		  {
			assertEquals(PARAMETERS.getParameterMetadata(i), RHO_CURVE.getParameterMetadata(i - 4));
			assertEquals(PARAMETERS.getParameter(i), RHO_CURVE.getParameter(i - 4));
		  }
		  else if (i < 8)
		  {
			assertEquals(PARAMETERS.getParameterMetadata(i), NU_CURVE.getParameterMetadata(i - 6));
			assertEquals(PARAMETERS.getParameter(i), NU_CURVE.getParameter(i - 6));
		  }
		  else
		  {
			assertEquals(PARAMETERS.getParameterMetadata(i), ParameterMetadata.empty());
			assertEquals(PARAMETERS.getParameter(i), 0d);
		  }
		}
	  }

	  public virtual void negativeRates()
	  {
		double shift = 0.05;
		Curve surface = ConstantCurve.of("shfit", shift);
		SabrParameters @params = SabrParameters.of(ALPHA_CURVE, BETA_CURVE, RHO_CURVE, NU_CURVE, surface, FORMULA);
		double expiry = 2.0;
		assertEquals(@params.alpha(expiry), ALPHA_CURVE.yValue(expiry));
		assertEquals(@params.beta(expiry), BETA_CURVE.yValue(expiry));
		assertEquals(@params.rho(expiry), RHO_CURVE.yValue(expiry));
		assertEquals(@params.nu(expiry), NU_CURVE.yValue(expiry));
		double strike = -0.02;
		double forward = 0.015;
		double alpha = ALPHA_CURVE.yValue(expiry);
		double beta = BETA_CURVE.yValue(expiry);
		double rho = RHO_CURVE.yValue(expiry);
		double nu = NU_CURVE.yValue(expiry);
		assertEquals(@params.volatility(expiry, strike, forward), FORMULA.volatility(forward + shift, strike + shift, expiry, alpha, beta, rho, nu));
		double[] adjCmp = @params.volatilityAdjoint(expiry, strike, forward).Derivatives.toArray();
		double[] adjExp = FORMULA.volatilityAdjoint(forward + shift, strike + shift, expiry, alpha, beta, rho, nu).Derivatives.toArray();
		for (int i = 0; i < 4; ++i)
		{
		  assertEquals(adjCmp[i], adjExp[i]);
		}
	  }

	  public virtual void perturbation()
	  {
		SabrParameters test = PARAMETERS.withPerturbation((i, v, m) => (2d + i) * v);
		SabrParameters expected = PARAMETERS;
		for (int i = 0; i < PARAMETERS.ParameterCount; ++i)
		{
		  expected = expected.withParameter(i, (2d + i) * expected.getParameter(i));
		}
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(PARAMETERS);
	  }

	}

}
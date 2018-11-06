using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.PUT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;

	/// <summary>
	/// Test <seealso cref="NormalPriceFunction"/>,
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalPriceFunctionTest
	public class NormalPriceFunctionTest
	{

	  private const double T = 4.5;
	  private const double F = 104;
	  private const double DELTA = 10;
	  private static readonly EuropeanVanillaOption ITM_CALL = EuropeanVanillaOption.of(F - DELTA, T, CALL);
	  private static readonly EuropeanVanillaOption OTM_CALL = EuropeanVanillaOption.of(F + DELTA, T, CALL);
	  private static readonly EuropeanVanillaOption ITM_PUT = EuropeanVanillaOption.of(F + DELTA, T, PUT);
	  private static readonly EuropeanVanillaOption OTM_PUT = EuropeanVanillaOption.of(F - DELTA, T, PUT);
	  private const double DF = 0.9;
	  private const double SIGMA = 20.0;
	  private static readonly NormalFunctionData VOL_DATA = NormalFunctionData.of(F, DF, SIGMA);
	  private static readonly NormalFunctionData ZERO_VOL_DATA = NormalFunctionData.of(F, DF, 0);
	  private static readonly NormalPriceFunction FUNCTION = new NormalPriceFunction();

	  public virtual void testInvalid()
	  {
		assertThrowsIllegalArg(() => FUNCTION.getPriceFunction(null));
		assertThrowsIllegalArg(() => FUNCTION.getPriceFunction(ITM_CALL).apply((NormalFunctionData) null));
	  }

	  public virtual void testZeroVolPrice()
	  {
		assertEquals(FUNCTION.getPriceFunction(ITM_CALL).apply(ZERO_VOL_DATA), DF * DELTA, 1e-15);
		assertEquals(FUNCTION.getPriceFunction(OTM_CALL).apply(ZERO_VOL_DATA), 0, 1e-15);
		assertEquals(FUNCTION.getPriceFunction(ITM_PUT).apply(ZERO_VOL_DATA), DF * DELTA, 1e-15);
		assertEquals(FUNCTION.getPriceFunction(OTM_PUT).apply(ZERO_VOL_DATA), 0, 1e-15);
	  }

	  public virtual void testPriceAdjoint()
	  {
		// Price
		double price = FUNCTION.getPriceFunction(ITM_CALL).apply(VOL_DATA);
		ValueDerivatives priceAdjoint = FUNCTION.getPriceAdjoint(ITM_CALL, VOL_DATA);
		assertEquals(priceAdjoint.Value, price, 1E-10);
		// Price with 0 volatility
		double price0 = FUNCTION.getPriceFunction(ITM_CALL).apply(ZERO_VOL_DATA);
		ValueDerivatives price0Adjoint = FUNCTION.getPriceAdjoint(ITM_CALL, ZERO_VOL_DATA);
		assertEquals(price0Adjoint.Value, price0, 1E-10);
		// Derivative forward.
		double deltaF = 0.01;
		NormalFunctionData dataFP = NormalFunctionData.of(F + deltaF, DF, SIGMA);
		NormalFunctionData dataFM = NormalFunctionData.of(F - deltaF, DF, SIGMA);
		double priceFP = FUNCTION.getPriceFunction(ITM_CALL).apply(dataFP);
		double priceFM = FUNCTION.getPriceFunction(ITM_CALL).apply(dataFM);
		double derivativeF_FD = (priceFP - priceFM) / (2 * deltaF);
		assertEquals(priceAdjoint.getDerivative(0), derivativeF_FD, 1E-7);
		// Derivative strike.
		double deltaK = 0.01;
		EuropeanVanillaOption optionKP = EuropeanVanillaOption.of(F - DELTA + deltaK, T, CALL);
		EuropeanVanillaOption optionKM = EuropeanVanillaOption.of(F - DELTA - deltaK, T, CALL);
		double priceKP = FUNCTION.getPriceFunction(optionKP).apply(VOL_DATA);
		double priceKM = FUNCTION.getPriceFunction(optionKM).apply(VOL_DATA);
		double derivativeK_FD = (priceKP - priceKM) / (2 * deltaK);
		assertEquals(priceAdjoint.getDerivative(2), derivativeK_FD, 1E-7);
		// Derivative volatility.
		double deltaV = 0.0001;
		NormalFunctionData dataVP = NormalFunctionData.of(F, DF, SIGMA + deltaV);
		NormalFunctionData dataVM = NormalFunctionData.of(F, DF, SIGMA - deltaV);
		double priceVP = FUNCTION.getPriceFunction(ITM_CALL).apply(dataVP);
		double priceVM = FUNCTION.getPriceFunction(ITM_CALL).apply(dataVM);
		double derivativeV_FD = (priceVP - priceVM) / (2 * deltaV);
		assertEquals(priceAdjoint.getDerivative(1), derivativeV_FD, 1E-6);
	  }

	  private static readonly EuropeanVanillaOption ATM_CALL = EuropeanVanillaOption.of(F, T, CALL);
	  private static readonly EuropeanVanillaOption ATM_PUT = EuropeanVanillaOption.of(F, T, PUT);

	  // Test getDelta, getGamma and getVega
	  public virtual void greeksTest()
	  {
		double tol = 1.0e-12;
		double eps = 1.0e-5;
		EuropeanVanillaOption[] options = new EuropeanVanillaOption[] {ITM_CALL, ITM_PUT, OTM_CALL, OTM_PUT, ATM_CALL, ATM_PUT};
		foreach (EuropeanVanillaOption option in options)
		{
		  // consistency with getPriceFunction for first order derivatives
		  ValueDerivatives price = FUNCTION.getPriceAdjoint(option, VOL_DATA);
		  double delta = FUNCTION.getDelta(option, VOL_DATA);
		  double vega = FUNCTION.getVega(option, VOL_DATA);
		  assertEquals(price.getDerivative(0), delta, tol);
		  assertEquals(price.getDerivative(1), vega, tol);

		  // testing second order derivative against finite difference approximation
		  NormalFunctionData dataUp = NormalFunctionData.of(F + eps, DF, SIGMA);
		  NormalFunctionData dataDw = NormalFunctionData.of(F - eps, DF, SIGMA);
		  double deltaUp = FUNCTION.getDelta(option, dataUp);
		  double deltaDw = FUNCTION.getDelta(option, dataDw);
		  double @ref = 0.5 * (deltaUp - deltaDw) / eps;
		  double gamma = FUNCTION.getGamma(option, VOL_DATA);
		  assertEquals(gamma, @ref, eps);

		  EuropeanVanillaOption optionUp = EuropeanVanillaOption.of(option.Strike, T + eps, option.PutCall);
		  EuropeanVanillaOption optionDw = EuropeanVanillaOption.of(option.Strike, T - eps, option.PutCall);
		  double priceTimeUp = FUNCTION.getPriceFunction(optionUp).apply(VOL_DATA);
		  double priceTimeDw = FUNCTION.getPriceFunction(optionDw).apply(VOL_DATA);
		  @ref = -0.5 * (priceTimeUp - priceTimeDw) / eps;
		  double theta = FUNCTION.getTheta(option, VOL_DATA);
		  assertEquals(theta, @ref, eps);
		}
	  }

	  // Testing the branch for sigmaRootT < 1e-16
	  public virtual void smallParameterGreeksTest()
	  {
		double eps = 1.0e-5;
		NormalFunctionData dataVolUp = NormalFunctionData.of(F, DF, eps);
		NormalFunctionData dataFwUp = NormalFunctionData.of(F + eps, DF, 0.0);
		NormalFunctionData dataFwDw = NormalFunctionData.of(F - eps, DF, 0.0);

		EuropeanVanillaOption[] options = new EuropeanVanillaOption[] {ITM_CALL, ITM_PUT, OTM_CALL, OTM_PUT, ATM_CALL, ATM_PUT};
		foreach (EuropeanVanillaOption option in options)
		{
		  double delta = FUNCTION.getDelta(option, ZERO_VOL_DATA);
		  double priceUp = FUNCTION.getPriceFunction(option).apply(dataFwUp);
		  double priceDw = FUNCTION.getPriceFunction(option).apply(dataFwDw);
		  double refDelta = 0.5 * (priceUp - priceDw) / eps;
		  assertEquals(delta, refDelta, eps);

		  double vega = FUNCTION.getVega(option, ZERO_VOL_DATA);
		  double priceVolUp = FUNCTION.getPriceFunction(option).apply(dataVolUp);
		  double price = FUNCTION.getPriceFunction(option).apply(ZERO_VOL_DATA);
		  double refVega = (priceVolUp - price) / eps;
		  assertEquals(vega, refVega, eps);

		  double gamma = FUNCTION.getGamma(option, ZERO_VOL_DATA);
		  double deltaUp = FUNCTION.getDelta(option, dataFwUp);
		  double deltaDw = FUNCTION.getDelta(option, dataFwDw);
		  double refGamma = 0.5 * (deltaUp - deltaDw) / eps;
		  if (Math.Abs(refGamma) > 0.1 / eps)
		  { // infinity handled
			assertTrue(double.IsInfinity(gamma));
		  }
		  else
		  {
			assertEquals(gamma, refGamma, eps);
		  }

		  EuropeanVanillaOption optionUp = EuropeanVanillaOption.of(option.Strike, T + eps, option.PutCall);
		  EuropeanVanillaOption optionDw = EuropeanVanillaOption.of(option.Strike, T - eps, option.PutCall);
		  double priceTimeUp = FUNCTION.getPriceFunction(optionUp).apply(ZERO_VOL_DATA);
		  double priceTimeDw = FUNCTION.getPriceFunction(optionDw).apply(ZERO_VOL_DATA);
		  double refTheta = -0.5 * (priceTimeUp - priceTimeDw) / eps;
		  double theta = FUNCTION.getTheta(option, ZERO_VOL_DATA);
		  assertEquals(theta, refTheta, eps);
		}
	  }

	}

}
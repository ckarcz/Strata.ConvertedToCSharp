/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="NormalFormulaRepository"/> implied volatility.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalFormulaRepositoryImpliedVolatilityTest
	public class NormalFormulaRepositoryImpliedVolatilityTest
	{

	  private const double FORWARD = 100.0;
	  private const double DF = 0.87;
	  private const double T = 4.5;
	  private static readonly NormalFunctionData[] DATA;
	  private const int N = 10;
	  private static readonly double[] PRICES;
	  private static readonly double[] STRIKES = new double[N];
	  private static readonly double[] STRIKES_ATM = new double[N];
	  private static readonly EuropeanVanillaOption[] OPTIONS = new EuropeanVanillaOption[N];
	  private static readonly double[] SIGMA;
	  private static readonly double[] SIGMA_BLACK = new double[N];
	  private static readonly NormalPriceFunction FUNCTION = new NormalPriceFunction();

	  static NormalFormulaRepositoryImpliedVolatilityTest()
	  {
		PRICES = new double[N];
		SIGMA = new double[N];
		DATA = new NormalFunctionData[N];
		for (int i = 0; i < N; i++)
		{
		  STRIKES[i] = FORWARD + (-N / 2 + i) * 10;
		  STRIKES_ATM[i] = FORWARD + (-0.5d * N + i) / 100.0d;
		  SIGMA[i] = FORWARD * (0.05 + 4.0 * i / 100.0);
		  SIGMA_BLACK[i] = 0.20 + i / 100.0d;
		  DATA[i] = NormalFunctionData.of(FORWARD, DF, SIGMA[i]);
		  OPTIONS[i] = EuropeanVanillaOption.of(STRIKES[i], T, PutCall.CALL);
		  PRICES[i] = FUNCTION.getPriceFunction(OPTIONS[i]).apply(DATA[i]);
		}
	  }
	  private const double TOLERANCE_PRICE = 1.0E-4;
	  private const double TOLERANCE_VOL = 1.0E-6;

	  public virtual void implied_volatility()
	  {
		double[] impliedVolatility = new double[N];
		for (int i = 0; i < N; i++)
		{
		  impliedVolatility[i] = this.impliedVolatility(DATA[i], OPTIONS[i], PRICES[i]);
		  assertEquals(impliedVolatility[i], SIGMA[i], 1e-6);
		}
	  }

	  public virtual void intrinsic_price()
	  {
		NormalFunctionData data = NormalFunctionData.of(1.0, 1.0, 0.01);
		EuropeanVanillaOption option1 = EuropeanVanillaOption.of(0.5, 1.0, PutCall.CALL);
		assertThrowsIllegalArg(() => impliedVolatility(data, option1, 1e-6));
		EuropeanVanillaOption option2 = EuropeanVanillaOption.of(1.5, 1.0, PutCall.PUT);
		assertThrowsIllegalArg(() => impliedVolatility(data, option2, 1e-6));
	  }

	  private double impliedVolatility(NormalFunctionData data, EuropeanVanillaOption option, double price)
	  {

		return NormalFormulaRepository.impliedVolatility(price, data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, data.Numeraire, option.PutCall);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrong_strike()
	  public virtual void wrong_strike()
	  {
		NormalFormulaRepository.impliedVolatilityFromBlackApproximated(FORWARD, -1.0d, T, 0.20d);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void wrong_forward()
	  public virtual void wrong_forward()
	  {
		NormalFormulaRepository.impliedVolatilityFromBlackApproximated(-1.0d, FORWARD, T, 0.20d);
	  }

	  public virtual void price_comparison()
	  {
		priceCheck(STRIKES);
		priceCheck(STRIKES_ATM);
	  }

	  private void priceCheck(double[] strikes)
	  {
		for (int i = 0; i < N; i++)
		{
		  double ivNormalComputed = NormalFormulaRepository.impliedVolatilityFromBlackApproximated(FORWARD, strikes[i], T, SIGMA_BLACK[i]);
		  double priceNormalComputed = NormalFormulaRepository.price(FORWARD, strikes[i], T, ivNormalComputed, PutCall.CALL) * DF;
		  double priceBlack = BlackFormulaRepository.price(FORWARD, strikes[i], T, SIGMA_BLACK[i], true) * DF;
		  assertEquals(priceNormalComputed, priceBlack, TOLERANCE_PRICE);
		}
	  }

	  public virtual void implied_volatility_adjoint()
	  {
		double shiftFd = 1.0E-6;
		for (int i = 0; i < N; i++)
		{
		  double impliedVol = NormalFormulaRepository.impliedVolatilityFromBlackApproximated(FORWARD, STRIKES[i], T, SIGMA_BLACK[i]);
		  ValueDerivatives impliedVolAdj = NormalFormulaRepository.impliedVolatilityFromBlackApproximatedAdjoint(FORWARD, STRIKES[i], T, SIGMA_BLACK[i]);
		  assertEquals(impliedVolAdj.Value, impliedVol, TOLERANCE_VOL);
		  double impliedVolP = NormalFormulaRepository.impliedVolatilityFromBlackApproximated(FORWARD, STRIKES[i], T, SIGMA_BLACK[i] + shiftFd);
		  double impliedVolM = NormalFormulaRepository.impliedVolatilityFromBlackApproximated(FORWARD, STRIKES[i], T, SIGMA_BLACK[i] - shiftFd);
		  double derivativeApproximated = (impliedVolP - impliedVolM) / (2 * shiftFd);
		  assertEquals(impliedVolAdj.Derivatives.size(), 1);
		  assertEquals(impliedVolAdj.getDerivative(0), derivativeApproximated, TOLERANCE_VOL);
		}
	  }

	}

}
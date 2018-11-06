using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using SabrFormulaData = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData;
	using SabrHaganVolatilityFunctionProvider = com.opengamma.strata.pricer.impl.volatility.smile.SabrHaganVolatilityFunctionProvider;
	using VolatilityFunctionProvider = com.opengamma.strata.pricer.impl.volatility.smile.VolatilityFunctionProvider;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="SabrExtrapolationRightFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrExtrapolationRightFunctionTest
	public class SabrExtrapolationRightFunctionTest
	{

	  private const double NU = 0.50;
	  private const double RHO = -0.25;
	  private const double BETA = 0.50;
	  private const double ALPHA = 0.05;
	  private const double FORWARD = 0.05;
	  private static readonly SabrFormulaData SABR_DATA = SabrFormulaData.of(ALPHA, BETA, RHO, NU);
	  private const double CUT_OFF_STRIKE = 0.10; // Set low for the test
	  private const double MU = 4.0;
	  private const double TIME_TO_EXPIRY = 2.0;
	  private static readonly SabrExtrapolationRightFunction SABR_EXTRAPOLATION = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, SABR_DATA, CUT_OFF_STRIKE, MU);
	  private static readonly SabrHaganVolatilityFunctionProvider SABR_FUNCTION = SabrHaganVolatilityFunctionProvider.DEFAULT;
	  private const double TOLERANCE_PRICE = 1.0E-10;

	  /// <summary>
	  /// Tests getter.
	  /// </summary>
	  public virtual void getter()
	  {
		SabrExtrapolationRightFunction func = SabrExtrapolationRightFunction.of(FORWARD, SABR_DATA, CUT_OFF_STRIKE, TIME_TO_EXPIRY, MU, SabrHaganVolatilityFunctionProvider.DEFAULT);
		assertEquals(func.CutOffStrike, CUT_OFF_STRIKE);
		assertEquals(func.Mu, MU);
		assertEquals(func.SabrData, SABR_DATA);
		assertEquals(func.TimeToExpiry, TIME_TO_EXPIRY);
	  }

	  /// <summary>
	  /// Tests the price for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void price()
	  {
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		double volatilityIn = SABR_FUNCTION.volatility(FORWARD, strikeIn, TIME_TO_EXPIRY, SABR_DATA);
		double priceExpectedIn = BlackFormulaRepository.price(FORWARD, strikeIn, TIME_TO_EXPIRY, volatilityIn, true);
		double priceIn = SABR_EXTRAPOLATION.price(strikeIn, PutCall.CALL);
		assertEquals(priceExpectedIn, priceIn, TOLERANCE_PRICE);
		double volatilityAt = SABR_FUNCTION.volatility(FORWARD, strikeAt, TIME_TO_EXPIRY, SABR_DATA);
		double priceExpectedAt = BlackFormulaRepository.price(FORWARD, strikeAt, TIME_TO_EXPIRY, volatilityAt, true);
		double priceAt = SABR_EXTRAPOLATION.price(strikeAt, PutCall.CALL);
		assertEquals(priceExpectedAt, priceAt, TOLERANCE_PRICE);
		double priceOut = SABR_EXTRAPOLATION.price(strikeOut, PutCall.CALL);
		double priceExpectedOut = 5.427104E-5; // From previous run
		assertEquals(priceExpectedOut, priceOut, TOLERANCE_PRICE);
	  }

	  /// <summary>
	  /// Tests the price for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceCloseToExpiry()
	  {
		double[] timeToExpiry = new double[] {1.0 / 365, 0.0}; // One day and on expiry day.
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		for (int loopexp = 0; loopexp < timeToExpiry.Length; loopexp++)
		{
		  SabrExtrapolationRightFunction sabrExtra = SabrExtrapolationRightFunction.of(FORWARD, timeToExpiry[loopexp], SABR_DATA, CUT_OFF_STRIKE, MU);
		  double volatilityIn = SABR_FUNCTION.volatility(FORWARD, strikeIn, timeToExpiry[loopexp], SABR_DATA);
		  double priceExpectedIn = BlackFormulaRepository.price(FORWARD, strikeIn, timeToExpiry[loopexp], volatilityIn, true);
		  double priceIn = sabrExtra.price(strikeIn, PutCall.CALL);
		  assertEquals(priceExpectedIn, priceIn, TOLERANCE_PRICE);
		  double volatilityAt = SABR_FUNCTION.volatility(FORWARD, strikeAt, timeToExpiry[loopexp], SABR_DATA);
		  double priceExpectedAt = BlackFormulaRepository.price(FORWARD, strikeAt, timeToExpiry[loopexp], volatilityAt, true);
		  double priceAt = sabrExtra.price(strikeAt, PutCall.CALL);
		  assertEquals(priceExpectedAt, priceAt, TOLERANCE_PRICE);
		  double priceOut = sabrExtra.price(strikeOut, PutCall.CALL);
		  double priceExpectedOut = 0.0; // From previous run
		  assertEquals(priceExpectedOut, priceOut, TOLERANCE_PRICE);
		}
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceDerivativeForwardCall()
	  {
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		EuropeanVanillaOption optionIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.CALL);
		double shiftF = 0.000001;
		SabrFormulaData sabrDataFP = SabrFormulaData.of(ALPHA, BETA, RHO, NU);
		SabrExtrapolationRightFunction sabrExtrapolationFP = SabrExtrapolationRightFunction.of(FORWARD + shiftF, TIME_TO_EXPIRY, sabrDataFP, CUT_OFF_STRIKE, MU);
		// Below cut-off strike
		double priceIn = SABR_EXTRAPOLATION.price(optionIn.Strike, optionIn.PutCall);
		double priceInFP = sabrExtrapolationFP.price(optionIn.Strike, optionIn.PutCall);
		double priceInDF = SABR_EXTRAPOLATION.priceDerivativeForward(optionIn.Strike, optionIn.PutCall);
		double priceInDFExpected = (priceInFP - priceIn) / shiftF;
		assertEquals(priceInDFExpected, priceInDF, 1E-5);
		// At cut-off strike
		double priceAt = SABR_EXTRAPOLATION.price(optionAt.Strike, optionAt.PutCall);
		double priceAtFP = sabrExtrapolationFP.price(optionAt.Strike, optionAt.PutCall);
		double priceAtDF = SABR_EXTRAPOLATION.priceDerivativeForward(optionAt.Strike, optionAt.PutCall);
		double priceAtDFExpected = (priceAtFP - priceAt) / shiftF;
		assertEquals(priceAtDFExpected, priceAtDF, 1E-6);
		// Above cut-off strike
		double[] abc = SABR_EXTRAPOLATION.Parameter;
		double[] abcDF = SABR_EXTRAPOLATION.ParameterDerivativeForward;
		double[] abcFP = sabrExtrapolationFP.Parameter;
		double[] abcDFExpected = new double[3];
		for (int loopparam = 0; loopparam < 3; loopparam++)
		{
		  abcDFExpected[loopparam] = (abcFP[loopparam] - abc[loopparam]) / shiftF;
		  assertEquals(1.0, abcDFExpected[loopparam] / abcDF[loopparam], 5E-2);
		}
		double priceOut = SABR_EXTRAPOLATION.price(optionOut.Strike, optionOut.PutCall);
		double priceOutFP = sabrExtrapolationFP.price(optionOut.Strike, optionOut.PutCall);
		double priceOutDF = SABR_EXTRAPOLATION.priceDerivativeForward(optionOut.Strike, optionOut.PutCall);
		double priceOutDFExpected = (priceOutFP - priceOut) / shiftF;
		assertEquals(priceOutDFExpected, priceOutDF, 1E-5);
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceDerivativeForwardPut()
	  {
		SabrExtrapolationRightFunction func = SabrExtrapolationRightFunction.of(FORWARD, SABR_DATA, CUT_OFF_STRIKE, TIME_TO_EXPIRY, MU, SabrHaganVolatilityFunctionProvider.DEFAULT);
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		EuropeanVanillaOption optionIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.PUT);
		double shiftF = 0.000001;
		SabrFormulaData sabrDataFP = SabrFormulaData.of(ALPHA, BETA, RHO, NU);
		SabrExtrapolationRightFunction sabrExtrapolationFP = SabrExtrapolationRightFunction.of(FORWARD + shiftF, TIME_TO_EXPIRY, sabrDataFP, CUT_OFF_STRIKE, MU);
		// Below cut-off strike
		double priceIn = func.price(optionIn.Strike, optionIn.PutCall);
		double priceInFP = sabrExtrapolationFP.price(optionIn.Strike, optionIn.PutCall);
		double priceInDF = func.priceDerivativeForward(optionIn.Strike, optionIn.PutCall);
		double priceInDFExpected = (priceInFP - priceIn) / shiftF;
		assertEquals(priceInDFExpected, priceInDF, 1E-5);
		// At cut-off strike
		double priceAt = func.price(optionAt.Strike, optionAt.PutCall);
		double priceAtFP = sabrExtrapolationFP.price(optionAt.Strike, optionAt.PutCall);
		double priceAtDF = func.priceDerivativeForward(optionAt.Strike, optionAt.PutCall);
		double priceAtDFExpected = (priceAtFP - priceAt) / shiftF;
		assertEquals(priceAtDFExpected, priceAtDF, 1E-6);
		// Above cut-off strike
		double priceOut = func.price(optionOut.Strike, optionOut.PutCall);
		double priceOutFP = sabrExtrapolationFP.price(optionOut.Strike, optionOut.PutCall);
		double priceOutDF = func.priceDerivativeForward(optionOut.Strike, optionOut.PutCall);
		double priceOutDFExpected = (priceOutFP - priceOut) / shiftF;
		assertEquals(priceOutDFExpected, priceOutDF, 1E-5);
		double[] abc = func.Parameter;
		double[] abcDF = func.ParameterDerivativeForward;
		double[] abcFP = sabrExtrapolationFP.Parameter;
		double[] abcDFExpected = new double[3];
		for (int loopparam = 0; loopparam < 3; loopparam++)
		{
		  abcDFExpected[loopparam] = (abcFP[loopparam] - abc[loopparam]) / shiftF;
		  assertEquals(1.0, abcDFExpected[loopparam] / abcDF[loopparam], 5E-2);
		}
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceDerivativeStrikeCall()
	  {
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		double shiftK = 0.000001;
		EuropeanVanillaOption optionIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionInKP = EuropeanVanillaOption.of(strikeIn + shiftK, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionAtKP = EuropeanVanillaOption.of(strikeAt + shiftK, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionOutKP = EuropeanVanillaOption.of(strikeOut + shiftK, TIME_TO_EXPIRY, PutCall.CALL);
		// Below cut-off strike
		double priceIn = SABR_EXTRAPOLATION.price(optionIn.Strike, optionIn.PutCall);
		double priceInKP = SABR_EXTRAPOLATION.price(optionInKP.Strike, optionInKP.PutCall);
		double priceInDK = SABR_EXTRAPOLATION.priceDerivativeStrike(optionIn.Strike, optionIn.PutCall);
		double priceInDFExpected = (priceInKP - priceIn) / shiftK;
		assertEquals(priceInDFExpected, priceInDK, 1E-5);
		// At cut-off strike
		double priceAt = SABR_EXTRAPOLATION.price(optionAt.Strike, optionAt.PutCall);
		double priceAtKP = SABR_EXTRAPOLATION.price(optionAtKP.Strike, optionAtKP.PutCall);
		double priceAtDK = SABR_EXTRAPOLATION.priceDerivativeStrike(optionAt.Strike, optionAt.PutCall);
		double priceAtDFExpected = (priceAtKP - priceAt) / shiftK;
		assertEquals(priceAtDFExpected, priceAtDK, 1E-5);
		// At cut-off strike
		double priceOut = SABR_EXTRAPOLATION.price(optionOut.Strike, optionOut.PutCall);
		double priceOutKP = SABR_EXTRAPOLATION.price(optionOutKP.Strike, optionOutKP.PutCall);
		double priceOutDK = SABR_EXTRAPOLATION.priceDerivativeStrike(optionOut.Strike, optionOut.PutCall);
		double priceOutDFExpected = (priceOutKP - priceOut) / shiftK;
		assertEquals(priceOutDFExpected, priceOutDK, 1E-5);
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceDerivativeStrikePut()
	  {
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		double shiftK = 0.000001;
		EuropeanVanillaOption optionIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionInKP = EuropeanVanillaOption.of(strikeIn + shiftK, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionAtKP = EuropeanVanillaOption.of(strikeAt + shiftK, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionOutKP = EuropeanVanillaOption.of(strikeOut + shiftK, TIME_TO_EXPIRY, PutCall.PUT);
		// Below cut-off strike
		double priceIn = SABR_EXTRAPOLATION.price(optionIn.Strike, optionIn.PutCall);
		double priceInKP = SABR_EXTRAPOLATION.price(optionInKP.Strike, optionInKP.PutCall);
		double priceInDK = SABR_EXTRAPOLATION.priceDerivativeStrike(optionIn.Strike, optionIn.PutCall);
		double priceInDFExpected = (priceInKP - priceIn) / shiftK;
		assertEquals(priceInDFExpected, priceInDK, 1E-5);
		// At cut-off strike
		double priceAt = SABR_EXTRAPOLATION.price(optionAt.Strike, optionAt.PutCall);
		double priceAtKP = SABR_EXTRAPOLATION.price(optionAtKP.Strike, optionAtKP.PutCall);
		double priceAtDK = SABR_EXTRAPOLATION.priceDerivativeStrike(optionAt.Strike, optionAt.PutCall);
		double priceAtDFExpected = (priceAtKP - priceAt) / shiftK;
		assertEquals(priceAtDFExpected, priceAtDK, 1E-5);
		// At cut-off strike
		double priceOut = SABR_EXTRAPOLATION.price(optionOut.Strike, optionOut.PutCall);
		double priceOutKP = SABR_EXTRAPOLATION.price(optionOutKP.Strike, optionOutKP.PutCall);
		double priceOutDK = SABR_EXTRAPOLATION.priceDerivativeStrike(optionOut.Strike, optionOut.PutCall);
		double priceOutDFExpected = (priceOutKP - priceOut) / shiftK;
		assertEquals(priceOutDFExpected, priceOutDK, 1E-5);
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceDerivativeSabrCall()
	  {
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		EuropeanVanillaOption optionIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption optionOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.CALL);
		double shift = 0.000001;
		SabrFormulaData sabrDataAP = SabrFormulaData.of(ALPHA + shift, BETA, RHO, NU);
		SabrFormulaData sabrDataBP = SabrFormulaData.of(ALPHA, BETA + shift, RHO, NU);
		SabrFormulaData sabrDataRP = SabrFormulaData.of(ALPHA, BETA, RHO + shift, NU);
		SabrFormulaData sabrDataNP = SabrFormulaData.of(ALPHA, BETA, RHO, NU + shift);
		SabrExtrapolationRightFunction sabrExtrapolationAP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataAP, CUT_OFF_STRIKE, MU);
		SabrExtrapolationRightFunction sabrExtrapolationBP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataBP, CUT_OFF_STRIKE, MU);
		SabrExtrapolationRightFunction sabrExtrapolationRP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataRP, CUT_OFF_STRIKE, MU);
		SabrExtrapolationRightFunction sabrExtrapolationNP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataNP, CUT_OFF_STRIKE, MU);
		// Below cut-off strike
		double priceInExpected = SABR_EXTRAPOLATION.price(optionIn.Strike, optionIn.PutCall);
		double[] priceInPP = new double[4];
		priceInPP[0] = sabrExtrapolationAP.price(optionIn.Strike, optionIn.PutCall);
		priceInPP[1] = sabrExtrapolationBP.price(optionIn.Strike, optionIn.PutCall);
		priceInPP[2] = sabrExtrapolationRP.price(optionIn.Strike, optionIn.PutCall);
		priceInPP[3] = sabrExtrapolationNP.price(optionIn.Strike, optionIn.PutCall);
		ValueDerivatives resIn = SABR_EXTRAPOLATION.priceAdjointSabr(optionIn.Strike, optionIn.PutCall);
		double priceIn = resIn.Value;
		double[] priceInDsabr = resIn.Derivatives.toArray();
		assertEquals(priceInExpected, priceIn, TOLERANCE_PRICE);
		double[] priceInDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 3; loopparam++)
		{
		  priceInDsabrExpected[loopparam] = (priceInPP[loopparam] - priceIn) / shift;
		  assertEquals(priceInDsabrExpected[loopparam], priceInDsabr[loopparam], 1E-5);
		}
		// At cut-off strike
		double priceAtExpected = SABR_EXTRAPOLATION.price(optionAt.Strike, optionAt.PutCall);
		double[] priceAtPP = new double[4];
		priceAtPP[0] = sabrExtrapolationAP.price(optionAt.Strike, optionAt.PutCall);
		priceAtPP[1] = sabrExtrapolationBP.price(optionAt.Strike, optionAt.PutCall);
		priceAtPP[2] = sabrExtrapolationRP.price(optionAt.Strike, optionAt.PutCall);
		priceAtPP[3] = sabrExtrapolationNP.price(optionAt.Strike, optionAt.PutCall);
		ValueDerivatives resAt = SABR_EXTRAPOLATION.priceAdjointSabr(optionAt.Strike, optionAt.PutCall);
		double priceAt = resAt.Value;
		double[] priceAtDsabr = resAt.Derivatives.toArray();
		assertEquals(priceAtExpected, priceAt, TOLERANCE_PRICE);
		double[] priceAtDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 3; loopparam++)
		{
		  priceAtDsabrExpected[loopparam] = (priceAtPP[loopparam] - priceAt) / shift;
		  assertEquals(priceAtDsabrExpected[loopparam], priceAtDsabr[loopparam], 1E-5);
		}
		// Above cut-off strike
		double[] abc = SABR_EXTRAPOLATION.Parameter;
		double[][] abcDP = SABR_EXTRAPOLATION.ParameterDerivativeSabr;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] abcPP = new double[4][3];
		double[][] abcPP = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		abcPP[0] = sabrExtrapolationAP.Parameter;
		abcPP[1] = sabrExtrapolationBP.Parameter;
		abcPP[2] = sabrExtrapolationRP.Parameter;
		abcPP[3] = sabrExtrapolationNP.Parameter;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] abcDPExpected = new double[4][3];
		double[][] abcDPExpected = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  for (int loopabc = 0; loopabc < 3; loopabc++)
		  {
			abcDPExpected[loopparam][loopabc] = (abcPP[loopparam][loopabc] - abc[loopabc]) / shift;
			assertEquals(1.0, abcDPExpected[loopparam][loopabc] / abcDP[loopparam][loopabc], 5.0E-2);
		  }
		}
		double priceOutExpected = SABR_EXTRAPOLATION.price(optionOut.Strike, optionOut.PutCall);
		double[] priceOutPP = new double[4];
		priceOutPP[0] = sabrExtrapolationAP.price(optionOut.Strike, optionOut.PutCall);
		priceOutPP[1] = sabrExtrapolationBP.price(optionOut.Strike, optionOut.PutCall);
		priceOutPP[2] = sabrExtrapolationRP.price(optionOut.Strike, optionOut.PutCall);
		priceOutPP[3] = sabrExtrapolationNP.price(optionOut.Strike, optionOut.PutCall);
		ValueDerivatives resOut = SABR_EXTRAPOLATION.priceAdjointSabr(optionOut.Strike, optionOut.PutCall);
		double priceOut = resOut.Value;
		double[] priceOutDsabr = resOut.Derivatives.toArray();
		assertEquals(priceOutExpected, priceOut, TOLERANCE_PRICE);
		double[] priceOutDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  priceOutDsabrExpected[loopparam] = (priceOutPP[loopparam] - priceOut) / shift;
		  assertEquals(1.0, priceOutDsabrExpected[loopparam] / priceOutDsabr[loopparam], 4.0E-4);
		}
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void priceDerivativeSabrPut()
	  {
		SabrExtrapolationRightFunction func = SabrExtrapolationRightFunction.of(FORWARD, SABR_DATA, CUT_OFF_STRIKE, TIME_TO_EXPIRY, MU, SabrHaganVolatilityFunctionProvider.DEFAULT);
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		EuropeanVanillaOption optionIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption optionOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.PUT);
		double shift = 0.000001;
		SabrFormulaData sabrDataAP = SabrFormulaData.of(ALPHA + shift, BETA, RHO, NU);
		SabrFormulaData sabrDataBP = SabrFormulaData.of(ALPHA, BETA + shift, RHO, NU);
		SabrFormulaData sabrDataRP = SabrFormulaData.of(ALPHA, BETA, RHO + shift, NU);
		SabrFormulaData sabrDataNP = SabrFormulaData.of(ALPHA, BETA, RHO, NU + shift);
		SabrExtrapolationRightFunction sabrExtrapolationAP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataAP, CUT_OFF_STRIKE, MU);
		SabrExtrapolationRightFunction sabrExtrapolationBP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataBP, CUT_OFF_STRIKE, MU);
		SabrExtrapolationRightFunction sabrExtrapolationRP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataRP, CUT_OFF_STRIKE, MU);
		SabrExtrapolationRightFunction sabrExtrapolationNP = SabrExtrapolationRightFunction.of(FORWARD, TIME_TO_EXPIRY, sabrDataNP, CUT_OFF_STRIKE, MU);
		// Below cut-off strike
		double priceInExpected = func.price(optionIn.Strike, optionIn.PutCall);
		double[] priceInPP = new double[4];
		priceInPP[0] = sabrExtrapolationAP.price(optionIn.Strike, optionIn.PutCall);
		priceInPP[1] = sabrExtrapolationBP.price(optionIn.Strike, optionIn.PutCall);
		priceInPP[2] = sabrExtrapolationRP.price(optionIn.Strike, optionIn.PutCall);
		priceInPP[3] = sabrExtrapolationNP.price(optionIn.Strike, optionIn.PutCall);
		ValueDerivatives resIn = func.priceAdjointSabr(optionIn.Strike, optionIn.PutCall);
		double priceIn = resIn.Value;
		double[] priceInDsabr = resIn.Derivatives.toArray();
		assertEquals(priceInExpected, priceIn, TOLERANCE_PRICE);
		double[] priceInDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 3; loopparam++)
		{
		  priceInDsabrExpected[loopparam] = (priceInPP[loopparam] - priceIn) / shift;
		  assertEquals(priceInDsabrExpected[loopparam], priceInDsabr[loopparam], 1E-5);
		}
		// At cut-off strike
		double priceAtExpected = func.price(optionAt.Strike, optionAt.PutCall);
		double[] priceAtPP = new double[4];
		priceAtPP[0] = sabrExtrapolationAP.price(optionAt.Strike, optionAt.PutCall);
		priceAtPP[1] = sabrExtrapolationBP.price(optionAt.Strike, optionAt.PutCall);
		priceAtPP[2] = sabrExtrapolationRP.price(optionAt.Strike, optionAt.PutCall);
		priceAtPP[3] = sabrExtrapolationNP.price(optionAt.Strike, optionAt.PutCall);
		ValueDerivatives resAt = func.priceAdjointSabr(optionAt.Strike, optionAt.PutCall);
		double priceAt = resAt.Value;
		double[] priceAtDsabr = resAt.Derivatives.toArray();
		assertEquals(priceAtExpected, priceAt, TOLERANCE_PRICE);
		double[] priceAtDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 3; loopparam++)
		{
		  priceAtDsabrExpected[loopparam] = (priceAtPP[loopparam] - priceAt) / shift;
		  assertEquals(priceAtDsabrExpected[loopparam], priceAtDsabr[loopparam], 1E-5);
		}
		// Above cut-off strike
		double priceOutExpected = func.price(optionOut.Strike, optionOut.PutCall);
		double[] priceOutPP = new double[4];
		priceOutPP[0] = sabrExtrapolationAP.price(optionOut.Strike, optionOut.PutCall);
		priceOutPP[1] = sabrExtrapolationBP.price(optionOut.Strike, optionOut.PutCall);
		priceOutPP[2] = sabrExtrapolationRP.price(optionOut.Strike, optionOut.PutCall);
		priceOutPP[3] = sabrExtrapolationNP.price(optionOut.Strike, optionOut.PutCall);
		ValueDerivatives resOut = func.priceAdjointSabr(optionOut.Strike, optionOut.PutCall);
		double priceOut = resOut.Value;
		double[] priceOutDsabr = resOut.Derivatives.toArray();
		assertEquals(priceOutExpected, priceOut, TOLERANCE_PRICE);
		double[] abc = func.Parameter;
		double[][] abcDP = func.ParameterDerivativeSabr;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] abcPP = new double[4][3];
		double[][] abcPP = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		abcPP[0] = sabrExtrapolationAP.Parameter;
		abcPP[1] = sabrExtrapolationBP.Parameter;
		abcPP[2] = sabrExtrapolationRP.Parameter;
		abcPP[3] = sabrExtrapolationNP.Parameter;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] abcDPExpected = new double[4][3];
		double[][] abcDPExpected = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  for (int loopabc = 0; loopabc < 3; loopabc++)
		  {
			abcDPExpected[loopparam][loopabc] = (abcPP[loopparam][loopabc] - abc[loopabc]) / shift;
			assertEquals(1.0, abcDPExpected[loopparam][loopabc] / abcDP[loopparam][loopabc], 5.0E-2);
		  }
		}
		double[] priceOutDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  priceOutDsabrExpected[loopparam] = (priceOutPP[loopparam] - priceOut) / shift;
		  assertEquals(1.0, priceOutDsabrExpected[loopparam] / priceOutDsabr[loopparam], 4.0E-4);
		}
	  }

	  /// <summary>
	  /// Tests the price derivative with respect to forward for options in SABR model with extrapolation. Other data.
	  /// </summary>
	  public virtual void priceDerivativeSABR2()
	  {
		double alpha = 0.06;
		double beta = 0.5;
		double rho = 0.0;
		double nu = 0.3;
		double cutOff = 0.10;
		double mu = 2.5;
		double strike = 0.15;
		double t = 2.366105247;
		EuropeanVanillaOption option = EuropeanVanillaOption.of(strike, t, PutCall.CALL);
		SabrFormulaData sabrData = SabrFormulaData.of(alpha, beta, rho, nu);
		double forward = 0.0404500579038675;
		SabrExtrapolationRightFunction sabrExtrapolation = SabrExtrapolationRightFunction.of(forward, t, sabrData, cutOff, mu);
		double shift = 0.000001;
		SabrFormulaData sabrDataAP = SabrFormulaData.of(alpha + shift, beta, rho, nu);
		SabrFormulaData sabrDataBP = SabrFormulaData.of(alpha, beta + shift, rho, nu);
		SabrFormulaData sabrDataRP = SabrFormulaData.of(alpha, beta, rho + shift, nu);
		SabrFormulaData sabrDataNP = SabrFormulaData.of(alpha, beta, rho, nu + shift);
		SabrExtrapolationRightFunction sabrExtrapolationAP = SabrExtrapolationRightFunction.of(forward, t, sabrDataAP, cutOff, mu);
		SabrExtrapolationRightFunction sabrExtrapolationBP = SabrExtrapolationRightFunction.of(forward, t, sabrDataBP, cutOff, mu);
		SabrExtrapolationRightFunction sabrExtrapolationRP = SabrExtrapolationRightFunction.of(forward, t, sabrDataRP, cutOff, mu);
		SabrExtrapolationRightFunction sabrExtrapolationNP = SabrExtrapolationRightFunction.of(forward, t, sabrDataNP, cutOff, mu);
		// Above cut-off strike
		double[] abc = sabrExtrapolation.Parameter;
		double[][] abcDP = sabrExtrapolation.ParameterDerivativeSabr;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] abcPP = new double[4][3];
		double[][] abcPP = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		abcPP[0] = sabrExtrapolationAP.Parameter;
		abcPP[1] = sabrExtrapolationBP.Parameter;
		abcPP[2] = sabrExtrapolationRP.Parameter;
		abcPP[3] = sabrExtrapolationNP.Parameter;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] abcDPExpected = new double[4][3];
		double[][] abcDPExpected = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  for (int loopabc = 0; loopabc < 3; loopabc++)
		  {
			abcDPExpected[loopparam][loopabc] = (abcPP[loopparam][loopabc] - abc[loopabc]) / shift;
			assertEquals(1.0, abcDPExpected[loopparam][loopabc] / abcDP[loopparam][loopabc], 5.0E-2);
		  }
		}
		double priceOutExpected = sabrExtrapolation.price(option.Strike, option.PutCall);
		double[] priceOutPP = new double[4];
		priceOutPP[0] = sabrExtrapolationAP.price(option.Strike, option.PutCall);
		priceOutPP[1] = sabrExtrapolationBP.price(option.Strike, option.PutCall);
		priceOutPP[2] = sabrExtrapolationRP.price(option.Strike, option.PutCall);
		priceOutPP[3] = sabrExtrapolationNP.price(option.Strike, option.PutCall);
		ValueDerivatives resOut = sabrExtrapolation.priceAdjointSabr(option.Strike, option.PutCall);
		double priceOut = resOut.Value;
		double[] priceOutDsabr = resOut.Derivatives.toArray();
		assertEquals(priceOutExpected, priceOut, 1E-5);
		double[] priceOutDsabrExpected = new double[4];
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  priceOutDsabrExpected[loopparam] = (priceOutPP[loopparam] - priceOut) / shift;
		  assertEquals(1.0, priceOutDsabrExpected[loopparam] / priceOutDsabr[loopparam], 4.0E-4);
		}
	  }

	  /// <summary>
	  /// Tests the price put/call parity for options in SABR model with extrapolation.
	  /// </summary>
	  public virtual void pricePutCallParity()
	  {
		double strikeIn = 0.08;
		double strikeAt = CUT_OFF_STRIKE;
		double strikeOut = 0.12;
		EuropeanVanillaOption callIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption putIn = EuropeanVanillaOption.of(strikeIn, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption callAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption putAt = EuropeanVanillaOption.of(strikeAt, TIME_TO_EXPIRY, PutCall.PUT);
		EuropeanVanillaOption callOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.CALL);
		EuropeanVanillaOption putOut = EuropeanVanillaOption.of(strikeOut, TIME_TO_EXPIRY, PutCall.PUT);
		double priceCallIn = SABR_EXTRAPOLATION.price(callIn.Strike, callIn.PutCall);
		double pricePutIn = SABR_EXTRAPOLATION.price(putIn.Strike, putIn.PutCall);
		assertEquals(FORWARD - strikeIn, priceCallIn - pricePutIn, TOLERANCE_PRICE);
		double priceCallAt = SABR_EXTRAPOLATION.price(callAt.Strike, callAt.PutCall);
		double pricePutAt = SABR_EXTRAPOLATION.price(putAt.Strike, putAt.PutCall);
		assertEquals(FORWARD - strikeAt, priceCallAt - pricePutAt, TOLERANCE_PRICE);
		double priceCallOut = SABR_EXTRAPOLATION.price(callOut.Strike, callOut.PutCall);
		double pricePutOut = SABR_EXTRAPOLATION.price(putOut.Strike, putOut.PutCall);
		assertEquals(FORWARD - strikeOut, priceCallOut - pricePutOut, TOLERANCE_PRICE);
	  }

	  /// <summary>
	  /// Tests that the smile and its derivatives are smooth enough in SABR model with extrapolation.
	  /// </summary>
	  public virtual void smileSmooth()
	  {
		int nbPts = 100;
		double rangeStrike = 0.02;
		double[] price = new double[nbPts + 1];
		double[] strike = new double[nbPts + 1];
		for (int looppts = 0; looppts <= nbPts; looppts++)
		{
		  strike[looppts] = CUT_OFF_STRIKE - rangeStrike + looppts * 2.0 * rangeStrike / nbPts;
		  EuropeanVanillaOption option = EuropeanVanillaOption.of(strike[looppts], TIME_TO_EXPIRY, PutCall.CALL);
		  price[looppts] = SABR_EXTRAPOLATION.price(option.Strike, option.PutCall);
		}
		double[] priceD = new double[nbPts];
		double[] priceD2 = new double[nbPts];
		for (int looppts = 1; looppts < nbPts; looppts++)
		{
		  priceD[looppts] = (price[looppts + 1] - price[looppts - 1]) / (strike[looppts + 1] - strike[looppts - 1]);
		  priceD2[looppts] = (price[looppts + 1] + price[looppts - 1] - 2 * price[looppts]) / ((strike[looppts + 1] - strike[looppts]) * (strike[looppts + 1] - strike[looppts]));
		}
		for (int looppts = 2; looppts < nbPts; looppts++)
		{
		  assertEquals(priceD[looppts - 1], priceD[looppts], 1.5E-3);
		  assertEquals(priceD2[looppts - 1], priceD2[looppts], 1.5E-1);
		}
	  }

	  /// <summary>
	  /// Tests that the smile and its derivatives are smooth enough in SABR model with extrapolation 
	  /// for different time to maturity (in particular close to maturity).
	  /// </summary>
	  public virtual void smileSmoothMaturity()
	  {
		int nbPts = 100;
		double[] timeToExpiry = new double[] {2.0, 1.0, 0.50, 0.25, 1.0d / 12.0d, 1.0d / 52.0d, 1.0d / 365d};
		int nbTTM = timeToExpiry.Length;
		double rangeStrike = 0.02;
		double[] strike = new double[nbPts + 1];
		for (int looppts = 0; looppts <= nbPts; looppts++)
		{
		  strike[looppts] = CUT_OFF_STRIKE - rangeStrike + looppts * 2.0 * rangeStrike / nbPts;
		}
		SabrExtrapolationRightFunction[] sabrExtrapolation = new SabrExtrapolationRightFunction[nbTTM];
		for (int loopmat = 0; loopmat < nbTTM; loopmat++)
		{
		  sabrExtrapolation[loopmat] = SabrExtrapolationRightFunction.of(FORWARD, timeToExpiry[loopmat], SABR_DATA, CUT_OFF_STRIKE, MU);
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] price = new double[nbTTM][nbPts + 1];
		double[][] price = RectangularArrays.ReturnRectangularDoubleArray(nbTTM, nbPts + 1);
		for (int loopmat = 0; loopmat < nbTTM; loopmat++)
		{
		  for (int looppts = 0; looppts <= nbPts; looppts++)
		  {
			EuropeanVanillaOption option = EuropeanVanillaOption.of(strike[looppts], timeToExpiry[loopmat], PutCall.CALL);
			price[loopmat][looppts] = sabrExtrapolation[loopmat].price(option.Strike, option.PutCall);
		  }
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] priceD = new double[nbTTM][nbPts - 1];
		double[][] priceD = RectangularArrays.ReturnRectangularDoubleArray(nbTTM, nbPts - 1);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] priceD2 = new double[nbTTM][nbPts - 1];
		double[][] priceD2 = RectangularArrays.ReturnRectangularDoubleArray(nbTTM, nbPts - 1);
		for (int loopmat = 0; loopmat < nbTTM; loopmat++)
		{
		  for (int looppts = 1; looppts < nbPts; looppts++)
		  {
			priceD[loopmat][looppts - 1] = (price[loopmat][looppts + 1] - price[loopmat][looppts - 1]) / (strike[looppts + 1] - strike[looppts - 1]);
			priceD2[loopmat][looppts - 1] = (price[loopmat][looppts + 1] + price[loopmat][looppts - 1] - 2 * price[loopmat][looppts]) / ((strike[looppts + 1] - strike[looppts]) * (strike[looppts + 1] - strike[looppts]));
		  }
		}
		double epsDensity = 1.0E-20; // Conditions are not checked when the density is very small.
		for (int loopmat = 0; loopmat < nbTTM; loopmat++)
		{
		  for (int looppts = 1; looppts < nbPts - 1; looppts++)
		  {
			assertTrue(((priceD[loopmat][looppts] / priceD[loopmat][looppts - 1] < 1) && (priceD[loopmat][looppts] / priceD[loopmat][looppts - 1] > 0.50)) || Math.Abs(priceD2[loopmat][looppts]) < epsDensity);
			assertTrue(priceD2[loopmat][looppts] > 0 || Math.Abs(priceD2[loopmat][looppts]) < epsDensity);
			assertTrue((priceD2[loopmat][looppts] / priceD2[loopmat][looppts - 1] < 1 && priceD2[loopmat][looppts] / priceD2[loopmat][looppts - 1] > 0.50) || Math.Abs(priceD2[loopmat][looppts]) < epsDensity);
		  }
		}
	  }

	  private const double EPS = 1.0e-6;
	  private static readonly SabrHaganVolatilityFunctionProvider FUNC_HAGAN = SabrHaganVolatilityFunctionProvider.DEFAULT;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private static final com.opengamma.strata.pricer.impl.volatility.smile.VolatilityFunctionProvider<com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData>[] FUNCTIONS = new com.opengamma.strata.pricer.impl.volatility.smile.VolatilityFunctionProvider[] {FUNC_HAGAN};
	  private static readonly VolatilityFunctionProvider<SabrFormulaData>[] FUNCTIONS = new VolatilityFunctionProvider[] {FUNC_HAGAN}; // other volatility functions to be added

	  /// <summary>
	  /// Testing C2 continuity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void smoothnessTest()
	  public virtual void smoothnessTest()
	  {
		foreach (VolatilityFunctionProvider<SabrFormulaData> func in FUNCTIONS)
		{
		  SabrExtrapolationRightFunction extrapolation = SabrExtrapolationRightFunction.of(FORWARD, SABR_DATA, CUT_OFF_STRIKE, TIME_TO_EXPIRY, MU, func);
		  foreach (PutCall isCall in new PutCall[] {PutCall.CALL, PutCall.PUT})
		  {
			double priceBase = extrapolation.price(CUT_OFF_STRIKE, isCall);
			double priceUp = extrapolation.price(CUT_OFF_STRIKE + EPS, isCall);
			double priceDw = extrapolation.price(CUT_OFF_STRIKE - EPS, isCall);
			assertEquals(priceBase, priceUp, EPS);
			assertEquals(priceBase, priceDw, EPS);
			double priceUpUp = extrapolation.price(CUT_OFF_STRIKE + 2.0 * EPS, isCall);
			double priceDwDw = extrapolation.price(CUT_OFF_STRIKE - 2.0 * EPS, isCall);
			double firstUp = (-0.5 * priceUpUp + 2.0 * priceUp - 1.5 * priceBase) / EPS;
			double firstDw = (-2.0 * priceDw + 0.5 * priceDwDw + 1.5 * priceBase) / EPS;
			assertEquals(firstDw, firstUp, EPS);
			// The second derivative values are poorly connected due to finite difference approximation 
			double firstUpUp = 0.5 * (priceUpUp - priceBase) / EPS;
			double firstDwDw = 0.5 * (priceBase - priceDwDw) / EPS;
			double secondUp = (firstUpUp - firstUp) / EPS;
			double secondDw = (firstDw - firstDwDw) / EPS;
			double secondRef = 0.5 * (firstUpUp - firstDwDw) / EPS;
			assertEquals(secondRef, secondUp, secondRef * 0.15);
			assertEquals(secondRef, secondDw, secondRef * 0.15);
		  }
		}
	  }

	  /// <summary>
	  /// Test small forward.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void smallForwardTest()
	  public virtual void smallForwardTest()
	  {
		double smallForward = 0.1e-6;
		double smallCutoff = 0.9e-6;
		foreach (VolatilityFunctionProvider<SabrFormulaData> func in FUNCTIONS)
		{
		  SabrExtrapolationRightFunction right = SabrExtrapolationRightFunction.of(smallForward, SABR_DATA, smallCutoff, TIME_TO_EXPIRY, MU, func);
		  foreach (PutCall isCall in new PutCall[] {PutCall.CALL, PutCall.PUT})
		  {
			double priceBase = right.price(smallCutoff, isCall);
			double priceUp = right.price(smallCutoff + EPS * 0.1, isCall);
			double priceDw = right.price(smallCutoff - EPS * 0.1, isCall);
			assertEquals(priceBase, priceUp, EPS * 10.0);
			assertEquals(priceBase, priceDw, EPS * 10.0);
		  }
		}
	  }

	  /// <summary>
	  /// Extrapolator is not calibrated in this case, then the gap may be produced at the cutoff.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void smallExpiryTest()
	  public virtual void smallExpiryTest()
	  {
		double smallExpiry = 0.5e-6;
		foreach (VolatilityFunctionProvider<SabrFormulaData> func in FUNCTIONS)
		{
		  SabrExtrapolationRightFunction right = SabrExtrapolationRightFunction.of(FORWARD * 0.01, SABR_DATA, CUT_OFF_STRIKE, smallExpiry, MU, func);
		  foreach (PutCall isCall in new PutCall[] {PutCall.CALL, PutCall.PUT})
		  {
			double priceBase = right.price(CUT_OFF_STRIKE, isCall);
			double priceUp = right.price(CUT_OFF_STRIKE + EPS * 0.1, isCall);
			double priceDw = right.price(CUT_OFF_STRIKE - EPS * 0.1, isCall);
			assertEquals(priceBase, priceUp, EPS);
			assertEquals(priceBase, priceDw, EPS);
			assertEquals(right.Parameter[0], -1.0E4, 1.e-12);
			assertEquals(right.Parameter[1], 0.0, 1.e-12);
			assertEquals(right.Parameter[2], 0.0, 1.e-12);
		  }
		}
	  }

	}

}
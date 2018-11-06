/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using GenericVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfaceYearFractionParameterMetadata;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;

	/// <summary>
	/// Test <seealso cref="SmileDeltaParameters"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SmileDeltaParametersTest
	public class SmileDeltaParametersTest
	{

	  private const double TIME_TO_EXPIRY = 2.0;
	  private const double FORWARD = 1.40;
	  private const double ATM = 0.185;
	  private static readonly DoubleArray DELTA = DoubleArray.of(0.10, 0.25);
	  private static readonly DoubleArray RISK_REVERSAL = DoubleArray.of(-0.0130, -0.0050);
	  private static readonly DoubleArray STRANGLE = DoubleArray.of(0.0300, 0.0100);
	  private static readonly ImmutableList<ParameterMetadata> PARAMETER_METADATA = ImmutableList.of(GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, DeltaStrike.of(0.9d)), GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, DeltaStrike.of(0.75d)), GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, DeltaStrike.of(0.5d)), GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, DeltaStrike.of(0.25d)), GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, DeltaStrike.of(0.1d)));

	  private static readonly SmileDeltaParameters SMILE = SmileDeltaParameters.of(TIME_TO_EXPIRY, ATM, DELTA, RISK_REVERSAL, STRANGLE);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDelta()
	  public virtual void testNullDelta()
	  {
		SmileDeltaParameters.of(TIME_TO_EXPIRY, ATM, null, RISK_REVERSAL, STRANGLE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testRRLength()
	  public virtual void testRRLength()
	  {
		SmileDeltaParameters.of(TIME_TO_EXPIRY, ATM, DELTA, DoubleArray.filled(3), STRANGLE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testStrangleLength()
	  public virtual void testStrangleLength()
	  {
		SmileDeltaParameters.of(TIME_TO_EXPIRY, ATM, DELTA, RISK_REVERSAL, DoubleArray.filled(3));
	  }

	  /// <summary>
	  /// Tests the constructor directly from volatilities (not RR and S).
	  /// </summary>
	  public virtual void constructorVolatility()
	  {
		DoubleArray volatility = SMILE.Volatility;
		SmileDeltaParameters smileFromVolatility = SmileDeltaParameters.of(TIME_TO_EXPIRY, DELTA, volatility, PARAMETER_METADATA);
		assertEquals(smileFromVolatility, SMILE, "Smile by delta: constructor");
	  }

	  /// <summary>
	  /// Tests the getters.
	  /// </summary>
	  public virtual void getter()
	  {
		assertEquals(SMILE.Expiry, TIME_TO_EXPIRY, "Smile by delta: time to expiry");
		assertEquals(SMILE.Delta, DELTA, "Smile by delta: delta");
		SmileDeltaParameters smile2 = SmileDeltaParameters.of(TIME_TO_EXPIRY, DELTA, SMILE.Volatility);
		assertEquals(smile2.Volatility, SMILE.Volatility, "Smile by delta: volatility");
	  }

	  /// <summary>
	  /// Tests the volatility computations.
	  /// </summary>
	  public virtual void volatility()
	  {
		DoubleArray volatility = SMILE.Volatility;
		int nbDelta = DELTA.size();
		assertEquals(volatility.get(nbDelta), ATM, "Volatility: ATM");
		for (int loopdelta = 0; loopdelta < nbDelta; loopdelta++)
		{
		  assertEquals(volatility.get(2 * nbDelta - loopdelta) - volatility.get(loopdelta), RISK_REVERSAL.get(loopdelta), 1e-8, "Volatility: Risk Reversal " + loopdelta);
		  assertEquals((volatility.get(2 * nbDelta - loopdelta) + volatility.get(loopdelta)) / 2 - volatility.get(nbDelta), STRANGLE.get(loopdelta), 1e-8, "Volatility: Strangle " + loopdelta);
		}
	  }

	  /// <summary>
	  /// Tests the strikes computations.
	  /// </summary>
	  public virtual void strike()
	  {
		double[] strike = SMILE.strike(FORWARD).toArrayUnsafe();
		DoubleArray volatility = SMILE.Volatility;
		int nbDelta = DELTA.size();
		for (int loopdelta = 0; loopdelta < nbDelta; loopdelta++)
		{
		  ValueDerivatives dPut = BlackFormulaRepository.priceAdjoint(FORWARD, strike[loopdelta], TIME_TO_EXPIRY, volatility.get(loopdelta), false);
		  assertEquals(-DELTA.get(loopdelta), dPut.getDerivative(0), 1e-8, "Strike: Put " + loopdelta);
		  ValueDerivatives dCall = BlackFormulaRepository.priceAdjoint(FORWARD, strike[2 * nbDelta - loopdelta], TIME_TO_EXPIRY, volatility.get(2 * nbDelta - loopdelta), true);
		  assertEquals(DELTA.get(loopdelta), dCall.getDerivative(0), 1e-8, "Strike: Call " + loopdelta);
		}
		ValueDerivatives dPut = BlackFormulaRepository.priceAdjoint(FORWARD, strike[nbDelta], TIME_TO_EXPIRY, volatility.get(nbDelta), false);
		ValueDerivatives dCall = BlackFormulaRepository.priceAdjoint(FORWARD, strike[nbDelta], TIME_TO_EXPIRY, volatility.get(nbDelta), true);
		assertEquals(0.0, dCall.getDerivative(0) + dPut.getDerivative(0), 1e-8, "Strike: ATM");
	  }

	}

}
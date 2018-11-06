/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using EuropeanVanillaOption = com.opengamma.strata.pricer.impl.option.EuropeanVanillaOption;

	/// <summary>
	/// Test case for SABR volatility function providers.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class SabrVolatilityFunctionProviderTestCase
	public abstract class SabrVolatilityFunctionProviderTestCase
	{

	  private const double K = 105;
	  private const double T = 1.5;
	  protected internal const double FORWARD = 103;
	  protected internal static readonly EuropeanVanillaOption OPTION = EuropeanVanillaOption.of(K, T, CALL);
	  protected internal static readonly SabrFormulaData LOG_NORMAL_EQUIVALENT = SabrFormulaData.of(0.8, 1, 0.5, 0);
	  protected internal static readonly SabrFormulaData APPROACHING_LOG_NORMAL_EQUIVALENT1 = SabrFormulaData.of(0.8, 1, 0.5, 1e-6);
	  protected internal static readonly SabrFormulaData APPROACHING_LOG_NORMAL_EQUIVALENT2 = SabrFormulaData.of(0.8, 1 + 1e-6, 0.5, 0);
	  protected internal static readonly SabrFormulaData APPROACHING_LOG_NORMAL_EQUIVALENT3 = SabrFormulaData.of(0.8, 1 - 1e-6, 0.5, 0);

	  protected internal abstract VolatilityFunctionProvider<SabrFormulaData> Function {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullData()
	  public virtual void testNullData()
	  {
		Function.volatility(FORWARD, K, T, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testLogNormalEquivalent()
	  public virtual void testLogNormalEquivalent()
	  {
		assertEquals(Function.volatility(FORWARD, K, T, LOG_NORMAL_EQUIVALENT), LOG_NORMAL_EQUIVALENT.Alpha, 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testApproachingLogNormalEquivalent1()
	  public virtual void testApproachingLogNormalEquivalent1()
	  {
		assertEquals(Function.volatility(FORWARD, K, T, APPROACHING_LOG_NORMAL_EQUIVALENT1), LOG_NORMAL_EQUIVALENT.Alpha, 1e-5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testApproachingLogNormalEquivalent2()
	  public virtual void testApproachingLogNormalEquivalent2()
	  {
		assertEquals(Function.volatility(FORWARD, K, T, APPROACHING_LOG_NORMAL_EQUIVALENT2), LOG_NORMAL_EQUIVALENT.Alpha, 1e-5);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testApproachingLogNormalEquivalent3()
	  public virtual void testApproachingLogNormalEquivalent3()
	  {
		assertEquals(Function.volatility(FORWARD, K, T, APPROACHING_LOG_NORMAL_EQUIVALENT3), LOG_NORMAL_EQUIVALENT.Alpha, 1e-5);
	  }

	  //TODO need to fill in tests
	  //TODO beta = 1 nu = 0 => Black equivalent volatility
	  //TODO beta = 0 nu = 0 => Bachelier
	  //TODO beta != 0 nu = 0 => CEV

	}

}
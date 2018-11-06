/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using TermDepositTradeCalculationFunctionTest = com.opengamma.strata.measure.deposit.TermDepositTradeCalculationFunctionTest;
	using FraTradeCalculationFunctionTest = com.opengamma.strata.measure.fra.FraTradeCalculationFunctionTest;
	using FxNdfTradeCalculationFunctionTest = com.opengamma.strata.measure.fx.FxNdfTradeCalculationFunctionTest;
	using FxSingleTradeCalculationFunctionTest = com.opengamma.strata.measure.fx.FxSingleTradeCalculationFunctionTest;
	using FxSwapTradeCalculationFunctionTest = com.opengamma.strata.measure.fx.FxSwapTradeCalculationFunctionTest;
	using SwapTradeCalculationFunctionTest = com.opengamma.strata.measure.swap.SwapTradeCalculationFunctionTest;

	/// <summary>
	/// Test <seealso cref="StandardComponents"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class StandardComponentsTest
	public class StandardComponentsTest
	{

	  public virtual void test_standard()
	  {
		CalculationFunctions test = StandardComponents.calculationFunctions();
		assertEquals(test.findFunction(FraTradeCalculationFunctionTest.TRADE).Present, true);
		assertEquals(test.findFunction(FxSingleTradeCalculationFunctionTest.TRADE).Present, true);
		assertEquals(test.findFunction(FxNdfTradeCalculationFunctionTest.TRADE).Present, true);
		assertEquals(test.findFunction(FxSwapTradeCalculationFunctionTest.TRADE).Present, true);
		assertEquals(test.findFunction(SwapTradeCalculationFunctionTest.TRADE).Present, true);
		assertEquals(test.findFunction(TermDepositTradeCalculationFunctionTest.TRADE).Present, true);
	  }

	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(StandardComponents));
	  }

	}

}
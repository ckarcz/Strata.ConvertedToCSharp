/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;

	/// <summary>
	/// Test <seealso cref="FxRateShifts"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateShiftsTest
	public class FxRateShiftsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly DoubleArray SHIFT_AMOUNT_1 = DoubleArray.of(0.95d, 0.2d, -0.5d);
	  private static readonly DoubleArray SHIFT_AMOUNT_2 = DoubleArray.of(0.85d, 1.2d, 1.5d);
	  private static readonly CurrencyPair EURUSD = CurrencyPair.of(EUR, USD);
	  private static readonly CurrencyPair USDEUR = CurrencyPair.of(USD, EUR);
	  private const double BASE_RATE = 1.2d;
	  private static readonly FxRate FX_RATE = FxRate.of(EURUSD, BASE_RATE);

	  public virtual void test_of()
	  {
		FxRateShifts test = FxRateShifts.of(ShiftType.SCALED, SHIFT_AMOUNT_2, EURUSD);
		assertEquals(test.CurrencyPair, EURUSD);
		assertEquals(test.ScenarioCount, 3);
		assertEquals(test.ShiftAmount, SHIFT_AMOUNT_2);
		assertEquals(test.ShiftType, ShiftType.SCALED);
	  }

	  public virtual void test_applyTo()
	  {
		MarketDataBox<FxRate> marketData = MarketDataBox.ofSingleValue(FX_RATE);
		FxRateShifts testScaled = FxRateShifts.of(ShiftType.SCALED, SHIFT_AMOUNT_2, EURUSD);
		MarketDataBox<FxRate> computedScaled = testScaled.applyTo(marketData, REF_DATA);
		MarketDataBox<FxRate> expectedScaled = MarketDataBox.ofScenarioValues(ImmutableList.of(FxRate.of(EURUSD, BASE_RATE * SHIFT_AMOUNT_2.get(0)), FxRate.of(EURUSD, BASE_RATE * SHIFT_AMOUNT_2.get(1)), FxRate.of(EURUSD, BASE_RATE * SHIFT_AMOUNT_2.get(2))));
		assertEquals(computedScaled, expectedScaled);
		FxRateShifts testScaledInv = FxRateShifts.of(ShiftType.SCALED, SHIFT_AMOUNT_2, USDEUR);
		MarketDataBox<FxRate> computedScaledInv = testScaledInv.applyTo(marketData, REF_DATA);
		MarketDataBox<FxRate> expectedScaledInv = MarketDataBox.ofScenarioValues(ImmutableList.of(FxRate.of(USDEUR, 1d / BASE_RATE * SHIFT_AMOUNT_2.get(0)), FxRate.of(USDEUR, 1d / BASE_RATE * SHIFT_AMOUNT_2.get(1)), FxRate.of(USDEUR, 1d / BASE_RATE * SHIFT_AMOUNT_2.get(2))));
		assertEquals(computedScaledInv, expectedScaledInv);
		FxRateShifts testAbsolute = FxRateShifts.of(ShiftType.ABSOLUTE, SHIFT_AMOUNT_1, EURUSD);
		MarketDataBox<FxRate> computedAbsolute = testAbsolute.applyTo(marketData, REF_DATA);
		MarketDataBox<FxRate> expectedAbsolute = MarketDataBox.ofScenarioValues(ImmutableList.of(FxRate.of(EURUSD, BASE_RATE + SHIFT_AMOUNT_1.get(0)), FxRate.of(EURUSD, BASE_RATE + SHIFT_AMOUNT_1.get(1)), FxRate.of(EURUSD, BASE_RATE + SHIFT_AMOUNT_1.get(2))));
		assertEquals(computedAbsolute, expectedAbsolute);
		FxRateShifts testRelative = FxRateShifts.of(ShiftType.RELATIVE, SHIFT_AMOUNT_1, EURUSD);
		MarketDataBox<FxRate> computedRelative = testRelative.applyTo(marketData, REF_DATA);
		MarketDataBox<FxRate> expectedRelative = MarketDataBox.ofScenarioValues(ImmutableList.of(FxRate.of(EURUSD, BASE_RATE * (1d + SHIFT_AMOUNT_1.get(0))), FxRate.of(EURUSD, BASE_RATE * (1d + SHIFT_AMOUNT_1.get(1))), FxRate.of(EURUSD, BASE_RATE * (1d + SHIFT_AMOUNT_1.get(2)))));
		assertEquals(computedRelative, expectedRelative);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxRateShifts test1 = FxRateShifts.of(ShiftType.SCALED, SHIFT_AMOUNT_2, EURUSD);
		coverImmutableBean(test1);
		FxRateShifts test2 = FxRateShifts.of(ShiftType.ABSOLUTE, SHIFT_AMOUNT_1, USDEUR);
		coverBeanEquals(test1, test2);
	  }

	}

}
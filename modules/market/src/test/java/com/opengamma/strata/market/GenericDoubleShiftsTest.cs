/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;

	/// <summary>
	/// Test <seealso cref="GenericDoubleShifts"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GenericDoubleShiftsTest
	public class GenericDoubleShiftsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly DoubleArray SHIFT_AMOUNT = DoubleArray.of(3.5d, 2d, -5d);
	  private const double SPREAD = 0.5;

	  public virtual void test_of()
	  {
		GenericDoubleShifts test = GenericDoubleShifts.of(ShiftType.ABSOLUTE, SHIFT_AMOUNT);
		assertEquals(test.ScenarioCount, 3);
		assertEquals(test.ShiftAmount, SHIFT_AMOUNT);
		assertEquals(test.ShiftType, ShiftType.ABSOLUTE);
		assertEquals(test.Spread, 0d);
	  }

	  public virtual void test_of_spread()
	  {
		GenericDoubleShifts test = GenericDoubleShifts.of(ShiftType.RELATIVE, SHIFT_AMOUNT, SPREAD);
		assertEquals(test.ScenarioCount, 3);
		assertEquals(test.ShiftAmount, SHIFT_AMOUNT);
		assertEquals(test.ShiftType, ShiftType.RELATIVE);
		assertEquals(test.Spread, SPREAD);
	  }

	  public virtual void test_applyTo()
	  {
		double baseValue = 3d;
		MarketDataBox<double> marketData = MarketDataBox.ofSingleValue(baseValue);
		GenericDoubleShifts testScaled = GenericDoubleShifts.of(ShiftType.SCALED, SHIFT_AMOUNT, SPREAD);
		MarketDataBox<double> computedScaled = testScaled.applyTo(marketData, REF_DATA);
		MarketDataBox<double> expectedScaled = MarketDataBox.ofScenarioValues(ImmutableList.of((baseValue + SPREAD) * SHIFT_AMOUNT.get(0) - SPREAD, (baseValue + SPREAD) * SHIFT_AMOUNT.get(1) - SPREAD, (baseValue + SPREAD) * SHIFT_AMOUNT.get(2) - SPREAD));
		assertEquals(computedScaled, expectedScaled);
		GenericDoubleShifts testRelative = GenericDoubleShifts.of(ShiftType.RELATIVE, SHIFT_AMOUNT, SPREAD);
		MarketDataBox<double> computedRelative = testRelative.applyTo(marketData, REF_DATA);
		MarketDataBox<double> expectedRelative = MarketDataBox.ofScenarioValues(ImmutableList.of((baseValue + SPREAD) * (1d + SHIFT_AMOUNT.get(0)) - SPREAD, (baseValue + SPREAD) * (1d + SHIFT_AMOUNT.get(1)) - SPREAD, (baseValue + SPREAD) * (1d + SHIFT_AMOUNT.get(2)) - SPREAD));
		assertEquals(computedRelative, expectedRelative);
		GenericDoubleShifts testAbsolute = GenericDoubleShifts.of(ShiftType.ABSOLUTE, SHIFT_AMOUNT);
		MarketDataBox<double> computedAbsolute = testAbsolute.applyTo(marketData, REF_DATA);
		MarketDataBox<double> expectedAbsolute = MarketDataBox.ofScenarioValues(ImmutableList.of(baseValue + SHIFT_AMOUNT.get(0), baseValue + SHIFT_AMOUNT.get(1), baseValue + SHIFT_AMOUNT.get(2)));
		assertEquals(computedAbsolute, expectedAbsolute);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		GenericDoubleShifts test1 = GenericDoubleShifts.of(ShiftType.ABSOLUTE, SHIFT_AMOUNT);
		coverImmutableBean(test1);
		GenericDoubleShifts test2 = GenericDoubleShifts.of(ShiftType.SCALED, DoubleArray.of(3.5d, 2d), SPREAD);
		coverBeanEquals(test1, test2);
	  }

	}

}
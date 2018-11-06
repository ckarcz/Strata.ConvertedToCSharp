/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="SeasonalityDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SeasonalityDefinitionTest
	public class SeasonalityDefinitionTest
	{

	  private static readonly DoubleArray SEASONALITY_ADDITIVE = DoubleArray.of(1.0, 1.5, 1.0, -0.5, -0.5, -1.0, -1.5, 0.0, 0.5, 1.0, 1.0, -2.5);
	  private const ShiftType ADDITIVE = ShiftType.ABSOLUTE;

	  public virtual void test_builder1()
	  {
		SeasonalityDefinition test = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ADDITIVE);
		assertEquals(test.SeasonalityMonthOnMonth, SEASONALITY_ADDITIVE);
		assertEquals(test.AdjustmentType, ADDITIVE);
	  }

	  public virtual void test_of()
	  {
		SeasonalityDefinition test = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ADDITIVE);
		assertEquals(test.SeasonalityMonthOnMonth, SEASONALITY_ADDITIVE);
		assertEquals(test.AdjustmentType, ADDITIVE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SeasonalityDefinition test = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ADDITIVE);
		coverImmutableBean(test);
		DoubleArray seasonalityMultiplicative = DoubleArray.of(1.0, 1.0, 1.1d, 1.0, 1.0, 1.0, 1.0d / 1.1d, 1.0, 1.0, 1.0, 1.0, 1.0);
		SeasonalityDefinition test2 = SeasonalityDefinition.of(seasonalityMultiplicative, ShiftType.SCALED);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SeasonalityDefinition test = SeasonalityDefinition.of(SEASONALITY_ADDITIVE, ADDITIVE);
		assertSerialization(test);
	  }

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using TestingMeasures = com.opengamma.strata.calc.TestingMeasures;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using TestParameter = com.opengamma.strata.calc.runner.TestParameter;
	using TestParameter2 = com.opengamma.strata.calc.runner.TestParameter2;

	/// <summary>
	/// Test <seealso cref="TargetTypeCalculationParameter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TargetTypeCalculationParameterTest
	public class TargetTypeCalculationParameterTest
	{

	  private static readonly CalculationParameter PARAM1 = new TestParameter();
	  private static readonly CalculationParameter PARAM2 = new TestParameter();
	  private static readonly CalculationParameter PARAM3 = new TestParameter();
	  private static readonly CalculationParameter PARAM_OTHER = new TestParameter2();
	  private static readonly TestTarget TARGET1 = new TestTarget();
	  private static readonly TestTarget2 TARGET2 = new TestTarget2();
	  private static readonly TestTarget3 TARGET3 = new TestTarget3();

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		TargetTypeCalculationParameter test = TargetTypeCalculationParameter.of(ImmutableMap.of(typeof(TestTarget), PARAM1, typeof(TestTarget2), PARAM2), PARAM3);
		assertEquals(test.QueryType, typeof(TestParameter));
		assertEquals(test.Parameters.size(), 2);
		assertEquals(test.DefaultParameter, PARAM3);
		assertEquals(test.queryType(), typeof(TestParameter));
		assertEquals(test.filter(TARGET1, TestingMeasures.PRESENT_VALUE), PARAM1);
		assertEquals(test.filter(TARGET2, TestingMeasures.PRESENT_VALUE), PARAM2);
		assertEquals(test.filter(TARGET3, TestingMeasures.PRESENT_VALUE), PARAM3);
	  }

	  public virtual void of_empty()
	  {
		assertThrowsIllegalArg(() => TargetTypeCalculationParameter.of(ImmutableMap.of(), PARAM3));
	  }

	  public virtual void of_badType()
	  {
		assertThrowsIllegalArg(() => TargetTypeCalculationParameter.of(ImmutableMap.of(typeof(TestTarget), PARAM_OTHER), PARAM3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TargetTypeCalculationParameter test = TargetTypeCalculationParameter.of(ImmutableMap.of(typeof(TestTarget), PARAM1, typeof(TestTarget2), PARAM2), PARAM3);
		coverImmutableBean(test);
		TargetTypeCalculationParameter test2 = TargetTypeCalculationParameter.of(ImmutableMap.of(typeof(TestTarget), PARAM1), PARAM2);
		coverBeanEquals(test, test2);
	  }

	  //-------------------------------------------------------------------------
	  private class TestTarget : CalculationTarget
	  {
	  }

	  private class TestTarget2 : CalculationTarget
	  {
	  }

	  private class TestTarget3 : CalculationTarget
	  {
	  }

	}

}
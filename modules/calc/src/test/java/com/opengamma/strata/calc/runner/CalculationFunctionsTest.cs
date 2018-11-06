/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using TestFunction = com.opengamma.strata.calc.runner.CalculationTaskTest.TestFunction;
	using TestTarget = com.opengamma.strata.calc.runner.CalculationTaskTest.TestTarget;

	/// <summary>
	/// Test <seealso cref="CalculationFunctions"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationFunctionsTest
	public class CalculationFunctionsTest
	{

	  private static readonly CalculationFunction<TestTarget> TARGET = new TestFunction();

	  public virtual void empty()
	  {
		CalculationFunctions test = CalculationFunctions.empty();
		assertEquals(test.getFunction(new TestTarget()).supportedMeasures().size(), 0);
		assertEquals(test.findFunction(new TestTarget()), null);
	  }

	  public virtual void of_array()
	  {
		CalculationFunctions test = CalculationFunctions.of(TARGET);
		assertEquals(test.getFunction(new TestTarget()), TARGET);
		assertEquals(test.findFunction(new TestTarget()), TARGET);
	  }

	  public virtual void of_list()
	  {
		CalculationFunctions test = CalculationFunctions.of(ImmutableList.of(TARGET));
		assertEquals(test.getFunction(new TestTarget()), TARGET);
		assertEquals(test.findFunction(new TestTarget()), TARGET);
	  }

	  public virtual void of_map()
	  {
		CalculationFunctions test = CalculationFunctions.of(ImmutableMap.of(typeof(TestTarget), TARGET));
		assertEquals(test.getFunction(new TestTarget()), TARGET);
		assertEquals(test.findFunction(new TestTarget()), TARGET);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultCalculationFunctions test = DefaultCalculationFunctions.of(ImmutableMap.of(typeof(TestTarget), TARGET));
		coverImmutableBean(test);
		DefaultCalculationFunctions test2 = DefaultCalculationFunctions.EMPTY;
		coverBeanEquals(test, test2);
		assertNotNull(DefaultCalculationFunctions.meta());
	  }

	}

}
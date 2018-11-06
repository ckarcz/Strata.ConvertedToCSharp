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
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// Test <seealso cref="CalculationResults"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationResultsTest
	public class CalculationResultsTest
	{

	  private static readonly CalculationTarget TARGET = new CalculationTargetAnonymousInnerClass();

	  private class CalculationTargetAnonymousInnerClass : CalculationTarget
	  {
		  public CalculationTargetAnonymousInnerClass()
		  {
		  }

	  }
	  private static readonly CalculationTarget TARGET2 = new CalculationTargetAnonymousInnerClass2();

	  private class CalculationTargetAnonymousInnerClass2 : CalculationTarget
	  {
		  public CalculationTargetAnonymousInnerClass2()
		  {
		  }

	  }
	  private static readonly Result<string> RESULT = Result.success("OK");
	  private static readonly CalculationResult CALC_RESULT = CalculationResult.of(1, 2, RESULT);
	  private static readonly CalculationResult CALC_RESULT2 = CalculationResult.of(1, 2, RESULT);

	  //-------------------------------------------------------------------------
	  public virtual void of()
	  {
		CalculationResults test = CalculationResults.of(TARGET, ImmutableList.of(CALC_RESULT));
		assertEquals(test.Target, TARGET);
		assertEquals(test.Cells, ImmutableList.of(CALC_RESULT));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CalculationResults test = CalculationResults.of(TARGET, ImmutableList.of(CALC_RESULT));
		coverImmutableBean(test);
		CalculationResults test2 = CalculationResults.of(TARGET2, ImmutableList.of(CALC_RESULT2));
		coverBeanEquals(test, test2);
		assertNotNull(CalculationResults.meta());
	  }

	}

}
/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;

	using Test = org.testng.annotations.Test;

	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// Test <seealso cref="CalculationResult"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationResultTest
	public class CalculationResultTest
	{

	  private static readonly Result<string> RESULT = Result.success("OK");
	  private static readonly Result<string> RESULT2 = Result.success("OK2");
	  private static readonly Result<string> FAILURE = Result.failure(FailureReason.NOT_APPLICABLE, "N/A");

	  //-------------------------------------------------------------------------
	  public virtual void of()
	  {
		CalculationResult test = CalculationResult.of(1, 2, RESULT);
		assertEquals(test.RowIndex, 1);
		assertEquals(test.ColumnIndex, 2);
		assertEquals(test.Result, RESULT);
		assertEquals(test.getResult(typeof(string)), RESULT);
		assertThrows(() => test.getResult(typeof(Integer)), typeof(System.InvalidCastException));
	  }

	  public virtual void of_failure()
	  {
		CalculationResult test = CalculationResult.of(1, 2, FAILURE);
		assertEquals(test.RowIndex, 1);
		assertEquals(test.ColumnIndex, 2);
		assertEquals(test.Result, FAILURE);
		assertEquals(test.getResult(typeof(string)), FAILURE);
		assertEquals(test.getResult(typeof(Integer)), FAILURE); // cannot throw exception as generic type not known
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CalculationResult test = CalculationResult.of(1, 2, RESULT);
		coverImmutableBean(test);
		CalculationResult test2 = CalculationResult.of(0, 3, RESULT2);
		coverBeanEquals(test, test2);
		assertNotNull(CalculationResult.meta());
	  }

	}

}
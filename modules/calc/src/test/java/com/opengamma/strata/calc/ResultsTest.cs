using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// Test <seealso cref="Results"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResultsTest
	public class ResultsTest
	{

	  private static readonly ColumnName NAME_A = ColumnName.of("A");
	  private static readonly ColumnName NAME_B = ColumnName.of("B");
	  private static readonly ColumnName NAME_C = ColumnName.of("C");
	  private static readonly ColumnHeader HEADER1 = ColumnHeader.of(NAME_A, TestingMeasures.PRESENT_VALUE);
	  private static readonly ColumnHeader HEADER2 = ColumnHeader.of(NAME_B, TestingMeasures.PRESENT_VALUE);
	  private static readonly ColumnHeader HEADER3 = ColumnHeader.of(NAME_C, TestingMeasures.PRESENT_VALUE);

	  public virtual void test_empty()
	  {
		Results test = Results.of(ImmutableList.of(), ImmutableList.of());
		assertEquals(test.Columns, ImmutableList.of());
		assertEquals(test.RowCount, 0);
		assertEquals(test.ColumnCount, 0);
		assertThrows(() => test.get(0, 0), typeof(System.ArgumentException), "Row index must be greater than or.*");
		assertThrows(() => test.get(0, 0, typeof(string)), typeof(System.ArgumentException), "Row index must be greater than or.*");
		assertThrows(() => test.get(0, NAME_A), typeof(System.ArgumentException), "Column name not found.*");
		assertThrows(() => test.get(0, NAME_A, typeof(string)), typeof(System.ArgumentException), "Column name not found.*");
	  }

	  public virtual void nonEmpty()
	  {
		Results test = Results.of(ImmutableList.of(HEADER1, HEADER2, HEADER3), results("1", "2", "3", "4", "5", "6"));
		assertEquals(test.Columns, ImmutableList.of(HEADER1, HEADER2, HEADER3));
		assertEquals(test.RowCount, 2);
		assertEquals(test.ColumnCount, 3);
		assertEquals(test.get(0, 0).Value, "1");
		assertEquals(test.get(0, 0, typeof(string)).Value, "1");
		assertEquals(test.get(0, NAME_A).Value, "1");
		assertEquals(test.get(0, NAME_A, typeof(string)).Value, "1");
		assertEquals(test.get(0, NAME_B).Value, "2");
		assertEquals(test.get(0, NAME_B, typeof(string)).Value, "2");
		assertEquals(test.get(1, 2).Value, "6");
		assertThrows(() => test.get(-1, 0), typeof(System.ArgumentException), "Row index must be greater than or.*");
		assertThrows(() => test.get(2, 0), typeof(System.ArgumentException), "Row index must be greater than or.*");
		assertThrows(() => test.get(0, -1), typeof(System.ArgumentException), "Column index must be greater than or.*");
		assertThrows(() => test.get(0, 3), typeof(System.ArgumentException), "Column index must be greater than or.*");
		assertThrows(() => test.get(0, 0, typeof(Integer)), typeof(System.InvalidCastException), "Result queried with type 'java.lang.Integer' but was 'java.lang.String'");
		assertThrows(() => test.get(0, NAME_A, typeof(Integer)), typeof(System.InvalidCastException), "Result queried with type 'java.lang.Integer' but was 'java.lang.String'");
	  }

	  /// <summary>
	  /// Tests that it's not possible to create results with invalid combinations of row and column
	  /// count and number of items
	  /// </summary>
	  public virtual void createInvalid()
	  {
		// Zero columns, non-zero cells
		assertThrowsIllegalArg(() => Results.of(ImmutableList.of(), results(1)), "The number of cells.*");
		// More columns than cells
		assertThrowsIllegalArg(() => Results.of(ImmutableList.of(HEADER1, HEADER2, HEADER3), results(1)), "The number of cells.*");
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs private static <T> java.util.List<com.opengamma.strata.collect.result.Result<T>> results(T... items)
	  private static IList<Result<T>> results<T>(params T[] items)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return java.util.items.Select(Result.success).collect(toImmutableList());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		Results test = Results.of(ImmutableList.of(HEADER1, HEADER2, HEADER3), results(1, 2, 3, 4, 5, 6));
		coverImmutableBean(test);
		Results test2 = Results.of(ImmutableList.of(HEADER1), results(9));
		coverBeanEquals(test, test2);
	  }

	}

}
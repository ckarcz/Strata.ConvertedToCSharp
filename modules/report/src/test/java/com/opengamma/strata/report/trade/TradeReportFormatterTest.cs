using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.trade
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ArrayTable = com.google.common.collect.ArrayTable;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// Test <seealso cref="TradeReportFormatter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradeReportFormatterTest
	public class TradeReportFormatterTest
	{

	  private static readonly ImmutableList<int> INDICES = ImmutableList.of(0, 1);

	  public virtual void getColumnTypes()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ArrayTable<int, int, com.opengamma.strata.collect.result.Result<?>> table = com.google.common.collect.ArrayTable.create(INDICES, INDICES);
		ArrayTable<int, int, Result<object>> table = ArrayTable.create(INDICES, INDICES);
		table.put(0, 0, Result.success(1));
		table.put(0, 1, Result.success("abc"));
		table.put(1, 0, Result.success(2));
		table.put(1, 1, Result.success("def"));

		IList<Type> columnTypes = TradeReportFormatter.INSTANCE.getColumnTypes(report(table));
		assertThat(columnTypes).isEqualTo(ImmutableList.of(typeof(Integer), typeof(string)));
	  }

	  public virtual void getColumnTypesWithSomeFailures()
	  {
		ImmutableList<int> indices = ImmutableList.of(0, 1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ArrayTable<int, int, com.opengamma.strata.collect.result.Result<?>> table = com.google.common.collect.ArrayTable.create(indices, indices);
		ArrayTable<int, int, Result<object>> table = ArrayTable.create(indices, indices);
		table.put(0, 0, Result.failure(FailureReason.ERROR, "fail"));
		table.put(0, 1, Result.failure(FailureReason.ERROR, "fail"));
		table.put(1, 0, Result.success(2));
		table.put(1, 1, Result.success("def"));

		IList<Type> columnTypes = TradeReportFormatter.INSTANCE.getColumnTypes(report(table));
		assertThat(columnTypes).isEqualTo(ImmutableList.of(typeof(Integer), typeof(string)));
	  }

	  public virtual void getColumnTypesWithAllFailures()
	  {
		ImmutableList<int> indices = ImmutableList.of(0, 1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ArrayTable<int, int, com.opengamma.strata.collect.result.Result<?>> table = com.google.common.collect.ArrayTable.create(indices, indices);
		ArrayTable<int, int, Result<object>> table = ArrayTable.create(indices, indices);
		table.put(0, 0, Result.failure(FailureReason.ERROR, "fail"));
		table.put(0, 1, Result.failure(FailureReason.ERROR, "fail"));
		table.put(1, 0, Result.failure(FailureReason.ERROR, "fail"));
		table.put(1, 1, Result.failure(FailureReason.ERROR, "fail"));

		IList<Type> columnTypes = TradeReportFormatter.INSTANCE.getColumnTypes(report(table));
		assertThat(columnTypes).isEqualTo(ImmutableList.of(typeof(object), typeof(object)));
	  }

	  private TradeReport report<T1>(ArrayTable<T1> table)
	  {
		return TradeReport.builder().columns(TradeReportColumn.builder().header("col0").build(), TradeReportColumn.builder().header("col1").build()).data(table).valuationDate(LocalDate.now(ZoneOffset.UTC)).runInstant(Instant.now()).build();
	  }
	}

}
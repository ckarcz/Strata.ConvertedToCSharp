using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.trade
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableTable = com.google.common.collect.ImmutableTable;
	using Column = com.opengamma.strata.calc.Column;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ValuePathEvaluator = com.opengamma.strata.report.framework.expression.ValuePathEvaluator;

	/// <summary>
	/// Report runner for trade reports.
	/// <para>
	/// Trade reports are driven by a <seealso cref="TradeReportTemplate trade report template"/>.
	/// The resulting report is a table containing one row per trade, and the requested columns each
	/// showing a value for that trade.
	/// </para>
	/// </summary>
	public sealed class TradeReportRunner : ReportRunner<TradeReportTemplate>
	{

	  /// <summary>
	  /// The single shared instance of this report runner.
	  /// </summary>
	  public static readonly TradeReportRunner INSTANCE = new TradeReportRunner();

	  // restricted constructor
	  private TradeReportRunner()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public ReportRequirements requirements(TradeReportTemplate reportTemplate)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Column> measureRequirements = reportTemplate.Columns.Select(TradeReportColumn::getValue).flatMap(Guavate.stream).Select(ValuePathEvaluator.measure).flatMap(Guavate.stream).Select(Column.of).collect(toImmutableList());

		return ReportRequirements.of(measureRequirements);
	  }

	  public TradeReport runReport(ReportCalculationResults results, TradeReportTemplate reportTemplate)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableTable.Builder<int, int, com.opengamma.strata.collect.result.Result<?>> resultTable = com.google.common.collect.ImmutableTable.builder();
		ImmutableTable.Builder<int, int, Result<object>> resultTable = ImmutableTable.builder();

		for (int reportColumnIdx = 0; reportColumnIdx < reportTemplate.Columns.Count; reportColumnIdx++)
		{
		  TradeReportColumn reportColumn = reportTemplate.Columns[reportColumnIdx];
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> columnResults;
		  IList<Result<object>> columnResults;

		  if (reportColumn.Value.Present)
		  {
			columnResults = ValuePathEvaluator.evaluate(reportColumn.Value.get(), results);
		  }
		  else
		  {
			columnResults = IntStream.range(0, results.Targets.Count).mapToObj(i => Result.failure(FailureReason.INVALID, "No value specified in report template")).collect(toImmutableList());
		  }
		  int rowCount = results.CalculationResults.RowCount;

		  for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
		  {
			resultTable.put(rowIdx, reportColumnIdx, columnResults[rowIdx]);
		  }
		}

		return TradeReport.builder().runInstant(Instant.now()).valuationDate(results.ValuationDate).columns(reportTemplate.Columns).data(resultTable.build()).build();
	  }

	}

}
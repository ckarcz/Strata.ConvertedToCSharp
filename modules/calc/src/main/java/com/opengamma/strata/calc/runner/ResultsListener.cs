using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using Result = com.opengamma.strata.collect.result.Result;

	/// <summary>
	/// Calculation listener that receives the results of individual calculations and builds a set of <seealso cref="Results"/>.
	/// </summary>
	public sealed class ResultsListener : AggregatingCalculationListener<Results>
	{

	  /// <summary>
	  /// Comparator for sorting the results by row and then column. </summary>
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
	  private static readonly IComparer<CalculationResult> COMPARATOR = System.Collections.IComparer.comparingInt(CalculationResult::getRowIndex).thenComparingInt(CalculationResult::getColumnIndex);

	  /// <summary>
	  /// List that is populated with the results as they arrive. </summary>
	  private readonly IList<CalculationResult> results = new List<CalculationResult>();

	  /// <summary>
	  /// The columns that define what values are calculated. </summary>
	  private IList<Column> columns;

	  /// <summary>
	  /// Creates a new instance.
	  /// </summary>
	  public ResultsListener()
	  {
	  }

	  public override void calculationsStarted(IList<CalculationTarget> targets, IList<Column> columns)
	  {
		this.columns = ImmutableList.copyOf(columns);
	  }

	  public override void resultReceived(CalculationTarget target, CalculationResult result)
	  {
		results.Add(result);
	  }

	  protected internal override Results createAggregateResult()
	  {
		results.sort(COMPARATOR);
		return buildResults(results, columns);
	  }

	  /// <summary>
	  /// Builds a set of results from the results of the individual calculations.
	  /// </summary>
	  /// <param name="calculationResults"> the results of the individual calculations </param>
	  /// <param name="columns"> the columns that define what values are calculated </param>
	  /// <returns> the results </returns>
	  private static Results buildResults(IList<CalculationResult> calculationResults, IList<Column> columns)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> results = calculationResults.stream().map(CalculationResult::getResult).collect(toImmutableList());
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Result<object>> results = calculationResults.Select(CalculationResult::getResult).collect(toImmutableList());

//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<ColumnHeader> headers = columns.Select(Column::toHeader).collect(toImmutableList());

		return Results.of(headers, results);
	  }
	}

}
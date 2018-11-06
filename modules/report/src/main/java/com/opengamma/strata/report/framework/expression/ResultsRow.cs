using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ResolvableCalculationTarget = com.opengamma.strata.basics.ResolvableCalculationTarget;
	using Column = com.opengamma.strata.calc.Column;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using Position = com.opengamma.strata.product.Position;
	using Product = com.opengamma.strata.product.Product;
	using ProductTrade = com.opengamma.strata.product.ProductTrade;
	using Security = com.opengamma.strata.product.Security;
	using SecurityTrade = com.opengamma.strata.product.SecurityTrade;
	using Trade = com.opengamma.strata.product.Trade;

	/// <summary>
	/// Wraps a set of <seealso cref="ReportCalculationResults"/> and exposes the contents of a single row.
	/// </summary>
	internal class ResultsRow
	{

	  /// <summary>
	  /// The results used to generate a report. </summary>
	  private readonly ReportCalculationResults results;

	  /// <summary>
	  /// The index of the row in the result whose data is exposed by this object. </summary>
	  private readonly int rowIndex;

	  /// <summary>
	  /// Returns a new instance exposing the data from a single row in the results.
	  /// </summary>
	  /// <param name="results">  the results used to generate a report </param>
	  /// <param name="rowIndex">  the index of the row in the result whose data is exposed by this object </param>
	  internal ResultsRow(ReportCalculationResults results, int rowIndex)
	  {
		this.results = results;
		this.rowIndex = rowIndex;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the target from the row.
	  /// </summary>
	  /// <returns> the target from the row </returns>
	  internal virtual CalculationTarget Target
	  {
		  get
		  {
			return results.Targets[rowIndex];
		  }
	  }

	  /// <summary>
	  /// Returns the position from the row.
	  /// </summary>
	  /// <returns> the position from the row </returns>
	  internal virtual Result<Position> Position
	  {
		  get
		  {
			CalculationTarget target = Target;
			if (target is Position)
			{
			  return Result.success((Position) target);
			}
			return Result.failure(FailureReason.INVALID, "Calculaton target is not a position");
		  }
	  }

	  /// <summary>
	  /// Returns the trade from the row.
	  /// </summary>
	  /// <returns> the trade from the row </returns>
	  internal virtual Result<Trade> Trade
	  {
		  get
		  {
			CalculationTarget target = Target;
			if (target is Trade)
			{
			  return Result.success((Trade) target);
			}
			return Result.failure(FailureReason.INVALID, "Calculaton target is not a trade");
		  }
	  }

	  /// <summary>
	  /// Returns the product from the row.
	  /// <para>
	  /// This returns a successful result where the trade associated with the row
	  /// implements <seealso cref="ProductTrade"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the product from the row </returns>
	  internal virtual Result<Product> Product
	  {
		  get
		  {
			CalculationTarget target = Target;
			if (target is ResolvableCalculationTarget)
			{
			  ResolvableCalculationTarget idTrade = (ResolvableCalculationTarget) target;
			  target = idTrade.resolveTarget(results.ReferenceData);
			}
			if (target is ProductTrade)
			{
			  return Result.success(((ProductTrade) target).Product);
			}
			if (target is Trade)
			{
			  return Result.failure(FailureReason.INVALID, "Trade does not contain a product");
			}
			return Result.failure(FailureReason.INVALID, "Calculaton target is not a trade");
		  }
	  }

	  /// <summary>
	  /// Returns the security from the row.
	  /// <para>
	  /// This returns a successful result where the trade associated with the row
	  /// implements <seealso cref="GenericSecurityTrade"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the security from the row </returns>
	  internal virtual Result<Security> Security
	  {
		  get
		  {
			CalculationTarget target = Target;
			if (target is SecurityTrade)
			{
			  SecurityTrade secTrade = (SecurityTrade) target;
			  Security security = results.ReferenceData.getValue(secTrade.SecurityId);
			  return Result.success(security);
			}
			if (target is GenericSecurityTrade)
			{
			  GenericSecurityTrade secTrade = (GenericSecurityTrade) target;
			  return Result.success(secTrade.Security);
			}
			if (target is Trade)
			{
			  return Result.failure(FailureReason.INVALID, "Trade does not contain a security");
			}
			return Result.failure(FailureReason.INVALID, "Calculaton target is not a trade");
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the result of calculating the named measure for the trade in the row.
	  /// </summary>
	  /// <param name="measureName">  the name of the measure </param>
	  /// <returns> the result of calculating the named measure for the trade in the row </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> getResult(String measureName)
	  internal virtual Result<object> getResult(string measureName)
	  {
		IList<string> validMeasureNames = measureNames(results.Targets[rowIndex], results.CalculationFunctions);
		if (!validMeasureNames.Contains(measureName))
		{
		  return Result.failure(FailureReason.INVALID, "Invalid measure name: {}. Valid measure names: {}", measureName, validMeasureNames);
		}
		try
		{
		  Column column = Column.of(Measure.of(measureName));
		  int columnIndex = results.Columns.IndexOf(column);
		  if (columnIndex == -1)
		  {
			return Result.failure(FailureReason.INVALID, "Measure not found in results: '{}'. Valid measure names: {}", measureName, validMeasureNames);
		  }
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = results.getCalculationResults().get(rowIndex, columnIndex);
		  Result<object> result = results.CalculationResults.get(rowIndex, columnIndex);
		  if (result.Failure && result.Failure.Reason == FailureReason.ERROR)
		  {
			return Result.failure(FailureReason.INVALID, "Unable to calculate measure '{}'. Reason: {}", measureName, validMeasureNames, result.Failure.Message);
		  }
		  return result;

		}
		catch (System.ArgumentException ex)
		{
		  return Result.failure(FailureReason.INVALID, "Unable to calculate measure '{}'. Reason: {}. Valid measure names: {}", measureName, ex.Message, validMeasureNames);
		}
	  }

	  // determine the available measures
	  internal static IList<string> measureNames(CalculationTarget target, CalculationFunctions calculationFunctions)
	  {
		ISet<Measure> validMeasures = calculationFunctions.findFunction(target).map(fn => fn.supportedMeasures()).orElse(ImmutableSet.of());
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return validMeasures.Select(Measure::getName).OrderBy(c => c).collect(toImmutableList());
	  }

	}

}
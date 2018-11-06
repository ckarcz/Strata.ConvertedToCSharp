using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioFxConvertible = com.opengamma.strata.data.scenario.ScenarioFxConvertible;
	using ScenarioFxRateProvider = com.opengamma.strata.data.scenario.ScenarioFxRateProvider;

	/// <summary>
	/// A single cell within a calculation task.
	/// <para>
	/// Each <seealso cref="CalculationTask"/> calculates a result for one or more cells.
	/// This class capture details of each cell.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class CalculationTaskCell implements org.joda.beans.ImmutableBean
	public sealed class CalculationTaskCell : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final int rowIndex;
		private readonly int rowIndex;
	  /// <summary>
	  /// The column index of the cell in the results grid.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final int columnIndex;
	  private readonly int columnIndex;
	  /// <summary>
	  /// The measure to be calculated.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.Measure measure;
	  private readonly Measure measure;
	  /// <summary>
	  /// The reporting currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.ReportingCurrency reportingCurrency;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ReportingCurrency reportingCurrency_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance, specifying the cell indices, measure and reporting currency.
	  /// <para>
	  /// The result will contain no calculation parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rowIndex">  the row index </param>
	  /// <param name="columnIndex">  the column index </param>
	  /// <param name="measure">  the measure to calculate </param>
	  /// <param name="reportingCurrency">  the reporting currency </param>
	  /// <returns> the cell </returns>
	  public static CalculationTaskCell of(int rowIndex, int columnIndex, Measure measure, ReportingCurrency reportingCurrency)
	  {

		return new CalculationTaskCell(rowIndex, columnIndex, measure, reportingCurrency);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Determines the reporting currency.
	  /// <para>
	  /// The reporting currency is specified using <seealso cref="ReportingCurrency"/>.
	  /// If the currency is defined to be the "natural" currency, then the function
	  /// is used to determine the natural currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="task">  the calculation task </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the reporting currency </returns>
	  internal Currency reportingCurrency(CalculationTask task, ReferenceData refData)
	  {
		if (reportingCurrency_Renamed.Specific)
		{
		  return reportingCurrency_Renamed.Currency;
		}
		// this should never throw an exception, because it is only called if the measure is currency-convertible
		return task.naturalCurrency(refData);
	  }

	  /// <summary>
	  /// Creates the result from the map of calculated measures.
	  /// <para>
	  /// This extracts the calculated measure and performs currency conversion if necessary.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="task">  the calculation task </param>
	  /// <param name="target">  the target of the calculation </param>
	  /// <param name="results">  the map of result by measure </param>
	  /// <param name="fxProvider">  the market data </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the calculation result </returns>
	  internal CalculationResult createResult<T1>(CalculationTask task, CalculationTarget target, IDictionary<T1> results, ScenarioFxRateProvider fxProvider, ReferenceData refData)
	  {

		// caller expects that this method does not throw an exception
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> calculated = results.get(measure);
		Result<object> calculated = results[measure];
		if (calculated == null)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  calculated = Result.failure(FailureReason.CALCULATION_FAILED, "Measure '{}' was not calculated by the function for target type '{}'", measure, target.GetType().FullName);
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = convertCurrencyIfNecessary(task, calculated, fxProvider, refData);
		Result<object> result = convertCurrencyIfNecessary(task, calculated, fxProvider, refData);
		return CalculationResult.of(rowIndex, columnIndex, result);
	  }

	  // converts the value, if appropriate
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> convertCurrencyIfNecessary(CalculationTask task, com.opengamma.strata.collect.result.Result<?> result, com.opengamma.strata.data.scenario.ScenarioFxRateProvider fxProvider, com.opengamma.strata.basics.ReferenceData refData)
	  private Result<object> convertCurrencyIfNecessary<T1>(CalculationTask task, Result<T1> result, ScenarioFxRateProvider fxProvider, ReferenceData refData)
	  {

		// the result is only converted if it is a success and both the measure and value are convertible
		if (measure.CurrencyConvertible && !reportingCurrency_Renamed.None && result.Success && result.Value is ScenarioFxConvertible)
		{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.ScenarioFxConvertible<?> convertible = (com.opengamma.strata.data.scenario.ScenarioFxConvertible<?>) result.getValue();
		  ScenarioFxConvertible<object> convertible = (ScenarioFxConvertible<object>) result.Value;
		  return convertCurrency(task, convertible, fxProvider, refData);
		}
		return result;
	  }

	  // converts the value
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.collect.result.Result<?> convertCurrency(CalculationTask task, com.opengamma.strata.data.scenario.ScenarioFxConvertible<?> value, com.opengamma.strata.data.scenario.ScenarioFxRateProvider fxProvider, com.opengamma.strata.basics.ReferenceData refData)
	  private Result<object> convertCurrency<T1>(CalculationTask task, ScenarioFxConvertible<T1> value, ScenarioFxRateProvider fxProvider, ReferenceData refData)
	  {

		Currency resolvedReportingCurrency = reportingCurrency(task, refData);
		try
		{
		  return Result.success(value.convertedTo(resolvedReportingCurrency, fxProvider));
		}
		catch (Exception ex)
		{
		  return Result.failure(FailureReason.CURRENCY_CONVERSION, ex, "Failed to convert value '{}' to currency '{}'", value, resolvedReportingCurrency);
		}
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Messages.format("CalculationTaskCell[({}, {}), measure={}, currency={}]", rowIndex, columnIndex, measure, reportingCurrency_Renamed);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationTaskCell}.
	  /// </summary>
	  private static readonly TypedMetaBean<CalculationTaskCell> META_BEAN = LightMetaBean.of(typeof(CalculationTaskCell), MethodHandles.lookup(), new string[] {"rowIndex", "columnIndex", "measure", "reportingCurrency"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code CalculationTaskCell}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<CalculationTaskCell> meta()
	  {
		return META_BEAN;
	  }

	  static CalculationTaskCell()
	  {
		MetaBean.register(META_BEAN);
	  }

	  private CalculationTaskCell(int rowIndex, int columnIndex, Measure measure, ReportingCurrency reportingCurrency)
	  {
		ArgChecker.notNegative(rowIndex, "rowIndex");
		ArgChecker.notNegative(columnIndex, "columnIndex");
		JodaBeanUtils.notNull(measure, "measure");
		JodaBeanUtils.notNull(reportingCurrency, "reportingCurrency");
		this.rowIndex = rowIndex;
		this.columnIndex = columnIndex;
		this.measure = measure;
		this.reportingCurrency_Renamed = reportingCurrency;
	  }

	  public override TypedMetaBean<CalculationTaskCell> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the row index of the cell in the results grid. </summary>
	  /// <returns> the value of the property </returns>
	  public int RowIndex
	  {
		  get
		  {
			return rowIndex;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the column index of the cell in the results grid. </summary>
	  /// <returns> the value of the property </returns>
	  public int ColumnIndex
	  {
		  get
		  {
			return columnIndex;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the measure to be calculated. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Measure Measure
	  {
		  get
		  {
			return measure;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reporting currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ReportingCurrency ReportingCurrency
	  {
		  get
		  {
			return reportingCurrency_Renamed;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  CalculationTaskCell other = (CalculationTaskCell) obj;
		  return (rowIndex == other.rowIndex) && (columnIndex == other.columnIndex) && JodaBeanUtils.equal(measure, other.measure) && JodaBeanUtils.equal(reportingCurrency_Renamed, other.reportingCurrency_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rowIndex);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(columnIndex);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(measure);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(reportingCurrency_Renamed);
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}
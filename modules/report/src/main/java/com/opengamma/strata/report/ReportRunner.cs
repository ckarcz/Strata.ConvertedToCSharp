/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{
	/// <summary>
	/// Runs a report for a specific template type.
	/// <para>
	/// A report is a transformation from trade and/or aggregate calculation results into a
	/// specific business format.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the report template </param>
	public interface ReportRunner<T> where T : ReportTemplate
	{

	  /// <summary>
	  /// Gets a description of the requirements to run a report for the given template.
	  /// Requirements include trade-level measures.
	  /// <para>
	  /// The report may be run on calculation results including at least these requirements.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="reportTemplate">  the report template </param>
	  /// <returns> the requirements to run the report </returns>
	  ReportRequirements requirements(T reportTemplate);

	  /// <summary>
	  /// Runs a report from a set of calculation results.
	  /// The contents of the report are dictated by the template provided.
	  /// The calculation results may be substantially more complete than the template requires.
	  /// </summary>
	  /// <param name="calculationResults">  the calculation results </param>
	  /// <param name="reportTemplate">  the report template </param>
	  /// <returns>  the report </returns>
	  Report runReport(ReportCalculationResults calculationResults, T reportTemplate);

	}

}
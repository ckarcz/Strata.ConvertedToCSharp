using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Messages = com.opengamma.strata.collect.Messages;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using PropertySet = com.opengamma.strata.collect.io.PropertySet;
	using CashFlowReportTemplateIniLoader = com.opengamma.strata.report.cashflow.CashFlowReportTemplateIniLoader;
	using TradeReportTemplateIniLoader = com.opengamma.strata.report.trade.TradeReportTemplateIniLoader;

	/// <summary>
	/// Loads report templates from ini files by delegating to specific loaders for the different report types.
	/// </summary>
	internal sealed class MasterReportTemplateIniLoader
	{

	  /// <summary>
	  /// The known report template loaders.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Set<ReportTemplateIniLoader<? extends ReportTemplate>> LOADERS = com.google.common.collect.ImmutableSet.of(new com.opengamma.strata.report.trade.TradeReportTemplateIniLoader(), new com.opengamma.strata.report.cashflow.CashFlowReportTemplateIniLoader());
	  private static readonly ISet<ReportTemplateIniLoader<ReportTemplate>> LOADERS = ImmutableSet.of(new TradeReportTemplateIniLoader(), new CashFlowReportTemplateIniLoader());

	  // restricted constructor
	  private MasterReportTemplateIniLoader()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Loads a report template from an .ini file.
	  /// </summary>
	  /// <param name="iniFile">  the .ini file containing the definition of a report template </param>
	  /// <returns> the template defined in the .ini file </returns>
	  /// <exception cref="RuntimeException"> if the ini file cannot be parsed </exception>
	  public static ReportTemplate load(IniFile iniFile)
	  {
		string settingsSectionKey = iniFile.sections().Where(k => k.ToLower(Locale.ENGLISH).Equals(ReportTemplateIniLoader_Fields.SETTINGS_SECTION)).First().orElseThrow(() => new System.ArgumentException(Messages.format("Report template INI file must contain a {} section", ReportTemplateIniLoader_Fields.SETTINGS_SECTION)));
		PropertySet settingsSection = iniFile.section(settingsSectionKey);
		string reportType = settingsSection.value(ReportTemplateIniLoader_Fields.SETTINGS_REPORT_TYPE);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ReportTemplateIniLoader<? extends ReportTemplate> iniLoader = LOADERS.stream().filter(loader -> loader.getReportType().equalsIgnoreCase(reportType)).findFirst().orElseThrow(() -> new IllegalArgumentException(com.opengamma.strata.collect.Messages.format("Unsupported report type: {}", reportType)));
		ReportTemplateIniLoader<ReportTemplate> iniLoader = LOADERS.Where(loader => loader.ReportType.equalsIgnoreCase(reportType)).First().orElseThrow(() => new System.ArgumentException(Messages.format("Unsupported report type: {}", reportType)));
		return iniLoader.load(iniFile);
	  }

	}

}
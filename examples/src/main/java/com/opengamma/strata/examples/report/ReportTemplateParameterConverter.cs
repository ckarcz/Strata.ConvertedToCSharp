using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{

	using IStringConverter = com.beust.jcommander.IStringConverter;
	using ParameterException = com.beust.jcommander.ParameterException;
	using CharSource = com.google.common.io.CharSource;
	using Messages = com.opengamma.strata.collect.Messages;
	using IniFile = com.opengamma.strata.collect.io.IniFile;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ReportTemplate = com.opengamma.strata.report.ReportTemplate;

	/// <summary>
	/// Parameter converter for <seealso cref="ReportTemplate"/>.
	/// </summary>
	public class ReportTemplateParameterConverter : IStringConverter<ReportTemplate>
	{

	  public override ReportTemplate convert(string fileName)
	  {
		try
		{
		  File file = new File(fileName);
		  CharSource charSource = ResourceLocator.ofFile(file).CharSource;
		  IniFile ini = IniFile.of(charSource);

		  return ReportTemplate.load(ini);

		}
		catch (Exception ex)
		{
		  if (ex.InnerException is FileNotFoundException)
		  {
			throw new ParameterException(Messages.format("File not found: {}", fileName));
		  }
		  throw new ParameterException(Messages.format("Invalid report template file: {}" + Environment.NewLine + "Exception: {}", fileName, ex.Message));
		}
	  }

	}

}
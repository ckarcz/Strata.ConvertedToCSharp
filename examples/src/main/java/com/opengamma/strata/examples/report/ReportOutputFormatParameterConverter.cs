using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.report
{

	using IStringConverter = com.beust.jcommander.IStringConverter;
	using ReportOutputFormat = com.opengamma.strata.report.framework.format.ReportOutputFormat;

	/// <summary>
	/// Parameter converter for <seealso cref="ReportOutputFormat"/>.
	/// <para>
	/// This parses the input leniently.
	/// </para>
	/// </summary>
	public class ReportOutputFormatParameterConverter : IStringConverter<ReportOutputFormat>
	{

	  public override ReportOutputFormat convert(string value)
	  {
		if (value.ToLower(Locale.ENGLISH).StartsWith("c", StringComparison.Ordinal))
		{
		  return ReportOutputFormat.CSV;
		}
		return ReportOutputFormat.ASCII_TABLE;
	  }

	}

}
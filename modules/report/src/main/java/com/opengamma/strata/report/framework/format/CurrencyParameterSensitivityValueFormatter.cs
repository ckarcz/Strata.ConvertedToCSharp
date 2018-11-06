using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{

	using Strings = com.google.common.@base.Strings;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Formatter for currency parameter sensitivity.
	/// </summary>
	internal sealed class CurrencyParameterSensitivityValueFormatter : ValueFormatter<CurrencyParameterSensitivity>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly CurrencyParameterSensitivityValueFormatter INSTANCE = new CurrencyParameterSensitivityValueFormatter();

	  private const int PADDED_FIELD_WIDTH = 15;

	  private readonly DoubleValueFormatter doubleFormatter = DoubleValueFormatter.INSTANCE;

	  // restricted constructor
	  private CurrencyParameterSensitivityValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public string formatForCsv(CurrencyParameterSensitivity sensitivity)
	  {
		return getSensitivityString(sensitivity, doubleFormatter.formatForCsv, false);
	  }

	  public string formatForDisplay(CurrencyParameterSensitivity sensitivity)
	  {
		return getSensitivityString(sensitivity, doubleFormatter.formatForDisplay, true);
	  }

	  private string getSensitivityString(CurrencyParameterSensitivity sensitivity, System.Func<double, string> formatFn, bool pad)
	  {

		StringBuilder sb = new StringBuilder();
		IList<ParameterMetadata> parameterMetadata = sensitivity.ParameterMetadata;
		System.Func<int, string> labelProvider = i => Objects.ToString(Strings.emptyToNull(parameterMetadata[i].Label), (i + 1).ToString());

		for (int i = 0; i < sensitivity.Sensitivity.size(); i++)
		{
		  string formattedSensitivity = formatFn(sensitivity.Sensitivity.get(i));
		  string field = labelProvider(i) + " = " + formattedSensitivity;
		  if (pad)
		  {
			field = Strings.padEnd(field, PADDED_FIELD_WIDTH, ' ');
		  }
		  sb.Append(field);
		  if (i < sensitivity.Sensitivity.size() - 1)
		  {
			sb.Append(" | ");
		  }
		}
		return sb.ToString();
	  }

	}

}
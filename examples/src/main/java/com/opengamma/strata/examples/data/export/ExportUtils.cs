using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.data.export
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Files = com.google.common.io.Files;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Unchecked = com.opengamma.strata.collect.Unchecked;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using SwaptionSurfaceExpiryTenorParameterMetadata = com.opengamma.strata.pricer.swaption.SwaptionSurfaceExpiryTenorParameterMetadata;

	/// <summary>
	/// Utilities to export objects (in csv files or in the console). Typically used in the tutorials.
	/// </summary>
	public class ExportUtils
	{

	  /// <summary>
	  /// Exports a <seealso cref="MultiCurrencyAmount"/> to a csv file.
	  /// </summary>
	  /// <param name="multiCurrencyAmount">  the amount </param>
	  /// <param name="fileName">  the file name </param>
	  public static void export(MultiCurrencyAmount multiCurrencyAmount, string fileName)
	  {
		StringBuilder builder = new StringBuilder();
		foreach (CurrencyAmount ca in multiCurrencyAmount.Amounts)
		{
		  builder.Append(ca.Currency.ToString()).Append(',').Append(ca.Amount).Append(',');
		}
		export(builder.ToString(), fileName);
	  }

	  /// <summary>
	  /// Exports into a csv file a <seealso cref="CurrencyParameterSensitivity"/>, which is the sensitivity with respect to
	  /// a unique curve or surface. 
	  /// <para>
	  /// In the export the figures are often scaled to match market conventions, for examples a one basis point 
	  /// scaling for interest rate curves. The factor can be provided and will apply to all points of the sensitivity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity object </param>
	  /// <param name="scale">  the scaling factor </param>
	  /// <param name="fileName">  the file name for the export </param>
	  public static void export(CurrencyParameterSensitivity sensitivity, double scale, string fileName)
	  {

		ArgChecker.isTrue(sensitivity.ParameterMetadata.size() > 0, "Parameter metadata must be present");
		DoubleArray s = sensitivity.Sensitivity;
		IList<ParameterMetadata> pmdl = sensitivity.ParameterMetadata;
		int nbPts = sensitivity.Sensitivity.size();
		string output = "Expiry, Tenor, Label, Value\n";
		for (int looppts = 0; looppts < nbPts; looppts++)
		{
		  ArgChecker.isTrue(pmdl[looppts] is SwaptionSurfaceExpiryTenorParameterMetadata, "tenor expiry");
		  SwaptionSurfaceExpiryTenorParameterMetadata pmd = (SwaptionSurfaceExpiryTenorParameterMetadata) pmdl[looppts];
		  double sens = s.get(looppts) * scale;
		  output = output + pmd.YearFraction + ", " + pmd.Tenor + ", " + pmd.Label + ", " + sens + "\n";
		}
		export(output, fileName);
	  }

	  /// <summary>
	  /// Exports into a csv file a <seealso cref="CurrencyParameterSensitivities"/>, which is the sensitivity with respect to
	  /// multiple curves or surfaces. 
	  /// <para>
	  /// In the export the figures are often scaled to match market conventions, for examples a one basis point 
	  /// scaling for interest rate curves. The factor can be provided and will apply to all points of the sensitivity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity object </param>
	  /// <param name="scale">  the scaling factor </param>
	  /// <param name="fileName">  the file name for the export </param>
	  public static void export(CurrencyParameterSensitivities sensitivity, double scale, string fileName)
	  {

		ImmutableList<CurrencyParameterSensitivity> sl = sensitivity.Sensitivities;
		string output = "Label, Value\n";
		foreach (CurrencyParameterSensitivity s in sl)
		{
		  output = output + s.MarketDataName.ToString() + ", " + s.Currency.ToString() + "\n";
		  ArgChecker.isTrue(s.ParameterMetadata.size() > 0, "Parameters metadata required");
		  DoubleArray sa = s.Sensitivity;
		  IList<ParameterMetadata> pmd = s.ParameterMetadata;
		  for (int loopnode = 0; loopnode < sa.size(); loopnode++)
		  {
			output = output + pmd[loopnode].Label + ", " + (sa.get(loopnode) * scale) + "\n";
		  }
		}
		export(output, fileName);
	  }

	  /// <summary>
	  /// Exports a string to a file. Useful in particular for XML and beans.
	  /// </summary>
	  /// <param name="string">  the string to export </param>
	  /// <param name="fileName">  the name of the file </param>
	  public static void export(string @string, string fileName)
	  {
		File file = new File(fileName);
		Unchecked.wrap(() => Files.createParentDirs(file));
		Unchecked.wrap(() => Files.asCharSink(file, StandardCharsets.UTF_8).write(@string));
	  }

	}

}
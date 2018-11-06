using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{

	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using CsvOutput = com.opengamma.strata.collect.io.CsvOutput;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenoredParameterMetadata = com.opengamma.strata.market.param.TenoredParameterMetadata;
	using CurveSensitivities = com.opengamma.strata.market.sensitivity.CurveSensitivities;
	using CurveSensitivitiesType = com.opengamma.strata.market.sensitivity.CurveSensitivitiesType;

	/// <summary>
	/// Writes sensitivities to a CSV file.
	/// <para>
	/// This takes a Strata <seealso cref="CurveSensitivities"/> instance and creates a matching CSV file.
	/// The output is written in standard format, with no identifier columns.
	/// The parameter metadata must contain tenors.
	/// </para>
	/// </summary>
	public sealed class SensitivityCsvWriter
	{

	  /// <summary>
	  /// The supplier, providing additional information.
	  /// </summary>
	  private readonly SensitivityCsvInfoSupplier supplier;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
	  public static SensitivityCsvWriter standard()
	  {
		return new SensitivityCsvWriter(SensitivityCsvInfoSupplier.standard());
	  }

	  /// <summary>
	  /// Obtains an instance that uses the specified supplier for additional information.
	  /// </summary>
	  /// <param name="supplier">  the supplier used to extract additional information to output </param>
	  /// <returns> the loader </returns>
	  public static SensitivityCsvWriter of(SensitivityCsvInfoSupplier supplier)
	  {
		return new SensitivityCsvWriter(supplier);
	  }

	  // restricted constructor
	  private SensitivityCsvWriter(SensitivityCsvInfoSupplier supplier)
	  {
		this.supplier = ArgChecker.notNull(supplier, "supplier");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Write sensitivities to an appendable in the standard sensitivities format.
	  /// <para>
	  /// The output is written in standard format, with no identifier columns.
	  /// The parameter metadata must contain tenors.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveSens">  the curve sensitivities to write </param>
	  /// <param name="output">  the appendable to write to </param>
	  /// <exception cref="IllegalArgumentException"> if the metadata does not contain tenors </exception>
	  /// <exception cref="UncheckedIOException"> if an IO error occurs </exception>
	  public void write(CurveSensitivities curveSens, Appendable output)
	  {
		CsvOutput csv = CsvOutput.standard(output, "\n");
		IList<string> additionalHeaders = supplier.headers(curveSens);

		// check for dates
		if (curveSens.TypedSensitivities.values().stream().flatMap(allParamSens => allParamSens.Sensitivities.stream()).flatMap(paramSens => paramSens.ParameterMetadata.stream()).anyMatch(pmd => !(pmd is TenoredParameterMetadata)))
		{
		  throw new System.ArgumentException("Parameter metadata must contain tenors");
		}
		bool containsDates = curveSens.TypedSensitivities.values().stream().flatMap(allParamSens => allParamSens.Sensitivities.stream()).flatMap(paramSens => paramSens.ParameterMetadata.stream()).anyMatch(pmd => pmd is DatedParameterMetadata);

		// headers
		csv.writeCell(SensitivityCsvLoader.REFERENCE_HEADER);
		csv.writeCell(SensitivityCsvLoader.TYPE_HEADER);
		csv.writeCell(SensitivityCsvLoader.TENOR_HEADER);
		if (containsDates)
		{
		  csv.writeCell(SensitivityCsvLoader.DATE_HEADER);
		}
		csv.writeCell(SensitivityCsvLoader.CURRENCY_HEADER);
		csv.writeCell(SensitivityCsvLoader.VALUE_HEADER);
		csv.writeLine(additionalHeaders);

		// content, grouped by reference, then type
		MapStream.of(curveSens.TypedSensitivities).flatMapValues(sens => sens.Sensitivities.stream()).mapKeys((type, sens) => Pair.of(sens.MarketDataName.Name, type)).sortedKeys().forEach((pair, paramSens) => write(pair.First, pair.Second, curveSens, paramSens, additionalHeaders, containsDates, csv));
	  }

	  // writes the rows for a single CurrencyParameterSensitivity
	  private void write(string reference, CurveSensitivitiesType type, CurveSensitivities curveSens, CurrencyParameterSensitivity paramSens, IList<string> additionalHeaders, bool containsDates, CsvOutput csv)
	  {

		IList<string> additionalCells = supplier.values(additionalHeaders, curveSens, paramSens);
		for (int i = 0; i < paramSens.ParameterCount; i++)
		{
		  ParameterMetadata pmd = paramSens.getParameterMetadata(i);
		  Tenor tenor = ((TenoredParameterMetadata) pmd).Tenor;
		  double value = paramSens.Sensitivity.get(i);
		  csv.writeCell(reference);
		  csv.writeCell(type.Name);
		  csv.writeCell(tenor.ToString());
		  if (containsDates)
		  {
			csv.writeCell(pmd is DatedParameterMetadata ? ((DatedParameterMetadata) pmd).Date.ToString() : "");
		  }
		  csv.writeCell(paramSens.Currency.Code);
		  csv.writeCell(decimal.valueOf(value).toPlainString());
		  csv.writeLine(additionalCells);
		}
	  }

	}

}
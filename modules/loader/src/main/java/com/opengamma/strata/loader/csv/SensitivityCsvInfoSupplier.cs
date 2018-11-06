/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using CurveSensitivities = com.opengamma.strata.market.sensitivity.CurveSensitivities;

	/// <summary>
	/// Resolves additional information when parsing sensitivity CSV files.
	/// <para>
	/// Data loaded from a CSV may contain additional information that needs to be captured.
	/// This plugin point allows the additional CSV columns to be parsed and captured.
	/// </para>
	/// </summary>
	public interface SensitivityCsvInfoSupplier
	{

	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SensitivityCsvInfoSupplier standard()
	//  {
	//	return StandardCsvInfoImpl.INSTANCE;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the column header is an info column that this resolver will parse.
	  /// </summary>
	  /// <param name="curveSens">  the curve sensitivities to output </param>
	  /// <returns> the list of additional headers </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.List<String> headers(com.opengamma.strata.market.sensitivity.CurveSensitivities curveSens)
	//  {
	//	return ImmutableList.of();
	//  }

	  /// <summary>
	  /// Gets the values associated with the headers.
	  /// <para>
	  /// This must return a list of the same size as {@code additionalHeaders}
	  /// where each element in the list is the value for the matching header.
	  /// </para>
	  /// <para>
	  /// This will be invoked once for each <seealso cref="CurrencyParameterSensitivity"/> in the <seealso cref="CurveSensitivities"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="additionalHeaders">  the additional headers </param>
	  /// <param name="curveSens">  the curve sensitivities to output </param>
	  /// <param name="paramSens">  the parameter sensitivities to output </param>
	  /// <returns> the value for the specified header, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.List<String> values(java.util.List<String> additionalHeaders, com.opengamma.strata.market.sensitivity.CurveSensitivities curveSens, com.opengamma.strata.market.param.CurrencyParameterSensitivity paramSens)
	//  {
	//
	//	return ImmutableList.of();
	//  }

	}

}
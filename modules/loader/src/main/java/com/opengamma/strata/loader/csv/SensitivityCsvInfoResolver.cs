/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using TenoredParameterMetadata = com.opengamma.strata.market.param.TenoredParameterMetadata;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;

	/// <summary>
	/// Resolves additional information when parsing sensitivity CSV files.
	/// <para>
	/// Data loaded from a CSV may contain additional information that needs to be captured.
	/// This plugin point allows the additional CSV columns to be parsed and captured.
	/// </para>
	/// </summary>
	public interface SensitivityCsvInfoResolver
	{

	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SensitivityCsvInfoResolver standard()
	//  {
	//	return StandardCsvInfoImpl.INSTANCE;
	//  }

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SensitivityCsvInfoResolver of(com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//	return StandardCsvInfoImpl.of(refData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reference data being used.
	  /// </summary>
	  /// <returns> the reference data </returns>
	  ReferenceData ReferenceData {get;}

	  /// <summary>
	  /// Checks if the column header is an info column that this resolver will parse.
	  /// </summary>
	  /// <param name="headerLowerCase">  the header case, in lower case (ENGLISH) form </param>
	  /// <returns> true if the header is for an info column </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isInfoColumn(String headerLowerCase)
	//  {
	//	return false;
	//  }

	  /// <summary>
	  /// Parses attributes to update {@code PortfolioItemInfo}.
	  /// <para>
	  /// If it is available, the sensitivity ID will have been set before this method is called.
	  /// It may be altered if necessary, although this is not recommended.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="info">  the info to update and return </param>
	  /// <returns> the updated info </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.PortfolioItemInfo parseSensitivityInfo(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.PortfolioItemInfo info)
	//  {
	//	// do nothing
	//	return info;
	//  }

	  /// <summary>
	  /// Checks whether a tenor is required.
	  /// <para>
	  /// This defaults to true, ensuring that the metadata will implement <seealso cref="TenoredParameterMetadata"/>.
	  /// Override to set it to false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if the tenor is required, false to allow date-based sensitvities </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isTenorRequired()
	//  {
	//	return true;
	//  }

	  /// <summary>
	  /// Checks the parsed sensitivity tenor, potentially altering the value.
	  /// <para>
	  /// The input is the tenor exactly as parsed.
	  /// The default implementation normalizes it and ensures that it does not contain a combination
	  /// of years/months and days.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor to check and potentially alter </param>
	  /// <returns> the potentially adjusted tenor </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.date.Tenor checkSensitivityTenor(com.opengamma.strata.basics.date.Tenor tenor)
	//  {
	//	Tenor resultTenor = tenor.normalized();
	//	if (resultTenor.getPeriod().toTotalMonths() > 0 && resultTenor.getPeriod().getDays() > 0)
	//	{
	//	  throw new IllegalArgumentException("Invalid tenor, cannot mix years/months and days: " + tenor);
	//	}
	//	return resultTenor;
	//  }

	  /// <summary>
	  /// Checks the parsed curve name, potentially altering the value.
	  /// <para>
	  /// The input is the curve name exactly as parsed.
	  /// The default implementation simply returns it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveName">  the curve name to check and potentially alter </param>
	  /// <returns> the potentially adjusted curve name </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.curve.CurveName checkCurveName(com.opengamma.strata.market.curve.CurveName curveName)
	//  {
	//	return curveName;
	//  }

	}

}
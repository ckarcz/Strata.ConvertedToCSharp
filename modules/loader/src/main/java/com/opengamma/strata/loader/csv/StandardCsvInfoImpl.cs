/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Standard CSV information resolver.
	/// </summary>
	internal sealed class StandardCsvInfoImpl : TradeCsvInfoResolver, PositionCsvInfoResolver, SensitivityCsvInfoResolver, SensitivityCsvInfoSupplier
	{

	  /// <summary>
	  /// Standard instance.
	  /// </summary>
	  internal static readonly StandardCsvInfoImpl INSTANCE = new StandardCsvInfoImpl(ReferenceData.standard());

	  /// <summary>
	  /// The reference data.
	  /// </summary>
	  private readonly ReferenceData refData;

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
	  public static StandardCsvInfoImpl of(ReferenceData refData)
	  {
		return new StandardCsvInfoImpl(refData);
	  }

	  // restricted constructor
	  private StandardCsvInfoImpl(ReferenceData refData)
	  {
		this.refData = ArgChecker.notNull(refData, "refData");
	  }

	  //-------------------------------------------------------------------------
	  public ReferenceData ReferenceData
	  {
		  get
		  {
			return refData;
		  }
	  }

	}

}
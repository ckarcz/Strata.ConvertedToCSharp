/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.CONTRACT_CODE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.DEFAULT_OPTION_VERSION_NUMBER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.EXCHANGE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.EXERCISE_PRICE_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.PUT_CALL_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.UNDERLYING_EXPIRY_FIELD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.loader.csv.CsvLoaderUtils.VERSION_FIELD;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using Position = com.opengamma.strata.product.Position;
	using PositionInfo = com.opengamma.strata.product.PositionInfo;
	using PositionInfoBuilder = com.opengamma.strata.product.PositionInfoBuilder;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityPosition = com.opengamma.strata.product.SecurityPosition;
	using ExchangeId = com.opengamma.strata.product.common.ExchangeId;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using EtdContractCode = com.opengamma.strata.product.etd.EtdContractCode;
	using EtdContractSpec = com.opengamma.strata.product.etd.EtdContractSpec;
	using EtdContractSpecId = com.opengamma.strata.product.etd.EtdContractSpecId;
	using EtdFuturePosition = com.opengamma.strata.product.etd.EtdFuturePosition;
	using EtdFutureSecurity = com.opengamma.strata.product.etd.EtdFutureSecurity;
	using EtdIdUtils = com.opengamma.strata.product.etd.EtdIdUtils;
	using EtdOptionPosition = com.opengamma.strata.product.etd.EtdOptionPosition;
	using EtdOptionSecurity = com.opengamma.strata.product.etd.EtdOptionSecurity;
	using EtdType = com.opengamma.strata.product.etd.EtdType;
	using EtdVariant = com.opengamma.strata.product.etd.EtdVariant;

	/// <summary>
	/// Resolves additional information when parsing position CSV files.
	/// <para>
	/// Data loaded from a CSV may contain additional information that needs to be captured.
	/// This plugin point allows the additional CSV columns to be parsed and captured.
	/// It also allows the ETD contract specification to be loaded.
	/// </para>
	/// </summary>
	public interface PositionCsvInfoResolver
	{

	  /// <summary>
	  /// Obtains an instance that uses the standard set of reference data.
	  /// </summary>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PositionCsvInfoResolver standard()
	//  {
	//	return StandardCsvInfoImpl.INSTANCE;
	//  }

	  /// <summary>
	  /// Obtains an instance that uses the specified set of reference data.
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the loader </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PositionCsvInfoResolver of(com.opengamma.strata.basics.ReferenceData refData)
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
	  /// Parses attributes into {@code PositionInfo}.
	  /// <para>
	  /// If it is available, the position ID will have been set before this method is called.
	  /// It may be altered if necessary, although this is not recommended.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="builder">  the builder to update </param>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default void parsePositionInfo(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.PositionInfoBuilder builder)
	//  {
	//	// do nothing
	//  }

	  /// <summary>
	  /// Completes the position, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the position has been parsed and after
	  /// <seealso cref="#parsePositionInfo(CsvRow, PositionInfoBuilder)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="position">  the parsed position </param>
	  /// <param name="spec">  the contract specification </param>
	  /// <returns> the updated position </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.etd.EtdFuturePosition completePosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.etd.EtdFuturePosition position, com.opengamma.strata.product.etd.EtdContractSpec spec)
	//  {
	//	// do nothing
	//	return position;
	//  }

	  /// <summary>
	  /// Completes the position, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the position has been parsed and after
	  /// <seealso cref="#parsePositionInfo(CsvRow, PositionInfoBuilder)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="position">  the parsed position </param>
	  /// <param name="spec">  the contract specification </param>
	  /// <returns> the updated position </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.etd.EtdOptionPosition completePosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.etd.EtdOptionPosition position, com.opengamma.strata.product.etd.EtdContractSpec spec)
	//  {
	//	// do nothing
	//	return position;
	//  }

	  /// <summary>
	  /// Completes the position, potentially parsing additional columns.
	  /// <para>
	  /// This is called after the position has been parsed and after
	  /// <seealso cref="#parsePositionInfo(CsvRow, PositionInfoBuilder)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="position">  the parsed position </param>
	  /// <returns> the updated position </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.SecurityPosition completePosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.SecurityPosition position)
	//  {
	//	// do nothing
	//	return position;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the contract specification from the row.
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="type">  the ETD type </param>
	  /// <returns> the contract specification </returns>
	  /// <exception cref="IllegalArgumentException"> if the specification is not found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.etd.EtdContractSpec parseEtdContractSpec(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.etd.EtdType type)
	//  {
	//	ExchangeId exchangeId = ExchangeId.of(row.getValue(EXCHANGE_FIELD));
	//	EtdContractCode contractCode = EtdContractCode.of(row.getValue(CONTRACT_CODE_FIELD));
	//	EtdContractSpecId specId = EtdIdUtils.contractSpecId(type, exchangeId, contractCode);
	//	return getReferenceData().findValue(specId).orElseThrow(() -> new IllegalArgumentException("ETD contract specification not found in reference data: " + specId));
	//  }

	  /// <summary>
	  /// Parses an ETD future position from the CSV row.
	  /// <para>
	  /// This is intended to use reference data to find the ETD future security,
	  /// returning it as an instance of <seealso cref="EtdFuturePosition"/>.
	  /// The reference data lookup uses <seealso cref="#parseEtdContractSpec(CsvRow, EtdType)"/> by default,
	  /// however it could be overridden to lookup the security directly in reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="info">  the position information </param>
	  /// <returns> the parsed position </returns>
	  /// <exception cref="IllegalArgumentException"> if the row cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.Position parseEtdFuturePosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.PositionInfo info)
	//  {
	//	EtdContractSpec contract = parseEtdContractSpec(row, EtdType.FUTURE);
	//	Pair<YearMonth, EtdVariant> variant = CsvLoaderUtils.parseEtdVariant(row, EtdType.FUTURE);
	//	EtdFutureSecurity security = contract.createFuture(variant.getFirst(), variant.getSecond());
	//	DoublesPair quantity = CsvLoaderUtils.parseQuantity(row);
	//	EtdFuturePosition position = EtdFuturePosition.ofLongShort(info, security, quantity.getFirst(), quantity.getSecond());
	//	return completePosition(row, position, contract);
	//  }

	  /// <summary>
	  /// Parses an ETD future position from the CSV row.
	  /// <para>
	  /// This is intended to use reference data to find the ETD future security,
	  /// returning it as an instance of <seealso cref="EtdOptionPosition"/>.
	  /// The reference data lookup uses <seealso cref="#parseEtdContractSpec(CsvRow, EtdType)"/> by default,
	  /// however it could be overridden to lookup the security directly in reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row </param>
	  /// <param name="info">  the position info </param>
	  /// <returns> the parsed position </returns>
	  /// <exception cref="IllegalArgumentException"> if the row cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.Position parseEtdOptionPosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.PositionInfo info)
	//  {
	//	EtdContractSpec contract = parseEtdContractSpec(row, EtdType.OPTION);
	//	Pair<YearMonth, EtdVariant> variant = CsvLoaderUtils.parseEtdVariant(row, EtdType.OPTION);
	//	int version = row.findValue(VERSION_FIELD).map(System.Nullable<int>::parseInt).orElse(DEFAULT_OPTION_VERSION_NUMBER);
	//	PutCall putCall = LoaderUtils.parsePutCall(row.getValue(PUT_CALL_FIELD));
	//	double strikePrice = double.Parse(row.getValue(EXERCISE_PRICE_FIELD));
	//	YearMonth underlyingExpiry = row.findValue(UNDERLYING_EXPIRY_FIELD).map(str -> LoaderUtils.parseYearMonth(str)).orElse(null);
	//	EtdOptionSecurity security = contract.createOption(variant.getFirst(), variant.getSecond(), version, putCall, strikePrice, underlyingExpiry);
	//	DoublesPair quantity = CsvLoaderUtils.parseQuantity(row);
	//	EtdOptionPosition position = EtdOptionPosition.ofLongShort(info, security, quantity.getFirst(), quantity.getSecond());
	//	return completePosition(row, position, contract);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses an ETD future position from the CSV row without using reference data.
	  /// <para>
	  /// This returns a <seealso cref="SecurityPosition"/> based on a standard ETD identifier from <seealso cref="EtdIdUtils"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="info">  the position information </param>
	  /// <returns> the loaded positions, position-level errors are captured in the result </returns>
	  /// <exception cref="IllegalArgumentException"> if the row cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.SecurityPosition parseEtdFutureSecurityPosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.PositionInfo info)
	//  {
	//	ExchangeId exchangeId = ExchangeId.of(row.getValue(EXCHANGE_FIELD));
	//	EtdContractCode contractCode = EtdContractCode.of(row.getValue(CONTRACT_CODE_FIELD));
	//	Pair<YearMonth, EtdVariant> variant = CsvLoaderUtils.parseEtdVariant(row, EtdType.FUTURE);
	//	SecurityId securityId = EtdIdUtils.futureId(exchangeId, contractCode, variant.getFirst(), variant.getSecond());
	//	DoublesPair quantity = CsvLoaderUtils.parseQuantity(row);
	//	SecurityPosition position = SecurityPosition.ofLongShort(info, securityId, quantity.getFirst(), quantity.getSecond());
	//	return completePosition(row, position);
	//  }

	  /// <summary>
	  /// Parses an ETD option position from the CSV row without using reference data.
	  /// <para>
	  /// This returns a <seealso cref="SecurityPosition"/> based on a standard ETD identifier from <seealso cref="EtdIdUtils"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="row">  the CSV row to parse </param>
	  /// <param name="info">  the position information </param>
	  /// <returns> the loaded positions, position-level errors are captured in the result </returns>
	  /// <exception cref="IllegalArgumentException"> if the row cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.SecurityPosition parseEtdOptionSecurityPosition(com.opengamma.strata.collect.io.CsvRow row, com.opengamma.strata.product.PositionInfo info)
	//  {
	//	ExchangeId exchangeId = ExchangeId.of(row.getValue(EXCHANGE_FIELD));
	//	EtdContractCode contractCode = EtdContractCode.of(row.getValue(CONTRACT_CODE_FIELD));
	//	Pair<YearMonth, EtdVariant> variant = CsvLoaderUtils.parseEtdVariant(row, EtdType.OPTION);
	//	int version = row.findValue(VERSION_FIELD).map(System.Nullable<int>::parseInt).orElse(DEFAULT_OPTION_VERSION_NUMBER);
	//	PutCall putCall = LoaderUtils.parsePutCall(row.getValue(PUT_CALL_FIELD));
	//	double strikePrice = double.Parse(row.getValue(EXERCISE_PRICE_FIELD));
	//	YearMonth underlyingExpiry = row.findValue(UNDERLYING_EXPIRY_FIELD).map(str -> LoaderUtils.parseYearMonth(str)).orElse(null);
	//	SecurityId securityId = EtdIdUtils.optionId(exchangeId, contractCode, variant.getFirst(), variant.getSecond(), version, putCall, strikePrice, underlyingExpiry);
	//	DoublesPair quantity = CsvLoaderUtils.parseQuantity(row);
	//	SecurityPosition position = SecurityPosition.ofLongShort(info, securityId, quantity.getFirst(), quantity.getSecond());
	//	return completePosition(row, position);
	//  }

	}

}
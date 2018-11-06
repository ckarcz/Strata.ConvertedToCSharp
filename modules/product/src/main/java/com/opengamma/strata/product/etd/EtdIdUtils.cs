using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{


	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExchangeId = com.opengamma.strata.product.common.ExchangeId;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// A utility for generating ETD identifiers.
	/// <para>
	/// An exchange traded derivative (ETD) is uniquely identified by a set of fields.
	/// In most cases, these fields should be kept separate, as on <seealso cref="EtdContractSpec"/>.
	/// However, it can be useful to create a single identifier from the separate fields.
	/// We do not recommend parsing the combined identifier to retrieve individual fields.
	/// </para>
	/// </summary>
	public sealed class EtdIdUtils
	{

	  /// <summary>
	  /// Scheme used for ETDs.
	  /// </summary>
	  public const string ETD_SCHEME = "OG-ETD";
	  /// <summary>
	  /// The separator to use.
	  /// </summary>
	  private const string SEPARATOR = "-";
	  /// <summary>
	  /// Prefix for futures.
	  /// </summary>
	  private static readonly string FUT_PREFIX = "F" + SEPARATOR;
	  /// <summary>
	  /// Prefix for option.
	  /// </summary>
	  private static readonly string OPT_PREFIX = "O" + SEPARATOR;
	  /// <summary>
	  /// The year-month format.
	  /// </summary>
	  private static readonly DateTimeFormatter YM_FORMAT = new DateTimeFormatterBuilder().appendValue(YEAR, 4).appendValue(MONTH_OF_YEAR, 2).toFormatter(Locale.ROOT);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an identifier for a contract specification.
	  /// <para>
	  /// This will have the format:
	  /// {@code 'OG-ETD~F-ECAG-FGBS'} or {@code 'OG-ETD~O-ECAG-OGBS'}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type">  type of the contract - future or option </param>
	  /// <param name="exchangeId">  the MIC code of the exchange where the instruments are traded </param>
	  /// <param name="contractCode">  the code supplied by the exchange for use in clearing and margining, such as in SPAN </param>
	  /// <returns> the identifier </returns>
	  public static EtdContractSpecId contractSpecId(EtdType type, ExchangeId exchangeId, EtdContractCode contractCode)
	  {
		ArgChecker.notNull(type, "type");
		ArgChecker.notNull(exchangeId, "exchangeId");
		ArgChecker.notNull(contractCode, "contractCode");
		switch (type.innerEnumValue)
		{
		  case com.opengamma.strata.product.etd.EtdType.InnerEnum.FUTURE:
			return EtdContractSpecId.of(ETD_SCHEME, FUT_PREFIX + exchangeId + SEPARATOR + contractCode);
		  case com.opengamma.strata.product.etd.EtdType.InnerEnum.OPTION:
			return EtdContractSpecId.of(ETD_SCHEME, OPT_PREFIX + exchangeId + SEPARATOR + contractCode);
		  default:
			throw new System.ArgumentException("Unknown ETD type: " + type);
		}
	  }

	  /// <summary>
	  /// Creates an identifier for an ETD future instrument.
	  /// <para>
	  /// A typical monthly ETD will have the format:
	  /// {@code 'OG-ETD~O-ECAG-OGBS-201706'}.
	  /// </para>
	  /// <para>
	  /// A more complex flex ETD (12th of the month, Physical settlement) will have the format:
	  /// {@code 'OG-ETD~O-ECAG-OGBS-20170612E'}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="exchangeId">  the MIC code of the exchange where the instruments are traded </param>
	  /// <param name="contractCode">  the code supplied by the exchange for use in clearing and margining, such as in SPAN </param>
	  /// <param name="expiryMonth">  the month of expiry </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <returns> the identifier </returns>
	  public static SecurityId futureId(ExchangeId exchangeId, EtdContractCode contractCode, YearMonth expiryMonth, EtdVariant variant)
	  {

		ArgChecker.notNull(exchangeId, "exchangeId");
		ArgChecker.notNull(contractCode, "contractCode");
		ArgChecker.notNull(expiryMonth, "expiryMonth");
		ArgChecker.isTrue(expiryMonth.Year >= 1000 && expiryMonth.Year <= 9999, "Invalid expiry year: ", expiryMonth);
		ArgChecker.notNull(variant, "variant");

		string id = (new StringBuilder(40)).Append(FUT_PREFIX).Append(exchangeId).Append(SEPARATOR).Append(contractCode).Append(SEPARATOR).Append(expiryMonth.format(YM_FORMAT)).Append(variant.Code).ToString();
		return SecurityId.of(ETD_SCHEME, id);
	  }

	  /// <summary>
	  /// Creates an identifier for an ETD option instrument.
	  /// <para>
	  /// A typical monthly ETD with version zero will have the format:
	  /// {@code 'OG-ETD~O-ECAG-OGBS-201706-P1.50'}.
	  /// </para>
	  /// <para>
	  /// A more complex flex ETD (12th of the month, Cash settlement, European) with version two will have the format:
	  /// {@code 'OG-ETD~O-ECAG-OGBS-20170612CE-V2-P1.50'}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="exchangeId">  the MIC code of the exchange where the instruments are traded </param>
	  /// <param name="contractCode">  the code supplied by the exchange for use in clearing and margining, such as in SPAN </param>
	  /// <param name="expiryMonth">  the month of expiry </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <param name="version">  the non-negative version, zero by default </param>
	  /// <param name="putCall">  the Put/Call flag </param>
	  /// <param name="strikePrice">  the strike price </param>
	  /// <returns> the identifier </returns>
	  public static SecurityId optionId(ExchangeId exchangeId, EtdContractCode contractCode, YearMonth expiryMonth, EtdVariant variant, int version, PutCall putCall, double strikePrice)
	  {

		return optionId(exchangeId, contractCode, expiryMonth, variant, version, putCall, strikePrice, null);
	  }

	  /// <summary>
	  /// Creates an identifier for an ETD option instrument.
	  /// <para>
	  /// This takes into account the expiry of the underlying instrument. If the underlying expiry
	  /// is the same as the expiry of the option, the identifier is the same as the normal one.
	  /// Otherwise, the underlying expiry is added after the option expiry. For example:
	  /// {@code 'OG-ETD~O-ECAG-OGBS-201706-P1.50-U201709'}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="exchangeId">  the MIC code of the exchange where the instruments are traded </param>
	  /// <param name="contractCode">  the code supplied by the exchange for use in clearing and margining, such as in SPAN </param>
	  /// <param name="expiryMonth">  the month of expiry </param>
	  /// <param name="variant">  the variant of the ETD, such as 'Monthly', 'Weekly, 'Daily' or 'Flex' </param>
	  /// <param name="version">  the non-negative version, zero by default </param>
	  /// <param name="putCall">  the Put/Call flag </param>
	  /// <param name="strikePrice">  the strike price </param>
	  /// <param name="underlyingExpiryMonth">  the expiry of the underlying instrument, such as a future, may be null </param>
	  /// <returns> the identifier </returns>
	  public static SecurityId optionId(ExchangeId exchangeId, EtdContractCode contractCode, YearMonth expiryMonth, EtdVariant variant, int version, PutCall putCall, double strikePrice, YearMonth underlyingExpiryMonth)
	  {

		ArgChecker.notNull(exchangeId, "exchangeId");
		ArgChecker.notNull(contractCode, "contractCode");
		ArgChecker.notNull(expiryMonth, "expiryMonth");
		ArgChecker.notNull(variant, "variant");
		ArgChecker.notNull(putCall, "putCall");

		string putCallStr = putCall == PutCall.PUT ? "P" : "C";
		string versionCode = version > 0 ? "V" + version + SEPARATOR : "";

		NumberFormat f = NumberFormat.getIntegerInstance(Locale.ENGLISH);
		f.GroupingUsed = false;
		f.MaximumFractionDigits = 8;
		string strikeStr = f.format(strikePrice).replace('-', 'M');

		string underlying = "";
		if (underlyingExpiryMonth != null && !underlyingExpiryMonth.Equals(expiryMonth))
		{
		  underlying = SEPARATOR + "U" + underlyingExpiryMonth.format(YM_FORMAT);
		}

		string id = (new StringBuilder(40)).Append(OPT_PREFIX).Append(exchangeId).Append(SEPARATOR).Append(contractCode).Append(SEPARATOR).Append(expiryMonth.format(YM_FORMAT)).Append(variant.Code).Append(SEPARATOR).Append(versionCode).Append(putCallStr).Append(strikeStr).Append(underlying).ToString();
		return SecurityId.of(ETD_SCHEME, id);
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  private EtdIdUtils()
	  {
	  }

	}

}
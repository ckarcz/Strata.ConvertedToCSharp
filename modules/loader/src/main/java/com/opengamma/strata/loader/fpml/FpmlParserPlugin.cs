/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using Trade = com.opengamma.strata.product.Trade;

	/// <summary>
	/// Pluggable FpML trade parser.
	/// <para>
	/// Implementations of this interface parse an FpML trade element, including any trade header.
	/// The <seealso cref="FpmlDocument"/> instance provides many useful helper methods.
	/// </para>
	/// <para>
	/// See <seealso cref="FpmlDocumentParser"/> for the main entry point for FpML parsing.
	/// </para>
	/// </summary>
	public interface FpmlParserPlugin : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the parser </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FpmlParserPlugin of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FpmlParserPlugin of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the parser to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<FpmlParserPlugin> extendedEnum()
	//  {
	//	return FpmlDocumentParser.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a single FpML format trade.
	  /// <para>
	  /// This parses a trade from the given XML element.
	  /// Details of the whole document and parser helper methods are provided.
	  /// </para>
	  /// <para>
	  /// It is intended that this method is only called when the specified trade element
	  /// contains a child element of the correct type for this parser.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="document">  the document-wide information and parser helper </param>
	  /// <param name="tradeEl">  the trade element to parse </param>
	  /// <returns> the trade object </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  Trade parseTrade(FpmlDocument document, XmlElement tradeEl);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this parser.
	  /// <para>
	  /// The name must be the name of the product element in FpML that is to be parsed.
	  /// For example, 'fra', 'swap' or 'fxSingleLeg'.
	  /// </para>
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	}

}
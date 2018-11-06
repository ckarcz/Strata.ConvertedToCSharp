/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{

	using ListMultimap = com.google.common.collect.ListMultimap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;

	/// <summary>
	/// Pluggable FpML trade information parser.
	/// <para>
	/// Implementations of this interface parse FpML to produce <seealso cref="TradeInfo"/>.
	/// The <seealso cref="FpmlDocument"/> instance provides many useful helper methods.
	/// </para>
	/// <para>
	/// See <seealso cref="FpmlDocumentParser"/> for the main entry point for FpML parsing.
	/// </para>
	/// </summary>
	public interface FpmlTradeInfoParserPlugin
	{

	  /// <summary>
	  /// Returns the standard parser plugin that parses the trade date and the first
	  /// identifier of "our" party.
	  /// </summary>
	  /// <returns> the standard trade info parser </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FpmlTradeInfoParserPlugin standard()
	//  {
	//	return FpmlDocument.TRADE_INFO_STANDARD;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses trade information from the FpML document.
	  /// <para>
	  /// This parses any trade info that is desired from the specified FpML document.
	  /// Details of the whole document and parser helper methods are provided.
	  /// Typically such parsing will require accessing the {@code <tradeHeader>} element
	  /// from the root FpML element in the document.
	  /// </para>
	  /// <para>
	  /// Since most implementations will need the trade date and a trade identifier,
	  /// these are pre-parsed before the method is invoked. The parties associated with
	  /// the party href id can be obtained from the document.
	  /// </para>
	  /// <para>
	  /// A new instance of the builder must be returned each time the method is invoked.
	  /// The builder is returned to allow the counterparty to be added by the
	  /// <seealso cref="FpmlParserPlugin"/> implementation based on the trade direction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date from the document </param>
	  /// <param name="allTradeIds">  the collection of trade identifiers in the document, keyed by party href id </param>
	  /// <param name="document">  the document-wide information and parser helper </param>
	  /// <returns> the trade info object </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  TradeInfoBuilder parseTrade(FpmlDocument document, LocalDate tradeDate, ListMultimap<string, StandardId> allTradeIds);

	}

}
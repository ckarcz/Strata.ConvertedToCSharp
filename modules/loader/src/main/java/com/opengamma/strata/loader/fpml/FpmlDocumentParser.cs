using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ByteSource = com.google.common.io.ByteSource;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using XmlFile = com.opengamma.strata.collect.io.XmlFile;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Trade = com.opengamma.strata.product.Trade;

	/// <summary>
	/// Loader of trade data in FpML format.
	/// <para>
	/// This handles the subset of FpML necessary to populate the trade model.
	/// The standard parsers accept FpML v5.8, which is often the same as earlier versions.
	/// </para>
	/// <para>
	/// The trade parsers implement <seealso cref="FpmlParserPlugin"/> and are pluggable using
	/// the {@code FpmlParserPlugin.ini} configuration file.
	/// </para>
	/// </summary>
	public sealed class FpmlDocumentParser
	{
	  // Notes: Streaming trades directly from the file is difficult due to the
	  // need to parse the party element at the root, which is after the trades

	  /// <summary>
	  /// The lookup of trade parsers.
	  /// </summary>
	  internal static readonly ExtendedEnum<FpmlParserPlugin> ENUM_LOOKUP = ExtendedEnum.of(typeof(FpmlParserPlugin));

	  /// <summary>
	  /// The selector used to find "our" party within the set of parties in the FpML document.
	  /// </summary>
	  private readonly FpmlPartySelector ourPartySelector;
	  /// <summary>
	  /// The trade info parser.
	  /// </summary>
	  private readonly FpmlTradeInfoParserPlugin tradeInfoParser;
	  /// <summary>
	  /// The trade parsers.
	  /// </summary>
	  private readonly IDictionary<string, FpmlParserPlugin> tradeParsers;
	  /// <summary>
	  /// The reference data.
	  /// </summary>
	  private readonly ReferenceData refData;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of the parser, based on the specified selector.
	  /// <para>
	  /// The FpML parser has a number of plugin points that can be controlled:
	  /// <ul>
	  /// <li>the <seealso cref="FpmlPartySelector party selector"/>
	  /// <li>the <seealso cref="FpmlTradeInfoParserPlugin trade info parser"/>
	  /// <li>the <seealso cref="FpmlParserPlugin trade parsers"/>
	  /// <li>the <seealso cref="ReferenceData reference data"/>
	  /// </ul>
	  /// This method uses the <seealso cref="FpmlTradeInfoParserPlugin#standard() standard"/>
	  /// trade info parser, the trade parsers registered in <seealso cref="FpmlParserPlugin"/>
	  /// configuration and the <seealso cref="ReferenceData#standard() standard"/> reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ourPartySelector">  the selector used to find "our" party within the set of parties in the FpML document </param>
	  /// <returns> the document parser </returns>
	  public static FpmlDocumentParser of(FpmlPartySelector ourPartySelector)
	  {
		return of(ourPartySelector, FpmlTradeInfoParserPlugin.standard());
	  }

	  /// <summary>
	  /// Obtains an instance of the parser, based on the specified selector and trade info plugin.
	  /// <para>
	  /// The FpML parser has a number of plugin points that can be controlled:
	  /// <ul>
	  /// <li>the <seealso cref="FpmlPartySelector party selector"/>
	  /// <li>the <seealso cref="FpmlTradeInfoParserPlugin trade info parser"/>
	  /// <li>the <seealso cref="FpmlParserPlugin trade parsers"/>
	  /// <li>the <seealso cref="ReferenceData reference data"/>
	  /// </ul>
	  /// This method uses the trade parsers registered in <seealso cref="FpmlParserPlugin"/> configuration
	  /// and the <seealso cref="ReferenceData#standard() standard"/> reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ourPartySelector">  the selector used to find "our" party within the set of parties in the FpML document </param>
	  /// <param name="tradeInfoParser">  the trade info parser </param>
	  /// <returns> the document parser </returns>
	  public static FpmlDocumentParser of(FpmlPartySelector ourPartySelector, FpmlTradeInfoParserPlugin tradeInfoParser)
	  {

		return of(ourPartySelector, tradeInfoParser, FpmlParserPlugin.extendedEnum().lookupAllNormalized());
	  }

	  /// <summary>
	  /// Obtains an instance of the parser, based on the specified selector and plugins.
	  /// <para>
	  /// The FpML parser has a number of plugin points that can be controlled:
	  /// <ul>
	  /// <li>the <seealso cref="FpmlPartySelector party selector"/>
	  /// <li>the <seealso cref="FpmlTradeInfoParserPlugin trade info parser"/>
	  /// <li>the <seealso cref="FpmlParserPlugin trade parsers"/>
	  /// <li>the <seealso cref="ReferenceData reference data"/>
	  /// </ul>
	  /// This method uses the <seealso cref="ReferenceData#standard() standard"/> reference data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ourPartySelector">  the selector used to find "our" party within the set of parties in the FpML document </param>
	  /// <param name="tradeInfoParser">  the trade info parser </param>
	  /// <param name="tradeParsers">  the map of trade parsers, keyed by the FpML element name </param>
	  /// <returns> the document parser </returns>
	  public static FpmlDocumentParser of(FpmlPartySelector ourPartySelector, FpmlTradeInfoParserPlugin tradeInfoParser, IDictionary<string, FpmlParserPlugin> tradeParsers)
	  {

		return of(ourPartySelector, tradeInfoParser, tradeParsers, ReferenceData.standard());
	  }

	  /// <summary>
	  /// Obtains an instance of the parser, based on the specified selector and plugins.
	  /// <para>
	  /// The FpML parser has a number of plugin points that can be controlled:
	  /// <ul>
	  /// <li>the <seealso cref="FpmlPartySelector party selector"/>
	  /// <li>the <seealso cref="FpmlTradeInfoParserPlugin trade info parser"/>
	  /// <li>the <seealso cref="FpmlParserPlugin trade parsers"/>
	  /// <li>the <seealso cref="ReferenceData reference data"/>
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="ourPartySelector">  the selector used to find "our" party within the set of parties in the FpML document </param>
	  /// <param name="tradeInfoParser">  the trade info parser </param>
	  /// <param name="tradeParsers">  the map of trade parsers, keyed by the FpML element name </param>
	  /// <param name="refData">  the reference data to use </param>
	  /// <returns> the document parser </returns>
	  public static FpmlDocumentParser of(FpmlPartySelector ourPartySelector, FpmlTradeInfoParserPlugin tradeInfoParser, IDictionary<string, FpmlParserPlugin> tradeParsers, ReferenceData refData)
	  {

		return new FpmlDocumentParser(ourPartySelector, tradeInfoParser, tradeParsers, refData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, based on the specified element.
	  /// </summary>
	  /// <param name="ourPartySelector">  the selector used to find "our" party within the set of parties in the FpML document </param>
	  /// <param name="tradeInfoParser">  the trade info parser </param>
	  /// <param name="tradeParsers">  the map of trade parsers, keyed by the FpML element name </param>
	  private FpmlDocumentParser(FpmlPartySelector ourPartySelector, FpmlTradeInfoParserPlugin tradeInfoParser, IDictionary<string, FpmlParserPlugin> tradeParsers, ReferenceData refData)
	  {

		this.ourPartySelector = ourPartySelector;
		this.tradeInfoParser = tradeInfoParser;
		this.tradeParsers = tradeParsers;
		this.refData = refData;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses FpML from the specified source, extracting the trades.
	  /// <para>
	  /// This parses the specified byte source which must be an XML document.
	  /// </para>
	  /// <para>
	  /// Sometimes, the FpML document is embedded in a non-FpML wrapper.
	  /// This method will intelligently find the FpML document at the root or within one or two levels
	  /// of wrapper by searching for an element that contains both {@code <trade>} and {@code <party>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="source">  the source of the FpML XML document </param>
	  /// <returns> the parsed trades </returns>
	  /// <exception cref="RuntimeException"> if a parse error occurred </exception>
	  public IList<Trade> parseTrades(ByteSource source)
	  {
		XmlFile xmlFile = XmlFile.of(source, FpmlDocument.ID);
		XmlElement root = findFpmlRoot(xmlFile.Root);
		return parseTrades(root, xmlFile.References);
	  }

	  // intelligently finds the FpML root element
	  private static XmlElement findFpmlRoot(XmlElement root)
	  {
		XmlElement fpmlRoot = getFpmlRoot(root);
		if (fpmlRoot != null)
		{
		  return fpmlRoot;
		}
		// try children of root element
		foreach (XmlElement el in root.Children)
		{
		  fpmlRoot = getFpmlRoot(el);
		  if (fpmlRoot != null)
		  {
			return fpmlRoot;
		  }
		}
		// try grandchildren of root element
		foreach (XmlElement el1 in root.Children)
		{
		  foreach (XmlElement el2 in el1.Children)
		  {
			fpmlRoot = getFpmlRoot(el2);
			if (fpmlRoot != null)
			{
			  return fpmlRoot;
			}
		  }
		}
		throw new FpmlParseException("Unable to find FpML root element");
	  }

	  // simple check to see if this is an FpML root
	  private static XmlElement getFpmlRoot(XmlElement el)
	  {
		if (el.getChildren("party").size() > 0)
		{
		  // party and trade are siblings (the common case)
		  if (el.getChildren("trade").size() > 0)
		  {
			return el;
		  }
		  // trade is within a child alongside party (the unusual case, within clearingStatus/clearingStatusItem)
		  foreach (XmlElement child in el.Children)
		  {
			if (child.getChildren("trade").size() > 0)
			{
			  IList<XmlElement> fakeChildren = new List<XmlElement>();
			  ((IList<XmlElement>)fakeChildren).AddRange(el.getChildren("party"));
			  ((IList<XmlElement>)fakeChildren).AddRange(child.getChildren("trade"));
			  XmlElement fakeRoot = XmlElement.ofChildren(el.Name, el.Attributes, fakeChildren);
			  return fakeRoot;
			}
		  }
		  // trade is within a grandchild alongside party (the unusual case, within clearingConfirmed/clearing/cleared)
		  foreach (XmlElement child in el.Children)
		  {
			foreach (XmlElement grandchild in child.Children)
			{
			  if (grandchild.getChildren("trade").size() > 0)
			  {
				IList<XmlElement> fakeChildren = new List<XmlElement>();
				((IList<XmlElement>)fakeChildren).AddRange(el.getChildren("party"));
				((IList<XmlElement>)fakeChildren).AddRange(grandchild.getChildren("trade"));
				XmlElement fakeRoot = XmlElement.ofChildren(el.Name, el.Attributes, fakeChildren);
				return fakeRoot;
			  }
			}
		  }
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the FpML document extracting the trades.
	  /// <para>
	  /// This parses the specified FpML root element, using the map of references.
	  /// The FpML specification uses references to link one part of the XML to another.
	  /// For example, if one part of the XML has {@code <foo id="fooId">}, the references
	  /// map will contain an entry mapping "fooId" to the parsed element {@code <foo>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fpmlRootEl">  the source of the FpML XML document </param>
	  /// <param name="references">  the map of id/href to referenced element </param>
	  /// <returns> the parsed trades </returns>
	  /// <exception cref="RuntimeException"> if a parse error occurred </exception>
	  public IList<Trade> parseTrades(XmlElement fpmlRootEl, IDictionary<string, XmlElement> references)
	  {

		FpmlDocument document = new FpmlDocument(fpmlRootEl, references, ourPartySelector, tradeInfoParser, refData);
		IList<XmlElement> tradeEls = document.FpmlRoot.getChildren("trade");
		ImmutableList.Builder<Trade> builder = ImmutableList.builder();
		foreach (XmlElement tradeEl in tradeEls)
		{
		  builder.add(parseTrade(document, tradeEl));
		}
		return builder.build();
	  }

	  // parses one trade element
	  private Trade parseTrade(FpmlDocument document, XmlElement tradeEl)
	  {
		// find which trade type it is by comparing children to known parsers
		foreach (KeyValuePair<string, FpmlParserPlugin> entry in tradeParsers.SetOfKeyValuePairs())
		{
		  Optional<XmlElement> productOptEl = tradeEl.findChild(entry.Key);
		  if (productOptEl.Present)
		  {
			return entry.Value.parseTrade(document, tradeEl);
		  }
		}
		// failed to find a known trade type
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSet<string> childNames = tradeEl.Children.Select(XmlElement::getName).collect(toImmutableSet());
		throw new FpmlParseException("Unknown product type: " + childNames);
	  }

	}

}
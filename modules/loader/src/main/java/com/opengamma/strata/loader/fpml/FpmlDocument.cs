using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ArrayListMultimap = com.google.common.collect.ArrayListMultimap;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ListMultimap = com.google.common.collect.ListMultimap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConvention = com.opengamma.strata.basics.date.BusinessDayConvention;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using FloatingRateName = com.opengamma.strata.basics.index.FloatingRateName;
	using Index = com.opengamma.strata.basics.index.Index;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using Messages = com.opengamma.strata.collect.Messages;
	using XmlElement = com.opengamma.strata.collect.io.XmlElement;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using TradeInfoBuilder = com.opengamma.strata.product.TradeInfoBuilder;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;

	/// <summary>
	/// Provides data about the whole FpML document and parse helper methods.
	/// <para>
	/// This is primarily used to support implementations of <seealso cref="FpmlParserPlugin"/>.
	/// See <seealso cref="FpmlDocumentParser"/> for the main entry point for FpML parsing.
	/// </para>
	/// </summary>
	public sealed class FpmlDocument
	{
	  // FRN definition is dates that are on same numerical day of month
	  // Use last business day of month if no matching numerical date (eg. 31st June replaced by last business day of June)
	  // Non-business days are replaced by following, or preceding to avoid changing the month
	  // If last date was last business day of month, then all subsequent dates are last business day of month
	  // While close to ModifiedFollowing, it is unclear is that is correct for BusinessDayConvention
	  // FpML also has a 'NotApplicable' option, which probably should map to null in the caller

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(FpmlDocument));

	  /// <summary>
	  /// The 'id' attribute key.
	  /// </summary>
	  public const string ID = "id";
	  /// <summary>
	  /// The 'href' attribute key.
	  /// </summary>
	  public const string HREF = "href";

	  /// <summary>
	  /// Scheme used for parties that are read from FpML.
	  /// </summary>
	  private const string FPML_PARTY_SCHEME = "FpML-partyId";
	  /// <summary>
	  /// The enum group for FpML conversions.
	  /// </summary>
	  private const string ENUM_FPML = "FpML";
	  /// <summary>
	  /// The FpML date parser.
	  /// </summary>
	  private static readonly DateTimeFormatter FPML_DATE_FORMAT = DateTimeFormatter.ofPattern("uuuu-MM-dd[XXX]", Locale.ENGLISH);
	  /// <summary>
	  /// The map of frequencies, designed to normalize and reduce object creation.
	  /// </summary>
	  private static readonly IDictionary<string, Frequency> FREQUENCY_MAP = ImmutableMap.builder<string, Frequency>().put("1D", Frequency.P12M).put("7D", Frequency.P1W).put("14D", Frequency.P2W).put("28D", Frequency.P4W).put("91D", Frequency.P13W).put("182D", Frequency.P26W).put("364D", Frequency.P52W).put("1W", Frequency.P1W).put("2W", Frequency.P2W).put("4W", Frequency.P4W).put("13W", Frequency.P13W).put("26W", Frequency.P26W).put("52W", Frequency.P52W).put("1M", Frequency.P1M).put("2M", Frequency.P2M).put("3M", Frequency.P3M).put("4M", Frequency.P4M).put("6M", Frequency.P6M).put("12M", Frequency.P12M).put("1Y", Frequency.P12M).build();
	  /// <summary>
	  /// The map of index tenors, designed to normalize and reduce object creation.
	  /// </summary>
	  private static readonly IDictionary<string, Tenor> TENOR_MAP = ImmutableMap.builder<string, Tenor>().put("7D", Tenor.TENOR_1W).put("14D", Tenor.TENOR_2W).put("21D", Tenor.TENOR_3W).put("28D", Tenor.TENOR_4W).put("1W", Tenor.TENOR_1W).put("2W", Tenor.TENOR_2W).put("1M", Tenor.TENOR_1M).put("2M", Tenor.TENOR_2M).put("3M", Tenor.TENOR_3M).put("6M", Tenor.TENOR_6M).put("12M", Tenor.TENOR_12M).put("1Y", Tenor.TENOR_12M).build();

	  /// <summary>
	  /// The map of holiday calendar ids to zone ids.
	  /// </summary>
	  private static readonly IDictionary<string, ZoneId> HOLIDAY_CALENDARID_MAP = ImmutableMap.builder<string, ZoneId>().put("BEBR", ZoneId.of("Europe/Brussels")).put("CATO", ZoneId.of("America/Toronto")).put("CHZU", ZoneId.of("Europe/Zurich")).put("DEFR", ZoneId.of("Europe/Berlin")).put("FRPA", ZoneId.of("Europe/Paris")).put("GBLO", ZoneId.of("Europe/London")).put("JPTO", ZoneId.of("Asia/Tokyo")).put("USNY", ZoneId.of("America/New_York")).build();

	  /// <summary>
	  /// Constant defining the "any" selector.
	  /// This must be defined as a constant so that == works when comparing it.
	  /// FpmlPartySelector is an interface and can only define public constants, thus it is declared here.
	  /// </summary>
	  internal static readonly FpmlPartySelector ANY_SELECTOR = allParties => ImmutableList.of();
	  /// <summary>
	  /// Constant defining the "standard" trade info parser.
	  /// </summary>
	  internal static readonly FpmlTradeInfoParserPlugin TRADE_INFO_STANDARD = (doc, tradeDate, allTradeIds) =>
	  {
	TradeInfoBuilder builder = TradeInfo.builder();
	builder.tradeDate(tradeDate);
	allTradeIds.entries().Where(e => doc.OurPartyHrefIds.contains(e.Key)).Select(e => e.Value).First().ifPresent(id => builder.id(id));
	return builder;
	  };

	  /// <summary>
	  /// The parsed file.
	  /// </summary>
	  private readonly XmlElement fpmlRoot;
	  /// <summary>
	  /// The map of references.
	  /// </summary>
	  private readonly ImmutableMap<string, XmlElement> references;
	  /// <summary>
	  /// Map of reference id to partyId.
	  /// </summary>
	  private readonly ImmutableListMultimap<string, string> parties;
	  /// <summary>
	  /// The party reference id.
	  /// </summary>
	  private readonly ImmutableList<string> ourPartyHrefIds;
	  /// <summary>
	  /// The trade info builder.
	  /// </summary>
	  private readonly FpmlTradeInfoParserPlugin tradeInfoParser;
	  /// <summary>
	  /// The reference data.
	  /// </summary>
	  private readonly ReferenceData refData;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance, based on the specified element.
	  /// <para>
	  /// The map of references is used to link one part of the XML to another.
	  /// For example, if one part of the XML has {@code <foo id="fooId">}, the references
	  /// map will contain an entry mapping "fooId" to the parsed element {@code <foo>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fpmlRootEl">  the source of the FpML XML document </param>
	  /// <param name="references">  the map of id/href to referenced element </param>
	  /// <param name="ourPartySelector">  the selector used to find "our" party within the set of parties in the FpML document </param>
	  /// <param name="tradeInfoParser">  the trade info parser </param>
	  /// <param name="refData">  the reference data to use </param>
	  public FpmlDocument(XmlElement fpmlRootEl, IDictionary<string, XmlElement> references, FpmlPartySelector ourPartySelector, FpmlTradeInfoParserPlugin tradeInfoParser, ReferenceData refData)
	  {

		this.fpmlRoot = fpmlRootEl;
		this.references = ImmutableMap.copyOf(references);
		this.parties = parseParties(fpmlRootEl);
		this.ourPartyHrefIds = findOurParty(ourPartySelector);
		this.tradeInfoParser = tradeInfoParser;
		this.refData = refData;
	  }

	  // parse all the root-level party elements
	  private static ImmutableListMultimap<string, string> parseParties(XmlElement root)
	  {
		ListMultimap<string, string> parties = ArrayListMultimap.create();
		foreach (XmlElement child in root.getChildren("party"))
		{
		  parties.putAll(child.getAttribute(ID), findPartyIds(child));
		}
		return ImmutableListMultimap.copyOf(parties);
	  }

	  // find the party identifiers
	  private static IList<string> findPartyIds(XmlElement party)
	  {
		ImmutableList.Builder<string> builder = ImmutableList.builder();
		foreach (XmlElement child in party.getChildren("partyId"))
		{
		  if (child.hasContent())
		  {
			builder.add(child.Content);
		  }
		}
		return builder.build();
	  }

	  // locate our party href/id reference
	  private ImmutableList<string> findOurParty(FpmlPartySelector ourPartySelector)
	  {
		// check for "any" selector to avoid logging message in normal case
		if (ourPartySelector == FpmlPartySelector.any())
		{
		  return ImmutableList.of();
		}
		IList<string> selected = ourPartySelector.selectParties(parties);
		if (selected.Count > 0)
		{
		  foreach (string id in selected)
		  {
			if (!parties.Keys.Contains(id))
			{
			  throw new FpmlParseException(Messages.format("Selector returned an ID '{}' that is not present in the document: {}", id, parties));
			}
		  }
		  return ImmutableList.copyOf(selected);
		}
		log.warn("Failed to resolve \"our\" counterparty from FpML document, using leg defaults instead: " + parties);
		return ImmutableList.of();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FpML root element.
	  /// <para>
	  /// This is not necessarily the root of the whole document.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the FpML root element </returns>
	  public XmlElement FpmlRoot
	  {
		  get
		  {
			return fpmlRoot;
		  }
	  }

	  /// <summary>
	  /// Gets the map of href/id references.
	  /// </summary>
	  /// <returns> the reference map </returns>
	  public ImmutableMap<string, XmlElement> References
	  {
		  get
		  {
			return references;
		  }
	  }

	  /// <summary>
	  /// Gets the map of party identifiers keyed by href/id reference.
	  /// </summary>
	  /// <returns> the party map </returns>
	  public ImmutableListMultimap<string, string> Parties
	  {
		  get
		  {
			return parties;
		  }
	  }

	  /// <summary>
	  /// Gets the party href/id references representing "our" party.
	  /// <para>
	  /// In a typical trade there are two parties, where one pays and the other receives.
	  /// In FpML these parties are represented by the party structure, which lists each party
	  /// and assigns them identifiers. These identifiers are then used throughout the rest
	  /// of the FpML document to specify who pays/receives each item.
	  /// By contrast, the Strata trade model is directional. Each item in the Strata trade
	  /// specifies whether it is pay or receive with respect to the company running the library.
	  /// </para>
	  /// <para>
	  /// To convert between these two models, the <seealso cref="FpmlPartySelector"/> is used to find
	  /// "our" party identifiers, in other words those party identifiers that belong to the
	  /// company running the library. Note that the matching occurs against the content of
	  /// {@code <partyId>} but the result of this method is the content of the attribute {@code <party id="">}.
	  /// </para>
	  /// <para>
	  /// Most FpML documents have one party identifier for each party, however it is
	  /// possible for a document to contain multiple identifiers for the same party.
	  /// The list allows all these parties to be stored.
	  /// The list will be empty if "our" party could not be identified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> our party, empty if not known </returns>
	  public IList<string> OurPartyHrefIds
	  {
		  get
		  {
			return ourPartyHrefIds;
		  }
	  }

	  /// <summary>
	  /// Gets the reference data.
	  /// <para>
	  /// Use of reference data is not necessary to parse most FpML documents.
	  /// It is only needed to handle some edge cases, notably around relative dates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the reference data </returns>
	  public ReferenceData ReferenceData
	  {
		  get
		  {
			return refData;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses the trade header element.
	  /// <para>
	  /// This parses the trade date and identifier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeEl">  the trade element </param>
	  /// <returns> the trade info builder </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public TradeInfoBuilder parseTradeInfo(XmlElement tradeEl)
	  {
		XmlElement tradeHeaderEl = tradeEl.getChild("tradeHeader");
		LocalDate tradeDate = parseDate(tradeHeaderEl.getChild("tradeDate"));
		return tradeInfoParser.parseTrade(this, tradeDate, parseAllTradeIds(tradeHeaderEl));
	  }

	  // find all trade IDs by party herf id
	  private ListMultimap<string, StandardId> parseAllTradeIds(XmlElement tradeHeaderEl)
	  {
		// look through each partyTradeIdentifier
		ListMultimap<string, StandardId> allIds = ArrayListMultimap.create();
		IList<XmlElement> partyTradeIdentifierEls = tradeHeaderEl.getChildren("partyTradeIdentifier");
		foreach (XmlElement partyTradeIdentifierEl in partyTradeIdentifierEls)
		{
		  Optional<XmlElement> partyRefOptEl = partyTradeIdentifierEl.findChild("partyReference");
		  if (partyRefOptEl.Present && partyRefOptEl.get().findAttribute(HREF).Present)
		  {
			string partyHref = partyRefOptEl.get().findAttribute(HREF).get();
			// try to find a tradeId, either in versionedTradeId or as a direct child
			Optional<XmlElement> vtradeIdOptEl = partyTradeIdentifierEl.findChild("versionedTradeId");
			Optional<XmlElement> tradeIdOptEl = vtradeIdOptEl.map(vt => vt.getChild("tradeId")).orElse(partyTradeIdentifierEl.findChild("tradeId"));
			if (tradeIdOptEl.Present && tradeIdOptEl.get().findAttribute("tradeIdScheme").Present)
			{
			  XmlElement tradeIdEl = tradeIdOptEl.get();
			  string scheme = tradeIdEl.getAttribute("tradeIdScheme");
			  // ignore if there is an empty scheme or value
			  if (scheme.Length > 0 && tradeIdEl.Content.Length > 0)
			  {
				allIds.put(partyHref, StandardId.of(StandardId.encodeScheme(scheme), tradeIdEl.Content));
			  }
			}
		  }
		}
		return allIds;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'BuyerSeller.model' to a {@code BuySell}.
	  /// <para>
	  /// The <seealso cref="TradeInfo"/> builder is updated with the counterparty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseEl">  the FpML payer receiver model element </param>
	  /// <param name="tradeInfoBuilder">  the builder of the trade info </param>
	  /// <returns> the pay/receive flag </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public BuySell parseBuyerSeller(XmlElement baseEl, TradeInfoBuilder tradeInfoBuilder)
	  {
		string buyerPartyReference = baseEl.getChild("buyerPartyReference").getAttribute(FpmlDocument.HREF);
		string sellerPartyReference = baseEl.getChild("sellerPartyReference").getAttribute(FpmlDocument.HREF);
		if (ourPartyHrefIds.Empty || ourPartyHrefIds.contains(buyerPartyReference))
		{
		  tradeInfoBuilder.counterparty(StandardId.of(FPML_PARTY_SCHEME, parties.get(sellerPartyReference).get(0)));
		  return BuySell.BUY;
		}
		else if (ourPartyHrefIds.contains(sellerPartyReference))
		{
		  tradeInfoBuilder.counterparty(StandardId.of(FPML_PARTY_SCHEME, parties.get(buyerPartyReference).get(0)));
		  return BuySell.SELL;
		}
		else
		{
		  throw new FpmlParseException(Messages.format("Neither buyerPartyReference nor sellerPartyReference contain our party ID: {}", ourPartyHrefIds));
		}
	  }

	  /// <summary>
	  /// Converts an FpML 'PayerReceiver.model' to a {@code PayReceive}.
	  /// <para>
	  /// The <seealso cref="TradeInfo"/> builder is updated with the counterparty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="baseEl">  the FpML payer receiver model element </param>
	  /// <param name="tradeInfoBuilder">  the builder of the trade info </param>
	  /// <returns> the pay/receive flag </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public PayReceive parsePayerReceiver(XmlElement baseEl, TradeInfoBuilder tradeInfoBuilder)
	  {
		string payerPartyReference = baseEl.getChild("payerPartyReference").getAttribute(HREF);
		string receiverPartyReference = baseEl.getChild("receiverPartyReference").getAttribute(HREF);
		object currentCounterparty = tradeInfoBuilder.build().Counterparty.orElse(null);
		// determine direction and setup counterparty
		if ((ourPartyHrefIds.Empty && currentCounterparty == null) || ourPartyHrefIds.contains(payerPartyReference))
		{
		  StandardId proposedCounterparty = StandardId.of(FPML_PARTY_SCHEME, parties.get(receiverPartyReference).get(0));
		  if (currentCounterparty == null)
		  {
			tradeInfoBuilder.counterparty(proposedCounterparty);
		  }
		  else if (!currentCounterparty.Equals(proposedCounterparty))
		  {
			throw new FpmlParseException(Messages.format("Two different counterparties found: {} and {}", currentCounterparty, proposedCounterparty));
		  }
		  return PayReceive.PAY;

		}
		else if (ourPartyHrefIds.Empty || ourPartyHrefIds.contains(receiverPartyReference))
		{
		  StandardId proposedCounterparty = StandardId.of(FPML_PARTY_SCHEME, parties.get(payerPartyReference).get(0));
		  if (currentCounterparty == null)
		  {
			tradeInfoBuilder.counterparty(proposedCounterparty);
		  }
		  else if (!currentCounterparty.Equals(proposedCounterparty))
		  {
			throw new FpmlParseException(Messages.format("Two different counterparties found: {} and {}", currentCounterparty, proposedCounterparty));
		  }
		  return PayReceive.RECEIVE;

		}
		else
		{
		  throw new FpmlParseException(Messages.format("Neither payerPartyReference nor receiverPartyReference contain our party ID: {}", ourPartyHrefIds));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'AdjustedRelativeDateOffset' to a resolved {@code LocalDate}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML adjustable date element </param>
	  /// <returns> the resolved date </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public AdjustableDate parseAdjustedRelativeDateOffset(XmlElement baseEl)
	  {
		// FpML content: ('periodMultiplier', 'period', 'dayType?',
		//                'businessDayConvention', 'BusinessCentersOrReference.model?'
		//                'dateRelativeTo', 'adjustedDate', 'relativeDateAdjustments?')
		// The 'adjustedDate' element is ignored
		XmlElement relativeToEl = lookupReference(baseEl.getChild("dateRelativeTo"));
		LocalDate baseDate;
		if (relativeToEl.hasContent())
		{
		  baseDate = parseDate(relativeToEl);
		}
		else if (relativeToEl.Name.Contains("relative"))
		{
		  baseDate = parseAdjustedRelativeDateOffset(relativeToEl).Unadjusted;
		}
		else
		{
		  throw new FpmlParseException("Unable to resolve 'dateRelativeTo' to a date: " + baseEl.getChild("dateRelativeTo").getAttribute(HREF));
		}
		Period period = parsePeriod(baseEl);
		Optional<XmlElement> dayTypeEl = baseEl.findChild("dayType");
		bool calendarDays = period.Zero || (dayTypeEl.Present && dayTypeEl.get().Content.Equals("Calendar"));
		BusinessDayAdjustment bda1 = parseBusinessDayAdjustments(baseEl);
		BusinessDayAdjustment bda2 = baseEl.findChild("relativeDateAdjustments").map(el => parseBusinessDayAdjustments(el)).orElse(bda1);
		// interpret and resolve, simple calendar arithmetic or business days
		LocalDate resolvedDate;
		if (period.Years > 0 || period.Months > 0 || calendarDays)
		{
		  resolvedDate = bda1.adjust(baseDate.plus(period), refData);
		}
		else
		{
		  LocalDate datePlusBusDays = bda1.Calendar.resolve(refData).shift(baseDate, period.Days);
		  resolvedDate = bda1.adjust(datePlusBusDays, refData);
		}
		return AdjustableDate.of(resolvedDate, bda2);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'RelativeDateOffset' to a {@code DaysAdjustment}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML adjustable date element </param>
	  /// <returns> the days adjustment </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public DaysAdjustment parseRelativeDateOffsetDays(XmlElement baseEl)
	  {
		// FpML content: ('periodMultiplier', 'period', 'dayType?',
		//                'businessDayConvention', 'BusinessCentersOrReference.model?'
		//                'dateRelativeTo', 'adjustedDate')
		// The 'dateRelativeTo' element is not used here
		// The 'adjustedDate' element is ignored
		Period period = parsePeriod(baseEl);
		if (period.toTotalMonths() != 0)
		{
		  throw new FpmlParseException("Expected days-based period but found " + period);
		}
		Optional<XmlElement> dayTypeEl = baseEl.findChild("dayType");
		bool calendarDays = period.Zero || (dayTypeEl.Present && dayTypeEl.get().Content.Equals("Calendar"));
		BusinessDayConvention fixingBdc = convertBusinessDayConvention(baseEl.getChild("businessDayConvention").Content);
		HolidayCalendarId calendar = parseBusinessCenters(baseEl);
		if (calendarDays)
		{
		  return DaysAdjustment.ofCalendarDays(period.Days, BusinessDayAdjustment.of(fixingBdc, calendar));
		}
		else
		{
		  return DaysAdjustment.ofBusinessDays(period.Days, calendar);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'AdjustableDate' or 'AdjustableDate2' to an {@code AdjustableDate}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML adjustable date element </param>
	  /// <returns> the adjustable date </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public AdjustableDate parseAdjustableDate(XmlElement baseEl)
	  {
		// FpML content: ('unadjustedDate', 'dateAdjustments', 'adjustedDate?')
		Optional<XmlElement> unadjOptEl = baseEl.findChild("unadjustedDate");
		if (unadjOptEl.Present)
		{
		  LocalDate unadjustedDate = parseDate(unadjOptEl.get());
		  Optional<XmlElement> adjustmentOptEl = baseEl.findChild("dateAdjustments");
		  Optional<XmlElement> adjustmentRefOptEl = baseEl.findChild("dateAdjustmentsReference");
		  if (!adjustmentOptEl.Present && !adjustmentRefOptEl.Present)
		  {
			return AdjustableDate.of(unadjustedDate);
		  }
		  XmlElement adjustmentEl = adjustmentRefOptEl.Present ? lookupReference(adjustmentRefOptEl.get()) : adjustmentOptEl.get();
		  BusinessDayAdjustment adjustment = parseBusinessDayAdjustments(adjustmentEl);
		  return AdjustableDate.of(unadjustedDate, adjustment);
		}
		LocalDate adjustedDate = parseDate(baseEl.getChild("adjustedDate"));
		return AdjustableDate.of(adjustedDate);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'BusinessDayAdjustments' to a {@code BusinessDayAdjustment}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML business centers or reference element to parse </param>
	  /// <returns> the business day adjustment </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public BusinessDayAdjustment parseBusinessDayAdjustments(XmlElement baseEl)
	  {
		// FpML content: ('businessDayConvention', 'BusinessCentersOrReference.model?')
		BusinessDayConvention bdc = convertBusinessDayConvention(baseEl.getChild("businessDayConvention").Content);
		Optional<XmlElement> centersEl = baseEl.findChild("businessCenters");
		Optional<XmlElement> centersRefEl = baseEl.findChild("businessCentersReference");
		HolidayCalendarId calendar = (centersEl.Present || centersRefEl.Present ? parseBusinessCenters(baseEl) : HolidayCalendarIds.NO_HOLIDAYS);
		return BusinessDayAdjustment.of(bdc, calendar);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'BusinessCentersOrReference.model' to a {@code HolidayCalendar}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML business centers or reference element to parse </param>
	  /// <returns> the holiday calendar </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public HolidayCalendarId parseBusinessCenters(XmlElement baseEl)
	  {
		// FpML content: ('businessCentersReference' | 'businessCenters')
		// FpML 'businessCenters' content: ('businessCenter+')
		// Each 'businessCenter' is a location treated as a holiday calendar
		Optional<XmlElement> optionalBusinessCentersEl = baseEl.findChild("businessCenters");
		XmlElement businessCentersEl = optionalBusinessCentersEl.orElseGet(() => lookupReference(baseEl.getChild("businessCentersReference")));
		HolidayCalendarId calendar = HolidayCalendarIds.NO_HOLIDAYS;
		foreach (XmlElement businessCenterEl in businessCentersEl.getChildren("businessCenter"))
		{
		  calendar = calendar.combinedWith(parseBusinessCenter(businessCenterEl));
		}
		return calendar;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'BusinessCenter' to a {@code HolidayCalendar}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML calendar element to parse </param>
	  /// <returns> the calendar </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public HolidayCalendarId parseBusinessCenter(XmlElement baseEl)
	  {
		validateScheme(baseEl, "businessCenterScheme", "http://www.fpml.org/coding-scheme/business-center");
		return convertHolidayCalendar(baseEl.Content);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'FloatingRateIndex.model' to a {@code PriceIndex}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML floating rate model element to parse </param>
	  /// <returns> the index </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public PriceIndex parsePriceIndex(XmlElement baseEl)
	  {
		XmlElement indexEl = baseEl.getChild("floatingRateIndex");
		validateScheme(indexEl, "floatingRateIndexScheme", "http://www.fpml.org/coding-scheme/inflation-index-description");
		FloatingRateName floatingName = FloatingRateName.of(indexEl.Content);
		return floatingName.toPriceIndex();
	  }

	  /// <summary>
	  /// Converts an FpML 'FloatingRateIndex.model' to an {@code Index}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML floating rate model element to parse </param>
	  /// <returns> the index </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public Index parseIndex(XmlElement baseEl)
	  {
		IList<Index> indexes = parseIndexes(baseEl);
		if (indexes.Count != 1)
		{
		  throw new FpmlParseException("Expected one index but found " + indexes.Count);
		}
		return indexes[0];
	  }

	  /// <summary>
	  /// Converts an FpML 'FloatingRateIndex' with multiple tenors to an {@code Index}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML floating rate index element to parse </param>
	  /// <returns> the index </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public IList<Index> parseIndexes(XmlElement baseEl)
	  {
		XmlElement indexEl = baseEl.getChild("floatingRateIndex");
		validateScheme(indexEl, "floatingRateIndexScheme", "http://www.fpml.org/coding-scheme/floating-rate-index");
		FloatingRateName floatingName = FloatingRateName.of(indexEl.Content);
		IList<XmlElement> tenorEls = baseEl.getChildren("indexTenor");
		if (tenorEls.Count == 0)
		{
		  return ImmutableList.of(floatingName.toOvernightIndex());
		}
		else
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  return tenorEls.Select(el => floatingName.toIborIndex(parseIndexTenor(el))).collect(toImmutableList());
		}
	  }

	  /// <summary>
	  /// Converts an FpML 'FloatingRateIndex' tenor to a {@code Tenor}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML floating rate index element to parse </param>
	  /// <returns> the period </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public Tenor parseIndexTenor(XmlElement baseEl)
	  {
		// FpML content: ('periodMultiplier', 'period')
		string multiplier = baseEl.getChild("periodMultiplier").Content;
		string unit = baseEl.getChild("period").Content;
		return convertIndexTenor(multiplier, unit);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'Period' to a {@code Period}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <returns> the period </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public Period parsePeriod(XmlElement baseEl)
	  {
		// FpML content: ('periodMultiplier', 'period')
		string multiplier = baseEl.getChild("periodMultiplier").Content;
		string unit = baseEl.getChild("period").Content;
		return Period.parse("P" + multiplier + unit);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML frequency to a {@code Frequency}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <returns> the frequency </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public Frequency parseFrequency(XmlElement baseEl)
	  {
		// FpML content: ('periodMultiplier', 'period')
		string multiplier = baseEl.getChild("periodMultiplier").Content;
		string unit = baseEl.getChild("period").Content;
		if (unit.Equals("T"))
		{
		  return Frequency.TERM;
		}
		return convertFrequency(multiplier, unit);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'Money' to a {@code CurrencyAmount}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML money element to parse </param>
	  /// <returns> the currency amount </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public CurrencyAmount parseCurrencyAmount(XmlElement baseEl)
	  {
		// FpML content: ('currency', 'amount')
		Currency currency = parseCurrency(baseEl.getChild("currency"));
		double amount = parseDecimal(baseEl.getChild("amount"));
		return CurrencyAmount.of(currency, amount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'Currency' to a {@code Currency}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML currency element to parse </param>
	  /// <returns> the currency </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public Currency parseCurrency(XmlElement baseEl)
	  {
		// allow various schemes
		// http://www.fpml.org/docs/FpML-AWG-Expanding-the-Currency-Codes-v2016.pdf
		validateScheme(baseEl, "currencyScheme", "http://www.fpml.org/coding-scheme/external/iso4217", "http://www.fpml.org/ext/iso4217", "http://www.fpml.org/coding-scheme/currency", "http://www.fpml.org/codingscheme/non-iso-currency"); // newer, see link above
		return Currency.of(baseEl.Content);
	  }

	  /// <summary>
	  /// Converts an FpML 'DayCountFraction' to a {@code DayCount}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML day count element to parse </param>
	  /// <returns> the day count </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public DayCount parseDayCountFraction(XmlElement baseEl)
	  {
		validateScheme(baseEl, "dayCountFractionScheme", "http://www.fpml.org/coding-scheme/day-count-fraction", "http://www.fpml.org/spec/2004/day-count-fraction"); // seen in the wild
		return convertDayCount(baseEl.Content);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML 'decimal' to a {@code double}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <returns> the double </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public double parseDecimal(XmlElement baseEl)
	  {
		return double.Parse(baseEl.Content);
	  }

	  /// <summary>
	  /// Converts an FpML 'date' to a {@code LocalDate}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <returns> the date </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public LocalDate parseDate(XmlElement baseEl)
	  {
		return convertDate(baseEl.Content);
	  }

	  /// <summary>
	  /// Converts an FpML 'hourMinuteTime' to a {@code LocalTime}.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <returns> the time </returns>
	  /// <exception cref="RuntimeException"> if unable to parse </exception>
	  public LocalTime parseTime(XmlElement baseEl)
	  {
		return LocalTime.parse(baseEl.Content);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an FpML day count string to a {@code DayCount}.
	  /// </summary>
	  /// <param name="fpmlDayCountName">  the day count name used by FpML </param>
	  /// <returns> the day count </returns>
	  /// <exception cref="IllegalArgumentException"> if the day count is not known </exception>
	  public DayCount convertDayCount(string fpmlDayCountName)
	  {
		return DayCount.extendedEnum().externalNames(ENUM_FPML).lookup(fpmlDayCountName);
	  }

	  /// <summary>
	  /// Converts an FpML business day convention string to a {@code BusinessDayConvention}.
	  /// </summary>
	  /// <param name="fmplBusinessDayConventionName">  the business day convention name used by FpML </param>
	  /// <returns> the business day convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the business day convention is not known </exception>
	  public BusinessDayConvention convertBusinessDayConvention(string fmplBusinessDayConventionName)
	  {
		return BusinessDayConvention.extendedEnum().externalNames(ENUM_FPML).lookup(fmplBusinessDayConventionName);
	  }

	  /// <summary>
	  /// Converts an FpML roll convention string to a {@code RollConvention}.
	  /// </summary>
	  /// <param name="fmplRollConventionName">  the roll convention name used by FpML </param>
	  /// <returns> the roll convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the roll convention is not known </exception>
	  public RollConvention convertRollConvention(string fmplRollConventionName)
	  {
		return RollConvention.extendedEnum().externalNames(ENUM_FPML).lookup(fmplRollConventionName);
	  }

	  /// <summary>
	  /// Converts an FpML business center string to a {@code HolidayCalendar}.
	  /// </summary>
	  /// <param name="fpmlBusinessCenter">  the business center name used by FpML </param>
	  /// <returns> the holiday calendar </returns>
	  /// <exception cref="IllegalArgumentException"> if the holiday calendar is not known </exception>
	  public HolidayCalendarId convertHolidayCalendar(string fpmlBusinessCenter)
	  {
		return HolidayCalendarId.of(fpmlBusinessCenter);
	  }

	  /// <summary>
	  /// Converts an FpML frequency string to a {@code Frequency}.
	  /// </summary>
	  /// <param name="multiplier">  the multiplier </param>
	  /// <param name="unit">  the unit </param>
	  /// <returns> the frequency </returns>
	  /// <exception cref="IllegalArgumentException"> if the frequency is not known </exception>
	  public Frequency convertFrequency(string multiplier, string unit)
	  {
		string periodStr = multiplier + unit;
		Frequency frequency = FREQUENCY_MAP[periodStr];
		return frequency != null ? frequency : Frequency.parse(periodStr);
	  }

	  /// <summary>
	  /// Converts an FpML tenor string to a {@code Tenor}.
	  /// </summary>
	  /// <param name="multiplier">  the multiplier </param>
	  /// <param name="unit">  the unit </param>
	  /// <returns> the tenor </returns>
	  /// <exception cref="IllegalArgumentException"> if the tenor is not known </exception>
	  public Tenor convertIndexTenor(string multiplier, string unit)
	  {
		string periodStr = multiplier + unit;
		Tenor tenor = TENOR_MAP[periodStr];
		return tenor != null ? tenor : Tenor.parse(periodStr);
	  }

	  /// <summary>
	  /// Converts an FpML date to a {@code LocalDate}.
	  /// </summary>
	  /// <param name="dateStr">  the business center name used by FpML </param>
	  /// <returns> the holiday calendar </returns>
	  /// <exception cref="DateTimeParseException"> if the date cannot be parsed </exception>
	  public LocalDate convertDate(string dateStr)
	  {
		return LocalDate.parse(dateStr, FPML_DATE_FORMAT);
	  }

	  /// <summary>
	  /// Returns the {@code ZoneId} matching this string representation of a holiday calendar id.
	  /// </summary>
	  /// <param name="holidayCalendarId">  the holiday calendar id string. </param>
	  /// <returns> an optional zone id, an empty optional is returned if no zone id can be found for the holiday calendar id. </returns>
	  public Optional<ZoneId> getZoneId(string holidayCalendarId)
	  {
		ZoneId zoneId = HOLIDAY_CALENDARID_MAP[holidayCalendarId];
		if (zoneId == null)
		{
		  return null;
		}
		return zoneId;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Validates that a specific element is not present.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <param name="elementName">  the element name </param>
	  /// <exception cref="FpmlParseException"> if the element is found </exception>
	  public void validateNotPresent(XmlElement baseEl, string elementName)
	  {
		if (baseEl.findChild(elementName).Present)
		{
		  throw new FpmlParseException("Unsupported element: '" + elementName + "'");
		}
	  }

	  /// <summary>
	  /// Validates that the scheme attribute is known.
	  /// </summary>
	  /// <param name="baseEl">  the FpML element to parse </param>
	  /// <param name="schemeAttr">  the scheme attribute name </param>
	  /// <param name="schemeValues">  the scheme attribute values that are accepted </param>
	  /// <exception cref="FpmlParseException"> if the scheme does not match </exception>
	  public void validateScheme(XmlElement baseEl, string schemeAttr, params string[] schemeValues)
	  {
		if (baseEl.Attributes.containsKey(schemeAttr))
		{
		  string scheme = baseEl.getAttribute(schemeAttr);
		  foreach (string schemeValue in schemeValues)
		  {
			if (scheme.StartsWith(schemeValue, StringComparison.Ordinal))
			{
			  return;
			}
		  }
		  throw new FpmlParseException("Unknown '" + schemeAttr + "' attribute value: " + scheme);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Looks up an element by href/id reference.
	  /// </summary>
	  /// <param name="hrefEl">  the element containing the href/id </param>
	  /// <returns> the matched element </returns>
	  /// <exception cref="FpmlParseException"> if the reference is not found </exception>
	  // lookup an element via href/id reference
	  public XmlElement lookupReference(XmlElement hrefEl)
	  {
		string hrefId = hrefEl.getAttribute(HREF);
		XmlElement el = references.get(hrefId);
		if (el == null)
		{
		  throw new FpmlParseException(Messages.format("Document reference not found: href='{}'", hrefId));
		}
		return el;
	  }

	}

}
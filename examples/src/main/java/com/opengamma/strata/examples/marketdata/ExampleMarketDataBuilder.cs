using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ListMultimap = com.google.common.collect.ListMultimap;
	using Maps = com.google.common.collect.Maps;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Messages = com.opengamma.strata.collect.Messages;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using ImmutableMarketDataBuilder = com.opengamma.strata.data.ImmutableMarketDataBuilder;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using FixingSeriesCsvLoader = com.opengamma.strata.loader.csv.FixingSeriesCsvLoader;
	using QuotesCsvLoader = com.opengamma.strata.loader.csv.QuotesCsvLoader;
	using RatesCurvesCsvLoader = com.opengamma.strata.loader.csv.RatesCurvesCsvLoader;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using RatesMarketDataLookup = com.opengamma.strata.measure.rate.RatesMarketDataLookup;

	/// <summary>
	/// Builds a market data snapshot from user-editable files in a prescribed directory structure.
	/// <para>
	/// Descendants of this class provide the ability to source this directory structure from any
	/// location.
	/// </para>
	/// <para>
	/// The directory structure must look like:
	/// <ul>
	///   <li>root
	///   <ul>
	///     <li>curves
	///     <ul>
	///       <li>groups.csv
	///       <li>settings.csv
	///       <li>one or more curve CSV files
	///     </ul>
	///     <li>historical-fixings
	///     <ul>
	///       <li>one or more time-series CSV files
	///     </ul>
	///   </ul>
	/// </ul>
	/// </para>
	/// </summary>
	public abstract class ExampleMarketDataBuilder
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(ExampleMarketDataBuilder));

	  /// <summary>
	  /// The name of the subdirectory containing historical fixings. </summary>
	  private const string HISTORICAL_FIXINGS_DIR = "historical-fixings";

	  /// <summary>
	  /// The name of the subdirectory containing calibrated rates curves. </summary>
	  private const string CURVES_DIR = "curves";
	  /// <summary>
	  /// The name of the curve groups file. </summary>
	  private const string CURVES_GROUPS_FILE = "groups.csv";
	  /// <summary>
	  /// The name of the curve settings file. </summary>
	  private const string CURVES_SETTINGS_FILE = "settings.csv";

	  /// <summary>
	  /// The name of the subdirectory containing simple market quotes. </summary>
	  private const string QUOTES_DIR = "quotes";
	  /// <summary>
	  /// The name of the quotes file. </summary>
	  private const string QUOTES_FILE = "quotes.csv";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from a given classpath resource root location using the class loader
	  /// which created this class.
	  /// <para>
	  /// This is designed to handle resource roots which may physically correspond to a directory on
	  /// disk, or be located within a jar file.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resourceRoot">  the resource root path </param>
	  /// <returns> the market data builder </returns>
	  public static ExampleMarketDataBuilder ofResource(string resourceRoot)
	  {
		return ofResource(resourceRoot, typeof(ExampleMarketDataBuilder).ClassLoader);
	  }

	  /// <summary>
	  /// Creates an instance from a given classpath resource root location, using the given class loader
	  /// to find the resource.
	  /// <para>
	  /// This is designed to handle resource roots which may physically correspond to a directory on
	  /// disk, or be located within a jar file.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resourceRoot">  the resource root path </param>
	  /// <param name="classLoader">  the class loader with which to find the resource </param>
	  /// <returns> the market data builder </returns>
	  public static ExampleMarketDataBuilder ofResource(string resourceRoot, ClassLoader classLoader)
	  {
		// classpath resources are forward-slash separated
		string qualifiedRoot = resourceRoot;
		qualifiedRoot = qualifiedRoot.StartsWith("/", StringComparison.Ordinal) ? qualifiedRoot.Substring(1) : qualifiedRoot;
		qualifiedRoot = qualifiedRoot.StartsWith("\\", StringComparison.Ordinal) ? qualifiedRoot.Substring(1) : qualifiedRoot;
		qualifiedRoot = qualifiedRoot.EndsWith("/", StringComparison.Ordinal) ? qualifiedRoot : qualifiedRoot + "/";
		URL url = classLoader.getResource(qualifiedRoot);
		if (url == null)
		{
		  throw new System.ArgumentException(Messages.format("Classpath resource not found: {}", qualifiedRoot));
		}
		if (url.Protocol != null && "jar".Equals(url.Protocol.ToLower(Locale.ENGLISH)))
		{
		  // Inside a JAR
		  int classSeparatorIdx = url.File.IndexOf("!");
		  if (classSeparatorIdx == -1)
		  {
			throw new System.ArgumentException(Messages.format("Unexpected JAR file URL: {}", url));
		  }
		  string jarPath = StringHelper.SubstringSpecial(url.File, "file:".Length, classSeparatorIdx);
		  File jarFile;
		  try
		  {
			jarFile = new File(jarPath);
		  }
		  catch (Exception e)
		  {
			throw new System.ArgumentException(Messages.format("Unable to create file for JAR: {}", jarPath), e);
		  }
		  return new JarMarketDataBuilder(jarFile, resourceRoot);
		}
		else
		{
		  // Resource is on disk
		  File file;
		  try
		  {
			file = new File(url.toURI());
		  }
		  catch (URISyntaxException e)
		  {
			throw new System.ArgumentException(Messages.format("Unexpected file location: {}", url), e);
		  }
		  return new DirectoryMarketDataBuilder(file.toPath());
		}
	  }

	  /// <summary>
	  /// Creates an instance from a given directory root.
	  /// </summary>
	  /// <param name="rootPath">  the root directory </param>
	  /// <returns> the market data builder </returns>
	  public static ExampleMarketDataBuilder ofPath(Path rootPath)
	  {
		return new DirectoryMarketDataBuilder(rootPath);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds a market data snapshot from this environment.
	  /// </summary>
	  /// <param name="marketDataDate">  the date of the market data </param>
	  /// <returns> the snapshot </returns>
	  public virtual ImmutableMarketData buildSnapshot(LocalDate marketDataDate)
	  {
		ImmutableMarketDataBuilder builder = ImmutableMarketData.builder(marketDataDate);
		loadFixingSeries(builder);
		loadRatesCurves(builder, marketDataDate);
		loadQuotes(builder, marketDataDate);
		loadFxRates(builder);
		return builder.build();
	  }

	  /// <summary>
	  /// Gets the rates market lookup to use with this environment.
	  /// </summary>
	  /// <param name="marketDataDate">  the date of the market data </param>
	  /// <returns> the rates lookup </returns>
	  public virtual RatesMarketDataLookup ratesLookup(LocalDate marketDataDate)
	  {
		SortedDictionary<LocalDate, RatesCurveGroup> curves = loadAllRatesCurves();
		return RatesMarketDataLookup.of(curves[marketDataDate]);
	  }

	  /// <summary>
	  /// Gets all rates curves.
	  /// </summary>
	  /// <returns> the map of all rates curves </returns>
	  public virtual SortedDictionary<LocalDate, RatesCurveGroup> loadAllRatesCurves()
	  {
		if (!subdirectoryExists(CURVES_DIR))
		{
		  throw new System.ArgumentException("No rates curves directory found");
		}
		ResourceLocator curveGroupsResource = getResource(CURVES_DIR, CURVES_GROUPS_FILE);
		if (curveGroupsResource == null)
		{
		  throw new System.ArgumentException(Messages.format("Unable to load rates curves: curve groups file not found at {}/{}", CURVES_DIR, CURVES_GROUPS_FILE));
		}
		ResourceLocator curveSettingsResource = getResource(CURVES_DIR, CURVES_SETTINGS_FILE);
		if (curveSettingsResource == null)
		{
		  throw new System.ArgumentException(Messages.format("Unable to load rates curves: curve settings file not found at {}/{}", CURVES_DIR, CURVES_SETTINGS_FILE));
		}
		ListMultimap<LocalDate, RatesCurveGroup> curveGroups = RatesCurvesCsvLoader.loadAllDates(curveGroupsResource, curveSettingsResource, RatesCurvesResources);

		// There is only one curve group in the market data file so this will always succeed
		IDictionary<LocalDate, RatesCurveGroup> curveGroupMap = Maps.transformValues(curveGroups.asMap(), groups => groups.GetEnumerator().next());
		return new SortedDictionary<>(curveGroupMap);
	  }

	  //-------------------------------------------------------------------------
	  private void loadFixingSeries(ImmutableMarketDataBuilder builder)
	  {
		if (!subdirectoryExists(HISTORICAL_FIXINGS_DIR))
		{
		  log.debug("No historical fixings directory found");
		  return;
		}
		try
		{
		  ICollection<ResourceLocator> fixingSeriesResources = getAllResources(HISTORICAL_FIXINGS_DIR);
		  IDictionary<ObservableId, LocalDateDoubleTimeSeries> fixingSeries = FixingSeriesCsvLoader.load(fixingSeriesResources);
		  builder.addTimeSeriesMap(fixingSeries);
		}
		catch (Exception e)
		{
		  log.error("Error loading fixing series", e);
		}
	  }

	  private void loadRatesCurves(ImmutableMarketDataBuilder builder, LocalDate marketDataDate)
	  {
		if (!subdirectoryExists(CURVES_DIR))
		{
		  log.debug("No rates curves directory found");
		  return;
		}

		ResourceLocator curveGroupsResource = getResource(CURVES_DIR, CURVES_GROUPS_FILE);
		if (curveGroupsResource == null)
		{
		  log.error("Unable to load rates curves: curve groups file not found at {}/{}", CURVES_DIR, CURVES_GROUPS_FILE);
		  return;
		}

		ResourceLocator curveSettingsResource = getResource(CURVES_DIR, CURVES_SETTINGS_FILE);
		if (curveSettingsResource == null)
		{
		  log.error("Unable to load rates curves: curve settings file not found at {}/{}", CURVES_DIR, CURVES_SETTINGS_FILE);
		  return;
		}
		try
		{
		  ICollection<ResourceLocator> curvesResources = RatesCurvesResources;
		  IList<RatesCurveGroup> ratesCurves = RatesCurvesCsvLoader.load(marketDataDate, curveGroupsResource, curveSettingsResource, curvesResources);

		  foreach (RatesCurveGroup group in ratesCurves)
		  {
			// add entry for higher level discount curve name
			group.DiscountCurves.forEach((ccy, curve) => builder.addValue(CurveId.of(group.Name, curve.Name), curve));
			// add entry for higher level forward curve name
			group.ForwardCurves.forEach((idx, curve) => builder.addValue(CurveId.of(group.Name, curve.Name), curve));
		  }

		}
		catch (Exception e)
		{
		  log.error("Error loading rates curves", e);
		}
	  }

	  // load quotes
	  private void loadQuotes(ImmutableMarketDataBuilder builder, LocalDate marketDataDate)
	  {
		if (!subdirectoryExists(QUOTES_DIR))
		{
		  log.debug("No quotes directory found");
		  return;
		}

		ResourceLocator quotesResource = getResource(QUOTES_DIR, QUOTES_FILE);
		if (quotesResource == null)
		{
		  log.error("Unable to load quotes: quotes file not found at {}/{}", QUOTES_DIR, QUOTES_FILE);
		  return;
		}

		try
		{
		  IDictionary<QuoteId, double> quotes = QuotesCsvLoader.load(marketDataDate, quotesResource);
		  builder.addValueMap(quotes);

		}
		catch (Exception ex)
		{
		  log.error("Error loading quotes", ex);
		}
	  }

	  private void loadFxRates(ImmutableMarketDataBuilder builder)
	  {
		// TODO - load from CSV file - format to be defined
		builder.addValue(FxRateId.of(Currency.GBP, Currency.USD), FxRate.of(Currency.GBP, Currency.USD, 1.61));
	  }

	  //-------------------------------------------------------------------------
	  private ICollection<ResourceLocator> RatesCurvesResources
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
			return getAllResources(CURVES_DIR).Where(res => !res.Locator.EndsWith(CURVES_GROUPS_FILE)).Where(res => !res.Locator.EndsWith(CURVES_SETTINGS_FILE)).collect(toImmutableList());
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets all available resources from a given subdirectory.
	  /// </summary>
	  /// <param name="subdirectoryName">  the name of the subdirectory </param>
	  /// <returns> a collection of locators for the resources in the subdirectory </returns>
	  protected internal abstract ICollection<ResourceLocator> getAllResources(string subdirectoryName);

	  /// <summary>
	  /// Gets a specific resource from a given subdirectory.
	  /// </summary>
	  /// <param name="subdirectoryName">  the name of the subdirectory </param>
	  /// <param name="resourceName">  the name of the resource </param>
	  /// <returns> a locator for the requested resource </returns>
	  protected internal abstract ResourceLocator getResource(string subdirectoryName, string resourceName);

	  /// <summary>
	  /// Checks whether a specific subdirectory exists.
	  /// </summary>
	  /// <param name="subdirectoryName">  the name of the subdirectory </param>
	  /// <returns> whether the subdirectory exists </returns>
	  protected internal abstract bool subdirectoryExists(string subdirectoryName);

	}

}
using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.sensitivity.CurveSensitivitiesType.ZERO_RATE_DELTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.sensitivity.CurveSensitivitiesType.ZERO_RATE_GAMMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ListMultimap = com.google.common.collect.ListMultimap;
	using CharSource = com.google.common.io.CharSource;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CsvRow = com.opengamma.strata.collect.io.CsvRow;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using FailureItem = com.opengamma.strata.collect.result.FailureItem;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using ValueWithFailures = com.opengamma.strata.collect.result.ValueWithFailures;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using CurveSensitivities = com.opengamma.strata.market.sensitivity.CurveSensitivities;
	using CurveSensitivitiesType = com.opengamma.strata.market.sensitivity.CurveSensitivitiesType;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;

	/// <summary>
	/// Test <seealso cref="SensitivityCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public final class SensitivityCsvLoaderTest
	public sealed class SensitivityCsvLoaderTest
	{

	  private static readonly CurveSensitivitiesType OTHER = CurveSensitivitiesType.of("Other");
	  private static readonly AttributeType<string> CCP_ATTR = AttributeType.of("CCP");
	  private static readonly SensitivityCsvInfoResolver RESOLVER_CCP = new SensitivityCsvInfoResolverAnonymousInnerClass();

	  private class SensitivityCsvInfoResolverAnonymousInnerClass : SensitivityCsvInfoResolver
	  {
		  public SensitivityCsvInfoResolverAnonymousInnerClass()
		  {
		  }

		  public override bool isInfoColumn(string headerLowerCase)
		  {
			return "ccp".Equals(headerLowerCase);
		  }

		  public override PortfolioItemInfo parseSensitivityInfo(CsvRow row, PortfolioItemInfo info)
		  {
			return info.withAttribute(CCP_ATTR, row.getValue("CCP"));
		  }

		  public override ReferenceData ReferenceData
		  {
			  get
			  {
				return ReferenceData.standard();
			  }
		  }
	  }
	  private static readonly SensitivityCsvInfoResolver RESOLVER_DATE = new SensitivityCsvInfoResolverAnonymousInnerClass2();

	  private class SensitivityCsvInfoResolverAnonymousInnerClass2 : SensitivityCsvInfoResolver
	  {
		  public SensitivityCsvInfoResolverAnonymousInnerClass2()
		  {
		  }

		  public override bool TenorRequired
		  {
			  get
			  {
				return false;
			  }
		  }

		  public override Tenor checkSensitivityTenor(Tenor tenor)
		  {
			Tenor adjustedTenor = SensitivityCsvInfoResolver.this.checkSensitivityTenor(tenor);
			return adjustedTenor.Equals(Tenor.TENOR_12M) ? Tenor.TENOR_1Y : adjustedTenor;
		  }

		  public override ReferenceData ReferenceData
		  {
			  get
			  {
				return ReferenceData.standard();
			  }
		  }
	  }
	  private static readonly SensitivityCsvLoader LOADER = SensitivityCsvLoader.standard();
	  private static readonly SensitivityCsvLoader LOADER_CCP = SensitivityCsvLoader.of(RESOLVER_CCP);
	  private static readonly SensitivityCsvLoader LOADER_DATE = SensitivityCsvLoader.of(RESOLVER_DATE);

	  //-------------------------------------------------------------------------
	  public void test_parse_standard()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-standard.csv").CharSource;

		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 2);
		string tenors = "1D, 1W, 2W, 1M, 3M, 6M, 12M, 2Y, 5Y, 10Y";
		assertSens(csens0, ZERO_RATE_DELTA, "GBP", GBP, tenors, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		assertSens(csens0, ZERO_RATE_DELTA, "GBP-LIBOR", GBP, tenors, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		assertSens(csens0, ZERO_RATE_GAMMA, "GBP", GBP, tenors, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1);
		assertSens(csens0, ZERO_RATE_GAMMA, "GBP-LIBOR", GBP, tenors, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1);
	  }

	  public void test_parse_standard_full()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-standard-full.csv").CharSource;

		assertEquals(LOADER_CCP.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_CCP.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 2);

		IList<CurveSensitivities> list0 = test.Value.get("SCHEME~TR1");
		assertEquals(list0.Count, 1);
		CurveSensitivities csens0 = list0[0];
		assertEquals(csens0.Info.getAttribute(CCP_ATTR), "LCH");
		assertEquals(csens0.TypedSensitivities.size(), 1);
		assertSens(csens0, ZERO_RATE_DELTA, "GBCURVE", GBP, "1M, 3M, 6M", 1, 2, 3);

		IList<CurveSensitivities> list1 = test.Value.get("OG-Sensitivity~TR2");
		assertEquals(list1.Count, 1);
		CurveSensitivities csens1 = list1[0];
		assertEquals(csens1.Info.getAttribute(CCP_ATTR), "CME");
		assertEquals(csens1.TypedSensitivities.size(), 1);
		assertSens(csens1, ZERO_RATE_GAMMA, "GBCURVE", GBP, "1M, 3M, 6M", 4, 5, 6);
	  }

	  public void test_parse_standard_dateInTenorColumn()
	  {
		CharSource source = CharSource.wrap("Reference,Sensitivity Type,Sensitivity Tenor,Value\n" + "GBP,ZeroRateGamma,2018-06-30,1\n");
		assertEquals(LOADER_DATE.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_DATE.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Invalid tenor '2018-06-30', must be expressed as nD, nW, nM or nY");
	  }

	  //-------------------------------------------------------------------------
	  public void test_parse_list()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-list.csv").CharSource;

		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 3);
		string tenors = "1D, 1W, 2W, 1M, 3M, 6M, 12M, 2Y, 5Y, 10Y";
		assertSens(csens0, ZERO_RATE_DELTA, "GBP", GBP, tenors, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		assertSens(csens0, ZERO_RATE_DELTA, "GBP-LIBOR", GBP, tenors, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		assertSens(csens0, ZERO_RATE_GAMMA, "GBP", GBP, tenors, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1);
		assertSens(csens0, ZERO_RATE_GAMMA, "GBP-LIBOR", GBP, tenors, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1);
		assertSens(csens0, OTHER, "GBP", GBP, tenors, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0);
		assertSens(csens0, OTHER, "GBP-LIBOR", GBP, tenors, 0, 0, 0, 0, 0, 0, 10, 0, 0, 0);
	  }

	  public void test_parse_list_full()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-list-full.csv").CharSource;

		assertEquals(LOADER_CCP.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_CCP.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 2);

		IList<CurveSensitivities> list0 = test.Value.get("SCHEME~TR1");
		assertEquals(list0.Count, 1);
		CurveSensitivities csens0 = list0[0];
		assertEquals(csens0.Info.getAttribute(CCP_ATTR), "LCH");
		assertEquals(csens0.TypedSensitivities.size(), 1);
		assertSens(csens0, ZERO_RATE_DELTA, "GBCURVE", GBP, "1M, 3M, 6M", 1, 2, 3);

		IList<CurveSensitivities> list1 = test.Value.get("OG-Sensitivity~TR2");
		assertEquals(list1.Count, 1);
		CurveSensitivities csens1 = list1[0];
		assertEquals(csens1.Info.getAttribute(CCP_ATTR), "CME");
		assertEquals(csens1.TypedSensitivities.size(), 1);
		assertSens(csens1, ZERO_RATE_GAMMA, "GBCURVE", GBP, "1M, 3M, 6M", 4, 5, 6);
	  }

	  public void test_parse_list_duplicateTenor()
	  {
		CharSource source = CharSource.wrap("Reference,Sensitivity Tenor,Zero Rate Delta\n" + "GBP,P1D,1\n" + "GBP,P1M,2\n" + "GBP,P1M,3\n");
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		assertSens(csens0, ZERO_RATE_DELTA, "GBP", GBP, "1D, 1M", 1, 5);
	  }

	  //-------------------------------------------------------------------------
	  public void test_parse_list_allRowsBadNoEmptySensitvityCreated()
	  {
		CharSource source = CharSource.wrap("Reference,Sensitivity Tenor,ZeroRateDelta\n" + "GBP,XX,1\n");
		assertEquals(LOADER_DATE.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_DATE.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Invalid tenor 'XX', must be expressed as nD, nW, nM or nY");
	  }

	  public void test_parse_list_unableToGetCurrency()
	  {
		CharSource source = CharSource.wrap("Reference,Sensitivity Tenor,Zero Rate Delta\n" + "X,1D,1.0");
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Unable to parse currency from reference, " + "consider adding a 'Currency' column");
	  }

	  public void test_parse_list_ioException()
	  {
		CharSource source = new CharSourceAnonymousInnerClass(this);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Failures.get(0).Reason, FailureReason.PARSING);
		assertEquals(test.Failures.get(0).Message.StartsWith("CSV file could not be parsed: "), true);
		assertEquals(test.Failures.get(0).Message.contains("Oops"), true);
	  }

	  private class CharSourceAnonymousInnerClass : CharSource
	  {
		  private readonly SensitivityCsvLoaderTest outerInstance;

		  public CharSourceAnonymousInnerClass(SensitivityCsvLoaderTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public java.io.Reader openStream() throws java.io.IOException
		  public override Reader openStream()
		  {
			throw new IOException("Oops");
		  }
	  }

	  public void test_parse_list_badDayMonthTenor()
	  {
		CharSource source = CharSource.wrap("Reference,Sensitivity Tenor,Zero Rate Delta\n" + "GBP-LIBOR,P2M1D,1.0");
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Failures.get(0).Reason, FailureReason.PARSING);
		assertEquals(test.Failures.get(0).Message, "CSV file could not be parsed at line 2: Invalid tenor, cannot mix years/months and days: 2M1D");
	  }

	  //-------------------------------------------------------------------------
	  public void test_parse_grid()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-grid.csv").CharSource;

		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		string tenors = "1D, 1W, 2W, 1M, 3M, 6M, 12M, 2Y, 5Y, 10Y";
		assertSens(csens0, ZERO_RATE_DELTA, "GBP", GBP, tenors, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		assertSens(csens0, ZERO_RATE_DELTA, "USD-LIBOR-3M", USD, tenors, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
	  }

	  public void test_parse_grid_full()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-grid-full.csv").CharSource;

		assertEquals(LOADER_CCP.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_CCP.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());

		IList<CurveSensitivities> list0 = test.Value.get("SCHEME~TR1");
		assertEquals(list0.Count, 1);
		CurveSensitivities csens0 = list0[0];
		assertEquals(csens0.Id, StandardId.of("SCHEME", "TR1"));
		assertEquals(csens0.Info.getAttribute(CCP_ATTR), "LCH");
		assertEquals(csens0.TypedSensitivities.size(), 2);
		assertSens(csens0, ZERO_RATE_DELTA, "GBCURVE", GBP, "1M, 3M, 6M", 1, 2, 3);
		assertSens(csens0, ZERO_RATE_GAMMA, "GBCURVE", GBP, "1M, 3M, 6M", 4, 5, 6);

		IList<CurveSensitivities> list1 = test.Value.get("OG-Sensitivity~TR2");
		assertEquals(list1.Count, 1);
		CurveSensitivities csens1 = list1[0];
		assertEquals(csens1.Id, StandardId.of("OG-Sensitivity", "TR2"));
		assertEquals(csens1.Info.getAttribute(CCP_ATTR), "CME");
		assertEquals(csens1.TypedSensitivities.size(), 1);
		assertSens(csens1, ZERO_RATE_DELTA, "GBCURVE", GBP, "1M, 3M, 6M", 7, 8, 9);
	  }

	  public void test_parse_grid_duplicateTenor()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,GBP\n" + "ZeroRateGamma,P6M,1\n" + "ZeroRateGamma,12M,2\n" + "ZeroRateGamma,12M,3\n");
		assertEquals(LOADER_DATE.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_DATE.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		assertSens(csens0, ZERO_RATE_GAMMA, "GBP", GBP, "6M, 1Y", 1, 5); // 12M -> 1Y
	  }

	  public void test_parse_grid_tenorAndDateColumns()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,Sensitivity Date,GBP\n" + "ZeroRateGamma,1M,2018-06-30,1\n");
		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		CurrencyParameterSensitivities cpss = csens0.getTypedSensitivity(ZERO_RATE_GAMMA);
		assertEquals(cpss.Sensitivities.size(), 1);
		CurrencyParameterSensitivity cps = cpss.Sensitivities.get(0);
		assertEquals(cps.ParameterMetadata.size(), 1);
		assertEquals(cps.ParameterMetadata.get(0), TenorDateParameterMetadata.of(date(2018, 6, 30), TENOR_1M));
	  }

	  public void test_parse_grid_dateColumn()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Date,GBP\n" + "ZeroRateGamma,2018-06-30,1\n");
		assertEquals(LOADER_DATE.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_DATE.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		CurrencyParameterSensitivities cpss = csens0.getTypedSensitivity(ZERO_RATE_GAMMA);
		assertEquals(cpss.Sensitivities.size(), 1);
		CurrencyParameterSensitivity cps = cpss.Sensitivities.get(0);
		assertEquals(cps.ParameterMetadata.size(), 1);
		assertEquals(cps.ParameterMetadata.get(0), LabelDateParameterMetadata.of(date(2018, 6, 30), "2018-06-30"));
	  }

	  public void test_parse_grid_dateInTenorColumn()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,GBP\n" + "ZeroRateGamma,2018-06-30,1\n");
		assertEquals(LOADER_DATE.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_DATE.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.size(), 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 1);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		CurrencyParameterSensitivities cpss = csens0.getTypedSensitivity(ZERO_RATE_GAMMA);
		assertEquals(cpss.Sensitivities.size(), 1);
		CurrencyParameterSensitivity cps = cpss.Sensitivities.get(0);
		assertEquals(cps.ParameterMetadata.size(), 1);
		assertEquals(cps.ParameterMetadata.get(0), LabelDateParameterMetadata.of(date(2018, 6, 30), "2018-06-30"));
	  }

	  //-------------------------------------------------------------------------
	  public void test_parse_grid_allRowsBadNoEmptySensitvityCreated()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,GBP\n" + "ZeroRateGamma,XX,1\n");
		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Invalid tenor 'XX', must be expressed as nD, nW, nM or nY");
	  }

	  public void test_parse_grid_badTenorWithValidDateColumn()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,Sensitivity Date,GBP\n" + "ZeroRateGamma,XXX,2018-06-30,1\n");
		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Invalid tenor 'XXX', must be expressed as nD, nW, nM or nY");
	  }

	  public void test_parse_grid_missingColumns()
	  {
		CharSource source = CharSource.wrap("GBP\n" + "1");
		assertEquals(LOADER.isKnownFormat(source), false);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed as sensitivities, invalid format");
	  }

	  public void test_parse_grid_neitherTenorNorDate()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,Sensitivity Date,GBP\n" + "ZeroRateGamma,,,1\n");
		assertEquals(LOADER_DATE.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER_DATE.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Unable to parse tenor or date, " + "check 'Sensitivity Tenor' and 'Sensitivity Date' columns");
	  }

	  public void test_parse_grid_dateButTenorRequired()
	  {
		CharSource source = CharSource.wrap("Sensitivity Type,Sensitivity Tenor,Sensitivity Date,GBP\n" + "ZeroRateGamma,,2018-06-30,1\n");
		assertEquals(LOADER.isKnownFormat(source), true);
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source));
		assertEquals(test.Failures.size(), 1);
		assertEquals(test.Value.size(), 0);
		FailureItem failure0 = test.Failures.get(0);
		assertEquals(failure0.Reason, FailureReason.PARSING);
		assertEquals(failure0.Message, "CSV file could not be parsed at line 2: Missing value for 'Sensitivity Tenor' column");
	  }

	  //-------------------------------------------------------------------------
	  public void test_parse_multipleSources()
	  {
		CharSource source1 = CharSource.wrap("Reference,Sensitivity Tenor,Zero Rate Delta\n" + "GBP-LIBOR,P1M,1.1\n" + "GBP-LIBOR,P2M,1.2\n");
		CharSource source2 = CharSource.wrap("Reference,Sensitivity Tenor,Zero Rate Delta\n" + "GBP-LIBOR,P3M,1.3\n" + "GBP-LIBOR,P6M,1.4\n");
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> test = LOADER.parse(ImmutableList.of(source1, source2));
		assertEquals(test.Failures.size(), 0, test.Failures.ToString());
		assertEquals(test.Value.Keys.Count, 1);
		IList<CurveSensitivities> list = test.Value.get("");
		assertEquals(list.Count, 2);

		CurveSensitivities csens0 = list[0];
		assertEquals(csens0.TypedSensitivities.size(), 1);
		assertSens(csens0, ZERO_RATE_DELTA, "GBP-LIBOR", GBP, "1M, 2M", 1.1, 1.2);

		CurveSensitivities csens1 = list[1];
		assertEquals(csens1.TypedSensitivities.size(), 1);
		assertSens(csens1, ZERO_RATE_DELTA, "GBP-LIBOR", GBP, "3M, 6M", 1.3, 1.4);
	  }

	  //-------------------------------------------------------------------------
	  private void assertSens(CurveSensitivities sens, CurveSensitivitiesType type, string curveNameStr, Currency currency, string tenors, params double[] values)
	  {

		CurveName curveName = CurveName.of(curveNameStr);
		CurrencyParameterSensitivity sensitivity = sens.getTypedSensitivity(type).getSensitivity(curveName, currency);
		assertEquals(sensitivity.MarketDataName, CurveName.of(curveNameStr));
		assertEquals(sensitivity.Currency, currency);
		assertEquals(metadataString(sensitivity.ParameterMetadata), tenors);
		assertEquals(sensitivity.Sensitivity, DoubleArray.ofUnsafe(values));
	  }

	  private string metadataString(ImmutableList<ParameterMetadata> parameterMetadata)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return parameterMetadata.Select(md => md.Label).collect(joining(", "));
	  }

	}

}
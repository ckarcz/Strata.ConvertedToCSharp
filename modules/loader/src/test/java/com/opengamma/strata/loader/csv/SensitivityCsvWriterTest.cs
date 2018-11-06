using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ValueWithFailures = com.opengamma.strata.collect.result.ValueWithFailures;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;
	using CurveSensitivities = com.opengamma.strata.market.sensitivity.CurveSensitivities;
	using AttributeType = com.opengamma.strata.product.AttributeType;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;

	/// <summary>
	/// Test <seealso cref="SensitivityCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public final class SensitivityCsvWriterTest
	public sealed class SensitivityCsvWriterTest
	{

	  private static readonly AttributeType<string> CCP_ATTR = AttributeType.of("CCP");
	  private static readonly SensitivityCsvInfoSupplier SUPPLIER_CCP = new SensitivityCsvInfoSupplierAnonymousInnerClass();

	  private class SensitivityCsvInfoSupplierAnonymousInnerClass : SensitivityCsvInfoSupplier
	  {
		  public SensitivityCsvInfoSupplierAnonymousInnerClass()
		  {
		  }

		  public override IList<string> headers(CurveSensitivities curveSens)
		  {
			return ImmutableList.of("CCP");
		  }

		  public override IList<string> values(IList<string> additionalHeaders, CurveSensitivities curveSens, CurrencyParameterSensitivity paramSens)
		  {

			return ImmutableList.of(curveSens.Info.findAttribute(CCP_ATTR).orElse(""));
		  }
	  }
	  private static readonly SensitivityCsvLoader LOADER = SensitivityCsvLoader.standard();
	  private static readonly SensitivityCsvWriter WRITER = SensitivityCsvWriter.standard();
	  private static readonly SensitivityCsvWriter WRITER_CCP = SensitivityCsvWriter.of(SUPPLIER_CCP);

	  //-------------------------------------------------------------------------
	  public void test_write_standard()
	  {
		CurveName curve1 = CurveName.of("GBDSC");
		CurveName curve2 = CurveName.of("GBFWD");
		// listed in reverse order to check ordering
		CurveSensitivities sens = CurveSensitivities.builder(PortfolioItemInfo.empty().withAttribute(CCP_ATTR, "LCH")).add(ZERO_RATE_GAMMA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_3M), 1).add(ZERO_RATE_GAMMA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_6M), 2).add(ZERO_RATE_DELTA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_3M), 3).add(ZERO_RATE_DELTA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_6M), 5).add(ZERO_RATE_DELTA, curve1, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_3M), 2).add(ZERO_RATE_DELTA, curve1, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_6M), 4).build();

		StringBuilder buf = new StringBuilder();
		WRITER_CCP.write(sens, buf);
		string content = buf.ToString();

		string expected = "" +
			"Reference,Sensitivity Type,Sensitivity Tenor,Currency,Value,CCP\n" +
			"GBDSC,ZeroRateDelta,3M,GBP,2.0,LCH\n" +
			"GBDSC,ZeroRateDelta,6M,GBP,4.0,LCH\n" +
			"GBFWD,ZeroRateDelta,3M,GBP,3.0,LCH\n" +
			"GBFWD,ZeroRateDelta,6M,GBP,5.0,LCH\n" +
			"GBFWD,ZeroRateGamma,3M,GBP,1.0,LCH\n" +
			"GBFWD,ZeroRateGamma,6M,GBP,2.0,LCH\n";
		assertEquals(content, expected);
	  }

	  public void test_write_standard_withDate()
	  {
		CurveName curve1 = CurveName.of("GBDSC");
		CurveName curve2 = CurveName.of("GBFWD");
		// listed in reverse order to check ordering
		CurveSensitivities sens = CurveSensitivities.builder(PortfolioItemInfo.empty().withAttribute(CCP_ATTR, "LCH")).add(ZERO_RATE_GAMMA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_3M), 1).add(ZERO_RATE_GAMMA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_6M), 2).add(ZERO_RATE_DELTA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_3M), 3).add(ZERO_RATE_DELTA, curve2, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_6M), 5).add(ZERO_RATE_DELTA, curve1, Currency.GBP, TenorDateParameterMetadata.of(date(2018, 6, 30), Tenor.TENOR_3M), 2).add(ZERO_RATE_DELTA, curve1, Currency.GBP, TenorParameterMetadata.of(Tenor.TENOR_6M), 4).build();

		StringBuilder buf = new StringBuilder();
		WRITER_CCP.write(sens, buf);
		string content = buf.ToString();

		string expected = "" +
			"Reference,Sensitivity Type,Sensitivity Tenor,Sensitivity Date,Currency,Value,CCP\n" +
			"GBDSC,ZeroRateDelta,3M,2018-06-30,GBP,2.0,LCH\n" +
			"GBDSC,ZeroRateDelta,6M,,GBP,4.0,LCH\n" +
			"GBFWD,ZeroRateDelta,3M,,GBP,3.0,LCH\n" +
			"GBFWD,ZeroRateDelta,6M,,GBP,5.0,LCH\n" +
			"GBFWD,ZeroRateGamma,3M,,GBP,1.0,LCH\n" +
			"GBFWD,ZeroRateGamma,6M,,GBP,2.0,LCH\n";
		assertEquals(content, expected);
	  }

	  public void test_write_standard_roundTrip()
	  {
		CharSource source = ResourceLocator.ofClasspath("com/opengamma/strata/loader/csv/sensitivity-standard.csv").CharSource;
		ValueWithFailures<ListMultimap<string, CurveSensitivities>> parsed1 = LOADER.parse(ImmutableList.of(source));
		assertEquals(parsed1.Failures.size(), 0, parsed1.Failures.ToString());
		assertEquals(parsed1.Value.size(), 1);
		IList<CurveSensitivities> csensList1 = parsed1.Value.get("");
		assertEquals(csensList1.Count, 1);
		CurveSensitivities csens1 = csensList1[0];

		StringBuilder buf = new StringBuilder();
		WRITER.write(csens1, buf);
		string content = buf.ToString();

		ValueWithFailures<ListMultimap<string, CurveSensitivities>> parsed2 = LOADER.parse(ImmutableList.of(CharSource.wrap(content)));
		assertEquals(parsed2.Failures.size(), 0, parsed2.Failures.ToString());
		assertEquals(parsed2.Value.size(), 1);
		IList<CurveSensitivities> csensList2 = parsed2.Value.get("");
		assertEquals(csensList2.Count, 1);
		CurveSensitivities csens2 = csensList2[0];

		assertEquals(csens2, csens1);
	  }

	}

}
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;

	/// <summary>
	/// Test <seealso cref="RatesCurveGroupDefinitionCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveGroupDefinitionCsvLoaderTest
	public class RatesCurveGroupDefinitionCsvLoaderTest
	{

	  private const string GROUPS_1 = "classpath:com/opengamma/strata/loader/csv/groups.csv";
	  private const string SETTINGS_1 = "classpath:com/opengamma/strata/loader/csv/settings.csv";
	  private const string CURVES_1 = "classpath:com/opengamma/strata/loader/csv/curves-1.csv";
	  private const string CURVES_2 = "classpath:com/opengamma/strata/loader/csv/curves-2.csv";
	  private static readonly LocalDate CURVE_DATE = LocalDate.of(2009, 7, 31);

	  //-------------------------------------------------------------------------
	  public virtual void test_loadCurveGroupDefinition()
	  {
		IList<RatesCurveGroupDefinition> defns = RatesCurveGroupDefinitionCsvLoader.loadCurveGroupDefinitions(ResourceLocator.of(GROUPS_1));
		assertEquals(defns.Count, 1);
		RatesCurveGroupDefinition defn = defns[0];
		assertEquals(defn.Entries.get(0), RatesCurveGroupEntry.builder().curveName(CurveName.of("USD-Disc")).discountCurrencies(USD).build());
		assertEquals(defn.Entries.get(1), RatesCurveGroupEntry.builder().curveName(CurveName.of("USD-3ML")).indices(USD_LIBOR_3M).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_writeCurveGroupDefinition()
	  {
		RatesCurveGroupDefinition defn = RatesCurveGroupDefinitionCsvLoader.loadCurveGroupDefinitions(ResourceLocator.of(GROUPS_1))[0];
		Appendable underlying = new StringBuilder();
		RatesCurveGroupDefinitionCsvLoader.writeCurveGroupDefinition(underlying, defn);
		string created = underlying.ToString();
		string expected = "Group Name,Curve Type,Reference,Curve Name" + Environment.NewLine +
				"Default,discount,USD,USD-Disc" + Environment.NewLine +
				"Default,forward,USD-LIBOR-3M,USD-3ML" + Environment.NewLine +
				"Default,forward,US-CPI-U,USD-CPI" + Environment.NewLine;
		assertEquals(created, expected);
	  }

	  public virtual void test_writeCurveGroup()
	  {
		IList<RatesCurveGroup> curveGroups = RatesCurvesCsvLoader.load(CURVE_DATE, ResourceLocator.of(GROUPS_1), ResourceLocator.of(SETTINGS_1), ImmutableList.of(ResourceLocator.of(CURVES_1), ResourceLocator.of(CURVES_2)));
		Appendable underlying = new StringBuilder();
		RatesCurveGroupDefinitionCsvLoader.writeCurveGroup(underlying, curveGroups[0]);
		string created = underlying.ToString();
		string expected = "Group Name,Curve Type,Reference,Curve Name" + Environment.NewLine +
				"Default,discount,USD,USD-Disc" + Environment.NewLine +
				"Default,forward,USD-LIBOR-3M,USD-3ML" + Environment.NewLine;
		assertEquals(created, expected);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_test_writeCurveGroupDefinition_roundtrip() throws Exception
	  public virtual void test_test_writeCurveGroupDefinition_roundtrip()
	  {
		IList<RatesCurveGroupDefinition> defn = RatesCurveGroupDefinitionCsvLoader.loadCurveGroupDefinitions(ResourceLocator.of(GROUPS_1));
		File tempFile = File.createTempFile("TestCurveGroupLoading", "csv");
		tempFile.deleteOnExit();
		RatesCurveGroupDefinitionCsvLoader.writeCurveGroupDefinition(tempFile, defn[0]);
		assertEquals(RatesCurveGroupDefinitionCsvLoader.loadCurveGroupDefinitions(ResourceLocator.ofFile(tempFile)), defn);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(RatesCurveGroupDefinitionCsvLoader));
	  }

	}

}
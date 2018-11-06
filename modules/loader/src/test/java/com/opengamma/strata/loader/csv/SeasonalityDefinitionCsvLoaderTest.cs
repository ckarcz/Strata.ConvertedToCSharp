using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using SeasonalityDefinition = com.opengamma.strata.market.curve.SeasonalityDefinition;

	/// <summary>
	/// Test <seealso cref="SeasonalityDefinitionCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SeasonalityDefinitionCsvLoaderTest
	public class SeasonalityDefinitionCsvLoaderTest
	{

	  private const string GROUPS_1 = "classpath:com/opengamma/strata/loader/csv/seasonality.csv";

	  //-------------------------------------------------------------------------
	  public virtual void test_loadSeasonalityDefinition()
	  {
		IDictionary<CurveName, SeasonalityDefinition> defns = SeasonalityDefinitionCsvLoader.loadSeasonalityDefinitions(ResourceLocator.of(GROUPS_1));
		assertEquals(defns.Count, 1);
		SeasonalityDefinition defn = defns[CurveName.of("USD-CPI")];
		assertEquals(defn.AdjustmentType, ShiftType.SCALED);
		assertEquals(defn.SeasonalityMonthOnMonth.size(), 12);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(SeasonalityDefinitionCsvLoader));
	  }

	}

}
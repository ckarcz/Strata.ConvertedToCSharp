using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using ShiftType = com.opengamma.strata.market.ShiftType;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveGroupName = com.opengamma.strata.market.curve.CurveGroupName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;

	/// <summary>
	/// Test <seealso cref="RatesCalibrationCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCalibrationCsvLoaderTest
	public class RatesCalibrationCsvLoaderTest
	{

	  private const string GROUPS_1 = "classpath:com/opengamma/strata/loader/csv/groups.csv";
	  private const string SETTINGS_1 = "classpath:com/opengamma/strata/loader/csv/settings.csv";
	  private const string SEASONALITY_1 = "classpath:com/opengamma/strata/loader/csv/seasonality.csv";
	  private const string CALIBRATION_1 = "classpath:com/opengamma/strata/loader/csv/calibration-1.csv";

	  private const string SETTINGS_EMPTY = "classpath:com/opengamma/strata/loader/csv/settings-empty.csv";
	  private const string CALIBRATION_INVALID_TYPE = "classpath:com/opengamma/strata/loader/csv/calibration-invalid-type.csv";

	  //-------------------------------------------------------------------------
	  public virtual void test_parsing()
	  {
		IDictionary<CurveGroupName, RatesCurveGroupDefinition> test = RatesCalibrationCsvLoader.loadWithSeasonality(ResourceLocator.of(GROUPS_1), ResourceLocator.of(SETTINGS_1), ResourceLocator.of(SEASONALITY_1), ImmutableList.of(ResourceLocator.of(CALIBRATION_1)));
		assertEquals(test.Count, 1);

		assertDefinition(test[CurveGroupName.of("Default")]);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Missing settings for curve: .*") public void test_noSettings()
	  public virtual void test_noSettings()
	  {
		RatesCalibrationCsvLoader.load(ResourceLocator.of(GROUPS_1), ResourceLocator.of(SETTINGS_EMPTY), ResourceLocator.of(CALIBRATION_1));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class, expectedExceptionsMessageRegExp = "Multiple entries with same key: .*") public void test_single_curve_multiple_Files()
	  public virtual void test_single_curve_multiple_Files()
	  {
		RatesCalibrationCsvLoader.load(ResourceLocator.of(GROUPS_1), ResourceLocator.of(SETTINGS_1), ImmutableList.of(ResourceLocator.of(CALIBRATION_1), ResourceLocator.of(CALIBRATION_1)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_invalid_curve_duplicate_points()
	  public virtual void test_invalid_curve_duplicate_points()
	  {
		RatesCalibrationCsvLoader.load(ResourceLocator.of(GROUPS_1), ResourceLocator.of(SETTINGS_1), ImmutableList.of(ResourceLocator.of(CALIBRATION_INVALID_TYPE)));
	  }

	  //-------------------------------------------------------------------------
	  private void assertDefinition(RatesCurveGroupDefinition defn)
	  {
		assertEquals(defn.Name, CurveGroupName.of("Default"));
		assertEquals(defn.Entries.size(), 3);
		assertEquals(defn.SeasonalityDefinitions.size(), 1);
		assertEquals(defn.SeasonalityDefinitions.get(CurveName.of("USD-CPI")).AdjustmentType, ShiftType.SCALED);

		RatesCurveGroupEntry entry0 = findEntry(defn, "USD-Disc");
		RatesCurveGroupEntry entry1 = findEntry(defn, "USD-3ML");
		RatesCurveGroupEntry entry2 = findEntry(defn, "USD-CPI");
		CurveDefinition defn0 = defn.findCurveDefinition(entry0.CurveName).get();
		CurveDefinition defn1 = defn.findCurveDefinition(entry1.CurveName).get();
		CurveDefinition defn2 = defn.findCurveDefinition(entry2.CurveName).get();

		assertEquals(entry0.DiscountCurrencies, ImmutableSet.of(Currency.USD));
		assertEquals(entry0.Indices, ImmutableSet.of());
		assertEquals(defn0.Name, CurveName.of("USD-Disc"));
		assertEquals(defn0.YValueType, ValueType.ZERO_RATE);
		assertEquals(defn0.ParameterCount, 17);

		assertEquals(entry1.DiscountCurrencies, ImmutableSet.of());
		assertEquals(entry1.Indices, ImmutableSet.of(IborIndices.USD_LIBOR_3M));
		assertEquals(defn1.Name, CurveName.of("USD-3ML"));
		assertEquals(defn1.YValueType, ValueType.ZERO_RATE);
		assertEquals(defn1.ParameterCount, 27);

		assertEquals(entry2.DiscountCurrencies, ImmutableSet.of());
		assertEquals(entry2.Indices, ImmutableSet.of(PriceIndices.US_CPI_U));
		assertEquals(defn2.Name, CurveName.of("USD-CPI"));
		assertEquals(defn2.YValueType, ValueType.PRICE_INDEX);
		assertEquals(defn2.ParameterCount, 2);
	  }

	  private RatesCurveGroupEntry findEntry(RatesCurveGroupDefinition defn, string curveName)
	  {
		return defn.Entries.Where(d => d.CurveName.Name.Equals(curveName)).First().get();
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(RatesCalibrationCsvLoader));
	  }

	}

}
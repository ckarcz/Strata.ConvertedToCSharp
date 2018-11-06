using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;

	/// <summary>
	/// Tests <seealso cref="CurveSensitivityUtils"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveSensitivityUtilsTest
	public class CurveSensitivityUtilsTest
	{

	  private static readonly CurveName NAME_1 = CurveName.of("CURVE 1");
	  private static readonly Currency CCY_1 = Currency.EUR;
	  private static readonly CurveName NAME_2 = CurveName.of("CURVE 2");
	  private static readonly Currency CCY_2 = Currency.USD;
	  private static readonly IList<LocalDate> TARGET_DATES = new List<LocalDate>();
	  static CurveSensitivityUtilsTest()
	  {
		TARGET_DATES.Add(LocalDate.of(2016, 8, 18));
		TARGET_DATES.Add(LocalDate.of(2020, 1, 5));
		TARGET_DATES.Add(LocalDate.of(2025, 12, 20));
		TARGET_DATES.Add(LocalDate.of(2045, 7, 4));
		SENSITIVITY_DATES.Add(LocalDate.of(2016, 8, 17));
		SENSITIVITY_DATES.Add(LocalDate.of(2016, 8, 18));
		SENSITIVITY_DATES.Add(LocalDate.of(2016, 8, 19));
		SENSITIVITY_DATES.Add(LocalDate.of(2019, 1, 5));
		SENSITIVITY_DATES.Add(LocalDate.of(2020, 1, 5));
		SENSITIVITY_DATES.Add(LocalDate.of(2021, 1, 5));
		SENSITIVITY_DATES.Add(LocalDate.of(2024, 12, 25));
		SENSITIVITY_DATES.Add(LocalDate.of(2025, 12, 20));
		SENSITIVITY_DATES.Add(LocalDate.of(2026, 12, 15));
		SENSITIVITY_DATES.Add(LocalDate.of(2045, 7, 4));
		SENSITIVITY_DATES.Add(LocalDate.of(2055, 7, 4));
	  }
	  private static readonly IList<LocalDate> SENSITIVITY_DATES = new List<LocalDate>();
	  private const double SENSITIVITY_AMOUNT = 123.45;
	  private static readonly double[] WEIGHTS_HC = new double[] {1.0, 1.0, 0.999190283, 0.295546559, 0.0, 0.831801471, 0.165441176, 1.0, 0.94955157, 0.0, 0.0};
	  // weights externally provided and hard-coded here
	  private static readonly int[] WEIGHTS_START = new int[] {0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2};
	  private const double TOLERANCE_SENSI = 1.0E-5;

	  public virtual void hard_coded_value_one_curve_one_date_dated()
	  {
		System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction = (d) => LabelDateParameterMetadata.of(d, "test");
		System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction = (s) => CurveSensitivityUtils.linearRebucketing(s, TARGET_DATES);
		test_from_functions_one_curve_one_date(parameterMetadataFunction, rebucketFunction);
	  }

	  public virtual void hard_coded_value_one_curve_one_date_tenor()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.time.LocalDate sensitivityDate = java.time.LocalDate.of(2015, 8, 18);
		LocalDate sensitivityDate = LocalDate.of(2015, 8, 18);
		System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction = (d) => TenorParameterMetadata.of(Tenor.of(Period.ofDays((int)(d.toEpochDay() - sensitivityDate.toEpochDay()))));
		System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction = (s) => CurveSensitivityUtils.linearRebucketing(s, TARGET_DATES, sensitivityDate);
		test_from_functions_one_curve_one_date(parameterMetadataFunction, rebucketFunction);
	  }

	  public virtual void hard_coded_value_one_curve_one_date_dated_sd()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.time.LocalDate sensitivityDate = java.time.LocalDate.of(2015, 8, 18);
		LocalDate sensitivityDate = LocalDate.of(2015, 8, 18);
		System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction = (d) => LabelDateParameterMetadata.of(d, "test");
		System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction = (s) => CurveSensitivityUtils.linearRebucketing(s, TARGET_DATES, sensitivityDate);
		test_from_functions_one_curve_one_date(parameterMetadataFunction, rebucketFunction);
	  }

	  private void test_from_functions_one_curve_one_date(System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction, System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction)
	  {
		for (int loopdate = 0; loopdate < SENSITIVITY_DATES.Count; loopdate++)
		{
		  IList<ParameterMetadata> pmdInput = new List<ParameterMetadata>();
		  pmdInput.Add(parameterMetadataFunction(SENSITIVITY_DATES[loopdate]));
		  CurrencyParameterSensitivity s = CurrencyParameterSensitivity.of(NAME_1, pmdInput, CCY_1, DoubleArray.of(SENSITIVITY_AMOUNT));
		  CurrencyParameterSensitivities s2 = CurrencyParameterSensitivities.of(s);
		  CurrencyParameterSensitivities sTarget = rebucketFunction(s2);
		  assertTrue(sTarget.Sensitivities.size() == 1);
		  CurrencyParameterSensitivity sTarget1 = sTarget.Sensitivities.get(0);
		  assertTrue(sTarget1.MarketDataName.Equals(NAME_1));
		  assertTrue(sTarget1.Currency.Equals(CCY_1));
		  assertTrue(sTarget1.Sensitivity.size() == TARGET_DATES.Count);
		  assertEquals(sTarget1.Sensitivity.get(WEIGHTS_START[loopdate]), WEIGHTS_HC[loopdate] * SENSITIVITY_AMOUNT, TOLERANCE_SENSI);
		  assertEquals(sTarget1.Sensitivity.get(WEIGHTS_START[loopdate] + 1), (1.0d - WEIGHTS_HC[loopdate]) * SENSITIVITY_AMOUNT, TOLERANCE_SENSI);
		}
	  }

	  public virtual void hard_coded_value_one_curve_all_dates()
	  {
		System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction = (d) => LabelDateParameterMetadata.of(d, "test");
		System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction = (s) => CurveSensitivityUtils.linearRebucketing(s, TARGET_DATES);
		test_from_functions_one_curve_all_dates(parameterMetadataFunction, rebucketFunction);
	  }

	  public virtual void hard_coded_value_one_curve_all_dates_tenor()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.time.LocalDate sensitivityDate = java.time.LocalDate.of(2015, 8, 18);
		LocalDate sensitivityDate = LocalDate.of(2015, 8, 18);
		System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction = (d) => TenorParameterMetadata.of(Tenor.of(Period.ofDays((int)(d.toEpochDay() - sensitivityDate.toEpochDay()))));
		System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction = (s) => CurveSensitivityUtils.linearRebucketing(s, TARGET_DATES, sensitivityDate);
		test_from_functions_one_curve_all_dates(parameterMetadataFunction, rebucketFunction);
	  }

	  public virtual void hard_coded_value_one_curve_all_dates_dated_sd()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.time.LocalDate sensitivityDate = java.time.LocalDate.of(2015, 8, 18);
		LocalDate sensitivityDate = LocalDate.of(2015, 8, 18);
		System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction = (d) => LabelDateParameterMetadata.of(d, "test");
		System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction = (s) => CurveSensitivityUtils.linearRebucketing(s, TARGET_DATES, sensitivityDate);
		test_from_functions_one_curve_all_dates(parameterMetadataFunction, rebucketFunction);
	  }

	  private void test_from_functions_one_curve_all_dates(System.Func<LocalDate, ParameterMetadata> parameterMetadataFunction, System.Func<CurrencyParameterSensitivities, CurrencyParameterSensitivities> rebucketFunction)
	  {
		IList<ParameterMetadata> pmdInput = new List<ParameterMetadata>();
		double[] sensiExpected = new double[TARGET_DATES.Count];
		for (int loopdate = 0; loopdate < SENSITIVITY_DATES.Count; loopdate++)
		{
		  pmdInput.Add(parameterMetadataFunction(SENSITIVITY_DATES[loopdate]));
		  sensiExpected[WEIGHTS_START[loopdate]] += WEIGHTS_HC[loopdate] * SENSITIVITY_AMOUNT;
		  sensiExpected[WEIGHTS_START[loopdate] + 1] += (1.0d - WEIGHTS_HC[loopdate]) * SENSITIVITY_AMOUNT;
		}
		DoubleArray sens = DoubleArray.of(SENSITIVITY_DATES.Count, (d) => SENSITIVITY_AMOUNT);
		CurrencyParameterSensitivity s = CurrencyParameterSensitivity.of(NAME_1, pmdInput, CCY_1, sens);
		CurrencyParameterSensitivities s2 = CurrencyParameterSensitivities.of(s);
		CurrencyParameterSensitivities sTarget = rebucketFunction(s2);
		assertTrue(sTarget.Sensitivities.size() == 1);
		CurrencyParameterSensitivity sTarget1 = sTarget.Sensitivities.get(0);
		assertTrue(sTarget1.MarketDataName.Equals(NAME_1));
		assertTrue(sTarget1.Currency.Equals(CCY_1));
		assertTrue(sTarget1.Sensitivity.size() == TARGET_DATES.Count);
		for (int looptarget = 0; looptarget < TARGET_DATES.Count; looptarget++)
		{
		  assertEquals(sTarget1.Sensitivity.get(looptarget), sensiExpected[looptarget], TOLERANCE_SENSI);
		}
	  }

	  public virtual void hard_coded_value_two_curves_one_date()
	  {
		for (int loopdate = 0; loopdate < SENSITIVITY_DATES.Count - 1; loopdate++)
		{
		  IList<ParameterMetadata> pmdInput1 = new List<ParameterMetadata>();
		  pmdInput1.Add(LabelDateParameterMetadata.of(SENSITIVITY_DATES[loopdate], "test"));
		  CurrencyParameterSensitivity s1 = CurrencyParameterSensitivity.of(NAME_1, pmdInput1, CCY_1, DoubleArray.of(SENSITIVITY_AMOUNT));
		  IList<ParameterMetadata> pmdInput2 = new List<ParameterMetadata>();
		  pmdInput2.Add(LabelDateParameterMetadata.of(SENSITIVITY_DATES[loopdate + 1], "test"));
		  CurrencyParameterSensitivity s2 = CurrencyParameterSensitivity.of(NAME_2, pmdInput2, CCY_2, DoubleArray.of(SENSITIVITY_AMOUNT));
		  CurrencyParameterSensitivities sList = CurrencyParameterSensitivities.of(s1, s2);
		  CurrencyParameterSensitivities sTarget = CurveSensitivityUtils.linearRebucketing(sList, TARGET_DATES);
		  assertTrue(sTarget.Sensitivities.size() == 2);
		  CurrencyParameterSensitivity sTarget1 = sTarget.Sensitivities.get(0);
		  assertTrue(sTarget1.MarketDataName.Equals(NAME_1));
		  assertTrue(sTarget1.Currency.Equals(CCY_1));
		  assertTrue(sTarget1.Sensitivity.size() == TARGET_DATES.Count);
		  assertEquals(sTarget1.Sensitivity.get(WEIGHTS_START[loopdate]), WEIGHTS_HC[loopdate] * SENSITIVITY_AMOUNT, TOLERANCE_SENSI);
		  assertEquals(sTarget1.Sensitivity.get(WEIGHTS_START[loopdate] + 1), (1.0d - WEIGHTS_HC[loopdate]) * SENSITIVITY_AMOUNT, TOLERANCE_SENSI);
		  CurrencyParameterSensitivity sTarget2 = sTarget.Sensitivities.get(1);
		  assertTrue(sTarget2.MarketDataName.Equals(NAME_2));
		  assertTrue(sTarget2.Currency.Equals(CCY_2));
		  assertTrue(sTarget2.Sensitivity.size() == TARGET_DATES.Count);
		  assertEquals(sTarget2.Sensitivity.get(WEIGHTS_START[loopdate + 1]), WEIGHTS_HC[loopdate + 1] * SENSITIVITY_AMOUNT, TOLERANCE_SENSI);
		  assertEquals(sTarget2.Sensitivity.get(WEIGHTS_START[loopdate + 1] + 1), (1.0d - WEIGHTS_HC[loopdate + 1]) * SENSITIVITY_AMOUNT, TOLERANCE_SENSI);
		}
	  }

	  public virtual void missing_metadata()
	  {
		CurrencyParameterSensitivity s1 = CurrencyParameterSensitivity.of(NAME_1, CCY_1, DoubleArray.of(SENSITIVITY_AMOUNT));
		CurrencyParameterSensitivities s2 = CurrencyParameterSensitivities.of(s1);
		assertThrowsIllegalArg(() => CurveSensitivityUtils.linearRebucketing(s2, TARGET_DATES));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.time.LocalDate sensitivityDate = java.time.LocalDate.of(2015, 8, 18);
		LocalDate sensitivityDate = LocalDate.of(2015, 8, 18);
		assertThrowsIllegalArg(() => CurveSensitivityUtils.linearRebucketing(s2, TARGET_DATES, sensitivityDate));
	  }

	  public virtual void wrong_metadata()
	  {
		IList<ParameterMetadata> pmdInput = new List<ParameterMetadata>();
		pmdInput.Add(TenorParameterMetadata.of(Tenor.TENOR_10M));
		CurrencyParameterSensitivity s1 = CurrencyParameterSensitivity.of(NAME_1, pmdInput, CCY_1, DoubleArray.of(SENSITIVITY_AMOUNT));
		CurrencyParameterSensitivities s2 = CurrencyParameterSensitivities.of(s1);
		assertThrowsIllegalArg(() => CurveSensitivityUtils.linearRebucketing(s2, TARGET_DATES));
	  }

	}

}
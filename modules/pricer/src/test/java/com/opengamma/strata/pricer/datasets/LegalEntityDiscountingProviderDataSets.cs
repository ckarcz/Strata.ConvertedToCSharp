/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.datasets
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ImmutableLegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.ImmutableLegalEntityDiscountingProvider;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;

	/// <summary>
	/// LegalEntityDiscountingProvider data sets for testing.
	/// </summary>
	public class LegalEntityDiscountingProviderDataSets
	{

	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LINEAR;

	  //  =====     issuer curve + repo curve in USD      =====     
	  private static readonly LocalDate VAL_DATE_USD = LocalDate.of(2011, 6, 20);
	  private static readonly LegalEntityId ISSUER_ID_USD = LegalEntityId.of("OG-Ticker", "GOVT1");
	  private static readonly CurveName NAME_REPO_USD = CurveName.of("TestRepoCurve");
	  private static readonly CurveName NAME_ISSUER_USD = CurveName.of("TestIssuerCurve");
	  /// <summary>
	  /// time data for repo rate curve </summary>
	  public static readonly DoubleArray REPO_TIME_USD = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  /// <summary>
	  /// zero rate data for repo rate curve </summary>
	  public static readonly DoubleArray REPO_RATE_USD = DoubleArray.of(0.0120, 0.0120, 0.0120, 0.0140, 0.0140, 0.0140);
	  /// <summary>
	  /// discount factor data for repo rate curve </summary>
	  public static readonly DoubleArray REPO_FACTOR_USD = DoubleArray.of(1.0, 0.9940, 0.9881, 0.9724, 0.9324, 0.8694);
	  /// <summary>
	  /// meta data of repo zero rate curve </summary>
	  public static readonly CurveMetadata META_ZERO_REPO_USD = Curves.zeroRates(NAME_REPO_USD, ACT_ACT_ISDA);
	  /// <summary>
	  /// meta data of repo discount factor curve </summary>
	  public static readonly CurveMetadata META_SIMPLE_REPO_USD = Curves.discountFactors(NAME_REPO_USD, ACT_ACT_ISDA);
	  /// <summary>
	  /// time data for issuer curve </summary>
	  public static readonly DoubleArray ISSUER_TIME_USD = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  /// <summary>
	  /// zero rate data for issuer curve </summary>
	  public static readonly DoubleArray ISSUER_RATE_USD = DoubleArray.of(0.0100, 0.0100, 0.0100, 0.0120, 0.0120, 0.0120);
	  /// <summary>
	  /// discount factor data for issuer curve </summary>
	  public static readonly DoubleArray ISSUER_FACTOR_USD = DoubleArray.of(1.0, 0.9950, 0.9900, 0.9763, 0.9418, 0.8869);
	  /// <summary>
	  /// meta data of issuer zero rate curve </summary>
	  public static readonly CurveMetadata META_ZERO_ISSUER_USD = Curves.zeroRates(NAME_ISSUER_USD, ACT_ACT_ISDA);
	  /// <summary>
	  /// meta data of issuer discount factor curve </summary>
	  public static readonly CurveMetadata META_SIMPLE_ISSUER_USD = Curves.discountFactors(NAME_ISSUER_USD, ACT_ACT_ISDA);
	  private static readonly RepoGroup GROUP_REPO_USD = RepoGroup.of("GOVT1 BONDS");
	  private static readonly LegalEntityGroup GROUP_ISSUER_USD = LegalEntityGroup.of("GOVT1");
	  // zero rate curves
	  private static readonly InterpolatedNodalCurve CURVE_ZERO_REPO_USD = InterpolatedNodalCurve.of(META_ZERO_REPO_USD, REPO_TIME_USD, REPO_RATE_USD, INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_ZERO_REPO_USD = ZeroRateDiscountFactors.of(USD, VAL_DATE_USD, CURVE_ZERO_REPO_USD);
	  private static readonly InterpolatedNodalCurve CURVE_ZERO_ISSUER_USD = InterpolatedNodalCurve.of(META_ZERO_ISSUER_USD, ISSUER_TIME_USD, ISSUER_RATE_USD, INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_ZERO_ISSUER_USD = ZeroRateDiscountFactors.of(USD, VAL_DATE_USD, CURVE_ZERO_ISSUER_USD);
	  // discount factor curves
	  private static readonly InterpolatedNodalCurve CURVE_SIMPLE_REPO = InterpolatedNodalCurve.of(META_SIMPLE_REPO_USD, REPO_TIME_USD, REPO_FACTOR_USD, INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_SIMPLE_REPO = SimpleDiscountFactors.of(USD, VAL_DATE_USD, CURVE_SIMPLE_REPO);
	  private static readonly InterpolatedNodalCurve CURVE_SIMPLE_ISSUER_USD = InterpolatedNodalCurve.of(META_SIMPLE_ISSUER_USD, ISSUER_TIME_USD, ISSUER_FACTOR_USD, INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_SIMPLE_ISSUER_USD = SimpleDiscountFactors.of(USD, VAL_DATE_USD, CURVE_SIMPLE_ISSUER_USD);

	  //  =====     issuer curve + repo curve in EUR      =====     
	  private static readonly LocalDate VAL_DATE_EUR = LocalDate.of(2014, 3, 31);
	  private static readonly LegalEntityId ISSUER_ID_EUR = LegalEntityId.of("OG-Ticker", "GOVT2");
	  private static readonly CurveName NAME_REPO_EUR = CurveName.of("TestRepoCurve2");
	  private static readonly CurveName NAME_ISSUER_EUR = CurveName.of("TestIssuerCurve2");
	  /// <summary>
	  /// time data for repo rate curve </summary>
	  public static readonly DoubleArray REPO_TIME_EUR = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  /// <summary>
	  /// zero rate data for repo rate curve </summary>
	  public static readonly DoubleArray REPO_RATE_EUR = DoubleArray.of(0.0150, 0.0125, 0.0150, 0.0175, 0.0150, 0.0150);
	  /// <summary>
	  /// meta data of repo zero rate curve </summary>
	  public static readonly CurveMetadata META_ZERO_REPO_EUR = Curves.zeroRates(NAME_REPO_EUR, ACT_ACT_ISDA);
	  /// <summary>
	  /// meta data of repo discount factor curve </summary>
	  public static readonly CurveMetadata META_SIMPLE_REPO_EUR = Curves.discountFactors(NAME_REPO_EUR, ACT_ACT_ISDA);
	  /// <summary>
	  /// time data for issuer curve </summary>
	  public static readonly DoubleArray ISSUER_TIME_EUR = DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 10.0);
	  /// <summary>
	  /// zero rate data for issuer curve </summary>
	  public static readonly DoubleArray ISSUER_RATE_EUR = DoubleArray.of(0.0250, 0.0225, 0.0250, 0.0275, 0.0250, 0.0250);
	  /// <summary>
	  /// meta data of issuer zero rate curve </summary>
	  public static readonly CurveMetadata META_ZERO_ISSUER_EUR = Curves.zeroRates(NAME_ISSUER_EUR, ACT_ACT_ISDA);
	  /// <summary>
	  /// meta data of issuer discount factor curve </summary>
	  public static readonly CurveMetadata META_SIMPLE_ISSUER_EUR = Curves.discountFactors(NAME_ISSUER_EUR, ACT_ACT_ISDA);
	  private static readonly RepoGroup GROUP_REPO_EUR = RepoGroup.of("GOVT2 BONDS");
	  private static readonly LegalEntityGroup GROUP_ISSUER_EUR = LegalEntityGroup.of("GOVT2");
	  // zero rate curves
	  private static readonly InterpolatedNodalCurve CURVE_ZERO_REPO_EUR = InterpolatedNodalCurve.of(META_ZERO_REPO_EUR, REPO_TIME_EUR, REPO_RATE_EUR, INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_ZERO_REPO_EUR = ZeroRateDiscountFactors.of(EUR, VAL_DATE_EUR, CURVE_ZERO_REPO_EUR);
	  private static readonly InterpolatedNodalCurve CURVE_ZERO_ISSUER_EUR = InterpolatedNodalCurve.of(META_ZERO_ISSUER_EUR, ISSUER_TIME_EUR, ISSUER_RATE_EUR, INTERPOLATOR);
	  private static readonly DiscountFactors DSC_FACTORS_ZERO_ISSUER_EUR = ZeroRateDiscountFactors.of(EUR, VAL_DATE_EUR, CURVE_ZERO_ISSUER_EUR);

	  /// <summary>
	  /// provider with zero rate curves, USD </summary>
	  public static readonly LegalEntityDiscountingProvider ISSUER_REPO_ZERO = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER_USD, USD), DSC_FACTORS_ZERO_ISSUER_USD)).issuerCurveGroups(ImmutableMap.of(ISSUER_ID_USD, GROUP_ISSUER_USD)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_USD, USD), DSC_FACTORS_ZERO_REPO_USD)).repoCurveGroups(ImmutableMap.of(ISSUER_ID_USD, GROUP_REPO_USD)).build();
	  /// <summary>
	  /// provider with zero rate curves, EUR </summary>
	  public static readonly LegalEntityDiscountingProvider ISSUER_REPO_ZERO_EUR = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER_EUR, EUR), DSC_FACTORS_ZERO_ISSUER_EUR)).issuerCurveGroups(ImmutableMap.of(ISSUER_ID_EUR, GROUP_ISSUER_EUR)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_EUR, EUR), DSC_FACTORS_ZERO_REPO_EUR)).repoCurveGroups(ImmutableMap.of(ISSUER_ID_EUR, GROUP_REPO_EUR)).build();
	  /// <summary>
	  /// provider with discount factor curve, USD </summary>
	  public static readonly LegalEntityDiscountingProvider ISSUER_REPO_SIMPLE = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER_USD, USD), DSC_FACTORS_SIMPLE_ISSUER_USD)).issuerCurveGroups(ImmutableMap.of(ISSUER_ID_USD, GROUP_ISSUER_USD)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_USD, USD), DSC_FACTORS_SIMPLE_REPO)).repoCurveGroups(ImmutableMap.of(ISSUER_ID_USD, GROUP_REPO_USD)).build();
	}

}
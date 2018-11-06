using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.datasets
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ConstantRecoveryRates = com.opengamma.strata.pricer.credit.ConstantRecoveryRates;
	using ImmutableCreditRatesProvider = com.opengamma.strata.pricer.credit.ImmutableCreditRatesProvider;
	using IsdaCreditDiscountFactors = com.opengamma.strata.pricer.credit.IsdaCreditDiscountFactors;
	using LegalEntitySurvivalProbabilities = com.opengamma.strata.pricer.credit.LegalEntitySurvivalProbabilities;

	/// <summary>
	/// {@code CreditRatesProvider} data sets for testing.
	/// </summary>
	public class CreditRatesProviderDataSets
	{

	  private const double RECOVERY_RATE_US = 0.25;
	  private const double RECOVERY_RATE_JP = 0.35;
	  private static readonly StandardId LEGAL_ENTITY_US = StandardId.of("OG", "ABC");
	  private static readonly StandardId LEGAL_ENTITY_JP = StandardId.of("OG", "DEF");
	  // discount curves
	  private static readonly DoubleArray TIME_YC_USD = DoubleArray.ofUnsafe(new double[] {0.09041095890410959, 0.16712328767123288, 0.2547945205479452, 0.5041095890410959, 0.7534246575342466, 1.0054794520547945, 2.0054794520547947, 3.008219178082192, 4.013698630136987, 5.010958904109589, 6.008219178082192, 7.010958904109589, 8.01095890410959, 9.01095890410959, 10.016438356164384, 12.013698630136986, 15.021917808219179, 20.01917808219178, 30.024657534246575});
	  private static readonly DoubleArray RATE_YC_USD = DoubleArray.ofUnsafe(new double[] {-0.002078655697855299, -0.001686438401304855, -0.0013445486228483379, -4.237819925898475E-4, 2.5142499469348057E-5, 5.935063895780138E-4, -3.247081037469503E-4, 6.147182786549223E-4, 0.0019060597240545122, 0.0033125742254568815, 0.0047766352312329455, 0.0062374324537341225, 0.007639664176639106, 0.008971003650150983, 0.010167545380711455, 0.012196853322376243, 0.01441082634734099, 0.016236611610989507, 0.01652439910865982});
	  private static readonly DefaultCurveMetadata METADATA_YC_USD = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("yieldUsd").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_YC_USD = InterpolatedNodalCurve.of(METADATA_YC_USD, TIME_YC_USD, RATE_YC_USD, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private static readonly DoubleArray TIME_YC_JPY = DoubleArray.ofUnsafe(new double[] {0.09041095890410959, 0.1726027397260274, 0.26301369863013696, 0.5041095890410959, 1.010958904109589, 2.008219178082192, 3.008219178082192, 4.008219178082192, 5.008219178082192, 6.008219178082192, 7.013698630136987, 8.01095890410959, 9.01095890410959, 10.01095890410959, 12.01917808219178, 15.016438356164384, 20.01917808219178, 30.027397260273972});
	  private static readonly DoubleArray RATE_YC_JPY = DoubleArray.ofUnsafe(new double[] {0.001122321508200541, 0.001291089999069537, 0.0014417799977308034, 0.0021281253625937105, 0.003974208890510453, 0.0021575032806582083, 0.002518780098670605, 0.0030701822006373924, 0.003761981561690137, 0.004581475125390347, 0.0054671808559551205, 0.006410615451054819, 0.007348653868573937, 0.008306763274584402, 0.01027398170777581, 0.013161163859389004, 0.01662634115023716, 0.018953926358174045});
	  private static readonly DefaultCurveMetadata METADATA_YC_JPY = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("yieldJpy").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_YC_JPY = InterpolatedNodalCurve.of(METADATA_YC_JPY, TIME_YC_JPY, RATE_YC_JPY, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  // credit curves
	  private static readonly DoubleArray TIME_CC_US = DoubleArray.ofUnsafe(new double[] {1.2054794520547945, 1.7095890410958905, 2.712328767123288, 3.712328767123288, 4.712328767123288, 5.712328767123288, 7.715068493150685, 10.717808219178082});
	  private static readonly DoubleArray RATE_CC_US = DoubleArray.ofUnsafe(new double[] {0.009950492020354761, 0.01203385973637765, 0.01418821591480718, 0.01684815168721049, 0.01974873350586718, 0.023084203422383043, 0.02696911931489543, 0.029605642651816415});
	  private static readonly DefaultCurveMetadata METADATA_CC_US = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("creditUs").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_CC_US = InterpolatedNodalCurve.of(METADATA_CC_US, TIME_CC_US, RATE_CC_US, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);
	  private static readonly DoubleArray TIME_CC_JP = DoubleArray.ofUnsafe(new double[] {0.5205479452054794, 1.021917808219178, 3.021917808219178, 5.024657534246575, 7.024657534246575, 10.027397260273972});
	  private static readonly DoubleArray RATE_CC_JP = DoubleArray.ofUnsafe(new double[] {0.01336177891566421, 0.013358446703154937, 0.020778114792013535, 0.029425047945587392, 0.033805034897276036, 0.03693847325607831});
	  private static readonly DefaultCurveMetadata METADATA_CC_JP = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName("creditJp").dayCount(ACT_365F).build();
	  private static readonly InterpolatedNodalCurve NODAL_CC_JP = InterpolatedNodalCurve.of(METADATA_CC_JP, TIME_CC_JP, RATE_CC_JP, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates credit rates provider with valuation date specified.
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the rates provider </returns>
	  public static ImmutableCreditRatesProvider createCreditRatesProvider(LocalDate valuationDate)
	  {

		IsdaCreditDiscountFactors ycUsd = IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_YC_USD);
		IsdaCreditDiscountFactors ycJpy = IsdaCreditDiscountFactors.of(JPY, valuationDate, NODAL_YC_JPY);
		IsdaCreditDiscountFactors ccUs = IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_CC_US);
		IsdaCreditDiscountFactors ccJp = IsdaCreditDiscountFactors.of(JPY, valuationDate, NODAL_CC_JP);
		ConstantRecoveryRates rrUs = ConstantRecoveryRates.of(LEGAL_ENTITY_US, valuationDate, RECOVERY_RATE_US);
		ConstantRecoveryRates rrJp = ConstantRecoveryRates.of(LEGAL_ENTITY_JP, valuationDate, RECOVERY_RATE_JP);
		return ImmutableCreditRatesProvider.builder().valuationDate(valuationDate).creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY_US, USD), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_US, ccUs), Pair.of(LEGAL_ENTITY_JP, JPY), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY_JP, ccJp))).discountCurves(ImmutableMap.of(USD, ycUsd, JPY, ycJpy)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY_US, rrUs, LEGAL_ENTITY_JP, rrJp)).build();
	  }

	  /// <summary>
	  /// Gets all the discount factors
	  /// </summary>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <returns> the discount factors </returns>
	  public static IList<IsdaCreditDiscountFactors> getAllDiscountFactors(LocalDate valuationDate)
	  {

		IsdaCreditDiscountFactors ycUsd = IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_YC_USD);
		IsdaCreditDiscountFactors ycJpy = IsdaCreditDiscountFactors.of(JPY, valuationDate, NODAL_YC_JPY);
		IsdaCreditDiscountFactors ccUs = IsdaCreditDiscountFactors.of(USD, valuationDate, NODAL_CC_US);
		IsdaCreditDiscountFactors ccJp = IsdaCreditDiscountFactors.of(JPY, valuationDate, NODAL_CC_JP);
		return ImmutableList.of(ycUsd, ycJpy, ccUs, ccJp);
	  }

	}

}
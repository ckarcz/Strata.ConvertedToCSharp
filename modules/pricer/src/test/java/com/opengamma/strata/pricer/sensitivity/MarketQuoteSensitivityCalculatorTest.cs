/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.CurveInfoType.JACOBIAN;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using ImmutableLegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.ImmutableLegalEntityDiscountingProvider;
	using CalibrationDiscountingSimpleEur3Test = com.opengamma.strata.pricer.curve.CalibrationDiscountingSimpleEur3Test;
	using LegalEntityId = com.opengamma.strata.product.LegalEntityId;
	using SecurityId = com.opengamma.strata.product.SecurityId;

	/// <summary>
	/// Test <seealso cref="MarketQuoteSensitivityCalculator"/>.
	/// <para>
	/// Market quote sensitivity calculations with {@code RatesProvider}, {@code CreditRatesProvider} are tested in other unit tests, 
	/// e.g., <seealso cref="CalibrationDiscountingSimpleEur3Test"/>, {@code SpreadSensitivityCalculatorTest}, together with curve calibrations.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MarketQuoteSensitivityCalculatorTest
	public class MarketQuoteSensitivityCalculatorTest
	{

	  private static readonly LocalDate DATE = date(2017, 12, 11);
	  private static readonly SecurityId ID_SECURITY = SecurityId.of("OG-Ticker", "Bond-5Y");
	  private static readonly RepoGroup GROUP_REPO_SECURITY = RepoGroup.of("ISSUER1 BND 5Y");
	  private static readonly LegalEntityGroup GROUP_ISSUER = LegalEntityGroup.of("ISSUER1");
	  private static readonly LegalEntityId ID_ISSUER = LegalEntityId.of("OG-Ticker", "Issuer-1");
	  private static readonly MarketQuoteSensitivityCalculator CALC = MarketQuoteSensitivityCalculator.DEFAULT;
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new OGMatrixAlgebra();
	  // curve data
	  private static readonly CurveName CURVE_NAME_1 = CurveName.of("market1");
	  private static readonly DoubleArray SENSI_1 = DoubleArray.of(1d, 2d, 3d);
	  private static readonly DoubleArray X_VALUES_1 = DoubleArray.of(0.5d, 1d, 3d);
	  private static readonly DoubleArray Y_VALUES_1 = DoubleArray.of(0.05d, 0.04d, 0.03d);
	  private static readonly CurveParameterSize SIZE_1 = CurveParameterSize.of(CURVE_NAME_1, X_VALUES_1.size());
	  private static readonly CurveName CURVE_NAME_2 = CurveName.of("market2");
	  private static readonly DoubleArray SENSI_2 = DoubleArray.of(-3d, -6d, 4d, 2d);
	  private static readonly DoubleArray X_VALUES_2 = DoubleArray.of(1d, 5d, 7d, 10d);
	  private static readonly DoubleArray Y_VALUES_2 = DoubleArray.of(0.1d, 0.05d, -0.08d, -0.01d);
	  private static readonly CurveParameterSize SIZE_2 = CurveParameterSize.of(CURVE_NAME_2, X_VALUES_2.size());
	  // interpolated curves
	  private static readonly double[][] MATRIX_1 = new double[][]
	  {
		  new double[] {1.5d, 0d, 0d, 0d, 0d, 0d, 0d},
		  new double[] {0d, 1.2d, 0d, 0d, 0d, 0d, 0d},
		  new double[] {0d, 0d, 1.3d, 0d, 0d, 0d, 0d}
	  };
	  private static readonly double[][] MATRIX_11 = new double[][]
	  {
		  new double[] {1.5d, 0d, 0d},
		  new double[] {0d, 1.2d, 0d},
		  new double[] {0d, 0d, 1.3d}
	  };
	  private static readonly double[][] MATRIX_12 = new double[][]
	  {
		  new double[] {0d, 0d, 0d, 0d},
		  new double[] {0d, 0d, 0d, 0d},
		  new double[] {0d, 0d, 0d, 0d}
	  };
	  private static readonly JacobianCalibrationMatrix JACONIAN_MATRIX_1 = JacobianCalibrationMatrix.of(ImmutableList.of(SIZE_1, SIZE_2), DoubleMatrix.copyOf(MATRIX_1));
	  private static readonly CurveMetadata METADATA_1 = Curves.zeroRates(CURVE_NAME_1, ACT_365F).withInfo(JACOBIAN, JACONIAN_MATRIX_1);
	  private static readonly InterpolatedNodalCurve CURVE_1 = InterpolatedNodalCurve.of(METADATA_1, X_VALUES_1, Y_VALUES_1, LINEAR);
	  private static readonly double[][] MATRIX_2 = new double[][]
	  {
		  new double[] {1.5d, 0.5d, 0.1d, 2d, 0d, 0d, 0d},
		  new double[] {0.2d, 1.2d, 0.9d, 0d, 1.5d, 0d, 0d},
		  new double[] {0.1d, 0.5d, 1.0d, 0d, 0d, 1.1d, 0d},
		  new double[] {0d, 0.2d, 1.2d, 0d, 0d, 0d, 1.1d}
	  };
	  private static readonly double[][] MATRIX_21 = new double[][]
	  {
		  new double[] {1.5d, 0.5d, 0.1d},
		  new double[] {0.2d, 1.2d, 0.9d},
		  new double[] {0.1d, 0.5d, 1.0d},
		  new double[] {0d, 0.2d, 1.2d}
	  };
	  private static readonly double[][] MATRIX_22 = new double[][]
	  {
		  new double[] {2d, 0d, 0d, 0d},
		  new double[] {0d, 1.5d, 0d, 0d},
		  new double[] {0d, 0d, 1.1d, 0d},
		  new double[] {0d, 0d, 0d, 1.1d}
	  };
	  private static readonly JacobianCalibrationMatrix JACONIAN_MATRIX_2 = JacobianCalibrationMatrix.of(ImmutableList.of(SIZE_1, SIZE_2), DoubleMatrix.copyOf(MATRIX_2));
	  private static readonly CurveMetadata METADATA_2 = Curves.zeroRates(CURVE_NAME_2, ACT_365F).withInfo(JACOBIAN, JACONIAN_MATRIX_2);
	  private static readonly InterpolatedNodalCurve CURVE_2 = InterpolatedNodalCurve.of(METADATA_2, X_VALUES_2, Y_VALUES_2, LINEAR);
	  // sensitivities and provider
	  private static readonly CurrencyParameterSensitivities PARAMETER_SENSITIVITIES;
	  private static readonly ImmutableLegalEntityDiscountingProvider PROVIDER;
	  static MarketQuoteSensitivityCalculatorTest()
	  {
		CurrencyParameterSensitivity sensi1 = CurrencyParameterSensitivity.of(CURVE_NAME_1, USD, SENSI_1);
		CurrencyParameterSensitivity sensi2 = CurrencyParameterSensitivity.of(CURVE_NAME_2, GBP, SENSI_2);
		ZeroRateDiscountFactors dscIssuer = ZeroRateDiscountFactors.of(USD, DATE, CURVE_1);
		ZeroRateDiscountFactors dscRepo = ZeroRateDiscountFactors.of(GBP, DATE, CURVE_2);
		PARAMETER_SENSITIVITIES = CurrencyParameterSensitivities.of(sensi1, sensi2);
		PROVIDER = ImmutableLegalEntityDiscountingProvider.builder().issuerCurves(ImmutableMap.of(Pair.of(GROUP_ISSUER, USD), dscIssuer)).issuerCurveGroups(ImmutableMap.of(ID_ISSUER, GROUP_ISSUER)).repoCurves(ImmutableMap.of(Pair.of(GROUP_REPO_SECURITY, GBP), dscRepo)).repoCurveSecurityGroups(ImmutableMap.of(ID_SECURITY, GROUP_REPO_SECURITY)).build();
	  }

	  private const double TOL = 1.0e-14;

	  public virtual void test_sensitivity_LegalEntityDiscountingProvider()
	  {
		CurrencyParameterSensitivities computed = CALC.sensitivity(PARAMETER_SENSITIVITIES, PROVIDER);
		assertEquals(computed.Sensitivities.size(), 4);
		DoubleArray expected11 = (DoubleArray) MATRIX_ALGEBRA.multiply(SENSI_1, DoubleMatrix.copyOf(MATRIX_11));
		DoubleArray expected12 = (DoubleArray) MATRIX_ALGEBRA.multiply(SENSI_1, DoubleMatrix.copyOf(MATRIX_12));
		DoubleArray expected21 = (DoubleArray) MATRIX_ALGEBRA.multiply(SENSI_2, DoubleMatrix.copyOf(MATRIX_21));
		DoubleArray expected22 = (DoubleArray) MATRIX_ALGEBRA.multiply(SENSI_2, DoubleMatrix.copyOf(MATRIX_22));
		assertTrue(computed.getSensitivity(CURVE_NAME_1, USD).Sensitivity.equalWithTolerance(expected11, TOL));
		assertTrue(computed.getSensitivity(CURVE_NAME_1, GBP).Sensitivity.equalWithTolerance(expected21, TOL));
		assertTrue(computed.getSensitivity(CURVE_NAME_2, USD).Sensitivity.equalWithTolerance(expected12, TOL));
		assertTrue(computed.getSensitivity(CURVE_NAME_2, GBP).Sensitivity.equalWithTolerance(expected22, TOL));
	  }

	}

}
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;

	/// <summary>
	/// Test <seealso cref="CrossGammaParameterSensitivities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CrossGammaParameterSensitivitiesTest
	public class CrossGammaParameterSensitivitiesTest
	{

	  private const double FACTOR1 = 3.14;
	  private static readonly DoubleMatrix MATRIX_USD1 = DoubleMatrix.of(2, 2, 100, 200, 300, 123);
	  private static readonly DoubleMatrix MATRIX_USD2 = DoubleMatrix.of(2, 2, 1000, 250, 321, 123);
	  private static readonly DoubleMatrix MATRIX_USD2_IN_EUR = DoubleMatrix.of(2, 2, 1000 / 1.6, 250 / 1.6, 321 / 1.6, 123 / 1.6);
	  private static readonly DoubleMatrix MATRIX_USD12 = DoubleMatrix.of(2, 4, 100, 200, 1000, 250, 300, 123, 321, 123);
	  private static readonly DoubleMatrix MATRIX_USD21 = DoubleMatrix.of(2, 4, 1000, 250, -500, -400, 321, 123, -200, -300);
	  private static readonly DoubleMatrix MATRIX_ZERO = DoubleMatrix.of(2, 2, 0, 0, 0, 0);
	  private static readonly DoubleMatrix TOTAL_USD = DoubleMatrix.of(2, 2, 1100, 450, 621, 246);
	  private static readonly DoubleMatrix MATRIX_EUR1 = DoubleMatrix.of(2, 2, 1000, 250, 321, 123);
	  private static readonly DoubleMatrix MATRIX_EUR1_IN_USD = DoubleMatrix.of(2, 2, 1000 * 1.6, 250 * 1.6, 321 * 1.6, 123 * 1.6);
	  private static readonly Currency USD = Currency.USD;
	  private static readonly Currency EUR = Currency.EUR;
	  private static readonly FxRate FX_RATE = FxRate.of(EUR, USD, 1.6d);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME0 = com.opengamma.strata.market.curve.CurveName.of("NAME-0");
	  private static readonly MarketDataName<object> NAME0 = CurveName.of("NAME-0");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME1 = com.opengamma.strata.market.curve.CurveName.of("NAME-1");
	  private static readonly MarketDataName<object> NAME1 = CurveName.of("NAME-1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME2 = com.opengamma.strata.market.curve.CurveName.of("NAME-2");
	  private static readonly MarketDataName<object> NAME2 = CurveName.of("NAME-2");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME3 = com.opengamma.strata.market.curve.CurveName.of("NAME-3");
	  private static readonly MarketDataName<object> NAME3 = CurveName.of("NAME-3");
	  private static readonly IList<ParameterMetadata> METADATA0 = ParameterMetadata.listOfEmpty(2);
	  private static readonly IList<ParameterMetadata> METADATA1 = ParameterMetadata.listOfEmpty(2);
	  private static readonly IList<ParameterMetadata> METADATA2 = ParameterMetadata.listOfEmpty(2);
	  private static readonly IList<ParameterMetadata> METADATA3 = ParameterMetadata.listOfEmpty(2);

	  private static readonly CrossGammaParameterSensitivity ENTRY_USD = CrossGammaParameterSensitivity.of(NAME1, METADATA1, USD, MATRIX_USD1);
	  private static readonly CrossGammaParameterSensitivity ENTRY_USD2 = CrossGammaParameterSensitivity.of(NAME1, METADATA1, USD, MATRIX_USD2);
	  private static readonly CrossGammaParameterSensitivity ENTRY_USD_TOTAL = CrossGammaParameterSensitivity.of(NAME1, METADATA1, USD, TOTAL_USD);
	  private static readonly CrossGammaParameterSensitivity ENTRY_USD2_IN_EUR = CrossGammaParameterSensitivity.of(NAME1, METADATA1, EUR, MATRIX_USD2_IN_EUR);
	  private static readonly CrossGammaParameterSensitivity ENTRY_EUR = CrossGammaParameterSensitivity.of(NAME2, METADATA2, EUR, MATRIX_EUR1);
	  private static readonly CrossGammaParameterSensitivity ENTRY_EUR_IN_USD = CrossGammaParameterSensitivity.of(NAME2, METADATA2, USD, MATRIX_EUR1_IN_USD);
	  private static readonly CrossGammaParameterSensitivity ENTRY_ZERO0 = CrossGammaParameterSensitivity.of(NAME0, METADATA0, USD, MATRIX_ZERO);
	  private static readonly CrossGammaParameterSensitivity ENTRY_ZERO3 = CrossGammaParameterSensitivity.of(NAME3, METADATA3, USD, MATRIX_ZERO);
	  private static readonly CrossGammaParameterSensitivity ENTRY_USD12 = CrossGammaParameterSensitivity.of(NAME1, METADATA1, ImmutableList.of(Pair.of(NAME1, METADATA1), Pair.of(NAME2, METADATA2)), USD, MATRIX_USD12);
	  private static readonly CrossGammaParameterSensitivity ENTRY_USD21 = CrossGammaParameterSensitivity.of(NAME2, METADATA2, ImmutableList.of(Pair.of(NAME1, METADATA1), Pair.of(NAME2, METADATA2)), USD, MATRIX_USD21);

	  private static readonly CrossGammaParameterSensitivities SENSI_1 = CrossGammaParameterSensitivities.of(ENTRY_USD);
	  private static readonly CrossGammaParameterSensitivities SENSI_2 = CrossGammaParameterSensitivities.of(ImmutableList.of(ENTRY_USD2, ENTRY_EUR));
	  private static readonly CrossGammaParameterSensitivities SENSI_3 = CrossGammaParameterSensitivities.of(ImmutableList.of(ENTRY_USD12, ENTRY_USD21));

	  private const double TOLERENCE_CMP = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_empty()
	  {
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.empty();
		assertEquals(test.size(), 0);
		assertEquals(test.Sensitivities.size(), 0);
	  }

	  public virtual void test_of_single()
	  {
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of(ENTRY_USD);
		assertEquals(test.size(), 1);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD));
	  }

	  public virtual void test_of_array_none()
	  {
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of();
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_of_list_none()
	  {
		ImmutableList<CrossGammaParameterSensitivity> list = ImmutableList.of();
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of(list);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_of_list_notNormalized()
	  {
		ImmutableList<CrossGammaParameterSensitivity> list = ImmutableList.of(ENTRY_USD, ENTRY_EUR);
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of(list);
		assertEquals(test.size(), 2);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD, ENTRY_EUR));
	  }

	  public virtual void test_of_list_normalized()
	  {
		ImmutableList<CrossGammaParameterSensitivity> list = ImmutableList.of(ENTRY_USD, ENTRY_USD2);
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of(list);
		assertEquals(test.size(), 1);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD_TOTAL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getSensitivity()
	  {
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of(ENTRY_USD);
		assertEquals(test.getSensitivity(NAME1, USD), ENTRY_USD);
		assertThrowsIllegalArg(() => test.getSensitivity(NAME1, EUR));
		assertThrowsIllegalArg(() => test.getSensitivity(NAME0, USD));
		assertThrowsIllegalArg(() => test.getSensitivity(NAME0, EUR));
	  }

	  public virtual void test_findSensitivity()
	  {
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.of(ENTRY_USD);
		assertEquals(test.findSensitivity(NAME1, USD), ENTRY_USD);
		assertEquals(test.findSensitivity(NAME1, EUR), null);
		assertEquals(test.findSensitivity(NAME0, USD), null);
		assertEquals(test.findSensitivity(NAME0, EUR), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_one_notNormalized()
	  {
		CrossGammaParameterSensitivities test = SENSI_1.combinedWith(ENTRY_EUR);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD, ENTRY_EUR));
	  }

	  public virtual void test_combinedWith_one_normalized()
	  {
		CrossGammaParameterSensitivities test = SENSI_1.combinedWith(ENTRY_USD2);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD_TOTAL));
	  }

	  public virtual void test_combinedWith_other()
	  {
		CrossGammaParameterSensitivities test = SENSI_1.combinedWith(SENSI_2);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD_TOTAL, ENTRY_EUR));
	  }

	  public virtual void test_combinedWith_otherEmpty()
	  {
		CrossGammaParameterSensitivities test = SENSI_1.combinedWith(CrossGammaParameterSensitivities.empty());
		assertEquals(test, SENSI_1);
	  }

	  public virtual void test_combinedWith_empty()
	  {
		CrossGammaParameterSensitivities test = CrossGammaParameterSensitivities.empty().combinedWith(SENSI_1);
		assertEquals(test, SENSI_1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo_singleCurrency()
	  {
		CrossGammaParameterSensitivities test = SENSI_1.convertedTo(USD, FxMatrix.empty());
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD));
	  }

	  public virtual void test_convertedTo_multipleCurrency()
	  {
		CrossGammaParameterSensitivities test = SENSI_2.convertedTo(USD, FX_RATE);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD2, ENTRY_EUR_IN_USD));
	  }

	  public virtual void test_convertedTo_multipleCurrency_mergeWhenSameName()
	  {
		CrossGammaParameterSensitivities test = SENSI_1.combinedWith(ENTRY_USD2_IN_EUR).convertedTo(USD, FX_RATE);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_USD_TOTAL));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_total_singleCurrency()
	  {
		assertEquals(SENSI_1.total(USD, FxMatrix.empty()).Amount, MATRIX_USD1.total(), 1e-8);
	  }

	  public virtual void test_total_multipleCurrency()
	  {
		assertEquals(SENSI_2.total(USD, FX_RATE).Amount, MATRIX_USD2.total() + MATRIX_EUR1.total() * 1.6d, 1e-8);
	  }

	  public virtual void test_totalMulti_singleCurrency()
	  {
		assertEquals(SENSI_1.total().size(), 1);
		assertEquals(SENSI_1.total().getAmount(USD).Amount, MATRIX_USD1.total(), 1e-8);
	  }

	  public virtual void test_totalMulti_multipleCurrency()
	  {
		assertEquals(SENSI_2.total().size(), 2);
		assertEquals(SENSI_2.total().getAmount(USD).Amount, MATRIX_USD2.total(), 1e-8);
		assertEquals(SENSI_2.total().getAmount(EUR).Amount, MATRIX_EUR1.total(), 1e-8);
	  }

	  public virtual void test_diagonal()
	  {
		assertEquals(SENSI_2.diagonal().size(), 2);
		assertEquals(SENSI_2.diagonal().getSensitivity(NAME1, USD), ENTRY_USD2.diagonal());
		assertEquals(SENSI_2.diagonal().getSensitivity(NAME2, EUR), ENTRY_EUR.diagonal());
		assertEquals(SENSI_3.diagonal().getSensitivity(NAME1, USD), ENTRY_USD12.diagonal());
		assertEquals(SENSI_3.diagonal().getSensitivity(NAME2, USD), ENTRY_USD21.diagonal());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		CrossGammaParameterSensitivities multiplied = SENSI_1.multipliedBy(FACTOR1);
		DoubleMatrix test = multiplied.Sensitivities.get(0).Sensitivity;
		for (int i = 0; i < MATRIX_USD1.columnCount(); i++)
		{
		  for (int j = 0; j < MATRIX_USD1.rowCount(); j++)
		  {
			assertEquals(test.get(i, j), MATRIX_USD1.get(i, j) * FACTOR1);
		  }
		}
	  }

	  public virtual void test_mapSensitivities()
	  {
		CrossGammaParameterSensitivities multiplied = SENSI_1.mapSensitivities(a => 1 / a);
		DoubleMatrix test = multiplied.Sensitivities.get(0).Sensitivity;
		for (int i = 0; i < MATRIX_USD1.columnCount(); i++)
		{
		  for (int j = 0; j < MATRIX_USD1.rowCount(); j++)
		  {
			assertEquals(test.get(i, j), 1 / MATRIX_USD1.get(i, j));
		  }
		}
	  }

	  public virtual void test_multipliedBy_vs_combinedWith()
	  {
		CrossGammaParameterSensitivities multiplied = SENSI_2.multipliedBy(2d);
		CrossGammaParameterSensitivities added = SENSI_2.combinedWith(SENSI_2);
		assertEquals(multiplied, added);
	  }

	  public virtual void test_getSensitivity_name()
	  {
		assertEquals(SENSI_3.getSensitivity(NAME1, NAME1, USD), ENTRY_USD);
		assertEquals(SENSI_3.getSensitivity(NAME1, NAME2, USD), CrossGammaParameterSensitivity.of(NAME1, METADATA1, NAME2, METADATA2, USD, MATRIX_USD2));
		assertEquals(SENSI_3.getSensitivity(NAME2, NAME1, USD), CrossGammaParameterSensitivity.of(NAME2, METADATA2, NAME1, METADATA1, USD, MATRIX_USD2));
		assertEquals(SENSI_3.getSensitivity(NAME2, NAME2, USD), CrossGammaParameterSensitivity.of(NAME2, METADATA2, NAME2, METADATA2, USD, DoubleMatrix.of(2, 2, -500, -400, -200, -300)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalWithTolerance()
	  {
		CrossGammaParameterSensitivities sensUsdTotal = CrossGammaParameterSensitivities.of(ENTRY_USD_TOTAL);
		CrossGammaParameterSensitivities sensEur = CrossGammaParameterSensitivities.of(ENTRY_EUR);
		CrossGammaParameterSensitivities sens1plus2 = SENSI_1.combinedWith(ENTRY_USD2);
		CrossGammaParameterSensitivities sensZeroA = CrossGammaParameterSensitivities.of(ENTRY_ZERO3);
		CrossGammaParameterSensitivities sensZeroB = CrossGammaParameterSensitivities.of(ENTRY_ZERO0);
		CrossGammaParameterSensitivities sens1plus2plus0a = SENSI_1.combinedWith(ENTRY_USD2).combinedWith(ENTRY_ZERO0);
		CrossGammaParameterSensitivities sens1plus2plus0b = SENSI_1.combinedWith(ENTRY_USD2).combinedWith(ENTRY_ZERO3);
		CrossGammaParameterSensitivities sens1plus2plus0 = SENSI_1.combinedWith(ENTRY_USD2).combinedWith(ENTRY_ZERO0).combinedWith(ENTRY_ZERO3);
		CrossGammaParameterSensitivities sens2plus0 = SENSI_2.combinedWith(sensZeroA);
		assertEquals(SENSI_1.equalWithTolerance(sensZeroA, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(SENSI_1, TOLERENCE_CMP), true);
		assertEquals(SENSI_1.equalWithTolerance(SENSI_2, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sens2plus0, TOLERENCE_CMP), false);

		assertEquals(SENSI_2.equalWithTolerance(sensZeroA, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(SENSI_1, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(SENSI_2, TOLERENCE_CMP), true);
		assertEquals(SENSI_2.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(sens2plus0, TOLERENCE_CMP), true);

		assertEquals(sensZeroA.equalWithTolerance(sensZeroA, TOLERENCE_CMP), true);
		assertEquals(sensZeroA.equalWithTolerance(SENSI_1, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(SENSI_2, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sens2plus0, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sensZeroB, TOLERENCE_CMP), true);

		assertEquals(sensZeroB.equalWithTolerance(sensZeroB, TOLERENCE_CMP), true);
		assertEquals(sensZeroB.equalWithTolerance(SENSI_1, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(SENSI_2, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sens2plus0, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sensZeroA, TOLERENCE_CMP), true);

		assertEquals(sens1plus2.equalWithTolerance(sens1plus2, TOLERENCE_CMP), true);
		assertEquals(sens1plus2.equalWithTolerance(sens1plus2plus0a, TOLERENCE_CMP), true);
		assertEquals(sens1plus2.equalWithTolerance(sens1plus2plus0b, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2plus0, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2plus0a, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2plus0b, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2plus0, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2plus0a, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2plus0b, TOLERENCE_CMP), true);
		assertEquals(sens2plus0.equalWithTolerance(sens2plus0, TOLERENCE_CMP), true);

		assertEquals(sensZeroA.equalWithTolerance(CrossGammaParameterSensitivities.empty(), TOLERENCE_CMP), true);
		assertEquals(CrossGammaParameterSensitivities.empty().equalWithTolerance(sensZeroA, TOLERENCE_CMP), true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(CrossGammaParameterSensitivities.empty());
		coverImmutableBean(SENSI_1);
		coverBeanEquals(SENSI_1, SENSI_2);
	  }

	}

}
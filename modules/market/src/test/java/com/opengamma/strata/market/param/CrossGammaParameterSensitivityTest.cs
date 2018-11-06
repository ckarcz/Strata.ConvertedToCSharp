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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;

	/// <summary>
	/// Test <seealso cref="CrossGammaParameterSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CrossGammaParameterSensitivityTest
	public class CrossGammaParameterSensitivityTest
	{

	  private const double FACTOR1 = 3.14;
	  private static readonly DoubleMatrix MATRIX_USD1 = DoubleMatrix.of(2, 2, 100, 200, 300, 123);
	  private static readonly DoubleMatrix MATRIX_USD_FACTOR = DoubleMatrix.of(2, 2, 100 * FACTOR1, 200 * FACTOR1, 300 * FACTOR1, 123 * FACTOR1);
	  private static readonly DoubleMatrix MATRIX_EUR1 = DoubleMatrix.of(2, 2, 1000, 250, 321, 123);
	  private static readonly DoubleMatrix MATRIX_EUR1_IN_USD = DoubleMatrix.of(2, 2, 1000 * 1.5, 250 * 1.5, 321 * 1.5, 123 * 1.5);
	  private static readonly DoubleMatrix MATRIX_USD_EUR = DoubleMatrix.of(2, 4, 100, 200, 1000, 250, 300, 123, 321, 123);
	  private static readonly Currency USD = Currency.USD;
	  private static readonly Currency EUR = Currency.EUR;
	  private static readonly FxRate FX_RATE = FxRate.of(EUR, USD, 1.5d);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME1 = com.opengamma.strata.market.curve.CurveName.of("NAME-1");
	  private static readonly MarketDataName<object> NAME1 = CurveName.of("NAME-1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME2 = com.opengamma.strata.market.curve.CurveName.of("NAME-2");
	  private static readonly MarketDataName<object> NAME2 = CurveName.of("NAME-2");
	  private static readonly IList<ParameterMetadata> METADATA_USD1 = ParameterMetadata.listOfEmpty(2);
	  private static readonly IList<ParameterMetadata> METADATA_EUR1 = ParameterMetadata.listOfEmpty(2);
	  private static readonly IList<ParameterMetadata> METADATA_BAD = ParameterMetadata.listOfEmpty(1);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_metadata()
	  {
		CrossGammaParameterSensitivity test = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		assertEquals(test.MarketDataName, NAME1);
		assertEquals(test.ParameterCount, 2);
		assertEquals(test.ParameterMetadata, METADATA_USD1);
		assertEquals(test.getParameterMetadata(0), METADATA_USD1[0]);
		assertEquals(test.Currency, USD);
		assertEquals(test.Sensitivity, MATRIX_USD1);
		assertEquals(test.Order, ImmutableList.of(Pair.of(NAME1, METADATA_USD1)));
	  }

	  public virtual void test_of_eurUsd()
	  {
		CrossGammaParameterSensitivity test = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, ImmutableList.of(Pair.of(NAME1, METADATA_USD1), Pair.of(NAME2, METADATA_EUR1)), USD, MATRIX_USD_EUR);
		assertEquals(test.MarketDataName, NAME1);
		assertEquals(test.ParameterCount, 2);
		assertEquals(test.ParameterMetadata, METADATA_USD1);
		assertEquals(test.getParameterMetadata(0), METADATA_USD1[0]);
		assertEquals(test.Currency, USD);
		assertEquals(test.Sensitivity, MATRIX_USD_EUR);
		assertEquals(test.Order, ImmutableList.of(Pair.of(NAME1, METADATA_USD1), Pair.of(NAME2, METADATA_EUR1)));
	  }

	  public virtual void test_of_metadata_badMetadata()
	  {
		assertThrowsIllegalArg(() => CrossGammaParameterSensitivity.of(NAME1, METADATA_BAD, USD, MATRIX_USD1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		CrossGammaParameterSensitivity @base = CrossGammaParameterSensitivity.of(NAME1, METADATA_EUR1, EUR, MATRIX_EUR1);
		CrossGammaParameterSensitivity test = @base.convertedTo(USD, FX_RATE);
		assertEquals(@base.convertedTo(EUR, FX_RATE), @base);
		assertEquals(test, CrossGammaParameterSensitivity.of(NAME1, METADATA_EUR1, USD, MATRIX_EUR1_IN_USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		CrossGammaParameterSensitivity @base = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		CrossGammaParameterSensitivity test = @base.multipliedBy(FACTOR1);
		assertEquals(test, CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD_FACTOR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		CrossGammaParameterSensitivity @base = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		CrossGammaParameterSensitivity test = @base.withSensitivity(MATRIX_USD_FACTOR);
		assertEquals(test, CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD_FACTOR));
		assertThrowsIllegalArg(() => @base.withSensitivity(DoubleMatrix.of(1, 1, 1d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_total()
	  {
		CrossGammaParameterSensitivity @base = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		CurrencyAmount test = @base.total();
		assertEquals(test.Currency, USD);
		double expected = MATRIX_USD1.get(0, 0) + MATRIX_USD1.get(0, 1) + MATRIX_USD1.get(1, 0) + MATRIX_USD1.get(1, 1);
		assertEquals(test.Amount, expected);
	  }

	  public virtual void test_diagonal()
	  {
		CrossGammaParameterSensitivity @base = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		CurrencyParameterSensitivity test = @base.diagonal();
		DoubleArray value = DoubleArray.of(MATRIX_USD1.get(0, 0), MATRIX_USD1.get(1, 1));
		assertEquals(test, CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, value));
	  }

	  public virtual void test_diagonal_eurUsd()
	  {
		CrossGammaParameterSensitivity @base = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, ImmutableList.of(Pair.of(NAME1, METADATA_USD1), Pair.of(NAME2, METADATA_EUR1)), USD, MATRIX_USD_EUR);
		CurrencyParameterSensitivity test = @base.diagonal();
		DoubleArray value = DoubleArray.of(MATRIX_USD1.get(0, 0), MATRIX_USD1.get(1, 1));
		assertEquals(test, CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, value));
	  }

	  public virtual void test_getSensitivity_eurUsd()
	  {
		CrossGammaParameterSensitivity test = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, ImmutableList.of(Pair.of(NAME1, METADATA_USD1), Pair.of(NAME2, METADATA_EUR1)), USD, MATRIX_USD_EUR);
		CrossGammaParameterSensitivity expected1 = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		assertEquals(test.getSensitivity(NAME1), expected1);
		CrossGammaParameterSensitivity expected2 = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, NAME2, METADATA_EUR1, USD, MATRIX_EUR1);
		assertEquals(test.getSensitivity(NAME2), expected2);
		assertThrowsIllegalArg(() => test.getSensitivity(CurveName.of("NAME-3")));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CrossGammaParameterSensitivity test = CrossGammaParameterSensitivity.of(NAME1, METADATA_USD1, USD, MATRIX_USD1);
		coverImmutableBean(test);
		CrossGammaParameterSensitivity test2 = CrossGammaParameterSensitivity.of(NAME2, METADATA_EUR1, EUR, MATRIX_EUR1);
		coverBeanEquals(test, test2);
	  }

	}

}